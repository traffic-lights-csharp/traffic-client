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
		bool _connected = false;

		private MainForm _mainForm;

		private TrafficData _trafficData;

		public Client()
		{
			this._trafficData = new TrafficData();

			this._networkListenerThread = new Thread(this.NetworkListener);

			this._mainForm = new MainForm(this);
			this._mainForm.Show();
			System.Windows.Forms.Application.EnableVisualStyles();
		}

		public void Run()
		{
			while (this._mainForm.Visible)
			{
				System.Windows.Forms.Application.DoEvents();
			}

			this.Close();
		}

		public void Connect(string address, int port)
		{
			if (this._connected)
				return;

			this._networkClient = new TcpClient(address, port);
			this._networkStream = this._networkClient.GetStream();
			this._networkListenerThread.Start();
			this._connected = true;

			this.SendString("Hello!");
		}

		public void Close()
		{
			this._networkListenerThread.Abort();
			this._networkStream.Close();
			this._networkClient.Close();
			Console.WriteLine("Aborted network listener");
		}

		public void NetworkListener()
		{
			// TODO : Make this neater
			while (true)
			{
				string packet = this.ReceiveString();

				Console.WriteLine("Got message from server : {0}", packet);
				if (!this.HandleMessage(packet))
					Console.WriteLine("Failed to properly handle message");
			}
		}

		public bool HandleMessage(string message)
		{
			string[] components = message.Split(',');

			if (components.Length <= 0)
				return false;

			switch (components [0])
			{
			case "update-car-count":
				{
					if (components.Length != 2)
						return false;

					this._trafficData.CarsWaiting = Convert.ToInt32(components[1]);
				}
				break;

			case "update-light-colour":
				{
					if (components.Length != 2)
						return false;

					this._trafficData.LightColour = Convert.ToInt32(components[1]);
				}
				break;

			default:
				return false;
			}

			return false;
		}

		public void SendString(string message)
		{
			List<byte> packet = new List<byte>();

			foreach (char c in message)
				packet.Add((byte)c);
			
			packet.Add(4); // EoT delimiter
			this._networkStream.Write(packet.ToArray(), 0, packet.ToArray().Length);
		}

		public string ReceiveString()
		{
			List<byte> tmpList = new List<byte>();

			int readByte = 0x00;
			while ((readByte = this._networkStream.ReadByte()) == -1) { };
			tmpList.Add((byte)readByte);

			while ((readByte = this._networkStream.ReadByte()) != 4)
				tmpList.Add((byte)readByte);

			string builder = "";
			foreach (byte b in tmpList.ToArray())
				builder += (char)b;

			return builder;
		}

		public void AddCarEvent(int number)
		{
			if (this._connected)
				this.SendString("add-car," + number.ToString());
			else
				Console.WriteLine("Cannot send event: Not connected to a server");
		}

		public TrafficData TrafficData
		{
			get { return this._trafficData; }
		}
	}
}
