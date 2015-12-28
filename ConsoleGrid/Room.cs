using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleGrid
{
    public class Room
    {
        public List<List<Tile>> Grid;

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
                        room.Grid[i][j] = new Tile {Background = charSet.Background, Foreground = charSet.Wall};
                    }
                    else
                    {
                        room.Grid[i][j] = new Tile {Background = charSet.Background};
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
                    var t = new Tile { Background = charSet.Background };

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
                var enumerable = grid[i].FindAll(tile => tile.Foreground != charSet.Wall && tile.Foreground != charSet.Item);

                if (enumerable.Count <= r.Next(0, enumerable.Count)) continue;
                enumerable[r.Next(0, enumerable.Count)].Foreground = charSet.Door;
                break;
            }

            return grid;
        } 
    }
}
