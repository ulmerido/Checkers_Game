using System;
using System.Collections.Generic;

using System.Text;

namespace Damka.Logic
{
    // $G$ SFN-012 (+7) Bonus: Events in the Logic layer are handled by the UI.
    public delegate void SquareChangedEventHandlerDelegate(Square i_Sqr);

    public class Square
    {
        private Checker m_Pawn;

        public event SquareChangedEventHandlerDelegate EventSquareChanged;

        public void NotifyListenersAboutSquareChange()
        {
            if (EventSquareChanged != null)
            {
                EventSquareChanged.Invoke(this);
            }
        }

        public void ClearSquare()
        {
            m_Pawn = null;
            NotifyListenersAboutSquareChange();
        }

        public void CopyChecker(Checker i_Copy)
        {
            m_Pawn = new Checker(i_Copy);
            NotifyListenersAboutSquareChange();
        }

        public void SetChecker(char i_Color, bool i_IsKing)
        {          
            if (m_Pawn == null)
            {
                m_Pawn = new Checker(i_Color, i_IsKing);
            }
            else
            {
                m_Pawn.Color = i_Color;
                m_Pawn.IsKing = i_IsKing;
            }

            NotifyListenersAboutSquareChange();
        }

        public Checker GetChecker()
        {
            return m_Pawn;
        }

        public Square(char i_Color, bool i_IsKing)
        {
            m_Pawn = new Checker(i_Color, i_IsKing);
        }

        public Square()
        {
            m_Pawn = null;
        }
    }
}
