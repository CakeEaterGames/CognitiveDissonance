using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CognitiveDissonance
{
    public class Tileset
    {
        public Texture2D tex;
        public String name = "";

        public int TileW = 1;
        public int TileH = 1;

        public int maxX = 1;
        public int maxY = 1;




        public Tileset(Texture2D Tex, int w,int h)
        {
            tex = Tex;
            TileW = w;
            TileH = h;

            maxX = tex.Width / w;
            maxY = tex.Height / h;
        }
        public Rectangle GetRect(int n)
        {
            if (n<maxX*maxY)
            {
                return new Rectangle((n % maxX)* TileW, (n / maxX)* TileH, TileW, TileH);
            }
            else
            {
                throw new Exception("Out of range");
            }
        
        }
    }
}
