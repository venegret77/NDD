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
using NetworkDesign.Buildings;
using System.Threading;

namespace NetworkDesign
{
    public partial class MainForm : Form
    {
        #region Инициализация
        #region Объявление переменных
        private bool isResizeMap = false;
        Element elem = new Element();
        Element _elem = new Element();
        public static UserPrincipal user;
        public static bool edit;
        static public Map MyMap;
        static public DrawLevel drawLevel;
        static public ColorSettings colorSettings = new ColorSettings();
        static public Parametrs parametrs = new Parametrs();
        //
        static public ListOfTextures ImagesURL = new ListOfTextures();
        static public List<uint> Textures = new List<uint>();
        static public List<uint> MTTextures = new List<uint>();
        static public ListOfTextures DeleteImages = new ListOfTextures();
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
        public static bool isInitMap = false;
        static public double wkoef = 1;
        static public double hkoef = 1;
        static public int imgwidth = 100;
        static public int imgheight = 100;

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
        public MainForm(SizeRenderingArea mapSettings)
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            StartPosition = FormStartPosition.CenterScreen;
            //user = Autorisation(out edit);
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
            AnT.GotFocus += AnT_GotFocus;
            AnT.MouseDoubleClick += AnT_MouseDoubleClick;
            // 
            AnT.InitializeContexts();
            MyMap = new Map(mapSettings);
            Text = mapSettings.Name;
            drawLevel.Level = -1;
            drawLevel.Floor = -1;
            _Height = AnT.Height;
            _Width = AnT.Width;
            panel1.Parent = this;
            trackBar1.Parent = this;
            trackBar1.BringToFront();
            colorSettings = ColorSettings.Open();
            PingDeviceTimer.Interval = colorSettings.TimerInterval;
            if (colorSettings.backgroundurl != "")
            {
                if (File.Exists(Application.StartupPath + @"\Textures\" + colorSettings.backgroundurl))
                    GenTex(Application.StartupPath + @"\Textures\" + colorSettings.backgroundurl);
                else
                    colorSettings.backgroundurl = "";
            }

            parametrs = Parametrs.Open();
            ImagesURL = OpenTextures();
            LoadImages();
            ID_TEXT temp = NEButton.Open();
            for (int i = 0; i < temp.TEXT.Count; i++)
            {
                int id = ImagesURL.IndexOf(temp.TEXT[i]);
                if (id != -1)
                {
                    neButtons.Add(new NEButton(new ToolStripButton(Images.Images[id]), id, ImagesURL.Textures[id].URL, toolStrip1.Items.Count));
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
            линияToolStripMenuItem.Checked = true;
        }

        private void AnT_GotFocus(object sender, EventArgs e)
        {
            if (panel1.AutoScrollPosition != asp)
                panel1.AutoScrollPosition = new Point(-asp.X, -asp.Y);
        }
        #endregion
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
                    MyMap.NetworkElements.TempNetworkElement = new NetworkElement(new Texture(colorSettings.TextureWidth, new Point(x, y), IT.imageindex, ImagesURL.Textures[IT.imageindex].URL), drawLevel);
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

        static public TextBox textBox;
        TextBox _textBox;
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
                    Text = "",
                    Multiline = true,
                    Font = new Font(Font.FontFamily, colorSettings.fontsize)
                };
            }
            Size size = TextRenderer.MeasureText("  ", textBox.Font);
            textBox.Width = size.Width + 2;
            textBox.Height = size.Height;
            textBox.Focus();
            textBox.KeyDown += TextBox_KeyDown;
            textBox.LostFocus += TextBox_LostFocus;
            textBox.KeyUp += TextBox_KeyUp;
        }

        private int textid = -1;

        private void MouseText(int id)
        {
            textid = id;
            focusbox.Focus();
            Point location = GenZoomPoint(MyMap.MyTexts.MyTexts[textid].location);
            Point _location;
            if (drawLevel.Level == -1)
                _location = new Point(location.X + (int)((double)MyMap.sizeRenderingArea.Width * zoom / 2d), (int)((double)MyMap.sizeRenderingArea.Height * zoom / 2d) - location.Y);
            else
                _location = new Point(location.X + (int)((double)MyMap.Buildings.Buildings[drawLevel.Level].sizeRenderingArea.Width * zoom / 2d), (int)((double)MyMap.Buildings.Buildings[drawLevel.Level].sizeRenderingArea.Height * zoom / 2d) - location.Y);
            float fontsize = MyMap.MyTexts.MyTexts[textid].fontsize;
            Size size = new Size((int)((double)MyMap.MyTexts.MyTexts[textid].size.Width * zoom), (int)((double)MyMap.MyTexts.MyTexts[textid].size.Height * zoom));
            string text = MyMap.MyTexts.MyTexts[textid].text;
            MyMap.MyTexts.MyTexts[textid].delete = true;
            if (_textBox == null || _textBox.Text == "")
            {
                _textBox = new TextBox
                {
                    Parent = AnT,
                    BackColor = Color.White,
                    BorderStyle = BorderStyle.None,
                    Visible = true,
                    Enabled = true,
                    Location = _location,
                    Size = size,
                    Font = new Font(FontFamily.GenericSansSerif, fontsize * (float)zoom),
                    Text = text,
                    Multiline = true
                };
            }
            _textBox.Focus();
            _textBox.KeyDown += _TextBox_KeyDown;
            _textBox.LostFocus += _TextBox_LostFocus;
            _textBox.KeyUp += _TextBox_KeyUp;
            size = TextRenderer.MeasureText(_textBox.Text, _textBox.Font);
            _textBox.Width = size.Width + 2;
            _textBox.Height = size.Height;
        }

        private void _TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            Size size = TextRenderer.MeasureText(_textBox.Text, _textBox.Font);
            _textBox.Width = size.Width + 2;
            _textBox.Height = size.Height;
        }

        private void _TextBox_LostFocus(object sender, EventArgs e)
        {
            if (_textBox.Text != "" & _textBox.Text != " ")
            {
                if (textid == -1)
                {
                    MyMap.MyTexts.Add(new MyText(drawLevel, new Point(MyMap.RecalcMouseX(x), MyMap.RecalcMouseY(y)), _textBox));
                    int lastindex = MyMap.MyTexts.MyTexts.Count - 1;
                    Element elem = new Element(10, lastindex, MyMap.MyTexts.MyTexts[lastindex].Clone(), -1);
                    Element _elem = new Element(10, lastindex, new MyText(), -1);
                    MyMap.log.Add(new LogMessage("Добавил надпись", elem, _elem));
                    InfoLable.Text = "Добавил надпись";
                }
                else
                {
                    MyMap.MyTexts.MyTexts[textid].fontsize = _textBox.Font.Size;
                    MyMap.MyTexts.MyTexts[textid].text = _textBox.Text;
                    MyMap.MyTexts.MyTexts[textid].delete = false;
                    MyMap.MyTexts.MyTexts[textid].GenNewTexture();
                    UpdateTextLogAdd(textid);
                    textid = -1;
                }
                CheckButtons(true);
            }
            else
            {
                if (textid != -1)
                    MyMap.MyTexts.MyTexts[textid].delete = false;
            }
            _textBox.Dispose();
        }

        //bool isControl = false;

        private void _TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            /*if (e.KeyCode == Keys.ControlKey)
            {
                isControl = true;
            }*/
            if (!e.Control & e.KeyCode == Keys.Enter)
            {
                focusbox.Focus();
            }
            if (e.Control & e.KeyCode == Keys.Up)
            {
                _textBox.Font = new Font(_textBox.Font.FontFamily, _textBox.Font.Size + 1);
            }
            if (e.Control & e.KeyCode == Keys.Down)
            {
                if (colorSettings.fontsize > 1)
                {
                    _textBox.Font = new Font(_textBox.Font.FontFamily, _textBox.Font.Size - 1);
                }
            }
            /*if (e.KeyCode != Keys.ControlKey)
            {
                isControl = false;
            }*/
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            Size size = TextRenderer.MeasureText(textBox.Text, textBox.Font);
            textBox.Width = size.Width + 2;
            textBox.Height = size.Height;
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
                else
                {
                    MyMap.MyTexts.MyTexts[textid].fontsize = textBox.Font.Size;
                    MyMap.MyTexts.MyTexts[textid].text = textBox.Text;
                    MyMap.MyTexts.MyTexts[textid].delete = false;
                    MyMap.MyTexts.MyTexts[textid].GenNewTexture();
                    UpdateTextLogAdd(textid);
                    textid = -1;
                }
                CheckButtons(true);
            }
            else
            {
                if (textid != -1)
                    MyMap.MyTexts.MyTexts[textid].delete = false;
            }
            textBox.Dispose();
        }

        //bool isControl = false;

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            /*if (e.KeyCode == Keys.ControlKey)
            {
                isControl = true;
            }*/
            if (!e.Control & e.KeyCode == Keys.Enter)
            {
                focusbox.Focus();
            }
            if (e.Control & e.KeyCode == Keys.Up)
            {
                textBox.Font = new Font(textBox.Font.FontFamily, textBox.Font.Size + 1);
                colorSettings.fontsize++;
                colorDialog.numericUpDown6.Value = (decimal)colorSettings.fontsize;
            }
            if (e.Control & e.KeyCode == Keys.Down)
            {
                if (colorSettings.fontsize > 1)
                {
                    textBox.Font = new Font(textBox.Font.FontFamily, textBox.Font.Size - 1);
                    colorSettings.fontsize--;
                    colorDialog.numericUpDown6.Value = (decimal)colorSettings.fontsize;
                }
            }
            /*if (e.KeyCode != Keys.ControlKey)
            {
                isControl = false;
            }*/
        }

        private void MouseNEElems(int x, int y)
        {
            if (!MyMap.NetworkElements.step)
            {
                MyMap.NetworkElements.step = true;
                MyMap.NetworkElements.TempNetworkElement = new NetworkElement(new Texture(colorSettings.TextureWidth, new Point(x, y), MyMap.Instrument - nebutnscount, ImagesURL.Textures[MyMap.Instrument - nebutnscount].URL), drawLevel);
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
        #region Работа с мышью на области отрисовки
        /// <summary>
        /// Событие нажатия мыши по области отрисовки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnT_MouseDown(object sender, MouseEventArgs e)
        {
            int y = MyMap.RecalcMouseY(e.Y);
            int x = MyMap.RecalcMouseX(e.X);
            if (e.Button == MouseButtons.Left)
            {
                if (drawLevel.Level == -1 & MyMap.Instrument == 0)
                {
                    if (ChechEdges(x, y))
                    {
                        elem = new Element(14, drawLevel.Level, MyMap.sizeRenderingArea, -2);
                        isResizeMap = true;
                        MyMap.RenderTimer.Stop();
                    }
                }
                else if (MyMap.Instrument == 0 & drawLevel.Level != -1)
                {
                    if (ChechEdges(x, y, drawLevel.Level))
                    {
                        elem = new Element(14, drawLevel.Level, MyMap.Buildings.Buildings[drawLevel.Level].Clone(), -2);
                        isResizeMap = true;
                        MyMap.RenderTimer.Stop();
                    }
                }
            }
            if (MyMap.Instrument >= nebutnscount & MyMap.Instrument != 360)
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
                    switch (MyMap.Instrument)
                    {
                        case 0:
                            if (!MyMap.isMove)
                            {
                                stopwatch.Restart();
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
                                    if (!MyMap.Buildings.Buildings[drawLevel.Level].InputWires.step)
                                    {
                                        MyMap.Buildings.Buildings[drawLevel.Level].AddIWInBuild(x, y, drawLevel);
                                    }
                                    else
                                    {
                                        MyMap.Buildings.Buildings[drawLevel.Level].AddIWInBuild(x, y, drawLevel);
                                        int lastindex = MyMap.Buildings.Buildings[drawLevel.Level].InputWires.InputWires.Circles.Count - 1;
                                        Element elem = new Element(6, lastindex, MyMap.Buildings.Buildings[drawLevel.Level].InputWires.InputWires.Circles[lastindex].Clone(), -1);
                                        Element _elem = new Element(6, lastindex, new Circle(), -1);
                                        MyMap.log.Add(new LogMessage("Добавил проход провода через потолок", elem, _elem, drawLevel.Level));
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
                    switch (MyMap.Instrument)
                    {
                        case 0:
                            SelectItems(x, y);
                            if (activeElem.type == 4)
                            {
                                MyMap.ResizeRenderingArea(activeElem.item);
                                drawLevel.Level = activeElem.item;
                                drawLevel.Floor = 0;
                                ReturnToMainBtn.Enabled = true;
                                UpgrateFloors();
                                Unfocus("Выбрано здание '" + MyMap.Buildings.Buildings[activeElem.item].Name + "'");
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
            int y = MyMap.RecalcMouseY(e.Y);
            int x = MyMap.RecalcMouseX(e.X);
            if (e.Button == MouseButtons.Left)
            {
                switch (MyMap.Instrument)
                {
                    case 0:
                        if (MyMap.SearchNE(x, y))
                        {
                            CheckButtons(true);
                            break;
                        }
                        if (MyMap.SearchNW(x, y))
                            break;
                        if (MyMap.SearchText(x, y, out int id))
                        {
                            UpdateTextLogAdd(id);
                            MouseText(id);
                        }
                        MyMap.SearchBuild(x, y);
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
            if (isResizeMap)
            {
                MyMap.RenderTimer.Start();
                if (drawLevel.Level == -1)
                {
                    MyMap.ResizeRenderingArea();
                    _elem = new Element(14, drawLevel.Level, MyMap.sizeRenderingArea, -2);
                }
                else
                {
                    MyMap.ResizeRenderingArea(drawLevel.Level);
                    MyMap.Buildings.Buildings[drawLevel.Level].RefreshLocal();
                    _elem = new Element(14, drawLevel.Level, MyMap.Buildings.Buildings[drawLevel.Level].Clone(), -2);
                }
                MyMap.log.Add(new LogMessage("Изменил размеры области отрисовки", elem, _elem));
                isResizeMap = false;
                CheckButtons(true);
            }
            if (e.Button == MouseButtons.Left)
            {
                switch (MyMap.Instrument)
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
                if (isResizeMap)
                {
                    if (drawLevel.Level == -1)
                        ChechEdges(x, y);
                    else
                        ChechEdges(x, y, drawLevel.Level);
                }
                else
                {
                    if (drawLevel.Level == -1 & MyMap.Instrument == 0)
                    {
                        if (ChechEdges(x, y))
                        {
                            elem = new Element(14, drawLevel.Level, MyMap.sizeRenderingArea, -2);
                            isResizeMap = true;
                            MyMap.RenderTimer.Stop();
                        }
                    }
                    else if (drawLevel.Level != -1 & MyMap.Instrument == 0)
                    {
                        if (ChechEdges(x, y, drawLevel.Level))
                        {
                            elem = new Element(14, drawLevel.Level, MyMap.Buildings.Buildings[drawLevel.Level].Clone(), -2);
                            isResizeMap = true;
                            MyMap.RenderTimer.Stop();
                        }
                    }
                }
            }
            if (MyMap.Instrument >= nebutnscount & MyMap.Instrument != 360)
            {
                if (MyMap.NetworkElements.step)
                {
                    MyMap.NetworkElements.TempNetworkElement.SetPoint(x, y);
                }
            }
            else
            {
                switch (MyMap.Instrument)
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
                                    if (type == 3)
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

                        if (drawLevel.Level == -1)
                        {
                            if (MyMap.Buildings.Buildings[activeElem.item].InputWires.step)
                            {
                                MyMap.Buildings.Buildings[activeElem.item].MoveIW(x, y);
                            }
                        }
                        else
                        {
                            MyMap.Buildings.Buildings[drawLevel.Level].MoveIWInBuild(x, y);
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
        #endregion
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
        /// Проверка границ области отрисовки для главного вида
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private bool ChechEdges(int x, int y)
        {
            int dif = 0;
            int n = (int)(10d * zoom);
            bool refresh = false;
            double Left = (double)MyMap.sizeRenderingArea.Left * zoom;
            double Right = (double)MyMap.sizeRenderingArea.Right * zoom;
            double Top = (double)MyMap.sizeRenderingArea.Top * zoom;
            double Bottom = (double)MyMap.sizeRenderingArea.Bottom * zoom;
            if (x < Left + n)
            {
                dif = (int)(x - n - Left);
                MyMap.sizeRenderingArea = new SizeRenderingArea(MyMap.sizeRenderingArea.Name, MyMap.sizeRenderingArea.Left + dif,
                    MyMap.sizeRenderingArea.Right - dif, MyMap.sizeRenderingArea.Top, MyMap.sizeRenderingArea.Bottom);
                refresh = true;
            }
            if (x > Right - n)
            {
                dif = (int)(x + n - Right);
                MyMap.sizeRenderingArea = new SizeRenderingArea(MyMap.sizeRenderingArea.Name, MyMap.sizeRenderingArea.Left - dif,
                    MyMap.sizeRenderingArea.Right + dif, MyMap.sizeRenderingArea.Top, MyMap.sizeRenderingArea.Bottom);
                refresh = true;
            }
            if (y > Top - n)
            {
                dif = (int)(y + n - Top);
                //Top = y + n;
                //Bottom = -Top;
                MyMap.sizeRenderingArea = new SizeRenderingArea(MyMap.sizeRenderingArea.Name, MyMap.sizeRenderingArea.Left,
                    MyMap.sizeRenderingArea.Right, MyMap.sizeRenderingArea.Top + dif, MyMap.sizeRenderingArea.Bottom - dif);
                refresh = true;
            }
            if (y < Bottom + n)
            {
                dif = (int)(y - n - Bottom);
                //Bottom = y - n;
                //Top = -Bottom;
                MyMap.sizeRenderingArea = new SizeRenderingArea(MyMap.sizeRenderingArea.Name, MyMap.sizeRenderingArea.Left,
                    MyMap.sizeRenderingArea.Right, MyMap.sizeRenderingArea.Top - dif, MyMap.sizeRenderingArea.Bottom + dif);
                refresh = true;
            }
            if (refresh)
            {
                int Height = (int)((double)MyMap.sizeRenderingArea.Height * MainForm.zoom);
                int Width = (int)((double)MyMap.sizeRenderingArea.Width * MainForm.zoom);
                /*int Left = (int)((double)MyMap.sizeRenderingArea.Left * MainForm.zoom);
                int Right = (int)((double)MyMap.sizeRenderingArea.Right * MainForm.zoom);
                int Top = (int)((double)MyMap.sizeRenderingArea.Top * MainForm.zoom);
                int Bottom = (int)((double)MyMap.sizeRenderingArea.Bottom * MainForm.zoom);*/
                MainForm.AnT.Height = Height;
                MainForm.AnT.Width = Width;
                //MyMap.ResizeRenderingArea();
                refresh = false;
                return true;
            }
            return false;
        }
        private bool ChechEdges(int x, int y, int id)
        {
            int dif = 0;
            double pk = MyMap.Buildings.Buildings[id].pk;
            //Считать все края с учетом коэффициента, пересчитывать локальную фигуру и локальные входы и входы проводов.
            int n = (int)(10d * zoom);
            bool refresh = false;
            double Left = (double)MyMap.Buildings.Buildings[id].sizeRenderingArea.Left * zoom;
            double Right = (double)MyMap.Buildings.Buildings[id].sizeRenderingArea.Right * zoom;
            double Top = (double)MyMap.Buildings.Buildings[id].sizeRenderingArea.Top * zoom;
            double Bottom = (double)MyMap.Buildings.Buildings[id].sizeRenderingArea.Bottom * zoom;
            if (x < Left + n)
            {
                dif = (int)(x - n - Left);
                MyMap.Buildings.Buildings[id].sizeRenderingArea = new SizeRenderingArea(MyMap.Buildings.Buildings[id].sizeRenderingArea.Name, MyMap.Buildings.Buildings[id].sizeRenderingArea.Left + dif,
                     MyMap.Buildings.Buildings[id].sizeRenderingArea.Right - dif, MyMap.Buildings.Buildings[id].sizeRenderingArea.Top - (int)((double)dif / pk), MyMap.Buildings.Buildings[id].sizeRenderingArea.Bottom + (int)((double)dif / pk));
                refresh = true;
            }
            if (x > Right - n)
            {
                dif = (int)(x + n - Right);
                MyMap.Buildings.Buildings[id].sizeRenderingArea = new SizeRenderingArea(MyMap.Buildings.Buildings[id].sizeRenderingArea.Name, MyMap.Buildings.Buildings[id].sizeRenderingArea.Left - dif,
                     MyMap.Buildings.Buildings[id].sizeRenderingArea.Right + dif, MyMap.Buildings.Buildings[id].sizeRenderingArea.Top + (int)((double)dif / pk), MyMap.Buildings.Buildings[id].sizeRenderingArea.Bottom - (int)((double)dif / pk));
                refresh = true;
            }
            if (y > Top - n)
            {
                dif = (int)(y + n - Top);
                MyMap.Buildings.Buildings[id].sizeRenderingArea = new SizeRenderingArea(MyMap.Buildings.Buildings[id].sizeRenderingArea.Name, MyMap.Buildings.Buildings[id].sizeRenderingArea.Left - (int)((double)dif * pk),
                     MyMap.Buildings.Buildings[id].sizeRenderingArea.Right + (int)((double)dif * pk), MyMap.Buildings.Buildings[id].sizeRenderingArea.Top + dif, MyMap.Buildings.Buildings[id].sizeRenderingArea.Bottom - dif);
                refresh = true;
            }
            if (y < Bottom + n)
            {
                dif = (int)(y - n - Bottom);
                MyMap.Buildings.Buildings[id].sizeRenderingArea = new SizeRenderingArea(MyMap.Buildings.Buildings[id].sizeRenderingArea.Name, MyMap.Buildings.Buildings[id].sizeRenderingArea.Left + (int)((double)dif * pk),
                     MyMap.Buildings.Buildings[id].sizeRenderingArea.Right - (int)((double)dif * pk), MyMap.Buildings.Buildings[id].sizeRenderingArea.Top - dif, MyMap.Buildings.Buildings[id].sizeRenderingArea.Bottom + dif);
                refresh = true;
            }
            if (refresh)
            {
                int Height = (int)((double)MyMap.Buildings.Buildings[id].sizeRenderingArea.Height * MainForm.zoom);
                int Width = (int)((double)MyMap.Buildings.Buildings[id].sizeRenderingArea.Width * MainForm.zoom);
                /*int Left = (int)((double)Buildings.Buildings[buildid].sizeRenderingArea.Left * MainForm.zoom);
                int Right = (int)((double)Buildings.Buildings[buildid].sizeRenderingArea.Right * MainForm.zoom);
                int Top = (int)((double)Buildings.Buildings[buildid].sizeRenderingArea.Top * MainForm.zoom);
                int Bottom = (int)((double)Buildings.Buildings[buildid].sizeRenderingArea.Bottom * MainForm.zoom);*/
                MainForm.AnT.Height = Height;
                MainForm.AnT.Width = Width;
                //MyMap.ResizeRenderingArea(id);
                refresh = false;
                return true;
            }
            return false;
        }
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
                if (g.Name.ToLower().Contains("администратор") | g.Name.ToLower().Contains("administrator"))
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
                    InfoLable.Text = "Выбрана линия ";
                    DeleteBtn.Enabled = true;
                    CopyBtn.Enabled = true;
                    return true;
                case 2:
                    MyMap.Rectangles.Choose(activeElem.item);
                    InfoLable.Text = "Выбран прямоугольник ";
                    DeleteBtn.Enabled = true;
                    CopyBtn.Enabled = true;
                    if (drawLevel.Level == -1)
                        ToBuildBtn.Enabled = true;
                    return true;
                case 3:
                    MyMap.Polygons.Choose(activeElem.item);
                    InfoLable.Text = "Выбран многоугольник ";
                    DeleteBtn.Enabled = true;
                    CopyBtn.Enabled = true;
                    if (drawLevel.Level == -1)
                        ToBuildBtn.Enabled = true;
                    AddPP.Visible = true;
                    //AddPP.Enabled = true;
                    DeletePP.Visible = true;
                    //DeletePP.Enabled = true;
                    return true;
                case 4:
                    MyMap.Buildings.Choose(activeElem.item);
                    InfoLable.Text = "Выбрано здание '" + MyMap.Buildings.Buildings[activeElem.item].Name + "'";
                    DeleteBtn.Enabled = true;
                    if (drawLevel.Level == -1)
                    {
                        ToBuildBtn.Enabled = true;
                        EntranceBtn.Enabled = true;
                        IWBtn.Enabled = true;
                    }
                    return true;
                case 6:
                    InfoLable.Text = "Выбран вход провода в здание '" + MyMap.Buildings.Buildings[activeElem.build].Name + "'";
                    DeleteBtn.Enabled = true;
                    MyMap.Buildings.Buildings[activeElem.build].InputWires.InputWires.Choose(activeElem.item);
                    break;
                case 7:
                    InfoLable.Text = "Выбран вход в здание '" + MyMap.Buildings.Buildings[activeElem.build].Name + "'";
                    DeleteBtn.Enabled = true;
                    MyMap.Buildings.Buildings[activeElem.build].Entrances.Enterances.Choose(activeElem.item);
                    break;
                case 8:
                    MyMap.NetworkElements.Choose(activeElem.item);
                    InfoLable.Text = "Выбран сетевой элемент '" + MyMap.NetworkElements.NetworkElements[activeElem.item].Options.Name + "'";
                    DeleteBtn.Enabled = true;
                    CopyBtn.Enabled = true;
                    break;
                case 9:
                    MyMap.NetworkWires.Choose(activeElem.item);
                    InfoLable.Text = "Выбран провод ";
                    DeleteBtn.Enabled = true;
                    DelNWPBtn.Visible = true;
                    DelNWPBtn.Enabled = true;
                    AddNWPBtn.Visible = true;
                    AddNWPBtn.Enabled = true;
                    return true;
                case 10:
                    MyMap.MyTexts.Choose(activeElem.item);
                    InfoLable.Text = "Выбрана надпись ";
                    DeleteBtn.Enabled = true;
                    CopyBtn.Enabled = true;
                    return true;
                case 360:
                    MyMap.Circles.Choose(activeElem.item);
                    InfoLable.Text = "Выбран круг";
                    DeleteBtn.Enabled = true;
                    CopyBtn.Enabled = true;
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
            /*FloorDown.Visible = true;
            FloorUP.Visible = true;*/
            comboBox1.Enabled = true;
            floors_name.AddRange(MyMap.Buildings.Buildings[activeElem.item].floors_name);
            comboBox1.Items.Clear();
            foreach (var fn in floors_name)
                comboBox1.Items.Add(fn.ToString());
            comboBox1.SelectedIndex = drawLevel.Floor;
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
            //AddPP.Enabled = false;
            DeletePP.Visible = false;
            //DeletePP.Enabled = false;
            //
            DelNWPBtn.Visible = false;
            //DelNWPBtn.Enabled = false;
            AddNWPBtn.Visible = false;
            CopyBtn.Enabled = false;
            //AddNWPBtn.Enabled = false;
            //
            ToBuildBtn.Enabled = false;
            DeleteBtn.Enabled = false;
            EntranceBtn.Enabled = false;
            if (drawLevel.Level == -1)
                IWBtn.Enabled = false;
            else
                IWBtn.Enabled = true;
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
                    if (MessageBox.Show("Преобразовать здание в прямоугольник? Все элементы внутри здания будут удалены", "Преобразование здания", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        MyMap.RemoveBuildElements(activeElem.item);
                        MyMap.Rectangles.Add(MyMap.Buildings.Buildings[activeElem.item].MainRectangle);
                        int lastindex = MyMap.Rectangles.Rectangles.Count - 1;
                        Element elem = new Element(4, activeElem.item, MyMap.Buildings.Buildings[activeElem.item].Clone(), 1);
                        Element _elem = new Element(2, lastindex, MyMap.Rectangles.Rectangles[lastindex].Clone(), 1);
                        MyMap.log.Add(new LogMessage("Преобразовал здание в прямоугольник", elem, _elem));
                        CheckButtons(true);
                        MyMap.Buildings.Remove(activeElem.item);
                        Unfocus("Преобразовал здание в прямоугольник");
                    }
                }
                else if (MyMap.Buildings.Buildings[activeElem.item].type == 3)
                {
                    if (MessageBox.Show("Преобразовать здание в многоугольник? Все элементы внутри здания будут удалены", "Преобразование здания", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        MyMap.RemoveBuildElements(activeElem.item);
                        MyMap.Polygons.Add(MyMap.Buildings.Buildings[activeElem.item].MainPolygon);
                        int lastindex = MyMap.Polygons.Polygons.Count - 1;
                        Element elem = new Element(4, activeElem.item, MyMap.Buildings.Buildings[activeElem.item].Clone(), 2);
                        Element _elem = new Element(3, lastindex, MyMap.Polygons.Polygons[lastindex].Clone(), 2);
                        MyMap.log.Add(new LogMessage("Преобразовал здание в многоугольник", elem, _elem));
                        CheckButtons(true);
                        MyMap.Buildings.Remove(activeElem.item);
                        Unfocus("Преобразовал здание в многоугольник");
                    }
                }
                else if (MyMap.Buildings.Buildings[activeElem.item].type == 360)
                {
                    if (MessageBox.Show("Преобразовать здание в окружность? Все элементы внутри здания будут удалены", "Преобразование здания", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        MyMap.RemoveBuildElements(activeElem.item);
                        MyMap.Circles.Add(MyMap.Buildings.Buildings[activeElem.item].MainCircle);
                        int lastindex = MyMap.Circles.Circles.Count - 1;
                        Element elem = new Element(4, activeElem.item, MyMap.Buildings.Buildings[activeElem.item].Clone(), 5);
                        Element _elem = new Element(360, lastindex, MyMap.Circles.Circles[lastindex].Clone(), 5);
                        MyMap.log.Add(new LogMessage("Преобразовал здание в окружность", elem, _elem));
                        CheckButtons(true);
                        MyMap.Buildings.Remove(activeElem.item);
                        Unfocus("Преобразовал здание в окружность");
                    }
                }
            }
            else if (MessageBox.Show("Преобразовать в здание?", "Преобразование в здание", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                BuildForm buildForm = new BuildForm();
                buildForm.ShowDialog();
                if (buildForm.dialogResult == DialogResult.Yes)
                {
                    if (activeElem.type == 2)
                    {
                        MyMap.Buildings.Add(new Building(buildForm.name, buildForm.loft, buildForm.basement, buildForm.count, MyMap.Rectangles.Rectangles[activeElem.item], MyMap.Buildings.Buildings.Count, buildForm.width));
                        int lastindex = MyMap.Buildings.Buildings.Count - 1;
                        Element elem = new Element(2, activeElem.item, MyMap.Rectangles.Rectangles[activeElem.item].Clone(), 3);
                        Element _elem = new Element(4, lastindex, MyMap.Buildings.Buildings[lastindex].Clone(), 3);
                        MyMap.log.Add(new LogMessage("Преобразовал прямоугольник в здание", elem, _elem));
                        CheckButtons(true);
                        MyMap.Rectangles.Remove(activeElem.item);
                        Unfocus("Преобразовал прямоугольник в здание");
                    }
                    else if (activeElem.type == 3)
                    {
                        MyMap.Buildings.Add(new Building(buildForm.name, buildForm.loft, buildForm.basement, buildForm.count, MyMap.Polygons.Polygons[activeElem.item], MyMap.Buildings.Buildings.Count, buildForm.width));
                        int lastindex = MyMap.Buildings.Buildings.Count - 1;
                        Element elem = new Element(3, activeElem.item, MyMap.Polygons.Polygons[activeElem.item].Clone(), 4);
                        Element _elem = new Element(4, lastindex, MyMap.Buildings.Buildings[lastindex].Clone(), 4);
                        MyMap.log.Add(new LogMessage("Преобразовал многоугольник в здание", elem, _elem));
                        CheckButtons(true);
                        MyMap.Polygons.Remove(activeElem.item);
                        Unfocus("Преобразовал многоугольник в здание");
                    }
                    else if (activeElem.type == 360)
                    {
                        MyMap.Buildings.Add(new Building(buildForm.name, buildForm.loft, buildForm.basement, buildForm.count, MyMap.Circles.Circles[activeElem.item], MyMap.Buildings.Buildings.Count, buildForm.width));
                        int lastindex = MyMap.Buildings.Buildings.Count - 1;
                        Element elem = new Element(360, activeElem.item, MyMap.Circles.Circles[activeElem.item].Clone(), 6);
                        Element _elem = new Element(4, lastindex, MyMap.Buildings.Buildings[lastindex].Clone(), 6);
                        MyMap.log.Add(new LogMessage("Преобразовал окружность в здание", elem, _elem));
                        CheckButtons(true);
                        MyMap.Circles.Remove(activeElem.item);
                        Unfocus("Преобразовал окружность в здание");
                    }
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
                    if (MessageBox.Show("Вы уверены что хотите удалить здание? Все элементы, находящиеся внутри здания будут удалены", "Удаление здания", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        MyMap.RemoveBuildElements(activeElem.item);
                        elem = new Element(4, activeElem.item, new Building(), -1);
                        _elem = new Element(4, activeElem.item, MyMap.Buildings.Buildings[activeElem.item], -1);
                        MyMap.log.Add(new LogMessage("Удалил здание", elem, _elem));
                        InfoLable.Text = "Удалил здание";
                        MyMap.Buildings.Remove(activeElem.item);
                        CheckButtons(true);
                    }
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
            Unfocus("");
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
            Unfocus("Идет поиск");
            SearchDialog sd = new SearchDialog(MyMap.MyTexts, MyMap.Buildings, MyMap.NetworkElements, MyMap.NetworkWires);
            sd.ShowDialog();
            Unfocus("Поиск завершен");
            if (sd._type != -1 & sd._item != -1)
            {
                MyMap.SetInstrument(0);
                int id = sd._item;
                switch (sd._type)
                {
                    case 1:
                        drawLevel.Level = -1;
                        drawLevel.Floor = -1;
                        activeElem.type = 4;
                        activeElem.item = id;
                        MyMap.Buildings.Choose(activeElem.item);
                        InfoLable.Text = "Выбрано здание '" + MyMap.Buildings.Buildings[activeElem.item].Name + "'";
                        DeleteBtn.Enabled = true;
                        ToBuildBtn.Enabled = true;
                        EntranceBtn.Enabled = true;
                        IWBtn.Enabled = true;
                        break;
                    case 2:
                        drawLevel = MyMap.NetworkElements.NetworkElements[id].DL;
                        activeElem.type = 8;
                        activeElem.item = id;
                        MyMap.NetworkElements.Choose(activeElem.item);
                        InfoLable.Text = "Выбран сетевой элемент '" + MyMap.NetworkElements.NetworkElements[activeElem.item].Options.Name + "'";
                        DeleteBtn.Enabled = true;
                        break;
                    case 3:
                        drawLevel = MyMap.MyTexts.MyTexts[id].DL;
                        activeElem.type = 10;
                        activeElem.item = id;
                        MyMap.MyTexts.Choose(activeElem.item);
                        InfoLable.Text = "Выбрана надпись ";
                        DeleteBtn.Enabled = true;
                        break;
                    case 4:
                        drawLevel = MyMap.NetworkWires.NetworkWires[id].DL;
                        activeElem.type = 9;
                        activeElem.item = id;
                        MyMap.NetworkWires.Choose(activeElem.item);
                        InfoLable.Text = "Выбран провод";
                        DeleteBtn.Enabled = true;
                        break;
                }
                if (drawLevel.Level != -1)
                {
                    MyMap.ResizeRenderingArea(drawLevel.Level);
                    ReturnToMainBtn.Enabled = true;
                    UpgrateFloors();
                    IWBtn.Enabled = true;
                }
            }
        }
        #endregion
        #region Различные функции для работы с логом
        /// <summary>
        /// Запись в лог перемещений элементов
        /// </summary>
        private void UpdateTextLogAdd(int id)
        {
            object elem =  MyMap.MyTexts.MyTexts[id].Clone();
            if (isMoveLog)
            {
                isMoveLog = false;
                _MoveElem = new Element(10, id, elem, -2);
                MyMap.log.Add(new LogMessage("Изменил надпись", _MoveElem, MoveElem));
            }
            else
            {
                isMoveLog = true;
                MoveElem = new Element(10, id, elem, -2);
            }
        }
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
                case 4:
                    elem = MyMap.Buildings.Buildings[activeElem.item].Clone();
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
            if (activeElem.type == 1 | activeElem.type == 2 | activeElem.type == 3 | activeElem.type == 4 | activeElem.type == 360 | activeElem.type == 6 | activeElem.type == 7 | activeElem.type == 8 | activeElem.type == 9 | activeElem.type == 10)
            {
                if (isMoveLog)
                {
                    if (activeElem.type == 4)
                        MyMap.Buildings.Buildings[activeElem.item].EndMove();
                    isMoveLog = false;
                    _MoveElem = new Element(activeElem.type, activeElem.item, elem, -2);
                    if (_MoveElem.type == 6 | _MoveElem.type == 7)
                        MyMap.log.Add(new LogMessage("Переместил элемент", _MoveElem, MoveElem, activeElem.build));
                    else
                        MyMap.log.Add(new LogMessage("Переместил элемент", _MoveElem, MoveElem));
                    CheckButtons(true);
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
                            if (MyMap.Buildings.Buildings[_elem.index].MT.idtexture != -1 && !MTTextures.Contains((uint)MyMap.Buildings.Buildings[_elem.index].MT.idtexture))
                                MyMap.Buildings.Buildings[_elem.index].MT.GenTextureFromBuild();
                            break;
                        case 2:
                            MyMap.Polygons.Remove(elem.index);
                            MyMap.Buildings.Buildings[_elem.index] = (Building)_elem.elem;
                            if (MyMap.Buildings.Buildings[_elem.index].MT.idtexture != -1 && !MTTextures.Contains((uint)MyMap.Buildings.Buildings[_elem.index].MT.idtexture))
                                MyMap.Buildings.Buildings[_elem.index].MT.GenTextureFromBuild();
                            break;
                        case 3:
                            MyMap.Buildings.Remove(elem.index);
                            MyMap.Rectangles.Rectangles[_elem.index] = (MyRectangle)_elem.elem;
                            if (elem.index == drawLevel.Level)
                            {
                                drawLevel = new DrawLevel(-1, -1);
                                MyMap.ResizeRenderingArea();
                            }
                            break;
                        case 4:
                            MyMap.Buildings.Remove(elem.index);
                            MyMap.Polygons.Polygons[_elem.index] = (Polygon)_elem.elem;
                            if (elem.index == drawLevel.Level)
                            {
                                drawLevel = new DrawLevel(-1, -1);
                                MyMap.ResizeRenderingArea();
                            }
                            break;
                        case 5:
                            MyMap.Circles.Remove(elem.index);
                            MyMap.Buildings.Buildings[_elem.index] = (Building)_elem.elem;
                            if (MyMap.Buildings.Buildings[_elem.index].MT.idtexture != -1 && !MTTextures.Contains((uint)MyMap.Buildings.Buildings[_elem.index].MT.idtexture))
                                MyMap.Buildings.Buildings[_elem.index].MT.GenTextureFromBuild();
                            break;
                        case 6:
                            MyMap.Buildings.Remove(elem.index);
                            MyMap.Circles.Circles[_elem.index] = (Circle)_elem.elem;
                            if (elem.index == drawLevel.Level)
                            {
                                drawLevel = new DrawLevel(-1, -1);
                                MyMap.ResizeRenderingArea();
                            }
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
                            if (elem.index == drawLevel.Level)
                            {
                                drawLevel = new DrawLevel(-1, -1);
                                MyMap.ResizeRenderingArea();
                            }
                            break;
                        case 2:
                            MyMap.Buildings.Remove(elem.index);
                            MyMap.Polygons.Polygons[_elem.index] = (Polygon)_elem.elem;
                            if (elem.index == drawLevel.Level)
                            {
                                drawLevel = new DrawLevel(-1, -1);
                                MyMap.ResizeRenderingArea();
                            }
                            break;
                        case 3:
                            MyMap.Rectangles.Remove(elem.index);
                            MyMap.Buildings.Buildings[_elem.index] = (Building)_elem.elem;
                            if (MyMap.Buildings.Buildings[_elem.index].MT.idtexture != -1 && !MTTextures.Contains((uint)MyMap.Buildings.Buildings[_elem.index].MT.idtexture))
                                MyMap.Buildings.Buildings[_elem.index].MT.GenTextureFromBuild();
                            break;
                        case 4:
                            MyMap.Polygons.Remove(elem.index);
                            MyMap.Buildings.Buildings[_elem.index] = (Building)_elem.elem;
                            if (MyMap.Buildings.Buildings[_elem.index].MT.idtexture != -1 && !MTTextures.Contains((uint)MyMap.Buildings.Buildings[_elem.index].MT.idtexture))
                                MyMap.Buildings.Buildings[_elem.index].MT.GenTextureFromBuild();
                            break;
                        case 5:
                            MyMap.Buildings.Remove(elem.index);
                            MyMap.Circles.Circles[_elem.index] = (Circle)_elem.elem;
                            if (elem.index == drawLevel.Level)
                            {
                                drawLevel = new DrawLevel(-1, -1);
                                MyMap.ResizeRenderingArea();
                            }
                            break;
                        case 6:
                            MyMap.Circles.Remove(elem.index);
                            MyMap.Buildings.Buildings[_elem.index] = (Building)_elem.elem;
                            if (MyMap.Buildings.Buildings[_elem.index].MT.idtexture != -1 && !MTTextures.Contains((uint)MyMap.Buildings.Buildings[_elem.index].MT.idtexture))
                                MyMap.Buildings.Buildings[_elem.index].MT.GenTextureFromBuild();
                            break;
                    }
                    drawLevel = new DrawLevel(-1, -1);
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
                        if (MyMap.Buildings.Buildings[elem.index].MT.idtexture != -1 && !MTTextures.Contains((uint)MyMap.Buildings.Buildings[elem.index].MT.idtexture))
                            MyMap.Buildings.Buildings[elem.index].MT.GenTextureFromBuild();
                        MyMap.Buildings.CheckIW(elem.index, MyMap.NetworkWires);
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
                            MyMap.MyTexts.MyTexts[elem.index].GenNewTexture();
                        break;
                    case 11:
                        if (backbtn)
                            AddTextureFromLog(elem.index, (URL_ID)_elem.elem);
                        else
                            DeleteTextureFromLog(elem.index);
                        break;
                    case 12:
                        if (backbtn)
                            DeleteTextureFromLog(elem.index);
                        else
                            AddTextureFromLog(elem.index, (URL_ID)_elem.elem);
                        break;
                    case 14:
                        if (_elem.index == -1)
                        {
                            MyMap.sizeRenderingArea = (SizeRenderingArea)_elem.elem;
                            MyMap.ResizeRenderingArea();
                        }
                        else
                        {
                            MyMap.Buildings.Buildings[_elem.index] = (Building)_elem.elem;
                            MyMap.Buildings.Buildings[_elem.index].RefreshLocal();
                            MyMap.ResizeRenderingArea(_elem.index);
                        }
                        if (drawLevel.Level == -1)
                            MyMap.ResizeRenderingArea();
                        else
                            MyMap.ResizeRenderingArea(drawLevel.Level);
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
                    bool isNotNull = false;
                    for (int i = 0; i < MyMap.Buildings.Buildings[buildid].InputWires.InputWires.Circles.Count; i++)
                    {
                        if (i == elem.index)
                            isNotNull = true;
                    }
                    if (isNotNull)
                        MyMap.Buildings.Buildings[buildid].InputWires.InputWires.Circles[elem.index] = (Circle)elem.elem;
                    else
                        MyMap.Buildings.Buildings[buildid].InputWires.InputWires.Circles.Insert(elem.index, (Circle)elem.elem);
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
                    isNotNull = false;
                    for (int i = 0; i < MyMap.Buildings.Buildings[buildid].Entrances.Enterances.Circles.Count; i++)
                    {
                        if (i == elem.index)
                            isNotNull = true;
                    }
                    if (isNotNull)
                        MyMap.Buildings.Buildings[buildid].Entrances.Enterances.Circles[elem.index] = (Circle)elem.elem;
                    else
                        MyMap.Buildings.Buildings[buildid].Entrances.Enterances.Circles.Insert(elem.index, (Circle)elem.elem);
                    break;
            }
        }
        private void PharseElem(Element _elem, Element elem, bool backbtn, Parametrs temp)
        {
            if (backbtn)
            {
                switch (_elem.transform)
                {
                    case -1:
                        parametrs.Remove(elem.index);
                        MyMap.NetworkElements.UpdateOptions(elem.index);
                        break;
                    case -2:
                        parametrs.Edit(elem.index, _elem.elem.ToString());
                        MyMap.NetworkElements.UpdateOptions(elem.index, "");
                        break;
                    case -3:
                        parametrs.Add(_elem.elem.ToString());
                        break;
                    case -4:
                        MyMap.NetworkElements.NetworkElements[elem.index] = (NetworkElement)_elem.elem;
                        //Где-то тут генерить текстуру
                        if (MyMap.NetworkElements.NetworkElements[elem.index].MT.idtexture != -1 && !MTTextures.Contains((uint)MyMap.NetworkElements.NetworkElements[elem.index].MT.idtexture))
                            MyMap.NetworkElements.NetworkElements[elem.index].MT.GenTextureFromBuild();
                        break;
                }
            }
            else
            {
                switch (_elem.transform)
                {
                    case -1:
                        parametrs.Add(_elem.elem.ToString());
                        break;
                    case -2:
                        parametrs.Edit(elem.index, _elem.elem.ToString());
                        MyMap.NetworkElements.UpdateOptions(elem.index, "");
                        break;
                    case -3:
                        parametrs.Remove(elem.index);
                        MyMap.NetworkElements.UpdateOptions(elem.index);
                        break;
                    case -4:
                        MyMap.NetworkElements.NetworkElements[elem.index] = (NetworkElement)_elem.elem;
                        if (MyMap.NetworkElements.NetworkElements[elem.index].MT.idtexture != -1 && !MTTextures.Contains((uint)MyMap.NetworkElements.NetworkElements[elem.index].MT.idtexture))
                            MyMap.NetworkElements.NetworkElements[elem.index].MT.GenTextureFromBuild();
                        break;
                }
            }
        }
        /// <summary>
        /// Проверка кнопок для лога
        /// </summary>
        /// <param name="clearForward"></param>
        public void CheckButtons(bool clearForward)
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
                MyMap.UserLogin = user.SamAccountName;
                if (Directory.Exists(Application.StartupPath + @"\###tempdirectory._temp###\"))
                    Directory.Delete(Application.StartupPath + @"\###tempdirectory._temp###\", true);
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
                foreach (var url in ImagesURL.Textures)
                    File.Copy(Application.StartupPath + @"\Textures\" + url.URL, Application.StartupPath + @"\###tempdirectory._temp###\" + url.URL);
                if (File.Exists(filename))
                    File.Delete(filename);
                ZipFile.CreateFromDirectory(Application.StartupPath + @"\###tempdirectory._temp###\", filename);
                Directory.Delete(Application.StartupPath + @"\###tempdirectory._temp###\", true);
            }
        }
        /// <summary>
        /// Функция для открытия файла карты
        /// </summary>
        /// <param name="path">Путь</param>
        static public bool OpenMap(string path)
        {
            try
            {
                if (Directory.Exists(Application.StartupPath + @"\###tempdirectory._temp###\"))
                    Directory.Delete(Application.StartupPath + @"\###tempdirectory._temp###\", true);
                ZipFile.ExtractToDirectory(path, Application.StartupPath + @"\###tempdirectory._temp###\");
                XmlSerializer formatter = new XmlSerializer(typeof(Map));
                // получаем поток, куда будем записывать сериализованный объект
                using (FileStream fs = new FileStream(Application.StartupPath + @"\###tempdirectory._temp###\mapfile.map", FileMode.OpenOrCreate))
                {
                    Map TempMap = (Map)formatter.Deserialize(fs);
                    if (TempMap.UserLogin == user.SamAccountName | edit)
                    {
                        for (int i = 0; i < TempMap.MyTexts.MyTexts.Count; i++)
                            TempMap.MyTexts.MyTexts[i].GenNewTexture();
                        for (int i = 0; i < TempMap.Buildings.Buildings.Count; i++)
                            TempMap.Buildings.Buildings[i].GenText();
                        for (int i = 0; i < TempMap.NetworkElements.NetworkElements.Count; i++)
                            TempMap.NetworkElements.NetworkElements[i].GenText();
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
                        MessageBox.Show("Вам запрещен доступ к данной карте");
                        return false;
                    }
                }
                if (Directory.Exists(Application.StartupPath + @"\###tempdirectory._temp###\"))
                    Directory.Delete(Application.StartupPath + @"\###tempdirectory._temp###\", true);
                return true;
            }
            catch
            {
                if (Directory.Exists(Application.StartupPath + @"\###tempdirectory._temp###\"))
                    Directory.Delete(Application.StartupPath + @"\###tempdirectory._temp###\", true);
                return false;
            }
        }
        /// <summary>
        /// Сохранение (экспорт) здания в файл 
        /// </summary>
        /// <param name="fileExtension">Расширение файла в формате .*</param>
        /// <param name="descriptionFE">Описание заданного формата для отображения в диалоге</param>
        /// <param name="buildid"></param>
        public static void SaveBuild(string fileExtension, string descriptionFE, int buildid)
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
                Map TempMap = new Map();
                TempMap.Buildings.Add(MyMap.Buildings.Buildings[buildid].Clone());
                TempMap.Circles.AddGroupElems(MyMap.Circles.GetInBuild(buildid));
                TempMap.Lines.AddGroupElems(MyMap.Lines.GetInBuild(buildid));
                TempMap.Polygons.AddGroupElems(MyMap.Polygons.GetInBuild(buildid));
                TempMap.Rectangles.AddGroupElems(MyMap.Rectangles.GetInBuild(buildid));
                TempMap.NetworkElements.AddGroupElems(MyMap.NetworkElements.GetInBuild(buildid));
                TempMap.NetworkWires.AddGroupElems(MyMap.NetworkWires.GetInBuild(buildid));
                TempMap.MyTexts.AddGroupElems(MyMap.MyTexts.GetInBuild(buildid));
                TempMap.UserLogin = user.SamAccountName;
                if (Directory.Exists(Application.StartupPath + @"\###tempdirectory._temp###\"))
                    Directory.Delete(Application.StartupPath + @"\###tempdirectory._temp###\", true);
                Directory.CreateDirectory(Application.StartupPath + @"\###tempdirectory._temp###\");
                // получаем поток, куда будем записывать сериализованный объект
                using (FileStream fs = new FileStream(Application.StartupPath + @"\###tempdirectory._temp###\mapfile.map", FileMode.Create))
                {
                    formatter.Serialize(fs, TempMap);
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
                foreach (var url in ImagesURL.Textures)
                    File.Copy(Application.StartupPath + @"\Textures\" + url.URL, Application.StartupPath + @"\###tempdirectory._temp###\" + url.URL);
                if (File.Exists(filename))
                    File.Delete(filename);
                ZipFile.CreateFromDirectory(Application.StartupPath + @"\###tempdirectory._temp###\", filename);
                Directory.Delete(Application.StartupPath + @"\###tempdirectory._temp###\", true);
            }
        }
        /// <summary>
        /// Открытие файла с картой сети
        /// </summary>
        /// <param name="fileExtension">Расширение файла в формате .*</param>
        /// <param name="descriptionFE">Описание заданного формата для отображения в диалоге</param>
        public static bool OpenBuild(string fileExtension, string descriptionFE)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Filter = descriptionFE + "|*" + fileExtension
            };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (Directory.Exists(Application.StartupPath + @"\###tempdirectory._temp###\"))
                    Directory.Delete(Application.StartupPath + @"\###tempdirectory._temp###\", true);
                ZipFile.ExtractToDirectory(openFileDialog1.FileName, Application.StartupPath + @"\###tempdirectory._temp###\");
                XmlSerializer formatter = new XmlSerializer(typeof(Map));
                // получаем поток, куда будем записывать сериализованный объект
                using (FileStream fs = new FileStream(Application.StartupPath + @"\###tempdirectory._temp###\mapfile.map", FileMode.OpenOrCreate))
                {
                    MyMap.Unfocus(true);
                    Map OpenMap = (Map)formatter.Deserialize(fs);
                    if (OpenMap.UserLogin == user.SamAccountName | edit)
                    {
                        _OpenTexturesFromBuild(ref OpenMap.NetworkElements);
                        Parametrs._OpenFromBuild(ref OpenMap.NetworkElements);
                        int newdll = MyMap.Buildings.Buildings.Count;
                        OpenMap.Buildings.Buildings[0].LocalDL.Level = newdll;
                        MyMap.Buildings.Add(OpenMap.Buildings.Buildings[0].Clone());
                        int lastindex = MyMap.Buildings.Buildings.Count - 1;
                        MyMap.Buildings.Buildings[lastindex].GenText();
                        MyMap.Buildings.Buildings[lastindex].CalcCenterPoint();
                        MyMap.Buildings.Buildings[lastindex].EndMove();
                        foreach(var iw in MyMap.Buildings.Buildings[lastindex].InputWires.InputWires.Circles)
                        {
                            iw.LocalDL.Level = newdll;
                            if (iw.MainDL.Level != -1)
                                iw.MainDL.Level = newdll;
                        }
                        foreach (var ent in MyMap.Buildings.Buildings[lastindex].Entrances.Enterances.Circles)
                        {
                            ent.LocalDL.Level = newdll;
                        }
                        Element _elem = new Element(4, lastindex, new Building(), -1);
                        Element elem = new Element(4, lastindex, MyMap.Buildings.Buildings[lastindex].Clone(), -1);
                        MyMap.log.Add(new LogMessage("Импортировал здание", elem, _elem));
                        foreach (var item in OpenMap.Lines.Lines)
                        {
                            item.DL.Level = newdll;
                            MyMap.Lines.Add(item.Clone());
                            lastindex = MyMap.Lines.Lines.Count - 1;
                            Element _elem1 = new Element(1, lastindex, new Line(), -1);
                            Element elem1 = new Element(1, lastindex, MyMap.Lines.Lines[lastindex].Clone(), -1);
                            MyMap.log.Add(new LogMessage("Импортировал линию внутри здания", elem1, _elem1));
                        }
                        foreach (var item in OpenMap.Rectangles.Rectangles)
                        {
                            item.DL.Level = newdll;
                            MyMap.Rectangles.Add(item.Clone());
                            lastindex = MyMap.Rectangles.Rectangles.Count - 1;
                            Element _elem2 = new Element(2, lastindex, new MyRectangle(), -1);
                            Element elem2 = new Element(2, lastindex, MyMap.Rectangles.Rectangles[lastindex].Clone(), -1);
                            MyMap.log.Add(new LogMessage("Импортировал прямоугольник внутри здания", elem2, _elem2));
                        }
                        foreach (var item in OpenMap.Polygons.Polygons)
                        {
                            item.DL.Level = newdll;
                            MyMap.Polygons.Add(item.Clone());
                            lastindex = MyMap.Polygons.Polygons.Count - 1;
                            Element _elem3 = new Element(3, lastindex, new Polygon(), -1);
                            Element elem3 = new Element(3, lastindex, MyMap.Polygons.Polygons[lastindex].Clone(), -1);
                            MyMap.log.Add(new LogMessage("Импортировал многоугольник внутри здания", elem3, _elem3));
                        }
                        foreach (var item in OpenMap.Circles.Circles)
                        {
                            item.MainDL.Level = newdll;
                            item.DL.Level = newdll;
                            MyMap.Circles.Add(item.Clone());
                            lastindex = MyMap.Circles.Circles.Count - 1;
                            Element _elem4 = new Element(360, lastindex, new Circle(), -1);
                            Element elem4 = new Element(360, lastindex, MyMap.Circles.Circles[lastindex].Clone(), -1);
                            MyMap.log.Add(new LogMessage("Импортировал круг внутри здания", elem4, _elem4));
                        }
                        foreach (var item in OpenMap.MyTexts.MyTexts)
                        {
                            item.DL.Level = newdll;
                            item.GenNewTexture();
                            MyMap.MyTexts.Add(item.Clone());
                            lastindex = MyMap.MyTexts.MyTexts.Count - 1;
                            Element _elem5 = new Element(10, lastindex, new MyText(), -1);
                            Element elem5 = new Element(10, lastindex, MyMap.MyTexts.MyTexts[lastindex].Clone(), -1);
                            MyMap.log.Add(new LogMessage("Импортировал надпись внутри здания", elem5, _elem5));
                        }
                        foreach (var item in OpenMap.NetworkElements.NetworkElements)
                        {
                            item.DL.Level = newdll;
                            item.GenText();
                            MyMap.NetworkElements.Add(item.Clone());
                            lastindex = MyMap.NetworkElements.NetworkElements.Count - 1;
                            Element _elem6 = new Element(8, lastindex, new NetworkElement(), -1);
                            Element elem6 = new Element(8, lastindex, MyMap.NetworkElements.NetworkElements[lastindex].Clone(), -1);
                            MyMap.log.Add(new LogMessage("Импортировал сетевой элемент внутри здания", elem6, _elem6));
                        }
                        foreach (var item in OpenMap.NetworkWires.NetworkWires)
                        {
                            item.DL.Level = newdll;
                            MyMap.NetworkWires.Add(item.Clone());
                            lastindex = MyMap.NetworkWires.NetworkWires.Count - 1;
                            Element _elem7 = new Element(9, lastindex, new NetworkWire(), -1);
                            Element elem7 = new Element(9, lastindex, MyMap.NetworkWires.NetworkWires[lastindex].Clone(), -1);
                            MyMap.log.Add(new LogMessage("Импортировал провод внутри здания", elem7, _elem7));
                        }
                    }
                    else
                    {
                        MessageBox.Show("Вам запрещен доступ к данному зданию");
                        return false;
                    }
                }
                if (Directory.Exists(Application.StartupPath + @"\###tempdirectory._temp###\"))
                    Directory.Delete(Application.StartupPath + @"\###tempdirectory._temp###\", true);
                return true;
            }
            return false;
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
                    if (neButtons[i].textname == ImagesURL.Textures[j].URL)
                    {
                        neButtons[i].id = j;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Функция для сохранения шаблона карты сети в файл 
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
                TempMap.Buildings = MyMap.Buildings;
                TempMap.Circles = MyMap.Circles;
                TempMap.Lines = MyMap.Lines;
                TempMap.Polygons = MyMap.Polygons;
                TempMap.Rectangles = MyMap.Rectangles;
                TempMap.MyTexts = MyMap.MyTexts;
                TempMap.sizeRenderingArea = MyMap.sizeRenderingArea;
                string filename;
                if (saveFileDialog1.FileName.Contains(fileExtension))
                {
                    filename = saveFileDialog1.FileName;
                }
                else
                {
                    filename = saveFileDialog1.FileName + fileExtension;
                }
                MyMap.UserLogin = user.SamAccountName;
                if (Directory.Exists(Application.StartupPath + @"\###tempdirectory._temp###\"))
                    Directory.Delete(Application.StartupPath + @"\###tempdirectory._temp###\", true);
                Directory.CreateDirectory(Application.StartupPath + @"\###tempdirectory._temp###\");
                // получаем поток, куда будем записывать сериализованный объект
                using (FileStream fs = new FileStream(Application.StartupPath + @"\###tempdirectory._temp###\mapfile.map", FileMode.Create))
                {
                    formatter.Serialize(fs, TempMap);
                }
                ZipFile.CreateFromDirectory(Application.StartupPath + @"\###tempdirectory._temp###\", filename);
                Directory.Delete(Application.StartupPath + @"\###tempdirectory._temp###\", true);
            }
        }
        public static bool OpenTemplateMap(string path)
        {
            try
            {
                if (Directory.Exists(Application.StartupPath + @"\###tempdirectory._temp###\"))
                    Directory.Delete(Application.StartupPath + @"\###tempdirectory._temp###\", true);
                ZipFile.ExtractToDirectory(path, Application.StartupPath + @"\###tempdirectory._temp###\");
                XmlSerializer formatter = new XmlSerializer(typeof(Map));
                // получаем поток, куда будем записывать сериализованный объект
                using (FileStream fs = new FileStream(Application.StartupPath + @"\###tempdirectory._temp###\mapfile.map", FileMode.OpenOrCreate))
                {
                    Map TempMap = (Map)formatter.Deserialize(fs);
                    if (TempMap.UserLogin == user.SamAccountName | edit)
                    {
                        for (int i = 0; i < TempMap.MyTexts.MyTexts.Count; i++)
                            TempMap.MyTexts.MyTexts[i].GenNewTexture();
                        for (int i = 0; i < TempMap.Buildings.Buildings.Count; i++)
                            TempMap.Buildings.Buildings[i].GenText();
                        MyMap.MapLoad(TempMap);
                    }
                    else
                    {
                        MessageBox.Show("Вам запрещен доступ к данному шаблону");
                        return false;
                    }
                }
                if (Directory.Exists(Application.StartupPath + @"\###tempdirectory._temp###\"))
                    Directory.Delete(Application.StartupPath + @"\###tempdirectory._temp###\", true);
                return true;
            }
            catch
            {
                if (Directory.Exists(Application.StartupPath + @"\###tempdirectory._temp###\"))
                    Directory.Delete(Application.StartupPath + @"\###tempdirectory._temp###\", true);
                return false;
            }
            return false;
        }
        /// <summary>
        /// Конвертер
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        static object Conv(object elem) => elem;
        #endregion
        #region События
        /// <summary>
        /// Событие нажатия кнопки экспорта карты
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportMapClick(object sender, EventArgs e)
        {
            MyMap.Unfocus(true);
            Filtres _filtres = filtres;
            MapExportForm MEF = new MapExportForm(MyMap.Buildings.Buildings);
            MEF.ShowDialog();
            filtres = _filtres;
        }
        /// <summary>
        /// Событие нажатия кнопки экспорта и импорта здания
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportImportBuildClick(object sender, EventArgs e)
        {
            BuildExportImport buildExportImport = new BuildExportImport(MyMap.Buildings.Buildings);
            buildExportImport.ShowDialog();
            if (buildExportImport.action)
                CheckButtons(true);
            //OpenBuild(".build", "Building File");
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ImageTextures IT = new ImageTextures(ref MyMap.NetworkElements);
            IT.ShowDialog();
            if (IT.action == 0)
            {
                AddTexture(IT.imageindex);
            }
            else if (IT.action == 1)
            {
                neButtons.Add(new NEButton(new ToolStripButton(Images.Images[IT.imageindex]), IT.imageindex, ImagesURL.Textures[IT.imageindex].URL, toolStrip1.Items.Count));
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
            Element elem = new Element(11, id, ImagesURL.Textures[id], -1);
            DeleteImages.Add(ImagesURL.Textures[id]);
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
                    neb.textname = ImagesURL.Textures[newid].URL;
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
            Element _elem = new Element(12, id, ImagesURL.Textures.Last(), -1);
            MyMap.log.Add(new LogMessage("Добавил текстуру", elem, _elem));
            InfoLable.Text = "Добавил текстуру";
            CheckButtons(true);
        }

        private void DeleteTextureFromLog(int id)
        {
            DeleteImages.Add(ImagesURL.Textures[id]);
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
                    neb.textname = ImagesURL.Textures[newid].URL;
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

        private void AddTextureFromLog(int id, URL_ID uRL_ID)
        {
            ImagesURL.Textures.Insert(id, uRL_ID);
            isLoad = false;
            LoadImages();
            foreach (var neb in neButtons)
            {
                if (neb.id >= id)
                {
                    int newid = neb.id + 1;
                    neb.id = newid;
                    neb.toolStripButton.Image = Images.Images[newid];
                    neb.textname = ImagesURL.Textures[newid].URL;
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
            CreateMapForm createMapForm = new CreateMapForm();
            createMapForm.ShowDialog();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Network Design Map File|*.ndm|Network Design Map File (Template)|*.ndmt"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (openFileDialog.FileName.Contains(".ndm") & !openFileDialog.FileName.Contains(".ndmt"))
                    OpenMap(openFileDialog.FileName);
                else if (openFileDialog.FileName.Contains(".ndmt"))
                    OpenTemplateMap(openFileDialog.FileName);
                CheckButtons(true);
                Text = MyMap.sizeRenderingArea.Name;
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveMap(".ndm", "Network Design Map File");
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
        ColorDialogForm colorDialog;
        /// <summary>
        /// Событие нажатия кнопки параметры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParamsMapClick(object sender, EventArgs e)
        {
            colorDialog = new ColorDialogForm();
            colorDialog.Show();
        }
        /// <summary>
        /// Событие нажатия кнопки посмотреть лог
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowLogClick(object sender, EventArgs e)
        {
            FormLog formlog = new FormLog(MyMap.log);
            formlog.ShowDialog();
            int backcount = MyMap.log.Back.Count();
            if (formlog.RowIndex != -1 & formlog.RowIndex != backcount)
            {
                int count = 0;
                int forwardcount = MyMap.log.Forward.Count();
                if (formlog.RowIndex > backcount)
                {
                    count = backcount + 1 + forwardcount - formlog.RowIndex;
                    for (int i = 0; i < count; i++)
                        ForwardClick();
                }
                else
                {
                    count = backcount - formlog.RowIndex;
                    for (int i = 0; i < count; i++)
                        BackClick();
                }
            }
        }
        /// <summary>
        /// Событие нажатия кнопки создания новой карты с заданными параметрами
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateMapClick(object sender, EventArgs e)
        {
            CreateMapForm createMapForm = new CreateMapForm();
            createMapForm.ShowDialog();
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
        private void OpenMapClick(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Network Design Map File|*.ndm|Network Design Map File (Template)|*.ndmt"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (openFileDialog.FileName.Contains(".ndm") & !openFileDialog.FileName.Contains(".ndmt"))
                    OpenMap(openFileDialog.FileName);
                else if (openFileDialog.FileName.Contains(".ndmt"))
                    OpenTemplateMap(openFileDialog.FileName);
                CheckButtons(true);
                Text = MyMap.sizeRenderingArea.Name;
            }
        }

        /// <summary>
        /// Событие нажатия кнопки сохранение шаблона карты
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveTemplateMapClick(object sender, EventArgs e) => SaveTemplateMap(".ndmt", "Network Design Map File (Template)");
        /// <summary>
        /// Собитые нажатия кнопки назад
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackBtn_Click(object sender, EventArgs e)
        {
            BackClick();
            Unfocus("Нажата стрелочка назад");
        }

        private void BackClick()
        {
            Element _elem = MyMap.log.DeleteLastBack(out Element elem, out int buildid);
            if (_elem.type != 7 & _elem.type != 6 & _elem.type != 13 & _elem.type != 15)
                PharseElem(_elem, elem, true);
            else if (_elem.type == 13)
                PharseElem(_elem, elem, true, parametrs);
            else if (_elem.type == 15)
                BuildEdit(_elem, elem);
            else
                PharseElem(_elem, elem, buildid);
            CheckButtons(false);
        }

        /// <summary>
        /// Событие нажатия кнопки вперед
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ForwardBrn_Click(object sender, EventArgs e)
        {
            ForwardClick();
            Unfocus("Нажата стрелочка вперед");
        }

        private void ForwardClick()
        {
            Element _elem = MyMap.log.DeleteLastForward(out Element elem, out int buildid);
            if (_elem.type != 7 & _elem.type != 6 & _elem.type != 13 & _elem.type != 15)
                PharseElem(_elem, elem, false);
            else if (_elem.type == 13)
                PharseElem(_elem, elem, false, parametrs);
            else if (_elem.type == 15)
                BuildEdit(_elem, elem);
            else
                PharseElem(_elem, elem, buildid);
            CheckButtons(false);
        }

        private void BuildEdit(Element _elem, Element elem)
        {
            if (drawLevel.Level == _elem.index)
                drawLevel = new DrawLevel(-1, -1);
            var temp = (BUILDLIST)_elem.elem;
            MyMap.Buildings.Buildings[_elem.index] = (Building)temp.building.Clone();
            if (MyMap.Buildings.Buildings[_elem.index].MT.idtexture != -1 && !MTTextures.Contains((uint)MyMap.Buildings.Buildings[_elem.index].MT.idtexture))
                MyMap.Buildings.Buildings[_elem.index].MT.GenTextureFromBuild();
            List<int> _Added = new List<int>();
            foreach (var add in temp.Deteled)
                _Added.Add(add);
            List<int> _Deleted = new List<int>();
            foreach (var del in temp.Added)
                _Deleted.Add(del);
            _Added.Reverse();
            _Deleted.Reverse();
            MyMap.MoveElementsInBuild(_elem.index, _Added, _Deleted, MyMap.Buildings.Buildings[_elem.index].basement);
            return;
        }

        /// <summary>
        /// Собитые нажатия кнопки курсор
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CursorClick(object sender, EventArgs e)
        {
            MyMap.SetInstrument(0);
            редактированиеToolStripMenuItem.Checked = false;
            курсорToolStripMenuItem.Checked = true;
            линияToolStripMenuItem.Checked = false;
            кругToolStripMenuItem.Checked = false;
            прямоугольникToolStripMenuItem.Checked = false;
            многоугольникToolStripMenuItem.Checked = false;
            текстToolStripMenuItem.Checked = false;
            проводаToolStripMenuItem.Checked = false;
        }

        /// <summary>
        /// Событие нажатия кнопки линия
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LineClick(object sender, EventArgs e)
        {
            MyMap.SetInstrument(1);
            редактированиеToolStripMenuItem.Checked = false;
            курсорToolStripMenuItem.Checked = false;
            линияToolStripMenuItem.Checked = true;
            кругToolStripMenuItem.Checked = false;
            прямоугольникToolStripMenuItem.Checked = false;
            многоугольникToolStripMenuItem.Checked = false;
            текстToolStripMenuItem.Checked = false;
            проводаToolStripMenuItem.Checked = false;
        }

        /// <summary>
        /// Событие нажатия кнопки многоугольник
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PolygonClick(object sender, EventArgs e)
        {
            MyMap.SetInstrument(5);
            редактированиеToolStripMenuItem.Checked = false;
            курсорToolStripMenuItem.Checked = false;
            линияToolStripMenuItem.Checked = false;
            кругToolStripMenuItem.Checked = false;
            прямоугольникToolStripMenuItem.Checked = false;
            многоугольникToolStripMenuItem.Checked = true;
            текстToolStripMenuItem.Checked = false;
            проводаToolStripMenuItem.Checked = false;
        }

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
        private void RectangleClick(object sender, EventArgs e)
        {
            MyMap.SetInstrument(2);
            редактированиеToolStripMenuItem.Checked = false;
            курсорToolStripMenuItem.Checked = false;
            линияToolStripMenuItem.Checked = false;
            кругToolStripMenuItem.Checked = false;
            прямоугольникToolStripMenuItem.Checked = true;
            многоугольникToolStripMenuItem.Checked = false;
            текстToolStripMenuItem.Checked = false;
            проводаToolStripMenuItem.Checked = false;
        }

        /// <summary>
        /// Событие нажатия кнопки круг
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripButton6_Click_1(object sender, EventArgs e)
        {
            MyMap.SetInstrument(360);
            редактированиеToolStripMenuItem.Checked = false;
            курсорToolStripMenuItem.Checked = false;
            линияToolStripMenuItem.Checked = false;
            кругToolStripMenuItem.Checked = true;
            прямоугольникToolStripMenuItem.Checked = false;
            многоугольникToolStripMenuItem.Checked = false;
            текстToolStripMenuItem.Checked = false;
            проводаToolStripMenuItem.Checked = false;
        }

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
                редактированиеToolStripMenuItem.Checked = false;
                курсорToolStripMenuItem.Checked = true;
                линияToolStripMenuItem.Checked = false;
                кругToolStripMenuItem.Checked = false;
                прямоугольникToolStripMenuItem.Checked = false;
                многоугольникToolStripMenuItem.Checked = false;
                текстToolStripMenuItem.Checked = false;
                проводаToolStripMenuItem.Checked = false;
            }
            else
            {
                MyMap.SetInstrument(3);
                редактированиеToolStripMenuItem.Checked = true;
                курсорToolStripMenuItem.Checked = false;
                линияToolStripMenuItem.Checked = false;
                кругToolStripMenuItem.Checked = false;
                прямоугольникToolStripMenuItem.Checked = false;
                многоугольникToolStripMenuItem.Checked = false;
                текстToolStripMenuItem.Checked = false;
                проводаToolStripMenuItem.Checked = false;
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
            MyMap.ResizeRenderingArea();
            drawLevel.Level = -1;
            drawLevel.Floor = -1;
            ReturnToMainBtn.Enabled = false;
            comboBox1.Enabled = false;
            comboBox1.Items.Clear();
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
        private void TextClick(object sender, EventArgs e)
        {
            MyMap.SetInstrument(10);
            редактированиеToolStripMenuItem.Checked = false;
            курсорToolStripMenuItem.Checked = false;
            линияToolStripMenuItem.Checked = false;
            кругToolStripMenuItem.Checked = false;
            прямоугольникToolStripMenuItem.Checked = false;
            многоугольникToolStripMenuItem.Checked = false;
            текстToolStripMenuItem.Checked = true;
            проводаToolStripMenuItem.Checked = false;
        }

        /// <summary>
        /// Событие нажатия кнопки провода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NWClick(object sender, EventArgs e)
        {
            MyMap.SetInstrument(9);
            редактированиеToolStripMenuItem.Checked = false;
            курсорToolStripMenuItem.Checked = false;
            линияToolStripMenuItem.Checked = false;
            кругToolStripMenuItem.Checked = false;
            прямоугольникToolStripMenuItem.Checked = false;
            многоугольникToolStripMenuItem.Checked = false;
            текстToolStripMenuItem.Checked = false;
            проводаToolStripMenuItem.Checked = true;
        }

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
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            zoom = (double)trackBar1.Value / 10d;
            if (drawLevel.Level == -1)
                MyMap.ResizeRenderingArea();
            else
                MyMap.ResizeRenderingArea(drawLevel.Level);
        }
        /// <summary>
        /// Событие закрытия формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //StartForm.SaveMapList();
            ColorSettings.Save(colorSettings);
            SaveTextures(ImagesURL);
            Parametrs.Save(parametrs);
            ID_TEXT temp = new ID_TEXT();
            foreach (var n in neButtons)
                temp.ADD(n.id, n.textname);
            NEButton.Save(temp);
            Application.Exit();
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
                if (File.Exists(Application.StartupPath + @"\Textures\" + ImagesURL.Textures[j].URL))
                {
                    Image image = Image.FromFile(Application.StartupPath + @"\Textures\" + ImagesURL.Textures[j].URL);
                    Bitmap bitmap = new Bitmap(image);
                    if (image.Height != 1024 | image.Width != 1024)
                    {
                        bitmap.Dispose();
                        bitmap = new Bitmap(image, 1024, 1024);
                        image.Dispose();
                        bitmap.Save(Application.StartupPath + @"\Textures\" + ImagesURL.Textures[j].URL);
                        bitmap.Dispose();
                        image = Image.FromFile(Application.StartupPath + @"\Textures\" + ImagesURL.Textures[j].URL);
                    }
                    Images.Images.Add(image);
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
                        if (!GenTex(j, Application.StartupPath + @"\Textures\" + ImagesURL.Textures[j].URL))
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
        static public ListOfTextures OpenTextures()
        {
            if (!Directory.Exists(Application.StartupPath + @"\Textures"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\Textures");
                SaveTextures(new ListOfTextures());
                return new ListOfTextures();
            }
            if (!File.Exists(Application.StartupPath + @"\Textures\ListOfTextures"))
            {
                SaveTextures(new ListOfTextures());
                return new ListOfTextures();
            }
            XmlSerializer formatter = new XmlSerializer(typeof(ListOfTextures));
            using (FileStream fs = new FileStream(Application.StartupPath + @"\Textures\ListOfTextures", FileMode.Open))
            {
                return (ListOfTextures)formatter.Deserialize(fs);
            }
        }
        static public void _OpenTextures()
        {
            if (!Directory.Exists(Application.StartupPath + @"\Textures"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\Textures");
                SaveTextures(new ListOfTextures());
                ImagesURL = new ListOfTextures();
            }
            if (!File.Exists(Application.StartupPath + @"\Textures\ListOfTextures"))
            {
                SaveTextures(new ListOfTextures());
                ImagesURL = new ListOfTextures();
            }
            XmlSerializer formatter = new XmlSerializer(typeof(ListOfTextures));
            using (FileStream fs = new FileStream(Application.StartupPath + @"\###tempdirectory._temp###\ListOfTextures", FileMode.Open))
            {
                ListOfTextures textures = (ListOfTextures)formatter.Deserialize(fs);
                ListOfTextures _textures = new ListOfTextures();
                _textures = (ListOfTextures)ImagesURL.Clone();
                ImagesURL.Clear();
                ImagesURL = (ListOfTextures)textures.Clone();
                foreach (var url in ImagesURL.Textures)
                    if (!File.Exists(Application.StartupPath + @"\Textures\" + url.URL))
                        File.Copy(Application.StartupPath + @"\###tempdirectory._temp###\" + url.URL, Application.StartupPath + @"\Textures\" + url.URL);
                int id = -1;
                for (int i = 0; i < _textures.Count; i++)
                {
                    id = -1;
                    for (int j = 0; j < ImagesURL.Count; j++)
                    {
                        if (ImagesURL.Textures[j].URL == _textures.Textures[i].URL)
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
                        ImagesURL.Add(_textures.Textures[i]);
                }
                isLoad = false;
                LoadImages();
            }
        }

        private static void _OpenTexturesFromBuild(ref GroupOfNE NE)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(ListOfTextures));
            using (FileStream fs = new FileStream(Application.StartupPath + @"\###tempdirectory._temp###\ListOfTextures", FileMode.Open))
            {
                ListOfTextures textures = (ListOfTextures)formatter.Deserialize(fs);
                for (int i = 0; i < NE.NetworkElements.Count; i++)
                {
                    bool isLoadTexture = false;
                    for (int j = 0; j < ImagesURL.Textures.Count; j++)
                    {
                        if (NE.NetworkElements[i].texture.url == ImagesURL.Textures[j].URL)
                        {
                            NE.NetworkElements[i].texture.idimage = j;
                            isLoadTexture = true;
                            break;
                        }
                    }
                    if (!isLoadTexture)
                    {
                        ImagesURL.Add(textures.Textures[NE.NetworkElements[i].texture.idimage]);
                        if (!File.Exists(Application.StartupPath + @"\Textures\" + NE.NetworkElements[i].texture.url))
                            File.Copy(Application.StartupPath + @"\###tempdirectory._temp###\" + NE.NetworkElements[i].texture.url, Application.StartupPath + @"\Textures\" + NE.NetworkElements[i].texture.url);
                        NE.NetworkElements[i].texture.idimage = ImagesURL.Count - 1;
                        string empty = "";
                        Element elem = new Element(12, ImagesURL.Count - 1, empty, -1);
                        Element _elem = new Element(12, ImagesURL.Count - 1, ImagesURL.Textures.Last(), -1);
                        MyMap.log.Add(new LogMessage("Добавил текстуру", elem, _elem));
                    }
                }
                isLoad = false;
                LoadImages();
            }
        }

        static public void SaveTextures(ListOfTextures imglist)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(ListOfTextures));
            using (FileStream fs = new FileStream(Application.StartupPath + @"\Textures\ListOfTextures", FileMode.Create))
            {
                formatter.Serialize(fs, imglist);
            }
        }

        private void panel1_Scroll_1(object sender, ScrollEventArgs e) => asp = panel1.AutoScrollPosition;
        //копировать
        private void CopyBtn_Click(object sender, EventArgs e)
        {
            int i = activeElem.item;
            int lastindex;
            Element _elem;
            Element elem;
            switch (activeElem.type)
            {
                case 1:
                    MyMap.Lines.Add(MyMap.Lines.Lines[i].Clone());
                    var line = MyMap.Lines.Lines.Last();
                    for (int j = 0; j < line.Points.Count; j++)
                        line.Points[j] = new Point(line.Points[j].X + 50, line.Points[j].Y - 50);
                    lastindex = MyMap.Lines.Lines.Count - 1;
                    _elem = new Element(1, lastindex, new Line(), -1);
                    elem = new Element(1, lastindex, MyMap.Lines.Lines[lastindex].Clone(), -1);
                    MyMap.log.Add(new LogMessage("Скопировал линию", elem, _elem));
                    break;
                case 2:
                    MyMap.Rectangles.Add(MyMap.Rectangles.Rectangles[i].Clone());
                    var rect = MyMap.Rectangles.Rectangles.Last();
                    for (int j = 0; j < rect.Points.Count; j++)
                        rect.Points[j] = new Point(rect.Points[j].X + 100, rect.Points[j].Y - 100);
                    lastindex = MyMap.Rectangles.Rectangles.Count - 1;
                    _elem = new Element(2, lastindex, new MyRectangle(), -1);
                    elem = new Element(2, lastindex, MyMap.Rectangles.Rectangles[lastindex].Clone(), -1);
                    MyMap.log.Add(new LogMessage("Скопировал прямоугольник", elem, _elem));
                    break;
                case 3:
                    MyMap.Polygons.Add(MyMap.Polygons.Polygons[i].Clone());
                    var poly = MyMap.Polygons.Polygons.Last();
                    for (int j = 0; j < poly.Points.Count; j++)
                        poly.Points[j] = new Point(poly.Points[j].X + 100, poly.Points[j].Y - 100);
                    lastindex = MyMap.Polygons.Polygons.Count - 1;
                    _elem = new Element(3, lastindex, new Polygon(), -1);
                    elem = new Element(3, lastindex, MyMap.Polygons.Polygons[lastindex].Clone(), -1);
                    MyMap.log.Add(new LogMessage("Скопировал многоугольник", elem, _elem));
                    break;
                case 360:
                    MyMap.Circles.Add(MyMap.Circles.Circles[i].Clone());
                    var circ = MyMap.Circles.Circles.Last();
                    circ.MainCenterPoint = new Point(circ.MainCenterPoint.X + circ.radius, circ.MainCenterPoint.Y - circ.radius);
                    lastindex = MyMap.Circles.Circles.Count - 1;
                    _elem = new Element(360, lastindex, new Circle(), -1);
                    elem = new Element(360, lastindex, MyMap.Circles.Circles[lastindex].Clone(), -1);
                    MyMap.log.Add(new LogMessage("Скопировал круг", elem, _elem));
                    break;
                case 8:
                    MyMap.NetworkElements.Add(MyMap.NetworkElements.NetworkElements[i].Clone());
                    var ne = MyMap.NetworkElements.NetworkElements.Last();
                    ne.texture.location = new Point(ne.texture.location.X + (int)ne.texture.width, ne.texture.location.Y - (int)ne.texture.width);
                    ne.GenText();
                    lastindex = MyMap.NetworkElements.NetworkElements.Count - 1;
                    _elem = new Element(8, lastindex, new NetworkElement(), -1);
                    elem = new Element(8, lastindex, MyMap.NetworkElements.NetworkElements[lastindex].Clone(), -1);
                    MyMap.log.Add(new LogMessage("Скопировал сетевой элемент", elem, _elem));
                    break;
                case 10:
                    MyMap.MyTexts.Add(MyMap.MyTexts.MyTexts[i].Clone());
                    var mt = MyMap.MyTexts.MyTexts.Last();
                    mt.location = new Point(mt.location.X + mt.size.Width, mt.location.Y - mt.size.Width);
                    mt.GenNewTexture();
                    lastindex = MyMap.MyTexts.MyTexts.Count - 1;
                    _elem = new Element(10, lastindex, new MyText(), -1);
                    elem = new Element(10, lastindex, MyMap.MyTexts.MyTexts[lastindex].Clone(), -1);
                    MyMap.log.Add(new LogMessage("Скопировал текст", elem, _elem));
                    break;
            }
        }

        public static void ImageExport(List<bool> build)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                DrawLevel dl = drawLevel;
                if (MessageBox.Show("Некоторые элементы могут быть заменены и удалены. Продолжить?") == DialogResult.OK)
                {
                    string path = fbd.SelectedPath;
                    drawLevel = new DrawLevel(-1, -1);
                    MyMap.Drawing();
                    Thread.Sleep(1000);
                    Bitmap image = GetBitmap();
                    image.Save(path + "/Главный вид.png");
                    List<Building> buildings = new List<Building>();
                    foreach (var b in MyMap.Buildings.Buildings)
                        if (!b.delete)
                            buildings.Add(b);
                    for (int i = 0; i < buildings.Count; i++)
                    {
                        if (build[i])
                        {
                            string buildname = path + "\\Здание " + i + " " + MyMap.Buildings.Buildings[i].Name + "\\";
                            buildname = buildname.Replace("\r\n", "");
                            if (Directory.Exists(buildname))
                                Directory.Delete(buildname, true);
                            Directory.CreateDirectory(buildname);
                            for (int j = 0; j < MyMap.Buildings.Buildings[i].floors_name.Count; j++)
                            {
                                drawLevel = new DrawLevel(i, j);
                                MyMap.Drawing();
                                Thread.Sleep(1000);
                                Bitmap _image = GetBitmap();
                                _image.Save(buildname + MyMap.Buildings.Buildings[i].floors_name[j].ToString() + ".png");
                            }
                        }
                    }
                }
                drawLevel = dl;
            }
        }
        /// <summary>
        /// Получает изображение из области отрисовки
        /// </summary>
        /// <returns></returns>
        private static Bitmap GetBitmap()
        {
            Bitmap bitmap = new Bitmap(AnT.Width, AnT.Height);
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            Gl.glReadPixels(0, 0, bitmap.Width, bitmap.Height, Gl.GL_BGR_EXT, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);
            bitmap.UnlockBits(bitmapData);
            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
            return bitmap;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            
        }
        /// <summary>
        /// Экспортирует список сетевых элементов для нужных зданий
        /// </summary>
        /// <param name="build">Список зданий</param>
        static public void ExportListNE(List<bool> build)
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
                MyMap.ExportListNE(filename, build);
            }
        }

        static public System.Windows.Forms.Timer PingDeviceTimer = new System.Windows.Forms.Timer();

        private void pingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PingDeviceTimer = new System.Windows.Forms.Timer();
            PingDeviceTimer.Interval = 60000;
            PingDeviceTimer.Tick += PingDeviceTimer_Tick;
            if (!isPing)
            {
                pingToolStripMenuItem.Checked = true;
                PingDeviceTimer.Start();
                isPing = true;
            }
            else
            {
                PingDeviceTimer.Stop();
                pingToolStripMenuItem.Checked = false;
                isPing = false;
            }
        }

        private bool isReady = true;
        private bool isPing = false;

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
        private void toolStripButton1_Click_1(object sender, EventArgs e) => MyMap.СlipRenderingArea(drawLevel);

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            drawLevel.Floor = comboBox1.SelectedIndex;
        }

        private void toolStripButton3_ButtonClick(object sender, EventArgs e)
        {

        }

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

        private void сетевыеЭлементыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LegendForm legendForm = new LegendForm();
            legendForm.Show();
        }

        private void редактированиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MyMap.EditRects.edit_mode)
            {
                MyMap.SetInstrument(0);
                редактированиеToolStripMenuItem.Checked = false;
                курсорToolStripMenuItem.Checked = true;
                линияToolStripMenuItem.Checked = false;
                кругToolStripMenuItem.Checked = false;
                прямоугольникToolStripMenuItem.Checked = false;
                многоугольникToolStripMenuItem.Checked = false;
                текстToolStripMenuItem.Checked = false;
                проводаToolStripMenuItem.Checked = false;
            }
            else
            {
                редактированиеToolStripMenuItem.Checked = true;
                курсорToolStripMenuItem.Checked = false;
                линияToolStripMenuItem.Checked = false;
                кругToolStripMenuItem.Checked = false;
                прямоугольникToolStripMenuItem.Checked = false;
                многоугольникToolStripMenuItem.Checked = false;
                текстToolStripMenuItem.Checked = false;
                проводаToolStripMenuItem.Checked = false;
                MyMap.SetInstrument(3);
                MyMap.RefreshEditRect();
            }
        }

        private void поискToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void фильтрыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FiltersForm ff = new FiltersForm();
            ff.ShowDialog();
            RefreshButtons();
        }

        private void обрезатьОбластьОтрисовкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyMap.СlipRenderingArea(drawLevel);
        }

        private void курсорToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyMap.SetInstrument(0);
            редактированиеToolStripMenuItem.Checked = false;
            курсорToolStripMenuItem.Checked = true;
            линияToolStripMenuItem.Checked = false;
            кругToolStripMenuItem.Checked = false;
            прямоугольникToolStripMenuItem.Checked = false;
            многоугольникToolStripMenuItem.Checked = false;
            текстToolStripMenuItem.Checked = false;
            проводаToolStripMenuItem.Checked = false;
        }

        private void линияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyMap.SetInstrument(1);
            редактированиеToolStripMenuItem.Checked = false;
            курсорToolStripMenuItem.Checked = false;
            линияToolStripMenuItem.Checked = true;
            кругToolStripMenuItem.Checked = false;
            прямоугольникToolStripMenuItem.Checked = false;
            многоугольникToolStripMenuItem.Checked = false;
            текстToolStripMenuItem.Checked = false;
            проводаToolStripMenuItem.Checked = false;
        }

        private void многоугольникToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyMap.SetInstrument(5);
            редактированиеToolStripMenuItem.Checked = false;
            курсорToolStripMenuItem.Checked = false;
            линияToolStripMenuItem.Checked = false;
            кругToolStripMenuItem.Checked = false;
            прямоугольникToolStripMenuItem.Checked = false;
            многоугольникToolStripMenuItem.Checked = true;
            текстToolStripMenuItem.Checked = false;
            проводаToolStripMenuItem.Checked = false;
        }

        private void прямоугольникToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyMap.SetInstrument(2);
            редактированиеToolStripMenuItem.Checked = false;
            курсорToolStripMenuItem.Checked = false;
            линияToolStripMenuItem.Checked = false;
            кругToolStripMenuItem.Checked = false;
            прямоугольникToolStripMenuItem.Checked = true;
            многоугольникToolStripMenuItem.Checked = false;
            текстToolStripMenuItem.Checked = false;
            проводаToolStripMenuItem.Checked = false;
        }

        private void кругToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyMap.SetInstrument(360);
            редактированиеToolStripMenuItem.Checked = false;
            курсорToolStripMenuItem.Checked = false;
            линияToolStripMenuItem.Checked = false;
            кругToolStripMenuItem.Checked = true;
            прямоугольникToolStripMenuItem.Checked = false;
            многоугольникToolStripMenuItem.Checked = false;
            текстToolStripMenuItem.Checked = false;
            проводаToolStripMenuItem.Checked = false;
        }

        private void текстToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyMap.SetInstrument(10);
            редактированиеToolStripMenuItem.Checked = false;
            курсорToolStripMenuItem.Checked = false;
            линияToolStripMenuItem.Checked = false;
            кругToolStripMenuItem.Checked = false;
            прямоугольникToolStripMenuItem.Checked = false;
            многоугольникToolStripMenuItem.Checked = false;
            текстToolStripMenuItem.Checked = true;
            проводаToolStripMenuItem.Checked = false;
        }

        private void экспортКартыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyMap.Unfocus(true);
            Filtres _filtres = filtres;
            MapExportForm MEF = new MapExportForm(MyMap.Buildings.Buildings);
            MEF.ShowDialog();
            filtres = _filtres;
        }

        private void экспортимпортЗданийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BuildExportImport buildExportImport = new BuildExportImport(MyMap.Buildings.Buildings);
            buildExportImport.ShowDialog();
            if (buildExportImport.action)
                CheckButtons(true);
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveTemplateMap(".ndmt", "Network Design Map File (Template)");
        }

        private void проводаToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Генерация текстуры
        /// </summary>
        /// <param name="id">Идентификатор текстуры в списке ссылок</param>
        /// <param name="url">Ссылка</param>
        public static bool GenTex(string url)
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
                if (bitspp == 32 | bitspp == 24 | bitspp == 16)
                {
                    switch (bitspp) // в зависимости от полученного результата 
                    {
                        case 16:
                            colorSettings.idtexture = MakeGlTexture(Gl.GL_RGB16, Il.ilGetData(), width, height);
                            break;
                        case 24:
                            colorSettings.idtexture = MakeGlTexture(Gl.GL_RGB, Il.ilGetData(), width, height);
                            break;
                        case 32:
                            colorSettings.idtexture = MakeGlTexture(Gl.GL_RGBA, Il.ilGetData(), width, height);
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
