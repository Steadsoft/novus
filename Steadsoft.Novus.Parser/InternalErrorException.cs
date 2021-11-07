using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox
{
    public class InternalErrorException : InvalidOperationException
    {
        public InternalErrorException (string Msg):base(Msg)
        {

        }
    }
}
