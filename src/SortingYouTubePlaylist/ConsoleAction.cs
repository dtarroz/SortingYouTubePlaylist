namespace SortingYouTubePlaylist;

internal static class ConsoleAction
{
    private static YoutubeSource? _youtubeSource;

    public static async Task ExecuteAsync(ConsoleArgument consoleArgument) {
        ThrowIfConsoleArgumentInvalid(consoleArgument);
        _youtubeSource = new YoutubeSource(consoleArgument.ClientSecretFile, consoleArgument.DataStoreDirectory);
        switch (consoleArgument.Action.ToLower()) {
            case "authorize":
                await AuthorizeAsync();
                break;
            case "sort":
                await SortingPlaylist(consoleArgument);
                break;
            default: throw new Exception("Action non trouvée");
        }
    }

    private static void ThrowIfConsoleArgumentInvalid(ConsoleArgument consoleArgument) {
        if (consoleArgument == null)
            throw new ArgumentNullException(nameof(consoleArgument));
        if (consoleArgument.Action.ToLower() == "sort" && consoleArgument.PlaylistId == null)
            throw new Exception("La liste de lecture est manquante");
    }

    private static async Task AuthorizeAsync() {
        Console.WriteLine("Demande d'autorisation à un compte Youtube");
        await _youtubeSource!.WebAuthorizeAsync();
        Console.WriteLine("Fait");
    }

    private static async Task SortingPlaylist(ConsoleArgument consoleArgument) {
        Console.WriteLine("Récupération des informations sur les vidéos de la liste de lecture...");
        List<PlaylistItem> items = await _youtubeSource!.GetItemsFromPlaylistAsync(consoleArgument.PlaylistId!);
        Console.WriteLine("Mise à jour des positions des vidéos dans la liste de lecture...");
        await UpdateItemPositionsAsync(items);
        Console.WriteLine("Fait");
    }

    private static async Task UpdateItemPositionsAsync(List<PlaylistItem> items) {
        if (items.Count < 6)
            return;
        string channelId = items[3].Video.ChannelId;
        items = items.Skip(4).ToList();
        int count = 0;
        PlaylistItem? item;
        while (items.Count > 0) {
            if (count < 3) {
                count++;
                item = items.Where(i => i.Video.ChannelId != channelId).MinBy(i => i.Video.Duration) ?? items.MinBy(i => i.Video.Duration);
            }
            else {
                count = 0;
                item = items.Where(i => i.Video.ChannelId != channelId).MinBy(i => i.PublishedAt) ?? items.MinBy(i => i.PublishedAt);
            }
            long position = items.First().Position;
            channelId = item!.Video.ChannelId;
            items.Remove(item);
            if (item.Position != position) {
                await _youtubeSource!.UpdateItemPositionInPlaylistAsync(item.Id, position);
                foreach (PlaylistItem itemPosition in items.Where(i => i.Position < item.Position))
                    itemPosition.Position++;
            }
        }
    }
}
