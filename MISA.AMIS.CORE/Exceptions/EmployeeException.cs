﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.CORE.Exceptions
{
    public class EmployeeException : Exception
    {
        public EmployeeException(String message) : base(message)
        {

        }
    }
}
