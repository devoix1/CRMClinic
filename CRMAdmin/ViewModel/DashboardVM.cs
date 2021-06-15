using CRMBase.Messages;
using CRMBase.Tokens;
using GalaSoft.MvvmLight.Command;
using CRMBase.Services;
using CRMBase.Model;
using System.Windows.Input;
using System.Threading.Tasks;

namespace CRMAdmin.ViewModel
{
	public class DashboardVM : SuperViewModel
	{
		public DashboardVM()
		{
			async void TryLoadAvatar(Account account)
			{
				try
				{
					await avatarLoader.LoadAvatar(account);
					AvatarSource = avatarLoader.GetAvatarFilepathFor(currentAccount);
				}
				catch
				{
					// Пиздец костыль
					Task.Run(async () =>
					{
						await Task.Delay(5000);
						TryLoadAvatar(account);
					});
				}
			}
			CurrentVM = App.Container.GetInstance<DashboardInfoVM>();
			MessengerInstance.Register<VMUpdateMessage>(this, DashboardVMToken.Instance, message =>
			{
				if (CurrentVM != message.VM)
				{
					CurrentVM = message.VM as SuperViewModel;
				}
			});
			MessengerInstance.Register<ItemUpdateMessage<Account>>(this, message =>
			{
				if (message.Sender is AccountVM)
				{
					AvatarSource = null;
					TryLoadAvatar(message.Item);
				}
			});
			MessengerInstance.Register<ItemUpdateMessage<Account>>(this, message =>
			{
				if (message.Sender is LoginVM)
				{
					Login = message.Item.Login;
					TryLoadAvatar(message.Item);
				}
			});
		}
		private RelayCommand<Key> hotkeyCommand;
		public RelayCommand<Key> HotkeyCommand => hotkeyCommand ??= new RelayCommand<Key>(key =>
		{
			if (currentAccount != null)
			{
				switch (key)
				{
					case Key.F1:
					{
						if (DashboardCommand.CanExecute(null))
						{
							DashboardCommand.Execute(null);
						}
						break;
					}
					case Key.F2:
					{
						if (PatientCommand.CanExecute(null))
						{
							PatientCommand.Execute(null);
						}
						break;
					}
					case Key.F3:
					{
						if (DoctorCommand.CanExecute(null))
						{
							DoctorCommand.Execute(null);
						}
						break;
					}
					case Key.F4:
					{
						if (CabinetCommand.CanExecute(null))
						{
							CabinetCommand.Execute(null);
						}
						break;
					}
					case Key.F5:
					{
						if (MedicineCommand.CanExecute(null))
						{
							MedicineCommand.Execute(null);
						}
						break;
					}
					case Key.F6:
					{
						if (ProcedureCommand.CanExecute(null))
						{
							ProcedureCommand.Execute(null);
						}
						break;
					}
					case Key.F7:
					{
						if (EquipmentCommand.CanExecute(null))
						{
							EquipmentCommand.Execute(null);
						}
						break;
					}
					case Key.F8:
					{
						if (AccountCommand.CanExecute(null))
						{
							AccountCommand.Execute(null);
						}
						break;
					}
				}
			}
		});
		private readonly AvatarManager avatarLoader = App.Container.GetInstance<AvatarManager>();
		private SuperViewModel currentVM;
		public SuperViewModel CurrentVM
		{
			get => currentVM;
			set => Set(ref currentVM, value);
		}
		public RelayCommand dashboardCommand;
		public RelayCommand DashboardCommand => dashboardCommand ??= new RelayCommand(async () =>
		{
			LockCommandExecution();
			MessengerInstance.Send(new CollectionUpdateMessage<Log>(this)
			{
				Collection = await logTableManager.SelectAll()
			});
			UnlockCommandExecution();
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<DashboardInfoVM>()
			}, DashboardVMToken.Instance);
		}, () => !ExecutionLocked);
		public RelayCommand patientCommand;
		public RelayCommand PatientCommand => patientCommand ??= new RelayCommand(() =>
		{
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<PatientVM>()
			}, DashboardVMToken.Instance);
		});
		public RelayCommand doctorCommand;
		public RelayCommand DoctorCommand => doctorCommand ??= new RelayCommand(() =>
		{
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<DoctorVM>()
			}, DashboardVMToken.Instance);
		});
		public RelayCommand accountCommand;
		public RelayCommand AccountCommand => accountCommand ??= new RelayCommand(() =>
		{
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<AccountVM>()
			}, DashboardVMToken.Instance);
		});
		public RelayCommand cabinetCommand;
		public RelayCommand CabinetCommand => cabinetCommand ??= new RelayCommand(() =>
		{
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<CabinetVM>()
			}, DashboardVMToken.Instance);
		});
		public RelayCommand medicineCommand;
		public RelayCommand MedicineCommand => medicineCommand ??= new RelayCommand(() =>
		{
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<MedicineVM>()
			}, DashboardVMToken.Instance);
		});
		public RelayCommand procedureCommand;
		public RelayCommand ProcedureCommand => procedureCommand ??= new RelayCommand(() =>
		{
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<ProcedureVM>()
			}, DashboardVMToken.Instance);
		});
		public RelayCommand equipmentCommand;
		public RelayCommand EquipmentCommand => equipmentCommand ??= new RelayCommand(() =>
		{
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<EquipmentVM>()
			}, DashboardVMToken.Instance);
		});
		private RelayCommand logoutCommand;
		public RelayCommand LogoutCommand => logoutCommand ??= new RelayCommand(() =>
		{
			AvatarSource = null;
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<LoginVM>()
			}, MainVMToken.Instance);
			MessengerInstance.Send(new VMUpdateMessage(this)
			{
				VM = App.Container.GetInstance<DashboardInfoVM>()
			}, DashboardVMToken.Instance);
		});
		private string login;
		public string Login
		{
			get
			{
				if (login != null && (login.ToLower().StartsWith("Billy") || login.ToLower() == "billy"))
				{
					return $"\u2642{login}\u2642";
				}
				return login;
			}
			set => Set(ref login, value);
		}
		private string avatarSource;
		public string AvatarSource
		{
			get => avatarSource;
			set => Set(ref avatarSource, value);
		}
		protected override void TriggerCommandExecution()
		{
			DashboardCommand.RaiseCanExecuteChanged();
		}
	}
}
