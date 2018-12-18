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
            CheckBoard1 = new ChessBoardMannager(panel1, textBox1,pictureBox1);
            CheckBoard1.EndedGame += ChessBoard_EndedGame;
            CheckBoard1.PlayerMarked += ChessBoard_PlayerMarked;
          
            progressBar1.Step = Cons.COOL_DOWN_STEP;
            progressBar1.Maximum = Cons.COOL_DOWN_TIME;
            progressBar1.Value = 0;
            timer1.Interval = Cons.COOL_DOWN_INERVAl;
            NewGame();
           
        }
        void EndGame()
        {
            timer1.Stop();
            panel1.Enabled = false; 
            MessageBox.Show("kết thúc");
        }
        private void ChessBoard_PlayerMarked(object sender, EventArgs e)
        {
            timer1.Start();
            progressBar1.Value = 0;
        }

        private void ChessBoard_EndedGame(object sender, EventArgs e)
        {
            EndGame();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.PerformStep();
            if (progressBar1.Value >= progressBar1.Maximum)
                EndGame();
        }
        void NewGame()
        {
            timer1.Stop();
            progressBar1.Value = 0;
            CheckBoard1.CreatChessboard();
        }
        void Quit()
        {
            
                Application.Exit();
        }
        void Undo()
        {

        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Quit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn thoát", "Thông Báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.OK)
                e.Cancel=true;
        }
    }
}
