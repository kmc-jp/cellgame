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
        // 現在コマンドの入力中であるかどうか
        public bool moving = false;
        public bool attacking = false;
        public UnitType producing = UnitType.NULL;
        // 現在移動可能な位置のリスト
        public List<PAIR> range = new List<PAIR>();
        public List<int> moveCost = new List<int>();
        // 現在のターン
        int pturn = 0;
        int turn = 0;

        // ユニットボックス
        UnitBox ub;

        Unit[,] unitMap;

        #endregion

        #region Method
        // コンストラクタ
        public UnitManager(ref UnitBox _ub)
        {
            // 味方ユニット
            myUnits = new List<PAIR>();

            // 敵ユニット
            enemyUnits = new List<PAIR>();

            ub = _ub;

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
            if (moving || attacking || producing != UnitType.NULL)
            {
                foreach (PAIR p in range)
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
        public void Update()
        {
            if (pturn < turn)
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
                Unit u = unitMap[enemyUnits[i].i, enemyUnits[i].j];
                if (u.HP <= 0 || u.LP <= 0)
                {
                    unitMap[enemyUnits[i].i, enemyUnits[i].j] = new Unit(UnitType.NULL);
                    enemyUnits.RemoveAt(i);
                    i--;
                }
            }
            // 選択中のユニットが死滅すると選択解除
            if (select_i != -1 && unitMap[select_i, select_j].type == UnitType.NULL)
            {
                Unselect();
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
            pturn = turn;
            NextUnit();
        }
        // ユニットの選択
        public void Select(int x_index, int y_index)
        {
            if (IsExist(x_index, y_index))
            {
                ub.Select(x_index, y_index, Find(x_index, y_index));
                select_i = x_index + (y_index + 1) / 2;
                select_j = y_index;
                moving = false;
            }
            else
            {
                Unselect();
            }
        }
        // ユニットの選択解除
        public void Unselect()
        {
            ub.Unselect();
            select_i = -1;
            select_j = 0;
            moving = false;
        }
        // コマンドが実行されていない次のユニットを選択
        public void NextUnit()
        {
            bool flag = true;
            foreach(PAIR p in myUnits)
            {
                if(unitMap[p.i, p.j].command)
                {
                    ub.Select(p.i - (p.j + 1) / 2, p.j, unitMap[p.i, p.j]);
                    select_i = p.i;
                    select_j = p.j;
                    flag = false;
                    break;
                }
            }
            if (flag)
            {
                Unselect();
                if (myUnits.Count != 0) turn++;
            }
        }
        // スキップコマンド
        public void Skip()
        {
            if (moving || attacking || select_i == -1) return;

            unitMap[select_i, select_j].command = false;
            NextUnit();
        }
        // 休眠コマンド
        public void Sleep()
        {
            if (moving || attacking) return;

            unitMap[select_i, select_j].defcommand = unitMap[select_i, select_j].command = false;
            NextUnit();
        }
        // 生産コマンドが実行されるための前処理
        public void StartProducing(UnitType ut, Map nMap)
        {
            producing = ut;
            moving = false;
            attacking = false;
            range.Clear();
            moveCost.Clear();
            for (int x = 0; x < DataBase.MAP_MAX; x++)
            {
                for (int y = 0; y < DataBase.MAP_MAX; y++)
                {
                    if(unitMap[x + (y + 1) / 2, y].type == UnitType.NULL &&
                        nMap.Data[x, y] != 0)
                    {
                        range.Add(new PAIR(x + (y + 1) / 2, y));
                    }
                }
            }
        }
        // 生産コマンド
        public bool Produce(int x_index, int y_index)
        {
            if (IsExist(x_index, y_index)) return false;

            if(producing > 0) myUnits.Add(new PAIR(x_index + (y_index + 1) / 2, y_index));
            else enemyUnits.Add(new PAIR(x_index + (y_index + 1) / 2, y_index));
            unitMap[x_index + (y_index + 1) / 2, y_index] = new Unit(producing);

            producing = UnitType.NULL;

            Select(x_index, y_index);

            return true;
        }
        // 攻撃コマンドが実行されるための前処理
        public void StartAttacking()
        {
            if (unitMap[select_i, select_j].ATK < 0) return;

            moving = false;
            attacking = true;
            producing = UnitType.NULL;
            range.Clear();
            moveCost.Clear();

            List<PAIR> bfs = new List<PAIR>();
            int[,] map = new int[DataBase.MAP_MAX + (DataBase.MAP_MAX + 1) / 2, DataBase.MAP_MAX];
            for(int i = 0; i < DataBase.MAP_MAX + (DataBase.MAP_MAX + 1) / 2; i++)
            {
                for(int j = 0; j < DataBase.MAP_MAX; j++)
                {
                    map[i, j] = -1;
                }
            }
            map[select_i, select_j] = 0;
            bfs.Add(new PAIR(select_i, select_j));
            int[] di = { 1, 1, 0, -1, -1, 0 };
            int[] dj = { 0, 1, 1, 0, -1, -1 };
            while (bfs.Count != 0)
            {
                PAIR p = bfs.Last(); bfs.RemoveAt(bfs.Count - 1);
                for(int k = 0; k < 6; k++)
                {
                    if (p.i + di[k] - (p.j + dj[k] + 1) / 2 >= 0 && p.i + di[k] - (p.j + dj[k] + 1) / 2 < DataBase.MAP_MAX &&
                        p.j + dj[k] >= 0 && p.j + dj[k] < DataBase.MAP_MAX &&
                        map[p.i, p.j] < unitMap[select_i, select_j].ATK_range &&
                        map[p.i + di[k], p.j + dj[k]] == -1)
                    {
                        bfs.Add(new PAIR(p.i + di[k], p.j + dj[k]));
                        map[p.i + di[k], p.j + dj[k]] = map[p.i, p.j] + 1;
                        if(unitMap[p.i + di[k], p.j + dj[k]].type < 0)
                        {
                            range.Add(new PAIR(p.i + di[k], p.j + dj[k]));
                        }
                    }
                }
            }
            if(range.Count == 0)
            {
                attacking = false;
            }
        }
        // 攻撃コマンド
        public void Attack(int x_index, int y_index)
        {
            bool flag = true;
            foreach (PAIR pos in range)
            {
                if (x_index == pos.i - (pos.j + 1) / 2 && y_index == pos.j)
                {
                    flag = false;
                    break;
                }
            }
            if (flag) return;

            if(unitMap[select_i, select_j].ATK > 0)
                unitMap[x_index + (y_index + 1) / 2, y_index].HP -= unitMap[select_i, select_j].ATK;

            unitMap[select_i, select_j].ATK *= -1;
            attacking = false;
            unitMap[select_i, select_j].defcommand = true;
            unitMap[select_i, select_j].command = false;
            NextUnit();
        }
        // 移動可能な位置を求める深さ優先探索関数
        void dfs(ref int[,] map, int pow_2, PAIR now, ref List<PAIR> res, Map nMap)
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
            attacking = false;
            moving = true;
            producing = UnitType.NULL;
            range.Clear();
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
                    range.Add(new PAIR(res[i].i, res[i].j));
                    moveCost.Add(unitMap[select_i, select_j].movePower - map[res[i].i, res[i].j] / 2);
                }
            }
        }
        // 移動コマンド
        public void Move(int x_index, int y_index)
        {
            bool flag = true;
            foreach (PAIR pos in range)
            {
                if (x_index == pos.i - (pos.j + 1) / 2 && y_index == pos.j)
                {
                    flag = false;
                    break;
                }
            }
            if (flag) return;

            int n = 0;
            for(int k = 0; k < range.Count; k++)
            {
                if (range[k].i == x_index + (y_index + 1) / 2 && range[k].j == y_index)
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
            Select(x_index, y_index);
            
            unitMap[select_i, select_j].defcommand = true;
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
        // マップの座標(i,j)のユニットの種類
        public UnitType FindType(int x_index, int y_index)
        {
            return unitMap[x_index + (y_index + 1) / 2, y_index].type;
        }
        #endregion
    }// class end
}// namespace end
