using System.ComponentModel.Composition.Hosting;
using NUnit.Framework;

namespace Beeffective.Tests
{
    public class TestFixture
    {
        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            Container = CompositionContainerFactory.Create();
        }

        public CompositionContainer Container { get; private set; }

        public TValue Get<TValue>() => Container.GetExportedValue<TValue>();
    }

    public class TestFixture<T> : TestFixture where T : class
    {
        [OneTimeSetUp]
        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            CreateSUT();
        }

        protected virtual void CreateSUT() => SUT = Get<T>();

        protected T SUT { get; private set; }
    }
}