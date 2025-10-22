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
    public partial class AldigimKitaplarcs : Form
    {
        SqlBaglantisi baglan = new SqlBaglantisi(); // SQl bağlantısı
        public string TC; //TC'yi saklar
        public AldigimKitaplarcs()
        {
            InitializeComponent();
        }

        private void AldigimKitaplarcs_Load(object sender, EventArgs e)
        {
            Listele(TC); 
        }
        public void Listele(string Tc) // Girilen TC'ye göre kullanıcı tablosunu listeler
        {
            string komut = "SELECT * FROM OduncTablosu WHERE KullaniciTc = @tc ";
            using (SqlDataAdapter da = new SqlDataAdapter(komut, baglan.baglanti()))
            {
                da.SelectCommand.Parameters.AddWithValue("@tc", Tc);


                DataSet ds = new DataSet();
                da.Fill(ds);
                gridControl1.DataSource = ds.Tables[0];


            }
        }
    }
}
