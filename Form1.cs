using System;
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
        int aclow, cil,cIN;
        int INTR, INTA;
        int bpo;
        int cALU, zALU, sALU, vALU;

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


                    switch (operatieALU)
                    {
                        case (int)ALU.NONE: break;
                        case (int)ALU.SBUS: RBUS = SBUS; break;
                        case (int)ALU.DBUS: RBUS = DBUS; break;
                        case (int)ALU.ADD: RBUS = (UInt16)(SBUS + DBUS); break;
                        case (int)ALU.SUB: RBUS = (UInt16)(SBUS - DBUS); break;
                        case (int)ALU.AND: RBUS = (UInt16)(SBUS & DBUS); break;
                        case (int)ALU.OR: RBUS = (UInt16)(SBUS | DBUS); break;
                        case (int)ALU.XOR: RBUS = (UInt16)(SBUS ^ DBUS); break;
                        case (int)ALU.ASL: RBUS = (UInt16)(SBUS >> DBUS); break;
                        case (int)ALU.ASR: RBUS = (UInt16)(SBUS << DBUS); break;
                        case (int)ALU.LSR: int x = DBUS;
                            RBUS = SBUS;
                            while (x>0)
                            {
                                RBUS= (UInt16) (RBUS >> 1);
                                RBUS &= 0x7FFF;
                                x--;
                            }
                            break;
                        case (int)ALU.ROL: RBUS = (UInt16)((UInt16) (SBUS << DBUS) | (UInt16)(SBUS >> (-DBUS & 16))); break;
                        case (int)ALU.ROR: RBUS = (UInt16)((UInt16)(SBUS >> DBUS) | (UInt16)(SBUS << (-DBUS & 16))); break;
                        case (int)ALU.RLC: 
                            int y = DBUS;
                            RBUS = SBUS;
                            while (y > 0)
                            {
                                cALU = (RBUS & 0x8000); 
                                RBUS = (UInt16)((SBUS <<1) | (cALU >> 15));
                                y--;
                            } break;
                        case (int)ALU.RRC:
                            int w = DBUS;
                            RBUS = SBUS;
                            while (w > 0)
                            {
                                cALU = RBUS & 0x0001;
                                RBUS = (UInt16)((SBUS >> 1) | (cALU << 15));
                                w--;
                            }
                            break;
                    }
                    if (RBUS == 0)
                    {
                        zALU = 1;
                    }
                    if(RBUS >= 0)
                    {
                        sALU = 0;
                    }
                    else
                    {
                        sALU = 1;
                    }



                    switch (alteOperatii)
                    {
                        case (int)OTHERS.NONE: break;
                        case (int)OTHERS.PLUS2SP: SP = SP + 2; break;
                        case (int)OTHERS.MIN2SP: SP = SP - 2; break;
                        case (int)OTHERS.PLUS2PC: PC = PC + 2; break;
                        case (int)OTHERS.A1BE0: aclow = 1; break;
                        case (int)OTHERS.A1BE1: cil = 1;break;
                        case (int)OTHERS.PdCONDa:
                            FLAGS |= (ushort)(cALU << 3); 
                            FLAGS |= (ushort)(zALU << 2);   
                            FLAGS |= (ushort)(sALU << 1);  
                            FLAGS |= (ushort)vALU; break;
                        case (int)OTHERS.CinPdCONDa: 
                            cIN = 1; 
                            FLAGS |= (ushort)(cALU << 3);
                            FLAGS |= (ushort)(zALU << 2);
                            FLAGS |= (ushort)(sALU << 1);
                            FLAGS |= (ushort)vALU; break;
                        case (int)OTHERS.PdCONDl:
                            FLAGS |= (ushort)(zALU << 2);
                            FLAGS |= (ushort)(sALU << 1);
                            break;
                        case (int)OTHERS.A1BVI: bvi = 1; break;
                        case (int)OTHERS.A0BVI: bvi = 0; break;
                        case (int)OTHERS.INTAMIN2SP: INTA = 1; SP = SP - 2; break;
                        case (int)OTHERS.A0BEA0BI: aclow = 0; cil = 0; bvi = 0; break;
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
            int cl1 = IR >> 15 & (IR & 0b0100000000000000) >>14;
            int cl0 = IR >> 15 & ~((IR & 0b0010000000000000) >> 13);
            int index1 = cl1 << 1;
            index1 = index1 + cl0;
            switch(index)
            {
                case 0:
                    return 0;
                case 1: return index1;
                case 2:
                    return (IR & 0b0000110000000000)>>10;
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
           
        }

    }
}
