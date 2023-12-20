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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Пожалуйста, введите имя пользователя и пароль.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (username == "admin" && password == "admin")
            {
                MessageBox.Show("Вход успешен.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                A.access = "Admin";
                Main main = new Main();
                this.Close();
                main.Show();
            }
            else if (Login(username, password))
            {
                MessageBox.Show("Вход успешен.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                A.access = "User";
                Main main = new Main();
                this.Close();
                main.Show();
            }
            else
            {
                MessageBox.Show("Неверные учетные данные.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool Login(string username, string password)
        {
            string filePath = "users.txt";
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    string[] user = line.Split(',');
                    if (user.Length == 2 && user[0] == username && user[1] == password)
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
