using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Damka.Logic;

namespace DamkaUI
{
    public class DamkaSquereButton : Button
    {
        private readonly Damka.Logic.Point r_Coordnate;

        public DamkaSquereButton(Damka.Logic.Point i_Coordnate, Square sqr) : base()
        {
            r_Coordnate = i_Coordnate;
            sqr.EventSquareChanged += this.setCorrectButtonDetails;
        }

        public Damka.Logic.Point Coordnate
        {
            get
            {
                return r_Coordnate;
            }
        }

        private void setCorrectButtonDetails(Square i_Sqr)
        {
            if (i_Sqr.GetChecker() == null)
            {
                this.Text = string.Empty;
                this.BackgroundImage = null;
            }
            else
            {
                switch (i_Sqr.GetChecker().Color)
                {
                    case (char)eCheckerGame.BlackPlayer:
                        if (i_Sqr.GetChecker().IsKing)
                        {
                            this.BackgroundImage = DamkaUI.Properties.Resources.redKing;
                        }
                        else
                        {
                            this.BackgroundImage = DamkaUI.Properties.Resources.red;
                        }

                        break;

                    case (char)eCheckerGame.WhitePlayer:
                        if (i_Sqr.GetChecker().IsKing)
                        {
                            this.BackgroundImage = DamkaUI.Properties.Resources.whiteKing;
                        }
                        else
                        {
                            this.BackgroundImage = DamkaUI.Properties.Resources.white;
                        }

                        break;
                }

                this.BackgroundImageLayout = ImageLayout.Stretch;
            }

            if (this.Enabled)
            {
              this.BackColor = Color.FromArgb(209, 139, 71);  // was white;
            }
        }
    }
}
