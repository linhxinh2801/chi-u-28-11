using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace BT27._11
{
    public partial class frmXoaCapNhatDV : Form
    {
        private void frmXoaCapNhatDV_Load(object sender, EventArgs e)
        {
            this.Invalidate();
        }
        private void InitValues()
        {
            string connectionString = "server=PC334\\SQLEXPRESS; database = QuanLyDichVuKhachSan; Integrated Security = true;";
            SqlConnection conn = new SqlConnection(connectionString);

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID_DichVu, ID_LoaiDV, TenDV, DonGia, GhiChu, Enable FROM dbo.DichVu";

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            // mở kết nối
            conn.Open();

            // Lấy dữ liệu từ csdl đưa vào DataTable
            adapter.Fill(ds, "LoaiDV");

            // Hiển thị nhóm món ăn
            cbbLoaiDV.DataSource = ds.Tables["LoaiDV"];
            cbbLoaiDV.DisplayMember = "TenLoai";
            cbbLoaiDV.ValueMember = "ID_LoaiDV";

            // đóng kết nối và giải phóng bộ nhớ
            conn.Close();
            conn.Dispose();
        }
        public override void ResetText()
        {
            txtMaDV.Text = "";
            txtTenMatHang.ResetText();
            cbbLoaiDV.ResetText();
            txtDonGia.ResetText();
            txtHinh.Text = "";
            txtGhiChu.ResetText();

        }

        public void DisplayDichVuInfo(DataRowView rowView)
        {
            try
            {
                txtMaDV.Text = rowView["ID_LoaiDV"].ToString();
                txtTenMatHang.Text = rowView["TenDV"].ToString();

                cbbLoaiDV.SelectedIndex = -1;

                // chọn nhóm món ăn tương ứng
                for (int index = 0; index < cbbLoaiDV.Items.Count; index++)
                {
                    DataRowView cat = cbbLoaiDV.Items[index] as DataRowView;
                    if (cat["ID_LoaiDV"].ToString() == rowView["DichVu"].ToString())
                    {
                        cbbLoaiDV.SelectedIndex = index;
                        break;
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
                this.Close();
            }
        }
        private void btnXoa_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnCapnhat_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = "server=PC334\\SQLEXPRESS; database = QuanLyDichVuKhachSan; Integrated Security = true;";
                SqlConnection conn = new SqlConnection(connectionString);

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "EXECUTE UpdateDV @id, @ten";

                cmd.Parameters.Add("@id", SqlDbType.NVarChar, 100);
                cmd.Parameters.Add("@ten", SqlDbType.NVarChar, 1000);

                
                cmd.Parameters["@id"].Value = txtMaDV.Text;
                cmd.Parameters["@ten"].Value = txtTenMatHang.Text;

                //mở kết nối.
                conn.Open();

                int numRowAffected = cmd.ExecuteNonQuery();

                // Thông báo kết quả
                if (numRowAffected > 0)
                {
                    MessageBox.Show("Successfully updating food", "Message");

                    this.ResetText();
                }
                else
                {
                    MessageBox.Show("Updating food failed");
                }

                // đóng kết nối
                conn.Close();
                conn.Dispose();
            }
            catch (SqlException exception)
            {
                MessageBox.Show(exception.Message, "SQL Error");

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error");
            }
        }
    }
}


