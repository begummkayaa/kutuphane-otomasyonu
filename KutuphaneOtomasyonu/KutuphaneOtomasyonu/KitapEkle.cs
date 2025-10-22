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
    public partial class KitapEkle : Form
    {
        SqlBaglantisi baglan = new SqlBaglantisi(); // SQl Bağlantısı
        public KitapEkle()
        {
         
            InitializeComponent();
            cmbKategori.Items.Clear(); // CmbKategoriyi temizler
            cmbKategori.Items.AddRange(new string[] {"Roman","Hikaye","Kurgu Dışı" }); // CmbKategoriye değişken atar

        }

        

        private void btnEkle_Click(object sender, EventArgs e) // Ekle butonu
        {
            try
            {
                // Kitap Tablosundaki ISBN kontrolü
                SqlCommand kontrol = new SqlCommand("select count (*) from KitapTablosu where ISBN = @b", baglan.baglanti());
                kontrol.Parameters.AddWithValue("@b", mskISBN.Text);
                int kayit = (int)kontrol.ExecuteScalar();
                if (kayit > 0) // Aynı ISBN mevcutsa
                {
                    MessageBox.Show("Bu ISBN başka bir kitap mevcuttur. ", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }

                // Kitap Tablosuna ekleme komutu
                SqlCommand Ekle = new SqlCommand("insert into KitapTablosu (ISBN,Ad,Kategori,Tür,Konu,Yazar,Yayinevi,BaskiYili,ToplamAdetSayisi, KutuphaneAdetSayisi) values" +
                    "(@a,@a2,@a3,@a4,@a5,@a6,@a7,@a8,@a9,@a10)", baglan.baglanti());

                Ekle.Parameters.AddWithValue("@a", mskISBN.Text);
                Ekle.Parameters.AddWithValue("@a2", txtAd.Text);
                Ekle.Parameters.AddWithValue("@a3", cmbKategori.Text);
                Ekle.Parameters.AddWithValue("@a4", cmbTur.Text);
                Ekle.Parameters.AddWithValue("@a5", rchKonu.Text);
                Ekle.Parameters.AddWithValue("@a6", txtYazar.Text);
                Ekle.Parameters.AddWithValue("@a7", txtYayinevi.Text);
                Ekle.Parameters.AddWithValue("@a8", mskBaski.Text);
                Ekle.Parameters.AddWithValue("@a9", mskAdet.Text);
                Ekle.Parameters.AddWithValue("@a10", mskAdet.Text);
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

        public void Listele() // Kitap Tablosunu listeler
        {
            String komut = " Select * from KitapTablosu ";
            SqlDataAdapter da = new SqlDataAdapter(komut, baglan.baglanti());
            DataSet ds = new DataSet();
            da.Fill(ds);
            gridControl1.DataSource = ds.Tables[0];
        }

        private void KitapEkle_Load(object sender, EventArgs e)
        {
            Listele();
        }

        private void cmbKategori_SelectedIndexChanged_1(object sender, EventArgs e) //  CmbKategorinin değişmesi
        {
            cmbTur.Items.Clear(); // CmbTürü temizler
            switch (cmbKategori.SelectedItem.ToString()) // CmbKategori değişkenine göre CmbTüre değişken atar
            {
                case "Roman":
                    cmbTur.Items.AddRange(new String[] { "Tarihi Roman", "Bilimkurgu", "Fantastik", "Polisiye", "Dram", "Aşk/Romantik", "Macera", "Psikoloji", "Komedi" });
                    break;

                case "Hikaye":
                    cmbTur.Items.AddRange(new String[] { "Tarihi Hikaye", "Bilimkurgu", "Fantastik", "Polisiye", "Dram", "Aşk/Romantik", "Macera", "Komedi", "Çocuk Hikayeleri" });
                    break;

                case "Kurgu Dışı":
                    cmbTur.Items.AddRange(new String[] { "Biyografi/Otobiyografi", "Anı(Hatırat)", "Gezi Yazıları", "Deneme", "Makale/İnceleme", "Araştırma", "Tarih", "Bilimsel Yayınlar", "Psikoloji", "Sosyoloji", "Felsefe", "Akademik ve Eğitim", "Kişisel Gelişim", "Sanat ve Hobi", "Din ve İnanç", "Eğlence ve Popüler Kültür", "Teknik ve Mesleki Kitaplar" });
                    break;

                default:
                    cmbTur.Items.Add("Tür Bulunamadı");
                    break;
            }
        }


        public void BilgileriSil() // Araçları temizler
        {
            mskISBN.Text = "";
            txtAd.Text = "";
            cmbKategori.Text = "";
            cmbTur.Text="";
            rchKonu.Text = "";
            txtYazar.Text = "";
            txtYayinevi.Text = "";
            mskBaski.Text = "";
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }
    }
}
