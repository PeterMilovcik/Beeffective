using System;
using Beeffective.Core.Models;
using Beeffective.Presentation.Annotations;
using Beeffective.Presentation.Common;

namespace Beeffective.Presentation.Main.Goals
{
    public class GoalViewModel : ViewModel
    {
        public GoalViewModel([NotNull] GoalModel model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public GoalModel Model { get; }
    }
}