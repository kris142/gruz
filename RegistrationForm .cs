using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gruz
{
    public partial class RegistrationForm : Form
    {
        public RegistrationForm()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Пожалуйста, введите имя пользователя и пароль.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RegisterUser(username, password);
        }

        private void RegisterUser(string username, string password)
        {
            string filePath = "users.txt";
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
            if (IsUserRegistered(username))
            {
                MessageBox.Show("Пользователь с таким именем уже зарегистрирован.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            using (StreamWriter writer = File.AppendText(filePath))
            {
                writer.WriteLine($"{username},{password}");
            }

            MessageBox.Show("Регистрация успешна.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Main main = new Main();
            this.Close();
            main.Show();
        }

        private bool IsUserRegistered(string username)
        {
            string filePath = "users.txt";
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    string[] user = line.Split(',');
                    if (user.Length == 2 && user[0] == username)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Main main = new Main();
            this.Close();
            main.Show();
        }
    }
}
