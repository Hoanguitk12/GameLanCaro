using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameCaroLan
{
    public partial class Form1 : Form
    {
        #region Properties
        ChessBoardMannager CheckBoard1;

#endregion

        public Form1()
        {
            InitializeComponent();
            CheckBoard1 = new ChessBoardMannager(panel1);
            CheckBoard1.CreatChessboard();

        }
    }
}
