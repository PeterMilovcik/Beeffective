using System.Collections.Generic;
using System.Threading.Tasks;
using Beeffective.Core.Models;

namespace Beeffective.Services.Repository
{
    public interface IRepositoryService<T>
    {
        Task<List<T>> LoadAsync();
        Task<T> AddAsync(T newItem);
        Task UpdateAsync(T item);
        Task RemoveAsync(T item);
        Task SaveAsync(List<T> items);
    }

    public interface IRepositoryService
    {
        IRepositoryService<GoalModel> Goals { get; }
        IRepositoryService<ProjectModel> Projects { get; }
        IRepositoryService<TaskModel> Tasks { get; }
        IRepositoryService<LabelModel> Labels { get; }
    }
}
