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
                    return 1;
                case 2:
                    return 2;
                case 3:
                    return 3; 
                case 4:
                    return 4;
                case 5:
                    return 5;
                case 6:
                    return 6;
                case 7:
                    return 7;
                 default:
                    return 0; 
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
