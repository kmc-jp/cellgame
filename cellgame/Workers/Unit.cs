using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonPart;

namespace CommonPart
{
    abstract class Unit
    {
        #region Variable
        public int x_index { get; private set; }
        public int y_index { get; private set; }
        protected readonly int type;
        public int HP { get; protected set; }
        public int LP { get; protected set; }
        public int EXP { get; protected set; }
        protected readonly int MAX_HP;
        protected readonly int MAX_LP;
        protected readonly int MAX_EXP;
        protected readonly string unitName;
        #endregion
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Unit(int _x_index, int _y_index, int _type)
        {
            x_index = _x_index;
            y_index = _y_index;
            type = _type;
            if(type >= 0)
            {
                HP = MAX_HP = DataBase.MyUnitMAX_HP[type];
                LP = MAX_LP = DataBase.MyUnitMAX_LP[type];
                EXP = MAX_EXP = DataBase.MyUnitMAX_EXP[type];
                unitName = DataBase.MyUnitName[type];
            }
            else
            {
                HP = MAX_HP = DataBase.EnemyUnitMAX_HP[type + 5];
                LP = MAX_LP = DataBase.EnemyUnitMAX_LP[type + 5];
                EXP = MAX_EXP = DataBase.EnemyUnitMAX_EXP[type + 5];
                unitName = DataBase.EnemyUnitName[type];
            }
        }
        #region Method
        protected Vector DrawPos(Vector camera, int scale)
        {
            return DataBase.WhereDisp(x_index, y_index, camera, scale) + new Vector(26 * DataBase.MapScale[scale], 36 * DataBase.MapScale[scale]);
        }
        public void Draw(Drawing d, Vector camera, int scale)
        {
            if(type >= 0)
                d.Draw(DrawPos(camera, scale), DataBase.myUnit_tex[type], DepthID.Player, (float)DataBase.MapScale[scale]);
            else
                d.Draw(DrawPos(camera, scale), DataBase.enemyUnit_tex[type + 5], DepthID.Player, (float)DataBase.MapScale[scale]);
        }
        public void MoveTo(int x_index2,int y_index2)
        {
            x_index = x_index2;
            y_index = y_index2;

        }
        #endregion
    }// Unit end
}// namespace CommonPart End
