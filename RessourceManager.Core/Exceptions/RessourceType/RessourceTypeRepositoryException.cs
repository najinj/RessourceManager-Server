
using System;

namespace RessourceManager.Core.Exceptions.RessourceType
{
    public class RessourceTypeRepositoryException : Exception
    {
        public string Field { get; set; }
        public RessourceTypeRepositoryException()
        {
        }

        public RessourceTypeRepositoryException(string message,string field)
            : base(message)
        {
            Field = field;
        }

        public RessourceTypeRepositoryException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
