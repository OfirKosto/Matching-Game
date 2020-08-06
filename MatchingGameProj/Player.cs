using System;

namespace Matching_Game
{
    public class Player
    {
        // Data members:
        private readonly string r_Name;
        private int m_Score;

        //Methods:
        public Player(string i_Name)
        {
            r_Name = i_Name;
            m_Score = 0; // starting score
        }

        public string Name
        {
            get
            {
                return r_Name;
            }
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
    }
}
