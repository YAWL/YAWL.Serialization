namespace YAWL.Serialization
{
    public interface ISerializable
    {
        void Serialize(ISerializer serializer);
    }
}