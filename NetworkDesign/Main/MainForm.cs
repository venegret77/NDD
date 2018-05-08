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

namespace NetworkDesign
{
    public partial class MainForm : Form
    {
        public static string user = "";
        MapSettings DefaultSettings = new MapSettings("DefaultMap", 1000, 1000);
        static public Map MyMap = new Map();
        static public DrawLevel drawLevel;
        static public ColorSettings colorSettings = new ColorSettings();
        static public Parametrs parametrs = new Parametrs();
        //
        static public List<string> ImagesURL = new List<string>();
        static public List<uint> Textures = new List<uint>();
        static public List<string> DeleteImages = new List<string>();
        static public bool isLoad = false;
        //
        static public int _Height = 0, _Width = 0;
        static public SimpleOpenGlControl AnT = new SimpleOpenGlControl();
        ActiveElem activeElem = new ActiveElem();
        private List<string> floors_name = new List<string>();
        private int floor_index = 0;

        public MainForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            panel1.Controls.Add(AnT);
            // AnT
            // 
            AnT.AccumBits = ((byte)(0));
            AnT.AutoCheckErrors = false;
            AnT.AutoFinish = false;
            AnT.AutoMakeCurrent = true;
            AnT.AutoSize = true;
            AnT.AutoSwapBuffers = true;
            AnT.BackColor = Color.Black;
            AnT.ColorBits = ((byte)(32));
            AnT.Cursor = Cursors.Cross;
            AnT.DepthBits = ((byte)(16));
            AnT.Location = new Point(3, 3);
            AnT.Name = "AnT";
            AnT.Size = new Size(1000, 1000);
            AnT.StencilBits = ((byte)(0));
            AnT.TabIndex = 1;
            AnT.MouseDown += new MouseEventHandler(AnT_MouseDown);
            AnT.MouseMove += new MouseEventHandler(AnT_MouseMove);
            AnT.MouseUp += new MouseEventHandler(AnT_MouseUp);
            AnT.MouseDoubleClick += AnT_MouseDoubleClick;
            // 
            AnT.InitializeContexts();
            MyMap = new Map(DefaultSettings);
            Text = DefaultSettings.Name;
            drawLevel.Level = -1;
            drawLevel.Floor = -1;
            _Height = AnT.Height;
            _Width = AnT.Width;
            panel2.Parent = this;
            panel3.Parent = this;
            colorSettings = ColorSettings.Open();
            parametrs = Parametrs.Open();
            ImagesURL = ImageTextures.Open();
        }

        /// <summary>
        /// Функция для поиска элемента, в который попал клик мыши
        /// </summary>
        /// <param name="x">Координата мыши X</param>
        /// <param name="y">Координата мыши Y</param>
        private void SelectItems(int x, int y)
        {
            Unfocus("Не выбран элемент");
            activeElem.item = MyMap.SearchElem(x, y, out activeElem.type, out activeElem.build, drawLevel);
            switch (activeElem.type)
            {
                case 1:
                    MyMap.Lines.Choose(activeElem.item);
                    InfoLable.Text = "Выбрана линия " + activeElem.item;
                    DeleteBtn.Enabled = true;
                    break;
                case 2:
                    MyMap.Rectangles.Choose(activeElem.item);
                    InfoLable.Text = "Выбран прямоугольник " + activeElem.item;
                    DeleteBtn.Enabled = true;
                    if (drawLevel.Level == -1)
                        BuildBtn.Enabled = true;
                    break;
                case 3:
                    MyMap.Polygons.Choose(activeElem.item);
                    InfoLable.Text = "Выбран многоугольник " + activeElem.item;
                    DeleteBtn.Enabled = true;
                    if (drawLevel.Level == -1)
                        BuildBtn.Enabled = true;
                    break;
                case 4:
                    MyMap.Buildings.Choose(activeElem.item);
                    InfoLable.Text = "Выбрано здание " + activeElem.item + " '" + MyMap.Buildings.Buildings[activeElem.item].Name + "'";
                    DeleteBtn.Enabled = true;
                    if (drawLevel.Level == -1)
                    {
                        BuildBtn.Enabled = true;
                        AddEntranceBtn.Enabled = true;
                        AddIWBtn.Enabled = true;
                    }
                    break;
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
                case 360:
                    MyMap.Circles.Choose(activeElem.item);
                    InfoLable.Text = "Выбрана окружность" + activeElem.item;
                    DeleteBtn.Enabled = true;
                    if (drawLevel.Level == -1)
                        BuildBtn.Enabled = true;
                    break;
            }
        }

        private void MouseBLines(int x, int y)
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
                Element elem = new Element(3, lastindex, MyMap.Polygons.Polygons[lastindex], -1);
                Element _elem = new Element(3, lastindex, new Polygon(), -1);
                MyMap.log.Add(new LogMessage("Добавил многоугольник", elem, _elem));
                InfoLable.Text = "Добавил многоугольник";
                CheckButtons(true);
            }
        }

        private void MouseLines(int x, int y)
        {
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
                Element elem = new Element(1, lastindex, MyMap.Lines.Lines[lastindex], -1);
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
                Element elem = new Element(2, lastindex, MyMap.Rectangles.Rectangles[lastindex], -1);
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
                Element elem = new Element(360, lastindex, MyMap.Circles.Circles[lastindex], -1);
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
                if (IT.imageindex > -1)
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
                Element elem = new Element(8, lastindex, MyMap.NetworkElements.NetworkElements[lastindex], -1);
                Element _elem = new Element(8, lastindex, new NetworkElement(), -1);
                MyMap.log.Add(new LogMessage("Добавил сетевой элемент", elem, _elem));
                InfoLable.Text = "Добавил сетевой элемент";
                CheckButtons(true);
            }
        }

        private void AnT_MouseDown(object sender, MouseEventArgs e)
        {
            int y = MyMap.InverseY(e.Y);
            if (e.Button == MouseButtons.Left)
            {
                switch (MyMap.RB)
                {
                    case 0:
                        SelectItems(e.X, y);
                        break;
                    case 1:
                        MouseLines(e.X, y);
                        break;
                    case 2:
                        MouseRects(e.X, y);
                        break;
                    case 3:
                        if (MyMap.EditRects.edit_active)
                        {
                            MyMap.EditRects.edit_active = false;
                        }
                        break;
                    case 4:
                        break;
                    case 5:
                        MyMap.Polygons.active = true;
                        MouseBLines(e.X, y);
                        break;
                    case 360:
                        MouseCircle(e.X, y);
                        break;
                    case 6:
                        if (drawLevel.Level != -1)
                        {
                            if (drawLevel.Floor != 0)
                            {
                                if (!MyMap.Buildings.Buildings[activeElem.item].InputWires.step)
                                {
                                    MyMap.Buildings.Buildings[activeElem.item].AddIWInBuild(e.X, y, drawLevel);
                                }
                                else
                                {
                                    MyMap.Buildings.Buildings[activeElem.item].AddIWInBuild(e.X, y, drawLevel);
                                    int lastindex = MyMap.Buildings.Buildings[activeElem.item].InputWires.InputWires.Circles.Count - 1;
                                    Element elem = new Element(6, lastindex, MyMap.Buildings.Buildings[activeElem.item].InputWires.InputWires.Circles[lastindex], -1);
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
                                    MyMap.Buildings.Buildings[activeElem.item].AddIW(e.X, y, IWF.side, IWF.floor_index);
                            }
                            else
                            {
                                MyMap.Buildings.Buildings[activeElem.item].AddIW(e.X, y, false, -1);
                                int lastindex = MyMap.Buildings.Buildings[activeElem.item].InputWires.InputWires.Circles.Count - 1;
                                Element elem = new Element(6, lastindex, MyMap.Buildings.Buildings[activeElem.item].InputWires.InputWires.Circles[lastindex], -1);
                                Element _elem = new Element(6, lastindex, new Circle(), -1);
                                MyMap.log.Add(new LogMessage("Добавил вход провода в здание", elem, _elem, activeElem.item));
                                InfoLable.Text = "Добавил вход провода в здание";
                            }
                        }
                        break;
                    case 7:
                        if (MyMap.Buildings.Buildings[activeElem.item].AddEntrance(e.X, y))
                        {
                            int lastindex = MyMap.Buildings.Buildings[activeElem.item].Entrances.Enterances.Circles.Count - 1;
                            Element elem = new Element(7, lastindex, MyMap.Buildings.Buildings[activeElem.item].Entrances.Enterances.Circles[lastindex], -1);
                            Element _elem = new Element(7, lastindex, new Circle(), -1);
                            MyMap.log.Add(new LogMessage("Добавил вход в здание", elem, _elem, activeElem.item));
                            InfoLable.Text = "Добавил вход в здание";
                        }
                        break;
                    case 8:
                        MouseNE(e.X, y);
                        break;
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                switch (MyMap.RB)
                {
                    case 0:
                        SelectItems(e.X, y);
                        if (activeElem.type == 4)
                        {
                            drawLevel.Level = activeElem.item;
                            drawLevel.Floor = 0;
                            ButtonReturnToMain.Enabled = true;
                            UpgrateFloors();
                            Unfocus("Выбрано здание " + activeElem.item + " '" + MyMap.Buildings.Buildings[activeElem.item].Name + "'");
                            AddIWBtn.Enabled = true;
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
                        MyMap.Polygons.TempPolygon.ClearTempPoint();
                        MyMap.Polygons.active = false;
                        MouseBLines(e.X, y);
                        MyMap.Polygons.TempPolygon = new Polygon();
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
                }
            }
        }

        private void AnT_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int y = MyMap.InverseY(e.Y);
            if (e.Button == MouseButtons.Left)
            {
                switch (MyMap.RB)
                {
                    case 0:

                    case 8:
                        MyMap.SearchNE(e.X, y);
                        break;
                }
            }
        }

        private void UpgrateFloors()
        {
            FloorDown.Visible = true;
            FloorUP.Visible = true;
            label1.Visible = true;
            floors_name.AddRange(MyMap.Buildings.Buildings[activeElem.item].floors_name);
            label1.Text = floors_name[0];
            floor_index = 0;
        }

        private void AnT_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                switch (MyMap.RB)
                {
                    case 3:
                        MyMap.RefreshEditRect();
                        break;
                }
            }
        }

        private void AnT_MouseMove(object sender, MouseEventArgs e)
        {
            int y = MyMap.InverseY(e.Y);
            switch (MyMap.RB)
            {
                case 1:
                    if (MyMap.Lines.step)
                    {
                        MyMap.Lines.TempLine.SetPoint(e.X, y, 1);
                    }
                    break;
                case 2:
                    if (MyMap.Rectangles.step_rect == 1)
                    {
                        MyMap.Rectangles.TempRectangle.SetPoint(e.X, y, 2);
                    }
                    else if (MyMap.Rectangles.step_rect == 2)
                    {
                        MyMap.Rectangles.TempRectangle.SetPoint(e.X, y, 34);
                    }
                    break;
                case 3:
                    if (e.Button == MouseButtons.Left)
                    {
                        if (!MyMap.EditRects.edit_active)
                        {
                            MyMap.SearchEditElem(e.X, y);
                        }
                        else
                        {
                            MyMap.MoveElements(e.X, y);
                        }
                    }
                    break;
                case 5:
                    if (MyMap.Polygons.active & MyMap.Polygons.step)
                    {
                        MyMap.Polygons.TempPolygon.SetTempPoint(e.X, y);
                    }
                    break;
                case 360:
                    if (MyMap.Circles.step)
                    {
                        MyMap.Circles.TempCircle.SetRadius(e.X, y);
                    }
                    break;
                case 6:
                    if (MyMap.Buildings.Buildings[activeElem.item].InputWires.step)
                    {
                        if (drawLevel.Level == -1)
                        {
                            MyMap.Buildings.Buildings[activeElem.item].MoveIW(e.X, y);
                        }
                        else
                        {
                            MyMap.Buildings.Buildings[activeElem.item].MoveIWInBuild(e.X, y);
                        }
                    }
                    break;
                case 7:
                    if (MyMap.Buildings.Buildings[activeElem.item].Entrances.step)
                    {
                        MyMap.Buildings.Buildings[activeElem.item].MoveEntrance(e.X, y);
                    }
                    break;
                case 8:
                    if (MyMap.NetworkElements.step)
                    {
                        MyMap.NetworkElements.TempNetworkElement.SetPoint(e.X, y);
                    }
                    break;
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
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

        private void сохранитьToolStripButton1_Click(object sender, EventArgs e) => SaveMap(".ndm", "Network Design Map File");

        /// <summary>
        /// Функция для сохранения карты в файл 
        /// </summary>
        /// <param name="fileExtension">Расширение файла в формате .*</param>
        /// <param name="descriptionFE">Описание заданного формата для отображения в диалоге</param>
        private void SaveMap(string fileExtension, string descriptionFE)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = descriptionFE + "|*" + fileExtension;
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
                // получаем поток, куда будем записывать сериализованный объект
                using (FileStream fs = new FileStream(filename + "._temp", FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, MyMap);
                }
                Compress(filename + "._temp", filename);
                File.Delete(filename + "._temp");
            }
        }

        /// <summary>
        /// Функиця для архивации одного файла с использованием алгоритма сжатия ZIP
        /// </summary>
        /// <param name="sourceFile">Путь к исходному файлу</param>
        /// <param name="compressedFile">Путь к получаемому файлу</param>
        public static void Compress(string sourceFile, string compressedFile)
        {
            // поток для чтения исходного файла
            using (FileStream sourceStream = new FileStream(sourceFile, FileMode.OpenOrCreate))
            {
                // поток для записи сжатого файла
                using (FileStream targetStream = File.Create(compressedFile))
                {
                    // поток архивации
                    using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                    {
                        sourceStream.CopyTo(compressionStream); // копируем байты из одного потока в другой
                        /*Console.WriteLine("Сжатие файла {0} завершено. Исходный размер: {1}  сжатый размер: {2}.",
                            sourceFile, sourceStream.Length.ToString(), targetStream.Length.ToString());*/
                    }
                }
            }
        }

        /// <summary>
        /// Функция для разархивации одного файла с использованием алгоритма ZIP
        /// </summary>
        /// <param name="compressedFile">Путь к исходному файлу</param>
        /// <param name="targetFile">Путь к получаемому файлу</param>
        public static void Decompress(string compressedFile, string targetFile)
        {
            // поток для чтения из сжатого файла
            using (FileStream sourceStream = new FileStream(compressedFile, FileMode.OpenOrCreate))
            {
                // поток для записи восстановленного файла
                using (FileStream targetStream = File.Create(targetFile))
                {
                    // поток разархивации
                    using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(targetStream);
                        //Console.WriteLine("Восстановлен файл: {0}", targetFile);
                    }
                }
            }
        }

        private string SubStrDel(string str, string substr)
        {
            int n = str.IndexOf(substr);
            str = str.Remove(n, substr.Length);
            return str;
        }

        private void создатьToolStripButton1_Click(object sender, EventArgs e)
        {
            MapControl mapControl = new MapControl(MyMap.mapSetting);
            mapControl.ShowDialog();
            MyMap.SetNewSettings(mapControl.mapSettings);
            //MyMap = new Map(mapControl.mapSettings);
            Text = MyMap.mapSetting.Name;
        }

        private void открытьToolStripButton1_Click(object sender, EventArgs e) => OpenMap(".ndm", "Network Design Map File");

        /// <summary>
        /// Функция для открытия файла карты
        /// </summary>
        /// <param name="fileExtension">Расширение файла в формате .*</param>
        /// <param name="descriptionFE">Описание заданного формата для отображения в диалоге</param>
        private void OpenMap(string fileExtension, string descriptionFE)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = descriptionFE + "|*" + fileExtension;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Decompress(openFileDialog1.FileName, openFileDialog1.FileName + "._temp");
                XmlSerializer formatter = new XmlSerializer(typeof(Map));
                // получаем поток, куда будем записывать сериализованный объект
                using (FileStream fs = new FileStream(openFileDialog1.FileName + "._temp", FileMode.OpenOrCreate))
                {
                    Map TempMap = (Map)formatter.Deserialize(fs);
                    MyMap.MapLoad(TempMap);
                }
                File.Delete(openFileDialog1.FileName + "._temp");
            }
            CheckButtons(true);
        }

        private void toolStripButton3_Click(object sender, EventArgs e) => MyMap.SetInstrument(0);

        private void toolStripButton1_Click_1(object sender, EventArgs e) => MyMap.SetInstrument(1);

        private void toolStripButton2_Click_1(object sender, EventArgs e) => MyMap.SetInstrument(5);

        private void toolStripButton4_Click(object sender, EventArgs e) => MyMap.SetInstrument(2);

        private void параметрыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialogForm colorDialog = new ColorDialogForm();
            colorDialog.ShowDialog();
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

        private void посмотретьЛогToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormLog formlog = new FormLog(MyMap.log.Back);
            formlog.Show();
        }

        /// <summary>
        /// Функция для возврата элементов к исходному состоянию, без фокусировки на определенном элементе
        /// </summary>
        private void Unfocus(string info)
        {
            MyMap.DefaultTempElems();
            //
            InfoLable.Text = info;
            //
            BuildBtn.Enabled = false;
            DeleteBtn.Enabled = false;
            AddEntranceBtn.Enabled = false;
            AddIWBtn.Enabled = false;
            //
            /*activeElem.type = -1;
            activeElem.item = -1;
            activeElem.build = -1;*/
            //
            MyMap.Lines.Choose(-1);
            MyMap.Rectangles.Choose(-1);
            MyMap.Polygons.Choose(-1);
            MyMap.Buildings.Choose(-1);
            MyMap.Circles.Choose(-1);
        }

        private void BuildBtn_Click(object sender, EventArgs e)
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

        private void ButtonReturnToMain_Click(object sender, EventArgs e)
        {
            drawLevel.Level = -1;
            drawLevel.Floor = -1;
            ButtonReturnToMain.Enabled = false;
            FloorUP.Visible = false;
            FloorDown.Visible = false;
            label1.Visible = false;
            floor_index = 0;
            floors_name = new List<string>();
            Unfocus("Нет активного элемента");
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            Element elem, _elem;
            switch (activeElem.type)
            {
                case 1:
                    elem = new Element(1, activeElem.item, new Line(), -1);
                    _elem = new Element(1, activeElem.item, MyMap.Lines.Lines[activeElem.item], -1);
                    MyMap.log.Add(new LogMessage("Удалил линию", elem, _elem));
                    InfoLable.Text = "Удалил линию";
                    MyMap.Lines.Remove(activeElem.item);
                    CheckButtons(true);
                    break;
                case 2:
                    elem = new Element(2, activeElem.item, new MyRectangle(), -1);
                    _elem = new Element(2, activeElem.item, MyMap.Rectangles.Rectangles[activeElem.item], -1);
                    MyMap.log.Add(new LogMessage("Удалил прямоугольник", elem, _elem));
                    InfoLable.Text = "Удалил прямоугольник";
                    MyMap.Rectangles.Remove(activeElem.item);
                    CheckButtons(true);
                    break;
                case 3:
                    elem = new Element(3, activeElem.item, new Polygon(), -1);
                    _elem = new Element(3, activeElem.item, MyMap.Polygons.Polygons[activeElem.item], -1);
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
                    elem = new Element(6, activeElem.item, new Circle(), -1);
                    _elem = new Element(6, activeElem.item, MyMap.Buildings.Buildings[activeElem.build].InputWires.InputWires.Circles[activeElem.item], -1);
                    MyMap.log.Add(new LogMessage("Удалил вход провода в здание", elem, _elem, activeElem.build));
                    InfoLable.Text = "Удалил вход провода в здание";
                    MyMap.Buildings.Buildings[activeElem.build].InputWires.InputWires.Remove(activeElem.item);
                    CheckButtons(true);
                    break;
                case 7:
                    elem = new Element(7, activeElem.item, new Circle(), -1);
                    _elem = new Element(7, activeElem.item, MyMap.Buildings.Buildings[activeElem.build].Entrances.Enterances.Circles[activeElem.item], -1);
                    MyMap.log.Add(new LogMessage("Удалил вход в здание", elem, _elem, activeElem.build));
                    InfoLable.Text = "Удалил вход в здание";
                    MyMap.Buildings.Buildings[activeElem.build].Entrances.Enterances.Remove(activeElem.item);
                    CheckButtons(true);
                    break;
                case 360:
                    elem = new Element(360, activeElem.item, new Circle(), -1);
                    _elem = new Element(360, activeElem.item, MyMap.Circles.Circles[activeElem.item], -1);
                    MyMap.log.Add(new LogMessage("Удалил окружность", elem, _elem));
                    InfoLable.Text = "Удалил окружность";
                    MyMap.Circles.Remove(activeElem.item);
                    break;
            }
            Unfocus("Удалил элемент");
        }

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

        private void PharseElem(Element _elem, Element elem, bool b)
        {
            if (_elem.transform != -1)
            {
                if (b)
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
                }
            }
        }

        private void PharseElem(Element _elem, Element elem, int buildid)
        {
            switch (elem.type)
            {
                case 6:
                    MyMap.Buildings.Buildings[buildid].InputWires.InputWires.Circles[elem.index] = (Circle)elem.elem;
                    break;
                case 7:
                    MyMap.Buildings.Buildings[buildid].Entrances.Enterances.Circles[elem.index] = (Circle)elem.elem;
                    break;
            }
        }

        //private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e) => drawLevel.Floor = domainUpDown1.SelectedIndex;

        private void ToolStripButton6_Click_1(object sender, EventArgs e) => MyMap.SetInstrument(360);

        private void AddEntranceBtn_Click(object sender, EventArgs e) => MyMap.SetInstrument(7);

        private void AddIWBtn_Click(object sender, EventArgs e) => MyMap.SetInstrument(6);

        private void ExporImportBuildBtn_Click(object sender, EventArgs e)
        {
            /*if (activeElem.type == 4)
            {*/
                SaveBuild(".build", "Building File");
            /*}
            else
            {
                OpenBuild(".build", "Building File");
            }*/
        }

        /// <summary>
        /// Функция для сохранения здания в файл 
        /// </summary>
        /// <param name="fileExtension">Расширение файла в формате .*</param>
        /// <param name="descriptionFE">Описание заданного формата для отображения в диалоге</param>
        private void SaveBuild(string fileExtension, string descriptionFE)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = descriptionFE + "|*" + fileExtension;
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
                Compress(filename + "._temp", filename);
                File.Delete(filename + "._temp");
            }
        }

        /// <summary>
        /// Функция для открытия карты файла
        /// </summary>
        /// <param name="fileExtension">Расширение файла в формате .*</param>
        /// <param name="descriptionFE">Описание заданного формата для отображения в диалоге</param>
        private void OpenBuild(string fileExtension, string descriptionFE)
        {
            Map TempMap = new Map();
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = descriptionFE + "|*" + fileExtension;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Decompress(openFileDialog1.FileName, openFileDialog1.FileName + "._temp");
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

        static object Conv(object elem) => elem;

        private void toolStripButton7_Click(object sender, EventArgs e) => SaveTemplateMap(".ndm", "Network Design Map File");

        /// <summary>
        /// Функция для сохранения здания в файл 
        /// </summary>
        /// <param name="fileExtension">Расширение файла в формате .*</param>
        /// <param name="descriptionFE">Описание заданного формата для отображения в диалоге</param>
        private void SaveTemplateMap(string fileExtension, string descriptionFE)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = descriptionFE + "|*" + fileExtension;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                XmlSerializer formatter = new XmlSerializer(typeof(Map));
                Map TempMap = new Map();
                foreach(var b in MyMap.Buildings.Buildings)
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
                Compress(filename + "._temp", filename);
                File.Delete(filename + "._temp");
            }
        }

        private void FloorUP_Click(object sender, EventArgs e)
        {
            if (floor_index != floors_name.Count - 1)
            {
                floor_index++;
                label1.Text = floors_name[floor_index];
                drawLevel.Floor = floor_index;
            }
        }

        private void FloorDown_Click(object sender, EventArgs e)
        {
            if (floor_index != 0)
            {
                floor_index--;
                label1.Text = floors_name[floor_index];
                drawLevel.Floor = floor_index;
            }
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            OpenBuild(".build", "Building File");
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ColorSettings.Save(colorSettings);
            ImageTextures.Save(ImagesURL);
            Parametrs.Save(parametrs);
            Application.Exit();
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            SearchDialog sd = new SearchDialog();
            sd.ShowDialog();
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            MessageBox.Show("заметка добавлена");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            /*ElementParams elementParams = new ElementParams();
            elementParams.ShowDialog();*/
        }

        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            ImageTextures imageTextures = new ImageTextures(ref MyMap.NetworkElements);
            imageTextures.ShowDialog();

            /*ToolStripButton knopka = new ToolStripButton(images.Images[imageTextures.imageindex]);
            toolStrip1.Items.Add(knopka);*/
        }

        private void toolStripButton13_Click(object sender, EventArgs e) => MyMap.SetInstrument(8);
    }
}
