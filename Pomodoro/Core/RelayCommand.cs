using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pomodoro.Core
{
	public class RelayCommand : ICommand
	{
		private Action<object> _execute;
		private Func<object, bool> _canExectute;

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public RelayCommand(Action<object> execute, Func<object, bool> canExectute = null)
		{
			_execute = execute;
			_canExectute = canExectute;
		}

		public bool CanExecute(object parameter)
		{
			return _canExectute == null || _canExectute(parameter);
		}
		public void Execute(object parameter)
		{
			_execute(parameter);
		}
	}
}
