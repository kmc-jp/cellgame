﻿using System;
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
        // 現在アニメーション中であるかどうか
        public bool moveAnimation = false;
        public bool attackAnimation = false;
        public UnitType producing = UnitType.NULL;
        // 現在移動可能な位置のリスト
        public List<PAIR> range = new List<PAIR>();
        public List<int> moveCost = new List<int>();
        // 移動コストのマップと最短経路復元ルート
        int[,] dijkMap = new int[DataBase.MAP_MAX + (DataBase.MAP_MAX + 1) / 2, DataBase.MAP_MAX];
        List<PAIR> moveRoute = new List<PAIR>();
        // 移動した割合
        int moveState;
        const int maxMoveState = 30;
        PAIR movingUnit;
        // 現在のターン
        int pturn = 0;
        int turn = 0;

        // ユニットボックス
        UnitBox ub;

        Unit[,] unitMap;

        const int INF = 1000000000;

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
                if((!moveAnimation && !attackAnimation) || p.i != select_i || p.j != select_j)
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
            }
            foreach (PAIR p in enemyUnits)
            {
                if ((!moveAnimation && !attackAnimation) || p.i != select_i || p.j != select_j)
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
            }
            if (moveAnimation)
            {
                int x_index = movingUnit.i - (movingUnit.j + 1) / 2, y_index = movingUnit.j;
                int tx_index = moveRoute.Last().i - (moveRoute.Last().j + 1) / 2, ty_index = moveRoute.Last().j;
                Vector pos = DataBase.WhereDisp(x_index, y_index, camera, scale);
                Vector tpos = DataBase.WhereDisp(tx_index, ty_index, camera, scale);
                d.Draw(pos + ((tpos - pos) * moveState / maxMoveState) + new Vector(26 * DataBase.MapScale[scale], 36 * DataBase.MapScale[scale]), unitMap[select_i, select_j].type > 0 ? DataBase.myUnit_tex[(int)unitMap[select_i, select_j].type - 1] : DataBase.enemyUnit_tex[(int)unitMap[select_i, select_j].type + 5], DepthID.Player, (float)DataBase.MapScale[scale]);
                moveState++;
                if(moveState == maxMoveState)
                {
                    moveState = 0;
                    movingUnit = moveRoute.Last();
                    moveRoute.RemoveAt(moveRoute.Count - 1);
                    if (moveRoute.Count == 0)
                    {
                        moveAnimation = false;
                    }
                }
            }
            else if (attackAnimation)
            {

            }
            else if (moving || attacking || producing != UnitType.NULL)
            {
                foreach (PAIR p in range)
                {
                    d.Draw(DataBase.WhereDisp(p.i - (p.j + 1) / 2, p.j, camera, scale), DataBase.select_tex, DepthID.Player, (float)DataBase.MapScale[scale]);
                }
            }
            else if (select_i != -1)
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
            if (unitMap[select_i, select_j].Strength < 0) return;

            moving = false;
            attacking = true;
            producing = UnitType.NULL;
            range.Clear();
            moveCost.Clear();
            
            int[] di = { 1, 1, 0, -1, -1, 0 };
            int[] dj = { 0, 1, 1, 0, -1, -1 };
            for(int i = 0; i < 6; i++)
            {
                int ni = select_i + di[i], nj = select_j + dj[i];
                if (ni - (nj + 1) / 2 >= 0 && ni - (nj + 1) / 2 < DataBase.MAP_MAX &&
                    nj >= 0 && nj < DataBase.MAP_MAX && unitMap[ni, nj].type < 0)
                {
                    range.Add(new PAIR(ni, nj));
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

            if (unitMap[select_i, select_j].Strength > 0)
            {
                unitMap[x_index + (y_index + 1) / 2, y_index].HP -= unitMap[select_i, select_j].Strength;
                unitMap[select_i, select_j].Strength *= -1;
            }
            attacking = false;
            unitMap[select_i, select_j].defcommand = true;
            unitMap[select_i, select_j].command = false;
            NextUnit();
        }
        // 攻撃コマンド中止
        public void CancelAttacking()
        {
            attacking = false;
        }
        // SortedSetを使うための比較関数を定義
        public class CompareDijkstraNode : IComparer<DijkstraNode>
        {
            public int Compare(DijkstraNode l, DijkstraNode r)
            {
                return (l.cost < r.cost ? -1 : (l.cost > r.cost ? 1 : (l.pos < r.pos ? -1 : (l.pos > r.pos ? 1 : 0))));
            }
        }
        public class DijkstraNode {
            public int cost;
            public PAIR pos;
            public DijkstraNode(int _cost, PAIR _pos)
            {
                cost = _cost;
                pos = _pos;
            }
        }
        // 移動可能な位置を求める探索関数
        void dijkstra(int pow_2, PAIR s, ref List<PAIR> res, ref List<int> costs, Map nMap)
        {
            int[] si = { 1, 1, 0, -1, -1, 0 };
            int[] sj = { 0, 1, 1, 0, -1, -1 };
            for (int i = 0; i < DataBase.MAP_MAX + (DataBase.MAP_MAX + 1) / 2; i++)
            {
                for (int j = 0; j < DataBase.MAP_MAX; j++)
                {
                    dijkMap[i, j] = -1;
                }
            }
            SortedSet<DijkstraNode> pq = new SortedSet<DijkstraNode>(new CompareDijkstraNode());
            pq.Add(new DijkstraNode(0, s));
            while(pq.Count != 0)
            {
                int pi = pq.First().pos.i, pj = pq.First().pos.j, pc = pq.First().cost;
                pq.Remove(pq.First());
                if (dijkMap[pi, pj] > 0) continue;
                dijkMap[pi, pj] = pc;
                if (pc != 0)
                {
                    res.Add(new PAIR(pi, pj));
                    costs.Add(pc);
                }
                for (int i = 0;i < 6; i++)
                {
                    int ni = pi + si[i], nj = pj + sj[i];
                    if (ni - (nj + 1) / 2 >= 0 && ni - (nj + 1) / 2 < DataBase.MAP_MAX && nj >= 0 && nj < DataBase.MAP_MAX)
                    {
                        int nc = pc + ((nMap.Data[pi - (pj + 1) / 2, pj] == 2 && nMap.Data[ni - (nj + 1) / 2, nj] == 2) ? 1 : 2);
                        if (nMap.Data[ni - (nj + 1) / 2, nj] != 0 && nc <= pow_2 && unitMap[ni, nj].type == UnitType.NULL)
                        {
                            pq.Add(new DijkstraNode(nc, new PAIR(ni, nj)));
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
            dijkstra(unitMap[select_i, select_j].movePower * 2, new PAIR(select_i, select_j), ref range, ref moveCost, nMap);
            if(range.Count == 0) moving = false;
        }
        // 移動コマンド
        public void Move(int x_index, int y_index, Map nMap)
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
            

            unitMap[x_index + (y_index + 1) / 2, y_index] = unitMap[select_i, select_j];
            unitMap[select_i, select_j] = new Unit(UnitType.NULL);
            unitMap[x_index + (y_index + 1) / 2, y_index].movePower -= (moveCost[n] + 1) / 2;
            Select(x_index, y_index);
            
            unitMap[select_i, select_j].defcommand = true;


            moveAnimation = true;
            moveState = 0;

            int[] si = { 1, 1, 0, -1, -1, 0 };
            int[] sj = { 0, 1, 1, 0, -1, -1 };

            PAIR tp = new PAIR(select_i, select_j);
            moveRoute.Add(tp);

            while (dijkMap[tp.i, tp.j] != 0)
            {
                int ti = 0, tj = 0, tc = INF;
                for (int i = 0; i < 6; i++)
                {
                    int ni = tp.i + si[i], nj = tp.j + sj[i], nc = dijkMap[ni, nj];
                    if (ni - (nj + 1) / 2 >= 0 && ni - (nj + 1) / 2 < DataBase.MAP_MAX && nj >= 0)
                    {
                        if(nj < DataBase.MAP_MAX && nc != -1 && nc == dijkMap[tp.i, tp.j] - ((nMap.Data[tp.i - (tp.j + 1) / 2, tp.j] == 2 && nMap.Data[ni - (nj + 1) / 2, nj] == 2) ? 1 : 2))
                        {
                            ti = ni;
                            tj = nj;
                            tc = nc;
                        }
                    }
                }
                tp.i = ti;
                tp.j = tj;
                moveRoute.Add(tp);
            }
            movingUnit = moveRoute.Last();
            moveRoute.RemoveAt(moveRoute.Count - 1);
        }
        // 移動コマンド中止
        public void CancelMoving()
        {
            moving = false;
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
