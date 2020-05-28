using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Data.Entities;

namespace Beeffective.Data.Repositories
{
    [Export(typeof(IRepository<GoalEntity>))]
    public class GoalRepository : IRepository<GoalEntity>
    {
        public Task<GoalEntity> GetById(int id) =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                return context.Find<GoalEntity>(id);
            });

        public Task<GoalEntity> AddAsync(GoalEntity entity) =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                var entry = context.Goals.Add(entity);
                context.SaveChanges();
                return entry.Entity;
            });

        public Task UpdateAsync(GoalEntity entity) =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                context.Update(entity);
                context.SaveChanges();
            });

        public Task RemoveAsync(GoalEntity entity) =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                context.Remove(entity);
                context.SaveChanges();
            });

        public Task SaveAsync(IEnumerable<GoalEntity> entities) =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                context.UpdateRange(entities);
                context.SaveChanges();
            });

        public Task<List<GoalEntity>> LoadAsync() =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                return context.Goals.ToList();
            });
    }
}