using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer.Protocol
{
    public class UdpHelper
    {
        public bool connected = false;
        private System.Net.Sockets.UdpClient udpSender;
        private System.Net.Sockets.UdpClient udpListener;

        public int SenderPort = 0;
        public int ListenerPort = 0;
        public int MsgSendCount = 0;
        public int MsgRecvCount = 0;


        // event
        public delegate void NewUDPEventHandler(UdpHelper handle, object sender);
        /// <summary>
        /// 
        /// </summary>
        public event NewUDPEventHandler DataRecevivedEvent;

        #region UDP streaming
        //////////////////////////////////////////////////////////////////////////
        // UDP streaming
        //////////////////////////////////////////////////////////////////////////
        private void Connect(IPEndPoint endPointListener, IPEndPoint endPointSender)
        {
            udpSender = new System.Net.Sockets.UdpClient();
            udpListener = new System.Net.Sockets.UdpClient();

            // To allow us to talk to ourselves for test purposes:
            // http://stackoverflow.com/questions/687868/sending-and-receiving-udp-packets-between-two-programs-on-the-same-computer
            udpListener.Client.SetSocketOption(System.Net.Sockets.SocketOptionLevel.Socket, System.Net.Sockets.SocketOptionName.ReuseAddress, true);
            udpListener.Client.Bind(endPointListener);

            udpSender.Connect(endPointSender);
            connected = true;
            var state = new ListenerThreadState { EndPoint = endPointListener };
            System.Threading.ThreadPool.QueueUserWorkItem(ListenerThread, state);

            SenderPort = endPointSender.Port;
            ListenerPort = endPointListener.Port;
        }

        public void Disconnect()
        {
            if (connected)
            {
                connected = false;
                udpSender.Close();
                udpListener.Close();
            }
        }


        public void Send(byte[] b, int len)
        {
            if(connected)
                udpSender.Send(b, len);
        }


        class ListenerThreadState
        {
            public IPEndPoint EndPoint { get; set; }
        }

        private void ListenerThread(object state)
        {
            var listenerThreadState = (ListenerThreadState)state;
            var endPoint = listenerThreadState.EndPoint;
            try
            {
                while (connected)
                {
                    byte[] b = udpListener.Receive(ref endPoint);
                    if (DataRecevivedEvent != null)
                    {
                        DataRecevivedEvent(this, b);
                    }
                }
            }
            catch (System.Net.Sockets.SocketException)
            {
                // usually not a problem - just means we have disconnected
            }
        }

        public void TongleConnect(string IPAddressText, string RecvPort, string SendPort)
        {
            if (!connected)
            {
                IPEndPoint endPointListener = new IPEndPoint(IPAddress.Parse(IPAddressText), int.Parse(RecvPort));
                IPEndPoint endPointSender = new IPEndPoint(IPAddress.Parse(IPAddressText), int.Parse(SendPort));

                Connect(endPointListener, endPointSender);
            }
            else
            {
                Disconnect();
            }
        }

        public void Connect(string IPAddressText, string RecvPort, string SendPort)
        {
            if (connected)
            {
                Disconnect();
            }
            IPEndPoint endPointListener = new IPEndPoint(IPAddress.Parse(IPAddressText), int.Parse(RecvPort));
            IPEndPoint endPointSender = new IPEndPoint(IPAddress.Parse(IPAddressText), int.Parse(SendPort));

            Connect(endPointListener, endPointSender);

        }

        #endregion

    }
}
