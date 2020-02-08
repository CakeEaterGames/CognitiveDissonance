using System;
using MonoCake;
using MonoCake.Objects;
namespace CognitiveDissonance
{
    public class Main : BasicObject
    {
        public static SceneManager sceneManager;
        public Main()
        {
            var a = new Tileset(GlobalContent.LoadImg("main",true),32,32);
           
            Tilesets.Get.Add("main",a);

            sceneManager = new SceneManager();
             sceneManager.SetScene(new Gameplay());
            //sceneManager.SetScene(new LevelEditor());
            sceneManager.AddUR(this);
        }
        public override void Update()
        {

        }

    }
}
