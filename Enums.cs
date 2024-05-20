using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Secventiator
{
    enum SBUS
    {
        NONE,
        PdFLAG,
        PdRG,
        PdSP,
        PdT,
        PdTN,
        PdPC,
        PdIVR,
        PdADR,
        PdMDR,
        PdIR,
        Pd0,
        PdMinus1
    };

    enum DBUS
    {
        NONE,
        PdFLAG,
        PdRG,
        PdSP,
        PdT,
        PdPC,
        PdIVR,
        PdADR,
        PdMDR,
        PdMDRN, 
        PdIR,
        Pd0,
        PdMinus1
    };

    enum ALU
    {
        NONE, 
        SBUS, 
        DBUS,
        ADD,
        SUB,
        AND,
        OR,
        XOR,
        ASL, 
        ASR,
        LSR,
        ROL,
        ROR,
        RLC,
        RRC,
    };

    enum RBUS
    {
        NONE, 
        PmFLAG,
        PmFLAG3,
        PmRG,
        PmSP,
        PmT,
        PmPC,
        PmIVR,
        PmADR,
        PmMDR,
    };

    enum MEM
    {
        NONE,
        IFCH,
        RD,
        WR
    };

    enum OTHERS
    {
        NONE,
        PLUS2SP,
        MIN2SP,
        PLUS2PC,
        A1BE0,
        A1BE1,
        PdCONDa,
        CinPdCONDa,
        PdCONDl,
        A1BVI,
        A0BVI,
        A0BPO,
        INTAMIN2SP,
        A0BEA0BI
    };

    enum SUCCESOR
    {
        STEP,
        JUMPI,
        IFACLOW,
        IFCIL,
        IFC,
        IFZ,
        IFS,
        IFV
    };
}
