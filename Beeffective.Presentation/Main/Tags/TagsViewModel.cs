using System.ComponentModel.Composition;
using Beeffective.Core.Models;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Priority;

namespace Beeffective.Presentation.Main.Tags
{
    [Export]
    public class TagsViewModel : ContentViewModel
    {
        private TagModel selectedTag;

        [ImportingConstructor]
        public TagsViewModel(PriorityObservableCollection tasks) : base(tasks)
        {
        }

        public TagModel SelectedTag
        {
            get => selectedTag;
            set => SetProperty(ref selectedTag, value);
        }
    }
}
