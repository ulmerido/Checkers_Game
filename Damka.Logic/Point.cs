using System;
using System.Collections.Generic;
using System.Text;

namespace Damka.Logic
{
    public struct Point
        {
            private int m_X;
            private int m_Y;

            public Point(int i_X, int i_Y)
            {
                m_X = i_X;
                m_Y = i_Y;
            }

            public int GetX()
            {
                return m_X;
            }

            public int GetY()
            {
                return m_Y;
            }

            public void SetXY(int i_X, int i_Y)
            {
                m_X = i_X;
                m_Y = i_Y;
            }

            public bool ComparePoints(Point i_CmpTo)
            {
                return (m_X == i_CmpTo.GetX()) && (m_Y == i_CmpTo.GetY());
            }
        }
}
