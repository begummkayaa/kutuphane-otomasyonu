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
    public partial class KitapPuanlarıveYorumları : Form
    {
        SqlBaglantisi baglan = new SqlBaglantisi(); // SQl Bağlantısı

        public KitapPuanlarıveYorumları()
        {
            InitializeComponent();
        }

        private void KitapPuanlarıveYorumları_Load(object sender, EventArgs e)
        {
            Listele();
        }
        public void Listele() // Yorum Tablosunu listeler
        {
            String komut = " Select * from YorumTablosu ";
            SqlDataAdapter da = new SqlDataAdapter(komut, baglan.baglanti());
            DataSet ds = new DataSet();
            da.Fill(ds);
            gridControl1.DataSource = ds.Tables[0];
        }

        public void Arama(string Ad) // Kitap adına göre listeleme yapar
        {
            try
            {
                string komut = "SELECT * FROM YorumTablosu WHERE KitapAdi = @p1";
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
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnAra_Click(object sender, EventArgs e) // Arama butonu
        {
            Arama(txtArama.Text);
            
        }

        private void btnAc_Click(object sender, EventArgs e) // Gridden verileri araçlara taşır
        {
            mskPuan.Text = gridView1.GetFocusedRowCellValue("Puan").ToString();
            rchYorum.Text = gridView1.GetFocusedRowCellValue("Yorum").ToString();
            mskIsbn.Text = gridView1.GetFocusedRowCellValue("ISBN").ToString();
            label4.Text = "Bu Kitabın Ortalama Puanı: " + ortalamaPuan(mskIsbn.Text).ToString("0.0"); // Labeli değiştirir

        }

        public double ortalamaPuan(string isbn) // ISBN'ye göre ortalama puan hesaplar
        {
            string komut = "SELECT Puan FROM YorumTablosu WHERE ISBN = @isbn";
            double toplamPuan = 0;
            int puanSayisi = 0;

            using (SqlDataAdapter da = new SqlDataAdapter(komut, baglan.baglanti()))
            {
                da.SelectCommand.Parameters.AddWithValue("@isbn", isbn);
                DataSet ds = new DataSet();
                da.Fill(ds);

                // YorumTablosu'ndan tüm puanları topluyoruz
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    toplamPuan += Convert.ToDouble(row["Puan"]);
                    puanSayisi++;
                }

                // Eğer hiç puan yoksa 0 döndürüyoruz
                if (puanSayisi == 0)
                    return 0;

                // Ortalama puanı hesaplıyoruz
                return toplamPuan / puanSayisi;
            }
        }



        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
