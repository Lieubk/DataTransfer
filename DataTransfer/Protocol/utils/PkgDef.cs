using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer.Protocol.utils
{
    // Subtype( SMS or File or Control)
    public enum PackageType : byte
    {
        PT_SMS = 0x00,
        PT_HEADER_FILE = 0x01,
        PT_DATA_FILE = 0x02,
        PT_CTRL = 0x03,

        PT_ERROR = 0xFF,
    }

    public class PkgDef
    {
        public PackageType PD_Subtype;
        public int PD_Length;
        public byte[] Payload;

        public override string ToString()
        {
            string str = string.Empty;
            str += ("Subtype = " + PD_Subtype + ",  ");
            str += ("Length = " + PD_Length.ToString() + ",  ");
            str += ("Payload = " + ", Payload[] = [ ");
            foreach (byte b in Payload)
            {
                str += (b.ToString("X2") + " ");
            }
            str += (" ]");
            return str;
        }
    }

    public class HeaderPkg
    {
        public PackageType PD_Subtype;
        public int PD_Length;
        public UInt32 PD_NumOfPkg;
        public string PD_FileName;
        public byte[] Buf;

        public override string ToString()
        {
            string str = string.Empty;
            str += ("Subtype = " + PD_Subtype + ",  ");
            str += ("Length = " + PD_Length.ToString() + ",  ");
            str += ("Number of Packages = " + PD_NumOfPkg + ",  ");
            str += "FileName = " + PD_FileName;

            return str;
        }
    }

    public class DataPkg
    {
        public PackageType PD_Subtype;
        public int PD_Length;
        public byte[] Payload;

        public override string ToString()
        {
            string str = string.Empty;
            str += ("Subtype = " + PD_Subtype + ",  ");
            str += ("Length = " + PD_Length.ToString() + ",  ");
            str += ("Payload = " + ", Payload[] = [ ");
            foreach (byte b in Payload)
            {
                str += (b.ToString("X2") + " ");
            }
            str += (" ]");
            return str;
        }
    }

    public class CtrlPkg
    {
        public PackageType PD_Subtype;
        public int PD_Length;
        public int PD_Ctrl;
        public int PD_SubCtrl;
        public byte[] Payload;

        public override string ToString()
        {
            string str = string.Empty;
            str += ("Subtype = " + PD_Subtype + ",  ");
            str += ("Length = " + PD_Length.ToString() + ",  ");
            str += ("Control = " + PD_Ctrl.ToString() + ",  ");
            str += ("Sub Control = " + PD_SubCtrl.ToString() + ",  ");
            str += ("Payload = " + ", Payload[] = [ ");
            foreach (byte b in Payload)
            {
                str += (b.ToString("X2") + " ");
            }
            str += (" ]");
            return str;
        }
    }
}
