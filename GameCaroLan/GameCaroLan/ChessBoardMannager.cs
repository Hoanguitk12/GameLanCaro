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


        #endregion
        #region Initalize
        public ChessBoardMannager(Panel chessBoard)
        {
            this.ChessBoard1 = chessBoard;
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
                ButtonArray[i].Size = new Size(size, size);
                int x = i % n;
                int y = i / n;
                ButtonArray[i].Location = new Point(y * size, x * size);
                ChessBoard1.Controls.Add(ButtonArray[i]);//thềm button bào pannel
            }
        }
        #endregion


    }
}
