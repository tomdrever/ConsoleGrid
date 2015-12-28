using System.Collections.Generic;

namespace ConsoleGrid
{
    public class MultiTileObject : MultiTile
    {
        public List<List<Tile>> Grid { get; set; }

        public MultiTileObject(CharSet charSet, char foregroundTile, int width, int height)
        {
            Grid = new List<List<Tile>>();

            for (var i = 0; i < height; i++)
            {
                var tempRow = new List<Tile>();
                for (var j = 0; j < width; j++)
                {
                    tempRow.Add(new Tile(charSet) {Foreground = foregroundTile});
                }
                Grid.Add(tempRow);
            }
        }
    }
}