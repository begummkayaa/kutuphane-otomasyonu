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
    public partial class IadeAlma : Form
    {
        SqlBaglantisi baglan = new SqlBaglantisi(); // SQl bağlantısı
        DateTime dt = DateTime.Now; // Bugünün tarihini tutar
        public IadeAlma()
        {
            InitializeComponent();
            
        }

        private void IadeAlma_Load(object sender, EventArgs e)
        {
            Listele(); 
            txtISBN.ReadOnly = true; // TxtISBN sadece okunabilir 

            txtKitapAdi.ReadOnly = true; // txtKitapAdi sadece okunabilir
        }

        public void Listele() // Ödünç Tablosunu listeler
        {
            String komut = " Select * from OduncTablosu where IadeTarihi=null ";
            SqlDataAdapter da = new SqlDataAdapter(komut, baglan.baglanti());
            DataSet ds = new DataSet();
            da.Fill(ds);
            gridControl1.DataSource = ds.Tables[0];
        }

        private void btnIade_Click(object sender, EventArgs e) // İade etme butonu
        {
            try
            {
                // Ödünç Tablosunu güncelleme komutu
                SqlCommand komut = new SqlCommand("Update OduncTablosu set IadeTarihi = @a1 where KullaniciTc = @a2", baglan.baglanti());
                komut.Parameters.AddWithValue("@a1", dt.ToString("yyyy-MM-dd HH:mm:ss"));
                komut.Parameters.AddWithValue("@a2", txtTc.Text);
                int degisiklik = komut.ExecuteNonQuery();

                if (degisiklik > 0) // Ödünç Tablosu güncellenirse
                {
                    // Kütüpühane adet sayısını güncelleme komutu
                    SqlCommand stok = new SqlCommand("Update KitapTablosu set KutuphaneAdetSayisi = KutuphaneAdetSayisi +1 where ISBN =@b1", baglan.baglanti());
                    stok.Parameters.AddWithValue("@b1", txtISBN.Text);
                    stok.ExecuteNonQuery();
                    MessageBox.Show("Kitap İade Alındı", "Kütüphane Otomasyonu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Listele();
                }
                else
                {
                    MessageBox.Show("İade Hatası", "Kütüphane Otomasyonu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        public void BilgileriSil() // Araçları temizler
        {
            txtTc.Text = "";
            txtISBN.Text = "";
            txtKitapAdi.Text = "";
        }

        private void btnTası_Click(object sender, EventArgs e) // Gridden bilgileri araçlara taşır
        {
            txtTc.Text = gridView1.GetFocusedRowCellValue("KullaniciTc").ToString();
            txtISBN.Text = gridView1.GetFocusedRowCellValue("ISBN").ToString();
            txtKitapAdi.Text = gridView1.GetFocusedRowCellValue("KitapAd").ToString();
        }
    }
}
