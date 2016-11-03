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
            arrangeBox = new ArrangeBox();
            productBox = new ProductBox(ref arrangeBox);

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
        public override void Draw(Drawing d)
        {
            base.Draw(d);

            new TextAndFont("ユニット生産", productBox.showing ? Color.White : Color.Black).Draw(d, windowPosition + new Vector(10, 35), DepthID.Message);

            new TextAndFont("ユニット配置", arrangeBox.showing ? Color.White : Color.Black).Draw(d, windowPosition + new Vector(154, 35), DepthID.Message);

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
        const int pageMax = 5;
        int pagen = 0;
        int Page {
            get {
                return pagen;
            }
            set {
                pagen = Math.Max(0, Math.Min(Math.Max((productQ.Count + pageMax - 1) / pageMax - 1, 0), value));
            }
        }
        public Button create, stop;
        public BlindButton prePage, nexPage;
        public List<UnitType> productQ;
        public List<int> maxPP;
        public List<int> PP;
        FilledBox fb;
        BoxFrame bf;

        ArrangeBox ab;
        #endregion
        #region Method
        public ProductBox(ref ArrangeBox _ab)
            : base(DataBase.BarPos[5], DataBase.BarWidth[5], DataBase.BarHeight[5]) {
            create = new Button(new Vector(1036, 360), 200, new Color(255, 162, 0), new Color(200, 120, 0), "新規作成");
            stop = new Button(new Vector(1036, 410), 200, new Color(255, 162, 0), new Color(200, 120, 0), "生産中止");

            prePage = new BlindButton(windowPosition + new Vector2(9, 9), new Vector2(15, 250));
            nexPage = new BlindButton(windowPosition + new Vector2(264, 9), new Vector2(15, 250));

            fb = new FilledBox(new Vector(270, 250), Color.White);
            bf = new BoxFrame(new Vector(238, 50), Color.Black);
            productQ = new List<UnitType>();
            maxPP = new List<int>();
            PP = new List<int>();

            ab = _ab;
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
        public void UpdateTurn()
        {
            List<UnitType> tmpQ = new List<UnitType>();
            tmpQ.AddRange(productQ);
            List<int> tmpmaxPP = new List<int>();
            tmpmaxPP.AddRange(maxPP);
            List<int> tmpPP = new List<int>();
            tmpPP.AddRange(PP);

            productQ.Clear();
            maxPP.Clear();
            PP.Clear();

            PlayScene.productPower = PlayScene.maxProductPower;
            for (int i = 0;i < tmpQ.Count; i++)
            {
                if(tmpmaxPP[i] <= tmpPP[i])
                {
                    ab.Add(tmpQ[i]);
                }
                else
                {
                    productQ.Add(tmpQ[i]);
                    maxPP.Add(tmpmaxPP[i] - tmpPP[i]);

                    int pro = Math.Min(PlayScene.productPower, Math.Min(DataBase.maxProductPower[(int)tmpQ[i] - 1], tmpmaxPP[i] - tmpPP[i]));
                    PP.Add(pro);
                    PlayScene.productPower -= pro;
                }
            }
            Page = 0;
        }
        public void Add(UnitType ut)
        {
            productQ.Add(ut);
            maxPP.Add(DataBase.sumProductPower[(int)ut - 1]);

            int pro = Math.Min(PlayScene.productPower, DataBase.maxProductPower[(int)ut - 1]);
            PP.Add(pro);
            PlayScene.productPower -= pro;
        }
        public void Update(MouseState pstate, MouseState state, SceneManager s, ProductArrangeBar pab)
        {
            base.Update();
            if (!showing) return;
            create.Update(pstate, state);
            if (select != -1)
                stop.Update(pstate, state);

            if (Page > 0)
                prePage.Update(pstate, state);
            else
                prePage.Update(state, state, false);
            if (Page < Math.Max((productQ.Count + pageMax - 1) / pageMax - 1, 0))
                nexPage.Update(pstate, state);
            else
                nexPage.Update(state, state, false);

            if (create.Clicked())
            {
                new ProductScene(s, pab);
            }
            if (stop.Clicked() && select != -1)
            {
                if(PP[select] != 0)
                {
                    PlayScene.productPower += PP[select];
                    for(int i = select + 1; i < productQ.Count && PlayScene.productPower != 0; i++)
                    {
                        int pro = Math.Min(PlayScene.productPower, DataBase.maxProductPower[(int)productQ[i] - 1] - PP[i]);
                        PlayScene.productPower -= pro;
                        PP[i] += pro;
                    }
                }

                productQ.RemoveAt(select);
                PP.RemoveAt(select);
                maxPP.RemoveAt(select);

                if (select >= productQ.Count)
                {
                    select = -1;
                }
                Page = Page;
            }
            if (prePage.Clicked())
            {
                Page--;
                select = -1;
            }
            else if (nexPage.Clicked())
            {
                Page++;
                select = -1;
            }
            if (pstate.LeftButton == ButtonState.Released && state.LeftButton == ButtonState.Pressed)
            {
                for (int i = Page * pageMax; i < Math.Min(productQ.Count, (Page + 1) * pageMax); i++)
                {
                    if (state.X >= windowPosition.X + 25 && state.X <= windowPosition.X + 260 && state.Y >= windowPosition.Y + 10 + 50 * (i - Page * pageMax) && state.Y <= windowPosition.Y + 60 + 50 * (i - Page * pageMax))
                    {
                        select = i;
                        return;
                    }
                }
            }

            if (showing)
            {
                for (int i = Page * pageMax; i < Math.Min(productQ.Count, (Page + 1) * pageMax); i++)
                {
                    if (state.X >= windowPosition.X + 25 && state.X <= windowPosition.X + 260 && state.Y >= windowPosition.Y + 15 + 50 * (i - Page * pageMax) && state.Y <= windowPosition.Y + 65 + 50 * (i - Page * pageMax))
                    {
                        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Hand;
                    }
                }
            }
        }
        public override void Draw(Drawing d)
        {
            if (showing)
            {
                base.Draw(d);
                for(int i = Page * pageMax; i < Math.Min(productQ.Count, (Page + 1) * pageMax); i++)
                {
                    string name = productQ[i] > 0 ? DataBase.MyUnitName[(int)productQ[i] - 1] : DataBase.EnemyUnitName[(int)productQ[i] + 5];
                    new TextAndFont(name , Color.Black).Draw(d, windowPosition + new Vector(25, 10 + 50 * (i - Page * pageMax)), DepthID.Message);
                    new TextAndFont(string.Format("{0}/{1}", PP[i], DataBase.maxProductPower[(int)productQ[i] - 1]), Color.Black).Draw(d, windowPosition + new Vector(35, 35 + 50 * (i - Page * pageMax)), DepthID.Message);
                    if (PP[i] != 0)
                        new TextAndFont(string.Format("あと{0}ターン", (maxPP[i] + PP[i] - 1) / PP[i]), Color.Black).Draw(d, windowPosition + new Vector(100, 35 + 50 * (i - Page * pageMax)), DepthID.Message);
                    else
                        new TextAndFont("   停止中", Color.Black).Draw(d, windowPosition + new Vector(100, 35 + 50 * (i - Page * pageMax)), DepthID.Message);
                }
                if(select != -1)
                {
                    bf.Draw(d, windowPosition + new Vector(25, 10 + 50 * (select - Page * pageMax)), DepthID.Status);
                }
                if (Page > 0)
                    d.Draw(windowPosition + new Vector(9, 9), DataBase.productButton[0], DepthID.Status);
                if (Page < Math.Max((productQ.Count + pageMax - 1) / pageMax - 1, 0))
                    d.Draw(windowPosition + new Vector(264, 9), DataBase.productButton[1], DepthID.Status);
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
        const int pageMax = 14;
        int pagen = 0;
        int Page {
            get {
                return pagen;
            }
            set {
                pagen = Math.Max(0, Math.Min(Math.Max((arrange.Count + pageMax - 1) / pageMax - 1, 0), value));
            }
        }
        BlindButton prePage, nexPage;
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
            bf = new BoxFrame(new Vector(238, 25), Color.Black);

            prePage = new BlindButton(windowPosition + new Vector(9, 9), new Vector2(15, 350));
            nexPage = new BlindButton(windowPosition + new Vector(264, 9), new Vector2(15, 350));
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
            Page = 0;
        }
        public override bool IsOn(int x, int y)
        {
            return showing && base.IsOn(x, y);
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

            prePage.Update(pstate, state, Page > 0);
            nexPage.Update(pstate, state, Page < Math.Max((arrange.Count + pageMax - 1) / pageMax - 1, 0));

            if (prePage.Clicked())
            {
                Page--;
                select = -1;
                um.producing = UnitType.NULL;
            }
            if (nexPage.Clicked())
            {
                Page++;
                select = -1;
                um.producing = UnitType.NULL;
            }

            if (!showing & um.producing != UnitType.NULL) um.CancelProducing();

            if (pstate.LeftButton == ButtonState.Released && state.LeftButton == ButtonState.Pressed && showing)
            {
                if (IsOn(state.X, state.Y))
                {
                    for (int i = Page * pageMax; i < Math.Min(arrange.Count, (Page + 1) * pageMax); i++)
                    {
                        if (state.X >= windowPosition.X + 24 && state.X <= windowPosition.X + 264
                            && state.Y >= windowPosition.Y + 9 + 25 * (i - Page * pageMax) && state.Y <= windowPosition.Y + 34 + 25 * (i - Page * pageMax))
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
                            Page = Page;
                        }
                        else if (um.GetType(p.i, p.j) == UnitType.NULL)
                        {
                            um.producing = UnitType.NULL;
                            Hide();
                        }
                    }
                }
            }
            if (showing)
            {
                for (int i = Page * pageMax; i < Math.Min(arrange.Count, (Page + 1) * pageMax); i++)
                {
                    if (state.X >= windowPosition.X + 24 && state.X <= windowPosition.X + 264
                        && state.Y >= windowPosition.Y + 9 + 25 * (i - Page * pageMax) && state.Y <= windowPosition.Y + 34 + 25 * (i - Page * pageMax))
                    {
                        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Hand;
                    }
                }
            }
        }
        public override void Draw(Drawing d)
        {
            if (showing)
            {
                base.Draw(d);
                for(int i = Page * pageMax; i < Math.Min(arrange.Count, (Page + 1) * pageMax); i++)
                {
                    new TextAndFont(TypeName(arrange[i]), Color.Black).Draw(d, windowPosition + new Vector(29, 9 + 25 * (i - Page * pageMax)), DepthID.Map);
                }
                fb.Draw(d, windowPosition + new Vector(9, 9), DepthID.Effect);
                if(select != -1)
                    bf.Draw(d, windowPosition + new Vector(25, 9 + 25 * (select - Page * pageMax)), DepthID.Status);
                if (Page > 0)
                    d.Draw(windowPosition + new Vector(9, 9), DataBase.arrangeButton[0], DepthID.Status);
                if (Page < Math.Max((arrange.Count + pageMax - 1) / pageMax - 1, 0))
                    d.Draw(windowPosition + new Vector(264, 9), DataBase.arrangeButton[1], DepthID.Status);
            }
        }
        #endregion
    }// class end
}// namespace end
