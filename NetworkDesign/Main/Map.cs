﻿using NetworkDesign.NetworkElements;
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
    public class Map
    {
        //Базовые параметры
        public SizeRenderingArea mapSetting;
        //private SimpleOpenGlControl AnT;
        public int RB = 1; //Какой инструмент выбран //1-линия //2-прямоугольник
        //Для линий
        public GroupOfLines Lines = new GroupOfLines(); //Группа линий
        //Для многоугольников
        public GroupOfPolygons Polygons = new GroupOfPolygons();
        //Для прямоугольников
        public GroupOfRectangles Rectangles = new GroupOfRectangles(); //Группа прямоугольников
        //Для кругов
        public GroupOfCircle Circles = new GroupOfCircle();
        //Для элементов сети
        public GroupOfNE NetworkElements = new GroupOfNE();
        public GroupOfNW NetworkWires = new GroupOfNW();
        public GroupOfMT MyTexts = new GroupOfMT();
        //Редактирование
        public GroupOfEditRects EditRects = new GroupOfEditRects();
        //Лог
        public Log log = new Log();
        //Здания
        public GroupOfBuildings Buildings = new GroupOfBuildings();

        public int UserID;
        //Другое
        [XmlIgnore()]
        public Timer RenderTimer = new Timer();
        /// <summary>
        /// Переменная, показывающая, перемещается ли в данный момент элемент или нет
        /// </summary>
        public bool isMove = false;

        public Map()
        {

        }

        /// <summary>
        /// Конструктор класса Map
        /// </summary>
        /// <param name="_MapSetting">Настройки карты</param>
        public Map(SizeRenderingArea _MapSetting)
        {
            MainForm.AnT.Height = _MapSetting.Height;
            MainForm.AnT.Width = _MapSetting.Width;
            if (!MainForm.isInit)
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
            Gl.glClearColor(255, 255, 255, 1);
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
            mapSetting = _MapSetting;
            RenderTimer.Interval = 15;
            RenderTimer.Tick += RenderTimer_Tick;
            RenderTimer.Start();
            MainForm.isInit = true;
        }

        public void RenderTimer_Tick(object sender, EventArgs e) => Drawing();

        public void Drawing()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glLoadIdentity();
            Gl.glClearColor(1, 1, 1, 1);
            DrawTemp();
            Gl.glPushMatrix();
            Gl.glScaled(MainForm.zoom, MainForm.zoom, MainForm.zoom);
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

        public void DrawingWOF()
        {
            Gl.glClear(Gl.GL_CLEAR | Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glLoadIdentity();
            Gl.glClearColor(1, 1, 1, 1);
            Gl.glPushMatrix();
            NetworkWires.Draw();
            Buildings.Draw();
            Polygons.Draw();
            Lines.Draw();
            Rectangles.Draw();
            Circles.Draw();
            NetworkElements.Draw();
            MyTexts.Draw();
            Gl.glFinish();
            MainForm.AnT.Invalidate();
        }

        public void ExportListNE(string path)
        {
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.Default))
            {
                sw.WriteLine("Список элементов для карты сети " + mapSetting.Name);
                for (int i = 0; i < Buildings.Buildings.Count; i++)
                {
                    sw.WriteLine("Здание " + Buildings.Buildings[i].Name + ":");
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

        public void ResizeRenderingArea()
        {
            int Height = (int)((double)mapSetting.Height * MainForm.zoom);
            int Width = (int)((double)mapSetting.Width * MainForm.zoom);
            int Left = (int)((double)mapSetting.Left * MainForm.zoom);
            int Right = (int)((double)mapSetting.Right * MainForm.zoom);
            int Top = (int)((double)mapSetting.Top * MainForm.zoom);
            int Bottom = (int)((double)mapSetting.Bottom * MainForm.zoom);
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
                        Buildings.Buildings[id].MoveElem(x, y);
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
            RB = instrument;
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
        /// Функция для загрузки карты
        /// </summary>
        /// <param name="TempMap">Карта из файла / Пустая карта</param>
        public void MapLoad(Map TempMap)
        {
            RB = 0;
            mapSetting = TempMap.mapSetting;
            MainForm.AnT.Height = mapSetting.Height;
            MainForm.AnT.Width = mapSetting.Width;
            RB = TempMap.RB;
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
            SetInstrument(RB);
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
                mapSetting = _mapSettings
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
            /*EditRects.Add(_EditRects[0]);
            for (int i = 1; i < _EditRects.Count; i++)
            {
                for (int j = 0; j < EditRects.Count; j++)
                {
                    if (EditRects[j].coords == _EditRects[i].coords)
                    {
                        EditRects[j].elems.Add(_EditRects[i].elems[0]);
                        _EditRects.RemoveAt(i);
                        i--;
                        break;
                    }
                    else
                    {
                        EditRects.Add(_EditRects[i]);
                    }
                }
            }*/
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
                /*int entrance = Buildings.Buildings[build].Entrances.CalcNearestEnterise(x, y, dl);
                if (entrance != -1)
                {
                    buildindex = build;
                    type = 7;
                    return entrance;
                }
                int IW = Buildings.Buildings[build].InputWires.CalcNearestIW(x, y, dl);
                if (IW != -1)
                {
                    buildindex = build;
                    type = 6;
                    return IW;
                }*/
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
        }

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
                    case 5:
                        Polygons.Choose(id);
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
                        case 5:
                            Polygons.Polygons[id].SetPoint(x, y, point);
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
                return (int)((double)mapSetting.Height / 2d * MainForm.zoom) - y;
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
                return x - (int)((double)mapSetting.Width / 2d * MainForm.zoom);
            else
                return x - (int)((double)Buildings.Buildings[MainForm.drawLevel.Level].sizeRenderingArea.Width / 2d * MainForm.zoom);
        }

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

        internal bool SearchNE(int x, int y)
        {
            int NE = NetworkElements.Search(x, y, MainForm.drawLevel);
            if (NE != -1)
            {
                Element elem = new Element(13, NE, NetworkElements.NetworkElements[NE].Options.Clone(), -4);
                NESettings nes = new NESettings(NetworkElements.NetworkElements[NE].Options, NetworkElements, ref NetworkElements.NetworkElements[NE].notes);
                nes.ShowDialog();
                foreach (var lm in nes.log.Back)
                {
                    log.Add(lm);
                }
                NetworkElements = nes.NetworkElements;
                NetworkElements.NetworkElements[NE].Options = nes.Options;
                Element _elem = new Element(13, NE, NetworkElements.NetworkElements[NE].Options.Clone(), -4);
                log.Add(new LogMessage("Изменил параметры устройства", elem, _elem));
                return true;
            }
            return false;
        }

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
