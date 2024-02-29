using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Interactivity;


namespace Pomodoro.Core
{
	public class TextBoxLostFocusBehavior : Behavior<TextBox>
	{
		protected override void OnAttached()
		{
			AssociatedObject.LostFocus += AssociatedObject_LostFocus;
			base.OnAttached();
		}

		protected override void OnDetaching()
		{
			AssociatedObject.LostFocus -= AssociatedObject_LostFocus;
			base.OnDetaching();
		}

		private void AssociatedObject_LostFocus(object sender, System.Windows.RoutedEventArgs e)
		{
			if (int.TryParse(AssociatedObject.Text, out int value))
			{
				// Mettez à jour votre logique ici
				Console.WriteLine("TextBox value changed: " + value);
			}
		}
	}
}
