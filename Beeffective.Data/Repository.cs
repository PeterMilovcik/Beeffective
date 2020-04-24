using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Beeffective.Data.Entities;

namespace Beeffective.Data
{
    [Export(typeof(IRepository))]
    public class Repository : IRepository
    {
        public Task<List<TaskEntity>> LoadTaskAsync() => 
            Task.FromResult(new List<TaskEntity>());

        public Task<TaskEntity> AddTaskAsync(TaskEntity taskEntity) => 
            Task.FromResult(taskEntity);
    }
}