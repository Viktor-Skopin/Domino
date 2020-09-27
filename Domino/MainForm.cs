using System;
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
        public MainForm()
        {
            InitializeComponent();

            Game MyGame = new Game();

            MyGame.InitializeCells();
            MyGame.PaintCell(panel1);
        }

        

    }
}
