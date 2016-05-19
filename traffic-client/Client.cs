using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;

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

			byte[] message = new byte[]{(byte)'H', (byte)'e', (byte)'l', (byte)'l', (byte)'o', (byte)4};
			this._networkStream.Write(message, 0, message.Length);
		}

		public void NetworkListener()
		{
			// TODO : Make this neater
			while (true)
			{
				byte[] packet = this.GetBytePacket(this._networkStream);

				StringBuilder builder = new StringBuilder();
				foreach (byte b in packet)
					builder.Append(b);

				Console.WriteLine("Got message from server : {0}", builder.ToString());
			}
		}

		public byte[] GetBytePacket(NetworkStream stream)
		{
			List<byte> tmpList = new List<byte>();

			int readByte = 0x00;
			while ((readByte = stream.ReadByte()) == -1) { };
			tmpList.Add((byte)readByte);

			while ((readByte = stream.ReadByte()) != 4)
				tmpList.Add((byte)readByte);

			return tmpList.ToArray();
		}
	}
}