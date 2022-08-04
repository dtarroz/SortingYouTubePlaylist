namespace SortingYouTubePlaylist;

internal sealed class ConsoleArgument
{
    public string Action { get; init; } = null!;
    public string ClientSecretFile { get; init; } = null!;
    public string DataStoreDirectory { get; init; } = null!;

    private ConsoleArgument() { }

    public static ConsoleArgument CreateFromArgs(string[] args) {
        ThrowIfArgsInvalid(args);
        return new ConsoleArgument {
            Action = args[0],
            ClientSecretFile = GetValueFromKey(args, "-c") ?? "client_secrets.json",
            DataStoreDirectory = GetValueFromKey(args, "-d") ?? "youtube-data-store"
        };
    }

    private static void ThrowIfArgsInvalid(IReadOnlyCollection<string> args) {
        if (args.Count == 0)
            throw new Exception("Arguments manquants");
    }

    private static string? GetValueFromKey(IReadOnlyList<string> args, string key) {
        int index = args.ToList().IndexOf(key);
        if (index > 0 && index + 1 < args.Count)
            return args[index + 1];
        return null;
    }
}
