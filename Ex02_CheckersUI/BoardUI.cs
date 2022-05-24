using System;
using Ex02.ConsoleUtils;
 
namespace Ex02_CheckersUI
{
    public class BoardUI
    {
        private const char k_ColLetter = 'A';
        private const char k_Rowletter = 'a';

        public static void DrawBoard(Ex02_Checkers.Board i_Board)
        {
            Screen.Clear();
            Console.Write("  ");
            for (int i = 0; i < i_Board.BoardSize; i++)
            {
                Console.Write(string.Format(" {0}  ", (char)(i + k_ColLetter)));
            }

            printSeparatingLine(i_Board.BoardSize);
            for (int indexRow = 0; indexRow < i_Board.BoardSize; indexRow++)
            {
                Console.Write(string.Format("{0}|", (char)(indexRow + k_Rowletter)));
                for (int indexCol = 0; indexCol < i_Board.BoardSize; indexCol++)
                {
                    Console.Write(string.Format(" {0} |", printCharToSlot(i_Board[indexCol, indexRow])));
                }

                printSeparatingLine(i_Board.BoardSize);
            }
        }

        private static void printSeparatingLine(int i_Length)
        {
            Console.WriteLine();
            Console.Write(" =");
            for (int i = 0; i < i_Length; i++)
            {
                Console.Write("====");
            }

            Console.WriteLine();
        }

        private static char printCharToSlot(Ex02_Checkers.eSquareStatus i_SlotStatus)
        {
            char charSlotStatus = ' ';

            switch (i_SlotStatus)
            {
                case Ex02_Checkers.eSquareStatus.Clear:
                    charSlotStatus = ' ';
                    break;
                case Ex02_Checkers.eSquareStatus.WhiteSoldier:
                    charSlotStatus = 'O';
                    break;
                case Ex02_Checkers.eSquareStatus.BlackSoldier:
                    charSlotStatus = 'X';
                    break;
                case Ex02_Checkers.eSquareStatus.WhiteKing:
                    charSlotStatus = 'U';
                    break;
                case Ex02_Checkers.eSquareStatus.BlackKing:
                    charSlotStatus = 'K';
                    break;
            }

            return charSlotStatus;
        }
    }
}
