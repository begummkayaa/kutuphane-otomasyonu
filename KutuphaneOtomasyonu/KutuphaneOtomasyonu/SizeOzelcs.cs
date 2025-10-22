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
    public partial class SizeOzelcs : Form
    {
        SqlBaglantisi baglan = new SqlBaglantisi(); // SQl Bağlantısı
        public string TC; // Tc'yi tutar

        public SizeOzelcs()
        {
            InitializeComponent();
        }

        private void SizeOzelcs_Load(object sender, EventArgs e)
        {
            try
            {
                // Son alınan kitabı bulma sorgusu
                string querySonKitap = @"SELECT TOP 1 k.Ad, k.Kategori, k.Tür, k.Yazar
                         FROM KitapTablosu k
                         INNER JOIN OduncTablosu o ON k.ISBN = o.ISBN
                         WHERE o.KullaniciTc = @KullaniciTc
                           AND o.IAdeTarihi IS NOT NULL 
                         ORDER BY o.IAdeTarihi DESC"; 

              SqlCommand komut = new SqlCommand(querySonKitap, baglan.baglanti());
                komut.Parameters.AddWithValue("@KullaniciTc", TC);

                SqlDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    // Son kitap bilgilerini doldur
                    txtAd.Text = dr["Ad"].ToString();
                    txtKategori.Text = dr["Kategori"].ToString();
                    txtTur.Text = dr["Tür"].ToString();
                    txtYazar.Text = dr["Yazar"].ToString();
                }
                else
                {
                    MessageBox.Show("Daha Önce Aldığınız Kitap Bulunmamaktadır.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                dr.Close(); // DataReader'i kapat

                // Benzer kitapları sorgula
                string queryBenzerKategori = "Select * from kitapTablosu where Kategori = @Kategori";
                                       

                SqlCommand cmd = new SqlCommand(queryBenzerKategori, baglan.baglanti());
                cmd.Parameters.AddWithValue("@KullaniciTc", TC);
                cmd.Parameters.AddWithValue("@Kategori", txtKategori.Text); // txtKategori'den alınan güncel değer

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                gridControl1.DataSource = dt;

                // Benzer Tür Sorgulama
                string queryBenzerTur = "Select * From KitapTablosu where Tür=@KitapTur";
                SqlCommand cmd2 = new SqlCommand(queryBenzerTur,baglan.baglanti());
                cmd2.Parameters.AddWithValue("Kullanıcı@Tc", TC);
                cmd2.Parameters.AddWithValue("@KitapTur",txtTur.Text);

                SqlDataAdapter adapter2 = new SqlDataAdapter(cmd2);
                DataTable dt2 = new DataTable();
                adapter2.Fill(dt2);
                gridControl2.DataSource = dt2;


            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtAd_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
