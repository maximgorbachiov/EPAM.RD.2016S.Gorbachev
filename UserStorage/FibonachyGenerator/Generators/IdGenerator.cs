using System;
using System.Collections;
using System.Collections.Generic;
using StorageInterfaces.IGenerators;

namespace FibonachyGenerator.Generators
{
    [Serializable]
    public class IdGenerator : IGenerator
    {
        private readonly IEnumerator<int> iterator;

        public IdGenerator()
        {
            iterator = new FibonachyIterator();
        }

        public int Current
        {
            get
            {
                iterator.MoveNext();
                return iterator.Current;
            }
        }

        public void SetGeneratorState(int value)
        {
            while (iterator.Current < value)
            {
                iterator.MoveNext();
            }
        }

        [Serializable]
        private class FibonachyIterator : IEnumerator<int>
        {
            private int prevLastValue;
            private int lastValue = 1;
            private int currentValue;

            public int Current => currentValue;

            object IEnumerator.Current
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public bool MoveNext()
            {
                currentValue = prevLastValue + lastValue;
                prevLastValue = lastValue;
                lastValue = currentValue;
                return true;
            }

            public void Reset()
            {
                prevLastValue = 0;
                lastValue = 1;
                currentValue = 0;
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }
    }
}
