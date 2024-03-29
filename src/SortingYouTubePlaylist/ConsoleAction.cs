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
            case "list":
                await ListPlaylist(consoleArgument);
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
        int countPositionChanged = await UpdateItemPositionsAsync(items);
        Console.WriteLine($"{countPositionChanged} vidéo(s) déplacé(s) dans la playlist");
        Console.WriteLine("Fait");
    }

    private static async Task<int> UpdateItemPositionsAsync(List<PlaylistItem> items) {
        const int notChangeCount = 3;
        if (items.Count < notChangeCount + 2)
            return 0;
        string channelId = items[notChangeCount - 1].Video.ChannelId;
        string? note = items[notChangeCount - 1].Note;
        items = items.Skip(notChangeCount).ToList();
        if (note != null)
            while (items.Count > 0 && items[0].Note == note) {
                channelId = items[0].Video.ChannelId;
                items.Remove(items[0]);
            }
        int count = 0;
        int countPositionChanged = 0;
        PlaylistItem? item;
        DateTime thresholdDate = DateTime.Now.AddMonths(-1);
        List<string> notes = new List<string>();
        if (note != null)
            notes.Add(note);
        while (items.Count > 0) {
            List<PlaylistItem> nextItems = items.Where(i => i.Video.ChannelId != channelId).ToList();
            if (count < 5) {
                item = null;
                if (count == 0) {
                    note = null;
                    item = nextItems.Where(i => i.PublishedAt.CompareTo(thresholdDate) < 0).MinBy(i => i.Video.Duration);
                    if (item == null)
                        count++;
                }
                if (count == 3) {
                    item = nextItems.Where(i => i.PublishedAt.CompareTo(thresholdDate) < 0).MaxBy(i => i.Video.Duration);
                    item ??= nextItems.MaxBy(i => i.Video.Duration);
                    item ??= items.MaxBy(i => i.Video.Duration);
                }
                item ??= nextItems.MinBy(i => i.Video.Duration);
                item ??= items.MinBy(i => i.Video.Duration);
                if (note == null) {
                    note = item!.Note ?? Guid.NewGuid().ToString();
                    if (notes.Contains(note))
                        note = Guid.NewGuid().ToString();
                    notes.Add(note);
                }
                count++;
            }
            else {
                item = nextItems.MinBy(i => i.PublishedAt);
                item ??= items.MinBy(i => i.PublishedAt);
                count = 0;
            }
            long position = items.First().Position;
            channelId = item!.Video.ChannelId;
            items.Remove(item);
            if (item.Position != position || item.Note != note) {
                countPositionChanged++;
                await _youtubeSource!.UpdateItemPositionAndNoteInPlaylistAsync(item.Id, position, note);
                foreach (PlaylistItem itemPosition in items.Where(i => i.Position < item.Position))
                    itemPosition.Position++;
            }
        }
        return countPositionChanged;
    }

    private static async Task ListPlaylist(ConsoleArgument consoleArgument) {
        Console.WriteLine("Récupération des informations sur les vidéos de la liste de lecture...");
        List<PlaylistItem> items = await _youtubeSource!.GetItemsFromPlaylistAsync(consoleArgument.PlaylistId!);
        foreach (PlaylistItem item in items)
            Console.WriteLine($"{item.Video.Title} [{item.Id.VideoId}] | {item.Video.Duration} | {item.Video.Channel}");
        Console.WriteLine("Fait");
    }
}
