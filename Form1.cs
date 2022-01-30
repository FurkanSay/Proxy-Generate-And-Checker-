using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SetProxy;

namespace GenerateProxy
{
    public partial class Form1 : Form
    {   public int count = 0;
        string Allproxies = "";
        ArrayList allproxies = new ArrayList();
        List<String> allproxies1 = new List<string>();
        string proxyWorkornotwork;


        public Form1()
        {
            
            InitializeComponent();
            timer1.Interval = 1000;
        }

        private void btn_start_Click(object sender, EventArgs e)
        { // Proxy Çekilir Listbox all_proxy
            richTextBox__AllProxy.Text = "";
            MessageBox.Show("Proxyler Çekilmektedir. Lütfen Bekleyin...");
            pictureBox1.Visible = true;
            String selected =comboBox_Select.Text;
            if (selected.Equals("Turkey"))
            {
                
                listBox_AllProxy.Items.Clear();
                Spysone spysone = new Spysone();
                ArrayList proxy80 = new ArrayList();
                ArrayList proxy8080 = new ArrayList();
                proxy80 = spysone.SpysoneGetTR80(webBrowser1);
                proxy8080 = spysone.SpysoneGetTR8080(webBrowser1);

                foreach (string ip in proxy80)
                { string porxyport = ip + ":80";
                    allproxies.Add(porxyport);
                    
                    listBox_AllProxy.Items.Add(porxyport);
                    Allproxies += porxyport + "\n";
                }
                foreach (string ip in proxy8080)
                {
                    string porxyport = ip + ":8080";
                    allproxies.Add(porxyport);
                  
                    listBox_AllProxy.Items.Add(porxyport);
                    Allproxies += porxyport + "\n";
                }
                richTextBox__AllProxy.Text = Allproxies;
                label_All_Count.Text = listBox_AllProxy.Items.Count.ToString();
                pictureBox1.Visible = false;


            }
            if (selected.Equals("World"))
            { 
                listBox_AllProxy.Items.Clear();
                Spysone spysone = new Spysone();
                ArrayList proxy80 = new ArrayList();
                ArrayList proxy8080 = new ArrayList();
                proxy80 = spysone.SpysoneGetWorld80(webBrowser1);
                proxy8080 = spysone.SpysoneGetWorld8080(webBrowser1);
               
                foreach (string ip in proxy80)
                {
                    string porxyport = ip + ":80";
                    allproxies.Add(porxyport);
                   
                    listBox_AllProxy.Items.Add(porxyport);
                    Allproxies += porxyport + "\n";
                }


                foreach (string ip in proxy8080)
                {
                    string porxyport = ip + ":8080";
                    allproxies.Add(porxyport);
                    
                    listBox_AllProxy.Items.Add(porxyport);
                    Allproxies += porxyport + "\n";
                }
                richTextBox__AllProxy.Text = Allproxies;
                label_All_Count.Text = listBox_AllProxy.Items.Count.ToString();
                pictureBox1.Visible = false;

            }
        }

        public void proxiesandports(List<String> proxyandport)
        {
            listBox_proxy.Items.Clear();
            foreach (string proxy in proxyandport)
            {
               label_showproxy.Text = "Şimdi Bu Proxy Kontrol Ediliyor: " + proxy;
                WinInetInterop.SetConnectionProxy(proxy);
                Stopwatch watch = new Stopwatch();
                string url = "https://www.google.com/search?sxsrf=ALeKk009TFektV5y0IIxLnoxJ0LkNgs8HA%3A1613178975496&source=hp&ei=XygnYIzUG4L4U4n2u4gO&iflsig=AINFCbYAAAAAYCc2bz3rO9cMc4XnP_P33YwRUS-_QfLr&q=what+is+my+ip&oq=what+&gs_lcp=Cgdnd3Mtd2l6EAMYADIECCMQJzIECCMQJzIECCMQJzIFCAAQsQMyAgguMgUIABCxAzIICAAQsQMQgwEyAggAMgIILjICCAA6CwgAELEDEMcBEKMCOggILhCxAxCDAToECAAQQzoHCAAQsQMQQ1CLBViTDGCNFGgAcAB4AIABowGIAZkGkgEDMC41mAEAoAEBqgEHZ3dzLXdpeg&sclient=gws-wiz";
                watch.Start();
                webBrowser2.Navigate(url);
                timer1.Start();
                WB_Yuklenmesini_Bekle(webBrowser2);
                timer1.Stop();
                watch.Stop();
                Console.WriteLine(count+" Toplam Gecen süre");
                //MessageBox.Show(webBrowser.Document.Body.InnerText.Trim());
                Console.WriteLine("Bağlanti süresi :{0}", watch.Elapsed.Seconds + "saniye");
                
                    proxyWorkornotwork = "";
                    proxyWorkornotwork = proxyWorkornotwork + webBrowser2.Document?.Body?.InnerText.Trim();
                Console.WriteLine(proxyWorkornotwork.Length);
                if (proxyWorkornotwork.Length >0) 
                {
                    if (proxyWorkornotwork.Substring(0, 1).Equals("A"))
                    {
                        label_showproxy.Text = "Bu Proxy Aktif: " + proxy;
                        listBox_proxy.Items.Add(proxy);
                        count = 0;
                    }

                    if (proxyWorkornotwork.Substring(0, 1).Equals("C") || proxyWorkornotwork.Substring(0, 1).Equals("T") || proxyWorkornotwork.Substring(0, 1).Equals("N") || proxyWorkornotwork.Substring(0, 1).Equals("B"))
                    {
                        label_showproxy.Text = "Bu Proxy Aktif Değil: " + proxy;
                        count = 0;
                    }
                    else
                    {
                        label_showproxy.Text = "Bu Proxy Aktif: " + proxy;
                        listBox_proxy.Items.Add(proxy);
                        count = 0;
                    }


                }
                else
                {
                    count = 0;
                }
            }
        }



        public void WB_Yuklenmesini_Bekle(WebBrowser webBrowser)
        {
            try
            {
                WebBrowserReadyState loadStatus = default(WebBrowserReadyState);
                int beklemeSuresi = 50;
                int sayac = 0;
                while (true)
                {
                    loadStatus = webBrowser.ReadyState;
                    Application.DoEvents();

                    if ((sayac > beklemeSuresi) || (loadStatus == WebBrowserReadyState.Uninitialized) || (loadStatus == WebBrowserReadyState.Loading) || (loadStatus == WebBrowserReadyState.Interactive))
                    {
                        break;
                    }
                    sayac += 1;
                }
                sayac = 0;
                while (true)
                {
                    loadStatus = webBrowser.ReadyState;
                    Application.DoEvents();

                    if (loadStatus == WebBrowserReadyState.Complete || count ==10)
                    {
                        break;
                    }

                    sayac += 1;
                }

            }
            catch { }
        }

        private void btn_CheckProxy_Click(object sender, EventArgs e)
        {//Proxy Kontrol edilir Listbox Work_Proxy
            MessageBox.Show("Proxyler kontrol ediliyor. Lütfen Bekleyin...");
            pictureBox1.Visible = true;

            allproxies1 = richTextBox__AllProxy.Lines.ToList<String>();


           proxiesandports(allproxies1);
            label_Work_Count.Text = listBox_proxy.Items.Count.ToString();
            pictureBox1.Visible = false;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {   //txt dosyasına kayıt edilir
            String source = ("CheckkedProxy.txt");
            StreamWriter wr = File.CreateText(source);
            foreach(string i in allproxies1)
            {
                wr.Write(i+"\n");
            }
            
            wr.Close();
            Process.Start("notepad.exe", source);

            String source1 = ("AllProxy.txt");
            StreamWriter wr1 = File.CreateText(source1);
            foreach (string i in allproxies)
            {
                wr1.Write(i + "\n");
            }

            wr1.Close();
            Process.Start("notepad.exe", source1);
        }

      
        private bool mouseDown;
        private Point lastLocation;
        

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                   (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();

            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {

            mouseDown = false;
        
        }
        

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        

        private void timer1_Tick(object sender, EventArgs e)
        {
            count++;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            
                webBrowser3.Navigate("www.youtube.com/watch?v=c84kE3UG1UA");
            WebBrowserReadyState loadStatus = default(WebBrowserReadyState);
            loadStatus = webBrowser3.ReadyState;
            Application.DoEvents();

            if (loadStatus == WebBrowserReadyState.Complete)
            {
                SendKeys.Send("m");
            }
            
            
            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
          

        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void webBrowser3_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

            if (Convert.ToInt32(e.KeyValue) == 109 || Convert.ToInt32(e.KeyValue) == 77)
            {
                e.IsInputKey = true;
            }
        }
    }
}
