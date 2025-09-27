using Notekeeper;

public class Menus
{
    public static Menu mainMenu = new("Notekeeper");

    // Constructor for main menu
    public void ConstructMainMenu()
    {
        var newNote = new MenuOption("New Note", () => NewNote().GetAwaiter().GetResult());
        mainMenu.AddOption(newNote);
        var loadNote = new MenuOption("Load Note", () => LoadMenuSetup().GetAwaiter().GetResult());
        mainMenu.AddOption(loadNote);
        var filterNote = new MenuOption("Filter Notes by Age", () => FilterNote());
        mainMenu.AddOption(filterNote);
        var deleteNote = new MenuOption("Delete Note", () => DeleteMenuSetup());
        mainMenu.AddOption(deleteNote);
        var quit = new MenuOption("Quit", () => Environment.Exit(0));
        mainMenu.AddOption(quit);
    }
        
// Displays Main Menu
    public void MainMenu()
    {
        mainMenu.Display()();
    }

// Dynamically reads the notes into a new Load Menu for display, then shares them
    public async Task LoadMenuSetup(bool filter = false, DateTime filterDate = new DateTime(), bool before = false)
    {
        Console.Clear();
        var loadMenu = new Menu("Load Menu");
        List<Note> noteList;
        if (!filter)
        {
            noteList = await NoteTable.ReadAllNotes();
        }
        else
        {
            noteList = await NoteTable.ReadAllNotesByTime(filterDate, before);
        }
        
        foreach (var note in noteList)
        {
            loadMenu.AddOption(new MenuOption(note.NoteName, () =>
            {
                NoteFunctions.LoadNote(note).GetAwaiter().GetResult();
                loadMenu.Display()();
            }));
        }
        loadMenu.AddOption(new MenuOption("Return to Main Menu", () => { mainMenu.Display()(); }));
        loadMenu.Display()();
    }

    public async Task DeleteMenuSetup()
    {
        Console.Clear();
        var deleteMenu = new Menu("Delete Menu");
        List<Note> noteList = noteList = await NoteTable.ReadAllNotes();

        foreach (var note in noteList)
        {
            deleteMenu.AddOption(new MenuOption(note.NoteName, () =>
            {
                NoteFunctions.DeleteNote(note).GetAwaiter().GetResult();
                deleteMenu.Display()();
            }));
        }
        deleteMenu.AddOption(new MenuOption("Return to Main Menu", () => { mainMenu.Display()(); }));
        deleteMenu.Display()();
        mainMenu.Display()();
    }

    public async Task FilterNote()
    {
        DateTime filterDay = DateTime.Today;
        bool before = false;

        var dayValid = false;
        while (!dayValid)
        {
            Console.Clear();
            Console.WriteLine("Enter the number of days back to filter by, and whether to do all newer than or older than the selection.");
            Console.Write("\nDays Back (enter number): ");
            var dayChoice = Console.ReadLine();
            if (int.TryParse(dayChoice, out var userInt))
            {
                dayValid = true;
                var baValid = false;
                filterDay = DateTime.Today.AddDays(-userInt);
                while (!baValid)
                {
                    Console.Clear();
                    Console.Write($"You selected {filterDay.ToShortDateString()}.\nGet notes from before or after entered date? (b/a): ");
                    var baChoice = Console.ReadLine() ?? "no";
                    if (new List<string> { "b", "B" }.Contains(baChoice))
                    {
                        baValid = true;
                        before = true;
                    }
                    else if (new List<string> { "a", "A" }.Contains(baChoice))
                    {
                        baValid = true;
                    }
                }
            }
        }

        await LoadMenuSetup(true, filterDay, before);
    }

// Calls the NoteFunction for a new note and walks the user through it
    public static async Task NewNote()
    {
        Console.Clear();
        NoteFunctions.NewNote().GetAwaiter().GetResult();
        Console.Clear();
        Console.WriteLine("Editing complete. Press any key to continue.");
        Console.ReadKey(true);
        Console.Clear();
        mainMenu.Display()();
    }
}
