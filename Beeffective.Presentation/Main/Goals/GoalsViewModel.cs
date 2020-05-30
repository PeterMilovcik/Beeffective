using System.ComponentModel.Composition;
using Beeffective.Presentation.Common;
using Beeffective.Presentation.Main.Dialogs;
using Beeffective.Services.Repository;

namespace Beeffective.Presentation.Main.Goals
{
    [Export]
    public class GoalsViewModel : ContentViewModel
    {
        [ImportingConstructor]
        public GoalsViewModel(Core core, IDialogDisplay dialogDisplay, IRepositoryService repository) : base(core)
        {
            New = new NewGoalViewModel(core, dialogDisplay, repository);
        }

        public NewGoalViewModel New { get; }
    }
}
