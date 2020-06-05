using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Models;
using Beeffective.Data.Repositories;

namespace Beeffective.Services.Repository
{
    public class GoalRepositoryService : IRepositoryService<GoalModel>
    {
        private readonly IRepository repository;

        public GoalRepositoryService(IRepository repository)
        {
            this.repository = repository;
            List = new List<GoalModel>();
        }

        public List<GoalModel> List { get; }

        public async Task<List<GoalModel>> LoadAsync()
        {
            var entities = await repository.Goals.LoadAsync();
            var goalModels = entities.Select(e => e.ToModel()).ToList();
            List.ForEach(Unsubscribe);
            List.Clear();
            goalModels.ForEach(Subscribe);
            List.AddRange(goalModels);
            return goalModels;
        }

        public async Task<GoalModel> AddAsync(GoalModel newGoalModel)
        {
            var goalModel = (await repository.Goals.AddAsync(newGoalModel.ToEntity())).ToModel();
            Subscribe(goalModel);
            List.Add(goalModel);
            return goalModel;
        }

        public Task UpdateAsync(GoalModel goalModel) =>
            repository.Goals.UpdateAsync(goalModel.ToEntity());

        public async Task RemoveAsync(GoalModel goalModel)
        {
            Unsubscribe(goalModel);
            List.Remove(goalModel);
            await repository.Goals.RemoveAsync(goalModel.ToEntity());
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