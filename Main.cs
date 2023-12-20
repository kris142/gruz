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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace gruz
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            listView.Items.Clear();

            string filePath = "cargo.txt";
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] cargoData = line.Split(',');

                        if (cargoData.Length == 4)
                        {
                            ListViewItem item = new ListViewItem(cargoData);
                            listView.Items.Add(item);
                        }
                        else
                        {
                            MessageBox.Show("Некорректные данные в файле.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Файл не существует.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if(A.access == "admin")
            {
                Add.Visible = true;
                Del.Visible = true;
                btnEditCargo.Visible = true;
                btnUpdateCargo.Visible=true;
            }
            else if(A.access == "User")
            {
                Add.Visible = true;
                Del.Visible = false;
                btnEditCargo.Visible = true;
                btnUpdateCargo.Visible = true;
            }
            else if(A.access == "none")
            {
                Add.Visible = false;
                Del.Visible = false;
                btnEditCargo.Visible = false;
                btnUpdateCargo.Visible = false;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            string quantity = txtQuantity.Text;
            string weight = txtWeight.Text;
            if (cboType.SelectedItem == null || string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(quantity) || string.IsNullOrWhiteSpace(weight))
            {
                MessageBox.Show("Пожалуйста, введите все данные.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string type = cboType.SelectedItem.ToString();
            ListViewItem item = new ListViewItem(new[] { name, quantity, weight, type });
            listView.Items.Add(item);
            txtName.Clear();
            txtQuantity.Clear();
            txtWeight.Clear();
            cboType.SelectedIndex = -1;
            AddCargoToFile(name, quantity, weight, type);
        }
        private void AddCargoToFile(string name, string quantity, string weight, string type)
        {
            string filePath = "cargo.txt";
            using (StreamWriter writer = File.AppendText(filePath))
            {
                writer.WriteLine($"{name},{quantity},{weight},{type}");
            }
        }

        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView.SelectedItems[0];
                string name = selectedItem.SubItems[0].Text;
                string quantity = selectedItem.SubItems[1].Text;
                string weight = selectedItem.SubItems[2].Text;
                string type = selectedItem.SubItems[3].Text;
                listView.Items.Remove(selectedItem);

                RemoveCargoFromFile(name, quantity, weight, type);
            }
            else
            {
                MessageBox.Show("Выберите элемент для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void RemoveCargoFromFile(string name, string quantity, string weight, string type)
        {
            string filePath = "cargo.txt";
            string tempFilePath = "tempCargo.txt";
            using (StreamReader reader = new StreamReader(filePath))
            {
                using (StreamWriter writer = new StreamWriter(tempFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (!line.Contains($"{name},{quantity},{weight},{type}"))
                        {
                            writer.WriteLine(line);
                        }
                    }
                }
            }
            File.Delete(filePath);
            File.Move(tempFilePath, filePath);
        }

        private void входToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            this.Hide();
            loginForm.Show();
        }

        private void регистрацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegistrationForm registrationForm = new RegistrationForm();
            this.Hide();
            registrationForm.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView.SelectedItems[0];
                txtName.Text = selectedItem.SubItems[0].Text;
                txtQuantity.Text = selectedItem.SubItems[1].Text;
                txtWeight.Text = selectedItem.SubItems[2].Text;
                cboType.SelectedItem = selectedItem.SubItems[3].Text;
                btnEditCargo.Enabled = false;
                Add.Enabled = false;
                Del.Enabled = false;
                btnUpdateCargo.Enabled = true;
            }
            else
            {
                MessageBox.Show("Выберите элемент для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            string filePath = "cargo.txt";
            string tempFilePath = "tempCargo.txt";
            if (listView.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView.SelectedItems[0];

                selectedItem.SubItems[0].Text = txtName.Text;
                selectedItem.SubItems[1].Text = txtQuantity.Text;
                selectedItem.SubItems[2].Text = txtWeight.Text;
                selectedItem.SubItems[3].Text = cboType.SelectedItem.ToString();

                using (StreamReader reader = new StreamReader(filePath))
                {
                    using (StreamWriter writer = new StreamWriter(tempFilePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (line != $"{selectedItem.SubItems[0].Text},{selectedItem.SubItems[1].Text},{selectedItem.SubItems[2].Text},{selectedItem.SubItems[3].Text}")
                            {
                                writer.WriteLine(line);
                            }
                            else
                            {
                                writer.WriteLine($"{txtName.Text},{txtQuantity.Text},{txtWeight.Text},{cboType.SelectedItem}");
                            }
                        }
                    }
                }
                File.Delete(filePath);
                File.Move(tempFilePath, filePath);

                txtName.Clear();
                txtQuantity.Clear();
                txtWeight.Clear();
                cboType.SelectedIndex = -1;

                btnEditCargo.Enabled = true;
                Add.Enabled = true;
                Del.Enabled = true;
                btnUpdateCargo.Enabled = false;
            }
            else
            {
                MessageBox.Show("Выберите элемент для обновления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
