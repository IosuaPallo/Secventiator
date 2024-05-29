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
        int f, g;
        int aclow, cil;
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
                    stare = 1;
                    break; 
                case 1:
                    int sursaSBUS = (int)((MIR >> 32) & 0xF);       // Bits 35-32
                    int sursaDBUS = (int)((MIR >> 28) & 0xF);       // Bits 31-28
                    int operatieALU = (int)((MIR >> 24) & 0xF);     // Bits 27-24
                    int operatieMem = (int)((MIR >> 20) & 0xF);     // Bits 23-20
                    int alteOperatii = (int)((MIR >> 14) & 0xF);    // Bits 17-14
                    int succesor = (int)((MIR >> 11) & 0x7);        // Bits 13-11
                    int index = (int)((MIR >> 8) & 0x7);            // Bits 10-8
                    int NTF = (int)((MIR >> 7) & 0x1);              // Bit 7
                    int microAdresaSalt = (int)(MIR & 0x7F);        // Bits 6-0


                    int bvi = (FLAGS >> 7) & 1; // bit 7
                    int c = (FLAGS >> 3) & 1;   // bit 3
                    int z = (FLAGS >> 2) & 1;   // bit 2
                    int s = (FLAGS >> 1) & 1;   // bit 1
                    int v = FLAGS & 1;          // bit 0


                    switch (sursaDBUS)
                    {
                        case (int)Secventiator.DBUS.NONE: break ;
                        case (int)Secventiator.DBUS.PdFLAG: DBUS = FLAGS; break;
                        case (int)Secventiator.DBUS.PdRG:DBUS = RG[IR & 0xF]; break;
                        case (int)Secventiator.DBUS.PdSP: DBUS= SP; break;
                        case (int)Secventiator.DBUS.PdT: DBUS = T; break;
                        case (int)Secventiator.DBUS.PdPC: DBUS = PC; break;
                        case (int)Secventiator.DBUS.PdIVR: DBUS =IVR; break;
                        case (int)Secventiator.DBUS.PdADR: DBUS = ADR; break;
                        case (int)Secventiator.DBUS.PdMDR: DBUS = MDR; break;
                        case (int)Secventiator.DBUS.PdMDRN: DBUS = (ushort)~MDR; break;
                        case (int)Secventiator.DBUS.PdIR: DBUS = (ushort)(IR & 0x00FF) ; break;
                        case (int)Secventiator.DBUS.Pd0: DBUS = 0xFF; break;
                        case (int)Secventiator.DBUS.PdMinus1: DBUS = 0xFF; break ;

                    }


                    switch (alteOperatii)
                    {
                        case (int)OTHERS.A1BE0: aclow = 1; break;
                        case (int)OTHERS.A1BE1: cil = 1;break;
                    }

                    switch (succesor)
                    {
                        case (int)SUCCESOR.STEP: f = NTF; g = 0; break;
                        case (int)SUCCESOR.JUMPI: f = NTF==0?1:0; g = 1; break;
                        case (int)SUCCESOR.IFACLOW: f = aclow; g = f ^ NTF; break;
                        case (int)SUCCESOR.IFCIL: f = cil; g = cil ^ NTF; break;
                        case (int)SUCCESOR.IFC: f = c; g = c ^ NTF; break;
                        case (int)SUCCESOR.IFZ: f = z; g = z ^ NTF; break;
                        case (int)SUCCESOR.IFS: f = s; g = s ^ NTF; break;
                        case (int)SUCCESOR.IFV: f = v; g = v ^ NTF; break;
                    }

                    if (g == 1)
                    {
                        MAR = (ulong) (microAdresaSalt + index); 
                    }
                    else
                    {
                        MAR++;
                    }

                    if(operatieMem == (int)Secventiator.MEM.NONE)
                    {
                        stare = 0;
                    }
                    else
                    {
                        stare = 2;
                    }

                    break;

                case 2:
                    break;
                default:
                    break;
            }
        }

        private int GetIndex(int index) {
            int cl1 = IR >> 15 & (IR & 0b0100000000000000) >>14; //IR15 & IR14
            int cl0 = IR >> 15 & ~((IR & 0b0010000000000000) >> 13); // IR15& ~IR13
            int index1 = cl1 << 1;
            index1 = index1 + cl0;
            switch(index)
            {
                case 0:
                    return 0;
                case 1: return index1;
                case 2:
                    return (IR & 0b0000110000000000)>>10; // 
                case 3:
                    return (IR & 0b0000000000110000)>>4; 
                case 4:
                    return (IR & 0b0111000000000000)>>12;
                case 5:
                    return (IR & 0b0000111100000000)>>8;
                case 6:
                    return (IR & 0b0000111100000000)>>7;
                case 7:
                    return INTR<<2;
                 default:
                    return 0; 
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            IR = 0b1110000000000000;
            MessageBox.Show(GetIndex(1).ToString());
        }
    }
}
