using FluentValidation;
using System.ComponentModel;
using System.Linq;

namespace CRMAdmin.ViewModel
{
	public abstract class ValidableViewModel<T> : SuperViewModel, IDataErrorInfo
		where T : SuperViewModel
	{
		public virtual IValidator<T> Validator { get; protected set; }
		public string Error
		{
			get
			{
				if (Validator != null)
				{
					var results = Validator.Validate(this as T);
					if (results != null && results.Errors.Count > 0)
					{
						return string.Join('\n', results.Errors.Select(e => e.ErrorMessage));
					}
				}

				return string.Empty;
			}
		}
		public string this[string columnName]
		{
			get
			{
				if (Validator != null)
				{
					var firstOrDefault = Validator.Validate(this as T).Errors.FirstOrDefault(e => e.PropertyName == columnName);
					if (firstOrDefault != null)
					{
						return Validator != null ? firstOrDefault.ErrorMessage : string.Empty;
					}
				}
				return string.Empty;
			}
		}
		protected bool IsVMValid()
		{
			var results = Validator.Validate(this as T);
			return results == null || results.Errors.Count == 0;
		}
	}
}