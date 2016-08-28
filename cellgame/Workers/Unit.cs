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
        private int x_index;
        private int y_index;
        protected int HP;
        protected int LP;
        protected int EXP;
        protected readonly int MAX_HP;
        protected readonly int MAX_LP;
        protected readonly int MAX_EXP;
        #endregion
        /*
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Unit(int x_index, int y_index,int hp)
        {
            this.x_index = x_index;
            this.y_index = y_index;
            this.unit_type = unit_type;
            this.hp = hp;
        }
        public Unit(int x_index, int y_index, UnitType unit_type) :this(x_index, y_index,unit_type, unit_type.maxhp)
        {        }
        */
        #region Method
        public void MoveTo(int x_index2,int y_index2)
        {
            x_index = x_index2;
            y_index = y_index2;

        }
        #endregion
    }// Unit end
}// namespace CommonPart End
