using System;
using System.Collections.Generic;
using System.Text;

namespace RessourceManager.Core.Exceptions.Space
{
    public class SpaceRepositoryException : Exception
    {
        public string Field { get; set; }
        public SpaceRepositoryException()
        {
        }

        public SpaceRepositoryException(string message, string field)
            : base(message)
        {
            Field = field;
        }

        public SpaceRepositoryException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
