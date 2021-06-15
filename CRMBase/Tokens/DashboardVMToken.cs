namespace CRMBase.Tokens
{
	public sealed class DashboardVMToken
	{
		private DashboardVMToken() { }
		private static DashboardVMToken instance;
		public static DashboardVMToken Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new DashboardVMToken();
				}
				return instance;
			}
		}
	}
}
