using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Data.Entities;
using Beeffective.Data.Repositories;

namespace Beeffective.Tests.Doubles.Repositories
{
    [Export(typeof(IRepository<RecordEntity>))]
    public class RecordRepository : IRepository<RecordEntity>
    {
        private readonly List<RecordEntity> list;

        public RecordRepository()
        {
            list = new List<RecordEntity>();
        }

        public Task<List<RecordEntity>> LoadAsync() =>
            Task.FromResult(list);

        public Task<RecordEntity> GetById(int id) =>
            Task.FromResult(list.SingleOrDefault(i => i.Id == id));

        public Task<RecordEntity> AddAsync(RecordEntity entity) =>
            Task.Run(() =>
            {
                entity.Id = list.Select(i => i.Id).Max() + 1;
                list.Add(entity);
                return entity;
            });

        public Task UpdateAsync(RecordEntity entity) =>
            Task.Run(() =>
            {
                var existing = list.SingleOrDefault(i => i.Id == entity.Id);
                if (existing == null) return;
                existing.TaskId = entity.TaskId;
                existing.StartAt = entity.StartAt;
                existing.StopAt = entity.StopAt;
            });

        public Task RemoveAsync(RecordEntity entity) =>
            Task.Run(() => { list.Remove(entity); });

        public async Task SaveAsync(IEnumerable<RecordEntity> entities)
        {
            foreach (var entity in entities)
            {
                await UpdateAsync(entity);
            }
        }
    }
}