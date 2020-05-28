﻿using System.ComponentModel.Composition;
using Beeffective.Presentation.Common;

namespace Beeffective.Presentation.Main.Goals
{
    [Export]
    public class GoalsViewModel : ContentViewModel
    {
        [ImportingConstructor]
        public GoalsViewModel(Core core) : base(core)
        {
        }
    }
}
