using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonPart
{
    class UnitManager
    {
        #region Variable
        // 味方ユニット
        List<PAIR> myUnits;
        // 敵ユニット
        List<PAIR> enemyUnits;
        // 現在選択中のユニット
        int select_i = -1, select_j = 0;
        // 現在移動コマンドの入力中であるかどうか
        public bool moving = false;
        // 現在移動可能な位置のリスト
        public List<PAIR> movable = new List<PAIR>();
        public List<int> moveCost = new List<int>();

        Unit[,] unitMap;

        #endregion

        #region Method
        // コンストラクタ
        public UnitManager()
        {
            // 味方ユニット
            myUnits = new List<PAIR>();

            // 敵ユニット
            enemyUnits = new List<PAIR>();

            // ユニットのマップ情報の初期化
            unitMap = new Unit[DataBase.MAP_MAX + (DataBase.MAP_MAX + 1) / 2, DataBase.MAP_MAX];
            for(int i = 0; i < DataBase.MAP_MAX + (DataBase.MAP_MAX + 1) / 2; i++)
            {
                for(int j = 0; j < DataBase.MAP_MAX; j++)
                {
                    unitMap[i, j] = new Unit(UnitType.NULL);
                }
            }
        }


        // 描画
        public void Draw(Drawing d, Vector camera, int scale)
        {
            foreach (PAIR p in myUnits)
            {
                int x_index = p.i - (p.j + 1) / 2, y_index = p.j;
                Vector pos = DataBase.WhereDisp(x_index, y_index, camera, scale);
                if (DataBase.IsInDisp(pos, scale) && x_index >= 0 && x_index < DataBase.MAP_MAX && y_index >= 0 && y_index < DataBase.MAP_MAX)
                {
                    pos += new Vector(26 * DataBase.MapScale[scale], 36 * DataBase.MapScale[scale]);
                    if(unitMap[p.i, p.j].type > 0)
                    {
                        d.Draw(pos, DataBase.myUnit_tex[(int)unitMap[p.i, p.j].type - 1], DepthID.Player, (float)DataBase.MapScale[scale]);
                    }
                    else
                    {
                        d.Draw(pos, DataBase.enemyUnit_tex[(int)unitMap[p.i, p.j].type + 5], DepthID.Player, (float)DataBase.MapScale[scale]);
                    }
                }
            }
            foreach (PAIR p in enemyUnits)
            {
                int x_index = p.i - (p.j + 1) / 2, y_index = p.j;
                Vector pos = DataBase.WhereDisp(x_index, y_index, camera, scale);
                if (DataBase.IsInDisp(pos, scale) && x_index >= 0 && x_index < DataBase.MAP_MAX && y_index >= 0 && y_index < DataBase.MAP_MAX)
                {
                    pos += new Vector(26 * DataBase.MapScale[scale], 36 * DataBase.MapScale[scale]);
                    if (unitMap[p.i, p.j].type > 0)
                    {
                        d.Draw(pos, DataBase.myUnit_tex[(int)unitMap[p.i, p.j].type - 1], DepthID.Player, (float)DataBase.MapScale[scale]);
                    }
                    else
                    {
                        d.Draw(pos, DataBase.enemyUnit_tex[(int)unitMap[p.i, p.j].type + 5], DepthID.Player, (float)DataBase.MapScale[scale]);
                    }
                }
            }
            if (moving)
            {
                foreach (PAIR p in movable)
                {
                    d.Draw(DataBase.WhereDisp(p.i - (p.j + 1) / 2, p.j, camera, scale), DataBase.select_tex, DepthID.Player, (float)DataBase.MapScale[scale]);
                }
            }
            else if(select_i != -1)
            {
                d.Draw(DataBase.WhereDisp(select_i - (select_j + 1) / 2, select_j, camera, scale), DataBase.select_tex, DepthID.Player, (float)DataBase.MapScale[scale]);
            }
        }
        // 更新
        public void Update(bool changeTurn = false)
        {
            if (changeTurn)
                UpdateTurn();

            // HP か LP が 0 以下になったユニットを削除
            for (int i = 0; i < myUnits.Count;i++)
            {
                Unit u = unitMap[myUnits[i].i, myUnits[i].j];
                if(u.HP <= 0 || u.LP <= 0)
                {
                    unitMap[myUnits[i].i, myUnits[i].j] = new Unit(UnitType.NULL);
                    myUnits.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < enemyUnits.Count; i++)
            {
                Unit u = unitMap[myUnits[i].i, myUnits[i].j];
                if (u.HP <= 0 || u.LP <= 0)
                {
                    unitMap[enemyUnits[i].i, enemyUnits[i].j] = new Unit(UnitType.NULL);
                    enemyUnits.RemoveAt(i);
                    i--;
                }
            }
        }
        // ターンの更新
        public void UpdateTurn()
        {
            foreach(PAIR p in myUnits)
            {
                unitMap[p.i, p.j].UpdateTurn();
            }
            foreach (PAIR p in enemyUnits)
            {
                unitMap[p.i, p.j].UpdateTurn();
            }
        }
        // ユニットの選択
        public void Select(int x_index, int y_index, UnitBox ub)
        {
            if (IsExist(x_index, y_index))
            {
                ub.Select(x_index, y_index, Find(x_index, y_index));
                select_i = x_index + (y_index + 1) / 2;
                select_j = y_index;
            }
            else
            {
                ub.Select(-1, 0, Find(x_index, y_index));
                select_i = -1;
                select_j = 0;
            }
            moving = false;
        }
        // ユニットの生産
        public void Produce(int x_index, int y_index, UnitType unitType)
        {
            if (IsExist(x_index, y_index) || unitType <= 0 || (int)unitType > 9) return;

            myUnits.Add(new PAIR(x_index + (y_index + 1) / 2, y_index));
            unitMap[x_index + (y_index + 1) / 2, y_index] = new Unit(unitType);
        }
        // 移動可能な位置を求める深さ優先探索関数
        public void dfs(ref int[,] map, int pow_2, PAIR now, ref List<PAIR> res, Map nMap)
        {
            int[] sx = { 1, 1, 0, -1, -1, 0 };
            int[] sy = { 0, 1, 1, 0, -1, -1 };
            for (int i = 0; i < 6; i++)
            {
                int x = now.i + sx[i] - (now.j + sy[i] + 1) / 2, y = now.j + sy[i];
                if (x >= 0 && x < DataBase.MAP_MAX && y >= 0 && y < DataBase.MAP_MAX &&
                    nMap.Data[x, y] != 0 && unitMap[now.i + sx[i], now.j + sy[i]].type == UnitType.NULL)
                {
                    if(nMap.Data[now.i - (now.j + 1) / 2, now.j] == 2 && nMap.Data[x, y] == 2)
                    {
                        if(pow_2 >= 1)
                        {
                            PAIR next = new PAIR(now.i + sx[i], now.j + sy[i]);
                            res.Add(next);
                            if (map[next.i, next.j] < pow_2 - 1)
                                map[next.i, next.j] = pow_2 - 1;
                            dfs(ref map, pow_2 - 1, next, ref res, nMap);
                        }
                    }
                    else
                    {
                        if (pow_2 >= 2)
                        {
                            PAIR next = new PAIR(now.i + sx[i], now.j + sy[i]);
                            res.Add(next);
                            if (map[next.i, next.j] < pow_2 - 2)
                                map[next.i, next.j] = pow_2 - 2;
                            dfs(ref map, pow_2 - 2, next, ref res, nMap);
                        }
                    }
                }
            }
        }
        // 移動のコマンドが実行されるための前処理
        public void StartMoving(Map nMap)
        {
            moving = true;
            movable.Clear();
            moveCost.Clear();
            int[,] map = new int[DataBase.MAP_MAX + (DataBase.MAP_MAX + 1) / 2, DataBase.MAP_MAX];
            List<PAIR> res = new List<PAIR>();
            for(int i = 0; i < DataBase.MAP_MAX + (DataBase.MAP_MAX + 1) / 2; i++)
            {
                for(int j = 0; j < DataBase.MAP_MAX; j++)
                {
                    map[i, j] = -1;
                }
            }
            dfs(ref map, unitMap[select_i, select_j].movePower * 2, new PAIR(select_i, select_j), ref res, nMap);
            if(res.Count == 0)
            {
                moving = false;
            }
            else
            {
                res.Distinct();
                for (int i = 0; i < res.Count; i++)
                {
                    movable.Add(new PAIR(res[i].i, res[i].j));
                    moveCost.Add(unitMap[select_i, select_j].movePower - map[res[i].i, res[i].j] / 2);
                }
            }
        }
        // 選択中のユニットの移動
        public void Move(int x_index, int y_index, UnitBox ub)
        {
            int n = 0;
            for(int k = 0; k < movable.Count; k++)
            {
                if (movable[k].i == x_index + (y_index + 1) / 2 && movable[k].j == y_index)
                {
                    n = k;
                    break;
                }
            }

            for (int i = 0; i < myUnits.Count;i++)
            {
                if(myUnits[i].i == select_i && myUnits[i].j == select_j)
                {
                    myUnits[i] = new PAIR(x_index + (y_index + 1) / 2, y_index);
                }
            }

            unitMap[x_index + (y_index + 1) / 2,y_index] = unitMap[select_i, select_j];
            unitMap[select_i, select_j] = new Unit(UnitType.NULL);
            unitMap[x_index + (y_index + 1) / 2, y_index].movePower -= moveCost[n];
            Select(x_index, y_index, ub);
        }
        // マップの座標(x_index, y_index)にユニットが存在するかどうか
        public bool IsExist(int x_index, int y_index)
        {
            return unitMap[x_index + (y_index + 1) / 2, y_index].type != UnitType.NULL;
        }
        // マップの座標(i,j)のユニット
        public Unit Find(int x_index, int y_index)
        {
            return unitMap[x_index + (y_index + 1) / 2, y_index];
        }
        #endregion
    }// class end
}// namespace end
