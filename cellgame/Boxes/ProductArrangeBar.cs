using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CommonPart
{
    class ProductArrangeBar : WindowBar
    {
        #region Variable
        ProductBox productBox;
        ArrangeBox arrangeBox;
        #endregion
        #region Method
        public ProductArrangeBar()
            : base(DataBase.BarPos[4], DataBase.BarWidth[4], DataBase.BarHeight[4]) {
            productBox = new ProductBox();
            arrangeBox = new ArrangeBox();
        }
        public void Update(MouseState pstate, MouseState state, UnitManager um, PlayScene ps, Map nMap)
        {
            base.Update();
            if(pstate.LeftButton == ButtonState.Released && state.LeftButton == ButtonState.Pressed)
            {
                if (state.X >= windowPosition.X && state.X <= windowPosition.X +width / 2 * 16 && state.Y >= windowPosition.Y && state.Y <= windowPosition.Y + height * 16)
                {// 左のボタンをクリック
                    productBox.Show();
                    arrangeBox.Hide();
                }
                else if(state.X >= windowPosition.X + width / 2 * 16 && state.X <= windowPosition.X + width * 16 && state.Y >= windowPosition.Y && state.Y <= windowPosition.Y + height * 16)
                {// 右のボタンをクリック
                    arrangeBox.Show();
                    productBox.Hide();
                }
                else if (!IsOn(state.X, state.Y))
                {
                    productBox.Hide();
                    if(um.producing == UnitType.NULL) arrangeBox.Hide();
                }
            }
            arrangeBox.Update(pstate, state, um, productBox.Update(pstate, state), ps, nMap);
        }
        public override bool IsOn(int x, int y)
        {
            return base.IsOn(x, y) || productBox.IsOn(x, y) || arrangeBox.IsOn(x, y);
        }
        public override bool IsOnButton(int x, int y)
        {
            return base.IsOn(x, y) || productBox.IsOnButton(x, y) || arrangeBox.IsOnButton(x, y);
        }
        public override void Draw(Drawing d)
        {
            base.Draw(d);
            if(productBox.showing)
                new TextAndFont("生産ボックス", Color.White).Draw(d, windowPosition + new Vector(10, 35), DepthID.Message);
            else
                new TextAndFont("生産ボックス", Color.Black).Draw(d, windowPosition + new Vector(10, 35), DepthID.Message);
            if(arrangeBox.showing)
                new TextAndFont("配置ボックス", Color.White).Draw(d, windowPosition + new Vector(154, 35), DepthID.Message);
            else
                new TextAndFont("配置ボックス", Color.Black).Draw(d, windowPosition + new Vector(154, 35), DepthID.Message);
            new FilledBox(new Vector2(4, 96), Color.Black).Draw(d, windowPosition + new Vector(142, 0), DepthID.Message);
            productBox.Draw(d);
            arrangeBox.Draw(d);
        }
        #endregion
    }// class end

    
    class ProductBox : WindowBar
    {
        #region Variable
        public bool showing = false;
        #endregion
        #region Method
        public ProductBox()
            : base(DataBase.BarPos[5], DataBase.BarWidth[5], DataBase.BarHeight[5]) {
        }
        public void Show()
        {
            showing = true;
        }
        public void Hide()
        {
            showing = false;
        }
        public bool IsOn(int x, int y)
        {
            return showing && base.IsOn(x, y);
        }
        public bool IsOnButton(int x, int y)
        {
            if (showing)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (x >= windowPosition.X + 10 && x <= windowPosition.X + 10 + DataBase.MyUnitName[i].Length * 20 && y >= windowPosition.Y + 10 + 25 * i && y <= windowPosition.Y + 35 + 25 * i)
                    {
                        return true;
                    }
                }
                for (int i = 0; i < 5; i++)
                {
                    if (x >= windowPosition.X + 10 && x <= windowPosition.X + 10 + DataBase.EnemyUnitName[i].Length * 20 && y >= windowPosition.Y + 235 + 25 * i && y <= windowPosition.Y + 260 + 25 * i)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public UnitType Update(MouseState pstate, MouseState state)
        {
            base.Update();
            if (pstate.LeftButton == ButtonState.Released && state.LeftButton == ButtonState.Pressed && showing)
            {
                for(int i = 0; i < 9; i++)
                {
                    if (state.X >= windowPosition.X + 10 && state.X <= windowPosition.X + 10 + DataBase.MyUnitName[i].Length * 20 && state.Y >= windowPosition.Y + 10 + 25 * i && state.Y <= windowPosition.Y + 35 + 25 * i)
                    {
                        return (UnitType)(i + 1);
                    }
                }
                for (int i = 0; i < 5; i++)
                {
                    if (state.X >= windowPosition.X + 10 && state.X <= windowPosition.X + 10 + DataBase.EnemyUnitName[i].Length * 20 && state.Y >= windowPosition.Y + 235 + 25 * i && state.Y <= windowPosition.Y + 260 + 25 * i)
                    {
                        return (UnitType)(i - 5);
                    }
                }
            }
            return UnitType.NULL;
        }
        public override void Draw(Drawing d)
        {
            if (showing)
            {
                base.Draw(d);
                for(int i = 0; i < 9; i++)
                {
                    new TextAndFont(DataBase.MyUnitName[i], Color.Black).Draw(d, windowPosition + new Vector(10, 10 + 25 * i), DepthID.Message);
                }
                for (int i = 0; i < 5; i++)
                {
                    new TextAndFont(DataBase.EnemyUnitName[i], Color.Black).Draw(d, windowPosition + new Vector(10, 235 + 25 * i), DepthID.Message);
                }
            }
        }
        #endregion
    }// class end

    class ArrangeBox : WindowBar
    {
        #region Variable
        public bool showing = false;
        int select = -1;
        List<UnitType> arrange;
        #endregion
        #region Method
        string TypeName(UnitType tp)
        {
            if(tp > 0)
            {
                return DataBase.MyUnitName[(int)tp - 1];
            }
            else if(tp < 0)
            {
                return DataBase.EnemyUnitName[(int)tp + 5];
            }
            else
            {
                return "";
            }
        }
        public ArrangeBox()
            : base(DataBase.BarPos[5], DataBase.BarWidth[5], DataBase.BarHeight[5])
        {
            arrange = new List<UnitType>();
        }
        public void Show()
        {
            showing = true;
        }
        public void Hide()
        {
            showing = false;
        }
        public bool IsOn(int x, int y)
        {
            return showing && base.IsOn(x, y);
        }
        public bool IsOnButton(int x, int y)
        {
            if (showing)
            {
                for (int i = 0; i < Math.Min(arrange.Count, 14); i++)
                {
                    if (x >= windowPosition.X + 10 && x <= windowPosition.X + 10 + TypeName(arrange[i]).Length * 20 && y >= windowPosition.Y + 10 + 25 * i && y <= windowPosition.Y + 35 + 25 * i)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void Update(MouseState pstate, MouseState state, UnitManager um, UnitType ut, PlayScene ps, Map nMap)
        {
            base.Update();
            if(ut != UnitType.NULL)
            {
                arrange.Add(ut);
            }

            if (pstate.LeftButton == ButtonState.Released && state.LeftButton == ButtonState.Pressed && showing)
            {
                if (IsOn(state.X, state.Y))
                {
                    for (int i = 0; i < Math.Min(arrange.Count, 14); i++)
                    {
                        if (state.X >= windowPosition.X + 10 && state.X <= windowPosition.X + 10 + TypeName(arrange[i]).Length * 20 && state.Y >= windowPosition.Y + 10 + 25 * i && state.Y <= windowPosition.Y + 35 + 25 * i)
                        {
                            um.StartProducing(arrange[i], nMap);
                            select = i;
                        }
                    }
                }
                else if (um.producing != UnitType.NULL)
                {
                    PAIR p = ps.WhichHex(state.X, state.Y);
                    if(p.i >= 0 && p.j >= 0 && um.Produce(p.i, p.j))
                    {
                        arrange.RemoveAt(select);
                        select = -1;
                    }
                }
            }
        }
        public override void Draw(Drawing d)
        {
            if (showing)
            {
                base.Draw(d);
                for(int i = 0; i < Math.Min(arrange.Count, 14); i++)
                {
                    new TextAndFont(TypeName(arrange[i]), Color.Black).Draw(d, windowPosition + new Vector(10, 10 + 25 * i), DepthID.Message);
                }
            }
        }
        #endregion
    }// class end
}// namespace end
