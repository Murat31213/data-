using System;

class Program
{
    static void Main()
    {
        // Входные данные
        string[] startTimes = { "08:00", "10:00", "11:00", "15:00", "15:30", "16:50" };
        int[] durations = { 30, 60, 30, 10, 10, 40 };
        string beginWorkingTime = "08:00";
        string endWorkingTime = "18:00";
        int consultationTime = 30;

        // Вывод входных данных
        Console.WriteLine("Входные данные:");
        Console.WriteLine("Занятые промежутки времени:");
        Console.WriteLine("Начало   | Длительность (мин.)");
        for (int i = 0; i < startTimes.Length; i++)
        {
            Console.WriteLine($"{startTimes[i]} | {durations[i]}");
        }
        Console.WriteLine($"Рабочее время: {beginWorkingTime} - {endWorkingTime}");
        Console.WriteLine($"Необходимое время для консультации: {consultationTime} минут");
        Console.WriteLine();

        // Вызов функции для нахождения свободных промежутков
        string[] freeIntervals = FindFreeIntervals(startTimes, durations, consultationTime, beginWorkingTime, endWorkingTime);

        // Вывод результата
        Console.WriteLine("Свободные временные промежутки:");
        foreach (var interval in freeIntervals)
        {
            if (!string.IsNullOrEmpty(interval)) // Проверяем на пустую строку
                Console.WriteLine(interval);
        }
    }

    static string[] FindFreeIntervals(string[] startTimes, int[] durations, int consultationTime, string beginWorkingTime, string endWorkingTime)
    {
        // Определяем количество занятых интервалов
        int busyCount = startTimes.Length;
        string[] freeIntervals = new string[100]; // Предполагаем максимальное количество свободных интервалов
        int freeCount = 0;

        // Создание массива занятых интервалов
        DateTime[] busyStarts = new DateTime[busyCount];
        DateTime[] busyEnds = new DateTime[busyCount];

        // Заполнение массива занятых интервалов
        for (int i = 0; i < busyCount; i++)
        {
            busyStarts[i] = DateTime.Parse(startTimes[i]);
            busyEnds[i] = busyStarts[i].AddMinutes(durations[i]);
        }

        // Добавление рабочего времени в занятые интервалы
        DateTime begin = DateTime.Parse(beginWorkingTime);
        DateTime endDay = DateTime.Parse(endWorkingTime);
        busyStarts = Append(busyStarts, busyCount, begin);
        busyEnds = Append(busyEnds, busyCount, endDay);
        busyCount++; // Увеличиваем количество занятых интервалов

        // Поиск свободных промежутков
        DateTime lastEnd = begin;

        for (int i = 0; i < busyCount; i++)
        {
            // Проверяем, есть ли свободное время между последним занятым интервалом и текущим
            if (lastEnd < busyStarts[i])
            {
                TimeSpan freeTime = busyStarts[i] - lastEnd;
                if (freeTime.TotalMinutes >= consultationTime)
                {
                    for (int j = 0; j <= freeTime.TotalMinutes - consultationTime; j += consultationTime)
                    {
                        DateTime freeStart = lastEnd.AddMinutes(j);
                        DateTime freeEnd = freeStart.AddMinutes(consultationTime);
                        freeIntervals[freeCount++] = $"{freeStart:HH:mm}-{freeEnd:HH:mm}";
                    }
                }
            }

            // Обновление последнего конца
            lastEnd = busyEnds[i] > lastEnd ? busyEnds[i] : lastEnd;
        }

        // Обрезаем массив до фактического количества свободных интервалов
        string[] result = new string[freeCount];
        Array.Copy(freeIntervals, result, freeCount);
        return result;
    }

    // Метод для добавления элемента в массив
    static DateTime[] Append(DateTime[] array, int count, DateTime value)
    {
        DateTime[] newArray = new DateTime[count + 1];
        Array.Copy(array, newArray, count);
        newArray[count] = value;
        return newArray;
    }
}
