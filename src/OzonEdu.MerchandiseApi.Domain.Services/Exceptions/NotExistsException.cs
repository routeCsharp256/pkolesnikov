using System;

namespace OzonEdu.MerchandiseApi.Domain.Services.Exceptions
{
    public class NotExistsException : NullReferenceException
    {
        public NotExistsException(string message) : base(message)
        { }

        public NotExistsException(string message, Exception exception) : base(message, exception)
        { }
    }
}