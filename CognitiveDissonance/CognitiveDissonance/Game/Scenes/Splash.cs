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
    public class Splash : Scene
    {
        
        List<TextField> infos = new List<TextField>();
        public override void Init()
        {
            AddUR();
            var info = new TextField();
            infos.Add(info);
            info.color = Color.White;
            info.text =
                "\nHello! Welcome to the game \"Cognitive Dissonance\"" +
   "\nFor the Full Moon Game Jam. Theme: DUALITY" +
   "\nBy CakeEaterGames(code and level designs)" +
   "\nAnd Ehidney(Art, Assets and level designs)" +
   "\nOur sound designer gave up on us so the game doesn't have sound... sorry about that" +
   "\n" +
   "\nWe didn't finish everything that we wanted" +
   "\nNo sound hints" +
   "\nNo menu and ending" +
   "\nNo story line(that is actually pretty interesting in my opinion!hehe)" +
   "\n" +
   "\nA game amount the MC talking to himself and arguing with himself as he plays levels" +
   "\nthat symbolize his mind mazes(mind places)." +
   "\nA game about understanding yourself." +
   "\n" +
   "\nKnown bugs:" +
   "\nDon't put objects inside walls, or they will fly into stratosphere" +
   "\nYou can clip yourself by putting a box on the ground." +
   "\n" +
   "\nPress Enter to continue  ";

            var info2 = new TextField();
            infos.Add(info2);
            info2.text = "\nSelect difficulty:"+"\n" +"\nPress 1 for Confusion(easy)  You can see the other world any time be pressing TAB" +"\nPress 2 for Dissonance(normal, INTENDED)  You can see the other world when you lose" +"\nPress 3 for Schizophrenia(hard)  You can’t see the other world at all(not recommended for new players) ";

            var info3 = new TextField();
            infos.Add(info3);
            info3.text = "\nControls " +"\nWSAD  Arrows" +"\nLeft  Right  walk" +"\nSpace  jump" +"\nUp  pick up  put down the item" +"\ndown  crawl  uncrawl" +"\n" +"\nPress Enter to continue";


            infos[0].AddUR(this);
        }

        int step = 0;
        public override void Update()
        {
            if (step ==0 && KEY.IsTyped(Keys.Enter))
            {
                step++;
                infos[0].Destruct();
                infos[1].AddUR(this);
            }

          /*  if (step == 1 && KEY.IsTyped(Keys.D1))
            {
                Gameplay.difficulty = Gameplay.Difficulty.easy;
                infos[1].Destruct();
                infos[2].AddUR(this);
                step++;
            }

            if (step == 1 && KEY.IsTyped(Keys.D2))
            {
                Gameplay.difficulty = Gameplay.Difficulty.normal;
                infos[1].Destruct();
                infos[2].AddUR(this);
                step++;
            }

            if (step == 1 && KEY.IsTyped(Keys.D3))
            {
                Gameplay.difficulty = Gameplay.Difficulty.hard;
                infos[1].Destruct();
                infos[2].AddUR(this);
                step++;
            }
            if (step == 2 && KEY.IsTyped(Keys.Enter))
            {
                infos[2].Destruct();
                infos[3].AddUR(this);
                step++;
            }
            if (step == 2 && KEY.IsTyped(Keys.Enter))
            {
                infos[3].Destruct();
                this.Destruct();
                Main.sceneManager.SetScene(new Gameplay());
                step++;
            }*/
        }

    }
}
