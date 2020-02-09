using MonoCake.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CognitiveDissonance 
{
   public class DualLevel : BasicObject
    {
        Level a;
        Level b;
      
        public DualLevel()
        {
            
        }
        public int level = 1;

        public void init(int level)
        {
            this.level = level;

            a = new Level();
            b = new Level();

            a.LoadJSON(level, "PartA");
            b.LoadJSON(level, "PartB");

            a.levelGroop = this;
            b.levelGroop = this;

            a.Init();
            b.Init();

            a.AddUR(this);
            b.AddUR(this);

            b.BaseRenderParameters.Alpha = 0;
        }

        public void restartLevel()
        {
            a.Destruct();
            b.Destruct();
            timer = 0;
            showLevel = false;
            init(level);
        }

        bool showLevel = false;

        int timer = 0;
        public void showLevelB()
        {
            timer = 60*3;

            a.RemoveUpdate();
            b.RemoveUpdate();

            showLevel = true;
        }
        public void hideLevelB()
        {
            b.RemoveRender();
        }

        public void FinishLevel()
        {
            Gameplay.self.NextLevel();
        }

        public override void Update()
        {
         

            if (Gameplay.difficulty == Gameplay.Difficulty.easy && timer == 0)
            {
                showLevel = Controls.SHOW;
            }
       
            if (showLevel)
            {
                b.BaseRenderParameters.Alpha += (1.0 / 25.0);
            }
            else
            {
                b.BaseRenderParameters.Alpha -= (1.0 / 25.0);
            }
 
            b.BaseRenderParameters.Alpha =Math.Max(0, Math.Min(b.BaseRenderParameters.Alpha, 1));

            a.BaseRenderParameters.Alpha = 1 - b.BaseRenderParameters.Alpha;

            if (Controls.RESET)
            {
                restartLevel();
            }

            if (timer > 0)
            {
                b.BaseRenderParameters.Alpha += 0.01;
                timer--;
                if (timer == 0)
                {
                    restartLevel();
                }
            }

            if (a.victory && b.victory)
            {
                FinishLevel();
                RemoveUpdate();
            }

        }
        public override void Destruct()
        {
            base.Destruct();
            a.Destruct();
            b.Destruct();
        }

    }
}
