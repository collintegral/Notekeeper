using Microsoft.Data.Sqlite;
using Notekeeper;

public class NoteTable
{
    private static string connectionString = "Data Source=notes.db";

    static public async Task SeedTable()
    {
        string seedQuery = @"
        CREATE TABLE IF NOT EXISTS Notes (
        NoteID INTEGER PRIMARY KEY AUTOINCREMENT
        , NoteName TEXT NOT NULL
        , CreatedBy INTEGER DEFAULT 1
        , Created TEXT DEFAULT CURRENT_TIMESTAMP
        , EditedBy INTEGER DEFAULT 1
        , LastEdited TEXT DEFAULT CURRENT_TIMESTAMP
        , Content TEXT);";

        using (SqliteConnection connection = new(connectionString))
        {
            await connection.OpenAsync();
            using (SqliteCommand command = new(seedQuery, connection))
            {
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    static public async Task CreateNote(string noteName, string content)
    {
        string insertQuery = @"
        INSERT INTO Notes (Notename, Content)
        VALUES (@noteName, @content);";

        using (SqliteConnection connection = new(connectionString))
        {
            await connection.OpenAsync();

            using (SqliteCommand command = new(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@noteName", noteName);
                command.Parameters.AddWithValue("@content", content);

                await command.ExecuteNonQueryAsync();
            }
        }
    }

    static public async Task<List<Note>> ReadAllNotes()
    {
        string readAllQuery = @"
        SELECT * from Notes;";

        List<Note> notes = [];

        using (SqliteConnection connection = new(connectionString))
        {
            await connection.OpenAsync();

            using (SqliteCommand command = new(readAllQuery, connection))
            {
                using (SqliteDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            notes.Add(new Note(Convert.ToInt32(reader["NoteID"]), (string)reader["NoteName"], Convert.ToInt32(reader["CreatedBy"]),
                            (string)reader["Created"], Convert.ToInt32(reader["EditedBy"]), (string)reader["LastEdited"], (string)reader["Content"]));
                        }
                    }
                }
            }
        }

        return notes;
    }

    static public async Task<List<Note>> ReadAllNotesByTime(DateTime filterDate, bool before)
    {
        string readAllTimeQuery;
        if (!before)
        {
            readAllTimeQuery = @"
            SELECT * from Notes
            WHERE Created >= @filterDate;";
        }
        else
        {
            readAllTimeQuery = @"
            SELECT * from Notes
            WHERE Created < @filterDate;";
        }


        List<Note> notes = [];

        using (SqliteConnection connection = new(connectionString))
        {
            await connection.OpenAsync();

            using (SqliteCommand command = new(readAllTimeQuery, connection))
            {
                var tempDate = filterDate.ToString("yyyy-MM-dd HH:mm:ss");
                command.Parameters.AddWithValue("@filterDate", filterDate.ToString("yyyy-MM-dd HH:mm:ss"));

                using (SqliteDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            notes.Add(new Note(Convert.ToInt32(reader["NoteID"]), (string)reader["NoteName"], Convert.ToInt32(reader["CreatedBy"]),
                            (string)reader["Created"], Convert.ToInt32(reader["EditedBy"]), (string)reader["LastEdited"], (string)reader["Content"]));
                        }
                    }
                }
            }
        }

        return notes;
    }

    static public async Task<Note> ReadNote(int noteId)
    {
        string readQuery = @"
        SELECT * FROM Notes
        WHERE NoteID = @noteId;";

        using (SqliteConnection connection = new(connectionString))
        {
            await connection.OpenAsync();

            using (SqliteCommand command = new(readQuery, connection))
            {
                command.Parameters.AddWithValue("@noteId", noteId);

                using (SqliteDataReader reader = await command.ExecuteReaderAsync())
                {
                    await reader.ReadAsync();
                    var note = new Note(Convert.ToInt32(reader["NoteID"]), (string)reader["NoteName"], Convert.ToInt32(reader["CreatedBy"]),
                            (string)reader["Created"], Convert.ToInt32(reader["EditedBy"]), (string)reader["LastEdited"], (string)reader["Content"]);
                    return note;
                }
            }
        }
    }

    static public async Task UpdateNote(int noteId, string content)
    {
        string updateQuery = @"
        UPDATE Notes
        SET
            EditedBy = @editedBy,
            LastEdited = @edited,
            Content = @content
        WHERE NoteID = @noteId;";

        using (SqliteConnection connection = new(connectionString))
        {
            await connection.OpenAsync();

            using (SqliteCommand command = new(updateQuery, connection))
            {
                // Placeholder for future user system
                command.Parameters.AddWithValue("@editedBy", 1);
                // Actual update
                command.Parameters.AddWithValue("@edited", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@content", content);
                command.Parameters.AddWithValue("@noteId", noteId);

                await command.ExecuteNonQueryAsync();
            }
        }
    }

    static public async Task DeleteNote(int noteId)
    {
        string deleteQuery = @"
        DELETE FROM Notes
        WHERE NoteID = @noteId;";

        using (SqliteConnection connection = new(connectionString))
        {
            await connection.OpenAsync();

            using (SqliteCommand command = new(deleteQuery, connection))
            {
                command.Parameters.AddWithValue("@noteId", noteId);

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}