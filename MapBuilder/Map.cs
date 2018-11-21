using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapBuilder.Biomes;
using MapBuilder.Tile;

namespace MapBuilder
{
     class Map
    {
         public ITile[,] MapTiles { get; set; }
        public List<ITile> Tiles { get; set; } = new List<ITile>(); 
         public int BiomeIds { get; set; } = 0;
         public List<IBiome> MapBiomes { get; set; }
         public Queue<MapChange> MapChanges { get; set; }
         public int TotalHeight { get; set; }
         public int Width { get; set; }
         public int Height { get; set; }
        
        public Map()
        {
            MapChanges = new Queue<MapChange>();
        }
        public Map(int width,int height)
        {
            Width = width;
            Height = height;
            MapBiomes = new List<IBiome>();
        }

         public void Start()
         {
            MapBiomes = new List<IBiome>();
         }

         public int DetermineSign()
         {
            int average = (int)((double)TotalHeight / (Width * Height));

            //Console.WriteLine("Average: "+average + " TotalHeight: "+ TotalHeight);
            if (average < 0) average = 0;
            return average;
         }
        

         public void AddChangeToQueue(MapChange mp)
         {
            MapChanges.Enqueue(mp);
         }


    }
}
