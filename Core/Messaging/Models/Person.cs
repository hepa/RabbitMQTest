using ProtoBuf;

namespace Core.Messaging.Models
{
    [ProtoContract]
    public class Person
    {
        [ProtoMember(1)]
        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format("[Name : {0}]", Name);
        }
    }
}