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


        public class PB : PictureBox
        {
            public int X { get; set; }
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



                    //Передача координат элемента массива в его параметры.
                    Cells[x, y].X = x;
                    Cells[x, y].Y = y;
                }
            }
        }

        /// <summary>
        /// Событие нажатия на PictureBox
        /// </summary>
        public void CellClick(object sender, EventArgs e)
        {
            MessageBox.Show("Сообщение - " + Convert.ToString(Cells[1, 2].X) + " " + Convert.ToString(Cells[1, 2].Y));

            PB cell = sender as PB;
            
            cell.X = 10;

            MessageBox.Show(Convert.ToString(cell.X));
        }

        /// <summary>
        /// Отрисовка ячеек
        /// </summary>
        /// <param name="PaintForm"> Форма для отрисовки</param>
        public void PaintCell(Panel PaintForm)
        {
            int AligmentOffset = 210;
            
            int HotizontalGap;
            int VerticalGap = 0;
            
            for (int y = 0; y < Cells.GetLength(0); y++)
            {
                HotizontalGap = 0;
                for (int x = 0; x < Cells.GetLength(1); x++)
                {
                    if (Cells[x, y].IsActive)
                    {
                        PaintForm.Controls.Add(Cells[x, y].PB_Cell);

                        Cells[x, y].PB_Cell.Height = 30;
                        Cells[x, y].PB_Cell.Width = 60;
                        Cells[x, y].PB_Cell.BackColor = Color.Transparent;
                        Cells[x, y].PB_Cell.BorderStyle = BorderStyle.None;
                        Cells[x, y].PB_Cell.BackgroundImage = Image.FromFile(@"images/06.png");
                        Cells[x, y].PB_Cell.BackgroundImageLayout = ImageLayout.Stretch;

                        Cells[x, y].PB_Cell.Location = new Point((60 * x) + AligmentOffset + HotizontalGap, (30 * y) + VerticalGap);

                        HotizontalGap += 3;
                        
                    }
                }
                VerticalGap += 3;
                AligmentOffset -= 30;
            }

        }

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


            public bool IsSelected { get; set; }
            /// <summary>
            /// Кость домино, привязанная к ячейке.
            /// </summary>
            public Domino Domino { get; set; }

            /// <summary>
            /// Указывает открыта ли ячейка.
            /// </summary>
            public bool IsUnlocked { get; set; }

            /// <summary>
            /// Координата X.
            /// </summary>
            public int X { get; set; }
            /// <summary>
            /// Координата Y.
            /// </summary>
            public int Y { get; set; }           

            public void CellSelection()
            {
                IsSelected = true;

                PB_Cell.BorderStyle = BorderStyle.Fixed3D;
            }
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
                    if (Cells[x, y].IsActive)
                    {
                        Cells[x, y].Domino = Dominos[k];
                        k++;

                        if (y == 0 || y == 6)
                        {
                            Cells[x, y].PB_Cell.BackgroundImage = Cells[x, y].Domino.DominoImage;
                            Cells[x, y].IsUnlocked = true;
                        }
                        else
                        {
                            Cells[x, y].PB_Cell.BackgroundImage = Image.FromFile(@"images/DominoBack.png");
                            Cells[x, y].IsUnlocked = false;
                        }
                    }
                }
            }
        }


    }
}
