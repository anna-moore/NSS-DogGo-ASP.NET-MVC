using System;
using System.Runtime.Serialization;

namespace DogGo.Repositories
{
    [Serializable]
    internal class NotImplementExpection : Exception
    {
        public NotImplementExpection()
        {
        }

        public NotImplementExpection(string message) : base(message)
        {
        }

        public NotImplementExpection(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotImplementExpection(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}