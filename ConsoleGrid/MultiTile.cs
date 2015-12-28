using System.Collections.Generic;

namespace ConsoleGrid
{
    public interface MultiTile
    {
        List<List<Tile>> Grid { get; set; }
    }
}