using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ConnectorV2
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        sqltestEntities data = new sqltestEntities();
        public Form1()
        {
            InitializeComponent();
        }

        List<S_WebServisTanimlari_Result> wslist = new List<S_WebServisTanimlari_Result>();

        private void gv_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            int id = Convert.ToInt32(gvTanimlar.GetFocusedRowCellValue("ID"));
            if (id > 0)
            {
                if (XtraMessageBox.Show(id + " ID numaralı kayıdı güncellemek istediğinizden eminmisiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {

                    data.IUD_WebServisTanimlari(id,
                          Convert.ToInt32(gvTanimlar.GetFocusedRowCellValue("FirmaID")),
                          Convert.ToInt32(gvTanimlar.GetFocusedRowCellValue("ServisTipi_Tip1")),
                          Convert.ToInt32(gvTanimlar.GetFocusedRowCellValue("SablonTipi_Tip2")),
                           gvTanimlar.GetFocusedRowCellValue("URL").ToString(),
                           gvTanimlar.GetFocusedRowCellValue("Username").ToString(),
                           gvTanimlar.GetFocusedRowCellValue("Password").ToString(),
                           DateTime.Now.Date,
                           -1, true, false, Convert.ToBoolean( gvTanimlar.GetFocusedRowCellValue("ServisYonu")));

                    wslist.Add(new S_WebServisTanimlari_Result());

                    var list = new BindingList<S_WebServisTanimlari_Result>(data.S_WebServisTanimlari(-1).ToList());
                    if (list.Count > 0)
                        grdTanimlar.DataSource = list;

                    XtraMessageBox.Show(id + " ID numaralı kayıt güncellenmiştir.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Question);

                }
            }
            else
            {
                data.IUD_WebServisTanimlari(id,
                Convert.ToInt32(gvTanimlar.GetFocusedRowCellValue("FirmaID")),
                Convert.ToInt32(gvTanimlar.GetFocusedRowCellValue("ServisTipi_Tip1")),
                Convert.ToInt32(gvTanimlar.GetFocusedRowCellValue("SablonTipi_Tip2")),
                 gvTanimlar.GetFocusedRowCellValue("URL").ToString(),
                 gvTanimlar.GetFocusedRowCellValue("Username").ToString(),
                 gvTanimlar.GetFocusedRowCellValue("Password").ToString(),
                 DateTime.Now.Date,
                 -1, true, false, Convert.ToBoolean(gvTanimlar.GetFocusedRowCellValue("ServisYonu")));

                wslist.Add(new S_WebServisTanimlari_Result());

                var list = new BindingList<S_WebServisTanimlari_Result>(data.S_WebServisTanimlari(-1).ToList());
                if (list.Count > 0)
                    grdTanimlar.DataSource = list;

                XtraMessageBox.Show("Yeni kayıt eklenmiştir.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Question);

            }

        }

        private void barBtnCalistir_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            xtraTabControl1.SelectedTabPageIndex = 1;
            var list = new BindingList<S_WebServisTanimlari_Result>(data.S_WebServisTanimlari(-1).ToList());
            if (list.Count > 0)
                grdCalistir.DataSource = list.Where(c => c.ServisYonu == false);
        }

        private void barBtnTanimlar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            xtraTabControl1.SelectedTabPageIndex = 0;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                CheckForIllegalCrossThreadCalls = false;

                xtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
                xtraTabControl1.SelectedTabPageIndex = 0;
                //xtraTabControl1.TabPages[0].PageVisible = false;
                //xtraTabControl1.TabPages[1].PageVisible = false;

                var listtip = data.S_Tip(-1, -1).ToList();
                grdServisTipi.DataSource = listtip.Where(c => c.Ayirac == 1).ToList();
                grdSablonTipi.DataSource = listtip.Where(c => c.Ayirac == 2).ToList();
                grdCmbServisTipi.DataSource = listtip.Where(c => c.Ayirac == 1).ToList();
                grdCmbSablonTipi.DataSource = listtip.Where(c => c.Ayirac == 2).ToList();

                var listfirma = data.S_Firma(-1).ToList();
                grdFirmaAdi.DataSource = listfirma;
                grdCmbFirma.DataSource = listfirma;

                var list = new BindingList<S_WebServisTanimlari_Result>(data.S_WebServisTanimlari(-1).ToList());
                if (list.Count > 0)
                    grdTanimlar.DataSource = list;
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString());
                throw;
            }
        }

        public static Thread thread;
        public static string deger = "";
        private void txtBtnCalistir_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                string servistipistr = gvCalistir.GetFocusedRowCellDisplayText("ServisTipi_Tip1");
                if (XtraMessageBox.Show(servistipistr + ", servis tipli kayıdı aktarım işlemine başlatmak istediğinizden eminmisiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    grdCalistir.Enabled = false;
                    grdTanimlar.Enabled = false;

                    int firmaid = Convert.ToInt32(gvCalistir.GetFocusedRowCellValue("FirmaID"));
                    int servistipi = Convert.ToInt32(gvCalistir.GetFocusedRowCellValue("ServisTipi_Tip1"));
                    int sablontipi = Convert.ToInt32(gvCalistir.GetFocusedRowCellValue("SablonTipi_Tip2"));

                    string username = gvCalistir.GetFocusedRowCellValue("Username").ToString();
                    string password = gvCalistir.GetFocusedRowCellValue("Password").ToString();
                    string sirketkodu = gvCalistir.GetFocusedRowCellValue("SirketKodu").ToString();
                    string firmaapikey = gvCalistir.GetFocusedRowCellValue("FirmaApiKey").ToString();

                    Calistir.AktarimBaslat(firmaid, servistipi, sablontipi, username, password, sirketkodu, firmaapikey);
                    //thread = new Thread(() => Calistir.AktarimBaslat(firmaid, servistipi, sablontipi, username, password, sirketkodu, firmaapikey));
                    //thread.Start();

                    grdCalistir.Enabled = true;
                    grdTanimlar.Enabled = true;
                    Cursor.Current = Cursors.Default;

                    XtraMessageBox.Show("Aktarım işlemi Tamamlanmıştır.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString());
                thread.Abort();
            }
        }

        private void grdBtnSil_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            int id = Convert.ToInt32(gvTanimlar.GetFocusedRowCellValue("ID"));
            if (XtraMessageBox.Show(id + " ID numaralı kayıdı silmek istediğinizden eminmisiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                //data.IUD_WebServisTanimlari(id,
                //   -1,
                //   -1,
                //   -1,
                //    "",
                //    "",
                //    "",
                //    DateTime.Now.Date,
                //    -1, false, true);

                data.IUD_WebServisTanimlari(id,
                     Convert.ToInt32(gvTanimlar.GetFocusedRowCellValue("FirmaID")),
                     Convert.ToInt32(gvTanimlar.GetFocusedRowCellValue("ServisTipi_Tip1")),
                     Convert.ToInt32(gvTanimlar.GetFocusedRowCellValue("SablonTipi_Tip2")),
                      gvTanimlar.GetFocusedRowCellValue("URL").ToString(),
                      gvTanimlar.GetFocusedRowCellValue("Username").ToString(),
                      gvTanimlar.GetFocusedRowCellValue("Password").ToString(),
                      DateTime.Now.Date,
                      -1, true, true, Convert.ToBoolean(gvTanimlar.GetFocusedRowCellValue("ServisYonu")));

                //wslist.Add(new S_WebServisTanimlari_Result());
                var list = new BindingList<S_WebServisTanimlari_Result>(data.S_WebServisTanimlari(-1).ToList());
                if (list.Count > 0)
                    grdTanimlar.DataSource = list;

                XtraMessageBox.Show(id + " ID numaralı kayıt silinmiştir.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Question);

            }
        }
    }
}
