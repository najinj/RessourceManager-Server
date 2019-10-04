using System;


namespace RessourceManager.Core.Exceptions.Asset
{
    public class AssetRepositoryException : Exception
    {
        public string Field { get; set; }
        public AssetRepositoryException()
        {
        }

        public AssetRepositoryException(string message, string field)
            : base(message)
        {
            Field = field;
        }

        public AssetRepositoryException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
