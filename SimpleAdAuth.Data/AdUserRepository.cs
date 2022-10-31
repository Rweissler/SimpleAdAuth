using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace SimpleAdAuth.Data
{
    public class AdUserRepository
    {
        private string _connectionString { get; set; }
        public AdUserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddUser(User user,string password)
        {
            using var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO User(@name, @email, @hash)";
            cmd.Parameters.AddWithValue("@name", user.Name);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@hash", BCrypt.Net.BCrypt.HashPassword(password));
            connection.Open();
            cmd.ExecuteNonQuery();                             
        }

        public User Login(string email, string password)
        {
            var user = GetByEmail(email);
            if(user == null)
            {
                return null;
            }

            bool Isvalid = BCrypt.Net.BCrypt.Verify(password, User.PaswordHash);
            return Isvalid ? user : null;
        }

        public User GetByEmail(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT TOP 1 FROM Users WHERE email = @email";
            cmd.Parameters.AddWithValue("@email", email);
            connection.Open();
            var reader = cmd.ExecuteReader();
            if(!reader.Read())
            {
                return null;
            }

            return new User
            {
                Id = (int)reader["Id"],
                Name = (string)reader["Name"],
                Email = (string)reader["Email"],
                PasswordHash = (string)reader["PasswordHash"]
               
            };
        }

        public void AddAd(Ad ad)
        {

            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Ad VALUES(@date, @phoneNumber, @description, @userId)";
            cmd.Parameters.AddWithValue("@date", DateTime.Now);
            cmd.Parameters.AddWithValue("@phoneNumber", ad.PhoneNumber);
            cmd.Parameters.AddWithValue("@description", ad.Description);
            cmd.Parameters.AddWithValue("@userId", ad.UserId);
            connection.Open();
            cmd.ExecuteNonQuery();
        }

        public void DeleteAd(Ad ad)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE * FROM Ad WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", ad.Id);              
            connection.Open();
            cmd.ExecuteNonQuery();
        }
        public List<Ad> GetAds()
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT a.*, u.Name FROM Ad a JOIN Users u On a.UserId = u.Id ";
            connection.Open();
            var reader = cmd.ExecuteReader();
            var ads = new List<Ad>();
            while (reader.Read())
            {
                ads.Add(new Ad
                {
                    Id = (int)reader["ID"],
                    Name = (string)reader["Name"],
                    Date = (DateTime)reader["Date"],
                    PhoneNumber = (string)reader["Number"],
                    Description = (string)reader["Description"],
                    UserId = (int)reader["UserID"]

                });
            }
            return ads;
        }

        public List<Ad> GetAdsByUser(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT a*, u.Name FROM Ad 
                                JOIN User u
                                On u.id = a.UserId
                                WHERE u.Email = @email ";
            cmd.Parameters.AddWithValue("@email", email); var reader = cmd.ExecuteReader();
            var ads = new List<Ad>();
            while (reader.Read())
            {
                ads.Add(new Ad
                {
                    Id = (int)reader["ID"],
                    Name = (string)reader["Name"],
                    Date = (DateTime)reader["Date"],
                    PhoneNumber = (string)reader["Number"],
                    Description = (string)reader["Description"],
                    UserId = (int)reader["UserID"]

                });
            }
            return ads;
        }
    }
} 
