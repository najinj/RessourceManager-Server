using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RessourceManagerApi.Exceptions.Asset
{
    public class AssetNotFoundException : Exception
    {
        public AssetNotFoundException()
        {
        }

        public AssetNotFoundException(string message)
            : base(message)
        {

        }

        public AssetNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
