using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RessourceManagerApi.Exceptions.Reservation
{
    public class ReservationDuplicateKeyException : Exception
    {
        public ReservationDuplicateKeyException()
        {
        }

        public ReservationDuplicateKeyException(string message)
            : base(message)
        {

        }

        public ReservationDuplicateKeyException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
