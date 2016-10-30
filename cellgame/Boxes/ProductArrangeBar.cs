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
        public ProductBox productBox;
        public ArrangeBox arrangeBox;

        public BlindButton productButton;
        public BlindButton arrangeButton;
        #endregion
        #region Method
        public ProductArrangeBar()
            : base(DataBase.BarPos[4], DataBase.BarWidth[4], DataBase.BarHeight[4]) {
            productBox = new ProductBox();
            arrangeBox = new ArrangeBox();

            productButton = new BlindButton(windowPosition, new Vector(144, 96));
            arrangeButton = new BlindButton(windowPosition + new Vector(144, 0), new Vector(144, 96));
        }
        public void Update(MouseState pstate, MouseState state, UnitManager um, PlayScene ps, SceneManager s)
        {
            base.Update();
            productButton.Update(pstate, state);
            arrangeButton.Update(pstate, state);
            if (productButton.Clicked())
            {
                if (productBox.showing)
                {
                    productBox.Hide();
                }
                else
                {
                    productBox.Show();
                    arrangeBox.Hide();
                }
            }
            if (arrangeButton.Clicked())
            {
                if (arrangeBox.showing)
                {
                    arrangeBox.Hide();
                }
                else
                {
                    arrangeBox.Show();
                    productBox.Hide();
                }
            }
            if(pstate.LeftButton == ButtonState.Released && state.LeftButton == ButtonState.Pressed && !IsOn(state.X, state.Y))
            {
                productBox.Hide();
                if(um.producing == UnitType.NULL) arrangeBox.Hide();
            }
            productBox.Update(pstate, state, s, this);
            arrangeBox.Update(pstate, state, um, ps);
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
        public int select = -1;
        public Button create, stop;
        public List<UnitType> productQ;
        public List<int> maxPP;
        public List<int> PP;
        FilledBox fb;
        BoxFrame bf;
        #endregion
        #region Method
        public ProductBox()
            : base(DataBase.BarPos[5], DataBase.BarWidth[5], DataBase.BarHeight[5]) {
            create = new Button(new Vector(1036, 360), 200, new Color(255, 162, 0), new Color(200, 120, 0), "新規作成");
            stop = new Button(new Vector(1036, 410), 200, new Color(255, 162, 0), new Color(200, 120, 0), "生産中止");
            fb = new FilledBox(new Vector(270, 250), Color.White);
            bf = new BoxFrame(new Vector(240, 25), Color.Black);
            productQ = new List<UnitType>();
            maxPP = new List<int>();
            PP = new List<int>();
        }
        public void Show()
        {
            showing = true;
            select = -1;
        }
        public void Hide()
        {
            showing = false;
            select = -1;
        }
        public override bool IsOn(int x, int y)
        {
            return showing && base.IsOn(x, y);
        }
        public override bool IsOnButton(int x, int y)
        {
            if (showing)
            {
                for (int i = 0; i < Math.Min(productQ.Count, 9); i++)
                {
                    UnitType ut = productQ[i];
                    int nl = (ut > 0 ? DataBase.MyUnitName[(int)ut - 1] : DataBase.EnemyUnitName[(int)ut + 5]).Length;
                    if (x >= windowPosition.X + 15 && x <= windowPosition.X + 15 + nl * 20 && y >= windowPosition.Y + 15 + 25 * i && y <= windowPosition.Y + 40 + 25 * i)
                    {
                        return true;
                    }
                }
                if(create.IsOn(Mouse.GetState()) || stop.IsOn(Mouse.GetState()))
                {
                    return true;
                }
            }
            return false;
        }
        public void UpdateTurn()
        {

        }
        public void Add(UnitType ut)
        {
            if(ut > 0 && (int)ut < 8)
            {
                productQ.Add(ut);
                maxPP.Add(DataBase.maxProductPower[(int)ut - 1]);
                PP.Add(Math.Min(PlayScene.productPower, DataBase.maxProductPower[(int)ut - 1]));
            }
        }
        public void Update(MouseState pstate, MouseState state, SceneManager s, ProductArrangeBar pab)
        {
            base.Update();
            if (!showing) return;
            create.Update(pstate, state);
            stop.Update(pstate, state);
            if (create.Clicked())
            {
                new ProductScene(s, pab);
            }
            if (stop.Clicked() && select != -1)
            {
                PP[select] = 0;
            }
            if (pstate.LeftButton == ButtonState.Released && state.LeftButton == ButtonState.Pressed)
            {
                for (int i = 0; i < Math.Min(productQ.Count, 9); i++)
                {
                    UnitType ut = productQ[i];
                    int nl = (ut > 0 ? DataBase.MyUnitName[(int)ut - 1] : DataBase.EnemyUnitName[(int)ut + 5]).Length;
                    if (state.X >= windowPosition.X + 15 && state.X <= windowPosition.X + 15 + nl * 20 && state.Y >= windowPosition.Y + 15 + 25 * i && state.Y <= windowPosition.Y + 40 + 25 * i)
                    {
                        select = i;
                        return;
                    }
                }
            }
        }
        public override void Draw(Drawing d)
        {
            if (showing)
            {
                base.Draw(d);
                for(int i = 0; i < Math.Min(productQ.Count, 9); i++)
                {
                    string name = productQ[i] > 0 ? DataBase.MyUnitName[(int)productQ[i] - 1] : DataBase.EnemyUnitName[(int)productQ[i] + 5];
                    new TextAndFont(name , Color.Black).Draw(d, windowPosition + new Vector(15, 15 + 25 * i), DepthID.Message);
                }
                if(select != -1)
                {
                    bf.Draw(d, windowPosition + new Vector(15, 15 + 25 * select), DepthID.Status);
                }
                create.Draw(d);
                stop.Draw(d);
                fb.Draw(d, windowPosition + new Vector(9, 9), DepthID.StateFront);
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
        FilledBox fb;
        BoxFrame bf;
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
            fb = new FilledBox(new Vector(270, 350), Color.White);
            bf = new BoxFrame(new Vector(260, 25), Color.Black);
        }
        public void Show()
        {
            showing = true;
            select = -1;
        }
        public void Hide()
        {
            showing = false;
            select = -1;
        }
        public override bool IsOn(int x, int y)
        {
            return showing && base.IsOn(x, y);
        }
        public override bool IsOnButton(int x, int y)
        {
            if (showing)
            {
                for (int i = 0; i < Math.Min(arrange.Count, 14); i++)
                {
                    if (x >= windowPosition.X + 14 && x <= windowPosition.X + 14 + TypeName(arrange[i]).Length * 20 && y >= windowPosition.Y + 14 + 25 * i && y <= windowPosition.Y + 39 + 25 * i)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void Add(UnitType ut)
        {
            if (ut != UnitType.NULL)
            {
                arrange.Add(ut);
            }
        }
        public void Update(MouseState pstate, MouseState state, UnitManager um, PlayScene ps)
        {
            base.Update();

            if (!showing & um.producing != UnitType.NULL) um.CancelProducing();

            if (pstate.LeftButton == ButtonState.Released && state.LeftButton == ButtonState.Pressed && showing)
            {
                if (IsOn(state.X, state.Y))
                {
                    for (int i = 0; i < Math.Min(arrange.Count, 14); i++)
                    {
                        if (state.X >= windowPosition.X + 14 && state.X <= windowPosition.X + 14 + TypeName(arrange[i]).Length * 20 && state.Y >= windowPosition.Y + 14 + 25 * i && state.Y <= windowPosition.Y + 39 + 25 * i)
                        {
                            um.StartProducing(arrange[i]);
                            select = i;
                        }
                    }
                }
                else if (um.producing != UnitType.NULL)
                {
                    PAIR p = ps.WhichHex(state.X, state.Y);
                    if(p.i >= 0 && p.j >= 0)
                    {
                        if(um.Produce(p.i, p.j))
                        {
                            arrange.RemoveAt(select);
                            select = -1;
                        }
                        else if (um.GetType(p.i, p.j) == UnitType.NULL)
                        {
                            um.producing = UnitType.NULL;
                            Hide();
                        }
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
                    new TextAndFont(TypeName(arrange[i]), Color.Black).Draw(d, windowPosition + new Vector(14, 14 + 25 * i), DepthID.Map);
                }
                fb.Draw(d, windowPosition + new Vector(9, 9), DepthID.Effect);
                if(select != -1)
                    bf.Draw(d, windowPosition + new Vector(14, 14 + 25 * select), DepthID.Status);
            }
        }
        #endregion
    }// class end
}// namespace end
