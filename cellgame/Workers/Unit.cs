using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonPart;

namespace CommonPart
{
    class Unit
    {
        #region Variable
        public UnitType type;

        // ユニット名
        public string Name {
            get { return type > 0 ? DataBase.MyUnitName[(int)type - 1] : type < 0 ? DataBase.EnemyUnitName[(int)type + 5] : ""; }
        }
        // 戦闘力
        public int Strength {
            get { return type > 0 ? DataBase.MyUnitStrength[(int)type - 1] : type < 0 ? DataBase.EnemyUnitStrength[(int)type + 5] : 0; }
        }
        // 最大HP
        public int MAX_HP {
            get { return type > 0 ? DataBase.MyUnitMAX_HP[(int)type - 1] : type < 0 ? DataBase.EnemyUnitMAX_HP[(int)type + 5] : 0; }
        }
        // 最大LP
        public int MAX_LP {
            get { return type > 0 ? DataBase.MyUnitMAX_LP[(int)type - 1] : type < 0 ? DataBase.EnemyUnitMAX_LP[(int)type + 5] : 0; }
        }
        // HP
        int hp;
        public int HP {
            get { return hp; } set { if (value < 0) hp = 0; else if (value > MAX_HP) hp = MAX_HP; else hp = value; }
        }
        // LP
        public int LP {
            get; set;
        }
        // 最大移動力
        public int moveRange {
            get { return type > 0 ? DataBase.MyUnitMoveRange[(int)type - 1] : type < 0 ? (type == UnitType.Virus && virusState == 1) ? 0 : DataBase.EnemyUnitMoveRange[(int)type + 5] : 0; }
        }
        // 移動力
        public int movePower;
        // コマンドを行ったかどうか
        public bool defcommand = true;
        public bool command = true;
        // 攻撃コマンドを行ったかどうか
        bool defattack = false;
        public bool attack = false;

        // 弱体化させる敵ユニットの種類（プラズマ細胞にのみ意味のある変数）
        public UnitType enemyType = UnitType.NULL;
        // ウイルスの状態（ウイルスにのみ意味のある変数）
        public int virusState = 0;
        #endregion
        #region Method
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Unit(UnitType _type)
        {
            type = _type;

            HP = MAX_HP;
            LP = MAX_LP;
            movePower = moveRange;

            if (type == UnitType.HelperT || type == UnitType.Gan)
            {
                attack = defattack = true;
            }

            if(type == UnitType.NULL)
            {
                attack = defattack = true;
                command = defcommand = false;
            }
        }
        public void UpdateTurn()
        {
            LP--;
            movePower = moveRange;
            command = defcommand;
            attack = defattack;
        }
        // 敵を倒したB細胞の進化
        public void Evolve(UnitType ene)
        {
            if (type != UnitType.B || (ene != UnitType.Kin && ene != UnitType.Kabi && ene != UnitType.Virus && ene != UnitType.Kiseichu)) return;

            type = UnitType.Plasma;

            HP = MAX_HP;
            LP = MAX_LP;
            movePower = moveRange;
            attack = defattack = true;

            enemyType = ene;
        }
        // ウイルスの定着
        public void Fix()
        {
            if (type != UnitType.Virus) return;
            virusState = 1;
            movePower = moveRange;
        }
        #endregion
    }// Unit end
}// namespace CommonPart End
