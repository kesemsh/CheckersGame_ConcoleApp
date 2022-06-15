using System;
using System.Collections.Generic;

namespace Ex02_Checkers
{
    public static class MoveValidator
    {
        public static void InitPiecesLocationList(out List<string> o_PiecesLocationNewList, eBoardSizes i_BoardSize, ePieceColor i_PlayerColor)
        {
            int boardSize = (int)i_BoardSize;
            bool isPlayerWhitePieces = i_PlayerColor == ePieceColor.White_O ? true : false;

            o_PiecesLocationNewList = new List<string>();
            for (int row = isPlayerWhitePieces ? 0 : boardSize - 1; isPlayerWhitePieces ? row < (boardSize / 2) - 1 : row > (boardSize / 2); row += isPlayerWhitePieces ? 1 : -1)
            {
                for (int col = (row % 2 == 0) ? 1 : 0; col < boardSize; col += 2)
                {
                    o_PiecesLocationNewList.Add(MoveParser.ConvertIndexesLocationToLocationStr(row, col));
                }
            }
        }

        public static List<string> GetNewPiecesLocationList(eBoardSizes i_BoardSize, ePieceColor i_PlayerColor)
        {
            List<string> newList;

            InitPiecesLocationList(out newList, i_BoardSize, i_PlayerColor);

            return newList;
        }

        public static List<string> GetValidMoveList(Board i_Board, List<string> i_PiecesLocationNewList, ePieceColor i_PlayerColor)
        {
            bool isKing;
            int directionIndicatorByColor = i_PlayerColor == ePieceColor.White_O ? 1 : -1;
            List<string> newValidMoveList = new List<string>();

            foreach (string pieceLocation in i_PiecesLocationNewList)
            {
                MoveParser.ConvertLocationOnBoardToRowAndColIndexes(pieceLocation, out int row, out int col);
                isKing = i_Board[col, row] == eSquareStatus.WhiteKing || i_Board[col, row] == eSquareStatus.BlackKing;
                if (i_Board.IsSquareClearAndInBoard(col - directionIndicatorByColor, row + directionIndicatorByColor))
                {
                    newValidMoveList.Add(MoveParser.GetMoveFormat(pieceLocation, MoveParser.ConvertIndexesLocationToLocationStr(row + directionIndicatorByColor, col - directionIndicatorByColor)));
                }

                if (i_Board.IsSquareClearAndInBoard(col + directionIndicatorByColor, row + directionIndicatorByColor))
                {
                    newValidMoveList.Add(MoveParser.GetMoveFormat(pieceLocation, MoveParser.ConvertIndexesLocationToLocationStr(row + directionIndicatorByColor, col + directionIndicatorByColor)));
                }

                if (isKing)
                {
                    if (i_Board.IsSquareClearAndInBoard(col - directionIndicatorByColor, row - directionIndicatorByColor))
                    {
                        newValidMoveList.Add(MoveParser.GetMoveFormat(pieceLocation, MoveParser.ConvertIndexesLocationToLocationStr(row - directionIndicatorByColor, col - directionIndicatorByColor)));
                    }

                    if (i_Board.IsSquareClearAndInBoard(col + directionIndicatorByColor, row - directionIndicatorByColor))
                    {
                        newValidMoveList.Add(MoveParser.GetMoveFormat(pieceLocation, MoveParser.ConvertIndexesLocationToLocationStr(row - directionIndicatorByColor, col + directionIndicatorByColor)));
                    }
                }
            }

            return newValidMoveList;
        }

        public static bool UpdatePiecesLocationList(Player io_CurrentPlayer, string i_PlayerMove)
        {
            bool isUpdateOccurred = false;

            if (io_CurrentPlayer.PiecesLocationOnBoard.Contains(MoveParser.GetFromLocation(i_PlayerMove)))
            {
                io_CurrentPlayer.PiecesLocationOnBoard.Remove(MoveParser.GetFromLocation(i_PlayerMove));
                io_CurrentPlayer.PiecesLocationOnBoard.Add(MoveParser.GetDestinationLocation(i_PlayerMove));
                isUpdateOccurred = true;
            }

            return isUpdateOccurred;
        }

        public static void UpdateOpponentPiecesLocationList(Player io_OpponentPlayer, int i_EatenPieceCol, int i_EatenPieceRow)
        {
            string eatenPieceLocation = MoveParser.ConvertIndexesLocationToLocationStr(i_EatenPieceRow, i_EatenPieceCol);

            io_OpponentPlayer.PiecesLocationOnBoard.Remove(eatenPieceLocation);
        }

        public static List<string> GetValidJumpMoveList(Board i_Board, List<string> i_PiecesLocationList, ePieceColor i_Color)
        {
            bool isKing;
            int directionIndicator = i_Color == ePieceColor.White_O ? 2 : -2;
            int directionIndicatorStepBefore = i_Color == ePieceColor.White_O ? -1 : 1;
            List<string> newValidJumpMoveList = new List<string>();

            foreach (string pieceLocation in i_PiecesLocationList)
            {
                MoveParser.ConvertLocationOnBoardToRowAndColIndexes(pieceLocation, out int row, out int col);
                isKing = i_Board[col, row] == eSquareStatus.WhiteKing || i_Board[col, row] == eSquareStatus.BlackKing;
                if (IsPossibleJumpOverEnemy(i_Board, col - directionIndicator, row + directionIndicator, col - directionIndicator - directionIndicatorStepBefore, row + directionIndicator + directionIndicatorStepBefore, i_Color))
                {
                    newValidJumpMoveList.Add(MoveParser.GetMoveFormat(pieceLocation, MoveParser.ConvertIndexesLocationToLocationStr(row + directionIndicator, col - directionIndicator)));
                }

                if (IsPossibleJumpOverEnemy(i_Board, col + directionIndicator, row + directionIndicator, col + directionIndicator + directionIndicatorStepBefore, row + directionIndicator + directionIndicatorStepBefore, i_Color))
                {
                    newValidJumpMoveList.Add(MoveParser.GetMoveFormat(pieceLocation, MoveParser.ConvertIndexesLocationToLocationStr(row + directionIndicator, col + directionIndicator)));
                }

                if (isKing)
                {
                    if (IsPossibleJumpOverEnemy(i_Board, col - directionIndicator, row - directionIndicator, col - directionIndicator - directionIndicatorStepBefore, row - directionIndicator - directionIndicatorStepBefore, i_Color))
                    {
                        newValidJumpMoveList.Add(MoveParser.GetMoveFormat(pieceLocation, MoveParser.ConvertIndexesLocationToLocationStr(row - directionIndicator, col - directionIndicator)));
                    }

                    if (IsPossibleJumpOverEnemy(i_Board, col + directionIndicator, row - directionIndicator, col + directionIndicator + directionIndicatorStepBefore, row - directionIndicator - directionIndicatorStepBefore, i_Color))
                    {
                        newValidJumpMoveList.Add(MoveParser.GetMoveFormat(pieceLocation, MoveParser.ConvertIndexesLocationToLocationStr(row - directionIndicator, col + directionIndicator)));
                    }
                }
            }

            return newValidJumpMoveList;
        }

        public static bool IsPossibleJumpOverEnemy(Board i_Board, int i_ColDest, int i_RowDest, int i_ColOppositeColor, int i_RowOppositeColor, ePieceColor i_PlayerColor)
        {
            bool isOppositeColor = false;

            if (i_Board.IsSquareClearAndInBoard(i_ColDest, i_RowDest) && i_Board.IsSquareInBoard(i_ColOppositeColor, i_RowOppositeColor))
            {
                if (i_Board[i_ColOppositeColor, i_RowOppositeColor] == eSquareStatus.WhiteKing || i_Board[i_ColOppositeColor, i_RowOppositeColor] == eSquareStatus.WhiteSoldier)
                {
                    isOppositeColor = i_PlayerColor != ePieceColor.White_O;
                }
                else if (i_Board[i_ColOppositeColor, i_RowOppositeColor] == eSquareStatus.BlackKing || i_Board[i_ColOppositeColor, i_RowOppositeColor] == eSquareStatus.BlackSoldier)
                {
                    isOppositeColor = i_PlayerColor != ePieceColor.Black_X;
                }
                else
                {
                    isOppositeColor = false;
                }
            }

            return isOppositeColor;
        }

        public static bool WasJumpMove(int i_FromMoveCol, int i_FromMoveRow, int i_DestMoveCol, int i_DestMoveRow)
        {
            return Math.Abs(i_FromMoveCol - i_DestMoveCol) == 2 && Math.Abs(i_FromMoveRow - i_DestMoveRow) == 2;
        }
    }
}