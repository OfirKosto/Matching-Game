using System;
using System.Text;

namespace Matching_Game
{
    public class MatchingGameUi
    {
        // Date members:
        private eEndGameWinner m_EndGameState;
        private ModelBoard m_DataBoard;
        bool m_IsVersusComputer;
        bool m_GameStillRuning;
        bool m_IsRoundRuning;
        Position m_FirstSquareChoice;
        Position m_SecondSquareChoice;
        Player m_MainPlayer;


        public MatchingGameUi()
        {
            m_FirstSquareChoice = new Position();
            m_SecondSquareChoice = new Position();
        }

        public enum eEndGameWinner
        {
            PlayerWon,
            ComputerWon,
            Tie,
        }

        public void PlayGame()
        {
            // Start of the game
            m_GameStillRuning = true;
            m_MainPlayer = new Player(MessageOrganizer.EnterName());
            m_IsVersusComputer = (MessageOrganizer.ChooseOpponent() == 2); // checks which opponent is selected

            // starts the game that's fit for the choosen opponent:
            if (m_IsVersusComputer)
            {
                playerVsComputer();
            }
            else
            {
                playerVsPlayer();
            }

            MessageOrganizer.exitGame();
        }

        private void startRound()
        {
            int boardHeight = 0, boardWidth = 0;

            MessageOrganizer.ClearScreen();
            MessageOrganizer.HandleBoardSize(ref boardHeight, ref boardWidth);
            m_DataBoard = new ModelBoard(boardHeight, boardWidth);
        }

        private void playerVsComputer()
        {
            while (m_GameStillRuning)
            {
                m_MainPlayer.Score = 0;
                bool isMainPlayerTurn = true;
                m_IsRoundRuning = true;
                Computer aiComputer;

                startRound();
                aiComputer = new Computer(m_DataBoard.BoardHeight, m_DataBoard.BoardWidth);

                // turns start:
                while (m_IsRoundRuning)
                {
                    if (isMainPlayerTurn)
                    {
                        MessageOrganizer.TurnMsg(m_MainPlayer.Name);
                    }
                    else
                    {
                        MessageOrganizer.TurnMsg();
                    }

                    MessageOrganizer.ShowBoard(m_DataBoard.RealTimeBoard);

                    if (isMainPlayerTurn)
                    {
                        playerPairPick();
                    }
                    else // COMPUTER AI PICK
                    {
                        aiComputer.PickPair(m_DataBoard, m_FirstSquareChoice, m_SecondSquareChoice);
                        showComputerPicks();
                    }

                    if (m_DataBoard.IsIdenticalPair(m_FirstSquareChoice, m_SecondSquareChoice))
                    {
                        if (isMainPlayerTurn)
                        {
                            aiComputer.UpdateMemory(m_DataBoard, m_FirstSquareChoice, m_SecondSquareChoice, true);
                            m_MainPlayer.Score++;
                        }
                        else
                        {
                            aiComputer.Score++;
                        }

                        if (checkFullBoard(m_MainPlayer.Score, aiComputer.Score))
                        {
                            string winnerName = gameWinner(string.Empty, aiComputer.Score);
                            endRound(winnerName);
                        }

                    }
                    else
                    {
                        aiComputer.UpdateMemory(m_DataBoard, m_FirstSquareChoice, m_SecondSquareChoice, false);
                        m_DataBoard.RemovePairFromRealTimeBoard(m_FirstSquareChoice, m_SecondSquareChoice);

                        isMainPlayerTurn = !(isMainPlayerTurn);

                        System.Threading.Thread.Sleep(2000); // if its not a matched pair let the viewers see the board for additonal  second(2 second total)
                    }

                }
            }
        }

        private void showComputerPicks()
        {
            m_DataBoard.AddSignToRealTimeBoard(m_FirstSquareChoice);
            MessageOrganizer.ShowBoard(m_DataBoard.RealTimeBoard);
            m_DataBoard.AddSignToRealTimeBoard(m_SecondSquareChoice);
            MessageOrganizer.ShowBoard(m_DataBoard.RealTimeBoard);

        }

        private int playerVsComputerWin(int i_PlayerScore, int i_ComputerScore)
        {
            int winnerType;

            if (i_PlayerScore > i_ComputerScore)
            {
                winnerType = 0;
            }
            else if (i_PlayerScore < i_ComputerScore)
            {
                winnerType = 1;
            }
            else
            {
                winnerType = 2;
            }

            return winnerType;
        }

        private void playerVsPlayer()
        {
            Player secondPlayer = new Player(MessageOrganizer.EnterName());

            while (m_GameStillRuning)
            {
                Player currentPlayer = m_MainPlayer;
                m_IsRoundRuning = true;
                m_MainPlayer.Score = secondPlayer.Score = 0;

                startRound();

                while (m_IsRoundRuning)
                {
                    MessageOrganizer.TurnMsg(currentPlayer.Name);
                    MessageOrganizer.ShowBoard(m_DataBoard.RealTimeBoard);

                    playerPairPick();

                    if (m_DataBoard.IsIdenticalPair(m_FirstSquareChoice, m_SecondSquareChoice))
                    {
                        currentPlayer.Score++;

                        if (checkFullBoard(m_MainPlayer.Score, secondPlayer.Score))
                        {
                            string winnerName = gameWinner(secondPlayer.Name, secondPlayer.Score);
                            endRound(winnerName);
                        }
                    }
                    else
                    {
                        m_DataBoard.RemovePairFromRealTimeBoard(m_FirstSquareChoice, m_SecondSquareChoice);

                        currentPlayer = currentPlayer == m_MainPlayer ? secondPlayer : m_MainPlayer;

                        System.Threading.Thread.Sleep(2000); // if its not a matched pair let the viewers see the board for additonal  second(2 second total)
                    }
                }
            }
        }

        private string playerVsPlayerWin(Player i_FirstPlayer, Player i_SecondPlayer, ref int o_EndState)
        {
            string winnerName = string.Empty;

            if (i_FirstPlayer.Score > i_SecondPlayer.Score)
            {
                o_EndState = 0;
                winnerName = i_FirstPlayer.Name;
            }
            else if (i_FirstPlayer.Score < i_SecondPlayer.Score)
            {
                o_EndState = 0;
                winnerName = i_SecondPlayer.Name;
            }
            else // tie
            {
                o_EndState = 2;
            }

            return winnerName;
        }

        private void playerPairPick()
        {
            playerSquarePick(m_FirstSquareChoice);
            playerSquarePick(m_SecondSquareChoice);
        }

        private void playerSquarePick(Position io_SquareChoice)
        {
            StringBuilder squareToRevelaStr = new StringBuilder(MessageOrganizer.ChooseSquareToReveal(m_DataBoard.BoardHeight, m_DataBoard.BoardWidth));
            squareStrToCoordinates(squareToRevelaStr.ToString(), io_SquareChoice);

            // checks if the choosen square is taken
            while (!m_DataBoard.IsSquareAvailable(io_SquareChoice))
            {
                MessageOrganizer.TakenSquareMsg();
                squareToRevelaStr.Replace(squareToRevelaStr.ToString(), MessageOrganizer.ChooseSquareToReveal(m_DataBoard.BoardHeight, m_DataBoard.BoardWidth));
                squareStrToCoordinates(squareToRevelaStr.ToString(), io_SquareChoice);
            }

            m_DataBoard.AddSignToRealTimeBoard(io_SquareChoice);
            MessageOrganizer.ShowBoard(m_DataBoard.RealTimeBoard);
        }

        private bool checkFullBoard(int i_PlayerOneScore, int i_PlayerTwoScore)
        {
            return (i_PlayerOneScore + i_PlayerTwoScore) == ((m_DataBoard.BoardHeight * m_DataBoard.BoardWidth) / 2);
        }

        private void squareStrToCoordinates(string i_SquareStr, Position o_SignPosition)
        {
            o_SignPosition.Col = i_SquareStr[0] - 'A';
            o_SignPosition.Row = (int)Char.GetNumericValue(i_SquareStr[1]) - 1;
        }

        private string gameWinner(string i_OpponentName, int i_OpponentScore)
        {
            string gameWinnerName = i_OpponentName;

            if (m_MainPlayer.Score > i_OpponentScore)
            {
                gameWinnerName = m_MainPlayer.Name;
                m_EndGameState = eEndGameWinner.PlayerWon;
            }
            else if (m_MainPlayer.Score == i_OpponentScore)
            {
                m_EndGameState = eEndGameWinner.Tie;
            }
            else
            {
                m_EndGameState = m_IsVersusComputer ? eEndGameWinner.ComputerWon : eEndGameWinner.PlayerWon;
            }

            return gameWinnerName;
        }

        private void endRound(string i_WinnerName)
        {
            MessageOrganizer.WinMessage(i_WinnerName, m_EndGameState);

            // end the round:
            m_IsRoundRuning = false;
            m_GameStillRuning = MessageOrganizer.ReplayGame();
        }

    }
}
