using CinemaTicketSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;

namespace CinemaTicketSystem.Helpers
{
    public class DatabaseHelper
    {
        public string connectionString = $"Data Source={Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CinemaTicket_db.db")};Version=3;Pooling=True;";

        public SQLiteConnection Connect()
        {
            var connection = new SQLiteConnection(connectionString);
            connection.Open();
            return connection;
        }

       
        public void CreateDatabase()
        {
            using (var connection = Connect())
            {
                string dropSeats = "DROP TABLE IF EXISTS Seats";
                string dropSalons = "DROP TABLE IF EXISTS Salons";

                var command = new SQLiteCommand(dropSeats, connection);
                command.ExecuteNonQuery();

                command = new SQLiteCommand(dropSalons, connection);
                command.ExecuteNonQuery();

                string createSalonTable = @"
            CREATE TABLE IF NOT EXISTS Salons (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT NOT NULL,
                film_name TEXT NOT NULL,
                film_time TEXT NOT NULL
            )";
               string createSeatsTable = @"
            CREATE TABLE IF NOT EXISTS Seats (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                salon_id INTEGER NOT NULL,
                seat_number INTEGER NOT NULL,
                is_booked INTEGER NOT NULL DEFAULT 0,
                FOREIGN KEY (salon_id) REFERENCES Salons(id) ON DELETE CASCADE
            )";

                command = new SQLiteCommand(createSalonTable, connection);
                command.ExecuteNonQuery();

                command = new SQLiteCommand(createSeatsTable, connection);
                command.ExecuteNonQuery();
            }
        }


        public void AddSalon(string name, string filmName, string time)
        {
            string query = "INSERT INTO Salons (name, film_name, film_time) VALUES (@name, @filmName, @filmtime)";
            using (var connection = Connect())
            {
                var command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@filmName", filmName);
                command.Parameters.AddWithValue("@filmtime", time);
                command.ExecuteNonQuery();
            }
        }


        // Koltuk eklemek için metot
        public void AddSeats(int salonId, int seatCount)
        {
            using (var connection = Connect())
            {
                for (int i = 1; i <= seatCount; i++)
                {
                    string insertSeat = "INSERT INTO Seats (salon_id, seat_number, is_booked) VALUES (@salonId, @seatNumber, @isBooked)";
                    var command = new SQLiteCommand(insertSeat, connection);
                    command.Parameters.AddWithValue("@salonId", salonId);
                    command.Parameters.AddWithValue("@seatNumber", i);
                    command.Parameters.AddWithValue("@isBooked", 0); // Başlangıçta koltuklar boş
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Salon> GetSalonlarWithMovies()
        {
            List<Salon> salonlar = new List<Salon>();

            using (var connection = Connect())
            {
                string query = @"
                    SELECT s.id, s.name, s.film_name, s.film_time
                    FROM Salons s
                    ORDER BY s.name, s.film_time";

                var command = new SQLiteCommand(query, connection);
                var reader = command.ExecuteReader();

                Salon currentSalon = null;

                while (reader.Read())
                {
                    string salonName = reader["name"].ToString();
                    string filmName = reader["film_name"].ToString();
                    string filmTime = reader["film_time"].ToString();

                    // Yeni salon geldiğinde yeni bir Salon nesnesi oluştur
                    if (currentSalon == null || currentSalon.Name != salonName)
                    {
                        currentSalon = new Salon
                        {
                            Name = salonName,
                            Films = new List<Film>()
                        };
                        salonlar.Add(currentSalon);
                    }

                    // Filmi o salona ekle
                    currentSalon.Films.Add(new Film
                    {
                        Name = filmName,
                        Time = filmTime
                    });
                }
            }

            return salonlar;
        }

        public bool[] GetSeats(int salonId)
        {
            int seatCount = 0;
            using (var connection = Connect())
            {
                string query = "SELECT COUNT(*) FROM Seats WHERE salon_id = @salonId";
                var command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@salonId", salonId);
                object result = command.ExecuteScalar();
                seatCount = result != null ? Convert.ToInt32(result) : 0;
            }

            bool[] seats = new bool[seatCount];

            using (var connection = Connect())
            {
                string query = "SELECT seat_number, is_booked FROM Seats WHERE salon_id = @salonId";
                var command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@salonId", salonId);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int seatNumber = reader["seat_number"] is long
                        ? Convert.ToInt32((long)reader["seat_number"])
                        : Convert.ToInt32(reader["seat_number"]);
                    bool isBooked = Convert.ToInt32(reader["is_booked"]) == 1;

                    if (seatNumber - 1 >= 0 && seatNumber - 1 < seatCount)
                        seats[seatNumber - 1] = isBooked;
                }
            }

            return seats;
        }
        public bool IsSeatBooked(int salonId, int seatNumber)
        {
            using (var connection = Connect())
            {
                string query = "SELECT is_booked FROM Seats WHERE salon_id = @salonId AND seat_number = @seatNumber";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@salonId", salonId);
                    command.Parameters.AddWithValue("@seatNumber", seatNumber);
                    var result = command.ExecuteScalar();
                    return result != null && Convert.ToBoolean(result);
                }
            }
        }



        // Koltuk durumu güncelleme
        public async Task UpdateSeatStatusAsync(int salonId, int seatNumber, bool isBooked)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                await connection.OpenAsync();
                string query = "UPDATE Seats SET is_booked = @isBooked WHERE salon_id = @salonId AND seat_number = @seatNumber";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@salonId", salonId);
                    command.Parameters.AddWithValue("@seatNumber", seatNumber);
                    command.Parameters.AddWithValue("@isBooked", isBooked ? 1 : 0);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}

