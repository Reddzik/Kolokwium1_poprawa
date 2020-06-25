using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kolokwium1Poprawa.Exceptions
{
    public class MemberDoesntExistsException : Exception
    {
        public MemberDoesntExistsException(string msg) : base(msg)
        {

        }
    }
}
