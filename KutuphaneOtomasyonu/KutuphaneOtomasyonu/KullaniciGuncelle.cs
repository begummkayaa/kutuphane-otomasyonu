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
    public partial class KullaniciGuncelle : Form
    {
        SqlBaglantisi baglan = new SqlBaglantisi(); // SQl Bağlantısı
        public KullaniciGuncelle()
        {
            InitializeComponent();
            Listele();
        }

        private void btnTasi_Click(object sender, EventArgs e) // Gridden verileri araçlara taşır
        {
            txtAd.Text = gridView1.GetFocusedRowCellValue("KullaniciAd").ToString();
            txtSoyad.Text = gridView1.GetFocusedRowCellValue("KullaniciSoyad").ToString();
            txtTc.Text = gridView1.GetFocusedRowCellValue("KullaniciTc").ToString();
            txtSifre.Text = gridView1.GetFocusedRowCellValue("KullaniciSifre").ToString();
        }
        public void Listele() // Kullanıcı Tablosunu Listeler
        {
            String komut = " Select * from KullaniciTablosu ";
            SqlDataAdapter da = new SqlDataAdapter(komut, baglan.baglanti());
            DataSet ds = new DataSet();
            da.Fill(ds);
            gridControl1.DataSource = ds.Tables[0];
        }

        private void btnGuncelle_Click(object sender, EventArgs e) // Güncelleme Butonu
        {
            try
            {
                // TC değişkenini kontrol etme komutu
                SqlCommand kontrol = new SqlCommand("Select * From KullaniciTablosu where KullaniciTc=@tc", baglan.baglanti());
                kontrol.Parameters.AddWithValue("@tc", txtTc.Text);
                int sayım = (int)kontrol.ExecuteScalar();
                if (sayım > 0)
                {
                    MessageBox.Show("Bu TC Başka Kullanıcıya Aittir", "Dikkat", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }



                // Kullanıcı Tablosunu güncelleme komutu
                SqlCommand guncelle = new SqlCommand("Update KullaniciTablosu set KullaniciSoyad=@d3,KullaniciTc=@d4,KullaniciSifre=@d5 where KullaniciAd=@d2", baglan.baglanti());
                guncelle.Parameters.AddWithValue("@d2", txtAd.Text);
                guncelle.Parameters.AddWithValue("@d3", txtSoyad.Text);
                guncelle.Parameters.AddWithValue("@d4", txtTc.Text);
                guncelle.Parameters.AddWithValue("@d5", txtSifre.Text);
                guncelle.ExecuteNonQuery();
                baglan.baglanti().Close();
                Listele();
                BilgileriSil();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void BilgileriSil() // Araçları Temizler
        {
            txtAd.Text = "";
            txtSoyad.Text = "";
            txtTc.Text = "";
            txtSifre.Text = "";
        }

        private void KullaniciGuncelle_Load(object sender, EventArgs e)
        {

        }
    }
}
