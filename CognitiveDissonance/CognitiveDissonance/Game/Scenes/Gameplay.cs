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
        public Gameplay()
        {
          
        }
        public override void Init()
        {
            AddUR();

            Level a = new Level();


            a.AddUR(this);
        }
        public override void Update()
        {
            
        }
    }
}
