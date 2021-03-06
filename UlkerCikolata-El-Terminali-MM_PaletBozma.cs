using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GETFirst
{
    public partial class MM_PaletBozma : Form
    {
        WebRfrMM_Palet_Boz.Y_FW0_PP_TRNS_CREATE_PALET_BOZService WS_PaletBoz = new GETFirst.WebRfrMM_Palet_Boz.Y_FW0_PP_TRNS_CREATE_PALET_BOZService();
        WebRfrMM_Palet_Boz.Yfw0Return[] T_Return = new GETFirst.WebRfrMM_Palet_Boz.Yfw0Return[0];
        //WebRfrMM_Palet_Boz.ZppTransferTes[] T_Transfer = new GETFirst.WebRfrMM_Palet_Boz.ZppTransferTes[0];

        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        int indexim = 0;
        int sayi = 0;
        int DTRowCount = 0;
        decimal toplam = 0;
        int Ondaliklicount = 0;
        public MM_PaletBozma()
        {
            InitializeComponent();
            WS_PaletBoz.Url = Utility.GetWSUrl("Y_FW0_PP_TRNS_CREATE_PALET_BOZ");
        }

        private void dataGrid1_CurrentCellChanged(object sender, EventArgs e)
        {

        }

        private void MM_PaletBozma_Load(object sender, EventArgs e)
        {
            dt.Columns.Add("Miktar");
            DataRow row = dt.NewRow();

            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.WindowState = FormWindowState.Maximized;
            this.Text = "Palet Bozma - " + PP_YM_Teyit.AllD.wsUretimYeri;

            DataRow roww = dt.NewRow();
            dataGrid1.DataSource = dt;
            txbBarkod.Focus();
        }

        private void txbBarkod_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.KeyCode == Keys.Enter)
            {
                if (MM_AllData.BarkodCheck(txbBarkod.Text) == "E")
                    MessageBox.Show("Bu Barkodu Kullanamazsınız!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                else
                {
                    Cursor.Current = Cursors.WaitCursor;
                    //MM_AllData.MalzemeAra(txbBarkod.Text, "");

                    WebRfrMM_Get_First.Y_FW0_PP_TRNS_GET_FIRSTService Ws_MMGetFirst = new GETFirst.WebRfrMM_Get_First.Y_FW0_PP_TRNS_GET_FIRSTService();
                    Ws_MMGetFirst.Url = Utility.GetWSUrl("Y_FW0_PP_TRNS_GET_FIRST");


                    MM_AllData.T_DonuleceTablolar.TStok = "X";
                    MM_AllData.T_DonuleceTablolar.TBwart = "X";
                    MM_AllData.HataFlag = "";

                    try
                    {
                        Ws_MMGetFirst.YFw0PpTrnsGetFirst(txbBarkod.Text, 
                            MM_AllData.T_DonuleceTablolar, ref MM_AllData.e_Param,
                            "", PP_YM_Teyit.AllD.wspassword, PP_YM_Teyit.AllD.wssession,
                            ref MM_AllData.T_Depo, ref MM_AllData.T_Siparisler,
                            ref MM_AllData.T_Werks, PP_YM_Teyit.AllD.wsusercode,
                            PP_YM_Teyit.AllD.wsuserno, PP_YM_Teyit.AllD.wsUretimYeri);
                        MM_AllData.HataFlag = "S";
                    }
                    catch(Exception ex)
                    {
                        MM_AllData.HataFlag = "E";
                        Utility.LogTheException("ULKERMOBIL", "Y_FW0_PP_TRNS_GET_FIRST",
                        ex.Message, this.Name, "72");
                    }
                    if (MM_AllData.HataFlag == "S")
                    {
                        lblStok.Text = MM_AllData.e_Param[0].Stok.ToString();//.Remove(MM_AllData.e_Param[0].Stok.ToString().Length-2,2);
                        lblBirim.Text = MM_AllData.e_Param[0].OlcuBirim;
                    }
                    else
                    {
                        lblStok.Text = "0";
                        lblBirim.Text = "";
                    }
                    Cursor.Current = Cursors.Default;
                }
            }            
        }

        private void dataGrid1_CurrentCellChanged_1(object sender, EventArgs e)
        {

            DataGridCell currentCell;
            currentCell = dataGrid1.CurrentCell;
            indexim = currentCell.RowNumber;
            txbMiktar.Text = dataGrid1[currentCell.RowNumber, 0].ToString();

            txbMiktar.Focus();
            txbMiktar.SelectAll();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {

           // Ekle();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            
                if ((dt.Rows.Count > 0) && (dataGrid1.CurrentCell.RowNumber > -1))
                {
                    lblGirilen.Text = Convert.ToString(Convert.ToDecimal(lblGirilen.Text) - Convert.ToDecimal(dataGrid1[dataGrid1.CurrentCell.RowNumber, 0].ToString()));
                    dt.Rows.RemoveAt(dataGrid1.CurrentCell.RowNumber);
                    dataGrid1.DataSource = dt;
                    txbMiktar.Focus();
                    txbMiktar.Text = "";
                    DTRowCount--;
                }
            



        }
        public void Ekle()
        {
                lblGirilen.Text = Convert.ToString(Convert.ToInt32(lblGirilen.Text) - Convert.ToInt32(txbMiktar.Text));
        }

        public void YeniEkle()
        {
            bool c = true;
            txbMiktar.Text = txbMiktar.Text.Replace('.', ',');
            int indexof = 0;
            indexof = txbMiktar.Text.ToString().IndexOf(',');
            if (indexof != -1)
            {
                int noktadansonra = txbMiktar.Text.Substring(indexof + 1).Length;
                if (noktadansonra < 1)
                    txbMiktar.Text = txbMiktar.Text + "000";

                else if (noktadansonra < 2)
                    txbMiktar.Text = txbMiktar.Text + "00";
                else if (noktadansonra < 3)
                    txbMiktar.Text = txbMiktar.Text + "0";
                else if (noktadansonra == 3)
                {
                }
                else
                {
                    MessageBox.Show("Miktar Alanında Virgülden sonra 3 basamak yazabilirsiniz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                    c = false;
                }
            }
            if (c)
            {

                try
                {
                    if (decimal.Parse(txbMiktar.Text) != 0)
                    {
                        if (txbMiktar.Text.Trim() != "" || txbMiktar.Text.IndexOf('.') != -1)
                        {
                            decimal stok = Convert.ToDecimal(lblStok.Text);
                            lblStok.Text = lblStok.Text.Replace('.', ',');

                            toplam = decimal.Parse(lblGirilen.Text) + decimal.Parse(txbMiktar.Text);
                            if (stok >= toplam)
                            {
                                DataRow row = dt.NewRow();
                                row[0] = txbMiktar.Text;
                                dt.Rows.Add(row);
                                dataGrid1.DataSource = dt;
                                lblGirilen.Text = Convert.ToString(Convert.ToDecimal(lblGirilen.Text) + Convert.ToDecimal(txbMiktar.Text));
                                txbMiktar.Text = "";
                                DTRowCount++;
                            }
                            else
                                MessageBox.Show("Stokdan Fazla Deger Giremezsiniz", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                        }
                        else
                            MessageBox.Show("Miktar Kutusunu Doldurunuz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                    }
                }
                catch
                {
                    MessageBox.Show("Tam Sayı Değer Giriniz", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                }
            }


        }
        private void txbMiktar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                YeniEkle();
        }

        private void btnYurut_Click(object sender, EventArgs e)
        {
            if (lblStok.Text != "0,0" && dt.Rows.Count != 0 && txbBarkod.Text != "" && decimal.Parse(lblStok.Text) == decimal.Parse(lblGirilen.Text))
            {
                WebRfrMM_Palet_Boz.ZppTransferTes[] T_Transfer = new GETFirst.WebRfrMM_Palet_Boz.ZppTransferTes[dt.Rows.Count];
                int myindex = 0;
                foreach (WebRfrMM_Palet_Boz.ZppTransferTes rows in T_Transfer)
                {
                    WebRfrMM_Palet_Boz.ZppTransferTes satir = new GETFirst.WebRfrMM_Palet_Boz.ZppTransferTes();
                    satir.Barkod = txbBarkod.Text;
                    string str = dataGrid1[myindex, 0].ToString();
                    satir.Miktar = decimal.Parse(str);
                    T_Transfer[myindex] = satir;
                    myindex += 1;
                }
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    WS_PaletBoz.YFw0PpTrnsCreatePaletBoz(ref T_Return,
                        MM_AllData.e_Param[0].DepoOut, PP_YM_Teyit.AllD.wsUretimYeri, 
                        PP_YM_Teyit.AllD.wspassword, PP_YM_Teyit.AllD.wssession, 
                        ref T_Transfer, PP_YM_Teyit.AllD.wsusercode, PP_YM_Teyit.AllD.wsuserno);
                    if (T_Return[0].RcCheck != "E")
                    {

                        Cursor.Current = Cursors.Default;
                        MessageBox.Show(T_Return[0].RcText, "Tamamlandı", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                        MM_AllData.X_Transfer = T_Transfer;
                        MM_AllData.xPaletBozFlag = "X";
                        Yazdir();
                        dt.Clear();
                        dataGrid1.DataSource = dt;
                        txbBarkod.Text = "";
                        txbMiktar.Text = "";
                        txbBarkod.Focus();
                        lblBirim.Text = "";
                        lblGirilen.Text = "0";
                        lblStok.Text = "0";
                    }
                    else
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show(T_Return[0].RcText, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                    }
                }
                catch (Exception ex)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Bağlantı Hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                    Utility.LogTheException("ULKERMOBIL", "Y_FW0_PP_TRNS_CREATE_PALET_BOZ",
                    ex.Message, this.Name, "230");
                }
                Cursor.Current = Cursors.Default;
            }
            else
            {
                if (lblStok.Text == lblGirilen.Text)
                    MessageBox.Show("Lütfen Girdiğiniz Bilgilerin Doğruluğunu Kontrol Ediniz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                else
                {
                    MessageBox.Show("Girilen miktarı otomatik tamamlamak bu mesajı kapattıktan sonra Enter tuşuna basınız!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                    decimal fark = decimal.Parse(lblStok.Text) - decimal.Parse(lblGirilen.Text);
                    txbMiktar.Text = fark.ToString();
                    txbMiktar.Focus();
                }
            }
        }

        private void txbBarkod_TextChanged(object sender, EventArgs e)
        {

        }


        public void Yazdir()
        {
            frmPrint fPr1 = new frmPrint();
            if (fPr1.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Etiket basıldı");
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}