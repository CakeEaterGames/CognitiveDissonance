using MonoCake.Objects;
using MonoCake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;
using Microsoft.Xna.Framework;

namespace CognitiveDissonance
{
    public class Level : Scene
    {
        public DualLevel levelGroop;
        public Player player = new Player();

        public List<Block> Blocks = new List<Block>();
        public Dictionary<int, Block> ids = new Dictionary<int, Block>();
        public List<Block> IsPickable = new List<Block>();
        public List<Block> IsOpenable = new List<Block>();
        public List<Block> SolidObjects = new List<Block>();

        public int tileW = 32;
        public int tileH = 32;

        public string LevelName = "test";

        public double SpawnX = 0;
        public double SpawnY = 0;
        public double ExitX = 0;
        public double ExitY = 0;

        public string levelStr = "";

       public bool victory = false;

        public Level()
        {

        }
        public override void Init()
        {
            Build();


            player.Blocks = SolidObjects;
            player.level = this;
            player.AddUR(this);
            player.SetXY(SpawnX * tileW, SpawnY * tileH);
        }

        public void Loss()
        {

            levelGroop.showLevelB();
        }

        public override void Update()
        {
            Console.WriteLine((int)(player.X / 32));
            Console.WriteLine((int)(player.Y / 32));
            Console.WriteLine(player.GetRect());
            Console.WriteLine(ExitX * 32 +" "+ ExitY * 32);

            if (player.hitbox().Intersects(new Rectangle((int)ExitX * 32, (int)ExitY*32,64,64)))
            {
                victory = true;
            }
        }

        public void LoadJSON(int levelNumber, string part)
        {
            string objects = "objects";
            string name = "name";

            StreamReader sr = new StreamReader("Levels/" + levelNumber + ".json");
            string str = sr.ReadToEnd();

            levelStr = str;

            JObject a = JObject.Parse(levelStr);

            LevelName = a.SelectToken(name).ToString();
            //Console.WriteLine(levelName);
            BaseRenderParameters.ScaleH = double.Parse(a.SelectToken("scale").ToString());
            BaseRenderParameters.ScaleW = BaseRenderParameters.ScaleH;

            BaseRenderParameters.X = 32 * BaseRenderParameters.ScaleW * double.Parse(a.SelectToken("ofsetX").ToString());



            var c = a.SelectToken(part);
            SpawnX = double.Parse(c.SelectToken("spawn").SelectToken("x").ToString());
            SpawnY = double.Parse(c.SelectToken("spawn").SelectToken("y").ToString());
            ExitX = double.Parse(c.SelectToken("exit").SelectToken("x").ToString());
            ExitY = double.Parse(c.SelectToken("exit").SelectToken("y").ToString());
            List<JToken> objs = c.SelectToken(objects).ToList();

            foreach (JToken j in objs)
            {
                //string t = j.SelectToken(type).ToString();
                Block b = new Block();
                Blocks.Add(b);
                b.level = this;
                b.Configure(j);

                if (b.IsSolid)
                {
                    SolidObjects.Add(b);
                }
                if (b.id != 0)
                {
                    ids.Add(b.id, b);
                }
                if (b.Pickable)
                {
                    IsPickable.Add(b);
                }
                if (b.Openable)
                {
                    IsOpenable.Add(b);
                }


            }

            sr.Close();
        }

        public void Build()
        {
            foreach (var b in Blocks)
            {

                b.AddUR(this);
                b.X = tileW * b.GridX;
                b.Y = tileH * b.GridY;
            }
        }

        public override void Destruct()
        {
            base.Destruct();
            foreach (Block b in Blocks)
            {
                b.Destruct();
            }
            if (player.Holding != null)
            {
                player.Holding.Destruct();
            }
            player.Destruct();

            Blocks.Clear();
            ids.Clear();
            IsPickable.Clear();
            IsOpenable.Clear();
            SolidObjects.Clear();
        }


    }
}
