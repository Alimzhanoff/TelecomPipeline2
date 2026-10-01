using System;
using System.Threading;

public static class CallProcessor {
    public static decimal ProcessCallsSequential(CallRecord[] records)
    {
        if (records == null) throw new ArgumentNullException(nameof(records));

        decimal totalCost = 0m;
        foreach (var record in records)
        {
            totalCost += CallPricing.CalculateCost(in record);
        }
        return totalCost;
    }

    
    public static decimal ProcessCallsParallel(CallRecord[] records)
    {
        if (records == null) throw new ArgumentNullException(nameof(records));

        if (records.Length == 0) return 0m;

        
        if (records.Length % 2 != 0)
        {
            throw new ArgumentException("Array length must be even for two-thread partitioning.");  
        }

        int mid = records.Length / 2;


        CallRecord[] firstHalf = records[..mid];
        CallRecord[] secondHalf = records[mid..];

        decimal[] results1 = new decimal[firstHalf.Length];
        decimal[] results2 = new decimal[secondHalf.Length];

        Exception? threadException1 = null;
        Exception? threadException2 = null;

        Thread thread1 = new Thread(() =>
        {
            try
            {
                for (int i = 0; i < firstHalf.Length; i++)
                {
                    results1[i] = CallPricing.CalculateCost(in firstHalf[i]);
                }
            }
            catch (Exception ex)
            {
                threadException1 = ex;
            }
        });

        Thread thread2 = new Thread(() =>
        {
            try
            {
                for (int i = 0; i < secondHalf.Length; i++)
                {
                    results2[i] = CallPricing.CalculateCost(in secondHalf[i]);
                }
            }
            catch (Exception ex)
            {
                threadException2 = ex;
            }
        });

        thread1.Start();
        thread2.Start();

        thread1.Join();
        thread2.Join();

        if (threadException1 != null) throw new AggregateException(threadException1);
        if (threadException2 != null) throw new AggregateException(threadException2);

        decimal totalSum = 0m;
        foreach (var val in results1) totalSum += val;
        foreach (var val in results2) totalSum += val;

        return totalSum;
    }
}
