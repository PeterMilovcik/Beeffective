using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Data.Entities;

namespace Beeffective.Data.Repositories
{
    [Export(typeof(IRepository<LabelEntity>))]
    public class LabelRepository : IRepository<LabelEntity>
    {
        public Task<LabelEntity> GetById(int id) =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                return context.Find<LabelEntity>(id);
            });

        public Task<LabelEntity> AddAsync(LabelEntity entity) =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                var entry = context.Labels.Add(entity);
                context.SaveChanges();
                return entry.Entity;
            });

        public Task UpdateAsync(LabelEntity entity) =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                context.Update(entity);
                context.SaveChanges();
            });

        public Task RemoveAsync(LabelEntity entity) =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                context.Remove(entity);
                context.SaveChanges();
            });

        public Task SaveAsync(IEnumerable<LabelEntity> entities) =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                context.UpdateRange(entities);
                context.SaveChanges();
            });

        public Task<List<LabelEntity>> LoadAsync() =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                return context.Labels.ToList();
            });
    }
}