using System.ComponentModel.Composition;

namespace Beeffective.Presentation.Common
{
    public class CoreViewModel : Initializable
    {
        [ImportingConstructor]
        public CoreViewModel(Main.Core core)
        {
            Core = core;
        }

        public Main.Core Core { get; }
    }
}