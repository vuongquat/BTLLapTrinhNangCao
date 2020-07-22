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
    public partial class frmTaiKhoanNV : Form
    {
        public frmTaiKhoanNV()
        {
            InitializeComponent();
        }
        string chuoiketnoi = "server = DESKTOP-FFGALN9;database = QLCH;user id = vuongquat;pwd = 123456";
        SqlConnection connection = null;

        private void frmTaiKhoanNV_Load(object sender, EventArgs e)
        {
            HienThiMaNhanVien();
            HienThiDanhSachTaiKhoan();
        }
        private void OpenConnection()
        {
            if (connection == null)
                connection = new SqlConnection(chuoiketnoi);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
        }
        private void HienThiMaNhanVien()
        {

            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "select * from NhanVien";
                command.Connection = connection;

                SqlDataReader reader = command.ExecuteReader();
                cboMaNV.Items.Clear();
                while (reader.Read())
                {
                    cboMaNV.Items.Add(reader.GetString(0) + "-" + reader.GetString(1));
                }
                reader.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo");
            }
        }

        private void HienThiDanhSachTaiKhoan()
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "select * from TaiKhoanNhanVien where Level = 1";
                command.Connection = connection;

                SqlDataReader reader = command.ExecuteReader();
                lvTaiKhoan.Items.Clear();
                while (reader.Read())
                {
                    ListViewItem lvi = new ListViewItem(reader.GetInt32(0) + "");
                    lvi.SubItems.Add(reader.GetString(1));
                    lvi.SubItems.Add(reader.GetString(2));
                    lvi.SubItems.Add(reader.GetString(3));
                    lvTaiKhoan.Items.Add(lvi);
                    lvi.Tag = reader.GetInt32(0);

                }
                reader.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi");
            }
        }

        private void btnTaoTaiKhoan_Click(object sender, EventArgs e)
        {
            if(txtTaiKhoan.Text == "" || txtMatKhau.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }    
            else if(cboMaNV.SelectedIndex == -1)
            {
               MessageBox.Show("Bạn chưa chọn nhân viên","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }    
            else
            {
                if(KiemTraTaiKhoanTonTai(txtTaiKhoan.Text))
                {
                    MessageBox.Show("Tài khoản đã tồn tại, vui lòng nhập tài khoản khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }    
                else
                {
                    try
                    {
                        OpenConnection();
                        SqlCommand command = new SqlCommand();
                        command.CommandType = CommandType.Text;
                        command.CommandText = "insert into TaiKhoanNhanVien (MaNhanVien,TaiKhoan,MatKhau,Level)" +
                            "values(@maNV,@taiKhoan,@matKhau,1)";
                        command.Connection = connection;

                        command.Parameters.Add("@maNV", SqlDbType.NVarChar).Value = maNV;
                        command.Parameters.Add("@taiKhoan", SqlDbType.NVarChar).Value = txtTaiKhoan.Text;
                        command.Parameters.Add("@matKhau", SqlDbType.NVarChar).Value = txtMatKhau.Text;

                        int kq = command.ExecuteNonQuery();
                        if (kq > 0)
                        {
                            MessageBox.Show("Tạo tài khoản thành công");
                            HienThiDanhSachTaiKhoan();
                        }
                        else
                        {
                            MessageBox.Show("Lỗi");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }    
            }    
            
        }

        
       
        string maNV = "";
        
        private void cboMaNV_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaNV.SelectedIndex == -1)
                return;
             string line = cboMaNV.SelectedItem + "";
            string[] arr = line.Split('-');
            maNV = arr[0];
            
           
        }

        private void btnTroVe_Click(object sender, EventArgs e)
        {
            Close();
        }

        int id_TaiKhoan;
        
        private void lvTaiKhoan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvTaiKhoan.SelectedItems.Count == 0)
                return;
                
            ListViewItem lvi = lvTaiKhoan.SelectedItems[0];
            maNV = lvi.SubItems[1].Text; 
            id_TaiKhoan = int.Parse(lvi.Tag+"");
            LayTenNhanVien();
            HienThiChiTietTaiKhoan(id_TaiKhoan);
            cboMaNV.Enabled = false;
            txtTaiKhoan.Enabled = false;
            
            
        }

        private void HienThiChiTietTaiKhoan(int id_TaiKhoan)
        {
           
                OpenConnection();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "select * from TaiKhoanNhanVien where id_TaiKhoan = @id_taiKhoan";
                command.Connection = connection;
                command.Parameters.Add("@id_taiKhoan", SqlDbType.Int).Value = id_TaiKhoan;

                SqlDataReader reader = command.ExecuteReader();
            
            while (reader.Read())
                {
                    
                    cboMaNV.Text = reader.GetString(1)+"-"+TenNV;
                    txtTaiKhoan.Text = reader.GetString(2);
                    txtMatKhau.Text = reader.GetString(3);
                }    
                reader.Close();
            
        }
        string TenNV = "";
        private void LayTenNhanVien()
        {
            try {
                OpenConnection();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "select * from NhanVien where MaNhanVien = @maNV";
                command.Connection = connection;
                command.Parameters.Add("@maNV", SqlDbType.NVarChar).Value = maNV;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    TenNV = reader.GetString(1);
                }
                reader.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo");
            }
            
            
            
        }

        private void btnXoaTK_Click(object sender, EventArgs e)
        {
            if(lvTaiKhoan.SelectedItems.Count == 0)
            {
                MessageBox.Show("Bạn chưa chọn nội dung để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }    
            else
            {
                DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa chứ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if(result == DialogResult.Yes)
                {
                    try
                    {
                        OpenConnection();
                        SqlCommand command = new SqlCommand();
                        command.CommandType = CommandType.Text;
                        command.CommandText = "Delete from TaiKhoanNhanVien where id_TaiKhoan = @id_taiKhoan";
                        command.Connection = connection;
                        command.Parameters.Add("@id_taiKhoan", SqlDbType.Int).Value = id_TaiKhoan;
                        int kq = command.ExecuteNonQuery();
                        if (kq > 0)
                        {
                            MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            HienThiDanhSachTaiKhoan();
                        }
                        else
                        {
                            MessageBox.Show("Xóa thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }    
               
            }    
        }

        private void btnDoiMatKhau_Click(object sender, EventArgs e)
        {
            if(lvTaiKhoan.SelectedItems.Count==0)
            {
                MessageBox.Show("Bạn chưa chọn nội dung!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }    
            else
            {
                try
                {
                    OpenConnection();
                    SqlCommand command = new SqlCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "update TaiKhoanNhanVien set MatKhau = @matKhau where id_TaiKhoan = @id_taiKhoan";
                    command.Connection = connection;
                    command.Parameters.Add("@matKhau", SqlDbType.NVarChar).Value = txtMatKhau.Text;
                    command.Parameters.Add("id_taiKhoan", SqlDbType.Int).Value = id_TaiKhoan;
                    int kq = command.ExecuteNonQuery();
                    if(kq>0)
                    {
                        MessageBox.Show("Bạn đã đổi mật khẩu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        HienThiDanhSachTaiKhoan();
                    }    
                    else
                    {
                        MessageBox.Show("Bạn đã đổi mật thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }    
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }    
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTaiKhoan.Text = "";
            txtMatKhau.Text = "";
            cboMaNV.Items.Clear();
            HienThiMaNhanVien();
            cboMaNV.Enabled = true;
            txtTaiKhoan.Enabled = true;
            txtMatKhau.Enabled = true;
        }

        private bool KiemTraTaiKhoanTonTai(string taiKhoan)
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "select * from TaiKhoanNhanVien where TaiKhoan = @taiKhoan";
                command.Connection = connection;
                command.Parameters.Add("@taiKhoan", SqlDbType.NVarChar).Value = taiKhoan;
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
