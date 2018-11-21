using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MapBuilder.Biomes;

namespace MapBuilder.Tile
{
    interface ITile
    {
        int Lock { get; set; }
        int Id { get; }
        IBiome Biome { get; set; }
        int BiomeId { get; set; }
        Map map { get; set; }
        string ImageString { get; set; }
        int XLocation { get; set; }
        int YLocation { get; set; }
        ITile LeftTile { get; set; }
        ITile RightTile { get; set; }
        ITile DownTile { get; set; }
        ITile UpTile { get; set; }
        int Height { get; set; }

          int TileWidth { get; set; }
         int TileHeight { get; set; }

        Color color { get; set; }
        
        PictureBox PictureBox { get; set; }

        List<ITile> AddToBorderList(List<ITile> border, ITile tile);
        void ChangeHeight(int amount, bool sign);

        int GetDist(ITile tile);
        void SetBorders();

        

    }
}
