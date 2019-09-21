using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RessourceManagerApi.Exceptions.Space
{
    public class SpaceNotFoundException : Exception
    {
       // public string type { get;set; }
        public SpaceNotFoundException()
        {
        }

        public SpaceNotFoundException(string message)
            : base(message)
        {

        }

        public SpaceNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
