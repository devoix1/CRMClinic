using System.Windows;
using CRMBase.Messages;
using CRMBase.Tokens;
using CRMBase.Model;
using GalaSoft.MvvmLight.Command;
using Dapper;
using System.Threading.Tasks;
using CRMAdmin.Validators;

namespace CRMAdmin.ViewModel
{
	public sealed class LoginVM : ValidableViewModel<LoginVM>
	{
		public LoginVM()
		{
			Validator = new LoginVMValidator();
		}
		private RelayCommand loginCommand;
		public RelayCommand LoginCommand => loginCommand ??= new RelayCommand(async () =>
		{
			var @params = new DynamicParameters();
			@params.Add("login", Login);
			@params.Add("password", Password);
			LockCommandExecution();
			var result = await procedureManager.ExecuteSingle<Account>("AccountByLoginAndPassword", @params);
			if (result != null)
			{
				MessengerInstance.Send(new ItemUpdateMessage<Account>(this)
				{
					Item = result
				});
				await loginTaskManager.WaitAllTasksToExecute();
				MessengerInstance.Send(new VMUpdateMessage(this)
				{
					VM = App.Container.GetInstance<DashboardVM>()
				}, MainVMToken.Instance);
			}
			else
			{
				MessageBox.Show("Account does not exists");
			}
			MimicPassword = null;
			UnlockCommandExecution();
		}, () => !ExecutionLocked && IsVMValid());
		private string login;
		public string Login
		{
			get => login;
			set
			{
				Set(ref login, value);
				TriggerCommandExecution();
			}
		}
		private string password;
		public string Password
		{
			get => password;
			set
			{
				Set(ref password, value);
				TriggerCommandExecution();
			}
		}
		private string mimicPassword;
		public string MimicPassword
		{
			get => new string('\u2642', (mimicPassword ?? string.Empty).Length);
			set
			{
				Set(ref mimicPassword, value);
				if (value == null || value.Length == 0)
				{
					Password = string.Empty;
					mimicPassword = string.Empty;
				}
				else
				{
					if (Password != null && value.Length < Password.Length)
					{
						Password = Password[..^1];
					}
					else
					{
						Password += value[^1];
					}
				}
			}
		}
		protected override void TriggerCommandExecution()
		{
			LoginCommand.RaiseCanExecuteChanged();
		}
	}
}
