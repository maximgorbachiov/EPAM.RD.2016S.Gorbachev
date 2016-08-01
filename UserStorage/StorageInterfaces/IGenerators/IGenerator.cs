namespace StorageInterfaces.IGenerators
{
    public interface IGenerator
    {
        int Current { get; }

        void MoveNext();

        void SetGeneratorState(int value);
    }
}
