using MonoCake.Objects;
using MonoCake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;

namespace CognitiveDissonance
{
    public class Level : Scene
    {
       public Player player = new Player();

        public List<Block> Blocks = new List<Block>();
        public Dictionary<int, Block> ids = new Dictionary<int, Block>();
        public List<Block> IsPickable = new List<Block>();
        public List<Block> IsOpenable = new List<Block>();
        public List<Block> SolidObjects = new List<Block>();

        public int tileW = 32;
        public int tileH = 32;

        public string LevelName = "test";

        public int SpawnX = 0;
        public int SpawnY = 0;
        public int ExitX = 0;
        public int ExitY = 0;
       

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

            Gameplay.self.showLevelB();
        }

        public void LoadJSON(int levelNumber, string part)
        {
            string objects = "objects";
            string name = "name";

            StreamReader sr = new StreamReader("Levels/" + levelNumber + ".json");
            string str = sr.ReadToEnd();

            JObject a = JObject.Parse(str);
           
            LevelName = a.SelectToken(name).ToString();
            //Console.WriteLine(levelName);
            BaseRenderParameters.ScaleH = double.Parse(a.SelectToken("scale").ToString());
            BaseRenderParameters.ScaleW = BaseRenderParameters.ScaleH;

            BaseRenderParameters.X = 32 * BaseRenderParameters.ScaleW * double.Parse(a.SelectToken("ofsetX").ToString());

            

           var c = a.SelectToken(part);
            SpawnX = int.Parse(c.SelectToken("spawn").SelectToken("x").ToString());
            SpawnY = int.Parse(c.SelectToken("spawn").SelectToken("y").ToString());
            ExitX = int.Parse(c.SelectToken("exit").SelectToken("x").ToString());
            ExitY = int.Parse(c.SelectToken("exit").SelectToken("y").ToString());
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

        
    }
}
