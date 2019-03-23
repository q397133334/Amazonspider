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
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            Core.Setting.Path = Application.StartupPath;
            this.Text = this.Text + Application.ProductVersion;
        }

    }
}
