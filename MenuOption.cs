namespace Notekeeper
{
    public class MenuOption(string label, Action action)
    {
        public string Label { get; set; } = label;
        public Action Action { get; set; } = action;
    }
}