using NetworkDesign.Buildings;
using NetworkDesign.Main;
using NetworkDesign.NetworkElements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Tao.DevIl;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform.Windows;

namespace NetworkDesign
{
    /// <summary>
    /// Карта сети
    /// </summary>
    public class Map
    {
        /// <summary>
        /// Параметры карты сети
        /// </summary>
        public SizeRenderingArea sizeRenderingArea;
        /// <summary>
        /// Инструмент
        /// </summary>
        public int Instrument = 1;
        /// <summary>
        /// Группа линий
        /// </summary>
        public GroupOfLines Lines = new GroupOfLines();
        /// <summary>
        /// Группа многоугольников
        /// </summary>
        public GroupOfPolygons Polygons = new GroupOfPolygons();
        /// <summary>
        /// Группа прямоугольников
        /// </summary>
        public GroupOfRectangles Rectangles = new GroupOfRectangles(); 
        /// <summary>
        /// Группа кругов
        /// </summary>
        public GroupOfCircle Circles = new GroupOfCircle();
        /// <summary>
        /// Группа сетевых элементов
        /// </summary>
        public GroupOfNE NetworkElements = new GroupOfNE();
        /// <summary>
        /// Группа проводов
        /// </summary>
        public GroupOfNW NetworkWires = new GroupOfNW();
        /// <summary>
        /// Группа надписей
        /// </summary>
        public GroupOfMT MyTexts = new GroupOfMT();
        /// <summary>
        /// Группа прямоугольников для редактирования
        /// </summary>
        public GroupOfEditRects EditRects = new GroupOfEditRects();
        /// <summary>
        /// Лог действий
        /// </summary>
        public Log log = new Log();
        /// <summary>
        /// Группа здани
        /// </summary>
        public GroupOfBuildings Buildings = new GroupOfBuildings();
        /// <summary>
        /// Логин текущего пользователя
        /// </summary>
        public string UserLogin;
        /// <summary>
        /// Таймер для отрисовки
        /// </summary>
        [XmlIgnore()]
        public Timer RenderTimer = new Timer();
        /// <summary>
        /// Переменная, показывающая, перемещается ли в данный момент элемент или нет
        /// </summary>
        public bool isMove = false;
        /// <summary>
        /// Пустой конструктор класса Map
        /// </summary>
        public Map()
        {

        }

        /// <summary>
        /// Конструктор класса Map с параметрама
        /// </summary>
        /// <param name="_MapSetting">Параметры карты сети</param>
        public Map(SizeRenderingArea _MapSetting)
        {
            MainForm.AnT.Height = _MapSetting.Height;
            MainForm.AnT.Width = _MapSetting.Width;
            if (!MainForm.isInitMap)
            {
                // инициализация библиотеки glut 
                Glut.glutInit();
            }
            // инициализация режима экрана 
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE);
            // инициализация библиотеки openIL 
            Il.ilInit();
            Il.ilEnable(Il.IL_ORIGIN_SET);
            // установка цвета очистки экрана (RGBA) 
            Gl.glClearColor(1, 1, 1, 1);
            // установка порта вывода 
            Gl.glViewport(0, 0, MainForm.AnT.Width, MainForm.AnT.Height);
            // активация проекционной матрицы 
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            // очистка матрицы 
            Gl.glLoadIdentity();
            // установка перспективы 
            Glu.gluOrtho2D(-_MapSetting.Width / 2, _MapSetting.Width / 2, -_MapSetting.Height / 2, _MapSetting.Height / 2);
            // установка объектно-видовой матрицы 
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Gl.glEnable(Gl.GL_BLEND);
            //Gl.glScaled(1, 1, 1);
            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
            // начальные настройки OpenGL 
            //Gl.glEnable( Gl.GL_DEPTH_TEST);
            sizeRenderingArea = _MapSetting;
            RenderTimer.Interval = 15;
            RenderTimer.Tick += RenderTimer_Tick;
            RenderTimer.Start();
            //MainForm.isInit = true;
        }
        /// <summary>
        /// Событие тика таймера отрисовки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void RenderTimer_Tick(object sender, EventArgs e) => Drawing();
        /// <summary>
        /// Отрисовка
        /// </summary>
        public void Drawing()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glLoadIdentity();
            Gl.glClearColor(1, 1, 1, 1);
            DrawTemp();
            Gl.glPushMatrix();
            Gl.glScaled(MainForm.zoom, MainForm.zoom, MainForm.zoom);
            DrawBackground();
            if (MainForm.filtres.NW)
                NetworkWires.Draw();
            if (MainForm.filtres.Build)
                Buildings.Draw();
            if (MainForm.filtres.Poly)
                Polygons.Draw();
            if (MainForm.filtres.Line)
                Lines.Draw();
            if (MainForm.filtres.Rect)
                Rectangles.Draw();
            if (MainForm.filtres.Circ)
                Circles.Draw();
            if (MainForm.filtres.NE)
                NetworkElements.Draw();
            if (MainForm.filtres.Text)
                MyTexts.Draw();
            if (EditRects.edit_mode)
            {
                EditRects.Draw();
            }
            Gl.glPopMatrix();
            MainForm.AnT.Invalidate();
        }
        /// <summary>
        /// Отрисовка фона / подложки
        /// </summary>
        private void DrawBackground()
        {
            if (MainForm.colorSettings.backgroundurl != "" & MainForm.colorSettings.isDrawBackground)
            {
                Gl.glEnable(Gl.GL_TEXTURE_2D);
                // включаем режим текстурирования, указывая идентификатор mGlTextureObject 
                Gl.glBindTexture(Gl.GL_TEXTURE_2D, MainForm.colorSettings.idtexture);
                // отрисовываем полигон 
                Gl.glBegin(Gl.GL_QUADS);
                Gl.glColor4f(0, 0, 0, 1);
                // указываем поочередно вершины и текстурные координаты 
                Gl.glVertex2d((double)sizeRenderingArea.Left, (double)sizeRenderingArea.Bottom);
                Gl.glTexCoord2f(0, (float)MainForm.hkoef * (float)MainForm.zoom);
                Gl.glVertex2d((double)sizeRenderingArea.Left, (double)sizeRenderingArea.Top);
                Gl.glTexCoord2f((float)MainForm.wkoef * (float)MainForm.zoom, (float)MainForm.hkoef * (float)MainForm.zoom);
                Gl.glVertex2d((double)sizeRenderingArea.Right, (double)sizeRenderingArea.Top);
                Gl.glTexCoord2f((float)MainForm.wkoef * (float)MainForm.zoom, 0);
                Gl.glVertex2d((double)sizeRenderingArea.Right , (double)sizeRenderingArea.Bottom);
                Gl.glTexCoord2f(0, 0);
                // завершаем отрисовку 
                Gl.glEnd();
                // отключаем режим текстурирования 
                Gl.glDisable(Gl.GL_TEXTURE_2D);
            }
        }
        /// <summary>
        /// Отрисовка временных элементов
        /// </summary>
        public void DrawTemp()
        {
            NetworkWires.DrawTemp();
            Buildings.DrawTemp();
            Polygons.DrawTemp();
            Lines.DrawTemp();
            Rectangles.DrawTemp();
            Circles.DrawTemp();
            NetworkElements.DrawTemp();
        }
        /// <summary>
        /// Уменьшение области отрисовки по размерам элементов для текущего вида
        /// </summary>
        /// <param name="dl">Текущи уровень отображения</param>
        internal void СlipRenderingArea(DrawLevel dl)
        {
            int minx = 0;
            int maxx = 0;
            int miny = 0;
            int maxy = 0;
            List<int> Xs = new List<int>();
            List<int> Ys = new List<int>();
            #region Ищем все точки
            foreach (var line in Lines.Lines)
            {
                if (line.DL == dl)
                {
                    foreach (var p in line.Points)
                    {
                        Xs.Add(p.X);
                        Ys.Add(p.Y);
                    }
                }
            }
            foreach (var rect in Rectangles.Rectangles)
            {
                if (rect.DL == dl)
                {
                    foreach (var p in rect.Points)
                    {
                        Xs.Add(p.X);
                        Ys.Add(p.Y);
                    }
                }
            }
            foreach (var pol in Polygons.Polygons)
            {
                if (pol.DL == dl)
                {
                    foreach (var p in pol.Points)
                    {
                        Xs.Add(p.X);
                        Ys.Add(p.Y);
                    }
                }
            }
            foreach (var nw in NetworkWires.NetworkWires)
            {
                if (nw.DL == dl)
                {
                    foreach (var p in nw.Points)
                    {
                        Xs.Add(p.X);
                        Ys.Add(p.Y);
                    }
                }
            }
            foreach (var cir in Circles.Circles)
            {
                if (cir.DL == dl | cir.MainDL == dl)
                {
                    Xs.Add(cir.MainCenterPoint.X + cir.radius);
                    Ys.Add(cir.MainCenterPoint.Y + cir.radius);
                    Xs.Add(cir.MainCenterPoint.X + cir.radius);
                    Ys.Add(cir.MainCenterPoint.Y - cir.radius);
                    Xs.Add(cir.MainCenterPoint.X - cir.radius);
                    Ys.Add(cir.MainCenterPoint.Y + cir.radius);
                    Xs.Add(cir.MainCenterPoint.X - cir.radius);
                    Ys.Add(cir.MainCenterPoint.Y - cir.radius);
                }
            }
            foreach (var build in Buildings.Buildings)
            {
                if (build.MainMapDL == dl)
                {
                    if (build.type == 2)
                    {
                        var rect = build.MainRectangle;
                        foreach (var p in rect.Points)
                        {
                            Xs.Add(p.X);
                            Ys.Add(p.Y);
                        }
                    }
                    else if (build.type == 3)
                    {
                        var pol = build.MainPolygon;
                        foreach (var p in pol.Points)
                        {
                            Xs.Add(p.X);
                            Ys.Add(p.Y);
                        }
                    }
                    else if (build.type == 360)
                    {
                        var cir = build.MainCircle;
                        Xs.Add(cir.MainCenterPoint.X + cir.radius);
                        Ys.Add(cir.MainCenterPoint.Y + cir.radius);
                        Xs.Add(cir.MainCenterPoint.X + cir.radius);
                        Ys.Add(cir.MainCenterPoint.Y - cir.radius);
                        Xs.Add(cir.MainCenterPoint.X - cir.radius);
                        Ys.Add(cir.MainCenterPoint.Y + cir.radius);
                        Xs.Add(cir.MainCenterPoint.X - cir.radius);
                        Ys.Add(cir.MainCenterPoint.Y - cir.radius);
                    }
                }
            }
            foreach (var mt in MyTexts.MyTexts)
            {
                if (mt.DL == dl)
                {
                    Xs.Add(mt.location.X);
                    Ys.Add(mt.location.Y);
                    Xs.Add(mt.location.X + mt.size.Width);
                    Ys.Add(mt.location.Y - mt.size.Height);
                }
            }
            foreach (var ne in NetworkElements.NetworkElements)
            {
                if (ne.DL == dl)
                {
                    Xs.Add(ne.texture.location.X);
                    Ys.Add(ne.texture.location.Y);
                    Xs.Add(ne.texture.location.X + (int)ne.texture.width);
                    Ys.Add(ne.texture.location.Y + (int)ne.texture.width);
                }
            }
#endregion
            foreach (var x in Xs)
            {
                if (x < minx)
                    minx = x;
                else if (x > maxx)
                    maxx = x;
            }
            foreach (var y in Ys)
            {
                if (y < miny)
                    miny = y;
                else if (y > maxy)
                    maxy = y;
            }
            minx -= 100;
            maxx += 100;
            miny -= 100;
            maxy += 100;
            if (dl.Level == -1)
            {
                Element elem = new Element(14, dl.Level, sizeRenderingArea, -2);
                CalcHandW(out int height, out int width, minx, maxx, miny, maxy);
                sizeRenderingArea = new SizeRenderingArea(sizeRenderingArea.Name, height, width);
                ResizeRenderingArea();
                Element _elem = new Element(14, dl.Level, sizeRenderingArea, -2);
                log.Add(new LogMessage("Изменил размеры области отрисовки", elem, _elem));
            }
            else
            {
                Element elem = new Element(14, dl.Level, Buildings.Buildings[dl.Level].Clone(), -2);
                CalcHandW(out int height, out int width, minx, maxx, miny, maxy);
                double pk = Buildings.Buildings[dl.Level].pk;
                if (height * pk > width)
                    width = (int)((double)height * pk);
                else
                    height = (int)((double)width / pk);
                Buildings.Buildings[dl.Level].sizeRenderingArea = new SizeRenderingArea(Buildings.Buildings[dl.Level].sizeRenderingArea.Name, height, width);
                ResizeRenderingArea(dl.Level);
                Buildings.Buildings[dl.Level].RefreshLocal();
                Element _elem = new Element(14, dl.Level, Buildings.Buildings[dl.Level].Clone(), -2);
                log.Add(new LogMessage("Изменил размеры области отрисовки", elem, _elem));
            }
        }
        /// <summary>
        /// Расчет ширины и высоты области отрисовки
        /// </summary>
        /// <param name="height">Возвращаемый параметр: высота</param>
        /// <param name="width">Возвращаемый параметр: ширина</param>
        /// <param name="minx">Минимум по X</param>
        /// <param name="maxx">Максимум по Х</param>
        /// <param name="miny">Минимум по У</param>
        /// <param name="maxy">Максимум по У</param>
        private void CalcHandW(out int height, out int width, int minx, int maxx, int miny, int maxy)
        {
            if (minx < 0 & maxx > 0)
            {
                if (-minx > maxx)
                    maxx = -minx;
                else
                    minx = -maxx;
            }
            else if (minx < 0 & maxx < 0)
            {
                maxx = -minx;
            }
            else if (minx > 0 & maxx > 0)
            {
                minx = -maxx;
            }
            if (miny < 0 & maxy > 0)
            {
                if (-miny > maxy)
                    maxy = -miny;
                else
                    miny = -maxy;
            }
            else if (miny < 0 & maxy < 0)
            {
                maxy = -miny;
            }
            else if (miny > 0 & maxy > 0)
            {
                miny = -maxy;
            }
            height = maxy - miny;
            width = maxx - minx;
        }
        /// <summary>
        /// Экспорт списка элементов
        /// </summary>
        /// <param name="path">Путь сохранения файла</param>
        /// <param name="build">Список выбранных для экспорта зданий</param>
        public void ExportListNE(string path, List<bool> build)
        {
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.Default))
            {
                sw.WriteLine("Список элементов для карты сети " + sizeRenderingArea.Name);
                List<Building> buildings = new List<Building>();
                foreach (var b in Buildings.Buildings)
                    if (!b.delete)
                        buildings.Add(b);
                for (int i = 0; i < buildings.Count; i++)
                {
                    if (build[i])
                    {
                        string buildname = "Здание " + Buildings.Buildings[i].Name + ":";
                        buildname = buildname.Replace("\r\n", "");
                        sw.WriteLine(buildname);
                        for (int n = 0; n < Buildings.Buildings[i].floors_name.Count; n++)
                        {
                            sw.WriteLine(Buildings.Buildings[i].floors_name[n] + ":");
                            for (int j = 0; j < NetworkElements.NetworkElements.Count; j++)
                            {
                                if (NetworkElements.NetworkElements[j].DL.Level == i & NetworkElements.NetworkElements[j].DL.Floor == n)
                                {
                                    sw.WriteLine(NetworkElements.NetworkElements[j].Options.ToString());
                                }
                            }
                        }
                    }
                }
                sw.WriteLine("Прочие сетевые элементы: ");
                for (int j = 0; j < NetworkElements.NetworkElements.Count; j++)
                {
                    if (NetworkElements.NetworkElements[j].DL.Level == -1 & NetworkElements.NetworkElements[j].DL.Floor == -1)
                    {
                        sw.WriteLine(NetworkElements.NetworkElements[j].Options.ToString());
                    }
                }
            }
        }
        /// <summary>
        /// Изменение размеров элемента, отвечающего за отрисовку, а также проекции
        /// </summary>
        public void ResizeRenderingArea()
        {
            int Height = (int)((double)sizeRenderingArea.Height * MainForm.zoom);
            int Width = (int)((double)sizeRenderingArea.Width * MainForm.zoom);
            int Left = (int)((double)sizeRenderingArea.Left * MainForm.zoom);
            int Right = (int)((double)sizeRenderingArea.Right * MainForm.zoom);
            int Top = (int)((double)sizeRenderingArea.Top * MainForm.zoom);
            int Bottom = (int)((double)sizeRenderingArea.Bottom * MainForm.zoom);
            MainForm.AnT.Height = Height;
            MainForm.AnT.Width = Width;
            Gl.glViewport(0, 0, Width, Height);
            // активация проекционной матрицы 
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            // очистка матрицы 
            Gl.glLoadIdentity();
            // установка перспективы 
            Glu.gluOrtho2D(Left, Right, Bottom, Top);
            // установка объектно-видовой матрицы 
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
        }
        /// <summary>
        /// Изменение размеров элемента, отвечающего за отрисовку, а также проекции для нужного здания
        /// </summary>
        /// <param name="buildid"></param>
        public void ResizeRenderingArea(int buildid)
        {
            int Height = (int)((double)Buildings.Buildings[buildid].sizeRenderingArea.Height * MainForm.zoom);
            int Width = (int)((double)Buildings.Buildings[buildid].sizeRenderingArea.Width * MainForm.zoom);
            int Left = (int)((double)Buildings.Buildings[buildid].sizeRenderingArea.Left * MainForm.zoom);
            int Right = (int)((double)Buildings.Buildings[buildid].sizeRenderingArea.Right * MainForm.zoom);
            int Top = (int)((double)Buildings.Buildings[buildid].sizeRenderingArea.Top * MainForm.zoom);
            int Bottom = (int)((double)Buildings.Buildings[buildid].sizeRenderingArea.Bottom * MainForm.zoom);
            MainForm.AnT.Height = Height;
            MainForm.AnT.Width = Width;
            Gl.glViewport(0, 0, Width, Height);
            // активация проекционной матрицы 
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            // очистка матрицы 
            Gl.glLoadIdentity();
            // установка перспективы 
            Glu.gluOrtho2D(Left, Right, Bottom, Top);
            // установка объектно-видовой матрицы 
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
        }
        /// <summary>
        /// Перемещение элемента на заданные координаты
        /// </summary>
        /// <param name="x">Координата Х</param>
        /// <param name="y">Координата У</param>
        public void MoveElem(int x, int y)
        {
            if (!isMove & MainForm.activeElem.item != -1)
                isMove = true;
            if (isMove)
            {
                int id = MainForm.activeElem.item;
                switch (MainForm.activeElem.type)
                {
                    case 1:
                        Lines.Lines[id].MoveElem(x, y);
                        Lines.Lines[id].CalcCenterPoint();
                        break;
                    case 2:
                        Rectangles.Rectangles[id].MoveElem(x, y);
                        Rectangles.Rectangles[id].CalcCenterPoint();
                        break;
                    case 3:
                        Polygons.Polygons[id].MoveElem(x, y);
                        Polygons.Polygons[id].CalcCenterPoint();
                        break;
                    case 4:
                        Buildings.Buildings[id].MoveElem(x, y, NetworkWires, id);
                        Buildings.Buildings[id].CalcCenterPoint();
                        break;
                    case 6:
                        Buildings.Buildings[MainForm.activeElem.build].MoveIW(x, y, id, MainForm.activeElem.build, NetworkWires);
                        break;
                    case 7:
                        if (MainForm.drawLevel.Level == -1)
                            Buildings.Buildings[MainForm.activeElem.build].MoveEntrance(x, y, id);
                        break;
                    case 8:
                        NetworkElements.NetworkElements[id].MoveElem(x, y, id, NetworkWires);
                        NetworkElements.NetworkElements[id].CalcCenterPoint();
                        break;
                    case 10:
                        MyTexts.MyTexts[id].MoveElem(x, y);
                        break;
                    case 360:
                        Circles.Circles[id].MoveElem(x, y);
                        break;
                }
            }
        }

        /// <summary>
        /// Функция для установки инструмента
        /// </summary>
        /// <param name="instrument">ID инструмента:
        /// 0 - курсор для выбора;
        /// 1 - линия;
        /// 2 - прямоугольник;
        /// 3 - редактирование;
        /// 5 - многоугольник;
        /// 6 - точки входа проводов;
        /// 7 - входы в здание;
        /// 8 - сетевые элементы;
        /// 9 - провод;
        /// 10 - текст;
        /// 360 - круг;</param>
        public void SetInstrument(int instrument)
        {
            DefaultTempElems(true);
            MainForm.AnT.Cursor = Cursors.Cross;
            Instrument = instrument;
            if (instrument == 3)
            {
                EditRects.edit_mode = true;
                MainForm.AnT.Cursor = Cursors.Arrow;
            }
            else if (instrument == 0 | instrument == 8)
            {
                MainForm.AnT.Cursor = Cursors.Arrow;
            }
            else if (instrument == 10)
            {
                MainForm.AnT.Cursor = Cursors.IBeam;
            }
        }

        /// <summary>
        /// Функция для задания параметров по умолчанию временным переменным
        /// </summary>
        public void DefaultTempElems(bool er)
        {
            Lines.TempDefault();
            Polygons.TempDefault();
            Rectangles.TempDefault();
            Circles.TempDefault();
            Buildings.TempDefault();
            if (er)
                EditRects.TempDefault();
            NetworkWires.TempDefault();
            NetworkElements.TempDefault();
        }
        /// <summary>
        /// Удаление элементов внутри здания
        /// </summary>
        /// <param name="id">Идентификатор здания</param>
        internal void RemoveBuildElements(int id)
        {
            Element elem, _elem;
            int i = 0;
            for (i = 0; i < Lines.Lines.Count; i++)
            {
                var line = Lines.Lines[i];
                if (line.DL.Level == id & !line.delete)
                {
                    elem = new Element(1, i, new Line(), -1);
                    _elem = new Element(1, i, line.Clone(), -1);
                    log.Add(new LogMessage("Удалил линию", elem, _elem));
                    Lines.Remove(i);
                }
            }
            for (i = 0; i < Rectangles.Rectangles.Count; i++)
            {
                var rect = Rectangles.Rectangles[i];
                if (rect.DL.Level == id & !rect.delete)
                {
                    elem = new Element(2, i, new MyRectangle(), -1);
                    _elem = new Element(2, i, rect.Clone(), -1);
                    log.Add(new LogMessage("Удалил прямоугольник", elem, _elem));
                    Rectangles.Remove(i);
                }
            }
            for (i = 0; i < Polygons.Polygons.Count; i++)
            {
                var pol = Polygons.Polygons[i];
                if (pol.DL.Level == id & !pol.delete)
                {
                    elem = new Element(3, i, new Polygon(), -1);
                    _elem = new Element(3, i, pol.Clone(), -1);
                    log.Add(new LogMessage("Удалил многоугольник", elem, _elem));
                    Polygons.Remove(i);
                }
            }
            for (i = 0; i < Circles.Circles.Count; i++)
            {
                var circ = Circles.Circles[i];
                if (circ.DL.Level == id & !circ.delete)
                {
                    elem = new Element(360, i, new Circle(), -1);
                    _elem = new Element(360, i, circ.Clone(), -1);
                    log.Add(new LogMessage("Удалил круг", elem, _elem));
                    Circles.Remove(i);
                }
            }
            for (i = 0; i < MyTexts.MyTexts.Count; i++)
            {
                var text = MyTexts.MyTexts[i];
                if (text.DL.Level == id & !text.delete)
                {
                    elem = new Element(10, i, new MyText(), -1);
                    _elem = new Element(10, i, text.Clone(), -1);
                    log.Add(new LogMessage("Удалил надпись", elem, _elem));
                    MyTexts.Remove(i);
                }
            }
            for (i = 0; i < NetworkWires.NetworkWires.Count; i++)
            {
                var nw = NetworkWires.NetworkWires[i];
                if (nw.DL.Level == id & !nw.delete)
                {
                    elem = new Element(9, i, new NetworkWire(), -1);
                    _elem = new Element(9, i, nw.Clone(), -1);
                    log.Add(new LogMessage("Удалил провод", elem, _elem));
                    NetworkWires.Remove(i);
                    var ne1 = nw.idiw1;
                    var ne2 = nw.idiw2;
                    if (!ne1.IW)
                    {
                        NetworkElements.NetworkElements[ne1.ID].Options.BusyPorts--;
                    }
                    if (!ne2.IW)
                    {
                        NetworkElements.NetworkElements[ne2.ID].Options.BusyPorts--;
                    }
                }
            }
            for (i = 0; i < NetworkElements.NetworkElements.Count; i++)
            {
                var ne = NetworkElements.NetworkElements[i];
                if (ne.DL.Level == id & !ne.delete)
                {
                    elem = new Element(8, i, new NetworkElement(), -1);
                    _elem = new Element(8, i, ne.Clone(), -1);
                    log.Add(new LogMessage("Удалил сетевой элемент", elem, _elem));
                    NetworkElements.Remove(i);
                }
            }
        }
        /// <summary>
        /// Проверка конкретного этажа в здании на наличие элементов
        /// </summary>
        /// <param name="id">Идентификатор здания</param>
        /// <param name="floor">Номер этажа</param>
        /// <returns></returns>
        internal bool CheckEmptyBuild(int id, int floor)
        {
            foreach (var iw in Buildings.Buildings[id].InputWires.InputWires.Circles)
            {
                if ((iw.LocalDL.Floor == floor | iw.MainDL.Floor == floor) & !iw.delete)
                    return false;
            }
            foreach (var ent in Buildings.Buildings[id].Entrances.Enterances.Circles)
            {
                if (ent.LocalDL.Floor == floor & !ent.delete)
                    return false;
            }
            foreach (var line in Lines.Lines)
            {
                if (line.DL.Level == id & !line.delete & line.DL.Floor == floor)
                    return false;
            }
            foreach (var rect in Rectangles.Rectangles)
            {
                if (rect.DL.Level == id & !rect.delete & rect.DL.Floor == floor)
                    return false;
            }
            foreach (var pol in Polygons.Polygons)
            {
                if (pol.DL.Level == id & !pol.delete & pol.DL.Floor == floor)
                    return false;
            }
            foreach (var circ in Circles.Circles)
            {
                if (circ.DL.Level == id & !circ.delete & circ.DL.Floor == floor)
                    return false;
            }
            foreach (var text in MyTexts.MyTexts)
            {
                if (text.DL.Level == id & !text.delete & text.DL.Floor == floor)
                    return false;
            }
            foreach (var ne in NetworkElements.NetworkElements)
            {
                if (ne.DL.Level == id & !ne.delete & ne.DL.Floor == floor)
                    return false;
            }
            foreach (var nw in NetworkWires.NetworkWires)
            {
                if (nw.DL.Level == id & !nw.delete & nw.DL.Floor == floor)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Функция для загрузки карты
        /// </summary>
        /// <param name="TempMap">Любая карта сети</param>
        public void MapLoad(Map TempMap)
        {
            Instrument = 0;
            sizeRenderingArea = TempMap.sizeRenderingArea;
            //MainForm.AnT.Refresh();
            MainForm.AnT.Height = sizeRenderingArea.Height;
            MainForm.AnT.Width = sizeRenderingArea.Width;
            Instrument = TempMap.Instrument;
            Rectangles = TempMap.Rectangles;
            Lines = TempMap.Lines;
            EditRects = TempMap.EditRects;
            Buildings = TempMap.Buildings;
            Circles = TempMap.Circles;
            Polygons = TempMap.Polygons;
            NetworkElements = TempMap.NetworkElements;
            NetworkWires = TempMap.NetworkWires;
            MyTexts = TempMap.MyTexts;
            log = TempMap.log;
            SetInstrument(Instrument);
            ResizeRenderingArea();
        }
        /// <summary>
        /// Загрузка карты сети с заданными настройками
        /// </summary>
        /// <param name="mapSettings"></param>
        internal void MapLoad(SizeRenderingArea mapSettings)
        {
            Instrument = 0;
            sizeRenderingArea = mapSettings;
            MainForm.AnT.Height = sizeRenderingArea.Height;
            MainForm.AnT.Width = sizeRenderingArea.Width;
            Instrument = 0;
            Rectangles = new GroupOfRectangles();
            Lines = new GroupOfLines();
            EditRects = new GroupOfEditRects();
            Buildings = new GroupOfBuildings();
            Circles = new GroupOfCircle();
            Polygons = new GroupOfPolygons();
            NetworkElements = new GroupOfNE();
            NetworkWires = new GroupOfNW();
            MyTexts = new GroupOfMT();
            log = new Log();
            SetInstrument(Instrument);
            ResizeRenderingArea();
        }

        /// <summary>
        /// Функция для Создания новой карты с новыми параметрами
        /// </summary>
        /// <param name="_mapSettings">Параметры карты</param>
        public void SetNewSettings(SizeRenderingArea _mapSettings)
        {
            Map TempMap = new Map
            {
                sizeRenderingArea = _mapSettings
            };
            MainForm.AnT.Height = _mapSettings.Height;
            MainForm.AnT.Width = _mapSettings.Width;
            //Gl.glClearColor(255, 255, 255, 1);
            Gl.glViewport(0, 0, MainForm.AnT.Width, MainForm.AnT.Height);
            // активация проекционной матрицы 
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            // очистка матрицы 
            Gl.glLoadIdentity();
            // установка перспективы 
            Glu.gluOrtho2D(-_mapSettings.Width / 2, _mapSettings.Width / 2, -_mapSettings.Height / 2, _mapSettings.Height / 2);
            // установка объектно-видовой матрицы 
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            MapLoad(TempMap);
        }

        /// <summary>
        /// Функция для обновления всех прямоугольников редактирования
        /// </summary>
        public void RefreshEditRect()
        {
            EditRects = new GroupOfEditRects();
            EditRects.EditRects.AddRange(_RefreshEditRect());
        }
        /// <summary>
        /// Генерация прямоугольников редактирования
        /// </summary>
        /// <returns>Возвращает список прямоугольников редактирования</returns>
        public List<EditRect> _RefreshEditRect()
        {
            List<EditRect> _EditRects = new List<EditRect>();
            List<EditRect> EditRects = new List<EditRect>();
            if (MainForm.filtres.Line)
                _EditRects.AddRange(Lines.GenEditRects());
            if (MainForm.filtres.Rect)
                _EditRects.AddRange(Rectangles.GenEditRects());
            if (MainForm.filtres.Poly)
                _EditRects.AddRange(Polygons.GenEditRects());
            if (MainForm.filtres.Circ)
                _EditRects.AddRange(Circles.GenEditRects());
            if (MainForm.filtres.NW)
                _EditRects.AddRange(NetworkWires.GenEditRects());
            if (MainForm.filtres.NE)
                _EditRects.AddRange(NetworkElements.GenEditRects());
            if (MainForm.filtres.Build)
                _EditRects.AddRange(Buildings.GenEditRects());
            return _EditRects;
        }

        /// <summary>
        /// Функция для поиска ближайшего элемента к клику мыши
        /// </summary>
        /// <param name="x">Координата X мыши</param>
        /// <param name="y">Координата Y мыши</param>
        /// <param name="type">Возвращаемый параметр - тип элемента</param>
        /// <param name="buildindex">Возвращаемый параметр - ID здания</param>
        /// <param name="dl">Уровень отображения</param>
        /// <returns>Возвращает ID ближайшего элемента</returns>
        public int SearchElem(int x, int y, out int type, out int buildindex, DrawLevel dl)
        {
            buildindex = -1;
            if (MainForm.filtres.Text)
            {
                int text = MyTexts.Search(x, y, dl);
                if (text != -1)
                {
                    type = 10;
                    return text;
                }
            }
            if (MainForm.filtres.Line)
            {
                int line = Lines.Search(x, y, dl);
                if (line != -1)
                {
                    type = 1;
                    return line;
                }
            }
            if (MainForm.filtres.NE)
            {
                int NE = NetworkElements.Search(x, y, dl);
                if (NE != -1)
                {
                    type = 8;
                    return NE;
                }
            }
            int build = -1;
            double distbuild = -1;
            if (MainForm.drawLevel.Level != -1)
            {
                if (MainForm.filtres.Ent)
                {
                    int entrance = Buildings.Buildings[MainForm.drawLevel.Level].Entrances.CalcNearestEnterise(x, y, dl);
                    if (entrance != -1)
                    {
                        buildindex = MainForm.drawLevel.Level;
                        type = 7;
                        return entrance;
                    }
                }
                if (MainForm.filtres.IW)
                {
                    int IW = Buildings.Buildings[MainForm.drawLevel.Level].InputWires.CalcNearestIW(x, y, dl);
                    if (IW != -1)
                    {
                        buildindex = MainForm.drawLevel.Level;
                        type = 6;
                        return IW;
                    }
                }
            }
            else
            {
                for (int i = 0; i < Buildings.Buildings.Count; i++)
                {
                    int entrance = Buildings.Buildings[i].Entrances.CalcNearestEnterise(x, y, dl);
                    if (entrance != -1)
                    {
                        buildindex = i;
                        type = 7;
                        return entrance;
                    }
                    int IW = Buildings.Buildings[i].InputWires.CalcNearestIW(x, y, dl);
                    if (IW != -1)
                    {
                        buildindex = i;
                        type = 6;
                        return IW;
                    }
                }
                if (MainForm.filtres.NW)
                {
                    int NW = NetworkWires.Search(x, y, dl);
                    if (NW != -1)
                    {
                        type = 9;
                        return NW;
                    }
                }
                if (MainForm.filtres.Build)
                    build = Buildings.Search(x, y, out distbuild, dl);
            }
            if (MainForm.filtres.NW)
            {
                int NW = NetworkWires.Search(x, y, dl);
                if (NW != -1)
                {
                    type = 9;
                    return NW;
                }
            }
            double distrect = -1;
            int rect = -1;
            if (MainForm.filtres.Rect)
            {
                rect = Rectangles.Search(x, y, out distrect, dl);
            }
            double distbline = -1;
            int bline = -1;
            if (MainForm.filtres.Poly)
            {
                bline = Polygons.Search(x, y, out distbline, dl);
            }

            double distcircle = -1;
            int circle = -1;
            if (MainForm.filtres.Circ)
            {
                circle = Circles.Search(x, y, out distcircle, dl);
            }
            if (distrect == -1)
                distrect = Int32.MaxValue;
            if (distbline == -1)
                distbline = Int32.MaxValue;
            if (distbuild == -1)
                distbuild = Int32.MaxValue;
            if (distcircle == -1)
                distcircle = Int32.MaxValue;
            if (distbuild < distrect & distbuild < distbline & distbuild < distcircle)
            {
                type = 4;
                return build;
            }
            else if (distbline < distrect & distbline < distbuild & distbline < distcircle)
            {
                type = 3;
                return bline;
            }
            else if (distrect < distbline & distrect < distbuild & distrect < distcircle)
            {
                type = 2;
                return rect;
            }
            else if (distcircle < distbline & distcircle < distbuild & distcircle < distrect)
            {
                type = 360;
                return circle;
            }
            type = -1;
            return -1;
        }
        /// <summary>
        /// Изменение параметров здания, по которому был произведен клик мыши
        /// </summary>
        /// <param name="x">Координата Х</param>
        /// <param name="y">Координата У</param>
        internal void SearchBuild(int x, int y)
        {
            int build = Buildings.Search(x, y, out double distbuild, new DrawLevel(-1, -1));
            if (build != -1)
            {
                List<bool> floorsempty = new List<bool>();
                for (int j = 0; j < Buildings.Buildings[build].Floors; j++)
                    floorsempty.Add(CheckEmptyBuild(build, j));
                BuildSettingsForm buildsettings = new BuildSettingsForm(Buildings.Buildings[build], floorsempty);
                buildsettings.ShowDialog();
                if (buildsettings.dialogResult == DialogResult.Yes)
                {
                    int newfloors = buildsettings.floors.Count - 2 - Buildings.Buildings[build].floors_count;
                    List<int> Deleted = new List<int>();
                    List<int> Added = new List<int>();
                    bool basement = Buildings.Buildings[build].basement;
                    bool loft = Buildings.Buildings[build].loft;
                    List<bool> BuildFloors = new List<bool>();
                    if (basement)
                        BuildFloors.Add(true);
                    else
                        BuildFloors.Add(false);
                    for (int i = 0; i < Buildings.Buildings[build].floors_count; i++)
                        BuildFloors.Add(true);
                    for (int i = 0; i < newfloors; i++)
                        BuildFloors.Add(false);
                    if (loft)
                        BuildFloors.Add(true);
                    else
                        BuildFloors.Add(false);
                    for (int i = 0; i < BuildFloors.Count; i++)
                    {
                        if (buildsettings.floors[i] & !BuildFloors[i])
                            Added.Add(i);
                        else if (!buildsettings.floors[i] & BuildFloors[i])
                            Deleted.Add(i);
                    }
                    Deleted.Reverse();
                    List<int> _Added = new List<int>();
                    foreach (var add in Added)
                        _Added.Add(add);
                    List<int> _Deleted = new List<int>();
                    foreach (var del in Deleted)
                        _Deleted.Add(del);
                    Element elem = new Element(15, build, new BUILDLIST((Building)Buildings.Buildings[build].Clone(), _Added, _Deleted), -2);
                    MoveElementsInBuild(build, Added, Deleted, buildsettings.floors[0]);
                    int floorscount = 0;
                    for (int i = 1; i < buildsettings.floors.Count - 1; i++)
                        if (buildsettings.floors[i])
                            floorscount++;
                    Buildings.Buildings[build].basement = buildsettings.floors[0];
                    Buildings.Buildings[build].loft = buildsettings.floors.Last();
                    Buildings.Buildings[build].floors_count = floorscount;
                    Buildings.Buildings[build].RefreshFloors();
                    Buildings.Buildings[build].Name = buildsettings.BuildName;
                    Buildings.Buildings[build].GenText();
                    Element _elem = new Element(15, build, new BUILDLIST((Building)Buildings.Buildings[build].Clone(), _Deleted, _Added), -2);
                    log.Add(new LogMessage("Изменил параметры здания", elem, _elem));
                }
            }
        }
        /// <summary>
        /// Перемещение элементов между этажами при удалении или добавлении этажей в здании
        /// </summary>
        /// <param name="build">Идентификатор здания</param>
        /// <param name="Added">Список добавленных этажей</param>
        /// <param name="Deleted">Список удаленных этажей</param>
        /// <param name="basement">Наличие подвала в здании</param>
        public void MoveElementsInBuild(int build, List<int> Added, List<int> Deleted, bool basement)
        {
            bool b = false;
            bool _b = false;
            if (Added.Contains(0))
            {
                UpdateFloorsIndex(build, -1, +1);
                b = true;
                _b = true;
            }
            else if (Deleted.Contains(0))
            {
                UpdateFloorsIndex(build, 0, -1);
                for (int i = 0; i < Added.Count; i++)
                    Added[i]--;
                for (int i = 0; i < Deleted.Count; i++)
                    Deleted[i]--;
                b = true;
            }
            else if (!basement)
            {
                for (int i = 0; i < Added.Count; i++)
                    Added[i]--;
                for (int i = 0; i < Deleted.Count; i++)
                    Deleted[i]--;
            }
            foreach (var del in Deleted)
            {
                if (b)
                {
                    if (del != -1)
                    {
                        UpdateFloorsIndex(build, del, -1);
                        for (int i = 0; i < Added.Count; i++)
                            if (Added[i] >= del)
                                Added[i]--;
                    }
                }
                else
                {
                    UpdateFloorsIndex(build, del, -1);
                    for (int i = 0; i < Added.Count; i++)
                        if (Added[i] >= del)
                            Added[i]--;
                }
            }
            foreach (var add in Added)
            {
                if (b)
                {
                    if (_b)
                    {
                        if (add != 0)
                            UpdateFloorsIndex(build, add - 1, +1);
                    }
                    else
                    {
                        UpdateFloorsIndex(build, add - 1, +1);
                    }
                }
                else
                {
                    UpdateFloorsIndex(build, add - 1, +1);
                }
            }
        }
        /// <summary>
        /// Обновление уровня отображения для элементов внутри здания
        /// </summary>
        /// <param name="id">Идентификатор здания</param>
        /// <param name="floor">Этаж</param>
        /// <param name="dif">В какую сторону обновлять</param>
        internal void UpdateFloorsIndex(int id, int floor, int dif)
        {
            foreach (var iw in Buildings.Buildings[id].InputWires.InputWires.Circles)
            {
                if (iw.LocalDL.Floor > floor)
                    iw.LocalDL.Floor += dif;
                if (iw.MainDL.Floor > floor)
                    iw.MainDL.Floor += dif;
            }
            foreach (var ent in Buildings.Buildings[id].Entrances.Enterances.Circles)
            {
                if (ent.LocalDL.Floor > floor)
                    ent.LocalDL.Floor += dif;
            }
            foreach (var line in Lines.Lines)
            {
                if (line.DL.Level == id & line.DL.Floor > floor)
                    line.DL.Floor += dif;
            }
            foreach (var rect in Rectangles.Rectangles)
            {
                if (rect.DL.Level == id & rect.DL.Floor > floor)
                    rect.DL.Floor += dif;
            }
            foreach (var pol in Polygons.Polygons)
            {
                if (pol.DL.Level == id & pol.DL.Floor > floor)
                    pol.DL.Floor += dif;
            }
            foreach (var circ in Circles.Circles)
            {
                if (circ.DL.Level == id & circ.DL.Floor > floor)
                    circ.DL.Floor += dif;
            }
            foreach (var text in MyTexts.MyTexts)
            {
                if (text.DL.Level == id & text.DL.Floor > floor)
                    text.DL.Floor += dif;
            }
            foreach (var ne in NetworkElements.NetworkElements)
            {
                if (ne.DL.Level == id & ne.DL.Floor > floor)
                    ne.DL.Floor += dif;
            }
            foreach (var nw in NetworkWires.NetworkWires)
            {
                if (nw.DL.Level == id & nw.DL.Floor > floor)
                    nw.DL.Floor += dif;
            }
        }

        /// <summary>
        /// Возврат элементов к исходному состоянию, без фокусировки на определенном элементе
        /// </summary>
        public void Unfocus(bool er)
        {
            DefaultTempElems(er);
            Lines.Choose(-1);
            Rectangles.Choose(-1);
            Polygons.Choose(-1);
            Buildings.Choose(-1);
            Circles.Choose(-1);
            NetworkElements.Choose(-1);
            NetworkWires.Choose(-1);
            MyTexts.Choose(-1);
        }
        /// <summary>
        /// Поиск элемента для редактирования
        /// </summary>
        /// <param name="x">Координата мыши Х</param>
        /// <param name="y">Координата мыши У</param>
        /// <param name="type">Возвращаемый параметр: тип</param>
        /// <returns></returns>
        public bool SearchEditElem(int x, int y, out int type)
        {
            EditRects.editRect = -1;
            EditRects.editRect = EditRects.Search(x, y);
            if (EditRects.editRect == -1)
            {
                EditRects.edit_active = false;
                type = -1;
                return false;
            }
            else
            {
                Unfocus(false);
                type = EditRects.EditRects[EditRects.editRect].elems[0].type;
                int id = EditRects.EditRects[EditRects.editRect].elems[0].id;
                MainForm.activeElem.type = type;
                MainForm.activeElem.item = id;
                switch (type)
                {
                    case 1:
                        Lines.Choose(id);
                        break;
                    case 2:
                        Rectangles.Choose(id);
                        break;
                    case 3:
                        Polygons.Choose(id);
                        break;
                    case 4:
                        Buildings.Choose(id);
                        break;
                    case 360:
                        Circles.Choose(id);
                        break;
                    case 8:
                        NetworkElements.Choose(id);
                        break;
                    case 9:
                        NetworkWires.Choose(id);
                        break;
                }
                EditRects.edit_active = true;
                return true;
            }
        }
        /// <summary>
        /// Перемещение элемента с помощью прямоугольника редактирования
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        public void MoveElements(int x, int y)
        {
            x = (int)((double)x / MainForm.zoom);
            y = (int)((double)y / MainForm.zoom);
            int type = 0, id = 0, point = 0;
            if (EditRects.edit_active)
            {
                EditRects.EditRects[EditRects.editRect].Refresh(new Point(x, y));
                for (int i = 0; i < EditRects.EditRects[EditRects.editRect].elems.Count; i++)
                {
                    type = EditRects.EditRects[EditRects.editRect].elems[i].type;
                    id = EditRects.EditRects[EditRects.editRect].elems[i].id;
                    point = EditRects.EditRects[EditRects.editRect].elems[i].point;
                    switch (type)
                    {
                        case 1:
                            Lines.Lines[id].SetPoint(x, y, point);
                            break;
                        case 2:
                            Rectangles.Rectangles[id].SetPoint(x, y, point);
                            break;
                        case 3:
                            Polygons.Polygons[id].SetPoint(x, y, point);
                            break;
                        case 4:
                            Buildings.Buildings[id].SetPoint(x, y, point, NetworkWires, id);
                            break;
                        case 360:
                            Circles.Circles[id].SetPoint(x, y, point);
                            break;
                        case 8:
                            NetworkElements.NetworkElements[id].SetPoint(x, y, id, NetworkWires);
                            break;
                        case 9:
                            NetworkWires.NetworkWires[id].SetPoint(x, y, point);
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// Пересчет координаты Y из оконных координат в координаты OpenGL
        /// </summary>
        /// <param name="y">Координата мыши Y</param>
        /// <returns></returns>
        public int RecalcMouseY(int y)
        {
            if (MainForm.drawLevel.Level == -1)
                return (int)((double)sizeRenderingArea.Height / 2d * MainForm.zoom) - y;
            else
                return (int)((double)Buildings.Buildings[MainForm.drawLevel.Level].sizeRenderingArea.Height / 2d * MainForm.zoom) - y;
        }
        /// <summary>
        /// Пересчет координаты X из оконных координат в координаты OpenGL
        /// </summary>
        /// <param name="x">Координата мыши X</param>
        /// <returns></returns>
        public int RecalcMouseX(int x)
        {
            if (MainForm.drawLevel.Level == -1)
                return x - (int)((double)sizeRenderingArea.Width / 2d * MainForm.zoom);
            else
                return x - (int)((double)Buildings.Buildings[MainForm.drawLevel.Level].sizeRenderingArea.Width / 2d * MainForm.zoom);
        }
        /// <summary>
        /// Проверка, в какой элемент попадают координаты мыши, вход провода или сетвой элемент
        /// </summary>
        /// <param name="x">Координата Х</param>
        /// <param name="y">Кордината У</param>
        /// <returns>Возвращает элемент, куда попал клик мыши</returns>
        public IDandIW ChechNE(int x, int y)
        {
            if (MainForm.drawLevel.Level == -1)
            {
                int NE = NetworkElements.Search(x, y, MainForm.drawLevel);
                if (NE != -1 && NetworkElements.NetworkElements[NE].Options.CheckPorts())
                {
                    NetworkElements.NetworkElements[NE].Options.BusyPorts++;
                    return new IDandIW(NE, false, -1);
                }
                int build = Buildings.Search(x, y, out double distbuild, MainForm.drawLevel);
                if (build != -1)
                {
                    int IW = Buildings.Buildings[build].InputWires.CalcNearestIW(x, y, MainForm.drawLevel);
                    if (IW != -1)
                    {
                        return new IDandIW(IW, true, build);
                    }
                }
            }
            else
            {
                int NE = NetworkElements.Search(x, y, MainForm.drawLevel);
                if (NE != -1 && NetworkElements.NetworkElements[NE].Options.CheckPorts())
                {
                    NetworkElements.NetworkElements[NE].Options.BusyPorts++;
                    return new IDandIW(NE, false, -1);
                }
                int build = MainForm.drawLevel.Level;
                int IW = Buildings.Buildings[build].InputWires.CalcNearestIW(x, y, MainForm.drawLevel);
                if (IW != -1)
                {
                    return new IDandIW(IW, true, build);
                }
            }
            return new IDandIW(-1, false, -1);
        }
        /// <summary>
        /// Поиск текста для редактирования
        /// </summary>
        /// <param name="x">Координата Х</param>
        /// <param name="y">Координата У</param>
        /// <param name="id">Возвращаемый параметр: идентификатор</param>
        /// <returns></returns>
        internal bool SearchText(int x, int y, out int id)
        {
            int MT = MyTexts.Search(x, y, MainForm.drawLevel);
            if (MT != -1)
            {
                id = MT;
                return true;
            }
            id = -1;
            return false;
        }
        /// <summary>
        /// Поиск сетевого элемента для редактирования параметров
        /// </summary>
        /// <param name="x">Координата Х</param>
        /// <param name="y">Координата У</param>
        /// <returns></returns>
        internal bool SearchNE(int x, int y)
        {
            int NE = NetworkElements.Search(x, y, MainForm.drawLevel);
            if (NE != -1)
            {
                var ns1 = NetworkElements.NetworkElements[NE].Options.ToString();
                Element elem = new Element(13, NE, NetworkElements.NetworkElements[NE].Clone(), -4);
                NESettings nes = new NESettings(NetworkElements.NetworkElements[NE].Options, NetworkElements, ref NetworkElements.NetworkElements[NE].notes);
                nes.ShowDialog();
                NetworkElements = nes.NetworkElements;
                NetworkElements.NetworkElements[NE].Options = nes.Options;
                NetworkElements.NetworkElements[NE].GenText();
                Element _elem = new Element(13, NE, NetworkElements.NetworkElements[NE].Clone(), -4);
                var ns2 = NetworkElements.NetworkElements[NE].Options.ToString();
                if (ns1 != ns2)
                    log.Add(new LogMessage("Изменил параметры устройства", elem, _elem));
                return true;
            }
            return false;
        }
        /// <summary>
        /// Поиск провода для изменения параметров
        /// </summary>
        /// <param name="x">Координата Х</param>
        /// <param name="y">Координата У</param>
        /// <returns></returns>
        internal bool SearchNW(int x, int y)
        {
            int NW = NetworkWires.Search(x, y, MainForm.drawLevel);
            if (NW != -1)
            {
                NWSettings nws = new NWSettings(NetworkWires.NetworkWires[NW].Throughput, ref NetworkWires.NetworkWires[NW].notes);
                nws.ShowDialog();
                NetworkWires.NetworkWires[NW].notes = nws.notes.Copy();
                NetworkWires.NetworkWires[NW].Throughput = nws.Throughput;
                return true;
            }
            return false;
        }
    }
}
