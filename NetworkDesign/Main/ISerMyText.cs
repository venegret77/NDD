using System.Runtime.Serialization;

namespace NetworkDesign.Main
{
    interface ISerMyText
    {
        void GetObjectData(SerializationInfo info, StreamingContext context);
    }
}