using System;
using System.Collections.Generic;
using System.Text;

namespace Damka.Logic
{
    public class Player
    {
        private int    m_FinalScore = 0;
        private string m_PlayerName;
        private bool   m_IsHuman;
        private char   m_Color;
        private int    m_NumOfPawnAlive;
        private int    m_PlayerScore = 0;
        private string m_LastMove = string.Empty;

        public int NumOfPawnAlive
        {
            get
            {
                return m_NumOfPawnAlive;
            }

            set
            {
                m_NumOfPawnAlive = value;
            }
        }

        public int PlayerScore
        {
            get
            {
                return m_PlayerScore;
            }

            set
            {
                m_PlayerScore = value;
            }
        }

        public string PlayerLastMove
        {
            get
            {
                return m_LastMove;
            }

            set
            {
                m_LastMove = value;
            }
        }

        public Player()
        {
            m_PlayerName = "Donald Trump";
            m_IsHuman = false;
            m_Color = (char)eCheckerGame.WhitePlayer;            
        }

        public Player(string i_UserInput, bool i_IsHuman)
        {
            m_PlayerName = string.Copy(i_UserInput);
            m_IsHuman    = i_IsHuman;
        }

        public void SetColor(char i_Color)
        {
            m_Color = i_Color;
        }

        public string GetName()
        {
            return m_PlayerName;
        }

        public char GetColor()
        {
            return m_Color;
        }

        public bool IsHuman()
        {
            return m_IsHuman;
        }

        public int FinalScore
        {
            get
            {
                return m_FinalScore;
            }

            set
            {
                m_FinalScore = value;
            }
        }

        public void UpdatePlayerPawnsCounterAndScore(GameBoard i_Board)
        {
            int pawnCounter = 0;    
            int kingCounter = 0;

            for (int i = 0; i < i_Board.GetSize; i++)
            {
                for (int j = 0; j < i_Board.GetSize; j++)
                {
                    if (i_Board.GetSquare(i, j).GetChecker() != null)
                    {                      
                        if (i_Board.GetSquare(i, j).GetChecker().Color == m_Color)  
                        {
                            if (i_Board.GetSquare(i, j).GetChecker().IsKing) 
                            {
                                kingCounter++;
                            }
                            else
                            { 
                                pawnCounter++;
                            }
                        }                   
                    }
                }
            }

            m_NumOfPawnAlive = kingCounter + pawnCounter;
            m_PlayerScore = (4 * kingCounter) + pawnCounter;  
        }
    }
}
