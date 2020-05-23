using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Xml;
using System.IO;
using NBarCodes;

namespace KOCTAS_PRINT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string result = "";
            //Functions.BatCreate(out result);
            if (result != "")
            {
                MessageBox.Show(result);
            }
            Functions.PrinterGetir(cbPrinter);
            try
            {
                Functions.initialization();
            }
            catch (Exception ex)
            {

                MessageBox.Show("FTP servera bağlanamadı " + ex.ToString());
                return;
            }

            if (!File.Exists(GlobalData.itabPath))
            {
                MessageBox.Show("Fiyat bulunamadı " + GlobalData.itabPath);
                return;
            }
            Functions.itaboku();
            GlobalData.bars.BackColor = Color.White;
            GlobalData.bars.Type = BarCodeType.Ean13;
            GlobalData.bars.Data = "8698541020759";
            tbEtk1.Text = GlobalData.etkList1.Count.ToString();
            tbEtk2.Text = GlobalData.etkList2.Count.ToString();
            tbEtk4.Text = GlobalData.etkList4.Count.ToString();
        }

        private void btnPrint1_Click(object sender, EventArgs e)
        {
            if (GlobalData.etkList1.Count == 0)
            {
                MessageBox.Show("Yazdırılarak etiket bulunmamaktadır");
                return;
            }
            try
            {
                string printer = cbPrinter.SelectedItem.ToString();
                int bosCount = 0;
                try
                {

                    Int32.TryParse(tbBos1.Text.Trim(), out bosCount);
                }
                catch
                {
                    MessageBox.Show("Boş etiket sayısı rakam olmak zorundadır");
                    return;
                }
                try
                {
                    itabprint("001", GlobalData.etkList1, printer, bosCount);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Etiket çıktısı alırken hata oluştu " + ex.ToString());
                }


            }
            catch
            {
                MessageBox.Show("Yazıcı seçmeniz gerekmektedir");

            }

        }

        private void btnPrint2_Click(object sender, EventArgs e)
        {
            if (GlobalData.etkList2.Count == 0)
            {
                MessageBox.Show("Yazdırılarak etiket bulunmamaktadır");
                return;
            }
            try
            {
                string printer = cbPrinter.SelectedItem.ToString();
                int bosCount = 0;
                try
                {
                    Int32.TryParse(tbBos2.Text.Trim(), out bosCount);
                }
                catch
                {
                    MessageBox.Show("Boş etiket sayısı rakam olmak zorundadır");
                    return;
                }
                try
                {
                    itabprint("002", GlobalData.etkList2, printer, bosCount);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Etiket çıktısı alırken hata oluştu " + ex.ToString());
                }

            }
            catch
            {
                MessageBox.Show("Yazıcı seçmeniz gerekmektedir");

            }
        }

        private void btnPrint4_Click(object sender, EventArgs e)
        {
            if (GlobalData.etkList4.Count == 0)
            {
                MessageBox.Show("Yazdırılarak etiket bulunmamaktadır");
                return;
            }
            try
            {
                string printer = cbPrinter.SelectedItem.ToString();
                int bosCount = 0;
                try
                {
                    Int32.TryParse(tbBos4.Text.Trim(), out bosCount);
                }
                catch
                {
                    MessageBox.Show("Boş etiket sayısı rakam olmak zorundadır");
                }
                try
                {
                    itabprint("004", GlobalData.etkList4, printer, bosCount);
             
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Etiket çıktısı alırken hata oluştu " + ex.ToString());
                }

            }
            catch
            {
                MessageBox.Show("Yazıcı seçmeniz gerekmektedir");

            }
        }

        private void itabprint(string tip, List<Etiket> list, string printer, int bosCount)
        {
            GlobalData.counter = 0;
            GlobalData.etkTip = tip;
            GlobalData.printer = printer;
            GlobalData.etkList.Clear();
            GlobalData.etkList = list.ToList<Etiket>();
            GlobalData.bosEtiket = bosCount;
            string orientat = "";
            Functions.orientation(tip, out orientat);

            PrintDocument pd = new PrintDocument();
            PageSettings psetting = new PageSettings();

            if (orientat == "yatay")
            {
                psetting.Landscape = true;
            }
            else
            {
                psetting.Landscape = false;
            }
            pd.DefaultPageSettings = psetting;
            pd.PrinterSettings.PrinterName = cbPrinter.SelectedItem.ToString();
            pd.PrintPage += new PrintPageEventHandler(print);
            pd.Print();
            pd.Dispose();
            GlobalData.counter = 0;
        }
        
        private void print(object o, PrintPageEventArgs e)
        {

            Pen pen = new Pen(Color.Black);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            string tip, Aciklama, Boy, DikeyEtiketSayisi, En, OrgX, OrgY,
               YatayEtiketSayisi, verspace, horspace, Yon = "";


            tip = GlobalData.etkTip;
            ListBox lbItems = new ListBox();
            if (GlobalData.doc != null)
            {
                XmlNodeList nodelist = GlobalData.doc.SelectNodes("koctas/etiket[@tip='" + tip + "']/items/item");
                foreach (XmlNode node in nodelist)
                {
                    lbItems.Items.Add(node.Attributes["name"].Value);

                }
            }

            Functions.BilgileriGetir(tip, out Yon, out Aciklama, out Boy,
               out DikeyEtiketSayisi, out En, out OrgX, out OrgY,
               out YatayEtiketSayisi, out verspace, out horspace);

            int x, y, w, h = 0;
            int en = Convert.ToInt32(En);
            int boy = Convert.ToInt32(Boy);
            int dikeysayi = Convert.ToInt32(DikeyEtiketSayisi);
            int yataysayi = Convert.ToInt32(YatayEtiketSayisi);
            int orgx = Convert.ToInt32(OrgX);
            int orgy = Convert.ToInt32(OrgY);
            int vspace = Convert.ToInt32(verspace);
            int hspace = Convert.ToInt32(horspace);
            w = en;
            h = boy;

            for (int i = 0; i < dikeysayi; i++)
            {
                y = i * boy + i * vspace + orgy;
                for (int k = 0; k < yataysayi; k++)
                {
                    if (GlobalData.counter >= GlobalData.etkList.Count)
                    {
                        return;
                    }

                    x = k * en + k * hspace + orgx;

                    //boş etiket için
                    if (GlobalData.bosEtiket > i * yataysayi + k && GlobalData.counter == 0)
                    {
                        continue;
                    }

                    // e.Graphics.DrawRectangle(pen, x, y, w, h);
                    foreach (string item in lbItems.Items)
                    {
                        string font, text, aktif, maxkar = "";
                        float size, p_x, p_y = 0;
                        text = "";
                        // tek fiyat ve çift fiyat aynı itab üzerinden geldiğinde koordinat ayrıştırması yapıyorum. 30,03,2017 -mcy
                        #region TEK FİYAT
                        if (GlobalData.etkList.ElementAt(GlobalData.counter).EskiFiyatBas.Trim() == "N")
                        {
                            switch (item)
                            {
                                case "fiyat1":
                                    text = GlobalData.etkList.ElementAt(GlobalData.counter).fiyat1;
                                    if (text.Length > 3)
                                    {
                                        text = text.Substring(0, text.Length - 3)
                                        + "." + text.Substring(text.Length - 3, 3);
                                    }
                                    break;
                                case "fiyat2":
                                    text = GlobalData.etkList.ElementAt(GlobalData.counter).fiyat2;
                                    break;
                                case "ayirac":
                                    text = GlobalData.etkList.ElementAt(GlobalData.counter).ayirac;
                                    break;
                                case "tanim":
                                    text = GlobalData.etkList.ElementAt(GlobalData.counter).maktx;
                                    break;
                                case "birim":
                                    text = GlobalData.etkList.ElementAt(GlobalData.counter).birim;
                                    break;
                                case "kdv":
                                    text = GlobalData.etkList.ElementAt(GlobalData.counter).kdv;
                                    break;
                                case "barkod":
                                    text = GlobalData.etkList.ElementAt(GlobalData.counter).barkod;
                                    break;
                                case "reyon":
                                    text = GlobalData.etkList.ElementAt(GlobalData.counter).reyon;
                                    break;
                                case "mensei":
                                    text = GlobalData.etkList.ElementAt(GlobalData.counter).mensei;
                                    break;
                                case "tarih":
                                    text = GlobalData.etkList.ElementAt(GlobalData.counter).tarih;
                                    break;
                                case "urunkodu":
                                    text = GlobalData.etkList.ElementAt(GlobalData.counter).matnr;
                                    break;
                                default:
                                    break;
                            }
                        }
                        #endregion
                        #region ÇİFT FİYAT
                        else if (GlobalData.etkList.ElementAt(GlobalData.counter).EskiFiyatBas.Trim() == "Y")
                        {
                            switch (item)
                            {
                                case "fiyat1_yeni":
                                    text = GlobalData.etkList.ElementAt(GlobalData.counter).fiyat1_yeni;
                                    if (text.Length > 3)
                                    {
                                        text = text.Substring(0, text.Length - 3)
                                        + "." + text.Substring(text.Length - 3, 3);
                                    }
                                    break;
                                case "fiyat2_yeni":
                                    text = GlobalData.etkList.ElementAt(GlobalData.counter).fiyat2_yeni;
                                    break;
                                case "ayirac_yeni":
                                    text = GlobalData.etkList.ElementAt(GlobalData.counter).ayirac_yeni;
                                    break;
                                case "tanim":
                                    text = GlobalData.etkList.ElementAt(GlobalData.counter).maktx;
                                    break;
                                case "birim_yeni":
                                    text = GlobalData.etkList.ElementAt(GlobalData.counter).birim_yeni;
                                    break;
                                case "kdv":
                                    text = GlobalData.etkList.ElementAt(GlobalData.counter).kdv;
                                    break;
                                case "barkod":
                                    text = GlobalData.etkList.ElementAt(GlobalData.counter).barkod;
                                    break;
                                case "reyon":
                                    text = GlobalData.etkList.ElementAt(GlobalData.counter).reyon;
                                    break;
                                case "mensei":
                                    text = GlobalData.etkList.ElementAt(GlobalData.counter).mensei;
                                    break;
                                case "tarih":
                                    text = GlobalData.etkList.ElementAt(GlobalData.counter).tarih;
                                    break;
                                case "urunkodu":
                                    text = GlobalData.etkList.ElementAt(GlobalData.counter).matnr;
                                    break;
                                case "fiyat1_eski":
                                    text = GlobalData.etkList.ElementAt(GlobalData.counter).fiyat1_eski;
                                    if (text.Length > 3)
                                    {
                                        text = text.Substring(0, text.Length - 3)
                                        + "." + text.Substring(text.Length - 3, 3);
                                    }
                                    break;
                                case "fiyat2_eski":
                                    text = GlobalData.etkList.ElementAt(GlobalData.counter).fiyat2_eski;
                                    break;
                                case "ayirac_eski":
                                    text = GlobalData.etkList.ElementAt(GlobalData.counter).ayirac_eski;
                                    break;
                                case "birim_eski":
                                    text = GlobalData.etkList.ElementAt(GlobalData.counter).birim_eski;
                                    break;
                                default:
                                    break;
                            }
                        }
                        #endregion

                        Functions.BilgiGetirItem(tip, text, item, out font, out size, out p_x, out aktif, out p_y, out maxkar);
 
                        if (aktif == "1")
                        {
                            if (item == "barkod")
                            {
                                #region BARKOD
                                if (text.Length != 13)
                                {
                                    GlobalData.bars.Type = BarCodeType.Code128;
                                }
                                else
                                {
                                    GlobalData.bars.Type = BarCodeType.Ean13;
                                }
                                BarCodeGenerator bg = new BarCodeGenerator(GlobalData.bars);

                                //Image img = bg.GenerateImage();
                                Image img = null;
                                if (tip == "001")
                                {
                                    GlobalData.bars.Font = new Font("Arial", 8);
                                    GlobalData.bars.BarHeight = 50;
                                    GlobalData.bars.Data = text;
                                    GlobalData.bars.ModuleWidth = 1;
                                    bg = new BarCodeGenerator(GlobalData.bars);
                                    img = bg.GenerateImage();
                                    img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                }
                                if (tip == "002")
                                {
                                    Size sz = new Size(190, 35);
                                    GlobalData.bars.Font = new Font("Arial", 10);
                                    GlobalData.bars.BarHeight = 30;
                                    GlobalData.bars.Data = text;
                                    GlobalData.bars.ModuleWidth = 1.5f;
                                    bg = new BarCodeGenerator(GlobalData.bars);
                                    img = bg.GenerateImage();
                                }
                                if (tip == "004")
                                {
                                    Size sz = new Size(300, 50);
                                    GlobalData.bars.BarHeight = 50;
                                    GlobalData.bars.Font = new Font("Arial", 12);
                                    GlobalData.bars.Data = text;
                                    GlobalData.bars.ModuleWidth = 2f;
                                    bg = new BarCodeGenerator(GlobalData.bars);
                                    img = bg.GenerateImage();
                                }

                                e.Graphics.DrawImage(img, new PointF(p_x + x, p_y + y));
                                #endregion
                            }
                            else
                            {
                                //barkod dışındakileri. 17.03.2017 -mcy
                                if (maxkar == "")
                                {
                                    e.Graphics.DrawString(text, new System.Drawing.Font(font, size), drawBrush, new PointF(p_x + x, p_y + y));

                                    //ESKİ FİYAT ÜZERİNE ÇARPI İŞLEMLERİ
                                    #region CARPI_ISLEMLERI
                                    if (item == "fiyat1_eski") //ikinci fiyatlar basılırken üzerlerine çizgi çekiyorum. 17.03.2017 -mcy
                                    {
                                        if (GlobalData.etkList.ElementAt(GlobalData.counter).EskiFiyatBas == "Y")
                                        {
                                            if (GlobalData.etkList.ElementAt(GlobalData.counter).EskiFiyatCarpi == "Y")
                                            {
                                                #region ETIKET_ÇARPI_CİZİM
                                              
                                                    string deger = text.ToString(); //genen rakkamın karakter uzunluğu kadar stopPoint_X noktasını uzata bilirim. 

                                                    float degerKarakterAdedi = deger.Length;
                                                    if (degerKarakterAdedi == 1) //kağıt boşluğundan dolayı geride kaldıkları için değerleri göz ile ayarlayarak yaptım. 17.03.2017 -mcy
                                                        degerKarakterAdedi = 2.5f;
                                                    else if (degerKarakterAdedi == 2)
                                                        degerKarakterAdedi = 3;
                                                    else if (degerKarakterAdedi == 3)
                                                        degerKarakterAdedi = 4;
                                                    float karakterCarpani = x + size * degerKarakterAdedi;

                                                    int _startPoint_X1 = 0
                                                      , _startPoint_Y1 = 0
                                                      , _stopPoint_X1 = 0
                                                      , _stopPoint_Y1 = 0
                                                      , _startPoint_X2 = 0
                                                      , _startPoint_Y2 = 0
                                                      , _stopPoint_X2 = 0
                                                      , _stopPoint_Y2 = 0;

                                                    CarpiKoorGetir(tip, text.Length.ToString(), out _startPoint_X1
                                                        , out  _startPoint_Y1
                                                    , out  _stopPoint_X1
                                                    , out  _stopPoint_Y1
                                                    , out  _startPoint_X2
                                                    , out  _startPoint_Y2
                                                    , out  _stopPoint_X2
                                                    , out  _stopPoint_Y2);

                                                   
                                                    e.Graphics.DrawLine(new Pen(Color.Black, 1),
                                                                             p_x + x + _startPoint_X1,//12,
                                                                             p_y + y + _startPoint_Y1,//18,
                                                                             p_x + karakterCarpani + _stopPoint_X1,//- 10,
                                                                             p_y + y + size + _stopPoint_Y1);//18);

                                                    e.Graphics.DrawLine(new Pen(Color.Black, 1),
                                                                         p_x + x + _startPoint_X2,//12,
                                                                         p_y + y + size + _startPoint_Y2,//18,
                                                                         p_x + karakterCarpani + _stopPoint_X2,//- 10,
                                                                         p_y + y + _stopPoint_Y2);//18);
                                               
                                               
                                                #endregion
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    int maks = Convert.ToInt32(maxkar);
                                    string[] words = text.Split(' ');
                                    List<string> wordlist = Functions.splitstring(text, maks);
                                    int count = 0;
                                    foreach (string word in wordlist)
                                    {
                                        int fy = Convert.ToInt32(count * size * 1.5);
                                        e.Graphics.DrawString(word.TrimStart(' '), new System.Drawing.Font(font, size), drawBrush, new PointF(p_x + x, p_y + y + fy));
                                        count++;

                                    }
                                }
                            }
                        }
                    }
                    GlobalData.counter = GlobalData.counter + 1;
                }
            }

            if (GlobalData.counter < GlobalData.etkList.Count)
            {
                e.HasMorePages = true;
            }
            else
            {
                e.HasMorePages = false;
            }

        }

        //29,03,2017 -mcy
        private void CarpiKoorGetir(string tip, string name, out int _startPoint_X1
            , out int _startPoint_Y1
            , out int _stopPoint_X1
            , out int _stopPoint_Y1
            , out int _startPoint_X2
            , out int _startPoint_Y2
            , out int _stopPoint_X2
            , out int _stopPoint_Y2)
        {
            _startPoint_X1 = 0;
            _startPoint_Y1 = 0;
            _stopPoint_X1 = 0;
            _stopPoint_Y1 = 0;
            _startPoint_X2 = 0;
            _startPoint_Y2 = 0;
            _stopPoint_X2 = 0;
            _stopPoint_Y2 = 0;

            object[] objarray = new object[9];
            if (GlobalData.doc != null)
            {
                XmlNode nodeItem = GlobalData.doc.SelectSingleNode("koctas/etiket[@tip='" + tip + "']/items/item[@name='carpi_eski']/durumlar/durum[@name='" + name + "']");
                XmlNodeList nodelist = nodeItem.ChildNodes;
                foreach (XmlNode node in nodelist)
                {
                    switch (node.Name)
                    {
                        case "startPoint_X1":
                            XmlNode startPoint_X1 = node.ChildNodes[0];
                            _startPoint_X1 = Convert.ToInt32(startPoint_X1.InnerText);
                            break;
                        case "startPoint_Y1":
                            XmlNode startPoint_Y1 = node.ChildNodes[0];
                            _startPoint_Y1 = Convert.ToInt32(startPoint_Y1.InnerText);
                            break;
                        case "stopPoint_X1":
                            XmlNode stopPoint_X1 = node.ChildNodes[0];
                            _stopPoint_X1 = Convert.ToInt32(stopPoint_X1.InnerText);
                            break;
                        case "stopPoint_Y1":
                            XmlNode stopPoint_Y1 = node.ChildNodes[0];
                            _stopPoint_Y1 = Convert.ToInt32(stopPoint_Y1.InnerText);
                            break;
                        case "startPoint_X2":
                            XmlNode startPoint_X2 = node.ChildNodes[0];
                            _startPoint_X2 = Convert.ToInt32(startPoint_X2.InnerText);
                            break;
                        case "startPoint_Y2":
                            XmlNode startPoint_Y2 = node.ChildNodes[0];
                            _startPoint_Y2 = Convert.ToInt32(startPoint_Y2.InnerText);
                            break;
                        case "stopPoint_X2":
                            XmlNode stopPoint_X2 = node.ChildNodes[0];
                            _stopPoint_X2 = Convert.ToInt32(stopPoint_X2.InnerText);
                            break;
                        case "stopPoint_Y2":
                            XmlNode stopPoint_Y2 = node.ChildNodes[0];
                            _stopPoint_Y2 = Convert.ToInt32(stopPoint_Y2.InnerText);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (File.Exists(GlobalData.XMLPath))
                {
                    File.Delete(GlobalData.XMLPath);
                }
                if (File.Exists(GlobalData.XMLPath2))
                {
                    File.Delete(GlobalData.XMLPath2);
                }
            }
            catch
            {
            }

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }



    }
}
