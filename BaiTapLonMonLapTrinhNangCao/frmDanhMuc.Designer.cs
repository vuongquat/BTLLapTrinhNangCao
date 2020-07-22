namespace BaiTapLonMonLapTrinhNangCao
{
    partial class frmDanhMuc
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDanhMuc));
            this.gbTaoDM = new System.Windows.Forms.GroupBox();
            this.txtMaDM = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTenDM = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbDSDM = new System.Windows.Forms.GroupBox();
            this.lvDanhMuc = new System.Windows.Forms.ListView();
            this.MaDM = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TenDM = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnXoaDM = new System.Windows.Forms.Button();
            this.btnDong = new System.Windows.Forms.Button();
            this.btnLuuDM = new System.Windows.Forms.Button();
            this.gbTaoDM.SuspendLayout();
            this.gbDSDM.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbTaoDM
            // 
            this.gbTaoDM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gbTaoDM.Controls.Add(this.btnXoaDM);
            this.gbTaoDM.Controls.Add(this.btnDong);
            this.gbTaoDM.Controls.Add(this.btnLuuDM);
            this.gbTaoDM.Controls.Add(this.txtMaDM);
            this.gbTaoDM.Controls.Add(this.label2);
            this.gbTaoDM.Controls.Add(this.txtTenDM);
            this.gbTaoDM.Controls.Add(this.label1);
            this.gbTaoDM.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbTaoDM.ForeColor = System.Drawing.Color.Red;
            this.gbTaoDM.Location = new System.Drawing.Point(0, 0);
            this.gbTaoDM.Name = "gbTaoDM";
            this.gbTaoDM.Size = new System.Drawing.Size(277, 337);
            this.gbTaoDM.TabIndex = 0;
            this.gbTaoDM.TabStop = false;
            this.gbTaoDM.Text = "Tạo danh mục";
            // 
            // txtMaDM
            // 
            this.txtMaDM.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMaDM.Location = new System.Drawing.Point(6, 137);
            this.txtMaDM.Name = "txtMaDM";
            this.txtMaDM.Size = new System.Drawing.Size(265, 32);
            this.txtMaDM.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(13, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 26);
            this.label2.TabIndex = 2;
            this.label2.Text = "Mã danh mục:";
            // 
            // txtTenDM
            // 
            this.txtTenDM.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTenDM.Location = new System.Drawing.Point(6, 65);
            this.txtTenDM.Name = "txtTenDM";
            this.txtTenDM.Size = new System.Drawing.Size(265, 32);
            this.txtTenDM.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(13, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(156, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tên danh mục:";
            // 
            // gbDSDM
            // 
            this.gbDSDM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gbDSDM.Controls.Add(this.lvDanhMuc);
            this.gbDSDM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbDSDM.ForeColor = System.Drawing.Color.Red;
            this.gbDSDM.Location = new System.Drawing.Point(277, 0);
            this.gbDSDM.Name = "gbDSDM";
            this.gbDSDM.Size = new System.Drawing.Size(315, 337);
            this.gbDSDM.TabIndex = 1;
            this.gbDSDM.TabStop = false;
            this.gbDSDM.Text = "Danh sách danh mục ";
            // 
            // lvDanhMuc
            // 
            this.lvDanhMuc.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.MaDM,
            this.TenDM});
            this.lvDanhMuc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvDanhMuc.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvDanhMuc.FullRowSelect = true;
            this.lvDanhMuc.GridLines = true;
            this.lvDanhMuc.HideSelection = false;
            this.lvDanhMuc.Location = new System.Drawing.Point(3, 32);
            this.lvDanhMuc.Name = "lvDanhMuc";
            this.lvDanhMuc.Size = new System.Drawing.Size(309, 302);
            this.lvDanhMuc.TabIndex = 0;
            this.lvDanhMuc.UseCompatibleStateImageBehavior = false;
            this.lvDanhMuc.View = System.Windows.Forms.View.Details;
            this.lvDanhMuc.SelectedIndexChanged += new System.EventHandler(this.lvDanhMuc_SelectedIndexChanged);
            // 
            // MaDM
            // 
            this.MaDM.Text = "Mã danh mục";
            this.MaDM.Width = 130;
            // 
            // TenDM
            // 
            this.TenDM.Text = "Tên danh mục";
            this.TenDM.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TenDM.Width = 175;
            // 
            // btnXoaDM
            // 
            this.btnXoaDM.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXoaDM.ForeColor = System.Drawing.Color.Black;
            this.btnXoaDM.Image = global::BaiTapLonMonLapTrinhNangCao.Properties.Resources.Trash_empty_icon;
            this.btnXoaDM.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnXoaDM.Location = new System.Drawing.Point(188, 175);
            this.btnXoaDM.Name = "btnXoaDM";
            this.btnXoaDM.Size = new System.Drawing.Size(83, 42);
            this.btnXoaDM.TabIndex = 5;
            this.btnXoaDM.Text = "Xóa DM";
            this.btnXoaDM.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnXoaDM.UseVisualStyleBackColor = true;
            this.btnXoaDM.Click += new System.EventHandler(this.btnXoaDM_Click);
            // 
            // btnDong
            // 
            this.btnDong.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDong.ForeColor = System.Drawing.Color.Black;
            this.btnDong.Image = global::BaiTapLonMonLapTrinhNangCao.Properties.Resources.Go_back_icon;
            this.btnDong.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDong.Location = new System.Drawing.Point(95, 289);
            this.btnDong.Name = "btnDong";
            this.btnDong.Size = new System.Drawing.Size(74, 42);
            this.btnDong.TabIndex = 7;
            this.btnDong.Text = "Trở về";
            this.btnDong.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDong.UseVisualStyleBackColor = true;
            this.btnDong.Click += new System.EventHandler(this.btnDong_Click);
            // 
            // btnLuuDM
            // 
            this.btnLuuDM.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLuuDM.ForeColor = System.Drawing.Color.Black;
            this.btnLuuDM.Image = global::BaiTapLonMonLapTrinhNangCao.Properties.Resources.save_icon;
            this.btnLuuDM.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLuuDM.Location = new System.Drawing.Point(6, 175);
            this.btnLuuDM.Name = "btnLuuDM";
            this.btnLuuDM.Size = new System.Drawing.Size(91, 42);
            this.btnLuuDM.TabIndex = 4;
            this.btnLuuDM.Text = "Lưu DM";
            this.btnLuuDM.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLuuDM.UseVisualStyleBackColor = true;
            this.btnLuuDM.Click += new System.EventHandler(this.btnLuuDM_Click);
            // 
            // frmDanhMuc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 337);
            this.Controls.Add(this.gbDSDM);
            this.Controls.Add(this.gbTaoDM);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.MaximizeBox = false;
            this.Name = "frmDanhMuc";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Danh mục sản phẩm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDanhMuc_FormClosing);
            this.Load += new System.EventHandler(this.frmDanhMuc_Load);
            this.gbTaoDM.ResumeLayout(false);
            this.gbTaoDM.PerformLayout();
            this.gbDSDM.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbTaoDM;
        private System.Windows.Forms.Button btnXoaDM;
        private System.Windows.Forms.Button btnDong;
        private System.Windows.Forms.Button btnLuuDM;
        private System.Windows.Forms.TextBox txtMaDM;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTenDM;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbDSDM;
        private System.Windows.Forms.ListView lvDanhMuc;
        private System.Windows.Forms.ColumnHeader MaDM;
        private System.Windows.Forms.ColumnHeader TenDM;
    }
}