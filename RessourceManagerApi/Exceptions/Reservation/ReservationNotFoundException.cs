using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RessourceManagerApi.Exceptions.Reservation
{
    public class ReservationNotFoundException : Exception
    {
        public ReservationNotFoundException()
        {
        }

        public ReservationNotFoundException(string message)
            : base(message)
        {

        }

        public ReservationNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
