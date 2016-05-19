using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace traffic_client
{
	public class Client
	{
		private Thread _networkListenerThread;
		private TcpClient _networkClient;
		private NetworkStream _networkStream;

		public Client()
		{
			this._networkListenerThread = new Thread(this.NetworkListener);
		}

		public void Run()
		{
			this._networkClient = new TcpClient("127.0.0.1", 5903);
			this._networkStream = this._networkClient.GetStream();
			this._networkListenerThread.Start();

			byte[] message = new byte[]{(byte)'H', (byte)'e', (byte)'l', (byte)'l', (byte)'o'};
			this._networkStream.Write(message, 0, message.Length);
		}

		public void NetworkListener()
		{
			// TODO : Make this neater
			while (true)
			{
				int result = -1;
				string message = "";

				while (result == -1)
				{
					result = this._networkStream.ReadByte();

					if (result != -1)
					{
						message += (char)result;
						break;
					}
				}

				while (result != -1)
				{
					result = this._networkStream.ReadByte();
					if (result == -1)
						break;
					else
					{
						message += (char)result;
						Console.WriteLine("Got {0}", (int)result);
					}
				}

				Console.WriteLine("Got message from server : {0}", message.ToString());
			}
		}
	}
}