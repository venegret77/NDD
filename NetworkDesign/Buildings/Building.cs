using NetworkDesign.NetworkElements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;

namespace NetworkDesign
{
    public class Building
    {
        public string Name;
        public int Floors; //0 - подвал если есть, последний - чердак
        public bool loft = false; //Чердак
        public bool basement = false; //Подвал
        public int type = 2; //2 - прямоугольник, 3 - многоугольник, 360 - круг
        public bool open = false;
        public bool delete = true;
        //
        private double koef = 1;
        private double alfa = 0;
        private Point MP = new Point();
        private int Ox, Oy, _Ox, _Oy;
        //
        public Polygon MainPolygon = new Polygon();
        public Polygon _MainPolygon = new Polygon();
        public Polygon LocalPolygon = new Polygon();
        //
        public MyRectangle MainRectangle = new MyRectangle();
        public MyRectangle _MainRectangle = new MyRectangle();
        public MyRectangle LocalRectangle = new MyRectangle();
        //
        public Circle MainCircle = new Circle();
        public Circle _MainCircle = new Circle();
        public Circle LocalCircle = new Circle();
        //
        public DrawLevel MainMapDL = new DrawLevel();
        public DrawLevel LocalDL = new DrawLevel();
        public List<string> floors_name = new List<string>();
        //
        public Entrances Entrances = new Entrances();
        //
        public InputWire InputWires = new InputWire();

        bool isMoveEnt = false;
        int id = -1;
        private bool isMoveIW;

        public Building()
        {
            delete = true;
        }

        /// <summary>
        /// Конструктор для копирования при создании шаблона
        /// </summary>
        /// <param name="_build"></param>
        public Building(Building _build)
        {
            alfa =_build.alfa;
            basement = _build.basement;
            delete = _build.delete;
            Entrances = _build.Entrances;
            Floors = _build.Floors;
            floors_name = _build.floors_name;
            koef = _build.koef;
            LocalDL = _build.LocalDL;
            LocalPolygon = _build.LocalPolygon;
            LocalRectangle = _build.LocalRectangle;
            loft = _build.loft;
            MainMapDL = _build.MainMapDL;
            MainPolygon = _build.MainPolygon;
            MainRectangle = _build.MainRectangle;
            MP = _build.MP;
            Name = _build.Name;
            type = _build.type;
            Ox = _build.Ox;
            Oy = _build.Oy;
            _Ox = _build._Ox;
            _Oy = _build._Oy;
            open = _build.open;
            InputWires = new InputWire();
        }

        public Building(string _Name, bool _loft, bool _basement, int floors_count, Polygon _pol, int index)
        {
            Name = _Name;
            loft = _loft;
            basement = _basement;
            if (basement & loft)
                Floors = floors_count + 2;
            else if (basement)
                Floors = floors_count + 1;
            else if (loft)
                Floors = floors_count + 1;
            else
                Floors = floors_count;
            type = 3;
            delete = false;
            MainMapDL.Level = -1;
            MainMapDL.Floor = -1;
            LocalDL.Level = index;
            LocalDL.Floor = 0;
            Point[] temp = new Point[_pol.Points.Count];
            _pol.Points.CopyTo(temp);
            MainPolygon = new Polygon
            {
                Points = temp.ToList(),
                delete = false,
                DL = MainMapDL
            };
            GenLocalPolygon();
            UpgrateFloors();
        }

        public Building(string _Name, bool _loft, bool _basement, int floors_count, MyRectangle _rect, int index)
        {
            Name = _Name;
            loft = _loft;
            basement = _basement;
            if (basement & loft)
                Floors = floors_count + 2;
            else if (basement)
                Floors = floors_count + 1;
            else if (loft)
                Floors = floors_count + 1;
            else
                Floors = floors_count;
            MainRectangle = _rect;
            type = 2;
            delete = false;
            MainMapDL.Level = -1;
            MainMapDL.Floor = -1;
            LocalDL.Level = index;
            LocalDL.Floor = 0;
            MainRectangle.DL = MainMapDL;
            GenLocalRect();
            UpgrateFloors();
        }

        public Building(string _Name, bool _loft, bool _basement, int floors_count, Circle _circle, int index)
        {
            Name = _Name;
            loft = _loft;
            basement = _basement;
            if (basement & loft)
                Floors = floors_count + 2;
            else if (basement)
                Floors = floors_count + 1;
            else if (loft)
                Floors = floors_count + 1;
            else
                Floors = floors_count;
            MainCircle = _circle;
            type = 360;
            delete = false;
            MainMapDL.Level = -1;
            MainMapDL.Floor = -1;
            LocalDL.Level = index;
            LocalDL.Floor = 0;
            MainCircle.DL = MainMapDL;
            RecalcCircle();
            UpgrateFloors();
        }

        private void RecalcCircle()
        {
            koef = 0;
            _MainCircle = LocalCircle;
            LocalCircle = new Circle
            {
                delete = false,
                radius = MainCircle.radius,
                DL = LocalDL
            };
            if (MainForm._Height >= MainForm._Width)
                koef = ((MainForm._Height / 2) - 150) / MainCircle.radius;
            else
                koef = ((MainForm._Width / 2) - 150) / MainCircle.radius;
            LocalCircle.radius = (int)(LocalCircle.radius * koef);
            LocalCircle.MainCenterPoint = new Point(0, 0);
        }

        public void AddTempIW()
        {
            if (isMoveIW & id != -1)
            {
                isMoveIW = false;
                InputWires.InputWires.TempCircle.LocalCenterPoint = CalcLocalPoint(InputWires.InputWires.TempCircle.MainCenterPoint);
                InputWires.InputWires.Circles[id].MainCenterPoint = MainForm._GenZoomPoint(new Point(InputWires.InputWires.TempCircle.MainCenterPoint.X, InputWires.InputWires.TempCircle.MainCenterPoint.Y));
                InputWires.InputWires.Circles[id].LocalCenterPoint = MainForm._GenZoomPoint(new Point(InputWires.InputWires.TempCircle.LocalCenterPoint.X, InputWires.InputWires.TempCircle.LocalCenterPoint.Y));
                InputWires.InputWires.Circles[id].delete = false;
                InputWires.InputWires.TempDefault();
            }
        }

        internal void MoveIW(int x, int y, int id, int build, GroupOfNW networkWires)
        {
            this.id = id;
            isMoveIW = true;
            var iw = InputWires.InputWires.Circles[id];
            InputWires.InputWires.TempCircle = new Circle
            {
                MainCenterPoint = new Point(iw.MainCenterPoint.X, iw.MainCenterPoint.Y),
                delete = false,
                koef = iw.koef,
                side = iw.side
            };
            InputWires.InputWires.Circles[id].delete = true;
            MoveIW(x, y);
            networkWires.CheckNW((int)((double)InputWires.InputWires.TempCircle.MainCenterPoint.X), (int)((double)InputWires.InputWires.TempCircle.MainCenterPoint.Y), id, true, build);
        }

        private void UpgrateFloors()
        {
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

        public void CalcCenterPoint()
        {
            if (type == 2)
                MainRectangle.CalcCenterPoint();
            else if (type == 3)
                MainPolygon.CalcCenterPoint();
            else if (type == 360)
                MainCircle.CalcCenterPoint();
        }

        public void MoveElem(int x, int y)
        {
            int difx = 0;
            int dify = 0;
            if (type == 2)
            {
                difx = x - (int)MainRectangle.CenterPointX;
                dify = y - (int)MainRectangle.CenterPointY;
                MainRectangle.MoveElem(x, y);
            }
            else if (type == 3)
            {
                difx = x - (int)MainPolygon.CenterPointX;
                dify = y - (int)MainPolygon.CenterPointY;
                MainPolygon.MoveElem(x, y);
            }
            else if (type == 360)
            {
                difx = (int)((double)x / MainForm.Zoom) - MainCircle.MainCenterPoint.X;
                dify = (int)((double)y / MainForm.Zoom) - MainCircle.MainCenterPoint.Y;
                MainCircle.MoveElem(x, y);
            }
            Entrances.MoveElem(difx, dify);
            InputWires.MoveElem(difx, dify);
        }

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
                return true;
            }
        }

        /// <summary>
        /// Расчет точки внутри здания //доделать точный расчет
        /// </summary>
        /// <param name="MainP">Точка на главном виде</param>
        /// <returns>Возвращает нужную точку</returns>
        public Point CalcLocalPoint(Point MainP)
        {
            if (type == 360)
            {
                Point LocalP = new Point
                {
                    X = MainP.X,
                    Y = MainP.Y
                };
                LocalP = new Point((int)(LocalCircle.radius * Math.Cos(Entrances.angle * Math.PI / 180) + LocalCircle.MainCenterPoint.X), (int)(LocalCircle.radius * Math.Sin(Entrances.angle * Math.PI / 180) + LocalCircle.MainCenterPoint.Y));
                return LocalP;
            }
            else
            {
                Point LocalP = RotatePoint(-alfa, MP, MainP); 
                LocalP.X -= Ox; LocalP.Y -= Oy;
                LocalP.X = (int)(LocalP.X * koef); LocalP.Y = (int)(LocalP.Y * koef);
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
                return true;
            }
        }

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

        public void MoveIWInBuild(int x, int y)
        {
            InputWires.SetTempPoint(x, y);
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
            double _alfa = Math.Atan(cat1 / cat2);
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
                X = (int)((_PointTemp.X - _PointMain.X) * Math.Cos(alfa) - (_PointTemp.Y - _PointMain.Y) * Math.Sin(alfa) + _PointMain.X),
                Y = (int)((_PointTemp.X - _PointMain.X) * Math.Sin(alfa) + (_PointTemp.Y - _PointMain.Y) * Math.Cos(alfa) + _PointMain.Y)
            };
            return Temp;
        }

        /// <summary>
        /// Генерация прямоугольника, отображаемого внутри здания
        /// </summary>
        private void GenLocalRect()
        {
            //Считаем угол наклона в зависимости от длин сторон
            double distance = CalcAB(MainRectangle.Points[0], MainRectangle.Points[1]);
            double _distance = CalcAB(MainRectangle.Points[1], MainRectangle.Points[3]);
            if (distance >= _distance)
            {
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
                alfa = CalcAlfa(MainRectangle.Points[1], MainRectangle.Points[3]);
                LocalRectangle.Points.Add(MainRectangle.Points[0]);
                LocalRectangle.Points.Add(new Point());
                LocalRectangle.Points.Add(new Point());
                LocalRectangle.Points.Add(RotatePoint(-alfa, MainRectangle.Points[0], MainRectangle.Points[3]));
                LocalRectangle.Points[2] = new Point(LocalRectangle.Points[3].X, LocalRectangle.Points[0].Y);
                LocalRectangle.Points[1] = new Point(LocalRectangle.Points[0].X, LocalRectangle.Points[3].Y);
            }
            _MainRectangle.Points.Add(MainRectangle.Points[0]);
            _MainRectangle.Points.Add(RotatePoint(-alfa, MainRectangle.Points[0], MainRectangle.Points[1]));
            _MainRectangle.Points.Add(RotatePoint(-alfa, MainRectangle.Points[0], MainRectangle.Points[2]));
            _MainRectangle.Points.Add(RotatePoint(-alfa, MainRectangle.Points[0], MainRectangle.Points[3]));
            MP = MainRectangle.Points[0];
            LocalRectangle.CalcMaxMin(out int maxx, out int minx, out int maxy, out int miny);
            int difx = maxx - minx;
            int dify = maxy - miny;
            koef = (double)(MainForm._Height - 100) / (double)dify;
            if (((double)(MainForm._Width - 100) / (double)difx) < koef)
                koef = (double)(MainForm._Width - 100) / (double)difx;
            Ox = (LocalRectangle.Points[0].X + LocalRectangle.Points[1].X + LocalRectangle.Points[2].X + LocalRectangle.Points[3].X) / 4;
            Oy = (LocalRectangle.Points[0].Y + LocalRectangle.Points[1].Y + LocalRectangle.Points[2].Y + LocalRectangle.Points[3].Y) / 4;
            _Ox = (MainForm._Width) / 2;
            _Oy = (MainForm._Height) / 2;
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
        }

        /// <summary>
        /// Генерация многоугольника, отображаемого внутри здания
        /// </summary>
        private void GenLocalPolygon()
        {
            Point[] temp = new Point[MainPolygon.Points.Count];
            MainPolygon.Points.CopyTo(temp);
            LocalPolygon = new Polygon
            {
                Points = temp.ToList(),
                delete = false
            };
            //_brokenLine = brokenLine;
            double distance = -1;
            int p1 = -1, p2 = -1;
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
            }
            MP = LocalPolygon.Points[p1];
            LocalPolygon.CalcMaxMin(out int maxx, out int minx, out int maxy, out int miny);
            int difx = maxx - minx;
            int dify = maxy - miny;
            koef = (double)(MainForm._Height - 150) / (double)dify;
            if (((double)(MainForm._Width - 150) / (double)difx) < koef)
                koef = (double)(MainForm._Width - 150) / (double)difx;
            _Ox = (MainForm._Width) / 2;
            _Oy = (MainForm._Height) / 2;
            for (int i = 0; i < LocalPolygon.Points.Count; i++)
            {
                Ox += LocalPolygon.Points[i].X;
                Oy += LocalPolygon.Points[i].Y;
            }
            Ox /= LocalPolygon.Points.Count;
            Oy /= LocalPolygon.Points.Count;
            for (int i = 0; i < LocalPolygon.Points.Count; i++)
            {
                int x = LocalPolygon.Points[i].X - Ox;
                int y = LocalPolygon.Points[i].Y - Oy;
                x = (int)(x * koef);
                y = (int)(y * koef);
                LocalPolygon.Points[i] = new Point(x, y);
            }
            LocalPolygon.DL = LocalDL;
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

        private double CalcAB(Point Point1, Point Point2)
        {
            return Math.Sqrt((Math.Pow((Point2.X - Point1.X), 2) + Math.Pow((Point2.Y - Point1.Y), 2)));
        }

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
                }
                else if (type == 2)
                {
                    if (MainMapDL == MainForm.drawLevel)
                        MainRectangle.DrawB();
                    else if (LocalDL.Level == MainForm.drawLevel.Level)
                        LocalRectangle.DrawB(koef);
                }
                else if (type == 360)
                {
                    if (MainMapDL == MainForm.drawLevel)
                        MainCircle.DrawB();
                    else if (LocalDL.Level == MainForm.drawLevel.Level)
                        LocalCircle.DrawB(koef);
                }
                Entrances.Draw();
                InputWires.Draw();
            }
        }
    }
}