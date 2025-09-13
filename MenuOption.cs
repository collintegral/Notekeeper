using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace Notekeeper
{
    public class MenuOption(string label, Action action)
    {
        public string Label { get; set; } = label;
        public Action Action { get; set; } = action;
    }
}