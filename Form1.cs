using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoapTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            toolTip1.SetToolTip(textBox1, "URL of the Webservice");
            toolTip2.SetToolTip(textBox4, "SOAPACTION field");
            toolTip3.SetToolTip(textBox2, "SOAP Document Entry");
            toolTip4.SetToolTip(textBox3, "Response from the Server");
        }

        private void button1_Click(object sender, EventArgs e)
        {

            System.Net.WebRequest r = null;
            try
            {
                r = System.Net.WebRequest.Create(textBox1.Text);
            }catch(Exception cv)
            {
                textBox2.Text = cv.Message;
                return;
            }
            r.ContentType = "text/xml";
            //r.Headers.Add("Content-Type:  application/soap+xml; charset=utf-8");
            //r.Headers.Add("Content-Length:  " + textBox2.Text.Length.ToString());
            r.Method = "POST";
            if(!String.IsNullOrWhiteSpace(textBox4.Text) ) { r.Headers.Add("SOAPAction: \"" + textBox4.Text + "\""); }
            System.IO.Stream reqstream = r.GetRequestStream();
            System.IO.StreamWriter sw = new System.IO.StreamWriter(reqstream);
            sw.Write(textBox2.Text);
            sw.Flush();
            try
            {
                //r.ContentLength = textBox2.Text.Length; //reqstream.Position;
                System.Net.WebResponse resp = r.GetResponse();

                System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                textBox3.Text = sr.ReadToEnd();
                sr.Dispose();
                resp.Dispose();
            }
            catch (Exception ext)
            {
                textBox3.Text = ext.Message + "\r\n" + ext.StackTrace;
                textBox3.Text += "\r\n";
                if (ext is System.Net.WebException)
                {
                    System.Net.WebException webex = (System.Net.WebException)ext;
                    try
                    {
                        using (System.IO.StreamReader exceptionreader = new System.IO.StreamReader(webex.Response.GetResponseStream()))
                        {
                            textBox3.Text += exceptionreader.ReadToEnd();
                        }
                    }catch(Exception ffff)
                    {
                        textBox3.Text += "\r\n Unknown Error trying to read response from Server";
                    }
                }
            }
            finally
            {
                sw.Dispose();
            }
            tabControl1.SelectedTab = tabPage2;

        }
    }
}
