namespace PeakLims.Services;

public interface IDateTimeProvider
{
    DateTime DateTimeUtcNow { get; }
    DateTime DateTimeNow { get; }
    DateTimeOffset DateTimeOffsetNow { get; }
    DateOnly DateOnlyUtcNow { get; }
    DateOnly DateOnlyNow { get; }
}

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime DateTimeUtcNow => DateTime.UtcNow;
    public DateTime DateTimeNow => DateTime.Now;
    public DateTimeOffset DateTimeOffsetNow => DateTimeOffset.Now;
    public DateOnly DateOnlyUtcNow => DateOnly.FromDateTime(DateTimeUtcNow);
    public DateOnly DateOnlyNow => DateOnly.FromDateTime(DateTimeNow);
}