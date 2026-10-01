using System;

public readonly record struct CallRecord
{
    public string RecordId { get; init; }
    public string DestinationCountry { get; init; }
    public double DurationMinutes { get; init; }
    public bool IsRoaming { get; init; }

    public CallRecord(string recordId, string destinationCountry, double durationMinutes, bool isRoaming)
    {
        if (string.IsNullOrWhiteSpace(recordId))
        {
            throw new ArgumentException("RecordId cannot be null, empty, or whitespace.", nameof(recordId));
        }

        if (string.IsNullOrWhiteSpace(destinationCountry))
        {
            throw new ArgumentException("DestinationCountry cannot be null, empty, or whitespace.", nameof(destinationCountry));
        }

        if (double.IsNaN(durationMinutes) || double.IsInfinity(durationMinutes) || durationMinutes < 0 || durationMinutes > 10000)
        {
            throw new ArgumentException("DurationMinutes must be a finite, non-negative number no greater than 10,000.", nameof(durationMinutes));
        }

        RecordId = recordId;
        DestinationCountry = destinationCountry;
        DurationMinutes = durationMinutes;
        IsRoaming = isRoaming;
    }
}