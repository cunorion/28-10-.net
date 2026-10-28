
using System;
using System.Drawing;
using System.Windows.Forms;
using MySqlConnector;

namespace bai4._1
{
    public partial class Form1 : Form
    {
        TextBox txtDisplay;

        double number1 = 0;
        string operation = "";
        bool newNumber = true;

        // [THÊM MỚI] Chuỗi kết nối MySQL, mật khẩu để trống
        string connectionString =
      "Server=localhost;" +
      "Port=3306;" +
      "Database=bai4.1;" +
      "Uid=root;" +
      "Password=;";

        public Form1()
        {
            InitializeComponent();
            DesignForm();

            // [THÊM MỚI] Kiểm tra kết nối khi mở chương trình
            TestConnection();
        }

        private void DesignForm()
        {
            this.Text = "Calculator Responsive";
            this.Size = new Size(350, 450);
            this.MinimumSize = new Size(300, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            TableLayoutPanel mainPanel = new TableLayoutPanel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Padding = new Padding(10);
            mainPanel.ColumnCount = 1;
            mainPanel.RowCount = 2;

            mainPanel.RowStyles.Add(
                new RowStyle(SizeType.Percent, 20));

            mainPanel.RowStyles.Add(
                new RowStyle(SizeType.Percent, 80));

            this.Controls.Add(mainPanel);

            txtDisplay = new TextBox();
            txtDisplay.Text = "0";
            txtDisplay.ReadOnly = true;
            txtDisplay.Dock = DockStyle.Fill;
            txtDisplay.Font = new Font("Arial", 22);
            txtDisplay.TextAlign = HorizontalAlignment.Right;

            mainPanel.Controls.Add(txtDisplay, 0, 0);

            TableLayoutPanel buttonPanel = new TableLayoutPanel();
            buttonPanel.Dock = DockStyle.Fill;
            buttonPanel.RowCount = 4;
            buttonPanel.ColumnCount = 4;

            for (int i = 0; i < 4; i++)
            {
                buttonPanel.ColumnStyles.Add(
                    new ColumnStyle(SizeType.Percent, 25));

                buttonPanel.RowStyles.Add(
                    new RowStyle(SizeType.Percent, 25));
            }

            mainPanel.Controls.Add(buttonPanel, 0, 1);

            string[,] buttons =
            {
                { "7", "8", "9", "/" },
                { "4", "5", "6", "*" },
                { "1", "2", "3", "-" },
                { "C", "0", "=", "+" }
            };

            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    Button btn = new Button();

                    btn.Text = buttons[row, col];
                    btn.Dock = DockStyle.Fill;
                    btn.Margin = new Padding(3);
                    btn.Font = new Font("Arial", 16);

                    // Event Sharing cho cac nut so
                    if (btn.Text.Length == 1 &&
                        btn.Text[0] >= '0' &&
                        btn.Text[0] <= '9')
                    {
                        btn.Click += btnNum_Click;
                    }
                    else
                    {
                        btn.Click += btnOperation_Click;
                    }

                    buttonPanel.Controls.Add(btn, col, row);
                }
            }
        }

        // Dung chung su kien cho cac nut so
        private void btnNum_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (newNumber || txtDisplay.Text == "0")
            {
                txtDisplay.Text = btn.Text;
                newNumber = false;
            }
            else
            {
                txtDisplay.Text += btn.Text;
            }
        }

        // Xu ly cac nut phep tinh
        private void btnOperation_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (btn.Text == "C")
            {
                txtDisplay.Text = "0";
                number1 = 0;
                operation = "";
                newNumber = true;
                return;
            }

            if (btn.Text == "=")
            {
                if (operation == "")
                    return;

                double number2 = double.Parse(txtDisplay.Text);
                double result = 0;

                switch (operation)
                {
                    case "+":
                        result = number1 + number2;
                        break;

                    case "-":
                        result = number1 - number2;
                        break;

                    case "*":
                        result = number1 * number2;
                        break;

                    case "/":
                        if (number2 == 0)
                        {
                            MessageBox.Show("Khong the chia cho 0!");
                            txtDisplay.Text = "0";
                            number1 = 0;
                            operation = "";
                            newNumber = true;
                            return;
                        }

                        result = number1 / number2;
                        break;
                }

                // [THÊM MỚI] Lưu phép tính vào MySQL
                SaveHistory(number1, operation, number2, result);

                txtDisplay.Text = result.ToString();
                operation = "";
                newNumber = true;
            }
            else
            {
                number1 = double.Parse(txtDisplay.Text);
                operation = btn.Text;
                newNumber = true;
            }
        }

        // [THÊM MỚI] Kiểm tra kết nối MySQL
        private void TestConnection()
        {
            try
            {
                using (MySqlConnection conn =
                       new MySqlConnection(connectionString))
                {
                    conn.Open();

                    MessageBox.Show(
                        "Ket noi MySQL thanh cong!",
                        "Thong bao",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ket noi MySQL that bai!\n" + ex.Message,
                    "Loi ket noi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // [THÊM MỚI] Lưu lịch sử phép tính vào database
        private void SaveHistory(
            double so1,
            string phepTinh,
            double so2,
            double ketQua)
        {
            string sql =
                "INSERT INTO lich_su " +
                "(so_thu_nhat, phep_tinh, so_thu_hai, ket_qua) " +
                "VALUES (@so1, @phepTinh, @so2, @ketQua)";

            try
            {
                using (MySqlConnection conn =
                       new MySqlConnection(connectionString))
                {
                    conn.Open();

                    using (MySqlCommand cmd =
                           new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@so1", so1);
                        cmd.Parameters.AddWithValue("@phepTinh", phepTinh);
                        cmd.Parameters.AddWithValue("@so2", so2);
                        cmd.Parameters.AddWithValue("@ketQua", ketQua);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Khong the luu lich su!\n" + ex.Message,
                    "Loi luu du lieu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
