using System;
using System.Threading.Tasks;

namespace CRMBase.Services
{
	public interface IAppTaskManager
	{
		void ResetTasks();
		void AddTask(Action task);
		Task WaitAllTasksToExecute();
	}
}
