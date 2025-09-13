using Notekeeper;

public class Menus
{
    public static Menu mainMenu = new("Notekeeper");

    public void ConstructMainMenu()
    {
        var newNote = new MenuOption("New Note", () => NewNote().GetAwaiter().GetResult());
        mainMenu.AddOption(newNote);
        var loadNote = new MenuOption("Load Note", LoadMenuSetup);
        mainMenu.AddOption(loadNote);
        var quit = new MenuOption("Quit", () => Environment.Exit(0));
        mainMenu.AddOption(quit);
    }
        
    public void MainMenu()
    {
        mainMenu.Display()();
    }

    public static void LoadMenuSetup()
    {
        Console.Clear();
        var loadMenu = new Menu("Load Menu");

        var noteList = NoteFunctions.LoadNoteList();
        foreach (var note in noteList)
        {
            loadMenu.AddOption(new MenuOption(Path.GetFileName(note), () =>
            {
                NoteFunctions.LoadNote(note).GetAwaiter().GetResult();
                loadMenu.Display()();
            }));
        }
        loadMenu.AddOption(new MenuOption("Return to Main Menu", () => { mainMenu.Display()(); }));
        loadMenu.Display()();
    }

    public static async Task NewNote()
    {
        Console.Clear();
        await NoteFunctions.NewNote();
        Console.Clear();
        Console.WriteLine("Editing complete. Press any key to continue.");
        Console.ReadKey(true);
        Console.Clear();
        mainMenu.Display()();
    }
}
