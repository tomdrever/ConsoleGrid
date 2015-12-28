using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGrid
{
    public class ColourSet
    {
        public ConsoleColor Background { get; set; }
        public ConsoleColor Player { get; set; }
        public ConsoleColor Door { get; set; }
        public ConsoleColor Item { get; set; }
        public ConsoleColor Wall { get; set; }

        public static ColourSet DefaultSet()
        {
            return new ColourSet
            {
                Background = ConsoleColor.Green,
                Player = ConsoleColor.Red,
                Door = ConsoleColor.Yellow,
                Item = ConsoleColor.Cyan,
                Wall = ConsoleColor.DarkGreen
            };
        }
    }
}
