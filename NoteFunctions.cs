using System.Diagnostics;

public static class NoteFunctions
{
    static readonly string currentDir = Directory.GetCurrentDirectory();
    static readonly string notesDir = Path.Combine(currentDir, "notes");
    static readonly string tempDir = Path.Combine(notesDir, "tmp");

    static public async Task NewNote()
    {
        Console.WriteLine("Please enter the name of your note:");
        var noteName = Console.ReadLine();
        noteName = $"{notesDir}\\{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}_{noteName}.txt";
        File.WriteAllText(noteName, "");
        Console.WriteLine("Opening new file in Notepad... Be sure to save before you close when you're done! Press any key to continue.");
        Console.ReadKey(true);

        await OpenNotepad(noteName);

        return;
    }

    static public List<string> LoadNoteList()
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
    }

    static public async Task LoadNote(string noteName)
    {
        Console.Clear();
        if (Directory.Exists(tempDir)) { Directory.Delete(tempDir, true); }
        Directory.CreateDirectory(tempDir);
        var tempNote = $"{tempDir}\\{Path.GetFileName(noteName)}";
        File.Copy(noteName, tempNote);
        Console.WriteLine("Opening file in Notepad... Be sure to save before you close when you're done! Press any key to continue.");
        Console.ReadKey(true);

        await OpenNotepad(tempNote);
        await SaveNote(noteName, tempNote);
        return;
    }

    static public async Task SaveNote(string oldNote, string newNote)
    {
        string newNoteContent = File.ReadAllText(newNote);

       Console.Clear();
        Console.WriteLine("= = = = = = = = =\nOriginal Note:");
        Console.WriteLine(File.ReadAllText(oldNote));
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
                File.WriteAllText(oldNote, newNoteContent);
                File.Delete(newNote);
                Directory.Delete(tempDir);
                Console.WriteLine("Changes finalized. Returning to load menu. Press any key to continue.");
                Console.ReadKey(true);
                return;
            }
        }
    }

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