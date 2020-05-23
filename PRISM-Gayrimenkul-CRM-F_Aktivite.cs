using DevExpress.XtraBars.Alerter;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraTab;
using PRISM.Properties;
using PRISM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using DevExpress.XtraGrid.Menu;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid.Localization;
using System.Data.Linq;
using System.Threading;
using System.Data.SqlClient;
using Model;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Reflection;



namespace PRISM
{



    public partial class F_Aktivite : XtraForm
    {


        public F_Aktivite(int AktiviteID)
        {
            Genel.YuklemeSuresiGetir(1, "");
            InitializeComponent();
            formFooter1._frm = this;
            iFormID = Genel.FormIDGetir(this.Name);
            iAktiviteID = AktiviteID;
            DilCevir();
        }

        List<Model.S_AktiviteSQL_KampanyaKontrol_Result> KampanyaSQLKontrolList = new List<Model.S_AktiviteSQL_KampanyaKontrol_Result>();
        List<Model.S_AktiviteSQL_IndirimKontrol_Result> IndirimSQLKontrolList = new List<Model.S_AktiviteSQL_IndirimKontrol_Result>();
        List<Model.S_Resimler_Result> listGayrimnekulDetayiPopup = new List<S_Resimler_Result>();

        private int iAktiviteID = -1;
        bool bIndirimQueryCalissinMi = false;
        bool bKampanyaQueryCalissinMi = false;
        bool bProjeOzelQueryCalissinMi = false;
        bool bFormQueryCalissinMi = false;
        bool bIsFormSaved = false;
        decimal dCaprazKur = 0;

        private int iGayrimenkulID = -1;
        public int _GayrimenkulID
        {
            get { return iGayrimenkulID; }
            set { iGayrimenkulID = value; }
        }


        private bool bAktiviteIptalMi = false;
        public bool _AktiviteIptalMi
        {
            get { return bAktiviteIptalMi; }
            set { bAktiviteIptalMi = value; }
        }

        private bool bFormReadOnly = false;
        public bool _FormReadOnly
        {
            get { return bFormReadOnly; }
            set { bFormReadOnly = value; }
        }




        int iFormID = -1;
        int iMaxPesinatSayisi = 1; //proje kartında tanımlı maksimum pesinat sayısını tutar.
        int iPesinatSayisi = 1; //pesinat sayısının kac adet gösterilecegini tutar.
        bool bIsFormLoaded = false;
        bool bIsAdmin = false;



        private void DilCevir()
        {
            lblGun.Text = Genel.DilGetirBaslik(3754, lblGun.Text);
            lblRezerveSuresi.Text = Genel.DilGetirBaslik(3755, lblRezerveSuresi.Text);
            lblBaslik.Text = Genel.DilGetirBaslik(1988, lblBaslik.Text);
            lblTempCaption.Text = Genel.DilGetirBaslik(1885, gcGayrimenkulBilgileri.Text); //gcGayrimenkulBilgileri için...
            gcMusteriBİlgileri.Text = Genel.DilGetirBaslik(1924, gcMusteriBİlgileri.Text);
            btnGayrimenkulDetayi.Text = Genel.DilGetirBaslik(1886, btnGayrimenkulDetayi.Text);

            bbtnDetay.Caption = Genel.DilGetirBaslik(1887, bbtnDetay.Caption);
            bbtnDairePlaniGID.Caption = Genel.DilGetirBaslik(1888, bbtnDairePlaniGID.Caption);
            bbtnKatPlani.Caption = Genel.DilGetirBaslik(1889, bbtnKatPlani.Caption);
            bbtnVaziyetPlani.Caption = Genel.DilGetirBaslik(3699, bbtnVaziyetPlani.Caption);
            bbtnBlokPlani.Caption = Genel.DilGetirBaslik(3700, bbtnBlokPlani.Caption);
            bbtnDairePlaniTip.Caption = Genel.DilGetirBaslik(3701, bbtnDairePlaniTip.Caption);
            bbtnDairePlaniTipSinifi.Caption = Genel.DilGetirBaslik(3702, bbtnDairePlaniTipSinifi.Caption);

            bbtnProjeTanitimVideosu.Caption = Genel.DilGetirBaslik(3820, bbtnProjeTanitimVideosu.Caption);
            bbtnBlokPlani_Video.Caption = Genel.DilGetirBaslik(3821, bbtnBlokPlani_Video.Caption);
            bbtnKatPlani_Video.Caption = Genel.DilGetirBaslik(3822, bbtnKatPlani_Video.Caption);
            bbtnDairePlani_GID_Video.Caption = Genel.DilGetirBaslik(3823, bbtnDairePlani_GID_Video.Caption);
            bbtnDairePlaniTip_Video.Caption = Genel.DilGetirBaslik(3824, bbtnDairePlaniTip_Video.Caption);
            bbtnDairePlaniTipSinifi_Video.Caption = Genel.DilGetirBaslik(3825, bbtnDairePlaniTipSinifi_Video.Caption);
                     

            bbtnGenelTanitimFormu.Caption = Genel.DilGetirBaslik(1890, bbtnGenelTanitimFormu.Caption);
            btnMusteriEkle.Text = Genel.DilGetirBaslik(1987, btnMusteriEkle.Text);
            gcSatisFiyatiNetlestirme.Text = Genel.DilGetirBaslik(1891, gcSatisFiyatiNetlestirme.Text);
            lblSatisFiyati.Text = Genel.DilGetirBaslik(1892, lblSatisFiyati.Text);
            lblListeFiyati.Text = Genel.DilGetirBaslik(1893, lblListeFiyati.Text);
            lblFarkSol.Text = Genel.DilGetirBaslik(1894, lblFarkSol.Text);
            lblTarih.Text = Genel.DilGetirBaslik(1895, lblTarih.Text);
            lblToplamIndirim.Text = Genel.DilGetirBaslik(1896, lblToplamIndirim.Text);
            lblVadeFarki.Text = Genel.DilGetirBaslik(1897, lblVadeFarki.Text);
            lblOdemeBicimi.Text = Genel.DilGetirBaslik(1898, lblOdemeBicimi.Text);
            lblIndirim.Text = Genel.DilGetirBaslik(1899, lblIndirim.Text);
            lblKampanya.Text = Genel.DilGetirBaslik(1900, lblKampanya.Text);
            lblOdenecekTutar.Text = Genel.DilGetirBaslik(1901, lblOdenecekTutar.Text);
            lblBeklenenTarih.Text = Genel.DilGetirBaslik(1902, lblBeklenenTarih.Text);
            lblPesinat1.Text = Genel.DilGetirBaslik(1903, lblPesinat1.Text);
            lblPesinat2.Text = Genel.DilGetirBaslik(1903, "Peşinat ") + "2";
            lblPesinat3.Text = Genel.DilGetirBaslik(1903, "Peşinat ") + "3";
            lblPesinat4.Text = Genel.DilGetirBaslik(1903, "Peşinat ") + "4";
            lblPesinat5.Text = Genel.DilGetirBaslik(1903, "Peşinat ") + "5";
            lblPesinat6.Text = Genel.DilGetirBaslik(1903, "Peşinat ") + "6";
            lblPesinat7.Text = Genel.DilGetirBaslik(1903, "Peşinat ") + "7";
            lblPesinat8.Text = Genel.DilGetirBaslik(1903, "Peşinat ") + "8";
            lblPesinat9.Text = Genel.DilGetirBaslik(1903, "Peşinat ") + "9";
            lblPesinat10.Text = Genel.DilGetirBaslik(1903, "Peşinat ") + "10";
            lblBankaKredisi.Text = Genel.DilGetirBaslik(1904, lblBankaKredisi.Text);
            btnBankaKredisi.Text = Genel.DilGetirBaslik(1904, btnBankaKredisi.Text);
            lblVadeli.Text = Genel.DilGetirBaslik(1905, lblVadeli.Text);
            lblToplam_Fark.Text = Genel.DilGetirBaslik(1906, lblToplam_Fark.Text);
            btnKrediHesapla.Text = Genel.DilGetirBaslik(1907, btnKrediHesapla.Text);
            lblTaksitToplami.Text = Genel.DilGetirBaslik(1908, lblTaksitToplami.Text);
            lblOdemeFarkAlt.Text = Genel.DilGetirBaslik(1909, lblOdemeFarkAlt.Text);
            btnVadeTumunuSil.Text = Genel.DilGetirBaslik(1910, btnVadeTumunuSil.Text);
            btnVadeliOdemeSihirbazi.Text = Genel.DilGetirBaslik(1911, btnVadeliOdemeSihirbazi.Text);
            tpOdemePlaniTanimlari.Text = Genel.DilGetirBaslik(1912, tpOdemePlaniTanimlari.Text);
            tpMaliDurum.Text = Genel.DilGetirBaslik(1913, tpMaliDurum.Text);
            lblAktiviteDurumu.Text = Genel.DilGetirBaslik(1914, lblAktiviteDurumu.Text);
            btnKaydet.Text = Genel.DilGetirBaslik(1915, btnKaydet.Text);
            bbiKaydetKapat.Caption = Genel.DilGetirBaslik(3153, bbiKaydetKapat.Caption);


          //  btnTeklifler.Text = Genel.DilGetirBaslik(3542, btnTeklifler.Text);

            btnAktiviteIptal.Text = Genel.DilGetirBaslik(1916, btnAktiviteIptal.Text);
            btnAktiviteYetki.Text = Genel.DilGetirBaslik(1917, btnAktiviteYetki.Text);

            //Mali işler tabı
            lblMD_Plan.Text = Genel.DilGetirBaslik(1918, lblMD_Plan.Text);
            lblMD_Yuzde.Text = Genel.DilGetirBaslik(1919, lblMD_Yuzde.Text);
            lblMD_Tahsilat.Text = Genel.DilGetirBaslik(1920, lblMD_Tahsilat.Text);
            lblMD_Kalan.Text = Genel.DilGetirBaslik(1921, lblMD_Kalan.Text);
            lblMD_NPV.Text = Genel.DilGetirBaslik(1922, lblMD_NPV.Text);
            lblMD_OrtalamaVade.Text = Genel.DilGetirBaslik(1923, lblMD_OrtalamaVade.Text);
            lblMD_Pesinat.Text = Genel.DilGetirBaslik(1903, lblMD_Pesinat.Text);
            lblMD_BankaKredisi.Text = Genel.DilGetirBaslik(1904, lblMD_BankaKredisi.Text);
            lblMD_Vadeli.Text = Genel.DilGetirBaslik(1905, lblMD_Vadeli.Text);
            lblMD_Toplam.Text = Genel.DilGetirBaslik(1989, lblMD_Toplam.Text);



            //Vadeli ödeme sihirbazı tabı
            lblOdemeTipi.Text = Genel.DilGetirBaslik(1979, lblOdemeTipi.Text);
            lblTaksitTuru.Text = Genel.DilGetirBaslik(1980, lblTaksitTuru.Text);
            lblIlkTaksitTarihi.Text = Genel.DilGetirBaslik(1981, lblIlkTaksitTarihi.Text);
            lblTaksitSayisi.Text = Genel.DilGetirBaslik(1982, lblTaksitSayisi.Text);
            lblSabitTutar.Text = Genel.DilGetirBaslik(1983, lblSabitTutar.Text);
            rgVadeliOdemeTipi.Properties.Items[0].Description = Genel.DilGetirBaslik(1983, rgVadeliOdemeTipi.Properties.Items[0].Description);
            rgVadeliOdemeTipi.Properties.Items[1].Description = Genel.DilGetirBaslik(1984, rgVadeliOdemeTipi.Properties.Items[1].Description);
            bbiUygulaVeKapat.Caption = Genel.DilGetirBaslik(1985, bbiUygulaVeKapat.Caption);
            ddbtnTaksitUygulaVeKapat.Text = Genel.DilGetirBaslik(3722, ddbtnTaksitUygulaVeKapat.Text);

            btnGeri.Text = Genel.DilGetirBaslik(1986, btnGeri.Text);



            //Gayrimenkul Grid Kolonları
            colGayrimenkulID.Caption = Genel.DilGetirBaslik(1925, colGayrimenkulID.Caption);
            colSiraNo.Caption = Genel.DilGetirBaslik(1926, colSiraNo.Caption);
            colBolumAdi.Caption = Genel.DilGetirBaslik(1927, colBolumAdi.Caption);
            colBlokNo.Caption = Genel.DilGetirBaslik(1928, colBlokNo.Caption);
            colGiris.Caption = Genel.DilGetirBaslik(1929, colGiris.Caption);
            colKat.Caption = Genel.DilGetirBaslik(1930, colKat.Caption);
            colDaireNo.Caption = Genel.DilGetirBaslik(1931, colDaireNo.Caption);
            colBrutM2.Caption = Genel.DilGetirBaslik(1932, colBrutM2.Caption);
            colNetM2.Caption = Genel.DilGetirBaslik(1933, colNetM2.Caption);
            colGayrimenkulTip.Caption = Genel.DilGetirBaslik(1934, colGayrimenkulTip.Caption);
            colProjeTipSinifi.Caption = Genel.DilGetirBaslik(1935, colProjeTipSinifi.Caption);
            colListeFiyati.Caption = Genel.DilGetirBaslik(1936, colListeFiyati.Caption);
            colListeFiyatiDovizKodu.Caption = Genel.DilGetirBaslik(1937, colListeFiyatiDovizKodu.Caption);
            colYon.Caption = Genel.DilGetirBaslik(1938, colYon.Caption);
            colManzara.Caption = Genel.DilGetirBaslik(1939, colManzara.Caption);
            colOtoParkNo.Caption = Genel.DilGetirBaslik(1940, colOtoParkNo.Caption);
            colOdaSayisi.Caption = Genel.DilGetirBaslik(1941, colOdaSayisi.Caption);
            colKatBrutM2.Caption = Genel.DilGetirBaslik(1942, colKatBrutM2.Caption);
            colBalkonM2.Caption = Genel.DilGetirBaslik(1943, colBalkonM2.Caption);
            colTerasM2.Caption = Genel.DilGetirBaslik(1944, colTerasM2.Caption);
            colBahceM2.Caption = Genel.DilGetirBaslik(1945, colBahceM2.Caption);
            colTapuBagimsizNo.Caption = Genel.DilGetirBaslik(1946, colTapuBagimsizNo.Caption);
            colSatisFiyati.Caption = Genel.DilGetirBaslik(1947, colSatisFiyati.Caption);
            colListeM2Fiyati.Caption = Genel.DilGetirBaslik(1948, colListeM2Fiyati.Caption);
            colNPV.Caption = Genel.DilGetirBaslik(1949, colNPV.Caption);
            colNPVM2.Caption = Genel.DilGetirBaslik(1950, colNPVM2.Caption);
            colKdvDurumuKisID_26.Caption = Genel.DilGetirBaslik(1951, colKdvDurumuKisID_26.Caption);

            //Müşteri Grid Kolonları
            colMusteriID.Caption = Genel.DilGetirBaslik(1952, colMusteriID.Caption);
            colMusteriAdiSoyadi.Caption = Genel.DilGetirBaslik(1953, colMusteriAdiSoyadi.Caption);
            colEvTel.Caption = Genel.DilGetirBaslik(1954, colEvTel.Caption);
            colCepTel.Caption = Genel.DilGetirBaslik(1955, colCepTel.Caption);
            colIsTel.Caption = Genel.DilGetirBaslik(1956, colIsTel.Caption);
            colDigerTel.Caption = Genel.DilGetirBaslik(1957, colDigerTel.Caption);
            colEmail.Caption = Genel.DilGetirBaslik(1958, colEmail.Caption);
            colSiraNo2.Caption = Genel.DilGetirBaslik(1926, colSiraNo2.Caption);

            //Senet Grid Kolonları
            colOdemePlani_BorcVadeFarkiTutari.Caption = Genel.DilGetirBaslik(1959, colOdemePlani_BorcVadeFarkiTutari.Caption);
            colOdemePlani_BorcTutariToplam.Caption = Genel.DilGetirBaslik(1960, colOdemePlani_BorcTutariToplam.Caption);
            colOdemePlani_ParcalamaYapilsinMi.Caption = Genel.DilGetirBaslik(1961, colOdemePlani_ParcalamaYapilsinMi.Caption);
            colOdemePlani_ParcalamadanOncekiTutar.Caption = Genel.DilGetirBaslik(1962, colOdemePlani_ParcalamadanOncekiTutar.Caption);
            colOdemePlani_KarsiParaDovizTutari.Caption = Genel.DilGetirBaslik(1963, colOdemePlani_KarsiParaDovizTutari.Caption);
            colOdemePlani_TufeliMi.Caption = Genel.DilGetirBaslik(1964, colOdemePlani_TufeliMi.Caption);
            colOdemePlani_AktiviteTefeTufeOranArtiID.Caption = Genel.DilGetirBaslik(1965, colOdemePlani_AktiviteTefeTufeOranArtiID.Caption);
            colOdemePlani_TufeBaslangicTarihi.Caption = Genel.DilGetirBaslik(1966, colOdemePlani_TufeBaslangicTarihi.Caption);
            colOdemePlani_TufeBitisTarihi.Caption = Genel.DilGetirBaslik(1967, colOdemePlani_TufeBitisTarihi.Caption);
            colOdemePlani_KdvOdemesiMi.Caption = Genel.DilGetirBaslik(1968, colOdemePlani_KdvOdemesiMi.Caption);
            colOdemePlani_FaizHesabiTaksitTutarindanYapilsinMi.Caption = Genel.DilGetirBaslik(1969, colOdemePlani_FaizHesabiTaksitTutarindanYapilsinMi.Caption);
            colOdemePlani_OdemeYeriBilgiMesaji.Caption = Genel.DilGetirBaslik(1970, colOdemePlani_OdemeYeriBilgiMesaji.Caption);
            colOdemePlani_No.Caption = Genel.DilGetirBaslik(1971, colOdemePlani_No.Caption);
            colOdemePlani_Tarih.Caption = Genel.DilGetirBaslik(1972, colOdemePlani_Tarih.Caption);
            colOdemePlani_OdemeSekli.Caption = Genel.DilGetirBaslik(1973, colOdemePlani_OdemeSekli.Caption);
            colOdemePlani_OdemeTutari.Caption = Genel.DilGetirBaslik(1974, colOdemePlani_OdemeTutari.Caption);
            colOdemePlani_Odenen.Caption = Genel.DilGetirBaslik(1975, colOdemePlani_Odenen.Caption);
            colOdemePlani_AraOdemeMi.Caption = Genel.DilGetirBaslik(1976, colOdemePlani_AraOdemeMi.Caption);
            colOdemePlani_CekSeriNo.Caption = Genel.DilGetirBaslik(1977, colOdemePlani_CekSeriNo.Caption);
            colOdemePlani_Aciklama.Caption = Genel.DilGetirBaslik(1978, colOdemePlani_Aciklama.Caption);
        }


        private void BaslikAyarla()
        {
            string strStatu = "";
            if (MListler.MList_S_TnKis_AktiviteStatu.Where(u => u.ID == C.ParseInt(MListler.dsAktivite.Tables[0].Rows[0]["AktiviteStatuKisID_308"])).ToList().Count > 0)
            {
                strStatu = MListler.MList_S_TnKis_AktiviteStatu.Where(u => u.ID == C.ParseInt(MListler.dsAktivite.Tables[0].Rows[0]["AktiviteStatuKisID_308"])).ToList().FirstOrDefault().Aciklama;

                strStatu = "      ٠•● " + strStatu + " ●•٠ ";
            }


            string strAktiviteID = "";
            if (iAktiviteID > 0)
                strAktiviteID = "[" + iAktiviteID.ToString() + "] ";

            string strAdmin = "";
            if (bIsAdmin)
                strAdmin = "   ₪ ΛĐмḯη ₪";




            this.Text = strAktiviteID + lblBaslik.Text + strStatu + "  " + strAdmin;


        }


        public void YetkiAyarla()
        {
            // NOT : Aktivite tarihi dısında bir tarihte aktiviteye giremez yetkisi, Aktivasyon ve IptalAktivasyon formlarından kontrol ediliyor...
            try
            {
                if (!bIsFormLoaded)
                    return;


                bIsAdmin = Genel.AktiviteYetkiGetir(AktiviteYetkileri.AdminMi);
                BaslikAyarla();


                #region Dinamik Butonlar ayarlanıyor...
                DataTable dt = Genel.ListToDt(MListler.MList_S_TnKis_AktiviteDinamikButonlari_313);
                int iButtonIndex = 1;
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    Control[] ctrls = this.Controls.Find("btn_" + iButtonIndex.ToString("00"), true);
                    if (ctrls.Length > 0)
                    {
                        SimpleButton btn = ((SimpleButton)ctrls[0]);
                        if (!C.ParseBool(dt.Rows[i - 1]["AktifMi"]))
                            continue;


                        int iID = C.ParseInt(dt.Rows[i - 1]["ID"]);
                        switch (iID)
                        {
                            case 600:
                                if (!bIsAdmin)
                                {
                                    if (Genel.AktiviteYetkiGetir(AktiviteYetkileri.Buton_OdemelerTahsilatlar))
                                    {
                                        btn.Visible = true;
                                        btn.Text = dt.Rows[i - 1]["Aciklama"].ToString();
                                        btn.Tag = dt.Rows[i - 1]["ID"];
                                        btn.Click += btn_Click;
                                    }
                                    else
                                        continue;
                                }
                                else
                                {
                                    btn.Visible = true;
                                    btn.Text = dt.Rows[i - 1]["Aciklama"].ToString();
                                    btn.Tag = dt.Rows[i - 1]["ID"];
                                    btn.Click += btn_Click;
                                }
                                break;
                            case 601:
                                if (!bIsAdmin)
                                {
                                    if (Genel.AktiviteYetkiGetir(AktiviteYetkileri.Buton_Evraklar))
                                    {
                                        btn.Visible = true;
                                        btn.Text = dt.Rows[i - 1]["Aciklama"].ToString();
                                        btn.Tag = dt.Rows[i - 1]["ID"];
                                        btn.Click += btn_Click;
                                    }
                                    else
                                        continue;
                                }
                                else
                                {
                                    btn.Visible = true;
                                    btn.Text = dt.Rows[i - 1]["Aciklama"].ToString();
                                    btn.Tag = dt.Rows[i - 1]["ID"];
                                    btn.Click += btn_Click;
                                }
                                break;
                            case 602:
                                if (!bIsAdmin)
                                {
                                    if (Genel.AktiviteYetkiGetir(AktiviteYetkileri.Buton_DigerDatalar))
                                    {
                                        btn.Visible = true;
                                        btn.Text = dt.Rows[i - 1]["Aciklama"].ToString();
                                        btn.Tag = dt.Rows[i - 1]["ID"];
                                        btn.Click += btn_Click;
                                    }
                                    else
                                        continue;
                                }
                                else
                                {
                                    btn.Visible = true;
                                    btn.Text = dt.Rows[i - 1]["Aciklama"].ToString();
                                    btn.Tag = dt.Rows[i - 1]["ID"];
                                    btn.Click += btn_Click;
                                }
                                break;
                            case 603:
                                if (!bIsAdmin)
                                {
                                    if (Genel.AktiviteYetkiGetir(AktiviteYetkileri.Buton_IadeEdilenOdeme))
                                    {
                                        if (iAktiviteID > 0 && !C.ParseBool(MListler.dsAktivite.Tables[0].Rows[0]["AktifMi"])) //sadece iptal edilen aktivitede görünür.
                                        {
                                            btn.Visible = true;
                                            btn.Text = dt.Rows[i - 1]["Aciklama"].ToString();
                                            btn.Tag = dt.Rows[i - 1]["ID"];
                                            btn.Click += btn_Click;
                                        }
                                        else
                                            continue;
                                    }
                                    else
                                        continue;
                                }
                                else
                                {
                                    btn.Visible = true;
                                    btn.Text = dt.Rows[i - 1]["Aciklama"].ToString();
                                    btn.Tag = dt.Rows[i - 1]["ID"];
                                    btn.Click += btn_Click;
                                }
                                break;
                            case 604:
                                if (!bIsAdmin)
                                {
                                    if (Genel.AktiviteYetkiGetir(AktiviteYetkileri.Buton_Dosyalar))
                                    {
                                        btn.Visible = true;
                                        btn.Text = dt.Rows[i - 1]["Aciklama"].ToString();
                                        btn.Tag = dt.Rows[i - 1]["ID"];
                                        btn.Click += btn_Click;
                                    }
                                    else
                                        continue;
                                }
                                else
                                {
                                    btn.Visible = true;
                                    btn.Text = dt.Rows[i - 1]["Aciklama"].ToString();
                                    btn.Tag = dt.Rows[i - 1]["ID"];
                                    btn.Click += btn_Click;
                                }
                                break;
                            case 605:
                                if (!bIsAdmin)
                                {
                                    if (Genel.AktiviteYetkiGetir(AktiviteYetkileri.Buton_SatisSonrasiIslemler))
                                    {
                                        btn.Visible = true;
                                        btn.Text = dt.Rows[i - 1]["Aciklama"].ToString();
                                        btn.Tag = dt.Rows[i - 1]["ID"];
                                        btn.Click += btn_Click;
                                    }
                                    else
                                        continue;
                                }
                                else
                                {
                                    btn.Visible = true;
                                    btn.Text = dt.Rows[i - 1]["Aciklama"].ToString();
                                    btn.Tag = dt.Rows[i - 1]["ID"];
                                    btn.Click += btn_Click;
                                }
                                break;
                            case 606:
                                if (!bIsAdmin)
                                {
                                    if (Genel.AktiviteYetkiGetir(AktiviteYetkileri.Buton_GayrimenkulBedeliDisindakiOdemeler))
                                    {
                                        btn.Visible = true;
                                        btn.Text = dt.Rows[i - 1]["Aciklama"].ToString();
                                        btn.Tag = dt.Rows[i - 1]["ID"];
                                        btn.Click += btn_Click;
                                    }
                                    else
                                        continue;
                                }
                                else
                                {
                                    btn.Visible = true;
                                    btn.Text = dt.Rows[i - 1]["Aciklama"].ToString();
                                    btn.Tag = dt.Rows[i - 1]["ID"];
                                    btn.Click += btn_Click;
                                }
                                break;
                            case 607:
                                if (!bIsAdmin)
                                {
                                    if (Genel.AktiviteYetkiGetir(AktiviteYetkileri.Buton_GayrimenkulData))
                                    {
                                        btn.Visible = true;
                                        btn.Text = dt.Rows[i - 1]["Aciklama"].ToString();
                                        btn.Tag = dt.Rows[i - 1]["ID"];
                                        btn.Click += btn_Click;
                                    }
                                    else
                                        continue;
                                }
                                else
                                {
                                    btn.Visible = true;
                                    btn.Text = dt.Rows[i - 1]["Aciklama"].ToString();
                                    btn.Tag = dt.Rows[i - 1]["ID"];
                                    btn.Click += btn_Click;
                                }
                                break;

                        }


                        if (iAktiviteID <= 0)
                            btn.Visible = false;

                    }
                    iButtonIndex++;
                }

                #endregion

                #region Peşinat tarihleri değiştirme yetkisi
                if (iAktiviteID > 0 && !bIsAdmin)
                {
                    bool bPesinatTarihiDegistirebilir = Genel.AktiviteYetkiGetir(AktiviteYetkileri.PesinOdemeTarihiniDegistirebilir);
                    dtmPesinat1.Enabled = bPesinatTarihiDegistirebilir;
                    dtmPesinat2.Enabled = bPesinatTarihiDegistirebilir;
                    dtmPesinat3.Enabled = bPesinatTarihiDegistirebilir;
                    dtmPesinat4.Enabled = bPesinatTarihiDegistirebilir;
                    dtmPesinat5.Enabled = bPesinatTarihiDegistirebilir;
                    dtmPesinat6.Enabled = bPesinatTarihiDegistirebilir;
                    dtmPesinat7.Enabled = bPesinatTarihiDegistirebilir;
                    dtmPesinat8.Enabled = bPesinatTarihiDegistirebilir;
                    dtmPesinat9.Enabled = bPesinatTarihiDegistirebilir;
                    dtmPesinat10.Enabled = bPesinatTarihiDegistirebilir;
                }
                #endregion

                #region Banka Ödeme tarihleri değiştirme yetkisi
                if (iAktiviteID > 0 && !bIsAdmin)
                {
                    dtmBankaKredisi.Enabled = Genel.AktiviteYetkiGetir(AktiviteYetkileri.BankaOdemeTarihiniDegistirebilir);
                }
                #endregion

                #region Kampanya tanımı yetkisi
                if (!bIsAdmin)
                    cmbKampanya.Properties.Buttons[1].Visible = Genel.AktiviteYetkiGetir(AktiviteYetkileri.KampanyaTanimiYapabilir);
                #endregion

                #region İndirim tanımı yetkisi
                if (!bIsAdmin)
                    cmbIndirim.Properties.Buttons[1].Visible = Genel.AktiviteYetkiGetir(AktiviteYetkileri.IndirimTanimiYapabilir);
                #endregion

                #region Vadeli Odeme Planı düzeltme yetkisi
                if (iAktiviteID > 0 && !bIsAdmin && !Genel.TeknikKullanici)
                {
                    if (!Genel.AktiviteYetkiGetir(AktiviteYetkileri.VadeliOdemePlanindaDuzeltmeYapabilir))
                    {

                        pnlFiyatlar.Enabled = false;
                        grdGayrimenkulBilgileri.Enabled = false;
                        pnlSecimler.Enabled = false;
                        pnlOdemePlani.Enabled = false;
                        pnlVadeKontrol.Enabled = false;
                        grdVadeliOdemeBilgileri.Enabled = false;
                    }
                }
                #endregion

                #region Başka personelin kaydını iptal edebilir.
                if (iAktiviteID > 0 && MListler.dsAktivite.Tables[0].Rows.Count > 0 && !bIsAdmin)
                {
                    int iAktivitePersonelID = C.ParseInt(MListler.dsAktivite.Tables[0].Rows[0]["AktivitePersonelID"]);
                    if (iAktivitePersonelID != Genel.AktifPersonelID && Genel.AktiviteYetkiGetir(AktiviteYetkileri.BaskaPersonelinKaydiniIptalEdebilir))
                        btnAktiviteIptal.Visible = true;
                    else if (iAktivitePersonelID != Genel.AktifPersonelID)
                        btnAktiviteIptal.Visible = false;
                }
                #endregion

                #region Ödemesi olan kaydı iptal edebilir.
                if (iAktiviteID > 0 && !Genel.AktiviteYetkiGetir(AktiviteYetkileri.OdemesiOlanKaydiIptalEdebilir) && !bIsAdmin)
                {
                    var listOdemeler = Genel.PrmDb.S_AktiviteBakiyeGetir(iAktiviteID).FirstOrDefault();
                    decimal dKaporaDisindakiOdeme = C.ParseDecimal(listOdemeler.OdedigiDepozitosuz);
                    if (dKaporaDisindakiOdeme > 0) //kapora dısında ödeme var
                        btnAktiviteIptal.Enabled = false;
                    else
                        btnAktiviteIptal.Enabled = true;
                }
                #endregion

                #region Satis girişi yapabilir. Bu yetki false ise sadece rezerve girişi yapabilir.
                if (bIsAdmin == false && Genel.AktiviteYetkiGetir(AktiviteYetkileri.SatisGirisiYapabilir) == false && iAktiviteID <= 0)
                {
                    cmbAktiviteDurumu.Properties.DataSource = MListler.MList_S_TnKis_GayrimenkulHareketTipleri_300.Where(u => u.ID == 302); //sadece rezerve 
                    cmbAktiviteDurumu.EditValue = 302;
                }
                #endregion

                #region Rezervasyon iptali yapamaz
                if (!bIsAdmin)
                {
                    if (C.ParseInt(cmbAktiviteDurumu.EditValue) == 302 && iAktiviteID > 0 && !Genel.AktiviteYetkiGetir(AktiviteYetkileri.RezerveIptaliYapabilir)) //REZERVE.
                    {
                        btnAktiviteIptal.Visible = false;
                    }
                }
                #endregion

                #region Satış iptali yapamaz
                if (!bIsAdmin)
                {
                    if (C.ParseInt(cmbAktiviteDurumu.EditValue) == 303 && iAktiviteID > 0 && !Genel.AktiviteYetkiGetir(AktiviteYetkileri.SatisIptaliYapabilir)) //SATILDI.
                    {
                        btnAktiviteIptal.Visible = false;
                    }
                }
                #endregion

                #region Esnek Karakterleri Göremez / Tarihçeyi göremez.
                //Bu yetkiler formYanMenu user kontrolü içinde,popupMenu1_BeforePopup eventine yazıldı.
                #endregion

                #region Kayıt düzenleme yapabilir.
                if (!bIsAdmin && iAktiviteID > 0 && !Genel.AktiviteYetkiGetir(AktiviteYetkileri.KayitDuzenlemeYapabilir))
                {
                    btnKaydet.Visible = false;
                    btnAktiviteIptal.Visible = false;
                }
                #endregion

                #region Sözleşme yapıldığı andan itibaren sadece kayıt izler
                if (iAktiviteID > 0 && !bIsAdmin && Genel.AktiviteYetkiGetir(AktiviteYetkileri.SatisGercekSatisaDonduguAndanItibarenSadeceIzlemeYapabilir) && C.ParseBool(MListler.dsAktivite.Tables[0].Rows[0]["SozlesmeYapildiMi"]))
                {
                    btnKaydet.Visible = false;
                    btnAktiviteIptal.Visible = false;
                }
                #endregion


                if (!bIsAdmin)
                {
                    btnAktiviteYetki.Visible = Genel.AktiviteYetkiGetir(AktiviteYetkileri.Buton_AktiviteYetkiler);
                    //if (iAktiviteID <= 0)
                        //btnTeklifler.Visible = Genel.AktiviteYetkiGetir(AktiviteYetkileri.Buton_Teklifler);
                    //else
                    //    btnTeklifler.Visible = false;
                }

            }
            catch (Exception Hata)
            {
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormReadOnlyAyarla()
        {

            try
            {

                if (!bIsFormLoaded)
                    return;

                if (iAktiviteID > 0 && !C.ParseBool(MListler.dsAktivite.Tables[0].Rows[0]["AktifMi"]))
                    bFormReadOnly = true;

                if (bFormReadOnly)
                {
                    gvVadeliOdemeBilgileri.OptionsBehavior.Editable = false;
                    colEkle.Visible = false;
                    colSil.Visible = false;
                    colMusteriSil.Visible = false;
                    colMusteriDuzenle.Visible = false;
                    colOdemePlani_Sil.Visible = false;

                    txtGrdGayrimenkulListeFiyati.Buttons[0].Visible = false;
                    btnPesinatEkle.Visible = false;
                    btnPesinatSil.Visible = false;
                    btnVadeliOdemeSihirbazi.Visible = false;
                    btnVadeTumunuSil.Visible = false;


                    cmbDoviz.Enabled = false;
                    btnMusteriEkle.Enabled = false;
                    cmbOdemeBicimiAna.Enabled = false;
                    cmbKampanya.Enabled = false;
                    cmbIndirim.Enabled = false;


                    btnKaydet.Visible = false;
                    // formYanMenu1.Visible = false;
                    btnAktiviteIptal.Visible = false;
                }
                else //iptal aktivite bir kez acılıp, silme ekleme butonları gizli haliyle grid ayarlarına kaydedildiği için,aktivasyondan tekrar acılısında gizli geliyor bu nedenle burada sadece grid ekle sil duzenle kolonları tekrar acılıyor.
                {
                    colEkle.Visible = true;
                    colEkle.VisibleIndex = 0;
                    colSil.Visible = true;
                    colSil.VisibleIndex = 1;

                    colMusteriDuzenle.Visible = true;
                    colMusteriDuzenle.VisibleIndex = 0;
                    colMusteriSil.Visible = true;
                    colMusteriSil.VisibleIndex = 1;

                    colOdemePlani_Sil.Visible = true;
                    colOdemePlani_Sil.VisibleIndex = 0;
                }
            }
            catch (Exception Hata)
            {
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        List<Model.S_Proje_Result> PRJ = new List<Model.S_Proje_Result>();

        private void F_Aktivite_Load(object sender, EventArgs e)
        {
            try
            {
                Ayar.FormSizeGetir(Name);


                if (iAktiviteID < 1) //yeni kayıtta lisans kontrolü
                {
                    int iLisans_MaxAktivite = C.ParseInt(Sifreleme.Coz(MListler.MList_S_VersiyonL.FirstOrDefault().A));
                    int iLisans_MevcutAktivite = C.ParseInt(MListler.MList_S_VersiyonL.FirstOrDefault().AktiviteSayisi);

                    if (iLisans_MevcutAktivite > iLisans_MaxAktivite && iLisans_MaxAktivite != 0)
                    {
                        XtraMessageBox.Show(Genel.DilGetirMesaj(3164), "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.Close();
                        return;
                    }
                }



                dtmTarih.Properties.MaxValue = DateTime.Now.Date;


                tcOdeme.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
                tcOdeme.Height = 182;

                GridAyarGetir();

                listGayrimnekulDetayiPopup = Genel.PrmDb.S_Resimler(-1).ToList();


                cmbAktiviteDurumu.Properties.DataSource = MListler.MList_S_TnKis_GayrimenkulHareketTipleri_300.Where(u => u.ID == 302 || u.ID == 303);
                cmbGrdKdvDurumuKisID_26.DataSource = MListler.MList_S_TnKis_KDV;
                sccGridler.SplitterPosition = sccGridler.Width / 2;



                #region Maksimum  peşinat sayısı ayarlanıyor...ve Genel.Aktivite_ProjeID ayarlanıyor...
                if (Genel.Aktivite_ProjeID < 1)
                    Genel.Aktivite_ProjeID = C.ParseInt(Genel.PrmDb.S_AktiviteGayrimenkulGetir(iAktiviteID, 7, Genel.DilID).FirstOrDefault().Sonuc);


                PRJ = Genel.PrmDb.S_Proje(Genel.Aktivite_ProjeID).ToList();
                iMaxPesinatSayisi = C.ParseInt(PRJ.FirstOrDefault().PesinatSayisi);
                #endregion





                ///////////////////////// PERFORMANS ARTISI ICIN COKLU SECMELI ALANLARIN DATALARI TOPLU OLARAK CEKEN SP YAPILDI. //////////////////////////////
                // ÖNEMLİ NOT :  KayitID Olarak Aktivitenin proje ID 'si gönderiliyor.Ancak secmelialan ID 50 oldugunda Kayit ID 1 kullanılıyor.
                // Bu kontrol sp içersinde yapılıyor...
                var CokluSecmeliList = Genel.PrmDb.S_AktiviteCokluSecmeliDataGetir(Genel.DilID, Genel.Aktivite_ProjeID, Genel.KullaniciOturumID, Genel.AktifKullaniciID).ToList();
                /////////////////////////////////////////////////////////////





                #region Seçilen projeye bağlı çalısılan doviz tipleri doviz listesine basılıyor...
                var listDoviz = CokluSecmeliList.Where(u => u.SecmeliAlanID == 37);
                cmbDoviz.Properties.DataSource = listDoviz;
                #endregion

                #region Seçilen projenin, proje kartlarından belirlenen ödeme tercihine göre combo doluyor....
                pnlAltBilgi.Height = 30;
                //311 peşin                    
                //312 peşin + banka kredisi
                //313 peşin + vadeli
                //314 peşin + banka kredisi + vadeli
                var ListOdemeTercihleri = CokluSecmeliList.Where(u => u.SecmeliAlanID == 51);
                if (ListOdemeTercihleri.Where(u => u.GetirilenID == 311).Count() < 1) //peşin seçilmemişse...
                {
                    Ayar.InfoMesajGoster(Genel.DilGetirMesaj(3092), Ayar.MesajTipi.Hata, this, 5);
                    this.Close();
                    return;
                }

                cmbOdemeBicimiAna.Properties.DataSource = ListOdemeTercihleri;
                #endregion

                //Genel ayarlar formunda tanımlanan aktivite vadeli ödeme tipleri ilgili comboya basılıyor...
                var lst = CokluSecmeliList.Where(u => u.SecmeliAlanID == 50);
                cmbOdemeSekli.Properties.DataSource = lst;
                cmbOdemeSekli.EditValue = 322;
                cmbGrdOdemeSekli.DataSource = lst;
                /////////////


                dtmIlkTaksitTarihi.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
                dtmIlkTaksitTarihi.DateTime = DateTime.Now;
                dtmTarih.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
                dtmTarih.DateTime = DateTime.Now;





                if (Genel.Aktivite_ProjeID > 0 && iAktiviteID < 1) //INSERT MODE ....
                {
                    OnDegerGetir();
                    AktiviteTableHazirla(true);


                    GayrimenkulEkle(iGayrimenkulID);
                    cmbDoviz.EditValue = C.ParseInt(gvGayrimenkulBilgileri.GetFocusedRowCellValue("SatisFiyatiDovizID"));
                    if (cmbDoviz.EditValue == null || C.ParseInt(cmbDoviz.EditValue) <= 0)
                    {
                        cmbDoviz.ItemIndex = 0;
                        for (int i = 0; i < gvGayrimenkulBilgileri.DataRowCount; i++)
                        {
                            gvGayrimenkulBilgileri.SetRowCellValue(i, "SatisFiyatiDovizID", cmbDoviz.EditValue);
                            gvGayrimenkulBilgileri.SetRowCellValue(i, "ListeFiyatiDovizKodu", cmbDoviz.Text);
                        }
                    }

                    #region Aktivite formu Proje Bazlı SQL kontrolleri açık/kapalı durumları çekiliyor..
                    bIndirimQueryCalissinMi = C.ParseBool(PRJ.FirstOrDefault().IndirimQueryCalissinMi);
                    bKampanyaQueryCalissinMi = C.ParseBool(PRJ.FirstOrDefault().KampanyaQueryCalissinMi);
                    bProjeOzelQueryCalissinMi = C.ParseBool(PRJ.FirstOrDefault().ProjeOzelQueryCalissinMi);
                    bFormQueryCalissinMi = C.ParseBool(PRJ.FirstOrDefault().FormQueryCalissinMi);
                    #endregion
                    txtVadeliFaiz.EditValue = Ayar.AyarGetirSayisal(1); //aylık faiz oranı ayarlardan alınıp gizli txtboxa (txtVadeliFaiz) yazılıyor böylece npv oranları varsayılan olarak hesaplanabilecek.
                }



                if (iAktiviteID > 0)  // UPDATE MODE .....
                {
                    DataGetir();

                    #region Satış tarihi dışında bir tarihteki aktiviteye de girebilir. yetki kontrolü
                    bool bIsAktiviteAdmin = Genel.AktiviteYetkiGetir(AktiviteYetkileri.AdminMi);
                    if (!bIsAktiviteAdmin && !Genel.AktiviteYetkiGetir(AktiviteYetkileri.SatisTarihiDisindaBirTarihtekiAktiviteyeDeGirebilir))
                    {
                        if (dtmTarih.DateTime.Date != DateTime.Now.Date)
                        {
                            this.Cursor = Cursors.Default;
                            Ayar.InfoMesajGoster(Genel.DilGetirMesaj(3108), Ayar.MesajTipi.Bilgi, this, 5);
                            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                        }
                    }
                    #endregion




                    #region Aktivite formu Proje Bazlı SQL kontrolleri açık/kapalı durumları çekiliyor..
                    bIndirimQueryCalissinMi = C.ParseBool(PRJ.FirstOrDefault().IndirimQueryCalissinMi);
                    bKampanyaQueryCalissinMi = C.ParseBool(PRJ.FirstOrDefault().KampanyaQueryCalissinMi);
                    bProjeOzelQueryCalissinMi = C.ParseBool(PRJ.FirstOrDefault().ProjeOzelQueryCalissinMi);
                    bFormQueryCalissinMi = C.ParseBool(PRJ.FirstOrDefault().FormQueryCalissinMi);
                    #endregion

                    btnAktiviteIptal.Visible = true;
                    formYanMenu1.Visible = true;


                    bAktiviteIptalMi = !C.ParseBool(MListler.dsAktivite.Tables[0].Rows[0]["AktifMi"]);
                    btnAktiviteIptal.Enabled = !bAktiviteIptalMi;
                    formYanMenu1.Enabled = !bAktiviteIptalMi;
                    btnKaydet.Enabled = !bAktiviteIptalMi;

                    if (C.ParseInt(cmbAktiviteDurumu.EditValue) == 303) //satıldı ise
                    {
                        colFiyatYenile.Visible = false;
                    }

                }

                GayrimenkulGridiListeFiyatiSonaAl();


                //vadeli odemeler için vadeli odeme panelinin visiblitysi ayarlanıyor...
                if (C.ParseInt(cmbOdemeBicimiAna.EditValue) == 313 || C.ParseInt(cmbOdemeBicimiAna.EditValue) == 314) //vadelilerden biriyse..
                    pnlVadeBilgileri.Visible = true;
                else
                    pnlVadeBilgileri.Visible = false;
                ////////////////


                MListler.dsAktivite_Gayrimenkul.Tables[0].AcceptChanges();
                UstPanelYukseklikAyarla();
                bIsFormLoaded = true;


                YetkiAyarla();
                Genel.PasifKontrolleriKapatVeZorunluAlanRenklendir(this, iFormID);
                FormReadOnlyAyarla();


                //AyarBooldan NPV görünür ayarına göre acılıp kapanıyor...
                bool bNPVGorunur = Ayar.AyarGetirBool(18);
                colNPV.Visible = bNPVGorunur;
                colNPV.OptionsColumn.ShowInCustomizationForm = bNPVGorunur;
                colNPV.OptionsColumn.ShowInExpressionEditor = bNPVGorunur;

                colNPVM2.Visible = bNPVGorunur;
                colNPVM2.OptionsColumn.ShowInCustomizationForm = bNPVGorunur;
                colNPVM2.OptionsColumn.ShowInExpressionEditor = bNPVGorunur;

                pnlNPV.Visible = bNPVGorunur;
                /////////////////////////////////////////////////////////////


                Genel.bAktiviteYanMenuReadOnly = bFormReadOnly;

                formFooter1._SureText = Genel.YuklemeSuresiGetir(0, "");
                this.Cursor = Cursors.Default;
            }
            catch (Exception Hata)
            {
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GayrimenkulGridiListeFiyatiSonaAl()
        {
            try
            {
                int iVisibleKolonSayisi = gvGayrimenkulBilgileri.VisibleColumns.Count;
                colListeFiyati.VisibleIndex = iVisibleKolonSayisi - 1;
                colFiyatYenile.VisibleIndex = iVisibleKolonSayisi;
            }
            catch (Exception Hata)
            {
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void UstPanelYukseklikAyarla()
        {
            try
            {
                int iGridSatirSayisi = gvGayrimenkulBilgileri.RowCount;

                if (gvMusteriBilgileri.RowCount > iGridSatirSayisi)
                    iGridSatirSayisi = gvMusteriBilgileri.RowCount;




                if (iGridSatirSayisi > 1)
                    pnlUst.Height = 124;
                else
                    pnlUst.Height = 102;


            }
            catch (Exception Hata)
            {
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public void DataGetir()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;



                MListler.dsAktivite.Tables.Clear();
                DataTable dt1 = Genel.ListToDt(Genel.PrmDb.S_Aktivite(iAktiviteID).ToList());
                MListler.dsAktivite.Tables.Add(dt1);



                BaslikAyarla();


                #region Aktivite alanlarının değerleri ilgili kontrollere yazılıyor.
                DataRow drwAktivite = MListler.dsAktivite.Tables[0].Rows[0];
                cmbAktiviteDurumu.EditValue = C.ParseInt(drwAktivite["HareketTipiKisID_300"]);
                cmbOdemeBicimiAna.EditValue = C.ParseInt(drwAktivite["OdemeTahutuKisID_301"]);
                cmbIndirim.EditValue = C.ParseInt(drwAktivite["AktiviteIndirimID"]);
                cmbKampanya.EditValue = C.ParseInt(drwAktivite["AktiviteKampanyaID"]);
                dtmTarih.EditValue = C.ParseDateTimeOrNull(drwAktivite["AktiviteTarihi"]);
                cmbDoviz.EditValue = C.ParseInt(drwAktivite["SatisFiyatiDovizID"]);
                txtListeFiyati.EditValue = C.ParseDecimal(drwAktivite["ListeFiyatiKur"]);
                txtSatisFiyati.EditValue = C.ParseDecimal(drwAktivite["SatisFiyati"]);

                txtTanimlananIndirim.EditValue = C.ParseDecimal(drwAktivite["TanimIndirimTutari"]);
                txtHesaplananIndirim.EditValue = C.ParseDecimal(drwAktivite["HesaplananIndirimTutari"]);
                txtTanimlananVadeFarki.EditValue = C.ParseDecimal(drwAktivite["TanimVadeFarkiTutari"]);
                txtHesaplananVadeFarki.EditValue = C.ParseDecimal(drwAktivite["HesaplananVadeFarkiTutari"]);
                txtVadeliFaiz.EditValue = C.ParseDecimal(drwAktivite["AylikBrutFaiz"]);

                txtRezerveSuresi.EditValue = C.ParseInt(drwAktivite["RezerveSuresi"]);

                #region Peşinatlar ve Banka Kredisi yazılıyor...
                txtPesinat1.EditValue = C.ParseDecimal(drwAktivite["PlanPesinat01"]);
                dtmPesinat1.EditValue = C.ParseDateTimeOrNull(drwAktivite["PlanPesinat01Tarih"]);
                txtPesinat2.EditValue = C.ParseDecimal(drwAktivite["PlanPesinat02"]);
                dtmPesinat2.EditValue = C.ParseDateTimeOrNull(drwAktivite["PlanPesinat02Tarih"]);
                txtPesinat3.EditValue = C.ParseDecimal(drwAktivite["PlanPesinat03"]);
                dtmPesinat3.EditValue = C.ParseDateTimeOrNull(drwAktivite["PlanPesinat03Tarih"]);
                txtPesinat4.EditValue = C.ParseDecimal(drwAktivite["PlanPesinat04"]);
                dtmPesinat4.EditValue = C.ParseDateTimeOrNull(drwAktivite["PlanPesinat04Tarih"]);
                txtPesinat5.EditValue = C.ParseDecimal(drwAktivite["PlanPesinat05"]);
                dtmPesinat5.EditValue = C.ParseDateTimeOrNull(drwAktivite["PlanPesinat05Tarih"]);
                txtPesinat6.EditValue = C.ParseDecimal(drwAktivite["PlanPesinat06"]);
                dtmPesinat6.EditValue = C.ParseDateTimeOrNull(drwAktivite["PlanPesinat06Tarih"]);
                txtPesinat7.EditValue = C.ParseDecimal(drwAktivite["PlanPesinat07"]);
                dtmPesinat7.EditValue = C.ParseDateTimeOrNull(drwAktivite["PlanPesinat07Tarih"]);
                txtPesinat8.EditValue = C.ParseDecimal(drwAktivite["PlanPesinat08"]);
                dtmPesinat8.EditValue = C.ParseDateTimeOrNull(drwAktivite["PlanPesinat08Tarih"]);
                txtPesinat9.EditValue = C.ParseDecimal(drwAktivite["PlanPesinat09"]);
                dtmPesinat9.EditValue = C.ParseDateTimeOrNull(drwAktivite["PlanPesinat09Tarih"]);
                txtPesinat10.EditValue = C.ParseDecimal(drwAktivite["PlanPesinat10"]);
                dtmPesinat10.EditValue = C.ParseDateTimeOrNull(drwAktivite["PlanPesinat10Tarih"]);

                txtBankaKredisi.EditValue = C.ParseDecimal(drwAktivite["PlanBanka"]).ToString("n2");
                dtmBankaKredisi.EditValue = C.ParseDateTimeOrNull(drwAktivite["PlanBankaTarih"]);
                txtVadeli.EditValue = C.ParseDecimal(drwAktivite["PlanVadeli"]);

                DataTable dtOdemePlaniPesinatBanka = MListler.dsAktivite_OdemePlaniPesinatBanka.Tables[0];
                int iSayac = 0;
                for (int i = 0; i < dtOdemePlaniPesinatBanka.Rows.Count - 1; i++)
                {
                    if (C.ParseDecimal(dtOdemePlaniPesinatBanka.Rows[i]["BorcTutari"]) <= 0)
                        continue;

                    iSayac++;
                }

                iPesinatSayisi = iSayac;

                PesinatPanelleriniAyarla();
                #endregion
                #endregion




                MListler.dsAktivite_Gayrimenkul.Tables.Clear();
                DataTable dt2 = Genel.ListToDt(Genel.PrmDb.S_AktiviteGayrimenkul(iAktiviteID, Genel.DilID).ToList());
                MListler.dsAktivite_Gayrimenkul.Tables.Add(dt2);

                MListler.dsAktivite_Musteri.Tables.Clear();
                DataTable dt3 = Genel.ListToDt(Genel.PrmDb.S_AktiviteMusteri(iAktiviteID, Genel.DilID).ToList());
                MListler.dsAktivite_Musteri.Tables.Add(dt3);

                MListler.dsAktivite_OdemePlaniPesinatBanka.Tables.Clear();
                DataTable dt5 = Genel.ListToDt(Genel.PrmDb.S_AktiviteOdemePlani(2, iAktiviteID, Genel.DilID).ToList());
                MListler.dsAktivite_OdemePlaniPesinatBanka.Tables.Add(dt5);


                MListler.dsAktivite_OdemePlaniSenet.Tables.Clear();
                DataTable dt4 = Genel.ListToDt(Genel.PrmDb.S_AktiviteOdemePlani(1, iAktiviteID, Genel.DilID).ToList());
                MListler.dsAktivite_OdemePlaniSenet.Tables.Add(dt4);



                grdGayrimenkulBilgileri.DataSource = MListler.dsAktivite_Gayrimenkul.Tables[0];
                grdMusteriBilgileri.DataSource = MListler.dsAktivite_Musteri.Tables[0];
                grdVadeliOdemeBilgileri.DataSource = MListler.dsAktivite_OdemePlaniSenet.Tables[0];


                //Kayıt işleminden sonra durumu satıldı ise, aktivite durumunu bir daha değiştiremez.bu nedenle cmbAktiviteDurum kapatılıyor.
                if (C.ParseInt(cmbAktiviteDurumu.EditValue) == 303) //SATILDI.
                {
                    cmbAktiviteDurumu.Enabled = false;
                }

                //Kampanya ve indirim kitleniyor...
                if (iAktiviteID > 0)
                {
                    cmbIndirim.Enabled = false;
                    cmbKampanya.Enabled = false;
                }
                //////////////////////////////////////


                this.Cursor = Cursors.Default;
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PesinatPanelleriniAyarla()
        {
            try
            {
                if (iPesinatSayisi == 0)
                    iPesinatSayisi++;

                if (iPesinatSayisi == 1)
                {
                    scPesinatlar.VerticalScroll.Visible = false;
                    btnPesinatEkle.Enabled = true;
                    btnPesinatSil.Enabled = false;
                }
                else if (iPesinatSayisi == 2)
                {
                    scPesinatlar.VerticalScroll.Visible = false;
                    btnPesinatEkle.Enabled = true;
                    btnPesinatSil.Enabled = true;
                }
                else if (iPesinatSayisi == 3)
                {
                    scPesinatlar.VerticalScroll.Visible = false;
                    btnPesinatEkle.Enabled = true;
                    btnPesinatSil.Enabled = true;
                }
                else if (iPesinatSayisi == 10)
                {
                    scPesinatlar.VerticalScroll.Visible = true;
                    btnPesinatEkle.Enabled = false;
                    btnPesinatSil.Enabled = true;
                }
                else
                {
                    scPesinatlar.VerticalScroll.Visible = true;
                    btnPesinatEkle.Enabled = true;
                    btnPesinatSil.Enabled = true;
                }

                for (int i = 1; i <= 10; i++) //önce tüm pesinat panelleri kapatılıyor...
                {
                    Control[] ctrl = this.Controls.Find("pnlPesinat" + (i).ToString(), true);
                    if (ctrl.Length > 0)
                    {
                        ctrl[0].Visible = false;
                    }
                }

                for (int i = 1; i <= iPesinatSayisi; i++) //peşinat sayısı kadar panel acılıyor.
                {
                    Control[] ctrl = this.Controls.Find("pnlPesinat" + (i).ToString(), true);
                    if (ctrl.Length > 0)
                    {
                        ctrl[0].Visible = true;
                        ctrl[0].BringToFront();
                    }
                }


                if (iPesinatSayisi == iMaxPesinatSayisi)
                {
                    btnPesinatEkle.Enabled = false;
                }

                //pnlPesinatlar yüksekliği ayarlanıyor...               
                if (iPesinatSayisi == 1)
                    pnlPesinatlar.Height = 49;
                else if (iPesinatSayisi == 2)
                    pnlPesinatlar.Height = 73;
                else
                    pnlPesinatlar.Height = 97;

            }
            catch (Exception Hata)
            {
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void btn_Click(object sender, EventArgs e)
        {
            try
            {

                if (iAktiviteID <= 0)
                    return;



                int iKisID = C.ParseInt(((SimpleButton)sender).Tag);
                switch (iKisID)
                {
                    case 600:
                        OdemelerTahsilatlarClick();
                        break;
                    case 601:
                        EvraklarClick();
                        break;
                    case 602:
                        DigerDatalarClick();
                        break;
                    case 603:
                        IadeEdilenOdemeClick();
                        break;
                    case 604:
                        DosyalarClick();
                        break;
                    case 605:
                        SatisSonrasiIslemlerClick();
                        break;
                    case 606:
                        GayrimenkulBedeliDisindakiOdemelerClick();
                        break;
                    case 607:
                        GayrimenkulDataClick();
                        break;

                }
            }
            catch (Exception Hata)
            {
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void GridAyarGetir()
        {
            try
            {
                //grid layout yükleniyor..
                object oStream = GridAyarlari.KullaniciFormGridAyariGetir(gvGayrimenkulBilgileri, this.Name);
                if (oStream != null)
                    gvGayrimenkulBilgileri.RestoreLayoutFromStream(GridAyarlari.KullaniciFormGridAyariGetir(gvGayrimenkulBilgileri, this.Name));

                //grid layout yükleniyor..
                object oStream2 = GridAyarlari.KullaniciFormGridAyariGetir(gvMusteriBilgileri, this.Name);
                if (oStream2 != null)
                    gvMusteriBilgileri.RestoreLayoutFromStream(GridAyarlari.KullaniciFormGridAyariGetir(gvMusteriBilgileri, this.Name));

                //grid layout yükleniyor..
                object oStream3 = GridAyarlari.KullaniciFormGridAyariGetir(gvVadeliOdemeBilgileri, this.Name);
                if (oStream3 != null)
                    gvVadeliOdemeBilgileri.RestoreLayoutFromStream(GridAyarlari.KullaniciFormGridAyariGetir(gvVadeliOdemeBilgileri, this.Name));
            }
            catch (Exception Hata)
            {
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private XtraTabPage TabPageGetir(string strPageName)
        {
            try
            {
                foreach (XtraTabPage page in tc.TabPages)
                {
                    if (page.Name == strPageName)
                        return page;
                }
                return null;
            }
            catch (Exception Hata)
            {
                return null;
            }
        }
        private XtraForm FormBul(string FormAdi)
        {
            try
            {
                if (FormAdi == "") return null;
                string FormTypeFullName = string.Format("{0}.{1}", this.GetType().Namespace, FormAdi);
                Type type = Type.GetType(FormTypeFullName, true);
                XtraForm frm = (XtraForm)Activator.CreateInstance(type);
                //var FormDil = Genel.PrmDb.S_DilCevirFormMenu(FormAdi).ToList();
                frm.Text = Genel.DilGetirBaslik(0, frm.Text);
                return frm;
            }
            catch (Exception Hata)
            {
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void TabdaFormAc(string strFormName)
        {
            try
            {

                #region Açılmak istenen form zaten açıksa tab pageine konumlanıp fonksiyondan çıkılıyor...
                foreach (XtraTabPage page in tc.TabPages)
                {
                    string strPageName = page.Name;

                    if (strPageName == strFormName)
                    {
                        tc.SelectedTabPage = TabPageGetir(strPageName);
                        return;
                    }
                }
                #endregion

                #region İlgili Form ve Tabpage oluşturuluyor...
                XtraForm frm = FormBul(strFormName);
                XtraTabPage tp = new XtraTabPage();
                tp.Name = frm.Name;
                tc.TabPages.Add(tp);
                frm.TopLevel = false;
                frm.Visible = true;
                frm.FormBorderStyle = FormBorderStyle.None;
                tc.SelectedTabPage = tc.TabPages[tc.TabPages.Count - 1]; //eklenen form tabı seçili hale getiriliyor...

                #endregion

                #region İlgili tabpage içine ScrollableControl oluşturuluyor.Ve içine form ekleniyor...
                if (frm.Name != "F_AktiviteDigerDatalar")
                {
                    XtraScrollableControl scroll = new XtraScrollableControl();
                    tp.Controls.Add(scroll);
                    scroll.Name = "Scroll" + tc.SelectedTabPageIndex;
                    scroll.Dock = DockStyle.Fill;
                    tc.TabPages[tc.TabPages.Count - 1].Text = frm.Text;
                    scroll.Controls.Add(frm);
                }
                else if (frm.Name == "F_AktiviteDigerDatalar")
                {
                    tc.TabPages[tc.TabPages.Count - 1].Text = frm.Text;
                    tp.Controls.Add(frm);
                    frm.Dock = DockStyle.Fill;

                }
                #endregion
            }
            catch (Exception Hata)
            {
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }






        private void tc_CloseButtonClick(object sender, EventArgs e)
        {
            try
            {
                if (tc.SelectedTabPageIndex > 1)
                {
                    tc.SelectedTabPage.Controls[0].Dispose();
                    tc.TabPages.Remove(tc.SelectedTabPage);
                }
            }
            catch (Exception Hata)
            {
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tc_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            try
            {
                tc.ClosePageButtonShowMode = ClosePageButtonShowMode.InActiveTabPageHeader;
                if (tc.SelectedTabPageIndex < 2)
                    tc.SelectedTabPage.ShowCloseButton = DevExpress.Utils.DefaultBoolean.False;
                else
                    tc.SelectedTabPage.ShowCloseButton = DevExpress.Utils.DefaultBoolean.True;

                if (tc.SelectedTabPageIndex == 1)
                {
                    MaliDurumHesapla();
                }
            }
            catch (Exception Hata)
            {
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void AktiviteTableHazirla(bool FormLoadIcin)
        {
            try
            {
                DataRow drwYeni;

                MListler.dsAktivite.Tables[0].BeginLoadData();



                if (FormLoadIcin)
                {
                    MListler.dsAktivite.Tables[0].Rows.Clear();
                    drwYeni = MListler.dsAktivite.Tables[0].NewRow();
                }
                else
                {
                    drwYeni = MListler.dsAktivite.Tables[0].Rows[0];
                }



                string strMusteriAdSoyadlariToplu = "";
                for (int i = 0; i < gvMusteriBilgileri.DataRowCount; i++)
                {
                    strMusteriAdSoyadlariToplu += gvMusteriBilgileri.GetRowCellValue(i, "MusteriAdiSoyadi").ToString() + ",";
                }

                if (strMusteriAdSoyadlariToplu.Length > 0 && strMusteriAdSoyadlariToplu.Substring(strMusteriAdSoyadlariToplu.Length - 1, 1) == ",")
                    strMusteriAdSoyadlariToplu = strMusteriAdSoyadlariToplu.Remove(strMusteriAdSoyadlariToplu.Length - 1, 1);


                #region Aktivite Tablosu Yeni Satır Alanları...
                //T_Aktivite
                drwYeni["ID"] = iAktiviteID;
                drwYeni["HareketTipiKisID_300"] = C.ParseInt(cmbAktiviteDurumu.EditValue);
                drwYeni["AktifMi"] = !bAktiviteIptalMi;
                drwYeni["AktiviteStatuKisID_308"] = -1; //içerde hesaplanıyor.
                drwYeni["ListeFiyatiKur"] = C.ParseDecimal(txtListeFiyati.EditValue);
                drwYeni["SatisFiyati"] = C.ParseDecimal(txtSatisFiyati.EditValue);
                drwYeni["SatisFiyatiDovizID"] = C.ParseInt(cmbDoviz.EditValue);
                drwYeni["OdemeTahutuKisID_301"] = C.ParseInt(cmbOdemeBicimiAna.EditValue);
                drwYeni["PlanPesinatToplam"] = ToplamPesinatTutariGetir();
                drwYeni["PlanPesinat01"] = C.ParseDecimal(txtPesinat1.EditValue);
                drwYeni["PlanPesinat02"] = C.ParseDecimal(txtPesinat2.EditValue);
                drwYeni["PlanPesinat03"] = C.ParseDecimal(txtPesinat3.EditValue);
                drwYeni["PlanPesinat04"] = C.ParseDecimal(txtPesinat4.EditValue);
                drwYeni["PlanPesinat05"] = C.ParseDecimal(txtPesinat5.EditValue);
                drwYeni["PlanPesinat06"] = C.ParseDecimal(txtPesinat6.EditValue);
                drwYeni["PlanPesinat07"] = C.ParseDecimal(txtPesinat7.EditValue);
                drwYeni["PlanPesinat08"] = C.ParseDecimal(txtPesinat8.EditValue);
                drwYeni["PlanPesinat09"] = C.ParseDecimal(txtPesinat9.EditValue);
                drwYeni["PlanPesinat10"] = C.ParseDecimal(txtPesinat10.EditValue);
                drwYeni["PlanBanka"] = C.ParseDecimal(txtBankaKredisi.EditValue);
                drwYeni["PlanBankaTarih"] = C.ParseDateTimeOrDBNULL(dtmBankaKredisi.EditValue);
                drwYeni["PlanVadeli"] = C.ParseDecimal(txtVadeli.EditValue);
                drwYeni["AktiviteTarihi"] = C.ParseDateTimeOrDBNULL(dtmTarih.DateTime.Date);
                drwYeni["SozlesmeYapildiMi"] = C.ParseBool(drwYeni["SozlesmeYapildiMi"]);
                drwYeni["SozlesmeTarihi"] = C.ParseDateTimeOrDBNULL(drwYeni["SozlesmeTarihi"]);
                drwYeni["PlanPesinat01Tarih"] = C.ParseDateTimeOrDBNULL(dtmPesinat1.EditValue);
                drwYeni["PlanPesinat02Tarih"] = C.ParseDateTimeOrDBNULL(dtmPesinat2.EditValue);
                drwYeni["PlanPesinat03Tarih"] = C.ParseDateTimeOrDBNULL(dtmPesinat3.EditValue);
                drwYeni["PlanPesinat04Tarih"] = C.ParseDateTimeOrDBNULL(dtmPesinat4.EditValue);
                drwYeni["PlanPesinat05Tarih"] = C.ParseDateTimeOrDBNULL(dtmPesinat5.EditValue);
                drwYeni["PlanPesinat06Tarih"] = C.ParseDateTimeOrDBNULL(dtmPesinat6.EditValue);
                drwYeni["PlanPesinat07Tarih"] = C.ParseDateTimeOrDBNULL(dtmPesinat7.EditValue);
                drwYeni["PlanPesinat08Tarih"] = C.ParseDateTimeOrDBNULL(dtmPesinat8.EditValue);
                drwYeni["PlanPesinat09Tarih"] = C.ParseDateTimeOrDBNULL(dtmPesinat9.EditValue);
                drwYeni["PlanPesinat10Tarih"] = C.ParseDateTimeOrDBNULL(dtmPesinat10.EditValue);

                drwYeni["OfisID"] = Genel.AktifPersonelOfisID;
                drwYeni["AnaMusteriID"] = C.ParseInt(gvMusteriBilgileri.GetRowCellValue(0, "MusteriID"));
                drwYeni["MusteriAdSoyadlariToplu"] = strMusteriAdSoyadlariToplu;
                drwYeni["NpvTutari"] = C.ParseDecimal(lblMD_NPVDeger.Text); //npv yazılacak


                drwYeni["OrtalamaVadeTarihi"] = C.ParseDateTimeOrDBNULL(lblMD_OrtalamaVadeDeger.Text); //ort.vade t. 
                drwYeni["TanimVadeFarkiTutari"] = C.ParseDecimal(txtTanimlananVadeFarki.EditValue);
                drwYeni["TanimIndirimTutari"] = C.ParseDecimal(txtTanimlananIndirim.EditValue);
                drwYeni["DigerYazilimID"] = ""; //elle girilecek
                drwYeni["MusteriIletisimID"] = -1; //elle girilecek
                drwYeni["SatisPersonelID1"] = C.ParseInt(drwYeni["SatisPersonelID1"]) <= 0 ? Genel.AktifPersonelID : C.ParseInt(drwYeni["SatisPersonelID1"]);
                drwYeni["SatisPersonelID2"] = C.ParseInt(drwYeni["SatisPersonelID2"]); // elle giriliyor.
                drwYeni["AktivitePersonelID"] = Genel.AktifPersonelID;
                drwYeni["Araci1AcentaID"] = C.ParseInt(drwYeni["Araci1AcentaID"]);
                drwYeni["Araci2AcentaID"] = C.ParseInt(drwYeni["Araci2AcentaID"]);
                drwYeni["HesaplananVadeFarkiTutari"] = C.ParseDecimal(txtHesaplananVadeFarki.EditValue);
                drwYeni["HesaplananIndirimTutari"] = C.ParseDecimal(txtHesaplananIndirim.EditValue);
                drwYeni["ToplamVadeFarkiTutari"] = (C.ParseDecimal(txtHesaplananVadeFarki.EditValue) + C.ParseDecimal(txtTanimlananVadeFarki.EditValue));
                drwYeni["ToplamIndirimTutari"] = C.ParseDecimal(txtTanimlananIndirim.EditValue) + C.ParseDecimal(txtHesaplananIndirim.EditValue);
                drwYeni["AktiviteIndirimID"] = C.ParseInt(cmbIndirim.EditValue);
                drwYeni["AktiviteKampanyaID"] = C.ParseInt(cmbKampanya.EditValue);

                drwYeni["RezerveSuresi"] = C.ParseInt(txtRezerveSuresi.EditValue);
                drwYeni["MusteriKaynagiID_1"] = C.ParseInt(drwYeni["MusteriKaynagiID_1"]);
                drwYeni["MusteriKaynagiID_2"] = C.ParseInt(drwYeni["MusteriKaynagiID_2"]);



                //T_AktiviteBakiye - sqlde hesaplanıyor
                drwYeni["Odedigi"] = 0;
                drwYeni["OdedigiDepozitosuz"] = 0;
                drwYeni["DepozitoOdenen"] = 0;
                drwYeni["KalanBorcu"] = 0;
                drwYeni["Odenecek30"] = 0;
                drwYeni["Odenecek60"] = 0;
                drwYeni["Odenecek90"] = 0;
                drwYeni["Odenecek180"] = 0;
                drwYeni["TahsilatPesinat"] = 0;
                drwYeni["TahsilatBanka"] = 0;
                drwYeni["TahsilatVadeli"] = 0;
                drwYeni["TahsilatKDV"] = 0;
                drwYeni["BankaKredisiPlan"] = 0;
                drwYeni["BankaKredisiOdeme"] = 0;
                drwYeni["BankaKredisiBakiye"] = 0;
                drwYeni["OdedigiVadeFarkiTutari"] = 0;
                drwYeni["OdedigiVadeFarkliTutar"] = 0;
                drwYeni["OdedigiYuzde"] = 0;
                drwYeni["ToplamVadeFarki"] = 0;
                drwYeni["KalanBorcuVadeFarkliTutar"] = 0;

                //T_AktiviteBanka
                drwYeni["BankaVadeliOdemeOnaylandiMi"] = C.ParseDecimal(drwYeni["BankaVadeliOdemeOnaylandiMi"]);
                drwYeni["BankaVadeliOdemeOnayTarihi"] = C.ParseDateTimeOrDBNULL(drwYeni["BankaVadeliOdemeOnayTarihi"]);
                drwYeni["BankaKatkipayiOdemesiDurumuKisID_44"] = C.ParseInt(drwYeni["BankaKatkipayiOdemesiDurumuKisID_44"]);
                drwYeni["KrediKullanilanBankaDurumSecID_45"] = C.ParseInt(drwYeni["KrediKullanilanBankaDurumSecID_45"]);
                drwYeni["KrediKullanilanBankaSubeDurumSecID_63"] = C.ParseInt(drwYeni["KrediKullanilanBankaSubeDurumSecID_63"]);
                drwYeni["KrediVade"] = C.ParseDecimal(drwYeni["KrediVade"]);
                drwYeni["KrediOran"] = C.ParseDecimal(drwYeni["KrediOran"]);
                drwYeni["KrediAylikOdeme"] = C.ParseDecimal(drwYeni["KrediAylikOdeme"]);
                drwYeni["KrediToplamOdeme"] = C.ParseDecimal(drwYeni["KrediToplamOdeme"]);
                drwYeni["KrediMusteriOran"] = C.ParseDecimal(drwYeni["KrediMusteriOran"]);
                drwYeni["KrediBankaMusteriNo"] = drwYeni["KrediBankaMusteriNo"] == null ? "" : drwYeni["KrediBankaMusteriNo"].ToString();
                drwYeni["KrediKullandirimTarihi"] = C.ParseDateTimeOrDBNULL(drwYeni["KrediKullandirimTarihi"]);
                drwYeni["KrediIpotekEvragiGeldiMi"] = C.ParseBool(drwYeni["KrediIpotekEvragiGeldiMi"]);
                drwYeni["KrediBankaYazisiGeldiMi"] = C.ParseBool(drwYeni["KrediBankaYazisiGeldiMi"]);
                drwYeni["KrediBankaTapudaIpotekKoyduMu"] = C.ParseBool(drwYeni["KrediBankaTapudaIpotekKoyduMu"]);
                drwYeni["KrediBankaIpotekTutari"] = C.ParseDecimal(drwYeni["KrediBankaIpotekTutari"]);
                drwYeni["BankaKredisiEvrakDurumKisID_54"] = C.ParseInt(drwYeni["BankaKredisiEvrakDurumKisID_54"]);
                drwYeni["KrediKomisyonOrani"] = C.ParseDecimal(drwYeni["KrediKomisyonOrani"]);
                drwYeni["BankaPesinatTutari"] = C.ParseDecimal(drwYeni["BankaPesinatTutari"]);
                drwYeni["BankaIstenilenKrediTutari"] = C.ParseDecimal(drwYeni["BankaIstenilenKrediTutari"]);

                //T_AktiviteDetay
                drwYeni["VekaletnameAlindiMi"] = C.ParseBool(drwYeni["VekaletnameAlindiMi"]);
                drwYeni["VadeliSatisIpotegiVarMi"] = C.ParseBool(drwYeni["VadeliSatisIpotegiVarMi"]);
                drwYeni["IpotekDerecesiDurumSecID_5"] = C.ParseInt(drwYeni["IpotekDerecesiDurumSecID_5"]);
                drwYeni["IpotekTutari"] = C.ParseDecimal(drwYeni["IpotekTutari"]);
                drwYeni["IpotekTutariDovizID"] = C.ParseInt(drwYeni["IpotekTutariDovizID"]);
                drwYeni["TapuTeslimIcinHazirMi"] = C.ParseBool(drwYeni["TapuTeslimIcinHazirMi"]);
                drwYeni["TapuIslemleriIcinHazir"] = C.ParseBool(drwYeni["TapuIslemleriIcinHazir"]);
                drwYeni["TapuTeslimOlduMu"] = C.ParseBool(drwYeni["TapuTeslimOlduMu"]);
                drwYeni["TapuTeslimTarihi"] = C.ParseDateTimeOrDBNULL(drwYeni["TapuTeslimTarihi"]);
                drwYeni["IbranameAlindiMi"] = C.ParseBool(drwYeni["IbranameAlindiMi"]);
                drwYeni["MuhasebeKodu"] = drwYeni["MuhasebeKodu"] == null ? "" : drwYeni["MuhasebeKodu"].ToString();
                drwYeni["SatisNo"] = drwYeni["SatisNo"] == null ? "" : drwYeni["SatisNo"].ToString();
                drwYeni["MusteriNo"] = drwYeni["MusteriNo"] == null ? "" : drwYeni["MusteriNo"].ToString();
                drwYeni["ProjeKayitNumarasi"] = C.ParseInt(drwYeni["ProjeKayitNumarasi"]);
                drwYeni["SerefiyeBedeli"] = C.ParseDecimal(drwYeni["SerefiyeBedeli"]);
                drwYeni["AylikBrutFaiz"] = C.ParseDecimal(txtVadeliFaiz.EditValue);
                drwYeni["SatisdaReferansOlanMusteriID"] = C.ParseInt(drwYeni["SatisdaReferansOlanMusteriID"]);
                drwYeni["BrutFaizTutari"] = C.ParseDecimal(drwYeni["BrutFaizTutari"]);
                drwYeni["NoterSozlesmesiYapildiMi"] = C.ParseBool(drwYeni["NoterSozlesmesiYapildiMi"]);
                drwYeni["NoterSozlesmesiRandevuTarihSaati"] = C.ParseDateTimeOrDBNULL(drwYeni["NoterSozlesmesiRandevuTarihSaati"]);
                drwYeni["NoterSozlesmesiNotu"] = drwYeni["NoterSozlesmesiNotu"] == null ? "" : drwYeni["NoterSozlesmesiNotu"].ToString();
                drwYeni["NoterSatisiYevmiyeNo"] = drwYeni["NoterSatisiYevmiyeNo"] == null ? "" : drwYeni["NoterSatisiYevmiyeNo"].ToString();
                drwYeni["NoterDurumSecID_62"] = C.ParseInt(drwYeni["NoterDurumSecID_62"]);
                drwYeni["MuhasebeKodu136"] = drwYeni["MuhasebeKodu136"] == null ? "" : drwYeni["MuhasebeKodu136"].ToString();
                drwYeni["MuhasebeKodu120"] = drwYeni["MuhasebeKodu120"] == null ? "" : drwYeni["MuhasebeKodu120"].ToString();
                drwYeni["ReferansOlanMusteriID"] = C.ParseInt(drwYeni["ReferansOlanMusteriID"]);
                drwYeni["IsimDegisikligiDevirOlarakYapildiMi"] = C.ParseBool(drwYeni["IsimDegisikligiDevirOlarakYapildiMi"]);
                drwYeni["OdemeIadesiYapilacakMi"] = C.ParseBool(drwYeni["OdemeIadesiYapilacakMi"]);
                drwYeni["EkProtokolEvragiAlindiMi"] = C.ParseBool(drwYeni["EkProtokolEvragiAlindiMi"]);
                drwYeni["EkProtokolEvragiTarihi"] = C.ParseDateTimeOrDBNULL(drwYeni["EkProtokolEvragiTarihi"]);
                drwYeni["IpotekTarihi"] = C.ParseDateTimeOrDBNULL(drwYeni["IpotekTarihi"]);
                drwYeni["DbsOnaylandiMi"] = C.ParseBool(drwYeni["DbsOnaylandiMi"]);
                drwYeni["DbsOnayTarihi"] = C.ParseDateTimeOrDBNULL(drwYeni["DbsOnayTarihi"]);
                drwYeni["NoterSozlesmesiniImzalayanDurumSecID_22"] = C.ParseInt(drwYeni["NoterSozlesmesiniImzalayanDurumSecID_22"]);
                drwYeni["VekilMusteriID"] = C.ParseInt(drwYeni["VekilMusteriID"]);
                drwYeni["VesayetMusteriID"] = C.ParseInt(drwYeni["VesayetMusteriID"]);
                drwYeni["KefilMusteriID"] = C.ParseInt(drwYeni["KefilMusteriID"]);

                //T_AktiviteIptal
                drwYeni["IptalNedenIDKisID_6"] = C.ParseInt(drwYeni["IptalNedenIDKisID_6"]);
                drwYeni["IptalTarihi"] = C.ParseDateTimeOrDBNULL(drwYeni["IptalTarihi"]);
                drwYeni["IptalEdenPersonel"] = drwYeni["IptalEdenPersonel"] == null ? "" : drwYeni["IptalEdenPersonel"].ToString();
                drwYeni["IptalTuruDurumSec_ID_49"] = drwYeni["IptalTuruDurumSec_ID_49"];
                drwYeni["IptalAciklama"] = drwYeni["IptalAciklama"] == null ? "" : drwYeni["IptalAciklama"].ToString();
                drwYeni["IptalIadeHesapSahibi"] = drwYeni["IptalIadeHesapSahibi"] == null ? "" : drwYeni["IptalIadeHesapSahibi"].ToString();
                drwYeni["IptalIadeBanka"] = drwYeni["IptalIadeBanka"] == null ? "" : drwYeni["IptalIadeBanka"].ToString();
                drwYeni["IptalIadeSubeSubeKodu"] = drwYeni["IptalIadeSubeSubeKodu"] == null ? "" : drwYeni["IptalIadeSubeSubeKodu"].ToString();
                drwYeni["IptalIadeIBAN"] = drwYeni["IptalIadeIBAN"] == null ? "" : drwYeni["IptalIadeIBAN"].ToString();
                drwYeni["OdemeIadesiKesintiTutari"] = C.ParseDecimal(drwYeni["OdemeIadesiKesintiTutari"]);
                drwYeni["IptalYerDegisikligiGyr_AktiviteID"] = C.ParseInt(drwYeni["IptalYerDegisikligiGyr_AktiviteID"]);
                drwYeni["IptalYerDegisikligiDurumu"] = C.ParseInt(drwYeni["IptalYerDegisikligiDurumu"]);
                drwYeni["OdemeIadesiPlanlananTarih"] = C.ParseDateTimeOrDBNULL(drwYeni["OdemeIadesiPlanlananTarih"]);


                //T_ProjeSatisKosullari
                drwYeni["IndirimQueryCalissinMi"] = bIndirimQueryCalissinMi;
                drwYeni["KampanyaQueryCalissinMi"] = bKampanyaQueryCalissinMi;
                drwYeni["ProjeOzelQueryCalissinMi"] = bProjeOzelQueryCalissinMi;
                drwYeni["FormQueryCalissinMi"] = bFormQueryCalissinMi;



                //T_AktivitePrim
                drwYeni["AcentaninKomisyonYuzdesi"] = C.ParseDecimal(drwYeni["AcentaninKomisyonYuzdesi"]);
                drwYeni["AcentaninVazgectigiKomisyonTutari"] = C.ParseDecimal(drwYeni["AcentaninVazgectigiKomisyonTutari"]);
                drwYeni["AcentaPrimHesaplamaTipiKisID_29"] = C.ParseInt(drwYeni["AcentaPrimHesaplamaTipiKisID_29"]);
                drwYeni["AcentaKomisyonu"] = C.ParseDecimal(drwYeni["AcentaKomisyonu"]);
                drwYeni["AcentaOdemeHakedisYuzdesi"] = C.ParseDecimal(drwYeni["AcentaOdemeHakedisYuzdesi"]);
                drwYeni["AcentaOdemeGunuArti"] = C.ParseInt(drwYeni["AcentaOdemeGunuArti"]);
                drwYeni["AcentaKomisyon2"] = C.ParseDecimal(drwYeni["AcentaKomisyon2"]);
                drwYeni["AcentaOdemeHakedisYuzdesi2"] = C.ParseDecimal(drwYeni["AcentaOdemeHakedisYuzdesi2"]);
                drwYeni["AcentaKomisyon3"] = C.ParseDecimal(drwYeni["AcentaKomisyon3"]);
                drwYeni["AcentaOdemeHakedisYuzdesi3"] = C.ParseDecimal(drwYeni["AcentaOdemeHakedisYuzdesi3"]);
                drwYeni["AcentaKdvDahilMi"] = C.ParseBool(drwYeni["AcentaKdvDahilMi"]);
                drwYeni["AcentaKdvOrani"] = C.ParseDecimal(drwYeni["AcentaKdvOrani"]);
                drwYeni["AcentaYuzde1EksikHesapla"] = C.ParseBool(drwYeni["AcentaYuzde1EksikHesapla"]);
                drwYeni["AcentaKomisyonunuNpvdenYapilsinMi"] = C.ParseBool(drwYeni["AcentaKomisyonunuNpvdenYapilsinMi"]);
                #endregion

                if (FormLoadIcin)
                    MListler.dsAktivite.Tables[0].Rows.Add(drwYeni);



                MListler.dsAktivite.Tables[0].EndLoadData();
            }
            catch (Exception Hata)
            {
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string SQL_Replace(string strSorgu)
        {
            try
            {

                #region dsAktivite Alanları Replace...
                DataTable dtA = MListler.dsAktivite.Tables[0];
                string[] DataDiziAktivite = new string[dtA.Columns.Count];

                for (int i = 0; i < dtA.Columns.Count; i++)
                {
                    string deger = "";
                    if (dtA.Rows[0][i] == null)
                        deger = "NULL";
                    else if (dtA.Rows[0][i].ToString().ToLower() == "true")
                        deger = "1";
                    else if (dtA.Rows[0][i].ToString().ToLower() == "false")
                        deger = "0";
                    else
                        deger = dtA.Rows[0][i].ToString();

                    DataDiziAktivite[i] = "#A_" + dtA.Columns[i].ColumnName + "_000#" + "|" + deger;
                }


                foreach (var item in DataDiziAktivite) //Sorgu içindeki alanların başlıklarına göre değerleri replace ile yerlerine basılıyor...
                {
                    string[] Deger = item.Split('|');
                    strSorgu = strSorgu.Replace(Deger[0], Deger[1]);
                }
                #endregion

                #region dsGayrimenkul Replace
                DataTable dtGayrimenkul = MListler.dsAktivite_Gayrimenkul.Tables[0];
                string[] DataDiziGayrimenkul = new string[dtGayrimenkul.Columns.Count * dtGayrimenkul.Rows.Count];


                int iSayac = 0;
                for (int rw = 0; rw < dtGayrimenkul.Rows.Count; rw++)
                {
                    for (int c = 0; c < dtGayrimenkul.Columns.Count; c++)
                    {
                        string deger = "";
                        if (dtGayrimenkul.Rows[rw][c] == null)
                            deger = "NULL";
                        else if (dtGayrimenkul.Rows[rw][c].ToString().ToLower() == "true")
                            deger = "1";
                        else if (dtGayrimenkul.Rows[rw][c].ToString().ToLower() == "false")
                            deger = "0";
                        else
                            deger = dtGayrimenkul.Rows[rw][c].ToString();


                        DataDiziGayrimenkul[iSayac] = "#G_" + dtGayrimenkul.Columns[c].ColumnName + "_" + rw.ToString("000") + "#" + "|" + deger;
                        iSayac++;
                    }
                }

                foreach (var item in DataDiziGayrimenkul) //Sorgu içindeki alanların başlıklarına göre değerleri replace ile yerlerine basılıyor...
                {
                    string[] Deger = item.Split('|');
                    int k = strSorgu.IndexOf("#");

                    strSorgu = strSorgu.Replace(Deger[0], Deger[1]);
                }
                #endregion

                #region dsMüşteri Replace
                DataTable dtMusteri = MListler.dsAktivite_Musteri.Tables[0];
                string[] DataDiziMusteri = new string[dtMusteri.Columns.Count * dtMusteri.Rows.Count];

                iSayac = 0;
                for (int rw = 0; rw < dtMusteri.Rows.Count; rw++)
                {
                    for (int c = 0; c < dtMusteri.Columns.Count; c++)
                    {
                        string deger = "";
                        if (dtMusteri.Rows[rw][c] == null)
                            deger = "NULL";
                        else if (dtMusteri.Rows[rw][c].ToString().ToLower() == "true")
                            deger = "1";
                        else if (dtMusteri.Rows[rw][c].ToString().ToLower() == "false")
                            deger = "0";
                        else
                            deger = dtMusteri.Rows[rw][c].ToString();

                        DataDiziMusteri[iSayac] = "#G_" + dtMusteri.Columns[c].ColumnName + "_" + rw.ToString("000") + "#" + "|" + deger;
                        iSayac++;
                    }
                }

                foreach (var item in DataDiziMusteri) //Sorgu içindeki alanların başlıklarına göre değerleri replace ile yerlerine basılıyor...
                {
                    string[] Deger = item.Split('|');
                    int k = strSorgu.IndexOf("#");
                    strSorgu = strSorgu.Replace(Deger[0], Deger[1]);
                }
                #endregion

                #region dsOdemePlani Replace
                DataTable dtOdemePlani = MListler.dsAktivite_OdemePlaniSenet.Tables[0];
                string[] DataDiziOdemePlani = new string[dtOdemePlani.Columns.Count * dtOdemePlani.Rows.Count];

                iSayac = 0;
                for (int rw = 0; rw < dtOdemePlani.Rows.Count; rw++)
                {
                    for (int c = 0; c < dtOdemePlani.Columns.Count; c++)
                    {
                        string deger = "";
                        if (dtOdemePlani.Rows[rw][c] == null)
                            deger = "NULL";
                        else if (dtOdemePlani.Rows[rw][c].ToString().ToLower() == "true")
                            deger = "1";
                        else if (dtOdemePlani.Rows[rw][c].ToString().ToLower() == "false")
                            deger = "0";
                        else
                            deger = dtOdemePlani.Rows[rw][c].ToString();

                        DataDiziOdemePlani[iSayac] = "#G_" + dtOdemePlani.Columns[c].ColumnName + "_" + rw.ToString("000") + "#" + "|" + deger;
                        iSayac++;
                    }
                }

                foreach (var item in DataDiziOdemePlani) //Sorgu içindeki alanların başlıklarına göre değerleri replace ile yerlerine basılıyor...
                {
                    string[] Deger = item.Split('|');
                    int k = strSorgu.IndexOf("#");
                    strSorgu = strSorgu.Replace(Deger[0], Deger[1]);
                }
                #endregion

                return strSorgu;
            }
            catch (Exception Hata)
            {
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
        }


        private bool SQLKontrolleri(bool bKayitIslemindenSonraMiCalisacak)
        {
            try
            {
                if (!bKayitIslemindenSonraMiCalisacak) //indirim, kampanya ve proje bazlı sql kontrolleri sadece kayıttan önce çalışır...
                {
                    #region Indirim Kontrolleri
                    if (C.ParseInt(cmbIndirim.EditValue) > 0 && bIndirimQueryCalissinMi)
                    {
                        var IndirimSQLKayitOncesiKontrolList = IndirimSQLKontrolList.Where(u => u.KayitIslemindenSonraCalisacakMi == bKayitIslemindenSonraMiCalisacak);
                        foreach (var iko in IndirimSQLKayitOncesiKontrolList)
                        {
                            string strSorgu = iko.SqlGenel.Replace("#DilID#", Genel.DilID.ToString()) //Sorgu içindeki TabloAlanID ve mesaj KayıtID set ediliyor..
                            .Replace("#TabloAlanID#", "41013") //TabloAlanID = 41013 bknz. [Tn_DilCevir_0250]
                            .Replace("#KayitID#", iko.ID.ToString());

                            strSorgu = SQL_Replace(strSorgu);

                            var Result = Genel.PrmDb.S_StringSQLCalistir(strSorgu).FirstOrDefault(); //Sorgunun son hali çalıştırılıyor.

                            if (Result.Msg.Length > 0) //mesaj var ise gösteriliyor...
                                XtraMessageBox.Show(Result.Msg, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            if (Result.Sonuc == 1) //Hata var ise kırılıyor...
                                return false; //ilk hatada döngü kesiliyor....
                        }
                    }
                    #endregion

                    #region Kampanya Kontrolleri
                    if (C.ParseInt(cmbKampanya.EditValue) > 0 && bKampanyaQueryCalissinMi)
                    {
                        var KampanyaSQLKayitOncesiKontrolList = KampanyaSQLKontrolList.Where(u => u.KayitIslemindenSonraCalisacakMi == bKayitIslemindenSonraMiCalisacak);
                        foreach (var kko in KampanyaSQLKayitOncesiKontrolList)
                        {
                            string strSorgu = kko.SqlGenel.Replace("#DilID#", Genel.DilID.ToString()) //Sorgu içindeki TabloAlanID ve mesaj KayıtID set ediliyor..
                            .Replace("#TabloAlanID#", "41013") //TabloAlanID = 41013 bknz. [Tn_DilCevir_0250]
                            .Replace("#KayitID#", kko.ID.ToString());

                            strSorgu = SQL_Replace(strSorgu);

                            var Result = Genel.PrmDb.S_StringSQLCalistir(strSorgu).FirstOrDefault(); //Sorgunun son hali çalıştırılıyor.

                            if (Result.Msg.Length > 0) //mesaj var ise gösteriliyor...
                                XtraMessageBox.Show(Result.Msg, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            if (Result.Sonuc == 1) //Hata var ise kırılıyor...
                                return false; //ilk hatada döngü kesiliyor....
                        }
                    }
                    #endregion

                    #region Proje Bazlı SQL Kontrolleri
                    if (bProjeOzelQueryCalissinMi)
                    {
                        var ProjeBazliSQLKontrolleri = Genel.PrmDb.S_AktiviteyeOzelSatisKosullariniGetir(Genel.Aktivite_ProjeID, -1).Where(u => u.AktifMi == true).ToList();
                        foreach (var s in ProjeBazliSQLKontrolleri)
                        {
                            string strSorgu = s.SqlQuery.Replace("#DilID#", Genel.DilID.ToString()) //Sorgu içindeki TabloAlanID ve mesaj KayıtID set ediliyor..
                           .Replace("#TabloAlanID#", "114519") //TabloAlanID = 114519 bknz. [Tn_DilCevir_0250]
                           .Replace("#KayitID#", ProjeBazliSQLKontrolleri.FirstOrDefault().ID.ToString());

                            strSorgu = SQL_Replace(strSorgu); //Aktivite public tablolaruındaki alanlar replace ediliyor...

                            #region Projeye Özel Satış Koşulları alanları replace ediliyor...
                            strSorgu = strSorgu.Replace("#P_Int1#", s.DegerInt1.ToString()).Replace("#P_Int2#", s.DegerInt2.ToString());

                            DateTime dtm1 = C.ParseDateTime(s.DegerTarih1);
                            string Date1 = C.ParseDateTimeToSQLString(dtm1);
                            DateTime dtm2 = C.ParseDateTime(s.DegerTarih2);
                            string Date2 = C.ParseDateTimeToSQLString(dtm2);



                            strSorgu = strSorgu.Replace("#P_Date1#", Date1).Replace("#P_Date2#", Date2);
                            strSorgu = strSorgu.Replace("#P_String1#", "'" + s.DegerString1.ToString() + "'").Replace("#P_String2#", "'" + s.DegerString2.ToString() + "'");
                            #endregion

                            var Result = Genel.PrmDb.S_StringSQLCalistir(strSorgu).FirstOrDefault(); //Sorgunun son hali çalıştırılıyor.

                            if (Result.Msg.Length > 0) //mesaj var ise gösteriliyor...
                                XtraMessageBox.Show(Result.Msg, "", MessageBoxButtons.OK, MessageBoxIcon.None);

                            if (Result.Sonuc == 1) //Hata var ise kırılıyor...
                                return false; //ilk hatada döngü kesiliyor....
                        }
                    }

                    #endregion
                }

                #region Genel SQL Kontrolleri
                if (bFormQueryCalissinMi)
                {
                    var Sorgular = Genel.PrmDb.S_FormMesajSql(Genel.DilID).Where(x => x.AktifMi == true && x.FormID == iFormID && x.KayitIslemindenSonraCalisacakMi == bKayitIslemindenSonraMiCalisacak && x.TabloSqlTipID < 3).ToList();
                    foreach (var s in Sorgular)
                    {
                        string strSorgu = s.SqlGenel.Replace("#DilID#", Genel.DilID.ToString()) //Sorgu içindeki TabloAlanID ve mesaj KayıtID set ediliyor..
                       .Replace("#TabloAlanID#", "41013") //TabloAlanID = 41013 bknz. [Tn_DilCevir_0250]
                       .Replace("#KayitID#", Sorgular.FirstOrDefault().ID.ToString());

                        strSorgu = SQL_Replace(strSorgu);

                        var Result = Genel.PrmDb.S_StringSQLCalistir(strSorgu).FirstOrDefault(); //Sorgunun son hali çalıştırılıyor.

                        if (Result.Msg.Length > 0) //mesaj var ise gösteriliyor...
                            XtraMessageBox.Show(Result.Msg, "", MessageBoxButtons.OK, MessageBoxIcon.None);

                        if (!bKayitIslemindenSonraMiCalisacak)
                        {
                            if (Result.Sonuc == 1) //Hata var ise kırılıyor...
                                return false; //ilk hatada döngü kesiliyor....
                        }
                        else if (Result.Sonuc == 0)
                            return false;
                    }
                }
                #endregion

                return true;

            }
            catch (Exception Hata)
            {
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private bool IndirimKontrol()
        {
            //İndirim kuralları kontrol ediliyor...

            var ListProjeVeKullaniciBazliIndirimOrani = Genel.PrmDb.S_KullanicininProjeIndirimOrani(Genel.AktifKullaniciID, Genel.Aktivite_ProjeID).ToList();
            decimal dProjeVeKullaniciBazliIndirimOrani = 0;
            if (ListProjeVeKullaniciBazliIndirimOrani.Count > 0)
                dProjeVeKullaniciBazliIndirimOrani = C.ParseDecimal(ListProjeVeKullaniciBazliIndirimOrani.FirstOrDefault().IndirimYuzdesi);



            decimal dToplamIndirim = C.ParseDecimal(C.ParseDecimal(txtHesaplananIndirim.EditValue).ToString("n2")) + C.ParseDecimal(C.ParseDecimal(txtTanimlananIndirim.EditValue).ToString("n2"));

            if (dProjeVeKullaniciBazliIndirimOrani <= 0 || dToplamIndirim <= 0)
            {
                return true;
            }

            decimal dYuzdeKacIndirimYapilmis = (dToplamIndirim * 100) / (C.ParseDecimal(txtListeFiyati.Text) + dToplamIndirim);

            if (dYuzdeKacIndirimYapilmis > dProjeVeKullaniciBazliIndirimOrani) //kullanıcıya bu projede tanınan indirim hakkından fazla indirim yaptıysa...
            {
                Ayar.InfoMesajGoster(Genel.DilGetirMesaj(3163), Ayar.MesajTipi.Hata, this);
                this.Cursor = Cursors.Default;
                return false;
            }

            return true;
        }



        private bool KayitOncesiVal()
        {
            try
            {

                if (gvMusteriBilgileri.DataRowCount < 1)
                {
                    Ayar.InfoMesajGoster(Genel.DilGetirMesaj(3089), Ayar.MesajTipi.Hata, this);
                    this.Cursor = Cursors.Default;
                    return false;
                }


                if (!IndirimKontrol())
                {
                    this.Cursor = Cursors.Default;
                    return false;
                }




                #region Rezerve dısındaki satıslarda daire fiyatı 0 olan gayrimenkul varsa...
                bool bSifirFiyatliGayrimenkulVar = false;
                if (C.ParseInt(cmbAktiviteDurumu.EditValue) == 303) //Satıldı
                {
                    for (int i = 0; i < gvGayrimenkulBilgileri.DataRowCount; i++)
                    {
                        decimal dFiyat = C.ParseDecimal(gvGayrimenkulBilgileri.GetRowCellValue(i, "SatisFiyati"));
                        if (dFiyat <= 0)
                        {
                            bSifirFiyatliGayrimenkulVar = true;
                        }
                    }

                    if (bSifirFiyatliGayrimenkulVar)
                    {
                        Ayar.InfoMesajGoster(Genel.DilGetirMesaj(3216), Ayar.MesajTipi.Hata, this);
                        return false;
                    }
                }

                #endregion








                #region Ödeme planındaki tutarların (peşinatlar,banka kredisi,vadeli) yapılan ödemelerden az olamaz kontrolü...
                if (iAktiviteID > 0)
                {
                    var ListTahsilatlar = Genel.PrmDb.S_AktiviteOdemePlaniKontrol(iAktiviteID, Genel.DilID).ToList();

                    //if ( C.ParseInt(ListTahsilatlar.PlanPesinat01) < ListTahsilatlar.
                    foreach (var item in ListTahsilatlar)
                    {
                        if (C.ParseInt(item.TipKisID_304) == 381) //peşinatlardan biriyse...
                        {
                            decimal dTutar = C.ParseDecimal(item.KapatilanBorcTutari);
                            Control[] ctrl = Controls.Find("txtPesinat" + item.PesinatNo.ToString(), true);
                            if (ctrl.Length > 0)
                            {
                                ButtonEdit txt = (ButtonEdit)ctrl[0];
                                if (C.ParseDecimal(txt.EditValue) < dTutar)
                                {
                                    Ayar.InfoMesajGoster(String.Format(Genel.DilGetirMesaj(3101), item.PesinatNo.ToString()), Ayar.MesajTipi.Uyari, this);
                                    return false;
                                }
                            }
                        }
                        else if (C.ParseInt(item.TipKisID_304) == 382) //banka kredisiyse
                        {
                            decimal dTutar = C.ParseDecimal(item.KapatilanBorcTutari);

                            if (C.ParseDecimal(txtBankaKredisi.EditValue) < dTutar)
                            {
                                Ayar.InfoMesajGoster(Genel.DilGetirMesaj(3102), Ayar.MesajTipi.Uyari, this);
                                return false;
                            }
                        }
                    }
                    var Vadeliler = ListTahsilatlar.Where(u => u.TipKisID_304 == 380).ToList(); //vadeli ödemeler...
                    decimal dVadeliToplam = C.ParseDecimal(Vadeliler.Sum(z => z.KapatilanBorcTutari));
                    if (C.ParseDecimal(txtVadeli.EditValue) < dVadeliToplam)
                    {
                        Ayar.InfoMesajGoster(Genel.DilGetirMesaj(3103), Ayar.MesajTipi.Uyari, this);
                        return false;
                    }
                }
                #endregion

                if (!Validation())
                {

                    this.Cursor = Cursors.Default;
                    return false;
                }


                this.Cursor = Cursors.WaitCursor;

                if (C.ParseInt(cmbKampanya.EditValue) > 0)
                    KampanyaSQLKontrolList = Genel.PrmDb.S_AktiviteSQL_KampanyaKontrol(C.ParseInt(cmbKampanya.EditValue)).ToList();
                if (C.ParseInt(cmbIndirim.EditValue) > 0)
                    IndirimSQLKontrolList = Genel.PrmDb.S_AktiviteSQL_IndirimKontrol(C.ParseInt(cmbIndirim.EditValue)).ToList();



                //SQL kontrolleri tabloları dönerken silinmiş kayıtlarda hata alacağı için silinen değişen kayıtlar AcceptChange ile onaylanıyor.
                MListler.dsAktivite.AcceptChanges();
                MListler.dsAktivite_Gayrimenkul.AcceptChanges();
                MListler.dsAktivite_Musteri.AcceptChanges();
                MListler.dsAktivite_OdemePlaniSenet.AcceptChanges();



                if (!SQLKontrolleri(false))
                {
                    this.Cursor = Cursors.Default;
                    return false;
                }

                return true;

            }
            catch (Exception Hata)
            {
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }








        private void formYanMenu1_butonKlk(object sender, EventArgs e)
        {

            formYanMenu1._Params = new object[] { 0, Genel.TabloIDGetir(Name), iAktiviteID };
        }




        private void txtGrdGayrimenkulSil_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                if (gvGayrimenkulBilgileri.DataRowCount == 1)
                {
                    Ayar.InfoMesajGoster(Genel.DilGetirMesaj(2082), Ayar.MesajTipi.Uyari, this);
                    return;
                }


                if (XtraMessageBox.Show(String.Format(Genel.DilGetirMesaj(2081),
                   gvGayrimenkulBilgileri.GetFocusedRowCellValue("DaireNo")), Genel.DilGetirMesaj(3), MessageBoxButtons.YesNo,
                   MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {

                    int iG_ID = C.ParseInt(gvGayrimenkulBilgileri.GetFocusedRowCellValue("GayrimenkulID"));
                    int iAktiviteGayrimenkul_ID = C.ParseInt(gvGayrimenkulBilgileri.GetFocusedRowCellValue("ID"));

                    if (iAktiviteGayrimenkul_ID < 1) //yeni eklenmiş gayrimenkul ise
                    {
                        var GayrimenkulKontrol = Genel.PrmDb.SEDS_AktiviteKayitKontrol(900, 2, iG_ID, Genel.AktifKullaniciID, Genel.AktifKullaniciAdiSoyadi).ToList().FirstOrDefault();
                    }
                    gvGayrimenkulBilgileri.DeleteRow(gvGayrimenkulBilgileri.FocusedRowHandle);

                    UstPanelYukseklikAyarla();

                    GayrimenkulFiyatlariHesapla();

                }
            }
            catch (Exception Hata)
            {
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        private void GayrimenkulEkle(int iG_ID)
        {
            try
            {

                if (iG_ID > 0)
                {

                    string strProje = "";
                    if (PRJ.FirstOrDefault() != null)
                    {
                        strProje = PRJ.FirstOrDefault().ProjeAdi;
                        gcGayrimenkulBilgileri.Text = lblTempCaption.Text + " [" + strProje + "]";
                    }


                    bool bGayrimenkulVarMi = false;
                    for (int i = 0; i < gvGayrimenkulBilgileri.RowCount; i++)
                    {
                        if (C.ParseInt(gvGayrimenkulBilgileri.GetRowCellValue(i, "GayrimenkulID")) == iG_ID)
                            bGayrimenkulVarMi = true;
                    }


                    if (!bGayrimenkulVarMi)
                    {
                        var Data = Genel.PrmDb.S_GayrimenkulSatisaHazirlar(Genel.DilID, Genel.AktifKullaniciID, Genel.Aktivite_ProjeID.ToString(), iG_ID).FirstOrDefault();

                        if (Data == null)
                        {
                            Dispose();
                            return;
                        }


                        DataRow drwYeni = MListler.dsAktivite_Gayrimenkul.Tables[0].NewRow();

                        #region AktiviteGayrimenkul Tablosu Yeni Satır Alanları...
                        drwYeni["BolumAdi"] = Data.BolumAdi;
                        drwYeni["BlokNo"] = Data.BlokNo;
                        drwYeni["Giris"] = Data.Giris;
                        drwYeni["DaireNo"] = Data.DaireNo;
                        drwYeni["GayrimenkulKatID"] = Data.GayrimenkulKatID;
                        drwYeni["GayrimenkulKatID_STR"] = Data.GayrimenkulKatID_STR;
                        drwYeni["BrutM2"] = Data.BrutM2;
                        drwYeni["GayrimenkulTipDurumSecID_206"] = Data.GayrimenkulTipDurumSecID_206;
                        drwYeni["GayrimenkulTip"] = Data.GayrimenkulTip;
                        drwYeni["YonDurumSecID_1"] = Data.YonDurumSecID_1;
                        drwYeni["Yon"] = Data.Yon;
                        drwYeni["ManzaraDurumsecID_2"] = Data.ManzaraDurumsecID_2;
                        drwYeni["Manzara"] = Data.Manzara;
                        drwYeni["OdaSayisi"] = Data.OdaSayisi;
                        drwYeni["NetM2"] = Data.NetM2;
                        drwYeni["KatBrutM2"] = Data.KatBrutM2;
                        drwYeni["BalkonM2"] = Data.BalkonM2;
                        drwYeni["TerasM2"] = Data.TerasM2;
                        drwYeni["BahceM2"] = Data.BahceM2;
                        drwYeni["TapuBagNo"] = Data.TapuBagNo;
                        drwYeni["GirisDurumSecID_7"] = Data.GirisDurumSecID_7;
                        drwYeni["OtoParkNo"] = Data.OtoParkNo;
                        drwYeni["GayrimenkulID"] = iG_ID;


                        drwYeni["ID"] = -1;
                        drwYeni["ProjeTipSiniflariID"] = Data.ProjeTipSiniflariID;
                        drwYeni["ProjeTipSinifi"] = Data.ProjeTipSinifi;
                        drwYeni["SatisFiyati"] = C.ParseDecimal(Data.SatisFiyati).ToString("n2");
                        drwYeni["ListeFiyatiDovizKodu"] = Data.SatisFiyatiDovizKodu;
                        drwYeni["SatisFiyatiDovizID"] = Data.SatisFiyatiDovizID;
                        drwYeni["ListeFiyatiKur"] = "0";
                        drwYeni["SatisCarpani"] = "0";

                        drwYeni["AktiviteID"] = iAktiviteID;
                        drwYeni["ListeFiyati"] = C.ParseDecimal(Data.SatisFiyati).ToString("n2");
                        drwYeni["ListeFiyatiDovizID"] = Data.SatisFiyatiDovizID;
                        drwYeni["ListeFiyatiKisID_312"] = Data.ListeFiyatiKisID_312;
                        drwYeni["KdvDurumuKisID_26"] = Data.SatisFiyatiKdvDurumuKisID_26;
                        drwYeni["EkspertizBedeli"] = Data.EkspertizBedeli;
                        drwYeni["DekorasyonFiyati1"] = Data.DekorasyonFiyati1;
                        drwYeni["DekorasyonFiyati2"] = Data.DekorasyonFiyati2;



                        //T_AktiviteGayrimenkulFiyatDetay (FYT butonu ile ilgili olan alanlar)
                        drwYeni["GayrimenkulListeFiyati01"] = Data.SatisFiyati01;
                        drwYeni["GayrimenkulListeFiyati01DovizID"] = Data.SatisFiyati01DovizID;
                        drwYeni["GayrimenkulListeFiyati02"] = Data.SatisFiyati02;
                        drwYeni["GayrimenkulListeFiyati02DovizID"] = Data.SatisFiyati02DovizID;
                        drwYeni["GayrimenkulListeFiyati03"] = Data.SatisFiyati03;
                        drwYeni["GayrimenkulListeFiyati03DovizID"] = Data.SatisFiyati03DovizID;
                        drwYeni["GayrimenkulListeFiyati04"] = Data.SatisFiyati04;
                        drwYeni["GayrimenkulListeFiyati04DovizID"] = Data.SatisFiyati04DovizID;
                        drwYeni["GayrimenkulListeFiyati05"] = Data.SatisFiyati05;
                        drwYeni["GayrimenkulListeFiyati05DovizID"] = Data.SatisFiyati05DovizID;
                        drwYeni["GayrimenkulListeFiyati06"] = Data.SatisFiyati06;
                        drwYeni["GayrimenkulListeFiyati06DovizID"] = Data.SatisFiyati06DovizID;
                        drwYeni["GayrimenkulListeFiyati07"] = Data.SatisFiyati07;
                        drwYeni["GayrimenkulListeFiyati07DovizID"] = Data.SatisFiyati07DovizID;
                        drwYeni["GayrimenkulListeFiyati08"] = Data.SatisFiyati08;
                        drwYeni["GayrimenkulListeFiyati08DovizID"] = Data.SatisFiyati08DovizID;
                        drwYeni["GayrimenkulListeFiyati09"] = Data.SatisFiyati09;
                        drwYeni["GayrimenkulListeFiyati09DovizID"] = Data.SatisFiyati09DovizID;
                        drwYeni["GayrimenkulListeFiyati10"] = Data.SatisFiyati10;
                        drwYeni["GayrimenkulListeFiyati10DovizID"] = Data.SatisFiyati10DovizID;
                        drwYeni["GayrimenkulListeFiyati11"] = Data.SatisFiyati11;
                        drwYeni["GayrimenkulListeFiyati11DovizID"] = Data.SatisFiyati11DovizID;
                        drwYeni["GayrimenkulListeFiyati12"] = Data.SatisFiyati12;
                        drwYeni["GayrimenkulListeFiyati12DovizID"] = Data.SatisFiyati12DovizID;
                        drwYeni["GayrimenkulListeFiyati13"] = Data.SatisFiyati13;
                        drwYeni["GayrimenkulListeFiyati13DovizID"] = Data.SatisFiyati13DovizID;
                        drwYeni["GayrimenkulListeFiyati14"] = Data.SatisFiyati14;
                        drwYeni["GayrimenkulListeFiyati14DovizID"] = Data.SatisFiyati14DovizID;
                        drwYeni["GayrimenkulListeFiyati15"] = Data.SatisFiyati15;
                        drwYeni["GayrimenkulListeFiyati15DovizID"] = Data.SatisFiyati15DovizID;
                        drwYeni["GayrimenkulListeFiyati16"] = Data.SatisFiyati16;
                        drwYeni["GayrimenkulListeFiyati16DovizID"] = Data.SatisFiyati16DovizID;
                        drwYeni["GayrimenkulListeFiyati17"] = Data.SatisFiyati17;
                        drwYeni["GayrimenkulListeFiyati17DovizID"] = Data.SatisFiyati17DovizID;
                        drwYeni["GayrimenkulListeFiyati18"] = Data.SatisFiyati18;
                        drwYeni["GayrimenkulListeFiyati18DovizID"] = Data.SatisFiyati18DovizID;
                        drwYeni["GayrimenkulListeFiyati19"] = Data.SatisFiyati19;
                        drwYeni["GayrimenkulListeFiyati19DovizID"] = Data.SatisFiyati19DovizID;
                        drwYeni["GayrimenkulListeFiyati20"] = Data.SatisFiyati20;
                        drwYeni["GayrimenkulListeFiyati20DovizID"] = Data.SatisFiyati20DovizID;
                        drwYeni["EkspertizBedeli1"] = Data.EkspertizBedeli1;
                        drwYeni["EkspertizBedeli2"] = Data.EkspertizBedeli2;
                        drwYeni["EkspertizBedeli3"] = Data.EkspertizBedeli3;
                        drwYeni["EkspertizBedeli4"] = Data.EkspertizBedeli4;
                        drwYeni["EkspertizBedeli5"] = Data.EkspertizBedeli5;
                        drwYeni["EkspertizBedeli6"] = Data.EkspertizBedeli6;
                        drwYeni["EkspertizBedeli7"] = Data.EkspertizBedeli7;
                        drwYeni["EkspertizBedeli8"] = Data.EkspertizBedeli8;
                        drwYeni["EkspertizBedeli9"] = Data.EkspertizBedeli9;
                        drwYeni["EkspertizBedeli10"] = Data.EkspertizBedeli10;
                        drwYeni["EkspertizBedeli11"] = Data.EkspertizBedeli11;
                        drwYeni["EkspertizBedeli12"] = Data.EkspertizBedeli12;
                        drwYeni["EkspertizBedeli13"] = Data.EkspertizBedeli13;
                        drwYeni["EkspertizBedeli14"] = Data.EkspertizBedeli14;
                        drwYeni["EkspertizBedeli15"] = Data.EkspertizBedeli15;
                        drwYeni["EkspertizBedeli16"] = Data.EkspertizBedeli16;
                        drwYeni["EkspertizBedeli17"] = Data.EkspertizBedeli17;
                        drwYeni["EkspertizBedeli18"] = Data.EkspertizBedeli18;
                        drwYeni["EkspertizBedeli19"] = Data.EkspertizBedeli19;
                        drwYeni["EkspertizBedeli20"] = Data.EkspertizBedeli20;






                        //T_AktiviteGayrimenkulDetay
                        drwYeni["TahsilEdilecekKDV"] = 0;
                        drwYeni["TahsilEdilecekKDVSenetliMi"] = false;
                        drwYeni["TahsilEdilecekKDVTarihi"] = DBNull.Value; // C.ParseDateTime("1901.01.01 00:00:00.000");
                        drwYeni["AktiviteKDVTutariHsp"] = 0;
                        drwYeni["AktiviteDamgaVergisiHsp"] = 0;
                        drwYeni["AktiviteTapuMasrafiHsp"] = 0;
                        drwYeni["TahsilEdilecekDamgaSenetliMi"] = false;
                        drwYeni["TahsilEdilecekDamgaVergisiOrani"] = 0;
                        drwYeni["TahsilEdilecekDamgaVergisiTarihi"] = DBNull.Value; //C.ParseDateTime("1901.01.01 00:00:00.000");
                        drwYeni["AktiviteToplamMasrafHsp"] = 0;
                        drwYeni["AktiviteNoterHarciHsp"] = 0;
                        drwYeni["AktiviteIskanHarciHsp"] = 0;
                        drwYeni["FaturaNo"] = "";
                        drwYeni["FaturaTutari"] = 0;
                        drwYeni["FaturaAciklama"] = "";
                        drwYeni["FaturaSirketi"] = "";
                        drwYeni["FaturaKDVTutari"] = 0;
                        drwYeni["FaturaNot"] = "";
                        drwYeni["FaturaTarihi"] = DBNull.Value; //C.ParseDateTime("1901.01.01 00:00:00.000");
                        drwYeni["TapudaGosterilecekBedel"] = 0;
                        drwYeni["FaturaKesildiMi"] = false;

                        //T_AktiviteGayrimenkulSatisSonrasi
                        drwYeni["TapuBasvurusuYapildiMi"] = false;
                        drwYeni["TapuBasvurusuYapan"] = "";
                        drwYeni["TapuBasvurusuNo"] = "";
                        drwYeni["TapuBasvurusuTarihi"] = DBNull.Value; //C.ParseDateTime("1901.01.01 00:00:00.000");
                        drwYeni["TapuTeslimEvraklariYapildiMi"] = false;
                        drwYeni["TapuBasvuruEvragiGeldiMi"] = false;
                        drwYeni["TapuBasvuruEvragiGelisTarihi"] = DBNull.Value; // C.ParseDateTime("1901.01.01 00:00:00.000");
                        drwYeni["TapuTeslimiYapildiMi"] = false;
                        drwYeni["TapuTeslimiTarihi"] = DBNull.Value; // C.ParseDateTime("1901.01.01 00:00:00.000");
                        drwYeni["TapuMasrafToplami"] = 0;
                        drwYeni["TapuMasrafDovizID"] = 0;
                        drwYeni["TapuKadastroNo"] = "";
                        drwYeni["BelediyeNo"] = "";
                        drwYeni["DamgaVergisiOrani"] = 0;
                        drwYeni["PlanlananTeslimTarihiOnaylandiMi"] = false;


                        drwYeni["TapuMasrafHesaplanmaTarihi"] = DBNull.Value; // C.ParseDateTime("1901.01.01 00:00:00.000");
                        drwYeni["TapuKadastroTarihi"] = DBNull.Value; //C.ParseDateTime("1901.01.01 00:00:00.000");
                        drwYeni["BelediyeTarihi"] = DBNull.Value; // C.ParseDateTime("1901.01.01 00:00:00.000");
                        drwYeni["SorunsuzTeslimIcinVerilenTarih"] = DBNull.Value; //C.ParseDateTime("1901.01.01 00:00:00.000");
                        drwYeni["PlanlananTeslimTarihi"] = DBNull.Value; //C.ParseDateTime("1901.01.01 00:00:00.000");

                        drwYeni["DaireBilgi"] = Data.BlokNo + " - " + Data.DaireNo;
                        #endregion


                        MListler.dsAktivite_Gayrimenkul.Tables[0].Rows.Add(drwYeni);
                        //MListler.dsAktivite_Gayrimenkul.Tables[0].AcceptChanges();


                        grdGayrimenkulBilgileri.DataSource = null;
                        grdGayrimenkulBilgileri.DataSource = MListler.dsAktivite_Gayrimenkul.Tables[0];


                        //gayrimenkuldata formu tabda acıksa, yeni gayrimenkul eklendiği için datasourceu yenileniyor.
                        F_AktiviteGayrimenkulData frmAGD = (F_AktiviteGayrimenkulData)Application.OpenForms["F_AktiviteGayrimenkulData"];
                        if (frmAGD != null)
                        {
                            frmAGD.vGridControl1.RefreshDataSource();
                        }
                        ////////

                        UstPanelYukseklikAyarla();

                        GayrimenkulFiyatlariHesapla();
                    }
                    else
                    {
                        Ayar.InfoMesajGoster(Genel.DilGetirMesaj(3093), Ayar.MesajTipi.Hata, this, 6);

                    }

                }
            }
            catch (Exception Hata)
            {
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void cmbDoviz_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (MListler.dsAktivite.Tables[0].Rows.Count > 0)
                    MListler.dsAktivite.Tables[0].Rows[0]["SatisFiyatiDovizID"] = C.ParseInt(cmbDoviz.EditValue); //F_AktiviteOdeme_Giris Ekranında kullanıldığı için her değiştiğinde set ediliyor.
                dCaprazKur = Genel.CaprazKurGetir(C.ParseInt(gvGayrimenkulBilgileri.GetRowCellValue(0, "SatisFiyatiDovizID")), C.ParseInt(cmbDoviz.EditValue), 1);
                GayrimenkulFiyatlariHesapla();


                txtPesinat1.EditValue = 0;
                txtPesinat2.EditValue = 0;
                txtPesinat3.EditValue = 0;
                txtPesinat4.EditValue = 0;
                txtPesinat5.EditValue = 0;
                txtPesinat6.EditValue = 0;
                txtPesinat7.EditValue = 0;
                txtPesinat8.EditValue = 0;
                txtPesinat9.EditValue = 0;
                txtPesinat10.EditValue = 0;
                txtBankaKredisi.EditValue = 0;
                txtVadeli.EditValue = 0;
                txtToplamTaksit.EditValue = 0;
                MListler.dsAktivite_OdemePlaniSenet.Tables[0].Rows.Clear();

            }
            catch (Exception Hata)
            {
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void txtGrdGayrimenkulEkle_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                F_GayrimenkulSec frm = new F_GayrimenkulSec();

                frm._FormAcilisTipi = 2;
                frm.ShowDialog();
                this.Cursor = Cursors.WaitCursor;


                string strProje = "";
                if (PRJ.FirstOrDefault() != null)
                {
                    strProje = PRJ.FirstOrDefault().ProjeAdi;
                    gcGayrimenkulBilgileri.Text = lblTempCaption.Text + " [" + strProje + "]";
                }


                if (!gcGayrimenkulBilgileri.Text.Contains(strProje) && gcGayrimenkulBilgileri.Text != lblTempCaption.Text) //farklı bir projeden daire ekleniyorsa...
                {
                    Ayar.InfoMesajGoster(Genel.DilGetirMesaj(3082), Ayar.MesajTipi.Uyari, this);
                    this.Cursor = Cursors.Default;
                    return;
                }


                if (iGayrimenkulID > 0)
                    GayrimenkulEkle(iGayrimenkulID);

                ToplamOdemeHesapla();
                TaksitToplamiVeFarkiHesapla();

                this.Cursor = Cursors.Default;
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gvGayrimenkulBilgileri_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.IsGetData)
                e.Value = e.ListSourceRowIndex + 1;   //sıra no kolonuna veri yazılıyor...
        }
        private void gvMusteriBilgileri_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.IsGetData)
                e.Value = e.ListSourceRowIndex + 1;   //sıra no kolonuna veri yazılıyor...
        }





        private void GayrimenkulFiyatlariHesapla()
        {
            try
            {
                if (gvGayrimenkulBilgileri.RowCount <= 0 && !bIsFormLoaded)
                    return;


                #region Liste Fiyati Kur (Tüm satırlara yazılıyor)
                for (int i = 0; i < gvGayrimenkulBilgileri.RowCount; i++)
                {
                    decimal dListeFiyati = C.ParseDecimal(gvGayrimenkulBilgileri.GetRowCellValue(i, "ListeFiyati"));
                    int iKaynakKur = C.ParseInt(gvGayrimenkulBilgileri.GetRowCellValue(i, "SatisFiyatiDovizID"));
                    int iHedefKur = C.ParseInt(cmbDoviz.EditValue);
                    decimal dKur = Genel.CaprazKurGetir(iKaynakKur, iHedefKur, 1);
                    decimal dListeFiyatiKur = (dKur == 0 ? 1 : dKur) * dListeFiyati;
                    gvGayrimenkulBilgileri.SetRowCellValue(i, "ListeFiyatiKur", dListeFiyatiKur.ToString("n2"));
                }
                #endregion

                #region Liste Fiyati Kur Toplamı (txtListeFiyati'na yazılıyor)
                decimal dListeFiyatiKurToplam = 0;
                for (int i = 0; i < gvGayrimenkulBilgileri.RowCount; i++)
                {
                    dListeFiyatiKurToplam += C.ParseDecimal(gvGayrimenkulBilgileri.GetRowCellValue(i, "ListeFiyatiKur"));
                }
                txtListeFiyati.EditValue = dListeFiyatiKurToplam.ToString();
                txtSatisFiyati.EditValue = dListeFiyatiKurToplam -
                    (C.ParseDecimal(txtHesaplananIndirim.EditValue) + C.ParseDecimal(txtTanimlananIndirim.EditValue)) +
                    (C.ParseDecimal(txtHesaplananVadeFarki.EditValue) + C.ParseDecimal(txtTanimlananVadeFarki.EditValue));

                //txtSatisFiyati.EditValue = (dListeFiyatiKurToplam - C.ParseDecimal(txtHesaplananIndirim.EditValue) + C.ParseDecimal(txtHesaplananVadeFarki.EditValue)).ToString();

                #endregion

                #region Satış Çarpanı (Tüm satırlara yazılıyor)
                for (int j = 0; j < gvGayrimenkulBilgileri.RowCount; j++)
                {
                    decimal dListeFiyatiKur = C.ParseDecimal(gvGayrimenkulBilgileri.GetRowCellValue(j, "ListeFiyatiKur"));
                    decimal dSatisCarpani = dListeFiyatiKur == 0 || dListeFiyatiKurToplam == 0 ? 0 : dListeFiyatiKur / dListeFiyatiKurToplam;
                    gvGayrimenkulBilgileri.SetRowCellValue(j, "SatisCarpani", dSatisCarpani);
                }
                #endregion

                #region Liste m2 Fiyati (Tüm satırlara yazılıyor)
                for (int m = 0; m < gvGayrimenkulBilgileri.RowCount; m++)
                {
                    decimal dListeFiyatiKur = C.ParseDecimal(gvGayrimenkulBilgileri.GetRowCellValue(m, "ListeFiyatiKur"));
                    decimal dBrutM2 = C.ParseDecimal(gvGayrimenkulBilgileri.GetRowCellValue(m, "BrutM2"));
                    decimal dListeM2Fiyati = dListeFiyatiKur == 0 || dBrutM2 == 0 ? 0 : dListeFiyatiKur / dBrutM2;


                    gvGayrimenkulBilgileri.SetRowCellValue(m, "ListeM2Fiyati", dListeM2Fiyati.ToString("n2"));
                }
                #endregion

                #region Satış Fiyatı (Tüm satırlara yazılıyor)
                for (int k = 0; k < gvGayrimenkulBilgileri.RowCount; k++)
                {
                    decimal dListeFiyatiKur = C.ParseDecimal(gvGayrimenkulBilgileri.GetRowCellValue(k, "ListeFiyatiKur"));
                    decimal dSatisCarpani = C.ParseDecimal(gvGayrimenkulBilgileri.GetRowCellValue(k, "SatisCarpani"));
                    decimal dSatisFiyati = dSatisCarpani * C.ParseDecimal(txtSatisFiyati.EditValue);
                    gvGayrimenkulBilgileri.SetRowCellValue(k, "SatisFiyati", dSatisFiyati.ToString("n2"));
                }
                #endregion

                #region BrutM2Toplami (NPV hesaplamasında aşağıda kullanılıyor)
                decimal dBrutM2Toplami = 0;
                for (int k = 0; k < gvGayrimenkulBilgileri.RowCount; k++)
                {
                    dBrutM2Toplami += C.ParseDecimal(gvGayrimenkulBilgileri.GetRowCellValue(k, "BrutM2"));
                }
                #endregion

                #region Satış Fiyatı Toplamı (txtSatisFiyati'na yazılıyor)
                decimal dSatisFiyatiToplami = 0;
                for (int t = 0; t < gvGayrimenkulBilgileri.RowCount; t++)
                {
                    decimal dSatisFiyati = C.ParseDecimal(gvGayrimenkulBilgileri.GetRowCellValue(t, "SatisFiyati"));
                    dSatisFiyatiToplami += dSatisFiyati;
                }
                #endregion

                #region NPV, NPVM2 (Tüm satırlara yazılıyor) ve Mali Durum sekmesindeki NPV Yüzde kısmınada yazılıyor (lblNPVYuzdeDeger)
                decimal dNPVToplami = C.ParseDecimal(lblMD_NPVDeger.Text);
                for (int m = 0; m < gvGayrimenkulBilgileri.RowCount; m++)
                {
                    decimal dSatisCarpani = C.ParseDecimal(gvGayrimenkulBilgileri.GetRowCellValue(m, "SatisCarpani"));

                    decimal dNPV = 0;
                    if (dSatisCarpani <= 0 || dNPVToplami <= 0)
                        dNPV = 0;
                    else
                        dNPV = dSatisCarpani * dNPVToplami / 100;
                    decimal dBrutM2 = C.ParseDecimal(gvGayrimenkulBilgileri.GetRowCellValue(m, "BrutM2"));
                    gvGayrimenkulBilgileri.SetRowCellValue(m, "NPV", dNPV);
                    if (dNPV > 0 && dBrutM2 > 0)
                    {
                        decimal dRes = dNPV == 0 || dBrutM2 == 0 ? 0 : dNPV / dBrutM2;
                        gvGayrimenkulBilgileri.SetRowCellValue(m, "NPVM2", dRes);
                    }
                }


                if (C.ParseDecimal(txtVadeliFaiz.EditValue) <= 0)
                {
                    dNPVToplami = C.ParseDecimal(txtSatisFiyati.EditValue); //vadeli faiz oranı 0 ise NPV toplamına satıs fiyatı yazılır.
                    lblMD_NPVDeger.Text = dNPVToplami.ToString("n2");
                }
                if (dNPVToplami > 0 && dBrutM2Toplami > 0)
                {
                    decimal dNPVM2 = dNPVToplami == 0 || dBrutM2Toplami == 0 ? 0 : dNPVToplami / dBrutM2Toplami;
                    if (dNPVM2 > 0)
                        lblMD_NPV_M2_Deger.Text = dNPVM2.ToString("n2");
                    else
                        lblMD_NPV_M2_Deger.Text = "-";
                }
                #endregion



                ToplamOdemeHesapla();
                TaksitToplamiVeFarkiHesapla();
                Validation();
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        private void txtGrdMusteriSil_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                if (gvMusteriBilgileri.FocusedRowHandle == 0 && !bIsAdmin && iAktiviteID > 0)
                {
                    if (Genel.AktiviteYetkiGetir(AktiviteYetkileri.MusteriIsmiDegistirebilir_MusteriKayitDahil) == false)
                    {
                        Ayar.InfoMesajGoster(Genel.DilGetirMesaj(3096), Ayar.MesajTipi.Uyari, this);
                        return;
                    }
                }

                if (XtraMessageBox.Show(String.Format(Genel.DilGetirMesaj(2080),
                        gvMusteriBilgileri.GetFocusedRowCellValue("MusteriAdiSoyadi")), Genel.DilGetirMesaj(3), MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    gvMusteriBilgileri.DeleteRow(gvMusteriBilgileri.FocusedRowHandle);
                    UstPanelYukseklikAyarla();
                }
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MusteriDuzenle(int iMusteriID)
        {
            try
            {
                //F_Musteri frm = new F_Musteri();
                var Modal = Ayar.FormGoster();

                //frm._FormTipi = 2;
                //frm._MusteriID = C.ParseInt(iMusteriID);
                //frm.ShowDialog();


                F_MusteriIrtibat_Giris frm = new F_MusteriIrtibat_Giris(iMusteriID, false);
                frm.ShowDialog();



                var ListYeniMusteriIletisim = Genel.PrmDb.S_MusteriDetayIletisim(iMusteriID).ToList().FirstOrDefault();
                string strMusteriYeniAdSoyad = ListYeniMusteriIletisim.MusteriAdiSoyadi;
                MListler.dsAktivite_Musteri.Tables[0].Rows[gvMusteriBilgileri.FocusedRowHandle]["MusteriAdiSoyadi"] = ListYeniMusteriIletisim.MusteriAdiSoyadi;
                MListler.dsAktivite_Musteri.Tables[0].Rows[gvMusteriBilgileri.FocusedRowHandle]["EvTel1"] = ListYeniMusteriIletisim.EvTel1;
                MListler.dsAktivite_Musteri.Tables[0].Rows[gvMusteriBilgileri.FocusedRowHandle]["CepTel1"] = ListYeniMusteriIletisim.CepTel1;
                MListler.dsAktivite_Musteri.Tables[0].Rows[gvMusteriBilgileri.FocusedRowHandle]["IsTel1"] = ListYeniMusteriIletisim.IsTel1;
                MListler.dsAktivite_Musteri.Tables[0].Rows[gvMusteriBilgileri.FocusedRowHandle]["DigerTel1"] = ListYeniMusteriIletisim.DigerTel1;
                MListler.dsAktivite_Musteri.Tables[0].Rows[gvMusteriBilgileri.FocusedRowHandle]["Email"] = ListYeniMusteriIletisim.EMail;
                Modal.Close();
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void txtGrdMusteriDuzenle_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            MusteriDuzenle(C.ParseInt(gvMusteriBilgileri.GetFocusedRowCellValue("MusteriID")));
        }

        private void btnMusteriEkle_Click(object sender, EventArgs e)
        {
            try
            {
                F_Musteri frm = new F_Musteri();
                var Modal = Ayar.FormGoster();

                frm._FormTipi = 1;
                frm.ShowDialog();
                Modal.Close();


                if (gvMusteriBilgileri.RowCount < 1 && frm.DialogResult != System.Windows.Forms.DialogResult.OK)
                    Genel.SatisaAktarMusteriID = -1;

                if (Genel.SatisaAktarMusteriID > 0 && frm.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    bool bMusteriVarMi = false;
                    foreach (DataRow row in MListler.dsAktivite_Musteri.Tables[0].Rows)
                    {
                        if (row["MusteriID"].ToString() == Genel.SatisaAktarMusteriID.ToString())
                            bMusteriVarMi = true;
                    }


                    if (!bMusteriVarMi)
                    {
                        DataRow drw = MListler.dsAktivite_Musteri.Tables[0].NewRow();
                        drw["ID"] = -1;
                        drw["AktiviteID"] = iAktiviteID;
                        drw["MusteriID"] = Genel.SatisaAktarMusteriID;
                        drw["SiraNo"] = gvMusteriBilgileri.RowCount + 1;
                        drw["HissePayi"] = "";
                        var ListMusteriIletisim = Genel.PrmDb.S_MusteriDetayIletisim(Genel.SatisaAktarMusteriID).ToList().FirstOrDefault();
                        drw["MusteriAdiSoyadi"] = ListMusteriIletisim.MusteriAdiSoyadi;
                        drw["EvTel1"] = ListMusteriIletisim.EvTel1;
                        drw["CepTel1"] = ListMusteriIletisim.CepTel1;
                        drw["IsTel1"] = ListMusteriIletisim.IsTel1;
                        drw["DigerTel1"] = ListMusteriIletisim.DigerTel1;
                        drw["Email"] = ListMusteriIletisim.EMail;


                        MListler.dsAktivite_Musteri.Tables[0].Rows.Add(drw);
                        grdMusteriBilgileri.DataSource = MListler.dsAktivite_Musteri.Tables[0];
                    }
                    UstPanelYukseklikAyarla();
                }
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnPesinatEkle_Click(object sender, EventArgs e)
        {
            try
            {
                iPesinatSayisi++;
                PesinatPanelleriniAyarla();

            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPesinatSil_Click(object sender, EventArgs e)
        {
            try
            {
                iPesinatSayisi--;
                PesinatPanelleriniAyarla();
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void grdMusteriBilgileri_DoubleClick(object sender, EventArgs e)
        {
            if (bFormReadOnly)
                return;

            MusteriDuzenle(C.ParseInt(gvMusteriBilgileri.GetFocusedRowCellValue("MusteriID")));
        }

        private void bbtnDetay_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (gvGayrimenkulBilgileri.FocusedRowHandle < 0) 
                    return;
                int iGayrimenkulID = C.ParseInt(gvGayrimenkulBilgileri.GetFocusedRowCellValue("GayrimenkulID"));
                var Modal = Ayar.FormGoster();
                F_Gayrimenkul_Giris frm = new F_Gayrimenkul_Giris(iGayrimenkulID, 1, 4, Genel.Aktivite_ProjeID);
                frm.ShowDialog();
                Modal.Close();
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void cmbOdemeBicimiAna_EditValueChanging(object sender, ChangingEventArgs e)
        {
            try
            {
                if (!bIsFormLoaded)
                    return;
                if (gvVadeliOdemeBilgileri.DataRowCount > 0)
                {
                    Ayar.InfoMesajGoster(Genel.DilGetirMesaj(3106), Ayar.MesajTipi.Uyari, this, 5);
                    e.Cancel = true;
                }
            }
            catch (Exception Hata)
            {
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void IndirimComboYukle()
        {
            #region İndirim combosu yükleniyor... (İndirim tanımlarında uygulanacak projelerde tanımlanan proje ile secilen proje aynı olan indirimler alınıyor...
            if (MListler.MList_S_Aktivite_ProjedeUygulanacakIndirimler != null)
            {
                if (MListler.MList_S_Aktivite_ProjedeUygulanacakIndirimler.Count > 0)
                {
                    var ListIndirimler = MListler.MList_S_Aktivite_ProjedeUygulanacakIndirimler.Where(u => u.ProjeID == Genel.Aktivite_ProjeID).ToList(); // projede uygulanacak tüm indirimler.            
                    cmbIndirim.Properties.DataSource = ListIndirimler;
                }
            }
            #endregion
        }

        private void cmbOdemeBicimi_EditValueChanged(object sender, EventArgs e)
        {
            try
            {

                if (C.ParseInt(cmbKampanya.EditValue) != 0)
                    KampanyaTemizle();


                IndirimComboYukle();

                #region Kampanya combosu yükleniyor... (Kampanya tanımlarında uygulanacak projelerde seçilen ve ödeme bicimi aktivitedeki ile aynı olan kampanyalar alınıyor...
                if (MListler.MList_S_Aktivite_ProjedeUygulanacakKampanyalar != null)
                {
                    if (MListler.MList_S_Aktivite_ProjedeUygulanacakKampanyalar.Count > 0)
                    {
                        var ListKampanyalar = MListler.MList_S_Aktivite_ProjedeUygulanacakKampanyalar.Where(u => u.ProjeID == Genel.Aktivite_ProjeID).ToList(); // projede uygulanacak tüm kampanyalar.
                        var List2 = ListKampanyalar.Where(u => u.OdemeTahutuKisID_301 == C.ParseInt(cmbOdemeBicimiAna.EditValue)).ToList();
                        cmbKampanya.Properties.DataSource = List2;
                    }
                }
                #endregion


                pnlIndirimKampanya.Visible = true;
                tcOdeme.Visible = true;

                txtVadeli.EditValue = C.ParseDecimal(0).ToString("n2");
                txtBankaKredisi.EditValue = C.ParseDecimal(0).ToString("n2");
                txtFarkOdeme.EditValue = 0;
                txtToplamOdeme.EditValue = 0;

                int iOdemeBicimiID = C.ParseInt(cmbOdemeBicimiAna.EditValue);
                switch (iOdemeBicimiID)
                {
                    case 311: //peşin
                        tcOdeme.SelectedTabPageIndex = 0;
                        pnlBankaKredisi.Visible = false;
                        pnlVadeli.Visible = false;
                        pnlVadeliOdemeGrid.Visible = false;
                        for (int i = 2; i <= 10; i++)
                        {
                            Control[] ctrls = this.Controls.Find("txtPesinat" + i.ToString(), true);
                            if (ctrls.Length > 0)
                            {
                                ((ButtonEdit)ctrls[0]).EditValue = 0;
                            }
                            Control[] ctrlsDate = this.Controls.Find("dtmPesinat" + i.ToString(), true);
                            if (ctrlsDate.Length > 0)
                            {
                                ((DateEdit)ctrlsDate[0]).EditValue = null;
                            }
                        }
                        txtPesinat1.EditValue = txtSatisFiyati.EditValue;
                        pnlAltBilgi.Height = 30;
                        break;
                    case 312: //peşin + banka kredisi
                        pnlBankaKredisi.Visible = true;
                        pnlVadeli.Visible = false;


                        pnlVadeliOdemeGrid.Visible = false;
                        pnlAltBilgi.Height = 57;
                        tcOdeme.SelectedTabPageIndex = 0;
                        break;
                    case 313: //peşin + vadeli                    
                        pnlBankaKredisi.Visible = false;
                        pnlVadeli.Visible = true;
                        pnlVadeBilgileri.Visible = true;
                        pnlVadeliOdemeGrid.Visible = true;
                        btnVadeliOdemeSihirbazi.Checked = false;
                        pnlAltBilgi.Height = 57;
                        VadeliOdemeTableHazirla();
                        break;
                    case 314: //peşin + banka kredisi + vadeli
                        pnlBankaKredisi.Visible = true;
                        pnlVadeli.Visible = true;
                        pnlVadeBilgileri.Visible = true;
                        pnlVadeliOdemeGrid.Visible = true;
                        btnVadeliOdemeSihirbazi.Checked = false;
                        pnlAltBilgi.Height = 82;
                        VadeliOdemeTableHazirla();
                        break;
                }

                pnlVadeliOdemeGrid.BringToFront();
                ToplamOdemeHesapla();

            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }






        private void bbtnVaziyetPlani_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ResimGoster(1, bbtnVaziyetPlani.Caption); // Vaziyet Planı FormTipi = 1
        }
        private void bbtnBlokPlani_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ResimGoster(2, bbtnBlokPlani.Caption); // Blok Planı FormTipi = 2
        }
        private void bbtnKatPlani_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ResimGoster(3, bbtnKatPlani.Caption); // Kat Planı FormTipi = 3
        }
        private void bbtnDairePlani_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ResimGoster(4, bbtnDairePlaniGID.Caption); // Daire Planı (Gayrimenkul ID) FormTipi = 4
        }
        private void bbtnDairePlaniTip_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ResimGoster(5, bbtnDairePlaniTip.Caption); // Daire Planı (Tip) FormTipi = 5
        }
        private void bbtnDairePlaniTipSinifi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ResimGoster(6, bbtnDairePlaniTipSinifi.Caption); // Daire Planı (Tip Sınıfı) FormTipi = 6
        }
        private void bbtnProjeTanitimVideosu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ResimGoster(7, bbtnProjeTanitimVideosu.Caption); // Proje Tanıtım Videosu FormTipi = 7        
        }
        private void bbtnBlokPlani_Video_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ResimGoster(8, bbtnBlokPlani_Video.Caption); // Blok Planı (Video) - FormTipi = 8
        }
        private void bbtnKatPlani_Video_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ResimGoster(9, bbtnKatPlani_Video.Caption); // Kat Planı (Video) - FormTipi = 9
        }
        private void bbtnDairePlani_GID_Video_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ResimGoster(10, bbtnDairePlani_GID_Video.Caption); // Daire Planı (Gayrimenkul ID) (Video) - FormTipi = 10
        }
        private void bbtnDairePlaniTip_Video_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ResimGoster(11, bbtnDairePlaniTip_Video.Caption); // Daire Planı (Tip) (Video) - FormTipi = 11
        }
        private void bbtnDairePlaniTipSinifi_Video_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ResimGoster(12, bbtnDairePlaniTipSinifi_Video.Caption); // Daire Planı (Tip) (Video) - FormTipi = 12
        }
        private void popupGayrimenkulDetayi_Popup(object sender, EventArgs e)
        {
            int iProjeID = Genel.Aktivite_ProjeID;
            int iKatID = C.ParseInt(gvGayrimenkulBilgileri.GetFocusedRowCellValue("GayrimenkulKatID"));
            int iGayrimenkulID = C.ParseInt(gvGayrimenkulBilgileri.GetFocusedRowCellValue("GayrimenkulID"));
            int iGayrimenkulTipDurumSecID_206 = C.ParseInt(gvGayrimenkulBilgileri.GetFocusedRowCellValue("GayrimenkulTipDurumSecID_206"));
            int iProjeTipSinifiID = C.ParseInt(gvGayrimenkulBilgileri.GetFocusedRowCellValue("ProjeTipSiniflariID"));


            //Resimler
            if (listGayrimnekulDetayiPopup.Where(x => x.AktifMi == true && x.ProjeID == iProjeID && x.ResimTipiKisID_303 == 420).ToList().Count < 1) //vaziyet planı
                bbtnVaziyetPlani.ItemAppearance.Normal.Font = new Font(bbtnVaziyetPlani.Font, FontStyle.Strikeout);
            else
                bbtnVaziyetPlani.ItemAppearance.Normal.Font = new Font(bbtnVaziyetPlani.Font, FontStyle.Regular);

            if (listGayrimnekulDetayiPopup.Where(x => x.AktifMi == true && x.ProjeID == iProjeID && x.ResimTipiKisID_303 == 421).ToList().Count < 1) //blok planı
                bbtnBlokPlani.ItemAppearance.Normal.Font = new Font(bbtnBlokPlani.Font, FontStyle.Strikeout);
            else
                bbtnBlokPlani.ItemAppearance.Normal.Font = new Font(bbtnBlokPlani.Font, FontStyle.Regular);

            if (listGayrimnekulDetayiPopup.Where(x => x.AktifMi == true && x.ProjeID == iProjeID && x.ResimTipiKisID_303 == 422 && x.GayrimenkulKatID == iKatID).ToList().Count < 1) //kat planı
                bbtnKatPlani.ItemAppearance.Normal.Font = new Font(bbtnKatPlani.Font, FontStyle.Strikeout);
            else
                bbtnKatPlani.ItemAppearance.Normal.Font = new Font(bbtnKatPlani.Font, FontStyle.Regular);

            if (listGayrimnekulDetayiPopup.Where(x => x.AktifMi == true && x.ResimTipiKisID_303 == 423 && x.GayrimenkulID == iGayrimenkulID).ToList().Count < 1) //Daire Planı (Gayrimenkul ID)
                bbtnDairePlaniGID.ItemAppearance.Normal.Font = new Font(bbtnDairePlaniGID.Font, FontStyle.Strikeout);
            else
                bbtnDairePlaniGID.ItemAppearance.Normal.Font = new Font(bbtnDairePlaniGID.Font, FontStyle.Regular);

            if (listGayrimnekulDetayiPopup.Where(x => x.AktifMi == true && x.ProjeID == iProjeID && x.ResimTipiKisID_303 == 424 && x.GayrimenkulTipDurumSecID_206 == iGayrimenkulTipDurumSecID_206).ToList().Count < 1) //Daire Planı (Tip)
                bbtnDairePlaniTip.ItemAppearance.Normal.Font = new Font(bbtnDairePlaniTip.Font, FontStyle.Strikeout);
            else
                bbtnDairePlaniTip.ItemAppearance.Normal.Font = new Font(bbtnDairePlaniTip.Font, FontStyle.Regular);

            if (listGayrimnekulDetayiPopup.Where(x => x.AktifMi == true && x.ProjeID == iProjeID && x.ResimTipiKisID_303 == 425 && x.ProjeTipSiniflariID == iProjeTipSinifiID).ToList().Count < 1) //Daire Planı (Tip Sınıfı)
                bbtnDairePlaniTipSinifi.ItemAppearance.Normal.Font = new Font(bbtnDairePlaniTipSinifi.Font, FontStyle.Strikeout);
            else
                bbtnDairePlaniTipSinifi.ItemAppearance.Normal.Font = new Font(bbtnDairePlaniTipSinifi.Font, FontStyle.Regular);



            //Videolar
            var listVideo = listGayrimnekulDetayiPopup.Where(x => x.AktifMi == true && x.DosyaUzantisi.ToUpper() != "JPG").ToList();
            if (listVideo.Where(x => x.ProjeID == iProjeID).Count() < 1)
                bbtnProjeTanitimVideosu.ItemAppearance.Normal.Font = new Font(bbtnProjeTanitimVideosu.Font, FontStyle.Strikeout);
            else
                bbtnProjeTanitimVideosu.ItemAppearance.Normal.Font = new Font(bbtnProjeTanitimVideosu.Font, FontStyle.Regular);

            if (listVideo.Where(x => x.AktifMi == true && x.ProjeID == iProjeID && x.ResimTipiKisID_303 == 428).ToList().Count < 1) //blok planı (Video)
                bbtnBlokPlani_Video.ItemAppearance.Normal.Font = new Font(bbtnBlokPlani_Video.Font, FontStyle.Strikeout);
            else
                bbtnBlokPlani_Video.ItemAppearance.Normal.Font = new Font(bbtnBlokPlani_Video.Font, FontStyle.Regular);

            if (listVideo.Where(x => x.AktifMi == true && x.ProjeID == iProjeID && x.ResimTipiKisID_303 == 429 && x.GayrimenkulKatID == iKatID).ToList().Count < 1) //kat planı (Video)
                bbtnKatPlani_Video.ItemAppearance.Normal.Font = new Font(bbtnKatPlani_Video.Font, FontStyle.Strikeout);
            else
                bbtnKatPlani_Video.ItemAppearance.Normal.Font = new Font(bbtnKatPlani_Video.Font, FontStyle.Regular);

            if (listVideo.Where(x => x.AktifMi == true && x.ProjeID == iProjeID && x.ResimTipiKisID_303 == 430 && x.GayrimenkulID == iGayrimenkulID).ToList().Count < 1) //Daire Planı (Gayrimenkul ID) (Video)
                bbtnDairePlani_GID_Video.ItemAppearance.Normal.Font = new Font(bbtnDairePlani_GID_Video.Font, FontStyle.Strikeout);
            else
                bbtnDairePlani_GID_Video.ItemAppearance.Normal.Font = new Font(bbtnDairePlani_GID_Video.Font, FontStyle.Regular);

            if (listVideo.Where(x => x.AktifMi == true && x.ProjeID == iProjeID && x.ResimTipiKisID_303 == 431 && x.GayrimenkulTipDurumSecID_206 == iGayrimenkulTipDurumSecID_206).ToList().Count < 1) //Daire Planı (Tip) (Video)
                bbtnDairePlaniTip_Video.ItemAppearance.Normal.Font = new Font(bbtnDairePlaniTip_Video.Font, FontStyle.Strikeout);
            else
                bbtnDairePlaniTip_Video.ItemAppearance.Normal.Font = new Font(bbtnDairePlaniTip_Video.Font, FontStyle.Regular);

            if (listVideo.Where(x => x.AktifMi == true && x.ProjeID == iProjeID && x.ResimTipiKisID_303 == 432 && x.ProjeTipSiniflariID == iProjeTipSinifiID).ToList().Count < 1) //Daire Planı (Tip Sınıfı) (Video)
                bbtnDairePlaniTipSinifi_Video.ItemAppearance.Normal.Font = new Font(bbtnDairePlaniTipSinifi_Video.Font, FontStyle.Strikeout);
            else
                bbtnDairePlaniTipSinifi_Video.ItemAppearance.Normal.Font = new Font(bbtnDairePlaniTipSinifi_Video.Font, FontStyle.Regular);
        }

        private void ResimGoster(int iFormTipi, string Caption)
        {
            try
            {
                if (iFormTipi >= 7) //video
                {
                    var listVideo = listGayrimnekulDetayiPopup.Where(x => x.AktifMi == true && x.ProjeID == Genel.Aktivite_ProjeID && x.DosyaUzantisi.ToUpper() != "JPG").ToList();
                    if (listVideo.Count < 1)
                        return;

                    string DosyaYolu = Ayar.AyarGetirKarakter(4) + listVideo.FirstOrDefault().RgID + "." + listVideo.FirstOrDefault().DosyaUzantisi;
                    this.Cursor = Cursors.WaitCursor;
                    byte[] bytearry = Genel.PrmDb.S_DosyaGet(DosyaYolu).FirstOrDefault();
                    if (bytearry != null && bytearry.Length > 0)
                    {
                        string localpath = Path.GetTempPath() + listVideo.FirstOrDefault().RgID + "." + listVideo.FirstOrDefault().DosyaUzantisi;
                        File.WriteAllBytes(localpath, bytearry.ToArray());
                        this.Cursor = Cursors.Default;
                        F_SesKaydi frm = new F_SesKaydi(localpath);
                        frm.ShowDialog();
                    }

                }
                else
                {
                    if (gvGayrimenkulBilgileri.FocusedRowHandle < 0)
                        return;

                    int iGayrimenkulID = C.ParseInt(gvGayrimenkulBilgileri.GetFocusedRowCellValue("GayrimenkulID"));
                    int iGayrimenkulTipDurumSecID_206 = C.ParseInt(gvGayrimenkulBilgileri.GetFocusedRowCellValue("GayrimenkulTipDurumSecID_206"));
                    int iProjeTipSinifiID = C.ParseInt(gvGayrimenkulBilgileri.GetFocusedRowCellValue("ProjeTipSiniflariID"));

                    F_ResimGoster frm = new F_ResimGoster(iGayrimenkulID, iGayrimenkulTipDurumSecID_206, iProjeTipSinifiID, iFormTipi);
                    frm._Caption = Caption;
                    var Modal = Ayar.FormGoster();
                    frm.ShowDialog();
                    Modal.Close();
                }
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        bool bFormKapatmaIzniVarMi = true;


        private void F_Aktivite_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {

                if (!bFormKapatmaIzniVarMi)
                {
                    e.Cancel = true;
                    return;
                }

                this.Cursor = Cursors.WaitCursor;
                Genel.PrmDb.SEDS_AktiviteKayitKontrol(0, 3, iAktiviteID, Genel.AktifKullaniciID, Genel.AktifKullaniciAdiSoyadi);

                if (bIsFormSaved)
                {
                    F_Aktivasyon f_Aktivasyon = (F_Aktivasyon)Application.OpenForms["F_Aktivasyon"];
                    if (f_Aktivasyon != null)
                        f_Aktivasyon.DataGetir();

                    F_AktivasyonIptal f_AktivasyonIptal = (F_AktivasyonIptal)Application.OpenForms["F_AktivasyonIptal"];
                    if (f_AktivasyonIptal != null)
                        f_AktivasyonIptal.DataGetir();

                    F_GayrimenkulSec f_SatisaHazirlar = (F_GayrimenkulSec)Application.OpenForms["F_GayrimenkulSec"];
                    if (f_SatisaHazirlar != null)
                        f_SatisaHazirlar.DataGetir();
                }

                MListler.dsAktivite.Tables[0].Rows.Clear();
                MListler.dsAktivite_Gayrimenkul.Tables[0].Rows.Clear();
                MListler.dsAktivite_Musteri.Tables[0].Rows.Clear();
                MListler.dsAktivite_OdemePlaniSenet.Tables[0].Rows.Clear();
                Genel.Aktivite_ProjeID = -1;
                Genel.bAktiviteYanMenuReadOnly = false;

                this.Cursor = Cursors.Default;
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DigerDatalarClick()
        {
            TabdaFormAc("F_AktiviteDigerDatalar");
        }
        private void GayrimenkulDataClick()
        {
            TabdaFormAc("F_AktiviteGayrimenkulData");
        }


        private void DosyalarClick()
        {
            try
            {

                #region Açılmak istenen form zaten açıksa tab pageine konumlanıp fonksiyondan çıkılıyor...
                foreach (XtraTabPage page in tc.TabPages)
                {
                    string strPageName = page.Name;

                    if (strPageName == "F_Dosyalar")
                    {
                        tc.SelectedTabPage = TabPageGetir(strPageName);
                        return;
                    }
                }
                #endregion

                #region İlgili Form ve Tabpage oluşturuluyor...
                F_Dosyalar frm = new F_Dosyalar(-1, 170, iAktiviteID, -1, -1, -1);
                frm._AktivitedenMiAcildi = true;
                //frm._FormReadOnly = bFormReadOnly;
                frm._FormReadOnly = false;
                XtraTabPage tp = new XtraTabPage();
                tp.Name = frm.Name;
                tc.TabPages.Add(tp);
                frm.TopLevel = false;
                frm.Visible = true;
                frm.FormBorderStyle = FormBorderStyle.None;
                tc.SelectedTabPage = tc.TabPages[tc.TabPages.Count - 1]; //eklenen form tabı seçili hale getiriliyor...
                #endregion

                #region İlgili tabpage içine ScrollableControl oluşturuluyor.Ve içine form ekleniyor...
                XtraScrollableControl scroll = new XtraScrollableControl();
                tp.Controls.Add(scroll);
                scroll.Name = "Scroll" + tc.SelectedTabPageIndex;
                scroll.Dock = DockStyle.Fill;
                tc.TabPages[tc.TabPages.Count - 1].Text = frm.Text;
                scroll.Controls.Add(frm);
                frm.Dock = DockStyle.Fill;
                #endregion
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void EvraklarClick()
        {
            try
            {

                #region Açılmak istenen form zaten açıksa tab pageine konumlanıp fonksiyondan çıkılıyor...
                foreach (XtraTabPage page in tc.TabPages)
                {
                    string strPageName = page.Name;

                    if (strPageName == "F_Evraklar_Goster")
                    {
                        tc.SelectedTabPage = TabPageGetir(strPageName);
                        return;
                    }
                }
                #endregion

                #region İlgili Form ve Tabpage oluşturuluyor...
                int iProjeID = -1;
                if (Genel.Aktivite_ProjeID > 0)
                    iProjeID = Genel.Aktivite_ProjeID;

                int iMusteriID = C.ParseInt(gvMusteriBilgileri.GetRowCellValue(0, "MusteriID")) > 0 ? C.ParseInt(gvMusteriBilgileri.GetRowCellValue(0, "MusteriID")) : -1;
                F_Evraklar_Goster frm = new F_Evraklar_Goster(1008, iMusteriID, iProjeID, iAktiviteID, -1, -1, -1, -1, -1, -1, -1);
                frm._FormHemenAcilsinMi = false;

                XtraTabPage tp = new XtraTabPage();
                tp.Name = frm.Name;
                tc.TabPages.Add(tp);
                frm.TopLevel = false;
                frm.Visible = true;
                frm.FormBorderStyle = FormBorderStyle.None;
                tc.SelectedTabPage = tc.TabPages[tc.TabPages.Count - 1]; //eklenen form tabı seçili hale getiriliyor...
                #endregion

                #region İlgili tabpage içine ScrollableControl oluşturuluyor.Ve içine form ekleniyor...
                XtraScrollableControl scroll = new XtraScrollableControl();
                tp.Controls.Add(scroll);
                scroll.Name = "Scroll" + tc.SelectedTabPageIndex;
                scroll.Dock = DockStyle.Fill;
                tc.TabPages[tc.TabPages.Count - 1].Text = frm.Text;
                scroll.Controls.Add(frm);
                frm.Dock = DockStyle.Fill;
                #endregion
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void OdemelerTahsilatlarClick()
        {
            if (iAktiviteID < 1)
                return;
            try
            {

                #region Açılmak istenen form zaten açıksa tab pageine konumlanıp fonksiyondan çıkılıyor...
                foreach (XtraTabPage page in tc.TabPages)
                {
                    string strPageName = page.Name;

                    if (strPageName == "F_AktiviteOdemeler")
                    {
                        tc.SelectedTabPage = TabPageGetir(strPageName);
                        return;
                    }
                }
                #endregion

                #region İlgili Form ve Tabpage oluşturuluyor...
                F_AktiviteOdemeler frm = new F_AktiviteOdemeler(iAktiviteID);
                frm._FormReadOnly = bFormReadOnly;
                XtraTabPage tp = new XtraTabPage();
                tp.Name = frm.Name;
                tc.TabPages.Add(tp);
                frm.TopLevel = false;
                frm.Visible = true;
                frm.FormBorderStyle = FormBorderStyle.None;
                tc.SelectedTabPage = tc.TabPages[tc.TabPages.Count - 1]; //eklenen form tabı seçili hale getiriliyor...
                #endregion

                #region İlgili tabpage içine ScrollableControl oluşturuluyor.Ve içine form ekleniyor...
                XtraScrollableControl scroll = new XtraScrollableControl();
                tp.Controls.Add(scroll);
                scroll.Name = "Scroll" + tc.SelectedTabPageIndex;
                scroll.Dock = DockStyle.Fill;
                tc.TabPages[tc.TabPages.Count - 1].Text = frm.Text;
                scroll.Controls.Add(frm);
                frm.Dock = DockStyle.Fill;
                #endregion
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void IadeEdilenOdemeClick()
        {
            if (iAktiviteID < 1)
                return;
            try
            {
                //rezerve durumunda Gyr.Bedeli Dış.Ödemelere ve İade Edilen Ödemelere giremez.
                if (C.ParseInt(cmbAktiviteDurumu.EditValue) == 302) //Rezerve
                {
                    Ayar.InfoMesajGoster(Genel.DilGetirMesaj(3115), Ayar.MesajTipi.Uyari, this);
                    return;
                }

                #region Açılmak istenen form zaten açıksa tab pageine konumlanıp fonksiyondan çıkılıyor...
                foreach (XtraTabPage page in tc.TabPages)
                {
                    string strPageName = page.Name;

                    if (strPageName == "F_AktiviteIadeEdilenOdeme")
                    {
                        tc.SelectedTabPage = TabPageGetir(strPageName);
                        return;
                    }
                }
                #endregion

                #region İlgili Form ve Tabpage oluşturuluyor...
                F_AktiviteIadeEdilenOdeme frm = new F_AktiviteIadeEdilenOdeme(iAktiviteID);
                frm._FormReadOnly = bFormReadOnly;
                XtraTabPage tp = new XtraTabPage();
                tp.Name = frm.Name;
                tc.TabPages.Add(tp);
                frm.TopLevel = false;
                frm.Visible = true;
                frm.FormBorderStyle = FormBorderStyle.None;
                tc.SelectedTabPage = tc.TabPages[tc.TabPages.Count - 1]; //eklenen form tabı seçili hale getiriliyor...
                #endregion

                #region İlgili tabpage içine ScrollableControl oluşturuluyor.Ve içine form ekleniyor...
                XtraScrollableControl scroll = new XtraScrollableControl();
                tp.Controls.Add(scroll);
                scroll.Name = "Scroll" + tc.SelectedTabPageIndex;
                scroll.Dock = DockStyle.Fill;
                tc.TabPages[tc.TabPages.Count - 1].Text = frm.Text;
                scroll.Controls.Add(frm);
                frm.Dock = DockStyle.Fill;
                #endregion
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void SatisSonrasiIslemlerClick()
        {

        }
        private void GayrimenkulBedeliDisindakiOdemelerClick()
        {
            if (iAktiviteID < 1)
                return;
            try
            {
                //rezerve durumunda Gyr.Bedeli Dış.Ödemelere ve İade Edilen Ödemelere giremez.
                if (C.ParseInt(cmbAktiviteDurumu.EditValue) == 302) //Rezerve
                {
                    Ayar.InfoMesajGoster(Genel.DilGetirMesaj(3116), Ayar.MesajTipi.Uyari, this);
                    return;
                }

                #region Açılmak istenen form zaten açıksa tab pageine konumlanıp fonksiyondan çıkılıyor...
                foreach (XtraTabPage page in tc.TabPages)
                {
                    string strPageName = page.Name;

                    if (strPageName == "F_Aktivite_GayrimenkulBedeliDisindakiOdemeler")
                    {
                        tc.SelectedTabPage = TabPageGetir(strPageName);
                        return;
                    }
                }
                #endregion

                #region İlgili Form ve Tabpage oluşturuluyor...
                F_Aktivite_GayrimenkulBedeliDisindakiOdemeler frm = new F_Aktivite_GayrimenkulBedeliDisindakiOdemeler(iAktiviteID);
                frm._AktiviteDovizID = C.ParseInt(cmbDoviz.EditValue);
                frm._FormReadOnly = bFormReadOnly;
                XtraTabPage tp = new XtraTabPage();
                tp.Name = frm.Name;
                tc.TabPages.Add(tp);
                frm.TopLevel = false;
                frm.Visible = true;
                frm.FormBorderStyle = FormBorderStyle.None;
                tc.SelectedTabPage = tc.TabPages[tc.TabPages.Count - 1]; //eklenen form tabı seçili hale getiriliyor...
                #endregion

                #region İlgili tabpage içine ScrollableControl oluşturuluyor.Ve içine form ekleniyor...
                XtraScrollableControl scroll = new XtraScrollableControl();
                tp.Controls.Add(scroll);
                scroll.Name = "Scroll" + tc.SelectedTabPageIndex;
                scroll.Dock = DockStyle.Fill;
                tc.TabPages[tc.TabPages.Count - 1].Text = frm.Text;
                scroll.Controls.Add(frm);
                frm.Dock = DockStyle.Fill;
                #endregion
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }







        private void OnDegerGetir()
        {
            try
            {
                var OnDegerList = MListler.OnDegerGetir(Genel.TabloIDGetir(this.Name), Genel.FormIDGetir(this.Name));
                foreach (var item in OnDegerList)
                {
                    if (item.NesneAdi.Length > 0)
                    {
                        Control[] ctrl = this.Controls.Find(item.NesneAdi, true);
                        foreach (var ctrlitem in ctrl)
                        {
                            if (item.NesneAdi == ctrlitem.Name)
                            {
                                if (item.TabloAlanTipi == "nvarchar" || item.TabloAlanTipi == "varchar")
                                {
                                    if (ctrlitem.GetType().Name == "TextEdit")
                                    {
                                        ((TextEdit)ctrl[0]).Text = item.OndegerChar;
                                    }
                                    else if (ctrlitem.GetType().Name == "MemoEdit")
                                    {
                                        ((MemoEdit)ctrl[0]).Text = item.OndegerChar;
                                    }
                                }
                                else if (item.TabloAlanTipi == "int")
                                {
                                    if (ctrlitem.GetType().Name == "LookUpEdit")
                                    {
                                        ((LookUpEdit)ctrl[0]).EditValue = C.ParseInt(item.OndegerChar);
                                    }
                                    else if (ctrlitem.GetType().Name == "TextEdit")
                                    {
                                        ((TextEdit)ctrl[0]).Text = item.OndegerChar.ToString();
                                    }
                                    else if (ctrlitem.GetType().Name == "SpinEdit")
                                    {
                                        ((SpinEdit)ctrl[0]).EditValue = C.ParseInt(item.OndegerChar);
                                    }
                                }
                                else if (item.TabloAlanTipi == "bit")
                                {
                                    if (ctrlitem.GetType().Name == "CheckEdit")
                                    {
                                        ((CheckEdit)ctrl[0]).Checked = C.ParseBool(item.OndegerChar);
                                    }
                                }
                                else if (item.TabloAlanTipi == "decimal")
                                {
                                    if (ctrlitem.GetType().Name == "SpinEdit")
                                    {
                                        ((SpinEdit)ctrl[0]).Value = C.ParseDecimal(item.OndegerChar);
                                    }
                                    else if (ctrlitem.GetType().Name == "TextEdit")
                                    {
                                        ((TextEdit)ctrl[0]).Text = item.OndegerChar.ToString();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Hata)
            {
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gvGayrimenkulBilgileri_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            try
            {

                if (e.MenuType == GridMenuType.Column)
                {
                    GridViewColumnMenu menu = e.Menu as GridViewColumnMenu;
                    if (menu.Column != null)
                    {
                        DXMenuItem miArtanSirala = GetItemByStringId(e.Menu, GridStringId.MenuColumnSortAscending);
                        if (miArtanSirala != null)
                            miArtanSirala.Visible = false;
                        DXMenuItem miAzalanSirala = GetItemByStringId(e.Menu, GridStringId.MenuColumnSortDescending);
                        if (miAzalanSirala != null)
                            miAzalanSirala.Visible = false;

                        DXMenuItem miSutunaGoreGrupla = GetItemByStringId(e.Menu, GridStringId.MenuColumnGroup);
                        if (miSutunaGoreGrupla != null)
                            miSutunaGoreGrupla.Visible = false;

                        DXMenuItem miGruplamaKutusunuGoster = GetItemByStringId(e.Menu, GridStringId.MenuGroupPanelShow);
                        if (miGruplamaKutusunuGoster != null)
                            miGruplamaKutusunuGoster.Visible = false;

                        //filtreleme iptali için gv'daki filter propertyleri kapatılır...
                    }
                }
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void gvMusteriBilgileri_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            try
            {
                if (e.MenuType == GridMenuType.Column)
                {
                    GridViewColumnMenu menu = e.Menu as GridViewColumnMenu;
                    if (menu.Column != null)
                    {
                        DXMenuItem miArtanSirala = GetItemByStringId(e.Menu, GridStringId.MenuColumnSortAscending);
                        if (miArtanSirala != null)
                            miArtanSirala.Visible = false;
                        DXMenuItem miAzalanSirala = GetItemByStringId(e.Menu, GridStringId.MenuColumnSortDescending);
                        if (miAzalanSirala != null)
                            miAzalanSirala.Visible = false;

                        DXMenuItem miSutunaGoreGrupla = GetItemByStringId(e.Menu, GridStringId.MenuColumnGroup);
                        if (miSutunaGoreGrupla != null)
                            miSutunaGoreGrupla.Visible = false;

                        DXMenuItem miGruplamaKutusunuGoster = GetItemByStringId(e.Menu, GridStringId.MenuGroupPanelShow);
                        if (miGruplamaKutusunuGoster != null)
                            miGruplamaKutusunuGoster.Visible = false;

                        //filtreleme iptali için gv'daki filter propertyleri kapatılır...
                    }
                }
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private DXMenuItem GetItemByStringId(DXPopupMenu menu, GridStringId id)
        {
            foreach (DXMenuItem item in menu.Items)

                if (item.Caption == GridLocalizer.Active.GetLocalizedString(id))

                    return item;

            return null;
        }



        private void btnVadeSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvVadeliOdemeBilgileri.FocusedRowHandle >= 0)
                    gvVadeliOdemeBilgileri.DeleteRow(gvVadeliOdemeBilgileri.FocusedRowHandle);
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVadeTumunuSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (MListler.dsAktivite_OdemePlaniSenet.Tables[0].Rows.Count < 1)
                    return;


                decimal dVadeliToplamOdeme = 0;
                foreach (DataRow row in MListler.dsAktivite_OdemePlaniSenet.Tables[0].Rows)
                {
                    dVadeliToplamOdeme += C.ParseDecimal(row["KapatilanBorcTutari"]);
                }

                if (dVadeliToplamOdeme > 0)
                {
                    Ayar.InfoMesajGoster(Genel.DilGetirMesaj(3105), Ayar.MesajTipi.Uyari, this);
                    return;
                }


                MListler.dsAktivite_OdemePlaniSenet.Tables[0].Rows.Clear();
                TaksitToplamiVeFarkiHesapla();
                Validation();
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void rgVadeliOdemeTipi_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {


                if (rgVadeliOdemeTipi.SelectedIndex == 1) //eşit taksitli
                {
                    lblSabitTutar.Enabled = false;
                    txtSabitTutar.Enabled = false;
                    txtSabitTutar.EditValue = null;
                    dxErrorProvider1.SetError(txtSabitTutar, String.Empty);
                }
                else //sabit tutar
                {
                    lblSabitTutar.Enabled = true;
                    txtSabitTutar.Enabled = true;
                }
            }
            catch (Exception Hata)
            {
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private decimal ToplamPesinatTutariGetir()
        {
            try
            {

                decimal dToplamPesinat = C.ParseDecimal(txtPesinat1.EditValue);
                if (C.ParseDecimal(txtPesinat2.EditValue) > 0)
                    dToplamPesinat += C.ParseDecimal(txtPesinat2.EditValue);
                if (C.ParseDecimal(txtPesinat3.EditValue) > 0)
                    dToplamPesinat += C.ParseDecimal(txtPesinat3.EditValue);
                if (C.ParseDecimal(txtPesinat4.EditValue) > 0)
                    dToplamPesinat += C.ParseDecimal(txtPesinat4.EditValue);
                if (C.ParseDecimal(txtPesinat5.EditValue) > 0)
                    dToplamPesinat += C.ParseDecimal(txtPesinat5.EditValue);
                if (C.ParseDecimal(txtPesinat6.EditValue) > 0)
                    dToplamPesinat += C.ParseDecimal(txtPesinat6.EditValue);
                if (C.ParseDecimal(txtPesinat7.EditValue) > 0)
                    dToplamPesinat += C.ParseDecimal(txtPesinat7.EditValue);
                if (C.ParseDecimal(txtPesinat8.EditValue) > 0)
                    dToplamPesinat += C.ParseDecimal(txtPesinat8.EditValue);
                if (C.ParseDecimal(txtPesinat9.EditValue) > 0)
                    dToplamPesinat += C.ParseDecimal(txtPesinat9.EditValue);
                if (C.ParseDecimal(txtPesinat10.EditValue) > 0)
                    dToplamPesinat += C.ParseDecimal(txtPesinat10.EditValue);

                return C.ParseDecimal(dToplamPesinat.ToString("n2"));
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }


        private void ToplamOdemeHesapla()
        {
            try
            {
                decimal dSatisFiyati = C.ParseDecimal(txtSatisFiyati.EditValue);
                decimal dListeFiyati = C.ParseDecimal(txtListeFiyati.EditValue);
                decimal dToplamPesinat = ToplamPesinatTutariGetir();
                decimal dToplamOdeme = dToplamPesinat + C.ParseDecimal(txtVadeli.EditValue) + C.ParseDecimal(txtBankaKredisi.EditValue);
                txtToplamOdeme.EditValue = dToplamOdeme.ToString("n2");

                txtFarkOdeme.EditValue = (dSatisFiyati - dToplamOdeme).ToString("n2");
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void txtPesinat1_Properties_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {

                ButtonEdit txt = (ButtonEdit)sender;
                if (txt.Tag == null || txt.Tag.ToString() != "0")
                    return;

                decimal dSatisFiyati = C.ParseDecimal(txtSatisFiyati.EditValue);
                decimal dPesinat = C.ParseDecimal(txt.EditValue);

                if (dPesinat <= 0 || dSatisFiyati <= 0)
                    return;

                txt.EditValue = ((dSatisFiyati * dPesinat) / 100).ToString("n2");
                txt.Tag = "1"; //peşinat,banka kredisi,vadelenecek tutar girişlerinin yanındaki buttonclick bir basmada iki kez tetikliyor. bu nedenle ikinciyi kesmek için flag tutuluyor.
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void txtPesinat1_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                ButtonEdit txt = (ButtonEdit)sender;
                txt.Tag = "0";
                ToplamOdemeHesapla();
                Validation();
            }
            catch (Exception Hata)
            {
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txtPesinat1_EditValueChanging(object sender, ChangingEventArgs e)
        {
            ButtonEdit txt = (ButtonEdit)sender;
            txt.Tag = "0";
        }
        private void txtPesinat1_Enter(object sender, EventArgs e)
        {
            try
            {
                ButtonEdit txt = (ButtonEdit)sender;
                txt.Properties.Buttons[0].Visible = true;
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtPesinat1_Leave(object sender, EventArgs e)
        {
            try
            {
                ButtonEdit txt = (ButtonEdit)sender;
                txt.Properties.Buttons[0].Visible = false;
                txt.Tag = "0";
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtListeFiyati_EditValueChanged(object sender, EventArgs e)
        {
            ToplamOdemeHesapla();
        }



        private void txtVadeli_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                txtVadeli.Tag = "0";
                ToplamOdemeHesapla();
                TaksitToplamiVeFarkiHesapla();
                Validation();
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void txtGrdOdemeSil_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                if (gvVadeliOdemeBilgileri.FocusedRowHandle < 0)
                    return;

                decimal dOdenen = C.ParseDecimal(gvVadeliOdemeBilgileri.GetFocusedRowCellValue("KapatilanBorcTutari"));
                if (dOdenen > 0)
                {
                    Ayar.InfoMesajGoster(Genel.DilGetirMesaj(3105), Ayar.MesajTipi.Uyari, this);
                    return;
                }

                gvVadeliOdemeBilgileri.DeleteRow(gvVadeliOdemeBilgileri.FocusedRowHandle);
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gvVadeliOdemeBilgileri_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            try
            {
                if (e.IsGetData)
                    e.Value = e.ListSourceRowIndex + 1;   //no kolonuna veri yazılıyor...
            }
            catch (Exception Hata)
            {
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void TaksitEkle(DateTime dtmTaksitTarihi, decimal dBorcTutari)
        {
            try
            {
                if (MListler.dsAktivite_OdemePlaniSenet.Tables[0].Rows.Count == 0)
                    MListler.dsAktivite_OdemePlaniSenet.Tables[0].Rows.Clear();

                DataRow drwYeniTaksit = MListler.dsAktivite_OdemePlaniSenet.Tables[0].NewRow();
                drwYeniTaksit["ID"] = -1;
                drwYeniTaksit["CariIslemYeriKisID_310"] = 550; //Aktivite Ödeme Planı
                drwYeniTaksit["KayitID"] = iAktiviteID;
                drwYeniTaksit["TipKisID_304"] = 380; //Vadeli Ödeme
                drwYeniTaksit["PesinatNo"] = 0;
                drwYeniTaksit["OdemeTipiKisID_302"] = C.ParseInt(cmbOdemeSekli.EditValue);
                drwYeniTaksit["BorcTutari"] = dBorcTutari;
                drwYeniTaksit["BorcTutariToplam"] = dBorcTutari;
                drwYeniTaksit["BorcDovizID"] = C.ParseInt(cmbDoviz.EditValue);
                drwYeniTaksit["BorcVadeFarkiTutari"] = 0;
                drwYeniTaksit["ParcalamaYapilmisMi"] = false;
                drwYeniTaksit["ParcalamadanOncekiTutar"] = 0;
                drwYeniTaksit["CariBorcParcalamaID"] = 0;
                drwYeniTaksit["CariBorcParcalamadanGelenID"] = 0;
                drwYeniTaksit["Aciklama"] = "";
                drwYeniTaksit["KarsiParaDovizTutari"] = 0;
                drwYeniTaksit["KarsiParaDovizID"] = 0;
                drwYeniTaksit["TufeliMi"] = false;
                drwYeniTaksit["AktiviteTefeTufeOranArtiID"] = 0;
                drwYeniTaksit["AraOdemeMi"] = false;
                drwYeniTaksit["KdvOdemesiMi"] = false;
                drwYeniTaksit["FaizHesabiTaksitTutarindanYapilsinMi"] = false;
                drwYeniTaksit["OdemeYeriBilgiMesaji"] = "";
                drwYeniTaksit["CekSeriNo"] = "";
                drwYeniTaksit["KapatilanBorcTutari"] = 0;
                drwYeniTaksit["KapatilanToplamBorcTutari"] = 0;
                drwYeniTaksit["Tarih"] = dtmTaksitTarihi.Date;
                drwYeniTaksit["TufeBaslangicTarihi"] = DBNull.Value; //C.ParseDateTime("1901.01.01 00:00:00.000");
                drwYeniTaksit["TufeBitisTarihi"] = DBNull.Value; // C.ParseDateTime("1901.01.01 00:00:00.000");
                MListler.dsAktivite_OdemePlaniSenet.Tables[0].Rows.Add(drwYeniTaksit);
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DateTime VadeGridMaxTarihBul()
        {
            try
            {
                int iPeriod = C.ParseInt(txtPeriod.EditValue);
                DateTime dtmMaxTarih = new DateTime();
                if (MListler.dsAktivite_OdemePlaniSenet.Tables[0].Rows.Count < 1)
                    dtmMaxTarih = dtmIlkTaksitTarihi.DateTime.Date;
                else
                {
                    DateTime dtmTemp = C.ParseDateTime(MListler.dsAktivite_OdemePlaniSenet.Tables[0].Rows[MListler.dsAktivite_OdemePlaniSenet.Tables[0].Rows.Count - 1]["Tarih"]).AddMonths(iPeriod);
                    dtmMaxTarih = dtmTemp.Date;
                }

                return dtmMaxTarih.Date;
            }
            catch (Exception Hata)
            {
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return C.ParseDateTime("1901.01.01 00:00:00.000");
            }
        }


        private void UygulaVeKapat(bool bKapatilacakMi)
        {
            try
            {

                if (C.ParseDecimal(txtVadeli.EditValue) <= 0 || C.ParseDecimal(txtFarkAlt.EditValue) <= 0)
                {
                    btnVadeliOdemeSihirbazi.Checked = !btnVadeliOdemeSihirbazi.Checked;
                    return;
                }

                grdVadeliOdemeBilgileri.DataSource = null;
                int iTaksitSayisi = C.ParseInt(txtTaksitSayisi.EditValue);

                if (C.ParseInt(rgVadeliOdemeTipi.EditValue) == 0) //sabit ödeme
                {
                    #region Sabit Tutar kontrolü
                    if (C.ParseDecimal(txtSabitTutar.EditValue) <= 0)
                    {
                        dxErrorProvider1.SetError(txtSabitTutar, " ");
                        return;
                    }
                    else
                        dxErrorProvider1.SetError(txtSabitTutar, String.Empty);
                    #endregion


                    //if (iTaksitSayisi > 0)
                    //{
                    //    decimal dTutar = C.ParseDecimal(txtSabitTutar.EditValue);
                    //    if (iTaksitSayisi * dTutar > C.ParseDecimal(txtFarkAlt.EditValue))
                    //    {
                    //        Ayar.InfoMesajGoster(Genel.DilGetirMesaj(3214), Ayar.MesajTipi.Uyari, this);
                    //        btnVadeTumunuSil_Click(null, null);
                    //        return;
                    //    }
                    //    for (int i = 0; i < iTaksitSayisi; i++)
                    //    {
                    //        TaksitToplamiVeFarkiHesapla();

                    //        if (i == iTaksitSayisi - 1 && (C.ParseDecimal(txtFarkAlt.EditValue) - dTutar < dTutar))
                    //            dTutar = dTutar + (C.ParseDecimal(txtVadeli.EditValue) % dTutar);

                    //        TaksitEkle(VadeGridMaxTarihBul(), dTutar);
                    //    }
                    //}
                    //else
                    //    TaksitEkle(dtmIlkTaksitTarihi.DateTime.Date, C.ParseDecimal(txtSabitTutar.EditValue));


                    if (iTaksitSayisi > 0)
                    {
                        for (int i = 0; i < iTaksitSayisi; i++)
                        {

                            TaksitEkle(VadeGridMaxTarihBul(), C.ParseDecimal(txtSabitTutar.EditValue));
                        }
                    }
                    else
                        TaksitEkle(dtmIlkTaksitTarihi.DateTime.Date, C.ParseDecimal(txtSabitTutar.EditValue));


                }
                else //eşit taksitli
                {
                    for (int i = 0; i < iTaksitSayisi; i++)
                    {
                        if (rgVadeliOdemeTipi.SelectedIndex == 1) //eşit taksitli
                        {
                            decimal dTaksitTutari = C.ParseDecimal((C.ParseDecimal(txtVadeli.EditValue) / iTaksitSayisi).ToString("n2"));

                            if (i == iTaksitSayisi - 1) //son taksitse
                            {
                                decimal dToplameklenen = dTaksitTutari * MListler.dsAktivite_OdemePlaniSenet.Tables[0].Rows.Count;
                                TaksitEkle(VadeGridMaxTarihBul(), C.ParseDecimal(txtVadeli.EditValue) - dToplameklenen);
                            }
                            else
                                TaksitEkle(VadeGridMaxTarihBul(), dTaksitTutari);
                        }
                        else //sabit tutar
                        {
                            if (i == iTaksitSayisi - 1) //son taksitse
                            {
                                decimal dToplameklenen = C.ParseDecimal(txtSabitTutar.EditValue) * MListler.dsAktivite_OdemePlaniSenet.Tables[0].Rows.Count;
                                TaksitEkle(VadeGridMaxTarihBul(), C.ParseDecimal(txtVadeli.EditValue) - dToplameklenen);
                                continue;
                            }
                            TaksitEkle(VadeGridMaxTarihBul(), C.ParseDecimal(txtSabitTutar.EditValue));
                        }
                    }
                }

                grdVadeliOdemeBilgileri.DataSource = MListler.dsAktivite_OdemePlaniSenet.Tables[0];


                GayrimenkulFiyatlariHesapla();
                NPV_Hesapla();
                OrtalamaVade_Hesapla();
                TaksitToplamiVeFarkiHesapla();

                if (bKapatilacakMi)
                    btnVadeliOdemeSihirbazi.Checked = false;

            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bbiUygulaVeKapat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            UygulaVeKapat(true);
        }

        private void ddbtnTaksitUygulaVeKapat_Click(object sender, EventArgs e)
        {
            UygulaVeKapat(false);

        }


        private void btnTaksitEkle_Click(object sender, EventArgs e)
        {
        }


        private void gvVadeliOdemeBilgileri_RowCountChanged(object sender, EventArgs e)
        {

            try
            {
                if (MListler.dsAktivite_OdemePlaniSenet.Tables[0].Rows.Count > 0)
                {
                    TaksitToplamiVeFarkiHesapla();
                }

            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtVadeliFaiz_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                F_AktiviteDigerDatalar frm = (F_AktiviteDigerDatalar)Application.OpenForms["F_AktiviteDigerDatalar"];

                if (frm != null)
                {
                    MListler.dsAktivite.Tables[0].Rows[0]["AylikBrutFaiz"] = C.ParseDecimal(txtVadeliFaiz.EditValue);
                }

                NPV_Hesapla();
                GayrimenkulFiyatlariHesapla();
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void OrtalamaVade_Hesapla()
        {
            try
            {
                if (C.ParseInt(cmbOdemeBicimiAna.EditValue) == 313 || C.ParseInt(cmbOdemeBicimiAna.EditValue) == 314) //vade varsa..
                {
                    if (gvVadeliOdemeBilgileri.DataRowCount < 1)
                        return;


                    //peşinat tarihleri tabloya atılıyor...
                    DataTable dtVadeler = new DataTable();
                    dtVadeler.Columns.Add("Tarih", typeof(System.DateTime));
                    dtVadeler.Columns.Add("Tutar", typeof(System.Decimal));
                    dtVadeler.Columns.Add("Carpim", typeof(System.Decimal));

                    for (int i = 1; i <= 10; i++)
                    {
                        Control[] ctrls = this.Controls.Find("dtmPesinat" + i.ToString(), true);
                        Control[] ctrls2 = this.Controls.Find("txtPesinat" + i.ToString(), true);

                        if (ctrls.Length > 0 && ctrls2.Length > 0 && ((DateEdit)ctrls[0]).DateTime.Year > 1)
                        {
                            DateEdit dtedt = ((DateEdit)ctrls[0]);
                            ButtonEdit btnedt = ((ButtonEdit)ctrls2[0]);


                            DataRow drw = dtVadeler.NewRow();
                            drw["Tarih"] = ((DateEdit)ctrls[0]).DateTime;
                            drw["Tutar"] = C.ParseDecimal(((ButtonEdit)ctrls2[0]).EditValue);
                            dtVadeler.Rows.Add(drw);
                        }
                    }

                    //vadeli ödeme tarihleri tabloya atılıyor...
                    for (int k = 0; k < MListler.dsAktivite_OdemePlaniSenet.Tables[0].Rows.Count; k++)
                    {
                        DateTime dtmVadeTarih = C.ParseDateTime(MListler.dsAktivite_OdemePlaniSenet.Tables[0].Rows[k]["Tarih"]);
                        Decimal dVadeBorcTutari = C.ParseDecimal(MListler.dsAktivite_OdemePlaniSenet.Tables[0].Rows[k]["BorcTutari"]);



                        DataRow drw = dtVadeler.NewRow();
                        drw["Tarih"] = dtmVadeTarih.Date;
                        drw["Tutar"] = dVadeBorcTutari;
                        dtVadeler.Rows.Add(drw);
                    }


                    DateTime dtTarih = dtmTarih.DateTime.Date;
                    foreach (DataRow row in dtVadeler.Rows)
                    {
                        TimeSpan tsDiff = dtTarih.Subtract(C.ParseDateTime(row["Tarih"]));
                        int iGunFarki = Math.Abs(tsDiff.Days);
                        if (iGunFarki == 0)
                            iGunFarki = 1;
                        row["Carpim"] = C.ParseDecimal(row["Tutar"]) * iGunFarki;
                    }


                    decimal dCarpimToplam = 0;
                    decimal dTutarToplam = 0;
                    foreach (DataRow row in dtVadeler.Rows)
                    {
                        dCarpimToplam += C.ParseDecimal(row["Carpim"]);
                        dTutarToplam += C.ParseDecimal(row["Tutar"]);
                    }

                    int iGunSonuc = C.ParseInt(dCarpimToplam / dTutarToplam);

                    if (iGunSonuc > 0)
                        lblMD_OrtalamaVadeDeger.Text = dtmTarih.DateTime.AddDays(iGunSonuc).ToShortDateString();
                    else
                        lblMD_OrtalamaVadeDeger.Text = "-";

                }
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void NPV_Hesapla()
        {
            //NPV(Net Present Value)(Net Bugünki Değer)...Süreleri eşit aralıkta olan dönemlerde gerçekleşen çeşitli nakit akışlarının belirli bir iskonto oranı üzerinden bugünkü değerinin bulunmasını sağlar.
            try
            {
                if (C.ParseInt(cmbOdemeBicimiAna.EditValue) == 313 || C.ParseInt(cmbOdemeBicimiAna.EditValue) == 314) //vade varsa..
                {
                    //peşinat tarihleri tabloya atılıyor...
                    DataTable dtNPV = new DataTable();
                    dtNPV.Columns.Add("Tarih", typeof(System.DateTime));
                    dtNPV.Columns.Add("Tutar", typeof(System.Decimal));

                    //Ödemelerde kapora varsa dizinin ilk kaydına aktivite tarihi ve kapora tutarı ile ekleniyor.Eğer kapora yoksa,aktivite tarihi ile 0 tutarında ilk row ekleniyor.
                    DataRow drwIlk = dtNPV.NewRow();

                    var listOdemeler = Genel.PrmDb.S_AktiviteBakiyeGetir(iAktiviteID).FirstOrDefault();
                    decimal dKapora = listOdemeler == null ? 0 : C.ParseDecimal(listOdemeler.DepozitoOdenen); //kapora tutarı                    
                    if (dKapora <= 0) //kapora yoksa...
                    {
                        drwIlk["Tarih"] = dtmTarih.DateTime;
                        drwIlk["Tutar"] = 0;
                        dtNPV.Rows.Add(drwIlk);
                    }

                    #region Peşinatlar ekleniyor...
                    for (int i = 1; i <= 10; i++)
                    {
                        Control[] ctrls = this.Controls.Find("dtmPesinat" + i.ToString(), true);
                        Control[] ctrls2 = this.Controls.Find("txtPesinat" + i.ToString(), true);

                        if (ctrls.Length > 0 && ctrls2.Length > 0 && ((DateEdit)ctrls[0]).DateTime.Year > 1)
                        {
                            DateEdit dtedt = ((DateEdit)ctrls[0]);
                            ButtonEdit btnedt = ((ButtonEdit)ctrls2[0]);

                            DataRow[] drws = dtNPV.Select(String.Format("convert(Tarih, 'System.String') LIKE '%{0}%'", dtedt.DateTime.Month.ToString("00") + "." + dtedt.DateTime.Year.ToString()));

                            if (drws.Length > 0)
                            {
                                drws[0]["Tutar"] = C.ParseDecimal(drws[0]["Tutar"]) + C.ParseDecimal(btnedt.EditValue);
                                continue;
                            }

                            DataRow drw = dtNPV.NewRow();
                            drw["Tarih"] = ((DateEdit)ctrls[0]).DateTime;
                            drw["Tutar"] = C.ParseDecimal(((ButtonEdit)ctrls2[0]).EditValue);
                            dtNPV.Rows.Add(drw);
                        }
                    }
                    #endregion


                    #region Banka kredisi ekleniyor...
                    DataRow[] drwBanka = MListler.dsAktivite_OdemePlaniPesinatBanka.Tables[0].Select("TipKisID_304 = 382");
                    if (drwBanka.Length > 0)
                    {
                        DateTime dtmVadeTarih = C.ParseDateTime(drwBanka[0]["Tarih"]);
                        Decimal dVadeBorcTutari = C.ParseDecimal(drwBanka[0]["BorcTutari"]);

                        DataRow[] drws = dtNPV.Select(String.Format("convert(Tarih, 'System.String') LIKE '%{0}%'", dtmVadeTarih.Month.ToString("00") + "." + dtmVadeTarih.Year.ToString()));
                        if (drws.Length > 0)
                        {
                            drws[0]["Tutar"] = C.ParseDecimal(drws[0]["Tutar"]) + dVadeBorcTutari;
                        }
                        else
                        {
                            DataRow drw = dtNPV.NewRow();
                            drw["Tarih"] = dtmVadeTarih.Date;
                            drw["Tutar"] = dVadeBorcTutari;
                            dtNPV.Rows.Add(drw);
                        }
                    }
                    #endregion

                    #region vadeli ödeme tarihleri tabloya atılıyor...
                    for (int k = 0; k < MListler.dsAktivite_OdemePlaniSenet.Tables[0].Rows.Count; k++)
                    {
                        DateTime dtmVadeTarih = C.ParseDateTime(MListler.dsAktivite_OdemePlaniSenet.Tables[0].Rows[k]["Tarih"]);
                        Decimal dVadeBorcTutari = C.ParseDecimal(MListler.dsAktivite_OdemePlaniSenet.Tables[0].Rows[k]["BorcTutari"]);


                        //temp tabloda aynı ayda kayıt varsa tutar o kayda ekleniyor..
                        DataRow[] drws = dtNPV.Select(String.Format("convert(Tarih, 'System.String') LIKE '%{0}%'", dtmVadeTarih.Month.ToString("00") + "." + dtmVadeTarih.Year.ToString()));

                        if (drws.Length > 0)
                        {
                            drws[0]["Tutar"] = C.ParseDecimal(drws[0]["Tutar"]) + dVadeBorcTutari;
                            continue;
                        }

                        DataRow drw = dtNPV.NewRow();
                        drw["Tarih"] = dtmVadeTarih.Date;
                        drw["Tutar"] = dVadeBorcTutari;
                        dtNPV.Rows.Add(drw);
                    }
                    #endregion

                    DateTime dtmMin = MinTarihBul(dtNPV);
                    DateTime dtmMax = MaxTarihBul(dtNPV);

                    for (DateTime date = dtmMin; date < dtmMax; date = date.AddMonths(1))
                    {
                        DataRow[] drws = dtNPV.Select(String.Format("convert(Tarih, 'System.String') LIKE '%{0}%'", date.Month.ToString("00") + "." + date.Year.ToString()));
                        if (drws.Length == 0)
                        {
                            DataRow drw = dtNPV.NewRow();
                            drw["Tarih"] = C.ParseDateTime("01." + date.Month.ToString("00") + "." + date.Year.ToString());
                            drw["Tutar"] = 0;
                            dtNPV.Rows.Add(drw);
                        }
                    }

                    //tarihe göre sıralanıyor
                    dtNPV.DefaultView.Sort = "Tarih ASC";
                    dtNPV = dtNPV.DefaultView.ToTable();

                    double dblVadeFaizi = double.Parse(txtVadeliFaiz.EditValue.ToString()) / 100;
                    double[] ddizi = new double[dtNPV.Rows.Count];
                    for (int t = 0; t < dtNPV.Rows.Count; t++)
                    {
                        ddizi[t] = double.Parse(dtNPV.Rows[t]["Tutar"].ToString());
                    }



                    double result = 0;
                    if (ddizi.Count() > 0)
                        result = Microsoft.VisualBasic.Financial.NPV(dblVadeFaizi, ref ddizi);


                    lblMD_NPVDeger.Text = result.ToString("n2");
                }


            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private DateTime MinTarihBul(DataTable dt)
        {
            try
            {
                DateTime minDate = new DateTime();
                foreach (DataRow row in dt.Rows)
                {
                    DateTime date = C.ParseDateTime(row["Tarih"]);

                    if (date.Year < 2000)
                        continue;
                    if (date < minDate || minDate.Year < 2)
                        minDate = date;
                }
                return minDate;
            }
            catch (Exception Hata)
            {
                Genel.LogErrorYaz(iFormID, Hata);
                return new DateTime(1900, 1, 1);
            }
        }
        private DateTime MaxTarihBul(DataTable dt)
        {
            try
            {
                DateTime maxDate = new DateTime();
                foreach (DataRow row in dt.Rows)
                {
                    DateTime date = C.ParseDateTime(row["Tarih"]);
                    if (date.Year < 2000)
                        continue;
                    if (date > maxDate)
                        maxDate = date;
                }
                return maxDate;
            }
            catch (Exception Hata)
            {
                Genel.LogErrorYaz(iFormID, Hata);
                return new DateTime(1900, 1, 1);
            }
        }


        private bool Validation()
        {
            try
            {

                bool b = true;

                if (C.ParseDecimal(txtFarkOdeme.EditValue) != 0 &&
                    (cmbOdemeBicimiAna.EditValue != null || C.ParseInt(cmbOdemeBicimiAna.EditValue) > 0)) //ödeme biçimi seçilmiş ve fark varsa..
                {
                    b = false;
                    dxErrorProvider1.SetError(txtFarkOdeme, " ");
                }
                else
                    dxErrorProvider1.SetError(txtFarkOdeme, String.Empty);


                if ((cmbDoviz.EditValue == null || C.ParseInt(cmbDoviz.EditValue) <= 0) && C.ParseInt(cmbAktiviteDurumu.EditValue) == 303) //satıldı
                {
                    dxErrorProvider1.SetError(cmbDoviz, " ");
                    return false;
                }
                else
                    dxErrorProvider1.SetError(cmbDoviz, String.Empty);



                if ((cmbOdemeBicimiAna.EditValue == null || C.ParseInt(cmbOdemeBicimiAna.EditValue) <= 0) && C.ParseInt(cmbAktiviteDurumu.EditValue) == 303) //satıldı
                {
                    dxErrorProvider1.SetError(cmbOdemeBicimiAna, " ");
                    return false;
                }
                else
                    dxErrorProvider1.SetError(cmbOdemeBicimiAna, String.Empty);



                if (C.ParseInt(cmbOdemeBicimiAna.EditValue) == 313 || C.ParseInt(cmbOdemeBicimiAna.EditValue) == 314) //vade varsa..
                {
                    if (C.ParseDecimal(txtFarkAlt.EditValue) != 0)
                    {
                        dxErrorProvider1.SetError(txtFarkAlt, " ");
                        return false;
                    }
                    else
                        dxErrorProvider1.SetError(txtFarkAlt, String.Empty);



                    if (C.ParseDecimal(txtVadeli.EditValue) <= 0)
                    {
                        dxErrorProvider1.SetError(txtVadeli, " ");
                        return false;
                    }
                    else
                        dxErrorProvider1.SetError(txtVadeli, String.Empty);
                }



                if (C.ParseInt(cmbAktiviteDurumu.EditValue) == 303) //satıldı
                {
                    for (int i = 2; i <= 10; i++)
                    {
                        Control[] ctrlPesinat = this.Controls.Find("txtPesinat" + i.ToString(), true);
                        Control[] ctrlPesinatDate = this.Controls.Find("dtmPesinat" + i.ToString(), true);

                        ButtonEdit btnedt = (ButtonEdit)ctrlPesinat[0];
                        DateEdit dtedt = (DateEdit)ctrlPesinatDate[0];

                        if (!btnedt.Visible)
                            continue;

                        if (C.ParseDecimal(btnedt.EditValue) > 0 && dtedt.DateTime.Year < 2000)
                        {
                            dxErrorProvider1.SetError(dtedt, " ");
                            b = false;
                        }
                        else
                            dxErrorProvider1.SetError(dtedt, String.Empty);


                        if (C.ParseDecimal(btnedt.EditValue) == 0 && dtedt.DateTime.Year != 1)
                        {
                            dxErrorProvider1.SetError(btnedt, " ");
                            b = false;
                        }
                        else
                            dxErrorProvider1.SetError(btnedt, String.Empty);
                    }




                    if (Ayar.AyarGetirBool(12) == false && C.ParseInt(cmbOdemeBicimiAna.EditValue) != 311) //Aktivitede PEŞİNAT sıfır geçilemez ise ve ödeme tipi peşin dısında birşey ise..
                    {
                        //txtPesinat1
                        if (C.ParseDecimal(txtPesinat1.EditValue) == 0)
                        {
                            dxErrorProvider1.SetError(txtPesinat1, " ");
                            b = false;
                        }
                        else
                            dxErrorProvider1.SetError(txtPesinat1, String.Empty);
                    }
                }


                //dtmPesinat1
                if (C.ParseInt(cmbOdemeBicimiAna.EditValue) > 0 && C.ParseDecimal(txtPesinat1.EditValue) > 0 && dtmPesinat1.DateTime.Year < 2000)
                {
                    dxErrorProvider1.SetError(dtmPesinat1, " ");
                    b = false;
                }
                else
                    dxErrorProvider1.SetError(dtmPesinat1, String.Empty);






                if (C.ParseInt(cmbOdemeBicimiAna.EditValue) == 312 || C.ParseInt(cmbOdemeBicimiAna.EditValue) == 314) //banka kredisi varsa..
                {
                    if (C.ParseDecimal(txtBankaKredisi.EditValue) > 0 && dtmBankaKredisi.DateTime.Year < 2000)
                    {
                        dxErrorProvider1.SetError(dtmBankaKredisi, " ");
                        b = false;
                    }
                    else
                        dxErrorProvider1.SetError(dtmBankaKredisi, String.Empty);


                    if (C.ParseDecimal(txtBankaKredisi.EditValue) <= 0)
                    {
                        dxErrorProvider1.SetError(txtBankaKredisi, " ");
                        b = false;
                    }
                    else
                        dxErrorProvider1.SetError(txtBankaKredisi, String.Empty);
                }
                return b;
            }
            catch (Exception Hata)
            {
                Genel.LogErrorYaz(iFormID, Hata);
                return false;
            }
        }

        private void dtmPesinat1_EditValueChanged(object sender, EventArgs e)
        {
            OrtalamaVade_Hesapla();
            Validation();
        }

        private void txtFarkAlt_EditValueChanged(object sender, EventArgs e)
        {
            Validation();
        }

        private void dtmTarih_EditValueChanged(object sender, EventArgs e)
        {
            OrtalamaVade_Hesapla();


            if (dtmTarih.DateTime.Date != DateTime.Now.Date)
                pnlRezerveSuresi.Enabled = false;
            else
                pnlRezerveSuresi.Enabled = true;
        }

        private void F_Aktivite_Resize(object sender, EventArgs e)
        {
            sccGridler.SplitterPosition = this.Width / 2;
        }


        private void cmbKampanya_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {

                if (e.Button.Index == 1) //kampanya formu
                {
                    var Modal = Ayar.FormGoster();
                    F_AktiviteKampanya frm = new F_AktiviteKampanya();
                    frm.ShowDialog();

                    if (frm._Saved)//kampanya formunda kayıt yapıldıysa...
                        cmbOdemeBicimi_EditValueChanged(null, null);


                    Modal.Close();
                }
                else if (e.Button.Index == 2)//temizle
                {
                    cmbKampanya.EditValue = null;
                }
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtGrdOdemeTutari_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void TaksitToplamiVeFarkiHesapla()
        {
            try
            {

                decimal dToplam = 0;

                MListler.dsAktivite_OdemePlaniSenet.Tables[0].AcceptChanges();

                foreach (DataRow drw in MListler.dsAktivite_OdemePlaniSenet.Tables[0].Rows)
                {
                    decimal dOdeme = C.ParseDecimal(drw["BorcTutari"]);
                    dToplam += dOdeme;
                }



                txtToplamTaksit.EditValue = dToplam.ToString("n2");
                txtFarkAlt.EditValue = Convert.ToDecimal((C.ParseDecimal(txtVadeli.EditValue) - dToplam).ToString("n2"));
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtGrdOdemeTutari_EditValueChanging(object sender, ChangingEventArgs e)
        {

            try
            {

                decimal dToplam = 0;

                for (int i = 0; i < gvVadeliOdemeBilgileri.RowCount; i++)
                {
                    if (i == gvVadeliOdemeBilgileri.FocusedRowHandle)
                        continue;
                    decimal dOdeme = C.ParseDecimal(gvVadeliOdemeBilgileri.GetRowCellValue(i, "BorcTutari"));
                    dToplam += dOdeme;
                }
                dToplam += C.ParseDecimal(e.NewValue);

                txtToplamTaksit.EditValue = dToplam.ToString();
                txtFarkAlt.EditValue = (C.ParseDecimal(txtVadeli.EditValue) - dToplam).ToString("n2");
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void btnVadeliOdemeSihirbazi_CheckedChanged(object sender, EventArgs e)
        {
            if (btnVadeliOdemeSihirbazi.Checked)
            {
                tcOdeme.Height = 111;
                tcOdeme.SelectedTabPageIndex = 1;
            }
            else
            {
                tcOdeme.Height = 182;
                tcOdeme.SelectedTabPageIndex = 0;
            }

        }



        private void btnGeri_Click(object sender, EventArgs e)
        {
            btnVadeliOdemeSihirbazi.Checked = false;
        }

        private void txtVadeli_EditValueChanging(object sender, ChangingEventArgs e)
        {
            try
            {
                txtVadeli.Tag = "0";

                if (C.ParseInt(cmbOdemeBicimiAna.EditValue) == 313 || C.ParseInt(cmbOdemeBicimiAna.EditValue) == 314) //vade varsa..
                {
                    decimal dDeger = C.ParseDecimal(e.NewValue);

                    if (dDeger <= 0)
                        dxErrorProvider1.SetError(txtVadeli, " ");
                    else
                        dxErrorProvider1.SetError(txtVadeli, String.Empty);
                }
                else
                    dxErrorProvider1.SetError(txtVadeli, String.Empty);
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnKrediHesapla_Click(object sender, EventArgs e)
        {
            var Modal = Ayar.FormGoster();
            F_AktiviteBankaKredisiHesaplama frm = new F_AktiviteBankaKredisiHesaplama();
            frm.ShowDialog();
            Modal.Close();
        }

        private void cmbKampanya_EditValueChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (iAktiviteID > 0)
            {
                this.Cursor = Cursors.Default;
                return;
            }

            if (C.ParseInt(cmbKampanya.EditValue) <= 0)
            {
                KampanyaTemizle();
                this.Cursor = Cursors.Default;
                return;
            }
            KampanyaUygula();
            this.Cursor = Cursors.Default;
        }


        private void KampanyaTemizle()
        {
            try
            {
                if (!bIsFormLoaded)
                    return;

                MListler.dsAktivite_OdemePlaniSenet.Tables[0].Rows.Clear();
                txtSatisFiyati.EditValue = txtListeFiyati.EditValue;

                decimal dLF = C.ParseDecimal(txtListeFiyati.EditValue);
                decimal dITD = C.ParseDecimal(txtHesaplananIndirim.EditValue) + C.ParseDecimal(txtTanimlananIndirim.EditValue);
                decimal dVFTD = C.ParseDecimal(txtHesaplananVadeFarki.EditValue) + C.ParseDecimal(txtTanimlananVadeFarki.EditValue);
                if (iAktiviteID < 1)
                    txtSatisFiyati.EditValue = dLF - dITD + dVFTD;


                txtPesinat1.EditValue = 0;
                txtPesinat2.EditValue = 0;
                txtPesinat3.EditValue = 0;
                txtPesinat4.EditValue = 0;
                txtPesinat5.EditValue = 0;
                txtPesinat6.EditValue = 0;
                txtPesinat7.EditValue = 0;
                txtPesinat8.EditValue = 0;
                txtPesinat9.EditValue = 0;
                txtPesinat10.EditValue = 0;
                txtBankaKredisi.EditValue = 0;

                dtmPesinat1.EditValue = null;
                dtmPesinat2.EditValue = null;
                dtmPesinat3.EditValue = null;
                dtmPesinat4.EditValue = null;
                dtmPesinat5.EditValue = null;
                dtmPesinat6.EditValue = null;
                dtmPesinat7.EditValue = null;
                dtmPesinat8.EditValue = null;
                dtmPesinat9.EditValue = null;
                dtmPesinat10.EditValue = null;
                dtmBankaKredisi.EditValue = null;

                txtVadeli.EditValue = 0;
                txtHesaplananVadeFarki.EditValue = 0;
                btnVadeliOdemeSihirbazi.Checked = false;
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void KampanyaUygula()
        {
            try
            {
                KampanyaTemizle();

                int iKampanyaID = C.ParseInt(cmbKampanya.EditValue);
                if (iKampanyaID <= 0) return;

                var Kampanya = MListler.MList_S_AktiviteKampanya.Where(u => u.ID == iKampanyaID).FirstOrDefault();
                decimal dSatisFiyati = C.ParseDecimal(txtListeFiyati.EditValue);

                #region ARA ÖDEMELER TABINDAKİ HESAPLAMALAR....
                int iTaksitGunu = 1;
                if (Kampanya.TaksitBaslangicGunuSabitMi)
                    iTaksitGunu = Kampanya.TaksitBaslangicGunu;

                #region AraÖdeme Tarihleri Hesaplanıyor...
                #region 1.Ödeme Tarihi hesaplanıyor...
                bool bAySabitMi1 = Kampanya.AraOdemedeAySabitMi01;
                DateTime dtOdemeTarihi1 = new DateTime();
                if (bAySabitMi1)
                    dtOdemeTarihi1 = C.ParseDateTime(Kampanya.AraOdemeYil01 + "." + Kampanya.AraOdemeAy01 + "." + iTaksitGunu.ToString("00"));
                else
                    dtOdemeTarihi1 = C.ParseDateTime(dtmTarih.DateTime.Date.AddMonths(Kampanya.AraOdemeSonrakiAy01));  //satış tarihine bakılır.
                #endregion
                #region 2.Ödeme Tarihi hesaplanıyor...
                bool bAySabitMi2 = Kampanya.AraOdemedeAySabitMi02;
                DateTime dtOdemeTarihi2 = new DateTime();
                if (bAySabitMi2)
                    dtOdemeTarihi2 = C.ParseDateTime(Kampanya.AraOdemeYil02 + "." + Kampanya.AraOdemeAy02 + "." + iTaksitGunu.ToString("00"));
                else
                    dtOdemeTarihi2 = C.ParseDateTime(dtmTarih.DateTime.Date.AddMonths(Kampanya.AraOdemeSonrakiAy02));  //satış tarihine bakılır.
                #endregion
                #region 3.Ödeme Tarihi hesaplanıyor...
                bool bAySabitMi3 = Kampanya.AraOdemedeAySabitMi03;
                DateTime dtOdemeTarihi3 = new DateTime();
                if (bAySabitMi3)
                    dtOdemeTarihi3 = C.ParseDateTime(Kampanya.AraOdemeYil03 + "." + Kampanya.AraOdemeAy03 + "." + iTaksitGunu.ToString("00"));
                else
                    dtOdemeTarihi3 = C.ParseDateTime(dtmTarih.DateTime.Date.AddMonths(Kampanya.AraOdemeSonrakiAy03));  //satış tarihine bakılır.
                #endregion
                #region 4.Ödeme Tarihi hesaplanıyor...
                bool bAySabitMi4 = Kampanya.AraOdemedeAySabitMi04;
                DateTime dtOdemeTarihi4 = new DateTime();
                if (bAySabitMi4)
                    dtOdemeTarihi4 = C.ParseDateTime(Kampanya.AraOdemeYil04 + "." + Kampanya.AraOdemeAy04 + "." + iTaksitGunu.ToString("00"));
                else
                    dtOdemeTarihi4 = C.ParseDateTime(dtmTarih.DateTime.Date.AddMonths(Kampanya.AraOdemeSonrakiAy04));  //satış tarihine bakılır.
                #endregion
                #region 5.Ödeme Tarihi hesaplanıyor...
                bool bAySabitMi5 = Kampanya.AraOdemedeAySabitMi05;
                DateTime dtOdemeTarihi5 = new DateTime();
                if (bAySabitMi5)
                    dtOdemeTarihi5 = C.ParseDateTime(Kampanya.AraOdemeYil05 + "." + Kampanya.AraOdemeAy05 + "." + iTaksitGunu.ToString("00"));
                else
                    dtOdemeTarihi5 = C.ParseDateTime(dtmTarih.DateTime.Date.AddMonths(Kampanya.AraOdemeSonrakiAy05));  //satış tarihine bakılır.
                #endregion
                #region 6.Ödeme Tarihi hesaplanıyor...
                bool bAySabitMi6 = Kampanya.AraOdemedeAySabitMi06;
                DateTime dtOdemeTarihi6 = new DateTime();
                if (bAySabitMi6)
                    dtOdemeTarihi6 = C.ParseDateTime(Kampanya.AraOdemeYil06 + "." + Kampanya.AraOdemeAy06 + "." + iTaksitGunu.ToString("00"));
                else
                    dtOdemeTarihi6 = C.ParseDateTime(dtmTarih.DateTime.Date.AddMonths(Kampanya.AraOdemeSonrakiAy06));  //satış tarihine bakılır.
                #endregion
                #region 7.Ödeme Tarihi hesaplanıyor...
                bool bAySabitMi7 = Kampanya.AraOdemedeAySabitMi07;
                DateTime dtOdemeTarihi7 = new DateTime();
                if (bAySabitMi7)
                    dtOdemeTarihi7 = C.ParseDateTime(Kampanya.AraOdemeYil07 + "." + Kampanya.AraOdemeAy07 + "." + iTaksitGunu.ToString("00"));
                else
                    dtOdemeTarihi7 = C.ParseDateTime(dtmTarih.DateTime.Date.AddMonths(Kampanya.AraOdemeSonrakiAy07));  //satış tarihine bakılır.
                #endregion
                #region 8.Ödeme Tarihi hesaplanıyor...
                bool bAySabitMi8 = Kampanya.AraOdemedeAySabitMi08;
                DateTime dtOdemeTarihi8 = new DateTime();
                if (bAySabitMi8)
                    dtOdemeTarihi8 = C.ParseDateTime(Kampanya.AraOdemeYil08 + "." + Kampanya.AraOdemeAy08 + "." + iTaksitGunu.ToString("00"));
                else
                    dtOdemeTarihi8 = C.ParseDateTime(dtmTarih.DateTime.Date.AddMonths(Kampanya.AraOdemeSonrakiAy08));  //satış tarihine bakılır.
                #endregion
                #region 9.Ödeme Tarihi hesaplanıyor...
                bool bAySabitMi9 = Kampanya.AraOdemedeAySabitMi09;
                DateTime dtOdemeTarihi9 = new DateTime();
                if (bAySabitMi9)
                    dtOdemeTarihi9 = C.ParseDateTime(Kampanya.AraOdemeYil09 + "." + Kampanya.AraOdemeAy09 + "." + iTaksitGunu.ToString("00"));
                else
                    dtOdemeTarihi9 = C.ParseDateTime(dtmTarih.DateTime.Date.AddMonths(Kampanya.AraOdemeSonrakiAy09));  //satış tarihine bakılır.
                #endregion
                #region 10.Ödeme Tarihi hesaplanıyor...
                bool bAySabitMi10 = Kampanya.AraOdemedeAySabitMi10;
                DateTime dtOdemeTarihi10 = new DateTime();
                if (bAySabitMi10)
                    dtOdemeTarihi10 = C.ParseDateTime(Kampanya.AraOdemeYil10 + "." + Kampanya.AraOdemeAy10 + "." + iTaksitGunu.ToString("00"));
                else
                    dtOdemeTarihi10 = C.ParseDateTime(dtmTarih.DateTime.Date.AddMonths(Kampanya.AraOdemeSonrakiAy10));  //satış tarihine bakılır.
                #endregion
                #endregion
                #region Ara Ödeme DataTable'a oluşturuluyor...
                DataTable dtAraOdemeler = new DataTable();
                dtAraOdemeler.Columns.Add("OdemeTarihi", typeof(System.DateTime));
                dtAraOdemeler.Columns.Add("Tutar", typeof(System.Decimal));
                dtAraOdemeler.Columns.Add("TufeliMi", typeof(System.Boolean));
                decimal dAraOdemeOrani = Kampanya.AraOdemeOrani01;
                decimal dAraOdemeTutari = 0;
                if (Kampanya.AraOdemeVarMi01 && dAraOdemeOrani > 0)
                {
                    dAraOdemeTutari = dSatisFiyati * (dAraOdemeOrani / 100);
                    DataRow drw = dtAraOdemeler.NewRow();
                    drw["OdemeTarihi"] = dtOdemeTarihi1;
                    drw["Tutar"] = dAraOdemeTutari;
                    drw["TufeliMi"] = Kampanya.AraOdemeTufeliMi01;
                    dtAraOdemeler.Rows.Add(drw);
                }
                dAraOdemeOrani = Kampanya.AraOdemeOrani02;
                if (Kampanya.AraOdemeVarMi02 && dAraOdemeOrani > 0)
                {
                    dAraOdemeTutari = dSatisFiyati * (dAraOdemeOrani / 100);
                    DataRow drw = dtAraOdemeler.NewRow();
                    drw["OdemeTarihi"] = dtOdemeTarihi2;
                    drw["Tutar"] = dAraOdemeTutari;
                    drw["TufeliMi"] = Kampanya.AraOdemeTufeliMi02;
                    dtAraOdemeler.Rows.Add(drw);
                }
                dAraOdemeOrani = Kampanya.AraOdemeOrani03;
                if (Kampanya.AraOdemeVarMi03 && dAraOdemeOrani > 0)
                {
                    dAraOdemeTutari = dSatisFiyati * (dAraOdemeOrani / 100);
                    DataRow drw = dtAraOdemeler.NewRow();
                    drw["OdemeTarihi"] = dtOdemeTarihi3;
                    drw["Tutar"] = dAraOdemeTutari;
                    drw["TufeliMi"] = Kampanya.AraOdemeTufeliMi03;
                    dtAraOdemeler.Rows.Add(drw);
                }
                dAraOdemeOrani = Kampanya.AraOdemeOrani04;
                if (Kampanya.AraOdemeVarMi04 && dAraOdemeOrani > 0)
                {
                    dAraOdemeTutari = dSatisFiyati * (dAraOdemeOrani / 100);
                    DataRow drw = dtAraOdemeler.NewRow();
                    drw["OdemeTarihi"] = dtOdemeTarihi4;
                    drw["Tutar"] = dAraOdemeTutari;
                    drw["TufeliMi"] = Kampanya.AraOdemeTufeliMi04;
                    dtAraOdemeler.Rows.Add(drw);
                }
                dAraOdemeOrani = Kampanya.AraOdemeOrani05;
                if (Kampanya.AraOdemeVarMi05 && dAraOdemeOrani > 0)
                {
                    dAraOdemeTutari = dSatisFiyati * (dAraOdemeOrani / 100);
                    DataRow drw = dtAraOdemeler.NewRow();
                    drw["OdemeTarihi"] = dtOdemeTarihi5;
                    drw["Tutar"] = dAraOdemeTutari;
                    drw["TufeliMi"] = Kampanya.AraOdemeTufeliMi05;
                    dtAraOdemeler.Rows.Add(drw);
                }
                dAraOdemeOrani = Kampanya.AraOdemeOrani06;
                if (Kampanya.AraOdemeVarMi06 && dAraOdemeOrani > 0)
                {
                    dAraOdemeTutari = dSatisFiyati * (dAraOdemeOrani / 100);
                    DataRow drw = dtAraOdemeler.NewRow();
                    drw["OdemeTarihi"] = dtOdemeTarihi6;
                    drw["Tutar"] = dAraOdemeTutari;
                    drw["TufeliMi"] = Kampanya.AraOdemeTufeliMi06;
                    dtAraOdemeler.Rows.Add(drw);
                }
                dAraOdemeOrani = Kampanya.AraOdemeOrani07;
                if (Kampanya.AraOdemeVarMi07 && dAraOdemeOrani > 0)
                {
                    dAraOdemeTutari = dSatisFiyati * (dAraOdemeOrani / 100);
                    DataRow drw = dtAraOdemeler.NewRow();
                    drw["OdemeTarihi"] = dtOdemeTarihi7;
                    drw["Tutar"] = dAraOdemeTutari;
                    drw["TufeliMi"] = Kampanya.AraOdemeTufeliMi07;
                    dtAraOdemeler.Rows.Add(drw);
                }
                dAraOdemeOrani = Kampanya.AraOdemeOrani08;
                if (Kampanya.AraOdemeVarMi08 && dAraOdemeOrani > 0)
                {
                    dAraOdemeTutari = dSatisFiyati * (dAraOdemeOrani / 100);
                    DataRow drw = dtAraOdemeler.NewRow();
                    drw["OdemeTarihi"] = dtOdemeTarihi8;
                    drw["Tutar"] = dAraOdemeTutari;
                    drw["TufeliMi"] = Kampanya.AraOdemeTufeliMi08;
                    dtAraOdemeler.Rows.Add(drw);
                }
                dAraOdemeOrani = Kampanya.AraOdemeOrani09;
                if (Kampanya.AraOdemeVarMi09 && dAraOdemeOrani > 0)
                {
                    dAraOdemeTutari = dSatisFiyati * (dAraOdemeOrani / 100);
                    DataRow drw = dtAraOdemeler.NewRow();
                    drw["OdemeTarihi"] = dtOdemeTarihi9;
                    drw["Tutar"] = dAraOdemeTutari;
                    drw["TufeliMi"] = Kampanya.AraOdemeTufeliMi09;
                    dtAraOdemeler.Rows.Add(drw);
                }
                dAraOdemeOrani = Kampanya.AraOdemeOrani10;
                if (Kampanya.AraOdemeVarMi10 && dAraOdemeOrani > 0)
                {
                    dAraOdemeTutari = dSatisFiyati * (dAraOdemeOrani / 100);
                    DataRow drw = dtAraOdemeler.NewRow();
                    drw["OdemeTarihi"] = dtOdemeTarihi10;
                    drw["Tutar"] = dAraOdemeTutari;
                    drw["TufeliMi"] = Kampanya.AraOdemeTufeliMi10;
                    dtAraOdemeler.Rows.Add(drw);
                }
                dAraOdemeOrani = Kampanya.TeslimdeOdemeOrani;
                if (Kampanya.TeslimdeOdemeVarMi && dAraOdemeOrani > 0)
                {
                    dAraOdemeTutari = dSatisFiyati * (dAraOdemeOrani / 100);
                    DataRow drw = dtAraOdemeler.NewRow();
                    drw["OdemeTarihi"] = Kampanya.TeslimTarihi;
                    drw["Tutar"] = dAraOdemeTutari;
                    drw["TufeliMi"] = Kampanya.AraOdemeTeslimTufeliMi;
                    dtAraOdemeler.Rows.Add(drw);
                }
                #endregion
                #endregion

                #region ÖDEME PLANI TABINDAKİ HESAPLAMALAR......
                #region Peşinat hesapları...
                if (Kampanya.PesinatOtomatikHesaplansinMi) //peşinat otomatik hesaplansın işaretliyse...
                {


                    if (Kampanya.PesinatSabitTutarMi) //peşinat sabit tutarmı işaretliyse...
                    {
                        txtPesinat1.EditValue = C.ParseDecimal(Kampanya.PesinatSabitTutari).ToString("n2");
                        dtmPesinat1.EditValue = dtmTarih.DateTime.Date.AddDays(Kampanya.PesinatSabitKacGunSonraYatirilacak);
                        iPesinatSayisi = 1;
                    }
                    else //peşinat sabit tutarmı işaretli değilse...
                    {

                        if (C.ParseDecimal(Kampanya.PesinatOrani) > 0)
                        {
                            decimal dOran = (Kampanya.PesinatOrani / 100);
                            txtPesinat1.EditValue = C.ParseDecimal(dOran * C.ParseDecimal(txtSatisFiyati.EditValue)).ToString("n2");
                            dtmPesinat1.EditValue = dtmTarih.DateTime.Date.AddDays(Kampanya.PesinatGunu);
                            iPesinatSayisi = 1;

                            if (C.ParseDecimal(Kampanya.PesinatOrani2) > 0)
                            {
                                dOran = (Kampanya.PesinatOrani2 / 100);
                                txtPesinat2.EditValue = C.ParseDecimal(dOran * C.ParseDecimal(txtSatisFiyati.EditValue)).ToString("n2");
                                dtmPesinat2.EditValue = C.ParseDateTime(dtmPesinat1.EditValue).AddMonths(Kampanya.PesinatAyi2);
                                iPesinatSayisi = 2;

                                if (C.ParseDecimal(Kampanya.PesinatOrani3) > 0)
                                {
                                    dOran = (Kampanya.PesinatOrani3 / 100);
                                    txtPesinat3.EditValue = C.ParseDecimal(dOran * C.ParseDecimal(txtSatisFiyati.EditValue)).ToString("n2");
                                    dtmPesinat3.EditValue = C.ParseDateTime(dtmPesinat2.EditValue).AddMonths(Kampanya.PesinatAyi3);
                                    iPesinatSayisi = 3;
                                }
                            }
                        }
                    }

                    PesinatPanelleriniAyarla();
                }
                #endregion

                #region Banka Kredisi Hesapları...
                bool bBankaKredisiVarMi = Kampanya.BankaKredisiVarMi;
                decimal dBankaKredisiTutari = 0;
                if (bBankaKredisiVarMi)
                {
                    decimal dBankaKredisiOrani = Kampanya.BankaKredisiOrani;
                    if (dBankaKredisiOrani > 0)
                    {
                        dBankaKredisiTutari = C.ParseDecimal(txtListeFiyati.EditValue) * (dBankaKredisiOrani / 100);
                        txtBankaKredisi.EditValue = C.ParseDecimal(dBankaKredisiTutari).ToString("n2");
                    }
                }
                #endregion


                #region Faiz hesabı varsa hesaplanıyor...   (ListeFiyati - Peşinatlar + Banka Kredisi) x Faiz Katsayısı
                if (Kampanya.FaizHesaplamasiYapilacakMi) //faiz hesabı yapılacak işaretliyse...
                {

                    decimal dToplamPesinat = ToplamPesinatTutariGetir();
                    decimal dFaizTutari = (C.ParseDecimal(txtListeFiyati.EditValue) - (dToplamPesinat + dBankaKredisiTutari)) * Kampanya.VadeFarkiKatsayisi;
                    txtHesaplananVadeFarki.EditValue = dFaizTutari.ToString("n2");
                    txtVadeli.EditValue = (C.ParseDecimal(txtSatisFiyati.EditValue) - (dToplamPesinat + dBankaKredisiTutari)).ToString("n2");

                }
                decimal dVadelenecekTutar = C.ParseDecimal(txtVadeli.EditValue);
                #endregion


                if (C.ParseInt(cmbOdemeBicimiAna.EditValue) == 313 || C.ParseInt(cmbOdemeBicimiAna.EditValue) == 314) //vadeli ödeme varsa..
                {
                    #region vadeli kısmının taksitleri ayarlanıyor....


                    bool bTaksitGunuSabit = Kampanya.TaksitBaslangicGunuSabitMi;
                    int iTaksitBaslangicGunu = Kampanya.TaksitBaslangicGunu;
                    int iTaksitSayisi = Kampanya.TaksitSayisi;
                    int iPeriod = Kampanya.Period;

                    decimal dAraOdemelerToplami = 0;
                    foreach (DataRow rAraO in dtAraOdemeler.Rows) //ara ödemelerin toplamı bulunuyor...
                    {
                        dAraOdemelerToplami += C.ParseDecimal(rAraO["Tutar"]);
                    }

                    dVadelenecekTutar = C.ParseDecimal(txtVadeli.EditValue) - dAraOdemelerToplami;
                    // dVadelenecekTutar = (dSatisFiyati - dBankaKredisiTutari - ToplamPesinatTutariGetir()) - dAraOdemelerToplami;

                    DataTable dtOdemePlani = MListler.dsAktivite_OdemePlaniSenet.Tables[0];
                    dtOdemePlani.Rows.Clear();
                    DateTime dtmIlkTaksitTarihi = new DateTime();
                    if (bTaksitGunuSabit) //taksit günleri iTaksitBaslangicGunu değişkeninden alınarak sabitlenir.İlk Taksit tarihi için 1.peşinat tarihine 1 ay eklenip 1'er ay ilave ile devam eder.
                    {
                        dtmIlkTaksitTarihi = C.ParseDateTime(dtmTarih.DateTime.Year.ToString() + "." + dtmTarih.DateTime.Month.ToString("00") + "." + iTaksitBaslangicGunu.ToString("00")).AddMonths(1);
                    }
                    else //taksit günü sabit değilse, İlk Taksit tarihi için 1.peşinat tarihine 1 ay eklenip 1'er ay ilave ile devam eder...gün 1.peşinat günü ile aynıdır.
                    {
                        dtmIlkTaksitTarihi = dtmTarih.DateTime.AddMonths(1);
                    }

                    DateTime dtTarih = dtmIlkTaksitTarihi;
                    for (int i = 1; i <= iTaksitSayisi; i++)
                    {
                        if (i > 1)
                            dtTarih = dtTarih.AddMonths(iPeriod);

                        DataRow drwTaksit = dtOdemePlani.NewRow();
                        drwTaksit["ID"] = -1;
                        drwTaksit["CariIslemYeriKisID_310"] = 550; //aktivite ödeme planı
                        drwTaksit["KayitID"] = iAktiviteID;
                        drwTaksit["TipKisID_304"] = 380; //vadeli ödeme
                        drwTaksit["PesinatNo"] = 0;
                        drwTaksit["Tarih"] = dtTarih;
                        drwTaksit["OdemeTipiKisID_302"] = Kampanya.OdemeTipiKisID_302;
                        drwTaksit["BorcTutari"] = dVadelenecekTutar / iTaksitSayisi;
                        drwTaksit["BorcDovizID"] = C.ParseInt(cmbDoviz.EditValue);
                        drwTaksit["BorcVadeFarkiTutari"] = 0;
                        drwTaksit["BorcTutariToplam"] = 0;
                        drwTaksit["ParcalamaYapilmisMi"] = false;
                        drwTaksit["ParcalamadanOncekiTutar"] = 0;
                        drwTaksit["CariBorcParcalamaID"] = 0;
                        drwTaksit["CariBorcParcalamadanGelenID"] = 0;
                        drwTaksit["Aciklama"] = "";
                        drwTaksit["KarsiParaDovizTutari"] = 0;
                        drwTaksit["KarsiParaDovizID"] = 0;
                        drwTaksit["TufeliMi"] = false;
                        drwTaksit["AktiviteTefeTufeOranArtiID"] = 0;
                        drwTaksit["AraOdemeMi"] = false;
                        drwTaksit["KdvOdemesiMi"] = false;
                        drwTaksit["FaizHesabiTaksitTutarindanYapilsinMi"] = false;
                        drwTaksit["OdemeYeriBilgiMesaji"] = "";
                        drwTaksit["CekSeriNo"] = "";
                        drwTaksit["KapatilanBorcTutari"] = 0;
                        drwTaksit["KapatilanToplamBorcTutari"] = 0;
                        drwTaksit["TufeBaslangicTarihi"] = DBNull.Value; // C.ParseDateTime("1901.01.01 00:00:00.000");
                        drwTaksit["TufeBitisTarihi"] = DBNull.Value; // C.ParseDateTime("1901.01.01 00:00:00.000");
                        dtOdemePlani.Rows.Add(drwTaksit);
                    }
                    #endregion

                    #region Ara Ödeme ile taksit birleştirme...
                    bool bTaksitlerleAraOdemelerBirlestirilsinMi = Kampanya.AraOdemelerleTaksitlerBirlestirilsinMi;
                    if (bTaksitlerleAraOdemelerBirlestirilsinMi) //Ara ödemeler ile aynı ay ve yıla denk gelen taksit varsa birleştiriliyor...
                    {
                        foreach (DataRow dr in dtAraOdemeler.Rows)
                        {
                            DateTime dtAraOdemeTarihi = C.ParseDateTime(dr["OdemeTarihi"]);

                            DataRow[] drws = dtOdemePlani.Select(String.Format("convert(Tarih, 'System.String') LIKE '%{0}%'", dtAraOdemeTarihi.Month.ToString("00") + "." + dtAraOdemeTarihi.Year.ToString()));

                            if (drws.Length > 0)
                            {
                                decimal dAraOTutari = C.ParseDecimal(dr["Tutar"]);
                                drws[0]["BorcTutari"] = C.ParseDecimal(drws[0]["BorcTutari"]) + dAraOTutari;
                            }
                            else
                            {
                                DataRow drwYeniAraOdeme = dtOdemePlani.NewRow();
                                drwYeniAraOdeme["ID"] = -1;
                                drwYeniAraOdeme["CariIslemYeriKisID_310"] = 550; //aktivite ödeme planı
                                drwYeniAraOdeme["KayitID"] = iAktiviteID;
                                drwYeniAraOdeme["TipKisID_304"] = 380; //vadeli ödeme
                                drwYeniAraOdeme["PesinatNo"] = 0;
                                drwYeniAraOdeme["Tarih"] = dr["OdemeTarihi"];
                                drwYeniAraOdeme["OdemeTipiKisID_302"] = Kampanya.OdemeTipiKisID_302; //???????????????????????????????? ARA ÖDEMELERİN ÖDEME TİPİ NE OLACAK ?
                                drwYeniAraOdeme["BorcTutari"] = dr["Tutar"];
                                drwYeniAraOdeme["BorcDovizID"] = C.ParseInt(cmbDoviz.EditValue);
                                drwYeniAraOdeme["BorcVadeFarkiTutari"] = 0;
                                drwYeniAraOdeme["BorcTutariToplam"] = 0;
                                drwYeniAraOdeme["ParcalamaYapilmisMi"] = false;
                                drwYeniAraOdeme["ParcalamadanOncekiTutar"] = 0;
                                drwYeniAraOdeme["CariBorcParcalamaID"] = 0;
                                drwYeniAraOdeme["CariBorcParcalamadanGelenID"] = 0;
                                drwYeniAraOdeme["Aciklama"] = "";
                                drwYeniAraOdeme["KarsiParaDovizTutari"] = 0;
                                drwYeniAraOdeme["KarsiParaDovizID"] = 0;
                                drwYeniAraOdeme["TufeliMi"] = dr["TufeliMi"]; //TÜFELİ Mİ...
                                drwYeniAraOdeme["AktiviteTefeTufeOranArtiID"] = 0;
                                drwYeniAraOdeme["AraOdemeMi"] = Kampanya.OdemelerAraOdemeOlarakCheckEdilsinMi; //ARA ÖDEMEDEN GELİYOR...
                                drwYeniAraOdeme["KdvOdemesiMi"] = false;
                                drwYeniAraOdeme["FaizHesabiTaksitTutarindanYapilsinMi"] = false;
                                drwYeniAraOdeme["OdemeYeriBilgiMesaji"] = "";
                                drwYeniAraOdeme["CekSeriNo"] = "";
                                drwYeniAraOdeme["KapatilanBorcTutari"] = 0;
                                drwYeniAraOdeme["KapatilanToplamBorcTutari"] = 0;
                                drwYeniAraOdeme["TufeBaslangicTarihi"] = DBNull.Value; //C.ParseDateTime("1901.01.01 00:00:00.000");
                                drwYeniAraOdeme["TufeBitisTarihi"] = DBNull.Value; // C.ParseDateTime("1901.01.01 00:00:00.000");
                                dtOdemePlani.Rows.Add(drwYeniAraOdeme);
                            }
                        }
                    }
                    else //ara ödemeler taksitlerden ayrı kayıtlar olarak yazılıyor.
                    {
                        foreach (DataRow dr in dtAraOdemeler.Rows)
                        {
                            DataRow drwYeniAraOdeme = dtOdemePlani.NewRow();
                            drwYeniAraOdeme["ID"] = -1;
                            drwYeniAraOdeme["CariIslemYeriKisID_310"] = 550; //aktivite ödeme planı
                            drwYeniAraOdeme["KayitID"] = iAktiviteID;
                            drwYeniAraOdeme["TipKisID_304"] = 380; //vadeli ödeme
                            drwYeniAraOdeme["PesinatNo"] = 0;
                            drwYeniAraOdeme["Tarih"] = dr["OdemeTarihi"];
                            drwYeniAraOdeme["OdemeTipiKisID_302"] = Kampanya.OdemeTipiKisID_302;
                            drwYeniAraOdeme["BorcTutari"] = dr["Tutar"];
                            drwYeniAraOdeme["BorcDovizID"] = C.ParseInt(cmbDoviz.EditValue);
                            drwYeniAraOdeme["BorcVadeFarkiTutari"] = 0;
                            drwYeniAraOdeme["BorcTutariToplam"] = 0;
                            drwYeniAraOdeme["ParcalamaYapilmisMi"] = false;
                            drwYeniAraOdeme["ParcalamadanOncekiTutar"] = 0;
                            drwYeniAraOdeme["CariBorcParcalamaID"] = 0;
                            drwYeniAraOdeme["CariBorcParcalamadanGelenID"] = 0;
                            drwYeniAraOdeme["Aciklama"] = "";
                            drwYeniAraOdeme["KarsiParaDovizTutari"] = 0;
                            drwYeniAraOdeme["KarsiParaDovizID"] = 0;
                            drwYeniAraOdeme["TufeliMi"] = dr["TufeliMi"]; //TÜFELİ Mİ...
                            drwYeniAraOdeme["AktiviteTefeTufeOranArtiID"] = 0;
                            drwYeniAraOdeme["AraOdemeMi"] = Kampanya.OdemelerAraOdemeOlarakCheckEdilsinMi; //ARA ÖDEMEDEN GELİYOR...
                            drwYeniAraOdeme["KdvOdemesiMi"] = false;
                            drwYeniAraOdeme["FaizHesabiTaksitTutarindanYapilsinMi"] = false;
                            drwYeniAraOdeme["OdemeYeriBilgiMesaji"] = "";
                            drwYeniAraOdeme["CekSeriNo"] = "";
                            drwYeniAraOdeme["KapatilanBorcTutari"] = 0;
                            drwYeniAraOdeme["KapatilanToplamBorcTutari"] = 0;
                            drwYeniAraOdeme["TufeBaslangicTarihi"] = DBNull.Value; //C.ParseDateTime("1901.01.01 00:00:00.000");
                            drwYeniAraOdeme["TufeBitisTarihi"] = DBNull.Value; //C.ParseDateTime("1901.01.01 00:00:00.000");
                            dtOdemePlani.Rows.Add(drwYeniAraOdeme);
                        }
                    }



                    //TABLO SIRALANIYOR...
                    dtOdemePlani.DefaultView.Sort = "Tarih ASC";
                    dtOdemePlani = dtOdemePlani.DefaultView.ToTable();
                    #endregion
                }
                #endregion
                btnVadeliOdemeSihirbazi.Checked = false;
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void txtSatisFiyati_EditValueChanging(object sender, ChangingEventArgs e)
        {
            try
            {

                decimal dSatisFiyati = C.ParseDecimal(e.NewValue);
                decimal dListeFiyati = C.ParseDecimal(txtListeFiyati.EditValue);

                if (dListeFiyati == 0)
                    return;

                txtFark.EditValue = (dSatisFiyati - dListeFiyati).ToString("n2");

                if ((dSatisFiyati - dListeFiyati) == 0)
                {
                    txtFarkYuzde.EditValue = "";
                    return;
                }

                if (dListeFiyati > 0)
                {
                    decimal dF = ((dSatisFiyati - dListeFiyati) / dListeFiyati) * 100;
                    txtFarkYuzde.EditValue = dF.ToString("n2");
                }
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void cmbIndirim_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {

                if (e.Button.Index == 1) //aktivite statu indirim formu
                {
                    var Modal = Ayar.FormGoster();
                    F_AktiviteIndirim frm = new F_AktiviteIndirim();
                    frm.ShowDialog();
                    Modal.Close();

                    IndirimComboYukle();
                }
                else if (e.Button.Index == 2)//temizle
                {
                    cmbIndirim.EditValue = null;
                }
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void cmbIndirim_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (iAktiviteID > 0)
                {
                    this.Cursor = Cursors.Default;
                    return;
                }

                if (C.ParseInt(cmbKampanya.EditValue) > 0 && C.ParseInt(cmbIndirim.EditValue) <= 0)
                {
                    KampanyaTemizle();
                    cmbKampanya.EditValue = null;
                    txtHesaplananIndirim.EditValue = 0;
                    this.Cursor = Cursors.Default;
                    return;
                }
                else if (C.ParseInt(cmbIndirim.EditValue) <= 0)
                {
                    txtHesaplananIndirim.EditValue = 0;
                    this.Cursor = Cursors.Default;
                    return;
                }
                IndirimUygula();
                this.Cursor = Cursors.Default;
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void IndirimUygula()
        {
            try
            {

                var Indirim = MListler.MList_S_AktiviteIndirim.Where(u => u.ID == C.ParseInt(cmbIndirim.EditValue)).FirstOrDefault();
                decimal dSatisFiyati = C.ParseDecimal(txtListeFiyati.EditValue);
                decimal dSonFiyat = 0;
                decimal dToplamIndirim = 0;
                decimal dOran1 = C.ParseDecimal(Indirim.GenelIndirimOrani1);
                decimal dOran2 = C.ParseDecimal(Indirim.GenelIndirimOrani2);
                decimal dOran3 = C.ParseDecimal(Indirim.GenelIndirimOrani3);
                decimal dOran4 = C.ParseDecimal(Indirim.GenelIndirimOrani4);
                decimal dOran5 = C.ParseDecimal(Indirim.GenelIndirimOrani5);


                dSonFiyat = dSatisFiyati;
                if (C.ParseBool(Indirim.SeyyanenTutarIlkUygulansın))
                {
                    dToplamIndirim += C.ParseDecimal(Indirim.GenelIndirimSeyyanenTutar);
                    dSonFiyat = dSonFiyat - C.ParseDecimal(Indirim.GenelIndirimSeyyanenTutar);
                    dSatisFiyati = dSonFiyat;
                }


                if (dOran1 > 0)
                {
                    decimal dIndirim1 = dSonFiyat * (dOran1 / 100);
                    dSonFiyat -= dIndirim1;
                    dToplamIndirim += dIndirim1;
                }
                if (dOran2 > 0)
                {
                    decimal dIndirim2 = 0;
                    if (C.ParseBool(Indirim.GenelIndirimOrani2SiraliMi)) //Sıralıysa son tutar üzerinden indirim yapılıyor...
                    {
                        dIndirim2 = dSonFiyat * (dOran2 / 100);
                        dSonFiyat -= dIndirim2;
                    }
                    else //sıralı değilse satıs tutarı üzerinden indirim yapılıyor...
                    {
                        dIndirim2 = dSatisFiyati * (dOran2 / 100);
                        dSonFiyat -= dIndirim2;
                    }
                    dToplamIndirim += dIndirim2;
                }
                if (dOran3 > 0)
                {
                    decimal dIndirim3 = 0;
                    if (C.ParseBool(Indirim.GenelIndirimOrani3SiraliMi)) //Sıralıysa son tutar üzerinden indirim yapılıyor...
                    {
                        dIndirim3 = dSonFiyat * (dOran3 / 100);
                        dSonFiyat -= dIndirim3;
                    }
                    else //sıralı değilse satıs tutarı üzerinden indirim yapılıyor...
                    {
                        dIndirim3 = dSatisFiyati * (dOran3 / 100);
                        dSonFiyat -= dIndirim3;
                    }
                    dToplamIndirim += dIndirim3;
                }
                if (dOran4 > 0)
                {
                    decimal dIndirim4 = 0;
                    if (C.ParseBool(Indirim.GenelIndirimOrani4SiraliMi)) //Sıralıysa son tutar üzerinden indirim yapılıyor...
                    {
                        dIndirim4 = dSonFiyat * (dOran4 / 100);
                        dSonFiyat -= dIndirim4;
                    }
                    else //sıralı değilse satıs tutarı üzerinden indirim yapılıyor...
                    {
                        dIndirim4 = dSatisFiyati * (dOran4 / 100);
                        dSonFiyat -= dIndirim4;
                    }
                    dToplamIndirim += dIndirim4;
                }
                if (dOran5 > 0)
                {
                    decimal dIndirim5 = 0;
                    if (C.ParseBool(Indirim.GenelIndirimOrani5SiraliMi)) //Sıralıysa son tutar üzerinden indirim yapılıyor...
                    {
                        dIndirim5 = dSonFiyat * (dOran5 / 100);
                        dSonFiyat -= dIndirim5;
                    }
                    else //sıralı değilse satıs tutarı üzerinden indirim yapılıyor...
                    {
                        dIndirim5 = dSatisFiyati * (dOran5 / 100);
                        dSonFiyat -= dIndirim5;
                    }
                    dToplamIndirim += dIndirim5;
                }


                if (!C.ParseBool(Indirim.SeyyanenTutarIlkUygulansın))
                {
                    dToplamIndirim += C.ParseDecimal(Indirim.GenelIndirimSeyyanenTutar);
                    dSonFiyat = dSonFiyat - C.ParseDecimal(Indirim.GenelIndirimSeyyanenTutar);
                }

                txtHesaplananIndirim.EditValue = dToplamIndirim.ToString("n2");
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtTanimlananIndirim_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                SatisFiyatiHesapla();
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtTanimlananVadeFarki_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                SatisFiyatiHesapla();
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void txtHesaplananIndirim_EditValueChanged_1(object sender, EventArgs e)
        {
            try
            {
                SatisFiyatiHesapla();
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtHesaplananVadeFarki_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                SatisFiyatiHesapla();
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SatisFiyatiHesapla()
        {
            decimal dListeFiyati = C.ParseDecimal(txtListeFiyati.EditValue);
            txtSatisFiyati.EditValue = dListeFiyati -
                  (C.ParseDecimal(txtHesaplananIndirim.EditValue) + C.ParseDecimal(txtTanimlananIndirim.EditValue)) +
                  (C.ParseDecimal(txtHesaplananVadeFarki.EditValue) + C.ParseDecimal(txtTanimlananVadeFarki.EditValue));
            GayrimenkulFiyatlariHesapla();
        }




        List<Model.S_AktiviteFYT_ButonData_Result> FYT_Data = new List<Model.S_AktiviteFYT_ButonData_Result>();
        private void txtGrdGayrimenkulListeFiyati_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                ButtonEdit editor = sender as ButtonEdit;
                ButtonEditViewInfo evi = editor.GetViewInfo() as ButtonEditViewInfo;
                DevExpress.XtraEditors.Drawing.EditorButtonObjectInfoArgs bvi = evi.ButtonInfoByButton(e.Button);
                Point pt = new Point(bvi.Bounds.Left, bvi.Bounds.Bottom);
                pt = editor.PointToScreen(pt);

                int iGayrimenkulID = C.ParseInt(gvGayrimenkulBilgileri.GetFocusedRowCellValue("GayrimenkulID"));
                FYT_Data = Genel.PrmDb.S_AktiviteFYT_ButonData(iGayrimenkulID, Genel.DilID).ToList();
                popupMenuFYT.ItemLinks.Clear();

                if (FYT_Data.Count < 1)
                    return;

                string[] str = new string[20];
                str[0] = FYT_Data.FirstOrDefault().ListeFiyatiBaslik01;
                str[1] = FYT_Data.FirstOrDefault().ListeFiyatiBaslik02;
                str[2] = FYT_Data.FirstOrDefault().ListeFiyatiBaslik03;
                str[3] = FYT_Data.FirstOrDefault().ListeFiyatiBaslik04;
                str[4] = FYT_Data.FirstOrDefault().ListeFiyatiBaslik05;
                str[5] = FYT_Data.FirstOrDefault().ListeFiyatiBaslik06;
                str[6] = FYT_Data.FirstOrDefault().ListeFiyatiBaslik07;
                str[7] = FYT_Data.FirstOrDefault().ListeFiyatiBaslik08;
                str[8] = FYT_Data.FirstOrDefault().ListeFiyatiBaslik09;
                str[9] = FYT_Data.FirstOrDefault().ListeFiyatiBaslik10;
                str[10] = FYT_Data.FirstOrDefault().ListeFiyatiBaslik11;
                str[11] = FYT_Data.FirstOrDefault().ListeFiyatiBaslik12;
                str[12] = FYT_Data.FirstOrDefault().ListeFiyatiBaslik13;
                str[13] = FYT_Data.FirstOrDefault().ListeFiyatiBaslik14;
                str[14] = FYT_Data.FirstOrDefault().ListeFiyatiBaslik15;
                str[15] = FYT_Data.FirstOrDefault().ListeFiyatiBaslik16;
                str[16] = FYT_Data.FirstOrDefault().ListeFiyatiBaslik17;
                str[17] = FYT_Data.FirstOrDefault().ListeFiyatiBaslik18;
                str[18] = FYT_Data.FirstOrDefault().ListeFiyatiBaslik19;
                str[19] = FYT_Data.FirstOrDefault().ListeFiyatiBaslik20;



                var List = Genel.KullaniciFiyatListesiYetkileri(Genel.AktifKullaniciID).Where(u => u.Checkbox == true).ToList();


                for (int i = 0; i < 20; i++)
                {
                    if (str[i].Trim().Length > 0)
                    {
                        var Test = List.Where(u => u.GetirilenID == 571 + i).ToList();
                        if (Test.Count() <= 0)
                            continue;

                        DevExpress.XtraBars.BarButtonItem bbi = new DevExpress.XtraBars.BarButtonItem();

                        string strListeFiyati_AlanAdi = "GayrimenkulListeFiyati" + (i + 1).ToString("00");
                        string strListeFiyatiDovizID_AlanAdi = "GayrimenkulListeFiyati" + (i + 1).ToString("00") + "DovizID";

                        string strListeFiyati = C.ParseDecimal(gvGayrimenkulBilgileri.GetFocusedRowCellValue(strListeFiyati_AlanAdi)).ToString("n2");
                        string strDovizKodu = Genel.DovizKoduGetir(C.ParseInt(gvGayrimenkulBilgileri.GetFocusedRowCellValue(strListeFiyatiDovizID_AlanAdi)));

                        bbi.Caption = str[i] + " - " + strListeFiyati + " " + strDovizKodu;
                        bbi.Tag = i + 1;
                        bbi.ItemClick += bbi_ItemClick;
                        popupMenuFYT.ItemLinks.Add(bbi);
                    }
                }
                popupMenuFYT.ShowPopup(pt);
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bbi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int iIndex = C.ParseInt(e.Link.Item.Tag);
                decimal dListeF = 0;
                decimal dEkspertizBedeli = 0;
                int iDovizID = 0;
                switch (iIndex)
                {
                    #region Değişkenler set ediliyor...
                    case 1:
                        dListeF = C.ParseDecimal(FYT_Data.FirstOrDefault().SatisFiyati01.ToString("n2"));
                        dEkspertizBedeli = C.ParseDecimal(FYT_Data.FirstOrDefault().EkspertizBedeli1);
                        iDovizID = C.ParseInt(FYT_Data.FirstOrDefault().SatisFiyati01DovizID);
                        break;
                    case 2:
                        dListeF = C.ParseDecimal(FYT_Data.FirstOrDefault().SatisFiyati02.ToString("n2"));
                        dEkspertizBedeli = C.ParseDecimal(FYT_Data.FirstOrDefault().EkspertizBedeli2);
                        iDovizID = C.ParseInt(FYT_Data.FirstOrDefault().SatisFiyati02DovizID);
                        break;
                    case 3:
                        dListeF = C.ParseDecimal(FYT_Data.FirstOrDefault().SatisFiyati03.ToString("n2"));
                        dEkspertizBedeli = C.ParseDecimal(FYT_Data.FirstOrDefault().EkspertizBedeli3);
                        iDovizID = C.ParseInt(FYT_Data.FirstOrDefault().SatisFiyati03DovizID);
                        break;
                    case 4:
                        dListeF = C.ParseDecimal(FYT_Data.FirstOrDefault().SatisFiyati04.ToString("n2"));
                        dEkspertizBedeli = C.ParseDecimal(FYT_Data.FirstOrDefault().EkspertizBedeli4);
                        iDovizID = C.ParseInt(FYT_Data.FirstOrDefault().SatisFiyati04DovizID);
                        break;
                    case 5:
                        dListeF = C.ParseDecimal(FYT_Data.FirstOrDefault().SatisFiyati05.ToString("n2"));
                        dEkspertizBedeli = C.ParseDecimal(FYT_Data.FirstOrDefault().EkspertizBedeli5);
                        iDovizID = C.ParseInt(FYT_Data.FirstOrDefault().SatisFiyati05DovizID);
                        break;
                    case 6:
                        dListeF = C.ParseDecimal(FYT_Data.FirstOrDefault().SatisFiyati06.ToString("n2"));
                        dEkspertizBedeli = C.ParseDecimal(FYT_Data.FirstOrDefault().EkspertizBedeli6);
                        iDovizID = C.ParseInt(FYT_Data.FirstOrDefault().SatisFiyati06DovizID);
                        break;
                    case 7:
                        dListeF = C.ParseDecimal(FYT_Data.FirstOrDefault().SatisFiyati07.ToString("n2"));
                        dEkspertizBedeli = C.ParseDecimal(FYT_Data.FirstOrDefault().EkspertizBedeli7);
                        iDovizID = C.ParseInt(FYT_Data.FirstOrDefault().SatisFiyati07DovizID);
                        break;
                    case 8:
                        dListeF = C.ParseDecimal(FYT_Data.FirstOrDefault().SatisFiyati08.ToString("n2"));
                        dEkspertizBedeli = C.ParseDecimal(FYT_Data.FirstOrDefault().EkspertizBedeli8);
                        iDovizID = C.ParseInt(FYT_Data.FirstOrDefault().SatisFiyati08DovizID);
                        break;
                    case 9:
                        dListeF = C.ParseDecimal(FYT_Data.FirstOrDefault().SatisFiyati09.ToString("n2"));
                        dEkspertizBedeli = C.ParseDecimal(FYT_Data.FirstOrDefault().EkspertizBedeli9);
                        iDovizID = C.ParseInt(FYT_Data.FirstOrDefault().SatisFiyati09DovizID);
                        break;
                    case 10:
                        dListeF = C.ParseDecimal(FYT_Data.FirstOrDefault().SatisFiyati10.ToString("n2"));
                        dEkspertizBedeli = C.ParseDecimal(FYT_Data.FirstOrDefault().EkspertizBedeli10);
                        iDovizID = C.ParseInt(FYT_Data.FirstOrDefault().SatisFiyati10DovizID);
                        break;
                    case 11:
                        dListeF = C.ParseDecimal(FYT_Data.FirstOrDefault().SatisFiyati11.ToString("n2"));
                        dEkspertizBedeli = C.ParseDecimal(FYT_Data.FirstOrDefault().EkspertizBedeli11);
                        iDovizID = C.ParseInt(FYT_Data.FirstOrDefault().SatisFiyati11DovizID);
                        break;
                    case 12:
                        dListeF = C.ParseDecimal(FYT_Data.FirstOrDefault().SatisFiyati12.ToString("n2"));
                        dEkspertizBedeli = C.ParseDecimal(FYT_Data.FirstOrDefault().EkspertizBedeli12);
                        iDovizID = C.ParseInt(FYT_Data.FirstOrDefault().SatisFiyati12DovizID);
                        break;
                    case 13:
                        dListeF = C.ParseDecimal(FYT_Data.FirstOrDefault().SatisFiyati13.ToString("n2"));
                        dEkspertizBedeli = C.ParseDecimal(FYT_Data.FirstOrDefault().EkspertizBedeli13);
                        iDovizID = C.ParseInt(FYT_Data.FirstOrDefault().SatisFiyati13DovizID);
                        break;
                    case 14:
                        dListeF = C.ParseDecimal(FYT_Data.FirstOrDefault().SatisFiyati14.ToString("n2"));
                        dEkspertizBedeli = C.ParseDecimal(FYT_Data.FirstOrDefault().EkspertizBedeli14);
                        iDovizID = C.ParseInt(FYT_Data.FirstOrDefault().SatisFiyati14DovizID);
                        break;
                    case 15:
                        dListeF = C.ParseDecimal(FYT_Data.FirstOrDefault().SatisFiyati15.ToString("n2"));
                        dEkspertizBedeli = C.ParseDecimal(FYT_Data.FirstOrDefault().EkspertizBedeli15);
                        iDovizID = C.ParseInt(FYT_Data.FirstOrDefault().SatisFiyati15DovizID);
                        break;
                    case 16:
                        dListeF = C.ParseDecimal(FYT_Data.FirstOrDefault().SatisFiyati16.ToString("n2"));
                        dEkspertizBedeli = C.ParseDecimal(FYT_Data.FirstOrDefault().EkspertizBedeli16);
                        iDovizID = C.ParseInt(FYT_Data.FirstOrDefault().SatisFiyati16DovizID);
                        break;
                    case 17:
                        dListeF = C.ParseDecimal(FYT_Data.FirstOrDefault().SatisFiyati17.ToString("n2"));
                        dEkspertizBedeli = C.ParseDecimal(FYT_Data.FirstOrDefault().EkspertizBedeli17);
                        iDovizID = C.ParseInt(FYT_Data.FirstOrDefault().SatisFiyati17DovizID);
                        break;
                    case 18:
                        dListeF = C.ParseDecimal(FYT_Data.FirstOrDefault().SatisFiyati18.ToString("n2"));
                        dEkspertizBedeli = C.ParseDecimal(FYT_Data.FirstOrDefault().EkspertizBedeli18);
                        iDovizID = C.ParseInt(FYT_Data.FirstOrDefault().SatisFiyati18DovizID);
                        break;
                    case 19:
                        dListeF = C.ParseDecimal(FYT_Data.FirstOrDefault().SatisFiyati19.ToString("n2"));
                        dEkspertizBedeli = C.ParseDecimal(FYT_Data.FirstOrDefault().EkspertizBedeli19);
                        iDovizID = C.ParseInt(FYT_Data.FirstOrDefault().SatisFiyati19DovizID);
                        break;
                    case 20:
                        dListeF = C.ParseDecimal(FYT_Data.FirstOrDefault().SatisFiyati20.ToString("n2"));
                        dEkspertizBedeli = C.ParseDecimal(FYT_Data.FirstOrDefault().EkspertizBedeli20);
                        iDovizID = C.ParseInt(FYT_Data.FirstOrDefault().SatisFiyati20DovizID);
                        break;
                    #endregion
                }


                gvGayrimenkulBilgileri.SetFocusedRowCellValue("SatisFiyatiDovizID", iDovizID);
                gvGayrimenkulBilgileri.SetFocusedRowCellValue("ListeFiyati", dListeF);
                gvGayrimenkulBilgileri.SetFocusedRowCellValue("ListeFiyatiDovizKodu", Genel.DovizKoduGetir(iDovizID));
                gvGayrimenkulBilgileri.SetFocusedRowCellValue("ListeFiyatiKisID_312", 570 + iIndex);
                gvGayrimenkulBilgileri.SetFocusedRowCellValue("EkspertizBedeli", dEkspertizBedeli);
                gvGayrimenkulBilgileri.UpdateCurrentRow();


                ToplamOdemeHesapla();
                Validation();

            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void gvGayrimenkulBilgileri_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (bIsFormLoaded && e.Column.FieldName == "ListeFiyati")
                {

                    int i = e.RowHandle;

                    decimal dListeFiyati = C.ParseDecimal(gvGayrimenkulBilgileri.GetRowCellValue(i, "ListeFiyati"));
                    int iKaynakKur = C.ParseInt(gvGayrimenkulBilgileri.GetRowCellValue(i, "SatisFiyatiDovizID"));
                    int iHedefKur = C.ParseInt(cmbDoviz.EditValue);
                    decimal dKur = Genel.CaprazKurGetir(iKaynakKur, iHedefKur, 1);
                    decimal dListeFiyatiKur = (dKur == 0 ? 1 : dKur) * dListeFiyati;
                    gvGayrimenkulBilgileri.SetRowCellValue(i, "ListeFiyatiKur", dListeFiyatiKur);

                    GayrimenkulFiyatlariHesapla();
                }
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void MaliDurumHesapla()
        {
            try
            {

                decimal dSatisFiyati = C.ParseDecimal(txtSatisFiyati.EditValue);
                if (dSatisFiyati <= 0)
                    return;

                decimal dToplamPesinat = ToplamPesinatTutariGetir();


                txtMD_PesinatPlan.EditValue = dToplamPesinat.ToString("n2");
                txtMD_PesinatYuzde.EditValue = ((dToplamPesinat * 100) / dSatisFiyati).ToString("n2");


                decimal dBankaKredisi = C.ParseDecimal(txtBankaKredisi.EditValue);
                txtMD_BankaKredisiPlan.EditValue = dBankaKredisi.ToString("n2");
                txtMD_BankaKredisiYuzde.EditValue = ((dBankaKredisi * 100) / dSatisFiyati).ToString("n2");

                decimal dVadeli = C.ParseDecimal(txtVadeli.EditValue);
                txtMD_VadeliPlan.EditValue = dVadeli.ToString("n2");
                txtMD_VadeliYuzde.EditValue = ((dVadeli * 100) / dSatisFiyati).ToString("n2");



                #region Tahsilatlar...
                var ListTahsilatlar = Genel.PrmDb.S_AktiviteBakiyeGetir(iAktiviteID).ToList().FirstOrDefault();
                if (ListTahsilatlar != null)
                {
                    txtMD_PesinatTahsilat.EditValue = ListTahsilatlar.TahsilatPesinat.ToString("n2");
                    txtMD_BankaKredisiTahsilat.EditValue = ListTahsilatlar.TahsilatBanka.ToString("n2");
                    txtMD_VadeliTahsilat.EditValue = ListTahsilatlar.TahsilatVadeli.ToString("n2");
                    txtMD_ToplamTahsilat.EditValue = (ListTahsilatlar.TahsilatPesinat + ListTahsilatlar.TahsilatBanka + ListTahsilatlar.TahsilatVadeli).ToString("n2");
                }
                #endregion


                txtMD_PesinatKalan.EditValue = (C.ParseDecimal(txtMD_PesinatPlan.EditValue) - C.ParseDecimal(txtMD_PesinatTahsilat.EditValue)).ToString("n2");
                txtMD_BankaKredisiKalan.EditValue = (C.ParseDecimal(txtMD_BankaKredisiPlan.EditValue) - C.ParseDecimal(txtMD_BankaKredisiTahsilat.EditValue)).ToString("n2");
                txtMD_VadeliKalan.EditValue = (C.ParseDecimal(txtMD_VadeliPlan.EditValue) - C.ParseDecimal(txtMD_VadeliTahsilat.EditValue)).ToString("n2");

                txtMD_ToplamPlan.EditValue = (C.ParseDecimal(txtMD_PesinatPlan.EditValue) + C.ParseDecimal(txtMD_BankaKredisiPlan.EditValue) + C.ParseDecimal(txtMD_VadeliPlan.EditValue)).ToString("n2");
                txtMD_ToplamTahsilat.EditValue = (C.ParseDecimal(txtMD_PesinatTahsilat.EditValue) + C.ParseDecimal(txtMD_BankaKredisiTahsilat.EditValue) + C.ParseDecimal(txtMD_VadeliTahsilat.EditValue)).ToString("n2");
                txtMD_ToplamKalan.EditValue = (C.ParseDecimal(txtMD_ToplamPlan.EditValue) - C.ParseDecimal(txtMD_ToplamTahsilat.EditValue)).ToString("n2");


                int iOdemeBicimiID = C.ParseInt(cmbOdemeBicimiAna.EditValue);
                switch (iOdemeBicimiID)
                {
                    case 311: //peşin
                        pnlMaliDurum_BankaKredisi.Visible = false;
                        pnlMaliDurum_Vadeli.Visible = false;
                        break;
                    case 312: //peşin + banka kredisi
                        pnlMaliDurum_BankaKredisi.Visible = true;
                        pnlMaliDurum_Vadeli.Visible = false;
                        break;
                    case 313: //peşin + vadeli                    
                        pnlMaliDurum_BankaKredisi.Visible = false;
                        pnlMaliDurum_Vadeli.Visible = true;
                        break;
                    case 314: //peşin + banka kredisi + vadeli
                        pnlMaliDurum_BankaKredisi.Visible = true;
                        pnlMaliDurum_Vadeli.Visible = true;
                        break;
                }

            }
            catch (Exception Hata)
            {
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gvVadeliOdemeBilgileri_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            try
            {
                DataRowView drw = (DataRowView)e.Row;

                if (drw == null)
                {
                    return;
                }

                drw["ID"] = -1;
                drw["CariIslemYeriKisID_310"] = 550; //Aktivite Ödeme Planı            
                drw["KayitID"] = iAktiviteID;
                drw["TipKisID_304"] = 380; //Vadeli Ödeme

                int iOdemeTipi = drw["OdemeTipiKisID_302"] == null ? 322 : C.ParseInt(drw["OdemeTipiKisID_302"]);
                drw["OdemeTipiKisID_302"] = iOdemeTipi;

                decimal dBorcTutari = drw["BorcTutari"] == null ? 0 : C.ParseDecimal(drw["BorcTutari"]);
                drw["BorcTutari"] = dBorcTutari;
                drw["BorcTutariToplam"] = dBorcTutari;

                drw["BorcDovizID"] = C.ParseInt(cmbDoviz.EditValue);
                drw["BorcVadeFarkiTutari"] = 0;
                drw["ParcalamaYapilmisMi"] = false;
                drw["ParcalamadanOncekiTutar"] = 0;
                drw["CariBorcParcalamaID"] = 0;
                drw["CariBorcParcalamadanGelenID"] = 0;

                string strAciklama = drw["Aciklama"] == null ? "" : drw["Aciklama"].ToString().Trim();
                drw["Aciklama"] = strAciklama;

                drw["KarsiParaDovizTutari"] = 0;
                drw["KarsiParaDovizID"] = 0;
                drw["TufeliMi"] = drw["TufeliMi"] == null ? false : C.ParseBool(drw["TufeliMi"]);
                drw["AktiviteTefeTufeOranArtiID"] = 0;


                drw["AraOdemeMi"] = drw["AraOdemeMi"] == null ? false : C.ParseBool(drw["AraOdemeMi"]);

                drw["KdvOdemesiMi"] = false;
                drw["FaizHesabiTaksitTutarindanYapilsinMi"] = false;
                drw["OdemeYeriBilgiMesaji"] = "";

                string strCekSeriNo = drw["CekSeriNo"] == null ? "" : drw["CekSeriNo"].ToString().Trim();
                drw["CekSeriNo"] = strCekSeriNo;

                drw["KapatilanBorcTutari"] = drw["KapatilanBorcTutari"] == null ? 0 : drw["KapatilanBorcTutari"];
                drw["KapatilanToplamBorcTutari"] = drw["KapatilanToplamBorcTutari"] == null ? 0 : drw["KapatilanToplamBorcTutari"];

                DateTime dtm = drw["Tarih"] == null || C.ParseDateTime(drw["Tarih"]).Year < 1901 ? DateTime.Now.Date : C.ParseDateTime(drw["Tarih"]);
                drw["Tarih"] = dtm;

                drw["PesinatNo"] = 0;
                drw["TufeBaslangicTarihi"] = DBNull.Value; //C.ParseDateTime("1901.01.01 00:00:00.000");
                drw["TufeBitisTarihi"] = DBNull.Value; // C.ParseDateTime("1901.01.01 00:00:00.000");

                MListler.dsAktivite_OdemePlaniSenet.Tables[0].AcceptChanges();
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void btnAktiviteIptal_Click(object sender, EventArgs e)
        {
            try
            {

                var Modal = Ayar.FormGoster();
                F_AktiviteIptal frm = new F_AktiviteIptal();
                frm._AktiviteID = iAktiviteID;
                frm.ShowDialog();
                Modal.Close();

                if (frm.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    btnAktiviteIptal.Tag = 1;
                    if (C.ParseInt(cmbAktiviteDurumu.EditValue) == 302) //Rezerve
                        MListler.dsAktivite.Tables[0].Rows[0]["AktiviteStatuKisID_308"] = 460; // rezerve iptal
                    else  //Satıldı.(303)
                        MListler.dsAktivite.Tables[0].Rows[0]["AktiviteStatuKisID_308"] = 459; // satış iptal
                    bAktiviteIptalMi = true;
                    btnKaydet_Click_1(null, null);
                }
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAktiviteYetki_Click(object sender, EventArgs e)
        {
            try
            {
                var Modal = Ayar.FormGoster();
                F_AktiviteYetkileri frm = new F_AktiviteYetkileri(iAktiviteID, MListler.dsAktivite_Gayrimenkul.Tables[0]);
                frm.ShowDialog();
                Modal.Close();
                GayrimenkulFiyatlariHesapla();
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbAktiviteDurumu_EditValueChanged(object sender, EventArgs e)
        {
            YetkiAyarla();

            if (bIsFormLoaded && C.ParseInt(cmbAktiviteDurumu.EditValue) == 303) //Satıldıya cekildi..             
                GayrimenkulFiyatlariYenile(true);


            if (C.ParseInt(cmbAktiviteDurumu.EditValue) == 303) //SATILDI
                pnlRezerveSuresi.Visible = false;
            else
                pnlRezerveSuresi.Visible = true;




        }


        private void gvVadeliOdemeBilgileri_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.Ignore;
        }


        private void btnBankaKredisi_Click(object sender, EventArgs e)
        {
            try
            {
                var Modal = Ayar.FormGoster();
                F_AktiviteBankaKredisi frm = new F_AktiviteBankaKredisi(iAktiviteID);
                frm.ShowDialog();
                Modal.Close();
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gvVadeliOdemeBilgileri_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            try
            {
                DataRowView focusedRow = (DataRowView)gvVadeliOdemeBilgileri.GetFocusedRow();
                if (focusedRow != null && focusedRow.IsNew)
                {
                    foreach (DataColumn column in focusedRow.DataView.Table.Columns)
                    {
                        if (column.ColumnName == "Tarih" || column.ColumnName == "BorcTutari" || column.ColumnName == "OdemeTipiKisID_302")
                        {
                            e.Valid = !focusedRow.Row.IsNull(column);
                            if (!e.Valid)
                                return;
                        }
                        else
                            continue;
                    }
                }



                decimal dBorc = C.ParseDecimal(gvVadeliOdemeBilgileri.GetFocusedRowCellValue("BorcTutari"));
                decimal dKapatilanBorc = C.ParseDecimal(gvVadeliOdemeBilgileri.GetFocusedRowCellValue("KapatilanBorcTutari"));
                if (dBorc < dKapatilanBorc)
                {
                    Ayar.InfoMesajGoster(Genel.DilGetirMesaj(3104), Ayar.MesajTipi.Uyari, this);
                    e.Valid = false;
                    gvVadeliOdemeBilgileri.CancelUpdateCurrentRow();

                    TaksitToplamiVeFarkiHesapla();
                    return;
                }


            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void VadeliOdemeTableHazirla()
        {

            MListler.dsAktivite_OdemePlaniSenet.Tables[0].Columns["ID"].AllowDBNull = true;
            MListler.dsAktivite_OdemePlaniSenet.Tables[0].Columns["CariIslemYeriKisID_310"].AllowDBNull = true;
            MListler.dsAktivite_OdemePlaniSenet.Tables[0].Columns["KayitID"].AllowDBNull = true;
            MListler.dsAktivite_OdemePlaniSenet.Tables[0].Columns["TipKisID_304"].AllowDBNull = true;
            MListler.dsAktivite_OdemePlaniSenet.Tables[0].Columns["PesinatNo"].AllowDBNull = true;
            MListler.dsAktivite_OdemePlaniSenet.Tables[0].Columns["OdemeTipiKisID_302"].AllowDBNull = true;
            MListler.dsAktivite_OdemePlaniSenet.Tables[0].Columns["BorcTutari"].AllowDBNull = true;
            MListler.dsAktivite_OdemePlaniSenet.Tables[0].Columns["BorcDovizID"].AllowDBNull = true;
            MListler.dsAktivite_OdemePlaniSenet.Tables[0].Columns["BorcVadeFarkiTutari"].AllowDBNull = true;
            MListler.dsAktivite_OdemePlaniSenet.Tables[0].Columns["BorcTutariToplam"].AllowDBNull = true;
            MListler.dsAktivite_OdemePlaniSenet.Tables[0].Columns["ParcalamaYapilmisMi"].AllowDBNull = true;
            MListler.dsAktivite_OdemePlaniSenet.Tables[0].Columns["ParcalamadanOncekiTutar"].AllowDBNull = true;
            MListler.dsAktivite_OdemePlaniSenet.Tables[0].Columns["CariBorcParcalamaID"].AllowDBNull = true;
            MListler.dsAktivite_OdemePlaniSenet.Tables[0].Columns["CariBorcParcalamadanGelenID"].AllowDBNull = true;
            MListler.dsAktivite_OdemePlaniSenet.Tables[0].Columns["Aciklama"].AllowDBNull = true;
            MListler.dsAktivite_OdemePlaniSenet.Tables[0].Columns["KarsiParaDovizTutari"].AllowDBNull = true;
            MListler.dsAktivite_OdemePlaniSenet.Tables[0].Columns["KarsiParaDovizID"].AllowDBNull = true;
            MListler.dsAktivite_OdemePlaniSenet.Tables[0].Columns["TufeliMi"].AllowDBNull = true;
            MListler.dsAktivite_OdemePlaniSenet.Tables[0].Columns["AktiviteTefeTufeOranArtiID"].AllowDBNull = true;
            MListler.dsAktivite_OdemePlaniSenet.Tables[0].Columns["AraOdemeMi"].AllowDBNull = true;
            MListler.dsAktivite_OdemePlaniSenet.Tables[0].Columns["KdvOdemesiMi"].AllowDBNull = true;
            MListler.dsAktivite_OdemePlaniSenet.Tables[0].Columns["FaizHesabiTaksitTutarindanYapilsinMi"].AllowDBNull = true;
            MListler.dsAktivite_OdemePlaniSenet.Tables[0].Columns["OdemeYeriBilgiMesaji"].AllowDBNull = true;
            MListler.dsAktivite_OdemePlaniSenet.Tables[0].Columns["CekSeriNo"].AllowDBNull = true;
            MListler.dsAktivite_OdemePlaniSenet.Tables[0].Columns["KapatilanBorcTutari"].AllowDBNull = true;
            MListler.dsAktivite_OdemePlaniSenet.Tables[0].Columns["KapatilanToplamBorcTutari"].AllowDBNull = true;
            MListler.dsAktivite_OdemePlaniSenet.Tables[0].Columns["Tarih"].AllowDBNull = true;
            MListler.dsAktivite_OdemePlaniSenet.Tables[0].Columns["TufeBaslangicTarihi"].AllowDBNull = true;
            MListler.dsAktivite_OdemePlaniSenet.Tables[0].Columns["TufeBitisTarihi"].AllowDBNull = true;
            grdVadeliOdemeBilgileri.DataSource = MListler.dsAktivite_OdemePlaniSenet.Tables[0];
        }

        private void txtTanimlananVadeFarki_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            ButtonEdit txt = (ButtonEdit)sender;
            if (C.ParseDecimal(txtSatisFiyati.EditValue) <= 0)
                return;

            F_AktiviteTutarOran_Giris frm = new F_AktiviteTutarOran_Giris();
            frm._AnaTutar = C.ParseDecimal(txtSatisFiyati.EditValue);
            frm.ShowDialog();

            if (frm.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                txt.EditValue = frm._AktarilacakTutar;
            }


        }

        private void bbtnGenelTanitimFormu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }



        private void bbiKaydetKapat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            KaydetKapat(true);
        }

        public void btnKaydet_Click_1(object sender, EventArgs e)
        {
            KaydetKapat(false);
        }



        private SqlParameter ParametreAl(string strAlanAdi)
        {
            try
            {

                SqlParameter parameter = new SqlParameter();
                string Tip = MListler.dsAktivite.Tables[0].Columns[strAlanAdi].DataType.FullName;

                if (Tip.Contains("Int32"))
                {
                    parameter.SqlDbType = SqlDbType.Int;
                    parameter.Value = C.ParseInt(MListler.dsAktivite.Tables[0].Rows[0][strAlanAdi]);
                }
                else if (Tip.Contains("Decimal"))
                {
                    parameter.SqlDbType = SqlDbType.Decimal;
                    parameter.Value = C.ParseDecimal(MListler.dsAktivite.Tables[0].Rows[0][strAlanAdi]);
                }
                else if (Tip.Contains("Bool"))
                {
                    parameter.SqlDbType = SqlDbType.Bit;
                    parameter.Value = C.ParseInt(MListler.dsAktivite.Tables[0].Rows[0][strAlanAdi]);
                }
                else if (Tip.Contains("DateTime"))
                {
                    parameter.SqlDbType = SqlDbType.DateTime;
                    parameter.Value = C.ParseDateTimeOrDBNULL(MListler.dsAktivite.Tables[0].Rows[0][strAlanAdi]);
                }
                else if (Tip.Contains("String"))
                {
                    parameter.SqlDbType = SqlDbType.NVarChar;
                    parameter.Value = MListler.dsAktivite.Tables[0].Rows[0][strAlanAdi].ToString();
                }
                parameter.ParameterName = "@" + strAlanAdi;
                return parameter;
            }
            catch (Exception exc)
            {
                return null;
            }
        }

        private void KaydetKapat(bool bKayittanSonraKapansin)
        {
            try
            {
                if (C.ParseInt(btnAktiviteIptal.Tag) != 1)
                {
                    if (!KayitOncesiVal())
                    {
                        this.Cursor = Cursors.Default;
                        Ayar.InfoMesajGoster(Genel.DilGetirMesaj(3140), Ayar.MesajTipi.Hata, this);
                        return;
                    }
                }



                this.Cursor = Cursors.WaitCursor;

                if (iAktiviteID <= 0 && Genel.Aktivite_ProjeID > 0) //yeni kayıt ise önce bi row atılıyor...
                    AktiviteTableHazirla(true);


                AktiviteTableHazirla(false);




                //F_Dosyalar frm = (F_Dosyalar)Application.OpenForms["F_Dosyalar"];
                //if (frm != null)
                //    frm.Kaydet();


                #region Peşinat ve BankaKredileri MListler.dsAktivite_OdemePlaniPesinatBanka.Tables[0] tablosuna yazılıyor.
                if (iAktiviteID < 1)
                    MListler.dsAktivite_OdemePlaniPesinatBanka.Tables[0].Rows.Clear();


                if (MListler.dsAktivite_OdemePlaniPesinatBanka.Tables[0].Rows.Count == 0)
                {
                    for (int i = 1; i <= 11; i++)
                    {
                        DataRow drwYeniPesinat = MListler.dsAktivite_OdemePlaniPesinatBanka.Tables[0].NewRow();
                        drwYeniPesinat["ID"] = -1;
                        drwYeniPesinat["CariIslemYeriKisID_310"] = 550; //Aktivite Ödeme Planı
                        drwYeniPesinat["KayitID"] = iAktiviteID;

                        int iTipKisID_304 = 381;//peşinat ise peşinat
                        if (i == 11) //Banka Kredisi ise b.k.
                            iTipKisID_304 = 382; //B.Kredisi.
                        drwYeniPesinat["TipKisID_304"] = iTipKisID_304;

                        int iOdemeTipiKisID_302 = 330;//peşinat ise peşinat
                        if (i == 11) //Banka Kredisi ise BankaK.
                            iOdemeTipiKisID_302 = 327; //B.Kredisi.
                        drwYeniPesinat["OdemeTipiKisID_302"] = iOdemeTipiKisID_302;

                        drwYeniPesinat["PesinatNo"] = i;
                        drwYeniPesinat["BorcDovizID"] = C.ParseInt(cmbDoviz.EditValue);
                        drwYeniPesinat["BorcVadeFarkiTutari"] = 0;
                        drwYeniPesinat["ParcalamaYapilmisMi"] = false;
                        drwYeniPesinat["ParcalamadanOncekiTutar"] = 0;
                        drwYeniPesinat["CariBorcParcalamaID"] = 0;
                        drwYeniPesinat["CariBorcParcalamadanGelenID"] = 0;
                        drwYeniPesinat["Aciklama"] = "";
                        drwYeniPesinat["KarsiParaDovizTutari"] = 0;
                        drwYeniPesinat["KarsiParaDovizID"] = 0;
                        drwYeniPesinat["TufeliMi"] = false;
                        drwYeniPesinat["AktiviteTefeTufeOranArtiID"] = 0;
                        drwYeniPesinat["AraOdemeMi"] = false;
                        drwYeniPesinat["KdvOdemesiMi"] = false;
                        drwYeniPesinat["FaizHesabiTaksitTutarindanYapilsinMi"] = false;
                        drwYeniPesinat["OdemeYeriBilgiMesaji"] = "";
                        drwYeniPesinat["CekSeriNo"] = "";
                        drwYeniPesinat["KapatilanBorcTutari"] = 0;
                        drwYeniPesinat["KapatilanToplamBorcTutari"] = 0;
                        //drwYeniPesinat["TufeBaslangicTarihi"] = C.ParseDateTime("1901.01.01 00:00:00.000");
                        //drwYeniPesinat["TufeBitisTarihi"] = C.ParseDateTime("1901.01.01 00:00:00.000");
                        //TARIH
                        drwYeniPesinat["TufeBaslangicTarihi"] = DBNull.Value; // C.ParseDateTime("1901.01.01 00:00:00.000");
                        drwYeniPesinat["TufeBitisTarihi"] = DBNull.Value; //C.ParseDateTime("1901.01.01 00:00:00.000");


                        Control[] ctrls = this.Controls.Find("txtPesinat" + i.ToString(), true);
                        if (ctrls.Length > 0) //Peşinatlar ekleniyorsa...
                        {
                            decimal dBorcTutari = C.ParseDecimal(C.ParseDecimal(((TextEdit)ctrls[0]).EditValue).ToString("n2"));
                            drwYeniPesinat["BorcTutari"] = dBorcTutari;
                            drwYeniPesinat["BorcTutariToplam"] = dBorcTutari;
                        }
                        else if (i == 11) //Banka Kredisi ekleniyorsa..
                        {
                            drwYeniPesinat["BorcTutari"] = C.ParseDecimal(C.ParseDecimal(txtBankaKredisi.EditValue).ToString("n2"));
                            drwYeniPesinat["BorcTutariToplam"] = C.ParseDecimal(C.ParseDecimal(txtBankaKredisi.EditValue).ToString("n2"));
                        }

                        Control[] ctrlsDate = this.Controls.Find("dtmPesinat" + i.ToString(), true);
                        if (ctrlsDate.Length > 0)
                        {
                            object dt = ((DateEdit)ctrlsDate[0]).DateTime.Date;
                            //if (C.ParseDateTime(dt).Year < 1901)
                            //    dt = C.ParseDateTime("1901.01.01 00:00:00.000");

                            //drwYeniPesinat["Tarih"] = dt;
                            //TARIH
                            if (C.ParseDateTime(dt).Year < 2000)
                                dt = DBNull.Value;

                            drwYeniPesinat["Tarih"] = dt;

                        }
                        else if (i == 11) //Banka Kredisi ekleniyorsa..
                        {
                            //drwYeniPesinat["Tarih"] = dtmBankaKredisi.DateTime.Date.Year < 2000 ? C.ParseDateTime("1901.01.01 00:00:00.000") : dtmBankaKredisi.DateTime.Date;
                            //TARIH
                            object dt = dtmBankaKredisi.DateTime.Date;
                            if (C.ParseDateTime(dt).Year < 2000)
                                dt = DBNull.Value;

                            drwYeniPesinat["Tarih"] = dt;
                        }

                        MListler.dsAktivite_OdemePlaniPesinatBanka.Tables[0].Rows.Add(drwYeniPesinat);
                    }
                }
                else
                {
                    for (int i = 0; i <= 10; i++)
                    {
                        MListler.dsAktivite_OdemePlaniPesinatBanka.Tables[0].Rows[i]["BorcDovizID"] = C.ParseInt(cmbDoviz.EditValue);
                        Control[] ctrls = this.Controls.Find("txtPesinat" + (i + 1).ToString(), true);
                        if (ctrls.Length > 0) //Peşinatlar...
                        {
                            decimal dBorcTutari = C.ParseDecimal(C.ParseDecimal(((TextEdit)ctrls[0]).EditValue).ToString("n2"));
                            MListler.dsAktivite_OdemePlaniPesinatBanka.Tables[0].Rows[i]["BorcTutari"] = dBorcTutari;
                            MListler.dsAktivite_OdemePlaniPesinatBanka.Tables[0].Rows[i]["BorcTutariToplam"] = dBorcTutari;
                        }
                        else if (i == 10) //Banka Kredisi ise..
                        {
                            MListler.dsAktivite_OdemePlaniPesinatBanka.Tables[0].Rows[i]["BorcTutari"] = C.ParseDecimal(C.ParseDecimal(txtBankaKredisi.EditValue).ToString("n2"));
                            MListler.dsAktivite_OdemePlaniPesinatBanka.Tables[0].Rows[i]["BorcTutariToplam"] = C.ParseDecimal(C.ParseDecimal(txtBankaKredisi.EditValue).ToString("n2"));

                        }

                        Control[] ctrlsDate = this.Controls.Find("dtmPesinat" + (i + 1).ToString(), true);
                        if (ctrlsDate.Length > 0)
                        {
                            object dt = ((DateEdit)ctrlsDate[0]).DateTime.Date;
                            if (C.ParseDateTime(dt).Year < 2000)
                                dt = DBNull.Value;

                            //dt = C.ParseDateTime("1901.01.01 00:00:00.000");

                            MListler.dsAktivite_OdemePlaniPesinatBanka.Tables[0].Rows[i]["Tarih"] = dt;
                        }
                        else if (i == 10) //Banka Kredisi ise..
                        {
                            //MListler.dsAktivite_OdemePlaniPesinatBanka.Tables[0].Rows[i]["Tarih"] = dtmBankaKredisi.DateTime.Date.Year < 2000 ? C.ParseDateTime("1901.01.01 00:00:00.000") : dtmBankaKredisi.DateTime.Date;
                            object dt = dtmBankaKredisi.DateTime.Date;
                            if (C.ParseDateTime(dt).Year < 2000)
                                dt = DBNull.Value;

                            MListler.dsAktivite_OdemePlaniPesinatBanka.Tables[0].Rows[i]["Tarih"] = dt;
                        }
                    }
                }
                #endregion



                DataTable dtAktiviteMusteri = MListler.dsAktivite_Musteri.Tables[0];
                DataTable dtAktiviteGayrimenkul = MListler.dsAktivite_Gayrimenkul.Tables[0];
                DataTable dtAktiviteOdemePlaniSenet = MListler.dsAktivite_OdemePlaniSenet.Tables[0];
                DataTable dtAktiviteOdemePlaniPesinatBanka = MListler.dsAktivite_OdemePlaniPesinatBanka.Tables[0];

                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = Genel.PrmDb.Database.Connection.ConnectionString;
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SEDS_Aktivite";
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter prmAktiviteMusteriTablo = new SqlParameter("@DT_AktiviteMusteri", dtAktiviteMusteri);
                    prmAktiviteMusteriTablo.SqlDbType = SqlDbType.Structured;
                    prmAktiviteMusteriTablo.TypeName = "dbo.AktiviteMusteriTablo";
                    cmd.Parameters.Add(prmAktiviteMusteriTablo);

                    SqlParameter prmAktiviteGayrimenkulTablo = new SqlParameter("@DT_AktiviteGayrimenkul", dtAktiviteGayrimenkul);
                    prmAktiviteGayrimenkulTablo.SqlDbType = SqlDbType.Structured;
                    prmAktiviteGayrimenkulTablo.TypeName = "dbo.AktiviteGayrimenkulTablo";
                    cmd.Parameters.Add(prmAktiviteGayrimenkulTablo);

                    SqlParameter prmAktiviteSenetTablo = new SqlParameter("@DT_AktiviteOdemePlaniSenet", dtAktiviteOdemePlaniSenet);
                    prmAktiviteSenetTablo.SqlDbType = SqlDbType.Structured;
                    prmAktiviteSenetTablo.TypeName = "dbo.AktiviteOdemePlaniTablo";
                    cmd.Parameters.Add(prmAktiviteSenetTablo);

                    SqlParameter prmAktivitePesinatBankaTablo = new SqlParameter("@DT_AktiviteOdemePlaniPesinatBanka", dtAktiviteOdemePlaniPesinatBanka);
                    prmAktivitePesinatBankaTablo.SqlDbType = SqlDbType.Structured;
                    prmAktivitePesinatBankaTablo.TypeName = "dbo.AktiviteOdemePlaniTablo";
                    cmd.Parameters.Add(prmAktivitePesinatBankaTablo);

                    SqlParameter prmKullaniciID = new SqlParameter("@KullaniciID", Genel.AktifKullaniciID);
                    prmKullaniciID.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(prmKullaniciID);

                    SqlParameter prmAktiviteID = new SqlParameter("@AktiviteID", iAktiviteID);
                    prmAktiviteID.SqlDbType = SqlDbType.Int;
                    prmAktiviteID.Direction = ParameterDirection.InputOutput;
                    cmd.Parameters.Add(prmAktiviteID);

                    cmd.Parameters.Add(ParametreAl("AktifMi"));
                    cmd.Parameters.Add(ParametreAl("HareketTipiKisID_300"));
                    cmd.Parameters.Add(ParametreAl("AktiviteStatuKisID_308"));
                    cmd.Parameters.Add(ParametreAl("ListeFiyatiKur"));
                    cmd.Parameters.Add(ParametreAl("SatisFiyati"));
                    cmd.Parameters.Add(ParametreAl("SatisFiyatiDovizID"));
                    cmd.Parameters.Add(ParametreAl("OdemeTahutuKisID_301"));
                    cmd.Parameters.Add(ParametreAl("PlanPesinatToplam"));
                    cmd.Parameters.Add(ParametreAl("PlanPesinat01"));
                    cmd.Parameters.Add(ParametreAl("PlanPesinat02"));
                    cmd.Parameters.Add(ParametreAl("PlanPesinat03"));
                    cmd.Parameters.Add(ParametreAl("PlanPesinat04"));
                    cmd.Parameters.Add(ParametreAl("PlanPesinat05"));
                    cmd.Parameters.Add(ParametreAl("PlanPesinat06"));
                    cmd.Parameters.Add(ParametreAl("PlanPesinat07"));
                    cmd.Parameters.Add(ParametreAl("PlanPesinat08"));
                    cmd.Parameters.Add(ParametreAl("PlanPesinat09"));
                    cmd.Parameters.Add(ParametreAl("PlanPesinat10"));
                    cmd.Parameters.Add(ParametreAl("PlanBanka"));
                    cmd.Parameters.Add(ParametreAl("PlanVadeli"));
                    cmd.Parameters.Add(ParametreAl("AktiviteTarihi"));
                    cmd.Parameters.Add(ParametreAl("SozlesmeYapildiMi"));
                    cmd.Parameters.Add(ParametreAl("SozlesmeTarihi"));
                    cmd.Parameters.Add(ParametreAl("PlanPesinat01Tarih"));
                    cmd.Parameters.Add(ParametreAl("PlanPesinat02Tarih"));
                    cmd.Parameters.Add(ParametreAl("PlanPesinat03Tarih"));
                    cmd.Parameters.Add(ParametreAl("PlanPesinat04Tarih"));
                    cmd.Parameters.Add(ParametreAl("PlanPesinat05Tarih"));
                    cmd.Parameters.Add(ParametreAl("PlanPesinat06Tarih"));
                    cmd.Parameters.Add(ParametreAl("PlanPesinat07Tarih"));
                    cmd.Parameters.Add(ParametreAl("PlanPesinat08Tarih"));
                    cmd.Parameters.Add(ParametreAl("PlanPesinat09Tarih"));
                    cmd.Parameters.Add(ParametreAl("PlanPesinat10Tarih"));
                    cmd.Parameters.Add(ParametreAl("PlanBankaTarih"));
                    cmd.Parameters.Add(ParametreAl("OfisID"));
                    cmd.Parameters.Add(ParametreAl("AnaMusteriID"));
                    cmd.Parameters.Add(ParametreAl("MusteriAdSoyadlariToplu"));
                    cmd.Parameters.Add(ParametreAl("NpvTutari"));
                    cmd.Parameters.Add(ParametreAl("OrtalamaVadeTarihi"));
                    cmd.Parameters.Add(ParametreAl("TanimVadeFarkiTutari"));
                    cmd.Parameters.Add(ParametreAl("TanimIndirimTutari"));
                    cmd.Parameters.Add(ParametreAl("DigerYazilimID"));
                    cmd.Parameters.Add(ParametreAl("MusteriIletisimID"));
                    cmd.Parameters.Add(ParametreAl("SatisPersonelID1"));
                    cmd.Parameters.Add(ParametreAl("SatisPersonelID2"));
                    cmd.Parameters.Add(ParametreAl("AktivitePersonelID"));
                    cmd.Parameters.Add(ParametreAl("Araci1AcentaID"));
                    cmd.Parameters.Add(ParametreAl("Araci2AcentaID"));
                    cmd.Parameters.Add(ParametreAl("HesaplananVadeFarkiTutari"));
                    cmd.Parameters.Add(ParametreAl("HesaplananIndirimTutari"));
                    cmd.Parameters.Add(ParametreAl("ToplamVadeFarkiTutari"));
                    cmd.Parameters.Add(ParametreAl("ToplamIndirimTutari"));
                    cmd.Parameters.Add(ParametreAl("AktiviteIndirimID"));
                    cmd.Parameters.Add(ParametreAl("AktiviteKampanyaID"));
                    cmd.Parameters.Add(ParametreAl("RezerveSuresi"));
                    cmd.Parameters.Add(ParametreAl("MusteriKaynagiID_1"));
                    cmd.Parameters.Add(ParametreAl("MusteriKaynagiID_2"));
                    cmd.Parameters.Add(ParametreAl("Odedigi"));
                    cmd.Parameters.Add(ParametreAl("OdedigiDepozitosuz"));
                    cmd.Parameters.Add(ParametreAl("DepozitoOdenen"));
                    cmd.Parameters.Add(ParametreAl("KalanBorcu"));
                    cmd.Parameters.Add(ParametreAl("Odenecek30"));
                    cmd.Parameters.Add(ParametreAl("Odenecek60"));
                    cmd.Parameters.Add(ParametreAl("Odenecek90"));
                    cmd.Parameters.Add(ParametreAl("Odenecek180"));
                    cmd.Parameters.Add(ParametreAl("TahsilatPesinat"));
                    cmd.Parameters.Add(ParametreAl("TahsilatBanka"));
                    cmd.Parameters.Add(ParametreAl("TahsilatVadeli"));
                    cmd.Parameters.Add(ParametreAl("TahsilatKDV"));
                    cmd.Parameters.Add(ParametreAl("BankaKredisiPlan"));
                    cmd.Parameters.Add(ParametreAl("BankaKredisiOdeme"));
                    cmd.Parameters.Add(ParametreAl("BankaKredisiBakiye"));
                    cmd.Parameters.Add(ParametreAl("OdedigiVadeFarkiTutari"));
                    cmd.Parameters.Add(ParametreAl("OdedigiVadeFarkliTutar"));
                    cmd.Parameters.Add(ParametreAl("OdedigiYuzde"));
                    cmd.Parameters.Add(ParametreAl("ToplamVadeFarki"));
                    cmd.Parameters.Add(ParametreAl("KalanBorcuVadeFarkliTutar"));
                    cmd.Parameters.Add(ParametreAl("BankaVadeliOdemeOnaylandiMi"));
                    cmd.Parameters.Add(ParametreAl("BankaVadeliOdemeOnayTarihi"));
                    cmd.Parameters.Add(ParametreAl("BankaKatkipayiOdemesiDurumuKisID_44"));
                    cmd.Parameters.Add(ParametreAl("KrediKullanilanBankaDurumSecID_45"));
                    cmd.Parameters.Add(ParametreAl("KrediKullanilanBankaSubeDurumSecID_63"));
                    cmd.Parameters.Add(ParametreAl("KrediVade"));
                    cmd.Parameters.Add(ParametreAl("KrediOran"));
                    cmd.Parameters.Add(ParametreAl("KrediAylikOdeme"));
                    cmd.Parameters.Add(ParametreAl("KrediToplamOdeme"));
                    cmd.Parameters.Add(ParametreAl("KrediMusteriOran"));
                    cmd.Parameters.Add(ParametreAl("KrediBankaMusteriNo"));
                    cmd.Parameters.Add(ParametreAl("KrediKullandirimTarihi"));
                    cmd.Parameters.Add(ParametreAl("KrediIpotekEvragiGeldiMi"));
                    cmd.Parameters.Add(ParametreAl("KrediBankaYazisiGeldiMi"));
                    cmd.Parameters.Add(ParametreAl("KrediBankaTapudaIpotekKoyduMu"));
                    cmd.Parameters.Add(ParametreAl("KrediBankaIpotekTutari"));
                    cmd.Parameters.Add(ParametreAl("BankaKredisiEvrakDurumKisID_54"));
                    cmd.Parameters.Add(ParametreAl("KrediKomisyonOrani"));
                    cmd.Parameters.Add(ParametreAl("BankaPesinatTutari"));
                    cmd.Parameters.Add(ParametreAl("BankaIstenilenKrediTutari"));
                    cmd.Parameters.Add(ParametreAl("VekaletnameAlindiMi"));
                    cmd.Parameters.Add(ParametreAl("VadeliSatisIpotegiVarMi"));
                    cmd.Parameters.Add(ParametreAl("IpotekDerecesiDurumSecID_5"));
                    cmd.Parameters.Add(ParametreAl("IpotekTutari"));
                    cmd.Parameters.Add(ParametreAl("IpotekTutariDovizID"));
                    cmd.Parameters.Add(ParametreAl("TapuTeslimIcinHazirMi"));
                    cmd.Parameters.Add(ParametreAl("TapuIslemleriIcinHazir"));
                    cmd.Parameters.Add(ParametreAl("TapuTeslimOlduMu"));
                    cmd.Parameters.Add(ParametreAl("TapuTeslimTarihi"));
                    cmd.Parameters.Add(ParametreAl("IbranameAlindiMi"));
                    cmd.Parameters.Add(ParametreAl("MuhasebeKodu"));
                    cmd.Parameters.Add(ParametreAl("SatisNo"));
                    cmd.Parameters.Add(ParametreAl("MusteriNo"));
                    cmd.Parameters.Add(ParametreAl("ProjeKayitNumarasi"));
                    cmd.Parameters.Add(ParametreAl("SerefiyeBedeli"));
                    cmd.Parameters.Add(ParametreAl("AylikBrutFaiz"));
                    cmd.Parameters.Add(ParametreAl("SatisdaReferansOlanMusteriID"));
                    cmd.Parameters.Add(ParametreAl("BrutFaizTutari"));
                    cmd.Parameters.Add(ParametreAl("NoterSozlesmesiYapildiMi"));
                    cmd.Parameters.Add(ParametreAl("NoterSozlesmesiRandevuTarihSaati"));
                    cmd.Parameters.Add(ParametreAl("NoterSozlesmesiNotu"));
                    cmd.Parameters.Add(ParametreAl("NoterSatisiYevmiyeNo"));
                    cmd.Parameters.Add(ParametreAl("NoterDurumSecID_62"));
                    cmd.Parameters.Add(ParametreAl("MuhasebeKodu136"));
                    cmd.Parameters.Add(ParametreAl("MuhasebeKodu120"));
                    cmd.Parameters.Add(ParametreAl("ReferansOlanMusteriID"));
                    cmd.Parameters.Add(ParametreAl("IsimDegisikligiDevirOlarakYapildiMi"));
                    cmd.Parameters.Add(ParametreAl("OdemeIadesiYapilacakMi"));
                    cmd.Parameters.Add(ParametreAl("EkProtokolEvragiAlindiMi"));
                    cmd.Parameters.Add(ParametreAl("EkProtokolEvragiTarihi"));
                    cmd.Parameters.Add(ParametreAl("IpotekTarihi"));
                    cmd.Parameters.Add(ParametreAl("DbsOnaylandiMi"));
                    cmd.Parameters.Add(ParametreAl("DbsOnayTarihi"));
                    cmd.Parameters.Add(ParametreAl("VekilMusteriID"));
                    cmd.Parameters.Add(ParametreAl("VesayetMusteriID"));
                    cmd.Parameters.Add(ParametreAl("KefilMusteriID"));
                    cmd.Parameters.Add(ParametreAl("NoterSozlesmesiniImzalayanDurumSecID_22"));
                    cmd.Parameters.Add(ParametreAl("IndirimQueryCalissinMi"));
                    cmd.Parameters.Add(ParametreAl("KampanyaQueryCalissinMi"));
                    cmd.Parameters.Add(ParametreAl("ProjeOzelQueryCalissinMi"));
                    cmd.Parameters.Add(ParametreAl("FormQueryCalissinMi"));
                    cmd.Parameters.Add(ParametreAl("AcentaninKomisyonYuzdesi"));
                    cmd.Parameters.Add(ParametreAl("AcentaninVazgectigiKomisyonTutari"));
                    cmd.Parameters.Add(ParametreAl("AcentaPrimHesaplamaTipiKisID_29"));
                    cmd.Parameters.Add(ParametreAl("AcentaKomisyonu"));
                    cmd.Parameters.Add(ParametreAl("AcentaOdemeHakedisYuzdesi"));
                    cmd.Parameters.Add(ParametreAl("AcentaOdemeGunuArti"));
                    cmd.Parameters.Add(ParametreAl("AcentaKomisyon2"));
                    cmd.Parameters.Add(ParametreAl("AcentaOdemeHakedisYuzdesi2"));
                    cmd.Parameters.Add(ParametreAl("AcentaKomisyon3"));
                    cmd.Parameters.Add(ParametreAl("AcentaOdemeHakedisYuzdesi3"));
                    cmd.Parameters.Add(ParametreAl("AcentaKdvDahilMi"));
                    cmd.Parameters.Add(ParametreAl("AcentaKdvOrani"));
                    cmd.Parameters.Add(ParametreAl("AcentaYuzde1EksikHesapla"));
                    cmd.Parameters.Add(ParametreAl("AcentaKomisyonunuNpvdenYapilsinMi"));
                    cmd.Parameters.Add(ParametreAl("IptalNedenIDKisID_6"));
                    cmd.Parameters.Add(ParametreAl("IptalTarihi"));
                    cmd.Parameters.Add(ParametreAl("IptalEdenPersonel"));
                    cmd.Parameters.Add(ParametreAl("IptalTuruDurumSec_ID_49"));
                    cmd.Parameters.Add(ParametreAl("IptalAciklama"));
                    cmd.Parameters.Add(ParametreAl("IptalIadeHesapSahibi"));
                    cmd.Parameters.Add(ParametreAl("IptalIadeBanka"));
                    cmd.Parameters.Add(ParametreAl("IptalIadeSubeSubeKodu"));
                    cmd.Parameters.Add(ParametreAl("IptalIadeIBAN"));
                    cmd.Parameters.Add(ParametreAl("OdemeIadesiKesintiTutari"));
                    cmd.Parameters.Add(ParametreAl("IptalYerDegisikligiGyr_AktiviteID"));
                    cmd.Parameters.Add(ParametreAl("IptalYerDegisikligiDurumu"));
                    cmd.Parameters.Add(ParametreAl("OdemeIadesiPlanlananTarih"));


                    cmd.ExecuteNonQuery();

                    iAktiviteID = C.ParseInt(cmd.Parameters["@AktiviteID"].Value);
                }
                conn.Close();




                DataGetir();



                if (C.ParseInt(btnAktiviteIptal.Tag) == 0) //satıs iptal ve rezerve iptal değilse
                {
                    TeklifeAktiviteIDKaydet();
                }
                bFormKapatmaIzniVarMi = SQLKontrolleri(true);


                btnAktiviteIptal.Visible = true;
                formYanMenu1.Visible = true;


                if (C.ParseInt(btnAktiviteIptal.Tag) == 1)
                {
                    formYanMenu1.Enabled = false;
                    btnAktiviteIptal.Enabled = false;
                    btnKaydet.Enabled = false;
                }

                YetkiAyarla();
                Ayar.InfoMesajGoster(Genel.DilGetirMesaj(1017), Ayar.MesajTipi.Bilgi, this);
                bIsFormSaved = true;
                this.Cursor = Cursors.Default;

                if (bKayittanSonraKapansin)
                    this.Close();
            }
            catch (Exception Hata)
            {
                this.Cursor = Cursors.Default;
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TeklifeAktiviteIDKaydet()
        {


            bool bTopluSatisMi = false;
            if (gvGayrimenkulBilgileri.DataRowCount > 1)
            {
                bTopluSatisMi = true;
            }
            else
                iGayrimenkulID = C.ParseInt(gvGayrimenkulBilgileri.GetRowCellValue(0, "GayrimenkulID"));


            string strGayrimenkulIDS = "";

            for (int i = 0; i < gvGayrimenkulBilgileri.DataRowCount; i++)
            {
                string strG_ID = gvGayrimenkulBilgileri.GetRowCellValue(i, "GayrimenkulID").ToString();
                strGayrimenkulIDS += strG_ID + ",";
            }

            if (strGayrimenkulIDS.Length > 0)
                strGayrimenkulIDS = strGayrimenkulIDS.Remove(strGayrimenkulIDS.Length - 1, 1);



            if (bTopluSatisMi)
                iGayrimenkulID = 0;
            else
                strGayrimenkulIDS = "";

            int iMusteriID = C.ParseInt(gvMusteriBilgileri.GetRowCellValue(0, "MusteriID"));

            Genel.PrmDb.SEDS_Teklifler_AktiviteID(strGayrimenkulIDS, iGayrimenkulID, iMusteriID, iAktiviteID);
        }


 

        private void txtGrdFiyatYenile_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            GayrimenkulFiyatlariYenile(false);
        }




        private void GayrimenkulFiyatlariYenile(bool bTumuMu)
        {
            try
            {
                decimal dToplamGyrListeFiyati = 0;

                if (bTumuMu)
                {
                    for (int i = 0; i < gvGayrimenkulBilgileri.DataRowCount; i++)
                    {
                        int iGyrID = C.ParseInt(gvGayrimenkulBilgileri.GetRowCellValue(i, "GayrimenkulID"));

                        if (iGyrID > 0)
                        {
                            var GYR = Genel.PrmDb.S_GayrimenkulID(iGyrID, Genel.DilID).ToList();
                            if (GYR.Count <= 0)
                                continue;
                            decimal dSatisF = C.ParseDecimal(GYR.FirstOrDefault().SatisFiyati01);
                            dToplamGyrListeFiyati += dSatisF;
                            gvGayrimenkulBilgileri.SetRowCellValue(i, "ListeFiyati", dSatisF);

                            int iDoviz = C.ParseInt(GYR.FirstOrDefault().SatisFiyati01DovizID);
                            gvGayrimenkulBilgileri.SetRowCellValue(i, "ListeFiyatiDovizID", iDoviz);
                            gvGayrimenkulBilgileri.SetRowCellValue(i, "SatisFiyatiDovizID", iDoviz);

                            string strDoviz = Genel.DovizKoduGetir(iDoviz);
                            gvGayrimenkulBilgileri.SetRowCellValue(i, "ListeFiyatiDovizKodu", strDoviz);

                            gvGayrimenkulBilgileri.SetRowCellValue(i, "KdvDurumuKisID_26", GYR.FirstOrDefault().SatisFiyati01KdvDurumuKisID_26);
                        }
                    }



                    //alttaki iki satırın eklenme amacı : fiyatsız olarak eklenen ve rezerve olarak kaydedilen aktivitelerde, aktivite satısa cekildigi an 
                    //GayrimenkulFiyatlariYenile(true) olarak calısır yani burası...burada da gayrimenkullerin fiyatlarını, aktivite üzerindeki satıs ve liste fiyatlarını düzeltir...
                    ToplamOdemeHesapla();
                    Validation();


                }
                else
                {
                    int iGyrID = C.ParseInt(gvGayrimenkulBilgileri.GetFocusedRowCellValue("GayrimenkulID"));
                    if (iGyrID > 0)
                    {
                        var GYR = Genel.PrmDb.S_GayrimenkulID(iGyrID, Genel.DilID).ToList();
                        decimal dSatisF = C.ParseDecimal(GYR.FirstOrDefault().SatisFiyati01);
                        dToplamGyrListeFiyati += dSatisF;
                        gvGayrimenkulBilgileri.SetFocusedRowCellValue("ListeFiyati", dSatisF);

                        int iDoviz = C.ParseInt(GYR.FirstOrDefault().SatisFiyati01DovizID);
                        gvGayrimenkulBilgileri.SetFocusedRowCellValue("ListeFiyatiDovizID", iDoviz);
                        gvGayrimenkulBilgileri.SetFocusedRowCellValue("SatisFiyatiDovizID", iDoviz);

                        string strDoviz = Genel.DovizKoduGetir(iDoviz);
                        gvGayrimenkulBilgileri.SetFocusedRowCellValue("ListeFiyatiDovizKodu", strDoviz);

                        gvGayrimenkulBilgileri.SetFocusedRowCellValue("KdvDurumuKisID_26", GYR.FirstOrDefault().SatisFiyati01KdvDurumuKisID_26);
                    }

                }



            }
            catch (Exception Hata)
            {
                Genel.LogErrorYaz(iFormID, Hata);
                XtraMessageBox.Show(Genel.DilGetirMesaj(1034) + "\n\n" + Hata.Message, Genel.DilGetirMesaj(2), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void txtFarkOdeme_DoubleClick(object sender, EventArgs e)
        {
            if (C.ParseDecimal(txtFarkOdeme.EditValue) < 0)
                txtPesinat1.EditValue = C.ParseDecimal(txtPesinat1.EditValue) + C.ParseDecimal(txtFarkOdeme.EditValue);
            else if (C.ParseDecimal(txtFarkOdeme.EditValue) > 0)
            {

                if (C.ParseInt(cmbOdemeBicimiAna.EditValue) == 312) //peşin + banka kredisi
                    txtBankaKredisi.EditValue = C.ParseDecimal(txtBankaKredisi.EditValue) + C.ParseDecimal(txtFarkOdeme.EditValue);
                else if (C.ParseInt(cmbOdemeBicimiAna.EditValue) == 313) //peşin + vadeli
                    txtVadeli.EditValue = C.ParseDecimal(txtVadeli.EditValue) + C.ParseDecimal(txtFarkOdeme.EditValue);
                else if (C.ParseInt(cmbOdemeBicimiAna.EditValue) == 314) //peşin + banka kredisi + vadeli
                {
                    if (C.ParseDecimal(txtVadeli.EditValue) == 0)
                        txtVadeli.EditValue = C.ParseDecimal(txtVadeli.EditValue) + C.ParseDecimal(txtFarkOdeme.EditValue);
                    else if (C.ParseDecimal(txtBankaKredisi.EditValue) == 0)
                        txtBankaKredisi.EditValue = C.ParseDecimal(txtBankaKredisi.EditValue) + C.ParseDecimal(txtFarkOdeme.EditValue);

                }
            }
        }

        private void bbtnTeklif_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gvMusteriBilgileri.DataRowCount < 1)
            {
                Ayar.InfoMesajGoster(Genel.DilGetirMesaj(3165), Ayar.MesajTipi.Uyari, this);
                this.Cursor = Cursors.Default;
                return;
            }

            string strGayrimenkulIDS = "";
            int iGayrimenkulID = 0;

            bool bTopluSatisMi = false;
            if (gvGayrimenkulBilgileri.DataRowCount > 1)
            {
                bTopluSatisMi = true;
            }
            else
                iGayrimenkulID = C.ParseInt(gvGayrimenkulBilgileri.GetRowCellValue(0, "GayrimenkulID"));


            string strMusteri = "";
            string strGayrimenkul = "";

            strGayrimenkul = "(" + PRJ.FirstOrDefault().ProjeAdi + ") ";



            for (int i = 0; i < gvGayrimenkulBilgileri.DataRowCount; i++)
            {
                string strBlokNo = gvGayrimenkulBilgileri.GetRowCellValue(i, "BlokNo").ToString();
                string strDaireNo = gvGayrimenkulBilgileri.GetRowCellValue(i, "DaireNo").ToString();
                if (strBlokNo.Length > 0 && strDaireNo.Length > 0)
                    strGayrimenkul += strBlokNo + " / " + strDaireNo + ",";
                else if (strBlokNo.Length == 0 && strDaireNo.Length > 0)
                    strGayrimenkul += strDaireNo + ",";
                else if (strBlokNo.Length > 0 && strDaireNo.Length == 0)
                    strGayrimenkul += strBlokNo + ",";

                string strG_ID = gvGayrimenkulBilgileri.GetRowCellValue(i, "GayrimenkulID").ToString();
                strGayrimenkulIDS += strG_ID + ",";
            }

            if (strGayrimenkul.Replace(PRJ.FirstOrDefault().ProjeAdi, "").Trim().Length > 0)
                strGayrimenkul = strGayrimenkul.Remove(strGayrimenkul.Length - 1, 1);

            if (strGayrimenkulIDS.Length > 0)
                strGayrimenkulIDS = strGayrimenkulIDS.Remove(strGayrimenkulIDS.Length - 1, 1);

            strMusteri = gvMusteriBilgileri.GetRowCellValue(0, "MusteriAdiSoyadi").ToString();

            var Modal = Ayar.FormGoster();
            F_Teklif_Liste frm = new F_Teklif_Liste(iAktiviteID);
            frm._FormReadOnly = bFormReadOnly;
            frm._SatisFiyati = C.ParseDecimal(txtListeFiyati.EditValue);
            frm._MusteriID = C.ParseInt(gvMusteriBilgileri.GetRowCellValue(0, "MusteriID"));
            frm._Musteri = strMusteri;
            frm._Gayrimenkul = strGayrimenkul;
            frm._GayrimenkulID = iGayrimenkulID;
            frm._GayrimenkulIDS = strGayrimenkulIDS;
            frm._TopluSatisMi = bTopluSatisMi;
            frm.ShowDialog();
            Modal.Close();
        }

        private void bbtnOnTeklif_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int iAnaMusteriID = C.ParseInt(gvMusteriBilgileri.GetRowCellValue(0, "MusteriID"));
            if (iAnaMusteriID < 1)
            {
                Ayar.InfoMesajGoster(Genel.DilGetirMesaj(3089), Ayar.MesajTipi.Hata, this);
                return;
            }

            
            F_AktiviteOnTeklif frm = new F_AktiviteOnTeklif(iAnaMusteriID);
            frm._ListeFiyati = C.ParseDecimal(txtListeFiyati.EditValue);
            frm._SatisFiyati = C.ParseDecimal(txtSatisFiyati.EditValue);
            frm._ToplamIndirim = C.ParseDecimal(txtHesaplananIndirim.EditValue) + C.ParseDecimal(txtTanimlananIndirim.EditValue);
            frm.ShowDialog();
        }

       







    }
}
