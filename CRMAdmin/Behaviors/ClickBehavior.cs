using Microsoft.Xaml.Behaviors;
using System.Windows;

namespace CRMAdmin.Behaviors
{
	public class ClickBehavior : Behavior<UIElement>
	{
		public System.Windows.Input.ICommand Command
		{
			get
			{
				return GetValue(CommandProperty) as System.Windows.Input.ICommand;
			}
			set
			{
				SetValue(CommandProperty, value);
			}
		}
		public object CommandParameter
		{
			get
			{
				return GetValue(CommandParameterProperty);
			}
			set
			{
				SetValue(CommandParameterProperty, value);
			}
		}
		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(ClickBehavior));
		protected override void OnAttached()
		{
			AssociatedObject.MouseUp += (sender, e) =>
			{
				Command?.Execute(CommandParameter);
			};
		}
		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(System.Windows.Input.ICommand), typeof(ClickBehavior));
	}
}
