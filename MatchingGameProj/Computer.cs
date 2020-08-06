using System;

namespace Matching_Game
{
    /// <AiExplanation>
    /// The computer saves every square that has been reveled during the game.
    /// He have the abilty to pick pairs from his memory and can find match for a given square.
    /// He can also pick a square in a naive way.
    /// </AiExplanation>

    public class Computer
    {
        // Data members:
        private MemorySquare[,] m_BoardMemory;
        private int m_Score = 0;

        // methods:
        public Computer(int i_Height, int i_Width)
        {
            m_BoardMemory = new MemorySquare[i_Height, i_Width];
        }

        public int Score
        {
            get
            {
                return m_Score;
            }
            set
            {
                m_Score = value;
            }
        }

        public void PickPair(ModelBoard i_DataBoard, Position io_FirstChoicePosition, Position io_SecondChoicePosition)
        {
            char signValue;

            // copmputer AI algoritem to pick pair of squares:
            if (ChoosePairFromMemory(io_FirstChoicePosition, io_SecondChoicePosition))
            {
                m_BoardMemory[io_FirstChoicePosition.Row, io_FirstChoicePosition.Col].IsTaken = true;
                m_BoardMemory[io_SecondChoicePosition.Row, io_SecondChoicePosition.Col].IsTaken = true;
            }
            else
            {
                PickSquare(io_FirstChoicePosition); // select first square in a naive way.
                signValue = i_DataBoard.GetSignFromCompleteBoard(io_FirstChoicePosition); // gets the sign value from the complete board
                m_BoardMemory[io_FirstChoicePosition.Row, io_FirstChoicePosition.Col].Sign = signValue; // updates first naive choise sign to memory board

                if (SearchMatchInMemory(signValue, io_FirstChoicePosition, io_SecondChoicePosition))
                {
                    UpdateMemory(i_DataBoard, io_FirstChoicePosition, io_SecondChoicePosition, true);
                }
                else
                {
                    PickSquare(io_SecondChoicePosition); // select secound square in a naive way.

                    if (i_DataBoard.IsIdenticalPair(io_FirstChoicePosition, io_SecondChoicePosition))
                    {
                        UpdateMemory(i_DataBoard, io_FirstChoicePosition, io_SecondChoicePosition, true);
                    }
                    else  // failed to find a match sqaures
                    {
                        UpdateMemory(i_DataBoard, io_FirstChoicePosition, io_SecondChoicePosition, false);
                    }
                }
            }
        }

        public void UpdateMemory(ModelBoard i_DataBoard, Position i_FirstSignPosition, Position i_SecondSignPosition, bool i_IsTaken) // updates the computer memory board
        {
            m_BoardMemory[i_FirstSignPosition.Row, i_FirstSignPosition.Col].Sign = i_DataBoard.GetSignFromCompleteBoard(i_FirstSignPosition);
            m_BoardMemory[i_FirstSignPosition.Row, i_FirstSignPosition.Col].IsTaken = i_IsTaken;

            m_BoardMemory[i_SecondSignPosition.Row, i_SecondSignPosition.Col].Sign = i_DataBoard.GetSignFromCompleteBoard(i_SecondSignPosition);
            m_BoardMemory[i_SecondSignPosition.Row, i_SecondSignPosition.Col].IsTaken = i_IsTaken;
        }

        private bool ChoosePairFromMemory(Position io_FirstChoicePosition, Position io_SecondChoicePosition) // checks if there is any matched pair in the computer memory
        {
            Position candidatePosition = new Position();
            bool IsFoundPair = false;

            // check if there is a pair in the memory board that is not taken 
            for (int i = 0; i < m_BoardMemory.GetLength(0); i++)
            {
                for (int j = 0; j < m_BoardMemory.GetLength(1); j++)
                {
                    if (m_BoardMemory[i, j].Sign != default(char) && !(m_BoardMemory[i, j].IsTaken)) // a candidate to optinal pair
                    {
                        candidatePosition.SetPosition(i, j);

                        if (SearchMatchInMemory(m_BoardMemory[i, j].Sign, candidatePosition, io_SecondChoicePosition)) // calls a method to check if there is a match.
                        {
                            io_FirstChoicePosition.SetPosition(i, j);
                            IsFoundPair = true;
                            m_BoardMemory[io_FirstChoicePosition.Row, io_FirstChoicePosition.Col].IsTaken = true;
                            m_BoardMemory[io_SecondChoicePosition.Row, io_SecondChoicePosition.Col].IsTaken = true;
                            i = j = m_BoardMemory.GetLength(0) + m_BoardMemory.GetLength(1); // breaks all loops
                        }
                    }
                }
            }

            return IsFoundPair;
        }

        private bool SearchMatchInMemory(char i_Sign, Position io_SignPosition, Position o_MatchPosition)
        {
            bool foundMatch = false;

            for (int i = 0; i < m_BoardMemory.GetLength(0); i++)
            {
                for (int j = 0; j < m_BoardMemory.GetLength(1); j++)
                {
                    // if its the same sign but not in the same square of the one we sent
                    if (m_BoardMemory[i, j].Sign == i_Sign && !(io_SignPosition.Row == i && io_SignPosition.Col == j))
                    {
                        o_MatchPosition.SetPosition(i, j);
                        foundMatch = true;
                        i = j = m_BoardMemory.GetLength(0) + m_BoardMemory.GetLength(1); // breaks all loops
                    }
                }
            }

            return foundMatch;
        }

        private void PickSquare(Position o_SquarePosition) // computer naive selection 
        {
            for (int i = 0; i < m_BoardMemory.GetLength(0); i++)
            {
                for (int j = 0; j < m_BoardMemory.GetLength(1); j++)
                {
                    if (m_BoardMemory[i, j].Sign == default(Char)) // choose first empty square from the memory board
                    {
                        o_SquarePosition.SetPosition(i, j);
                        i = j = m_BoardMemory.GetLength(0) + m_BoardMemory.GetLength(1); // breaks all loops
                    }
                }
            }
        }

        // strcut that indicates if the square is in the real time board and saves his sign
        private struct MemorySquare
        {
            private char m_Sign;
            private bool m_IsTaken;

            public char Sign
            {
                get
                {
                    return m_Sign;
                }
                set
                {
                    m_Sign = value;
                }
            }

            public bool IsTaken
            {
                get
                {
                    return m_IsTaken;
                }
                set
                {
                    m_IsTaken = value;
                }
            }
        }


    }
}
