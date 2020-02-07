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
        public int id = 0;
        public int GridX = 0;
        public int GridY = 0;

        public string TileSet = "main";
        public int TileNumb = 0;

        public bool IsSolid = false;
        public bool Kills = false;
        public bool Pickable = false;
        public bool Openable = false;
        public bool IsKey = false;
        public bool Falling = false;
        public bool Stepable = false;
        public bool Clickable = false;

        public List<int> Opens = new List<int>();
        public List<int> Toggles = new List<int>();

        JToken J;

        public void Configure(JToken j)
        {
            J = j;
            GridX = int.Parse(j.SelectToken("x").ToString());
            GridY = int.Parse(j.SelectToken("y").ToString());

            TileSet = j.SelectToken("TileSet").ToString();
            //
            AddImg(Tilesets.Get[TileSet].tex, "");
            List<JToken> fr = j.SelectToken("TileNumb").ToList();
            for (int i=0;i<fr.Count;i+=2)
            {
                AddFrame("", int.Parse(fr[i+1].ToString()), Tilesets.Get[TileSet].GetRect(   int.Parse(fr[i].ToString())     ));
            }
           
            UpdateAnimation();

            IsSolid = parse("IsSolid");
            Kills = parse("Kills");
            Pickable = parse("Pickable");
            Openable = parse("Openable");
            IsKey = parse("IsKey");
            Falling = parse("Falling");
            Stepable = parse("Stepable");
            Clickable = parse("Clickable");
         
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
