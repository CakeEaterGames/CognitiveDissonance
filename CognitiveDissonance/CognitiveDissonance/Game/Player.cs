using Microsoft.Xna.Framework;
using MonoCake;
using MonoCake.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CognitiveDissonance
{

    public class Player : GameObject
    {
        public Level level;
        public Block Holding;

        public bool carried = false;

        int sw = 64;

        public List<int> FrameLoops = new List<int>();

        public Player()
        {

            int x = 0, y = 0;
            AddImg(GlobalContent.LoadImg("player", true), "");

            //idle

            FrameLoops.Add(Frames.Count);
            AddFrame("", new Rectangle(x * sw, y * sw, sw, sw), 25);
            x++;
            AddFrame("", new Rectangle(x * sw, y * sw, sw, sw), 40);
            x++;
            AddFrame("", new Rectangle(x * sw, y * sw, sw, sw), 25);
            x++;
            AddFrame("", new Rectangle(x * sw, y * sw, sw, sw), 40);
            x++;


            //run
            FrameLoops.Add(Frames.Count);
            y = 1;
            x = 0;
            AddFrame("", new Rectangle(x * sw, y * sw, sw, sw), 10);
            x++;
            AddFrame("", new Rectangle(x * sw, y * sw, sw, sw), 10);
            x++;
            AddFrame("", new Rectangle(x * sw, y * sw, sw, sw), 10);
            x++;
            AddFrame("", new Rectangle(x * sw, y * sw, sw, sw), 10);
            x++;

            //crawl
            FrameLoops.Add(Frames.Count);
            y = 2;
            x = 0;
            AddFrame("", new Rectangle(x * sw, y * sw, sw, sw), 10);
            x++;
            AddFrame("", new Rectangle(x * sw, y * sw, sw, sw), 10);
            x++;
            AddFrame("", new Rectangle(x * sw, y * sw, sw, sw), 10);
            x++;
            AddFrame("", new Rectangle(x * sw, y * sw, sw, sw), 10);
            x++;

            //Jump
            FrameLoops.Add(Frames.Count);
            y = 3;
            x = 0;
          //  AddFrame("", new Rectangle(x * sw, y * sw, sw, sw), 10);
            x++;
            AddFrame("", new Rectangle(x * sw, y * sw, sw, sw), 7);
            x++;
          //  AddFrame("", new Rectangle(x * sw, y * sw, sw, sw), 10);
            x++;

            //fall
            FrameLoops.Add(Frames.Count);
            AddFrame("", new Rectangle(x * sw, y * sw, sw, sw), 40);
            x++;
            FrameLoops.Add(Frames.Count);
            //AddFrame("p", 120);
            // AddFrame("",22);

            //SetCenter(0.5, 0.4);


        }
        public bool IsCrawling = false;
        public Rectangle NormalHitbox()
        {
            return new Rectangle(
                    (int)(22 + X - Ox * ScaleW),
                    (int)(14 + Y - Oy * ScaleH),
                    (int)(20 * ScaleW),
                    (int)(50 * ScaleH)
                    );
            /*  return new Rectangle(
                 (int)(X - Ox * ScaleW),
                 (int)(Y - Oy * ScaleH),
                 (int)(64 * ScaleW),
                 (int)(64 * ScaleH)
                 );*/
        }
        public Rectangle CrawlHitbox()
        {

            return new Rectangle(
                (int)(22 + X - Ox * ScaleW),
                (int)(14 + Y - Oy * ScaleH + 25),
                (int)(20 * ScaleW),
                (int)(25 * ScaleH)
         );

        }
        /*return new Rectangle(
                  (int)(22+X - Ox * ScaleW),
                  (int)(14+Y - Oy * ScaleH + 32),
                  (int)(20 * ScaleW),
                  (int)(50 * ScaleH)
                  );*/
        public Rectangle hitbox()
        {
            if (IsCrawling)
            {
                return CrawlHitbox();
            }
            else
            {
                return NormalHitbox();
            }
        }

        public enum anim
        {
            run,
            idle,
            jump,
            crawl,
            fall
        }

        public List<Block> Blocks;

        public anim currentAnim = anim.idle;
        public void setAnim(anim a)
        {
            if (currentAnim != a)
            {
                currentAnim = a;

                switch (a)
                {
                    case anim.idle:
                        GotoAndPlay(FrameLoops[0]);
                        break;
                    case anim.run:
                        GotoAndPlay(FrameLoops[1]);
                        break;
                    case anim.crawl:
                        GotoAndPlay(FrameLoops[2]);
                        break;
                    case anim.jump:
                        GotoAndPlay(FrameLoops[3]);
                        break;
                    case anim.fall:
                        GotoAndPlay(FrameLoops[4]);
                        break;

                }

            }
        }

        public double speedX = 0.5;
        public double speedY = 6;

        public double frX = 0;
        public double frY = 0;

        public double maxFrX = 4;
        public double maxFrY = 8;
        public double minFrY = -8;

        public double frXFade = 0.4;
        public double frYFade = 1;

        public bool releasedJump = true;
        public bool tg = true;
        public int holdingJumpFor = 0;
        public int maxJumpFor = 15;

        public bool canClimb = false;

        public override void Update()
        {

            //SetCenter(0.5, 0.4);
            UpdateAnimFrames();

            carried = false;
            physics();
            Pick();
            checkDepth();
        }
        void checkDepth()
        {
            if (Y > 720 * 3)
            {
                level.Loss();
            }
        }

        public void UpdateAnimFrames()
        {
            switch (currentAnim)
            {
                case anim.idle:
                    if (CurrentFrame >= FrameLoops[1])
                    {
                        GotoAndPlay(FrameLoops[0]);
                    }
                    break;
                case anim.run:
                    if (CurrentFrame >= FrameLoops[2])
                    {
                        GotoAndPlay(FrameLoops[1]);
                    }
                    break;
                case anim.crawl:
                    if (Math.Abs(frX) > 0)
                    {
                        Play();
                    }
                    else
                    {
                        Stop();
                    }
                    if (CurrentFrame >= FrameLoops[3])
                    {
                        GotoAndPlay(FrameLoops[2]);
                    }
                    break;
                case anim.jump:
                    if (CurrentFrame >= FrameLoops[4])
                    {
                        GotoAndPlay(FrameLoops[4]-1);
                    }
                    break;
                case anim.fall:
                    if (CurrentFrame >= FrameLoops[5])
                    {
                        GotoAndPlay(FrameLoops[4]);
                    }
                    break;
            }

        }

        public void Pick()
        {
            if (Holding == null)
            {
                if (Controls.INTERACT)
                {
                    foreach (var a in level.IsPickable)
                    {
                        if (a.IsSolid)
                        {
                            var r = a.GetRect();
                            r.X -= 3;
                            r.Y -= 3;
                            r.Width += 6;
                            r.Height += 6;
                            if (hitbox().Intersects(r) && Holding != a)
                            {
                                Holding = a;
                                a.RemoveRender();
                                a.AddUR(this);
                                break;
                            }
                        }
                        else if (hitbox().Intersects(a.GetRect()) && Holding != a)
                        {
                            Holding = a;
                            a.RemoveRender();
                            a.AddUR(this);
                            break;
                        }
                    }
                }
            }
            else
            {
                if (Orientation == Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally)
                {
                    Holding.SetXY(this.X, this.Y + 16);
                    Holding.Orientation = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
                }
                else
                {
                    Holding.SetXY(this.X + 32, this.Y + 16);
                    Holding.Orientation = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;
                }
                if (Controls.INTERACT)
                {
                    Holding.RemoveRender();
                    Holding.AddUR(level);
                    Holding = null;
                }
            }
        }

        void physics()
        {
            if (Controls.DOWN)
            {
                if (IsCrawling)
                {
                    if (!isColliding(NormalHitbox()))
                    {
                        IsCrawling = false;
                    }
                }
                else
                {
                    IsCrawling = true;
                }
            }

            if (frX > 0)
            {
                frX -= frXFade;
                frX = Math.Min(frX, maxFrX);
            }
            else if (frX < 0)
            {
                frX += frXFade;
                frX = Math.Max(frX, -maxFrX);
            }

            if (Controls.LEFT)
            {
                if (IsCrawling)
                {
                    frX -= speedX/ 1.5;
                }
                else
                {
                    frX -= speedX;
                }
                Orientation = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;
            }
            if (Controls.RIGHT)
            {
                if (IsCrawling)
                {
                    frX += speedX / 1.5;
                }
                else
                {
                    frX += speedX;
                }
                 
                Orientation = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
            }

        

 
            if (IsCrawling)
            {
                setAnim(anim.crawl);
            }
            else
            if ( (frY) < -0.1)
            {
                setAnim(anim.jump);
            }
            else if ((frY) > 1)
            {
                setAnim(anim.fall);
            }
            else if (Math.Abs(frX) > 1)
            {
                 setAnim(anim.run);
            }
            else if(tg)
            {
               setAnim(anim.idle);
            }




            if (!Controls.LEFT && !Controls.RIGHT && (frX >= -frXFade && frX <= frXFade))
            {
                frX = 0;
            }
            frY += frYFade;

            /* if (Controls.CLIMB && canClimb)
             {
                 frY = -4;
             }
             if (Controls.GRAB && canClimb)
             {
                 frY = 0;
             }*/

            if (Controls.JUMP && holdingJumpFor < maxJumpFor && !releasedJump)
            {

                frY = -speedY;
                holdingJumpFor++;
                tg = false;
            }
            if (!Controls.JUMP && !releasedJump)
            {

                holdingJumpFor = maxJumpFor + 1;
                releasedJump = true;
            }

            if (tg && Controls.JUMP && releasedJump)
            {
                releasedJump = false;
                holdingJumpFor = 0;
            }

            if (frY < minFrY)
            {
                frY = minFrY;
            }


            double moveX = frX;
            double moveY = frY;

            X += moveX;
            if (moveX != 0 && isColliding(hitbox()))
            {
                double step = moveX / 5;
                while (isColliding(hitbox()))
                {
                    X -= step;
                }
                frX = 0;
            }

            Y += moveY;
            if (moveY != 0 && isColliding(hitbox()))
            {
                double step = moveY / 5;
                while (isColliding(hitbox()))
                {
                    Y -= step;
                }
                frY = 0;
                if (moveY < 0) holdingJumpFor = maxJumpFor + 1;

            }

            Rectangle bot = hitbox();
            bot.Y += 2;
            if (isColliding(bot))
            {
                tg = true;
            }

        }


        public bool isColliding(Rectangle r)
        {
            foreach (Block lo in Blocks)
            {
                if (lo.IsSolid && r.Intersects(lo.GetRect()) && Holding != lo)
                {
                    return true;
                }
            }
            return false;
        }
    }

}
