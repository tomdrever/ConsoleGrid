using System;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace ConsoleGrid
{
    public static class ItemManager
    {
        private static Item[] _items;

        public static void LoadItems()
        {
            var sr = new StreamReader("F:/ProjectDirectory/ConsoleGrid/ConsoleGrid/Resources/Items.json");

            _items = JsonConvert.DeserializeObject<Item[]>(sr.ReadToEnd());
        }

        public static Item NewItem()
        {
            var r = new Random();
            return _items[r.Next(0, _items.Length)];
        }
    }
}
