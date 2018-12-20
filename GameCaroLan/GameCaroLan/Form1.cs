using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameCaroLan
{
    public partial class Form1 : Form
    {
        #region Properties
        ChessBoardMannager CheckBoard1;
        SocKetManager socket;

#endregion

        public Form1()
        {
            InitializeComponent();
            CheckBoard1 = new ChessBoardMannager(panel1, textBox1,pictureBox1);
            socket = new SocKetManager();

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
            undoToolStripMenuItem.Enabled = false;
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
            undoToolStripMenuItem.Enabled = true;
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
            CheckBoard1.Undo();
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

        private void button1_Click(object sender, EventArgs e)
        {
            socket.IP = textBox2.Text;
            if(!socket.ConnectServer())
            {
                socket.CreateServer();
                Thread listenThread = new Thread(() =>
                {
                    while (true)
                    {
                        Thread.Sleep(500);
                        try
                        {
                            listen();
                            break;
                        }
                        catch
                        {

                        }
                    }
                });
                listenThread.IsBackground = true;
                listenThread.Start();
                socket.Send("thông tin từ client");
            }
            else
            {
                Thread listenThread = new Thread(() =>
                 {
                     listen();
                 });
                listenThread.IsBackground = true;
                listenThread.Start();
                socket.Send("thông tin từ client");
            }
        }

        void listen()
        {
            string data = (string)socket.Receive();
            MessageBox.Show(data);
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            textBox2.Text = socket.GetLocalIPv4(NetworkInterfaceType.Wireless80211);
            if(string.IsNullOrEmpty(button1.Text))
            {
                textBox2.Text = socket.GetLocalIPv4(NetworkInterfaceType.Ethernet);
            }

          
        }
    }
}
