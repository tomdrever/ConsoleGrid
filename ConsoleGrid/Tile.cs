using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGrid
{
    public class Tile
    {
        public char? Player;
        public char? Foreground;
        public char Background;

        public Tile(CharSet charSet)
        {
            //Load tile's default
            Background = charSet.Background;
        }
    }
}
