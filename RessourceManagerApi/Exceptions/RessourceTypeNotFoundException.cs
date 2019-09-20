using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RessourceManagerApi.Exceptions
{
    public class RessourceTypeNotFoundException : Exception
    {
       // public string type { get;set; }
        public RessourceTypeNotFoundException()
        {
        }

        public RessourceTypeNotFoundException(string message)
            : base(message)
        {

        }

        public RessourceTypeNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
