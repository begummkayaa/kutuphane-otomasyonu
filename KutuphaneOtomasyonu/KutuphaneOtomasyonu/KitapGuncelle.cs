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
    public partial class KitapGuncelle : Form
    {
        SqlBaglantisi baglan = new SqlBaglantisi(); // SQl Bağlantısı
        public KitapGuncelle()
        {
            InitializeComponent();
            Listele();
        }

        private void btnTasi_Click(object sender, EventArgs e) // Gridden verileri araçlara taşır
        {
            mskISBN.Text = gridView1.GetFocusedRowCellValue("ISBN").ToString();
            txtAd.Text = gridView1.GetFocusedRowCellValue("Ad").ToString();
            cmbKategori.Text = gridView1.GetFocusedRowCellValue("Kategori").ToString();
            cmbTur.Text = gridView1.GetFocusedRowCellValue("Tür").ToString();
            rchKonu.Text = gridView1.GetFocusedRowCellValue("Konu").ToString();
            txtYazar.Text = gridView1.GetFocusedRowCellValue("Yazar").ToString();
            txtYayinevi.Text = gridView1.GetFocusedRowCellValue("Yayinevi").ToString();
            mskBaski.Text= gridView1.GetFocusedRowCellValue("BaskiYili").ToString();
        }
        public void Listele() // Kitap Tablosunu listeler
        {
            String komut = " Select * from KitapTablosu ";
            SqlDataAdapter da = new SqlDataAdapter(komut, baglan.baglanti());
            DataSet ds = new DataSet();
            da.Fill(ds);
            gridControl1.DataSource = ds.Tables[0];
        }

        private void btnGuncelle_Click(object sender, EventArgs e) // Güncelle butonu
        {
            try
            {
                // Kitap Tablosunu güncelleme komutu
                SqlCommand guncelle = new SqlCommand("Update KitapTablosu set Ad=@d3,Kategori=@d4,Tür=@d5,Konu=@d6," +
                    "Yazar=@d7,Yayinevi=@d8,BaskiYili=@d9 where ISBN=@d2", baglan.baglanti());
                guncelle.Parameters.AddWithValue("@d2", mskISBN.Text);
                guncelle.Parameters.AddWithValue("@d3", txtAd.Text);
                guncelle.Parameters.AddWithValue("@d4", cmbKategori.Text);
                guncelle.Parameters.AddWithValue("@d5", cmbTur.Text);
                guncelle.Parameters.AddWithValue("@d6", rchKonu.Text);
                guncelle.Parameters.AddWithValue("@d7", txtYazar.Text);
                guncelle.Parameters.AddWithValue("@d8", txtYayinevi.Text);
                guncelle.Parameters.AddWithValue("@d9", mskBaski.Text);
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
        public void BilgileriSil() // Araçları temizler
        {
            mskISBN.Text = "";
            txtAd.Text = "";
            cmbKategori.Text = "";
            cmbTur.Text = "";
            rchKonu.Text = "";
            txtYazar.Text = "";
            txtYayinevi.Text = "";
            mskBaski.Text = "";
        }

        private void KitapGuncelle_Load(object sender, EventArgs e)
        {
            mskISBN.ReadOnly = true; // mskISBN sadece okunur
        }
    }
}
