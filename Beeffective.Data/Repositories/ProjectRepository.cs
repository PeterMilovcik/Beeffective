using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Data.Entities;

namespace Beeffective.Data.Repositories
{
    [Export(typeof(IRepository<ProjectEntity>))]
    public class ProjectRepository : IRepository<ProjectEntity>
    {
        public Task<ProjectEntity> GetById(int id) =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                return context.Find<ProjectEntity>(id);
            });

        public Task<ProjectEntity> AddAsync(ProjectEntity entity) =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                var entry = context.Projects.Add(entity);
                context.SaveChanges();
                return entry.Entity;
            });

        public Task UpdateAsync(ProjectEntity entity) =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                context.Update(entity);
                context.SaveChanges();
            });

        public Task RemoveAsync(ProjectEntity entity) =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                context.Remove(entity);
                context.SaveChanges();
            });

        public Task SaveAsync(IEnumerable<ProjectEntity> entities) =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                context.UpdateRange(entities);
                context.SaveChanges();
            });

        public Task<List<ProjectEntity>> LoadAsync() =>
            Task.Run(() =>
            {
                using var context = new DataContext();
                return context.Projects.ToList();
            });
    }
}