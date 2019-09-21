using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RessourceManagerApi.Exceptions.Asset
{
    public class AssetDuplicateKeyException : Exception
    {
        public AssetDuplicateKeyException()
        {
        }

        public AssetDuplicateKeyException(string message)
            : base(message)
        {

        }

        public AssetDuplicateKeyException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
