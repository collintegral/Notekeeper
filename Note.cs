namespace Notekeeper
{
    public class Note
    {
        public int NoteId { get; set; }
        public string NoteName { get; set; }
        public int UserId { get; set; }
        public string Created { get; set; }
        public int EditedBy { get; set; }
        public string LastEdited { get; set; }
        public string Content { get; set; }
        
        public Note(int noteId, string noteName, int userId, string created, int editedBy, string lastEdited, string content)
    {
        NoteId = noteId;
        NoteName = noteName;
        UserId = userId;
        Created = created;
        EditedBy = editedBy;
        LastEdited = lastEdited;
        Content = content;
    }
    }
}