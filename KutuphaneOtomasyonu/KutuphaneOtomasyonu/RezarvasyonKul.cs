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
    public partial class RezarvasyonKul : Form
    {
        SqlBaglantisi baglan = new SqlBaglantisi(); // SQl Bağlantısı
        public string TC; // TC değerini tutar
        public RezarvasyonKul()
        {
            InitializeComponent();
        }

        private void RezarvasyonKul_Load(object sender, EventArgs e)
        {
            Listele();
        }
        public void Listele() // TC değerine göre rezervasyon tablosunu listeler
        {
            String komut = " Select * from RezervasyonTablosu where KullaniciTc=@tc ";
            SqlDataAdapter da = new SqlDataAdapter(komut, baglan.baglanti());
            da.SelectCommand.Parameters.AddWithValue("@tc",TC);

            DataSet ds = new DataSet();
            da.Fill(ds);
            gridControl1.DataSource = ds.Tables[0];
        }

        private void btnSil_Click(object sender, EventArgs e)// Silme butonu
        {
            // Onay İsteme
            DialogResult kabul = MessageBox.Show($"Rezervasyonunuzu Silmek İstediğinize Emin Misiniz?", "Dikkat", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (kabul == DialogResult.Yes) //Onay Verilirse
            {
                // TC değişkenine göre Rezervasyon Tablosundan veri siler 
                SqlCommand rezervasyonSil = new SqlCommand("Delete From RezervasyonTablosu where KullaniciTc = @b", baglan.baglanti());
                rezervasyonSil.Parameters.AddWithValue("@b", TC);
                rezervasyonSil.ExecuteNonQuery();

                MessageBox.Show("Rezervasyonunuz Silinmiştir!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Listele();
            }
        }
    }
}
