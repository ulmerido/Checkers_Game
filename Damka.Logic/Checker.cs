using System;
using System.Collections.Generic;

using System.Text;

namespace Damka.Logic
{
     public class Checker
    {      
        private char m_Color;
        private bool m_IsKing = false;

        public char Color
        {
            get
            {
                return m_Color;
            }

            set
            {
                m_Color = value;
            }
        }

        public bool IsKing
        {
            get
            {
                return m_IsKing;
            }

            set
            {
                m_IsKing = value;
            }
        }

        public Checker(Checker i_CopyMe)// copy ctr
        {
            m_Color  = i_CopyMe.m_Color;
            m_IsKing = i_CopyMe.m_IsKing;
        }

        public Checker(char i_Color, bool i_IsKing)// copy ctr
        {
            m_Color = i_Color;
            m_IsKing = i_IsKing;
        }
    }
}
