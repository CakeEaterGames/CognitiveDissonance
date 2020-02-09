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

        public static Difficulty difficulty = Difficulty.easy;
        public enum Difficulty
        {
            easy,
            normal,
            hard
        }

        public Gameplay()
        {
            self = this;

        }

        public DualLevel lvl;
        public int currentLevel = 7;

        public override void Init()
        {
            AddUR();
            startLevel();
        }
        public void startLevel()
        {
            lvl = new DualLevel();
            lvl.init(currentLevel);
            lvl.AddUR(this);
        }

        public void NextLevel()
        {
            currentLevel++;
            lvl.Destruct();
            startLevel();
        }
    }
}
