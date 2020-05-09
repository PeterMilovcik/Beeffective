using System.Threading.Tasks;

namespace Beeffective.Presentation.Common
{
    public class Initializable : ViewModel
    {
        public virtual Task InitializeAsync() => Task.CompletedTask;
    }
}