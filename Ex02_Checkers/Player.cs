using System;
using System.Collections.Generic;

namespace Ex02_Checkers
{
    public class Player
    {
        private readonly ePlayerType r_PlayerType;
        private readonly ePieceColor r_PlayerColor;
        private readonly string r_Name;
        private int m_NumberOfSoldierCoins;
        private int m_NumberOfKingCoins;
        private int m_Points;
        private int m_Coins;
        private bool m_HasValidMove;
        private List<string> m_ValidMoves;
        private List<string> m_PiecesLocationOnBoard;

        public Player(string i_PlayerName, ePieceColor i_PlayerColor, ePlayerType i_PlayerType)
        {
            r_Name = i_PlayerName;
            r_PlayerColor = i_PlayerColor;
            r_PlayerType = i_PlayerType;
            m_NumberOfSoldierCoins = 0;
            m_NumberOfKingCoins = 0;
            m_Points = 0;
            m_Coins = 0;
        }

        public ePieceColor PlayerColor
        {
            get
            {
                return r_PlayerColor;
            }
        }

        public List<string> ValidMoves
        {
            get
            {
                return m_ValidMoves;
            }

            set
            {
                m_ValidMoves = value;
            }
        }

        public List<string> PiecesLocationOnBoard
        {
            get
            {
                return m_PiecesLocationOnBoard;
            }

            set
            {
                m_PiecesLocationOnBoard = value;
            }
        }

        public string Name
        {
            get
            {
                return r_Name;
            }
        }

        public ePlayerType PlayerType
        {
            get
            {
                return r_PlayerType;
            }
        }

        public int SoldierCoins
        {
            get
            {
                return m_NumberOfSoldierCoins;
            }

            set
            {
                m_NumberOfSoldierCoins = value;
            }
        }

        public int KingCoins
        {
            get
            {
                return m_NumberOfKingCoins;
            }

            set
            {
                m_NumberOfKingCoins = value;
            }
        }

        public int Coins
        {
            get
            {
                return m_Coins;
            }

            set
            {
                m_Coins = value;
            }
        }

        public int Points
        {
            get
            {
                return m_Points;
            }

            set
            {
                m_Points = value;
            }
        }

        public bool HasValidMove
        {
            get
            {
                return m_HasValidMove;
            }
        }

        public int GetNumberOfPlayerPieces()
        {
            return m_PiecesLocationOnBoard.Count;
        }

        public void SetValidMoves(Board i_Board)
        {
            m_ValidMoves = MoveValidator.GetValidJumpMoveList(i_Board, m_PiecesLocationOnBoard, r_PlayerColor);
            if (m_ValidMoves.Count == 0)
            {
                m_ValidMoves = MoveValidator.GetValidMoveList(i_Board, m_PiecesLocationOnBoard, r_PlayerColor);
            }

            m_HasValidMove = m_ValidMoves.Count != 0;
        }

        public void UpdatePlayerSoldierKingCoins(Board i_Board)
        {
            foreach (string pieceLocation in m_PiecesLocationOnBoard)
            {
                MoveParser.ConvertLocationOnBoardToRowAndColIndexes(pieceLocation, out int row, out int col);
                if (i_Board[col, row] == eSquareStatus.WhiteSoldier || i_Board[col, row] == eSquareStatus.BlackSoldier)
                {
                    m_NumberOfSoldierCoins++;
                }
                else if (i_Board[col, row] == eSquareStatus.WhiteKing || i_Board[col, row] == eSquareStatus.BlackKing)
                {
                    m_NumberOfKingCoins++;
                }
            }
        }

        public void UpdatePlayerCoins(Board i_Board)
        {
            UpdatePlayerSoldierKingCoins(i_Board);
            m_Coins = (m_NumberOfKingCoins * 4) + m_NumberOfSoldierCoins;
        }
    }
}