using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Pomodoro.Core;


namespace Pomodoro
{
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		private TimeSpan pomodoroTime = TimeSpan.FromMinutes(25); // Temps par défaut pour le Pomodoro
		private DispatcherTimer timer;
		private bool isTimerRunning;

		public event PropertyChangedEventHandler PropertyChanged;

		public MainWindow()
		{
			InitializeComponent();
			DataContext = this;
			InitializeTimer();
		}

		// Propriété pour le temps restant du Pomodoro
		private TimeSpan remainingTime;
		public TimeSpan RemainingTime
		{
			get { return remainingTime; }
			set
			{
				if (value != remainingTime)
				{
					remainingTime = value;
					OnPropertyChanged(nameof(RemainingTime));
				}
			}
		}

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		// Initialiser le minuteur
		private void InitializeTimer()
		{
			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += Timer_Tick;
			RemainingTime = pomodoroTime;
		}

		// Gestionnaire pour l'événement Tick du minuteur
		private void Timer_Tick(object sender, EventArgs e)
		{
			if (isTimerRunning)
			{
				RemainingTime = RemainingTime.Subtract(TimeSpan.FromSeconds(1));
				if (RemainingTime == TimeSpan.Zero)
				{
					// Le temps du Pomodoro est écoulé
					// Vous pouvez ajouter ici le code pour notifier l'utilisateur ou démarrer une pause automatique, etc.
					StopTimer();
				}
			}
		}

		// Démarrer le minuteur
		private void StartTimer()
		{
			isTimerRunning = true;
			timer.Start();
		}

		// Mettre en pause le minuteur
		private void PauseTimer()
		{
			isTimerRunning = false;
			timer.Stop();
		}

		// Arrêter le minuteur
		private void StopTimer()
		{
			isTimerRunning = false;
			timer.Stop();
			RemainingTime = pomodoroTime; // Réinitialiser le temps restant
		}

		// Gestionnaire pour le clic sur le bouton Play/Pause
		private void ToggleButton_Checked(object sender, RoutedEventArgs e)
		{
			StartTimer();
		}

		// Gestionnaire pour le clic sur le bouton Play/Pause
		private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
		{
			PauseTimer();
		}

		// Gestionnaire pour le clic sur le bouton Restart
		private void Restart_Click(object sender, RoutedEventArgs e)
		{
			StopTimer();
			StartTimer();
		}
		// Gestionnaire pour le clic sur le bouton Restart
		private void Stop_Click(object sender, RoutedEventArgs e)
		{
			StopTimer();
		}


		// Gestionnaire pour l'événement LostFocus de la TextBox PomodoroValue
		private void PomodoroValue_LostFocus(object sender, RoutedEventArgs e)
		{
			if (int.TryParse(PomodoroValue.Text, out int pomodoroMinutes))
			{
				pomodoroTime = TimeSpan.FromMinutes(pomodoroMinutes);
				RemainingTime = pomodoroTime;
			}
		}

		// Gestionnaire pour l'événement LostFocus de la TextBox shortBreakValue
		private void ShortBreakValue_LostFocus(object sender, RoutedEventArgs e)
		{
			if (int.TryParse(shortBreakValue.Text, out int shortBreakMinutes))
			{
				// Mettre à jour la durée de la pause courte
			}
		}

		// Gestionnaire pour l'événement LostFocus de la TextBox longBreakValue
		private void LongBreakValue_LostFocus(object sender, RoutedEventArgs e)
		{
			if (int.TryParse(longBreakValue.Text, out int longBreakMinutes))
			{
				// Mettre à jour la durée de la pause longue
			}
		}





		// Gestionnaire pour le clic sur le bouton Minimize
		private void MinimizeButton_Click(object sender, RoutedEventArgs e)
		{
			WindowState = WindowState.Minimized;
		}

		// Gestionnaire pour le clic sur le bouton Close
		private void CloseButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		// Gestionnaire pour le déplacement de la fenêtre via la barre de titre (dragbar)
		private void MouseDragBar(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				DragMove();
			}
		}
	}
}
