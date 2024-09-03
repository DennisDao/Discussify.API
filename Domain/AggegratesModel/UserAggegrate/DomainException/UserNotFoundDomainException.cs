using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.AggegratesModel.UserAggegrate.DomainException
{
    public class UserNotFoundDomainException : System.Exception
    {
        public UserNotFoundDomainException()
        { }

        public UserNotFoundDomainException(string message)
            : base(message)
        { }

        public UserNotFoundDomainException(string message, System.Exception innerException)
            : base(message, innerException)
        { }
    }
}
