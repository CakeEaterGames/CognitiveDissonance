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
        public List<Block> Blocks = new List<Block>();
        public int tileW = 32;
        public int tileH = 32;

        public string LevelName = "test";

        public int SpawnX = 0;
        public int SpawnY = 0;
        public int ExitX = 0;
        public int ExitY = 0;
        public List<Block> SolidObjects = new List<Block>();

        public Level()
        {
            /*  for (int i= 0;i<2;i++)
              {
                  for (int j = 0; j < 2; j++)
                  {
                      Block a = new Block();
                      a.GridX = i;
                      a.GridY = j;
                      a.TileSet = "main";
                      a.TileNumb = 0;
                      Blocks.Add(a);
                  }
              }*/
           // BaseRenderParameters.ScaleH = 2;
           // BaseRenderParameters.ScaleW = 2;
            LoadJSON(2, "PartA");
            Build();

            Player p = new Player();
            p.Blocks = SolidObjects;
            p.AddUR(this);
            p.SetXY(SpawnX * tileW, SpawnY * tileH);
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

            var c = a.SelectToken(part);
            SpawnX = int.Parse(c.SelectToken("spawn").SelectToken("x").ToString());
            SpawnY = int.Parse(c.SelectToken("spawn").SelectToken("y").ToString());
            ExitX = int.Parse(c.SelectToken("exit").SelectToken("x").ToString());
            ExitY = int.Parse(c.SelectToken("exit").SelectToken("y").ToString());
            List<JToken> objs = c.SelectToken(objects).ToList();

            foreach (JToken j in objs)
            {
               // string t = j.SelectToken(type).ToString();
                Block b = new Block();
                Blocks.Add(b);
                b.Configure(j);

                if (b.IsSolid)
                {
                    SolidObjects.Add(b);
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
