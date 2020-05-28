using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Models;
using Beeffective.Data.Repositories;

namespace Beeffective.Services.Repository
{
    [Export(typeof(IRepositoryService<GoalModel>))]
    public class GoalRepositoryService : IRepositoryService<GoalModel>
    {
        private readonly IRepository repository;

        [ImportingConstructor]
        public GoalRepositoryService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<GoalModel>> LoadAsync()
        {
            var entities = await repository.Goals.LoadAsync();
            return entities.Select(e => e.ToModel()).ToList();
        }

        public async Task<GoalModel> AddAsync(GoalModel newGoalModel) =>
            (await repository.Goals.AddAsync(newGoalModel.ToEntity())).ToModel();

        public Task UpdateAsync(GoalModel goalModel) =>
            repository.Goals.UpdateAsync(goalModel.ToEntity());

        public Task RemoveAsync(GoalModel goalModel) =>
            repository.Goals.RemoveAsync(goalModel.ToEntity());

        public Task SaveAsync(List<GoalModel> goalModels) =>
            repository.Goals.SaveAsync(goalModels.Select(gm => gm.ToEntity()));
    }
}