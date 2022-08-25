using System.Reflection;
using System.Xml;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace SortingYouTubePlaylist;

internal sealed class YoutubeSource
{
    private readonly string _clientSecretFile;
    private readonly string _dataStoreDirectory;
    private YouTubeService? _youtubeService;
    private UserCredential? _credential;
    private const string User = "user";
    private readonly string[] _scope = { YouTubeService.Scope.Youtube };

    public YoutubeSource(string clientSecretFile, string dataStoreDirectory) {
        _clientSecretFile = clientSecretFile;
        _dataStoreDirectory = dataStoreDirectory;
    }

    public async Task WebAuthorizeAsync() {
        ClientSecrets clientSecrets = await GetClientSecretAsync();
        FileDataStore dataStore = GetDataStore();
        _credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(clientSecrets, _scope, User, CancellationToken.None, dataStore);
    }

    private async Task<ClientSecrets> GetClientSecretAsync() {
        await using FileStream stream = new FileStream(_clientSecretFile, FileMode.Open, FileAccess.Read);
        GoogleClientSecrets googleClientSecrets = await GoogleClientSecrets.FromStreamAsync(stream);
        return googleClientSecrets.Secrets;
    }

    private FileDataStore GetDataStore() {
        return new FileDataStore(_dataStoreDirectory, true);
    }

    public async Task<List<PlaylistItem>> GetItemsFromPlaylistAsync(string playListId) {
        await LoadYouTubeService();
        List<PlaylistItem> items = new List<PlaylistItem>();
        string nextPageToken = "";
        while (nextPageToken != null) {
            PlaylistItemListResponse playListItems = await GetPlayListItemsAsync(playListId, nextPageToken);
            foreach (var item in playListItems.Items.Where(IsStatusValid))
                items.Add(await ConvertToAsync(item));
            nextPageToken = playListItems.NextPageToken;
        }
        return items;
    }

    private async Task LoadYouTubeService() {
        await LoadCredentialAsync();
        _youtubeService ??= new YouTubeService(new BaseClientService.Initializer {
                                                   HttpClientInitializer = _credential,
                                                   ApplicationName = Assembly.GetExecutingAssembly().GetName().Name
                                               });
    }

    private async Task LoadCredentialAsync() {
        if (_credential == null) {
            GoogleAuthorizationCodeFlow.Initializer initializer = await GetInitializerAsync();
            GoogleAuthorizationCodeFlow flow = new GoogleAuthorizationCodeFlow(initializer);
            TokenResponse token = await GetTokenResponseAsync();
            _credential = new UserCredential(flow, User, token);
        }
    }

    private async Task<GoogleAuthorizationCodeFlow.Initializer> GetInitializerAsync() {
        ClientSecrets clientSecrets = await GetClientSecretAsync();
        return new GoogleAuthorizationCodeFlow.Initializer {
            ClientSecrets = clientSecrets,
            Scopes = _scope
        };
    }

    private async Task<TokenResponse> GetTokenResponseAsync() {
        return await GetDataStore().GetAsync<TokenResponse>(User);
    }

    private async Task<PlaylistItemListResponse> GetPlayListItemsAsync(string playListId, string nextPageToken) {
        PlaylistItemsResource.ListRequest playlistRequest = _youtubeService!.PlaylistItems.List("snippet,contentDetails,status");
        playlistRequest.PlaylistId = playListId;
        playlistRequest.MaxResults = 50;
        playlistRequest.PageToken = nextPageToken;
        return await playlistRequest.ExecuteAsync();
    }

    private static bool IsStatusValid(Google.Apis.YouTube.v3.Data.PlaylistItem item) {
        return item.Status.PrivacyStatus != "private" && item.Status.PrivacyStatus != "privacyStatusUnspecified";
    }

    private async Task<PlaylistItem> ConvertToAsync(Google.Apis.YouTube.v3.Data.PlaylistItem item) {
        return new PlaylistItem {
            Id = new PlaylistItemId {
                ItemId = item.Id,
                PlaylistId = item.Snippet.PlaylistId,
                Kind = item.Snippet.ResourceId.Kind,
                VideoId = item.Snippet.ResourceId.VideoId
            },
            Video = new PlaylistItemVideo {
                Duration = await GetDurationAsync(item.Snippet.ResourceId.VideoId),
                ChannelId = item.Snippet.VideoOwnerChannelId
            },
            PublishedAt = item.Snippet.PublishedAt ?? throw new InvalidOperationException(),
            Position = item.Snippet.Position ?? throw new InvalidOperationException()
        };
    }

    private async Task<int> GetDurationAsync(string videoId) {
        Video video = await GetVideoAsync(videoId);
        return Convert.ToInt32(XmlConvert.ToTimeSpan(video.ContentDetails.Duration).TotalSeconds);
    }

    private async Task<Video> GetVideoAsync(string videoId) {
        VideosResource.ListRequest listRequest = _youtubeService!.Videos.List("contentDetails");
        listRequest.Id = videoId;
        listRequest.MaxResults = 1;
        VideoListResponse videoList = await listRequest.ExecuteAsync();
        return videoList.Items.Count == 1 ? videoList.Items[0] : throw new InvalidOperationException();
    }

    public async Task UpdateItemPositionInPlaylistAsync(PlaylistItemId itemId, long position) {
        var item = new Google.Apis.YouTube.v3.Data.PlaylistItem {
            Id = itemId.ItemId,
            Snippet = new PlaylistItemSnippet {
                PlaylistId = itemId.PlaylistId,
                ResourceId = new ResourceId {
                    Kind = itemId.Kind,
                    VideoId = itemId.VideoId
                },
                Position = position
            }
        };
        var update = _youtubeService!.PlaylistItems.Update(item, "snippet");
        await update.ExecuteAsync();
    }
}
