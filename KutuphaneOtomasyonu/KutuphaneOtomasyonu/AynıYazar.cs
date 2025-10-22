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
    public partial class AynıYazar : Form
    {
        SqlBaglantisi baglan = new SqlBaglantisi(); // SQl bağlantısı
        public string Yazar; // Yazar değişkenini saklar
        public AynıYazar()
        {
            InitializeComponent();
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void AynıYazar_Load(object sender, EventArgs e)
        {
            Listele();
        }
        public void Listele() // Yazar değişkenine göre kitap tablosunu listeler
        {
            String komut = " Select * from KitapTablosu where Yazar=@yazar ";
            SqlDataAdapter da = new SqlDataAdapter(komut, baglan.baglanti());
            da.SelectCommand.Parameters.AddWithValue("@yazar", Yazar);
            DataSet ds = new DataSet();
            da.Fill(ds);
            gridControl1.DataSource = ds.Tables[0];
        }
    }
}
