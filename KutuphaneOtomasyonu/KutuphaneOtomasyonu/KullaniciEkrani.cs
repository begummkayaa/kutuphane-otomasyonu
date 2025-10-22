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
    public partial class KullaniciEkrani : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public string TC; // TC değerini tutar
        public KullaniciEkrani()
        {
            InitializeComponent();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e) //Bütün Kitaplar formunu Açar
        {
            ButunKitaplar bk = new ButunKitaplar();
            bk.MdiParent = this;
            bk.Show();
        }

        private void KullaniciEkrani_Load(object sender, EventArgs e)
        {

        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e) // Aldığım Kitaplar formunu Açar
        {
            AldigimKitaplarcs ak = new AldigimKitaplarcs();
            ak.TC = TC;
            ak.MdiParent = this;
            ak.Show();
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e) // Odunç Alma formunu açar
        {
            OduncAlma oa = new OduncAlma();
            oa.TC = TC;
            oa.MdiParent = this;
            oa.Show();
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)// İade Etme formunu Açar
        {
            IadeEtme ıe = new IadeEtme();
            ıe.TC = TC;
            ıe.MdiParent = this;
            ıe.Show();
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e) // Kullanıcının Rezervasyon Silme formunu açar
        {
            RezarvasyonKul rk = new RezarvasyonKul();
            rk.TC = TC;
            rk.Show();
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e) // Kitap Puanları ve Yorumları formunu açar
        {
            KitapPuanlarıveYorumları kp = new KitapPuanlarıveYorumları();
            kp.MdiParent = this;
            kp.Show();
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e) // Tavsiye Edlenler formunu açar
        {
            TavsiyeEdilenler te = new TavsiyeEdilenler();
            te.MdiParent = this;
            te.Show(); 
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e) // Size Özel formunu açar
        {
            SizeOzelcs so = new SizeOzelcs();
            so.TC = TC;
            so.MdiParent = this;
            so.Show();
        }
    }
}