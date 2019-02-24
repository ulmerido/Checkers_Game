using System;

namespace Damka.Logic
{
    public class GameBoard
    {  
        private Square[,] m_Matrix;
        private int       m_Size;

        private static char getCharOfPawnToPrint(Checker i_Pawn)
        {
            char res;
            if(i_Pawn.IsKing == true && i_Pawn.Color == (char)eCheckerGame.WhitePlayer )
            {
                res = (char)eCheckerGame.WhiteKing;
            }
            else if((i_Pawn.IsKing == true) && (i_Pawn.Color == (char)eCheckerGame.BlackPlayer))
            {
                res = (char)eCheckerGame.BlackKing;
            }
            else
            {
                res = i_Pawn.Color;
            }

            return res;
        }

        private static void printABC(int i_Size)
        {
            Console.Write("   "); // print before the A
            for (int k = 0; k < i_Size; k++)
            {
                Console.Write(" {0}  ", Convert.ToChar('A' + k));
            }

            Console.Write(Environment.NewLine + "  ");           
            for (int m = 0; m < 4 * i_Size; m++)
            {
                Console.Write('=');
            }

            Console.Write(Environment.NewLine);
        }

        private bool mudoloTwoRowXorCol(int i_Row, int i_Col)
        {
            return i_Col % 2 != i_Row % 2;
        }

        public int GetSize
        {
            get
            {
                return m_Size;
            }
        }

        public GameBoard(int i_Size)
        {                   
            m_Size = i_Size;
            createMatrix();
            ResetGameBoard();
        }

        private void createMatrix()
        {
            m_Matrix = new Square[m_Size, m_Size];
            for (int i = 0; i < m_Size; i++)
            {
                for (int j = 0; j < m_Size; j++)
                {
                    m_Matrix[i, j] = new Square();                 
                }
            }
        }

        public void ResetGameBoard()
        {
            bool isKing = false;
            char color;

            for (int i = 0; i < m_Size; i++)
            {
                for (int j = 0; j < m_Size; j++)
                {
                    if ((i < (m_Size / 2) - 1) && (mudoloTwoRowXorCol(i, j)))
                    {
                        color = (char)eCheckerGame.WhitePlayer;
                        m_Matrix[i, j].SetChecker(color, isKing);
                    }
                    else if ((i >= (m_Size / 2) + 1) && (mudoloTwoRowXorCol(i, j)))
                    {
                        color = (char)eCheckerGame.BlackPlayer;
                        m_Matrix[i, j].SetChecker(color, isKing);
                    }
                    else
                    {
                        m_Matrix[i, j].ClearSquare();
                    }
                }
            }
        }

        public Checker GetChecker(int i_X, int i_Y) 
        { 
             return m_Matrix[i_X, i_Y].GetChecker(); 
        }

        public Square GetSquare(int i_X, int i_Y)
        {                
            return m_Matrix[i_X, i_Y];
        }

        public void PrintBoard()
        {
            char ch;
            printABC(m_Size);
            for (int i = 0; i < m_Size; i++)
            {              
                Console.Write("{0}| ", Convert.ToChar('a' + i)); 
                for (int j = 0; j < m_Size; j++)
                {
                    if (m_Matrix[i, j].GetChecker() == null)
                    {
                        Console.Write("   |");
                    }
                    else
                    {
                        ch = getCharOfPawnToPrint(m_Matrix[i, j].GetChecker());
                        Console.Write(" {0} |", ch);
                    }
                }

                Console.Write(Environment.NewLine + "  ");             
                for (int m = 0; m < 4 * m_Size; m++)
                {
                    Console.Write('=');
                }

                Console.Write(Environment.NewLine);              
            }
        }
    }
}
