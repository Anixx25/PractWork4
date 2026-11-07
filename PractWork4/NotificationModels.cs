using System;

namespace NotificationLibrary
{
    /// <summary>
    /// Типы уведомлений (Info, Warning, Error, System)
    /// </summary>
    public enum NotificationType
    {
        Info,
        Warning,
        Error,
        System
    }

    /// <summary>
    /// Критерии сортировки уведомлений
    /// </summary>
    public enum SortNotificationBy
    {
        Date,
        Priority,
        Title
    }

    /// <summary>
    /// Класс, описывающий структуру уведомления
    /// </summary>
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Content { get; set; }
        public NotificationType Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Priority { get; set; } // От 0 (низкий) до 5 (высокий)
        public bool IsRead { get; set; }
    }

    /// <summary>
    /// Класс параметров фильтрации и сортировки
    /// </summary>
    public class NotificationFilterOptions
    {
        public bool? IsRead { get; set; }
        public NotificationType[]? Types { get; set; } // Список нужных типов
        public string? SearchText { get; set; }
        public int? MinPriority { get; set; }
        public SortNotificationBy? SortBy { get; set; } // По Date, Priority, Title
        public bool Descending { get; set; }
    }
}
