using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using Tao.FreeGlut;
using Tao.Platform.Windows;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using NetworkDesign.NetworkElements;
using NetworkDesign.Main;
using System.Diagnostics;
using Tao.OpenGl;
using Tao.DevIl;
using System.DirectoryServices.AccountManagement;
using System.Drawing.Imaging;

namespace NetworkDesign
{
    public partial class MainForm : Form
    {
        #region Объявление переменных
        public static UserPrincipal user;
        public static bool edit;
        SizeRenderingArea DefaultSettings = new SizeRenderingArea("DefaultMap", 1000, 1000);
        static public Map MyMap = new Map();
        static public DrawLevel drawLevel;
        static public ColorSettings colorSettings = new ColorSettings();
        static public Parametrs parametrs = new Parametrs();
        //
        static public List<string> ImagesURL = new List<string>();
        static public List<uint> Textures = new List<uint>();
        static public List<uint> MTTextures = new List<uint>();
        static public List<string> DeleteImages = new List<string>();
        static public ImageList Images = new ImageList();
        static public bool isLoad = false;
        //
        static public int _Height = 0, _Width = 0;
        static public SimpleOpenGlControl AnT = new SimpleOpenGlControl();
        static public ActiveElem activeElem = new ActiveElem();
        private List<string> floors_name = new List<string>();
        private int floor_index = 0;
        //
        public static float font = 14;
        public static double zoom = 1;

        static public bool isInit = false;

        Stopwatch stopwatch = new Stopwatch();
        
        public static int nebutnscount = 15;
        static public List<NEButton> neButtons = new List<NEButton>();

        static public TextBox focusbox = new TextBox();

        static public Filtres filtres = new Filtres();

        private Element MoveElem;
        private Element _MoveElem;
        private bool isMoveLog = false;

        Point asp = new Point();
        #endregion
        /// <summary>
        /// Инициализация начальных параметров
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            StartPosition = FormStartPosition.CenterScreen;
            user = Autorisation(out edit);
            AnT.Parent = panel1;
            //panel1.Controls.Add(AnT);
            // AnT
            // 
            AnT.AccumBits = ((byte)(0));
            AnT.AutoCheckErrors = false;
            AnT.AutoFinish = false;
            AnT.AutoMakeCurrent = true;
            //AnT.AutoSize = true;
            AnT.AutoSwapBuffers = true;
            AnT.BackColor = Color.Black;
            AnT.ColorBits = ((byte)(32));
            AnT.Cursor = Cursors.Cross;
            AnT.DepthBits = ((byte)(16));
            AnT.Location = new Point(3, 3);
            AnT.Name = "AnT";
            AnT.Size = new Size(1000, 1000);
            //AnT.AutoScroll = true;
            AnT.StencilBits = ((byte)(0));
            AnT.TabIndex = 1;
            //AnT.AutoScroll = true;
            AnT.MouseDown += new MouseEventHandler(AnT_MouseDown);
            AnT.MouseMove += new MouseEventHandler(AnT_MouseMove);
            AnT.MouseUp += new MouseEventHandler(AnT_MouseUp);
            AnT.Click += AnT_Click;
            AnT.MouseDoubleClick += AnT_MouseDoubleClick;
            // 
            AnT.InitializeContexts();
            MyMap = new Map(DefaultSettings);
            Text = DefaultSettings.Name;
            drawLevel.Level = -1;
            drawLevel.Floor = -1;
            _Height = AnT.Height;
            _Width = AnT.Width;
            panel1.Parent = this;
            panel2.Parent = this;
            panel2.BringToFront();
            trackBar1.Parent = this;
            trackBar1.BringToFront();
            panel2.BackColor = Color.White;
            FloorUP.Parent = this;
            FloorDown.Parent = this;
            FloorUP.BringToFront();
            FloorDown.BringToFront();
            FloorUP.BackColor = Color.White;
            FloorDown.BackColor = Color.White;
            FloorUP.FlatAppearance.BorderSize = 0;
            FloorUP.FlatStyle = FlatStyle.Flat;
            FloorDown.FlatAppearance.BorderSize = 0;
            FloorDown.FlatStyle = FlatStyle.Flat;
            colorSettings = ColorSettings.Open();
            parametrs = Parametrs.Open();
            ImagesURL = OpenTextures();
            LoadImages();
            ID_TEXT temp = NEButton.Open();
            for (int i = 0; i < temp.TEXT.Count; i++)
            {
                int id = ImagesURL.IndexOf(temp.TEXT[i]);
                if (id != -1)
                {
                    neButtons.Add(new NEButton(new ToolStripButton(Images.Images[id]), id, ImagesURL[id], toolStrip1.Items.Count));
                    toolStrip1.Items.Add(neButtons.Last().toolStripButton);
                }
            }
            filtres = new Filtres(true, true, true, true, true, true, true, true, true, true);
            focusbox.TabIndex = 99;
            focusbox.Parent = this;
            focusbox.Visible = true;
            focusbox.Enabled = true;
            focusbox.Location = new Point(5000, 5000);
            Click += MainForm_Click;
            //panel1.AutoScroll = true;
            panel1.AutoScrollPosition = new Point(AnT.Height / 2, AnT.Width / 2);
            Timer time = new Timer
            {
                Interval = 15
            };
            time.Start();
            time.Tick += Time_Tick;
        }
        #region Обработка кликов мыши для различных инструментов
        private void MouseLines(int x, int y)
        {
            //var value = panel1.VerticalScroll.Value;
            if (!MyMap.Lines.step)
            {
                MyMap.Lines.step = true;
                MyMap.Lines.TempLine = new Line(x, y, drawLevel);
            }
            else
            {
                MyMap.Lines.step = false;
                MyMap.Lines.Add(MyMap.Lines.TempLine);
                MyMap.Lines.TempLine = new Line();
                int lastindex = MyMap.Lines.Lines.Count - 1;
                Element elem = new Element(1, lastindex, MyMap.Lines.Lines[lastindex].Clone(), -1);
                Element _elem = new Element(1, lastindex, new Line(), -1);
                MyMap.log.Add(new LogMessage("Добавил линию", elem, _elem));
                InfoLable.Text = "Добавил линию";
                CheckButtons(true);
            }
        }

        private void MouseRects(int x, int y)
        {
            if (MyMap.Rectangles.step_rect == 0)
            {
                MyMap.Rectangles.TempRectangle = new MyRectangle(x, y, drawLevel);
                MyMap.Rectangles.step_rect = 1;
            }
            else if (MyMap.Rectangles.step_rect == 1)
            {
                MyMap.Rectangles.TempRectangle.SetPoint(x, y, 2);
                MyMap.Rectangles.step_rect = 2;
            }
            else if (MyMap.Rectangles.step_rect == 2)
            {
                MyMap.Rectangles.Add(MyMap.Rectangles.TempRectangle);
                MyMap.Rectangles.step_rect = 0;
                MyMap.Rectangles.TempRectangle = new MyRectangle();
                int lastindex = MyMap.Rectangles.Rectangles.Count - 1;
                Element elem = new Element(2, lastindex, MyMap.Rectangles.Rectangles[lastindex].Clone(), -1);
                Element _elem = new Element(2, lastindex, new MyRectangle(), -1);
                MyMap.log.Add(new LogMessage("Добавил прямоугольник", elem, _elem));
                InfoLable.Text = "Добавил прямоугольник";
                CheckButtons(true);
            }
        }

        private void MouseCircle(int x, int y)
        {
            if (!MyMap.Circles.step)
            {
                MyMap.Circles.step = true;
                MyMap.Circles.TempCircle = new Circle(x, y, drawLevel);
            }
            else
            {
                MyMap.Circles.step = false;
                MyMap.Circles.Add(MyMap.Circles.TempCircle);
                MyMap.Circles.TempCircle = new Circle();
                int lastindex = MyMap.Circles.Circles.Count - 1;
                Element elem = new Element(360, lastindex, MyMap.Circles.Circles[lastindex].Clone(), -1);
                Element _elem = new Element(360, lastindex, new Circle(), -1);
                MyMap.log.Add(new LogMessage("Добавил круг", elem, _elem));
                InfoLable.Text = "Добавил круг";
                CheckButtons(true);
            }
        }

        private void MouseNE(int x, int y)
        {
            if (!MyMap.NetworkElements.step)
            {
                ImageTextures IT = new ImageTextures(ref MyMap.NetworkElements);
                IT.ShowDialog();
                if (IT.action == 0)
                {
                    MyMap.NetworkElements.step = true;
                    MyMap.NetworkElements.TempNetworkElement = new NetworkElement(new Texture(false, 50, new Point(x, y), IT.imageindex), drawLevel);
                }
            }
            else
            {
                MyMap.NetworkElements.Add(MyMap.NetworkElements.TempNetworkElement);
                MyMap.NetworkElements.TempDefault();
                int lastindex = MyMap.NetworkElements.NetworkElements.Count - 1;
                Element elem = new Element(8, lastindex, MyMap.NetworkElements.NetworkElements[lastindex].Clone(), -1);
                Element _elem = new Element(8, lastindex, new NetworkElement(), -1);
                MyMap.log.Add(new LogMessage("Добавил сетевой элемент", elem, _elem));
                InfoLable.Text = "Добавил сетевой элемент";
                CheckButtons(true);
            }
        }

        private void MouseNW(int x, int y)
        {
            if (MyMap.NetworkWires.active)
            {
                if (!MyMap.NetworkWires.step)
                {
                    var item = MyMap.ChechNE(x, y);
                    if (item.ID != -1)
                    {
                        MyMap.NetworkWires.step = true;
                        MyMap.NetworkWires.TempNetworkWire = new NetworkWire(x, y, drawLevel, item);
                    }
                }
                else
                {
                    var item = MyMap.ChechNE(x, y);
                    if (item.ID != -1)
                    {
                        MyMap.NetworkWires.TempNetworkWire.AddPoint();
                        MyMap.NetworkWires.TempNetworkWire.ClearTempPoint();
                        MyMap.NetworkWires.TempNetworkWire.idiw2 = item;
                        MyMap.NetworkWires.Add(MyMap.NetworkWires.TempNetworkWire);
                        MyMap.NetworkWires.TempDefault();
                        int lastindex = MyMap.NetworkWires.NetworkWires.Count - 1;
                        Element elem = new Element(9, lastindex, MyMap.NetworkWires.NetworkWires[lastindex].Clone(), -1);
                        Element _elem = new Element(9, lastindex, new NetworkWire(), -1);
                        MyMap.log.Add(new LogMessage("Добавил провод", elem, _elem));
                        InfoLable.Text = "Добавил провод";
                        CheckButtons(true);
                    }
                    else
                    {
                        MyMap.NetworkWires.TempNetworkWire.AddPoint();
                    }
                }
            }
        }

        private void MousePolygon(int x, int y)
        {
            if (MyMap.Polygons.active)
            {
                if (!MyMap.Polygons.step)
                {
                    MyMap.Polygons.TempPolygon = new Polygon(x, y, drawLevel);
                    MyMap.Polygons.step = true;
                }
                else
                {
                    MyMap.Polygons.TempPolygon.AddPoint();
                }
            }
            else
            {
                MyMap.Polygons.Add(MyMap.Polygons.TempPolygon);
                MyMap.Polygons.step = false;
                int lastindex = MyMap.Polygons.Polygons.Count - 1;
                Element elem = new Element(3, lastindex, MyMap.Polygons.Polygons[lastindex].Clone(), -1);
                Element _elem = new Element(3, lastindex, new Polygon(), -1);
                MyMap.log.Add(new LogMessage("Добавил многоугольник", elem, _elem));
                InfoLable.Text = "Добавил многоугольник";
                CheckButtons(true);
            }
        }

        TextBox textBox;
        int x, y;

        private void MouseText(int x, int y)
        {
            textid = -1;
            focusbox.Focus();
            this.x = x;
            this.y = y;
            if (textBox == null || textBox.Text == "")
            {
                textBox = new TextBox
                {
                    Parent = AnT,
                    BackColor = Color.White,
                    BorderStyle = BorderStyle.None,
                    Visible = true,
                    Enabled = true,
                    Location = new Point(x, y),
                    Text = ""
                };
            }
            textBox.Focus();
            textBox.KeyDown += TextBox_KeyDown;
            textBox.LostFocus += TextBox_LostFocus;
        }

        private int textid = -1;

        private void MouseText(int id)
        {
            textid = id;
            focusbox.Focus();
            Point location = MyMap.MyTexts.MyTexts[textid].location;
            float fontsize = MyMap.MyTexts.MyTexts[textid].fontsize;
            if (textBox == null || textBox.Text == "")
            {
                textBox = new TextBox
                {
                    Parent = AnT,
                    BackColor = Color.White,
                    BorderStyle = BorderStyle.None,
                    Visible = true,
                    Enabled = true,
                    Location = new Point(, ),
                    Text = ""
                };
            }
            textBox.Focus();
            textBox.KeyDown += TextBox_KeyDown;
            textBox.LostFocus += TextBox_LostFocus;
        }

        private void TextBox_LostFocus(object sender, EventArgs e)
        {
            if (textBox.Text != "" & textBox.Text != " ")
            {
                if (textid == -1)
                {
                    MyMap.MyTexts.Add(new MyText(drawLevel, new Point(MyMap.RecalcMouseX(x), MyMap.RecalcMouseY(y)), textBox));
                    int lastindex = MyMap.MyTexts.MyTexts.Count - 1;
                    Element elem = new Element(10, lastindex, MyMap.MyTexts.MyTexts[lastindex].Clone(), -1);
                    Element _elem = new Element(10, lastindex, new MyText(), -1);
                    MyMap.log.Add(new LogMessage("Добавил надпись", elem, _elem));
                    InfoLable.Text = "Добавил надпись";
                }
                /*else
                {
                    Element elem = new Element(10, textid, MyMap.MyTexts.MyTexts[textid].Clone(), -2);
                    MyMap.MyTexts.MyTexts[textid] = new MyText(drawLevel, new Point(MyMap.RecalcMouseX(x), MyMap.RecalcMouseY(y)), textBox);
                    Element _elem = new Element(10, textid, MyMap.MyTexts.MyTexts[textid].Clone(), -2);
                    MyMap.log.Add(new LogMessage("Изменил надпись", elem, _elem));
                    InfoLable.Text = "Изменил надпись";
                }*/
                CheckButtons(true);
            }
            textBox.Dispose();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            Size size = TextRenderer.MeasureText(textBox.Text, textBox.Font);
            textBox.Width = size.Width + 10;
            textBox.Height = size.Height;
            if (e.KeyCode == Keys.Enter)
            {
                focusbox.Focus();
            }
        }

        private void MouseNEElems(int x, int y)
        {
            if (!MyMap.NetworkElements.step)
            {
                /*ImageTextures IT = new ImageTextures(ref MyMap.NetworkElements);
                IT.ShowDialog();
                if (IT.imageindex > -1)
                {*/
                    MyMap.NetworkElements.step = true;
                    MyMap.NetworkElements.TempNetworkElement = new NetworkElement(new Texture(false, 50, new Point(x, y), MyMap.RB - nebutnscount), drawLevel);
                //}
            }
            else
            {
                MyMap.NetworkElements.Add(MyMap.NetworkElements.TempNetworkElement);
                MyMap.NetworkElements.TempDefault();
                int lastindex = MyMap.NetworkElements.NetworkElements.Count - 1;
                Element elem = new Element(8, lastindex, MyMap.NetworkElements.NetworkElements[lastindex].Clone(), -1);
                Element _elem = new Element(8, lastindex, new NetworkElement(), -1);
                MyMap.log.Add(new LogMessage("Добавил сетевой элемент", elem, _elem));
                InfoLable.Text = "Добавил сетевой элемент";
                CheckButtons(true);
            }
        }

        

        #endregion
        /// <summary>
        /// Событие нажатия мыши по области отрисовки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnT_MouseDown(object sender, MouseEventArgs e)
        {
            if (panel1.AutoScrollPosition != asp)
                panel1.AutoScrollPosition = new Point(-asp.X, -asp.Y);
            int y = MyMap.RecalcMouseY(e.Y);
            int x = MyMap.RecalcMouseX(e.X);
            if (MyMap.RB >= nebutnscount & MyMap.RB != 360)
            {
                if (e.Button == MouseButtons.Left)
                    MouseNEElems(x, y);
                else if (e.Button == MouseButtons.Right)
                    MyMap.NetworkElements.TempDefault();
            }
            else
            {
                if (e.Button == MouseButtons.Left)
                {
                    switch (MyMap.RB)
                    {
                        case 0:
                            stopwatch.Restart();
                            if (!MyMap.isMove)
                            {
                                SelectItems(x, y);
                            }
                            break;
                        case 1:
                            MouseLines(x, y);
                            break;
                        case 2:
                            MouseRects(x, y);
                            break;
                        case 3:
                            //SelectItems(x, y);
                            if (MyMap.EditRects.edit_active)
                            {
                                MyMap.EditRects.edit_active = false;
                            }
                            break;
                        case 5:
                            MyMap.Polygons.active = true;
                            MousePolygon(x, y);
                            break;
                        case 360:
                            MouseCircle(x, y);
                            break;
                        case 6:
                            if (drawLevel.Level != -1)
                            {
                                if (drawLevel.Floor != 0)
                                {
                                    if (!MyMap.Buildings.Buildings[activeElem.item].InputWires.step)
                                    {
                                        MyMap.Buildings.Buildings[activeElem.item].AddIWInBuild(x, y, drawLevel);
                                    }
                                    else
                                    {
                                        MyMap.Buildings.Buildings[activeElem.item].AddIWInBuild(x, y, drawLevel);
                                        int lastindex = MyMap.Buildings.Buildings[activeElem.item].InputWires.InputWires.Circles.Count - 1;
                                        Element elem = new Element(6, lastindex, MyMap.Buildings.Buildings[activeElem.item].InputWires.InputWires.Circles[lastindex].Clone(), -1);
                                        Element _elem = new Element(6, lastindex, new Circle(), -1);
                                        MyMap.log.Add(new LogMessage("Добавил проход провода через потолок", elem, _elem, activeElem.item));
                                        InfoLable.Text = "Добавил проход провода через потолок";
                                    }
                                }
                            }
                            else
                            {
                                if (!MyMap.Buildings.Buildings[activeElem.item].InputWires.step)
                                {
                                    InputWireForm IWF = new InputWireForm(MyMap.Buildings.Buildings[activeElem.item].floors_name);
                                    IWF.ShowDialog();
                                    if (IWF.dialogResult == DialogResult.OK)
                                    {
                                        MyMap.Buildings.Buildings[activeElem.item].AddIW(x, y, IWF.side, IWF.floor_index);
                                    }
                                }
                                else
                                {
                                    MyMap.Buildings.Buildings[activeElem.item].AddIW(x, y, false, -1);
                                    int lastindex = MyMap.Buildings.Buildings[activeElem.item].InputWires.InputWires.Circles.Count - 1;
                                    Element elem = new Element(6, lastindex, MyMap.Buildings.Buildings[activeElem.item].InputWires.InputWires.Circles[lastindex].Clone(), -1);
                                    Element _elem = new Element(6, lastindex, new Circle(), -1);
                                    MyMap.log.Add(new LogMessage("Добавил вход провода в здание", elem, _elem, activeElem.item));
                                    InfoLable.Text = "Добавил вход провода в здание";
                                }
                            }
                            break;
                        case 7:
                            if (MyMap.Buildings.Buildings[activeElem.item].AddEntrance(x, y))
                            {
                                int lastindex = MyMap.Buildings.Buildings[activeElem.item].Entrances.Enterances.Circles.Count - 1;
                                Element elem = new Element(7, lastindex, MyMap.Buildings.Buildings[activeElem.item].Entrances.Enterances.Circles[lastindex].Clone(), -1);
                                Element _elem = new Element(7, lastindex, new Circle(), -1);
                                MyMap.log.Add(new LogMessage("Добавил вход в здание", elem, _elem, activeElem.item));
                                InfoLable.Text = "Добавил вход в здание";
                            }
                            break;
                        case 8:
                            MouseNE(x, y);
                            break;
                        case 9:
                            MyMap.NetworkWires.active = true;
                            MouseNW(x, y);
                            break;
                        case 10:
                            MouseText(e.X, e.Y);
                            break;
                    }
                }
                if (e.Button == MouseButtons.Right)
                {
                    switch (MyMap.RB)
                    {
                        case 0:
                            SelectItems(x, y);
                            if (activeElem.type == 4)
                            {
                                drawLevel.Level = activeElem.item;
                                drawLevel.Floor = 0;
                                ReturnToMainBtn.Enabled = true;
                                UpgrateFloors();
                                Unfocus("Выбрано здание " + activeElem.item + " '" + MyMap.Buildings.Buildings[activeElem.item].Name + "'");
                                IWBtn.Enabled = true;
                            }
                            break;
                        case 1:
                            MyMap.Lines.step = false;
                            MyMap.Lines.TempLine = new Line();
                            break;
                        case 2:
                            MyMap.Rectangles.step_rect = 0;
                            MyMap.Rectangles.TempRectangle = new MyRectangle();
                            break;
                        case 5:
                            if (MyMap.Polygons.TempPolygon.Points.Count > 2)
                            {
                                MyMap.Polygons.TempPolygon.ClearTempPoint();
                                MyMap.Polygons.active = false;
                                MousePolygon(x, y);
                            }
                            MyMap.Polygons.TempDefault();
                            break;
                        case 360:
                            MyMap.Circles.step = false;
                            MyMap.Circles.TempCircle = new Circle();
                            break;
                        case 6:
                            MyMap.Buildings.Buildings[activeElem.item].InputWires.step = false;
                            MyMap.Buildings.Buildings[activeElem.item].InputWires.InputWires.TempCircle = new Circle();
                            break;
                        case 7:
                            MyMap.Buildings.Buildings[activeElem.item].Entrances.step = false;
                            MyMap.Buildings.Buildings[activeElem.item].Entrances.Enterances.TempCircle = new Circle();
                            break;
                        case 8:
                            MyMap.NetworkElements.TempDefault();
                            break;
                        case 9:
                            if (MyMap.NetworkWires.active)
                            {
                                int index = MyMap.NetworkWires.TempNetworkWire.Points.Count;
                                if (index > 1)
                                {
                                    MyMap.NetworkWires.TempNetworkWire.Points.RemoveAt(index - 1);
                                }
                                else
                                {
                                    int id = MyMap.NetworkWires.TempNetworkWire.idiw1.ID;
                                    MyMap.NetworkElements.NetworkElements[id].Options.BusyPorts--;
                                    MyMap.NetworkWires.TempDefault();
                                }
                            }
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// Событие двойного нажатия мыши по области отрисовки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnT_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            panel1.AutoScrollPosition = new Point(-asp.X, -asp.Y);
            int difx = asp.X - panel1.AutoScrollPosition.X;
            int dify = asp.Y - panel1.AutoScrollPosition.Y;
            panel1.AutoScrollPosition = new Point(-difx, -dify);
            int y = MyMap.RecalcMouseY(e.Y);
            int x = MyMap.RecalcMouseX(e.X);
            if (e.Button == MouseButtons.Left)
            {
                switch (MyMap.RB)
                {
                    case 0:
                        //Доделать редактирование надписей
                        if (MyMap.SearchNE(x, y))
                            break;
                        if (MyMap.SearchNW(x, y))
                            break;
                        if (MyMap.SearchText(x, y, out int id))
                        {
                            MouseText(x, y, id);
                        }
                        break;
                }
            }
        }
        /// <summary>
        /// Событие окончания нажатия мыши по области отрисовки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnT_MouseUp(object sender, MouseEventArgs e)
        {
            panel1.AutoScrollPosition = new Point(-asp.X, -asp.Y);
            int difx = asp.X - panel1.AutoScrollPosition.X;
            int dify = asp.Y - panel1.AutoScrollPosition.Y;
            panel1.AutoScrollPosition = new Point(-difx, -dify);
            if (e.Button == MouseButtons.Left)
            {
                switch (MyMap.RB)
                {
                    case 0:
                        stopwatch.Stop();
                        if (MyMap.isMove)
                        {
                            if (activeElem.build != -1)
                            {
                                MyMap.Buildings.Buildings[activeElem.build].AddTemp();
                            }
                            MoveLogAdd();
                            MyMap.isMove = false;
                        }
                        break;
                    case 3:
                        MoveLogAdd();
                        MyMap.RefreshEditRect();
                        break;
                }
            }
        }
        /// <summary>
        /// Событие движения мыши по области отрисовки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnT_MouseMove(object sender, MouseEventArgs e)
        {
            //panel1.AutoScrollPosition = new Point(-asp.X, -asp.Y);
            int y = MyMap.RecalcMouseY(e.Y);
            int x = MyMap.RecalcMouseX(e.X);
            if (e.Button == MouseButtons.Left)
            {
                ChechEdges(x, y);
            }
            if (MyMap.RB >= nebutnscount & MyMap.RB != 360)
            {
                if (MyMap.NetworkElements.step)
                {
                    MyMap.NetworkElements.TempNetworkElement.SetPoint(x, y);
                }
            }
            else
            {
                switch (MyMap.RB)
                {
                    case 0:
                        if (e.Button == MouseButtons.Left & stopwatch.ElapsedMilliseconds > 500)
                        {
                            if (!MyMap.isMove & activeElem.type != -1)
                            {
                                MoveLogAdd();
                                MyMap.MoveElem(x, y);
                            }
                            else
                            {
                                MyMap.MoveElem(x, y);
                            }
                        }
                        break;
                    case 1:
                        if (MyMap.Lines.step)
                        {
                            MyMap.Lines.TempLine.SetPoint(x, y, 1);
                        }
                        break;
                    case 2:
                        if (MyMap.Rectangles.step_rect == 1)
                        {
                            MyMap.Rectangles.TempRectangle.SetPoint(x, y, 2);
                        }
                        else if (MyMap.Rectangles.step_rect == 2)
                        {
                            MyMap.Rectangles.TempRectangle.SetPoint(x, y, 34);
                        }
                        break;
                    case 3:
                        if (e.Button == MouseButtons.Left)
                        {
                            if (!MyMap.EditRects.edit_active)
                            {
                                if (MyMap.SearchEditElem(x, y, out int type))
                                {
                                    if (type == 5)
                                    {
                                        AddPP.Visible = true;
                                        DeletePP.Visible = true;
                                        AddNWPBtn.Visible = false;
                                        DelNWPBtn.Visible = false;
                                    }
                                    else if (type == 9)
                                    {
                                        AddNWPBtn.Visible = true;
                                        DelNWPBtn.Visible = true;
                                        AddPP.Visible = false;
                                        DeletePP.Visible = false;
                                    }
                                    else
                                    {
                                        AddPP.Visible = false;
                                        DeletePP.Visible = false;
                                        AddNWPBtn.Visible = false;
                                        DelNWPBtn.Visible = false;
                                    }
                                    MoveLogAdd();
                                }
                            }
                            else
                            {
                                MyMap.MoveElements(x, y);
                            }
                        }
                        break;
                    case 5:
                        if (MyMap.Polygons.active & MyMap.Polygons.step)
                        {
                            MyMap.Polygons.TempPolygon.SetTempPoint(x, y);
                        }
                        break;
                    case 360:
                        if (MyMap.Circles.step)
                        {
                            MyMap.Circles.TempCircle.SetRadius(x, y);
                        }
                        break;
                    case 6:
                        if (MyMap.Buildings.Buildings[activeElem.item].InputWires.step)
                        {
                            if (drawLevel.Level == -1)
                            {
                                MyMap.Buildings.Buildings[activeElem.item].MoveIW(x, y);
                            }
                            else
                            {
                                MyMap.Buildings.Buildings[activeElem.item].MoveIWInBuild(x, y);
                            }
                        }
                        break;
                    case 7:
                        if (MyMap.Buildings.Buildings[activeElem.item].Entrances.step)
                        {
                            MyMap.Buildings.Buildings[activeElem.item].MoveEntrance(x, y);
                        }
                        break;
                    case 8:
                        if (MyMap.NetworkElements.step)
                        {
                            MyMap.NetworkElements.TempNetworkElement.SetPoint(x, y);
                        }
                        break;
                    case 9:
                        if (MyMap.NetworkWires.active & MyMap.NetworkWires.step)
                        {
                            MyMap.NetworkWires.TempNetworkWire.SetTempPoint(x, y);
                        }
                        break;
                }
            }
        }

        private void ChechEdges(int x, int y)
        {
            int dif = 0;
            int n = (int)(10d * zoom);
            bool refresh = false;
            double Left = (double)MyMap.mapSetting.Left * zoom;
            double Right = (double)MyMap.mapSetting.Right * zoom;
            double Top = (double)MyMap.mapSetting.Top * zoom;
            double Bottom = (double)MyMap.mapSetting.Bottom * zoom;
            if (x < Left + n)
            {
                dif = (int)(x - n - Left);
                MyMap.mapSetting = new SizeRenderingArea(MyMap.mapSetting.Name, MyMap.mapSetting.Left + dif, 
                    MyMap.mapSetting.Right - dif, MyMap.mapSetting.Top, MyMap.mapSetting.Bottom);
                refresh = true;
            }
            if (x > Right - n)
            {
                dif = (int)(x + n - Right);
                MyMap.mapSetting = new SizeRenderingArea(MyMap.mapSetting.Name, MyMap.mapSetting.Left - dif,
                    MyMap.mapSetting.Right + dif, MyMap.mapSetting.Top, MyMap.mapSetting.Bottom);
                refresh = true;
            }
            if (y > Top - n)
            {
                dif = (int)(y + n - Top);
                //Top = y + n;
                //Bottom = -Top;
                MyMap.mapSetting = new SizeRenderingArea(MyMap.mapSetting.Name, MyMap.mapSetting.Left,
                    MyMap.mapSetting.Right, MyMap.mapSetting.Top + dif, MyMap.mapSetting.Bottom - dif);
                refresh = true;
            }
            if (y < Bottom + n)
            {
                dif = (int)(y - n - Bottom);
                //Bottom = y - n;
                //Top = -Bottom;
                MyMap.mapSetting = new SizeRenderingArea(MyMap.mapSetting.Name, MyMap.mapSetting.Left,
                    MyMap.mapSetting.Right, MyMap.mapSetting.Top - dif, MyMap.mapSetting.Bottom + dif);
                refresh = true;
            }
            if (refresh)
            {
                MyMap.ResizeRenderingArea();
                refresh = false;
            }
        }
        #region Для работы с зумом
        static public Point GenZoomPoint(Point p)
        {
            return new Point((int)((double)p.X * zoom), (int)((double)p.Y * zoom));
        }

        static public Point _GenZoomPoint(Point p)
        {
            return new Point((int)((double)p.X / zoom), (int)((double)p.Y / zoom));
        }

        static public MyRectangle GenZoomRect(MyRectangle rect)
        {
            MyRectangle mr = new MyRectangle();
            foreach (var p in rect.Points)
            {
                mr.Points.Add(GenZoomPoint(p));
            }
            return mr;
        }

        static public Polygon GenZoomPolygon(Polygon pol)
        {
            Polygon pl = new Polygon();
            foreach (var p in pol.Points)
            {
                pl.Points.Add(GenZoomPoint(p));
            }
            return pl;
        }
        static public Circle GenZoomCircle(Circle cir)
        {
            Circle _cir = new Circle
            {
                radius = (int)((double)cir.radius * zoom),
                MainCenterPoint = GenZoomPoint(cir.MainCenterPoint),
                LocalCenterPoint = GenZoomPoint(cir.LocalCenterPoint)
            };
            return _cir;
        }
        #endregion
        #region Прочие функции
        /// <summary>
        /// Авторизация
        /// </summary>
        /// <param name="edit"></param>
        /// <returns></returns>
        public static UserPrincipal Autorisation(out bool edit)
        {
            UserPrincipal user;
            user = UserPrincipal.Current;
            var group = user.GetGroups();
            foreach (var g in group)
            {
                if (g.Name == "Администраторы")
                {
                    edit = true;
                    return user;
                }
            }
            edit = false;
            if (user != null)
                return user;
            return null;
        }
        /// <summary>
        /// Обновление отображения кнопок после применения фильтров
        /// </summary>
        private void RefreshButtons()
        {
            if (!filtres.Poly)
                PolygonBtn.Visible = false;
            else
                PolygonBtn.Visible = true;
            if (!filtres.Line)
                LineBtn.Visible = false;
            else
                LineBtn.Visible = true;
            if (!filtres.Rect)
                RectangleBtn.Visible = false;
            else
                RectangleBtn.Visible = true;
            if (!filtres.Circ)
                CircleBtn.Visible = false;
            else
                CircleBtn.Visible = true;
            if (!filtres.NW)
                NWBtn.Visible = false;
            else
                NWBtn.Visible = true;
            if (!filtres.NE)
                NEBtn.Visible = false;
            else
                NEBtn.Visible = true;
            if (!filtres.Text)
                TextBtn.Visible = false;
            else
                TextBtn.Visible = true;
            if (!filtres.IW)
                IWBtn.Visible = false;
            else
                IWBtn.Visible = true;
            if (!filtres.Ent)
                EntranceBtn.Visible = false;
            else
                EntranceBtn.Visible = true;
        }
        /// <summary>
        /// Функция для поиска элемента, в который попал клик мыши
        /// </summary>
        /// <param name="x">Координата мыши X</param>
        /// <param name="y">Координата мыши Y</param>
        private bool SelectItems(int x, int y)
        {
            Unfocus("Не выбран элемент");
            activeElem.item = MyMap.SearchElem(x, y, out activeElem.type, out activeElem.build, drawLevel);
            switch (activeElem.type)
            {
                case 1:
                    MyMap.Lines.Choose(activeElem.item);
                    InfoLable.Text = "Выбрана линия " + activeElem.item;
                    DeleteBtn.Enabled = true;
                    return true;
                case 2:
                    MyMap.Rectangles.Choose(activeElem.item);
                    InfoLable.Text = "Выбран прямоугольник " + activeElem.item;
                    DeleteBtn.Enabled = true;
                    if (drawLevel.Level == -1)
                        ToBuildBtn.Enabled = true;
                    return true;
                case 3:
                    MyMap.Polygons.Choose(activeElem.item);
                    InfoLable.Text = "Выбран многоугольник " + activeElem.item;
                    DeleteBtn.Enabled = true;
                    if (drawLevel.Level == -1)
                        ToBuildBtn.Enabled = true;
                    AddPP.Visible = true;
                    AddPP.Enabled = true;
                    DeletePP.Visible = true;
                    DeletePP.Enabled = true;
                    return true;
                case 4:
                    MyMap.Buildings.Choose(activeElem.item);
                    InfoLable.Text = "Выбрано здание " + activeElem.item + " '" + MyMap.Buildings.Buildings[activeElem.item].Name + "'";
                    DeleteBtn.Enabled = true;
                    if (drawLevel.Level == -1)
                    {
                        ToBuildBtn.Enabled = true;
                        EntranceBtn.Enabled = true;
                        IWBtn.Enabled = true;
                    }
                    return true;
                case 6:
                    InfoLable.Text = "Выбран вход провода в здание " + activeElem.build + " с ID " + activeElem.item;
                    DeleteBtn.Enabled = true;
                    MyMap.Buildings.Buildings[activeElem.build].InputWires.InputWires.Choose(activeElem.item);
                    break;
                case 7:
                    InfoLable.Text = "Выбран вход в здание " + activeElem.build + " с ID " + activeElem.item;
                    DeleteBtn.Enabled = true;
                    MyMap.Buildings.Buildings[activeElem.build].Entrances.Enterances.Choose(activeElem.item);
                    break;
                case 8:
                    MyMap.NetworkElements.Choose(activeElem.item);
                    InfoLable.Text = "Выбран сетевой элемент " + activeElem.item;
                    DeleteBtn.Enabled = true;
                    break;
                case 9:
                    MyMap.NetworkWires.Choose(activeElem.item);
                    InfoLable.Text = "Выбран провод " + activeElem.item;
                    DeleteBtn.Enabled = true;
                    DelNWPBtn.Visible = true;
                    DelNWPBtn.Enabled = true;
                    AddNWPBtn.Visible = true;
                    AddNWPBtn.Enabled = true;
                    return true;
                case 10:
                    MyMap.MyTexts.Choose(activeElem.item);
                    InfoLable.Text = "Выбрана надпись " + activeElem.item;
                    DeleteBtn.Enabled = true;
                    return true;
                case 360:
                    MyMap.Circles.Choose(activeElem.item);
                    InfoLable.Text = "Выбрана окружность" + activeElem.item;
                    DeleteBtn.Enabled = true;
                    if (drawLevel.Level == -1)
                        ToBuildBtn.Enabled = true;
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Обновление наименований этажей при открытии здания
        /// </summary>
        private void UpgrateFloors()
        {
            FloorDown.Visible = true;
            FloorUP.Visible = true;
            label1.Visible = true;
            floors_name.AddRange(MyMap.Buildings.Buildings[activeElem.item].floors_name);
            label1.Text = floors_name[drawLevel.Floor];
            floor_index = drawLevel.Floor;
        }
        /// <summary>
        /// Возврат элементов к исходному состоянию, без фокусировки на определенном элементе
        /// </summary>
        public void Unfocus(string info)
        {
            InfoLable.Text = info;
            //
            AddPP.Visible = false;
            AddPP.Enabled = false;
            DeletePP.Visible = false;
            DeletePP.Enabled = false;
            //
            DelNWPBtn.Visible = false;
            DelNWPBtn.Enabled = false;
            AddNWPBtn.Visible = false;
            AddNWPBtn.Enabled = false;
            //
            ToBuildBtn.Enabled = false;
            DeleteBtn.Enabled = false;
            EntranceBtn.Enabled = false;
            IWBtn.Enabled = false;
            //
            MyMap.Unfocus(true);
        }
        /// <summary>
        /// Преобразование зданий в фигуры и наоброт
        /// </summary>
        private void ToBuild()
        {
            if (activeElem.type == 4)
            {
                if (MyMap.Buildings.Buildings[activeElem.item].type == 2)
                {
                    if (MessageBox.Show("", "Преобразовать обратно в прямоугольник?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        MyMap.Rectangles.Add(MyMap.Buildings.Buildings[activeElem.item].MainRectangle);
                        int lastindex = MyMap.Rectangles.Rectangles.Count - 1;
                        Element elem = new Element(4, activeElem.item, MyMap.Buildings.Buildings[activeElem.item], 1);
                        Element _elem = new Element(2, lastindex, MyMap.Rectangles.Rectangles[lastindex], 1);
                        MyMap.log.Add(new LogMessage("Преобразовал здание в прямоугольник", elem, _elem));
                        CheckButtons(true);
                        MyMap.Buildings.Remove(activeElem.item);
                        Unfocus("Преобразовал здание в прямоугольник");
                    }
                }
                else if (MyMap.Buildings.Buildings[activeElem.item].type == 3)
                {
                    if (MessageBox.Show("", "Преобразовать обратно в многоугольник?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        MyMap.Polygons.Add(MyMap.Buildings.Buildings[activeElem.item].MainPolygon);
                        int lastindex = MyMap.Polygons.Polygons.Count - 1;
                        Element elem = new Element(4, activeElem.item, MyMap.Buildings.Buildings[activeElem.item], 2);
                        Element _elem = new Element(3, lastindex, MyMap.Polygons.Polygons[lastindex], 2);
                        MyMap.log.Add(new LogMessage("Преобразовал здание в многоугольник", elem, _elem));
                        CheckButtons(true);
                        MyMap.Buildings.Remove(activeElem.item);
                        Unfocus("Преобразовал здание в многоугольник");
                    }
                }
                else if (MyMap.Buildings.Buildings[activeElem.item].type == 360)
                {
                    if (MessageBox.Show("", "Преобразовать обратно в окружность?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        MyMap.Circles.Add(MyMap.Buildings.Buildings[activeElem.item].MainCircle);
                        int lastindex = MyMap.Circles.Circles.Count - 1;
                        Element elem = new Element(4, activeElem.item, MyMap.Buildings.Buildings[activeElem.item], 5);
                        Element _elem = new Element(360, lastindex, MyMap.Circles.Circles[lastindex], 5);
                        MyMap.log.Add(new LogMessage("Преобразовал здание в окружность", elem, _elem));
                        CheckButtons(true);
                        MyMap.Buildings.Remove(activeElem.item);
                        Unfocus("Преобразовал здание в окружность");
                    }
                }
            }
            else if (MessageBox.Show("", "Обозначить зданием?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                BuildForm buildForm = new BuildForm();
                buildForm.ShowDialog();
                if (activeElem.type == 2)
                {
                    MyMap.Buildings.Add(new Building(buildForm.name, buildForm.loft, buildForm.basement, buildForm.count, MyMap.Rectangles.Rectangles[activeElem.item], MyMap.Buildings.Buildings.Count));
                    int lastindex = MyMap.Buildings.Buildings.Count - 1;
                    Element elem = new Element(2, activeElem.item, MyMap.Rectangles.Rectangles[activeElem.item], 3);
                    Element _elem = new Element(4, lastindex, MyMap.Buildings.Buildings[lastindex], 3);
                    MyMap.log.Add(new LogMessage("Преобразовал прямоугольник в здание", elem, _elem));
                    CheckButtons(true);
                    MyMap.Rectangles.Remove(activeElem.item);
                    Unfocus("Преобразовал прямоугольник в здание");
                }
                else if (activeElem.type == 3)
                {
                    MyMap.Buildings.Add(new Building(buildForm.name, buildForm.loft, buildForm.basement, buildForm.count, MyMap.Polygons.Polygons[activeElem.item], MyMap.Buildings.Buildings.Count));
                    int lastindex = MyMap.Buildings.Buildings.Count - 1;
                    Element elem = new Element(3, activeElem.item, MyMap.Polygons.Polygons[activeElem.item], 4);
                    Element _elem = new Element(4, lastindex, MyMap.Buildings.Buildings[lastindex], 4);
                    MyMap.log.Add(new LogMessage("Преобразовал многоугольник в здание", elem, _elem));
                    CheckButtons(true);
                    MyMap.Polygons.Remove(activeElem.item);
                    Unfocus("Преобразовал многоугольник в здание");
                }
                else if (activeElem.type == 360)
                {
                    MyMap.Buildings.Add(new Building(buildForm.name, buildForm.loft, buildForm.basement, buildForm.count, MyMap.Circles.Circles[activeElem.item], MyMap.Buildings.Buildings.Count));
                    int lastindex = MyMap.Buildings.Buildings.Count - 1;
                    Element elem = new Element(360, activeElem.item, MyMap.Circles.Circles[activeElem.item], 6);
                    Element _elem = new Element(4, lastindex, MyMap.Buildings.Buildings[lastindex], 6);
                    MyMap.log.Add(new LogMessage("Преобразовал окружность в здание", elem, _elem));
                    CheckButtons(true);
                    MyMap.Circles.Remove(activeElem.item);
                    Unfocus("Преобразовал окружность в здание");
                }
            }
        }

        /// <summary>
        /// Удаление элемента
        /// </summary>
        private void DeleteElem()
        {
            Element elem, _elem;
            switch (activeElem.type)
            {
                case 1:
                    elem = new Element(1, activeElem.item, new Line(), -1);
                    _elem = new Element(1, activeElem.item, MyMap.Lines.Lines[activeElem.item].Clone(), -1);
                    MyMap.log.Add(new LogMessage("Удалил линию", elem, _elem));
                    InfoLable.Text = "Удалил линию";
                    MyMap.Lines.Remove(activeElem.item);
                    CheckButtons(true);
                    break;
                case 2:
                    elem = new Element(2, activeElem.item, new MyRectangle(), -1);
                    _elem = new Element(2, activeElem.item, MyMap.Rectangles.Rectangles[activeElem.item].Clone(), -1);
                    MyMap.log.Add(new LogMessage("Удалил прямоугольник", elem, _elem));
                    InfoLable.Text = "Удалил прямоугольник";
                    MyMap.Rectangles.Remove(activeElem.item);
                    CheckButtons(true);
                    break;
                case 3:
                    elem = new Element(3, activeElem.item, new Polygon(), -1);
                    _elem = new Element(3, activeElem.item, MyMap.Polygons.Polygons[activeElem.item].Clone(), -1);
                    MyMap.log.Add(new LogMessage("Удалил многоугольник", elem, _elem));
                    InfoLable.Text = "Удалил многоугольник";
                    MyMap.Polygons.Remove(activeElem.item);
                    CheckButtons(true);
                    break;
                case 4:
                    elem = new Element(4, activeElem.item, new Building(), -1);
                    _elem = new Element(4, activeElem.item, MyMap.Buildings.Buildings[activeElem.item], -1);
                    MyMap.log.Add(new LogMessage("Удалил здание", elem, _elem));
                    InfoLable.Text = "Удалил здание";
                    MyMap.Buildings.Remove(activeElem.item);
                    CheckButtons(true);
                    break;
                case 6:
                    if (CheckIW())
                    {
                        elem = new Element(6, activeElem.item, new Circle(), -1);
                        _elem = new Element(6, activeElem.item, MyMap.Buildings.Buildings[activeElem.build].InputWires.InputWires.Circles[activeElem.item].Clone(), -1);
                        MyMap.log.Add(new LogMessage("Удалил вход провода в здание", elem, _elem, activeElem.build));
                        InfoLable.Text = "Удалил вход провода в здание";
                        MyMap.Buildings.Buildings[activeElem.build].InputWires.InputWires.Remove(activeElem.item);
                        CheckButtons(true);
                    }
                    else
                    {
                        Unfocus("Невозможно удалить элемент");
                        return;
                    }
                    break;
                case 7:
                    elem = new Element(7, activeElem.item, new Circle(), -1);
                    _elem = new Element(7, activeElem.item, MyMap.Buildings.Buildings[activeElem.build].Entrances.Enterances.Circles[activeElem.item].Clone(), -1);
                    MyMap.log.Add(new LogMessage("Удалил вход в здание", elem, _elem, activeElem.build));
                    InfoLable.Text = "Удалил вход в здание";
                    MyMap.Buildings.Buildings[activeElem.build].Entrances.Enterances.Remove(activeElem.item);
                    CheckButtons(true);
                    break;
                case 8:
                    if (MyMap.NetworkElements.NetworkElements[activeElem.item].Options.BusyPorts != 0)
                    {
                        Unfocus("Невозможно удалить элемент");
                        return;
                    }
                    else
                    {
                        elem = new Element(8, activeElem.item, new NetworkElement(), -1);
                        _elem = new Element(8, activeElem.item, MyMap.NetworkElements.NetworkElements[activeElem.item].Clone(), -1);
                        MyMap.log.Add(new LogMessage("Удалил сетевой элемент", elem, _elem));
                        InfoLable.Text = "Удалил сетевой элемент";
                        MyMap.NetworkElements.Remove(activeElem.item);
                        CheckButtons(true);
                    }
                    break;
                case 9:
                    var ne1 = MyMap.NetworkWires.NetworkWires[activeElem.item].idiw1;
                    var ne2 = MyMap.NetworkWires.NetworkWires[activeElem.item].idiw2;
                    if (!ne1.IW)
                    {
                        MyMap.NetworkElements.NetworkElements[ne1.ID].Options.BusyPorts--;
                    }
                    if (!ne2.IW)
                    {
                        MyMap.NetworkElements.NetworkElements[ne2.ID].Options.BusyPorts--;
                    }
                    elem = new Element(9, activeElem.item, new NetworkWire(), -1);
                    _elem = new Element(9, activeElem.item, MyMap.NetworkWires.NetworkWires[activeElem.item].Clone(), -1);
                    MyMap.log.Add(new LogMessage("Удалил провод", elem, _elem));
                    InfoLable.Text = "Удалил провод";
                    MyMap.NetworkWires.Remove(activeElem.item);
                    CheckButtons(true);
                    break;
                case 10:
                    elem = new Element(10, activeElem.item, new MyText(), -1);
                    _elem = new Element(10, activeElem.item, MyMap.MyTexts.MyTexts[activeElem.item].Clone(), -1);
                    MyMap.log.Add(new LogMessage("Удалил надпись", elem, _elem));
                    InfoLable.Text = "Удалил надпись";
                    MyMap.MyTexts.Remove(activeElem.item);
                    CheckButtons(true);
                    break;
                case 360:
                    elem = new Element(360, activeElem.item, new Circle(), -1);
                    _elem = new Element(360, activeElem.item, MyMap.Circles.Circles[activeElem.item].Clone(), -1);
                    MyMap.log.Add(new LogMessage("Удалил окружность", elem, _elem));
                    InfoLable.Text = "Удалил окружность";
                    MyMap.Circles.Remove(activeElem.item);
                    break;
            }
            Unfocus("Удалил элемент");
        }

        private bool CheckIW()
        {
            foreach (var elem in MyMap.NetworkWires.NetworkWires)
            {
                if (elem.idiw1 == new IDandIW(activeElem.item,true,activeElem.build))
                    return false;
                if (elem.idiw2 == new IDandIW(activeElem.item, true, activeElem.build))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Поиск элемента
        /// </summary>
        private void Search()
        {
            SearchDialog sd = new SearchDialog();
            sd.ShowDialog();
            if (sd.text != "" & sd.text != " ")
            {
                DrawLevel _drawLevel = MyMap.MyTexts.Search(sd.text);
                if (_drawLevel.Level != -2)
                {
                    drawLevel = _drawLevel;
                    if (_drawLevel.Level != -1)
                    {
                        UpgrateFloors();
                        Unfocus("Выбрано здание " + activeElem.item + " '" + MyMap.Buildings.Buildings[activeElem.item].Name + "'");
                        IWBtn.Enabled = true;
                        ReturnToMainBtn.Enabled = true;
                    }
                }
                else
                {
                    MessageBox.Show("Не найдено");
                }
            }
        }
        #endregion
        #region Различные функции для работы с логом
        /// <summary>
        /// Запись в лог перемещений элементов
        /// </summary>
        private void MoveLogAdd()
        {
            object elem = new object();
            switch (activeElem.type)
            {
                case 1:
                    elem = MyMap.Lines.Lines[activeElem.item].Clone();
                    break;
                case 2:
                    elem = MyMap.Rectangles.Rectangles[activeElem.item].Clone();
                    break;
                case 3:
                    elem = MyMap.Polygons.Polygons[activeElem.item].Clone();
                    break;
                case 360:
                    elem = MyMap.Circles.Circles[activeElem.item].Clone();
                    break;
                case 6:
                    elem = MyMap.Buildings.Buildings[activeElem.build].InputWires.InputWires.Circles[activeElem.item].Clone();
                    break;
                case 7:
                    elem = MyMap.Buildings.Buildings[activeElem.build].Entrances.Enterances.Circles[activeElem.item].Clone();
                    break;
                case 8:
                    elem = MyMap.NetworkElements.NetworkElements[activeElem.item].Clone();
                    break;
                case 9:
                    elem = MyMap.NetworkWires.NetworkWires[activeElem.item].Clone();
                    break;
                case 10:
                    elem = MyMap.MyTexts.MyTexts[activeElem.item].Clone();
                    break;
            }
            if (activeElem.type == 1 | activeElem.type == 2 | activeElem.type == 3 | activeElem.type == 360 | activeElem.type == 6 | activeElem.type == 7 | activeElem.type == 8 | activeElem.type == 9 | activeElem.type == 10)
            {
                if (isMoveLog)
                {
                    isMoveLog = false;
                    _MoveElem = new Element(activeElem.type, activeElem.item, elem, -2);
                    if (_MoveElem.type == 6 | _MoveElem.type == 7)
                        MyMap.log.Add(new LogMessage("Переместил элемент", _MoveElem, MoveElem, activeElem.build));
                    else
                        MyMap.log.Add(new LogMessage("Переместил элемент", _MoveElem, MoveElem));
                }
                else
                {
                    isMoveLog = true;
                    MoveElem = new Element(activeElem.type, activeElem.item, elem, -2);
                }
            }
        }
        /// <summary>
        /// Обработка элементов лога
        /// </summary>
        /// <param name="_elem"></param>
        /// <param name="elem"></param>
        /// <param name="backbtn"></param>
        private void PharseElem(Element _elem, Element elem, bool backbtn)
        {
            if (_elem.transform != -1 & _elem.transform != -2)
            {
                if (backbtn)
                {
                    switch (_elem.transform)
                    {
                        case 1:
                            MyMap.Rectangles.Remove(elem.index);
                            MyMap.Buildings.Buildings[_elem.index] = (Building)_elem.elem;
                            break;
                        case 2:
                            MyMap.Polygons.Remove(elem.index);
                            MyMap.Buildings.Buildings[_elem.index] = (Building)_elem.elem;
                            break;
                        case 3:
                            MyMap.Buildings.Remove(elem.index);
                            MyMap.Rectangles.Rectangles[_elem.index] = (MyRectangle)_elem.elem;
                            break;
                        case 4:
                            MyMap.Buildings.Remove(elem.index);
                            MyMap.Polygons.Polygons[_elem.index] = (Polygon)_elem.elem;
                            break;
                        case 5:
                            MyMap.Circles.Remove(elem.index);
                            MyMap.Buildings.Buildings[_elem.index] = (Building)_elem.elem;
                            break;
                        case 6:
                            MyMap.Buildings.Remove(elem.index);
                            MyMap.Circles.Circles[_elem.index] = (Circle)_elem.elem;
                            break;
                    }
                }
                else
                {
                    switch (_elem.transform)
                    {
                        case 1:
                            MyMap.Buildings.Remove(elem.index);
                            MyMap.Rectangles.Rectangles[_elem.index] = (MyRectangle)_elem.elem;
                            break;
                        case 2:
                            MyMap.Buildings.Remove(elem.index);
                            MyMap.Polygons.Polygons[_elem.index] = (Polygon)_elem.elem;
                            break;
                        case 3:
                            MyMap.Rectangles.Remove(elem.index);
                            MyMap.Buildings.Buildings[_elem.index] = (Building)_elem.elem;
                            break;
                        case 4:
                            MyMap.Polygons.Remove(elem.index);
                            MyMap.Buildings.Buildings[_elem.index] = (Building)_elem.elem;
                            break;
                        case 5:
                            MyMap.Buildings.Remove(elem.index);
                            MyMap.Circles.Circles[_elem.index] = (Circle)_elem.elem;
                            break;
                        case 6:
                            MyMap.Circles.Remove(elem.index);
                            MyMap.Buildings.Buildings[_elem.index] = (Building)_elem.elem;
                            break;
                    }
                }
            }
            else
            {
                switch (elem.type)
                {
                    case 1:
                        MyMap.Lines.Lines[elem.index] = (Line)elem.elem;
                        break;
                    case 2:
                        MyMap.Rectangles.Rectangles[elem.index] = (MyRectangle)elem.elem;
                        break;
                    case 3:
                        MyMap.Polygons.Polygons[elem.index] = (Polygon)elem.elem;
                        break;
                    case 4:
                        MyMap.Buildings.Buildings[elem.index] = (Building)elem.elem;
                        break;
                    case 360:
                        MyMap.Circles.Circles[elem.index] = (Circle)elem.elem;
                        break;
                    case 8:
                        MyMap.NetworkElements.NetworkElements[elem.index] = (NetworkElement)elem.elem;
                        if (_elem.transform == -2)
                        {
                            Point location = new Point((int)(MyMap.NetworkElements.NetworkElements[elem.index].texture.location.X + MyMap.NetworkElements.NetworkElements[elem.index].texture.width / 2), (int)(MyMap.NetworkElements.NetworkElements[elem.index].texture.location.Y + MyMap.NetworkElements.NetworkElements[elem.index].texture.width / 2));
                            MyMap.NetworkWires.CheckNW(location.X, location.Y, elem.index, false, -1, MyMap.NetworkElements.NetworkElements[elem.index].DL);
                            //MyMap.NetworkElements.NetworkElements[elem.index].MoveElem(location.X, location.Y, elem.index, MyMap.NetworkWires);
                        }
                        break;
                    case 9:
                        MyMap.NetworkWires.NetworkWires[elem.index] = (NetworkWire)elem.elem;
                        if (MyMap.NetworkWires.NetworkWires[elem.index].delete)
                        {
                            var nw = (NetworkWire)_elem.elem;
                            var ne1 = nw.idiw1;
                            var ne2 = nw.idiw2;
                            if (!ne1.IW)
                            {
                                MyMap.NetworkElements.NetworkElements[ne1.ID].Options.BusyPorts--;
                            }
                            if (!ne2.IW)
                            {
                                MyMap.NetworkElements.NetworkElements[ne2.ID].Options.BusyPorts--;
                            }
                        }
                        else
                        {
                            var nw = (NetworkWire)elem.elem;
                            var ne1 = nw.idiw1;
                            var ne2 = nw.idiw2;
                            if (!ne1.IW)
                            {
                                MyMap.NetworkElements.NetworkElements[ne1.ID].Options.BusyPorts++;
                            }
                            if (!ne2.IW)
                            {
                                MyMap.NetworkElements.NetworkElements[ne2.ID].Options.BusyPorts++;
                            }
                        }
                        break;
                    case 10:
                        MyMap.MyTexts.MyTexts[elem.index] = (MyText)elem.elem;
                        if (MyMap.MyTexts.MyTexts[elem.index].idtexture != -1 && !MTTextures.Contains((uint)MyMap.MyTexts.MyTexts[elem.index].idtexture))
                            MyMap.MyTexts.MyTexts[elem.index].MapLoadGenTextures();
                        break;
                    case 11:
                        if (backbtn)
                            AddTextureFromLog(elem.index, (string)_elem.elem);
                        else
                            DeleteTextureFromLog(elem.index);
                        break;
                    case 12:
                        if (backbtn)
                            DeleteTextureFromLog(elem.index);
                        else
                            AddTextureFromLog(elem.index, (string)_elem.elem);
                        break;
                }
            }
        }
        //
        /// <summary>
        /// Обработка элементов лога
        /// </summary>
        /// <param name="_elem"></param>
        /// <param name="elem"></param>
        /// <param name="buildid"></param>
        private void PharseElem(Element _elem, Element elem, int buildid)
        {
            switch (elem.type)
            {
                case 6:
                    MyMap.Buildings.Buildings[buildid].InputWires.InputWires.Circles[elem.index] = (Circle)elem.elem;
                    // Доделать движение проводов
                    if (_elem.transform == -2)
                    {
                        Point location = new Point(MyMap.Buildings.Buildings[buildid].InputWires.InputWires.Circles[elem.index].MainCenterPoint.X, MyMap.Buildings.Buildings[buildid].InputWires.InputWires.Circles[elem.index].MainCenterPoint.Y);
                        Point _location = new Point(MyMap.Buildings.Buildings[buildid].InputWires.InputWires.Circles[elem.index].LocalCenterPoint.X, MyMap.Buildings.Buildings[buildid].InputWires.InputWires.Circles[elem.index].LocalCenterPoint.Y);
                        MyMap.NetworkWires.CheckNW(location.X, location.Y, elem.index, true, buildid, MyMap.Buildings.Buildings[buildid].InputWires.InputWires.Circles[elem.index].MainDL);
                        MyMap.NetworkWires.CheckNW(_location.X, _location.Y, elem.index, true, buildid, MyMap.Buildings.Buildings[buildid].InputWires.InputWires.Circles[elem.index].LocalDL);
                        //MyMap.NetworkElements.NetworkElements[elem.index].MoveElem(location.X, location.Y, elem.index, MyMap.NetworkWires);
                    }
                    break;
                case 7:
                    MyMap.Buildings.Buildings[buildid].Entrances.Enterances.Circles[elem.index] = (Circle)elem.elem;
                    break;
            }
        }
        /// <summary>
        /// Проверка кнопок для лога
        /// </summary>
        /// <param name="clearForward"></param>
        private void CheckButtons(bool clearForward)
        {
            if (clearForward)
                MyMap.log.ClearForward();
            if (MyMap.log.NotNullBack())
                BackBtn.Enabled = true;
            else
                BackBtn.Enabled = false;
            if (MyMap.log.NotNullForward())
                ForwardBrn.Enabled = true;
            else
                ForwardBrn.Enabled = false;
        }
        #endregion
        #region Открытие, сохранение, экспорт, импорт
        /// <summary>
        /// Функция для сохранения карты в файл 
        /// </summary>
        /// <param name="fileExtension">Расширение файла в формате .*</param>
        /// <param name="descriptionFE">Описание заданного формата для отображения в диалоге</param>
        private void SaveMap(string fileExtension, string descriptionFE)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                Filter = descriptionFE + "|*" + fileExtension
            };
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                XmlSerializer formatter = new XmlSerializer(typeof(Map));
                string filename;
                if (saveFileDialog1.FileName.Contains(fileExtension))
                {
                    filename = saveFileDialog1.FileName;
                }
                else
                {
                    filename = saveFileDialog1.FileName + fileExtension;
                }
                MyMap.UserID = user.GetHashCode();
                Directory.CreateDirectory(Application.StartupPath + @"\###tempdirectory._temp###\");
                // получаем поток, куда будем записывать сериализованный объект
                using (FileStream fs = new FileStream(Application.StartupPath + @"\###tempdirectory._temp###\mapfile.map", FileMode.Create))
                {
                    formatter.Serialize(fs, MyMap);
                }
                SaveTextures(ImagesURL);
                Parametrs.Save(parametrs);
                ID_TEXT temp = new ID_TEXT();
                foreach (var n in neButtons)
                    temp.ADD(n.id, n.textname);
                NEButton.Save(temp);
                File.Copy(Application.StartupPath + @"\Textures\ListOfTextures", Application.StartupPath + @"\###tempdirectory._temp###\ListOfTextures");
                File.Copy(Application.StartupPath + @"\Configurations\NetworkSettings", Application.StartupPath + @"\###tempdirectory._temp###\NetworkSettings");
                //File.Copy(Application.StartupPath + @"\Configurations\NEButtons", Application.StartupPath + @"\###tempdirectory._temp###\NEButtons");
                foreach(var url in ImagesURL)
                    File.Copy(Application.StartupPath + @"\Textures\" + url, Application.StartupPath + @"\###tempdirectory._temp###\" + url);
                if (File.Exists(filename))
                    File.Delete(filename);
                ZipFile.CreateFromDirectory(Application.StartupPath + @"\###tempdirectory._temp###\", filename);
                File.Delete(Application.StartupPath + @"\###tempdirectory._temp###\mapfile.map");
                File.Delete(Application.StartupPath + @"\###tempdirectory._temp###\ListOfTextures");
                File.Delete(Application.StartupPath + @"\###tempdirectory._temp###\NetworkSettings");
                //File.Delete(Application.StartupPath + @"\###tempdirectory._temp###\NEButtons");
                foreach (var url in ImagesURL)
                    File.Delete(Application.StartupPath + @"\###tempdirectory._temp###\" + url);
                Directory.Delete(Application.StartupPath + @"\###tempdirectory._temp###\");
            }
        }
        /// <summary>
        /// Функция для открытия файла карты
        /// </summary>
        /// <param name="fileExtension">Расширение файла в формате .*</param>
        /// <param name="descriptionFE">Описание заданного формата для отображения в диалоге</param>
        private void OpenMap(string fileExtension, string descriptionFE)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Filter = descriptionFE + "|*" + fileExtension
            };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ZipFile.ExtractToDirectory(openFileDialog1.FileName, Application.StartupPath + @"\###tempdirectory._temp###\");
                //Decompress(openFileDialog1.FileName, openFileDialog1.FileName + "._temp");
                XmlSerializer formatter = new XmlSerializer(typeof(Map));
                // получаем поток, куда будем записывать сериализованный объект
                using (FileStream fs = new FileStream(Application.StartupPath + @"\###tempdirectory._temp###\mapfile.map", FileMode.OpenOrCreate))
                {
                    Map TempMap = (Map)formatter.Deserialize(fs);
                    if (TempMap.UserID == user.GetHashCode() | edit)
                    {
                        MTTextures = new List<uint>();
                        for (int i = 0; i < TempMap.MyTexts.MyTexts.Count; i++)
                            TempMap.MyTexts.MyTexts[i].MapLoadGenTextures();
                        _OpenTextures();
                        Parametrs._Open();
                        ID_TEXT temp = new ID_TEXT();
                        foreach (var n in neButtons)
                            temp.ADD(n.id, n.textname);
                        RefreshNENuttons();
                        MyMap.MapLoad(TempMap);
                    }
                    else
                    {
                        File.Delete(Application.StartupPath + @"\###tempdirectory._temp###\mapfile.map");
                        File.Delete(Application.StartupPath + @"\###tempdirectory._temp###\ListOfTextures");
                        File.Delete(Application.StartupPath + @"\###tempdirectory._temp###\NetworkSettings");
                        foreach (var url in ImagesURL)
                            if (File.Exists(Application.StartupPath + @"\###tempdirectory._temp###\" + url))
                                File.Delete(Application.StartupPath + @"\###tempdirectory._temp###\" + url);
                        Directory.Delete(Application.StartupPath + @"\###tempdirectory._temp###\");
                        MessageBox.Show("Вам запрещен доступ к данной карте");
                        return;
                    }
                }
                File.Delete(Application.StartupPath + @"\###tempdirectory._temp###\mapfile.map");
                File.Delete(Application.StartupPath + @"\###tempdirectory._temp###\ListOfTextures");
                File.Delete(Application.StartupPath + @"\###tempdirectory._temp###\NetworkSettings");
                foreach (var url in ImagesURL)
                    if (File.Exists(Application.StartupPath + @"\###tempdirectory._temp###\" + url))
                        File.Delete(Application.StartupPath + @"\###tempdirectory._temp###\" + url);
                Directory.Delete(Application.StartupPath + @"\###tempdirectory._temp###\");
            }
            CheckButtons(true);
        }

        /// <summary>
        /// Сохранение (экспорт) здания в файл 
        /// </summary>
        /// <param name="fileExtension">Расширение файла в формате .*</param>
        /// <param name="descriptionFE">Описание заданного формата для отображения в диалоге</param>
        private void SaveBuild(string fileExtension, string descriptionFE)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                Filter = descriptionFE + "|*" + fileExtension
            };
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                XmlSerializer formatter = new XmlSerializer(typeof(Map));
                Map TempMap = new Map();
                TempMap.Buildings.Add(MyMap.Buildings.Buildings[activeElem.item]);
                TempMap.Circles.AddGroupElems(MyMap.Circles.GetInBuild(activeElem.item));
                TempMap.Lines.AddGroupElems(MyMap.Lines.GetInBuild(activeElem.item));
                TempMap.Polygons.AddGroupElems(MyMap.Polygons.GetInBuild(activeElem.item));
                TempMap.Rectangles.AddGroupElems(MyMap.Rectangles.GetInBuild(activeElem.item));
                string filename;
                if (saveFileDialog1.FileName.Contains(fileExtension))
                {
                    filename = saveFileDialog1.FileName;
                }
                else
                {
                    filename = saveFileDialog1.FileName + fileExtension;
                }
                // получаем поток, куда будем записывать сериализованный объект
                using (FileStream fs = new FileStream(filename + "._temp", FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, TempMap);
                }
                //Compress(filename + "._temp", filename);
                File.Delete(filename + "._temp");
            }
        }
        /// <summary>
        /// Открытие файла с картой сети
        /// </summary>
        /// <param name="fileExtension">Расширение файла в формате .*</param>
        /// <param name="descriptionFE">Описание заданного формата для отображения в диалоге</param>
        private void OpenBuild(string fileExtension, string descriptionFE)
        {
            Map TempMap = new Map();
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Filter = descriptionFE + "|*" + fileExtension
            };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Decompress(openFileDialog1.FileName, openFileDialog1.FileName + "._temp");
                XmlSerializer formatter = new XmlSerializer(typeof(Map));
                // получаем поток, куда будем записывать сериализованный объект
                using (FileStream fs = new FileStream(openFileDialog1.FileName + "._temp", FileMode.OpenOrCreate))
                {
                    TempMap = (Map)formatter.Deserialize(fs);
                }
                File.Delete(openFileDialog1.FileName + "._temp");
                if (TempMap.Buildings.Buildings[0].type == 2)
                {
                    TempMap.Buildings.Buildings[0].MainRectangle.Points.Clear();
                    TempMap.Buildings.Buildings[0].MainRectangle.Points.AddRange(TempMap.Buildings.Buildings[0]._MainRectangle.Points);
                }
                else if (TempMap.Buildings.Buildings[0].type == 3)
                {
                    TempMap.Buildings.Buildings[0].MainPolygon.Points.Clear();
                    TempMap.Buildings.Buildings[0].MainPolygon.Points.AddRange(TempMap.Buildings.Buildings[0]._MainPolygon.Points);
                }
                else if (TempMap.Buildings.Buildings[0].type == 360)
                {
                    //Заглушка
                }
                MyMap.Buildings.Add(TempMap.Buildings.Buildings[0]);
                MyMap.Circles.AddGroupElems(TempMap.Circles.Circles.ConvertAll(new Converter<Circle, object>(Conv)));
                MyMap.Lines.AddGroupElems(TempMap.Lines.Lines.ConvertAll(new Converter<Line, object>(Conv)));
                MyMap.Rectangles.AddGroupElems(TempMap.Rectangles.Rectangles.ConvertAll(new Converter<MyRectangle, object>(Conv)));
                MyMap.Polygons.AddGroupElems(TempMap.Polygons.Polygons.ConvertAll(new Converter<Polygon, object>(Conv)));
            }
        }
        /// <summary>
        /// Обновить ярлыки для сетевых элементов
        /// </summary>
        static public void RefreshNENuttons()
        {
            for (int i = 0; i < neButtons.Count; i++)
            {
                for (int j = 0; j < ImagesURL.Count; j++)
                {
                    if (neButtons[i].textname == ImagesURL[j])
                    {
                        neButtons[i].id = j;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Функция для сохранения здания в файл 
        /// </summary>
        /// <param name="fileExtension">Расширение файла в формате .*</param>
        /// <param name="descriptionFE">Описание заданного формата для отображения в диалоге</param>
        private void SaveTemplateMap(string fileExtension, string descriptionFE)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                Filter = descriptionFE + "|*" + fileExtension
            };
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                XmlSerializer formatter = new XmlSerializer(typeof(Map));
                Map TempMap = new Map();
                foreach (var b in MyMap.Buildings.Buildings)
                {
                    TempMap.Buildings.Add(new Building(b));
                }
                TempMap.Circles = MyMap.Circles;
                TempMap.Lines = MyMap.Lines;
                TempMap.Polygons = MyMap.Polygons;
                TempMap.Rectangles = MyMap.Rectangles;
                TempMap.mapSetting = MyMap.mapSetting;
                string filename;
                if (saveFileDialog1.FileName.Contains(fileExtension))
                {
                    filename = saveFileDialog1.FileName;
                }
                else
                {
                    filename = saveFileDialog1.FileName + fileExtension;
                }
                // получаем поток, куда будем записывать сериализованный объект
                using (FileStream fs = new FileStream(filename + "._temp", FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, TempMap);
                }
                //Compress(filename + "._temp", filename);
                File.Delete(filename + "._temp");
            }
        }
        /// <summary>
        /// Конвертер
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        static object Conv(object elem) => elem;
        #endregion
        #region События
        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            MyMap.SetInstrument(9);
            ImageTextures IT = new ImageTextures(ref MyMap.NetworkElements);
            IT.ShowDialog();
            if (IT.action == 0)
            {
                AddTexture(IT.imageindex);
            }
            else if (IT.action == 1)
            {
                neButtons.Add(new NEButton(new ToolStripButton(Images.Images[IT.imageindex]), IT.imageindex, ImagesURL[IT.imageindex], toolStrip1.Items.Count));
                toolStrip1.Items.Add(neButtons.Last().toolStripButton);
            }
            else if (IT.action == 2)
            {
                for (int i = 0; i < neButtons.Count; i++)
                {
                    if (neButtons[i].id == IT.imageindex)
                    {
                        int id = neButtons[i].id;
                        int tsid = neButtons[i].tsid;
                        toolStrip1.Items.RemoveAt(tsid);
                        neButtons.RemoveAt(i);
                        for (int j = 0; j < neButtons.Count; j++)
                        {
                            if (neButtons[j].tsid > tsid)
                                neButtons[j].tsid--;
                        }
                        break;
                    }
                }
            }
            else if (IT.action == 3)
            {
                DeleteTexture(IT.imageindex);
            }
        }

        private void DeleteTexture(int id)
        {
            Element elem = new Element(11, id, ImagesURL[id], -1);
            DeleteImages.Add(ImagesURL[id]);
            ImagesURL.RemoveAt(id);
            isLoad = false;
            LoadImages();
            foreach (var neb in neButtons)
            {
                if (neb.id > id)
                {
                    int newid = neb.id - 1;
                    neb.id = newid;
                    neb.toolStripButton.Image = Images.Images[newid];
                    neb.textname = ImagesURL[newid];
                }
            }
            foreach (var item in MyMap.NetworkElements.NetworkElements)
            {
                if (!item.delete)
                    if (item.texture.idimage > id)
                        item.texture.idimage--;
            }
            string empty = "";
            Element _elem = new Element(11, id, empty, -1);
            MyMap.log.Add(new LogMessage("Удалил текстуру", elem, _elem));
            InfoLable.Text = "Удалил текстуру";
            CheckButtons(true);
        }

        private void AddTexture(int id)
        {
            string empty = "";
            Element elem = new Element(12, id, empty, -1);
            isLoad = false;
            LoadImages();
            Element _elem = new Element(12, id, ImagesURL.Last(), -1);
            MyMap.log.Add(new LogMessage("Добавил текстуру", elem, _elem));
            InfoLable.Text = "Добавил текстуру";
            CheckButtons(true);
        }

        private void DeleteTextureFromLog(int id)
        {
            DeleteImages.Add(ImagesURL[id]);
            for (int i = 0; i < neButtons.Count; i++)
            {
                if (neButtons[i].id == id)
                {
                    toolStrip1.Items.RemoveAt(neButtons[i].tsid);
                    neButtons.RemoveAt(i);
                    break;
                }
            }
            ImagesURL.RemoveAt(id);
            isLoad = false;
            LoadImages();
            foreach (var neb in neButtons)
            {
                if (neb.id > id)
                {
                    int newid = neb.id - 1;
                    neb.id = newid;
                    neb.toolStripButton.Image = Images.Images[newid];
                    neb.textname = ImagesURL[newid];
                }
            }
            foreach (var item in MyMap.NetworkElements.NetworkElements)
            {
                if (!item.delete)
                    if (item.texture.idimage > id)
                        item.texture.idimage--;
            }
            CheckButtons(false);
        }

        private void AddTextureFromLog(int id, string name)
        {
            ImagesURL.Insert(id, name);
            isLoad = false;
            LoadImages();
            foreach (var neb in neButtons)
            {
                if (neb.id >= id)
                {
                    int newid = neb.id + 1;
                    neb.id = newid;
                    neb.toolStripButton.Image = Images.Images[newid];
                    neb.textname = ImagesURL[newid];
                }
            }
            foreach (var item in MyMap.NetworkElements.NetworkElements)
            {
                if (!item.delete)
                    if (item.texture.idimage >= id)
                        item.texture.idimage++;
            }
            CheckButtons(false);
        }

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Событие тика таймера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Time_Tick(object sender, EventArgs e) => panel1.AutoScrollPosition = new Point(-asp.X, -asp.Y);
        /// <summary>
        /// Событие нажатия мыши по форме
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Click(object sender, EventArgs e) => focusbox.Focus();
        /// <summary>
        /// Событие нажатия мыши по области отрисовки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnT_Click(object sender, EventArgs e) => focusbox.Focus();
        /// <summary>
        /// Событие нажатия кнопки параметры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParamsMapClick(object sender, EventArgs e)
        {
            ColorDialogForm colorDialog = new ColorDialogForm();
            colorDialog.ShowDialog();
        }
        /// <summary>
        /// Событие нажатия кнопки посмотреть лог
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowLogClick(object sender, EventArgs e)
        {
            FormLog formlog = new FormLog(MyMap.log.Back);
            formlog.Show();
        }
        /// <summary>
        /// Событие нажатия кнопки создания новой карты с заданными параметрами
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateMapClick(object sender, EventArgs e)
        {
            MapControl mapControl = new MapControl(MyMap.mapSetting);
            mapControl.ShowDialog();
            MyMap.SetNewSettings(mapControl.mapSettings);
            //MyMap = new Map(mapControl.mapSettings);
            Text = MyMap.mapSetting.Name;
        }
        /// <summary>
        /// Событие нажатия кнопки сохранения карты
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveBtnClick(object sender, EventArgs e) => SaveMap(".ndm", "Network Design Map File");
        /// <summary>
        /// СОбытие нажатия кнопки открыть карту
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenMapClick(object sender, EventArgs e) => OpenMap(".ndm", "Network Design Map File");
        /// <summary>
        /// Событие нажатия кнопки сохранение шаблона карты
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveTemplateMapClick(object sender, EventArgs e) => SaveTemplateMap(".ndm", "Network Design Map File");
        /// <summary>
        /// Событие нажатия кнопки экспорт здания
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExporBuildClick(object sender, EventArgs e) => SaveBuild(".build", "Building File");
        /// <summary>
        /// Событие нажатия кнопки испорта здания
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImpotrBuildClick(object sender, EventArgs e) => OpenBuild(".build", "Building File");
        /// <summary>
        /// Собитые нажатия кнопки назад
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackBtn_Click(object sender, EventArgs e)
        {
            Element _elem = MyMap.log.DeleteLastBack(out Element elem, out int buildid);
            if (_elem.type != 7 & _elem.type != 6)
                PharseElem(_elem, elem, true);
            else
                PharseElem(_elem, elem, buildid);
            CheckButtons(false);
            Unfocus("Нажата стрелочка назад");
        }

        /// <summary>
        /// Событие нажатия кнопки вперед
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ForwardBrn_Click(object sender, EventArgs e)
        {
            Element _elem = MyMap.log.DeleteLastForward(out Element elem, out int buildid);
            if (_elem.type != 7 & _elem.type != 6)
                PharseElem(_elem, elem, false);
            else
                PharseElem(_elem, elem, buildid);
            CheckButtons(false);
            Unfocus("Нажата стрелочка вперед");
        }
        /// <summary>
        /// Собитые нажатия кнопки курсор
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CursorClick(object sender, EventArgs e) => MyMap.SetInstrument(0);
        /// <summary>
        /// Событие нажатия кнопки линия
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LineClick(object sender, EventArgs e) => MyMap.SetInstrument(1);
        /// <summary>
        /// Событие нажатия кнопки многоугольник
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PolygonClick(object sender, EventArgs e) => MyMap.SetInstrument(5);
        /// <summary>
        /// Событие нажатия кнопки добавить точку у многоугольника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddPP_Click(object sender, EventArgs e)
        {
            MyMap.Polygons.Polygons[activeElem.item].AddNewPoint();
            if (!MyMap.EditRects.edit_mode)
            {
                MyMap.SetInstrument(3);
                MyMap.RefreshEditRect();
            }
            else
            {
                MyMap.RefreshEditRect();
            }
            //MyMap.RefreshEditRect();
        }
        /// <summary>
        /// Событие нажатия кнопки удалить точку прямоугольника
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeletePP_Click(object sender, EventArgs e)
        {
            MyMap.Polygons.Polygons[activeElem.item].RemovePoint();
            if (!MyMap.EditRects.edit_mode)
            {
                MyMap.SetInstrument(3);
                MyMap.RefreshEditRect();
            }
            else
            {
                MyMap.RefreshEditRect();
            }
            //MyMap.RefreshEditRect();
        }
        /// <summary>
        /// Событие нажатия кнопки прямоугольник
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RectangleClick(object sender, EventArgs e) => MyMap.SetInstrument(2);
        /// <summary>
        /// Событие нажатия кнопки круг
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripButton6_Click_1(object sender, EventArgs e) => MyMap.SetInstrument(360);
        /// <summary>
        /// Событие нажатия кнопки фильтры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FiltersBtn_Click(object sender, EventArgs e)
        {
            FiltersForm ff = new FiltersForm();
            ff.ShowDialog();
            RefreshButtons();
        }
        /// <summary>
        /// Событие нажатия кнопки редактирования
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditClick(object sender, EventArgs e)
        {
            if (MyMap.EditRects.edit_mode)
            {
                MyMap.SetInstrument(0);
            }
            else
            {
                MyMap.SetInstrument(3);
                MyMap.RefreshEditRect();
            }
        }
        /// <summary>
        /// Событие нажатия кнопки удаление элемента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteBtn_Click(object sender, EventArgs e) => DeleteElem();
        /// <summary>
        /// Событие нажатия кнопки поиск элемента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchClick(object sender, EventArgs e) => Search();
        /// <summary>
        /// Событие нажатия кнопки преобразование в здание
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BuildBtn_Click(object sender, EventArgs e) => ToBuild();
        /// <summary>
        /// Событие нажатия кнопки вернуться на главный вид
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonReturnToMain_Click(object sender, EventArgs e)
        {
            drawLevel.Level = -1;
            drawLevel.Floor = -1;
            ReturnToMainBtn.Enabled = false;
            FloorUP.Visible = false;
            FloorDown.Visible = false;
            label1.Visible = false;
            floor_index = 0;
            floors_name = new List<string>();
            Unfocus("Нет активного элемента");
        }
        /// <summary>
        /// Событие нажатия кнопки входы в здание
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddEntranceBtn_Click(object sender, EventArgs e) => MyMap.SetInstrument(7);
        /// <summary>
        /// Событие нажатия кнопки входы проводов в здание
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddIWBtn_Click(object sender, EventArgs e) => MyMap.SetInstrument(6);
        /// <summary>
        /// Событие нажатия кнопки текст
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextClick(object sender, EventArgs e) => MyMap.SetInstrument(10);
        /// <summary>
        /// Событие нажатия кнопки сетевые элементы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NEClick(object sender, EventArgs e)
        {
            MyMap.SetInstrument(8);
            ImageTextures IT = new ImageTextures(ref MyMap.NetworkElements);
            IT.ShowDialog();
            /*if (IT.imageindex > -1)
            {
                neButtons.Add(new NEButton(new ToolStripButton(Images.Images[IT.imageindex]), IT.imageindex));
                toolStrip1.Items.Add(neButtons.Last().toolStripButton);
            }*/
        }
        /// <summary>
        /// Событие нажатия кнопки провода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NWClick(object sender, EventArgs e) => MyMap.SetInstrument(9);
        /// <summary>
        /// Событие нажатия кнопки добавить точку для провода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNWPClick(object sender, EventArgs e)
        {
            MyMap.NetworkWires.NetworkWires[activeElem.item].AddNewPoint();
            MyMap.RefreshEditRect();
        }
        /// <summary>
        /// Событие нажатия кнопки удалить точку для провода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelNWPClick(object sender, EventArgs e)
        {
            MyMap.NetworkWires.NetworkWires[activeElem.item].RemovePoint();
            MyMap.RefreshEditRect();
        }
        /// <summary>
        /// Событие прокрутки основной панели
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel1_Scroll(object sender, ScrollEventArgs e) => asp = panel1.AutoScrollPosition;
        /// <summary>
        /// Событие клика по меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuStrip1_MouseClick(object sender, MouseEventArgs e) => focusbox.Focus();
        /// <summary>
        /// Событие клика по меню инстурментов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStrip1_MouseClick(object sender, MouseEventArgs e) => focusbox.Focus();
        /// <summary>
        /// Событие прокрутки трэкбара (зум)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private unsafe void trackBar1_Scroll(object sender, EventArgs e)
        {
            zoom = (double)trackBar1.Value / 10d;
            MyMap.ResizeRenderingArea();
        }
        /// <summary>
        /// Событие закрытия формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ColorSettings.Save(colorSettings);
            SaveTextures(ImagesURL);
            Parametrs.Save(parametrs);
            ID_TEXT temp = new ID_TEXT();
            foreach (var n in neButtons)
                temp.ADD(n.id, n.textname);
            NEButton.Save(temp);
            Application.Exit();
        }
        /// <summary>
        /// Событие нажатия кнопки на этаж выше
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FloorUP_Click(object sender, EventArgs e)
        {
            if (floor_index != floors_name.Count - 1)
            {
                floor_index++;
                label1.Text = floors_name[floor_index];
                drawLevel.Floor = floor_index;
            }
        }
        /// <summary>
        /// Событие нажатия кнопки на этаж ниже
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FloorDown_Click(object sender, EventArgs e)
        {
            if (floor_index != 0)
            {
                floor_index--;
                label1.Text = floors_name[floor_index];
                drawLevel.Floor = floor_index;
            }
        }
        #endregion
        #region Генерация текстур
        static public ListView LoadImages()
        {
            ListView listView1 = new ListView();
            if (!isLoad)
                Textures.Clear();
            int i = 0;
            Images = new ImageList();
            Images.ImageSize = new Size(200, 200);
            listView1.Clear();
            listView1.LargeImageList = Images;
            listView1.SmallImageList = Images;
            bool noexist = false;
            for (int j = 0; j < ImagesURL.Count; j++)
            {
                if (File.Exists(Application.StartupPath + @"\Textures\" + ImagesURL[j]))
                {
                    Image image = Image.FromFile(Application.StartupPath + @"\Textures\" + ImagesURL[j]);
                    Bitmap bitmap = new Bitmap(image);
                    if (image.Height != 1024 | image.Width != 1024)
                    {
                        bitmap.Dispose();
                        bitmap = new Bitmap(image, 1024, 1024);
                        image.Dispose();
                        bitmap.Save(Application.StartupPath + @"\Textures\" + ImagesURL[j]);
                        bitmap.Dispose();
                        image = Image.FromFile(Application.StartupPath + @"\Textures\" + ImagesURL[j]);
                    }
                    Images.Images.Add(image);
                    /*double koef = image.Height / 1000;
                    if (image.Width / 1000 > koef)
                        koef = image.Width / 1000;
                    if (koef > 1)
                    {
                        Bitmap bitmap = new Bitmap(image, (int)(image.Width * koef), (int)(image.Height * koef));
                        bitmap.Save(Application.StartupPath + @"\Textures\" + ImagesURL[j]);
                        image = Image.FromFile(Application.StartupPath + @"\Textures\" + ImagesURL[j]);
                        Images.Images.Add(image);
                    }
                    else
                    {
                        Images.Images.Add(image);
                    }*/
                    if (isLoad)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = "";
                        item.ImageIndex = i;
                        listView1.Items.Add(item);
                        i++;
                    }
                    else
                    {
                        if (!GenTex(j, Application.StartupPath + @"\Textures\" + ImagesURL[j]))
                        {
                            ImagesURL.RemoveAt(j);
                            j--;
                        }
                        else
                        {
                            ListViewItem item = new ListViewItem();
                            item.Text = "";
                            item.ImageIndex = i;
                            listView1.Items.Add(item);
                            i++;
                        }
                    }
                }
                else
                {
                    noexist = true;
                    ImagesURL.RemoveAt(j);
                    j--;
                }
            }
            if (noexist)
            {
                MessageBox.Show("Один или несколько файлов недоступны");
            }
            isLoad = true;
            return listView1;
        }
        static public List<string> OpenTextures()
        {
            if (!Directory.Exists(Application.StartupPath + @"\Textures"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\Textures");
                SaveTextures(new List<string>());
                return new List<string>();
            }
            if (!File.Exists(Application.StartupPath + @"\Textures\ListOfTextures"))
            {
                SaveTextures(new List<string>());
                return new List<string>();
            }
            XmlSerializer formatter = new XmlSerializer(typeof(List<string>));
            using (FileStream fs = new FileStream(Application.StartupPath + @"\Textures\ListOfTextures", FileMode.Open))
            {
                return (List<string>)formatter.Deserialize(fs);
            }
        }
        static public void _OpenTextures()
        {
            if (!Directory.Exists(Application.StartupPath + @"\Textures"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\Textures");
                SaveTextures(new List<string>());
                ImagesURL = new List<string>();
            }
            if (!File.Exists(Application.StartupPath + @"\Textures\ListOfTextures"))
            {
                SaveTextures(new List<string>());
                ImagesURL =  new List<string>();
            }
            XmlSerializer formatter = new XmlSerializer(typeof(List<string>));
            using (FileStream fs = new FileStream(Application.StartupPath + @"\###tempdirectory._temp###\ListOfTextures", FileMode.Open))
            {
                List<string> textures = (List<string>)formatter.Deserialize(fs);
                List<string> _textures = new List<string>();
                foreach (var url in ImagesURL)
                    _textures.Add(url);
                ImagesURL.Clear();
                ImagesURL.AddRange(textures);
                foreach (var url in ImagesURL)
                    if (!File.Exists(Application.StartupPath + @"\Textures\" + url))
                        File.Copy(Application.StartupPath + @"\###tempdirectory._temp###\" + url, Application.StartupPath + @"\Textures\" + url);
                int id = -1;
                for (int i = 0; i < _textures.Count; i++)
                {
                    id = -1;
                    for (int j = 0; j < ImagesURL.Count; j++)
                    {
                        if (ImagesURL[j] == _textures[i])
                        {
                            id = i;
                            break;
                        }
                    }
                    if (id != -1)
                    {
                        _textures.RemoveAt(id);
                        i--;
                    }
                    else
                        ImagesURL.Add(_textures[i]);
                }
                isLoad = false;
                LoadImages();
            }
        }
        static public void SaveTextures(List<string> imglist)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<string>));
            using (FileStream fs = new FileStream(Application.StartupPath + @"\Textures\ListOfTextures", FileMode.Create))
            {
                formatter.Serialize(fs, imglist);
            }
        }

        private void panel1_Scroll_1(object sender, ScrollEventArgs e) => asp = panel1.AutoScrollPosition;
        //копировать
        private void CopyBtn_Click(object sender, EventArgs e)
        {

        }
        //Экспорт
        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                DrawLevel dl = drawLevel;
                MyMap.RenderTimer.Stop();
                if (MessageBox.Show("Некоторые элементы могут быть заменены и удалены. Продолжить?") == DialogResult.OK)
                {
                    string path = fbd.SelectedPath;
                    drawLevel = new DrawLevel(-1, -1);
                    Unfocus("Идет экспорт");
                    MyMap.DrawingWOF();
                    Bitmap image = GetBitmap();
                    image.Save(path + "/Главный вид.png");
                    for (int i = 0; i < MyMap.Buildings.Buildings.Count; i++)
                    {
                        string buildname = path + "/Здание " + i + " " + MyMap.Buildings.Buildings[i].Name + "/";
                        if (Directory.Exists(buildname))
                            Directory.Delete(buildname, true);
                        Directory.CreateDirectory(buildname);
                        for (int j = 0; j < MyMap.Buildings.Buildings[i].floors_name.Count; j++)
                        {
                            drawLevel = new DrawLevel(i, j);
                            MyMap.DrawingWOF();
                            Bitmap _image = GetBitmap();
                            _image.Save(buildname + MyMap.Buildings.Buildings[i].floors_name[j].ToString() + ".png");
                        }
                    }
                }
                drawLevel = dl;
                MyMap.RenderTimer.Start();
                Unfocus("Экспорт завершен");
            }
        }

        private Bitmap GetBitmap()
        {
            Bitmap bitmap = new Bitmap(AnT.Width, AnT.Height);
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            //Gl.glReadBuffer(Gl.GL_FRONT);
            Gl.glReadPixels(0, 0, bitmap.Width, bitmap.Height, Gl.GL_BGR_EXT, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);
            bitmap.UnlockBits(bitmapData);
            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
            return bitmap;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                Filter = "Text File" + "|*" + ".txt"
            };
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename;
                if (saveFileDialog1.FileName.Contains(".txt"))
                {
                    filename = saveFileDialog1.FileName;
                }
                else
                {
                    filename = saveFileDialog1.FileName + ".txt";
                }
                MyMap.ExportListNE(filename);
            }
        }

        private void pingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Timer PingDeviceTimer = new Timer();
            PingDeviceTimer.Interval = 1000;
            PingDeviceTimer.Tick += PingDeviceTimer_Tick;
            if (!isPing)
            {
                PingDeviceTimer.Start();
                isPing = true;
            }
            else
            {
                PingDeviceTimer.Stop();
                isPing = false;
            }
        }

        private void PingDeviceTimer_Tick(object sender, EventArgs e)
        {
            if (isReady)
            {
                isReady = false;
                foreach (var ne in MyMap.NetworkElements.NetworkElements)
                {
                    ne.Ping();
                }
                isReady = true;
            }
        }

        private bool isReady = true;
        private bool isPing = false;

        /*private void panel1_Scroll_1(object sender, ScrollEventArgs e)
        {
            if (e.NewValue != 0)
            {
                if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
                    asp = new Point(-panel1.AutoScrollPosition.X - e.NewValue, -panel1.AutoScrollPosition.Y);
                else
                    asp = new Point(-panel1.AutoScrollPosition.X, -panel1.AutoScrollPosition.Y - e.NewValue);
            }
        }*/

        /// <summary>
        /// Генерация текстуры
        /// </summary>
        /// <param name="id">Идентификатор текстуры в списке ссылок</param>
        /// <param name="url">Ссылка</param>
        public static bool GenTex(int id, string url)
        {
            /*Gl.glGenTextures(1, Textures);
            glBindTexture(GL_TEXTURE_2D, textures[0]);
            glTexImage2D(GL_TEXTURE_2D, 0, 3, (GLsizei)texture1.width(), (GLsizei)texture1.height(), 0, GL_RGBA, GL_UNSIGNED_BYTE, texture1.bits());
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
            glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);*/
            // создаем изображение с идентификатором imageId 
            Il.ilGenImages(1, out int imageId);
            // делаем изображение текущим 
            Il.ilBindImage(imageId);
            // пробуем загрузить изображение 
            if (Il.ilLoadImage(url))
            {
                // если загрузка прошла успешно 
                // сохраняем размеры изображения 
                int width = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
                int height = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);
                // определяем число бит на пиксель 
                int bitspp = Il.ilGetInteger(Il.IL_IMAGE_BITS_PER_PIXEL);
                if (bitspp == 32 | bitspp == 24  | bitspp == 16)
                {
                    switch (bitspp) // в зависимости от полученного результата 
                    {
                        case 16:
                            Textures.Add(MakeGlTexture(Gl.GL_RGB16, Il.ilGetData(), width, height));
                            break;
                        case 24:
                            Textures.Add(MakeGlTexture(Gl.GL_RGB, Il.ilGetData(), width, height));
                            break;
                        case 32:
                            Textures.Add(MakeGlTexture(Gl.GL_RGBA, Il.ilGetData(), width, height));
                            break;
                    }
                }
                else
                {
                    Il.ilDeleteImages(1, ref imageId);
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Создание текстуры в памяти OpenGL
        /// </summary>
        /// <param name="Format">Формат изображения</param>
        /// <param name="pixels">Пиксели</param>
        /// <param name="w">Ширина</param>
        /// <param name="h">Высота</param>
        /// <returns></returns>
        public static uint MakeGlTexture(int Format, IntPtr pixels, int w, int h)
        {
            // идентификатор текстурного объекта 
            uint texObject;

            // генерируем текстурный объект 
            Gl.glGenTextures(1, out texObject);

            // устанавливаем режим упаковки пикселей 
            Gl.glPixelStorei(Gl.GL_UNPACK_ALIGNMENT, 1);

            // создаем привязку к только что созданной текстуре 
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texObject);

            // устанавливаем режим фильтрации и повторения текстуры 
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
            /*Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_NEAREST);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_NEAREST);*/
            Gl.glTexEnvf(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_REPLACE);
            Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_MODULATE);
            //Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_GENERATE_MIPMAP, Gl.GL_TRUE);

            // создаем RGB или RGBA текстуру 
            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Format, w, h, 0, Format, Gl.GL_UNSIGNED_BYTE, pixels);

            // возвращаем идентификатор текстурного объекта 
            return texObject;
        }
        #endregion
    }
}
