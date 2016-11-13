using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CommonPart
{
    class UnitManager
    {
        #region Variable
        // 味方ユニット
        public List<PAIR> myUnits;
        // 敵ユニット
        public List<PAIR> enemyUnits;
        // 現在選択中のユニット
        public int select_i = -1, select_j = 0;
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
        public int moveState;
        public int maxMoveState = 30;
        public int attackState;
        public int maxAttackState = 15;
        PAIR movingUnit;
        PAIR attackedUnit;

        int Da, Db;

        public bool phase = false;

        public bool nex = true;

        // ユニットボックス
        UnitBox ub;

        public UnitMap uMap;



        #endregion

        #region Method
        // コンストラクタ
        public UnitManager(ref UnitBox _ub, UnitMap _uMap)
        {
            // 味方ユニット
            myUnits = new List<PAIR>();

            // 敵ユニット
            enemyUnits = new List<PAIR>();

            ub = _ub;

            uMap = new UnitMap(_uMap);
            for(int i = 0;i < DataBase.MAP_MAX; i++)
            {
                for(int j = 0;j < DataBase.MAP_MAX; j++)
                {
                    if(uMap.GetType(i, j) > 0)
                    {
                        myUnits.Add(new PAIR(i + (j + 1) / 2, j));
                    }
                    else if (uMap.GetType(i, j) < 0)
                    {
                        enemyUnits.Add(new PAIR(i + (j + 1) / 2, j));
                        if(GetType(i, j) == UnitType.Gan)
                        {
                            AI.Gan_N++;
                        }
                    }
                }
            }
            NextUnit();
        }


        // 描画
        public void Draw(Drawing d, Vector camera, int scale)
        {
            if (moveAnimation || attackAnimation) uMap.Draw(d, camera, scale, select_i - (select_j + 1) / 2, select_j);
            else uMap.Draw(d, camera, scale);

            if (moveAnimation)
            {
                int x_index = movingUnit.i - (movingUnit.j + 1) / 2, y_index = movingUnit.j;
                int tx_index = moveRoute.Last().i - (moveRoute.Last().j + 1) / 2, ty_index = moveRoute.Last().j;
                Vector pos = DataBase.WhereDisp(x_index, y_index, camera, scale);
                Vector tpos = DataBase.WhereDisp(tx_index, ty_index, camera, scale);
                UnitType ut = uMap.data[select_i, select_j].type;
                if (ut == UnitType.Plasma)
                {
                    d.Draw(pos + ((tpos - pos) * moveState / Math.Max(maxMoveState, 1)) + new Vector(26 * DataBase.MapScale[scale], 36 * DataBase.MapScale[scale]), DataBase.Plasma_tex[(int)uMap.data[select_i, select_j].enemyType + 5], DepthID.Player, (float)DataBase.MapScale[scale]);
                }
                else
                {
                    d.Draw(pos + ((tpos - pos) * moveState / Math.Max(maxMoveState, 1)) + new Vector(26 * DataBase.MapScale[scale], 36 * DataBase.MapScale[scale]), uMap.data[select_i, select_j].type > 0 ? DataBase.myUnit_tex[(int)uMap.data[select_i, select_j].type - 1] : DataBase.enemyUnit_tex[(int)uMap.data[select_i, select_j].type + 5], DepthID.Player, (float)DataBase.MapScale[scale]);
                }
                moveState++;
                if(moveState >= maxMoveState)
                {
                    moveState = 0;
                    movingUnit = moveRoute.Last();
                    moveRoute.RemoveAt(moveRoute.Count - 1);
                    if (moveRoute.Count == 0)
                    {
                        moveAnimation = false;
                        int[] si = { 1, 1, 0, -1, -1, 0 };
                        int[] sj = { 0, 1, 1, 0, -1, -1 };
                        bool flag = true;
                        for(int i = 0;i < 6; i++)
                        {
                            int ni = select_i + si[i], nj = select_j + sj[i];
                            if(ni - (nj + 1) / 2 >= 0 && ni - (nj + 1) / 2 < DataBase.MAP_MAX && nj >= 0 && nj < DataBase.MAP_MAX && uMap.data[ni, nj].type < 0)
                            {
                                flag = false;
                            }
                        }
                        if(uMap.data[select_i, select_j].movePower == 0 && (uMap.data[select_i, select_j].attack || flag))
                        {
                            uMap.data[select_i, select_j].command = false;
                            NextUnit();
                        }
                    }
                }
            }
            else if (attackAnimation)
            {
                Vector pos = DataBase.WhereDisp(select_i - (select_j + 1) / 2, select_j, camera, scale);
                Vector tpos = DataBase.WhereDisp(attackedUnit.i - (attackedUnit.j + 1) / 2, attackedUnit.j, camera, scale);
                UnitType ut = uMap.data[select_i, select_j].type;
                if (ut == UnitType.Plasma)
                {
                    d.Draw(pos + ((tpos - pos) * Math.Min(attackState, maxAttackState / 3 * 4 - attackState) / Math.Max(maxAttackState, 1)) + new Vector(26 * DataBase.MapScale[scale], 36 * DataBase.MapScale[scale]), DataBase.Plasma_tex[(int)uMap.data[select_i, select_j].enemyType + 5], DepthID.Player, (float)DataBase.MapScale[scale]);
                }
                else
                {
                    d.Draw(pos + ((tpos - pos) * Math.Min(attackState, maxAttackState / 3 * 4 - attackState) / Math.Max(maxAttackState, 1)) + new Vector(26 * DataBase.MapScale[scale], 36 * DataBase.MapScale[scale]), uMap.data[select_i, select_j].type > 0 ? DataBase.myUnit_tex[(int)uMap.data[select_i, select_j].type - 1] : DataBase.enemyUnit_tex[(int)uMap.data[select_i, select_j].type + 5], DepthID.Player, (float)DataBase.MapScale[scale]);
                }
                attackState++;
                if(attackState > maxAttackState / 3 * 4)
                {
                    attackState = 0;
                    attackAnimation = false;
                    uMap.data[select_i, select_j].HP -= Da;
                    uMap.data[attackedUnit.i, attackedUnit.j].HP -= Db;
                    // ユニットを倒したとき
                    if (uMap.data[attackedUnit.i, attackedUnit.j].HP == 0)
                    {
                        // マクロファージか樹状細胞なら研究力を加算
                        if (uMap.data[select_i, select_j].type == UnitType.Macro)
                        {
                            PlayScene.studyPower += uMap.data[attackedUnit.i, attackedUnit.j].Strength / 2;
                        }
                        else if (uMap.data[select_i, select_j].type == UnitType.Jujo)
                        {
                            PlayScene.studyPower += uMap.data[attackedUnit.i, attackedUnit.j].Strength;
                        }
                        // B細胞ならプラズマ細胞に進化
                        else if (uMap.data[select_i, select_j].HP != 0 && uMap.data[select_i, select_j].type == UnitType.B)
                        {
                            uMap.data[select_i, select_j].Evolve(uMap.data[attackedUnit.i, attackedUnit.j].type);
                        }
                    }
                    // ユニットが倒されたとき
                    if (uMap.data[select_i, select_j].HP == 0)
                    {
                        // マクロファージか樹状細胞なら研究力を加算
                        if (uMap.data[attackedUnit.i, attackedUnit.j].type == UnitType.Macro)
                        {
                            PlayScene.studyPower += uMap.data[select_i, select_j].Strength / 2;
                        }
                        else if (uMap.data[attackedUnit.i, attackedUnit.j].type == UnitType.Jujo)
                        {
                            PlayScene.studyPower += uMap.data[select_i, select_j].Strength;
                        }
                        // B細胞ならプラズマ細胞に進化
                        else if (uMap.data[attackedUnit.i, attackedUnit.j].HP != 0 && uMap.data[attackedUnit.i, attackedUnit.j].type == UnitType.B)
                        {
                            uMap.data[attackedUnit.i, attackedUnit.j].Evolve(uMap.data[select_i, select_j].type);
                        }
                    }

                    if (nex && uMap.data[select_i, select_j].movePower == 0)
                    {
                        uMap.data[select_i, select_j].command = false;
                        NextUnit();
                    }
                }
            }
            else if (moving || attacking || producing != UnitType.NULL)
            {
                foreach (PAIR p in range)
                {
                    d.Draw(DataBase.WhereDisp(p.i - (p.j + 1) / 2, p.j, camera, scale), DataBase.select_tex, DepthID.Player, (float)DataBase.MapScale[scale]);
                }
            }
            else if (select_i != -1 && !AI.enemyTurn)
            {
                d.Draw(DataBase.WhereDisp(select_i - (select_j + 1) / 2, select_j, camera, scale), DataBase.select_tex, DepthID.Player, (float)DataBase.MapScale[scale]);
            }
        }
        // 更新
        public void Update(MouseState pstate, MouseState state, PlayScene ps, bool ai)
        {
            // 左クリックされたときに移動コマンド中でありその座標が移動可能な位置であればその位置へ選択中のユニットを移動
            if (!ai && pstate.LeftButton != ButtonState.Pressed && state.LeftButton == ButtonState.Pressed && !moveAnimation && !attackAnimation)
            {
                if (state.X >= 0 && state.X <= Game1._WindowSizeX && state.Y >= 0 && state.Y <= Game1._WindowSizeY)
                {
                    PAIR p = ps.WhichHex(state.X, state.Y);
                    if (p.i >= 0 && p.j >= 0)
                    {
                        if (moving)
                        {
                            Move(p.i, p.j);
                        }
                        else if (attacking)
                        {
                            Attack(p.i, p.j);
                        }
                        else if (producing == UnitType.NULL)
                        {
                            if (GetType(p.i, p.j) != UnitType.NULL)
                            {
                                Select(p.i, p.j);
                            }
                            else
                            {
                                Unselect();
                            }
                        }
                    }
                }
            }
            if(ub.x_index != -1)
                ub.u = Find(ub.x_index, ub.y_index);
            

            // HP か LP が 0 以下になったユニットを削除
            for (int i = 0; i < myUnits.Count;i++)
            {
                Unit u = uMap.data[myUnits[i].i, myUnits[i].j];
                if(u.HP <= 0 || u.LP <= 0)
                {
                    uMap.data[myUnits[i].i, myUnits[i].j] = new Unit(UnitType.NULL);
                    myUnits.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < enemyUnits.Count; i++)
            {
                Unit u = uMap.data[enemyUnits[i].i, enemyUnits[i].j];
                if (u.HP <= 0 || u.LP <= 0)
                {
                    uMap.data[enemyUnits[i].i, enemyUnits[i].j] = new Unit(UnitType.NULL);
                    enemyUnits.RemoveAt(i);
                    i--;
                }
            }
            // 選択中のユニットが死滅すると選択解除
            if (select_i != -1 && uMap.data[select_i, select_j].type == UnitType.NULL)
            {
                Unselect();
            }
        }
        // ターンの更新
        public int UpdateTurn()
        {
            foreach(PAIR p in myUnits)
            {
                uMap.data[p.i, p.j].UpdateTurn();
            }
            foreach (PAIR p in enemyUnits)
            {
                uMap.data[p.i, p.j].UpdateTurn();
            }
            NextUnit();

            // 体温の更新
            PlayScene.BodyTemp += (enemyUnits.Count - myUnits.Count) / 100m + 0.08m;

            if (PlayScene.BodyTemp > 42m) return -1;
            bool flag = true;
            for(int i = 0;i < DataBase.MAP_MAX; i++)
            {
                for(int j = 0;j < DataBase.MAP_MAX; j++)
                {
                    if(PlayScene.nMap.Data[i, j] == 3 && GetType(i, j) <= 0)
                    {
                        flag = false;
                        break;
                    }
                }
                if (!flag)
                    break;
            }
            if (flag) return 1;
            return 0;
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
            if (AI.enemyTurn) return;
            bool flag = true;
            foreach(PAIR p in myUnits)
            {
                if(uMap.data[p.i, p.j].command)
                {
                    ub.Select(p.i - (p.j + 1) / 2, p.j, uMap.data[p.i, p.j]);
                    select_i = p.i;
                    select_j = p.j;
                    flag = false;
                    phase = true;
                    break;
                }
            }
            if (flag)
            {
                Unselect();
            }
        }
        // スキップコマンド
        public void Skip()
        {
            if (moving || attacking || select_i < 0) return;

            uMap.data[select_i, select_j].command = false;
            NextUnit();
        }
        // 休眠コマンド
        public void Sleep()
        {
            if (moving || attacking || select_i < 0) return;

            uMap.data[select_i, select_j].defcommand = uMap.data[select_i, select_j].command = false;
            NextUnit();
        }
        // 生産コマンドが実行されるための前処理
        public void StartProducing(UnitType ut)
        {
            if (moveAnimation || attackAnimation) return;
            producing = ut;
            moving = false;
            attacking = false;
            range.Clear();
            moveCost.Clear();
            for (int x = 0; x < DataBase.MAP_MAX; x++)
            {
                for (int y = 0; y < DataBase.MAP_MAX; y++)
                {
                    if(uMap.data[x + (y + 1) / 2, y].type == UnitType.NULL &&
                        PlayScene.nMap.Data[x, y] == (ut > 0 ? 4 : 3))
                    {
                        range.Add(new PAIR(x + (y + 1) / 2, y));
                    }
                }
            }
            Unselect();
        }
        // 生産コマンド
        public bool Produce(int x_index, int y_index)
        {
            // 生産できる位置かどうか判定
            bool flag = true;
            foreach (PAIR p in range)
            {
                if(p.i - (p.j + 1) / 2 == x_index && p.j == y_index)
                {
                    flag = false;
                    break;
                }
            }
            if (flag) return false;
            // 判定終了

            if(producing > 0) myUnits.Add(new PAIR(x_index + (y_index + 1) / 2, y_index));
            else enemyUnits.Add(new PAIR(x_index + (y_index + 1) / 2, y_index));
            uMap.data[x_index + (y_index + 1) / 2, y_index] = new Unit(producing);

            producing = UnitType.NULL;

            Select(x_index, y_index);

            return true;
        }
        // 生産コマンド中止
        public void CancelProducing()
        {
            producing = UnitType.NULL;
        }
        // 攻撃コマンドが実行されるための前処理
        public void StartAttacking()
        {
            if (select_i < 0 || uMap.data[select_i, select_j].attack) return;

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
                    nj >= 0 && nj < DataBase.MAP_MAX && (int)uMap.data[ni, nj].type * (int)uMap.data[select_i, select_j].type < 0
                    && ((uMap.data[ni, nj].type != UnitType.Gan && !(uMap.data[ni, nj].type == UnitType.Virus && uMap.data[ni, nj].virusState == 1)) || uMap.data[select_i, select_j].type == UnitType.KillerT || uMap.data[select_i, select_j].type == UnitType.NK))
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
        public void Attack(int x_index, int y_index, bool _nex = true)
        {
            bool flag = true;
            nex = _nex;
            foreach (PAIR pos in range)
            {
                if (x_index == pos.i - (pos.j + 1) / 2 && y_index == pos.j)
                {
                    flag = false;
                    break;
                }
            }
            if (flag) return;

            int ai = x_index + (y_index + 1) / 2, aj = y_index;

            if (!uMap.data[select_i, select_j].attack)
            {
                uMap.data[select_i, select_j].attack = true;
                
                if(uMap.data[select_i, select_j].type == UnitType.Kosan && uMap.data[ai, aj].type == UnitType.Kiseichu)
                    DataBase.Battle(RealStrength(select_i, select_j) * 2, RealStrength(ai, aj), out Da, out Db);
                else
                    DataBase.Battle(RealStrength(select_i, select_j), RealStrength(ai, aj), out Da, out Db);

                if(StudyManager.IsDone(Study.Opuso) && IsWeakened(ai, aj))
                    Da = 0;

            }
            attacking = false;
            uMap.data[select_i, select_j].defcommand = true;
            attackState = 0;
            attackAnimation = true;
            attackedUnit = new PAIR(ai, aj);
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
        void dijkstra(int pow_2, PAIR s, ref List<PAIR> res, ref List<int> costs)
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
                if (dijkMap[pi, pj] >= 0) continue;
                dijkMap[pi, pj] = pc;
                if (pc != 0)
                {
                    res.Add(new PAIR(pi, pj));
                    costs.Add(pc);
                }
                if (pc == 5) continue;
                for (int i = 0;i < 6; i++)
                {
                    int ni = pi + si[i], nj = pj + sj[i];
                    if (ni - (nj + 1) / 2 >= 0 && ni - (nj + 1) / 2 < DataBase.MAP_MAX && nj >= 0 && nj < DataBase.MAP_MAX)
                    {
                        int nc = ((PlayScene.nMap.Data[ni - (nj + 1) / 2, nj] == 2) ? pc + 1 : Math.Max(pc + 1, pow_2));
                        if (PlayScene.nMap.Data[ni - (nj + 1) / 2, nj] != 0 && nc <= pow_2 && uMap.data[ni, nj].type == UnitType.NULL)
                        {
                            pq.Add(new DijkstraNode(nc, new PAIR(ni, nj)));
                        }
                    }
                }
            }
        }
        // 移動のコマンドが実行されるための前処理
        public void StartMoving()
        {
            if (select_i < 0) return;
            attacking = false;
            moving = true;
            producing = UnitType.NULL;
            range.Clear();
            moveCost.Clear();
            dijkstra(uMap.data[select_i, select_j].movePower, new PAIR(select_i, select_j), ref range, ref moveCost);
            if(range.Count == 0) moving = false;
        }
        // 移動コマンド
        public void Move(int x_index, int y_index)
        {
            if (select_i < 0) return;
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
            for (int i = 0; i < enemyUnits.Count; i++)
            {
                if (enemyUnits[i].i == select_i && enemyUnits[i].j == select_j)
                {
                    enemyUnits[i] = new PAIR(x_index + (y_index + 1) / 2, y_index);
                }
            }


            uMap.data[x_index + (y_index + 1) / 2, y_index] = uMap.data[select_i, select_j];
            uMap.data[select_i, select_j] = new Unit(UnitType.NULL);
            uMap.data[x_index + (y_index + 1) / 2, y_index].movePower -= moveCost[n];
            Select(x_index, y_index);
            
            uMap.data[select_i, select_j].defcommand = true;


            moveAnimation = true;
            moveState = 0;

            int[] si = { 1, 1, 0, -1, -1, 0 };
            int[] sj = { 0, 1, 1, 0, -1, -1 };
            const int INF = 1000000000;

            PAIR tp = new PAIR(select_i, select_j);
            moveRoute.Add(tp);

            while (dijkMap[tp.i, tp.j] != 0)
            {
                int ti = 0, tj = 0, tc = INF;
                for (int i = 0; i < 6; i++)
                {
                    int ni = tp.i + si[i], nj = tp.j + sj[i];
                    if (ni - (nj + 1) / 2 >= 0 && ni - (nj + 1) / 2 < DataBase.MAP_MAX && nj >= 0 && nj < DataBase.MAP_MAX)
                    {
                        int nc = dijkMap[ni, nj];
                        if (nc != -1 && ((PlayScene.nMap.Data[tp.i - (tp.j + 1) / 2, tp.j] == 2 && nc == dijkMap[tp.i, tp.j] - 1)) || (PlayScene.nMap.Data[tp.i - (tp.j + 1) / 2, tp.j] != 2 && nc >= 0 && nc < tc))
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
            range.Clear();
        }
        // 分裂準備
        public void StartDoubling()
        {
            moving = false;
            attacking = false;
            producing = UnitType.NULL;
            range.Clear();
            moveCost.Clear();

            int[] si = { 1, 1, 0, -1, -1, 0 };
            int[] sj = { 0, 1, 1, 0, -1, -1 };
            for (int i = 0;i < 6;i++)
            {
                int ni = select_i + si[i], nj = select_j + sj[i];
                if(ni - (nj + 1) / 2 >= 0 && ni - (nj + 1) / 2 < DataBase.MAP_MAX && nj >= 0 && nj < DataBase.MAP_MAX && PlayScene.nMap.Data[ni - (nj + 1) / 2, nj] != 0 && GetType(ni - (nj + 1) / 2, nj) == UnitType.NULL)
                {
                    range.Add(new PAIR(ni, nj));
                }
            }
        }
        // 分裂
        public void Double(PAIR p)
        {
            if (range.Count == 0) return;
            bool flag = true;
            foreach (PAIR pi in range)
            {
                if(pi == p)
                {
                    flag = false;
                    break;
                }
            }
            if (flag) return;
            uMap.data[p.i, p.j] = new Unit(GetType(select_i - (select_j + 1) / 2, select_j));
            enemyUnits.Add(p);
            if (GetType(select_i - (select_j + 1) / 2, select_j) == UnitType.Kin || GetType(select_i - (select_j + 1) / 2, select_j) == UnitType.Kabi)
            {
                uMap.data[p.i, p.j].LP = uMap.data[select_i, select_j].LP / 2;
                uMap.data[p.i, p.j].HP = uMap.data[select_i, select_j].HP / 2;
                uMap.data[select_i, select_j].LP -= uMap.data[p.i, p.j].LP;
                uMap.data[select_i, select_j].HP -= uMap.data[p.i, p.j].HP;
            }
        }
        // 削除コマンド
        public void Delete()
        {
            if (select_i < 0) return;

            myUnits.Remove(new PAIR(select_i, select_j));
            enemyUnits.Remove(new PAIR(select_i, select_j));

            uMap.data[select_i, select_j] = new Unit(UnitType.NULL);

            Unselect();
        }
        // マップの座標(x_index, y_index)にユニットが存在するかどうか
        public bool IsExist(int x_index, int y_index)
        {
            return uMap.data[x_index + (y_index + 1) / 2, y_index].type != UnitType.NULL;
        }
        // マップの座標(i,j)のユニット
        public Unit Find(int x_index, int y_index)
        {
            return uMap.data[x_index + (y_index + 1) / 2, y_index];
        }
        // マップの座標(i,j)のユニットの種類
        public UnitType GetType(int x_index, int y_index)
        {
            return uMap.data[x_index + (y_index + 1) / 2, y_index].type;
        }
        // ユニットの現在の実際の戦闘力
        public int RealStrength(int _i, int _j)
        {
            if (_i - (_j + 1) / 2 < 0 || _i - (_j + 1) / 2 >= DataBase.MAP_MAX || _j < 0 || _j >= DataBase.MAP_MAX) return 0;
            if (uMap.data[_i, _j].type == UnitType.NULL) return 0;

            int[] di = { 1, 1, 0, -1, -1, 0 };
            int[] dj = { 0, 1, 1, 0, -1, -1 };


            int rate = 10;
            if (uMap.data[_i, _j].type > 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    int ni = select_i + di[i], nj = select_j + dj[i];
                    if (ni - (nj + 1) / 2 >= 0 && ni - (nj + 1) / 2 < DataBase.MAP_MAX &&
                        nj >= 0 && nj < DataBase.MAP_MAX && uMap.data[ni, nj].type == UnitType.HelperT)
                    {
                        rate += (StudyManager.IsDone(Study.Saito) ? 4 : 2);
                    }
                }
                return uMap.data[_i, _j].Strength * rate / 10;
            }
            rate = 30;
            int[,] used = new int[DataBase.MAP_MAX, DataBase.MAP_MAX];

            for (int i = 0;i < DataBase.MAP_MAX;i++)
            {
                for (int j = 0;j < DataBase.MAP_MAX;j++)
                {
                    used[i, j] = -1;
                }
            }

            Queue<PAIR> Q = new Queue<PAIR>();
            Q.Enqueue(new PAIR(select_i, select_j));
            used[select_i - (select_j + 1) / 2, select_j] = 0;
            while(Q.Count != 0)
            {
                PAIR p = Q.Dequeue();
                if (used[p.i - (p.j + 1) / 2, p.j] == 2) continue;
                for (int i = 0; i < 6; i++)
                {
                    int ni = p.i + di[i], nj = p.j + dj[i];
                    if (ni - (nj + 1) / 2 >= 0 && ni - (nj + 1) / 2 < DataBase.MAP_MAX &&
                        nj >= 0 && nj < DataBase.MAP_MAX && used[ni - (nj + 1) / 2, nj] == -1)
                    {
                        used[ni - (nj + 1) / 2, nj] = used[p.i - (p.j + 1) / 2, p.j] + 1;
                        Q.Enqueue(new PAIR(ni, nj));
                        if(uMap.data[_i, _j].type == uMap.data[ni, nj].enemyType)
                        {
                            rate = Math.Max(0, rate - (StudyManager.IsDone(Study.Saito) ? 4 : 3));
                        }
                    }
                }
            }
            return uMap.data[_i, _j].Strength * rate / 30;
        }
        public bool IsWeakened(int _i, int _j)
        {
            if (_i - (_j + 1) / 2 < 0 || _i - (_j + 1) / 2>= DataBase.MAP_MAX || _j < 0 || _j >= DataBase.MAP_MAX) return false;
            if (uMap.data[_i, _j].type >= 0) return false;

            int[] di = { 1, 1, 0, -1, -1, 0 };
            int[] dj = { 0, 1, 1, 0, -1, -1 };

            
            int[,] used = new int[DataBase.MAP_MAX, DataBase.MAP_MAX];

            for (int i = 0; i < DataBase.MAP_MAX; i++)
            {
                for (int j = 0; j < DataBase.MAP_MAX; j++)
                {
                    used[i, j] = -1;
                }
            }

            Queue<PAIR> Q = new Queue<PAIR>();
            Q.Enqueue(new PAIR(select_i, select_j));
            used[select_i - (select_j + 1) / 2, select_j] = 0;
            while (Q.Count != 0)
            {
                PAIR p = Q.Dequeue();
                if (used[p.i - (p.j + 1) / 2, p.j] == 2) continue;
                for (int i = 0; i < 6; i++)
                {
                    int ni = p.i + di[i], nj = p.j + dj[i];
                    if (ni - (nj + 1) / 2 >= 0 && ni - (nj + 1) / 2 < DataBase.MAP_MAX &&
                        nj >= 0 && nj < DataBase.MAP_MAX && used[ni - (nj + 1) / 2, nj] == -1)
                    {
                        used[ni - (nj + 1), nj] = used[p.i - (p.j + 1) / 2, p.j] + 1;
                        Q.Enqueue(new PAIR(ni, nj));
                        if (uMap.data[_i, _j].type == uMap.data[ni, nj].enemyType)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        #endregion
    }// class end
}// namespace end
