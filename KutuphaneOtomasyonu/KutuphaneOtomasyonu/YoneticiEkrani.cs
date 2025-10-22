using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;

namespace KutuphaneOtomasyonu
{
    public partial class YoneticiEkrani : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public YoneticiEkrani()
        {
            InitializeComponent();
        }
         
        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)// Kitap Ekle formunu açar
        {
            KitapEkle ke = new KitapEkle();
            ke.MdiParent = this;
            ke.Show();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)// Kitap sil formunu açar
        {
            KitapSil ks = new KitapSil();
            ks.MdiParent = this;
            ks.Show();
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e) // Kitap Güncelle formunu açar
        {
            KitapGuncelle kg = new KitapGuncelle();
            kg.MdiParent = this;
            kg.Show();
        }

        private void Kullan_ItemClick(object sender, ItemClickEventArgs e)// Kullancı Ekle formunu açar
        {
            KullaniciEkle ke = new KullaniciEkle();
            ke.MdiParent = this;
            ke.Show();
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e) // Kullanıcı Sil formunu açar
        {
            KullaniciSil ks = new KullaniciSil();
            ks.MdiParent = this;
            ks.Show();
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e) // Kullanıcı Güncelle formunu açar
        {
            KullaniciGuncelle kg = new KullaniciGuncelle();
            kg.MdiParent = this;
            kg.Show();
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e) // Kullanıcı Listele formunu Açar
        {
            KullaniciListele kg = new KullaniciListele();
            kg.MdiParent = this;
            kg.Show();
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e) // Ödünç Verme formunu açar
        {
            OduncVerme ov = new OduncVerme();
            ov.MdiParent = this;
            ov.Show();

        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e) // İade Alma formunu açar
        {
            IadeAlma ia = new IadeAlma();
            ia.MdiParent = this;
            ia.Show();
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e) // Rezervasyon Sil formunu Açar
        {
            RezervasyonSil rs = new RezervasyonSil();
            rs.Show();
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e) // Bütün Kitaplar formunu açar
        {
            ButunKitaplar bk = new ButunKitaplar();
            bk.MdiParent = this;
            bk.Show();
        }

        private void YoneticiEkrani_Load(object sender, EventArgs e)
        {

        }
    }
}