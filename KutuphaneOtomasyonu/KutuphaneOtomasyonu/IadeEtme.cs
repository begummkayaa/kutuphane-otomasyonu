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
    public partial class IadeEtme : Form
    {
        SqlBaglantisi baglan = new SqlBaglantisi(); //SQl Bağlantısı
        public string TC; // Tc değişkeni saklanır
        DateTime dt = DateTime.Now; // Bugünün tarihini tutar
        public IadeEtme()
        {
            InitializeComponent();
        }

        private void IadeEtme_Load(object sender, EventArgs e)
        {
            Listele();
        }
        public void Listele() // İade tarihi boş olan Tc'ye ait ödünç tablosunu listeler
        {
            String komut = " Select * from OduncTablosu where IadeTarihi is null and KullaniciTc=@tc ";
            SqlDataAdapter da = new SqlDataAdapter(komut, baglan.baglanti());
            da.SelectCommand.Parameters.AddWithValue("@tc", TC);
            DataSet ds = new DataSet();
            da.Fill(ds);
            gridControl1.DataSource = ds.Tables[0];
        }

        private void btnIade_Click(object sender, EventArgs e) // İade butonu
        {
            try
            {
                // Puanın 1 ile 5 arasında olmasının kontrolü
                if (Convert.ToInt16(mskPuan.Text) > 5 || Convert.ToInt16(mskPuan.Text) < 1)
                {
                    MessageBox.Show("Kitap Puanı 1 ile 5 Arasında Olmalıdır!", "Uyarı!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Ödünç Tablosunu günceller
                SqlCommand komut = new SqlCommand("Update OduncTablosu set IadeTarihi = @a1 where KullaniciTc = @a2 and ISBN=@a3", baglan.baglanti());
                komut.Parameters.AddWithValue("@a1", dt.ToString("yyyy-MM-dd HH:mm:ss"));
                komut.Parameters.AddWithValue("@a2", TC);
                komut.Parameters.AddWithValue("@a3", txtISBN.Text);
                int degisiklik = komut.ExecuteNonQuery();

                if (degisiklik > 0) // Ödünç Tablosunda değişikliği kontrol eder
                {
                    // Kütüüphane adet sayısını artıran komut
                    SqlCommand stok = new SqlCommand("Update KitapTablosu set KutuphaneAdetSayisi = KutuphaneAdetSayisi +1 where ISBN = @b1", baglan.baglanti());
                    stok.Parameters.AddWithValue("@b1", txtISBN.Text);
                    stok.ExecuteNonQuery();
                    MessageBox.Show("Kitap İade Alındı", "Kütüphane Otomasyonu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Listele();
                }
                else
                {
                    MessageBox.Show("İade Hatası", "Kütüphane Otomasyonu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Yorum Tablosuna ekleme yapar
                SqlCommand Yorum = new SqlCommand("insert into YorumTablosu (ISBN,KitapAdi,Puan,Yorum) values (@y1,@y2,@y3,@y4)", baglan.baglanti());
                Yorum.Parameters.AddWithValue("@y1", txtISBN.Text);
                Yorum.Parameters.AddWithValue("@y2", txtKitapAdi.Text);
                Yorum.Parameters.AddWithValue("@y3", mskPuan.Text);
                Yorum.Parameters.AddWithValue("@y4", rchYorum.Text);
                Yorum.ExecuteNonQuery();
                MessageBox.Show("Puan ve Yorumunuz İçin Teşekkürler", "Kütüphane Otomasyonu", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                // Kullanıcıya tavsiye edip etmeyeceğini sorma
                DialogResult sonuc = MessageBox.Show("Bu kitabı tavsiye eder misiniz?", "Tavsiye", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (sonuc == DialogResult.Yes)
                {
                    // Tavsiye edilme sayısını artır
                    SqlCommand tavsiyeKomutu = new SqlCommand("UPDATE KitapTablosu SET TavsiyeEdilmeSayısı = TavsiyeEdilmeSayısı + 1 WHERE ISBN = @isbn", baglan.baglanti());
                    tavsiyeKomutu.Parameters.AddWithValue("@isbn", txtISBN.Text);
                    tavsiyeKomutu.ExecuteNonQuery();



                    MessageBox.Show("Tavsiyeniz için teşekkür ederiz!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Tavsiye edilme oranını güncelle
                SqlCommand oranGuncelleKomutu = new SqlCommand(
               @"UPDATE KitapTablosu 
                        SET TavsiyeOranı = (TavsiyeEdilmeSayısı * 100.0 / OduncSayısı)
                    WHERE ISBN = @isbn", baglan.baglanti());

                oranGuncelleKomutu.Parameters.AddWithValue("@isbn", txtISBN.Text);
                oranGuncelleKomutu.ExecuteNonQuery();

                Listele();
                BilgileriSil();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: Yorum Dışında Tüm Alanları Doldurunuz ","Dikkat",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }



        }

        private void btnTası_Click(object sender, EventArgs e) // Gridden verileri araçlara taşır
        {
            txtISBN.Text = gridView1.GetFocusedRowCellValue("ISBN").ToString();
            txtKitapAdi.Text = gridView1.GetFocusedRowCellValue("KitapAd").ToString();
        }

        public void BilgileriSil() // Araçları temizler
        {
            mskPuan.Text = "";
            rchYorum.Text = "";
            txtISBN.Text = "";
            txtKitapAdi.Text = "";
        }
    }
}
