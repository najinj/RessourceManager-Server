using System;


namespace RessourceManagerApi.Exceptions.RessourceType
{
    public class RessourceTypeDuplicateKeyException : Exception
    {
        public RessourceTypeDuplicateKeyException()
        {
        }

        public RessourceTypeDuplicateKeyException(string message)
            : base(message)
        {

        }

        public RessourceTypeDuplicateKeyException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
