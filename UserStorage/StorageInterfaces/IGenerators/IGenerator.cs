namespace StorageInterfaces.IGenerators
{
    public interface IGenerator
    {
        int Current { get; }

        void SetGeneratorState(int value);
    }
}
