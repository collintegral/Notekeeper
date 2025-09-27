using System.Diagnostics;
using Notekeeper;

public class NoteFunctions
{
    // Directories are compiled
    static readonly string currentDir = Directory.GetCurrentDirectory();
    static readonly string tempDir = Path.Combine(currentDir, "tmp");

    // A new note is named using datetime and a user-inputted name, then Notepad is opened to fill it.
    static public async Task NewNote()
    {
        Console.Clear();
        if (Directory.Exists(tempDir)) { Directory.Delete(tempDir, true); }
        Directory.CreateDirectory(tempDir);
        Console.WriteLine("Please enter the name of your note:");
        var noteName = Console.ReadLine();
        var tempName = $"{tempDir}\\{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}_{noteName}.txt";
        File.WriteAllText(tempName, "");
        Console.WriteLine("Opening new file in Notepad... Be sure to save before you close when you're done! Press any key to continue.");
        Console.ReadKey(true);

        await OpenNotepad(tempName);

        await NoteTable.CreateNote(noteName, File.ReadAllText(tempName));
        File.Delete(tempName);
        Directory.Delete(tempDir);

        return;
    }

// Previously created notes are loaded and returned.
/*     static public List<string> LoadNoteList()
    {
        Directory.CreateDirectory(notesDir);
        List<string> notes = [];
        var noteFiles = Directory.EnumerateFiles(notesDir, "*");

        foreach (var file in noteFiles)
        {
            var extension = Path.GetExtension(file);
            if (extension == ".txt") notes.Add(file);
        }

        return notes;
    } */

// A chosen existing note is loaded, a temporary one is created, and Notepad is called to make edits.
    static public async Task LoadNote(Note note)
    {
        Console.Clear();
        if (Directory.Exists(tempDir)) { Directory.Delete(tempDir, true); }
        Directory.CreateDirectory(tempDir);
        var tempNote = $"{tempDir}\\{Path.GetFileName(note.NoteName)}";
        File.WriteAllText(tempNote, note.Content);
        Console.WriteLine("Opening file in Notepad... Be sure to save before you close when you're done! Press any key to continue.");
        Console.ReadKey(true);

        await OpenNotepad(tempNote);
        await SaveNote(note, tempNote);
        return;
    }

// The user can decide whether to make more edits to their loaded note, save their edits permanently, or return without saving.
    static public async Task SaveNote(Note oldNote, string newNote)
    {
        string newNoteContent = File.ReadAllText(newNote);

        Console.Clear();
        Console.WriteLine("= = = = = = = = =\nOriginal Note:");
        Console.WriteLine(oldNote.Content);
        Console.WriteLine("= = = = = = = = =\nEdited Note:");
        Console.WriteLine(newNoteContent);
        Console.WriteLine("= = = = = = = = =\nFinalize changes? y/n");

        bool choiceMade = false;
        while (!choiceMade)
        {
            var choice = Console.ReadKey(true);
            if (choice.KeyChar == 'n' || choice.KeyChar == 'N')
            {
                choiceMade = true;
                Console.WriteLine("Return to editing? y/n");

                bool moreEdits = true;
                while (moreEdits)
                {
                    choice = Console.ReadKey(true);
                    if (choice.KeyChar == 'n' || choice.KeyChar == 'N')
                    {
                        return;
                    }
                    else if (choice.KeyChar == 'y' || choice.KeyChar == 'Y')
                    {
                        await OpenNotepad(newNote);
                        await SaveNote(oldNote, newNote);
                        return;
                    }
                }
            }
            else if (choice.KeyChar == 'y' || choice.KeyChar == 'Y')
            {
                NoteTable.UpdateNote(oldNote.NoteId, newNoteContent).GetAwaiter().GetResult();
                File.Delete(newNote);
                Directory.Delete(tempDir);
                Console.WriteLine("Changes finalized. Returning to load menu. Press any key to continue.");
                Console.ReadKey(true);
                return;
            }
        }
    }

    static public async Task DeleteNote(Note note)
    {
        Console.Clear();
        Console.WriteLine($"= = = = = = = = =\n{note.NoteName}:");
        Console.WriteLine(note.Content);
        Console.WriteLine("= = = = = = = = =\nReally Delete? y/n");

        bool choiceMade = false;
        while (!choiceMade)
        {
            var choice = Console.ReadKey(true);
            if (choice.KeyChar == 'n' || choice.KeyChar == 'N')
            {
                return;
            }
            else if (choice.KeyChar == 'y' || choice.KeyChar == 'Y')
            {
                await NoteTable.DeleteNote(note.NoteId);
                Console.WriteLine("Deletion finalized. Returning to main menu. Press any key to continue.");
                Console.ReadKey(true);
                return;
            }
        }
    }

// A function that runs Notepad on the given file and waits until the user has closed it to continue.
    static public async Task OpenNotepad(string fileName)
    {
        var notepadProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "notepad.exe",
                Arguments = fileName,
                UseShellExecute = false,
            },
            EnableRaisingEvents = true
        };

        notepadProcess.Start();

        Console.Clear();
        Console.WriteLine("Please see Notepad. Save and close when you're done.");

        await notepadProcess.WaitForExitAsync();

        Console.Clear();
        return;
    }
}