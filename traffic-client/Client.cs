using System;
using System.Threading;

namespace traffic_client
{
	public class Client
	{
		private Thread _networkListenerThread;
		private Mutex _localMutex;

		public Client()
		{
			this._localMutex = new Mutex();

			this._networkListenerThread = new Thread(this.NetworkListener);
			this._networkListenerThread.Start();
		}

		public void Run()
		{
			// Do nothing for now
		}

		public void NetworkListener()
		{
			for (int i = 0; i < 10; i++)
			{
				Thread.Sleep(1000);
				Console.WriteLine("Hello from the listener thread!");
			}
		}
	}
}

