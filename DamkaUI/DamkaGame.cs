using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Media;
using Damka.Logic;

namespace DamkaUI
{
    public class DamkaGame : Form
    {
        private readonly GameSettings gameSettingsForm;
        private readonly GroupBox     groupBoxForMatrix;
        private readonly Label        labelPlayer1HeadLine;
        private readonly Label        labelPlayer2HeadLine;
        private readonly PictureBox   pictureBoxWhosTurn;
        private readonly Label        labelWhosTurn;
        private readonly Move         r_GameMover;
        private readonly SoundPlayer  r_SoundGame;
        private DamkaSquereButton[,]  damkaSquereButtonMatrix;
        private GameBoard             m_Board;
        private Player                m_Player1;
        private Player                m_Player2;
        private Damka.Logic.Point     m_EndPoint;
        private Damka.Logic.Point     m_StartPoint;
        private char                  m_PlayerTurn;
        private eClickTypeMove        m_CurrentMove;

        public DamkaGame() : base()
        {
            gameSettingsForm = new GameSettings();
            groupBoxForMatrix = new GroupBox();
            m_CurrentMove = eClickTypeMove.Start;
            m_PlayerTurn = (char)eCheckerGame.WhitePlayer;
            r_GameMover = new Move();
            labelPlayer1HeadLine = new Label();
            labelPlayer2HeadLine = new Label();
            labelWhosTurn = new Label();
            pictureBoxWhosTurn = new PictureBox();
            r_SoundGame = new SoundPlayer(DamkaUI.Properties.Resources.DamkaGameMusic);          
            this.Text = "Damka";
            this.Icon = DamkaUI.Properties.Resources.Iron_Devil_Ids_3d_Icons_20_Ico_zilla;
            this.StartPosition = FormStartPosition.Manual;
        }

        private void gameTie()
        {
            StringBuilder msg = new StringBuilder();
            updateFinalScore();
            msg.AppendLine("Tie!");
            msg.AppendLine("Another Round?");

            MessageBox.Show(msg.ToString(), "Damka", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            roundEndMessegeBox(msg.ToString());
        }

        private void gameWin()
        {
            StringBuilder msg = new StringBuilder();
            string        playerWonName;
            bool          playerWithTurnHaveMoves;
            playerWithTurnHaveMoves = r_GameMover.CheckIfThereAreAnyPossibaleMoves(m_Board, m_PlayerTurn);

            if (m_Player1.NumOfPawnAlive == 0 || (!playerWithTurnHaveMoves && (m_PlayerTurn == (char)eCheckerGame.WhitePlayer)))
            {
                playerWonName = m_Player2.GetName();
            }
            else
            {
                playerWonName = m_Player1.GetName();
            }

            updateFinalScore();
            msg.AppendFormat("{0} Won!{1}", playerWonName, Environment.NewLine);
            msg.AppendLine("Another Round?");
            roundEndMessegeBox(msg.ToString());
        }

        private void updateFinalScore()
        {
            m_Player1.FinalScore = m_Player1.FinalScore + m_Player1.PlayerScore;
            m_Player2.FinalScore = m_Player2.FinalScore + m_Player2.PlayerScore;
        }

        private void roundEndMessegeBox(string i_msg)
        {
            DialogResult dialogResult;
            dialogResult = MessageBox.Show(i_msg.ToString(), "Damka", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                m_Board.ResetGameBoard();
                updateGameDetails();
                m_CurrentMove = eClickTypeMove.Start;
                m_PlayerTurn = (char)eCheckerGame.WhitePlayer;              
            }
            else
            {
                this.Close();
            }
        }

        private void checkGameOver()
        {
            bool playerWithTurnHaveMoves;
            bool playerWithOutTurnHaveMoves;
            playerWithTurnHaveMoves = r_GameMover.CheckIfThereAreAnyPossibaleMoves(m_Board, m_PlayerTurn);
            swapTurn();
            playerWithOutTurnHaveMoves = r_GameMover.CheckIfThereAreAnyPossibaleMoves(m_Board, m_PlayerTurn);
            swapTurn();

            if (!playerWithOutTurnHaveMoves && !playerWithTurnHaveMoves)
            {
                gameTie();
            }
            else if ((m_Player1.NumOfPawnAlive == 0) || (m_Player2.NumOfPawnAlive == 0) || (!playerWithTurnHaveMoves))
            {
                gameWin();
            }
        }

        private void swapTurn()
        {
            if (m_PlayerTurn == (char)eCheckerGame.WhitePlayer)
            {
                m_PlayerTurn = (char)eCheckerGame.BlackPlayer;
            }
            else
            {
                m_PlayerTurn = (char)eCheckerGame.WhitePlayer;
            }
        }

        private void damkaSquereButton_Click(object sender, EventArgs e)
        {
            checkIfCancelMove(sender as DamkaSquereButton);
            DamkaSquareMove(sender as DamkaSquereButton);
        }

        private void checkIfCancelMove(DamkaSquereButton i_Sender)
        {
            if (i_Sender.BackColor == Color.LightBlue && m_CurrentMove == eClickTypeMove.End)
            {
                m_CurrentMove = eClickTypeMove.Cancel;
            }
        }

        private void DamkaSquareMove(DamkaSquereButton i_Sender)
        {         
            switch (m_CurrentMove)
            {
                case eClickTypeMove.Start:
                    clickForStartMove(i_Sender);
                    break;
                case eClickTypeMove.End:                   
                case eClickTypeMove.Combo:
                    m_EndPoint = i_Sender.Coordnate;
                    clickForEndMove();
                    break;
                case eClickTypeMove.Cancel:
                    clickForCancelAMove(i_Sender);
                    break;
            }
        }

        private void clickForStartMove(DamkaSquereButton i_Sender)
        {
            Square sqr = m_Board.GetSquare(i_Sender.Coordnate.GetX(), i_Sender.Coordnate.GetY());
            if (sqr.GetChecker() == null)
            {
                MessageBox.Show("Error: Start Point is Empty", "Wrong Move", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if(sqr.GetChecker().Color != m_PlayerTurn)
            {
                MessageBox.Show("Wrong Pawn Choose: Please Choose Other Color ", "Wrong Pawn", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                m_CurrentMove = eClickTypeMove.End;
                m_StartPoint = i_Sender.Coordnate;
                i_Sender.BackColor = Color.LightBlue;
            }
        }

        private void clickForEndMove()
        {            
            try
            {
                r_GameMover.MakeAMove(m_Board, m_StartPoint, m_EndPoint, m_PlayerTurn);

                if (r_GameMover.CanEatAgain) 
                {
                    m_CurrentMove = eClickTypeMove.Combo;                    
                    m_StartPoint = m_EndPoint;
                    damkaSquereButtonMatrix[m_StartPoint.GetX(), m_StartPoint.GetY()].BackColor = Color.Blue;
                }
                else
                {
                    swapTurn();
                    checkGameOver();
                    checkIfPCTurn();
                    m_CurrentMove = eClickTypeMove.Start;
                }

                updateGameDetails();
                checkGameOver();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }                      
        }

        private void clickForCancelAMove(DamkaSquereButton i_Sender)
        {
            i_Sender.BackColor = Color.FromArgb(209, 139, 71);
            m_CurrentMove = eClickTypeMove.Start;
        }

        private void checkIfPCTurn()
        {
            if (((m_PlayerTurn == m_Player1.GetColor()) && (!m_Player1.IsHuman())) || ((m_PlayerTurn == m_Player2.GetColor()) && (!m_Player2.IsHuman())))
            {
                r_GameMover.MakeAIMove(m_Board, m_PlayerTurn);
                swapTurn();
            }

            updateGameDetails();
        }

        private void initilizeGameParams()
        {
            gameSettingsForm.Close();
            m_Player1 = new Player(gameSettingsForm.Player1, true);
            m_Player1.SetColor((char)eCheckerGame.WhitePlayer);
            m_Player2 = new Player(gameSettingsForm.Player2, gameSettingsForm.IsPlayer2Human);
            m_Player2.SetColor((char)eCheckerGame.BlackPlayer);
            m_Board = new GameBoard(gameSettingsForm.BoardSize);

            initializeBoardDamka();
            updateGameDetails();
            m_Board.ResetGameBoard();          
        }

        protected override void OnShown(EventArgs e)
        {
            gameSettingsForm.ShowDialog();
            if (gameSettingsForm.DialogResult == DialogResult.OK)
            {
                initilizeGameParams();
                this.BackColor = Color.DarkOliveGreen;
                r_SoundGame.PlayLooping();
                base.OnShown(e);
            }
            else
            {
                this.Close();
            }
        }

        private void createDamkaButtonBoard()
        {
            for (int i = 0; i < m_Board.GetSize; i++)
            {
                for (int j = 0; j < m_Board.GetSize; j++)
                {
                    damkaSquereButtonMatrix[i, j] = new DamkaSquereButton(new Damka.Logic.Point(i, j), m_Board.GetSquare(i, j));
                    damkaSquereButtonMatrix[i, j].Size = new Size(50, 50);
                    damkaSquereButtonMatrix[i, j].Location = new System.Drawing.Point(50 * j, 50 * i);
                    damkaSquereButtonMatrix[i, j].Click += new EventHandler(this.damkaSquereButton_Click);
                    if ((i < m_Board.GetSize) && (mudoloTwoRowXorCol(i, j)))   
                    {
                        damkaSquereButtonMatrix[i, j].BackColor = Color.FromArgb(209, 139, 71);  // was white;
                    }
                    else
                    {
                        damkaSquereButtonMatrix[i, j].BackColor = Color.FromArgb(255, 206, 158);  // was black;
                        damkaSquereButtonMatrix[i, j].Enabled = false;
                    }

                    groupBoxForMatrix.Controls.Add(damkaSquereButtonMatrix[i, j]);
                }
            }
        }

        private void initializeBoardDamka()
        {
            damkaSquereButtonMatrix = new DamkaSquereButton[m_Board.GetSize, m_Board.GetSize];
            labelWhosTurn.Location = new System.Drawing.Point((50 * m_Board.GetSize) + 60, 10);
            labelWhosTurn.Size = new Size(120, 40);
            labelWhosTurn.Font = new Font("Consolas", 10, FontStyle.Bold);
            pictureBoxWhosTurn.Location = new System.Drawing.Point((50 * m_Board.GetSize) + 75, 55);
            pictureBoxWhosTurn.Size = new Size(50, 50);
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            
            createDamkaButtonBoard();

            this.Size = new Size((50 * m_Board.GetSize) + gameSettingsForm.Player1.Length + gameSettingsForm.Player2.Length + 160, (50 * m_Board.GetSize) + 130);
            groupBoxForMatrix.Size = new Size(50 * m_Board.GetSize, 50 * m_Board.GetSize);
            groupBoxForMatrix.Location = new System.Drawing.Point(48, 48);
            this.Controls.Add(groupBoxForMatrix);
            this.Controls.Add(labelPlayer1HeadLine);
            labelPlayer1HeadLine.Location = new System.Drawing.Point(48, 10);
            labelPlayer1HeadLine.Size = new Size(99, 40);
            labelPlayer1HeadLine.Font = new Font("Consolas", 10, FontStyle.Bold);
            labelPlayer2HeadLine.Location = new System.Drawing.Point((m_Board.GetSize * 25) + 48, 10);
            labelPlayer2HeadLine.Size = new Size(99, 40);
            labelPlayer2HeadLine.Font = new Font("Consolas", 10, FontStyle.Bold);
            this.Controls.Add(labelPlayer2HeadLine);
            this.Controls.Add(labelWhosTurn);
            this.Controls.Add(pictureBoxWhosTurn);
        }

        private void updateGameDetails()
        {
            m_Player1.UpdatePlayerPawnsCounterAndScore(m_Board);
            m_Player2.UpdatePlayerPawnsCounterAndScore(m_Board);
            setPlayersDetilesHeadline();
            showWhosTurn();
        }
   
        private void showWhosTurn()
        {
            StringBuilder str = new StringBuilder();
            if (m_PlayerTurn == (char)eCheckerGame.WhitePlayer)
            {
                str.AppendFormat("{0}'s Turn:", m_Player1.GetName());
                pictureBoxWhosTurn.Image = DamkaUI.Properties.Resources.white;
            }
            else
            {
                str.AppendFormat("{0}'s Turn:", m_Player2.GetName());
                pictureBoxWhosTurn.Image = DamkaUI.Properties.Resources.red;
            }

            labelWhosTurn.Text = str.ToString();
        }

        private void setTheCorrectPawnForDamkaButton(Square i_Sqr, DamkaSquereButton i_Button)
        {         
            if (i_Sqr.GetChecker() == null)
            {
                i_Button.Text = string.Empty;
                i_Button.BackgroundImage = null;
            }
            else
            {
                switch (i_Sqr.GetChecker().Color)
                {
                    case (char)eCheckerGame.BlackPlayer:
                        if (i_Sqr.GetChecker().IsKing)
                        {
                            i_Button.BackgroundImage = DamkaUI.Properties.Resources.redKing;
                        }
                        else
                        {
                            i_Button.BackgroundImage = DamkaUI.Properties.Resources.red;
                        }

                        break;

                    case (char)eCheckerGame.WhitePlayer:
                        if (i_Sqr.GetChecker().IsKing)
                        {
                            i_Button.BackgroundImage = DamkaUI.Properties.Resources.whiteKing;
                        }
                        else
                        {
                            i_Button.BackgroundImage = DamkaUI.Properties.Resources.white;
                        }

                        break;
                }

                i_Button.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }

        private void setPlayersDetilesHeadline()
        {
            StringBuilder str1 = new StringBuilder();
            StringBuilder str2 = new StringBuilder();
            str1.AppendFormat("{0}: {1}", m_Player1.GetName(), m_Player1.FinalScore);
            str2.AppendFormat("{0}: {1}", m_Player2.GetName(), m_Player2.FinalScore);

            labelPlayer1HeadLine.Text = str1.ToString();
            labelPlayer2HeadLine.Text = str2.ToString();
        }

        private bool mudoloTwoRowXorCol(int i_Row, int i_Col)
        {
            return i_Col % 2 != i_Row % 2;
        }
    }
}
