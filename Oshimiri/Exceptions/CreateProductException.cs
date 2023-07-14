using System.Runtime.Serialization;

namespace Oshimiri.Exceptions
{
    [Serializable]
    public class CreateProductException : Exception
    {
        public CreateProductException()
        {
        }

        public CreateProductException(string? message) : base(message)
        {
        }

        public CreateProductException(string? message, Exception? innerException) : base(message, innerException)
        {
        }


        protected CreateProductException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}