using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

namespace ConsoleGrid
{
    public class Program
    {
        private static List<List<Tile>> _grid = new List<List<Tile>>();
        private static int[] _currentPosition = {1, 1};

        private static readonly Player Player = new Player();

        private static readonly CharSet CharSet = CharSet.DefaultSet();
        private static readonly ColourSet ColourSet = ColourSet.DefaultSet();

        private static void Main(string[] args)
        {
            Console.CursorVisible = false;

            ItemManager.LoadItems();

            var r = Room.RandomRoom(CharSet.DefaultSet());

            _grid = r.Grid;
            _grid[_currentPosition[1]][_currentPosition[0]].Player = CharSet.Player;

            Console.WriteLine("Welcome to this game! Please ignore the lag.");
            Console.WriteLine("Press any key to continue...");

            Console.ReadLine();

            // Enter first room
            Statistics.RoomsEntered = 1;

            while (true)
            {
                //Update inventorydisplay
                var items = Player.Inventory.Aggregate("", (current, item) => current + item.Name + ": " + item.Power + ", " + item.Protection + "\n");

                var inventoryDisplay = "--Current inventory-- \n" + (items != "" ? items : "Nothing, you worthless scrub.");

                //Update grid
                Console.Clear();
                _grid[_currentPosition[1]][_currentPosition[0]].Player = CharSet.Player;
                foreach (var outputrow in _grid.Select(row => row.Select(tile => tile.Player ?? tile.Foreground ?? tile.Background).ToList()))
                {
                    foreach (var character in outputrow)
                    {
                        #region Set character colour and draw character
                        if (character == CharSet.Door)
                        {
                            Console.ForegroundColor = ColourSet.Door;
                        }
                        else if (character == CharSet.Item)
                        {
                            Console.ForegroundColor = ColourSet.Item;
                        }
                        else if (character == CharSet.Wall)
                        {
                            Console.ForegroundColor = ColourSet.Wall;
                        }
                        else if (character == CharSet.Player)
                        {
                            Console.ForegroundColor = ColourSet.Player;
                        }
                        else if (character == CharSet.Background)
                        {
                            Console.ForegroundColor = ColourSet.Background;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        Console.Write(character);
                        #endregion
                    }
                    Console.Write("\n");
                }
                Console.ForegroundColor = ColourSet.Player;
                Console.WriteLine(inventoryDisplay);
                Console.SetCursorPosition(_currentPosition[0], _currentPosition[1]);
                

                RegisterInputs(Console.ReadKey(true).Key);
                
            }

            // ReSharper disable once FunctionNeverReturns
        }

        private static void RegisterInputs(ConsoleKey key)
        {
            Statistics.ButtonPresses++;
            switch (key)
            {
                #region Movement
                case ConsoleKey.LeftArrow:
                    if (_currentPosition[0] - 1 >= 0 && _currentPosition[0] - 1 <= _grid[0].Count - 1 && _grid[_currentPosition[1]][_currentPosition[0] - 1].Foreground != CharSet.Wall)
                    {
                        Move(Direction.Left);
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (_currentPosition[0] + 1 >= 0 && _currentPosition[0] + 1 <= _grid[0].Count - 1 && _grid[_currentPosition[1]][_currentPosition[0] + 1].Foreground != CharSet.Wall)
                    {
                        Move(Direction.Right);
                    }
                    break;
                case ConsoleKey.UpArrow:
                    if (_currentPosition[1] - 1 >= 0 && _currentPosition[1] - 1 < _grid.Count && _grid[_currentPosition[1] - 1][_currentPosition[0]].Foreground != CharSet.Wall)
                    {
                        Move(Direction.Up);
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (_currentPosition[1] + 1 >= 0 && _currentPosition[1] + 1 < _grid.Count && _grid[_currentPosition[1] + 1][_currentPosition[0]].Foreground != CharSet.Wall)
                    {
                        Move(Direction.Down);
                    }
                    break;
                #endregion
                case ConsoleKey.Enter: //Interact
                    if (_grid[_currentPosition[1]][_currentPosition[0]].Foreground == CharSet.Door)
                    {
                        var result = MessageBox.Show("Enter door?", "Door!", MessageBoxButtons.YesNo);

                        if (result == DialogResult.Yes)
                        {
                            Statistics.RoomsEntered++;
                            _grid = Room.RandomRoom(CharSet).Grid;
                            _currentPosition = new[] { 1, 1 };
                        }
                    }
                    else if (_grid[_currentPosition[1]][_currentPosition[0]].Foreground == CharSet.Item)
                    {
                        if (Player.Inventory.Count <= 8)
                        {
                            var newItem = ItemManager.NewItem();

                            var result = MessageBox.Show("Item: " + newItem.Name + "\n" + "Collect item?", "Found item!", MessageBoxButtons.YesNo);

                            if (result == DialogResult.Yes)
                            {
                                Statistics.ItemsCollected++;

                                Player.Inventory.Add(newItem);
                                _grid[_currentPosition[1]][_currentPosition[0]].Foreground = null;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Your inventory is full! Cannot pick up item.");
                        }
                    }
                    break;
                case ConsoleKey.I: //Display inventory
                    var items = Player.Inventory.Aggregate("", (current, item) => current + item.Name + ": " + item.Power + ", " + item.Protection + "\n");
                    MessageBox.Show(items != "" ? items : "Nothing, you worthless scrub.", "Current Inventory:");
                    break;
                case ConsoleKey.S: //Display statistics
                    var stats = string.Format("Button presses: " + Statistics.ButtonPresses + "\n" +
                                              "Items collected: " + Statistics.ItemsCollected + "\n" +
                                              "Rooms entered: " + Statistics.RoomsEntered);
                    MessageBox.Show(stats, "Statistics");
                    break;

            }
        }

        public static void Move(Direction direction)
        {
            _grid[_currentPosition[1]][_currentPosition[0]].Player = null;
            _grid[_currentPosition[1]][_currentPosition[0]].Background = CharSet.Background;

            switch (direction)
            {
                case Direction.Left:
                    Console.SetCursorPosition(_currentPosition[1], _currentPosition[0] - 1);
                    _currentPosition[0] = Console.CursorTop;
                    break;
                case Direction.Right:
                    Console.SetCursorPosition(_currentPosition[1], _currentPosition[0] + 1);
                    _currentPosition[0]++;
                    break;
                case Direction.Up:
                    Console.SetCursorPosition(_currentPosition[1] - 1, _currentPosition[0]);
                    _currentPosition[1]--;
                    break;
                case Direction.Down:
                    Console.SetCursorPosition(_currentPosition[1] + 1, _currentPosition[0]);
                    _currentPosition[1]++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }

            _grid[_currentPosition[1]][_currentPosition[0]].Player = CharSet.Player;
        }

        public enum Direction
        {
            Left,
            Right,
            Up,
            Down
        }
    }
}
