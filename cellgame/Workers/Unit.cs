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
        public int Strength;
        int hp;
        public readonly int MAX_HP;
        public readonly int MAX_LP;
        public readonly string unitName;
        public int moveRange;
        public int movePower;
        public bool defcommand = true;
        public bool command = true;
        #endregion
        #region Method
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
                Strength = DataBase.MyUnitStrength[(int)type - 1];
                unitName = DataBase.MyUnitName[(int)type - 1];
                movePower = moveRange = DataBase.MyUnitMoveRange[(int)type - 1];
            }
            else if(type < 0)
            {
                HP = MAX_HP = DataBase.EnemyUnitMAX_HP[(int)type + 5];
                LP = MAX_LP = DataBase.EnemyUnitMAX_LP[(int)type + 5];
                Strength = DataBase.EnemyUnitStrength[(int)type + 5];
                unitName = DataBase.EnemyUnitName[(int)type + 5];
                movePower = moveRange = DataBase.EnemyUnitMoveRange[(int)type + 5];
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
            if (Strength < 0) Strength *= -1;
        }
        #endregion
    }// Unit end
}// namespace CommonPart End
