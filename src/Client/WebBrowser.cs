using CefSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Amazonspider.Client
{
    public partial class WebBrowser : Form
    {

        public Func<List<Cookie>> GetCookieEvent;

        public WebBrowser()
        {
            InitializeComponent();
        }

        private void WebBrowser_Load(object sender, EventArgs e)
        {
            chromiumWebBrowser1.LoadUrl("https://www.amazon.com/");

            GetCookieEvent = () =>
            {
                return chromiumWebBrowser1.GetCookieManager().VisitAllCookiesAsync().Result;
            };
        }
    }
}
