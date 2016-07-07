using DataTransfer.Protocol.utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataTransfer.Protocol
{
    public delegate void DataTransEvent(object param);

    public class DataTrans
    {
        #region Parametter
        public _RXParam RXParam = new _RXParam();

        public _TXParam TXParam = new _TXParam();

        private static readonly object txSync = new object();
        protected ManualResetEvent readyToSend = new ManualResetEvent(false);

        public string SaveFolderPath = string.Empty;

        private const string IPA_Server = "127.0.0.1";

        private UdpHelper UdpPort = new UdpHelper();

        public DataTransEvent ProgressUpdateHandler;

        public DataTransEvent UpdateSMSHandler;

        public bool IsSendingData = false;

        public bool IsConnect
        {
            get { return UdpPort.connected; }
        }


        #endregion

        #region TX
        public void SendDataPackage(byte[] bufBytes, string fileName, ref bool IntrFlag)
        {
            lock (txSync)
            {
                DataPkg txData = new DataPkg();

                //TODO: check again might not need to reset again
                TXParam.txDataBuffer.Clear();

                //HEADER
                TXParam.txHeader.PD_Subtype         = PackageType.PT_HEADER_FILE;                     //0 - SMS, 1 - FILE, 2 - CTRL
                TXParam.txHeader.PD_FileName        = Path.GetFileName(fileName); //name 12 character  
                TXParam.txHeader.PD_NumOfPkg        = Convert.ToUInt32(Math.Ceiling(Convert.ToDouble(bufBytes.Length) / Convert.ToDouble(SysDef.DEF_DATA_PKG_PAYLOAD_LENGTH)));
                TXParam.txHeader.Buf                = new byte[SysDef.DEF_HEADER_PKG_LENGTH];

                //PAYLOAD
                txData.PD_Subtype = PackageType.PT_DATA_FILE;
                txData.Payload = bufBytes;

                TXParam.txDataBuffer.Add(txData);
            }
            IsSendingData = true;

            try
            {
	            SendHeader(ref IntrFlag);
	
	            SendData(ref IntrFlag);
            }
            catch (System.Exception ex)
            {
            	
            }

            //IsSendingData = false;
        }

        public void SendHeader(ref bool IntrFlag)
        {
            if (IntrFlag)
                return;
            HeaderPacking(TXParam.txHeader);

            //Send data.
            DataTransSendFunc(TXParam.txHeader.Buf, SysDef.DEF_HEADER_PKG_LENGTH);
        }

        public void HeaderPacking(HeaderPkg header)
        {
            header.Buf[SysDef.DEF_SUBTYPE_OFFSET] = (byte)(PackageType.PT_HEADER_FILE);

            unchecked
            {
                //Add Number of Packet - Big Endien
                header.Buf[SysDef.DEF_HEADER_NUMOFPKG_OFFSET  ] = (byte)(header.PD_NumOfPkg >> 24);
                header.Buf[SysDef.DEF_HEADER_NUMOFPKG_OFFSET+1] = (byte)(header.PD_NumOfPkg >> 16);
                header.Buf[SysDef.DEF_HEADER_NUMOFPKG_OFFSET+2] = (byte)(header.PD_NumOfPkg >> 8);
                header.Buf[SysDef.DEF_HEADER_NUMOFPKG_OFFSET+3] = (byte)(header.PD_NumOfPkg);
            }

            //Add FielName (25byte) - if fileName.Length >25 is trimed.
            if (header.PD_FileName.Length > 25) // NOTE :hard code
            {
                string temp;
                string extfile = Path.GetExtension(header.PD_FileName);
                temp = header.PD_FileName.Substring(0, 25 - extfile.Length);
                header.PD_FileName = temp + extfile;
            }
            byte[] filename = (header.PD_FileName != null) ? Encoding.ASCII.GetBytes(header.PD_FileName) : Encoding.ASCII.GetBytes("TEMP.TMP");
            Buffer.BlockCopy(filename, 0, header.Buf, SysDef.DEF_HEADER_NUMOFPKG_OFFSET + 4, filename.Length);

            unchecked
            {
                header.PD_Length = SysDef.DEF_HEADER_NUMOFPKG_LENGTH + filename.Length;

                // Length - Big Endien
                header.Buf[SysDef.DEF_LENGTH_OFFSET    ] = (byte)(header.PD_Length >> 24);
                header.Buf[SysDef.DEF_LENGTH_OFFSET + 1] = (byte)(header.PD_Length >> 16);
                header.Buf[SysDef.DEF_LENGTH_OFFSET + 2] = (byte)(header.PD_Length >> 8);
                header.Buf[SysDef.DEF_LENGTH_OFFSET + 3] = (byte)(header.PD_Length);
            }
            //Add Crc16 for header packet
//             crc16.AppendCode(header.Buf, 0, SUB_HEADER_PACKET_SIZE, false);

        }

        public void SendData(ref bool IntrFlag)
        {
            lock (txSync)
            {
                DataPkg txPacket = TXParam.txDataBuffer[0];
                Int32 currentLength = 0;
                Int32 totalLength = (Int32)txPacket.Payload.Length;

                bool isFirstPacketToSend = true;

                UInt32 noPkt = TXParam.txHeader.PD_NumOfPkg;

                for (int txPacketCount = 0; txPacketCount < noPkt; txPacketCount++)
                {
                    if (IntrFlag)
                        break;

                    if (totalLength > SysDef.DEF_DATA_PKG_PAYLOAD_LENGTH)
                    {
                        currentLength = SysDef.DEF_DATA_PKG_PAYLOAD_LENGTH;
                        totalLength = totalLength - currentLength;
                    }
                    else
                    {
                        currentLength = totalLength;
                    }
                    if ((isFirstPacketToSend ) || waitSignal(SysDef.REQUEST_TO_SEND_TIMEOUT)) //-- FM ignore delay for first packet
                    {
                        SendDataPacket(txPacket, noPkt, txPacketCount, currentLength);

                        try
                        {
	                        UpdateProgress((int)((txPacketCount+1) * 100 / noPkt));
                        }
                        catch (System.Exception ex)
                        {
                        	
                        }

                        isFirstPacketToSend = false;
                    }
                    else
                    {
                        //TODO: check again, might not need to reset in here
                        TXParam.txDataBuffer.Remove(txPacket);

                        return;
                    }
                }// end for send all window
            }
        }

        public void SendDataPacket(DataPkg txPacket, UInt32 noPkt, int txPktCnt, Int32 currentLength)
        {
            byte[] dataToSend = DataPacking(txPacket, noPkt, txPktCnt, currentLength);
            if (dataToSend != null)
            {
                //Send data in here
                DataTransSendFunc(dataToSend, dataToSend.Length);
            }
        }

        public byte[] DataPacking(DataPkg txPacket, uint noPkt, int txPktCnt, int currentLength)
        {
            byte[] dataToSend = new byte[SysDef.DEF_DATA_PKG_LENGTH];

            dataToSend[SysDef.DEF_SUBTYPE_OFFSET   ]    = (byte)(PackageType.PT_DATA_FILE);

            dataToSend[SysDef.DEF_LENGTH_OFFSET    ]    = (byte)(currentLength >> 24);
            dataToSend[SysDef.DEF_LENGTH_OFFSET + 1]    = (byte)(currentLength >> 16);
            dataToSend[SysDef.DEF_LENGTH_OFFSET + 2]    = (byte)(currentLength >> 8);
            dataToSend[SysDef.DEF_LENGTH_OFFSET + 3]    = (byte)(currentLength);

            //Add Payload
            Buffer.BlockCopy(txPacket.Payload, txPktCnt * SysDef.DEF_DATA_PKG_PAYLOAD_LENGTH, dataToSend, SysDef.DEF_LENGTH_OFFSET + 4, currentLength);
            
            //Add Crc16 for payload
            //crc16.AppendCode(dataToSend, 6, currentLength + CRC_SIZE, false);

            return dataToSend;
        }

        public void SendCtrl(byte Ctrl, byte SubCtrl, byte[] dat)
        {

        }

        public void SendSMS(byte[] sms)
        {
            byte[] sendArr = new byte[sms.Length + SysDef.DEF_LENGTH_BYTES + 1 + SysDef.DEF_CRC_LENGTH];

            sendArr[SysDef.DEF_SUBTYPE_OFFSET] = (byte)(PackageType.PT_SMS);

            sendArr[SysDef.DEF_LENGTH_OFFSET    ] = (byte)(sms.Length >> 24);
            sendArr[SysDef.DEF_LENGTH_OFFSET + 1] = (byte)(sms.Length >> 16);
            sendArr[SysDef.DEF_LENGTH_OFFSET + 2] = (byte)(sms.Length >> 8);
            sendArr[SysDef.DEF_LENGTH_OFFSET + 3] = (byte)(sms.Length);

            //Add Payload
            Buffer.BlockCopy(sms, 0, sendArr, SysDef.DEF_LENGTH_OFFSET + 4, sms.Length);


            DataTransSendFunc(sendArr, sendArr.Length);


            string str = UdpPort.SenderPort.ToString();
            str += " : ";
            str += Encoding.ASCII.GetString(sms, 0, sms.Length).TrimEnd(new char[] { '\0' });
            str += "\n";

            UpdateSMS(str);
        }

        private void DataTransSendFunc(byte[] sendArr,int len)
        {
            UdpPort.Send(sendArr, len);
        }

        public bool waitSignal(int timeout)
        {
            bool result = false;
            result = readyToSend.WaitOne(20);
            readyToSend.Reset();
            return true;// result;
        }

        #endregion

        #region RX

        public void DataRcvEventHandler(UdpHelper handle, object sender)
        {
            byte[] data = sender as byte[];

            PackageType Subtype = (PackageType)data[SysDef.DEF_SUBTYPE_OFFSET];
            int len             = (int)(data[SysDef.DEF_LENGTH_OFFSET    ] << 24) +
                                  (int)(data[SysDef.DEF_LENGTH_OFFSET + 1] << 16) +
                                  (int)(data[SysDef.DEF_LENGTH_OFFSET + 2] << 8) +
                                  (int)(data[SysDef.DEF_LENGTH_OFFSET + 3]);

            switch(Subtype)
            {
                case PackageType.PT_SMS:
                    // Display SMS in richTextBox
                    byte[] Payload         = new byte[len];
                    Buffer.BlockCopy(data, SysDef.DEF_DATA_OFFSET, Payload, 0, len);
                    ProcessSMSPacket(Payload);
                    break;

                case PackageType.PT_HEADER_FILE:
                    RXParam.ResetAllParam();

                    RXParam.rxHeader.PD_Subtype  = Subtype;
                    RXParam.rxHeader.PD_Length   = len;
                    RXParam.rxHeader.PD_NumOfPkg =  (UInt32)(data[SysDef.DEF_HEADER_NUMOFPKG_OFFSET    ] << 24) +
                                                    (UInt32)(data[SysDef.DEF_HEADER_NUMOFPKG_OFFSET + 1] << 16) +
                                                    (UInt32)(data[SysDef.DEF_HEADER_NUMOFPKG_OFFSET + 2] << 8) +
                                                    (UInt32)(data[SysDef.DEF_HEADER_NUMOFPKG_OFFSET + 3]);
                    RXParam.rxHeader.PD_FileName = Encoding.ASCII.GetString(data, SysDef.DEF_HEADER_FILENAME_OFFSET, len - SysDef.DEF_HEADER_NUMOFPKG_LENGTH).TrimEnd(new char[] { '\0' });
                    ProcessHeaderPacket();
                    
                    break;

                case PackageType.PT_DATA_FILE:
                    DataPkg pkg         = new DataPkg();
                    pkg.PD_Subtype      = Subtype;
                    pkg.PD_Length       = len;
                    pkg.Payload         = new byte[len];

                    Buffer.BlockCopy(data, SysDef.DEF_DATA_OFFSET, pkg.Payload, 0, len);
                    
                    RXParam.rxDataBuffer.Add(pkg);

                    ProcessDataPacket();
                    break;
                case PackageType.PT_CTRL:
                    
                    // Not Implement yet
                    break;
                default:
                    Log("Unknown SubType " + data[0].ToString("X2"));
                    break;
            }
        }

        public void ProcessHeaderPacket()
        {
            RXParam.HeaderSet = true;

        }

        public void ProcessSMSPacket(byte[] inputArr)
        {
            string str = UdpPort.ListenerPort.ToString();

            str += " : ";
            str += Encoding.ASCII.GetString(inputArr, 0, inputArr.Length).TrimEnd(new char[] { '\0' });
            str += "\n";
            UpdateSMS(str);
        }

        public void ProcessDataPacket()
        {
            while (RXParam.rxDataBuffer.Count > 0)
            {
                DataPkg rxPacket = RXParam.rxDataBuffer[0];

                if (!RXParam.HeaderSet)
                {
                    Log("Header packet does not get done. " + rxPacket.ToString());
                    RXParam.rxDataBuffer.Remove(rxPacket);
                    continue;
                }
                RXParam.rxDataStream.Write(rxPacket.Payload, 0, rxPacket.PD_Length);
                RXParam.rxPkgCount++;
                try
                {
	                UpdateProgress((int)(RXParam.rxPkgCount * 100 / RXParam.rxHeader.PD_NumOfPkg));
                }
                catch (System.Exception ex)
                {
                	
                }
                if (RXParam.rxHeader.PD_NumOfPkg == RXParam.rxPkgCount)
                {
                    Log("Receive Done!");
                    // Save data to file
                    // file name
                    // RXParam.rxHeader.PD_FileName;
                    // data 
                    // RXParam.rxDataStream
                    string path = string.Empty;

                    if (SaveFolderPath == "")
                    {
                        path = RXParam.rxHeader.PD_FileName;
                    }
                    else
                    {
                        path = SaveFolderPath + "\\" + RXParam.rxHeader.PD_FileName;
                    }

                    using(FileStream fs = new FileStream(path,FileMode.Create))
                    {
                        RXParam.rxDataStream.WriteTo(fs);
                        fs.Close();
                    }

                }
                RXParam.rxDataBuffer.Remove(rxPacket);
            }
        }

        private void UpdateProgress(int p)
        {
            if(ProgressUpdateHandler != null)
            {
                ProgressUpdateHandler(p);
            }
        }

        private void UpdateSMS(string s)
        {
            if(UpdateSMSHandler != null)
            {
                UpdateSMSHandler(s);
            }
        }

        #endregion

        public void Log(string ex)
        {

        }

        public void ConnectToUDPPort(string RecvPort, string SendPort)
        {
            UdpPort.DataRecevivedEvent += new UdpHelper.NewUDPEventHandler(DataRcvEventHandler);
            try
            {
                UdpPort.Connect(IPA_Server, RecvPort, SendPort);
            }
            catch (System.Exception ex)
            {
                Log("Cannot connect to " + IPA_Server + " " + RecvPort + "/" + SendPort + "\n" + ex.Message);
            }
        }

        public void Disconnect()
        {

        }

    }

    public class _RXParam
    {
        public MemoryStream rxDataStream = new MemoryStream();

        public List<DataPkg> rxDataBuffer = new List<DataPkg>();  //Shared Data Recevice Buffer        
        public HeaderPkg rxHeader = new HeaderPkg();
        public DataPkg dataRx = new DataPkg();
        public UInt32 rxPkgCount = 0;

        // Signal when get header success
        public bool HeaderSet = false;


        public _RXParam()
        {

        }

        public void ResetAllParam()
        {
            rxDataStream.SetLength(0);
            rxDataBuffer.Clear();
            rxPkgCount = 0;
            HeaderSet = false;
        }
    }

    public class _TXParam
    {
        public List<DataPkg> txDataBuffer = new List<DataPkg>();  //Shared Data Transmit Buffer
        public HeaderPkg txHeader = new HeaderPkg();


        public _TXParam()
        {

        }

        public void ResetAllParam()
        {
            txDataBuffer.Clear();
        }
    }
}
