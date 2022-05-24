using System;
using System.Collections.Generic;

namespace Ex02_Checkers
{
    public class GameLogicManager
    {
        private Board m_Board;
        private Player m_BlackPlayer;
        private Player m_WhitePlayer;
        private Player m_CurrentPlayer;
        private Player m_WinnerPlayer;

        public Board GameBoard
        {
            get
            {
                return m_Board; 
            }
        }

        public Player FirstPlayer
        {
            get
            { 
                return m_BlackPlayer;
            }
        }

        public Player SecondPlayer
        {
            get 
            { 
                return m_WhitePlayer;
            }
        }

        public Player CurrentPlayer
        {
            get
            {
                return m_CurrentPlayer;
            }
        }

        public Player WinnerPlayer
        {
            get 
            {
                return m_WinnerPlayer; 
            }
        }

        public void InitiateGame(string i_FirstPlayerName, string i_SecondPlayerName, ePlayerType i_SecondPlayerType, eBoardSizes i_BoardSize)
        {
            m_Board = new Board(i_BoardSize);
            createGamePlayers(i_FirstPlayerName, i_SecondPlayerName, i_SecondPlayerType);
            setPlayersSettings();
        }

        private void createGamePlayers(string i_FirstPlayerName, string i_SecondPlayerName, ePlayerType i_SecondPlayerType)
        {
            m_BlackPlayer = new Player(i_FirstPlayerName, ePieceColor.Black_X, ePlayerType.Human);
            m_WhitePlayer = new Player(i_SecondPlayerName, ePieceColor.White_O, i_SecondPlayerType);
        }

        private void setPlayersSettings()
        {
            setPlayerslocationList();
            setPlayersValidMovesList();
            setFirstPlayerInRound();
            resetPlayersCoins();
        }

        private void setPlayerslocationList()
        {
            m_BlackPlayer.PiecesLocationOnBoard = MoveValidator.GetNewPiecesLocationList((eBoardSizes)m_Board.BoardSize, m_BlackPlayer.PlayerColor);
            m_WhitePlayer.PiecesLocationOnBoard = MoveValidator.GetNewPiecesLocationList((eBoardSizes)m_Board.BoardSize, m_WhitePlayer.PlayerColor);
        }

        private void setPlayersValidMovesList()
        {
            m_BlackPlayer.SetValidMoves(m_Board);
            m_WhitePlayer.SetValidMoves(m_Board);
        }

        private void resetPlayersCoins()
        {
            m_BlackPlayer.KingCoins = m_WhitePlayer.KingCoins = 0;
            m_BlackPlayer.SoldierCoins = m_WhitePlayer.SoldierCoins = 0;
        }

        private void setFirstPlayerInRound()
        {
            m_CurrentPlayer = m_BlackPlayer;
        }

        public void InitRoundWithSameSettings()
        {
            m_Board.OrganizeBoard();
            setPlayersSettings();
        }

        public eMoveStatus MakeComputerPlayerMove(out string io_PlayerMove)
        {
            eMoveStatus moveStatus = eMoveStatus.Incomplete;

            io_PlayerMove = getComputerPlayerMove();
            if (CurrentPlayer.HasValidMove)
            {
                setPlayerMoveOnBoard(io_PlayerMove, ref moveStatus);
            }
            else
            {
                moveStatus = eMoveStatus.NoMoves;
            }

            System.Threading.Thread.Sleep(200);

            return moveStatus;
        }

        private string getComputerPlayerMove()
        {
            Random rand = new Random();
            int index = rand.Next(0, m_CurrentPlayer.ValidMoves.Count);

            return m_CurrentPlayer.ValidMoves[index];
        }

        public eMoveStatus MakeHumanPlayerMove(string i_PlayerMove)
        {
            eMoveStatus moveStatus = eMoveStatus.Incomplete;

            if (CurrentPlayer.HasValidMove)
            {
                setPlayerMoveOnBoard(i_PlayerMove, ref moveStatus);
            }
            else
            {
                moveStatus = eMoveStatus.NoMoves;
            }

            return moveStatus;
        }

        private void setPlayerMoveOnBoard(string i_PlayerMove, ref eMoveStatus io_MoveStatus)
        {
            int fromMoveCol, fromMoveRow, destMoveCol, destMoveRow;

            MoveParser.GetFromAndDestLocationIndexes(i_PlayerMove, out fromMoveCol, out fromMoveRow, out destMoveCol, out destMoveRow);
            checkAndUpdateIfPieceWasEaten(i_PlayerMove, ref io_MoveStatus, fromMoveCol, fromMoveRow, destMoveCol, destMoveRow);
            MoveValidator.UpdatePiecesLocationList(m_CurrentPlayer, i_PlayerMove);
            m_Board.UpdateMoveOnBoard(m_CurrentPlayer, fromMoveCol, fromMoveRow, destMoveCol, destMoveRow);
            m_Board.CheckAndUpdateIfKingWasMade(m_CurrentPlayer, destMoveCol, destMoveRow);
        }

        private void checkAndUpdateIfPieceWasEaten(string i_PlayerMove, ref eMoveStatus io_MoveStatus, int i_FromMoveCol, int i_FromMoveRow, int i_DestMoveCol, int i_DestMoveRow)
        {
            bool wasPieceEaten = MoveValidator.WasJumpMove(i_FromMoveCol, i_FromMoveRow, i_DestMoveCol, i_DestMoveRow);

            if (wasPieceEaten)
            {
                int eatenPieceCol = (i_FromMoveCol + i_DestMoveCol) / 2;
                int eatenPieceRow = (i_FromMoveRow + i_DestMoveRow) / 2;

                m_Board[eatenPieceCol, eatenPieceRow] = eSquareStatus.Clear;
                MoveValidator.UpdateOpponentPiecesLocationList(GetOpponentPlayer(m_CurrentPlayer), eatenPieceCol, eatenPieceRow);
                checkIfPlayerTurnIsCompleteAfterJump(i_PlayerMove, ref io_MoveStatus);
            }
            else
            {
                io_MoveStatus = eMoveStatus.Complete;
            }
        }

        private void checkIfPlayerTurnIsCompleteAfterJump(string i_PlayerMove, ref eMoveStatus io_MoveStatus)
        {
            List<string> locationOfLastPeiceMove = new List<string>();

            locationOfLastPeiceMove.Add(MoveParser.GetDestinationLocation(i_PlayerMove));
            m_CurrentPlayer.ValidMoves = MoveValidator.GetValidJumpMoveList(m_Board, locationOfLastPeiceMove, m_CurrentPlayer.PlayerColor);
            if (m_CurrentPlayer.ValidMoves.Count == 0)
            {
                io_MoveStatus = eMoveStatus.Complete;
            }
        }

        public void CheckPlayerMoveStatusAndUpdateRoundStatus(eMoveStatus i_MoveStatus, ref eRoundStatus io_RoundStatus)
        {
            switch(i_MoveStatus)
            {
                case eMoveStatus.Exit:
                    setQuitingPlayer(ref io_RoundStatus);
                    break;
                case eMoveStatus.NoMoves:
                    updatePlayersNoMovesStatus(ref io_RoundStatus);
                    break;
                case eMoveStatus.Complete:
                    switchPlayerTurn();
                    checkIfPlayerHasPiecesLeft(ref io_RoundStatus);
                    break;
            }
        }

        private void setQuitingPlayer(ref eRoundStatus io_RoundStatus)
        {
            io_RoundStatus = eRoundStatus.Quit;
            m_WinnerPlayer = GetOpponentPlayer(m_CurrentPlayer);
        }

        private void checkIfPlayerHasPiecesLeft(ref eRoundStatus io_RoundStatus)
        {
            int numberOfPieces = m_CurrentPlayer.GetNumberOfPlayerPieces();

            if(numberOfPieces == 0)
            {
                io_RoundStatus = eRoundStatus.Win;
                m_WinnerPlayer = GetOpponentPlayer(m_CurrentPlayer);
            }
        }

        public Player GetOpponentPlayer(Player i_Player)
        {
            Player opponentPlayer = m_WhitePlayer;

            if (i_Player == m_WhitePlayer)
            {
                opponentPlayer = m_BlackPlayer;
            }

            return opponentPlayer;
        }

        private void updatePlayersNoMovesStatus(ref eRoundStatus io_RoundStatus)
        {
            Player opponentPlayer = GetOpponentPlayer(m_CurrentPlayer);
            
            opponentPlayer.SetValidMoves(m_Board);
            if (opponentPlayer.HasValidMove)
            {
                io_RoundStatus = eRoundStatus.Win;
                m_WinnerPlayer = opponentPlayer;
            }
            else
            {
                io_RoundStatus = eRoundStatus.Tie;
            }
        }

        private void switchPlayerTurn()
        {
            if (m_CurrentPlayer.PlayerColor == ePieceColor.Black_X)
            {
                m_CurrentPlayer = m_WhitePlayer;
            }
            else
            {
                m_CurrentPlayer = m_BlackPlayer;
            }

            m_CurrentPlayer.SetValidMoves(m_Board);
        }

        public void CalculateRoundPoints(eMoveStatus i_MoveStatus)
        {
            Player opponnentPlayer = GetOpponentPlayer(m_CurrentPlayer);

            opponnentPlayer.UpdatePlayerCoins(m_Board);
            if (i_MoveStatus != eMoveStatus.Exit)
            {
                m_CurrentPlayer.UpdatePlayerCoins(m_Board);
            }
            else
            {
                m_CurrentPlayer.Coins = 0;
            }

            m_WinnerPlayer.Points += m_WinnerPlayer.Coins - GetOpponentPlayer(m_WinnerPlayer).Coins;
        }
    }
}
