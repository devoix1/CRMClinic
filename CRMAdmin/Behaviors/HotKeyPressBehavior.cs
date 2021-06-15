using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;

namespace CRMAdmin.Behaviors
{
	public sealed class HotKeyPressBehavior : Behavior<UIElement>
	{
		public ICommand Command
		{
			get => GetValue(CommandProperty) as System.Windows.Input.ICommand;
			set => SetValue(CommandProperty, value);
		}
		protected override void OnAttached()
		{
			AssociatedObject.KeyUp += (sender, e) => Command.Execute(e.Key);
		}
		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(HotKeyPressBehavior));
	}
}