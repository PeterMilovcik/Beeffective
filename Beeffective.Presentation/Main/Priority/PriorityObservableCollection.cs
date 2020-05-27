﻿using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Beeffective.Core.Models;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Goals;
using Beeffective.Presentation.Main.Tasks;
using Beeffective.Services.Repository;

namespace Beeffective.Presentation.Main.Priority
{
    [Export]
    public class PriorityObservableCollection :
        Changeable,
        ICollection<TaskViewModel>,
        INotifyCollectionChanged
    {
        private readonly IRepositoryService repository;
        private TaskViewModel selected;
        private bool isSelected;
        private readonly ObservableCollection<TaskViewModel> collection;
        private ObservableCollection<TagModel> tags;
        private ObservableCollection<string> tagNames;

        [ImportingConstructor]
        public PriorityObservableCollection(IRepositoryService repository)
        {
            this.repository = repository;
            collection = new ObservableCollection<TaskViewModel>();
            collection.CollectionChanged += OnCollectionChanged;
            Goals = new ObservableCollection<GoalViewModel>();
            Tags = new ObservableCollection<TagModel>();
        }

        public TaskViewModel Selected
        {
            get => selected;
            set
            {
                if (SetProperty(ref selected, value))
                {
                    IsSelected = selected != null;
                    NotifyPropertyChange(nameof(Selected));
                }
            }
        }

        public bool IsSelected
        {
            get => isSelected;
            set => SetProperty(ref isSelected, value);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected virtual void OnCollectionChanged(
            object sender, NotifyCollectionChangedEventArgs e) =>
            CollectionChanged?.Invoke(this, e);

        public IEnumerator<TaskViewModel> GetEnumerator() => 
            collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => 
            GetEnumerator();

        public void Add(TaskViewModel item)
        {
            if (item == null) return;
            collection.Add(item);
            item.PropertyChanged += OnTaskViewModelPropertyChanged;
            item.Model.PropertyChanged += OnTaskModelPropertyChanged;
            UpdateTags();
        }

        private void OnTaskViewModelPropertyChanged(object sender, PropertyChangedEventArgs e) => 
            UpdateTags();

        private void OnTaskModelPropertyChanged(object sender, PropertyChangedEventArgs e) => 
            UpdateTags();

        public void Clear()
        {
            collection.Clear();
            UpdateTags();
        }

        public bool Remove(TaskViewModel item)
        {
            bool result = collection.Remove(item);
            UpdateTags();
            return result;
        }

        private void UpdateTags()
        {
            Tags = new ObservableCollection<TagModel>(GetTags());
            TagNames = new ObservableCollection<string>(Tags.Select(t => t.Name));
        }

        private IEnumerable<TagModel> GetTags()
        {
            var result = new HashSet<TagModel>();
            foreach (var taskViewModel in collection.Where(t => !string.IsNullOrWhiteSpace(t.Model.Tags)))
            {
                var tagNames = taskViewModel.Model.Tags.Trim().Split(" ");
                foreach (var tagName in tagNames)
                {
                    var tagModel = new TagModel {Name = tagName};
                    result.Add(tagModel);
                }
            }

            foreach (var taskViewModel in collection.Where(tvm => !string.IsNullOrEmpty(tvm.Model.Tags)))
            {
                var tagNames = taskViewModel.Model.Tags.Trim().Split(" ");
                foreach (var tagName in tagNames)
                {
                    var tagModel = result.Single(tm => tm.Name == tagName);
                    tagModel.TimeSpent = tagModel.TimeSpent.Add(taskViewModel.Model.TimeSpent);
                    if (!tagModel.Tasks.Contains(taskViewModel.Model))
                    {
                        tagModel.Tasks.Add(taskViewModel.Model);
                    }
                }
            }

            return result;
        }

        public bool Contains(TaskViewModel item) => 
            collection.Contains(item);

        public void CopyTo(TaskViewModel[] array, int arrayIndex) => 
            collection.CopyTo(array, arrayIndex);

        public int Count => collection.Count;

        public bool IsReadOnly => false;

        public ObservableCollection<GoalViewModel> Goals { get; }

        public ObservableCollection<TagModel> Tags
        {
            get => tags;
            set => SetProperty(ref tags, value);
        }

        public ObservableCollection<string> TagNames
        {
            get => tagNames;
            set => SetProperty(ref tagNames, value);
        }

        public async Task LoadAsync()
        {
            var tasks = (await repository.LoadTaskAsync())
                .Select(taskModel => new TaskViewModel(taskModel)).ToList()
                .OrderBy(vm => vm.Model.Priority).ToList();
            Clear();
            tasks.ForEach(Add);
            Goals.Clear();
            var goals = (await repository.LoadGoalsAsync())
                .Select(gm => new GoalViewModel(gm))
                .OrderBy(gm => gm.Model.Title).ToList();
            goals.ForEach(g => Goals.Add(g));
        }

        public async Task SaveAsync() => 
            await repository.SaveTaskAsync(collection.Select(taskViewModel => taskViewModel.Model).ToList());

        public IEnumerable<TaskViewModel> Unfinished => collection.Where(t => t.Model.IsFinished == false);
        
        public IEnumerable<TaskViewModel> Finished => collection.Where(t => t.Model.IsFinished);
    }
}
