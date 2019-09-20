using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RessourceManagerApi.Exceptions
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
