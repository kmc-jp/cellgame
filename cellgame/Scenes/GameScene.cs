using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cellgame {
    /// <summary>
    /// ゲーム開始後の処理を書いたクラス
    /// </summary>
    class GameScene : Scene {
        public GameScene(SceneManager s)
            : base(s) {
            // TODO
        }

        public override void SceneUpdate() {
            base.SceneUpdate();
            // Zキーが押されると終了
            if (Input.GetKeyPressed(KeyID.Select)) Delete = true;
        }
    }
}
