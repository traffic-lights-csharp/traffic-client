using System;

namespace traffic_client
{
	public class TrafficData
	{
		private int _carsWaiting = 0;
		private int _lightColour = 0; // 0 = Red, 1 = Amber, 2 = Green

		public TrafficData()
		{
			
		}

		public int CarsWaiting
		{
			get { return this._carsWaiting; }
			set { this._carsWaiting = value; }
		}

		public int LightColour
		{
			get { return this._lightColour; }
			set { this._lightColour = value; }
		}
	}
}

