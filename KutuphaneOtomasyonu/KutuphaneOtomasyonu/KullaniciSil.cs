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
    public partial class KullaniciSil : Form
    {
        SqlBaglantisi baglan = new SqlBaglantisi(); //SQl Bağlantısı
        public KullaniciSil()
        {
            InitializeComponent();
            Listele();
        }

        private void KullaniciSil_Load(object sender, EventArgs e)
        {

        }
        public void Listele() // Kullanıcı Tablosunu Listeler
        {
            String komut = " Select * from KullaniciTablosu ";
            SqlDataAdapter da = new SqlDataAdapter(komut, baglan.baglanti());
            DataSet ds = new DataSet();
            da.Fill(ds);
            gridControl1.DataSource = ds.Tables[0];
        }

        private void simpleButton1_Click_1(object sender, EventArgs e) // Silme butonu
        {
            try
            {
                //Gridden seçilen satırın Tc değerini KullaniciTc değişkenine Atar
                string KullaniciTc = gridView1.GetFocusedRowCellValue("KullaniciTc").ToString();

                // Onay isteme
                DialogResult onay = MessageBox.Show($" {KullaniciTc} Kimlik Numaralı Kullanıcıyı Kalıcı Olarak Silmek İstediğinize Emin Misiniz? ", "Dikkat", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (onay == DialogResult.Yes) // Onay verilirse
                {
                    txtAd.Text = gridView1.GetFocusedRowCellValue("KullaniciAd").ToString();
                    txtSoyad.Text = gridView1.GetFocusedRowCellValue("KullaniciSoyad").ToString();
                    txtTc.Text = gridView1.GetFocusedRowCellValue("KullaniciTc").ToString();
                    txtSifre.Text = gridView1.GetFocusedRowCellValue("KullaniciSifre").ToString();


                    // Kullanıcı TC'sine Göre Kullanıcı Tablosundan veri siler
                    SqlCommand sil = new SqlCommand("delete from KullaniciTablosu where KullaniciTc = @c", baglan.baglanti());
                    sil.Parameters.AddWithValue("@c", KullaniciTc);
                    sil.ExecuteNonQuery();
                    baglan.baglanti().Close();
                    Listele();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void KullaniciSil_Load_1(object sender, EventArgs e)
        {

        }

        
    }
}
