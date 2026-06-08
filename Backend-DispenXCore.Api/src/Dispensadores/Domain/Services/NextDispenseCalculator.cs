using Backend_DispenXCore.Api.src.Dispensadores.Domain.Entities;

namespace Backend_DispenXCore.Api.src.Dispensadores.Domain.Services;

public class NextDispenseCalculator
{
    public DateTime CalculateNextDispenseAt(IEnumerable<Schedule> activeSchedules, DateTime now)
    {
        DateTime? next = null;
        foreach (var schedule in activeSchedules)
        {
            foreach (var day in schedule.FrequencyDays.Days)
            {
                // Encontrar la próxima ocurrencia de ese día y hora
                var candidate = GetNextOccurrence(now, day, schedule.ScheduledTime);
                if (next == null || candidate < next)
                    next = candidate;
            }
        }
        return next ?? DateTime.MaxValue;
    }

    private DateTime GetNextOccurrence(DateTime from, int targetDayOfWeek, TimeSpan time)
    {
        int currentDay = (int)from.DayOfWeek; // 0=domingo
        int daysUntilTarget = ((targetDayOfWeek - currentDay + 7) % 7);
        if (daysUntilTarget == 0 && from.TimeOfDay >= time)
            daysUntilTarget = 7; // ya pasó hoy, siguiente semana
        var nextDate = from.Date.AddDays(daysUntilTarget).Add(time);
        return nextDate;
    }
}