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
using System.Windows.Forms.VisualStyles;

namespace BaiTapLonMonLapTrinhNangCao
{
    public partial class frmMainNhanVien : Form
    {
        public frmMainNhanVien()
        {
            InitializeComponent();
        }

        //=================================== Chuỗi kết nối =====================================//
        string chuoiketnoi = "server = DESKTOP-FFGALN9;database = QLCH;user id = vuongquat;pwd = 123456";
        SqlConnection connection = null;
        //=================================== Chuỗi kết nối =====================================//


        string taiKhoanNhanVien;
        public frmMainNhanVien(string taiKhoan) : this()
        {
            taiKhoanNhanVien = taiKhoan;
            lblTaiKhoan.Text = taiKhoanNhanVien;
        }
        
        

        private void frmMainQuanLy_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát chương trình không?", "Hỏi thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void OpenConnection()
        {
            if (connection == null)
                connection = new SqlConnection(chuoiketnoi);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
        }

        private void frmMainQuanLy_Load(object sender, EventArgs e)
        { 
            LayMaNhanVien(taiKhoanNhanVien);
            HienThiThongTinNhanVien();
            HienThiDanhSachSanPham();
            HienThiDanhSachKhachHang();
            txtMaNVBan.Text = maNV;
        }
        //===========================================================================================================//
        //============================================ Xử lý code phần Khách hàng ===================================//

        private void HienThiDanhSachKhachHang()
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "select * from KhachHang";
                command.Connection = connection;
                SqlDataReader reader = command.ExecuteReader();
                lvKhachHang.Items.Clear();
                cboMaKH.Items.Clear();
                while (reader.Read())
                {
                    ListViewItem lvi = new ListViewItem(reader.GetString(0));
                    lvi.SubItems.Add(reader.GetString(1));
                    lvi.SubItems.Add(reader.GetString(2));
                    lvi.SubItems.Add(reader.GetString(3));
                    lvKhachHang.Items.Add(lvi);
                    cboMaKH.Items.Add(reader.GetString(0) + "-" + reader.GetString(1));
                    lvi.Tag = reader.GetString(0);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ThemKhachHang()
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "insert into KhachHang(MaKhach,TenKhach,DiaChi,DienThoai) " +
                    "values(@maKhach,@tenKhach,@diaChi,@dienThoai)";
                command.Connection = connection;
                command.Parameters.Add("@maKhach", SqlDbType.NVarChar).Value = txtMaKH.Text;
                command.Parameters.Add("@tenKhach", SqlDbType.NVarChar).Value = txtTenKH.Text;
                command.Parameters.Add("@diaChi", SqlDbType.NVarChar).Value = txtDiaChiKH.Text;
                command.Parameters.Add("@dienThoai", SqlDbType.NVarChar).Value = txtSDTKH.Text;
                int kq = command.ExecuteNonQuery();
                if (kq > 0)
                {
                    txtMaKH.Text = "";
                    txtTenKH.Text = "";
                    txtDiaChiKH.Text = "";
                    txtSDTKH.Text = "";
                    MessageBox.Show("Thêm thành công 1 khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HienThiDanhSachKhachHang();
                }
                else
                {
                    MessageBox.Show("Thêm thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool KiemTraTonTaiMaKhachHang(string maKH)
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "select * from KhachHang where MaKhach = @maKH";
                command.Connection = connection;
                command.Parameters.Add("@maKH", SqlDbType.NVarChar).Value = maKH;
                SqlDataReader reader = command.ExecuteReader();
                bool kq = reader.Read();
                reader.Close();
                return kq;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        private void txtSDTKH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && (!char.IsControl(e.KeyChar)) && e.KeyChar != '+')
            {
                e.Handled = true;
            }
        }

        string maKH = "";
        private void lvKhachHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvKhachHang.SelectedItems.Count == 0)
                return;
            ListViewItem lvi = lvKhachHang.SelectedItems[0];
            maKH = lvi.Tag + "";
            HienThiChiTietKhachHang(maKH);
        }

        private void HienThiChiTietKhachHang(string maKH)
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "select * from KhachHang where MaKhach = @maKH";
                command.Connection = connection;
                command.Parameters.Add("@maKH", SqlDbType.NVarChar).Value = maKH;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    txtTenKH.Text = reader.GetString(1);
                    txtMaKH.Text = reader.GetString(0);
                    txtDiaChiKH.Text = reader.GetString(2);
                    txtSDTKH.Text = reader.GetString(3);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bntXoaKH_Click(object sender, EventArgs e)
        {
            if (lvKhachHang.SelectedItems.Count == 0)
            {
                MessageBox.Show("Bạn chưa chọn khách hàng để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa khách hàng này không?", "Hỏi xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                try
                {
                    OpenConnection();
                    SqlCommand command = new SqlCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "delete from KhachHang where MaKhach = @maKhach";
                    command.Connection = connection;
                    command.Parameters.Add("@maKhach", SqlDbType.NVarChar).Value = maKH;
                    int kq = command.ExecuteNonQuery();
                    if (kq > 0)
                    {
                        txtMaKH.Text = "";
                        txtTenKH.Text = "";
                        txtDiaChiKH.Text = "";
                        txtSDTKH.Text = "";
                        MessageBox.Show("Bạn đã xóa thành công 1 khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        HienThiDanhSachKhachHang();

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

        private void btnLuuKH_Click(object sender, EventArgs e)
        {
            if (txtTenKH.Text == "" || txtMaKH.Text == "" || txtDiaChiKH.Text == "" || txtSDTKH.Text == "")
            {
                MessageBox.Show("Bạn chưa điền đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (KiemTraTonTaiMaKhachHang(txtMaKH.Text))
                {
                    SuaKhachHang();
                }
                else
                {
                    ThemKhachHang();
                }
            }
        }

        private void SuaKhachHang()
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "update KhachHang set TenKhach = @tenKhach,DiaChi = @diaChi,DienThoai = @dienThoai where MaKhach = @maKhach";
                command.Connection = connection;
                command.Parameters.Add("@tenKhach", SqlDbType.NVarChar).Value = txtTenKH.Text;
                command.Parameters.Add("@diaChi", SqlDbType.NVarChar).Value = txtDiaChiKH.Text;
                command.Parameters.Add("@dienThoai", SqlDbType.NVarChar).Value = txtSDTKH.Text;
                command.Parameters.Add("@maKhach", SqlDbType.NVarChar).Value = txtMaKH.Text;
                int kq = command.ExecuteNonQuery();
                if (kq > 0)
                {
                    txtTenKH.Text = "";
                    txtMaKH.Text = "";
                    txtDiaChiKH.Text = "";
                    txtSDTKH.Text = "";
                    MessageBox.Show("Sửa thành công 1 dữ liệu");
                    HienThiDanhSachKhachHang();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //============================================================================================================================//
        //============================================================================================================================//


        //====================================== Xử lý Phần code thêm hóa đơn ========================================================//
        //============================================================================================================================//

        double donGiaBan;


        private void HienThiTenSanPham(string tenSPBan)
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "Select * from SanPham where TenSanPham = @tenSPBan";
                command.Connection = connection;
                command.Parameters.Add("@tenSPBan", SqlDbType.NVarChar).Value = tenSPBan;
                SqlDataReader reader = command.ExecuteReader();
                while(reader.Read())
                {
                    txtMaSPBan.Text = reader.GetString(0);
                    txtDonGia.Text = reader.GetDouble(5)+"";
                    donGiaBan = reader.GetDouble(5);
                }
                reader.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void cboTenSP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTenSP.SelectedIndex == -1)
                return;
            string tenSPBan = cboTenSP.SelectedItem + "";
            HienThiTenSanPham(tenSPBan);
            txtThanhTien.Text = "0";
            numberSoLuongBan.Value = 0;
        }
        double thanhTien,tongTien;
        private void numberSoLuongBan_ValueChanged(object sender, EventArgs e)
        {
            double soLuongBan = (double)numberSoLuongBan.Value;
            thanhTien = soLuongBan * donGiaBan;
            txtThanhTien.Text = thanhTien+"";
        }
        SqlDataAdapter adapterChiTietHoaDon = null;
        DataSet dsChiTietHoaDon = null;
        private void HienThiChiTietHoaDon()
        {
            OpenConnection();
            adapterChiTietHoaDon = new SqlDataAdapter("select MaHoaDon as 'Mã HĐ',MaSanPham as 'Mã SP',TenSanPham as 'Tên sản phẩm',SoLuong as 'Số lượng',ThanhTien as 'Thành tiền'" +
                " from ChiTietHoaDon" +
                " where MaHoaDon = '" + txtMaHD.Text +"'",  connection);
            dsChiTietHoaDon = new DataSet();
            adapterChiTietHoaDon.Fill(dsChiTietHoaDon);
            gvChiTietHoaDon.DataSource = dsChiTietHoaDon.Tables[0];
        }
        private void btnChonSP_Click(object sender, EventArgs e)
        {
            if(cboTenSP.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }    
            else if(numberSoLuongBan.Value == 0)
            {
                MessageBox.Show("Bạn chưa nhập số lượng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }    
            else
            {
                try
                {
                    OpenConnection();
                    SqlCommand command = new SqlCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "insert into ChiTietHoaDon(MaHoaDon,MaSanPham,TenSanPham,SoLuong,DonGia,ThanhTien)" +
                        "values(@maHD,@maSPBan,@tenSPBan,@soLuongBan,@donGiaBan,@thanhTien)";
                    command.Connection = connection;
                    command.Parameters.Add("@maHD", SqlDbType.NVarChar).Value = txtMaHD.Text;
                    command.Parameters.Add("@maSPBan", SqlDbType.NVarChar).Value = txtMaSPBan.Text;
                    command.Parameters.Add("@tenSPBan", SqlDbType.NVarChar).Value = cboTenSP.Text;
                    command.Parameters.Add("@soLuongBan", SqlDbType.Int).Value = numberSoLuongBan.Value;
                    command.Parameters.Add("@donGiaBan", SqlDbType.Float).Value = txtDonGia.Text;
                    command.Parameters.Add("@thanhTien", SqlDbType.Float).Value = txtThanhTien.Text;

                    int kq = command.ExecuteNonQuery();
                    if (kq > 0)
                    {
                        HienThiChiTietHoaDon();
                        tongTien += thanhTien;
                        lblTongTien.Text = tongTien+"";
                    }
                    else
                    {
                        MessageBox.Show("Lỗi chọn hóa đơn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }    
            }    
        }
        

        private void btnLuuHoaDon_Click(object sender, EventArgs e)
        {
               

        }

        

        private void btnTaoHD_Click(object sender, EventArgs e)
        {
            if (txtMaHD.Text == "" || cboMaKH.Text == ""  || cboThanhToan.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if(KiemTraHoaDonTonTai(txtMaHD.Text))
                {
                    HienThiChiTietHoaDon();
                    lblMaHD.Text = "Tên hóa đơn: " + txtMaHD.Text;
                    cboTenSP.Enabled = true;
                    txtMaHD.Enabled = false;
                    cboTenSP.Enabled = true;
                    cboMaKH.Enabled = false;
                    cboThanhToan.Enabled = false;
                    dtpNgayBan.Enabled = false;
                    btnChonSP.Enabled = true;
                    btnTaoHD.Enabled = false;
                }   
                else
                {
                    try
                    {
                        OpenConnection();
                        SqlCommand command = new SqlCommand();
                        command.CommandType = CommandType.Text;
                        command.CommandText = "insert into HoaDon (MaHoaDon,MaNhanVien,MaKhach,NgayBan,PhuongThucThanhToan,TongTien)" +
                            "values(@maHD,@maNVBan,@maKhach,@ngayBan,@thanhToan,0)";
                        command.Connection = connection;
                        command.Parameters.Add("@maHD", SqlDbType.NVarChar).Value = txtMaHD.Text;
                        command.Parameters.Add("@maNVBan", SqlDbType.NVarChar).Value = txtMaNVBan.Text;
                        command.Parameters.Add("@maKhach", SqlDbType.NVarChar).Value = maKHMua;
                        command.Parameters.Add("@ngayBan", SqlDbType.Date).Value = dtpNgayBan.Value;
                        command.Parameters.Add("@thanhToan", SqlDbType.NVarChar).Value = cboThanhToan.Text;
                        int kq = command.ExecuteNonQuery();
                        if (kq > 0)
                        {
                            MessageBox.Show("Đã tạo mới một hóa đơn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lblMaHD.Text = "Tên hóa đơn: " + txtMaHD.Text;
                            txtMaHD.Enabled = false;
                            cboMaKH.Enabled = false;
                            cboThanhToan.Enabled = false;
                            dtpNgayBan.Enabled = false;
                            btnChonSP.Enabled = true;
                            btnTaoHD.Enabled = false;

                        }
                        else
                        {
                            MessageBox.Show("Tạo hóa đơn thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }    
            }     
        }


        
        
        string maKHMua = "";
        private void cboMaKH_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaKH.SelectedIndex == -1)
                return;
            string line = cboMaKH.SelectedItem + "";
            string[] arr = line.Split('-');
            maKHMua = arr[0];
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
           DialogResult result =  MessageBox.Show("Bạn muốn làm mới toàn bộ trang hóa đơn chứ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if(result == DialogResult.Yes)
            {
                txtMaHD.Text = "";
                txtMaHD.Enabled = true;
                cboMaKH.SelectedIndex = -1;
                cboMaKH.Enabled = true;
                cboThanhToan.SelectedIndex = -1;
                cboThanhToan.Enabled = true;
                cboTenSP.SelectedIndex = -1;
                cboTenSP.Enabled = false;
                txtMaSPBan.Text = "";
                txtDonGia.Text = "";
                txtThanhTien.Text = "";
                numberSoLuongBan.Value = 0;
                thanhTien = 0;
                tongTien = 0;
                lblTongTien.Text = "";
                gvChiTietHoaDon.DataSource = null;
            }    
        }

        private void btnDanhSachHoaDon_Click(object sender, EventArgs e)
        {
            frmDanhSachHoaDon frmDanhSachHoaDon = new frmDanhSachHoaDon();
            frmDanhSachHoaDon.Show();
        }

        private bool KiemTraHoaDonTonTai(string maHD)
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "select * from HoaDon Where MaHoaDon = @maHD";
                command.Connection = connection;
                command.Parameters.Add("@maHD", SqlDbType.NVarChar).Value = maHD;
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

        //===================================================================================================//
        //=====================================Sản phẩm ======================================//

        private void HienThiDanhSachSanPham()
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "select * from SanPham";
                command.Connection = connection;
                SqlDataReader reader = command.ExecuteReader();
                lvDSSanPham.Items.Clear();
                while(reader.Read())
                {
                    ListViewItem lvi = new ListViewItem(reader.GetString(0));
                    lvi.SubItems.Add(reader.GetString(1));
                    lvi.SubItems.Add(reader.GetString(2));
                    lvi.SubItems.Add(reader.GetInt32(3)+"");
                    lvi.SubItems.Add(reader.GetDouble(4) + "");
                    lvi.SubItems.Add(reader.GetDouble(5) + "");
                    lvDSSanPham.Items.Add(lvi);
                    cboTenSP.Items.Add(reader.GetString(1));
                }
                reader.Close();
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //===================================================================================================//
        //===================================================================================================//



        //===================================================================================================//
        //==========================================Nhân Viên================================================//
        string maNV;
        private void LayMaNhanVien(string taiKhoan)
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand("select MaNhanVien from TaiKhoanNhanVien where TaiKhoan = @taiKhoan");
                command.Connection = connection;
                command.Parameters.Add("@taiKhoan", SqlDbType.NVarChar).Value = taiKhoan;
                SqlDataReader reader = command.ExecuteReader();
                while(reader.Read())
                {
                    maNV = reader.GetString(0);
                }
                reader.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HienThiThongTinNhanVien()
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand("select * from NhanVien where MaNhanVien = @maNV");
                command.Connection = connection;
                command.Parameters.Add("@maNV", SqlDbType.NVarChar).Value = maNV;
                SqlDataReader reader = command.ExecuteReader();
                while(reader.Read())
                {
                    lblMaNV.Text = reader.GetString(0);
                    lblTenNV.Text = reader.GetString(1);
                    lblGioiTinh.Text = reader.GetString(2);
                    lblDiaChi.Text = reader.GetString(3);
                    lblSDT.Text = reader.GetString(4);
                    lblNgaySinh.Text = DateTime.Parse(reader.GetDateTime(5).ToString()).ToString("dd/MM/yyyy");
                }
                reader.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //===================================================================================================//
        //===================================================================================================//






    }
}
