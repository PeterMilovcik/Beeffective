using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Data.Entities;

namespace Beeffective.Data
{
    [Export(typeof(IRepository))]
    public class Repository : IRepository
    {
        public Task<List<TaskEntity>> LoadTaskAsync() =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                return context.Tasks.ToList();
            });

        public Task<TaskEntity> AddTaskAsync(TaskEntity taskEntity) =>
            Task.Run(() =>
            {
                using var context = new DataContext(); 
                var entry = context.Tasks.Add(taskEntity);
                context.SaveChanges();
                return entry.Entity;
            });

        public Task UpdateTaskAsync(TaskEntity taskEntity) =>
            Task.Run(() =>
            { 
                using var context = new DataContext();
                context.Update(taskEntity);
                context.SaveChanges();
            });

        public Task RemoveTaskAsync(TaskEntity taskEntity) =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                context.Remove(taskEntity);
                context.SaveChanges();
            });
    }
}