namespace Notekeeper
{
    //Menu options include a label and what the option should do
    public class MenuOption(string label, Action action)
    {
        public string Label { get; set; } = label;
        public Action Action { get; set; } = action;
    }
}