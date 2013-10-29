namespace MedArchon.Common.Commands.Bus
{
    public enum ValidationCode
    {
        IsRequired,
        MustBeADefinedEnumValue,
        ExceedsMaximumCharacterLength,
        MustBeAlphanumeric,
        UnknownValue,
        MustBeAPostiveNumber,
        AllDocumentsMustBeUploaded,
        CaseCanOnlyBeBundledOnce,
        DoesNotMeetMinimumCharacterLength,
        DateOfServiceToMustBeAfterDateOfServiceFrom
    }
}