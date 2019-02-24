using System;
using System.Collections.Generic;
using System.Text;

namespace Damka.Logic
{
    public class Move
    {    
        private bool   m_DidCapture = false;
        private bool   m_EatAgain = false;
        private string s_LastAIMove = string.Empty;

        private bool ifLegalMoveManThenMove(GameBoard i_Board, Point i_Start, Point i_End, char i_PlayerColor, int addX, int addY)
        {
            int startX = i_Start.GetX();
            int startY = i_Start.GetY();
            int endX = i_End.GetX();
            int endY = i_End.GetY();
            bool res = true;

            if (((startX + addX == endX) && (startY - addY == endY)) || ((startX + addX == endX) && (startY + addY == endY)))
            {
                moveChecker(i_Board, i_Start, i_End);
                m_DidCapture = false;
            }
            else if ((startX + (addX * 2) == endX) && (startY + (addY * 2) == endY))
            {
                if (i_Board.GetChecker(startX + addX, startY + addY) != null && (i_Board.GetChecker(startX + addX, startY + addY).Color != i_PlayerColor))
                {
                    moveChecker(i_Board, i_Start, i_End);
                    m_DidCapture = true;
                    i_Board.GetSquare(startX + addX, startY + addY).ClearSquare(); // remove the eaten one 
                }
                else
                {
                    res = false;
                }
            }
            else if ((startX + (addX * 2) == endX) && (startY - (addY * 2) == endY))
            {
                if (i_Board.GetChecker(startX + addX, startY - addY) != null && (i_Board.GetChecker(startX + addX, startY - addY).Color != i_PlayerColor))
                {
                    moveChecker(i_Board, i_Start, i_End);
                    m_DidCapture = true;
                    i_Board.GetSquare(startX + addX, startY - addY).ClearSquare(); // remove the eaten one 
                }
                else
                {
                    res = false;
                }
            }
            else
            {
                res = false;
            }

            return res;
        }
            
        private void isStartSquareNotEmpty(Square i_Start)
        {         
            if(i_Start.GetChecker() == null)
            {
                throw new ArgumentException("Error: Starting Point Is Empty!!");
            }
        }

        private void ifThereIsACaptureMoveThenCheckIfUserChoseIt(GameBoard i_Board, Point i_StartCord, Point i_EndCord, char i_PlayerColor)
        {
            List<Point> captureOptionsArr;
            bool        checkedAllArray = false;
            captureOptionsArr = captureOptions(i_Board, i_PlayerColor);
            if (captureOptionsArr != null)
            {
                for (int i = 0; i < captureOptionsArr.Count; i += 2)
                {
                    if ((i_StartCord.ComparePoints(captureOptionsArr[i])) && (i_EndCord.ComparePoints(captureOptionsArr[i + 1])))
                    { 
                        break;
                    }

                    checkedAllArray = (i == captureOptionsArr.Count - 2);
                    if (checkedAllArray)
                    {
                        throw new ArgumentException("Error - You must capture a pawn - Don't be a Loser ");                      
                    }
                }
            }
        }

        private void checkIfStartPawnBelongsToPlayer(Checker i_Pawn, char i_WhosTurn)
        {
            if (i_Pawn.Color != i_WhosTurn)
            {
                throw new ArgumentException("Error: Choose a " + i_WhosTurn.ToString());            
            }
        }

        private void checkIfDestIsEmpty(Square i_Dest)
        {
            if (i_Dest.GetChecker() != null)
            {
                throw new ArgumentException("Error: destination is occupied  ");             
            }
        }

        private void moveChecker(GameBoard i_Board, Point i_Start, Point i_End)
        {          
            bool isKing = i_Board.GetChecker(i_Start.GetX(), i_Start.GetY()).IsKing;
            char color = i_Board.GetChecker(i_Start.GetX(), i_Start.GetY()).Color;

            i_Board.GetSquare(i_End.GetX(), i_End.GetY()).SetChecker(color, isKing);
            i_Board.GetSquare(i_Start.GetX(), i_Start.GetY()).ClearSquare();         
        }

        private void checkCaptureOptionsOfaGivenPawn(GameBoard i_Matrix, Point i_Start, char i_PlayerColor, ref List<Point> o_ResDestanation)// returns start,end,start,end..
        {
            Square checkSquare1 = null;
            Square checkSquare2 = null;
            int x               = i_Start.GetX();
            int y               = i_Start.GetY();
            int matrixSize      = i_Matrix.GetSize;
            checkSquare1        = i_Matrix.GetSquare(x, y);
            if (checkSquare1.GetChecker() != null)
            {
                if ((i_PlayerColor == (char)eCheckerGame.WhitePlayer) || (checkSquare1.GetChecker().IsKing))
                {
                    if ((x + 2 < matrixSize) && (y - 2 >= 0))
                    {
                        checkSquare1 = i_Matrix.GetSquare(x + 1, y - 1);
                        checkSquare2 = i_Matrix.GetSquare(x + 2, y - 2);
                        if (checkSquare1.GetChecker() != null && checkSquare2.GetChecker() == null && (checkSquare1.GetChecker().Color != i_PlayerColor))
                        {
                            o_ResDestanation.Add(new Point(x, y));
                            o_ResDestanation.Add(new Point(x + 2, y - 2));
                        }
                    }

                    if ((x + 2 < matrixSize) && (y + 2 < matrixSize))
                    {
                        checkSquare1 = i_Matrix.GetSquare(x + 1, y + 1);
                        checkSquare2 = i_Matrix.GetSquare(x + 2, y + 2);

                        if (checkSquare1.GetChecker() != null && checkSquare2.GetChecker() == null && (checkSquare1.GetChecker().Color != i_PlayerColor))
                        {
                            o_ResDestanation.Add(new Point(x, y));
                            o_ResDestanation.Add(new Point(x + 2, y + 2));
                        }
                    }
                }

                checkSquare1 = i_Matrix.GetSquare(x, y);

                if ((i_PlayerColor == (char)eCheckerGame.BlackPlayer) || (checkSquare1.GetChecker().IsKing))
                {
                    if ((y + 2 < matrixSize) && (x - 2 >= 0))
                    {
                        checkSquare1 = i_Matrix.GetSquare(x - 1, y + 1);
                        checkSquare2 = i_Matrix.GetSquare(x - 2, y + 2);
                        if (checkSquare1.GetChecker() != null && checkSquare2.GetChecker() == null && (checkSquare1.GetChecker().Color != i_PlayerColor))
                        {
                            o_ResDestanation.Add(new Point(x, y));
                            o_ResDestanation.Add(new Point(x - 2, y + 2));
                        }
                    }

                    if ((x - 2 >= 0) && (y - 2 >= 0))
                    {
                        checkSquare1 = i_Matrix.GetSquare(x - 1, y - 1);
                        checkSquare2 = i_Matrix.GetSquare(x - 2, y - 2);
                        if (checkSquare1.GetChecker() != null && checkSquare2.GetChecker() == null && (checkSquare1.GetChecker().Color != i_PlayerColor))
                        {
                            o_ResDestanation.Add(new Point(x, y));
                            o_ResDestanation.Add(new Point(x - 2, y - 2));
                        }
                    }
                }
            }
        }

        private void checkWeHaveANewKing(Point i_PawnCord, Square i_Sqr, int i_MatrixSize)
        {         
            int  x = i_PawnCord.GetX();
            char color = i_Sqr.GetChecker().Color;

            if ((color == (char)eCheckerGame.BlackPlayer) && (x == 0))
            {
                i_Sqr.GetChecker().IsKing = true; 
                i_Sqr.NotifyListenersAboutSquareChange();
            }

            if((color == (char)eCheckerGame.WhitePlayer) && (x == i_MatrixSize - 1))
            {
                i_Sqr.GetChecker().IsKing = true;
                i_Sqr.NotifyListenersAboutSquareChange();
            }
        }

        private void ifPawnCanEatAgain(GameBoard i_Matrix, Point i_Start, char i_PlayerColor)
        {
            List<Point> movePoss = new List<Point>(1);
            checkCaptureOptionsOfaGivenPawn(i_Matrix, i_Start, i_PlayerColor, ref movePoss);
            if(movePoss.Count != 0)
            {
                m_EatAgain = true;
            }
        }

        private void findAIMove(GameBoard i_Matrix, out Point o_StartMove, out Point o_EndMove, char i_AIColor)
        {       
            List<Point> captureOptionsArray;
            captureOptionsArray = captureOptions(i_Matrix, i_AIColor);
            Random randNumMaker = new Random();
            o_StartMove = new Point();
            o_EndMove   = new Point();

            if ((captureOptionsArray != null) && (captureOptionsArray.Count > 0))
            {
                int randInt = randNumMaker.Next(0, captureOptionsArray.Count);
                if(randInt % 2 == 1)
                {
                    randInt--;
                }

                o_StartMove = captureOptionsArray[randInt];
                o_EndMove = captureOptionsArray[randInt + 1];
            }
            else
            {
                captureOptionsArray = moveOptions(i_Matrix, i_AIColor);
                if(captureOptionsArray != null)
                {
                    int randInt = randNumMaker.Next(0, captureOptionsArray.Count);
                    if (randInt % 2 == 1)
                    {
                        randInt--;
                    }

                    o_StartMove = captureOptionsArray[randInt];
                    o_EndMove = captureOptionsArray[randInt + 1];
                }
            }
        }

        private void checkMoveOptionsOfaGivenPawn(GameBoard i_Matrix, Point i_Start, char i_PlayerColor, ref List<Point> o_ResDestanation)// returns start,end,start,end..
        {
            Square startSquare = null;
            Square endSquare = null;
            int    x = i_Start.GetX();
            int    y = i_Start.GetY();
            int    matrixSize = i_Matrix.GetSize;
            startSquare = i_Matrix.GetSquare(x, y);

            // white move or black king
            if ((i_PlayerColor == (char)eCheckerGame.WhitePlayer) || (startSquare.GetChecker().IsKing))
            {
                if ((x + 1 < matrixSize) && (y - 1 >= 0))
                {
                    endSquare = i_Matrix.GetSquare(x + 1, y - 1);
                    if (endSquare.GetChecker() == null)
                    {
                        o_ResDestanation.Add(new Point(x, y));
                        o_ResDestanation.Add(new Point(x + 1, y - 1));
                    }
                }

                if ((x + 1 < matrixSize) && (y + 1 < matrixSize))
                {
                    endSquare = i_Matrix.GetSquare(x + 1, y + 1);
                    if (endSquare.GetChecker() == null)
                    {
                        o_ResDestanation.Add(new Point(x, y));
                        o_ResDestanation.Add(new Point(x + 1, y + 1));
                    }
                }
            }
         
            if ((i_PlayerColor == (char)eCheckerGame.BlackPlayer) || (startSquare.GetChecker().IsKing))
            {
                if ((y - 1 >= 0) && (x - 1 >= 0))
                {
                    endSquare = i_Matrix.GetSquare(x - 1, y - 1);
                    if (endSquare.GetChecker() == null)
                    {
                        o_ResDestanation.Add(new Point(x, y));
                        o_ResDestanation.Add(new Point(x - 1, y - 1));
                    }
                }

                if ((x - 1 >= 0) && (y + 1 < i_Matrix.GetSize))
                {
                    endSquare = i_Matrix.GetSquare(x - 1, y + 1);
                    if (endSquare.GetChecker() == null)
                    {
                        o_ResDestanation.Add(new Point(x, y));
                        o_ResDestanation.Add(new Point(x - 1, y + 1));
                    }
                }
            }
        }

        private void translatePointToString(Point i_Start, Point i_End)
        {
            StringBuilder str = new StringBuilder();
            str.Append(Convert.ToChar(i_Start.GetY() + 'A'));
            str.Append(Convert.ToChar(i_Start.GetX() + 'a'));
            str.Append('>');
            str.Append(Convert.ToChar(i_End.GetY() + 'A'));
            str.Append(Convert.ToChar(i_End.GetX() + 'a'));
            s_LastAIMove = str.ToString();
        }

        private List<Point> moveOptions(GameBoard i_Matrix, char i_PlayerColor)
        {
            List<Point> optionsArray = new List<Point>(1);
            Square currSquare;
            for (int i = 0; i < i_Matrix.GetSize; i++)
            {
                for (int j = 0; j < i_Matrix.GetSize; j++)
                {
                    currSquare = i_Matrix.GetSquare(i, j);
                    if ((currSquare.GetChecker() != null)  && (currSquare.GetChecker().Color == i_PlayerColor))
                    {
                        checkMoveOptionsOfaGivenPawn(i_Matrix, new Point(i, j), i_PlayerColor, ref optionsArray);
                    }
                }
            }

            if (optionsArray.Count == 0)
            {
                optionsArray = null;
            }

            return optionsArray;
        }

        private List<Point> captureOptions(GameBoard i_Matrix, char i_PlayerColor)
        {
            List<Point> optionsArray = new List<Point>(1);
            Square currSquare;
            for (int i = 0; i < i_Matrix.GetSize; i++)
            {
                for (int j = 0; j < i_Matrix.GetSize; j++)
                {
                    currSquare = i_Matrix.GetSquare(j, i);
                    if ((currSquare.GetChecker() != null) && (currSquare.GetChecker().Color == i_PlayerColor))
                    {
                        checkCaptureOptionsOfaGivenPawn(i_Matrix, new Point(j, i), i_PlayerColor, ref optionsArray);
                    }
                }
            }

            if (optionsArray.Count == 0)
            {
                optionsArray = null;
            }

            return optionsArray;
        }

        public string GetLastAIMove
        {
            get
            {
                return s_LastAIMove;
            }
        }

        public bool GetDidCapture
        {
            get
            {
                return m_DidCapture;
            }
        }

        public bool CanEatAgain
        {
            get
            {
                return m_EatAgain;
            }
        }
   
        public void MakeAMove(GameBoard i_Board, Point i_StartCord, Point i_EndCord, char i_PlayerColor)
        {
            m_EatAgain = false;
            m_DidCapture = false;          
            Square startSquare = i_Board.GetSquare(i_StartCord.GetX(), i_StartCord.GetY());
            Square endSquare = i_Board.GetSquare(i_EndCord.GetX(), i_EndCord.GetY());
            bool   isKing;
            bool   ValidMove = true;

            try
            {
                isStartSquareNotEmpty(startSquare);
                checkIfStartPawnBelongsToPlayer(startSquare.GetChecker(), i_PlayerColor);
                checkIfDestIsEmpty(endSquare);
                ifThereIsACaptureMoveThenCheckIfUserChoseIt(i_Board, i_StartCord, i_EndCord, i_PlayerColor);
                isKing = startSquare.GetChecker().IsKing;
                if (!isKing)
                {
                    if (i_PlayerColor == (char)eCheckerGame.WhitePlayer)
                    {
                        ValidMove = ifLegalMoveManThenMove(i_Board, i_StartCord, i_EndCord, i_PlayerColor, 1, 1); // WhiteMove
                    }
                    else
                    {
                        ValidMove = ifLegalMoveManThenMove(i_Board, i_StartCord, i_EndCord, i_PlayerColor, -1, 1); // BlackMove
                    }
                }             
                else 
                {     
                    if (!ifLegalMoveManThenMove(i_Board, i_StartCord, i_EndCord, i_PlayerColor, 1, 1))
                    {
                        ValidMove = ifLegalMoveManThenMove(i_Board, i_StartCord, i_EndCord, i_PlayerColor, -1, 1); // BlackMove                                     
                    }
                }      
                
                if (!ValidMove)
                {
                    throw new ArgumentException("Error: unvalid move");
                }

                checkWeHaveANewKing(i_EndCord, endSquare, i_Board.GetSize);
                if (m_DidCapture)
                {
                    ifPawnCanEatAgain(i_Board, i_EndCord, i_PlayerColor);
                }               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void MakeAIMove(GameBoard i_Matrix, char i_PlayerColor)
        {
            Point       startPoint, endPoint;
            List<Point> comboList = new List<Point>();
            findAIMove(i_Matrix, out startPoint, out endPoint, i_PlayerColor);
            MakeAMove(i_Matrix, startPoint, endPoint, i_PlayerColor);
            translatePointToString(startPoint, endPoint);
            while(m_EatAgain)
            {
                checkCaptureOptionsOfaGivenPawn(i_Matrix, endPoint, i_PlayerColor, ref comboList);
                MakeAMove(i_Matrix, endPoint, comboList[1], i_PlayerColor);
                StringBuilder str = new StringBuilder(s_LastAIMove);
                str.Append('>');
                str.Append(Convert.ToChar(comboList[1].GetY() + 'A'));
                str.Append(Convert.ToChar(comboList[1].GetX() + 'a'));
                s_LastAIMove = str.ToString();
            }
        }

        public void ResetStaticValsForNextGame()
        {
            s_LastAIMove = string.Empty;
            m_EatAgain = false;
            m_DidCapture = false;
        }

        public bool CheckIfThereAreAnyPossibaleMoves(GameBoard i_Matrix, char i_PlayerColor)
        {
            bool thereAreMoveOptions = true;
            List<Point> moveOptionArray;
            moveOptionArray = captureOptions(i_Matrix, i_PlayerColor);
            if(moveOptionArray == null)
            {
                moveOptionArray = moveOptions(i_Matrix, i_PlayerColor);
                if(moveOptionArray == null)
                {
                    thereAreMoveOptions = false;
                }
            }
          
            return thereAreMoveOptions;
        }
    }
}
