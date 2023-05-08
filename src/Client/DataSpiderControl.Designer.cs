namespace Amazonspider.Client
{
    partial class DataSpiderControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtRegisterCode = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSoftCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btsSaveSetting = new System.Windows.Forms.Button();
            this.gbxRunInfo = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.runInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gbxRunLog = new System.Windows.Forms.GroupBox();
            this.listboxLog = new System.Windows.Forms.ListBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnAddAsin = new System.Windows.Forms.Button();
            this.txtAsin = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbxRunInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.runInfoBindingSource)).BeginInit();
            this.gbxRunLog.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.gbxRunInfo, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.gbxRunLog, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.groupBox4, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 69F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 61.58798F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 38.41202F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1455, 692);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.txtRegisterCode);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtSoftCode);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btsSaveSetting);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 72);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1449, 69);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设置";
            // 
            // txtRegisterCode
            // 
            this.txtRegisterCode.Location = new System.Drawing.Point(377, 30);
            this.txtRegisterCode.Name = "txtRegisterCode";
            this.txtRegisterCode.Size = new System.Drawing.Size(204, 21);
            this.txtRegisterCode.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(308, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "注册码：";
            // 
            // txtSoftCode
            // 
            this.txtSoftCode.Location = new System.Drawing.Point(80, 30);
            this.txtSoftCode.Name = "txtSoftCode";
            this.txtSoftCode.Size = new System.Drawing.Size(204, 21);
            this.txtSoftCode.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "机器码：";
            // 
            // btsSaveSetting
            // 
            this.btsSaveSetting.Location = new System.Drawing.Point(607, 30);
            this.btsSaveSetting.Name = "btsSaveSetting";
            this.btsSaveSetting.Size = new System.Drawing.Size(83, 21);
            this.btsSaveSetting.TabIndex = 7;
            this.btsSaveSetting.Text = "保存";
            this.btsSaveSetting.UseVisualStyleBackColor = true;
            this.btsSaveSetting.Click += new System.EventHandler(this.btsSaveSetting_Click);
            // 
            // gbxRunInfo
            // 
            this.gbxRunInfo.Controls.Add(this.dataGridView1);
            this.gbxRunInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxRunInfo.Location = new System.Drawing.Point(3, 147);
            this.gbxRunInfo.Name = "gbxRunInfo";
            this.gbxRunInfo.Size = new System.Drawing.Size(1449, 331);
            this.gbxRunInfo.TabIndex = 0;
            this.gbxRunInfo.TabStop = false;
            this.gbxRunInfo.Text = "运行信息";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.DataSource = this.runInfoBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 17);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1443, 311);
            this.dataGridView1.TabIndex = 1;
            // 
            // gbxRunLog
            // 
            this.gbxRunLog.Controls.Add(this.listboxLog);
            this.gbxRunLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxRunLog.Location = new System.Drawing.Point(3, 484);
            this.gbxRunLog.Name = "gbxRunLog";
            this.gbxRunLog.Size = new System.Drawing.Size(1449, 205);
            this.gbxRunLog.TabIndex = 1;
            this.gbxRunLog.TabStop = false;
            this.gbxRunLog.Text = "运行日志";
            // 
            // listboxLog
            // 
            this.listboxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listboxLog.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listboxLog.FormattingEnabled = true;
            this.listboxLog.Location = new System.Drawing.Point(3, 17);
            this.listboxLog.Margin = new System.Windows.Forms.Padding(2);
            this.listboxLog.Name = "listboxLog";
            this.listboxLog.Size = new System.Drawing.Size(1443, 185);
            this.listboxLog.TabIndex = 1;
            this.listboxLog.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listboxLog_DrawItem);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button2);
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Controls.Add(this.btnStop);
            this.groupBox4.Controls.Add(this.btnStart);
            this.groupBox4.Controls.Add(this.btnAddAsin);
            this.groupBox4.Controls.Add(this.txtAsin);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(3, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(1449, 63);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "操作";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(611, 20);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "保存登陆状态";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(530, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "登陆亚马逊";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(391, 20);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 4;
            this.btnStop.Text = "暂停";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(310, 20);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "开始";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnAddAsin
            // 
            this.btnAddAsin.Location = new System.Drawing.Point(206, 19);
            this.btnAddAsin.Name = "btnAddAsin";
            this.btnAddAsin.Size = new System.Drawing.Size(75, 23);
            this.btnAddAsin.TabIndex = 2;
            this.btnAddAsin.Text = "添加";
            this.btnAddAsin.UseVisualStyleBackColor = true;
            this.btnAddAsin.Click += new System.EventHandler(this.btnAddAsin_Click);
            // 
            // txtAsin
            // 
            this.txtAsin.Location = new System.Drawing.Point(65, 20);
            this.txtAsin.Name = "txtAsin";
            this.txtAsin.Size = new System.Drawing.Size(135, 21);
            this.txtAsin.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "Asin:";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // DataSpiderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "DataSpiderControl";
            this.Size = new System.Drawing.Size(1455, 692);
            this.Load += new System.EventHandler(this.DataSpiderControl_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbxRunInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.runInfoBindingSource)).EndInit();
            this.gbxRunLog.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        //private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox gbxRunInfo;
        private System.Windows.Forms.GroupBox gbxRunLog;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ListBox listboxLog;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtAsin;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnAddAsin;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.BindingSource runInfoBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn runtimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pluginnameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn simulatornameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn runmsgDataGridViewTextBoxColumn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtRegisterCode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSoftCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btsSaveSetting;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
    }
}
