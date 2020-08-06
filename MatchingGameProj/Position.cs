using System;

namespace Matching_Game
{
    public class Position
    {
        private int m_Row = -1;
        private int m_Col = -1;

        public int Row
        {
            get
            {
                return m_Row;
            }
            set
            {
                m_Row = value;
            }
        }

        public int Col
        {
            get
            {
                return m_Col;
            }
            set
            {
                m_Col = value;
            }
        }

        public void SetPosition(int i_Row, int i_Col)
        {
            m_Row = i_Row;
            m_Col = i_Col;
        }
    }
}
