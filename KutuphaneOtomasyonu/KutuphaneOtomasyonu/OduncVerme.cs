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
    public partial class OduncVerme : Form
    {
        SqlBaglantisi baglan = new SqlBaglantisi(); // SQl Bağlantısı
        DateTime dt = DateTime.Now; //Bugünün tarihini tutar
        public OduncVerme()
        {
            InitializeComponent();
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

        private void OduncVerme_Load(object sender, EventArgs e)
        {
            dateTimePicker1.MinDate = dt; //dateTimePickeri1'i Sınırlar
            dateTimePicker1.MaxDate = dt.AddMonths(1); //dateTimePicker1'i sınırlar
        }

        private void btnEkle_Click(object sender, EventArgs e) // Ekle butonu
        {
            try
            {
                // Kullanıcının aynı kitabı ödünç alıp almadığını ve iade etmediğini kontrol et
                SqlCommand kontrolKitapKomutu = new SqlCommand("SELECT COUNT(*) FROM OduncTablosu WHERE KullaniciTc = @tc AND ISBN = @isbn AND IAdeTarihi IS NULL", baglan.baglanti());
                kontrolKitapKomutu.Parameters.AddWithValue("@tc", txtTc.Text);
                kontrolKitapKomutu.Parameters.AddWithValue("@isbn", txtISBN.Text);
                int oduncAlinmisKitapSayisi = (int)kontrolKitapKomutu.ExecuteScalar();

                if (oduncAlinmisKitapSayisi > 0)
                {
                    MessageBox.Show("Kullanıcıda Aynı Kitaptan Mevcut Ödünç Verilemez.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kullanıcı Tablosunda TC değişkeninie göre veri kontrolü yapan komut 
                SqlCommand kontrolKomutu = new SqlCommand("SELECT COUNT(*) FROM KullaniciTablosu WHERE KullaniciTc = @tc", baglan.baglanti());
                kontrolKomutu.Parameters.AddWithValue("@tc", txtTc.Text);
                int kayitSayisi = (int)kontrolKomutu.ExecuteScalar(); 

                if (kayitSayisi == 0) // Veri yoksa
                {
                    MessageBox.Show("Geçersiz TC kimlik numarası. Lütfen doğru bir TC giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Ödünç sayısını kontrol eden komut
                SqlCommand OduncSınırı = new SqlCommand("Select Count (*) from OduncTablosu where KullaniciTc=@t", baglan.baglanti());
                OduncSınırı.Parameters.AddWithValue("@t", txtTc.Text);
                int OduncSayısı = (int)OduncSınırı.ExecuteScalar();
                if (OduncSayısı > 2) // En fazla 3 kitap alınmasını sağlar
                {
                    MessageBox.Show("3'ten Fazla Kitap Ödünç Verilemez", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SqlCommand stokKontrolKomutu = new SqlCommand("SELECT KutuphaneAdetSayisi FROM KitapTablosu WHERE ISBN = @isbn", baglan.baglanti());
                stokKontrolKomutu.Parameters.AddWithValue("@isbn", txtISBN.Text);
                int kitapSayisi = (int)stokKontrolKomutu.ExecuteScalar();

                // Eğer kitap sayısı 1 ise, rezervasyon tablosunu kontrol et
                if (kitapSayisi == 1)
                {
                    SqlCommand rezervasyonKontrolKomutu = new SqlCommand("SELECT COUNT(*) FROM RezervasyonTablosu WHERE ISBN = @isbn", baglan.baglanti());
                    rezervasyonKontrolKomutu.Parameters.AddWithValue("@isbn", txtISBN.Text);
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
                    iadeTarihKomutu.Parameters.AddWithValue("@isbn", txtISBN.Text);
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
                            rezervasyonKomutu.Parameters.AddWithValue("@tc", txtTc.Text);
                            rezervasyonKomutu.Parameters.AddWithValue("@isbn", txtISBN.Text);
                            rezervasyonKomutu.Parameters.AddWithValue("@kitapad", txtKitapAdi.Text);
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
                komut.Parameters.AddWithValue("@a", txtTc.Text);
                komut.Parameters.AddWithValue("@a2", txtISBN.Text);
                komut.Parameters.AddWithValue("@a3", txtKitapAdi.Text);
                komut.Parameters.AddWithValue("@a4", dt.ToString("yyyy-MM-dd"));
                komut.Parameters.AddWithValue("@a5", dateTimePicker1.Value.ToString("yyyy-MM-dd"));

                komut.ExecuteNonQuery();
                baglan.baglanti().Close();

                // Bulunan Kitap Sayısı Güncelleme
                SqlCommand komut2 = new SqlCommand("UPDATE KitapTablosu SET KutuphaneAdetSayisi = KutuphaneAdetSayisi - 1 WHERE ISBN = @a1", baglan.baglanti());
                komut2.Parameters.AddWithValue("@a1", txtISBN.Text);
                komut2.ExecuteNonQuery();
                baglan.baglanti().Close();

                //Ödünç sayısını artırma 
                SqlCommand oduncSayisiGuncelle = new SqlCommand("UPDATE KitapTablosu SET OduncSayisi = OduncSayisi + 1 WHERE ISBN = @a1", baglan.baglanti());
                oduncSayisiGuncelle.Parameters.AddWithValue("@a1", txtISBN.Text);
                oduncSayisiGuncelle.ExecuteNonQuery();

                baglan.baglanti().Close();

                MessageBox.Show("Kitap ödünç verildi ve kitap sayısı güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            txtTc.Text = "";
            txtISBN.Text = "";
            txtKitapAdi.Text = "";
        }

        private void simpleButton1_Click(object sender, EventArgs e) // Gridden verileri araçlara taşır
        {
            txtISBN.Text = gridView1.GetFocusedRowCellValue("ISBN").ToString();
            txtKitapAdi.Text = gridView1.GetFocusedRowCellValue("Ad").ToString();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtKitapAdi_TextChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void txtTc_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtISBN_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }
    }
}
