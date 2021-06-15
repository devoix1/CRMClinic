using CRMBase.Model;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CRMBase.Services
{
	public sealed class AvatarManager
	{
		public AvatarManager(string manageablePath)
		{
			this.manageablePath = manageablePath;
		}
		private readonly string manageablePath;
		private string GetAvatarFilenameByTemplate(Account account)
		{
			return $"{account.Id}_{account.Login}.avatar";
		}
		public string GetAvatarFilepathFor(Account account)
		{
			var path = $@"{manageablePath}\{GetAvatarFilenameByTemplate(account)}";
			try
			{
				if (!File.Exists(path))
				{
					return null;
				}
			}
			catch
			{
				return null;
			}
			return path;
		}
		private readonly IProcedureManager procedureManager = new ProcedureManager();
		public async Task LoadAvatar(Account account)
		{
			Avatar av;
			var @params = new DynamicParameters();
			@params.Add("account_id", account.Id);
			av = await procedureManager.ExecuteSingle<Avatar>("GetAvatarByAccountId", @params);
			if (av == null)
			{
				return;
			}
			using (var memoryStream = new MemoryStream(av.Image))
			{
				using (var fileStream = new FileStream($@"{manageablePath}\{GetAvatarFilenameByTemplate(account)}", FileMode.Create))
				{
					memoryStream.WriteTo(fileStream);
				}
			}
		}
		public async Task UploadAvatar(Account account, string avatarPath)
		{
			using (var fileStream = new FileStream(avatarPath, FileMode.Open))
			{
				using (var memoryStream = new MemoryStream())
				{
					await fileStream.CopyToAsync(memoryStream);
					var @params = new DynamicParameters();
					@params.Add("account_id", account.Id);
					@params.Add("image", memoryStream.ToArray());
					await procedureManager.ExecuteVoid("UploadAvatar", @params);
				}
			}
		}
	}
}
