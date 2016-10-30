using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CommonPart
{
    class GameClearScene : Scene
    {
        public GameClearScene(SceneManager s)
            : base(s) { }

        public override void SceneDraw(Drawing d)
        {
            new TextAndFont("ゲームクリアー！！", Color.Black).Draw(d, new Vector(600, 470), DepthID.BackGroundFloor);
            new TextAndFont("Zキーで終了").Draw(d, new Vector(640, 870), DepthID.BackGroundFloor);
            base.SceneDraw(d);
        }
        public override void SceneUpdate()
        {
            if (Input.GetKeyPressed(KeyID.Select)) Delete = true;
            base.SceneUpdate();
        }
    }
}
