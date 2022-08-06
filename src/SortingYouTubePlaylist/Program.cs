using SortingYouTubePlaylist;

try {
    ConsoleArgument consoleArgument = ConsoleArgument.CreateFromArgs(args);
    await ConsoleAction.ExecuteAsync(consoleArgument);
    return 0;
}
catch (Exception ex) {
    Console.WriteLine();
    Console.WriteLine("-- ERROR --");
    Console.WriteLine(ex.Message);
    Console.WriteLine(ex.StackTrace);
    return 1;
}
