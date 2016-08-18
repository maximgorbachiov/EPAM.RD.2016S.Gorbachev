using FibonachyGenerator.Generators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageInterfaces.IGenerators;

namespace UnitTests
{
    [TestClass]
    public class GeneratorTest
    {
        [TestMethod]
        public void MoveNext_FirstValueIsOne_Test()
        {
            IGenerator generator = new IdGenerator();
            generator.MoveNext();

            Assert.AreEqual(1, generator.Current);
        }

        [TestMethod]
        public void SetGeneratorState_SetToStateFive_Test()
        {
            IGenerator generator = new IdGenerator();
            generator.SetGeneratorState(5);

            Assert.AreEqual(5, generator.Current);
        }
    }
}
