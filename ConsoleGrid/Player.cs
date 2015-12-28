using System.Collections.Generic;
using System.Linq;

namespace ConsoleGrid
{
    public class Player
    {
        public List<Item> Inventory = new List<Item>();
        private int _negativity;
        public int Negativity {
            get
            {
                return Inventory.Where(item => item.Execrable)
                    .Select(item => item)
                    .Count();
            }
            set
            {
                _negativity = value;
            }
        }
    }
}
