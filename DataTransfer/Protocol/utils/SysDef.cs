using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer.Protocol.utils
{
    public class SysDef
    {
        public const int REQUEST_TO_SEND_TIMEOUT = 3000; //3s

        /*-------------------------COMMON UDP package form-----------------------
         | Subtype(1 Byte)| Length(4 Bytes)| Data(n Bytes)| CRC(Reserve:2 Bytes)|
         -----------------------------------------------------------------------*/
        public const int DEF_SUBTYPE_OFFSET     = 0;
        public const int DEF_LENGTH_OFFSET      = DEF_SUBTYPE_OFFSET+1;
        public const int DEF_LENGTH_BYTES       = 4; // 4 bytes for LENGTH field
        public const int DEF_CRC_LENGTH         = 2;// Reserve

        /*--------------------------------Specific for Header UDP package form------------------------------------
         | Subtype(1 Byte)| Length(4 Bytes)| Number of packages(4 bytes)| FileName(n bytes)| CRC(Reserve:2 Bytes)|
         --------------------------------------------------------------------------------------------------------*/
        
        public const int DEF_HEADER_PKG_LENGTH      = 240;
        public const int DEF_HEADER_NUMOFPKG_LENGTH = 4;
        public const int DEF_HEADER_NUMOFPKG_OFFSET = DEF_LENGTH_OFFSET + DEF_LENGTH_BYTES;
        public const int DEF_HEADER_FILENAME_OFFSET = DEF_HEADER_NUMOFPKG_OFFSET + DEF_HEADER_NUMOFPKG_LENGTH;

        /*--------------------------DATA UDP package form------------------------
         | Subtype(1 Byte)| Length(4 Bytes)| Data(n Bytes)| CRC(Reserve:2 Bytes)|
         -----------------------------------------------------------------------*/
        public const int DEF_DATA_PKG_LENGTH            = 240;
        public const int DEF_DATA_OFFSET                = DEF_LENGTH_OFFSET + DEF_LENGTH_BYTES;
        public const int DEF_DATA_PKG_PAYLOAD_LENGTH    = DEF_DATA_PKG_LENGTH - DEF_LENGTH_BYTES - DEF_CRC_LENGTH - 1; // SubType have one byte only!
        /*--------------------------------------------CONTROL UDP package form-----------------------------------------
         | Subtype(1 Byte)| Length(4 Bytes)| Control(1 Byte)| Sub-control(1 Byte)| Data(n bytes)| CRC(Reserve:2 Bytes)|
         -------------------------------------------------------------------------------------------------------------*/
        public const int DEF_CTRL_PKG_LENGTH            = 240;
        public const int DEF_CTRL_CTRL_OFFSET           = DEF_LENGTH_OFFSET + DEF_LENGTH_BYTES;
        public const int DEF_CTRL_SUBCTRL_OFFSET        = DEF_CTRL_CTRL_OFFSET + 1;
        public const int DEF_CTRL_DATA_OFFSET           = DEF_CTRL_SUBCTRL_OFFSET + 1;

    }
}
