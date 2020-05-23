using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Kantar
{
    class KantarTip
    {
        System.IO.Ports.SerialPort serialPort = new System.IO.Ports.SerialPort();    
        public void OpenPort()
        {
            setSerialPort();
            if (serialPort.IsOpen)
            {
                //MessageBox.Show("Port zaten açýk!!!");
                return;
            }

            try
            {
                serialPort.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Port açýlamadý!!!\r\n" + ex.Message);
                ClosePort();
                return;
            }
           
        }

        public void ClosePort()
        {           
            if (!serialPort.IsOpen)
            {
                //MessageBox.Show("Port açýk deðil!!!");
                return;
            }
            try
            {
                serialPort.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Port kapatýlamadý!!!\r\n" + ex.Message);
            }
        }

        private void setSerialPort()
        {
            try
            {
                serialPort.PortName = PublicVariables.Port_Name;
                serialPort.BaudRate = int.Parse(PublicVariables.Port_BaudRate);
                serialPort.DataBits = int.Parse(PublicVariables.Port_DataBits);
                switch (PublicVariables.Port_Parity)
                {
                    case "Even":
                        serialPort.Parity = System.IO.Ports.Parity.Even;
                        return;
                    case "Mark":
                        serialPort.Parity = System.IO.Ports.Parity.Mark;
                        return;
                    case "None":
                        serialPort.Parity = System.IO.Ports.Parity.None;
                        return;
                    case "Odd":
                        serialPort.Parity = System.IO.Ports.Parity.Odd;
                        return;
                    case "Space":
                        serialPort.Parity = System.IO.Ports.Parity.Space;
                        return;
                }
                switch (PublicVariables.Port_StopBits)
                {
                    case "None":
                        serialPort.StopBits = System.IO.Ports.StopBits.None;
                        return;
                    case "One":
                        serialPort.StopBits = System.IO.Ports.StopBits.One;
                        return;
                    case "OnePointFive":
                        serialPort.StopBits = System.IO.Ports.StopBits.OnePointFive;
                        return;
                    case "Two":
                        serialPort.StopBits = System.IO.Ports.StopBits.Two;
                        return;
                }
            }
            catch
            {
                MessageBox.Show("Seri port ayarlarý hatalý!");
            }
        }

        protected int iCurrentWeight = 0;
        protected bool bReadingThePort = false;
        public string GetWeight()
        {
            switch (PublicVariables.KantarTip)
            {
                case "Esit":
                    return readPortForEsit();
                case "Tunaylar" :
                    return readPortForTunaylar();
                case "Tunaylar1":
                    return readPortForTunaylar1();
                case "Tunaylar2":
                    return readPortForTunaylar2();
                case "EsitAdana":
                    return readPortForEsitAdana();
                case "Piyale":
                    return readPortForPiyale();
                case "Marsan":
                    return readPortForMarsan();
                case "Test":
                    return readPortTest();
                case "All":
                    return readPortForAll();

                default:
                    MessageBox.Show("Kantar tipi tanýmsýz!","Hata",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return "-1";
            }
        }
        private string readPortTest()
        {
            return "11111";
        }

        //kantar Read Fonksiyonu : Eðer hata varsa "-1" döndür, Eðer veri gelmediyse "" döndür.
        // "" döndürüldüðü zaman ekrandaki aðýrlýk yenilenmiyecek!
        private string readPortForEsit()
        {
            string strPortBuffer, strReadBlock;
            int pos13 = -1;

            if (bReadingThePort)
            {
                //txtReadPort.SelectedText = "\r\nREADING.....";
                return "";
            }

            bReadingThePort = true;
            //txtReadPort.SelectedText = "\r\n###################################################################";

            if (serialPort.IsOpen)
            {
                // Must be min 8 bytes
                if (serialPort.BytesToRead < 8)
                { bReadingThePort = false; return ""; }

                // Read all buffer
                strPortBuffer = "";
                strPortBuffer = serialPort.ReadExisting();
                //txtReadPort.SelectedText = "\r\nBUFFER: " + strPortBuffer;

                // Find last enter char (13)
                pos13 = -1;
                for (int i = strPortBuffer.Length - 1; i >= 0; i++)
                    if (strPortBuffer[i] == 13)
                    {
                        pos13 = i;
                        break;
                    }
                //txtReadPort.SelectedText = "\r\nPOS: " + pos13.ToString();

                // Not found
                if (pos13 < 7) { bReadingThePort = false; return "-1"; }

                // read block
                strReadBlock = strPortBuffer.Substring(pos13 - 7, 7);
                //txtReadPort.SelectedText = "\r\nBLOCK: " + strReadBlock;

                // find weight
                iCurrentWeight = Convert.ToInt32(strReadBlock.Substring(1));
                //txtReadPort.SelectedText = "\r\nWEIGHT: " + iCurrentWeight.ToString();
                //txtReadPort.SelectedText = "\r\n" + iCurrentWeight.ToString();

                bReadingThePort = false;
                // write to screen               
                return  iCurrentWeight.ToString();
            }
            return "-1";
        }

        private string readPortForTunaylar()
        {
            string strPortBuffer, strReadBlock;

            if (bReadingThePort)
            {               
                return "";
            }

            bReadingThePort = true;
            //txtReadPort.SelectedText = "\r\n###################################################################";

            if (serialPort.IsOpen)
            {
                // Must be min 8 bytes
                if (serialPort.BytesToRead < 8) { bReadingThePort = false; return ""; }

                // Read all buffer
                strPortBuffer = "";
                strPortBuffer = serialPort.ReadExisting();
                //txtReadPort.SelectedText = "\r\nBUFFER: " + strPortBuffer;

                //
                // kantardan gelen veri yapýsý : (2) + 61 | 41 + 0000000 + (3)
                //

                // Find last  char (3)
                int pos3 = strPortBuffer.LastIndexOf((char)3);

                // Not found
                if (pos3 < 7) { bReadingThePort = false; return "-1"; }

                // read block
                strReadBlock = strPortBuffer.Substring(pos3 - 7, 7);
                //txtReadPort.SelectedText = "\r\nBLOCK: " + strReadBlock;

                // find weight
                iCurrentWeight = Convert.ToInt32(strReadBlock.Substring(1));

                bReadingThePort = false;
                // write to screen  
                return iCurrentWeight.ToString();
                
            }
            else
            {
                OpenPort();
            }
            return "-1";
        }

        private string readPortForTunaylar1()
        {
            string strPortBuffer;
            //Data doluyken gelen data:
            //strPortBuffer = "!10 000080 000000";
            //Data boþken gelen data:
            //)10 000000 000000
            if (bReadingThePort) {return "";}

            bReadingThePort = true;

            if (serialPort.IsOpen)
            {
                // Must be min 8 bytes
                if (serialPort.BytesToRead < 8) { bReadingThePort = false; return ""; }

                // Read all buffer
                strPortBuffer = "";
                strPortBuffer = serialPort.ReadExisting();
                try
                {
                    if (strPortBuffer.Substring(1, 1) == ")")
                    {
                        iCurrentWeight = 0;
                        bReadingThePort = false;
                        return iCurrentWeight.ToString();
                    }
                    else
                    {
                        int indx = strPortBuffer.IndexOf("!");
                        if (indx == -1)
                        {
                            iCurrentWeight = 0;
                            bReadingThePort = false;
                            return iCurrentWeight.ToString();
                        }
                        else
                        {
                            strPortBuffer = strPortBuffer.Substring(indx + 4, 6);
                            iCurrentWeight = Convert.ToInt32(strPortBuffer);
                            bReadingThePort = false;
                            return iCurrentWeight.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    iCurrentWeight = 0;
                    bReadingThePort = false;
                    return iCurrentWeight.ToString();
                }
            }
            else
            {
                OpenPort();
            }
            return "-1";
        }

        private string readPortForTunaylar2()
        {
            string strPortBuffer;
            //Data doluyken gelen data:
            //strPortBuffer = "23800";
            //Data boþken gelen data:
            //
            if (bReadingThePort) { return ""; }

            bReadingThePort = true;

            if (serialPort.IsOpen)
            {
                // Must be min 7 bytes
                if (serialPort.BytesToRead < 7) { bReadingThePort = false; return ""; }

                // Read all buffer
                strPortBuffer = "";
                strPortBuffer = serialPort.ReadExisting();
                try
                {
                    iCurrentWeight = Convert.ToInt32(strPortBuffer.Trim());
                    bReadingThePort = false;
                    return iCurrentWeight.ToString();
                }
                catch (Exception ex)
                {
                    iCurrentWeight = 0;
                    bReadingThePort = false;
                    return iCurrentWeight.ToString();
                }
                //if (strPortBuffer.Substring(1, 1) == ")")
                //{
                //    iCurrentWeight = 0;
                //    bReadingThePort = false;
                //    return iCurrentWeight.ToString();
                //}
                //else
                //{
                //    int indx = strPortBuffer.IndexOf("!");
                //    if (indx == -1)
                //    {
                //        iCurrentWeight = 0;
                //        bReadingThePort = false;
                //        return iCurrentWeight.ToString();
                //    }
                //    else
                //    {
                //        strPortBuffer = strPortBuffer.Substring(indx + 4, 6);
                //        iCurrentWeight = Convert.ToInt32(strPortBuffer);
                //        bReadingThePort = false;
                //        return iCurrentWeight.ToString();
                //    }
                //}
            }
            else
            {
                OpenPort();
            }
            return "-1";
        }

        private string readPortForEsitAdana()
        {
            string strPortBuffer;

            if (bReadingThePort)
            {
                //txtReadPort.SelectedText = "\r\nREADING.....";
                return "";
            }

            bReadingThePort = true;
            //txtReadPort.SelectedText = "\r\n###################################################################";

            if (serialPort.IsOpen)
            {
                // Must be min 8 bytes
                if (serialPort.BytesToRead < 8)
                { bReadingThePort = false; return ""; }

                // Read all buffer
                strPortBuffer = "";
                strPortBuffer = serialPort.ReadExisting();
                try
                {
                    //strPortBuffer = "@   540";
                    strPortBuffer = strPortBuffer.Substring(strPortBuffer.IndexOf("@"), 7).Replace("@", "").Trim();
                    strPortBuffer = strPortBuffer.Trim();
                }
                catch
                {
                    strPortBuffer = "0";
                }
                try
                {
                    iCurrentWeight = Convert.ToInt32(strPortBuffer);
                }
                catch
                {
                    iCurrentWeight = 0;
                }

                bReadingThePort = false;
                // write to screen               
                return iCurrentWeight.ToString();
            }
            return "-1";
        }

        private string readPortForPiyale()
        {
            string strPortBuffer;
            //Data doluyken gelen data:
            //strPortBuffer = "23800";
            //Data boþken gelen data:
            //
            if (bReadingThePort) { return ""; }

            bReadingThePort = true;

            if (serialPort.IsOpen)
            {
                // Must be min 7 bytes
                if (serialPort.BytesToRead < 7) { bReadingThePort = false; return ""; }

                // Read all buffer
                strPortBuffer = "";
                strPortBuffer = serialPort.ReadExisting();
                try
                {
                    iCurrentWeight = Convert.ToInt32(strPortBuffer.Substring(1,6).Trim());
                    bReadingThePort = false;
                    return iCurrentWeight.ToString();
                }
                catch (Exception ex)
                {
                    iCurrentWeight = 0;
                    bReadingThePort = false;
                    return iCurrentWeight.ToString();
                }
            }
            else
            {
                OpenPort();
            }
            return "-1";
        }

        private string readPortForAll()
        {
            string strPortBuffer;
            //Data doluyken gelen data:
            //strPortBuffer = "23800";
            //Data boþken gelen data:
            //
            if (bReadingThePort) { return ""; }

            bReadingThePort = true;

            if (serialPort.IsOpen)
            {
                // Must be min 7 bytes
                if (serialPort.BytesToRead < 7) { bReadingThePort = false; return ""; }

                // Read all buffer
                strPortBuffer = "";
                strPortBuffer = serialPort.ReadExisting();
                try
                {
                    iCurrentWeight = Convert.ToInt32(strPortBuffer.Substring(PublicVariables.startindex , PublicVariables.lenght).Trim());
                    bReadingThePort = false;
                    return iCurrentWeight.ToString();
                }
                catch (Exception ex)
                {
                    iCurrentWeight = 0;
                    bReadingThePort = false;
                    return iCurrentWeight.ToString();
                }
            }
            else
            {
                OpenPort();
            }
            return "-1";
        }
        private string readPortForMarsan()
        {

            string strPortBuffer;
            //Data doluyken gelen data:
            //strPortBuffer = "23800";
            //Data boþken gelen data:
            //
            if (bReadingThePort) { return ""; }

            bReadingThePort = true;

            if (serialPort.IsOpen)
            {
                // Must be min 7 bytes
                if (serialPort.BytesToRead < 7) { bReadingThePort = false; return ""; }

                // Read all buffer
                strPortBuffer = "";
                strPortBuffer = serialPort.ReadExisting();
                strPortBuffer = strPortBuffer.Substring(4, 6).Trim() ;
                try
                {
                    iCurrentWeight = Convert.ToInt32(strPortBuffer);
                    bReadingThePort = false;
                    return iCurrentWeight.ToString();
                }
                catch (Exception ex)
                {
                    iCurrentWeight = 0;
                    bReadingThePort = false;
                    return iCurrentWeight.ToString();
                }
            }
            else
            {
                OpenPort();
            }
            return "-1";
        
        }
    }
}
