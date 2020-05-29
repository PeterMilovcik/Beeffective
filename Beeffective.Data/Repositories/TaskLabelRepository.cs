using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Data.Entities;

namespace Beeffective.Data.Repositories
{
    [Export(typeof(IRepository<TaskLabelEntity>))]
    public class TaskLabelRepository : IRepository<TaskLabelEntity>
    {
        public Task<TaskLabelEntity> GetById(int id) =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                return context.Find<TaskLabelEntity>(id);
            });

        public Task<TaskLabelEntity> AddAsync(TaskLabelEntity entity) =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                var entry = context.TaskLabels.Add(entity);
                context.SaveChanges();
                return entry.Entity;
            });

        public Task UpdateAsync(TaskLabelEntity entity) =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                context.Update(entity);
                context.SaveChanges();
            });

        public Task RemoveAsync(TaskLabelEntity entity) =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                context.Remove(entity);
                context.SaveChanges();
            });

        public Task SaveAsync(IEnumerable<TaskLabelEntity> entities) =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                context.UpdateRange(entities);
                context.SaveChanges();
            });

        public Task<List<TaskLabelEntity>> LoadAsync() =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                return context.TaskLabels.ToList();
            });
    }
}