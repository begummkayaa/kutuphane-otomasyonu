using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace KutuphaneOtomasyonu
{
    class SqlBaglantisi
    {
        public SqlConnection baglanti()
        {
            SqlConnection baglan = new SqlConnection("Data Source=DESKTOP-ISJCI67\\SQLEXPRESS;Initial Catalog=KutuphaneOtomasyonu;Integrated Security=True");
            baglan.Open();
            return baglan;

        }
    }
}
