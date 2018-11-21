using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MapBuilder.Tile;
using MapBuilder.Biomes;
using MapBuilder.MathModel;
using System.Threading;
using System.Linq.Expressions;
using System.Reflection;

namespace MapBuilder
{
    class BuildMap
    {
        //private Color orange = Color.FromArgb(0, 0, 0);
        private Color red = Color.FromArgb(145, 17, 17);
        private Color blue = Color.FromArgb(19, 19, 220);
        private Color brown = Color.FromArgb(50, 30, 20);
        private Color green = Color.FromArgb(17, 163, 17);
        private Color tree = Color.FromArgb(13, 91, 13);
        

        private const int firstLevel = 15;
        private const int secondLevel = 35;
        private const int thirdLevel = 60;
        private const int fourthLevel = 95;



        private bool Threading = true;

        

        private static Fibb _fibb = new Fibb();
        private GroupBox _gb;
        private Control _control;
        private PictureBox _pb;
        private Bitmap _bm;

        public Map map = new Map();
        Random r = new Random();

        private static int MaxThreads = 20;
        List<List<int>> ChangedTiles = new List<List<int>>();
        public List<List<MapChange>> MapChanges { get; set; } = new List<List<MapChange>>();

        List<Timer.TimerEvent> timerEvents = new List<Timer.TimerEvent>();

        public Button DropButton { get; set; }
        public object Orange { get; private set; }

        public BuildMap(GroupBox mainBox)
        {
            _gb = mainBox;

            PictureBox pb = new PictureBox();
            pb.Location = new Point();
            pb.Size = _gb.Size;
            Console.WriteLine(pb.Size);
            pb.BackColor = Color.White;
            _gb.Controls.Add(pb);
            _pb = pb;

            //_pb.Dock = DockStyle.Fill;


            //1125 855
            InitializeMap(pb,1125,855);
        }

        public void Build(object sender, EventArgs e)
        {
            List<Thread> threads = new List<Thread>();
            DequeueObject dequeueObject;

            for (int t = 0; t < MaxThreads; t++)
            {
                MapChanges.Add(new List<MapChange>());
                ChangedTiles.Add(new List<int>());
                
                if (Threading)
                {
                    threads.Add(new Thread(new ParameterizedThreadStart(EnqueueDrops)));
                    threads[t].Start(t);
                }
                else
                {
                    EnqueueDrops(MapChanges[t]);
                }
            }
            if (Threading)
            {
                foreach (var thread in threads)
                {
                    thread.Join();
                }

                threads.Clear();
            }

             Console.WriteLine("Drops Enqueued: " + MapChanges[0].Count);

            Timer.TimerEvent timerEventThreaded = new Timer.TimerEvent("Threaded Dequeue", MaxThreads * 10000);
            Timer.TimerEvent timerEventNonThreaded = new Timer.TimerEvent("Not Threaded Dequeue", MaxThreads * 10000);
            Timer.TimingManager.Start();
            if (Threading)
            {
                timerEventThreaded.StartEvent();
            }
            else
            {
                timerEventNonThreaded.StartEvent();
            }

            
            for (int i = 0; i < MaxThreads; i++)
            {
                if (Threading)
                {
                    dequeueObject = new DequeueObject(MapChanges[i], ChangedTiles[i]);
                    threads.Add(new Thread(new ThreadStart(() => DequeueDrops(dequeueObject))));
                    threads[i].Start();
                }
                else
                {
                    dequeueObject = new DequeueObject(MapChanges[i], ChangedTiles[i]);
                    DequeueDrops(dequeueObject);
                }

            }
            if (Threading)
            {
                foreach (var thread in threads)
                {
                    thread.Join();
                }
                threads.Clear();
            }

            
            if (Threading)
            {
                //timerEventThreaded.FinishEvent(true);
            }
            else
            {
                timerEventNonThreaded.FinishEvent(true);
            }
            Timer.TimingManager.Stop();

            foreach(var t in timerEvents)
            {
                //t.WriteToFile();
            }

            PostProcessing(map.Tiles);
            MapChanges.Clear();
            ChangedTiles.Clear();
            GenerateImage();
            /*
            for (int i = 0; i <  MaxThreads; i++)
            {
                threads.Add(new Thread(new ThreadStart(() => PostProcessing(ChangedTiles[i]))));
                threads[i].Start();
            }


            form.Label.Invoke((MethodInvoker)delegate {
            
            form.Label.Text = newText;

            });
            

            foreach (var thread in threads)
            {
                thread.Join();
            }
            threads.Clear();
            */
            _pb.Image = _bm;
            _pb.Refresh();

            DropButton.Invoke((MethodInvoker)delegate
            {
                DropButton.PerformClick();

            });
            //WriteToConsole();
        }

        private void WriteToConsole()
        {
            for(int i = 0; i < map.Width; i++)
            {
                for(int j = 0; j< map.Height; j++)
                {
                    Console.Write(map.MapTiles[i,j].Height + " ");
                }
                Console.WriteLine();
            }
        }

        public void EnqueueDrops(Object t)
        {
            int i = 0;

            List<MapChange> mc = new List<MapChange>();

            while (i < 100000)
            {
                AddDropToQueue(mc);
                i++;
                
            }
            
            MapChanges[(int)t] = mc;
        }

        private void AddDropToQueue(List<MapChange> mapChanges)
        {
            ITile tile;
            int index, modelEnum;
            Random random = new Random();
            
                tile = map.MapTiles[random.Next(map.Width), random.Next(map.Height)];
                modelEnum = random.Next(_fibb.Models.Count);
                index = random.Next(_fibb.Models[modelEnum].Count);
            
            
            
            mapChanges.Add(new MapChange(tile, modelEnum, index));
            //map.AddChangeToQueue(new MapChange(tile,modelEnum,index));
            
        }

        private void DequeueDrops(Object Dequeue)
        {
            int start, dist;
            bool sign;
            DequeueObject dequeueObject = (DequeueObject)Dequeue;
            List<int> Tiles = dequeueObject.Tiles;
            List<MapChange> mapChanges = dequeueObject.MapChanges;

            int modelCount = Enum.GetNames(typeof(Fibb.ModelChanges)).Length;
            ITile tile, tempTile;
            List<int> model;
            List<ITile> borderTiles;
            //Console.WriteLine("Thread ID: "+ Thread.CurrentThread.ManagedThreadId);
            Timer.TimerEvent timerEvent = new Timer.TimerEvent("Before Propogation id: " + Thread.CurrentThread.ManagedThreadId, 10000);
           
                

            timerEvents.Add(timerEvent);
            timerEvent.StartEvent();
            
            
            //Console.WriteLine(MapChanges.Count);
            foreach (var mp in mapChanges)
            {
                tile = mp.Tile;

                model = _fibb.Models[mp.Enum];

                start = model[mp.Index];

                map.BiomeIds++;
                sign = map.DetermineSign() > 50 ? false : true;

                tile.BiomeId = map.BiomeIds;
                tile.ChangeHeight(start, sign);
                
                borderTiles = new List<ITile>();

                tile.AddToBorderList(borderTiles, tile);

                while (borderTiles.Count > 0)
                {
                    int index = r.Next(borderTiles.Count);
                    
                    tempTile = borderTiles[index];

                    dist = tile.GetDist(tempTile);

                    if (dist < mp.Index)
                    {
                        tempTile.ChangeHeight(model[mp.Index - dist], sign);

                        tempTile.BiomeId = map.BiomeIds;

                        tempTile.AddToBorderList(borderTiles, tempTile);
                    }
                    else
                    {
                        borderTiles.Remove(tempTile);
                    }

                    
                }
            }
        
            
            timerEvent.FinishEvent(false);
        }

        private void GenerateImage()
        {
            foreach(ITile tile in map.Tiles)
            {
                for (int x = 0; x < tile.TileWidth; x++)
                {
                    for (int y = 0; y < tile.TileHeight; y++)
                    {
                        _bm.SetPixel((tile.TileWidth * tile.XLocation) + x, (tile.TileHeight * tile.YLocation) + y, tile.color);
                    }
                }
            }

            _pb.Image = _bm;
            _pb.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void PostProcessing(Object tiles)
        {
            List<ITile> Tiles = (List<ITile>)tiles;

            for (int i = 0; i < Tiles.Count; i++)
            {
                //ITile tile = map.Tiles[Tiles[i]];
                ITile tile = Tiles[i];

                if (tile.Height <= firstLevel)
                {
                    tile.color = blue;
                }
                else if (tile.Height > firstLevel && tile.Height <= secondLevel)
                {
                    tile.color = green;
                }
                else if (tile.Height > secondLevel && tile.Height <= thirdLevel)
                {
                    tile.color = tree;
                }
                else if (tile.Height > thirdLevel && tile.Height <= fourthLevel)
                {
                    tile.color = brown;
                }
                else if (tile.Height > fourthLevel)
                {
                    tile.color = red;
                }
                
                
            }

            
            //List<int> tileQueue = (List<int>)tiles;
            //Console.WriteLine("Clearing "+ tileQueue.Count+ " Tiles");
            //tileQueue.Clear();
            
        }

        private void InitializeMap(PictureBox pictureBox, int widthAmount = 25, int heightAmount = 25)
        {
            map.Width = widthAmount;
            map.Height = heightAmount;
            _control = new Control();

            
            int width = pictureBox.Width / map.Width;
            int height = pictureBox.Height / map.Height;

            _bm = new Bitmap((width * widthAmount),(height * heightAmount));
            
            List<int> tiles = new List<int>();

            map.Start();
            map.MapTiles = new ITile[widthAmount, heightAmount];

            for (int i = 0; i < widthAmount; i++)
            {
                for (int j = 0; j < heightAmount; j++)
                {
                    ITile tile = new Tile.Tile((i * heightAmount) + j, -1, i, j);

                    tile.map = map;
                    tile.BiomeId = 0;
                    tile.TileHeight = (int)height;
                    tile.TileWidth = (int)width;
                    map.MapTiles[i, j] = tile;
                    map.TotalHeight += tile.TileHeight;
                    map.Tiles.Add(tile);

                    tiles.Add(tile.Id);
                    
                    if(tiles.Count > 1000)
                    {
                        //PostProcessing(tiles);
                    }
                }
            }

            for (int i = 0; i < widthAmount; i++)
            {
                for (int j = 0; j < heightAmount; j++)
                {
                    map.MapTiles[i, j].SetBorders();
                }
            }

            //while (tiles.Count > 0)
            {
                //PostProcessing(tiles);
                PostProcessing(map.Tiles);
            }
            GenerateImage();
            _pb.Image = _bm;

        }

        private int GetLock(int intLock)
        {


            return Interlocked.CompareExchange(ref intLock, 1, 0);
        }

        private void ReleaseLock(int intLock)
        {
            Interlocked.Decrement(ref intLock);
        }

        private PictureBox CreatePictureBox(Control parent, double width, double height, Control leftControl,
            Control aboveControl, double xPadding, double yPadding, Color backColor)
        {
            PictureBox pictureBox = new PictureBox();
            parent.Controls.Add(pictureBox);
            pictureBox.Location =
                new Point(
                    ReturnWidConvert(
                        (leftControl.Location.X + leftControl.Size.Width) / (double)parent.Width + xPadding, parent),
                    ReturnHitConvert(
                        (aboveControl.Location.Y + aboveControl.Size.Height) / (double)parent.Height + yPadding, parent));
            pictureBox.Height = ReturnHitConvert(height, parent);
            pictureBox.Width = ReturnWidConvert(width, parent);
            pictureBox.BackColor = backColor;
            return pictureBox;
        }

        private static int ReturnWidConvert(double integer, Control control)
        {
            return (int)Math.Round(integer * control.Width);
        }


        private static int ReturnHitConvert(double integer, Control control)
        {
            return (int)Math.Round(integer * control.Height);
        }


        private static int ReturnFontConvert(double fontSize)
        {
            return (int)Math.Round(fontSize * Screen.PrimaryScreen.Bounds.Width / 1920);
        }


    }

    
}
