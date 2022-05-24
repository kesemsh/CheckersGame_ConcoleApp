using System;

namespace Ex02_Checkers
{
    public static class MoveParser
    {
        private const char k_BeginCol = 'A';
        private const char k_BeginRow = 'a';

        public static string GetFromLocation(string i_PlayerMove)
        {
            return i_PlayerMove.Substring(0, 2);
        }

        public static string GetDestinationLocation(string i_PlayerMove)
        {
            return i_PlayerMove.Substring(3, 2);
        }

        public static void GetLocationIndexes(string i_PlayerMove, out int o_Row, out int o_Col)
        {
            int indexCol = 0;
            int indexRow = 1;
            
            o_Col = i_PlayerMove[indexCol] - k_BeginCol;
            o_Row = i_PlayerMove[indexRow] - k_BeginRow;
        }

        public static string ConvertIndexesLocationToLocationStr(int i_Row, int i_Col)
        {
            char row = (char)(k_BeginRow + i_Row);
            char col = (char)(k_BeginCol + i_Col);

            return col.ToString() + row.ToString();
        }

        public static void ConvertLocationOnBoardToRowAndColIndexes(string i_LocationOnBoard, out int o_Row, out int o_Col)
        {
            o_Col = i_LocationOnBoard[0] - k_BeginCol;
            o_Row = i_LocationOnBoard[1] - k_BeginRow;
        }

        public static string GetMoveFormat(string i_FromMove, string i_DestMove)
        {
            return string.Format("{0}>{1}", i_FromMove, i_DestMove);
        }

        public static void GetFromAndDestLocationIndexes(string i_PlayerMove, out int o_FromMoveCol, out int o_FromMoveRow, out int o_DestMoveCol, out int o_DestMoveRow)
        {
            string fromPlayerMove = GetFromLocation(i_PlayerMove);
            string destPlayerMove = GetDestinationLocation(i_PlayerMove);

            GetLocationIndexes(fromPlayerMove, out o_FromMoveRow, out o_FromMoveCol);
            GetLocationIndexes(destPlayerMove, out o_DestMoveRow, out o_DestMoveCol);
        }
    }
}
