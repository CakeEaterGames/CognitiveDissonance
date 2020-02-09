using Microsoft.Xna.Framework;
using MonoCake.Objects;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CognitiveDissonance
{
    public class Block : GameObject
    {
        public Level level;

        public string preset = "";

        public int id = 0;
        public int GridX = 0;
        public int GridY = 0;

        public string TileSet = "main";
        public List<int> TileNumb = new List<int>();

        public bool IsSolid = false;
        public bool Kills = false;
        public bool Pickable = false;
        public bool Openable = false;
        public bool IsKey = false;
        public bool Falling = false;
        public bool Stepable = false;
        public bool Clickable = false;
        public bool Anti = false;

        public List<int> Opens = new List<int>();
        public List<int> Toggles = new List<int>();
        public List<int> Links = new List<int>();

        public List<double> Moves = new List<double>();

        bool stepped = false;
 
        public enum presets
        {
            floorBtn,
            wallBtn,
            doorTop,
            doorBot,
            box,
            key,
            hole,
            bg,
            wall,
        }

        public void setPreset(presets set)
        {
            switch (set)
            {
                case presets.floorBtn:
                    preset = "floorBtn";
                    Stepable = true;
                    Toggles.Add(0);
                    TileSet = "main";
                    TileNumb.AddRange(new int[] { 4, 30, 5, 30 });
                    Stop();
 
                    break;
                case presets.wallBtn:
                    preset = "wallBtn";
                    Clickable = true;
                    Opens.Add(0);
                    TileSet = "main";
                    TileNumb.AddRange(new int[] { 12, 30, 13, 30 });
                    Stop();
 
 
                    break;
                case presets.doorTop:
                    preset = "doorTop";
                    IsSolid = true;
                    Openable = true;
                    Links.Add(0);
                    TileSet = "main";
                    TileNumb.AddRange(new int[] { 6, 30, 7, 30 });
                    Stop();
                    id = 1;
                    
                    break;
                case presets.doorBot:
                    preset = "doorBot";
                    IsSolid = true;
                
                    TileSet = "main";
                    TileNumb.AddRange(new int[] { 14, 30, 15, 30 });
                    Stop();
                    id = 1;
 
                    break;
                case presets.box:
                    preset = "box";
                    IsSolid = true;
                    Falling = true;
                    Pickable = true;
                    TileSet = "main";
                    TileNumb.AddRange(new int[] { 22, 1 });
          
                    break;
                case presets.key:
                    preset = "key";
 
                    Falling = true;
                    Pickable = true;
                    IsKey = true;
                    TileSet = "main";
                    TileNumb.AddRange(new int[] { 23, 1 });
 
                    break;
                case presets.hole:
                    preset = "hole";
                    Kills = true;
                    TileSet = "main";
                    TileNumb.AddRange(new int[] { 46, 5, 54, 5, 55, 5, 62, 5, 63, 5 });
 
                    break;
                case presets.bg:
                    preset = "bg";
                    TileSet = "main";
                  
                    break;
                case presets.wall:
                    preset = "wall";
                    TileSet = "main";
                    IsSolid = true;
                    break;
            }
        }

        public string toJSON()
        {
            string res = "";
            res += "{";
            if (IsSolid)
            {
                res += "'IsSolid' : true,";
            }
            if (Kills)
            {
                res += "'Kills' : true,";
            }
            if (Pickable)
            {
                res += "'Pickable' : true,";
            }
            if (Openable)
            {
                res += "'Openable' : true,";
            }
            if (IsKey)
            {
                res += "'IsKey' : true,";
            }
            if (Falling)
            {
                res += "'Falling' : true,";
            }
            if (Stepable)
            {
                res += "'Stepable' : true,";
            }
            if (Clickable)
            {
                res += "'Clickable' : true,";
            }

            if (Opens.Count > 0)
            {
                res += "'Opens' : [ ";
                foreach (var a in Opens)
                {
                    res += a + ",";
                }
                res = res.Remove(res.Length - 1);
                res += "],";
            }
            if (Toggles.Count > 0)
            {
                res += "'Toggles' : [ ";
                foreach (var a in Toggles)
                {
                    res += a + ",";
                }
                res = res.Remove(res.Length - 1);
                res += "],";
            }
            if (Links.Count > 0)
            {
                res += "'Links' : [ ";
                foreach (var a in Links)
                {
                    res += a + ",";
                }
                res = res.Remove(res.Length - 1);
                res += "],";
            }

            if (Moves.Count > 0)
            {
                res += "'Moves' : [ ";
                for (int i = 0; i < Moves.Count; i += 3)
                {
                    res += "[";
                    res += Moves[i] + ",";
                    res += Moves[i + 1] + ",";
                    res += Moves[i + 2];
                    res += "],";
                }
                /*  foreach (var a in Links)
                  {
                      res += a + ",";
                  }*/
                res = res.Remove(res.Length - 1);
                res += "],";
            }

            if (!IsPlaying)
            {
                res += "'Stop' : true,";
            }

            if (id != 0)
            {
                res += "'id' : "+id+",";
            }

            res += "'TileSet' : 'main',";

            res += "'TileNumb' : [";
            foreach (int i in TileNumb)
            {
                res += i +",";
            }
            res = res.Remove(res.Length - 1);
            res += "],";

            res += "'x' : " + GridX + ",";
            res += "'y' : " + GridY + "";
            while (res.IndexOf("'") > -1) res = res.Replace("'", "\"");

            res += "}";


            /*
               public List<int> Opens = new List<int>();
        public List<int> Toggles = new List<int>();
        public List<int> Links = new List<int>();

        public List<double> Moves = new List<double>();
             */

            return res;
        }

        public override void Update()
        {
            updateStepAndClick();
            updateOpen();
            updateGrav();
            updateMove();
            updateDanger();
        }

        void updateDanger()
        {
            if (Kills)
            {
                if (level.player.hitbox().Intersects(GetRect()))
                {
                    level.Loss();
                }
            }
        }

        int moveTimer = 0;
        int moveSet = 0;
        void updateMove()
        {
            if (Moves.Count > 0)
            {
                X += Moves[0 + moveSet * 3];
                Y += Moves[1 + moveSet * 3];
                if (!level.player.carried)
                {
                    var r = level.player.hitbox();
                    r.Y += 5;
                    if (r.Intersects(GetRect()))
                    {
                        level.player.X += Moves[0 + moveSet * 3];
                        level.player.Y += Moves[1 + moveSet * 3];
                        level.player.carried = true;
                    }
                }

                if (moveTimer == Moves[2 + moveSet * 3])
                {
                    moveTimer = 0;
                    moveSet++;
                    moveSet %= Moves.Count / 3;
                }
                moveTimer++;
            }
        }

        public void open()
        {
            GotoAndStop(Frames.Count - 1);
            IsSolid = false;
        }
        public void close()
        {
            GotoAndStop(0);
            IsSolid = true;
        }


        void updateOpen()
        {
            if (Openable && IsSolid)
            {
                foreach (var b in level.IsPickable)
                {
                    if (b.IsKey && this.GetRect().Intersects(b.GetRect()))
                    {
                        b.Destruct();
                        if (level.player.Holding == b)
                        {
                            level.player.Holding = null;
                        }
                        b.IsKey = false;
                        b.IsSolid = false;
                        b.X = -100;
                        open();
                        foreach (int i in Links)
                        {
                            level.ids[i].open();
                        }
                    }
                }
            }
        }



        public double frY = 0;
        public double minFrY = -8;

        public double frYFade = 1;

        void updateGrav()
        {
            if (Falling && level.player.Holding != this)
            {
                frY += frYFade;

                if (frY < minFrY)
                {
                    frY = minFrY;
                }

                double moveY = frY;

                Y += moveY;
                if (moveY != 0 && isColliding(GetRect()))
                {
                    double step = moveY / 5;
                    while (isColliding(GetRect()))
                    {
                        Y -= step;
                    }
                    frY = 0;
                }

                Rectangle bot = GetRect();
                bot.Y += 2;
            }
        }

        public bool isColliding(Rectangle r)
        {
            foreach (Block lo in level.SolidObjects)
            {
                if (lo.IsSolid && lo != this && r.Intersects(lo.GetRect()) && level.player.Holding != lo)
                {
                    return true;
                }
            }
            return false;
        }
        void updateStepAndClick()
        {
            if (Clickable && Controls.INTERACT && level.player.hitbox().Intersects(GetRect()))
            {
                foreach (int i in Opens)
                {
                    level.ids[i].IsSolid = false;
                    level.ids[i].GotoAndStop(level.ids[i].Frames.Count - 1);

                }

                foreach (int i in Toggles)
                {
                    level.ids[i].IsSolid = !level.ids[i].IsSolid;

                    if (level.ids[i].IsSolid)
                    {
                        level.ids[i].GotoAndStop(0);
                    }
                    else
                    {
                        level.ids[i].GotoAndStop(level.ids[i].Frames.Count - 1);
                    }
                }
            }
            stepped = false;
            if (Stepable && level.player.hitbox().Intersects(GetRect()))
            {
                stepped = true;
            }
            else if (Stepable)
            {
                foreach (var b in level.IsPickable)
                {
                    if (b.GetRect().Intersects(GetRect()))
                    {
                        stepped = true;
                        break;
                    }
                }
            }
            if (stepped)
            {
                foreach (int i in Opens)
                {
                    level.ids[i].IsSolid = false;
                    level.ids[i].GotoAndStop(level.ids[i].Frames.Count - 1);
                }
                foreach (int i in Toggles)
                {
                    level.ids[i].IsSolid = false;
                    level.ids[i].GotoAndStop(level.ids[i].Frames.Count - 1);
                }
            }
            else
            {
                foreach (int i in Toggles)
                {
                    level.ids[i].IsSolid = true;
                    level.ids[i].GotoAndStop(0);
                }
            }


            /* foreach (int i in Toggles)
             {
                 level.ids[i].IsSolid = !level.ids[i].IsSolid;

                 if (level.ids[i].IsSolid)
                 {
                     level.ids[i].GotoAndStop(0);
                 }
                 else
                 {
                     level.ids[i].GotoAndStop(level.ids[i].Frames.Count - 1);
                 }

             }*/
        }

        public void AddTexture()
        {
            AddImg(Tilesets.Get[TileSet].tex, "");
            for (int i = 0; i < TileNumb.Count; i += 2)
            {
                AddFrame("", int.Parse(TileNumb[i + 1].ToString()), Tilesets.Get[TileSet].GetRect(int.Parse(TileNumb[i].ToString())));
            }
            UpdateAnimation();

        }

        JToken J;

        public void Configure(JToken j)
        {
            J = j;
            GridX = int.Parse(j.SelectToken("x").ToString());
            GridY = int.Parse(j.SelectToken("y").ToString());

            TileSet = j.SelectToken("TileSet").ToString();
            //
          
            List<JToken> fr = j.SelectToken("TileNumb").ToList();
            for (int i = 0; i < fr.Count; i += 2)
            {
                TileNumb.Add(int.Parse(fr[i].ToString()));
                TileNumb.Add(int.Parse(fr[i + 1].ToString()));

                //AddFrame("", int.Parse(fr[i + 1].ToString()), Tilesets.Get[TileSet].GetRect(int.Parse(fr[i].ToString())));
            }

            //UpdateAnimation();
            AddTexture();

            IsSolid = parse("IsSolid");
            Anti = parse("Anti");
            Kills = parse("Kills");
            Pickable = parse("Pickable");
            Openable = parse("Openable");
            IsKey = parse("IsKey");
            Falling = parse("Falling");
            Stepable = parse("Stepable");
            Clickable = parse("Clickable");
            if (parse("Stop"))
            {
                Stop();
            }

            var idGet = j.SelectToken("id");
            if (idGet != null)
            {
                id = int.Parse(idGet.ToString());
            }

            var link = j.SelectToken("Links");
            if (link != null)
            {
                List<JToken> objs = j.SelectToken("Links").ToList();
                foreach (JToken b in objs)
                {
                    Links.Add(int.Parse(b.ToString()));
                }
            }

            var move = j.SelectToken("Moves");
            if (move != null)
            {
                List<JToken> objs = j.SelectToken("Moves").ToList();
                foreach (JToken b in objs)
                {
                    foreach (JToken c in b.ToList())
                    {
                        Moves.Add(double.Parse(c.ToString()));
                    }
                }
            }

            if (Clickable || Stepable)
            {
                var a = j.SelectToken("Opens");
                if (a != null)
                {
                    List<JToken> objs = j.SelectToken("Opens").ToList();
                    foreach (JToken b in objs)
                    {
                        Opens.Add(int.Parse(b.ToString()));
                    }
                }
                var c = j.SelectToken("Toggles");
                if (c != null)
                {
                    List<JToken> objs = j.SelectToken("Toggles").ToList();
                    foreach (JToken b in objs)
                    {
                        Toggles.Add(int.Parse(b.ToString()));
                    }
                }
            }

            // SetXY(int.Parse(j.SelectToken("x").ToString()), int.Parse(j.SelectToken("y").ToString()));
        }

        public bool parse(string s)
        {
            var a = J.SelectToken(s);
            if (a != null)
            {
                return bool.Parse(a.ToString());
            }
            return false;
        }


    }
}
