namespace Beeffective.Tests.MainViewModelTests.TopBarViewModelTests
{
    public class ShowAddGoalDialogCommand : TestFixture
    {
        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            MainViewModel.ShowAsync().GetAwaiter().GetResult();
            SUT.ShowAddGoalDialogCommand.Execute(null);
        }
    }
}