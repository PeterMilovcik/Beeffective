using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Models;
using Beeffective.Data.Repositories;

namespace Beeffective.Services.Repository
{
    public class LabelRepositoryService : IRepositoryService<LabelModel>
    {
        private readonly IRepository repository;

        public LabelRepositoryService(IRepository repository)
        {
            this.repository = repository;
            List = new List<LabelModel>();
        }

        public List<LabelModel> List { get; }

        public async Task<List<LabelModel>> LoadAsync()
        {
            var entities = await repository.Labels.LoadAsync();
            var labelModels = entities.Select(e => e.ToModel()).ToList();
            List.ForEach(Unsubscribe);
            List.Clear();
            labelModels.ForEach(Subscribe);
            List.AddRange(labelModels);
            return labelModels;
        }

        public async Task<LabelModel> AddAsync(LabelModel newLabelModel)
        {
            var labelModel = (await repository.Labels.AddAsync(newLabelModel.ToEntity())).ToModel();
            Subscribe(labelModel);
            List.Add(labelModel);
            return labelModel;
        }

        public Task UpdateAsync(LabelModel labelModel) =>
            repository.Labels.UpdateAsync(labelModel.ToEntity());

        public async Task RemoveAsync(LabelModel labelModel)
        {
            Unsubscribe(labelModel);
            List.Remove(labelModel);
            await repository.Labels.RemoveAsync(labelModel.ToEntity());
        }

        public Task SaveAsync(List<LabelModel> labelModels) =>
            repository.Labels.SaveAsync(labelModels.Select(gm => gm.ToEntity()));

        private void Subscribe(LabelModel labelModel) => labelModel.Changed += OnChanged;

        private void Unsubscribe(LabelModel labelModel) => labelModel.Changed -= OnChanged;

        private async void OnChanged(object sender, EventArgs e)
        {
            if (sender is LabelModel labelModel)
            {
                await UpdateAsync(labelModel);
            }
        }
    }
}