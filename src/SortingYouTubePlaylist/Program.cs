try {
    Console.WriteLine("Hello, World!");

    return 0;
}
catch (Exception ex) {
    Console.WriteLine();
    Console.WriteLine("-- ERROR --");
    Console.WriteLine(ex.Message);
    return 1;
}
