namespace Notekeeper
{
    //Menu options include a label and what the option should do
    public class MenuOption
    {
        public string Label { get; set; }
        public Delegate Action { get; set; }

        public MenuOption(string label, Delegate action)
        {
            Label = label;
            Action = action;
        }

        public void Execute(params object[] args)
        {
            var expectedParams = Action.Method.GetParameters().Length;  

            if (expectedParams == 0)
            {
                Action.DynamicInvoke();
            }
            else
            {
                Action.DynamicInvoke(args);
            }
        }
    }
}