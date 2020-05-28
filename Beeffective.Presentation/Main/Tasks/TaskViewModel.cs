using Beeffective.Core.Models;
using Beeffective.Presentation.Common;

namespace Beeffective.Presentation.Main.Tasks
{
    public class TaskViewModel : ViewModel
    {
        public TaskViewModel(TaskModel model)
        {
            Model = model;
        }

        public TaskModel Model { get; }
    }
}