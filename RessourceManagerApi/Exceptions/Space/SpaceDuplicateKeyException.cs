using System;


namespace RessourceManagerApi.Exceptions.Space
{
    public class SpaceDuplicateKeyException : Exception
    {
        public SpaceDuplicateKeyException()
        {
        }

        public SpaceDuplicateKeyException(string message)
            : base(message)
        {

        }

        public SpaceDuplicateKeyException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
