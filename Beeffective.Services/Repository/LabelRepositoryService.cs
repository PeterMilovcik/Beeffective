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

        [ImportingConstructor]
        public LabelRepositoryService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<LabelModel>> LoadAsync()
        {
            var entities = await repository.Labels.LoadAsync();
            return entities.Select(e => e.ToModel()).ToList();
        }

        public async Task<LabelModel> AddAsync(LabelModel newLabelModel) =>
            (await repository.Labels.AddAsync(newLabelModel.ToEntity())).ToModel();

        public Task UpdateAsync(LabelModel labelModel) =>
            repository.Labels.UpdateAsync(labelModel.ToEntity());

        public Task RemoveAsync(LabelModel labelModel) =>
            repository.Labels.RemoveAsync(labelModel.ToEntity());

        public Task SaveAsync(List<LabelModel> labelModels) =>
            repository.Labels.SaveAsync(labelModels.Select(gm => gm.ToEntity()));
    }
}