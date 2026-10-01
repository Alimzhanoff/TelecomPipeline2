using System;

public static class CallPricing
{
    public static decimal CalculateCost(in CallRecord record)
    {
        if (string.IsNullOrWhiteSpace(record.RecordId) || string.IsNullOrWhiteSpace(record.DestinationCountry))
        {
            throw new ArgumentException("Invalid or default CallRecord provided.");
        }

        //For Nan?
        if (double.IsNaN(record.DurationMinutes) || double.IsInfinity(record.DurationMinutes))
        {
            throw new ArgumentException("Duration cannot be NaN or Infinity.");
        }

        //switch 
        return (record.IsRoaming, record.DestinationCountry, record.DurationMinutes) switch
        {
            
            var (_, _, duration) when duration < 0 || duration > 10000 => throw new ArgumentException("Duration out of range."),

            (true, "KZ", < 1.0) => 50.00m,

            (false, "KZ", var dur) => Math.Round((decimal)dur * 15.00m, 2, MidpointRounding.AwayFromZero),

            (true, _, >= 10.0) => Math.Round((decimal)record.DurationMinutes * 120.00m, 2, MidpointRounding.AwayFromZero),

            _ => Math.Round((decimal)record.DurationMinutes * 45.00m, 2, MidpointRounding.AwayFromZero)
        };
    }
}