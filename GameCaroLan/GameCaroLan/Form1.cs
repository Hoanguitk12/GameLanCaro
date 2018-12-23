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
using static GameCaroLan.ChessBoardMannager;
using static GameCaroLan.SocketData;

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
            Control.CheckForIllegalCrossThreadCalls = false;
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
           
        }
        private void ChessBoard_PlayerMarked(object sender, ButtonClickEvent e)
        {
           timer1.Start();
            panel1.Enabled = false;
            progressBar1.Value = 0;
            socket.Send(new SocketData( (int)SocketCommand.SEND_POINT, "", e.ClickedPoint));
            undoToolStripMenuItem.Enabled = false;
            listen();
        }

        private void ChessBoard_EndedGame(object sender, EventArgs e)
        {
            EndGame();
            socket.Send(new SocketData((int)SocketCommand.END_GAME, "", new Point()));

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.PerformStep();
            if (progressBar1.Value >= progressBar1.Maximum)
            {
                EndGame();
                socket.Send(new SocketData((int)SocketCommand.TIME_OUT, "", new Point()));
            }
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
            progressBar1.Value = 0;
        
           
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
            socket.Send(new SocketData((int)SocketCommand.NEW_GAME, "", new Point()));
            panel1.Enabled = true;
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Undo();
            socket.Send(new SocketData((int)SocketCommand.UNDO, "", new Point()));

        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Quit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn thoát", "Thông Báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.OK)
                e.Cancel = true;
            else
                try
                {
                    socket.Send(new SocketData((int)SocketCommand.QUIT, "", new Point()));
                }
                catch { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            socket.IP = textBox2.Text;
            if(!socket.ConnectServer())
            {
                socket.CreateServer();
                socket.isServer = true;
                panel1.Enabled = true;
                
               
            }
            else
            {
                socket.isServer = false;
                panel1.Enabled = false;
                listen();
                
            }
        }
        private void ProcessData(SocketData data)
        {
            switch(data.Command)
            {
                case ((int)SocketCommand.NOTIFY):
                    MessageBox.Show(data.Message);
                    break;
                case ((int)SocketCommand.SEND_POINT):
                    this.Invoke((MethodInvoker)(() =>
                    {
                        progressBar1.Value = 0;
                        panel1.Enabled = true;
                        timer1.Start();
                        CheckBoard1.OtherPlayerMarked(data.Point);
                       
                        undoToolStripMenuItem.Enabled = true;
                    }));
                   
                    break;
                case ((int)SocketCommand.QUIT):
                    timer1.Stop();
                    MessageBox.Show("Người chơi đã thoát");
                    break;
                case ((int)SocketCommand.UNDO):
                    Undo();
                    progressBar1.Value = 0;
                   
                    break;

                case ((int)SocketCommand.END_GAME):
                    MessageBox.Show("5 con trên một hàng");
                    break;
                case ((int)SocketCommand.TIME_OUT):
                    MessageBox.Show("hết giờ");
                    break;
                case ((int)SocketCommand.NEW_GAME):
                    this.Invoke((MethodInvoker)(() =>
                    {
                        NewGame();
                        panel1.Enabled = false;
                    }));
                    break;
                default:
                    break;

            }
            listen();
        }
        void listen()
        {
            Thread listenThread = new Thread(() =>
            {
                try
                {
                    SocketData data = (SocketData)socket.Receive();

                    ProcessData(data);
                }
                catch (Exception e)
                {
                }
            });
            listenThread.IsBackground = true;
            listenThread.Start();
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
