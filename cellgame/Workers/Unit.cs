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
        public int x_index { get; private set; }
        public int y_index { get; private set; }
        protected int HP;
        protected int LP;
        protected int EXP;
        protected readonly int MAX_HP;
        protected readonly int MAX_LP;
        protected readonly int MAX_EXP;
        #endregion
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Unit(int _x_index, int _y_index, int _MAX_HP, int _MAX_LP, int _MAX_EXP)
        {
            x_index = _x_index;
            y_index = _y_index;
            HP = MAX_HP = _MAX_HP;
            LP = MAX_LP = _MAX_LP;
            EXP = MAX_EXP = _MAX_EXP;
        }
        #region Method
        public void MoveTo(int x_index2,int y_index2)
        {
            x_index = x_index2;
            y_index = y_index2;

        }
        #endregion
    }// Unit end
}// namespace CommonPart End
