using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace BaiTapLonMonLapTrinhNangCao
{
    public partial class frmDangNhap : Form
    {
        public frmDangNhap()
        {
            InitializeComponent();
        }

        SqlConnection connection = null;
        string chuoiketnoi = "server = DESKTOP-FFGALN9;database = QLCH;user id = vuongquat;pwd = 123456";
        
        private void OpenConnection()
        {
            if (connection == null)
                connection = new SqlConnection(chuoiketnoi);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            if (txtTaiKhoan.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập tài khoản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTaiKhoan.Focus();
            }
            else if (txtMatKhau.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập mật khẩu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMatKhau.Focus();
            }
            else
            {
                if (KiemTraTaiKhoanTonTai(txtTaiKhoan.Text, txtMatKhau.Text))
                {
                    PhanQuyenDangNhap(txtTaiKhoan.Text);
                    if (level == 1)
                    {
                        frmMainNhanVien frmMainNhanVien = new frmMainNhanVien(txtTaiKhoan.Text);
                        frmMainNhanVien.Show();
                        this.Hide();
                    }
                    else if (level == 2)
                    {
                        frmMainQuanLy frmMainQuanLy = new frmMainQuanLy();
                        frmMainQuanLy.Show();
                        this.Hide();
                    }
                }
                else
                {
                    MessageBox.Show("Tài khoản hoặc mật khẩu không đúng, vui lòng nhập lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
    }
    int level ;
        private void PhanQuyenDangNhap(string taiKhoan)
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand("select Level from TaiKhoanNhanVien where TaiKhoan = @taiKhoan");
                command.Parameters.Add("@taiKhoan", SqlDbType.NVarChar).Value = taiKhoan;
                command.Connection = connection;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    level = reader.GetInt32(0);
                }
                reader.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool KiemTraTaiKhoanTonTai(string taiKhoan, string matKhau)
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "select TaiKhoan,MatKhau from TaiKhoanNhanVien where TaiKhoan = @taiKhoan and MatKhau = @matKhau";
                command.Connection = connection;
                command.Parameters.Add("@taiKhoan", SqlDbType.NVarChar).Value = taiKhoan;
                command.Parameters.Add("@matKhau", SqlDbType.NVarChar).Value = matKhau;
                SqlDataReader reader = command.ExecuteReader();
                bool kq = reader.Read();
                reader.Close();
                return kq;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }
    }
}
