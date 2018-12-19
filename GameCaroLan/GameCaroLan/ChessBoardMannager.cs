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
        public List<List<Button>> Matrix { get => matrix; set => matrix = value; }
        internal Stack<PlayInfo> PlayTimeline { get => playTimeline; set => playTimeline = value; }

        private List<List<Button>> matrix;
        private event EventHandler playerMarked;
        public event EventHandler PlayerMarked
        {
            add
            {
                playerMarked += value;
            }
            remove
            {
                playerMarked -= value;
            }
        }
        private event EventHandler endedGame;
        public event EventHandler EndedGame
        {
            add
            {
                endedGame += value;
            }
            remove
            {
                endedGame -= value;
            }
        }
        private Stack<PlayInfo> playTimeline;


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


           
        }

        #endregion
        
        #region Methods
        public void CreatChessboard()
        {
            ChessBoard.Enabled = true;
            ChessBoard.Controls.Clear();
            PlayTimeline = new Stack<PlayInfo>();
            CurrentPlayer = 0;
            CurrentPlayer = CurrentPlayer == 1 ? 0 : 1;
            Matrix = new List<List<Button>>();
            Button oldButton = new Button() { Width = 0, Location = new Point(0, 0) };
            for (int i = 0; i < Cons.CHESS_BOARD_HEIGHT; i++)
            {
                Matrix.Add(new List<Button>());
                for (int j = 0; j < Cons.CHESS_BOARD_WIDTH; j++)
                {
                   
                    Button btn = new Button()
                    {
                        Width = Cons.CHESS_WIDTH,
                        Height = Cons.CHESS_HEIGHT,
                        Location = new Point(oldButton.Location.X + oldButton.Width, oldButton.Location.Y),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Tag = i.ToString()
                    };

                    btn.Click += btn_Click;

                    ChessBoard.Controls.Add(btn);
                    Matrix[i].Add(btn);

                    oldButton = btn;
                }
                oldButton.Location = new Point(0, oldButton.Location.Y + Cons.CHESS_HEIGHT);
                oldButton.Width = 0;
                oldButton.Height = 0;
            }

        }
        void btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.BackgroundImage != null)
                return;
            btn.BackgroundImage = Player[CurrentPlayer].Mark;
            PlayTimeline.Push(new PlayInfo(GetChessPoint(btn), CurrentPlayer));
            CurrentPlayer = CurrentPlayer == 1 ? 0 : 1;
            PlayerName.Text = Player[CurrentPlayer].Name;
            PlayerMark.Image = Player[CurrentPlayer].Mark;
          
            if (playerMarked != null)
                playerMarked(this, new EventArgs());
            if (IsEndGame(btn))
            {
                EndGame();
            }
           
     
        }
        public void EndGame()
        {
            if (endedGame != null)
                endedGame(this, new EventArgs());
        }
        public bool Undo()
        {
            if (PlayTimeline.Count <= 0)
                return false;
           PlayInfo oldPoint = PlayTimeline.Pop();
            Button btn = Matrix[oldPoint.Point.Y][oldPoint.Point.X];
            btn.BackgroundImage = null;
            if (PlayTimeline.Count <= 0)
                CurrentPlayer = 0;
            else
            {
                oldPoint = PlayTimeline.Peek();
                CurrentPlayer = PlayTimeline.Peek().CurrentPlayer == 1 ? 0 : 1;
            }
            PlayerName.Text = Player[CurrentPlayer].Name;
            PlayerMark.Image = Player[CurrentPlayer].Mark;
            return true;
        }
        private Point GetChessPoint(Button btn)
        {
           
            int vertical = Convert.ToInt32(btn.Tag);
            int horizontal = Matrix[vertical].IndexOf(btn);
            Point point = new Point(horizontal,vertical);
            return point;
        }

        private bool IsEndGame(Button btn)//Hàm kết thúc game
        {
            return IsEndColumn(btn) || IsEndRow(btn) || IsEndFistcross(btn) || IsEndSecondCross(btn);

        }
        private bool IsEndRow(Button btn)//kiểm tra thắng dọc
        {
            Point point = GetChessPoint(btn);
            int countLeft = 0;
            for(int i=point.X; i>=0;i--)
            {
                if (Matrix[point.Y][i].BackgroundImage == btn.BackgroundImage)
                {
                    countLeft++;
                }
                else
                    break;

            }
            int countRight = 0;
            for (int i = point.X+1; i <Cons.CHESS_BOARD_WIDTH; i++)
            {
                if (Matrix[point.Y][i].BackgroundImage == btn.BackgroundImage)
                {
                    countRight++;
                }
                else
                    break;

            }
            return countLeft + countRight == 5;


        }
        private bool IsEndColumn(Button btn)//Kiểm tra thắng ngang
        {
            Point point = GetChessPoint(btn);
            int countTop = 0;
            for (int i = point.Y; i >= 0; i--)
            {
                if (Matrix[i][point.X].BackgroundImage == btn.BackgroundImage)
                {
                    countTop++;
                }
                else
                    break;

            }
            int countBottom = 0;
            for (int i = point.Y + 1; i < Cons.CHESS_BOARD_HEIGHT; i++)
            {
                if (Matrix[i][point.X].BackgroundImage == btn.BackgroundImage)
                {
                    countBottom++;
                }
                else
                    break;

            }
            return countTop + countBottom == 5;

        }
        private bool IsEndFistcross(Button btn)//kiểm tra chéo chính
        {
            Point point = GetChessPoint(btn);
            int countTop = 0;
            for (int i = 0; i <= point.X; i++)
            {
                if (point.Y - i < 0 || point.X - i < 0)//kiểm tra có ra khỏi viền không?
                    break;
                if (Matrix[point.Y-i][point.X-i].BackgroundImage == btn.BackgroundImage)
                {
                    countTop++;
                }
                else
                    break;

            }
            int countBottom = 0;
            for (int i = 1; i <=Cons.CHESS_BOARD_WIDTH-point.X; i++)
            {
                if (point.Y + i >= Cons.CHESS_BOARD_HEIGHT || point.X + i >= Cons.CHESS_BOARD_WIDTH)//kiểm tra có ra khỏi viền không?
                    break;
                if (Matrix[point.Y+i][point.X+i].BackgroundImage == btn.BackgroundImage)
                {
                    countBottom++;
                }
                else
                    break;

            }
            return countTop + countBottom == 5;

        }
        private bool IsEndSecondCross(Button btn)//kiểm tra chéo phụ
        {
            Point point = GetChessPoint(btn);
            int countTop = 0;
            for (int i = 0; i <= point.X; i++)
            {
                if (point.X + i >= Cons.CHESS_BOARD_WIDTH || point.Y - i < 0)//kiểm tra có ra khỏi viền không?
                    break;
                if (Matrix[point.Y-i][point.X+i].BackgroundImage == btn.BackgroundImage)
                {
                    countTop++;
                }
                else
                    break;

            }
            int countBottom = 0;
            for (int i = 1; i <= Cons.CHESS_BOARD_WIDTH-point.X; i++)
            {
                if (point.Y + i >= Cons.CHESS_BOARD_HEIGHT || point.X - i <0)//kiểm tra có ra khỏi viền không?
                    break;
                if (Matrix[point.Y + i][point.X - i].BackgroundImage == btn.BackgroundImage)
                {
                    countBottom++;
                }
                else
                    break;

            }
            return countTop + countBottom == 5;

        }
        #endregion



    }
}
