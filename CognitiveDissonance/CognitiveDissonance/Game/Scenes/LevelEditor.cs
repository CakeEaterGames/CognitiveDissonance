using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoCake;
using MonoCake.Objects;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CognitiveDissonance
{
    class LevelEditor : Scene
    {
        public LevelEditor()
        {
            Engine.cfg.BgColor = Color.Gray;
        }
        List<Block> Blocks = new List<Block>();

        GameObject setSelector = new GameObject();

        public override void Init()
        {
            setSelector.SetImg(GlobalContent.LoadImg("main", true));
            setSelector.AddUR(this);
            AddUR();

             importJSON("output");
        }

        public int CurrentBlock = 0;
        public Block.presets preset = Block.presets.bg;

        public override void Update()
        {
            double mx = KEY.MouseX/BaseRenderParameters.ScaleW;
            double my = KEY.MouseY / BaseRenderParameters.ScaleH;

            if (KEY.IsTyped(Keys.LeftControl))
            {
                Console.WriteLine("presets: " + preset);
            }

            if (KEY.IsTyped(Keys.D1)) preset = Block.presets.bg;
            if (KEY.IsTyped(Keys.D2)) preset = Block.presets.wall;
            if (KEY.IsTyped(Keys.D3)) preset = Block.presets.doorBot;
            if (KEY.IsTyped(Keys.D4)) preset = Block.presets.doorTop;
            if (KEY.IsTyped(Keys.D5)) preset = Block.presets.floorBtn;
            if (KEY.IsTyped(Keys.D6)) preset = Block.presets.wallBtn;
            if (KEY.IsTyped(Keys.D7)) preset = Block.presets.key;
            if (KEY.IsTyped(Keys.D8)) preset = Block.presets.box;
            if (KEY.IsTyped(Keys.D9)) preset = Block.presets.hole;



            if (KEY.IsDown(Keys.LeftShift))
            {
                setSelector.AddRender();
                if (KEY.LClick)
                {
                    CurrentBlock = (int)(mx / 32) + ((int)(my / 32) * 8);
                    Console.WriteLine("CurrentBlock: " + CurrentBlock);
                }
            }
            else
            {
                setSelector.RemoveRender();

                if (KEY.LClick && mx < 1280 && mx > 0 && my < 720 && my > 0)
                {
                    Block b = new Block();
                    Blocks.Add(b);


                    b.setPreset(preset);
                    if (preset == Block.presets.bg || preset == Block.presets.wall)
                    {
                        b.TileNumb.AddRange(new int[] { CurrentBlock, 1 });
                    }

                    b.AddTexture();
                    b.AddRender(this);
                    b.GridX = (int)(mx) / 32;
                    b.GridY = (int)(my) / 32;
                    b.SetXY(b.GridX * 32, b.GridY * 32);
                }

                if (KEY.RClick)
                {
                    for (int i = Blocks.Count - 1; i >= 0; i--)
                    {
                        if (Blocks[i].GetRect().Contains((int)mx, (int)my))
                        {
                            Blocks[i].Destruct();
                            Blocks.RemoveAt(i);
                            break;
                        }
                    }
                }
            }

            if (KEY.IsTyped(Keys.Left))
            {
                foreach (Block b in Blocks)
                {
                    b.GridX--;
                    b.X -= 32;
                }
            }
            if (KEY.IsTyped(Keys.Right))
            {
                foreach (Block b in Blocks)
                {
                    b.GridX++;
                    b.X += 32;
                }
            }
            if (KEY.IsTyped(Keys.Up))
            {
                foreach (Block b in Blocks)
                {
                    b.GridY--;
                    b.Y -= 32;
                }
            }
            if (KEY.IsTyped(Keys.Down))
            {
                foreach (Block b in Blocks)
                {
                    b.GridY++;
                    b.Y += 32;
                }
            }

            if (KEY.IsTyped(Keys.OemPlus))
            {
                BaseRenderParameters.ScaleH+=0.1;
                BaseRenderParameters.ScaleW += 0.1;
                Console.WriteLine("Scale: " + BaseRenderParameters.ScaleH);
            }
            if (KEY.IsTyped(Keys.OemMinus))
            {
                BaseRenderParameters.ScaleH -= 0.1;
                BaseRenderParameters.ScaleW -= 0.1;

                Console.WriteLine("Scale: "+ BaseRenderParameters.ScaleH);
            }

            if (KEY.IsTyped(Keys.Enter))
            {
                ExportJSON(Blocks);
            }
        }

        public void importJSON(string name)
        {
            string objects = "objects";

            StreamReader sr = new StreamReader("Levels/" + name + ".json");
            string str = sr.ReadToEnd();

            JObject a = JObject.Parse("{"+str+"}");
            var c = a;
            List<JToken> objs = c.SelectToken(objects).ToList();

            foreach (JToken j in objs)
            {
                //string t = j.SelectToken(type).ToString();
                Block b = new Block();
                Blocks.Add(b);
                b.Configure(j);
                b.X = 32 * b.GridX;
                b.Y = 32 * b.GridY;
                b.AddRender(this);
            }

            sr.Close();
        }
  
        public static void ExportJSON(List<Block> Blocks, string path = "Levels/output.json")
        {
            string res = "\"objects\": [";
            foreach (Block b in Blocks)
            {
                res += b.toJSON() + ",\n";
            }
            res = res.Remove(res.Length - 2);
            res += "]";
            StreamWriter sw = new StreamWriter(path);
            sw.WriteLine(res);
            sw.Flush();
            sw.Close();
            Console.WriteLine(res);
        }
    }
}
