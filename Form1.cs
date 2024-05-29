using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Secventiator
{
    public partial class Form1 : Form
    {
        UInt64 MIR, MAR;
        UInt64[] MPM;
        UInt16 PC, IR, SP, FLAGS, T, ADR, MDR, IVR;
        UInt16[] RG;
        UInt16 SBUS, DBUS, RBUS;
        UInt16[] MEM;
        uint stare;
        int INTR; 


        public Form1()
        {
            InitializeComponent();
        }
        
        private void Sec()
        {
            switch (stare)
            {
                case 0:
                    MIR = MPM[MAR]; 
                    break; 
                case 1:
                    break;

                case 2:
                    break;
                default:
                    break;
            }
        }

        private int GetIndex(int index) {
            switch(index)
            {
                case 0:
                    return 0; 
                case 1:
                    return (IR & 0b0000110000000000)>>10 ;
                case 2:
                    return (IR & 0b0000110000000000)>>10;
                case 3:
                    return (IR & 0b0000000000110000)>>4; 
                case 4:
                    return IR & (0b0111000000000000)>>12;
                case 5:
                    return (IR & 0b0000111100000000)>>8;
                case 6:
                    return IR & (0b0000111100000000)>>7;
                case 7:
                    return INTR<<2;
                 default:
                    return 0; 
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show(GetIndex(5).ToString());
        }
    }
}
