namespace Core.Messaging.Serilaizers
{
    public static class SerializerFactory<T> where T : ISerializer, new()
    {
        public static T Instance
        {
            get { return new T();}
        }
    }
}