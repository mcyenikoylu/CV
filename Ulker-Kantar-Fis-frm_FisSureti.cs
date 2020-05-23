using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace Kantar
{
    public partial class frm_FisSureti : Form
    {
        public frm_FisSureti()
        {
            InitializeComponent();
        }
        private AppSettingsReader reader;
        public int gcid;
        public string sfis_no = "";
        public string sgiris_tar = "";
        public string splaka = "";
        public string ssurucu_ad_soyad = "";
        public string scep_tel = "";
        public string stc_no = "";
        public string smalzeme_kodu = "";
        public string ssehir = "";
        public string sfirma = "";
        public string sdurum = "";
        public string sgiris_kullanici = "";
        public string sgiris_kg = "";
        public string scikis_tar = "";
        public string scikis_kullanici = "";
        public string scikis_kg = "";
        public string snet_kg = "";       
        public string sirsaliye_no = "";
        public string sirsaliye_tar = "";
        public string durum = "";
        public string dorse = "";
        public string kon1 = "";
        public string kon2= "";
        public string kon3= "";
        public string kon4 = "";
        public bool getKg = false;
        DBClass mydb = new DBClass();
        private void frm_FisSureti_Load(object sender, EventArgs e)
        {
            txt_cep.Text = scep_tel;
            txt_cikis_kg.Text = scikis_kg;
            txt_cikis_kullanici.Text = scikis_kullanici;
            txt_cikis_tar.Text = scikis_tar;
            txt_durum.Text = sdurum;
            txt_firma.Text = sfirma;
            txt_fis_no.Text = sfis_no;
            txt_giris_kg.Text = sgiris_kg;
            txt_giris_tar.Text = sgiris_tar;
            txt_KantarNo.Text = PublicVariables.kantar_no;
            txt_giris_kullanici.Text = sgiris_kullanici;
            txt_LojistikMerkezi.Text = PublicVariables.uretim_yeri;
            txt_malzeme_kodu.Text = smalzeme_kodu;
            if (snet_kg != "")
            {
                txt_net_kg.Text = Convert.ToString(Math.Abs(Convert.ToInt64(snet_kg)));
            }
            txt_plaka.Text = splaka;
            txt_sehir.Text = ssehir;
            txt_surucu_ad_soyad.Text = ssurucu_ad_soyad;
            txt_tcno.Text = stc_no;
            lbl_IrsaliyeNo.Text = sirsaliye_no;
            lbl_IrsaliyeTar.Text = sirsaliye_tar;

            if (durum == "G")
            {
                lbl_fis_status.Text = "Kantar Giriþ Fiþi";
                groupBoxcikis.Visible = false;
                btn_fis.Location = new Point(133,440);
                this.Size = new Size(402, 510);
            }
            else
            {
                lbl_fis_status.Text = "Kantar Çýkýþ Fiþi";
                groupBoxcikis.Visible = true;
                btn_fis.Location = new Point(50, 559);
                btn_kapat.Location = new Point(200, 559);   
                this.Size = new Size(402, 625);
                DataTable dt = new DataTable();
                if (getKg == true)
                {
                    if (mydb.getCikisKgForFis(gcid, ref dt) == 0)
                    {
                        scikis_kg = dt.Rows[0]["CIKIS_KG"].ToString();
                        txt_cikis_kg.Text = scikis_kg;
                        snet_kg = Convert.ToString(int.Parse(scikis_kg) - int.Parse(sgiris_kg));
                        txt_net_kg.Text = snet_kg;
                    }
                    else
                    {
                        MessageBox.Show("Cýkýþ kilogram bilgisi alýnamadý.", "Veritabaný Hatasý", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btn_fis_Click(object sender, EventArgs e)
        {
            printDocument1.DocumentName = "Fis Sureti";
            //printDialog1.Document = printDocument1;
            //if (printDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    makePrintStr();
            //    printDocument1.Print();
            //}
            
            makePrintStr();
            System.Drawing.Printing.PaperSize ps = new System.Drawing.Printing.PaperSize("new",400,PublicVariables.kagit_boyu);
            System.Drawing.Printing.PrinterResolution pr = new System.Drawing.Printing.PrinterResolution();
            pr.Kind = System.Drawing.Printing.PrinterResolutionKind.Draft;
            
            printDocument1.DefaultPageSettings.PaperSize = ps;
            printDocument1.DefaultPageSettings.PrinterResolution = pr;            
            printDocument1.Print();
                     
            //string s = RawPrinterHelper.GetDefaultPrinterName();

            //RawPrinterHelper.SendStringToPrinter(s, print_str);
            if (durum == "G")
            {
                this.Close();
            }
            else { }

        }
       
        private void makePrintStr()
        {
            if (durum == "G")
            {
                print_str = "";
                reader = new AppSettingsReader();
                string tic_unvan = reader.GetValue("TicariUnvan", typeof(string)).ToString();
                //string adres = reader.GetValue("Adres", typeof(string)).ToString();
                //string tel = reader.GetValue("Telefon", typeof(string)).ToString();
                
                print_str += "\n " + tic_unvan + " ";
                //print_str += "          tel: " + tel + " \n\n";
                print_str += "\n             Kantar Giriþ Fiþi " + "\n";
                //print_str += "Üretim Yeri              :  " + PublicVariables.uretim_yeri + "\n";
                print_str += "\nFiþ Numarasý          :  " + sfis_no + "\n";
                print_str += "Kantar No                :  " + PublicVariables.kantar_no + "\n";
                print_str += "Fiþ Tarihi                 :  " + DateTime.Now.ToShortDateString() + "\n";
                print_str += "Fiþ Saati                   :  " + DateTime.Now.ToShortTimeString() + "\n";
                print_str += "----------------------------------------" + "\n";
                print_str += "Plaka                        :  " + splaka + "\n";
                print_str += "Dorse      :  " + dorse + "        Knt    :" + kon1 + " " + kon2 + " " + kon3 + " " + kon4 + "\n";
                print_str += "Sürücü Adý               :  " + ssurucu_ad_soyad + "\n";
                print_str += "Cep Telefonu           :  " + scep_tel + "\n";
                //print_str += "Giriþ Durumu          :  " + sdurum + "\n";
                //print_str += "Tc Kimlik No            :  " + stc_no + "\n";
                print_str += "Malzeme                  :  " + smalzeme_kodu + "\n";
                print_str += "Firma                        :  " + sfirma + "\n";
                print_str += "Þehir                         :  " + ssehir + "\n";
                print_str += "Ýrsaliye Tarihi           :  " + sirsaliye_tar + "\n";
                print_str += "Ýrsaliye No                :  " + sirsaliye_no + "\n";
                print_str += "----------------------------------------" + "";
                print_str += "\n Tartým Tarihi           :  " + sgiris_tar + "";
                print_str += "\n Tartým Aðýrlýðý         :  " + sgiris_kg + " Kg." ;
            }
            else
            {
                // !!! A D A N A !!!
                //print_str = "";
                //if (check_bilgi.Checked == true)
                //{
                //    reader = new AppSettingsReader();
                //    string tic_unvan = reader.GetValue("TicariUnvan", typeof(string)).ToString();
                //    string adres = reader.GetValue("Adres", typeof(string)).ToString();
                //    string tel = reader.GetValue("Telefon", typeof(string)).ToString();

                //    print_str += "\n " + tic_unvan + " \n";
                //    print_str += " " + adres + " \n";
                //    print_str += " " + tel + " \n";
                //}
                //print_str += "             Kantar Çýkýþ Fiþi " + "\n";
                //print_str += "Üretim Yeri              :  " + PublicVariables.uretim_yeri + "\n";
                //print_str += "Fiþ Numarasý          :  " + sfis_no + "\n";
                //print_str += "Kantar No                :  " + PublicVariables.kantar_no + "\n";
                //print_str += "Fiþ Tarihi                 :  " + DateTime.Now.ToShortDateString() + "\n";
                //print_str += "Fiþ Saati                   :  " + DateTime.Now.ToShortTimeString() + "\n";
                //print_str += "----------------------------------------" + "\n";
                //print_str += "Plaka                        :  " + splaka + "\n";
                //print_str += "Sürücü Adý               :  " + ssurucu_ad_soyad + "\n";
                //print_str += "Cep Telefonu          :  " + scep_tel + "\n";
                //print_str += "Giriþ Durumu          :  " + sdurum + "\n";
                //print_str += "Tc Kimlik No            :  " + stc_no + "\n";
                //print_str += "Malzeme                  :  " + smalzeme_kodu + "\n";
                //print_str += "Firma                        :  " + sfirma + "\n";
                //print_str += "Þehir                         :  " + ssehir + "\n";
                //print_str += "Ýrsaliye Tarihi           :  " + sirsaliye_tar + "\n";
                //print_str += "Ýrsaliye No                :  " + sirsaliye_no + "\n";
                //print_str += "----------------------------------------" + "\n";
                //print_str += "1. Tartým   :  " + sgiris_tar + " | " + sgiris_kg + " Kg. \n";
                //print_str += "2. Tartým   :  " + scikis_tar + " | " + scikis_kg + " Kg. \n";
                //print_str += "\n";
                //print_str += "\n";
                //print_str += "         --- NET  " + Convert.ToString(Math.Abs(Convert.ToInt64(snet_kg))) + "  Kg. ---" + "\n";

                print_str = "";
                reader = new AppSettingsReader();
                string tic_unvan = reader.GetValue("TicariUnvan", typeof(string)).ToString();
                //string adres = reader.GetValue("Adres", typeof(string)).ToString();
                string tel = reader.GetValue("Telefon", typeof(string)).ToString();
                print_str += "\n " + tic_unvan + " \n";
                print_str += "           tel: " + tel + " \n";
                print_str += "             Kantar Çýkýþ Fiþi " + "\n";
                //print_str += "Üretim Yeri              :  " + PublicVariables.uretim_yeri + "\n";
                print_str += "Fiþ Numarasý           :  " + sfis_no + "\n";
                print_str += "Kantar No                :  " + PublicVariables.kantar_no + "\n";
                print_str += "Fiþ Tarihi                 :  " + DateTime.Now.ToShortDateString() + "\n";
                print_str += "Fiþ Saati                   :  " + DateTime.Now.ToShortTimeString() + "\n";
                print_str += "----------------------------------------" + "\n";
                print_str += "Plaka                        :  " + splaka + "\n";
                print_str += "Dorse      :  " + dorse + "       Knt    :" + kon1 + " " + kon2 + " " + kon3 + " " + kon4 + "\n";
                print_str += "Sürücü Adý               :  " + ssurucu_ad_soyad + "\n";
                print_str += "Cep Telefonu           :  " + scep_tel + "\n";
                //print_str += "Giriþ Durumu          :  " + sdurum + "\n";
                //print_str += "Tc Kimlik No            :  " + stc_no + "\n";
                print_str += "Malzeme                  :  " + smalzeme_kodu + "\n";
                print_str += "Firma                        :  " + sfirma + "\n";
                print_str += "Þehir                         :  " + ssehir + "\n";
                print_str += "Ýrsaliye Tarihi           :  " + sirsaliye_tar + "\n";
                print_str += "Ýrsaliye No                :  " + sirsaliye_no + "\n";
                print_str += "----------------------------------------" + "\n";
                print_str += "1. Tartým   :  " + sgiris_tar + " | " + sgiris_kg + " Kg. \n";
                print_str += "2. Tartým   :  " + scikis_tar + " | " + scikis_kg + " Kg. \n";
                print_str += "\n";
                print_str += "\n";
                print_str += "         --- NET  " + Convert.ToString(Math.Abs(Convert.ToInt64(snet_kg))) + "  Kg. ---";
            }
        }

        string print_str = "";
        private void pd_printpage(object sender, System.Drawing.Printing.PrintPageEventArgs ev)
        {
            Font font = new Font("Gothic", PublicVariables.fontsize);
            //Font font = new Font("Arial", 10);
            string[] lines = print_str.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
               // float ypos = ev.MarginBounds.Top + (i * font.Height);
                float ypos = 10 + (i * font.Height);
                
                ev.Graphics.DrawString(lines[i], font, Brushes.Black, 10, ypos, new StringFormat()); 
                
            }
            
            //Font font = new Font(FontFamily.GenericMonospace, 11);
            //string[] lines = print_str.Split('\n');
            //for (int i = 0; i < lines.Length; i++)
            //{
            //   // float ypos = ev.MarginBounds.Top + (i * font.Height);
            //    float ypos = 10 + (i * font.Height);
                
            //    //ev.Graphics.DrawString(lines[i], font, Brushes.Black,
            //    //    ev.MarginBounds.Left, ypos, new StringFormat());
            //    ev.Graphics.DrawString(lines[i], font, Brushes.Black,
            //       10, ypos, new StringFormat());
            //}
            ev.HasMorePages = false;
        }

        private void frm_FisSureti_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_fis_Click(sender,e);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void txt_LojistikMerkezi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_fis_Click(sender, e);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void btn_kapat_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }           
    }        
}