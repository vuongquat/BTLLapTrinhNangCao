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
    public partial class frmMainQuanLy : Form
    {
        public frmMainQuanLy()
        {
            InitializeComponent();
        }

        //=================================== Chuỗi kết nối =====================================//
        string chuoiketnoi = "server = DESKTOP-FFGALN9;database = QLCH;user id = vuongquat;pwd = 123456";
        SqlConnection connection = null;
        //=================================== Chuỗi kết nối =====================================//

        private void btnThemDM_Click(object sender, EventArgs e)
        {
            frmDanhMuc frmDanhMuc = new frmDanhMuc();
            frmDanhMuc.layDuLieu = new frmDanhMuc.LayDuLieu(ThayDoiDuLieuTuDanhMuc);
            frmDanhMuc.Show();
        }
        public void ThayDoiDuLieuTuDanhMuc()
        {
            HienThiDanhMuc();
        }
        private void btnTaoTKNV_Click(object sender, EventArgs e)
        {
            frmTaiKhoanNV frmTaiKhoanNV = new frmTaiKhoanNV();
            frmTaiKhoanNV.Show();
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
            HienThiDanhSachNhanVien();
            HienThiDanhMuc();
            HienThiDanhSachSanPham();
            HienThiDanhSachKhachHang();

        }
        //===================================================================================================//
        //=====================================Thêm, Sửa, Xóa Sản phẩm ======================================//

        private void txtDonGiaNhap_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && (!char.IsControl(e.KeyChar)) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

        }

        private void txtDonGiaBan_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && (!char.IsControl(e.KeyChar)) && e.KeyChar != ',')
            {
                e.Handled = true;
            }
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
                SqlDataReader reader = command.ExecuteReader();
                cboDanhMuc.Items.Clear();
                while (reader.Read())
                {
                    cboDanhMuc.Items.Add(reader.GetString(0) + "-" + reader.GetString(1));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        string maDM = "";
        private void cboDanhMuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDanhMuc.SelectedIndex == -1)
                return;
            string line = cboDanhMuc.SelectedItem + "";
            string[] arr = line.Split('-');
            maDM = arr[0];
        }

        public void HienThiDanhSachSanPham()
        {

            OpenConnection();
            SqlCommand command = new SqlCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "select * from SanPham";
            command.Connection = connection;
            SqlDataReader reader = command.ExecuteReader();
            lvDSSanPham.Items.Clear();
            cboTenSP.Items.Clear();
            while (reader.Read())
            {

                ListViewItem lvi = new ListViewItem(reader.GetString(0));
                lvi.SubItems.Add(reader.GetString(1));
                lvi.SubItems.Add(reader.GetString(2));
                lvi.SubItems.Add(reader.GetInt32(3) + "");
                lvi.SubItems.Add(reader.GetDouble(4) + "");
                lvi.SubItems.Add(reader.GetDouble(5) + "");
                lvDSSanPham.Items.Add(lvi);
                cboTenSP.Items.Add(reader.GetString(1));
                lvi.Tag = reader.GetString(0);

            }
            reader.Close();
        }

        private void btnThemSP_Click(object sender, EventArgs e)
        {

            double donGiaNhap, donGiaBan;

            if (KiemTraTonTaiSanPham(txtMaSP.Text))
            {
                MessageBox.Show("Mã sản phẩm đã tồn tại vui lòng nhập mã sản phẩm khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (txtTenSP.Text == "" || txtMaSP.Text == "" || numberSoLuong.Value == 0 || txtDonGiaNhap.Text == "" || txtDonGiaBan.Text == "")
                {
                    MessageBox.Show("Bạn chưa nhập đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                else if (cboDanhMuc.SelectedIndex == -1)
                {
                    MessageBox.Show("Bạn chưa chọn danh mục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (double.TryParse(txtDonGiaNhap.Text, out donGiaNhap) == false || double.TryParse(txtDonGiaBan.Text, out donGiaBan) == false)
                {
                    MessageBox.Show("Kiểm tra lại đơn giá nhập hoặc đơn giá bán", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    try
                    {
                        OpenConnection();
                        SqlCommand command = new SqlCommand();
                        command.CommandType = CommandType.Text;
                        command.CommandText = "insert into SanPham(MaSanPham,TenSanPham,MaDanhMuc,SoLuong,DonGiaNhap,DonGiaBan)" +
                            "values(@maSP,@tenSP,@maDM,@soLuong,@donGiaNhap,@donGiaBan) ";
                        command.Connection = connection;
                        command.Parameters.Add("@maSP", SqlDbType.NVarChar).Value = txtMaSP.Text;
                        command.Parameters.Add("@tenSP", SqlDbType.NVarChar).Value = txtTenSP.Text;
                        command.Parameters.Add("@maDM", SqlDbType.NVarChar).Value = maDM;
                        command.Parameters.Add("@soLuong", SqlDbType.Int).Value = numberSoLuong.Value;
                        command.Parameters.Add("@donGiaNhap", SqlDbType.Float).Value = txtDonGiaNhap.Text;
                        command.Parameters.Add("@donGiaBan", SqlDbType.Float).Value = txtDonGiaBan.Text;
                        int kq = command.ExecuteNonQuery();
                        if (kq > 0)
                        {
                            txtTenSP.Text = "";
                            txtMaSP.Text = "";
                            numberSoLuong.Value = 0;
                            txtDonGiaNhap.Text = "";
                            txtDonGiaBan.Text = "";
                            cboDanhMuc.Items.Clear();
                            HienThiDanhMuc();
                            MessageBox.Show("Thêm thành công 1 sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            HienThiDanhSachSanPham();
                        }
                        else
                        {
                            MessageBox.Show("Thêm thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        string maSP = "";

        private void lvDSSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvDSSanPham.SelectedItems.Count == 0)
                return;
            ListViewItem lvi = lvDSSanPham.SelectedItems[0];
            maSP = lvi.Tag + "";
            maDM = lvi.SubItems[2].Text;
            LayTenDanhMuc(maDM);
            HienThiChiTietSanPham(maSP);

        }

        private void HienThiChiTietSanPham(string maSP)
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "select * from SanPham where MaSanPham = @maSP";
                command.Connection = connection;
                command.Parameters.Add("@maSP", SqlDbType.NVarChar).Value = maSP;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    txtTenSP.Text = reader.GetString(1);
                    txtMaSP.Text = reader.GetString(0);
                    numberSoLuong.Value = reader.GetInt32(3);
                    cboDanhMuc.Text = reader.GetString(2) + "-" + tenDM;
                    txtDonGiaNhap.Text = reader.GetDouble(4) + "";
                    txtDonGiaBan.Text = reader.GetDouble(5) + "";
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        string tenDM = "";
        private void LayTenDanhMuc(string maDM)
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
                    tenDM = reader.GetString(1);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSuaSP_Click(object sender, EventArgs e)
        {
            double donGiaNhap, donGiaBan;
            if (lvDSSanPham.SelectedItems.Count == 0)
            {
                MessageBox.Show("Bạn chưa chọn sản phẩm để sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            if (txtTenSP.Text == "" || txtMaSP.Text == "" || numberSoLuong.Value == 0 || txtDonGiaNhap.Text == "" || txtDonGiaBan.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else if (cboDanhMuc.SelectedIndex == -1)
            {
                MessageBox.Show("Bạn chưa chọn danh mục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (double.TryParse(txtDonGiaNhap.Text, out donGiaNhap) == false || double.TryParse(txtDonGiaBan.Text, out donGiaBan) == false)
            {
                MessageBox.Show("Kiểm tra lại đơn giá nhập hoặc đơn giá bán", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    OpenConnection();
                    SqlCommand command = new SqlCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "update SanPham set MaSanPham = @maSPMoi,TenSanPham = @tenSP,MaDanhMuc = @maDM , SoLuong = @soLuong , DonGiaNhap = @donGiaNhap, DonGiaBan = @donGiaBan where MaSanPham = @maSP";
                    command.Connection = connection;
                    command.Parameters.Add("@maSPMoi", SqlDbType.NVarChar).Value = txtMaSP.Text;
                    command.Parameters.Add("@tenSP", SqlDbType.NVarChar).Value = txtTenSP.Text;
                    command.Parameters.Add("@maDM", SqlDbType.NVarChar).Value = maDM;
                    command.Parameters.Add("@soLuong", SqlDbType.Int).Value = numberSoLuong.Value;
                    command.Parameters.Add("@donGiaNhap", SqlDbType.Float).Value = txtDonGiaNhap.Text;
                    command.Parameters.Add("@donGiaBan", SqlDbType.Float).Value = txtDonGiaBan.Text;
                    command.Parameters.Add("@maSP", SqlDbType.NVarChar).Value = maSP;
                    int kq = command.ExecuteNonQuery();
                    if (kq > 0)
                    {
                        txtTenSP.Text = "";
                        txtMaSP.Text = "";
                        numberSoLuong.Value = 0;
                        txtDonGiaNhap.Text = "";
                        txtDonGiaBan.Text = "";
                        cboDanhMuc.Items.Clear();
                        HienThiDanhMuc();
                        MessageBox.Show("Sửa thành công 1 sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        HienThiDanhSachSanPham();
                    }
                    else
                    {
                        MessageBox.Show("Sửa thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnXoaSP_Click(object sender, EventArgs e)
        {
            if (lvDSSanPham.SelectedItems.Count == 0)
            {
                MessageBox.Show("Bạn chưa chọn sản phẩm để sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa sản phẩm này không?", "Hỏi xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                try
                {
                    OpenConnection();
                    SqlCommand command = new SqlCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "delete SanPham where MaSanPham = @maSP";
                    command.Connection = connection;
                    command.Parameters.Add("@maSP", SqlDbType.NVarChar).Value = maSP;
                    int kq = command.ExecuteNonQuery();
                    if (kq > 0)
                    {
                        txtTenSP.Text = "";
                        txtMaSP.Text = "";
                        numberSoLuong.Value = 0;
                        txtDonGiaNhap.Text = "";
                        txtDonGiaBan.Text = "";
                        cboDanhMuc.Items.Clear();
                        HienThiDanhMuc();
                        MessageBox.Show("Xóa thành công 1 sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        HienThiDanhSachSanPham();

                    }
                    else
                    {
                        MessageBox.Show("Xóa thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool KiemTraTonTaiSanPham(string maSP)
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "select * from SanPham where MaSanPham = @maSP";
                command.Connection = connection;
                command.Parameters.Add("@maSP", SqlDbType.NVarChar).Value = maSP;
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



        //===================================================================================================//
        //===================================================================================================//



        //================================================================================================//
        //=============================== Thêm, Sửa, Xóa Nhân Viên =======================================//

        private void HienThiDanhSachNhanVien()
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "Select * from NhanVien except select * from NhanVien where MaNhanVien = 'QL' ";
                command.Connection = connection;

                lvNhanVien.Items.Clear();
                cboNhanVien.Items.Clear();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ListViewItem lvi = new ListViewItem(reader.GetString(0));
                    lvi.SubItems.Add(reader.GetString(1));
                    lvi.SubItems.Add(reader.GetString(2));
                    lvi.SubItems.Add(reader.GetString(3));
                    lvi.SubItems.Add(reader.GetString(4));
                    lvi.SubItems.Add(DateTime.Parse(reader.GetDateTime(5).ToString()).ToString("dd/MM/yyyy"));
                    lvNhanVien.Items.Add(lvi);
                    cboNhanVien.Items.Add(reader.GetString(0) + "-" + reader.GetString(1));
                    lvi.Tag = reader.GetString(0);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo");
            }
        }



        string maNV = "";
        private void lvNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvNhanVien.SelectedItems.Count == 0)
                return;
            ListViewItem lvi = lvNhanVien.SelectedItems[0];
            maNV = lvi.Tag + "";
            HienThiChiTietNhanVien(maNV);
        }
        private void HienThiChiTietNhanVien(string maNV)
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "select * from NhanVien where MaNhanVien = @maNV";
                command.Connection = connection;
                command.Parameters.Add("@maNV", SqlDbType.NVarChar).Value = maNV;

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    txtMaNV.Text = reader.GetString(0);
                    txtTenNV.Text = reader.GetString(1);
                    if (reader.GetString(2) == "Nam")
                        radNam.Checked = true;
                    else
                        radNu.Checked = true;
                    txtDiaChiNV.Text = reader.GetString(3);
                    txtSDTNV.Text = reader.GetString(4);
                    dtpNgaySinhNV.Value = reader.GetDateTime(5);

                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnXoaNV_Click(object sender, EventArgs e)
        {
            if (lvNhanVien.SelectedItems.Count == 0)
            {
                MessageBox.Show("Bạn chưa chọn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa nhân viên này không?", "Hỏi xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                { 
                    XoaNhanVien();
                }
            }

        }
        private void XoaNhanVien()
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand("delete from NhanVien where MaNhanVien = @maNV");
                command.Connection = connection;
                command.Parameters.Add("@maNV", SqlDbType.NVarChar).Value = maNV;
                command.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Bạn phải xóa tài khoản của nhân viên trước", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
        private void ThemNhanVien()
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "Insert into NhanVien(MaNhanVien,TenNhanVien,GioiTinh,DiaChi,DienThoai,NgaySinh) " +
                    "values (@maNV,@tenNV,@gioiTinhNV,@diaChiNV,@sdtNV,@ngaySinhNV)";
                command.Connection = connection;
                command.Parameters.Add("@maNV", SqlDbType.NVarChar).Value = txtMaNV.Text;
                command.Parameters.Add("@tenNV", SqlDbType.NVarChar).Value = txtTenNV.Text;
                if (radNam.Checked == true)
                    command.Parameters.Add("@gioiTinhNV", SqlDbType.NVarChar).Value = radNam.Text;
                else
                    command.Parameters.Add("@gioiTinhNV", SqlDbType.NVarChar).Value = radNu.Text;
                command.Parameters.Add("@diaChiNV", SqlDbType.NVarChar).Value = txtDiaChiNV.Text;
                command.Parameters.Add("@sdtNV", SqlDbType.NVarChar).Value = txtSDTNV.Text;
                command.Parameters.Add("@ngaySinhNV", SqlDbType.Date).Value = dtpNgaySinhNV.Value;

                int kq = command.ExecuteNonQuery();
                if (kq > 0)
                {
                    MessageBox.Show("Thêm thành công");
                    HienThiDanhSachNhanVien();
                    txtTenNV.Text = "";
                    txtMaNV.Text = "";
                    txtDiaChiNV.Text = "";
                    txtSDTNV.Text = "";
                    radNam.Checked = false;
                    radNu.Checked = false;
                    dtpNgaySinhNV.Value = DateTime.Now;

                }
                else
                {
                    MessageBox.Show("Lỗi,vui lòng thử lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SuaNhanVien()
        {

            OpenConnection();
            SqlCommand command = new SqlCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "update NhanVien set TenNhanVien = @tenNV , GioiTinh = @gioiTinhNV,DiaChi = @diaChiNV,DienThoai = @sdtNV,NgaySinh = @ngaySinhNV where MaNhanVien = @maNV";
            command.Connection = connection;

            command.Parameters.Add("@tenNV", SqlDbType.NVarChar).Value = txtTenNV.Text;
            if (radNam.Checked == true)
                command.Parameters.Add("@gioiTinhNV", SqlDbType.NVarChar).Value = radNam.Text;
            else
                command.Parameters.Add("@gioiTinhNV", SqlDbType.NVarChar).Value = radNu.Text;
            command.Parameters.Add("@diaChiNV", SqlDbType.NVarChar).Value = txtDiaChiNV.Text;
            command.Parameters.Add("@sdtNV", SqlDbType.NVarChar).Value = txtSDTNV.Text;
            command.Parameters.Add("@ngaySinhNV", SqlDbType.DateTime).Value = dtpNgaySinhNV.Value;
            command.Parameters.Add("@maNV", SqlDbType.NVarChar).Value = txtMaNV.Text;

            int kq = command.ExecuteNonQuery();
            if (kq > 0)
            {
                MessageBox.Show("Sửa dữ liệu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                HienThiDanhSachNhanVien();
            }
            else
            {
                MessageBox.Show("Lỗi");
            }



        }

        bool KiemTraTonTaiNhanVien(string maNV)
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "select * from NhanVien where MaNhanVien = @maNV";
                command.Connection = connection;
                command.Parameters.Add("@maNV", SqlDbType.NVarChar).Value = maNV;
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

        private void btnLuuNV_Click(object sender, EventArgs e)
        {
            if (txtTenNV.Text == "" || txtMaNV.Text == "" || txtDiaChiNV.Text == "" || txtSDTNV.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập đầy đủ thông tin, vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (radNam.Checked == true || radNu.Checked == true)
            {
                if (KiemTraTonTaiNhanVien(txtMaNV.Text) == true)
                {
                    SuaNhanVien();
                }
                else
                {
                    ThemNhanVien();
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn giới tính, vui lòng kiểm tra lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //================================================================================================//
        //================================================================================================//




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
            if (txtMaHD.Text == "" || cboMaKH.Text == "" || cboNhanVien.Text == "" || cboThanhToan.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if(KiemTraHoaDonTonTai(txtMaHD.Text))
                {
                    HienThiChiTietHoaDon();
                    lblMaHD.Text = "Tên hóa đơn: " + txtMaHD.Text;
                    txtMaHD.Enabled = false;
                    cboMaKH.Enabled = false;
                    cboNhanVien.Enabled = false;
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
                        command.Parameters.Add("@maNVBan", SqlDbType.NVarChar).Value = maNVBan;
                        command.Parameters.Add("@maKhach", SqlDbType.NVarChar).Value = maKHMua;
                        command.Parameters.Add("@ngayBan", SqlDbType.Date).Value = dtpNgayBan.Value;
                        command.Parameters.Add("@thanhToan", SqlDbType.NVarChar).Value = cboThanhToan.Text;
                        int kq = command.ExecuteNonQuery();
                        if (kq > 0)
                        {
                            MessageBox.Show("Tạo thành công 1 hóa đơn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lblMaHD.Text = "Tên hóa đơn: " + txtMaHD.Text;
                            txtMaHD.Enabled = false;
                            cboMaKH.Enabled = false;
                            cboNhanVien.Enabled = false;
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


        string maNVBan = "";
        private void cboNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboNhanVien.SelectedIndex == -1)
                return;
            string line = cboNhanVien.SelectedItem+"";
            string[] arr = line.Split('-');
            maNVBan = arr[0];
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
                txtMaHD.Enabled = true;
                txtMaHD.Text = "";
                cboNhanVien.SelectedIndex = -1;
                cboNhanVien.Enabled = true;
                cboMaKH.SelectedIndex = -1;
                cboMaKH.Enabled = true;
                cboThanhToan.SelectedIndex = -1;
                cboThanhToan.Enabled = true;
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
    }
}
