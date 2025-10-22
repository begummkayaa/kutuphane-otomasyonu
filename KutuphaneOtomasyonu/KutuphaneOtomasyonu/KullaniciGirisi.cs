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
    public partial class KullaniciGiris : Form
    {
        SqlBaglantisi baglan = new SqlBaglantisi(); // SQl Bağlantısı
        public KullaniciGiris()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btngiris_Click(object sender, EventArgs e) // Giriş butonu
        {
            try
            {
                // Araçtaki verilerden Kullanıcı Tablosundan veri seçer
                SqlCommand komut = new SqlCommand("Select * from KullaniciTablosu where KullaniciTc=@b and KullaniciSifre=@b2", baglan.baglanti());
                komut.Parameters.AddWithValue("@b", mskTc.Text);
                komut.Parameters.AddWithValue("@b2", mskSifre.Text);
                SqlDataReader dr = komut.ExecuteReader(); // Seçilen veri okunur
                if (dr.Read()) // Eğer veri varsa
                {
                    MessageBox.Show("Giriş Başarılı", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    KullaniciEkrani ke = new KullaniciEkrani();
                    ke.TC = mskTc.Text;
                    ke.Show(); // Kullanıcı Ekranını açar 
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Yanlış Tc veya Şifre", "Dikkat", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                baglan.baglanti().Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) // Yönetici Giriş formunu açar
        {
            YoneticiGiris yg = new YoneticiGiris();
            yg.Show();
            this.Hide();
        }
    }
}
