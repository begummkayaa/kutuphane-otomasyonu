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
    
    public partial class ButunKitaplar : Form
    {
        SqlBaglantisi baglan = new SqlBaglantisi(); // SQl bağlantısı
        public ButunKitaplar()
        {
            InitializeComponent();
            cmbKategori.Items.Clear(); // CmbKategoriyi temizler
            cmbKategori.Items.AddRange(new string[] { "Roman", "Hikaye", "Kurgu Dışı" }); //CmbKategoriye değişken atama
        }

        private void ButunKitaplar_Load(object sender, EventArgs e)
        {
            Listele();
        }
        public void Listele() // Kitap Tablosunu listeler
        {
            String komut = " Select * from KitapTablosu ";
            SqlDataAdapter da = new SqlDataAdapter(komut, baglan.baglanti());
            DataSet ds = new DataSet();
            da.Fill(ds);
            gridControl1.DataSource = ds.Tables[0];
        }

        

        public void Arama(string Ad) // Girilen değişkene göre Kitap Tablosunu listeler
        {
            string komut = "SELECT * FROM KitapTablosu WHERE Ad = @p1";
            using (SqlDataAdapter da = new SqlDataAdapter(komut, baglan.baglanti()))
            {
                da.SelectCommand.Parameters.AddWithValue("@p1", Ad);

                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    gridControl1.DataSource = ds.Tables[0];
                }
                else
                {
                    MessageBox.Show("Aradığınız Kitap Bulunamadı", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void cmbKategori_SelectedIndexChanged(object sender, EventArgs e) // CmbKategorinin değişmesi
        {
            cmbTur.Items.Clear(); // CmbTür temizlenir
            switch (cmbKategori.SelectedItem.ToString()) // CmbKategori değişkenine göre CmbTüre değer atanır
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

        private void btnFiltrele_Click(object sender, EventArgs e) // Filtrele butonu
        {
            Filtrele();
        }

        public void Filtrele() // CmbKategori ve CmbTür değişkenlerine göre Kitap Tablosunu listeler
        {
            try
            {
                string komut = "SELECT * FROM KitapTablosu WHERE Kategori = @p1 and Tür=@p2 ";
                using (SqlDataAdapter da = new SqlDataAdapter(komut, baglan.baglanti()))
                {
                    da.SelectCommand.Parameters.AddWithValue("@p1", cmbKategori.Text);
                    da.SelectCommand.Parameters.AddWithValue("@p2", cmbTur.Text);

                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        gridControl1.DataSource = ds.Tables[0];
                    }
                    else
                    {
                        MessageBox.Show("Aradığınız Kriterde Kitap Bulunamadı", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Hata: " + ex, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAra_Click_1(object sender, EventArgs e) // Arama butonu
        { 
            Arama(txtIsım.Text);
        }

        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
