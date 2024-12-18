﻿using System;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace CinemaTicketSystem
{
    public partial class LoginForm : Form
    {
        private static readonly string DatabasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CinemaTicket_db.db");
        private static readonly string ConnectionString = $"Data Source={DatabasePath};Version=3;";



        public LoginForm()
        {
            InitializeComponent();
            ResetDatabase(); 
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void ResetDatabase()
        {
            EnsureDatabaseExists();

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string deleteAllUsersQuery = "DELETE FROM Users";
                using (var deleteCommand = new SQLiteCommand(deleteAllUsersQuery, connection))
                {
                    deleteCommand.ExecuteNonQuery();
                }

                string insertDefaultUserQuery = "INSERT INTO Users (Username, Password) VALUES ('enes', '123')";
                using (var insertCommand = new SQLiteCommand(insertDefaultUserQuery, connection))
                {
                    insertCommand.ExecuteNonQuery();
                }

                //MessageBox.Show("Veritabanı sıfırlandı ve varsayılan kullanıcı eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void EnsureDatabaseExists()
        {
            if (!System.IO.File.Exists(DatabasePath))
            {
                SQLiteConnection.CreateFile(DatabasePath);
            }

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Users (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Username TEXT NOT NULL UNIQUE,
                        Password TEXT NOT NULL,
                        CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                    );";

                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private void registerkytbtn_Click(object sender, EventArgs e)
        {
            string username = registerusername.Text;
            string password = registerpassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Kullanıcı adı ve şifre boş bırakılamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO Users (Username, Password) VALUES (@username, @password)";

                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password); 
                    try
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Kayıt başarıyla oluşturuldu!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        tabControl1.SelectedIndex = 0; // Go back to login page
                    }
                    catch (SQLiteException ex)
                    {
                        MessageBox.Show("Bu kullanıcı adı zaten mevcut.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void registerbtn_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void loginbtn_Click_1(object sender, EventArgs e)
        {
            string username = this.username.Text;
            string password = this.password.Text;

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string selectQuery = "SELECT COUNT(1) FROM Users WHERE Username = @username AND Password = @password";

                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);

                    int userCount = Convert.ToInt32(command.ExecuteScalar());
                    if (userCount > 0)
                    {
                        Form1 form1 = new Form1();
                        form1.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Kullanıcı adı veya şifre yanlış!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Uygulamayı kapatmak istediğinize emin misiniz?", "Çıkış Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }
    }
}
