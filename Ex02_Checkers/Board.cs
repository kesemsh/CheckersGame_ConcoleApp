using System;

namespace Ex02_Checkers
{
    public class Board
    {
        private readonly eSquareStatus[,] r_SquareBoard2DArray;
        private eBoardSizes m_BoardSize;

        public Board(eBoardSizes i_BoardSize)
        {
            m_BoardSize = i_BoardSize;
            r_SquareBoard2DArray = new eSquareStatus[BoardSize, BoardSize];
            OrganizeBoard();
        }

        public int BoardSize
        {
            get
            {
                return (int)m_BoardSize;
            }

            set 
            {
                m_BoardSize = (eBoardSizes)value;
            }
        }

        public eSquareStatus this[int i_ColNum, int i_RowNum]
        {
            get 
            { 
                return r_SquareBoard2DArray[i_ColNum, i_RowNum]; 
            }

            set 
            { 
                r_SquareBoard2DArray[i_ColNum, i_RowNum] = value; 
            }
        }

        public void OrganizeBoard()
        {
            organizeSolidersOnBoard(ePieceColor.White_O);
            organizeTwoClearRows();
            organizeSolidersOnBoard(ePieceColor.Black_X);
        }

        private void organizeSolidersOnBoard(ePieceColor i_SoliderColor)
        {
            int row, col;
            bool isColorSoliderWhite = i_SoliderColor == ePieceColor.White_O;
            bool isWhiteSlotOnBoard = true;

            for (row = isColorSoliderWhite ? 0 : BoardSize - 1; isColorSoliderWhite ? row < (BoardSize / 2) - 1 : row > BoardSize / 2; row = isColorSoliderWhite ? row + 1 : row - 1)
            {
                for (col = isColorSoliderWhite ? 0 : BoardSize - 1; isColorSoliderWhite ? col < BoardSize : col >= 0; col = isColorSoliderWhite ? col + 1 : col - 1)
                {
                    if (isWhiteSlotOnBoard)
                    {
                        r_SquareBoard2DArray[col, row] = eSquareStatus.Illegal;
                        isWhiteSlotOnBoard = false;
                    }
                    else
                    {
                        r_SquareBoard2DArray[col, row] = isColorSoliderWhite ? eSquareStatus.WhiteSoldier : eSquareStatus.BlackSoldier;
                        isWhiteSlotOnBoard = true;
                    }
                }

                isWhiteSlotOnBoard = !isWhiteSlotOnBoard;
            }
        }

        private void organizeTwoClearRows()
        {
            bool isWhiteSlotOnBoard = false;

            if (this[0, (BoardSize / 2) - 2] != eSquareStatus.Illegal)
            {
                isWhiteSlotOnBoard = true;
            }

            for (int row = (BoardSize / 2) - 1; row < (BoardSize / 2) + 2; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    r_SquareBoard2DArray[col, row] = isWhiteSlotOnBoard ? eSquareStatus.Illegal : eSquareStatus.Clear;
                    isWhiteSlotOnBoard = !isWhiteSlotOnBoard;
                }

                isWhiteSlotOnBoard = !isWhiteSlotOnBoard;
            }
        }

        public bool IsSquareClearAndInBoard(int i_Col, int i_Row)
        {
            return IsSquareInBoard(i_Col, i_Row) && this[i_Col, i_Row] == eSquareStatus.Clear;
        }

        public bool IsSquareInBoard(int i_Col, int i_Row)
        {
            return (i_Col >= 0 && i_Col < BoardSize) && (i_Row >= 0 && i_Row < BoardSize);
        }

        public void UpdateMoveOnBoard(Player i_CurrentPlayer, int i_FromMoveCol, int i_FromMoveRow, int i_DestMoveCol, int i_DestMoveRow)
        {
            if(i_CurrentPlayer.PlayerColor == ePieceColor.Black_X)
            {
                if(this[i_FromMoveCol, i_FromMoveRow] == eSquareStatus.BlackSoldier)
                {
                    this[i_DestMoveCol, i_DestMoveRow] = eSquareStatus.BlackSoldier;
                }
                else
                {
                    this[i_DestMoveCol, i_DestMoveRow] = eSquareStatus.BlackKing;
                }
            }
            else
            {
                if (this[i_FromMoveCol, i_FromMoveRow] == eSquareStatus.WhiteSoldier)
                {
                    this[i_DestMoveCol, i_DestMoveRow] = eSquareStatus.WhiteSoldier;
                }
                else
                {
                    this[i_DestMoveCol, i_DestMoveRow] = eSquareStatus.WhiteKing;
                }
            }

            this[i_FromMoveCol, i_FromMoveRow] = eSquareStatus.Clear;
        }

        public void MakeSoliderToKing(int i_NewKingCol, int i_NewKingRow)
        {
            if (this[i_NewKingCol, i_NewKingRow] == eSquareStatus.WhiteSoldier)
            {
                this[i_NewKingCol, i_NewKingRow] = eSquareStatus.WhiteKing;
            }
            else if (this[i_NewKingCol, i_NewKingRow] == eSquareStatus.BlackSoldier)
            {
                this[i_NewKingCol, i_NewKingRow] = eSquareStatus.BlackKing;
            }
        }

        public void CheckAndUpdateIfKingWasMade(Player i_CurrentPlayer, int i_DestCol, int i_DestRow)
        {
            if (i_CurrentPlayer.PlayerColor == ePieceColor.Black_X)
            {
                if (i_DestRow == 0)
                {
                    MakeSoliderToKing(i_DestCol, i_DestRow);
                }
            }
            else
            {
                if (i_DestRow == BoardSize - 1)
                {
                    MakeSoliderToKing(i_DestCol, i_DestRow);
                }
            }
        }
    }
}
