using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;

namespace CRMAdmin.Behaviors
{
	public sealed class KeyPressBehavior : Behavior<UIElement>
	{
		public ICommand Command
		{
			get => GetValue(CommandProperty) as ICommand;
			set => SetValue(CommandProperty, value);
		}
		public object CommandParameter
		{
			get => GetValue(CommandParameterProperty);
			set => SetValue(CommandParameterProperty, value);
		}
		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(KeyPressBehavior));
		protected override void OnAttached()
		{
			AssociatedObject.KeyUp += (sender, e) =>
			{
				if (Command != null && e.Key == Key.Enter)
				{
					if (Command.CanExecute(CommandParameter))
					{
						Command.Execute(CommandParameter);
					}
				}
			};
		}
		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(KeyPressBehavior));
	}
}
