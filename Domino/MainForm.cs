﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DominoPyramidLib;

namespace Domino
{
    public partial class MainForm : Form
    {
        Game MyGame = new Game();

        public MainForm()
        {
            InitializeComponent();

            //Передача контролов в класс
            MyGame.CellsLabel = label1;
            MyGame.PrB = progressBar1;

            //Инициализация объектов и их отрисовка на форме
            MyGame.InitializeCells();
            MyGame.PaintCell(panel1);
            MyGame.InitializeDominos();

            //Начало игры
            MyGame.RandomizeDominos();
            MyGame.StartGame();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Новая игра
            MyGame.RandomizeDominos();
            MyGame.StartGame();
        }
    }
}
