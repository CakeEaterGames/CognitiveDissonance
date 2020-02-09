using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoCake;
using MonoCake.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CognitiveDissonance 
{
    class EndScene : Scene
    {
        public override void Init()
        {
            AddUR();
            TextField info = new TextField();
            info.text = "\nThat's it! Thanks for playing :)"+
"\nThere's clearly more to explore in this game mechanic" +
"\nWe will most likely finish this game!" +
"\nIf you liked it, let me know!" +
"\nVisit CakeEaterGames.ru and contact us any way you want!";

            info.color = Color.White;
            info.AddUR(this);
            BaseRenderParameters.X += 20;
        }
    }
}
