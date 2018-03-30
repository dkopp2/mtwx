using System;

namespace Mtwx.Web.Commands
{
    public class CommandResult
    {
    }

    public class InvalidCommandSpecifiedException : Exception
    {
        public InvalidCommandSpecifiedException(string message)
            : base(message)
        {
        }
    }

    public class UnknownCommandHandlerException<T> : Exception
    {
        public UnknownCommandHandlerException() : base($"Unknown Command handler: ({typeof(T).Name})")
        {
        }
    }
    public class UnknownCommandHandlerException<T1, T2> : Exception
    {
        public UnknownCommandHandlerException() : base($"Unknown Command handler: ({typeof(T1).Name}:{typeof(T2).Name})")
        {
        }
    }
}