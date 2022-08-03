using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;

namespace SortingYouTubePlaylist;

internal sealed class YoutubeSource
{
    private readonly string _clientSecretFile;
    private readonly string _dataStoreDirectory;
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
    
    // ---

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
}
