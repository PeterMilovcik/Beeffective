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
        public async Task<List<TaskEntity>> LoadTaskAsync()
        {
            await using DataContext context = new DataContext();
            return await context.Tasks.ToListAsync();
        }

        public async Task<TaskEntity> AddTaskAsync(TaskEntity taskEntity)
        {
            await using DataContext context = new DataContext();
            var entry = await context.Tasks.AddAsync(taskEntity);
            await context.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task UpdateTaskAsync(TaskEntity taskEntity)
        {
            await using DataContext context = new DataContext();
            context.Update(taskEntity);
            await context.SaveChangesAsync();
        }

        public async Task RemoveTaskAsync(TaskEntity taskEntity)
        {
            await using DataContext context = new DataContext();
            context.Remove(taskEntity);
            await context.SaveChangesAsync();
        }
    }
}