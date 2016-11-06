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
        public static bool enemyTurn = false;
        static int productPower = -50;
        static List<UnitType> Products;
        static readonly int[] need = {
            15, 15, 25, 1000, 50
        };
        Map nMap;
        UnitManager um;
        PlayScene ps;
        int tmpUnit;
        static int Gan_N = 0;

        bool moved = false;

        RandomXS rand;

        int pause = 0;

        // マップのすべての座標の点数
        int[,] scoreMap;

        // 六方向の方向ベクトル
        int[] si = { 1, 1, 0, -1, -1, 0 };
        int[] sj = { 0, 1, 1, 0, -1, -1 };

        public AI(SceneManager s, ref Map _nMap, ref UnitManager _um, PlayScene _ps)
            :base(s)
        {
            enemyTurn = true;
            turnNum++;
            productPower += 25;
            nMap = _nMap;
            um = _um;
            ps = _ps;
            tmpUnit = 0;

            Products = new List<UnitType>();

            rand = new RandomXS();

            InitScore();

            // ユニットの増殖
            int num = um.enemyUnits.Count;
            for (int i = 0;i < num;i++)
            {
                UnitType tp = um.uMap.data[um.enemyUnits[i].i, um.enemyUnits[i].j].type;
                if (tp == UnitType.Kin)
                {
                    if(rand.NextInt(100) < 5)
                    {
                        um.Select(um.enemyUnits[i].i - (um.enemyUnits[i].j + 1) / 2, um.enemyUnits[i].j);
                        um.StartDoubling();
                        um.Double(um.range[rand.NextInt(um.range.Count)]);
                    }
                }
                else if (tp == UnitType.Kabi)
                {
                    if (rand.NextInt(100) < 5)
                    {
                        um.Select(um.enemyUnits[i].i - (um.enemyUnits[i].j + 1) / 2, um.enemyUnits[i].j);
                        um.StartDoubling();
                        um.Double(um.range[rand.NextInt(um.range.Count)]);
                    }
                }
                else if (tp == UnitType.Virus)
                {
                    if (um.uMap.data[um.enemyUnits[i].i, um.enemyUnits[i].j].virusState == 1 && rand.NextInt(100) < 50)
                    {
                        um.Select(um.enemyUnits[i].i - (um.enemyUnits[i].j + 1) / 2, um.enemyUnits[i].j);
                        um.StartDoubling();
                        um.Double(um.range[rand.NextInt(um.range.Count)]);
                    }
                }
                else if (tp == UnitType.Gan)
                {
                    if (rand.NextInt(100) < Gan_N * 20)
                    {
                        um.Select(um.enemyUnits[i].i - (um.enemyUnits[i].j + 1) / 2, um.enemyUnits[i].j);
                        um.StartDoubling();
                        um.Double(um.range[rand.NextInt(um.range.Count)]);
                    }
                }
            }
            // ユニットの生産
            if (productPower >= 50)
            {
                while (true)
                {
                    int rr = rand.NextInt(100);
                    UnitType var = rr < 10 ? UnitType.Kiseichu : rr < 30 ? UnitType.Virus : rr < 60 ? UnitType.Kabi : UnitType.Kin;
                    if(productPower < need[(int)var + 5])
                    {
                        break;
                    }
                    productPower -= need[(int)var + 5];
                    Products.Add(var);
                }
            }
            // ユニットの配置
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
            if(Products.Count != 0)
            {
                int cnt = 3;
                while (lp.Count != 0 && cnt-- != 0 && Products.Count != 0)
                {
                    int i = rand.NextInt(lp.Count);
                    um.enemyUnits.Add(lp[i]);
                    um.uMap.data[lp[i].i, lp[i].j] = new Unit(Products.Last());
                    lp.RemoveAt(i);
                    Products.RemoveAt(Products.Count - 1);
                }
            }
            // ガン細胞のランダム発生
            if (rand.NextInt(100) == 28)
            {
                bool flag = true;
                int cnt = 0;
                do
                {
                    int x = rand.NextInt(DataBase.MAP_MAX), y = rand.NextInt(DataBase.MAP_MAX);
                    if(nMap.Data[x, y] != 0 && um.uMap.data[x + (y + 1) / 2, y].type == UnitType.NULL)
                    {
                        flag = false;
                        um.uMap.data[x + (y + 1) / 2, y] = new Unit(UnitType.Gan);
                        um.enemyUnits.Add(new PAIR(x + (y + 1) / 2, y));
                        Gan_N++;
                    }
                } while (flag && cnt++ != 3);
            }
        }
        public override void SceneDraw(Drawing d)
        {
            // マップの描画
            nMap.Draw(d, ps.Camera, ps.Scale);
            // ユニットの描画
            um.Draw(d, ps.Camera, ps.Scale);

            base.SceneDraw(d);
        }
        // ユニットの移動
        public override void SceneUpdate()
        {
            pause++;
            if (pause >= 30 && moved && !um.moveAnimation)
            {
                um.StartAttacking();
                int tg = -1, mins = 1000;
                for (int i = 0; i < um.range.Count; i++)
                {
                    int rs = um.RealStrength(um.range[i].i, um.range[i].j);
                    if (rs < mins)
                    {
                        tg = i;
                        mins = rs;
                    }
                }
                if (tg != -1)
                {
                    PAIR p = um.range[tg];
                    um.Attack(p.i - (p.j + 1) / 2, p.j, false);
                }
                else
                {
                    um.CancelAttacking();
                }
                // ウイルスの定着
                if(um.uMap.data[um.enemyUnits[tmpUnit].i ,um.enemyUnits[tmpUnit].j].type == UnitType.Virus && rand.NextInt(5) == 0)
                {
                    um.uMap.data[um.enemyUnits[tmpUnit].i, um.enemyUnits[tmpUnit].j].Fix();
                }
                moved = false;
                tmpUnit++;
                pause = 0;
            }

            if (pause >= 30 && tmpUnit == um.enemyUnits.Count)
            {
                if (PlayScene.BodyTemp > 42m)
                {
                    new GameOverScene(scenem);
                    ps.Delete = true;
                }
                else
                {
                    bool flag = true;
                    for (int i = 0; i < DataBase.MAP_MAX; i++)
                    {
                        for (int j = 0; j < DataBase.MAP_MAX; j++)
                        {
                            if (nMap.Data[i, j] == 4 && um.GetType(i, j) >= 0)
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (!flag)
                            break;
                    }
                    if (flag)
                    {
                        new GameOverScene(scenem);
                        ps.Delete = true;
                    }
                }
                enemyTurn = false;
                um.NextUnit();
                Delete = true;
                return;
            }
            else if(pause >= 30 && !um.moveAnimation)
            {
                PAIR eu = um.enemyUnits[tmpUnit];
                um.Select(eu.i - (eu.j + 1) / 2, eu.j);
                um.StartMoving();
                if (rand.NextInt(100) < 10 && um.range.Count != 0)
                {
                    eu = um.range[rand.NextInt(um.range.Count)];
                }
                else
                {
                    int score = scoreMap[eu.i, eu.j];
                    for (int j = 0; j < um.range.Count; j++)
                    {
                        PAIR p = um.range[j];
                        if (scoreMap[p.i, p.j] >= score)
                        {
                            score = scoreMap[p.i, p.j];
                            eu = p;
                        }
                    }
                }
                if (eu != um.enemyUnits[tmpUnit])
                {
                    um.Move(eu.i - (eu.j + 1) / 2, eu.j);
                }
                else
                {
                    um.CancelMoving();
                }
                moved = true;
                pause = 0;
            }
            ps.UpdateByAI();
        }
        // スコアマップを初期化
        void InitScore()
        {
            scoreMap = new int[DataBase.MAP_MAX + (DataBase.MAP_MAX + 1) / 2 ,DataBase.MAP_MAX];
            for (int i = 0; i < DataBase.MAP_MAX; i++)
            {
                for (int j = 0; j < DataBase.MAP_MAX; j++)
                {
                    scoreMap[i + (j + 1) / 2, j] = nMap.Data[i, j] == 4 ? 2 : nMap.Data[i, j] == 3 ? -1 : nMap.Data[i, j] == 2 ? 1 : 0;
                }
            }
            for (int ii = 0; ii < um.myUnits.Count; ii++)
            {
                int pi = um.myUnits[ii].i, pj = um.myUnits[ii].j;
                for (int i = 0;i < 6;i++)
                {
                    int ni = pi + si[i], nj = pj + sj[i];
                    if (ni - (nj + 1) / 2 >= 0 && ni - (nj + 1) / 2 < DataBase.MAP_MAX && nj >= 0 && nj < DataBase.MAP_MAX)
                    {
                        scoreMap[ni, nj] += 10;
                    }
                }
            }
            for (int ii = 0; ii < um.myUnits.Count; ii++)
            {
                if (um.uMap.data[um.myUnits[ii].i, um.myUnits[ii].j].type == UnitType.HelperT) continue;
                int[,] dijkMap = new int[DataBase.MAP_MAX + (DataBase.MAP_MAX + 1) / 2, DataBase.MAP_MAX];
                for (int i = 0; i < DataBase.MAP_MAX + (DataBase.MAP_MAX + 1) / 2; i++)
                {
                    for (int j = 0; j < DataBase.MAP_MAX; j++)
                    {
                        dijkMap[i, j] = -1;
                    }
                }
                SortedSet<UnitManager.DijkstraNode> pq = new SortedSet<UnitManager.DijkstraNode>(new UnitManager.CompareDijkstraNode());
                pq.Add(new UnitManager.DijkstraNode(0, um.myUnits[ii]));
                while (pq.Count != 0)
                {
                    int pi = pq.First().pos.i, pj = pq.First().pos.j, pc = pq.First().cost;
                    pq.Remove(pq.First());
                    if (dijkMap[pi, pj] > 0) continue;
                    scoreMap[pi, pj] -= 1;
                    if (pc > 5) continue;
                    dijkMap[pi, pj] = pc;
                    for (int i = 0; i < 6; i++)
                    {
                        int ni = pi + si[i], nj = pj + sj[i];
                        if (ni - (nj + 1) / 2 >= 0 && ni - (nj + 1) / 2 < DataBase.MAP_MAX && nj >= 0 && nj < DataBase.MAP_MAX)
                        {
                            int nc = pc + ((PlayScene.nMap.Data[pi - (pj + 1) / 2, pj] == 2 && PlayScene.nMap.Data[ni - (nj + 1) / 2, nj] == 2) ? 1 : 5);
                            if (PlayScene.nMap.Data[ni - (nj + 1) / 2, nj] != 0 && um.uMap.data[ni, nj].type == UnitType.NULL)
                            {
                                pq.Add(new UnitManager.DijkstraNode(nc, new PAIR(ni, nj)));
                            }
                        }
                    }
                }
            }
        }
    }
}
