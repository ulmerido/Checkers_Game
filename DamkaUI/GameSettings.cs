using System.Collections.Generic;
using System.Text;
using System;
using System.Windows.Forms;
using System.Drawing;

namespace DamkaUI
{
    public class GameSettings : Form
    {
        private readonly GroupBox                 groupboxBordSize;
        private readonly List<RadioButtonWithInt> radioButtonBoardSize;
        private readonly GroupBox                 groupBoxPlayers;
        private readonly Button                   buttonDone;
        private readonly Label                    labelPlayer1;
        private readonly TextBox                  textBoxPlayer1;
        private readonly CheckBox                 checkBoxPlayer2;
        private readonly TextBox                  textBoxPlayer2;
        private readonly System.Media.SoundPlayer r_Music; 
        private int m_BoardSize;

        public int BoardSize
        {
            get
            {
                return m_BoardSize;
            }
        }

        public string Player1
        {
            get
            {
                return textBoxPlayer1.Text;
            }
        }

        public string Player2
        {
            get
            {
                return textBoxPlayer2.Text;
            }
        }

        public bool IsPlayer2Human
        {
            get
            {
                return checkBoxPlayer2.Checked;
            }
        }

        public Button ButtonDone
        {
            get
            {
                return buttonDone;
            }
        }

        public GroupBox GroupBoxBoardSize
        {
            get
            {
                return groupboxBordSize;
            }
        }

        public GameSettings() : base()
        {
            this.Text = "Game Settings";
            this.Size = new Size(218, 185);
            groupboxBordSize = new GroupBox();
            radioButtonBoardSize = new List<RadioButtonWithInt>();
            groupBoxPlayers = new GroupBox();
            buttonDone = new Button();
            labelPlayer1 = new Label();
            textBoxPlayer1 = new TextBox();
            checkBoxPlayer2 = new CheckBox();
            textBoxPlayer2 = new TextBox();
            r_Music = new System.Media.SoundPlayer(DamkaUI.Properties.Resources.theme);
            checkBoxPlayer2.CheckedChanged += new EventHandler(checkBoxPlayer2_CheckChanged);
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;     
        }

        public bool SettingsAreOk()
        {
            bool sizeInputOK = false;
            bool player1NameOk = false;
            bool player2NameOk = false;

            foreach (RadioButton rb in radioButtonBoardSize)
            {
                if (rb.Checked)
                {
                    sizeInputOK = true;
                    break;
                }
            }

            player1NameOk = nameOk(textBoxPlayer1.Text);
            player2NameOk = nameOk(textBoxPlayer2.Text);

            if (!sizeInputOK)
            {
                throw new ArgumentException("Error: Choose Size");             
            }

            if (!player1NameOk)
            {
                throw new ArgumentException("Error: Player1 Name invalid ");
            }

            if (!player2NameOk)
            {
                throw new ArgumentException("Error: Player2 Name invalid");
            }

            return (sizeInputOK && player2NameOk && player1NameOk);
          }

        protected override void OnShown(EventArgs e)
        {
            this.BackColor = Color.LightSteelBlue;
            setRadioButtonsBoardSize();
            setGroupRadioBoardSize();
            setGroupPlayers();
            setButtonDone();
            r_Music.Play();
            base.OnShown(e);
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            try
            {
                if (SettingsAreOk())
                {
                    this.DialogResult = DialogResult.OK;
                    r_Music.Stop();
                    this.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
            }
        }

        private void setButtonDone()
        {
            buttonDone.Size = new Size(80, 25);
            buttonDone.Location = new Point(groupBoxPlayers.Right - buttonDone.Size.Width, groupBoxPlayers.Bottom + 10);
            buttonDone.Text = "Done";
            buttonDone.Click += buttonDone_Click;
            this.Controls.Add(buttonDone);
        }

        private void setGroupPlayers()
        {       
            groupBoxPlayers.Text = "Players:";
            labelPlayer1.Text = "Player 1:";
            checkBoxPlayer2.Text = "Player 2:";
            textBoxPlayer2.Enabled = false;
            textBoxPlayer2.Text = "[Computer]";

            groupBoxPlayers.Location = new Point(groupboxBordSize.Left, groupboxBordSize.Bottom + 10);
            labelPlayer1.Location = new Point(21, 15);
            labelPlayer1.Size = new Size(55, 15);
            textBoxPlayer1.Location = new Point(105, 15);
            textBoxPlayer1.Size = new Size(70, 15);

            checkBoxPlayer2.Location = new Point(21, 35);
            checkBoxPlayer2.Size = new Size(75, 15);
            textBoxPlayer2.Location = new Point(105, 35);
            textBoxPlayer2.Size = new Size(70, 15);
            groupBoxPlayers.Controls.AddRange(new Control[] { labelPlayer1, textBoxPlayer1, checkBoxPlayer2, textBoxPlayer2 });
            this.Controls.Add(groupBoxPlayers);
            groupBoxPlayers.Size = new Size(groupboxBordSize.Width, 60);
        }

        private void setRadioButtonsBoardSize()
        {
            int i = 21;

            radioButtonBoardSize.Add(new RadioButtonWithInt(6));
            radioButtonBoardSize[0].Text = "6x6";
            radioButtonBoardSize[0].Click += new EventHandler(radioButtonBoardSize_Click);
            radioButtonBoardSize.Add(new RadioButtonWithInt(8));
            radioButtonBoardSize[1].Text = "8x8";
            radioButtonBoardSize[1].Click += new EventHandler(radioButtonBoardSize_Click);
            radioButtonBoardSize.Add(new RadioButtonWithInt(10));
            radioButtonBoardSize[2].Text = "10x10";
            radioButtonBoardSize[2].Click += new EventHandler(radioButtonBoardSize_Click);

            foreach (RadioButton radioB in radioButtonBoardSize)
            {
                radioB.Location = new Point(i, 15);
                radioB.Size = new Size(58, 20);
                groupboxBordSize.Controls.Add(radioB);
                i += 58;
            }
        }

        private void setGroupRadioBoardSize()
        {
            this.Controls.Add(groupboxBordSize);
            groupboxBordSize.Text = "Board Size";
            groupboxBordSize.Size = new Size(200, 40);
            groupboxBordSize.Location = new Point(5, 4);
        }

        private void checkBoxPlayer2_CheckChanged(object sender, EventArgs e)
        {
            if (checkBoxPlayer2.Checked)
            {
                textBoxPlayer2.Enabled = true;
                textBoxPlayer2.Text = "Enter Name";
            }
            else
            {
                textBoxPlayer2.Enabled = false;
                textBoxPlayer2.Text = "[Computer]";
            }
        }

        // $G$ NTT-999 (-3) You should have use string built-in function : Contains

        private bool nameOk(string i_Name)
        {
            bool hasSpaces = false;
         
            foreach (char ch in i_Name)
            {
                if (ch == ' ')
                {
                    hasSpaces = true;
                    break;
                }        
            }

            return ((!hasSpaces) && (i_Name.Length <= 20) && (i_Name.Length > 0));
        }
      
        private void radioButtonBoardSize_Click(object sender, EventArgs e)
        {
            if (sender is RadioButtonWithInt)
            {
                m_BoardSize = (sender as RadioButtonWithInt).Data;
            }
        }
    }
}
