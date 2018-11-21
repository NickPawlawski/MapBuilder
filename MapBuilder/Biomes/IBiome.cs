using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapBuilder.Tile;
using System.Drawing;

namespace MapBuilder.Biomes
{
    interface IBiome
    {
        int Id { get; set; }
        Color Color { get; set; }
        
        string Name { get; set; }
    }
}
