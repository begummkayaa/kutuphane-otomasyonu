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

namespace KutuphaneOtomasyonu
{
    public partial class KullaniciListele : Form
    {
        SqlBaglantisi baglan = new SqlBaglantisi(); // SQl Bağlantısı
        public KullaniciListele()
        {
            InitializeComponent();
            Listele();
        }

        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        public void Listele() // Kullanıcı Tablosunu Lİsteler
        {
            String komut = " Select * from KullaniciTablosu ";
            SqlDataAdapter da = new SqlDataAdapter(komut, baglan.baglanti());
            DataSet ds = new DataSet();
            da.Fill(ds);
            gridControl1.DataSource = ds.Tables[0];
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void btnAra_Click(object sender, EventArgs e) // Arama butonu
        {
            Arama(mskTc.Text);
        }

        public void Arama(string Tc) // Girilen TC değerine göre Kullanıcı Tablosunda arama yapar
        {
            string komut = "SELECT * FROM KullaniciTablosu WHERE KullaniciTc = @p1";
            using (SqlDataAdapter da = new SqlDataAdapter(komut, baglan.baglanti()))
            {
                da.SelectCommand.Parameters.AddWithValue("@p1", Tc);

                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    gridControl1.DataSource = ds.Tables[0];
                }
                else
                {
                    MessageBox.Show("Bu TC Numarasına Ait Kullanıcı Bulunamadı", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
