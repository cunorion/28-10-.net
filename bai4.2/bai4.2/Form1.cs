

using MySqlConnector;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace bai4._2
{
    public partial class Form1 : Form
    {
        // Ket noi den MySQL Server
        private string connectionString =
            "Server=localhost;Port=3306;Database=bai4.2;Uid=root;Password=;";

        private TextBox txtHoTen;
        private TextBox txtEmail;
        private TextBox txtSoDienThoai;
        private ComboBox cboKhoaHoc;
        private Button btnDangKy;
        private Label lblKetNoi;
        private ErrorProvider errorProvider1;

        public Form1()
        {
            InitializeComponent();
            TaoGiaoDien();
            this.Load += Form1_Load;
        }

        // Tao giao dien bang code
        private void TaoGiaoDien()
        {
            this.Text = "Form dang ky hoc vien";
            this.Size = new Size(540, 480);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.WhiteSmoke;

            errorProvider1 = new ErrorProvider();
            errorProvider1.ContainerControl = this;

            Label lblTieuDe = new Label();
            lblTieuDe.Text = "DANG KY HOC VIEN";
            lblTieuDe.Font = new Font("Arial", 17, FontStyle.Bold);
            lblTieuDe.ForeColor = Color.DarkBlue;
            lblTieuDe.TextAlign = ContentAlignment.MiddleCenter;
            lblTieuDe.SetBounds(30, 20, 460, 40);
            this.Controls.Add(lblTieuDe);

            TaoNhan("Ho va ten:", 50, 90);
            txtHoTen = TaoTextBox(190, 88);

            TaoNhan("Email:", 50, 140);
            txtEmail = TaoTextBox(190, 138);

            TaoNhan("So dien thoai:", 50, 190);
            txtSoDienThoai = TaoTextBox(190, 188);

            TaoNhan("Khoa hoc:", 50, 240);

            cboKhoaHoc = new ComboBox();
            cboKhoaHoc.SetBounds(190, 238, 270, 30);
            cboKhoaHoc.Font = new Font("Arial", 10);
            cboKhoaHoc.DropDownStyle = ComboBoxStyle.DropDownList;
            cboKhoaHoc.Items.AddRange(new string[]
            {
                "Lap trinh C#",
                "Lap trinh Java",
                "Lap trinh Web",
                "Co so du lieu",
                "Thiet ke do hoa"
            });
            this.Controls.Add(cboKhoaHoc);

            btnDangKy = new Button();
            btnDangKy.Text = "DANG KY";
            btnDangKy.SetBounds(190, 295, 150, 40);
            btnDangKy.Font = new Font("Arial", 10, FontStyle.Bold);
            btnDangKy.BackColor = Color.RoyalBlue;
            btnDangKy.ForeColor = Color.White;
            btnDangKy.FlatStyle = FlatStyle.Flat;
            btnDangKy.Click += btnDangKy_Click;
            this.Controls.Add(btnDangKy);

            lblKetNoi = new Label();
            lblKetNoi.Text = "Dang kiem tra ket noi...";
            lblKetNoi.SetBounds(30, 365, 460, 30);
            lblKetNoi.Font = new Font("Arial", 10, FontStyle.Bold);
            lblKetNoi.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblKetNoi);
        }

        // Tao nhan
        private void TaoNhan(string noiDung, int x, int y)
        {
            Label lbl = new Label();
            lbl.Text = noiDung;
            lbl.SetBounds(x, y, 130, 30);
            lbl.Font = new Font("Arial", 10);
            lbl.TextAlign = ContentAlignment.MiddleLeft;
            this.Controls.Add(lbl);
        }

        // Tao o nhap du lieu
        private TextBox TaoTextBox(int x, int y)
        {
            TextBox txt = new TextBox();
            txt.SetBounds(x, y, 270, 30);
            txt.Font = new Font("Arial", 10);
            this.Controls.Add(txt);
            return txt;
        }

        // Kiem tra ket noi khi mo form
        private void Form1_Load(object sender, EventArgs e)
        {
            KiemTraKetNoi();
        }

        private void KiemTraKetNoi()
        {
            try
            {
                using (MySqlConnection conn =
                    new MySqlConnection(connectionString))
                {
                    conn.Open();
                }

                lblKetNoi.Text = "Ket noi MySQL thanh cong!";
                lblKetNoi.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                lblKetNoi.Text = "Ket noi MySQL that bai!";
                lblKetNoi.ForeColor = Color.Red;

                MessageBox.Show(
                    "Khong ket noi duoc MySQL.\n\n" + ex.Message,
                    "Loi ket noi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Xu ly nut dang ky
        private void btnDangKy_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            string hoTen = txtHoTen.Text.Trim();
            string email = txtEmail.Text.Trim();
            string sdt = txtSoDienThoai.Text.Trim();

            bool hopLe = true;

            // Kiem tra ho ten
            if (string.IsNullOrWhiteSpace(hoTen))
            {
                errorProvider1.SetError(
                    txtHoTen, "Vui long nhap ho ten!");
                hopLe = false;
            }

            // Kiem tra email
            if (string.IsNullOrWhiteSpace(email))
            {
                errorProvider1.SetError(
                    txtEmail, "Vui long nhap email!");
                hopLe = false;
            }
            else if (!Regex.IsMatch(
                email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                errorProvider1.SetError(
                    txtEmail, "Email khong dung dinh dang!");
                hopLe = false;
            }

            // Kiem tra so dien thoai
            if (string.IsNullOrWhiteSpace(sdt))
            {
                errorProvider1.SetError(
                    txtSoDienThoai, "Vui long nhap so dien thoai!");
                hopLe = false;
            }
            else if (!Regex.IsMatch(sdt, @"^[0-9]{10,11}$"))
            {
                errorProvider1.SetError(
                    txtSoDienThoai,
                    "So dien thoai phai co 10-11 chu so!");
                hopLe = false;
            }

            // Kiem tra khoa hoc
            if (cboKhoaHoc.SelectedIndex == -1)
            {
                errorProvider1.SetError(
                    cboKhoaHoc, "Vui long chon khoa hoc!");
                hopLe = false;
            }

            if (!hopLe)
            {
                MessageBox.Show(
                    "Vui long kiem tra thong tin!",
                    "Thong bao",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Luu thong tin hoc vien vao MySQL
            try
            {
                using (MySqlConnection conn =
                    new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string sql = @"INSERT INTO hoc_vien
                        (ho_ten, email, so_dien_thoai, khoa_hoc)
                        VALUES
                        (@hoTen, @email, @sdt, @khoaHoc)";

                    using (MySqlCommand cmd =
                        new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@hoTen", hoTen);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@sdt", sdt);
                        cmd.Parameters.AddWithValue(
                            "@khoaHoc", cboKhoaHoc.Text);

                        int ketQua = cmd.ExecuteNonQuery();

                        if (ketQua > 0)
                        {
                            MessageBox.Show(
                                "Dang ky hoc vien thanh cong!\n" +
                                "Du lieu da duoc luu vao MySQL.",
                                "Thanh cong",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                            txtHoTen.Clear();
                            txtEmail.Clear();
                            txtSoDienThoai.Clear();
                            cboKhoaHoc.SelectedIndex = -1;
                            txtHoTen.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Khong luu duoc du lieu!\n\n" + ex.Message,
                    "Loi MySQL",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
