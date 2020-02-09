using Microsoft.Xna.Framework.Input;
using MonoCake;

namespace CognitiveDissonance
{
    public static class Controls
    {
        public static bool PREJUMP => KEY.IsTyped(Keys.Space);
        public static bool JUMP => KEY.IsDown(Keys.Space);

        public static bool SHOW => KEY.IsDown(Keys.Tab);

        public static bool LEFT => KEY.IsDown(Keys.Left) || KEY.IsDown(Keys.A);
        public static bool RIGHT => KEY.IsDown(Keys.Right) || KEY.IsDown(Keys.D);
        public static bool DOWN => KEY.IsTyped(Keys.Down) || KEY.IsTyped(Keys.S);
        public static bool INTERACT => KEY.IsTyped(Keys.Up) || KEY.IsTyped(Keys.W);

        public static bool PAUSE => KEY.IsTyped(Keys.Escape);
        public static bool RESET => KEY.IsTyped(Keys.R);

        public static bool CONFIRM => KEY.IsTyped(Keys.Enter);

    }
}

/*

using Microsoft.Xna.Framework.Input;
using MonoCake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenReboot 
{
    public static class Controlls
    {
        public static Dictionary<ControllNames, KeyDescription> Configuration;

        static Controlls()
        {
 
        Configuration.Add(ControllNames.Jump, new KeyDescription()         { Key =  { Keys.Space, Keys.Space },  Desc = "Jump" });
            Configuration.Add(ControllNames.Left, new KeyDescription()     { Key = Keys.A,      Desc = "Move Left" });
            Configuration.Add(ControllNames.Right, new KeyDescription()    { Key = Keys.D,      Desc = "Move Right" });
            Configuration.Add(ControllNames.Up, new KeyDescription()       { Key = Keys.W,      Desc = "Move Up" });
            Configuration.Add(ControllNames.Down, new KeyDescription()     { Key = Keys.S,      Desc = "Move Down" });
            Configuration.Add(ControllNames.Pause, new KeyDescription()    { Key = Keys.Escape, Desc = "Pause" });
            Configuration.Add(ControllNames.Interact, new KeyDescription() { Key = Keys.E,      Desc = "Interact" });
        }


        public enum ControllNames{
            Jump,
            Left,
            Right,
            Up,
            Down,
            Interact,
            Pause,
        }

        public static bool PREJUMP => KEY.IsTyped(Keys.Space);
        public static bool JUMP => KEY.IsDown(Keys.Space);

        public static bool LEFT => KEY.IsDown(Keys.A);
        public static bool RIGHT => KEY.IsDown(Keys.D);
        public static bool UP => KEY.IsDown(Keys.W);
        public static bool DOWN => KEY.IsDown(Keys.S);

        public static bool INTERACT => KEY.IsDown(Keys.E) || KEY.IsDown(Keys.Enter);
        public static bool PAUSE => KEY.IsTyped(Keys.Escape);


        public struct KeyDescription
        {
            public Keys[] Key;
            public String Desc;
        }
    }
}


*/
