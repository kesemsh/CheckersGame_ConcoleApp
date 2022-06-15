using System;
using System.Collections.Generic;
using Ex02_Checkers;

namespace Ex02_CheckersUI
{
    public static class GameUI
    {
        public static void GetAndUpdateGameInformation(GameLogicManager io_Game)
        {
            string firstPlayerName, secondPlayerName;
            ePlayerType firstPlaterType = ePlayerType.Human, secondPlayerType;
            eBoardSizes boardSize;

            firstPlayerName = GetPlayerName(firstPlaterType);
            boardSize = GetBoardSize();
            secondPlayerType = GetPlayerType();
            secondPlayerName = GetPlayerName(secondPlayerType);
            io_Game.InitiateGame(firstPlayerName, secondPlayerName, secondPlayerType, boardSize);
        }

        public static void RunGame()
        {
            bool playAgain = true;
            GameLogicManager checkersLogicManager = new GameLogicManager();

            ShowWelcomeMsg();
            GetAndUpdateGameInformation(checkersLogicManager);
            do
            {
                StartRound(checkersLogicManager);
                playAgain = CheckIfPlayersWantsToPlayAgain();
                if (playAgain)
                {
                    checkersLogicManager.InitRoundWithSameSettings();
                }

            } while (playAgain);

            ShowGoodByeMsg();
        }

        public static void StartRound(GameLogicManager i_CheckersLogicManager)
        {
            eRoundStatus roundStatus = eRoundStatus.NotOver;
            eMoveStatus playerMoveStatus = eMoveStatus.Incomplete;

            BoardUI.DrawBoard(i_CheckersLogicManager.GameBoard);
            while (roundStatus == eRoundStatus.NotOver)
            {
                MakePlayerMove(i_CheckersLogicManager, ref playerMoveStatus);
                i_CheckersLogicManager.CheckPlayerMoveStatusAndUpdateRoundStatus(playerMoveStatus, ref roundStatus);
            }

            i_CheckersLogicManager.CalculateRoundPoints(playerMoveStatus);
            ShowRoundResults(i_CheckersLogicManager.WinnerPlayer, i_CheckersLogicManager.GetOpponentPlayer(i_CheckersLogicManager.WinnerPlayer), roundStatus);
        }

        public static void MakePlayerMove(GameLogicManager i_CheckersLogicManager, ref eMoveStatus io_MoveStatus)
        {
            string playerMove = null;

            if (i_CheckersLogicManager.CurrentPlayer.PlayerType == ePlayerType.Computer)
            {
                io_MoveStatus = i_CheckersLogicManager.MakePlayerMove(ref playerMove);
            }
            else
            {
                playerMove = GetHumanPlayerMove(i_CheckersLogicManager);
                if (InputStringChecker.IsPlayerWantExit(playerMove))
                {
                    io_MoveStatus = eMoveStatus.Exit;
                }
                else
                {
                    io_MoveStatus = i_CheckersLogicManager.MakePlayerMove(ref playerMove);
                }
            }

            BoardUI.DrawBoard(i_CheckersLogicManager.GameBoard);
            ShowCurrentPlayerMove(i_CheckersLogicManager.CurrentPlayer, playerMove);
        }

        public static string GetHumanPlayerMove(GameLogicManager i_CheckersLogicManager)
        {
            ShowCurrentPlayerTurnMsg(i_CheckersLogicManager.CurrentPlayer);

            return GetMoveInputFromHumanPlayer(i_CheckersLogicManager.CurrentPlayer.ValidMoves);
        }

        public static string GetMoveInputFromHumanPlayer(List<string> i_ValidMoves)
        {
            string playerMove;
            bool isPlayerWantToExit;
            bool isPlayerMoveValid = true;

            do
            {
                playerMove = Console.ReadLine();
                isPlayerWantToExit = InputStringChecker.IsPlayerWantExit(playerMove);
                if (!isPlayerWantToExit)
                {
                    isPlayerMoveValid = i_ValidMoves.Contains(playerMove);
                    if (!isPlayerMoveValid)
                    {
                        Console.Write("Illegal move! Please try again: ");
                    }
                }

            } while (!isPlayerMoveValid && !isPlayerWantToExit);

            return playerMove;
        }

        public static string GetPlayerName(ePlayerType i_PlayerType)
        {
            string playerName;

            if (i_PlayerType == ePlayerType.Human)
            {
                Console.Write("Please enter your first name: ");
                playerName = Console.ReadLine();
                while (!InputStringChecker.IsPlayerNameLegal(playerName))
                {
                    Console.Write("invalid player name. Please enter Name again: ");
                    playerName = Console.ReadLine();
                }
            }
            else
            {
                playerName = "Computer";
            }

            return playerName;
        }

        public static ePlayerType GetPlayerType()
        {
            string playerTypeStr;
            bool isPlayerTypeValid;
            ePlayerType playerType;

            Console.Write("Press 1 to play against another person or 2 to play against the computer: ");
            do
            {
                playerTypeStr = Console.ReadLine();
                isPlayerTypeValid = InputStringChecker.IsPlayerTypeLegal(playerTypeStr);
                if (!isPlayerTypeValid)
                {
                    Console.Write("Invalid input. Please enter choice again: ");
                }

            } while (!isPlayerTypeValid);

            if (InputStringChecker.IsPlayerTypeAHuman(playerTypeStr))
            {
                playerType = ePlayerType.Human;
            }
            else
            {
                playerType = ePlayerType.Computer;
            }

            return playerType;
        }

        public static eBoardSizes GetBoardSize()
        {
            bool isBoardSizeValid;
            string stringBoardSize;
            eBoardSizes intBoardSize;

            Console.Write("Please enter a size for the board game [small:6, medium:8, large:10]: ");
            do
            {
                stringBoardSize = Console.ReadLine();
                isBoardSizeValid = InputStringChecker.IsBoardSizeLegal(stringBoardSize, out intBoardSize);
                if (!isBoardSizeValid)
                {
                    Console.Write("Invalid board size. Please enter board size again: ");
                }

            } while (!isBoardSizeValid);

            return intBoardSize;
        }

        public static void ShowCurrentPlayerTurnMsg(Player i_CurrentPlayer)
        {
            Console.Write(string.Format("{0}'s Turn ({1}): ", i_CurrentPlayer.Name, i_CurrentPlayer.PlayerColor));
        }

        public static void ShowCurrentPlayerMove(Player i_CurrentPlayer, string i_PlayerMove)
        {
            Console.WriteLine(string.Format("{0}'s move was ({1}): {2}", i_CurrentPlayer.Name, i_CurrentPlayer.PlayerColor, i_PlayerMove));
        }

        public static bool CheckIfPlayersWantsToPlayAgain()
        {
            string playAgainAnswer;
            bool playAgain = false;
            bool isPlayAgainAnswerValid;

            Console.Write(string.Format("{0}Do you want to play again? {0}Please choose (1) for Yes, Or (2) for No: ", Environment.NewLine));
            do
            {
                playAgainAnswer = Console.ReadLine();
                isPlayAgainAnswerValid = InputStringChecker.IsPlayAgainAnswerLegal(playAgainAnswer);
                if (!isPlayAgainAnswerValid)
                {
                    Console.Write("Invalid input. Please enter choice again: ");
                }

            } while (!isPlayAgainAnswerValid);

            if (InputStringChecker.IsAnswerPlayeAgain(playAgainAnswer))
            {
                playAgain = true;
            }

            return playAgain;
        }

        public static void ShowRoundResults(Player i_Winner, Player i_Loser, eRoundStatus i_RoundStatus)
        {
            Console.Clear();
            Console.WriteLine("ROUND OVER!");
            if (i_RoundStatus == eRoundStatus.Win)
            {
                Console.WriteLine("{0}({1}) WINS the round!", i_Winner.Name, i_Winner.PlayerColor);
            }
            else if (i_RoundStatus == eRoundStatus.Tie)
            {
                Console.WriteLine("No One wins. it's a TIE!");
            }
            else
            {
                Console.WriteLine("{0}({1}) QUIT the round! {2}({3}) WINS the round!", i_Loser.Name, i_Loser.PlayerColor, i_Winner.Name, i_Winner.PlayerColor);
            }

            Console.WriteLine(string.Format("With {0} points to {1}.", i_Winner.Points, i_Winner.Name));
            Console.WriteLine(string.Format("And {0} points to {1}.", i_Loser.Points, i_Loser.Name));
            Console.ReadLine();
        }

        public static void ShowWelcomeMsg()
        {
            Console.WriteLine("==============================");
            Console.WriteLine("= WELCOME TO CHECKERS GAME ! =");
            Console.WriteLine("==============================");
        }

        public static void ShowGoodByeMsg()
        {
            Console.Clear();
            Console.WriteLine("==============");
            Console.WriteLine("= GOOD BYE ! =");
            Console.WriteLine("==============");
            Console.ReadLine();
        }
    }
}