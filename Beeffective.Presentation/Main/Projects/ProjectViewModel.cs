using System;
using Beeffective.Core.Models;
using Beeffective.Presentation.Annotations;
using Beeffective.Presentation.Common;

namespace Beeffective.Presentation.Main.Projects
{
    public class ProjectViewModel : ViewModel
    {
        public ProjectViewModel([NotNull] ProjectModel model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public ProjectModel Model { get; }
    }
}