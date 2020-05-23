using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing.Printing;
using System.Drawing;
using System.Data;
using System.Xml;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace KOCTAS_PRINT
{
    class Functions
    {
        
        internal static void initialization()
        {
            GlobalData.XMLPath = @"C:\SapLabel\koctasOrj.dsgn";
            GlobalData.XMLPath2 = @"C:\SapLabel\koctasOrj2.dsgn";
            if (File.Exists(GlobalData.XMLPath))
            {
                File.Delete(GlobalData.XMLPath);
            }
            if (File.Exists(GlobalData.XMLPath2))
            {
                File.Delete(GlobalData.XMLPath2);
            }
            File.Copy(GlobalData.orjXMLPath, GlobalData.XMLPath2);
            Functions.DecryptFile(GlobalData.XMLPath2, GlobalData.XMLPath);
            
            GlobalData.doc.Load(GlobalData.XMLPath);
        }

        internal static void itaboku()
        {

            string[] lines = System.IO.File.ReadAllLines(GlobalData.itabPath, System.Text.Encoding.GetEncoding("windows-1254"));
            GlobalData.etkList1.Clear();
            GlobalData.etkList2.Clear();
            GlobalData.etkList4.Clear();
            int counter = 0;
            foreach (string line in lines)
            {
                string[] values = line.Split('\t');
                Etiket etk = new Etiket();
                etk.barkod = values[0];
                etk.tip = values[2]; //etiket tipi
                etk.count = Convert.ToInt32(values[3]);
                etk.matnr = values[4];
                etk.maktx = values[5];
                string[] fiyat = values[6].Split(',');
                etk.fiyat1 = fiyat[0];
                etk.fiyat2 = fiyat[1];
                etk.ayirac = ",";
                etk.birim = "TL / " + values[7];
                etk.reyon = values[14];
                etk.mensei = values[20];
                etk.kdv = "FİYATA KDV DAHİLDİR";
                etk.tarih = DateTime.Now.ToShortDateString();

                //Meraba Cenk Bey,
                //İtab dosyasının  yeni alanlar eklenmiş hali ektedir .Dosya içerisindeki son 3 alan.
                //fiyat_2(16) type p decimals 2, ->ikinci fiyat değeri
                //fiyat2_bas(1) TYPE c,"Y,N  -> ikinci fiyat basılması isteniyorsa ‘Y’ olacak , aksi durumda ‘N’
                //fiyatcarpi_bas(1) TYPE c,"Y,N ->ikinci fiyatın üzerine çarpı atılacaksa ‘Y’ olacak, aksi durumda ‘N’

                string[] eskiFiyat = values[21].Split(','); //yeni eklenen fiyat alanlarını ekliyorum. 24.03.2017 -mcy
                etk.fiyat1_eski = eskiFiyat[0];
                etk.fiyat2_eski = eskiFiyat[1];
                etk.ayirac_eski = ",";
                etk.birim_eski = "TL / " + values[7];

                etk.fiyat1_yeni = etk.fiyat1;
                etk.fiyat2_yeni = etk.fiyat2;
                etk.ayirac_yeni = etk.ayirac;
                etk.birim_yeni = etk.birim;

                etk.EskiFiyatBas = values[22];
                etk.EskiFiyatCarpi = values[23];

                for (int k = 0; k < etk.count; k++)
                {
                    switch (etk.tip)
                    {
                        case "001":
                            GlobalData.etkList1.Add(etk);
                            break;
                        case "002":
                            GlobalData.etkList2.Add(etk);
                            break;
                        case "004":
                            GlobalData.etkList4.Add(etk);
                            break;
                        //case "011"://yeni alanlar 17.03.2017 -mcy
                        //    GlobalData.etkList1.Add(etk);
                        //    break;
                        //case "022":
                        //    GlobalData.etkList2.Add(etk);
                        //    break;
                        //case "044":
                        //    GlobalData.etkList4.Add(etk);
                        //    break;
                        default:
                            break;
                    }
                    counter += 1;
                }
                
                //if (counter == 45)
                //{
                //    break;
                //}
            }
        }
 
        public static void BilgiGetirItem(string tip, string text, string item, out string font, out float size, out float p_x, out string aktif, out float p_y, out string maxkar)
        {
           font = aktif = maxkar= "";
           size = p_x = p_y = 0;
           if (GlobalData.doc != null)
           {
               XmlNode nodeItem = GlobalData.doc.SelectSingleNode("koctas/etiket[@tip='" + tip + "']/items/item[@name='" + item + "']/size");
               size = Convert.ToInt32(nodeItem.InnerText);
               nodeItem = GlobalData.doc.SelectSingleNode("koctas/etiket[@tip='" + tip + "']/items/item[@name='" + item + "']/font");
               font = nodeItem.InnerText;
               nodeItem = GlobalData.doc.SelectSingleNode("koctas/etiket[@tip='" + tip + "']/items/item[@name='" + item + "']/aktif");
               aktif = nodeItem.InnerText;
               try
               {
                   nodeItem = GlobalData.doc.SelectSingleNode("koctas/etiket[@tip='" + tip + "']/items/item[@name='" + item + "']/karsayi");
                   maxkar = nodeItem.InnerText;
               }
               catch
               {
                   maxkar = "";
                   Console.WriteLine("maxkar");
               }
               nodeItem = GlobalData.doc.SelectSingleNode("koctas/etiket[@tip='" + tip + "']/items/item[@name='" + item + "']/XPoint");
               p_x = Convert.ToInt32(nodeItem.InnerText);
               nodeItem = GlobalData.doc.SelectSingleNode("koctas/etiket[@tip='" + tip + "']/items/item[@name='" + item + "']/YPoint");
               p_y = Convert.ToInt32(nodeItem.InnerText);

               try
               {
                   XmlNode nodeKontrol =
                   GlobalData.doc.SelectSingleNode("koctas/etiket[@tip='" + tip + "']/items/item[@name='" + item + "']/durumlar");
                   if (nodeKontrol != null)
                   {
                       int item_size = text.Length;
                       XmlNode node_PX = null;
                       XmlNode node_PY = null;
                       XmlNode node_Size = null;
                       node_PX = GlobalData.doc.SelectSingleNode("koctas/etiket[@tip='" + tip + "']/items/item[@name='" + item + "']/durumlar/durum[@name='" + item_size + "']/XPoint");
                       p_x = Convert.ToInt32(node_PX.InnerText);
                       node_PY = GlobalData.doc.SelectSingleNode("koctas/etiket[@tip='" + tip + "']/items/item[@name='" + item + "']/durumlar/durum[@name='" + item_size + "']/YPoint");
                       p_y = Convert.ToInt32(node_PY.InnerText);
                       node_Size = GlobalData.doc.SelectSingleNode("koctas/etiket[@tip='" + tip + "']/items/item[@name='" + item + "']/durumlar/durum[@name='" + item_size + "']/size");
                       size = Convert.ToInt32(node_Size.InnerText);
                   }
               }
               catch
               {
               }

           }
        
       }

       public static void BilgileriGetir(string tip,out string yon, out string Aciklama, out string Boy,
              out string DikeyEtiketSayisi, out string En, out string OrgX, out string OrgY,
              out string YatayEtiketSayisi, out string vspace, out string hspace)
       {
           Aciklama = Boy = DikeyEtiketSayisi = En = OrgX = OrgY = YatayEtiketSayisi
               = vspace = hspace =yon = "";
           if (GlobalData.doc != null)
           {
               XmlNode nodeEtiket =
               GlobalData.doc.SelectSingleNode("koctas/etiket[@tip='" + tip + "']");
               XmlNodeList nodelist = nodeEtiket.ChildNodes;
               foreach (XmlNode node in nodelist)
               {
                   switch (node.Name)
                   {
                       case "yatay":
                           YatayEtiketSayisi = node.InnerText;
                           break;
                       case "yon":
                           yon = node.InnerText;
                           break;
                       case "dikey":
                           DikeyEtiketSayisi = node.InnerText;
                           break;
                       case "en":
                           En = node.InnerText;
                           break;
                       case "boy":
                           Boy = node.InnerText;
                           break;
                       case "orgx":
                           OrgX = node.InnerText;
                           break;
                       case "orgy":
                           OrgY = node.InnerText;
                           break;
                       case "vspace":
                           vspace = node.InnerText;
                           break;
                       case "hspace":
                           hspace = node.InnerText;
                           break;
                       case "aciklama":
                           Aciklama = node.InnerText;
                           break;
                       default:
                           break;
                   }
               }
           }
           else
           {
           }
       }

       public static void PrinterGetir(ComboBox cbPrinter)
       {
           cbPrinter.Items.Clear();
           foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
           {
               cbPrinter.Items.Add(printer);
           }
           PrinterSettings settings = new PrinterSettings();
           cbPrinter.SelectedItem = settings.PrinterName;
       }

       public static void orientation(string tip, out string orientat)
       {
           orientat = "";
          
           if (GlobalData.doc != null)
           {
               XmlNode nodeItem = GlobalData.doc.SelectSingleNode("koctas/etiket[@tip='" + tip + "']/yon");
               orientat = nodeItem.InnerText;
           }

       }

       public static List<string> splitstring(string sentence, int max)
       {
           string[] splitArray = sentence.Split(' ');
           List<string> final = new List<string>();
           List<int> finalint = new List<int>();
           int[] indexEmpty = new int[splitArray.Length];
           char[] charArray = sentence.ToCharArray();
           for (int i = 0; i < charArray.Length; i++)
           {
               if (charArray[i] == ' ')
               {
                   finalint.Add(i);
               }
           }
           finalint.Add(sentence.Length);
           // "12 12 34567890 12 34 4124312312 12312312 31231234341231 321";
           //2,5,14,17,20,31,40,55,59
           //20
           int bas = 0;
           int bit = 0;
           for (int k = 0; k < finalint.Count; k++)
           {
               int tmp = finalint.ElementAt(k);
               if (tmp - bas < max + 1)
               {
                   bit = tmp;
                   continue;
               }
               if (tmp - bas == max + 1)
               {
                   final.Add(sentence.Substring(bas, tmp - bas));
                   bas = tmp;
                   bit = tmp;
                   continue;
               }
               if (tmp - bas > max + 1)
               {
                   if (bas == bit)
                   {
                       final.Add(sentence.Substring(bas, max));
                       bas = bas + max;
                       bit = bas;
                   }
                   else
                   {
                       final.Add(sentence.Substring(bas, bit - bas));
                       bas = bit;
                   }
                   k--;
               }
           }
           final.Add(sentence.Substring(bas));
           return final;
       }
       public static void EncryptFile(string inputFile, string outputFile)
       {

           try
           {
               string password = @"myKey123"; // Your Key Here
               UnicodeEncoding UE = new UnicodeEncoding();
               byte[] key = UE.GetBytes(password);

               string cryptFile = outputFile;
               FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

               RijndaelManaged RMCrypto = new RijndaelManaged();

               CryptoStream cs = new CryptoStream(fsCrypt,
                   RMCrypto.CreateEncryptor(key, key),
                   CryptoStreamMode.Write);

               FileStream fsIn = new FileStream(inputFile, FileMode.Open);

               int data;
               while ((data = fsIn.ReadByte()) != -1)
                   cs.WriteByte((byte)data);


               fsIn.Close();
               cs.Close();
               fsCrypt.Close();
           }
           catch
           {
               MessageBox.Show("Encryption failed!", "Error");
           }
       }
       public static void DecryptFile(string inputFile, string outputFile)
       {

           {
               string password = @"myKey123"; // Your Key Here

               UnicodeEncoding UE = new UnicodeEncoding();
               byte[] key = UE.GetBytes(password);

               FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

               RijndaelManaged RMCrypto = new RijndaelManaged();

               CryptoStream cs = new CryptoStream(fsCrypt,
                   RMCrypto.CreateDecryptor(key, key),
                   CryptoStreamMode.Read);

               FileStream fsOut = new FileStream(outputFile, FileMode.Create);

               int data;
               while ((data = cs.ReadByte()) != -1)
                   fsOut.WriteByte((byte)data);

               fsOut.Close();
               cs.Close();
               fsCrypt.Close();

           }
       }

       public static void BatCreate(out string result)
       {
           result = "";
           try
           {
               string path = @"C:\SapLabel\fiyat_etiket.bat";
               string orjPath = @"\\172.16.5.8\magaza_etiket\fiyat_etiket_yeni.bat";
               if (!File.Exists(path))
               {
                   File.Copy(orjPath, path);
               }
           }
           catch
           {
               result = "Batch dosyası kopyalanamadı";
           }
       }
    }
}
