using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleGrid
{
    public class CharSet
    {
        public char Background { get; set; }
        public char Player { get; set; }
        public char Door { get; set; }
        public char Item { get; set; }
        public char Wall { get; set; }

        public static CharSet LetterSet()
        {
            return new CharSet
            {
                Background = 'B',
                Player = 'P',
                Door = 'D',
                Item = 'I',
                Wall = 'W'
            };
        }

        public static CharSet DefaultSet()
        {
            return new CharSet
            {
                Background = 'o',
                Player = '+',
                Door = '-',
                Item = '*',
                Wall = '='
            };
        }
    }
}
