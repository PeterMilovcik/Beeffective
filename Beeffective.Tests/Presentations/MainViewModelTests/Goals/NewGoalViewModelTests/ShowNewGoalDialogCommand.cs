using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Beeffective.Tests.Presentations.MainViewModelTests.Goals.NewGoalViewModelTests
{
    public class ShowNewGoalDialogCommand : TestFixture
    {
        [Test]
        public void CanExecute_True() => 
            SUT.ShowNewGoalDialogCommand.CanExecute(null).Should().BeTrue();

        [Test]
        public async Task Execute_DialogIsShown()
        {
            await SUT.ShowNewGoalDialogCommand.ExecuteAsync();
            DialogDisplay.IsDialogShown.Should().BeTrue();
        }
    }
}