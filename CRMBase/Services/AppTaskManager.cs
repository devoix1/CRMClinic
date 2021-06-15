using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRMBase.Services
{
	public sealed class AppTaskManager : IAppTaskManager
	{
		private readonly List<Action> actions = new List<Action>();
		public void ResetTasks() => actions.Clear();
		public void AddTask(Action task) => actions.Add(task);
		public async Task WaitAllTasksToExecute()
		{
			var tasks = new List<Task>();
			foreach (var item in actions)
			{
				tasks.Add(Task.Run(item));
			}
			await Task.WhenAll(tasks.ToArray());
		}
	}
}