using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ConsoleGrid
{
    public class Room : MultiTile
    {
        public List<List<Tile>> Grid { get; set; }

        public Room(int x, int y, CharSet charSet, bool bordered)
        {
            Grid = LoadBaseGrid(x, y, charSet, bordered);
            
            Grid = LoadGridContents(charSet, Grid);
        }

        public static Room RandomRoom(CharSet charSet)
        {
            var r = new Random();
            return new Room(r.Next(4, 16), r.Next(8, 20), charSet, true);
        }

        public static Room LoadRoom(CharSet charSet, string fileName)
        {
            var sr = new StreamReader($"F:/ProjectDirectory/ConsoleGrid/ConsoleGrid/Resources/{fileName}.json");

            var charGrid = JsonConvert.DeserializeObject<List<List<string>>>(sr.ReadToEnd());
            var room = new Room(charGrid.Count, charGrid[0].Count, charSet, false);

            for (var i = 0; i < charGrid.Count; i++)
            {
                for (var j = 0; j < charGrid[0].Count; j++)
                {
                    if (charGrid[i][j] == "w")
                    {
                        room.Grid[i][j] = new Tile(charSet) {Foreground = charSet.Wall};
                    }
                    else
                    {
                        room.Grid[i][j] = new Tile(charSet);
                    }
                }
            }

            return room;
        }

        private static List<List<Tile>> LoadBaseGrid(int x, int y, CharSet charSet, bool bordered)
        {
            var tempGrid = new List<List<Tile>>();

            //Init grid
            for (var i = 0; i < x; i++)
            {
                var list = new List<Tile>();
                for (var j = 0; j < y; j++)
                {
                    var t = new Tile(charSet);

                    //Border grid with walls
                    if (bordered && (i == 0 || i == x - 1 || j == 0 || j == y - 1))
                    {
                        t.Foreground = charSet.Wall;
                    }
                    list.Add(t);
                }
                tempGrid.Add(list);
            }

            return tempGrid;
        }

        private static List<List<Tile>> LoadGridContents(CharSet charSet, List<List<Tile>> grid)
        {
            //Randoms
            var r = new Random();
            var rx = r.Next(0, grid.Count - 1);
            var ry = r.Next(0, grid[0].Count);

            //Distribute items
            for (var i = 0; i < r.Next(0, 3); i++)
            {
                grid[rx][ry].Foreground = grid[rx][ry].Foreground != charSet.Wall ? charSet.Item : charSet.Wall;

                //Re-randomise
                rx = r.Next(0, grid.Count - 1);
                ry = r.Next(0, grid[0].Count);
            }

            //Place door in lowest available row
            for (var i = grid.Count - 1; i >= 0; i--)
            {
                //Find all tiles on lowest row that aren't items
                var tiles = grid[i].FindAll(tile => tile.Foreground != charSet.Wall && tile.Foreground != charSet.Item);

                //Select one at random to be the door
                if (tiles.Count <= r.Next(0, tiles.Count)) continue;
                grid = LoadMultiTile(new MultiTileObject(charSet, charSet.Door, 2, 1), r.Next(1, tiles.Count - 1), i, grid);
                break;
            }

            return grid;
        }

        private static List<List<Tile>> LoadMultiTile(MultiTile multiTile, int x, int y, List<List<Tile>> grid)
        {
            foreach (var row in multiTile.Grid)
            {
                foreach (var tile in row)
                {
                    grid[y][x] = tile;
                    x++;
                }
                y++;
            }

            return grid;
        }
    }
}
