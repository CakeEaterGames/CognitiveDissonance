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

        public Player()
        {
            AddImg(GlobalContent.LoadImg("player", true), "");
 

            //AddFrame("p", 120);
            AddFrame("",22);

            //SetCenter(0.5, 0.4);
        }
        public bool IsCrawling = false;
        public Rectangle NormalHitbox()
        {

            return new Rectangle(
               (int)(X - Ox * ScaleW),
               (int)(Y - Oy * ScaleH),
               (int)(64 * ScaleW),
               (int)(64 * ScaleH)
               );
        }
        public Rectangle CrawlHitbox()
        {
            return new Rectangle(
                    (int)(X - Ox * ScaleW),
                    (int)(Y - Oy * ScaleH + 32),
                    (int)(64 * ScaleW),
                    (int)(32 * ScaleH)
                    );
        }

        public Rectangle hitbox()
        {
            if(IsCrawling)
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
            jump
        }

        public List<Block> Blocks;

        public anim currentAnim = anim.idle;
        public void setAnim(anim a)
        {
            switch (a)
            {
                case anim.idle:
                    currentAnim = anim.idle;
                    GotoAndStop(0);
                    break;
                case anim.run:
                    if (currentAnim != anim.run)
                    {
                        GotoAndPlay(1);
                    }
                    currentAnim = anim.run;
                    break;
                case anim.jump:
                    currentAnim = anim.jump;
                    GotoAndStop(21);
                    break;
            }
        }

        public double speedX = 0.5;
        public double speedY = 6;

        public double frX = 0;
        public double frY = 0;

        public double maxFrX = 5;
        public double maxFrY = 8;
        public double minFrY = -8;

        public double frXFade = 0.3;
        public double frYFade = 1;

        public bool releasedJump = true;
        public bool tg = true;
        public int holdingJumpFor = 0;
        public int maxJumpFor = 13;

        public bool canClimb = false;

        public override void Update()
        {
            //SetCenter(0.5, 0.4);
            carried = false;
            physics();
            Pick();
            checkDepth();
        }
        void checkDepth()
        {
            if (Y>720*3)
            {
                level.Loss();
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
                    Holding.SetXY(this.X - 32, this.Y+16);
                }
                else
                {
                    Holding.SetXY(this.X + hitbox().Width, this.Y + 16);
                   
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

            if (Controls.LEFT)
            {
                frX -= speedX;
                Orientation = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;
            }
            if (Controls.RIGHT)
            {
                frX += speedX;
                Orientation = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
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

            if (!tg)
            {
                setAnim(anim.jump);
            }
            else if (Math.Abs(frX) < 1)
            {
                setAnim(anim.idle);
            }
            else
            {
                setAnim(anim.run);
            }
            if (CurrentFrame == 20)
            {
                GotoAndPlay(0);
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
                Console.WriteLine("j");
                frY = -speedY;
                holdingJumpFor++;
                tg = false;
            }
            if (!Controls.JUMP && !releasedJump)
            {
                Console.WriteLine("r");
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
