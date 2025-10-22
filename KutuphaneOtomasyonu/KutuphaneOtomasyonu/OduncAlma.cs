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
    public partial class OduncAlma : Form
    {
        SqlBaglantisi baglan = new SqlBaglantisi(); // SQl Bağlantısı
        public string TC; // TC değerini tutar
        DateTime dt = DateTime.Now; // Bugünğn tarihini tutar
        public OduncAlma()
        {
            InitializeComponent();
        }

        private void btnOdunc_Click(object sender, EventArgs e) // Ödünç Butonu
        {
            // Kullanıcının aynı kitabı ödünç alıp almadığını ve iade etmediğini kontrol et
            SqlCommand kontrolKitapKomutu = new SqlCommand("SELECT COUNT(*) FROM OduncTablosu WHERE KullaniciTc = @tc AND ISBN = @isbn AND IAdeTarihi IS NULL", baglan.baglanti());
            kontrolKitapKomutu.Parameters.AddWithValue("@tc", TC);
            kontrolKitapKomutu.Parameters.AddWithValue("@isbn",mskIsbn.Text);
            int oduncAlinmisKitapSayisi = (int)kontrolKitapKomutu.ExecuteScalar();

            if (oduncAlinmisKitapSayisi > 0)
            {
                MessageBox.Show("Bu kitabı zaten ödünç aldınız ve henüz iade etmediniz. Aynı kitabı tekrar ödünç alamazsınız.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }




            // Kullanıcının aynı anda en fazla 3 kitap almasını sağlar
            SqlCommand OduncSınırı = new SqlCommand("Select Count (*) from OduncTablosu where IadeTarihi is null and KullaniciTc=@t", baglan.baglanti());
            OduncSınırı.Parameters.AddWithValue("@t", TC);
            int OduncSayısı = (int)OduncSınırı.ExecuteScalar();
            if (OduncSayısı > 2)
            {
                MessageBox.Show("3'ten Fazla Kitap Ödünç Alamazsınız", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Ödünç alınacak kitabın bulunma durumunu kontrol eder
            SqlCommand stokKontrolKomutu = new SqlCommand("SELECT KutuphaneAdetSayisi FROM KitapTablosu WHERE ISBN = @isbn", baglan.baglanti());
            stokKontrolKomutu.Parameters.AddWithValue("@isbn", mskIsbn.Text);
            int kitapSayisi = (int)stokKontrolKomutu.ExecuteScalar();

            // Eğer kitap sayısı 1 ise, rezervasyon tablosunu kontrol et
            if (kitapSayisi == 1)
            {
                SqlCommand rezervasyonKontrolKomutu = new SqlCommand("SELECT COUNT(*) FROM RezervasyonTablosu WHERE ISBN = @isbn", baglan.baglanti());
                rezervasyonKontrolKomutu.Parameters.AddWithValue("@isbn", mskIsbn.Text);
                int rezervasyonSayisi = (int)rezervasyonKontrolKomutu.ExecuteScalar();

                if (rezervasyonSayisi > 0)
                {
                    MessageBox.Show("Bu kitap rezerve edildiği için ödünç verilemez.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (kitapSayisi <= 0)
            {
                // En erken iade tarihini al
                SqlCommand iadeTarihKomutu = new SqlCommand("SELECT MIN(SonIadeTarihi) FROM OduncTablosu WHERE ISBN = @isbn", baglan.baglanti());
                iadeTarihKomutu.Parameters.AddWithValue("@isbn", mskIsbn.Text);
                object iadeTarihiObj = iadeTarihKomutu.ExecuteScalar();

                if (iadeTarihiObj != DBNull.Value)
                {
                    DateTime enErkenIadeTarihi = Convert.ToDateTime(iadeTarihiObj);
                    DialogResult rezervasyonSorusu = MessageBox.Show(
                        $"Bu kitap en erken {enErkenIadeTarihi:yyyy-MM-dd} tarihinde iade edilecektir. Rezervasyon yapmak ister misiniz?",
                        "Rezervasyon",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (rezervasyonSorusu == DialogResult.Yes)
                    {
                        // Rezervasyon kaydı ekleme
                        DateTime rezervasyonTarihi = enErkenIadeTarihi.AddDays(5);
                        SqlCommand rezervasyonKomutu = new SqlCommand("INSERT INTO RezervasyonTablosu (KullaniciTc, KitapAd, ISBN, SonAlisTarihi) VALUES (@tc,@kitapad, @isbn, @rezTarih)", baglan.baglanti());
                        rezervasyonKomutu.Parameters.AddWithValue("@tc", TC);
                        rezervasyonKomutu.Parameters.AddWithValue("@isbn", mskIsbn.Text);
                        rezervasyonKomutu.Parameters.AddWithValue("@kitapad", txtAd.Text);
                        rezervasyonKomutu.Parameters.AddWithValue("@rezTarih", rezervasyonTarihi.ToString("yyyy-MM-dd"));
                        rezervasyonKomutu.ExecuteNonQuery();

                        MessageBox.Show("Rezervasyon başarıyla yapıldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Bu kitap şu anda ödünç alınamaz ve iade tarihi bilgisi bulunmamaktadır.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            // Ödünç Verme
            SqlCommand komut = new SqlCommand("INSERT INTO OduncTablosu (KullaniciTc, ISBN, KitapAd, AlmaTarihi, SonIadeTarihi) VALUES (@a, @a2, @a3, @a4, @a5)", baglan.baglanti());
            komut.Parameters.AddWithValue("@a", TC);
            komut.Parameters.AddWithValue("@a2", mskIsbn.Text);
            komut.Parameters.AddWithValue("@a3", txtAd.Text);
            komut.Parameters.AddWithValue("@a4", dt.ToString("yyyy-MM-dd"));
            komut.Parameters.AddWithValue("@a5", dateTimePicker1.Value.ToString("yyyy-MM-dd"));

            komut.ExecuteNonQuery();
            baglan.baglanti().Close();

            // Bulunan Kitap Sayısı Güncelleme
            SqlCommand komut2 = new SqlCommand("UPDATE KitapTablosu SET KutuphaneAdetSayisi = KutuphaneAdetSayisi - 1 WHERE ISBN = @a1", baglan.baglanti());
            komut2.Parameters.AddWithValue("@a1", mskIsbn.Text);
            komut2.ExecuteNonQuery();
            baglan.baglanti().Close();

            // Ödünç verme sayısını artırma
            
            SqlCommand oduncSayisiGuncelle = new SqlCommand("UPDATE KitapTablosu SET OduncSayısı = OduncSayısı + 1 WHERE ISBN = @a1", baglan.baglanti());
            oduncSayisiGuncelle.Parameters.AddWithValue("@a1", mskIsbn.Text);
            oduncSayisiGuncelle.ExecuteNonQuery();

            baglan.baglanti().Close();

            MessageBox.Show("Kitap ödünç verildi ve kitap sayısı güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Listele();
            BilgileriSil();

            AynıYazar ay = new AynıYazar();
            ay.Yazar = txtYazar.Text;
            ay.Show();
        }

        private void OduncAlma_Load(object sender, EventArgs e)
        {
            dateTimePicker1.MinDate = dt; // DdateTimePickeri1'i Sınırlar
            dateTimePicker1.MaxDate = dt.AddMonths(1); //dateTimePickeri1'i Sınırlar
            Listele();
        }
        public void Listele() // Kitap Tablosunu Listeler
        {
            String komut = " Select * from KitapTablosu ";
            SqlDataAdapter da = new SqlDataAdapter(komut, baglan.baglanti());
            DataSet ds = new DataSet();
            da.Fill(ds);
            gridControl1.DataSource = ds.Tables[0];
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            Arama(txtArama.Text);
        }

        public void Arama(string Ad) // Girilen veriye göre Kitap Tablosunda arama yapar
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

        private void btnTası_Click(object sender, EventArgs e) //Gridden verileri araçlara taşır
        {
            mskIsbn.Text = gridView1.GetFocusedRowCellValue("ISBN").ToString();
            txtAd.Text = gridView1.GetFocusedRowCellValue("Ad").ToString();
            txtYazar.Text= gridView1.GetFocusedRowCellValue("Yazar").ToString();
        }

        public void BilgileriSil() // Araçtaki verileri siler
        {
            mskIsbn.Text = "";
            txtAd.Text = "";
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
