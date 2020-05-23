using System.ComponentModel.Composition.Hosting;
using NUnit.Framework;

namespace Beeffective.Tests
{
    public class TestFixture<T> where T : class
    {
        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            Container = CompositionContainerFactory.Create();
            CreateSUT();
        }

        public CompositionContainer Container { get; private set; }

        protected virtual void CreateSUT() => SUT = Get<T>();

        public TValue Get<TValue>() => Container.GetExportedValue<TValue>();

        public T SUT { get; private set; }
    }
}