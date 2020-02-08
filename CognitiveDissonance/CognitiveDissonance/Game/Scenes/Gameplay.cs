using MonoCake;
using MonoCake.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CognitiveDissonance
{
    public class Gameplay : Scene
    {
        public static Gameplay self;
        public Gameplay()
        {
            self = this;
        }
        Level a;
        Level b;
        public override void Init()
        {
            AddUR();
             a = new Level();
             b = new Level();

            a.LoadJSON(1, "PartA");
           b.LoadJSON(1, "PartB");

            a.Init();
            b.Init();
 
            a.AddUR(this);
            b.AddUpdate(this);
 

        }


        int timer = 0;
        public void showLevelB()
        {
            b.AddRender();
            timer = 120;


            a.RemoveUpdate();
            b.RemoveUpdate();
        }
        public void hideLevelB()
        {
            b.RemoveRender();

        }

        public override void Update()
        {
            if (timer>0)
            {
                timer--;
                if (timer == 0)
                {
                    hideLevelB();
                }
            }
            if (Controls.SHOW)
            {
                showLevelB();
            }
        }
    }
}
