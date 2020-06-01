using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Models;
using Beeffective.Data.Repositories;

namespace Beeffective.Services.Repository
{
    [Export(typeof(IRepositoryService<LabelModel>))]
    public class LabelRepositoryService : IRepositoryService<LabelModel>
    {
        private readonly IRepository repository;
        private readonly List<LabelModel> list;

        [ImportingConstructor]
        public LabelRepositoryService(IRepository repository)
        {
            this.repository = repository;
            list = new List<LabelModel>();
        }

        public async Task<List<LabelModel>> LoadAsync()
        {
            var entities = await repository.Labels.LoadAsync();
            var labelModels = entities.Select(e => e.ToModel()).ToList();
            list.ForEach(Unsubscribe);
            list.Clear();
            labelModels.ForEach(Subscribe);
            list.AddRange(labelModels);
            return labelModels;
        }

        public async Task<LabelModel> AddAsync(LabelModel newLabelModel)
        {
            var labelModel = (await repository.Labels.AddAsync(newLabelModel.ToEntity())).ToModel();
            Subscribe(labelModel);
            list.Add(labelModel);
            return labelModel;
        }

        public Task UpdateAsync(LabelModel labelModel) =>
            repository.Labels.UpdateAsync(labelModel.ToEntity());

        public async Task RemoveAsync(LabelModel labelModel)
        {
            Unsubscribe(labelModel);
            list.Remove(labelModel);
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