using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Data.Entities;
using Beeffective.Data.Repositories;

namespace Beeffective.Tests.Doubles.Repositories
{
    [Export(typeof(IRepository<TaskEntity>))]
    public class TaskRepository : IRepository<TaskEntity>
    {
        private readonly List<TaskEntity> list;

        public TaskRepository()
        {
            list = new List<TaskEntity>();
        }

        public Task<List<TaskEntity>> LoadAsync() =>
            Task.FromResult(list);

        public Task<TaskEntity> GetById(int id) =>
            Task.FromResult(list.SingleOrDefault(i => i.Id == id));

        public Task<TaskEntity> AddAsync(TaskEntity entity) =>
            Task.Run(() =>
            {
                if (list.Any()) entity.Id = list.Select(i => i.Id).Max() + 1;
                else entity.Id = 1;
                list.Add(entity);
                return entity;
            });

        public Task UpdateAsync(TaskEntity entity) =>
            Task.Run(() =>
            {
                var existing = list.SingleOrDefault(i => i.Id == entity.Id);
                if (existing == null) return;
                existing.Title = entity.Title;
                existing.DueTo = entity.DueTo;
            });

        public Task RemoveAsync(TaskEntity entity) =>
            Task.Run(() => { list.Remove(entity); });

        public async Task SaveAsync(IEnumerable<TaskEntity> entities)
        {
            foreach (var entity in entities)
            {
                await UpdateAsync(entity);
            }
        }
    }
}