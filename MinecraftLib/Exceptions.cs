using System;
using System.Collections.Generic;
using System.Text;

namespace MinecraftLib
{
    public class FailedAuthentication : Exception
    {
        public FailedAuthentication()
            : base("The selected username and password failed to authenticate!")
        { }
    }
}
