using Npgsql;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace Ежедневник
{
    public class DatabaseHelper
    {
        private string connectionString = "Host=localhost;Port=5432;Database=Ежедневник;Username=ilya;Password=111;";

        //------------------------------------------------------------------------------------------------------------
        // Заметки
        //------------------------------------------------------------------------------------------------------------
        public int GetTitleIdMax()
        {
            int maxId = 0;
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT MAX(id) FROM Заметка WHERE Тип = 'самостоятельная'";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        var result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                            maxId = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return maxId;
        }

        public int GetTitleIdMin()
        {
            int minId = 0;
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT MIN(id) FROM Заметка WHERE Тип = 'самостоятельная'";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        var result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                            minId = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return minId;
        }

        public string GetStringtionNotes(int id, string column)
        {
            string str = null;
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    string query = $"SELECT {column} FROM Заметка WHERE id = @id AND Тип = 'самостоятельная'";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        var result = cmd.ExecuteScalar();
                        if (result != null) str = result.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return str;
        }

        public void UpdateStringNotes(int id, string title, string description)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"UPDATE Заметка SET Заголовок = @title, Описание = @description 
                                    WHERE id = @id AND Тип = 'самостоятельная'";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@title", title);
                        cmd.Parameters.AddWithValue("@description", description);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            MessageBox.Show("Заметка не найдена или не является самостоятельной",
                                "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool DeleteNotes(int id)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM Заметка WHERE id = @id AND Тип = 'самостоятельная'";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("Заметка не найдена или не является самостоятельной",
                                "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public void CreateNotes(int id, string title, string description)
        {
            DateTime now = DateTime.Now;
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"INSERT INTO Заметка (id, Дата, Время, Заголовок, Описание, Тип)
                                    VALUES (@id, @date, @time, @title, @discription, @type);";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@date", now.Date);
                        cmd.Parameters.AddWithValue("@time", now.TimeOfDay);
                        cmd.Parameters.AddWithValue("@title", title);
                        cmd.Parameters.AddWithValue("@discription", description);
                        cmd.Parameters.AddWithValue("@type", "самостоятельная");
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Заметка успешно создана!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public int GetNextExistingId(int currentId)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT id FROM Заметка WHERE id > @currentId AND Тип = 'самостоятельная' ORDER BY id LIMIT 1";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@currentId", currentId);
                        var result = cmd.ExecuteScalar();
                        if (result != null)
                            return Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return -1;
        }

        public int GetPreviousExistingId(int currentId)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT id FROM Заметка WHERE id < @currentId AND Тип = 'самостоятельная' ORDER BY id DESC LIMIT 1";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@currentId", currentId);
                        var result = cmd.ExecuteScalar();
                        if (result != null)
                            return Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return -1;
        }

        public bool HasAnyNotes()
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Заметка WHERE Тип = 'самостоятельная'";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        //------------------------------------------------------------------------------------------------------------
        // Задачи
        //------------------------------------------------------------------------------------------------------------

        /*public int GetCount()
        {
            int count = 0;
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Задача";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return count;
            }
        }*/

        public List<int> GetAllTaskIds()
        {
            List<int> ids = new List<int>();
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT id FROM Задача ORDER BY id";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ids.Add(reader.GetInt32(0));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return ids;
        }

        public string GetRowString(int id, string column)
        {
            string title = null;
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    string query = $"SELECT {column} FROM Задача WHERE id = @id";
                    using (var cmd = new NpgsqlCommand(query , conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        var result = cmd.ExecuteScalar();
                        if (result != null) title = result.ToString();

                        return title;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return title;
            }
        }

        public void UpdateTask(int taskIndex, string title, int status, string date, string time, string description)
        {

            DateOnly dateOnly = DateOnly.ParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            TimeOnly timeOnly = TimeOnly.ParseExact(time, "HH:mm", CultureInfo.InvariantCulture);

            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"UPDATE Задача SET Дата = @date, Время = @time, Заголовок = @title, Описание = @description, Статус = @status
                                    WHERE id = @taskIndex";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@taskIndex", taskIndex);
                        cmd.Parameters.AddWithValue("@date", dateOnly);
                        cmd.Parameters.AddWithValue("@time", timeOnly);
                        cmd.Parameters.AddWithValue("@title", title);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@status", status);

                        cmd.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}