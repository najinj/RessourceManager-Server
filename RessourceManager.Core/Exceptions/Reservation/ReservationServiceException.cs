using System;

namespace RessourceManager.Core.Exceptions.Reservation
{
    public class ReservationServiceException : Exception
    {
        public string[] Fields
        {
            get => Fields;
            set { Fields = value; }
        }

        public ReservationServiceException()
        {
        }

        public ReservationServiceException(string message, string[] fields)
            : base(message)
        {
            Fields = fields;
        }

        public ReservationServiceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
