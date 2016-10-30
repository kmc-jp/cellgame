using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonPart
{
    class AI : Scene
    {
        static int turnNum = 0;
        Map nMap;
        UnitManager um;
        PlayScene ps;
        int tmpUnit;

        RandomXS rand;

        public AI(SceneManager s, ref Map _nMap, ref UnitManager _um, PlayScene _ps)
            :base(s)
        {
            s.BackSceneNumber++;
            turnNum++;
            nMap = _nMap;
            um = _um;
            ps = _ps;
            tmpUnit = 0;

            rand = new RandomXS();

            if(turnNum % 5 == 0)
            {
                List<PAIR> lp = new List<PAIR>();
                for(int i = 0;i < DataBase.MAP_MAX; i++)
                {
                    for(int j = 0;j < DataBase.MAP_MAX; j++)
                    {
                        if(nMap.Data[i, j] == 3 && um.GetType(i, j) == UnitType.NULL)
                        {
                            lp.Add(new PAIR(i + (j + 1) / 2, j));
                        }
                    }
                }
                int cnt = 3;
                while (lp.Count != 0 && cnt-- != 0)
                {
                    int i = rand.NextInt(lp.Count);
                    um.enemyUnits.Add(lp[i]);
                    um.uMap.data[lp[i].i, lp[i].j] = new Unit(UnitType.Kin);
                    lp.RemoveAt(i);
                }
            }
        }
        public override void SceneDraw(Drawing d)
        {
            base.SceneDraw(d);
        }
        public override void SceneUpdate()
        {
            if (tmpUnit == um.enemyUnits.Count)
            {
                if (PlayScene.bodyTemp > 42m) new GameOverScene(scenem);
                else
                {
                    bool flag = true;
                    for(int i = 0;i < DataBase.MAP_MAX; i++)
                    {
                        for(int j= 0;j < DataBase.MAP_MAX; j++)
                        {
                            if(nMap.Data[i, j] == 4 && um.GetType(i, j) >= 0)
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (!flag)
                            break;
                    }
                    if (flag) new GameOverScene(scenem);
                }
                Delete = true;
            }
            ps.UpdateByAI();

            Delete = true;
        }
    }
}
