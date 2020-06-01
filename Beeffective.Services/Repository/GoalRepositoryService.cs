using System;
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
        private readonly List<GoalModel> list;

        [ImportingConstructor]
        public GoalRepositoryService(IRepository repository)
        {
            this.repository = repository;
            list = new List<GoalModel>();
        }

        public async Task<List<GoalModel>> LoadAsync()
        {
            var entities = await repository.Goals.LoadAsync();
            var goalModels = entities.Select(e => e.ToModel()).ToList();
            list.ForEach(Unsubscribe);
            list.Clear();
            goalModels.ForEach(Subscribe);
            list.AddRange(goalModels);
            return goalModels;
        }

        public async Task<GoalModel> AddAsync(GoalModel newGoalModel)
        {
            var goalModel = (await repository.Goals.AddAsync(newGoalModel.ToEntity())).ToModel();
            Subscribe(goalModel);
            list.Add(goalModel);
            return goalModel;
        }

        public Task UpdateAsync(GoalModel goalModel) =>
            repository.Goals.UpdateAsync(goalModel.ToEntity());

        public Task RemoveAsync(GoalModel goalModel)
        {
            Unsubscribe(goalModel);
            list.Remove(goalModel);
            return repository.Goals.RemoveAsync(goalModel.ToEntity());
        }

        public Task SaveAsync(List<GoalModel> goalModels) =>
            repository.Goals.SaveAsync(goalModels.Select(gm => gm.ToEntity()));

        private void Subscribe(GoalModel goalModel) => goalModel.Changed += OnChanged;

        private void Unsubscribe(GoalModel goalModel) => goalModel.Changed -= OnChanged;

        private async void OnChanged(object sender, EventArgs e)
        {
            if (sender is GoalModel goalModel)
            {
                await UpdateAsync(goalModel);
            }
        }
    }
}