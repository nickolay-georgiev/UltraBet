namespace UltraBet.Services
{
    public interface IDeserializer
    {
        T Deserialize<T>(string input);
    }
}
