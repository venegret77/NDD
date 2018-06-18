using NetworkDesign.Main;
using NetworkDesign.NetworkElements;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tao.OpenGl;

namespace NetworkDesign
{
    /// <summary>
    /// Здание
    /// </summary>
    public class Building : ICloneable
    {
        /// <summary>
        /// Имя
        /// </summary>
        public string Name;
        /// <summary>
        /// Количество этажей с учетов чердака и подвала
        /// </summary>
        public int Floors;
        /// <summary>
        /// Наличие чердака
        /// </summary>
        public bool loft = false;
        /// <summary>
        /// Налачие подвала
        /// </summary>
        public bool basement = false;
        /// <summary>
        /// Количество этажей без учета чердака и подвала
        /// </summary>
        public int floors_count = 0;
        /// <summary>
        /// Тип:
        /// 2 - прямоугольник;
        /// 3 - многоугольник;
        /// 360 - круг;
        /// </summary>
        public int type = 2;
        /// <summary>
        /// Открыто здание или нет
        /// </summary>
        public bool open = false;
        /// <summary>
        /// Удалено или нет
        /// </summary>
        public bool delete = true;
        /// <summary>
        /// Коэффициент отношения сторон (ширина деленная на высоту)
        /// </summary>
        public double pk = 1d;
        /// <summary>
        /// Коэффициент при расчете локальных значений
        /// </summary>
        public double koef = 1;
        /// <summary>
        /// Угол поворота при расчете локальных значений
        /// </summary>
        public double alfa = 0;
        /// <summary>
        /// Точка поворота
        /// </summary>
        public Point MP = new Point();
        /// <summary>
        /// На сколько смещено относительно центра
        /// </summary>
        public int Ox, Oy;
        /// <summary>
        /// Параметры области отрисовки
        /// </summary>
        public SizeRenderingArea sizeRenderingArea = new SizeRenderingArea();
        //
        /// <summary>
        /// Временный многоугольник
        /// </summary>
        public Polygon TempPolygon = new Polygon();
        /// <summary>
        /// Многоугольник отображаемый на главном виде
        /// </summary>
        public Polygon MainPolygon = new Polygon();
        /// <summary>
        /// Многоугольник без поворота
        /// </summary>
        public Polygon _MainPolygon = new Polygon();
        /// <summary>
        /// Многоугольник для внутреннего вида
        /// </summary>
        public Polygon LocalPolygon = new Polygon();
        //
        /// <summary>
        /// Временный прямоугольник
        /// </summary>
        public MyRectangle TempRectangle = new MyRectangle();
        /// <summary>
        /// Прямоугольник отображаемый на главном виде
        /// </summary>
        public MyRectangle MainRectangle = new MyRectangle();
        /// <summary>
        /// Прямоугольник без поворота
        /// </summary>
        public MyRectangle _MainRectangle = new MyRectangle();
        /// <summary>
        /// Прямоугольник для внутреннего вида
        /// </summary>
        public MyRectangle LocalRectangle = new MyRectangle();
        //
        /// <summary>
        /// Круг, отображаемый на главном виде
        /// </summary>
        public Circle MainCircle = new Circle();
        /// <summary>
        /// Временный круг
        /// </summary>
        public Circle _MainCircle = new Circle();
        /// <summary>
        /// Круг для внуреннего вида
        /// </summary>
        public Circle LocalCircle = new Circle();
        //
        /// <summary>
        /// Основной уровень отображения
        /// </summary>
        public DrawLevel MainMapDL = new DrawLevel();
        /// <summary>
        /// Локальный (внутренний) уровень отображения
        /// </summary>
        public DrawLevel LocalDL = new DrawLevel();
        /// <summary>
        /// Список наименований этажей
        /// </summary>
        public List<string> floors_name = new List<string>();
        /// <summary>
        /// Временный вход
        /// </summary>
        public Entrances TempEntrances = new Entrances();
        /// <summary>
        /// Входы 
        /// </summary>
        public Entrances Entrances = new Entrances();
        /// <summary>
        /// Временный вход провода
        /// </summary>
        public InputWire TempInputWires = new InputWire();
        /// <summary>
        /// Входы проводов в здание
        /// </summary>
        public InputWire InputWires = new InputWire();
        /// <summary>
        /// Текст
        /// </summary>
        public MyText MT = new MyText();
        /// <summary>
        /// Показывает, перемещается ли вход
        /// </summary>
        bool isMoveEnt = false;
        /// <summary>
        /// Идентификатор
        /// </summary>
        int id = -1;
        /// <summary>
        /// Показывает, перемещается ли вход провода
        /// </summary>
        public bool isMoveIW;
        /// <summary>
        /// Конструктор
        /// </summary>
        public Building()
        {
            delete = true;
        }
        /// <summary>
        /// Отрисовка временных элементов
        /// </summary>
        internal void DrawTemp()
        {
            Entrances.DrawTemp();
            InputWires.DrawTemp();
        }
        /// <summary>
        /// Конструктор для многоугольника
        /// </summary>
        /// <param name="_Name">Имя</param>
        /// <param name="_loft">Чердак</param>
        /// <param name="_basement">Подвал</param>
        /// <param name="floors_count">Количество этажей</param>
        /// <param name="_pol">Многоугольник</param>
        /// <param name="index">Идентификатор</param>
        /// <param name="width">Ширина</param>
        public Building(string _Name, bool _loft, bool _basement, int floors_count, Polygon _pol, int index, int width)
        {
            sizeRenderingArea = new SizeRenderingArea(Name, (int)((double)width / pk), width);
            Name = _Name;
            loft = _loft;
            basement = _basement;
            this.floors_count = floors_count;
            RefreshFloors();
            MainPolygon = _pol;
            type = 3;
            delete = false;
            MainMapDL.Level = -1;
            MainMapDL.Floor = -1;
            LocalDL.Level = index;
            LocalDL.Floor = 0;
            GenLocalPolygon();
            GenText();
        }
        /// <summary>
        /// Конструктор для прямоугольников
        /// </summary>
        /// <param name="_Name">Имя</param>
        /// <param name="_loft">Чердак</param>
        /// <param name="_basement">Подвал</param>
        /// <param name="floors_count">Количество этажей</param>
        /// <param name="_rect">Прямоугольник</param>
        /// <param name="index">Идентификатор</param>
        /// <param name="width">Ширина</param>
        public Building(string _Name, bool _loft, bool _basement, int floors_count, MyRectangle _rect, int index, int width)
        {
            sizeRenderingArea = new SizeRenderingArea(Name, (int)((double)width / pk), width);
            Name = _Name;
            loft = _loft;
            basement = _basement;
            this.floors_count = floors_count;
            RefreshFloors();
            MainRectangle = _rect;
            type = 2;
            delete = false;
            MainMapDL.Level = -1;
            MainMapDL.Floor = -1;
            LocalDL.Level = index;
            LocalDL.Floor = 0;
            MainRectangle.DL = MainMapDL;
            GenLocalRect();
            GenText();
        }
        /// <summary>
        /// Конструктор для кругов
        /// </summary>
        /// <param name="_Name">Имя</param>
        /// <param name="_loft">Чердак</param>
        /// <param name="_basement">Подвал</param>
        /// <param name="floors_count">Количество этажей</param>
        /// <param name="_circle">Круг</param>
        /// <param name="index">Идентификатор</param>
        /// <param name="width">Ширина</param>
        public Building(string _Name, bool _loft, bool _basement, int floors_count, Circle _circle, int index, int width)
        {
            sizeRenderingArea = new SizeRenderingArea(Name, (int)((double)width / pk), width);
            Name = _Name;
            loft = _loft;
            basement = _basement;
            this.floors_count = floors_count;
            RefreshFloors();
            MainCircle = _circle;
            type = 360;
            delete = false;
            MainMapDL.Level = -1;
            MainMapDL.Floor = -1;
            LocalDL.Level = index;
            LocalDL.Floor = 0;
            MainCircle.DL = MainMapDL;
            GenLocalCircle();
            GenText();
        }
        /// <summary>
        /// Обновление этажей
        /// </summary>
        internal void RefreshFloors()
        {
            if (basement & loft)
                Floors = floors_count + 2;
            else if (basement)
                Floors = floors_count + 1;
            else if (loft)
                Floors = floors_count + 1;
            else
                Floors = floors_count;
            UpgrateFloors();
        }
        /// <summary>
        /// Генерация текстуры для надписи
        /// </summary>
        public void GenText()
        {
            int height = 0;
            int width = 0;
            Point CP = new Point();
            switch (type)
            {
                case 2:
                    MainRectangle.CalcMaxMin(out int maxx, out int minx, out int maxy, out int miny);
                    height = maxy - miny;
                    width = maxx - minx;
                    CP = new Point((maxx + minx) / 2, (maxy + miny) / 2);
                    break;
                case 3:
                    MainPolygon.CalcMaxMin(out maxx, out minx, out maxy, out miny);
                    height = maxy - miny;
                    width = maxx - minx;
                    CP = new Point((maxx + minx) / 2, (maxy + miny) / 2);
                    break;
                case 360:
                    width = height = MainCircle.radius * 2;
                    CP = MainCircle.MainCenterPoint;
                    break;
            }
            Size size = new Size(width / 2, height / 2);
            MT = new MyText(MainMapDL, CP, size, Name);
        }
        /// <summary>
        /// Если внутри здания
        /// </summary>
        bool isInBuild = false;
        /// <summary>
        /// Добавить временный вход провода
        /// </summary>
        public void AddTempIW()
        {
            if (isMoveIW & id != -1 & !isInBuild)
            {
                isMoveIW = false;
                InputWires.InputWires.TempCircle.LocalCenterPoint = CalcLocalPoint(MainForm._GenZoomPoint(InputWires.InputWires.TempCircle.MainCenterPoint));
                InputWires.InputWires.Circles[id].MainCenterPoint = MainForm._GenZoomPoint(new Point(InputWires.InputWires.TempCircle.MainCenterPoint.X, InputWires.InputWires.TempCircle.MainCenterPoint.Y));
                InputWires.InputWires.Circles[id].LocalCenterPoint = /*MainForm._GenZoomPoint(*/new Point(InputWires.InputWires.TempCircle.LocalCenterPoint.X, InputWires.InputWires.TempCircle.LocalCenterPoint.Y/*)*/);
                InputWires.InputWires.Circles[id].delete = false;
                InputWires.InputWires.TempDefault();
            }
            else if (isInBuild & isMoveIW & id != -1)
            {
                isMoveIW = false;
                isInBuild = false;
                InputWires.InputWires.TempCircle.LocalCenterPoint = new Point(InputWires.InputWires.TempCircle.MainCenterPoint.X, InputWires.InputWires.TempCircle.MainCenterPoint.Y);
                InputWires.InputWires.Circles[id].MainCenterPoint = MainForm._GenZoomPoint(new Point(InputWires.InputWires.TempCircle.MainCenterPoint.X, InputWires.InputWires.TempCircle.MainCenterPoint.Y));
                InputWires.InputWires.Circles[id].LocalCenterPoint = MainForm._GenZoomPoint(new Point(InputWires.InputWires.TempCircle.LocalCenterPoint.X, InputWires.InputWires.TempCircle.LocalCenterPoint.Y));
                InputWires.InputWires.Circles[id].delete = false;
                InputWires.InputWires.TempDefault();
            }
        }
        /// <summary>
        /// Перемещение входа провода
        /// </summary>
        /// <param name="x">Координата Х</param>
        /// <param name="y">Координата У</param>
        /// <param name="id">Идентификатор элемента</param>
        /// <param name="build">Идентификатор здания</param>
        /// <param name="networkWires">Список проводов</param>
        internal void MoveIW(int x, int y, int id, int build, GroupOfNW networkWires)
        {
            var iw = InputWires.InputWires.Circles[id];
            if (iw.MainDL.Level == -1 & MainForm.drawLevel.Level == -1)
            {
                this.id = id;
                isMoveIW = true;
                InputWires.InputWires.TempCircle = new Circle
                {
                    MainCenterPoint = MainForm.GenZoomPoint(iw.MainCenterPoint),
                    delete = false,
                    koef = iw.koef,
                    side = iw.side
                };
                InputWires.InputWires.Circles[id].delete = true;
                MoveIW(x, y);
                InputWires.InputWires.TempCircle.LocalCenterPoint = CalcLocalPoint(MainForm._GenZoomPoint(InputWires.InputWires.TempCircle.MainCenterPoint));
                Point point = MainForm._GenZoomPoint(InputWires.InputWires.TempCircle.MainCenterPoint);
                Point _point = InputWires.InputWires.TempCircle.LocalCenterPoint;
                networkWires.CheckNW(point.X, point.Y, id, true, build, iw.MainDL);
                networkWires.CheckNW(_point.X, _point.Y, id, true, build, iw.LocalDL);
            }
            else if (iw.MainDL.Level != -1 & MainForm.drawLevel.Level != -1)
            {
                this.id = id;
                isMoveIW = true;
                isInBuild = true;
                InputWires.InputWires.TempCircle = new Circle
                {
                    MainCenterPoint = MainForm.GenZoomPoint(iw.MainCenterPoint),
                    LocalCenterPoint = MainForm.GenZoomPoint(iw.LocalCenterPoint),
                    delete = false,
                    koef = iw.koef,
                    side = iw.side
                };
                InputWires.InputWires.Circles[id].delete = true;
                MoveIWInBuild(x, y);
                Point point = MainForm._GenZoomPoint(InputWires.InputWires.TempCircle.MainCenterPoint);
                Point _point = MainForm._GenZoomPoint(InputWires.InputWires.TempCircle.LocalCenterPoint);
                networkWires.CheckNW(point.X, point.Y, id, true, MainForm.drawLevel.Level, InputWires.InputWires.TempCircle.MainDL);
                networkWires.CheckNW(_point.X, _point.Y, id, true, MainForm.drawLevel.Level, InputWires.InputWires.TempCircle.LocalDL);
            }
        }
        /// <summary>
        /// Обновление наименований этажей
        /// </summary>
        private void UpgrateFloors()
        {
            floors_name.Clear();
            if (basement & loft)
            {
                floors_name.Add("Подвал");
                for (int i = 1; i < Floors - 1; i++)
                {
                    floors_name.Add("Этаж " + i);
                }
                floors_name.Add("Чердак");
            }
            else if (basement)
            {
                floors_name.Add("Подвал");
                for (int i = 1; i < Floors; i++)
                {
                    floors_name.Add("Этаж " + i);
                }
            }
            else if (loft)
            {
                for (int i = 0; i < Floors - 1; i++)
                {
                    floors_name.Add("Этаж " + (i + 1));
                }
                floors_name.Add("Чердак");
            }
            else
            {
                for (int i = 0; i < Floors; i++)
                {
                    floors_name.Add("Этаж " + (i + 1));
                }
            }
        }
        /// <summary>
        /// Расчет центральной точки для перемещения
        /// </summary>
        public void CalcCenterPoint()
        {
            if (type == 2)
            {
                MainRectangle.CalcCenterPoint();
                _MainRectangle.CalcCenterPoint();
            }
            else if (type == 3)
            {
                MainPolygon.CalcCenterPoint();
                _MainPolygon.CalcCenterPoint();
            }
            else if (type == 360)
                MainCircle.CalcCenterPoint();
        }
        /// <summary>
        /// Перемещение
        /// </summary>
        /// <param name="x">Координата Х</param>
        /// <param name="y">Координата У</param>
        /// <param name="networkWires">Группа проводов</param>
        /// <param name="build">Идентификатор здания</param>
        public void MoveElem(int x, int y, GroupOfNW networkWires, int build)
        {
            int difx = 0;
            int dify = 0;
            if (type == 2)
            {
                difx = x - (int)MainRectangle.CenterPointX;
                dify = y - (int)MainRectangle.CenterPointY;
                MainRectangle.MoveElem(x, y);
                _MainRectangle._MoveElem(difx, dify);
            }
            else if (type == 3)
            {
                difx = x - (int)MainPolygon.CenterPointX;
                dify = y - (int)MainPolygon.CenterPointY;
                MainPolygon.MoveElem(x, y);
                _MainPolygon._MoveElem(difx, dify);
            }
            else if (type == 360)
            {
                difx = (int)((double)x / MainForm.zoom) - MainCircle.MainCenterPoint.X;
                dify = (int)((double)y / MainForm.zoom) - MainCircle.MainCenterPoint.Y;
                MainCircle.MoveElem(x, y);
            }
            Entrances.MoveElem(difx, dify);
            InputWires.MoveElem(difx, dify, networkWires, build);
            MT._MoveElem(difx, dify);
        }
        /// <summary>
        /// Расчет центральной точки
        /// </summary>
        /// <param name="x">Координата Х</param>
        /// <param name="y">Координата У</param>
        /// <returns></returns>
        public double CalcPointInBuild(int x, int y)
        {
            if (!delete)
            {
                if (type == 2)
                    return MainRectangle.Search(x, y);
                else if (type == 3)
                    return MainPolygon.Search(x, y);
                else if (type == 360)
                    return MainCircle.Search(x, y);
            }
            return -1;
        }
        /// <summary>
        /// Установить активным элементом
        /// </summary>
        /// <param name="b"></param>
        public void SetActive(bool b)
        {
            if (type == 2)
                MainRectangle.SetActive(b);
            else if (type == 3)
                MainPolygon.SetActive(b);
            else if (type == 360)
                MainCircle.SetActive(b);
        }

        /// <summary>
        /// Добавление входа в здание
        /// </summary>
        /// <param name="x">Координата мыши X</param>
        /// <param name="y">Координата мыши Y</param>
        /// <returns></returns>
        public bool AddEntrance(int x, int y)
        {
            if (!Entrances.step)
            {
                Entrances.step = true;
                if (basement)
                {
                    Entrances.AddTemp(x, y, MainMapDL, new DrawLevel(LocalDL.Level, 1));
                }
                else
                {
                    Entrances.AddTemp(x, y, MainMapDL, new DrawLevel(LocalDL.Level, 0));
                }
                return false;
            }
            else
            {
                Entrances.step = false;
                Entrances.Enterances.TempCircle.LocalCenterPoint = CalcLocalPoint(MainForm._GenZoomPoint(Entrances.Enterances.TempCircle.MainCenterPoint));
                Entrances.Enterances.TempCircle.koef = koef;
                Entrances.Add();
                TempEntrances = (Entrances)Entrances.Clone();
                return true;
            }
        }

        /// <summary>
        /// Расчет точки внутри здания
        /// </summary>
        /// <param name="MainP">Точка на главном виде</param>
        /// <returns>Возвращает нужную точку</returns>
        public Point CalcLocalPoint(Point MainP)
        {
            if (type == 360)
            {
                Point LocalP = new Point
                {
                    X = (int)(((double)MainP.X - (double)MainCircle.MainCenterPoint.X) * koef),
                    Y = (int)(((double)MainP.Y - (double)MainCircle.MainCenterPoint.Y) * koef),
                };
                return LocalP;
            }
            else
            {
                Point LocalP = new Point();
                if (type == 2)
                {
                    LocalP = RotatePoint(-alfa, MainRectangle.Points[0], MainP);
                    LocalP.X -= Ox;
                    LocalP.Y -= Oy;
                }
                else if (type == 3)
                {
                    LocalP = RotatePoint(-alfa, MainPolygon.Points[p1], MainP);
                    LocalP.X -= Ox;
                    LocalP.Y -= Oy;
                }
                LocalP.X = (int)((double)LocalP.X * koef);
                LocalP.Y = (int)((double)LocalP.Y * koef);
                return LocalP;
            }
        }

        /// <summary>
        /// Перемещение входа в здание по контуру фигуры в зависимости от типа фигуры
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MoveEntrance(int x, int y)
        {
            if (type == 2)
                Entrances.NearestPoints(x, y, MainForm.GenZoomRect(MainRectangle));
            else if (type == 3)
                Entrances.NearestPoints(x, y, MainForm.GenZoomPolygon(MainPolygon));
            else if (type == 360)
                Entrances.NearestPoints(x, y, MainForm.GenZoomCircle(MainCircle));
        }

        public void AddTempEnt()
        {
            if (isMoveEnt & id != -1)
            {
                isMoveEnt = false;
                Entrances.Enterances.TempCircle.LocalCenterPoint = CalcLocalPoint(MainForm._GenZoomPoint(Entrances.Enterances.TempCircle.MainCenterPoint));
                Entrances.Enterances.Circles[id].MainCenterPoint = MainForm._GenZoomPoint(Entrances.Enterances.TempCircle.MainCenterPoint);//new Point(Entrances.Enterances.TempCircle.MainCenterPoint.X, Entrances.Enterances.TempCircle.MainCenterPoint.Y);
                Entrances.Enterances.Circles[id].LocalCenterPoint = new Point(Entrances.Enterances.TempCircle.LocalCenterPoint.X, Entrances.Enterances.TempCircle.LocalCenterPoint.Y);
                Entrances.Enterances.Circles[id].delete = false;
                Entrances.Enterances.TempDefault();
            }
        }

        /// <summary>
        /// Перемещение входа в здание
        /// </summary>
        /// <param name="x">Координата мыши X</param>
        /// <param name="y">Координата мыши Y</param>
        /// <param name="id">Номер в списке перемещаемого входа</param>
        public void MoveEntrance(int x, int y, int id)
        {
            this.id = id;
            isMoveEnt = true;
            var ent = Entrances.Enterances.Circles[id];
            Entrances.Enterances.TempCircle = new Circle
            {
                MainCenterPoint = MainForm.GenZoomPoint(ent.MainCenterPoint),//new Point(ent.MainCenterPoint.X, ent.MainCenterPoint.Y),
                delete = false,
                koef = ent.koef
            };
            Entrances.Enterances.Circles[id].delete = true;
            MoveEntrance(x, y);
        }
        /// <summary>
        /// Добавление входа провода внутри здания
        /// </summary>
        /// <param name="x">Координата Х</param>
        /// <param name="y">Координата У</param>
        /// <param name="dl">Уровень отображения</param>
        /// <returns></returns>
        public bool AddIWInBuild(int x, int y, DrawLevel dl)
        {
            if (!InputWires.step)
            {
                InputWires.step = true;
                InputWires.AddTemp(x, y, dl, new DrawLevel(dl.Level, dl.Floor - 1));
                return false;
            }
            else
            {
                InputWires.step = false;
                InputWires.AddInBuild();
                return true;
            }
        }

        /// <summary>
        /// Добавление входа провода в здание
        /// </summary>
        /// <param name="x">Координата мыши X</param>
        /// <param name="y">Координата мыши Y</param>
        /// <param name="_side">true - входит сбоку в стену, false - входит сверху через потолок</param>
        /// <param name="_floor">Этаж</param>
        /// <returns></returns>
        public bool AddIW(int x, int y, bool _side, int _floor)
        {
            InputWires.side = _side;
            if (!InputWires.step)
            {
                InputWires.step = true;
                if (_side)
                {
                    InputWires.AddTemp(x, y, MainMapDL, new DrawLevel(LocalDL.Level, _floor));
                }
                else
                {
                    InputWires.AddTemp(x, y, MainMapDL, new DrawLevel(LocalDL.Level, floors_name.Count - 1));
                }
                return false;
            }
            else
            {
                InputWires.step = false;
                InputWires.InputWires.TempCircle.LocalCenterPoint = CalcLocalPoint(MainForm._GenZoomPoint(InputWires.InputWires.TempCircle.MainCenterPoint));
                InputWires.InputWires.TempCircle.koef = koef;
                InputWires.Add();
                TempInputWires = (InputWire)InputWires.Clone();
                return true;
            }
        }
        /// <summary>
        /// Перемещение входа провода
        /// </summary>
        /// <param name="x">Координата Х</param>
        /// <param name="y">Координата У</param>
        public void MoveIW(int x, int y)
        {
            if (InputWires.InputWires.TempCircle.side)
            {
                if (type == 2)
                    InputWires.NearestPoints(x, y, MainForm.GenZoomRect(MainRectangle));
                else if (type == 3)
                    InputWires.NearestPoints(x, y, MainForm.GenZoomPolygon(MainPolygon));
                else if (type == 360)
                    InputWires.NearestPoints(x, y, MainForm.GenZoomCircle(MainCircle));
            }
            else
            {
                if (type == 2)
                    InputWires.CheckIW(x, y, MainRectangle);
                else if (type == 3)
                    InputWires.CheckIW(x, y, MainPolygon);
                else if (type == 360)
                    InputWires.CheckIW(x, y, MainCircle);
            }
        }
        /// <summary>
        /// Перемещение входа провода внутри здания
        /// </summary>
        /// <param name="x">Координата Х</param>
        /// <param name="y">Координата У</param>
        public void MoveIWInBuild(int x, int y)
        {
            if (type == 2)
                InputWires.CheckIWInBuild(x, y, LocalRectangle);
            else if (type == 3)
                InputWires.CheckIWInBuild(x, y, LocalPolygon);
            else if (type == 360)
                InputWires.CheckIWInBuild(x, y, LocalCircle);
            //InputWires.SetTempPointInBuild(x, y);
        }

        /// <summary>
        /// Расчет угла наклона между двумя точками относительно оси X
        /// </summary>
        /// <param name="Point1">Точка 1</param>
        /// <param name="Point2">Точка 2</param>
        /// <returns>Возвращает угол наклона</returns>
        private double CalcAlfa(Point Point1, Point Point2)
        {
            double cat1 = Point2.Y - Point1.Y; //Противолежащий
            double cat2 = Point2.X - Point1.X; //Прилежащий
            double _alfa = Math.Atan2(cat1 , cat2);
            return _alfa;
        }

        /// <summary>
        /// Поворот заданной точки, относительно другой точки на определенный угол
        /// </summary>
        /// <param name="alfa">Угол поворота</param>
        /// <param name="_PointMain">Точка, относительно которой осуществляется поворот</param>
        /// <param name="_PointTemp">Поворачиваемая точка</param>
        /// <returns>Возвращает повернутую точку</returns>
        private Point RotatePoint(double alfa, Point _PointMain, Point _PointTemp)
        {
            Point Temp = new Point
            {
                X = (int)((double)(_PointTemp.X - _PointMain.X) * Math.Cos(alfa) - (double)(_PointTemp.Y - _PointMain.Y) * Math.Sin(alfa) + (double)_PointMain.X),
                Y = (int)((double)(_PointTemp.X - _PointMain.X) * Math.Sin(alfa) + (double)(_PointTemp.Y - _PointMain.Y) * Math.Cos(alfa) + (double)_PointMain.Y)
            };
            return Temp;
        }

        /// <summary>
        /// Генерация прямоугольника, отображаемого внутри здания
        /// </summary>
        private void GenLocalRect()
        {
            LocalRectangle = new MyRectangle();
            _MainRectangle = new MyRectangle();
            //Считаем угол наклона в зависимости от длин сторон
            double distance = CalcAB(MainRectangle.Points[0], MainRectangle.Points[1]);
            double _distance = CalcAB(MainRectangle.Points[1], MainRectangle.Points[3]);
            if (distance >= _distance)
            {
                p1 = 0; p2 = 1;
                alfa = CalcAlfa(MainRectangle.Points[0], MainRectangle.Points[1]);
                LocalRectangle.Points.Add(MainRectangle.Points[0]);
                LocalRectangle.Points.Add(new Point());
                LocalRectangle.Points.Add(new Point());
                LocalRectangle.Points.Add(RotatePoint(-alfa, MainRectangle.Points[0], MainRectangle.Points[3]));
                LocalRectangle.Points[2] = new Point(LocalRectangle.Points[0].X, LocalRectangle.Points[3].Y);
                LocalRectangle.Points[1] = new Point(LocalRectangle.Points[3].X, LocalRectangle.Points[0].Y);
            }
            else
            {
                p1 = 1; p2 = 3;
                alfa = CalcAlfa(MainRectangle.Points[1], MainRectangle.Points[3]);
                LocalRectangle.Points.Add(MainRectangle.Points[0]);
                LocalRectangle.Points.Add(new Point());
                LocalRectangle.Points.Add(new Point());
                LocalRectangle.Points.Add(RotatePoint(-alfa, MainRectangle.Points[0], MainRectangle.Points[3]));
                LocalRectangle.Points[2] = new Point(LocalRectangle.Points[3].X, LocalRectangle.Points[0].Y);
                LocalRectangle.Points[1] = new Point(LocalRectangle.Points[0].X, LocalRectangle.Points[3].Y);
            }
            _MainRectangle.Points.Add(LocalRectangle.Points[0]);
            _MainRectangle.Points.Add(LocalRectangle.Points[1]);
            _MainRectangle.Points.Add(LocalRectangle.Points[2]);
            _MainRectangle.Points.Add(LocalRectangle.Points[3]);
            MP = MainRectangle.Points[0];
            _MainRectangle.CalcMaxMin(out int maxx, out int minx, out int maxy, out int miny);
            CalcRederingArea(maxx, minx, maxy, miny);
            for (int i = 0; i < 4; i++)
            {
                double x = LocalRectangle.Points[i].X - Ox;
                double y = LocalRectangle.Points[i].Y - Oy;
                x *= koef;
                y *= koef;
                LocalRectangle.Points[i] = new Point((int)x, (int)y);
            }
            LocalRectangle.DL = LocalDL;
            LocalRectangle.delete = false;
            MainRectangle.CalcCenterPointWOZ();
            TempRectangle = (MyRectangle)MainRectangle.Clone();
        }
        public int p1, p2;
        /// <summary>
        /// Генерация многоугольника, отображаемого внутри здания
        /// </summary>
        private void GenLocalPolygon()
        {
            LocalPolygon = new Polygon();
            _MainPolygon = new Polygon();
            foreach (var p in MainPolygon.Points)
                LocalPolygon.Points.Add(new Point(p.X, p.Y));
            LocalPolygon.delete = false;
            double distance = -1;
            p1 = -1;
            p2 = -1;
            //ищем самую длинную грань по формуле AB = √(xb - xa)2 + (yb - ya)2
            for (int i = 0; i < MainPolygon.Points.Count; i++)
            {
                if (i != MainPolygon.Points.Count - 1)
                {
                    double _distance = CalcAB(MainPolygon.Points[i], MainPolygon.Points[i + 1]);
                    if (_distance > distance)
                    {
                        distance = _distance;
                        p1 = i;
                        p2 = i + 1;
                    }
                }
                else
                {
                    double _distance = CalcAB(MainPolygon.Points[i], MainPolygon.Points[0]);
                    if (_distance > distance)
                    {
                        distance = _distance;
                        p1 = i;
                        p2 = 0;
                    }
                }
            }
            alfa = CalcAlfa(MainPolygon.Points[p1], MainPolygon.Points[p2]);
            for (int i = 0; i < LocalPolygon.Points.Count; i++)
            {
                if (i != p1)
                {
                    LocalPolygon.Points[i] = RotatePoint(-alfa, LocalPolygon.Points[p1], LocalPolygon.Points[i]);
                    _MainPolygon.Points.Add(LocalPolygon.Points[i]);
                }
                else
                {
                    _MainPolygon.Points.Add(LocalPolygon.Points[i]);
                }
            }
            MP = _MainPolygon.Points[p1];
            _MainPolygon.CalcMaxMin(out int maxx, out int minx, out int maxy, out int miny);
            CalcRederingArea(maxx, minx, maxy, miny);
            for (int i = 0; i < LocalPolygon.Points.Count; i++)
            {
                int x = LocalPolygon.Points[i].X - Ox;
                int y = LocalPolygon.Points[i].Y - Oy;
                x = (int)(x * koef);
                y = (int)(y * koef);
                LocalPolygon.Points[i] = new Point(x, y);
            }
            LocalPolygon.DL = LocalDL;
            MainPolygon.CalcCenterPointWOZ();
            TempPolygon = (Polygon)MainPolygon.Clone();
        }
        /// <summary>
        /// Генерация круга, отображаемого внутри здания
        /// </summary>
        private void GenLocalCircle()
        {
            koef = 0;
            _MainCircle = (Circle)LocalCircle.Clone();
            LocalCircle = new Circle
            {
                delete = false,
                radius = MainCircle.radius,
                DL = LocalDL
            };
            pk = 1;
            koef = ((sizeRenderingArea.Width / 2) - (50d * MainForm.zoom)) / MainCircle.radius;
            LocalCircle.radius = (int)(LocalCircle.radius * koef);
            LocalCircle.MainCenterPoint = new Point(0, 0);
        }
        /// <summary>
        /// Расчет области отрисовки
        /// </summary>
        /// <param name="maxx"></param>
        /// <param name="minx"></param>
        /// <param name="maxy"></param>
        /// <param name="miny"></param>
        public void CalcRederingArea(int maxx, int minx, int maxy, int miny)
        {
            int _height = maxy - miny;
            int _width = maxx - minx;
            pk = (double)_width / (double)_height;
            sizeRenderingArea = new SizeRenderingArea(Name, (int)((double)sizeRenderingArea.Width / pk), sizeRenderingArea.Width);
            int difx = maxx - minx;
            koef = (double)(sizeRenderingArea.Width - (50d * MainForm.zoom)) / (double)difx;
            Ox = (maxx + minx) / 2;
            Oy = (maxy + miny) / 2;
        }

        /// <summary>
        /// Завершение перемещение входа в здание или входа провода
        /// </summary>
        internal void AddTemp()
        {
            if (isMoveEnt)
                AddTempEnt();
            if (isMoveIW)
                AddTempIW();
        }
        /// <summary>
        /// Расчет длины линии
        /// </summary>
        /// <param name="Point1">Точка 1</param>
        /// <param name="Point2">Точка 2</param>
        /// <returns>Возвращает длину линии</returns>
        private double CalcAB(Point Point1, Point Point2)
        {
            return Math.Sqrt((Math.Pow((Point2.X - Point1.X), 2) + Math.Pow((Point2.Y - Point1.Y), 2)));
        }
        /// <summary>
        /// Отрисовка
        /// </summary>
        public void Draw()
        {
            if (!delete)
            {
                if (type == 3)
                {
                    if (MainMapDL == MainForm.drawLevel)
                        MainPolygon.DrawB();
                    else if (LocalDL.Level == MainForm.drawLevel.Level)
                        LocalPolygon.DrawB(koef);
                    //_MainPolygon.delete = false;
                    //_MainPolygon.DrawB();
                }
                else if (type == 2)
                {
                    if (MainMapDL == MainForm.drawLevel)
                        MainRectangle.DrawB();
                    else if (LocalDL.Level == MainForm.drawLevel.Level)
                        LocalRectangle.DrawB(koef);
                    //_MainRectangle.delete = false;
                    //_MainRectangle.DrawB();
                }
                else if (type == 360)
                {
                    if (MainMapDL == MainForm.drawLevel)
                        MainCircle.DrawB();
                    else if (LocalDL.Level == MainForm.drawLevel.Level)
                        LocalCircle.DrawB(koef);
                }
                if (MainForm.filtres.Ent)
                    Entrances.Draw();
                if (MainForm.filtres.IW)
                    InputWires.Draw();
                if (MainForm.filtres.Text)
                    MT.Draw();
            }
        }

        /// <summary>
        /// Копирование элемента
        /// </summary>
        /// <returns>Возвращает копию элемента</returns>
        public object Clone()
        {
            List<string> _floors_name = new List<string>();
            foreach (var fn in floors_name)
                _floors_name.Add(fn);
            return new Building
            {
                alfa = this.alfa,
                basement = this.basement,
                delete = false,
                Entrances = (Entrances)this.Entrances.Clone(),
                InputWires = (InputWire)this.InputWires.Clone(),
                Floors = this.Floors,
                id = this.id,
                isInBuild = this.isInBuild,
                isMoveEnt = this.isMoveEnt,
                isMoveIW = this.isMoveIW,
                koef = this.koef,
                LocalDL = this.LocalDL,
                LocalCircle = (Circle)this.LocalCircle.Clone(),
                LocalPolygon = (Polygon)this.LocalPolygon.Clone(),
                LocalRectangle = (MyRectangle)this.LocalRectangle.Clone(),
                MainCircle = (Circle)this.MainCircle.Clone(),
                MainPolygon = (Polygon)this.MainPolygon.Clone(),
                MainRectangle = (MyRectangle)this.MainRectangle.Clone(),
                loft = this.loft,
                MainMapDL = this.MainMapDL,
                MP = new Point(this.MP.X, this.MP.Y),
                Name = this.Name,
                open = this.open,
                Ox = this.Ox,
                Oy = this.Oy,
                type = this.type,
                _MainCircle = (Circle)this._MainCircle.Clone(),
                _MainPolygon = (Polygon)this._MainPolygon.Clone(),
                _MainRectangle = (MyRectangle)this._MainRectangle.Clone(),
                floors_name = _floors_name,
                pk = this.pk,
                sizeRenderingArea = this.sizeRenderingArea,
                p1 = this.p1,
                p2 = this.p2,
                MT = (MyText)this.MT.Clone(),
                TempRectangle = (MyRectangle)this.TempRectangle.Clone(),
                TempPolygon = (Polygon)this.TempPolygon.Clone(),
                TempEntrances = (Entrances)this.TempEntrances.Clone(),
                TempInputWires = (InputWire)this.TempInputWires.Clone(),
                floors_count = this.floors_count
            };
        }

        /// <summary>
        /// Пересчет локальных значений
        /// </summary>
        internal void RefreshLocal()
        {
            if (type == 3)
            {
                GenLocalPolygon();
            }
            else if (type == 2)
            {
                GenLocalRect();
            }
            else if (type == 360)
            {
                GenLocalCircle();
            }
            RecalcLocalPoints();
        }

        /// <summary>
        /// Пересчет локальных значений для входов и входов проводов
        /// </summary>
        internal void RecalcLocalPoints()
        {
            foreach (var iw in InputWires.InputWires.Circles)
                iw.LocalCenterPoint = CalcLocalPoint(iw.MainCenterPoint);
            foreach (var ent in Entrances.Enterances.Circles)
                ent.LocalCenterPoint = CalcLocalPoint(ent.MainCenterPoint);
        }

        /// <summary>
        /// Поворот здания
        /// </summary>
        /// <param name="x">Координата Х</param>
        /// <param name="y">Координата У</param>
        /// <param name="type">Тип здания</param>
        /// <param name="networkWires">Группа сетевых элементов</param>
        /// <param name="build">Идентификатор здания</param>
        internal void SetPoint(int x, int y, int type, GroupOfNW networkWires, int build)
        {
            Point cp = new Point();
            Point _cp = new Point();
            double difangle = 0;
            double angle = 0;
            double _angle = 0;
            switch (type)
            {
                case 2:
                    cp = new Point((int)MainRectangle._CenterPointX, (int)MainRectangle._CenterPointY);
                    _cp = new Point((int)MainRectangle._CenterPointX, (int)MainRectangle._CenterPointY);
                    angle = CalcAlfa(_cp, new Point(x, y)) * 57.2958d;
                    _angle = CalcAlfa(cp, MainRectangle.Points[0]);
                    difangle = (angle - _angle) / 57.2958d;
                    MainRectangle.Points.Clear();
                    for (int i = 0; i < 4; i++)
                    {
                        MainRectangle.Points.Add(RotatePoint(difangle, cp, TempRectangle.Points[i]));
                    }
                    break;
                case 3:
                    cp = new Point((int)MainPolygon._CenterPointX, (int)MainPolygon._CenterPointY);
                    _cp = new Point((int)MainPolygon._CenterPointX, (int)MainPolygon._CenterPointY);
                    angle = CalcAlfa(_cp, new Point(x, y)) * 57.2958d;
                    _angle = CalcAlfa(cp, MainPolygon.Points[p1]);
                    difangle = (angle - _angle) / 57.2958d;
                    MainPolygon.Points.Clear();
                    foreach (var p in TempPolygon.Points)
                    {
                        MainPolygon.Points.Add(RotatePoint(difangle, cp, p));
                    }
                    break;
            }
            for (int i = 0; i < Entrances.Enterances.Circles.Count; i++)
            {
                Entrances.Enterances.Circles[i].MainCenterPoint = RotatePoint(difangle, cp, TempEntrances.Enterances.Circles[i].MainCenterPoint);
            }
            for (int i = 0; i < InputWires.InputWires.Circles.Count; i++)
            {
                if (InputWires.InputWires.Circles[i].MainDL.Level == -1)
                    InputWires.InputWires.Circles[i].MainCenterPoint = RotatePoint(difangle, cp, TempInputWires.InputWires.Circles[i].MainCenterPoint);
            }
            InputWires.MoveElem(0, 0, networkWires, build);
        }
        /// <summary>
        /// Завершение перемещения или поворота здания
        /// </summary>
        internal void EndMove()
        {
            if (type == 2)
            {
                RefreshLocal();
            }
            else if (type == 3)
            {
                RefreshLocal();
            }
            TempEntrances = (Entrances)Entrances.Clone();
            TempInputWires = (InputWire)InputWires.Clone();
        }
    }
}