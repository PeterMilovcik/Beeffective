using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Data.Entities;
using Beeffective.Data.Repositories;

namespace Beeffective.Tests.Doubles.Repositories
{
    [Export(typeof(IRepository<GoalEntity>))]
    public class GoalRepository : IRepository<GoalEntity>
    {
        private readonly List<GoalEntity> list;

        public GoalRepository()
        {
            list = new List<GoalEntity>();
        }

        public Task<List<GoalEntity>> LoadAsync() => 
            Task.FromResult(list);

        public Task<GoalEntity> GetById(int id) => 
            Task.FromResult(list.SingleOrDefault(i => i.Id == id));

        public Task<GoalEntity> AddAsync(GoalEntity entity) =>
            Task.Run(() =>
            {
                entity.Id = list.Select(i => i.Id).Max() + 1;
                list.Add(entity);
                return entity;
            });

        public Task UpdateAsync(GoalEntity entity) =>
            Task.Run(() =>
            {
                var existing = list.SingleOrDefault(i => i.Id == entity.Id);
                if (existing == null) return;
                existing.Title = entity.Title;
                existing.Description = entity.Description;
            });

        public Task RemoveAsync(GoalEntity entity) =>
            Task.Run(() => { list.Remove(entity); });

        public async Task SaveAsync(IEnumerable<GoalEntity> entities)
        {
            foreach (var entity in entities)
            {
                await UpdateAsync(entity);
            }
        }
    }
}