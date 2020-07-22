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
    public partial class frmDanhSachHoaDon : Form
    {
        public frmDanhSachHoaDon()
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
        private void frmDanhSachHoaDon_Load(object sender, EventArgs e)
        {
            HienThiDanhSachHoaDon();
        }

        private void HienThiDanhSachHoaDon()
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand("select * from HoaDon");
                command.Connection = connection;
                SqlDataReader reader = command.ExecuteReader();
                lvHoaDon.Items.Clear();
                while(reader.Read())
                {
                    ListViewItem lvi = new ListViewItem(reader.GetString(0));
                    lvi.SubItems.Add(reader.GetString(1));
                    lvi.SubItems.Add(reader.GetString(2));
                    lvi.SubItems.Add(DateTime.Parse(reader.GetDateTime(3).ToString()).ToString("dd/MM/yyyy"));
                    lvi.SubItems.Add(reader.GetString(4));
                    lvi.SubItems.Add(reader.GetDouble(5)+"");
                    lvHoaDon.Items.Add(lvi);
                    lvi.Tag = reader.GetString(0);
                }
                reader.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        string maHD;
        private void lvHoaDon_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvHoaDon.SelectedItems.Count == 0)
                return;
            ListViewItem lvi = lvHoaDon.SelectedItems[0];
            maHD = lvi.Tag+"";
        }
        private void XoaHoaDon(string maHD)
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand("delete from HoaDon where MaHoaDon = @maHD");
                command.Connection = connection;
                command.Parameters.Add("@maHD", SqlDbType.NVarChar).Value = maHD;
                int kq = command.ExecuteNonQuery();
                if(kq>0)
                {
                    MessageBox.Show("Đã xóa 1 hóa đơn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }    
                else
                {
                    MessageBox.Show("Xóa thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }    
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if(lvHoaDon.SelectedItems.Count==0)
            {
                MessageBox.Show("Bạn chưa chọn hóa đơn để xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
            else
            {
                DialogResult result = MessageBox.Show("Xóa hóa đơn sẽ xóa toàn bộ chi tiết hóa đơn,Bạn có chắc muốn xóa chứ?", "Hỏi xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(result == DialogResult.Yes)
                {
                    XoaHoaDon(maHD);
                    HienThiDanhSachHoaDon();
                }    
            }    
            
            
        }
    }
}
