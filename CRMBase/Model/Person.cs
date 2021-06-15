using CRMBase.Model.Validators;
using System;
using System.ComponentModel;

namespace CRMBase.Model
{
	public abstract class Person<T> : ValidableModel<T>, INotifyPropertyChanged
		where T : HashComparator
	{
		private string surname;
		public string Surname
		{
			get => surname;
			set
			{
				surname = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Surname)));
			}
		}
		private string name;
		public string Name
		{
			get => name;
			set
			{
				name = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
			}
		}
		private string middlename;
		public string Middlename
		{
			get => middlename;
			set
			{
				middlename = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Middlename)));
			}
		}
		private string email;
		public string Email
		{
			get => email;
			set
			{
				email = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Email)));
			}
		}
		private string phone;
		public string Phone
		{
			get => phone;
			set
			{
				phone = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Phone)));
			}
		}
		private string address;
		public string Address
		{
			get => address;
			set
			{
				address = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Address)));
			}
		}
		private string gender;
		public string Gender
		{
			get => gender;
			set
			{
				gender = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Gender)));
			}
		}
		private DateTime? birthDate;
		public DateTime? BirthDate
		{
			get => birthDate;
			set
			{
				birthDate = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BirthDate)));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void RaisePropertyChanged(object sender, PropertyChangedEventArgs e) => PropertyChanged?.Invoke(sender, e);
	}
}
