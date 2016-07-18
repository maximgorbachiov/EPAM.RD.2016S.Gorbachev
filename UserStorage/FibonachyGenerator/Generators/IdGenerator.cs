using System;
using System.Collections;
using FibonachyGenerator.Interfaces;

namespace FibonachyGenerator.Generators
{
    public class IdGenerator : IGeneratorId
    {
        private int prevLastValue = 0;
        private int lastValue = 1;
        private int currentValue;

        public int Current
        {
            get
            {
                return currentValue;
            }
        }

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
            
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
