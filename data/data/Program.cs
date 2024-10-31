using System;
using System.Collections.Generic;
using System.Linq;

public class ScheduleHelper
{
    public static List<string> GetAvailableSlots(
        List<string> startTimes,
        List<int> durations,
        string beginWorkingTime,
        string endWorkingTime,
        int consultationTime
    )
    {
        TimeSpan workStart = TimeSpan.Parse(beginWorkingTime);
        TimeSpan workEnd = TimeSpan.Parse(endWorkingTime);

        List<(TimeSpan Start, TimeSpan End)> busyIntervals = new List<(TimeSpan, TimeSpan)>();

        // Формируем список занятых интервалов
        for (int i = 0; i < startTimes.Count; i++)
        {
            TimeSpan start = TimeSpan.Parse(startTimes[i]);
            TimeSpan end = start + TimeSpan.FromMinutes(durations[i]);
            busyIntervals.Add((start, end));
        }

        // Сортируем занятые интервалы
        busyIntervals = busyIntervals.OrderBy(interval => interval.Start).ToList();

        List<string> availableSlots = new List<string>();
        TimeSpan currentFreeStart = workStart;

        // Поиск свободных интервалов между занятыми
        foreach (var (busyStart, busyEnd) in busyIntervals)
        {
            if (busyStart > currentFreeStart && (busyStart - currentFreeStart).TotalMinutes >= consultationTime)
            {
                availableSlots.Add($"{currentFreeStart:hh\\:mm}-{busyStart:hh\\:mm}");
            }
            currentFreeStart = busyEnd > currentFreeStart ? busyEnd : currentFreeStart;
        }

        // Проверка последнего свободного интервала до конца рабочего дня
        if (workEnd > currentFreeStart && (workEnd - currentFreeStart).TotalMinutes >= consultationTime)
        {
            availableSlots.Add($"{currentFreeStart:hh\\:mm}-{workEnd:hh\\:mm}");
        }

        return availableSlots;
    }

    public static void Main()
    {
        List<string> startTimes = new List<string> { "10:00", "11:00", "15:00", "15:30", "16:50" };
        List<int> durations = new List<int> { 60, 30, 10, 10, 40 };
        string beginWorkingTime = "08:00";
        string endWorkingTime = "18:00";
        int consultationTime = 30;

        List<string> availableSlots = GetAvailableSlots(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

        Console.WriteLine("Свободные временные интервалы:");
        foreach (string slot in availableSlots)
        {
            Console.WriteLine(slot);
        }
    }
}
