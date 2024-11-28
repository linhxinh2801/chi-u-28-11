using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace BT27._11
{
    public partial class FrmDanhSachDVKS : Form
    {
        public FrmDanhSachDVKS()
        {
            InitializeComponent();
        }
        private DataTable loaidvTable;
        private void FrmDanhSachDVKS_Load(object sender, EventArgs e)
        {
            this.LoadDichVu();
        }
        private void LoadDichVu()
        { 
            string connectionString = "server=PC334\\SQLEXPRESS; database = QuanLyDichVuKhachSan; Integrated Security = true;";
            SqlConnection conn = new SqlConnection(connectionString);

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID_DichVu, ID_LoaiDV, TenDV, DonGia, GhiChu, Enable FROM dbo.DichVu";

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            conn.Open();

            adapter.Fill(dt);

            conn.Close();
            conn.Dispose();

            CbbLoaiDV.DataSource = dt;
            CbbLoaiDV.DisplayMember = "TenDV";
            CbbLoaiDV.ValueMember = "ID_LoaiDV";

        }
        private void CbbLoaiDV_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CbbLoaiDV.SelectedIndex == -1) return;

            string connectionString = "server=PC334\\SQLEXPRESS; database = QuanLyDichVuKhachSan; Integrated Security = true; ";
            SqlConnection conn = new SqlConnection(connectionString);

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM LoaiDV WHERE ID_LoaiDV = @id";

            // Truyền tham số
            cmd.Parameters.Add("@od", SqlDbType.Int);

            if (CbbLoaiDV.SelectedValue is DataRowView)
            {
                DataRowView rowView = CbbLoaiDV.SelectedValue as DataRowView;
                cmd.Parameters["@id"].Value = rowView["ID_LoaiDV"];
            }
            else
            {
                cmd.Parameters["@id"].Value = CbbLoaiDV.SelectedValue;
            }
            // Tạo bộ điều phiếu dữ liệu
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            loaidvTable = new DataTable();

            // Mở kết nối
            conn.Open();

            // Lấy dữ liệu từ csdl đưa vào DataTable
            adapter.Fill(loaidvTable);

            // Đóng kết nối và giải phóng bộ nhớ
            conn.Close();
            conn.Dispose();

            // Đưa dữ liệu vào data gridview
            dgvDanhSachDichVu.DataSource = loaidvTable;

            // Tính số lượng mẫu tin
            lblTongDichVu.Text = loaidvTable.Rows.Count.ToString();
            lblLoaiDV.Text = CbbLoaiDV.Text;
        }
    }
}
