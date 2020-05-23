using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConnectorV2
{
    class Calistir
    {
        public static void AktarimBaslat(int firmaid, int servistipi, int sablontipi, string username, string password, string sirketkodu, string firmaapikey)
        {
            try
            {
                sqltestEntities data = new sqltestEntities();

                string HesapKod = "";
                string GirisTarih = "";
                DataSet ds = new DataSet();

                #region şablon 1
                if (sablontipi == 7) //şablon tipi 1 ise
                {
                    #region Ana Veriler
                    if (servistipi == 1) //Anaveri Müşteri
                    {
                        WebServisURL.Al(firmaid, servistipi, sablontipi);
                        var list = data.S_WebServisTanimlari(firmaid).Where(c => c.ServisYonu == true && c.SablonTipi_Tip2 == sablontipi).ToList();
                        int gidenid = Convert.ToInt32(list.FirstOrDefault().ServisTipi_Tip1);
                        WebServisURL.MutabakatAl(firmaid, gidenid, sablontipi);

                        webservis.mutabakat.Mutabakat mutabakat = new webservis.mutabakat.Mutabakat();
                        mutabakat.Timeout = 43200;
                        sablon1.musteri.anaveri.ZISISMUT03 sapService = new sablon1.musteri.anaveri.ZISISMUT03();
                        sapService.Credentials = new NetworkCredential(username, password);
                        sablon1.musteri.anaveri.ZISISMUT031 saticiService = new sablon1.musteri.anaveri.ZISISMUT031();
                        saticiService.FT_CUSTOMER = new sablon1.musteri.anaveri.ZISISMUTS03V[] { };
                        saticiService.PIV_BUKRS = sirketkodu;
                        sablon1.musteri.anaveri.ZISISMUT03Response response = sapService.CallZISISMUT03(saticiService);
                        foreach (var item in response.FT_CUSTOMER)
                        {
                            mutabakat.FirmaBilgileri(firmaapikey, item.KUNNR, item.NAME1 + " " + item.NAME2, item.SMTP_ADDR,
                                        item.TEL_NUMBER, item.STCD2, item.STCD1, item.STR_SUPPL1, item.STR_SUPPL2, item.POST_CODE1, item.USER_SMTP_ADDR,
                                        "erp", "erp", "Musteri");
                        }
                    }     
                  
                    else if (servistipi == 2) //Anaveri Satıcı
                    {
                        WebServisURL.Al(firmaid, servistipi, sablontipi);
                        var list = data.S_WebServisTanimlari(firmaid).Where(c => c.ServisYonu == true && c.SablonTipi_Tip2 == sablontipi).ToList();
                        int gidenid = Convert.ToInt32(list.FirstOrDefault().ServisTipi_Tip1);
                        WebServisURL.MutabakatAl(firmaid, gidenid, sablontipi);

                        webservis.mutabakat.Mutabakat mutabakat = new webservis.mutabakat.Mutabakat();
                        mutabakat.Timeout = 43200;
                        sablon1.satici.anaveri.ZISISMUT01 sapService = new sablon1.satici.anaveri.ZISISMUT01();
                        sapService.Credentials = new NetworkCredential(username, password);
                        sablon1.satici.anaveri.ZISISMUT011 saticiService = new sablon1.satici.anaveri.ZISISMUT011();
                        saticiService.FT_VENDOR = new sablon1.satici.anaveri.ZISISMUTS01V[] { };
                        saticiService.PIV_BUKRS = sirketkodu;
                        sablon1.satici.anaveri.ZISISMUT01Response response = sapService.CallZISISMUT01(saticiService);

                        foreach (var vendor in response.FT_VENDOR)
                        {
                            mutabakat.FirmaBilgileri(firmaapikey, vendor.LIFNR, vendor.NAME1 + " " + vendor.NAME2, vendor.SMTP_ADDR,
                                vendor.TEL_NUMBER, vendor.STCD2, vendor.STCD1, vendor.STR_SUPPL1, vendor.STR_SUPPL2, vendor.POST_CODE1, vendor.USER_SMTP_ADDR,
                                "erp", "erp", "Satici");
                        }
                    }
                    #endregion
                    #region Hareketler
                    else if (servistipi == 3)//Hareket Müşteri
                    {
                        WebServisURL.Al(firmaid, servistipi, sablontipi);
                        var list = data.S_WebServisTanimlari(firmaid).Where(c => c.ServisYonu == true && c.SablonTipi_Tip2 == sablontipi).ToList();
                        int gidenid = Convert.ToInt32(list.FirstOrDefault().ServisTipi_Tip1);
                        WebServisURL.MutabakatAl(firmaid, gidenid, sablontipi);

                        webservis.mutabakat.Mutabakat mutabakat = new webservis.mutabakat.Mutabakat();
                        mutabakat.Timeout = 43200;
                        ds.Clear();
                        ds = mutabakat.HesapHareketParametreleri(firmaapikey, "Musteri");
                        sablon1.musteri.hareket.ZISISMUT04 sapService = new sablon1.musteri.hareket.ZISISMUT04();
                        sapService.Credentials = new NetworkCredential(username, password);
                        sapService.Timeout = 43200;
                        foreach (DataRow table in ds.Tables[0].Rows)
                        {
                            try
                            {
                                HesapKod = table[0].ToString();
                                GirisTarih = table[1].ToString();

                                sablon1.musteri.hareket.ZISISMUT041 musteriServis = new sablon1.musteri.hareket.ZISISMUT041();
                                musteriServis.FT_OPEN_ITEM = new sablon1.musteri.hareket.ZISISMUTS02T[] { };

                                musteriServis.PIV_BUKRS = sirketkodu;
                                musteriServis.PIV_CPUDT_LOW = C.ParseDateTimeToSQLString(Convert.ToDateTime(GirisTarih));
                                musteriServis.PIV_CPUDT_HIGH = C.ParseDateTimeToSQLString(DateTime.Now.Date);
                                musteriServis.PIV_KUNNR = HesapKod;
                                sablon1.musteri.hareket.ZISISMUT04Response response = sapService.CallZISISMUT04(musteriServis);

                                if (response != null)
                                {
                                    if (response.FT_OPEN_ITEM.Length > 0)
                                    {
                                        foreach (var vendor in response.FT_OPEN_ITEM)
                                        {
                                            mutabakat.CariHareket(firmaapikey, HesapKod, "Musteri", vendor.BLDAT, vendor.BUDAT, vendor.U_CPUDT,
                                                vendor.XBLNR, vendor.SGTXT, vendor.WRSHB, vendor.DMSHB, vendor.WAERS, vendor.UMSKZ, vendor.BELNR, vendor.U_STBLG,
                                                vendor.BUKRS + vendor.GJAHR + vendor.BELNR + vendor.BUZEI);
                                        }
                                    }
                                }
                            }
                            catch (Exception hata)
                            {
                                mutabakat.EventLog("Hareket Müşteri", hata.Message);
                                continue;
                            }
                        }
                    }
                    else if (servistipi == 4)//Hareket Satıcı
                    {
                        WebServisURL.Al(firmaid, servistipi, sablontipi);
                        var list = data.S_WebServisTanimlari(firmaid).Where(c => c.ServisYonu == true && c.SablonTipi_Tip2 == sablontipi).ToList();
                        int gidenid = Convert.ToInt32(list.FirstOrDefault().ServisTipi_Tip1);
                        WebServisURL.MutabakatAl(firmaid, gidenid, sablontipi);

                        webservis.mutabakat.Mutabakat mutabakat = new webservis.mutabakat.Mutabakat();
                        mutabakat.Timeout = 43200;
                        ds.Clear();
                        ds = mutabakat.HesapHareketParametreleri(firmaapikey, "Satici");
                        sablon1.satici.hareket.ZISISMUT02 sapService = new sablon1.satici.hareket.ZISISMUT02();
                        sapService.Credentials = new NetworkCredential(username, password);
                        sapService.Timeout = 43200;
                        foreach (DataRow table in ds.Tables[0].Rows)
                        {
                            try
                            {
                                HesapKod = table[0].ToString();
                                GirisTarih = table[1].ToString();

                                sablon1.satici.hareket.ZISISMUT021 saticiService = new sablon1.satici.hareket.ZISISMUT021();
                                saticiService.FT_OPEN_ITEM = new sablon1.satici.hareket.ZISISMUTS02T[] { };

                                saticiService.PIV_BUKRS = sirketkodu;
                                saticiService.PIV_BUDAT_LOW = C.ParseDateTimeToSQLString(Convert.ToDateTime(GirisTarih));
                                saticiService.PIV_BUDAT_HIGH = C.ParseDateTimeToSQLString(DateTime.Now.Date);
                                saticiService.PIV_LIFNR = HesapKod;
                                sablon1.satici.hareket.ZISISMUT02Response response = sapService.CallZISISMUT02(saticiService);
                                if (response != null)
                                {
                                    if (response.FT_OPEN_ITEM.Length > 0)
                                    {
                                        foreach (var vendor in response.FT_OPEN_ITEM)
                                        {
                                            mutabakat.CariHareket(
                                            firmaapikey,
                                            HesapKod,
                                            "Satici",
                                            vendor.BLDAT,
                                            vendor.BUDAT,
                                            vendor.U_CPUDT,
                                            vendor.XBLNR,
                                            vendor.SGTXT,
                                            vendor.WRSHB,
                                            vendor.DMSHB,
                                            vendor.WAERS,
                                            vendor.UMSKZ,
                                            vendor.BELNR,
                                            vendor.U_STBLG,
                                            vendor.BUKRS + vendor.GJAHR + vendor.BELNR + vendor.BUZEI);
                                        }
                                    }
                                }
                            }
                            catch (Exception hata)
                            {
                                mutabakat.EventLog("Hareket Satıcı", hata.Message);
                                continue;
                            }
                        }
                    }
                    #endregion
                    #region BABS
                    else if (servistipi == 5)//BABS Müşteri
                    {
                        WebServisURL.Al(firmaid, servistipi, sablontipi);
                        var list = data.S_WebServisTanimlari(firmaid).Where(c => c.ServisYonu == true && c.SablonTipi_Tip2 == sablontipi).ToList();
                        int gidenid = Convert.ToInt32(list.FirstOrDefault().ServisTipi_Tip1);
                        WebServisURL.MutabakatAl(firmaid, gidenid, sablontipi);

                        webservis.mutabakat.Mutabakat mutabakat = new webservis.mutabakat.Mutabakat();
                        mutabakat.Timeout = 43200;
                        ds.Clear();
                        ds = mutabakat.HesapHareketParametreleri(firmaapikey, "Musteri");
                        sablon1.musteri.babs.ZISISMUT06 sapService = new sablon1.musteri.babs.ZISISMUT06();
                        sapService.Credentials = new NetworkCredential(username, password);
                        sapService.Timeout = 43200;
                        foreach (DataRow table in ds.Tables[0].Rows)
                        {
                            try
                            {
                                HesapKod = table[0].ToString();
                                GirisTarih = table[1].ToString();

                                sablon1.musteri.babs.ZISISMUT061 saticiService = new sablon1.musteri.babs.ZISISMUT061();
                                saticiService.FT_KDV = new sablon1.musteri.babs.ZISISMUTS072[] { };

                                saticiService.PIV_BUKRS = sirketkodu;
                                saticiService.PIV_BUDAT_LOW = C.ParseDateTimeToSQLString(Convert.ToDateTime(GirisTarih)); //"2016-01-01";// 
                                saticiService.PIV_BUDAT_HIGH = C.ParseDateTimeToSQLString(DateTime.Now.Date);
                                saticiService.PIV_KUNNR = HesapKod;
                                sablon1.musteri.babs.ZISISMUT06Response response = sapService.CallZISISMUT06(saticiService);

                                if (response != null)
                                {
                                    if (response.FT_KDV.Length > 0)
                                    {
                                        foreach (var vendor in response.FT_KDV)
                                        {
                                            mutabakat.BABSHareket(firmaapikey, HesapKod, "Musteri", "", vendor.BLDAT, vendor.BUDAT, vendor.CPUDT, vendor.XBLNR,
                                                             vendor.HWBAS, vendor.BELNR, vendor.U_STBLG, vendor.KTOSL,
                                                             vendor.BUKRS + vendor.GJAHR + vendor.BELNR + vendor.BUZEI);
                                        }
                                    }
                                }
                            }
                            catch (Exception hata)
                            {
                                mutabakat.EventLog("BABS Müşteri", hata.Message);
                                continue;
                            }
                        }
                    }
                    else if (servistipi == 6)//BABS Satıcı
                    {
                        WebServisURL.Al(firmaid, servistipi, sablontipi);
                        var list = data.S_WebServisTanimlari(firmaid).Where(c => c.ServisYonu == true && c.SablonTipi_Tip2 == sablontipi).ToList();
                        int gidenid = Convert.ToInt32(list.FirstOrDefault().ServisTipi_Tip1);
                        WebServisURL.MutabakatAl(firmaid, gidenid, sablontipi);

                        webservis.mutabakat.Mutabakat mutabakat = new webservis.mutabakat.Mutabakat();
                        mutabakat.Timeout = 43200;
                        ds.Clear();
                        ds = mutabakat.HesapHareketParametreleri(firmaapikey, "Satici");
                        sablon1.satici.babs.ZISISMUT05 sapService = new sablon1.satici.babs.ZISISMUT05();
                        sapService.Credentials = new NetworkCredential(username, password);
                        sapService.Timeout = 43200;

                        foreach (DataRow table in ds.Tables[0].Rows)
                        {
                            try
                            {
                                HesapKod = table[0].ToString();
                                GirisTarih = table[1].ToString();

                                sablon1.satici.babs.ZISISMUT051 saticiService = new sablon1.satici.babs.ZISISMUT051();
                                saticiService.FT_KDV = new sablon1.satici.babs.ZISISMUTS072[] { };

                                saticiService.PIV_BUKRS = sirketkodu;
                                saticiService.PIV_BUDAT_LOW = C.ParseDateTimeToSQLString(Convert.ToDateTime(GirisTarih));//"2016-01-01";// 
                                saticiService.PIV_BUDAT_HIGH = C.ParseDateTimeToSQLString(DateTime.Now.Date);
                                saticiService.PIV_LIFNR = HesapKod;
                                sablon1.satici.babs.ZISISMUT05Response response = sapService.CallZISISMUT05(saticiService);

                                if (response != null)
                                {
                                    if (response.FT_KDV.Length > 0)
                                    {
                                        foreach (var vendor in response.FT_KDV)
                                        {
                                            mutabakat.BABSHareket(firmaapikey, HesapKod, "Satici", "", vendor.BLDAT, vendor.BUDAT, vendor.CPUDT, vendor.XBLNR, vendor.HWBAS,
                                                vendor.BELNR, vendor.U_STBLG, vendor.KTOSL,
                                                vendor.BUKRS + vendor.GJAHR + vendor.BELNR + vendor.BUZEI);
                                        }
                                    }
                                }
                            }
                            catch (Exception hata)
                            {
                                mutabakat.EventLog("BABS Satıcı", hata.Message);
                                continue;
                            }
                        }

                    }
                    #endregion
                    #region Kontak
                    else if (servistipi == 10)//Kontak Müşteri
                    {
                        WebServisURL.Al(firmaid, servistipi, sablontipi);
                        var list = data.S_WebServisTanimlari(firmaid).Where(c => c.ServisYonu == true && c.SablonTipi_Tip2 == sablontipi).ToList();
                        int gidenid = Convert.ToInt32(list.FirstOrDefault().ServisTipi_Tip1);
                        WebServisURL.MutabakatAl(firmaid, gidenid, sablontipi);

                        webservis.mutabakat.Mutabakat mutabakat = new webservis.mutabakat.Mutabakat();
                        mutabakat.Timeout = 43200;
                        sablon1.musteri.anaveri.ZISISMUT03 sapService2 = new sablon1.musteri.anaveri.ZISISMUT03();
                        sapService2.Credentials = new NetworkCredential(username, password);
                        sablon1.musteri.anaveri.ZISISMUT031 musteriService = new sablon1.musteri.anaveri.ZISISMUT031();
                        musteriService.FT_CONTACT = new sablon1.musteri.anaveri.ZISISMUTS01C[] { };
                        musteriService.PIV_BUKRS = sirketkodu;
                        sablon1.musteri.anaveri.ZISISMUT03Response response2 = sapService2.CallZISISMUT03(musteriService);

                        foreach (var item in response2.FT_CUSTOMER)
                        {
                            mutabakat.CariKontak(firmaapikey, item.KUNNR, "Musteri", item.NAME1, item.NAME2, item.SMTP_ADDR);
                        }
                    }
                    else if (servistipi == 11)//Kontak Satıcı
                    {
                        WebServisURL.Al(firmaid, servistipi, sablontipi);
                        var list = data.S_WebServisTanimlari(firmaid).Where(c => c.ServisYonu == true && c.SablonTipi_Tip2 == sablontipi).ToList();
                        int gidenid = Convert.ToInt32(list.FirstOrDefault().ServisTipi_Tip1);
                        WebServisURL.MutabakatAl(firmaid, gidenid, sablontipi);

                        webservis.mutabakat.Mutabakat mutabakat = new webservis.mutabakat.Mutabakat();
                        mutabakat.Timeout = 43200;
                        sablon1.satici.anaveri.ZISISMUT01 sapService = new sablon1.satici.anaveri.ZISISMUT01();
                        sapService.Credentials = new NetworkCredential(username, password);
                        sablon1.satici.anaveri.ZISISMUT011 saticiService = new sablon1.satici.anaveri.ZISISMUT011();
                        //saticiService.FT_CONTACT = new tr.com.solen.satici.ZISISMUTS01C[] { };
                        saticiService.FT_CONTACT = new sablon1.satici.anaveri.ZISISMUTS01C[] { };
                        saticiService.PIV_BUKRS = sirketkodu;
                        sablon1.satici.anaveri.ZISISMUT01Response response = sapService.CallZISISMUT01(saticiService);

                        foreach (var vendor in response.FT_VENDOR)
                        {
                            mutabakat.CariKontak(firmaapikey, vendor.LIFNR, "Satici", vendor.NAME1, vendor.NAME2, vendor.SMTP_ADDR);
                        }

                    }
                    #endregion
                } //şablon tipi 1 ise
                #endregion

            }
            catch (Exception hata)
            {
                
            }
        } //AktarimBaslat

        public static class C
        {






            /// <summary>
            /// Integer dönüştürme için kullanılır.
            /// </summary>
            public static int ParseInt(object o)
            {
                int i = 0;


                try
                {
                    i = Convert.ToInt32(o);
                }
                catch { }
                return i;
            }
            /// <summary>
            /// Boolean dönüştürme için kullanılır.
            /// </summary>
            public static bool ParseBool(object o)
            {
                if (o != null)
                {
                    if (o.ToString().ToLower() == "true" || o.ToString() == "1")
                        return true;
                    else if (o.ToString().ToLower() == "false" || o.ToString() == "0")
                        return false;
                }

                bool b = false;
                try
                {
                    b = Convert.ToBoolean(o);
                }
                catch { }
                return b;
            }

            /// <summary>
            /// Sql tipinde string DateTime dönüştürme için kullanılır.Parametre olarak değerleri DateTime şeklinde alır.
            /// </summary>
            public static string ParseDateTimeToSQLString(DateTime TarihSaat)
            {
                //1987-11-01 19:58:36.080
                try
                {
                    string Gun = TarihSaat.Day.ToString("00");
                    string Ay = TarihSaat.Month.ToString("00");
                    string Yil = TarihSaat.Year.ToString("0000");
                    string Saat = TarihSaat.Hour.ToString("00");
                    string Dakika = TarihSaat.Minute.ToString("00");
                    string Saniye = TarihSaat.Second.ToString("00");
                    string Salise = TarihSaat.Millisecond.ToString("000");

                    if (Saat.Length < 3)
                        Saat = C.ParseInt(Saat).ToString("00");
                    else
                        Saat = "00";

                    if (Dakika.Length < 3)
                        Dakika = C.ParseInt(Dakika).ToString("00");
                    else
                        Dakika = "00";

                    if (Saniye.Length < 3)
                        Saniye = C.ParseInt(Saniye).ToString("00");
                    else
                        Saniye = "00";

                    if (Salise.Length < 4)
                        Salise = C.ParseInt(Salise).ToString("000");
                    else
                        Salise = "000";




                    return C.ParseInt(Yil).ToString("0000") + "-" + C.ParseInt(Ay).ToString("00") + "-" + C.ParseInt(Gun).ToString("00");// + " " + Saat + ":" + Dakika + ":" + Saniye + "." + Salise + "'";


                }
                catch { }
                return "";
            }


            /// <summary>
            /// Sql tipinde string DateTime dönüştürme için kullanılır.Parametre olarak değerleri ayrı ayrı string şeklinde alır.
            /// </summary>
            public static string ParseDateTimeToSQLString(string Gun, string Ay, string Yil, string Saat = "", string Dakika = "", string Saniye = "", string Salise = "")
            {
                //1987-11-01 19:58:36.080
                try
                {
                    if (Saat.Length < 3)
                        Saat = C.ParseInt(Saat).ToString("00");
                    else
                        Saat = "00";

                    if (Dakika.Length < 3)
                        Dakika = C.ParseInt(Dakika).ToString("00");
                    else
                        Dakika = "00";

                    if (Saniye.Length < 3)
                        Saniye = C.ParseInt(Saniye).ToString("00");
                    else
                        Saniye = "00";

                    if (Salise.Length < 4)
                        Salise = C.ParseInt(Salise).ToString("000");
                    else
                        Salise = "000";


                    return C.ParseInt(Yil).ToString("0000") + "-" + C.ParseInt(Ay).ToString("00") + "-" + C.ParseInt(Gun).ToString("00");// + " " + Saat + ":" + Dakika + ":" + Saniye + "." + Salise + "'";

                }
                catch { }
                return "";
            }



            /// <summary>
            /// DateTime? dönüştürme için kullanılır.
            /// </summary>
            public static DateTime? ParseDateTimeOrNull(object o)
            {
                DateTime? dtm = new DateTime?();
                try
                {
                    dtm = Convert.ToDateTime(o);
                    if (dtm.Value.Year < 2000)
                        dtm = null;
                }
                catch
                {

                }
                return dtm;
            }


            /// <summary>
            /// DateTime dönüştürme için kullanılır. 
            /// </summary>
            public static object ParseDateTimeOrDBNULL(object o)
            {

                try
                {
                    if (o == null)
                        return DBNull.Value;
                    return Convert.ToDateTime(o);
                }
                catch { }
                return DBNull.Value;
            }


            /// <summary>
            /// DateTime dönüştürme için kullanılır. 
            /// </summary>
            public static DateTime ParseDateTime(object o)
            {
                DateTime dtm = new DateTime(1900, 1, 1);
                try
                {
                    if (o == null)
                        return dtm;
                    dtm = Convert.ToDateTime(o);
                }
                catch { }
                return dtm;
            }
            /// <summary>
            /// Decimal dönüştürme için kullanılır.
            /// </summary>
            public static decimal ParseDecimal(object o)
            {
                decimal d = 0;
                try
                {
                    //System.Globalization.CultureInfo usCulture = new System.Globalization.CultureInfo("en-US");
                    //d = Convert.ToDecimal(o.ToString().Replace(",", ".").Trim(), usCulture);

                    d = Convert.ToDecimal(o);
                }
                catch { }
                return d;
            }
            /// <summary>
            /// Double dönüştürme için kullanılır.
            /// </summary>
            public static double ParseDouble(object o)
            {
                double d = 0;
                try
                {
                    d = Convert.ToDouble(o);
                }
                catch { }
                return d;
            }





        }

    }
}
