using System.Collections.Generic;
using System.Text;
using System;
using System.Windows.Forms;
using System.Drawing;

namespace DamkaUI
{
    public class RadioButtonWithInt : RadioButton
    {
        private int m_Data;

        public RadioButtonWithInt(int i_Data) : base()
        {
            m_Data = i_Data;
        }

        public int Data
        {
            get
            {
                return m_Data;
            }
        }
    }
}
