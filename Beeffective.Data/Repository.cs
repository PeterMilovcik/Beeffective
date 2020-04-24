using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Beeffective.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Beeffective.Data
{
    [Export(typeof(IRepository))]
    public class Repository : IRepository
    {
        private readonly DataContext context;

        [ImportingConstructor]
        public Repository(DataContext context)
        {
            this.context = context;
        }

        public Task<List<TaskEntity>> LoadTaskAsync() => 
            context.Tasks.ToListAsync();

        public async Task<TaskEntity> AddTaskAsync(TaskEntity taskEntity)
        {
            var entry = await context.Tasks.AddAsync(taskEntity);
            await context.SaveChangesAsync();
            return entry.Entity;
        }
    }
}