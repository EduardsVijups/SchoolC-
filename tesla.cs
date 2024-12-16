using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace TeslaRental
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.Initialize();

            // Example of usage
            Console.WriteLine("Tesla Rental Backend Initialized.");
        }
    }

    public static class Database
    {
        private const string ConnectionString = "Data Source=tesla_rental.db;Version=3;";

        public static void Initialize()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                var carTable = @"CREATE TABLE IF NOT EXISTS Cars (
                                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Model TEXT NOT NULL,
                                    HourlyRate REAL NOT NULL,
                                    PerKmRate REAL NOT NULL
                                );";

                var clientTable = @"CREATE TABLE IF NOT EXISTS Clients (
                                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Name TEXT NOT NULL,
                                    Email TEXT NOT NULL UNIQUE
                                );";

                var rentalTable = @"CREATE TABLE IF NOT EXISTS Rentals (
                                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                    ClientID INTEGER NOT NULL,
                                    CarID INTEGER NOT NULL,
                                    StartTime DATETIME NOT NULL,
                                    EndTime DATETIME,
                                    KilometersDriven REAL,
                                    TotalAmount REAL,
                                    FOREIGN KEY(ClientID) REFERENCES Clients(ID),
                                    FOREIGN KEY(CarID) REFERENCES Cars(ID)
                                );";

                ExecuteNonQuery(connection, carTable);
                ExecuteNonQuery(connection, clientTable);
                ExecuteNonQuery(connection, rentalTable);
            }
        }

        public static void ExecuteNonQuery(SQLiteConnection connection, string query)
        {
            using (var command = new SQLiteCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        public static SQLiteConnection GetConnection() => new SQLiteConnection(ConnectionString);
    }

    public class Car
    {
        public int ID { get; set; }
        public string Model { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal PerKmRate { get; set; }

        public static void AddCar(string model, decimal hourlyRate, decimal perKmRate)
        {
            using (var connection = Database.GetConnection())
            {
                connection.Open();
                var query = "INSERT INTO Cars (Model, HourlyRate, PerKmRate) VALUES (@model, @hourlyRate, @perKmRate)";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@model", model);
                    command.Parameters.AddWithValue("@hourlyRate", hourlyRate);
                    command.Parameters.AddWithValue("@perKmRate", perKmRate);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static List<Car> GetAllCars()
        {
            using (var connection = Database.GetConnection())
            {
                connection.Open();
                var query = "SELECT * FROM Cars";
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    var cars = new List<Car>();
                    while (reader.Read())
                    {
                        cars.Add(new Car
                        {
                            ID = reader.GetInt32(0),
                            Model = reader.GetString(1),
                            HourlyRate = reader.GetDecimal(2),
                            PerKmRate = reader.GetDecimal(3)
                        });
                    }
                    return cars;
                }
            }
        }
    }

    public class Client
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public static void RegisterClient(string name, string email)
        {
            using (var connection = Database.GetConnection())
            {
                connection.Open();
                var query = "INSERT INTO Clients (Name, Email) VALUES (@name, @Email)";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@Email", email);
                    command.ExecuteNonQuery();
                }
            }
        }
    }

    public class Rental
    {
        public int ID { get; set; }
        public int ClientID { get; set; }
        public int CarID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double? KilometersDriven { get; set; }
        public decimal? TotalAmount { get; set; }

        public static void StartRental(int clientId, int carId)
        {
            using (var connection = Database.GetConnection())
            {
                connection.Open();
                var query = "INSERT INTO Rentals (ClientID, CarID, StartTime) VALUES (@clientId, @carId, @startTime)";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@clientId", clientId);
                    command.Parameters.AddWithValue("@carId", carId);
                    command.Parameters.AddWithValue("@startTime", DateTime.Now);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void EndRental(int rentalId, double kilometersDriven)
        {
            using (var connection = Database.GetConnection())
            {
                connection.Open();

                var rentalQuery = "SELECT Rentals.CarID, Rentals.StartTime, Cars.HourlyRate, Cars.PerKmRate " +
                                  "FROM Rentals JOIN Cars ON Rentals.CarID = Cars.ID WHERE Rentals.ID = @rentalId";

                using (var command = new SQLiteCommand(rentalQuery, connection))
                {
                    command.Parameters.AddWithValue("@rentalId", rentalId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var carId = reader.GetInt32(0);
                            var startTime = reader.GetDateTime(1);
                            var hourlyRate = reader.GetDecimal(2);
                            var perKmRate = reader.GetDecimal(3);

                            var endTime = DateTime.Now;
                            var rentalDuration = (endTime - startTime).TotalHours;
                            var totalAmount = (decimal)rentalDuration * hourlyRate + (decimal)kilometersDriven * perKmRate;

                            var updateQuery = "UPDATE Rentals SET EndTime = @endTime, KilometersDriven = @kilometersDriven, TotalAmount = @totalAmount WHERE ID = @rentalId";

                            using (var updateCommand = new SQLiteCommand(updateQuery, connection))
                            {
                                updateCommand.Parameters.AddWithValue("@endTime", endTime);
                                updateCommand.Parameters.AddWithValue("@kilometersDriven", kilometersDriven);
                                updateCommand.Parameters.AddWithValue("@totalAmount", totalAmount);
                                updateCommand.Parameters.AddWithValue("@rentalId", rentalId);
                                updateCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
        }
    }
}
