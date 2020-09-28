using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

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
        Domino[] Dominos = new Domino[28];

        /// <summary>
        /// Инициализация ячеек
        /// </summary>
        public void InitializeCells()
        {
            for (int x = 0; x < Cells.GetLength(0); x++)
            {
                for (int y = 0; y < Cells.GetLength(1); y++)
                {
                    Cells[x, y] = new Cell();
                    Cells[x, y].PB_Cell = new PictureBox();

                    Cells[x, y].IsActive = (x > y) ? false : true;
                }
            }
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
                        Cells[x, y].PB_Cell.BackColor = Color.Red;
                        Cells[x, y].PB_Cell.BorderStyle = BorderStyle.FixedSingle;
                        Cells[x, y].PB_Cell.BackgroundImage = Image.FromFile(@"images/GitHub.png");

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
            public PictureBox PB_Cell { get; set; }

            public bool IsActive { get; set; }
        }

        public class Domino
        {
            int value;
        }
    }
}
