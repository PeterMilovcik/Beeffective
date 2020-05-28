using System.Collections.Generic;
using System.Threading.Tasks;
using Beeffective.Data.Entities;

namespace Beeffective.Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        Task<List<TEntity>> LoadAsync();
        Task<TEntity> GetById(int id);
        Task<TEntity> AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task RemoveAsync(TEntity entity);
        Task SaveAsync(IEnumerable<TEntity> entities);
    }

    public interface IRepository
    {
        IRepository<GoalEntity> Goals { get; }
        IRepository<ProjectEntity> Projects { get; }
        IRepository<TaskEntity> Tasks { get; }
        IRepository<RecordEntity> Records { get; }
        IRepository<LabelEntity> Labels { get; set; }
    }
}