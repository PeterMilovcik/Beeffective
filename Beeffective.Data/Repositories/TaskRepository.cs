using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Data.Entities;

namespace Beeffective.Data.Repositories
{
    [Export(typeof(IRepository<TaskEntity>))]
    public class TaskRepository : IRepository<TaskEntity>
    {
        public Task<TaskEntity> GetById(int id) =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                return context.Find<TaskEntity>(id);
            });

        public Task<TaskEntity> AddAsync(TaskEntity entity) =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                var entry = context.Tasks.Add(entity);
                context.SaveChanges();
                return entry.Entity;
            });

        public Task UpdateAsync(TaskEntity entity) =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                context.Update(entity);
                context.SaveChanges();
            });

        public Task RemoveAsync(TaskEntity entity) =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                context.Remove(entity);
                context.SaveChanges();
            });

        public Task SaveAsync(IEnumerable<TaskEntity> entities) =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                context.UpdateRange(entities);
                context.SaveChanges();
            });

        public Task<List<TaskEntity>> LoadAsync() =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                return context.Tasks.ToList();
            });
    }
}