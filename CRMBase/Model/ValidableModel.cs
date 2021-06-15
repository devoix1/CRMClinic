using CRMBase.Model.Validators;
using FluentValidation;
using System.ComponentModel;
using System.Linq;

namespace CRMBase.Model
{
	public abstract class ValidableModel<T> : HashComparator, IDataErrorInfo
		where T : HashComparator
	{
		protected virtual IValidator<T> Validator { get; set; }
		public string Error
		{
			get
			{
				if (Validator != null)
				{
					var results = Validator.Validate(this as T);
					if (results != null && results.Errors.Count > 0)
					{
						return string.Join('\n'.ToString(), results.Errors.Select(e => e.ErrorMessage).ToArray());
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
		public bool IsValid()
		{
			var results = Validator.Validate(this as T);
			return results == null || results.Errors.Count == 0;
		}
	}
}