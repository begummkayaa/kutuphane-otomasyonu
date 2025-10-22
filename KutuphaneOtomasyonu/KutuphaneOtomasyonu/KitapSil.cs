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
    public partial class KitapSil : Form
    {
        SqlBaglantisi baglan = new SqlBaglantisi(); // SQl bağlantısı
        public KitapSil() 
        {
            InitializeComponent();
            Listele();
        }

        private void KitapSil_Load(object sender, EventArgs e)
        {
            
        }
        public void Listele() // Kitap Tablosunu listeler
        {
            String komut = " Select * from KitapTablosu ";
            SqlDataAdapter da = new SqlDataAdapter(komut, baglan.baglanti());
            DataSet ds = new DataSet();
            da.Fill(ds);
            gridControl1.DataSource = ds.Tables[0];
        }

        private void simpleButton1_Click(object sender, EventArgs e) // Silme Butonu
        {
            string ISBN = gridView1.GetFocusedRowCellValue("ISBN").ToString(); // Seçilen satırda ISBN değerini ISBN değişkenine atar

            // Onay İsteme 
            DialogResult onay = MessageBox.Show($"ISBN Numarası {ISBN} Olan Kitabı Kalıcı Olarak Silmek İstediğinize Emin Misiniz? ", "Dikkat", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (onay == DialogResult.Yes) // Onay Verirse
            {
                mskISBN.Text = gridView1.GetFocusedRowCellValue("ISBN").ToString();
                txtAd.Text = gridView1.GetFocusedRowCellValue("Ad").ToString();
                cmbKategori.Text = gridView1.GetFocusedRowCellValue("Kategori").ToString();
                cmbTur.Text = gridView1.GetFocusedRowCellValue("Tür").ToString();


                // Kİtap Tablosundan ISBN'ye göre kitap siler
                SqlCommand sil = new SqlCommand("delete from KitapTablosu where ISBN = @c",baglan.baglanti());
                sil.Parameters.AddWithValue("@c", ISBN);
                sil.ExecuteNonQuery();
                baglan.baglanti().Close();
                Listele();
            }

        }
    }
}
