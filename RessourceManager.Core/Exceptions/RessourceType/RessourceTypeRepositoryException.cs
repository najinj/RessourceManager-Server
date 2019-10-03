

using System;

namespace RessourceManager.Core.Exceptions.RessourceType
{
    public class RessourceTypeRepositoryException : Exception
    {
        public RessourceTypeRepositoryException()
        {
        }

        public RessourceTypeRepositoryException(string message)
            : base(message)
        {

        }

        public RessourceTypeRepositoryException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
