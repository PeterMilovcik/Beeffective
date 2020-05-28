using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Data.Entities;
using Beeffective.Data.Repositories;

namespace Beeffective.Tests.Doubles.Repositories
{
    [Export(typeof(IRepository<ProjectEntity>))]
    public class ProjectRepository : IRepository<ProjectEntity>
    {
        private readonly List<ProjectEntity> list;

        public ProjectRepository()
        {
            list = new List<ProjectEntity>();
        }

        public Task<List<ProjectEntity>> LoadAsync() =>
            Task.FromResult(list);

        public Task<ProjectEntity> GetById(int id) =>
            Task.FromResult(list.SingleOrDefault(i => i.Id == id));

        public Task<ProjectEntity> AddAsync(ProjectEntity entity) =>
            Task.Run(() =>
            {
                entity.Id = list.Select(i => i.Id).Max() + 1;
                list.Add(entity);
                return entity;
            });

        public Task UpdateAsync(ProjectEntity entity) =>
            Task.Run(() =>
            {
                var existing = list.SingleOrDefault(i => i.Id == entity.Id);
                if (existing == null) return;
                existing.GoalId = entity.GoalId;
                existing.Title = entity.Title;
                existing.Description = entity.Description;
            });

        public Task RemoveAsync(ProjectEntity entity) =>
            Task.Run(() => { list.Remove(entity); });

        public async Task SaveAsync(IEnumerable<ProjectEntity> entities)
        {
            foreach (var entity in entities)
            {
                await UpdateAsync(entity);
            }
        }
    }
}