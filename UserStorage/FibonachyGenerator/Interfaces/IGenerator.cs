namespace FibonachyGenerator.Interfaces
{
    public interface IGenerator
    {
        int Current { get; }

        void SetGeneratorState(int value);
    }
}
