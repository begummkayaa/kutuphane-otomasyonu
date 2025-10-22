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
    public partial class YoneticiGiris : Form
    {
        SqlBaglantisi baglan = new SqlBaglantisi(); //SQl Bağlantısı
        public YoneticiGiris()
        {
            InitializeComponent();
        }

        private void btngiris_Click(object sender, EventArgs e) // Giriş butonu
        {
            try
            {
                // Araçtaki verilere göre Yönetici Tablosundan veri seçer
                SqlCommand komut = new SqlCommand("Select * from YoneticiTablosu where YoneticiTc=@b and YoneticiSifre=@b2", baglan.baglanti());
                komut.Parameters.AddWithValue("@b", mskTc.Text);
                komut.Parameters.AddWithValue("@b2", mskSifre.Text);
                SqlDataReader dr = komut.ExecuteReader(); // Veriyi okur
                if (dr.Read()) // Veri varsa
                {
                    MessageBox.Show("Giriş Başarılı", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    YoneticiEkrani ye = new YoneticiEkrani();
                    ye.Show();// Yönetici Ekranını açar
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

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) // Kullanıcı Giriş ekranını açar
        {
            KullaniciGiris kg = new KullaniciGiris();
            kg.Show();
            this.Hide();
        }

        private void YoneticiGiris_Load(object sender, EventArgs e)
        {

        }
    }
}
