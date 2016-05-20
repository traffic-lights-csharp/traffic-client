using System;
using System.Windows.Forms;
using System.Drawing;

namespace traffic_client
{
	public class MainForm : Form
	{
		private System.ComponentModel.IContainer _components = null;

		private Timer _updateTimer;
		private TextBox _connectionAddressBox;
		private TextBox _connectionPortBox;
		private Button _connectButton;

		private Button _addCarButton;
		private Label _carsWaitingLabel;
		private Button _lightButton;

		private Client _parent;

		public MainForm(Client parent)
		{
			this._parent = parent;
			this.Init();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this._components != null)
				this._components.Dispose();
			base.Dispose(disposing);
		}

		public void Init()
		{
			this.Name = "traffic-client";
			this.Text = "Traffic Client";
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(640, 480);

			this._components = new System.ComponentModel.Container();

			this._updateTimer = new Timer();
			this._updateTimer.Interval = 100;
			this._updateTimer.Tick += this.UpdateTimerTickEvent;
			this._updateTimer.Start();

			this._connectionAddressBox = new TextBox();
			this._connectionAddressBox.Name = "ConnectionAddressBox";
			this._connectionAddressBox.Location = new Point(32, 32);
			this._connectionAddressBox.Size = new Size(128, 32);
			this._connectionAddressBox.Text = "127.0.0.1";

			this._connectionPortBox = new TextBox();
			this._connectionPortBox.Name = "ConnectionPortBox";
			this._connectionPortBox.Location = new Point(128 + 32, 32);
			this._connectionPortBox.Size = new Size(64, 32);
			this._connectionPortBox.Text = "5903";

			this._connectButton = new Button();
			this._connectButton.Name = "ConnectButton";
			this._connectButton.Location = new Point(256, 32);
			this._connectButton.Size = new Size(64, 32);
			this._connectButton.Text = "Connect";
			this._connectButton.UseVisualStyleBackColor = false;
			this._connectButton.Click += new EventHandler(this.ConnectButtonClickEvent);

			this._addCarButton = new Button();
			this._addCarButton.Name = "AddCarButton";
			this._addCarButton.Location = new Point(32, 128);
			this._addCarButton.Size = new Size(64, 32);
			this._addCarButton.Text = "Add Car";
			this._addCarButton.UseVisualStyleBackColor = false;
			this._addCarButton.Click += new EventHandler(this.AddCarButtonClickEvent);

			this._carsWaitingLabel = new Label();
			this._carsWaitingLabel.Text = "Cars waiting: 0";
			this._carsWaitingLabel.Location = new Point(128, 128);

			this._lightButton = new Button();
			this._lightButton.Name = "LightButton";
			this._lightButton.Location = new Point(256, 128);
			this._lightButton.Size = new Size(64, 64);
			this._lightButton.UseVisualStyleBackColor = false;

			this.Controls.Add(this._connectionAddressBox);
			this.Controls.Add(this._connectionPortBox);
			this.Controls.Add(this._connectButton);

			this.Controls.Add(this._addCarButton);
			this.Controls.Add(this._carsWaitingLabel);
			this.Controls.Add(this._lightButton);

			this.ResumeLayout(false);
		}

		private void AddCarButtonClickEvent(object sender, EventArgs e)
		{
			Console.WriteLine("Clicked 'Add Car' button!");
			this._parent.AddCarEvent(1);
		}

		private void ConnectButtonClickEvent(object sender, EventArgs e)
		{
			Console.WriteLine("Clicked 'Connect' button!");
			this._parent.Connect(this._connectionAddressBox.Text, Convert.ToInt32(this._connectionPortBox.Text));
		}

		private void UpdateTimerTickEvent(object sender, EventArgs e)
		{
			this._carsWaitingLabel.Text = "Cars waiting: " + this._parent.TrafficData.CarsWaiting.ToString();

			switch (this._parent.TrafficData.LightColour)
			{
			case 0:
				this._lightButton.BackColor = Color.Red;
				break;

			case 1:
				this._lightButton.BackColor = Color.Orange;
				break;

			case 2:
				this._lightButton.BackColor = Color.Green;
				break;

			default:
				this._lightButton.BackColor = Color.White;
				break;
			}
		}
	}
}