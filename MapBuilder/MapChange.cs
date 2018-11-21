using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapBuilder.Tile;

namespace MapBuilder
{
    class MapChange
    {
        public int Amount { get; set; }
        public ITile Tile { get; set; }
  
        public int Enum { get; set; }

        public int Index { get; set; }
        public MapChange(ITile tile, int Enum, int index)
        {
            Tile = tile;
            this.Enum = Enum;
            Index = index;
        }
    }
}
