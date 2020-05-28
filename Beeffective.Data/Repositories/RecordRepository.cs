using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Data.Entities;

namespace Beeffective.Data.Repositories
{
    [Export(typeof(IRepository<RecordEntity>))]
    public class RecordRepository : IRepository<RecordEntity>
    {
        public Task<RecordEntity> GetById(int id) =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                return context.Find<RecordEntity>(id);
            });

        public Task<RecordEntity> AddAsync(RecordEntity entity) =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                var entry = context.Records.Add(entity);
                context.SaveChanges();
                return entry.Entity;
            });

        public Task UpdateAsync(RecordEntity entity) =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                context.Update(entity);
                context.SaveChanges();
            });

        public Task RemoveAsync(RecordEntity entity) =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                context.Remove(entity);
                context.SaveChanges();
            });

        public Task SaveAsync(IEnumerable<RecordEntity> entities) =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                context.UpdateRange(entities);
                context.SaveChanges();
            });

        public Task<List<RecordEntity>> LoadAsync() =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                return context.Records.ToList();
            });
    }
}