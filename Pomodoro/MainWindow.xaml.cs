using Microsoft.SqlServer.Server;
using NAudio.Wave;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Compression;
using System.Media;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.IO;


namespace Pomodoro
{
	public partial class MainWindow : Window, INotifyPropertyChanged
	{

		#region Declaration
		private string currentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

		private TimeSpan pomodoroTime = TimeSpan.FromMinutes(25);
		private TimeSpan shortBreakTime = TimeSpan.FromMinutes(5);
		private TimeSpan longBreakTime = TimeSpan.FromMinutes(15);
		private int cyclesNumber = 4;
		private int pomodoroCompleted = 0;

		private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;

        private string state;
		private bool isTimerRunning;
		private TimeSpan remainingTime;

		private DispatcherTimer timer;
        public event PropertyChangedEventHandler PropertyChanged;
		private SolidColorBrush mainColorBrush;
		private SolidColorBrush secondaryColorBrush;
		private SolidColorBrush thirdColorBrush;
		private SolidColorBrush borderColorBrush;
		private SolidColorBrush transparentColorBrush;

		private SolidColorBrush mainColorDarkBrush;
		private SolidColorBrush secondaryColorDarkBrush;
		private SolidColorBrush thirdColorDarkBrush;
		private SolidColorBrush borderColorDarkBrush;
		private SolidColorBrush transparentColorDarkBrush;

		private SolidColorBrush mainColorDarkerBrush;
		private SolidColorBrush secondaryColorDarkerBrush;
		private SolidColorBrush thirdColorDarkerBrush;
		private SolidColorBrush borderColorDarkerBrush;
		#endregion

		#region Initialisation
		public MainWindow()
		{
			InitializeComponent();
			InitializeTimer();
			InitializeData();
			InitializeColors();
			InitializeVersion();
			//CheckUpdate();
		}

		private void InitializeData()
		{
            DataContext = this;
            state = "pomodoro";
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(Timer_Tick);
            pomodoroValue.Text = pomodoroTime.ToString(@"mm");
            shortBreakValue.Text = shortBreakTime.ToString(@"mm");
			longBreakValue.Text = longBreakTime.ToString(@"mm");
			TimerPomodoroText.Text = pomodoroTime.ToString(@"mm\:ss");
			TimerShortBreakText.Text = shortBreakTime.ToString(@"mm\:ss");
			TimerLongBreakText.Text = longBreakTime.ToString(@"mm\:ss");
			cycleValue.Text = cyclesNumber.ToString();
			remainingTime = pomodoroTime;
		}

		private void InitializeColors()
		{
			mainColorBrush = (SolidColorBrush)FindResource("mainColor");
			secondaryColorBrush = (SolidColorBrush)FindResource("secondaryColor");
			thirdColorBrush = (SolidColorBrush)FindResource("thirdColor");
			borderColorBrush = (SolidColorBrush)FindResource("borderColor");
			transparentColorBrush = (SolidColorBrush)FindResource("transparentColor");

			mainColorDarkBrush = (SolidColorBrush)FindResource("mainColorDark");
			secondaryColorDarkBrush = (SolidColorBrush)FindResource("secondaryColorDark");
			thirdColorDarkBrush = (SolidColorBrush)FindResource("thirdColorDark");
			borderColorDarkBrush = (SolidColorBrush)FindResource("borderColorDark");

			mainColorDarkerBrush = (SolidColorBrush)FindResource("mainColorDarker");
			secondaryColorDarkerBrush = (SolidColorBrush)FindResource("secondaryColorDarker");
			thirdColorDarkerBrush = (SolidColorBrush)FindResource("thirdColorDarker");
			borderColorDarkerBrush = (SolidColorBrush)FindResource("borderColorDarker");
		}
		#endregion

		#region Update
		private void InitializeVersion()
		{
			VersionArea.Text = currentVersion;
			Console.WriteLine("Pomodoro version : " + currentVersion);
		}

		private void CheckUpdate()
		{
			//WebClient webClient = new WebClient();
			//var client = new WebClient();

			//if (!webClient.DownloadString("https://raw.githubusercontent.com/Boutzi/pomodoro/main/Pomodoro_Setup/latest/update.txt").Contains("1.3.26"))
			//{
			//	if (MessageBox.Show("New update available ! Do you want to install it ?", "Pomodoro App", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
			//	{
			//		try
			//		{
			//			Console.WriteLine("Dowloading update...");
			//			if (File.Exists(@".\Pomodoro_Setup.msi")) { File.Delete(@".\Pomodoro_Setup.msi"); }
			//			client.DownloadFile("https://github.com/Boutzi/pomodoro/raw/main/Pomodoro_Setup/latest/Pomodoro_Setup.zip", @"Pomodoro_Setup.zip");
			//			string zipPath = @".\Pomodoro_Setup.zip";
			//			string extractPath = @".\.";
			//			ZipFile.ExtractToDirectory(zipPath, extractPath);

			//			Process process = new Process();
			//			process.StartInfo.FileName = "msiexec";
			//			process.StartInfo.Arguments = String.Format("/i Pomodoro_Setup.msi");

			//			this.Close();
			//			process.Start();
			//		}
			//		catch
			//		{
			//			Console.WriteLine("Update Error");
			//		}
			//	}
			//}
		}
		#endregion

		#region Timer Functions
		private void Timer_Tick(object sender, EventArgs e)
		{
			if (isTimerRunning)
			{
				remainingTime = remainingTime.Subtract(TimeSpan.FromSeconds(1));
                ResetTimerText();
                if (remainingTime == TimeSpan.Zero)
				{
                    PlaySound(Properties.Resources.end);

					switch (state)
					{
						case "pomodoro":
							pomodoroCompleted++;
							if (pomodoroCompleted >= cyclesNumber)
							{
								ChangeState("longbreak");
							}
							else
							{
								ChangeState("shortbreak");
							}
							break;
						case "shortbreak":
							ChangeState("pomodoro");
							break;
						case "longbreak":
							pomodoroCompleted = 0;
							ChangeState("pomodoro");
                            break;
                    }
					this.Topmost = true;

					if (this.WindowState == WindowState.Minimized)
					{
						this.WindowState = WindowState.Normal;
					}
					this.Activate();
				}
			}
		}

		private void ChangeState(string stateToGo)
		{
            switch (stateToGo)
            {
                case "pomodoro":
					state = "pomodoro";
                    if (int.TryParse(pomodoroValue.Text, out int pomodoroMinutes))
                    {
                        pomodoroTime = TimeSpan.FromMinutes(pomodoroMinutes);
                        remainingTime = pomodoroTime;
                        TimerPomodoroText.Text = remainingTime.ToString(@"mm\:ss");
                        TimerPomodoro.Visibility = Visibility.Visible;
                        TimerShortBreak.Visibility = Visibility.Hidden;
                        TimerLongBreak.Visibility = Visibility.Hidden;
                    }
					///change background
					Border1.Background = mainColorDarkerBrush;
					Border2.Background = mainColorBrush;
					Border3.Background = mainColorDarkBrush;
					Border4.Background = Brushes.Transparent;
					Border5.Background = Brushes.Transparent;
					Border6.Background = mainColorDarkBrush;
					Border7.Background = mainColorDarkBrush;
					Border8.Background = mainColorDarkBrush;
					Border10.Background = mainColorDarkBrush;
					break;
				case "shortbreak":
                    state = "shortbreak";
                    if (int.TryParse(shortBreakValue.Text, out int shortBreakMinutes))
                    {
                        shortBreakTime = TimeSpan.FromMinutes(shortBreakMinutes);
                        remainingTime = shortBreakTime;
                        TimerShortBreakText.Text = remainingTime.ToString(@"mm\:ss");
                        TimerShortBreak.Visibility = Visibility.Visible;
                        TimerPomodoro.Visibility = Visibility.Hidden;
                        TimerLongBreak.Visibility = Visibility.Hidden;
                    }
					///change background
					Border1.Background = secondaryColorDarkerBrush;
					Border2.Background = secondaryColorBrush;
					Border3.Background = Brushes.Transparent;
					Border4.Background = secondaryColorDarkBrush;
					Border5.Background = Brushes.Transparent;
					Border6.Background = secondaryColorDarkBrush;
					Border7.Background = secondaryColorDarkBrush;
					Border8.Background = secondaryColorDarkBrush;
					Border10.Background = secondaryColorDarkBrush;
					break;
				case "longbreak":
                    state = "longbreak";
                    if (int.TryParse(longBreakValue.Text, out int longBreakMinutes))
                    {
						longBreakTime = TimeSpan.FromMinutes(longBreakMinutes);
                        remainingTime = longBreakTime;
                        TimerLongBreakText.Text = remainingTime.ToString(@"mm\:ss");
                        TimerLongBreak.Visibility = Visibility.Visible;
                        TimerPomodoro.Visibility = Visibility.Hidden;
                        TimerShortBreak.Visibility = Visibility.Hidden;
                    }
					///change background
					Border1.Background = thirdColorDarkerBrush;
					Border2.Background = thirdColorBrush;
					Border3.Background = Brushes.Transparent;
					Border4.Background = Brushes.Transparent;
					Border5.Background = thirdColorDarkBrush;
					Border6.Background = thirdColorDarkBrush;
					Border7.Background = thirdColorDarkBrush;
					Border8.Background = thirdColorDarkBrush;
					Border10.Background = thirdColorDarkBrush;
					break;
            }
            StopTimer();
        }

		private void StartTimer()
        {
            timer.Start();
            isTimerRunning = true;
			playPausePlayerButton.IsChecked = true;
			PlaySound(Properties.Resources.start);
		}

		private void PlaySound(System.IO.UnmanagedMemoryStream sound)
        {
			if (Mute.IsChecked == false)
			{
				SoundPlayer soundstart = new SoundPlayer(sound);
				soundstart.Play();
			}
        }

		private void PauseTimer()
		{
			isTimerRunning = false;
			timer.Stop();
		}

		private void StopTimer()
		{
			isTimerRunning = false;
			playPausePlayerButton.IsChecked = false;
			timer.Stop();
			ResetTimerText();
		}

		private void ResetTimerText()
		{
			if(isTimerRunning)
            {
                TimerPomodoroText.Text = remainingTime.ToString(@"mm\:ss");
                TimerLongBreakText.Text = remainingTime.ToString(@"mm\:ss");
                TimerShortBreakText.Text = remainingTime.ToString(@"mm\:ss");
            }
			if (!isTimerRunning)
			{
				switch (state)
				{
					case "pomodoro":
                        TimerPomodoroText.Text = pomodoroTime.ToString(@"mm\:ss");
                        remainingTime = pomodoroTime;
						break;
					case "longbreak":
                        TimerLongBreakText.Text = longBreakTime.ToString(@"mm\:ss");
                        remainingTime = longBreakTime;
						break;
					case "shortbreak":
                        TimerShortBreakText.Text = shortBreakTime.ToString(@"mm\:ss");
                        remainingTime = shortBreakTime;
						break;
				}
			}

        }
		#endregion

		#region Input Focus Functions
		private void PomodoroValue_LostFocus(object sender, RoutedEventArgs e)
		{
			if (int.TryParse(pomodoroValue.Text, out int pomodoroMinutes))
			{
				pomodoroTime = TimeSpan.FromMinutes(pomodoroMinutes);
				if(!isTimerRunning)
                    remainingTime = pomodoroTime;
					TimerPomodoroText.Text = remainingTime.ToString(@"mm\:ss");
                }
		}

		private void ShortBreakValue_LostFocus(object sender, RoutedEventArgs e)
		{
			if (int.TryParse(shortBreakValue.Text, out int shortBreakMinutes))
			{
				shortBreakTime = TimeSpan.FromMinutes(shortBreakMinutes);
                if (!isTimerRunning)
                    remainingTime = shortBreakTime;
                TimerShortBreakText.Text = remainingTime.ToString(@"mm\:ss");
            }
		}

		private void LongBreakValue_LostFocus(object sender, RoutedEventArgs e)
		{
			if (int.TryParse(longBreakValue.Text, out int longBreakMinutes))
			{
				longBreakTime = TimeSpan.FromMinutes(longBreakMinutes);
				if (!isTimerRunning)
					remainingTime = longBreakTime;
				TimerLongBreakText.Text = remainingTime.ToString(@"mm\:ss");
			}
		}

		private void Cycles_LostFocus(object sender, RoutedEventArgs e)
		{
			if (int.TryParse(cycleValue.Text, out int cycles))
			{
				cyclesNumber = cycles;
			}
		}

		#endregion

		#region Button Click Functions
		private void PomodoroMode_Click(object sender, RoutedEventArgs e)
		{
			ChangeState("pomodoro");
		}

		private void ShortBreakMode_Click(object sender, RoutedEventArgs e)
        {
            ChangeState("shortbreak");
        }

		private void LongBreakMode_Click(object sender, RoutedEventArgs e)
        {
            ChangeState("longbreak");
		}
		private void OpenGitHubPage(object sender, RoutedEventArgs e)
		{
			System.Diagnostics.Process.Start("https://github.com/Boutzi");
		}
		#endregion

		#region Player Functions
		protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            StartTimer();
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            PauseTimer();
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            StopTimer();
            remainingTime = remainingTime.Add(TimeSpan.FromSeconds(1));
            StartTimer();
		}
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            StopTimer();
        }
        #endregion

        #region Window Configuration
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
		{
			WindowState = WindowState.Minimized;
		}

		private void CloseButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void MouseDragBar(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				DragMove();
			}
		}
		#endregion

	}
}
