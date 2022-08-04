namespace SortingYouTubePlaylist;

internal static class ConsoleAction
{
    public static async Task ExecuteAsync(ConsoleArgument consoleArgument) {
        ThrowIfConsoleArgumentInvalid(consoleArgument);
        switch (consoleArgument.Action.ToLower()) {
            case "authorize":
                await AuthorizeAsync(consoleArgument);
                break;
            default: throw new Exception("Action non trouvé");
        }
    }

    private static void ThrowIfConsoleArgumentInvalid(ConsoleArgument consoleArgument) {
        if (consoleArgument == null)
            throw new ArgumentNullException(nameof(consoleArgument));
    }

    private static async Task AuthorizeAsync(ConsoleArgument consoleArgument) {
        Console.WriteLine("Demande d'autorisation à un compte Youtube");
        YoutubeSource youtubeSource = new YoutubeSource(consoleArgument.ClientSecretFile, consoleArgument.DataStoreDirectory);
        await youtubeSource.WebAuthorizeAsync();
        Console.WriteLine("Demande effectuée");
    }
}
