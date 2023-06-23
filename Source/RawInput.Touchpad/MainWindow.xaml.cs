using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RawInput.Touchpad
{
	public partial class MainWindow : Window
	{
		public bool TouchpadExists
		{
			get { return (bool)GetValue(TouchpadExistsProperty); }
			set { SetValue(TouchpadExistsProperty, value); }
		}
		public static readonly DependencyProperty TouchpadExistsProperty =
			DependencyProperty.Register("TouchpadExists", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));

		public string TouchpadContacts
		{
			get { return (string)GetValue(TouchpadContactsProperty); }
			set { SetValue(TouchpadContactsProperty, value); }
		}
		public static readonly DependencyProperty TouchpadContactsProperty =
			DependencyProperty.Register("TouchpadContacts", typeof(string), typeof(MainWindow), new PropertyMetadata(null));
		public MainWindow()
		{
			InitializeComponent();
		}

		private HwndSource _targetSource;
		private readonly List<string> _log = new();
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetDesktopWindow();
        protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);
			_targetSource = PresentationSource.FromVisual(this) as HwndSource;
            _targetSource?.AddHook(WndProc);

			mouseProcessor = new MouseEventProcessor();

			TouchpadExists = TouchpadHelper.Exists();

			tpsx = 0;tpsy=0 ;tpex = 1920;tpey = 1080;scsx = 0;scsy = 0;scex = 1920;scey = 1080;tpgx = 1920;tpgy = 1080;scgx = 1920;scgy = 1080;

			IntPtr foregroundWindow = GetDesktopWindow();
			_log.Add($"Precision touchpad exists: {TouchpadExists}");

			if (TouchpadExists)
			{
				bool success;
				if (_targetSource != null)
					 success = TouchpadHelper.RegisterInput(_targetSource.Handle);
				else success = false;

				_log.Add($"Precision touchpad registered: {success}");
			}
		}
		private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			switch (msg)
			{
				case TouchpadHelper.WM_INPUT:
					foreach(TouchpadContact x in TouchpadHelper.ParseInput(lParam))
					{
						if (x.ContactId == 0)
						{
							try
							{
								int X, Y;
								X = (int)((decimal)(x.X - tpsx) / tpgx * scgx) + scsx;
								Y = (int)((decimal)(x.Y - tpsy) / tpgy * scgy) + scsy;
                                if(X<=scex && Y<=scey && X>=scsx && Y>=scsy) mouseProcessor.MoveCursor(X, Y);
								TouchpadContacts = string.Join(Environment.NewLine, x.ToString());

                            }
							catch (Exception e)
							{
								MessageBox.Show(e.ToString());
							}

							_log.Add("---");
							_log.Add(TouchpadContacts);
						}
                    }

					break;
			}
			return IntPtr.Zero;
		}

		private void Copy_Click(object sender, RoutedEventArgs e)
		{
			Clipboard.SetText(string.Join(Environment.NewLine, _log));
		}

		private MouseEventProcessor mouseProcessor;
		private int tpsx, tpsy, tpex, tpey, scsx, scsy, scex, scey, tpgx, tpgy, scgx, scgy;
		// s=Start, e=End, g=Gap, tp=Touchpad, sc=Screen
        private void Start_Click(object sender, RoutedEventArgs e)
        {
			string[] pos = this.MappingString.Text.Split('|');
            tpsx = int.Parse(pos[0].Split(',')[0]);
            tpsy = int.Parse(pos[0].Split(',')[1]);
            tpex = int.Parse(pos[1].Split(',')[0]);
            tpey = int.Parse(pos[1].Split(',')[1]);
            scsx = int.Parse(pos[2].Split(',')[0]);
            scsx = int.Parse(pos[2].Split(',')[1]);
            scex = int.Parse(pos[3].Split(',')[0]);
            scey = int.Parse(pos[3].Split(',')[1]);
			tpgx = tpex - tpsx;
			tpgy = tpey - tpsy;
			scgx = scex - scsx;
			scgy = scey - scsy;
            mouseProcessor.ToggleEnabled();
			return;
        }
    }
}