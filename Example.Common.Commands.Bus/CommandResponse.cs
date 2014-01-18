using System.Collections.Generic;

namespace Example.Common.Commands.Bus
{
    public class CommandResponse
    {
        public bool Success { get; set; }
        public IList<ValidationStatus> StatusCodes { get; set; }
    }
}