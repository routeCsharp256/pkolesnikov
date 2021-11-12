using System;

namespace OzonEdu.MerchandiseApi.Domain.Exceptions
{
    public class EmptyCollectionException : Exception
    {
        public EmptyCollectionException(string message) : base(message)
        { }

        public EmptyCollectionException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}