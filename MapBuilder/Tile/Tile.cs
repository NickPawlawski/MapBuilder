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
    class Tile : ITile
    {
        public int Lock { get; set; } = 0;
        private int max = 99;
        private int min = 0;
        public int Id { get; }
        private int type;

        public int BiomeId { get; set; }
        public IBiome Biome { get; set; }
        public Map map { get; set; }
        public string ImageString { get; set; }
        public int XLocation { get; set; }
        public int YLocation { get; set; }
        public ITile LeftTile { get; set; }
        public ITile RightTile { get; set; }
        public ITile DownTile { get; set; }
        public ITile UpTile { get; set; }

        public int Height { get; set; }

        public int TileWidth { get; set; }
        public int TileHeight { get; set; }

        public string Coordinates()
        {
            return "X: "+XLocation + " Y: "+ YLocation;
        }

        

        public PictureBox PictureBox { get; set; }
        public Color color { get; set; }

        public Tile(int id, int type, int x, int y, int min = 0, int max = 100)
        {
            this.type = type;
            Id = id;
            XLocation = x;
            YLocation = y;
            Height = (max - min)/2;
            Height = 40;

        }

        public void ChangeHeight(int amount, bool sign)
        {
            if(sign)
            {
                Height += amount;
                
                if(Height > max)
                {
                    map.TotalHeight += amount % max;
                    Height = max;
                }
                else
                {
                    map.TotalHeight += amount;
                }
            }
            else
            {
                Height -= amount;

                if (Height < 0)
                {
                    map.TotalHeight -= (Height * -1);
                    Height = 0;
                }
                else
                {
                    map.TotalHeight -= amount;
                }
            }
        }

        public int GetDist(ITile tile)
        {
            return Math.Abs(XLocation - tile.XLocation) + Math.Abs(YLocation - tile.YLocation);
        }

        public List<ITile> AddToBorderList(List<ITile> border, ITile tile)
        {
            if (border.Contains(tile))
            {
                border.Remove(tile);
            }

            if (tile.LeftTile != null && !border.Contains(tile.LeftTile) && tile.BiomeId != LeftTile.BiomeId)
            {
                 border.Add(tile.LeftTile);
            }

            if (tile.RightTile != null && !border.Contains(tile.RightTile) && tile.BiomeId != RightTile.BiomeId)
            {
                 border.Add(tile.RightTile);
            }

            if (tile.UpTile != null && !border.Contains(tile.UpTile) && tile.BiomeId != UpTile.BiomeId)
            {
                 border.Add(tile.UpTile);
            }

            if (tile.DownTile != null && !border.Contains(tile.DownTile) && tile.BiomeId != DownTile.BiomeId)
            {
                 border.Add(tile.DownTile);
            }

            return border;
        }

        public void SetBorders()
        {
            if(XLocation == 0)
            {
                LeftTile = map.MapTiles[map.Width-1,YLocation];
            }
            else
            {
                LeftTile = map.MapTiles[XLocation-1,YLocation];
            }
            
            if(XLocation == map.Width-1)
            {
                RightTile = map.MapTiles[0, YLocation];
            }
            else
            {
                RightTile = map.MapTiles[XLocation + 1, YLocation];
            }

            if (YLocation == 0)
            {
                UpTile = map.MapTiles[XLocation, map.Height - 1]; 
            }
            else
            {
                UpTile = map.MapTiles[XLocation, YLocation - 1];
            }

            if (YLocation == map.Height - 1)
            {
                DownTile = map.MapTiles[XLocation, 0];
            }
            else
            {
                DownTile = map.MapTiles[XLocation, YLocation + 1];
            }


        }
        
        
    }
}
