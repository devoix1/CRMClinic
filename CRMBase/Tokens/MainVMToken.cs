namespace CRMBase.Tokens
{
	public sealed class MainVMToken
	{
		private MainVMToken() { }
		private static MainVMToken instance;
		public static MainVMToken Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new MainVMToken();
				}
				return instance;
			}
		}
	}
}
