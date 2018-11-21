using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapBuilder.Tile;

namespace MapBuilder
{
    class DequeueObject
    {
        public List<MapChange> MapChanges { get; set; }
        public List<int> Tiles { get; set; }

        public DequeueObject(List<MapChange> mapChanges, List<int> tiles)
        {
            MapChanges = mapChanges;
            Tiles = tiles;
        }


    }
}
