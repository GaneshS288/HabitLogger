using Microsoft.Data.Sqlite;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;


class Program
{
    static string connectionString = @"Data Source=habit-tracker.db";
    static void Main(string[] args)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS drinking_water (
                             Id INTEGER PRIMARY KEY AUTOINCREMENT,
                             Date TEXT,
                             Quantity INTEGER
                             )";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }

        static void GetUserInput()
        {
            Console.Clear();
            bool closeApp = false;

            while (closeApp == false)
            {
                Console.WriteLine("MAIN MENU");
                Console.WriteLine("\nWhat would you Like to do?");
                Console.WriteLine("\ntype 0 to close the app");
                Console.WriteLine("Type 1 to View All Records");
                Console.WriteLine("Type 2 to Insert Record");
                Console.WriteLine("Type 3 to Delete Record");
                Console.WriteLine("Type 4 to Update Record");
                Console.WriteLine("_______________________________________________________\n");

                string? command = Console.ReadLine()?.Trim();

                switch (command)
                {
                    case "0":
                        Console.WriteLine("Bye");
                        closeApp = true;
                        break;
                    case "1":
                        ViewAllRecords();
                        break;
                    case "2":
                        InsertRecord();
                        break;
                    /*case "3":
                        DeleteRecord();
                        break;
                    case "4":
                        UpdateRecord();
                        break;*/
                    default:
                        Console.WriteLine("Invalid input, please enter a valid value. Press Enter to continue");
                        Console.ReadLine();
                        break;
                }
            }
        }

        static void ViewAllRecords()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = @"SELECT * FROM drinking_water";

                List<DrinkingWater> tableData = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                            new DrinkingWater
                            {
                                Id = reader.GetInt32(0),
                                Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", new CultureInfo("en-US")),
                                Quantity = reader.GetInt32(2)
                            });
                    }
                }

                else
                    Console.WriteLine("No rows found!");

                connection.Close();

                Console.WriteLine();
                foreach (var dw in tableData)
                {
                    Console.WriteLine($"{dw.Id}, {dw.Date.ToString("dd-MM-yyyy")}, {dw.Quantity}");
                }
                Console.WriteLine("_________________________________________________");
            }
        }
        static void InsertRecord()
        {
            string date = GetDateInput();
            int quantity = GetQuantity();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"INSERT INTO drinking_water(Date, Quantity) VALUES('{date}', {quantity})";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        static string? GetDateInput()
        {
            string? input = "";

            while (input == "")
            {
                Console.WriteLine("Please enter the date int this format \"dd-mm-yyyy\".");
                input = Console.ReadLine()?.Trim();
            }
            return input;
        }

        static int GetQuantity()
        {
            string? input = "";

            while (input == "")
            {
                Console.WriteLine("Enter how many glasses you drank");
                input = Console.ReadLine()?.Trim();
            }

            int quantity = ValidateNumber(input);
            return quantity;
        }

        static int ValidateNumber(string? input)
        {
            bool isNum = false;
            int cleanNum;

            isNum = int.TryParse(input, out cleanNum);

            if (isNum == false)
                return 0;

            else
                return cleanNum;
        }

        GetUserInput();

    }

    public class DrinkingWater
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int Quantity { get; set; }
    }
}

