﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    public class TickerServer : TCPServer
    {
        public TickerServer(ushort port)
            : base(port)
        {

        }

        public override void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;

            Log.Info("Client connected to TickerServer.");

            byte[] message = new byte[256];

            // read data
            NetworkStream stream = client.GetStream();

            int bytesRead;

            int requestCount = 0;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    bytesRead = stream.Read(message, 0, message.Length);
                }
                catch
                {
                    Log.Info("Client disconnected from TickerServer.");
                    break;
                }

                if (bytesRead == 0)
                {
                    break;
                }
                else
                {
                    requestCount++;

                    Log.Info(string.Format("Received ticker request {0}.", requestCount));
                    File.WriteAllBytes(string.Format("ticker-request-{0}.bin", requestCount), message);
                }
            }

            client.Close();
        }
    }
}
