using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Security.Cryptography;

namespace DominoPyramidLib
{
    public class Game
    {
        /// <summary>
        /// Ячейки для домино
        /// </summary>
        public Cell[,] Cells = new Cell[7,7];

        /// <summary>
        /// Кости домино
        /// </summary>
        public Domino[] Dominos = new Domino[28];

        /// <summary>
        /// Строка с количеством отавшихся костей
        /// </summary>
        public Label CellsLabel { get; set; }

        /// <summary>
        /// Програсс бар отображающий прогресс игры
        /// </summary>
        public ProgressBar PrB { get; set; }

        /// <summary>
        /// Выбрана ли какая-то ячейка
        /// </summary>
        bool IsSoomeSelected { get; set; }

        /// <summary>
        /// Координата X выбранной ячейки
        /// </summary>
        int SelectedCellX { get; set; }

        /// <summary>
        /// Координата Y выбранной ячейки
        /// </summary>
        int SelectedCellY { get; set; }

        /// <summary>
        /// PictureBox, хранящий в себе свои координаты в массиве
        /// </summary>
        public class PB : PictureBox
        {
            public int X { get; set; }
            public int Y { get; set; }
        }

        /// <summary>
        /// Инициализация ячеек
        /// </summary>
        public void InitializeCells()
        {
            for (int x = 0; x < Cells.GetLength(0); x++)
            {
                for (int y = 0; y < Cells.GetLength(1); y++)
                {
                    //Инициализация ячейки
                    Cells[x, y] = new Cell();
                    //Инициализация PictureBox-а
                    Cells[x, y].PB_Cell = new PB();

                    //Определение активности ячейки по её расположению
                    Cells[x, y].IsActive = (x > y) ? false : true;

                    //Создание событий ячеек
                    Cells[x, y].PB_Cell.Click += new EventHandler(CellClick);

                    //Передача ячейкам их координат
                    Cells[x, y].PB_Cell.X = x;
                    Cells[x, y].PB_Cell.Y = y;
                }
            }
        }

        /// <summary>
        /// Событие нажатия на PictureBox
        /// </summary>
        public void CellClick(object sender, EventArgs e)
        {
            //Представление PictureBox-а как объекта PB
            PB cell = sender as PB;

            //Извлечение из ячейки её координат           
            int X = cell.X;
            int Y = cell.Y;


            if (IsSoomeSelected)//Если какая-то ячейка уже выбрана...
            {
                if(Cells[X, Y].IsSelected)//...отменить выбор
                {
                    CellSelection(Cells[X, Y]);
                }
                else if(CellsComparison(Cells[X, Y], Cells[SelectedCellX, SelectedCellY]))//...убрать обе ячейки
                {
                    DeleteCell(Cells[X, Y]);
                    DeleteCell(Cells[SelectedCellX, SelectedCellY]);
                    IsSoomeSelected = false;
                }
            }
            else//Иначе - выбрать ячейку
            {
                CellSelection(Cells[X, Y]);
            }

            //Нахождение и открытие ячеек
            UnlockCheck();

            //Проверка победы
            if (WinCheck())
            {
                MessageBox.Show("Вы победили", "Победа!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                RandomizeDominos();
                StartGame();
            }

            //Проверка поражения
            if (LosingCheck())
            {
                MessageBox.Show("Вы проиграли", "Поражение", MessageBoxButtons.OK, MessageBoxIcon.Information);

                RandomizeDominos();
                StartGame();
            }

            //Вывод информации о кол-ве ячеек на форму
            GetCells();
        }

        /// <summary>
        /// Отрисовка ячеек
        /// </summary>
        /// <param name="PaintForm"> Форма для отрисовки</param>
        public void PaintCell(Panel PaintForm)
        {

            int AligmentOffset = 210;//Отступ для образования пирамиды
            
            int HotizontalGap;//Горизонтальный зазор между ячейками
            int VerticalGap = 0;//Вертикальный зазор между ячейками

            for (int y = 0; y < Cells.GetLength(0); y++)
            {
                HotizontalGap = 0;
                for (int x = 0; x < Cells.GetLength(1); x++)
                {
                    if (Cells[x, y].IsActive)
                    {
                        PaintForm.Controls.Add(Cells[x, y].PB_Cell);

                        //Задание параметров ячеек
                        Cells[x, y].PB_Cell.Height = 30;
                        Cells[x, y].PB_Cell.Width = 60;
                        Cells[x, y].PB_Cell.BackColor = Color.Transparent;
                        Cells[x, y].PB_Cell.BorderStyle = BorderStyle.None;
                        Cells[x, y].PB_Cell.Cursor = Cursors.Hand;
                        Cells[x, y].PB_Cell.BackgroundImage = Image.FromFile(@"images/06.png");
                        Cells[x, y].PB_Cell.BackgroundImageLayout = ImageLayout.Stretch;

                        //Задание положения ячеек на форме
                        Cells[x, y].PB_Cell.Location = new Point((60 * x) + AligmentOffset + HotizontalGap, (30 * y) + VerticalGap);

                        HotizontalGap += 3;                        
                    }
                }
                VerticalGap += 3;
                AligmentOffset -= 30;
            }
        }

        /// <summary>
        /// Выделение ячейки
        /// </summary>
        /// <param name="cell">Выделяемая ячейка</param>
        public void CellSelection(Cell cell)
        {
            if (cell.IsUnlocked)
            {
                if (cell.IsSelected)
                {
                    cell.PB_Cell.BorderStyle = BorderStyle.None;
                    cell.IsSelected = false;
                    IsSoomeSelected = false;
                }
                else
                {
                    cell.PB_Cell.BorderStyle = BorderStyle.Fixed3D;
                    cell.IsSelected = true;
                    IsSoomeSelected = true;

                    SelectedCellX = cell.PB_Cell.X;
                    SelectedCellY = cell.PB_Cell.Y;
                }
            }
        }

        /// <summary>
        /// Проверка, дают ли две ячейки в сумме 12 баллов
        /// </summary>
        /// <param name="FirstCell">Первая ячейка</param>
        /// <param name="SecondCell">Вторая ячейка</param>
        public bool CellsComparison(Cell FirstCell, Cell SecondCell)
        {
            if(FirstCell.Domino.Value + SecondCell.Domino.Value == 12)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Удаление ячейки
        /// </summary>
        public void DeleteCell(Cell cell)
        {
            cell.IsSelected = false;
            cell.IsActive = false;
            cell.PB_Cell.Visible = false;
        }

        /// <summary>
        /// Проверка всех ячеек на разблокировку
        /// </summary>
        public void UnlockCheck()
        {
            for (int x = 0; x < Cells.GetLength(0); x++)
            {
                for (int y = 0; y < Cells.GetLength(1); y++)
                {
                    if (Cells[x, y].IsActive)
                    {
                        if (y == 0 || y == 6)//Игнорирование верхних и нижних ячеек
                        {

                        }
                        else if (x == 0)//Проверка ячеек по левому краю
                        {
                            if ((Cells[x, y + 1].IsActive == false && Cells[x + 1, y + 1].IsActive == false) || Cells[x, y - 1].IsActive == false)
                            {
                                Cells[x, y].IsUnlocked = true;
                                Cells[x, y].PB_Cell.BackgroundImage = Cells[x, y].Domino.DominoImage;
                            }
                        }
                        else if (y == x)//Проверка ячеек по првому краю
                        {
                            if ((Cells[x, y + 1].IsActive == false && Cells[x + 1, y + 1].IsActive == false) || Cells[x - 1, y - 1].IsActive == false)
                            {
                                Cells[x, y].IsUnlocked = true;
                                Cells[x, y].PB_Cell.BackgroundImage = Cells[x, y].Domino.DominoImage;
                            }
                        }
                        else//Проверка остальных ячеек
                        {
                            if ((Cells[x, y + 1].IsActive == false && Cells[x + 1, y + 1].IsActive == false) || (Cells[x - 1, y - 1].IsActive == false && Cells[x, y - 1].IsActive == false))
                            {
                                Cells[x, y].IsUnlocked = true;
                                Cells[x, y].PB_Cell.BackgroundImage = Cells[x, y].Domino.DominoImage;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Проверка проигрыша
        /// </summary>
        public bool LosingCheck()
        {
            for (int x = 0; x < Cells.GetLength(0); x++)
            {
                for (int y = 0; y < Cells.GetLength(1); y++)
                {
                    if (Cells[x,y].IsActive && Cells[x, y].IsUnlocked)//Проверка наличия пары для каждой ячейки
                    {
                        if(PossibilityCheck(Cells[x, y], x, y) == false)
                        {
                            return false;
                        }                        
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Проверка возможности коомбинации для клетки
        /// </summary>
        /// <param name="cell">Первая ячейка</param>
        /// <param name="impX">Координата X второй ячейки</param>
        /// <param name="impY">Координата Y второй ячейки</param>
        public bool PossibilityCheck(Cell cell, int impX, int impY)
        {
            for (int x = 0; x < Cells.GetLength(0); x++)
            {
                for (int y = 0; y < Cells.GetLength(1); y++)
                {
                    if ((x != impX || y != impY) && (Cells[x, y].IsActive && Cells[x, y].IsUnlocked))//Проверка комбинации с каждой клеткой, кроме себя
                    {
                        if (Cells[x, y].Domino.Value + Cells[impX, impY].Domino.Value == 12)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Проверка победы
        /// </summary>
        public bool WinCheck()
        {
            for (int x = 0; x < Cells.GetLength(0); x++)
            {
                for (int y = 0; y < Cells.GetLength(1); y++)
                {
                    if (Cells[x, y].IsActive)//Проверка наличия активных ячеек
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Вывод информации о количестве оставшихся костей на форму
        /// </summary>
        public void GetCells()
        {
            //Счётчик
            int N=0;

            //Подмчёт ячеек
            for (int x = 0; x < Cells.GetLength(0); x++)
            {
                for (int y = 0; y < Cells.GetLength(1); y++)
                {
                    if (Cells[x, y].IsActive)
                    {
                        N++;
                    }
                }
            }

            PrB.Value = 28 - N;//Вывод на прогресс бар
            CellsLabel.Text = "Осталось " + Convert.ToString(N) + " костей из 28";//Вывод на лейбл
        }

        /// <summary>
        /// Ячейка в корой содержиться кость домино
        /// </summary>
        public class Cell
        {
            /// <summary>
            /// PictureBox, который отображает кость домино.
            /// </summary>
            public PB PB_Cell { get; set; }
            
            /// <summary>
            /// Указывает используется ли ячейка в игре.
            /// </summary>
            public bool IsActive { get; set; }

            /// <summary>
            /// Указывает выбрана ли ячейка
            /// </summary>
            public bool IsSelected { get; set; }

            /// <summary>
            /// Кость домино, привязанная к ячейке.
            /// </summary>
            public Domino Domino { get; set; }

            /// <summary>
            /// Указывает открыта ли ячейка.
            /// </summary>
            public bool IsUnlocked { get; set; }
        }

        /// <summary>
        /// Кость домино.
        /// </summary>
        public class Domino
        {
            /// <summary>
            /// Величина домино
            /// </summary>
            public int Value { get; set; }

            /// <summary>
            /// Изображение домино
            /// </summary>
            public Image DominoImage { get; set; }
        }

        /// <summary>
        /// Инициализация домино.
        /// </summary>
        public void InitializeDominos()
        {
            for (int i = 0; i < Dominos.Length; i++)
            {
                Dominos[i] = new Domino();
            }

            Dominos[0].DominoImage = Image.FromFile(@"images/00.png");
            Dominos[0].Value = 0;

            Dominos[1].DominoImage = Image.FromFile(@"images/01.png");
            Dominos[1].Value = 1;

            Dominos[2].DominoImage = Image.FromFile(@"images/02.png");
            Dominos[2].Value = 2;

            Dominos[3].DominoImage = Image.FromFile(@"images/03.png");
            Dominos[3].Value = 3;

            Dominos[4].DominoImage = Image.FromFile(@"images/04.png");
            Dominos[4].Value = 4;

            Dominos[5].DominoImage = Image.FromFile(@"images/05.png");
            Dominos[5].Value = 5;

            Dominos[6].DominoImage = Image.FromFile(@"images/06.png");
            Dominos[6].Value = 6;

            Dominos[7].DominoImage = Image.FromFile(@"images/11.png");
            Dominos[7].Value = 2;

            Dominos[8].DominoImage = Image.FromFile(@"images/12.png");
            Dominos[8].Value = 3;

            Dominos[9].DominoImage = Image.FromFile(@"images/13.png");
            Dominos[9].Value = 4;

            Dominos[10].DominoImage = Image.FromFile(@"images/14.png");
            Dominos[10].Value = 5;

            Dominos[11].DominoImage = Image.FromFile(@"images/15.png");
            Dominos[11].Value = 6;

            Dominos[12].DominoImage = Image.FromFile(@"images/16.png");
            Dominos[12].Value = 7;

            Dominos[13].DominoImage = Image.FromFile(@"images/22.png");
            Dominos[13].Value = 4;

            Dominos[14].DominoImage = Image.FromFile(@"images/23.png");
            Dominos[14].Value = 5;

            Dominos[15].DominoImage = Image.FromFile(@"images/24.png");
            Dominos[15].Value = 6;

            Dominos[16].DominoImage = Image.FromFile(@"images/25.png");
            Dominos[16].Value = 7;

            Dominos[17].DominoImage = Image.FromFile(@"images/26.png");
            Dominos[17].Value = 8;

            Dominos[18].DominoImage = Image.FromFile(@"images/33.png");
            Dominos[18].Value = 6;

            Dominos[19].DominoImage = Image.FromFile(@"images/34.png");
            Dominos[19].Value = 7;

            Dominos[20].DominoImage = Image.FromFile(@"images/35.png");
            Dominos[20].Value = 8;

            Dominos[21].DominoImage = Image.FromFile(@"images/36.png");
            Dominos[21].Value = 9;

            Dominos[22].DominoImage = Image.FromFile(@"images/44.png");
            Dominos[22].Value = 8;

            Dominos[23].DominoImage = Image.FromFile(@"images/45.png");
            Dominos[23].Value = 9;

            Dominos[24].DominoImage = Image.FromFile(@"images/46.png");
            Dominos[24].Value = 10;

            Dominos[25].DominoImage = Image.FromFile(@"images/55.png");
            Dominos[25].Value = 10;

            Dominos[26].DominoImage = Image.FromFile(@"images/56.png");
            Dominos[26].Value = 11;

            Dominos[27].DominoImage = Image.FromFile(@"images/66.png");
            Dominos[27].Value = 12;
        }

        /// <summary>
        /// Перемешивание домино.
        /// </summary>
        public void RandomizeDominos()
        {
            Random rnd = new Random();

            for (int i = Dominos.Length - 1; i >= 1; i--)
            {
                int j = rnd.Next(i + 1);

                var temp = Dominos[j];
                Dominos[j] = Dominos[i];
                Dominos[i] = temp;
            }
        }

        /// <summary>
        /// Присваивание домино к ячейкам и их отрисовка.
        /// </summary>
        public void StartGame()
        {
            int k = 0;
            for (int x = 0; x < Cells.GetLength(0); x++)
            {
                for (int y = 0; y < Cells.GetLength(1); y++)
                {
                    Cells[x, y].IsActive = (x > y) ? false : true;//Определение активности ячейки

                    if (Cells[x, y].IsActive)//Передача активным ячейкам костей домино
                    {
                        Cells[x, y].PB_Cell.Visible = true;
                        Cells[x, y].PB_Cell.BorderStyle = BorderStyle.None;
                        Cells[x, y].IsSelected = false;

                        Cells[x, y].Domino = Dominos[k];
                        k++;

                        if (y == 0 || y == 6)//Верхние и нижние ячейки отображают свои кости
                        {
                            Cells[x, y].PB_Cell.BackgroundImage = Cells[x, y].Domino.DominoImage;
                            Cells[x, y].IsUnlocked = true;
                        }
                        else//Остальные отображают заглушку
                        {
                            Cells[x, y].PB_Cell.BackgroundImage = Image.FromFile(@"images/DominoBack.png");
                            Cells[x, y].IsUnlocked = false;
                        }
                    }
                }
            }
            //Вывод информации о кол-ве ячеек на форму
            GetCells();
        }
    }
}
