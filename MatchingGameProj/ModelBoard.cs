using System;

namespace Matching_Game
{
    public class ModelBoard
    {
        // Data members:
        private char[,] m_realTimeBoard;
        private char[] m_completeBoard;

        // methods:
        public ModelBoard(int i_Height, int i_Width)
        {
            m_realTimeBoard = new char[i_Height, i_Width];
            buildRandomizedCompleteBoard(i_Height, i_Width);
        }

        public int BoardHeight
        {
            get
            {
                return m_realTimeBoard.GetLength(0);
            }
        }

        public int BoardWidth
        {
            get
            {
                return m_realTimeBoard.GetLength(1);
            }
        }

        public char[,] RealTimeBoard
        {
            get
            {
                return m_realTimeBoard;
            }
        }

        public void AddSignToRealTimeBoard(Position i_SignPosition)
        {
            m_realTimeBoard[i_SignPosition.Row, i_SignPosition.Col] = GetSignFromCompleteBoard(i_SignPosition);
        }

        public void RemovePairFromRealTimeBoard(Position i_FristSignPosition, Position i_SecondSignPosition)
        {
            m_realTimeBoard[i_FristSignPosition.Row, i_FristSignPosition.Col] = default(Char);
            m_realTimeBoard[i_SecondSignPosition.Row, i_SecondSignPosition.Col] = default(Char);
        }

        public bool IsSquareAvailable(Position i_SignPosition)
        {
            return (m_realTimeBoard[i_SignPosition.Row, i_SignPosition.Col] == default(Char));
        }

        public bool IsIdenticalPair(Position i_FirstChoicePosition, Position i_SecondChoicePosition)
        {
            return m_completeBoard[completeBoardIndex(i_FirstChoicePosition)] == m_completeBoard[completeBoardIndex(i_SecondChoicePosition)];
        }

        public char GetSignFromCompleteBoard(Position i_SignPosition)
        {
            return m_completeBoard[completeBoardIndex(i_SignPosition)];
        }

        private void buildRandomizedCompleteBoard(int i_Height, int i_Width)
        {
            m_completeBoard = new char[i_Height * i_Width];
            char signToAdd = 'A';

            // filling the board with pairs of letters:
            for (int i = 0; i < m_completeBoard.Length; i += 2)
            {
                m_completeBoard[i] = signToAdd;
                m_completeBoard[i + 1] = signToAdd;
                signToAdd++;
            }

            // shuffle the board:
            ShuffleBoard();
        }

        private void ShuffleBoard()
        {
            Random randomizeIndex = new Random();
            char savedSign;

            for (int i = 0, j; i < m_completeBoard.Length - 1; i++)
            {
                j = randomizeIndex.Next(i, m_completeBoard.Length);
                savedSign = m_completeBoard[i];
                m_completeBoard[i] = m_completeBoard[j];
                m_completeBoard[j] = savedSign;
            }
        }

        private int completeBoardIndex(Position i_PositionToConvert)// Convert matrix Position to array index 
        {
            return i_PositionToConvert.Row * m_realTimeBoard.GetLength(1) + i_PositionToConvert.Col;
        }
    }
}
