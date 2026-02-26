using System;
using System.IO;
using System.Windows.Forms;
using Npgsql;

namespace Ежедневник
{
    public class CreateReport
    {
        private string connectionString = "Host=localhost;Port=5432;Database=Ежедневник;Username=ilya;Password=111;";
        private string reportFilePath = "Отчет_Ежедневник.txt";

        public CreateReport()
        {
            GenerateReport();
        }

        private void GenerateReport()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(reportFilePath, false, System.Text.Encoding.UTF8))
                {
                    writer.WriteLine("================= ОТЧЕТ ЕЖЕДНЕВНИК =================");
                    writer.WriteLine($"Дата создания: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
                    writer.WriteLine("================================================\n");

                    // Сохраняем заметки
                    SaveNotesToFile(writer);

                    writer.WriteLine("\n" + new string('-', 50) + "\n");

                    // Сохраняем задачи
                    SaveTasksToFile(writer);

                    writer.WriteLine("\n================================================");
                    writer.WriteLine("КОНЕЦ ОТЧЕТА");
                }

                MessageBox.Show($"Отчет успешно сохранен в файл:\n{Path.GetFullPath(reportFilePath)}",
                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании отчета: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveNotesToFile(StreamWriter writer)
        {
            writer.WriteLine("ЗАМЕТКИ:");
            writer.WriteLine("---------");

            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT id, Дата, Время, Заголовок, Описание, Тип, задача_id 
                                    FROM Заметка ORDER BY id";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        int count = 0;
                        while (reader.Read())
                        {
                            count++;
                            writer.WriteLine($"Заметка {count} | ID: {reader["id"]} | Дата: {ConvertToDateString(reader["Дата"])} | " +
                                           $"Время: {ConvertToTimeString(reader["Время"])}");
                            writer.WriteLine($"Заголовок | {reader["Заголовок"]}");
                            writer.WriteLine($"Описание | {reader["Описание"]}");
                            writer.WriteLine($"Тип | {reader["Тип"]}");

                            if (reader["задача_id"] != DBNull.Value)
                                writer.WriteLine($"ID задачи | {reader["задача_id"]}");

                            writer.WriteLine("------------------------");
                        }

                        if (count == 0)
                            writer.WriteLine("Нет заметок в базе данных");
                    }
                }
            }
            catch (Exception ex)
            {
                writer.WriteLine($"Ошибка при загрузке заметок: {ex.Message}");
            }
        }

        private void SaveTasksToFile(StreamWriter writer)
        {
            writer.WriteLine("\nЗАДАЧИ:");
            writer.WriteLine("--------");

            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT id, Дата, Время, Заголовок, Описание, Статус 
                                    FROM Задача ORDER BY id";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        int count = 0;
                        while (reader.Read())
                        {
                            count++;
                            int status = Convert.ToInt32(reader["Статус"]);
                            string statusText = status == 1 ? "✅ Готово" : "❌ Не готово";

                            writer.WriteLine($"Задача {count} | ID: {reader["id"]} | Дата: {ConvertToDateString(reader["Дата"])} | " +
                                           $"Время: {ConvertToTimeString(reader["Время"])}");
                            writer.WriteLine($"Заголовок | {reader["Заголовок"]}");
                            writer.WriteLine($"Описание | {reader["Описание"]}");
                            writer.WriteLine($"Статус | {statusText} ({status})");

                            AddTaskComments(writer, Convert.ToInt32(reader["id"]));

                            writer.WriteLine("------------------------");
                        }

                        if (count == 0)
                            writer.WriteLine("Нет задач в базе данных");
                    }
                }
            }
            catch (Exception ex)
            {
                writer.WriteLine($"Ошибка при загрузке задач: {ex.Message}");
            }
        }

        private void AddTaskComments(StreamWriter writer, int taskId)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT id, Дата, Время, Описание 
                                    FROM Заметка 
                                    WHERE задача_id = @taskId AND Тип = 'комментарий'
                                    ORDER BY id";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@taskId", taskId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            int commentCount = 0;
                            while (reader.Read())
                            {
                                commentCount++;
                                writer.WriteLine($"  Комментарий {commentCount} | ID: {reader["id"]} | " +
                                               $"Дата: {ConvertToDateString(reader["Дата"])} | " +
                                               $"Время: {ConvertToTimeString(reader["Время"])}");
                                writer.WriteLine($"  Текст | {reader["Описание"]}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                writer.WriteLine($"  Ошибка при загрузке комментариев: {ex.Message}");
            }
        }

        private string ConvertToDateString(object dateObj)
        {
            if (dateObj == null || dateObj == DBNull.Value)
                return "Н/Д";

            if (dateObj is DateOnly dateOnly)
                return dateOnly.ToString("dd.MM.yyyy");
            else if (dateObj is DateTime dateTime)
                return dateTime.ToString("dd.MM.yyyy");

            return dateObj.ToString();
        }

        private string ConvertToTimeString(object timeObj)
        {
            if (timeObj == null || timeObj == DBNull.Value)
                return "Н/Д";

            if (timeObj is TimeOnly timeOnly)
                return timeOnly.ToString("HH:mm");
            else if (timeObj is TimeSpan timeSpan)
                return timeSpan.ToString(@"hh\:mm");
            else if (timeObj is DateTime dateTime)
                return dateTime.ToString("HH:mm");

            return timeObj.ToString();
        }
    }
}