using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace UlkerTouchScreen
{
    public partial class QM_Edit : Form
    {
        public QM_Edit()
        {
            InitializeComponent();
        }
        //WebRfrQM_GetDetail.Y_FW0_QM_PROSES_GET_DETAILService WS_QM_GetDetail = new UlkerTouchScreen.WebRfrQM_GetDetail.Y_FW0_QM_PROSES_GET_DETAILService();
        WebRfr_QM_Process.Y_FW0_QM_PROSESService WS_QM_GetDetail = new UlkerTouchScreen.WebRfr_QM_Process.Y_FW0_QM_PROSESService();
        int VornKeepIndex = QM_AllData.SecilenVornIndex;
        bool SaatdeHata = false;
        bool HataAlincaDur = false;
        string CurrentSecond = "";
        int datagridRowNumber = 0;
        string[] imagelist;
        private static bool HataGoster = true;

        private void checkTSTime(string SAPHour, string SAPMin)
        {
            //try
            //{
            //    string SapSaati = QM_AllData.SAPTime + ":00";
            //    TimeSpan sapsaat = TimeSpan.Parse(SapSaati);
            //    TimeSpan bilgisayarsaat = TimeSpan.Parse(DateTime.Now.TimeOfDay.ToString());
            //    TimeSpan timefark= Int32.Parse(sapsaat.ToString())-Int32.Parse(bilgisayarsaat.ToString());
            //    if (Math.Abs(sapsaat - bilgisayarsaat) > )
            //        MessageBox.Show("Saatiniz SAP Saatinden farklidir, sistem yöneticinizle görüşüp düzelttiriniz", "Hata",
            //            MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //catch (Exception ex)
            //{

            //}           
        }

        private void QM_Edit_Load(object sender, EventArgs e)
        {
            //26.08.2010
            if (QM_AllData.UretimYeriWerks == "0781")
            {
                lbl_batch.Visible = true;
                txt_batch.Visible = true;
                txt_batch.Text = "";
            }
            else
            {
                lbl_batch.Visible = false;
                txt_batch.Visible = false;
            }

            //26.08.2010
            try
            {
                WebRfr_QM_Process.Y_FW0_QM_PROSESService srv = new UlkerTouchScreen.WebRfr_QM_Process.Y_FW0_QM_PROSESService();
                srv.Url = Utility.GetWSUrl("Y_FW0_QM_PROSES");
                string saat;
                QM_AllData.SAPDate = srv.YFw0QmTarihsaat(out saat);
                QM_AllData.SAPTime = saat;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SAP Saati alınırken hata!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            QM_AllData.SaveTimerSayac = 0;
            timerSave.Start();
            eskitarih = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy"));
            eskisaat = DateTime.Now.ToString("HH:mm");
            WS_QM_GetDetail.Url = Utility.GetWSUrl("Y_FW0_QM_PROSES");
            // koray yeni düzenleme yetki yok ise tarih saat değişemez.
            // PP_YM_Teyit.AllD.user
            lbl_kontrol_no.Text = "";
            if (QM_AllData.TKontrolNoktasi.Length < 2)
            {
                btn_kn_ileri.Enabled = false;
                btn_kn_geri.Enabled = false;
            }
            if (QM_AllData.kontrolNoktasiSecilenIndex == 0)
            {
                btn_kn_geri.Enabled = false;
            }
            if (QM_AllData.kontrolNoktasiSecilenIndex == QM_AllData.TKontrolNoktasi.Length - 1)
            {
                btn_kn_ileri.Enabled = false;
            }
            dt_usert1.Enabled = true;
            dateTimePicker1.Enabled = true;
            //string tarihyetkisi = Login.AllD.user[0].Profile.Substring(19, 1).ToString();
            string tarihyetkisi = Login.AllD.user[0].Profile.Substring(18, 1).ToString();
            if (tarihyetkisi != "1")
            {
                dt_usert1.Enabled = true;
                dateTimePicker1.Enabled = true;
            }
            else
            {
                dt_usert1.Enabled = false;
                dateTimePicker1.Enabled = false;
            }

            //Nitelik Screen variablerini duzenle
            QM_AllData.ClearNitelikScreen();
            QM_AllData.ClearEditScreen();

            #region SonDuzeltmeler
            //ERDEM 18 ARALIK
            if (QM_AllData.E_Kontrol_Noktasi != "")
            {
                btnNext.Enabled = false;
                btnPrev.Enabled = false;
                btn_kn_ileri.Visible = true;
                btn_kn_geri.Visible = true;
                lbl_kontrol_no.Visible = true;
                label8.Visible = true;
                txbleridoldur();
            }
            else
            {
                btn_kn_ileri.Visible = false;
                btn_kn_geri.Visible = false;
                IslemIleriGeriTuslari();
                lbl_kontrol_no.Visible = false;
                label8.Visible = false;
            }
            //ERDEM 18 ARALIK
            #endregion

            try
            {
                // * 29032016
                // BISKOT Vardiya kaynaklı sorun için Gece 00:00 den sonra girilen sonuçlar için tarihi 1 arttır 
                // Gece 00:00 ile 07:00 vardiya başlangıcı arasında kontrol partisi yaratılmışsa arttırma.
                if (QM_AllData.UretimYeriWerks == "0381" ||
                     QM_AllData.UretimYeriWerks == "0382" ||
                     QM_AllData.UretimYeriWerks == "0383" ||
                     QM_AllData.UretimYeriWerks == "0384")
                {
                    DateTime PartiSaati = Convert.ToDateTime(QM_AllData.SecilenPartiSaati);
                    if ((PartiSaati.Hour >= 0) && (PartiSaati.Hour < 7))
                    {
                        // Kontrol Partisi Saat 00:00-07:00 aralığında yaratılmışsa arttırma
                    }
                    else
                    {
                        // Sistem Saati 00:00-07:00 aralığındaysa arttır
                        if ((DateTime.Now.Hour >= 0) && (DateTime.Now.Hour < 7))
                        {
                            dateTimePicker1.Value = Convert.ToDateTime(QM_AllData.SecilenPartiTarihi);
                            dateTimePicker1.Value = dateTimePicker1.Value.AddDays(1);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: BİSKOT vardiya 00:00-06:00 arası sonuç girişi kontrolü" + "Exception: " + ex.Message);
            }

        }

        private void IslemIleriGeriTuslari()
        {
            if (QM_AllData.Vorn1denFazla)
            {
                if (QM_AllData.SecilenVornIndex + 1 == QM_AllData.MaxVornIndex)
                {
                    btnPrev.Enabled = true;
                    btnNext.Enabled = false;
                }
                else if (QM_AllData.SecilenVornIndex == 0)
                {
                    btnPrev.Enabled = false;
                    btnNext.Enabled = true;
                }
                else
                {
                    btnPrev.Enabled = true;
                    btnNext.Enabled = true;
                }
                txbleridoldur();
            }
            else
            {
                btnPrev.Enabled = false;
                btnNext.Enabled = false;
                txbleridoldur();
            }
        }
        private void txbleridoldur()
        {
            try
            {
                kontrolNoktasiText();
                //txbSaat.Text = DateTime.Now.TimeOfDay.ToString().Substring(0, 2);
                //txbDakika.Text = DateTime.Now.TimeOfDay.ToString().Substring(3, 2);

                CurrentSecond = DateTime.Now.TimeOfDay.ToString().Substring(6, 2);
                dateTimePicker1.Value = DateTime.Parse(QM_AllData.SAPDate);
                //dt_usert1.Value = DateTime.Parse(QM_AllData.SAPDate + " " + QM_AllData.SAPTime);
                dt_usert1.Value = DateTime.Now;
                if (QM_AllData.Vorn1denFazla)
                    Filldatagrid(VornKeepIndex);
                else
                    Filldatagrid(0);
                //txb leri doldur
                //lblIslemAciklamasi.Text = QM_AllData.QM_IslemListesiDOLU[QM_AllData.SecilenVornIndex].Vorktxt;
                txbVornr.Text = QM_AllData.QM_IslemListesiDOLU[QM_AllData.SecilenVornIndex].Vornr;
                //txbTarih.Text = QM_AllData.WS_QM_HeaderDOLU[0].Pastrterm.ToString();
                //txbTarih.Text = txbTarih.Text.Substring(txbTarih.Text.Length - 2, 2) + "-" + txbTarih.Text.Substring(5, 2) + "-" + txbTarih.Text.Substring(0, 4);
                if (LBLIslem == false)
                {
                    lblIslemAciklamasi.Text += " " + QM_AllData.QM_IslemListesiDOLU[QM_AllData.SecilenVornIndex].Vorktxt;
                }
                txbMalzeme.Text = QM_AllData.WS_QM_HeaderDOLU[0].Matnr;
                txbIsyeri.Text = QM_AllData.WS_QM_HeaderDOLU[0].Arbpl;

                string Tarih = DateTime.Now.ToString("dd-MM-yyyy");
                string Saat = DateTime.Now.ToString("HH:mm:ss");
                string ss = Saat.Substring(0, 2);
                string VardiyaSaati = "";
                try
                {
                    // BISKOT SORUNU *
                    if (QM_AllData.TVardiya.Length > 0)
                    {
                        VardiyaSaati = QM_AllData.TVardiya[0].VardiyaSaat.Substring(0, 2);
                        int vardiyasaati = int.Parse(VardiyaSaati);
                        int x = int.Parse(ss);
                        if (VardiyaSaati != "00" && VardiyaSaati != "24")
                        {
                            if (x < vardiyasaati)
                            {
                                //// Gece 00:00 sonrası yeni tarih ile kontrol partisi girişine devam edilsin.
                                //if ( QM_AllData.UretimYeriWerks != "0381" &&
                                //     QM_AllData.UretimYeriWerks != "0382" &&
                                //     QM_AllData.UretimYeriWerks != "0383" &&
                                //     QM_AllData.UretimYeriWerks != "0384" &&
                                //     QM_AllData.UretimYeriWerks != "0385" )
                                //{
                                dateTimePicker1.Value = DateTime.Parse(QM_AllData.TVardiya[0].SapDate);//dateTimePicker1.Value = DateTime.Now;
                                dateTimePicker1.Value = dateTimePicker1.Value.AddDays(-1);
                                //}
                            }
                        }
                    }
                }
                catch
                {

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Metin alanları doldurulurken beklenmeyen bir hata oluştu!...", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }

        }
        private void kontrolNoktasiText()
        {
            try
            {
                lbl_kontrol_no.Text = "";
                if (QM_AllData.TKontrolNoktasi.Length == 0)
                {
                    lbl_kontrol_no.Visible = false;
                    return;
                }
                if (QM_AllData.TKontrolNoktasi[QM_AllData.kontrolNoktasiSecilenIndex].Userc1 != "")
                {
                    lbl_kontrol_no.Text = QM_AllData.TKontrolNoktasi[QM_AllData.kontrolNoktasiSecilenIndex].Userc1;
                }
                if (QM_AllData.TKontrolNoktasi[QM_AllData.kontrolNoktasiSecilenIndex].Userc2 != "")
                {
                    lbl_kontrol_no.Text += "/" + QM_AllData.TKontrolNoktasi[QM_AllData.kontrolNoktasiSecilenIndex].Userc2;
                }
                if (QM_AllData.TKontrolNoktasi[QM_AllData.kontrolNoktasiSecilenIndex].Usern1 != "")
                {
                    lbl_kontrol_no.Text += " /" + QM_AllData.TKontrolNoktasi[QM_AllData.kontrolNoktasiSecilenIndex].Usern1;
                }
                if (QM_AllData.TKontrolNoktasi[QM_AllData.kontrolNoktasiSecilenIndex].Usern2 != "")
                {
                    lbl_kontrol_no.Text += " /" + QM_AllData.TKontrolNoktasi[QM_AllData.kontrolNoktasiSecilenIndex].Usern2;
                }
                if (QM_AllData.TKontrolNoktasi[QM_AllData.kontrolNoktasiSecilenIndex].Userd1 != "")
                {
                    lbl_kontrol_no.Text += "/" + QM_AllData.TKontrolNoktasi[QM_AllData.kontrolNoktasiSecilenIndex].Userd1;
                }
                if (QM_AllData.TKontrolNoktasi[QM_AllData.kontrolNoktasiSecilenIndex].Usert1 != "")
                {
                    lbl_kontrol_no.Text += "/" + QM_AllData.TKontrolNoktasi[QM_AllData.kontrolNoktasiSecilenIndex].Usert1;
                }
            }
            catch (Exception ex)
            {

            }
            //kontrol noktasi bilgilerini yazdır           
        }
        bool LBLIslem = false;
        private void btnPrev_Click(object sender, EventArgs e)
        {
            try
            {
                btnNext.Enabled = true;
                if (VornKeepIndex > 0)
                {
                    VornKeepIndex--;
                    lblIslemAciklamasi.Text = QM_AllData.QM_IslemListesiDOLU[VornKeepIndex].Vorktxt;
                    txbVornr.Text = QM_AllData.QM_IslemListesiDOLU[VornKeepIndex].Vornr;
                }

                if (VornKeepIndex == 0)
                    btnPrev.Enabled = false;
                Filldatagrid(VornKeepIndex);
                LBLIslem = false;
                SaatiDegistir();
            }
            catch (Exception ex)
            {

            }

        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                btnPrev.Enabled = true;
                if (VornKeepIndex < QM_AllData.MaxVornIndex)
                {
                    VornKeepIndex++;
                    lblIslemAciklamasi.Text = QM_AllData.QM_IslemListesiDOLU[VornKeepIndex].Vorktxt;
                    txbVornr.Text = QM_AllData.QM_IslemListesiDOLU[VornKeepIndex].Vornr;
                }
                if (VornKeepIndex + 1 == QM_AllData.MaxVornIndex)
                    btnNext.Enabled = false;
                Filldatagrid(VornKeepIndex);
                LBLIslem = false;
                SaatiDegistir();
            }
            catch (Exception EX)
            {

            }

        }

        public void Filldatagrid(int VornIndex)
        {
            ////////////
            //////////
            datagridRowNumber = 0;
            QM_AllData.temp = VornIndex;
            //ERDEM
            String DesiredTime = dt_usert1.Value.ToString().Substring(11, 5) + ":00";
            //ERDEM
            QM_AllData.DesiredTime = DesiredTime;
            WebRfr_QM_Process.Yfw0Return[] WS_QM_Return = new UlkerTouchScreen.WebRfr_QM_Process.Yfw0Return[0];
            string date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            //txb leri doldur
            if (QM_AllData.TKontrolNoktasi.Length == 0) //kontrol partisinin kontrol noktasi yoksa 
            {
                WebRfr_QM_Process.ZqmTesisproses021 nullKontrolNoktasi = new WebRfr_QM_Process.ZqmTesisproses021();
                QM_AllData.TKontrolNoktasi = new WebRfr_QM_Process.ZqmTesisproses021[1];
                QM_AllData.TKontrolNoktasi[0] = nullKontrolNoktasi;
            }
            try
            {
                string SAAT = DateTime.Now.ToString("HH:mm:ss");
                Cursor.Current = Cursors.WaitCursor;
                if (QM_AllData.E_Kontrol_Noktasi != "")
                {
                    WS_QM_GetDetail.YFw0QmProsesGetDetail(ref QM_AllData.WS_QM_HeaderDOLU, ref WS_QM_Return,
                        QM_AllData.CHKZorunlu, QM_AllData.TKontrolNoktasi[QM_AllData.kontrolNoktasiSecilenIndex].Insppoint,
                        QM_AllData.SecilenKontrolNo, QM_AllData.CHKOnceki, DesiredTime, QM_AllData.CHKSonraki, date,
                        QM_AllData.QM_IslemListesiDOLU[VornIndex].Vorglfnr, Login.AllD.wspassword,
                       Login.AllD.wssession, ref QM_AllData.QM_KarakteristikDOLU, Login.AllD.wsusercode,
                       Login.AllD.wsuserno);
                }
                else
                {
                    WS_QM_GetDetail.YFw0QmProsesGetDetail(ref QM_AllData.WS_QM_HeaderDOLU, ref WS_QM_Return,
                    QM_AllData.CHKZorunlu, "", QM_AllData.SecilenKontrolNo, QM_AllData.CHKOnceki, DesiredTime, QM_AllData.CHKSonraki, date,
                    QM_AllData.QM_IslemListesiDOLU[VornIndex].Vorglfnr, Login.AllD.wspassword,
                   Login.AllD.wssession, ref QM_AllData.QM_KarakteristikDOLU, Login.AllD.wsusercode,
                   Login.AllD.wsuserno);
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                return;
            }
            if (WS_QM_Return[0].RcCheck == "E")
            {
                if (HataGoster)
                {
                    MessageBox.Show(WS_QM_Return[0].RcText, "Hata", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    HataGoster = false;
                    //dateTimePicker1.Value = DateTime.Parse(dataGridView1[4, 0].Value.ToString());
                    dateTimePicker1.Value = DateTime.Parse(QM_AllData.SAPDate);
                }
            }
            else
            {

                //datagrid i Dolduruyo
                for (int i = dataGridView1.Rows.Count - 1; i != -1; i--)
                    dataGridView1.Rows.RemoveAt(i);


                int index = 0;
                imagelist = new string[QM_AllData.QM_KarakteristikDOLU.Length];
                foreach (WebRfr_QM_Process.ZqmTesisproses002 rows in QM_AllData.QM_KarakteristikDOLU)
                {
                    dataGridView1.Rows.Add();
                    String ICON = QM_AllData.QM_KarakteristikDOLU[index].Stat;
                    if (ICON == "1")
                    {
                        imagelist[index] = "x";
                        try
                        {
                            dataGridView1[0, index].Value = Image.FromFile("red.jpg");
                            goto resimEklerkenCatlamadanDevamEt;
                        }
                        catch (Exception)
                        {
                            goto resimEklerkenCatlamadanDevamEt;
                        }
                    }
                    else if (ICON == "2")
                    {
                        imagelist[index] = "!";
                        try
                        {
                            dataGridView1[0, index].Value = Image.FromFile("unlem.jpg");
                            goto resimEklerkenCatlamadanDevamEt;
                        }
                        catch (Exception)
                        {
                            goto resimEklerkenCatlamadanDevamEt;
                        }
                    }
                    else
                    {
                        imagelist[index] = "o";
                        try
                        {
                            dataGridView1[0, index].Value = Image.FromFile("onay.jpg");
                            goto resimEklerkenCatlamadanDevamEt;
                        }
                        catch (Exception)
                        {
                            goto resimEklerkenCatlamadanDevamEt;
                        }
                    }

                resimEklerkenCatlamadanDevamEt:
                    string year = QM_AllData.QM_KarakteristikDOLU[index].Pastrterm.Substring(0, 4);
                    string month = QM_AllData.QM_KarakteristikDOLU[index].Pastrterm.Substring(5, 2);
                    string day = QM_AllData.QM_KarakteristikDOLU[index].Pastrterm.Substring(8, 2);
                    string formatteddate = day + "-" + month + "-" + year;

                    dataGridView1[1, index].Value = QM_AllData.QM_KarakteristikDOLU[index].Kurxtext;
                    dataGridView1[2, index].Value = QM_AllData.QM_KarakteristikDOLU[index].Time.ToString().Substring(0, 5);
                    dataGridView1[3, index].Value = QM_AllData.QM_KarakteristikDOLU[index].Deger.ToString().Trim();
                    dataGridView1[4, index].Value = formatteddate;

                    if (QM_AllData.QM_KarakteristikDOLU[index].Nortip == "X")
                    {
                        DataGridViewCellStyle renk = new DataGridViewCellStyle();
                        renk.BackColor = Color.Red;
                        dataGridView1.Rows[index].DefaultCellStyle = renk;
                    }

                    datagridRowNumber++;
                    index++;
                };

                Cursor.Current = Cursors.Default;
                if (dataGridView1.Rows.Count == 0)
                    btnDevam.Enabled = false;
                else
                    btnDevam.Enabled = true;
            }
        }

        private void btnDevam_Click(object sender, EventArgs e)
        {
            try
            {
                QM_AllData.Hero_Batch = txt_batch.Text;
                string a = imagelist[dataGridView1.CurrentCell.RowIndex];
                if (dataGridView1.CurrentCell.RowIndex == -1)
                    MessageBox.Show("Seçim Yapınız", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                else if (a != "o")
                    MessageBox.Show("Secilebilir Alan Degil", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    try
                    {
                        WebRfr_QM_Process.Y_FW0_QM_PROSESService srv = new UlkerTouchScreen.WebRfr_QM_Process.Y_FW0_QM_PROSESService();
                        srv.Url = Utility.GetWSUrl("Y_FW0_QM_PROSES");
                        string saat;
                        QM_AllData.SAPDate = srv.YFw0QmTarihsaat(out saat);
                        //aydın beyin talebi ile 0251 üretim yerinde sap den gelen saatin öncesinde giriş yapılamaz kısıtlaması getirildi.
                        if (QM_AllData.UretimYeriWerks == "0251")
                        {
                            DateTime sapdengelensaat = Convert.ToDateTime(saat);
                            DateTime elilegirilensaat = Convert.ToDateTime(dt_usert1.Text);
                            if (sapdengelensaat.Hour != elilegirilensaat.Hour)
                            {
                                MessageBox.Show("Bu saat değeriyle giriş yapamazsınız.", "HATA");
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "SAP Saati alınırken hata!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    }
                    QM_AllData.SonMerknr2 = QM_AllData.SonMerknr; //Aynı karakteristik için yeniden çağırmamasını sağlıyor
                    QM_AllData.SecilenKarakteristik = dataGridView1.CurrentCell.RowIndex;
                    QM_AllData.SonMerknr = QM_AllData.QM_KarakteristikDOLU[QM_AllData.SecilenKarakteristik].Merknr;
                    QM_AllData.SecilenKarakteristikName = dataGridView1[1, dataGridView1.CurrentCell.RowIndex].Value.ToString();
                    QM_AllData.QM_KarakteristikDOLU[QM_AllData.SecilenKarakteristik].Pastrterm = dateTimePicker1.Value.ToString("yyyy-MM-dd"); //BERK 10-01-09
                    timerSave.Stop();
                    if (QM_AllData.UretimYeriWerks == "0781")
                    {
                        QM_Third781 frm = new QM_Third781();
                        frm.ShowDialog();
                    }
                    else
                    {
                        QM_Third frm = new QM_Third();
                        frm.ShowDialog();
                    }

                    timerSave.Start();

                }
            }
            catch (Exception ex)
            {
                QM_AllData.SecilenKarakteristik = 0;
                QM_AllData.SecilenKarakteristikName = dataGridView1[1, 0].Value.ToString();

                //QM_Third781 frm = new QM_Third781();
                //frm.ShowDialog();
                if (QM_AllData.UretimYeriWerks == "0781")
                {
                    QM_Third781 frm = new QM_Third781();
                    frm.ShowDialog();
                }
                else
                {
                    QM_Third frm = new QM_Third();
                    frm.ShowDialog();
                }
            }

        }
        bool Activatex = true;
        public void FillCustomdatagrid(int VornIndex)
        {
            QM_AllData.temp = VornIndex;
            datagridRowNumber = 0;

            WebRfr_QM_Process.Yfw0Return[] WS_QM_Return = new UlkerTouchScreen.WebRfr_QM_Process.Yfw0Return[0];
            //txb leri doldur
            try
            {
                WS_QM_GetDetail.Url = Utility.GetWSUrl("Y_FW0_QM_PROSES");
                Cursor.Current = Cursors.WaitCursor;
                //ERDEM
                //String DesiredTime = txbSaat.Text + ":" + txbDakika.Text + ":" + CurrentSecond;

                string DesiredTime = "";
                string date = "";
                try
                {
                    DesiredTime = dt_usert1.Value.ToString().Substring(11, 5) + ":00";
                    //ERDEM
                    date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                }
                catch (Exception ex)
                {
                }

                if (QM_AllData.E_Kontrol_Noktasi != "")
                {
                    WS_QM_GetDetail.YFw0QmProsesGetDetail(ref QM_AllData.WS_QM_HeaderDOLU, ref WS_QM_Return,
                        QM_AllData.CHKZorunlu, QM_AllData.TKontrolNoktasi[QM_AllData.kontrolNoktasiSecilenIndex].Insppoint,
                        QM_AllData.SecilenKontrolNo, QM_AllData.CHKOnceki, DesiredTime, QM_AllData.CHKSonraki, date,
                        QM_AllData.QM_IslemListesiDOLU[VornIndex].Vorglfnr, Login.AllD.wspassword,
                        Login.AllD.wssession, ref QM_AllData.QM_KarakteristikDOLU, Login.AllD.wsusercode,
                        Login.AllD.wsuserno);
                }
                else
                {
                    HataAlincaDur = false;
                    WS_QM_GetDetail.YFw0QmProsesGetDetail(ref QM_AllData.WS_QM_HeaderDOLU, ref WS_QM_Return,
                    QM_AllData.CHKZorunlu, "", QM_AllData.SecilenKontrolNo, QM_AllData.CHKOnceki, DesiredTime,
                    QM_AllData.CHKSonraki, date, QM_AllData.QM_IslemListesiDOLU[VornIndex].Vorglfnr, Login.AllD.wspassword,
                    Login.AllD.wssession, ref QM_AllData.QM_KarakteristikDOLU, Login.AllD.wsusercode,
                    Login.AllD.wsuserno);
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                HataAlincaDur = true;
                MessageBox.Show("Hata RFC:YFw0QmProsesGetDetail " + ex.Message);
                return;
            }
            //if (QM_AllData.QM_KarakteristikDOLU.Length == 0)
            //{
            //    //MessageBox.Show("Bu Tarih/Saate Sonuç Girişi Yapılamaz.", "Hata");
            //    return;
            //}
            try
            {
                Cursor.Current = Cursors.Default;
                if (WS_QM_Return[0].RcCheck == "E")
                {
                    if (HataGoster)
                    {
                        MessageBox.Show(WS_QM_Return[0].RcText, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        HataGoster = false;
                        //datagridview icin
                        //dateTimePicker1.Value = DateTime.Parse(dataGridView1[4, 0].Value.ToString());
                        dateTimePicker1.Value = DateTime.Parse(QM_AllData.SAPDate);
                    }
                }
                else
                {
                    String ICON = "";
                    int index = 0;
                    if (dataGridView1.Rows.Count > 0)
                    {
                        if (QM_AllData.QM_KarakteristikDOLU.Length == 0)
                        {
                            //dateTimePicker1.Value = DateTime.Parse(eskitarih.ToString("dd-MM-yyyy"));
                            //dt_usert1.Value = DateTime.Parse(eskisaat);
                            //MessageBox.Show("Bu Tarih/Saate Sonuç Girişi Yapılamaz.", "Hata");
                            label9.Visible = true;
                            //return;
                        }
                        else
                        {
                            label9.Visible = false;
                        }
                    }

                    for (int i = dataGridView1.Rows.Count - 1; i != -1; i--)
                        dataGridView1.Rows.RemoveAt(i);
                    imagelist = new string[QM_AllData.QM_KarakteristikDOLU.Length];
                    foreach (WebRfr_QM_Process.ZqmTesisproses002 rows in QM_AllData.QM_KarakteristikDOLU)
                    {
                        dataGridView1.Rows.Add();
                        ICON = QM_AllData.QM_KarakteristikDOLU[index].Stat;
                        if (ICON == "1")
                        {
                            imagelist[index] = "x";
                            try
                            {
                                dataGridView1[0, index].Value = Image.FromFile("red.jpg");
                                goto resimEkleyemezseDevamEt;
                            }
                            catch (Exception)
                            {
                                goto resimEkleyemezseDevamEt;
                            }
                        }
                        else if (ICON == "2")
                        {
                            imagelist[index] = "!";
                            try
                            {
                                dataGridView1[0, index].Value = Image.FromFile("unlem.jpg");
                                goto resimEkleyemezseDevamEt;
                            }
                            catch (Exception)
                            {
                                goto resimEkleyemezseDevamEt;
                            }
                        }
                        else
                        {
                            imagelist[index] = "o";
                            try
                            {
                                dataGridView1[0, index].Value = Image.FromFile("onay.jpg");
                                goto resimEkleyemezseDevamEt;
                            }
                            catch (Exception)
                            {
                                goto resimEkleyemezseDevamEt;
                            }

                        }

                    resimEkleyemezseDevamEt: //yukarıdaki resim alanlarını ekleyemediği zaman patlıyordu daha sonra detaylı bakacağım. -mcy 19.04.2017
                        if (QM_AllData.UretimYeriWerks == "0271") //3.5.2017 -mcy
                        {
                            //DateTime PartiSaati = Convert.ToDateTime(QM_AllData.SecilenPartiSaati);
                            //if ((PartiSaati.Hour >= 0) && (PartiSaati.Hour < 8))
                            if ((DateTime.Now.Hour >= 0) && (DateTime.Now.Hour < 8))
                            {
                                DateTime d = Convert.ToDateTime(QM_AllData.QM_KarakteristikDOLU[index].Pastrterm);
                                string yil = d.ToString().Substring(6, 4);
                                string ay = d.ToString().Substring(3, 2);
                                string gun = d.AddDays(-1).ToString().Substring(0, 2);
                                string date = gun + "-" + ay + "-" + yil;
                                dataGridView1[1, index].Value = QM_AllData.QM_KarakteristikDOLU[index].Kurxtext;
                                dataGridView1[2, index].Value = QM_AllData.QM_KarakteristikDOLU[index].Time.ToString().Substring(0, 5);
                                dataGridView1[3, index].Value = QM_AllData.QM_KarakteristikDOLU[index].Deger.ToString().Trim();
                                dataGridView1[4, index].Value = date;
                            }
                            else
                            {
                                string year = QM_AllData.QM_KarakteristikDOLU[index].Pastrterm.Substring(0, 4);
                                string month = QM_AllData.QM_KarakteristikDOLU[index].Pastrterm.Substring(5, 2);
                                string day = QM_AllData.QM_KarakteristikDOLU[index].Pastrterm.Substring(8, 2);
                                string date = day + "-" + month + "-" + year;
                                dataGridView1[1, index].Value = QM_AllData.QM_KarakteristikDOLU[index].Kurxtext;
                                dataGridView1[2, index].Value = QM_AllData.QM_KarakteristikDOLU[index].Time.ToString().Substring(0, 5);
                                dataGridView1[3, index].Value = QM_AllData.QM_KarakteristikDOLU[index].Deger.ToString().Trim();
                                dataGridView1[4, index].Value = date;
                            }
                        }
                        else
                        {
                            string year = QM_AllData.QM_KarakteristikDOLU[index].Pastrterm.Substring(0, 4);
                            string month = QM_AllData.QM_KarakteristikDOLU[index].Pastrterm.Substring(5, 2);
                            string day = QM_AllData.QM_KarakteristikDOLU[index].Pastrterm.Substring(8, 2);
                            string date = day + "-" + month + "-" + year;
                            dataGridView1[1, index].Value = QM_AllData.QM_KarakteristikDOLU[index].Kurxtext;
                            dataGridView1[2, index].Value = QM_AllData.QM_KarakteristikDOLU[index].Time.ToString().Substring(0, 5);
                            dataGridView1[3, index].Value = QM_AllData.QM_KarakteristikDOLU[index].Deger.ToString().Trim();
                            dataGridView1[4, index].Value = date;
                        }

                        if (QM_AllData.QM_KarakteristikDOLU[index].Nortip == "X")
                        {
                            DataGridViewCellStyle renk = new DataGridViewCellStyle();
                            renk.BackColor = Color.Red;
                            dataGridView1.Rows[index].DefaultCellStyle = renk;
                        }

                        datagridRowNumber++;
                        index++;
                    };
                    Cursor.Current = Cursors.Default;
                    if (dataGridView1.Rows.Count == 0)
                        btnDevam.Enabled = false;
                    else
                        btnDevam.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "HATA!", MessageBoxButtons.OK, MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //private void QM_Edit_Activated(object sender, EventArgs e)
        //{
        //   

        //}

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            SaatiDegistir();
        }
        private void SaatiDegistir()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                //if (Convert.ToInt32(txbSaat.Text) > 23 || Convert.ToInt32(txbDakika.Text) > 59)
                //{
                //    SaatdeHata = true;
                //    Cursor.Current = Cursors.Default;
                //    MessageBox.Show("Gecerli Saat Degil", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);

                //}
                //else
                //{
                HataGoster = true;
                if (QM_AllData.Vorn1denFazla)
                    FillCustomdatagrid(VornKeepIndex);
                else
                    FillCustomdatagrid(0);
                //}
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {

            }

        }

        DateTime eskitarih;
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            label9.Visible = false;
            Cursor.Current = Cursors.WaitCursor;
            //if (Convert.ToInt32(txbSaat.Text) > 23 || Convert.ToInt32(txbDakika.Text) > 59)
            //{
            //    SaatdeHata = true;
            //    Cursor.Current = Cursors.Default;
            //    MessageBox.Show("Gecerli Saat Degil", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);

            //}
            //else
            //{

            HataGoster = true;
            if (QM_AllData.Vorn1denFazla)
                FillCustomdatagrid(VornKeepIndex);
            else
                FillCustomdatagrid(0);
            //}
            Cursor.Current = Cursors.Default;
        }

        private void dataGrid1_DoubleClick(object sender, EventArgs e)
        {

        }

        private void dataGrid1_MouseHover(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Hand;
        }

        private void dataGrid1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                string a = imagelist[dataGridView1.CurrentCell.RowIndex];
                if (dataGridView1.CurrentCell.RowIndex == -1)
                    MessageBox.Show("Seçim Yapınız", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                else if (a != "o")
                    MessageBox.Show("Secilebilir Alan Degil", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    try
                    {
                        WebRfr_QM_Process.Y_FW0_QM_PROSESService srv = new UlkerTouchScreen.WebRfr_QM_Process.Y_FW0_QM_PROSESService();
                        srv.Url = Utility.GetWSUrl("Y_FW0_QM_PROSES");
                        string saat;
                        QM_AllData.SAPDate = srv.YFw0QmTarihsaat(out saat);
                        //aydın beyin talebi ile 0251 üretim yerinde sap den gelen saatin öncesinde giriş yapılamaz kısıtlaması getirildi. 20.01.2017 -mcy
                        if (QM_AllData.UretimYeriWerks == "0251")
                        {
                            DateTime sapdengelensaat = Convert.ToDateTime(saat);
                            DateTime elilegirilensaat = Convert.ToDateTime(dt_usert1.Text);
                            if (sapdengelensaat.Hour != elilegirilensaat.Hour)
                            {
                                MessageBox.Show("Bu saat değeriyle giriş yapamazsınız.", "HATA");
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "SAP Saati alınırken hata!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    }
                    QM_AllData.SonMerknr2 = QM_AllData.SonMerknr; //Aynı karakteristik için yeniden çağırmamasını sağlıyor
                    QM_AllData.SecilenKarakteristik = dataGridView1.CurrentCell.RowIndex;
                    QM_AllData.SonMerknr = QM_AllData.QM_KarakteristikDOLU[QM_AllData.SecilenKarakteristik].Merknr;
                    QM_AllData.SecilenKarakteristikName = dataGridView1[1, dataGridView1.CurrentCell.RowIndex].Value.ToString();
                    QM_AllData.QM_KarakteristikDOLU[QM_AllData.SecilenKarakteristik].Pastrterm = dateTimePicker1.Value.ToString("yyyy-MM-dd"); //BERK 10-01-09
                    timerSave.Stop();
                    if (QM_AllData.UretimYeriWerks == "0781")
                    {
                        QM_Third781 frm = new QM_Third781();
                        frm.ShowDialog();
                    }
                    else
                    {
                        QM_Third frm = new QM_Third();
                        frm.ShowDialog();
                    }
                    timerSave.Start();

                }
            }
            catch (Exception ex)
            {

            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dt_usert1_ValueChanged(object sender, EventArgs e)
        {
            label9.Visible = false;
            SaatiDegistir();
        }

        private void dateTimePicker1_Enter(object sender, EventArgs e)
        {

            //MessageBox.Show(eskitarih.ToString());
        }

        private void btn_kn_ileri_Click(object sender, EventArgs e)
        {
            if (QM_AllData.kontrolNoktasiSecilenIndex == QM_AllData.TKontrolNoktasi.Length - 2) //sondan bir önceki
            {
                btn_kn_ileri.Enabled = false;
            }
            QM_AllData.kontrolNoktasiSecilenIndex++;
            btn_kn_geri.Enabled = true;
            LBLIslem = true;
            txbleridoldur();
        }

        private void btn_kn_geri_Click(object sender, EventArgs e)
        {
            try
            {
                if (QM_AllData.kontrolNoktasiSecilenIndex == 1) //baştan 1.
                {
                    btn_kn_geri.Enabled = false;
                }
                QM_AllData.kontrolNoktasiSecilenIndex--;
                btn_kn_ileri.Enabled = true;
                LBLIslem = true;
                txbleridoldur();
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_kn_ileri_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (QM_AllData.kontrolNoktasiSecilenIndex == QM_AllData.TKontrolNoktasi.Length - 2) //sondan bir önceki
                {
                    btn_kn_ileri.Enabled = false;
                }
                QM_AllData.kontrolNoktasiSecilenIndex++;
                btn_kn_geri.Enabled = true;
                LBLIslem = true;
                txbleridoldur();
            }
            catch (Exception ex)
            {

            }

        }

        private void dateTimePicker1_Enter_1(object sender, EventArgs e)
        {
            eskitarih = dateTimePicker1.Value;
        }

        private void lblIslemAciklamasi_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            QM_AllData.KontrolPartisindenCik = true;
            this.Close();
        }

        string eskisaat;
        private void dt_usert1_Enter(object sender, EventArgs e)
        {
            eskisaat = dt_usert1.Value.ToString("HH:mm");
            //MessageBox.Show(eskisaat.ToString());
        }

        private void dt_usert1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void dt_usert1_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Down)
            //{
            //    SaatiDegistir();
            //}
            //else if (e.KeyCode == Keys.Up)
            //{
            //    SaatiDegistir();
            //}

        }

        private void dt_usert1_KeyUp(object sender, KeyEventArgs e)
        {
            //SaatiDegistir();
        }

        private void dt_usert1_MouseUp(object sender, MouseEventArgs e)
        {
            //SaatiDegistir();
        }
        int SonucGirmeZamanAsimi = 0;
        private void timerSave_Tick(object sender, EventArgs e)
        {
            try
            {
                if (QM_AllData.TVardiya.Length != 0)
                {
                    SonucGirmeZamanAsimi = 60 * int.Parse(QM_AllData.TVardiya[0].SonucGirmeDak.ToString());
                    QM_AllData.SaveTimerSayac++;
                    //kalan = 60 - QM_AllData.SaveTimerSayac;
                    //kalan2 = 100 - QM_AllData.SaveTimerSayac2;
                    //textBox1.Text = kalan.ToString() + " " + kalan2.ToString();
                    if (QM_AllData.SaveTimerSayac >= SonucGirmeZamanAsimi)
                    {
                        QM_AllData.SaveTimerSayac = 0;
                        QM_AllData.FormzamanAsimi = true;
                        timerSave.Stop();
                        MessageBox.Show("Sonuç Girme İşlemi Zaman Aşımına Uğradı,Ekran Kapatılacak!...", "Bilgi", MessageBoxButtons.OK,
                        MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void QM_Edit_Activated(object sender, EventArgs e)
        {
            try
            {
                if (QM_AllData.FormzamanAsimi == true)
                {
                    QM_AllData.FormzamanAsimi = false;
                    this.Close();
                }
                else
                {

                    if (QM_AllData.KontrolPartiKapatildi)
                    {
                        this.Close();
                    }
                    else
                    {
                        //if (Convert.ToInt32(txbSaat.Text) > 23 || Convert.ToInt32(txbDakika.Text) > 59)
                        //{
                        //txbSaat.Text = DateTime.Now.TimeOfDay.ToString().Substring(0, 2);
                        //txbDakika.Text = DateTime.Now.TimeOfDay.ToString().Substring(3, 2);
                        //}
                        //erdem                        
                        //ERDEM
                        QM_AllData.ClearNitelikScreen();
                        if (!SaatdeHata)
                        {
                            HataGoster = false;
                            if (HataAlincaDur != true)
                            {
                                if (QM_AllData.Vorn1denFazla)
                                    FillCustomdatagrid(VornKeepIndex);
                                else
                                    FillCustomdatagrid(0);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void QM_Edit_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void QM_Edit_FormClosing(object sender, FormClosingEventArgs e)
        {
            timerSave.Stop();
        }

        private void txbVornr_TextChanged(object sender, EventArgs e)
        {

        }

        private void radPivotGrid1_Click(object sender, EventArgs e)
        {

        }





    }
}