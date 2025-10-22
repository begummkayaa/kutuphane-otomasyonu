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
    public partial class KullaniciEkle : Form
    {
        SqlBaglantisi baglan = new SqlBaglantisi(); //SQl Bağlantısı
        public KullaniciEkle()
        {
            InitializeComponent();
            Listele();
        }

        private void KullaniciEkle_Load(object sender, EventArgs e)
        {
            Listele();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnEkle_Click(object sender, EventArgs e) // Ekle butonu
        {
            try
            {
                // Kullanıcı Tablosundan girilen Tc'yi kontrol eder
                SqlCommand kontrol = new SqlCommand("select count (*) from KullaniciTablosu where KullaniciTc= @b", baglan.baglanti());
                kontrol.Parameters.AddWithValue("@b", txtTc.Text);
                int kayit = (int)kontrol.ExecuteScalar();
                if (kayit > 0) // Kayıt Varsa
                {
                    MessageBox.Show("Bu kullanıcı bulunmaktadır. ", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                 // Kullanıcı Tablosuna kullanıcı ekleme komutu
                SqlCommand Ekle = new SqlCommand("insert into KullaniciTablosu (KullaniciAd,KullaniciSoyad,KullaniciTc,KullaniciSifre) values" +
                    "(@a,@a2,@a3,@a4)", baglan.baglanti());

                Ekle.Parameters.AddWithValue("@a", txtKullaniciAdi.Text);
                Ekle.Parameters.AddWithValue("@a2", txtKullaniciSoyadi.Text);
                Ekle.Parameters.AddWithValue("@a3", txtTc.Text);
                Ekle.Parameters.AddWithValue("@a4", txtSifre.Text);
                Ekle.ExecuteNonQuery();
                baglan.baglanti().Close();
                Listele();
                BilgileriSil();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Listele() // Kullanıcı Tablosunu Listeler
        {
            try
            {
                String komut = " Select * from KullaniciTablosu ";
                SqlDataAdapter da = new SqlDataAdapter(komut, baglan.baglanti());
                DataSet ds = new DataSet();
                da.Fill(ds);
                gridControl1.DataSource = ds.Tables[0];
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
            

        

        public void BilgileriSil() // Araçları temiler
        {
            txtKullaniciAdi.Text = "";
            txtKullaniciSoyadi.Text = "";
            txtTc.Text = "";
            txtSifre.Text = "";
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void KullaniciEkle_Load_1(object sender, EventArgs e)
        {

        }
    }
}
