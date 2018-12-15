using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameCaroLan
{
    public class ChessBoardMannager
    {
       
        #region Properties
        private Panel ChessBoard;

        public Panel ChessBoard1 { get => ChessBoard; set => ChessBoard = value; }
        

        private List<Player> player;
        internal List<Player> Player { get => player; set => player = value; }
       
       
      
        private int currentPlayer;
        public int CurrentPlayer { get => currentPlayer; set => currentPlayer = value; }
       private TextBox playerName;
     
        
        private PictureBox playerMark;
        public PictureBox PlayerMark { get => playerMark; set => playerMark = value; }
        public TextBox PlayerName { get => playerName; set => playerName = value; }

        #endregion

        #region Initalize
        public ChessBoardMannager(Panel chessBoard,TextBox playerName,PictureBox mark)
        {
            this.ChessBoard1 = chessBoard;
            this.PlayerName = playerName;
            this.PlayerMark = mark;
            this.player = new List<GameCaroLan.Player>()
            {
                new Player("Player 1",Image.FromFile(Application.StartupPath + "//Resources//XX.png")),
                new Player("Player 2",Image.FromFile(Application.StartupPath + "//Resources//chu 0.png"))
            };
            CurrentPlayer = 0;

        }

        #endregion
        
        #region Methods
        public void CreatChessboard()
        {
            int n = 20;
            var ButtonArray = new Button[n * n];
            int size = ChessBoard1.Size.Width / n;
            for (int i = 0; i < n * n; i++)
            {
                ButtonArray[i] = new Button();
                ButtonArray[i].Click += btn_Click;
                ButtonArray[i].Size = new Size(size, size);
                int x = i % n;
                int y = i / n;
                ButtonArray[i].Location = new Point(y * size, x * size);
                ButtonArray[i].BackgroundImageLayout = ImageLayout.Stretch;
                ChessBoard1.Controls.Add(ButtonArray[i]);//thềm button bào pannel
            }
        }
        void btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.BackgroundImage != null)
                return;
            btn.BackgroundImage = Player[CurrentPlayer].Mark;
            CurrentPlayer = CurrentPlayer == 1 ? 0 : 1;
            PlayerName.Text = Player[CurrentPlayer].Name;
            PlayerMark.Image = Player[CurrentPlayer].Mark;
        }
        #endregion


    }
}
