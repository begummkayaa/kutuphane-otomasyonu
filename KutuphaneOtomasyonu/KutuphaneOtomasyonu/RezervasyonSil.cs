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
    public partial class RezervasyonSil : Form
    {
        SqlBaglantisi baglan = new SqlBaglantisi(); //SQl Bağlantısı
        public RezervasyonSil()
        {
            InitializeComponent();
        }

        private void RezervasyonSil_Load(object sender, EventArgs e)
        {
            Listele();
        }
        public void Listele() // Rezervasyon Tablosunu Listeler
        {
            String komut = " Select * from RezervasyonTablosu ";
            SqlDataAdapter da = new SqlDataAdapter(komut, baglan.baglanti());
            DataSet ds = new DataSet();
            da.Fill(ds);
            gridControl1.DataSource = ds.Tables[0];
        }

        private void btnSil_Click(object sender, EventArgs e) // Sil Butonu
        {
            
            object tc = gridView1.GetFocusedRowCellValue("KullaniciTc").ToString(); // Gridde seçilen satırın TC değerini tc değişkenine atar
            //Onay isteme
            DialogResult kabul = MessageBox.Show($"{tc} Kimlik Numaralı Kullanıcının Rezervasyonunu Silmek İstediğinize Emin Misiniz?","Dikkat",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
            if (kabul == DialogResult.Yes)// Onay verilirse
            {
                // tc değişkenine göre Rezervasyon Tablosundan veri siler
                SqlCommand rezervasyonSil = new SqlCommand("Delete From RezervasyonTablosu where KullaniciTc = @b", baglan.baglanti());
                rezervasyonSil.Parameters.AddWithValue("@b", tc);
                rezervasyonSil.ExecuteNonQuery();

                MessageBox.Show("Rezervasyon Silinmiştir!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Listele();
            }

        }
    }
}
