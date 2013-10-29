using System.Collections.Generic;

namespace MedArchon.Common.Commands.Bus
{
    public class CommandResponse
    {
        public bool Success { get; set; }
        public IList<ValidationStatus> StatusCodes { get; set; }
    }
}