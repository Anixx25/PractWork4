using System;
using System.Collections.Generic;
using System.Linq;

namespace NotificationLibrary
{
    public class NotificationService
    {
        /// <summary>
        /// Фильтрует и сортирует список уведомлений на основе переданных опций.
        /// </summary>
        public IEnumerable<Notification> FilterAndSort(
            IEnumerable<Notification>? notifications,
            NotificationFilterOptions? options)
        {
            // --- Защитное программирование (Принцип Single Responsibility / SOLID) ---
            // Если коллекция не передана, возвращаем пустой перечисляемый список во избежание NullReferenceException
            if (notifications == null)
            {
                return Enumerable.Empty<Notification>();
            }

            // Если параметры фильтрации не заданы, возвращаем исходный список
            if (options == null)
            {
                return notifications;
            }

            // Используем ленивые вычисления LINQ (выполняются только при итерации)
            var query = notifications;

            // --- 1. Фильтрация данных ---

            // Фильтрация по статусу прочтения (IsRead)
            if (options.IsRead.HasValue)
            {
                query = query.Where(n => n.IsRead == options.IsRead.Value);
            }

            // Фильтрация по массиву типов (Types)
            if (options.Types != null && options.Types.Length > 0)
            {
                query = query.Where(n => options.Types.Contains(n.Type));
            }

            // Регистронезависимый поиск по подстроке в Title или Content
            if (!string.IsNullOrWhiteSpace(options.SearchText))
            {
                var text = options.SearchText.Trim();
                query = query.Where(n =>
                    (n.Title != null && n.Title.Contains(text, StringComparison.OrdinalIgnoreCase)) ||
                    (n.Content != null && n.Content.Contains(text, StringComparison.OrdinalIgnoreCase))
                );
            }

            // Фильтрация по минимальному приоритету (MinPriority)
            if (options.MinPriority.HasValue)
            {
                query = query.Where(n => n.Priority >= options.MinPriority.Value);
            }

            // --- 2. Сортировка данных ---
            if (options.SortBy.HasValue)
            {
                query = options.SortBy.Value switch
                {
                    SortNotificationBy.Date => options.Descending
                        ? query.OrderByDescending(n => n.CreatedAt)
                        : query.OrderBy(n => n.CreatedAt),

                    SortNotificationBy.Priority => options.Descending
                        ? query.OrderByDescending(n => n.Priority)
                        : query.OrderBy(n => n.Priority),

                    SortNotificationBy.Title => options.Descending
                        ? query.OrderByDescending(n => n.Title)
                        : query.OrderBy(n => n.Title),

                    _ => query
                };
            }

            return query;
        }
    }
}
