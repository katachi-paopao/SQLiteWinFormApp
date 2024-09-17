using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQLiteWinFormApp
{
    public partial class Form1 : Form
    {
        private string connectionString = "Data Source=database.db;Version=3;";

        public Form1()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Customers";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // テキストボックスのバリデーション
            if (string.IsNullOrWhiteSpace(FirstNameText.Text) ||
                string.IsNullOrWhiteSpace(LastNameText.Text) ||
                string.IsNullOrWhiteSpace(EmailText.Text))
            {
                MessageBox.Show("すべてのフィールドを入力してください。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Customers (FirstName, LastName, Email) VALUES (@FirstName, @LastName, @Email)";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", FirstNameText.Text);
                    command.Parameters.AddWithValue("@LastName", LastNameText.Text);
                    command.Parameters.AddWithValue("@Email", EmailText.Text);
                    command.ExecuteNonQuery();
                }
            }
            LoadData();  // データをリロード

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                // メッセージボックスでエラーメッセージを表示
                MessageBox.Show("行が選択されていません。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // テキストボックスのバリデーション
            if (string.IsNullOrWhiteSpace(FirstNameText.Text) ||
                string.IsNullOrWhiteSpace(LastNameText.Text) ||
                string.IsNullOrWhiteSpace(EmailText.Text))
            {
                MessageBox.Show("すべてのフィールドを入力してください。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int customerId = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
            if (customerId > 0)
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Customers SET FirstName = @FirstName, LastName = @LastName, Email = @Email WHERE CustomerID = @CustomerID";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", FirstNameText.Text);
                        command.Parameters.AddWithValue("@LastName", LastNameText.Text);
                        command.Parameters.AddWithValue("@Email", EmailText.Text);
                        command.Parameters.AddWithValue("@CustomerID", customerId);
                        command.ExecuteNonQuery();
                    }
                }
                LoadData();  // データをリロード
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                // メッセージボックスでエラーメッセージを表示
                MessageBox.Show("行が選択されていません。", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            int customerId = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
            if (customerId > 0)
            {

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Customers WHERE CustomerID = @CustomerID";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerID", customerId);
                        command.ExecuteNonQuery();
                    }
                }
                LoadData();  // データをリロード
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // クリックされた行のインデックスが有効か確認
            if (e.RowIndex >= 0)
            {
                // クリックされた行を取得
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // 各TextBoxに、対応するセルの値を表示
                FirstNameText.Text = row.Cells["FirstName"].Value.ToString();  // FirstName列の値
                LastNameText.Text = row.Cells["LastName"].Value.ToString();    // LastName列の値
                EmailText.Text = row.Cells["Email"].Value.ToString();          // Email列の値
            }
        }
    }
}
