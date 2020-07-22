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
using System.Net.Http.Headers;

namespace BaiTapLonMonLapTrinhNangCao
{
    public partial class frmDanhMuc : Form
    {
        public frmDanhMuc()
        {
            InitializeComponent();
        }

        //=================================== Chuỗi kết nối =====================================//
        string chuoiketnoi = "server = DESKTOP-FFGALN9;database = QLCH;user id = vuongquat;pwd = 123456";
        SqlConnection connection = null;
        //=================================== Chuỗi kết nối =====================================//

        private void OpenConnection()
        {
            if (connection == null)
                connection = new SqlConnection(chuoiketnoi);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
        }
        public delegate void LayDuLieu();
        public LayDuLieu layDuLieu;

        private void btnDong_Click(object sender, EventArgs e)
        {
            layDuLieu();
            Close();
        }

        

        private void frmDanhMuc_Load(object sender, EventArgs e)
        {
            HienThiDanhMuc();
        }

        private bool KiemTraDanhMuc(string maDM)
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "select * from DanhMuc where MaDanhMuc = @maDM";
                command.Connection = connection;
                command.Parameters.Add("@maDM", SqlDbType.NVarChar).Value = maDM;
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

        private void HienThiDanhMuc()
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "select * from DanhMuc";
                command.Connection = connection;

                lvDanhMuc.Items.Clear();
                SqlDataReader reader = command.ExecuteReader();
                while(reader.Read())
                {
                    ListViewItem lvi = new ListViewItem(reader.GetString(0));
                    lvi.SubItems.Add(reader.GetString(1));
                    lvDanhMuc.Items.Add(lvi);
                    lvi.Tag = reader.GetString(0);
                }
                reader.Close();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLuuDM_Click(object sender, EventArgs e)
        {
            if(txtMaDM.Text ==""|| txtTenDM.Text =="")
            {
                MessageBox.Show("Bạn chưa nhập đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }    
            else
            {
                if(KiemTraDanhMuc(txtMaDM.Text))
                {
                    SuaDanhMuc();
                }    
                else
                {
                    ThemDanhMuc();
                }    
            }    
        }

        private void ThemDanhMuc()
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "insert into DanhMuc(MaDanhMuc,TenDanhMuc) " +
                    "values(@maDM,@tenDM)";
                command.Connection = connection;
                command.Parameters.Add("@maDM", SqlDbType.NVarChar).Value = txtMaDM.Text;
                command.Parameters.Add("@tenDM", SqlDbType.NVarChar).Value = txtTenDM.Text;
                int kq = command.ExecuteNonQuery();
                if(kq>0)
                {
                    MessageBox.Show("Bạn đã thêm thành công 1 danh mục", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HienThiDanhMuc();
                    txtMaDM.Text = "";
                    txtTenDM.Text = "";
                }    
                else
                {
                    MessageBox.Show("Thêm mới 1 danh mục thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }    
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SuaDanhMuc()
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "update DanhMuc set TenDanhMuc = @tenDM where MaDanhMuc = @maDM";
                command.Connection = connection;
                command.Parameters.Add("@tenDM", SqlDbType.NVarChar).Value = txtTenDM.Text;
                command.Parameters.Add("@maDM", SqlDbType.NVarChar).Value = txtMaDM.Text;
                int kq = command.ExecuteNonQuery();
                if (kq > 0)
                {
                    MessageBox.Show("Bạn đã sửa thành công 1 danh mục", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HienThiDanhMuc();
                    txtTenDM.Text = "";
                    txtMaDM.Text = "";
                }
                else
                {
                    MessageBox.Show("Sửa danh mục thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        string maDM = "";
        private void lvDanhMuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvDanhMuc.SelectedItems.Count == 0)
                return;
            ListViewItem lvi = lvDanhMuc.SelectedItems[0];
            maDM = lvi.Tag+"";
            HienThiChiTietDanhMuc(maDM);

        }
        private void HienThiChiTietDanhMuc(string maDM)
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "select * from DanhMuc where MaDanhMuc = @maDM";
                command.Connection = connection;
                command.Parameters.Add("@maDM", SqlDbType.NVarChar).Value = maDM;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    txtTenDM.Text = reader.GetString(1);
                    txtMaDM.Text = reader.GetString(0);
                }
                reader.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoaDM_Click(object sender, EventArgs e)
        {
            if(lvDanhMuc.SelectedItems.Count ==0)
            {
                MessageBox.Show("Bạn chưa chọn danh mục nào để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }   
            else
            {
                DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa danh mục này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if(result == DialogResult.Yes)
                {
                    try
                    {
                        OpenConnection();
                        SqlCommand command = new SqlCommand();
                        command.CommandType = CommandType.Text;
                        command.CommandText = "Delete from DanhMuc where MaDanhMuc = @maDM";
                        command.Connection = connection;
                        command.Parameters.Add("@maDM", SqlDbType.NVarChar).Value = maDM;
                        int kq = command.ExecuteNonQuery();
                        if (kq > 0)
                        {
                            MessageBox.Show("Xóa thành công 1 danh mục", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            HienThiDanhMuc();
                            txtMaDM.Text = "";
                            txtTenDM.Text = "";
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

        private void frmDanhMuc_FormClosing(object sender, FormClosingEventArgs e)
        {
            layDuLieu();
        }
    }
}
