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
    public partial class TavsiyeEdilenler : Form
    {
        SqlBaglantisi baglan = new SqlBaglantisi(); // SQl Bağlantısı
        public TavsiyeEdilenler()
        {
            InitializeComponent();
        }

        private void TavsiyeEdilenler_Load(object sender, EventArgs e)
        {
            Listele();

        }

        public void Listele() // Kitap Tablosunu Tavsiye Oranına göre listeler
        {
            string sorgu = "select * from KitapTablosu order by TavsiyeOranı desc";
            SqlDataAdapter da = new SqlDataAdapter(sorgu, baglan.baglanti());
            DataTable dt = new DataTable();
            da.Fill(dt);
            gridControl1.DataSource = dt;
        }

        private void btnAra_Click(object sender, EventArgs e) // Arama butonu
        {
            try
            {
                // Girilen değişkene göre arama yapar
                string arama = "select * from KitapTablosu where Ad =@a";
                SqlDataAdapter da = new SqlDataAdapter(arama, baglan.baglanti());
                da.SelectCommand.Parameters.AddWithValue("@a", txtArama.Text);
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
            catch
            {
                MessageBox.Show("Aradığınız Kitap Bulunamadı","Bilgi",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e) // Bigilendirme butonu
        {
            MessageBox.Show("Tavsiye edilme sayısı ödünç alma sayısına bölünür ve 100 ile çarpılır.Oluşan sonuç tavsiye edilme oranıdır.Lütfen kitapları alırken sadece tavsiye edilme " +
                "oranına değil ödünç alınma sayısına da bakınız. ", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
