public static class IEnumerableExtensions {
    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)       
       => self.Select((item, index) => (item, index));
}

namespace Notekeeper
{
    //Menus contain a title, the list of Menu Options, and a selector to show the currently-chosen optioin.
    public class Menu(string title, List<MenuOption>? options = null)
    {
        private readonly string Title = title;
        public List<MenuOption> Options { get; } = options ?? [];
        private int Sel = 0;

        public static void AddOption(string label, Action action)
        {
            MenuOption newOption = new(label, action);
        }

        public void AddOption(MenuOption option)
        {
            Options.Add(option);
        }

// Displaying is done in a loop, with a > next to the chosen option.
        public Action Display()
        {
            var exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.CursorVisible = false;

                Console.WriteLine($"{title}\n");
                foreach (var (option, index) in Options.WithIndex())
                {
                    if (index == Sel)
                    {
                        Console.WriteLine($"> {option.Label}");
                    }
                    else
                    {
                        Console.WriteLine($"{option.Label}");
                    }
                }

                ConsoleKey key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        Sel = (Sel - 1 + Options.Count) % Options.Count;
                        break;
                    case ConsoleKey.DownArrow:
                        Sel = (Sel + 1) % Options.Count;
                        break;
                    case ConsoleKey.Enter:
                        Options[Sel].Execute();
                        break;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;
                }
            }
            return new Action(() => { });
        }
    }
}