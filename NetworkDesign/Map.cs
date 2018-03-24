using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform.Windows;

namespace NetworkDesign
{
    public class Map
    {
        //Базовые параметры
        public MapSettings mapSetting;
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
        //Редактирование
        public GroupOfEditRects EditRects = new GroupOfEditRects();
        //Лог
        public Log log = new Log();
        //Здания
        public GroupOfBuildings Buildings = new GroupOfBuildings();
        //Другое
        Timer RenderTimer = new Timer();

        public Map()
        {

        }

        /// <summary>
        /// Конструктор класса Map
        /// </summary>
        /// <param name="_MapSetting">Настройки карты</param>
        public Map(MapSettings _MapSetting)
        {
            MainForm.AnT.Height = _MapSetting.Height;
            MainForm.AnT.Width = _MapSetting.Width;
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);
            Gl.glClearColor(255, 255, 255, 1);
            Gl.glViewport(0, 0, MainForm.AnT.Width, MainForm.AnT.Height);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Glu.gluOrtho2D(0.0, _MapSetting.Width, 0.0, _MapSetting.Height);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            mapSetting = _MapSetting;
            RenderTimer.Interval = 15;
            RenderTimer.Tick += RenderTimer_Tick;
            RenderTimer.Start();
        }

        public void RenderTimer_Tick(object sender, EventArgs e) => Drawing();

        public void Drawing()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glLoadIdentity();
            Gl.glClearColor(1, 1, 1, 1);
            //Для многоугольников
            Polygons.Draw();
            //Для обычных линий
            Lines.Draw();
            //Для прямоугольников
            Rectangles.Draw();
            //Для кругов
            Circles.Draw();
            //Для зданий
            Buildings.Draw();
            //Для прямоугольников редактирования
            if (EditRects.edit_mode)
            {
                EditRects.Draw();
            }
            Gl.glFlush();
            MainForm.AnT.Invalidate();
        }

        /// <summary>
        /// Функция для установки инструмента
        /// </summary>
        /// <param name="instrument">ID инструмента
        /// 0 - курсор для выбора
        /// 1 - линия
        /// 2 - прямоугольник
        /// 3 - редактирование
        /// 5 - многоугольник
        /// 6 - точки входа проводов
        /// 7 - входы в здание
        /// 360 - круг</param>
        public void SetInstrument(int instrument)
        {
            DefaultTempElems();
            MainForm.AnT.Cursor = Cursors.Cross;
            RB = instrument;
            if (instrument == 3)
            {
                EditRects.edit_mode = true;
                MainForm.AnT.Cursor = Cursors.Arrow;
            }
            else if (instrument == 0)
            {
                MainForm.AnT.Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Функция для задания параметров по умолчанию временным переменным
        /// </summary>
        public void DefaultTempElems()
        {
            Lines.TempDefault();
            Polygons.TempDefault();
            Rectangles.TempDefault();
            Circles.TempDefault();
            Buildings.TempDefault();
            EditRects.TempDefault();
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
            log = TempMap.log;
            SetInstrument(RB);
        }

        /// <summary>
        /// Функция для Создания новой карты с новыми параметрами
        /// </summary>
        /// <param name="_mapSettings">Параметры карты</param>
        public void SetNewSettings(MapSettings _mapSettings)
        {
            Map TempMap = new Map
            {
                mapSetting = _mapSettings
            };
            MapLoad(TempMap);
        }

        /// <summary>
        /// Функция для обновления всех прямоугольников редактирования
        /// </summary>
        public void RefreshEditRect()
        {
            if (EditRects.edit_mode)
            {
                EditRects = new GroupOfEditRects();
                EditRects.RefreshEditRect(Lines, Rectangles, Polygons);
            }
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
            int line = Lines.Search(x, y, dl);
            if (line != -1)
            {
                type = 1;
                return line;
            }
            else
            {
                int rect = Rectangles.Search(x, y, out double distrect, dl);
                int bline = Polygons.Search(x, y, out double distbline, dl);
                int build = Buildings.CalcNearestBuild(x, y, out double distbuild, dl); 
                if (distrect == -1)
                    distrect = Int32.MaxValue;
                if (distbline == -1)
                    distbline = Int32.MaxValue;
                if (distbuild == -1)
                    distbuild = Int32.MaxValue;
                if (distbuild < distrect & distbuild < distbline)
                {
                    int entrance = Buildings.Buildings[build].Entrances.CalcNearestEnterise(x, y, dl);
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
                    }
                    type = 4;
                    return build;
                }
                else if (distbline < distrect & distbline < distbuild)
                {
                    type = 3;
                    return bline;
                }
                else if (distrect < distbline & distrect < distbuild)
                {
                    type = 2;
                    return rect;
                }
            }
            type = -1;
            return -1;
        }

        public void SearchEditElem(int x, int y)
        {
            EditRects.editRect = -1;
            EditRects.editRect = EditRects.Search(x, y);
            if (EditRects.editRect == -1)
            {
                EditRects.edit_active = false;
            }
            else
            {
                EditRects.edit_active = true;
            }
        }

        public void MoveElements(int x, int y)
        {
            int type = 0, item = 0, point = 0;
            if (EditRects.edit_active)
            {
                EditRects.EditRects[EditRects.editRect].Refresh(new Point(x, y));
                for (int i = 0; i < EditRects.EditRects[EditRects.editRect].elems.Count; i++)
                {
                    type = EditRects.EditRects[EditRects.editRect].elems[i].type;
                    item = EditRects.EditRects[EditRects.editRect].elems[i].count;
                    point = EditRects.EditRects[EditRects.editRect].elems[i].point;
                    switch (type)
                    {
                        case 1:
                            switch (point)
                            {
                                case 1:
                                    Lines.Lines[item].SetPoint(x, y, 0);
                                    break;
                                case 2:
                                    Lines.Lines[item].SetPoint(x, y, 1);
                                    break;
                            }
                            break;
                        case 2:
                            /*switch (point)
                            {
                                case 12:
                                    Rectangles.Rectangles[item].SetPoint(x, y);
                                    break;
                                case 13:
                                    Rectangles.Rectangles[item].SetPoint13(x, y);
                                    break;
                                case 24:
                                    Rectangles.Rectangles[item].SetPoint24(x, y);
                                    break;
                                case 34:
                                    Rectangles.Rectangles[item].SetPoint34(x, y);
                                    break;
                            }*/
                            break;
                        case 3:
                            Polygons.Polygons[item].SetPoint(x, y, point);
                            break;
                    }
                }
            }
        }

        public int InverseY(int y)
        {
            if (y > mapSetting.Height)
            {
                return mapSetting.Height + y;
            }
            else if (y < mapSetting.Height)
            {
                return mapSetting.Height - y;
            }
            else
            {
                return y;
            }
        }
    }
}
