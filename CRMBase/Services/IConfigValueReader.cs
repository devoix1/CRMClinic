namespace CRMBase.Services
{
	public interface IConfigValueReader<T>
	{
		T Parse(object value);
	}
}