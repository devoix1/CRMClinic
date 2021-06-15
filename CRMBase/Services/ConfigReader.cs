using Newtonsoft.Json;
using System;
using System.IO;

namespace CRMBase.Services
{
	public sealed class ConfigReader
	{
		private ConfigReader()
		{
			var exePath = $@"{Directory.GetCurrentDirectory()}\{СonfigFilename}";
			// Выполняется только в случае билдинга
			try
			{
				var buildPath = $@"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName}\{СonfigFilename}";
				if (!File.Exists(buildPath))
				{
					throw new Exception("Configuration file does not exits");
				}
				using (var file = new StreamWriter(File.Create(exePath), System.Text.Encoding.UTF8))
				{
					var text = File.ReadAllText(buildPath);
					config = JsonConvert.DeserializeObject<Config>(text);
					file.Write(text);
				}
			}
			// Если пошло по пизде ИЛИ был обычный запуск приложения
			catch
			{
				if (!File.Exists(exePath))
				{
					using (var file = new StreamWriter(File.Create(exePath), System.Text.Encoding.UTF8))
					{
						config = new Config();
						file.Write("{}");
					}
				}
			}
		}
		private readonly dynamic config;
		private static ConfigReader instance;
		public static ConfigReader Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new ConfigReader();
				}
				return instance;
			}
		}
		public dynamic Read(string configKey) => config[configKey];
		public T Read<T>(string configKey, IConfigValueReader<T> reader) => reader.Parse(config[configKey]);
		private const string СonfigFilename = "config.json";
	}
}
