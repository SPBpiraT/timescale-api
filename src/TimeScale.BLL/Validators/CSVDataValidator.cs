using FluentValidation;
using TimeScale.BLL.Models.Value;

namespace TimeScale.BLL.Validators
{
    internal class CSVDataValidator : AbstractValidator<ValueDto>
    {
        private static readonly DateTime MinDate = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public CSVDataValidator()
        {
            RuleFor(x => x.Date)
                .Must(date => date <= DateTime.UtcNow) //TODO: DateTimeProvider
                .WithMessage("Date cannot be later than the current date.")
                .Must(date => date >= MinDate)
                .WithMessage("Date cannot be earlier than 01/01/2000.");

            RuleFor(x => x.ExecutionTime)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Execution time cannot be less than 0.");

            RuleFor(x => x.Value)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Value cannot be less than 0.");
        }
    }
}
