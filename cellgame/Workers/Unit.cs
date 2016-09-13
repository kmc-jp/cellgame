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
        public readonly UnitType type;
        public int HP { get { return hp; } set { if (value < 0) hp = 0; else if (value > MAX_HP) hp = MAX_HP; else hp = value; } }
        public int LP { get; protected set; }
        public int EXP { get; protected set; }
        public int ATK;
        public int ATK_range;
        int hp;
        public readonly int MAX_HP;
        public readonly int MAX_LP;
        public readonly int MAX_EXP;
        public readonly string unitName;
        public int moveRange = 2;
        public int movePower = 2;
        public bool defcommand = true;
        public bool command = true;
        #endregion
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Unit(UnitType _type)
        {
            type = _type;
            if(type > 0)
            {
                HP = MAX_HP = DataBase.MyUnitMAX_HP[(int)type - 1];
                LP = MAX_LP = DataBase.MyUnitMAX_LP[(int)type - 1];
                EXP = MAX_EXP = DataBase.MyUnitMAX_EXP[(int)type - 1];
                ATK = DataBase.MyUnitATK[(int)type - 1];
                ATK_range = DataBase.MyUnitATK_range[(int)type - 1];
                unitName = DataBase.MyUnitName[(int)type - 1];
            }
            else if(type < 0)
            {
                HP = MAX_HP = DataBase.EnemyUnitMAX_HP[(int)type + 5];
                LP = MAX_LP = DataBase.EnemyUnitMAX_LP[(int)type + 5];
                EXP = MAX_EXP = DataBase.EnemyUnitMAX_EXP[(int)type + 5];
                ATK = DataBase.MyUnitATK[(int)type + 5];
                ATK_range = DataBase.MyUnitATK_range[(int)type + 5];
                unitName = DataBase.EnemyUnitName[(int)type + 5];
            }
            else
            {
                command = defcommand = false;
            }
        }
        public void UpdateTurn()
        {
            LP--;
            movePower = moveRange;
            command = defcommand;
            if (ATK < 0) ATK *= -1;
        }
        #region Method
        #endregion
    }// Unit end
}// namespace CommonPart End
