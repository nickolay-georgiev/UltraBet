namespace UltraBet.Services
{
    public interface ISerializationService
    {
        T DeserializeSportData<T>(string input);
    }
}
