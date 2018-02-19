namespace YandereSpider
{
    partial class MainForm
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.cancelButton = new System.Windows.Forms.Button();
            this.goThroughButton = new System.Windows.Forms.Button();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.imageLinkTextBox = new System.Windows.Forms.TextBox();
            this.extractButton = new System.Windows.Forms.Button();
            this.copyButton = new System.Windows.Forms.Button();
            this.clearButton = new System.Windows.Forms.Button();
            this.backButton = new System.Windows.Forms.Button();
            this.forwardButton = new System.Windows.Forms.Button();
            this.homeButton = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.addressTextBox = new System.Windows.Forms.TextBox();
            this.webBrowserDocumentTextCompleteCheckBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 7;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel.Controls.Add(this.cancelButton, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.goThroughButton, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.webBrowser, 2, 1);
            this.tableLayoutPanel.Controls.Add(this.imageLinkTextBox, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.extractButton, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.copyButton, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.clearButton, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.backButton, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.forwardButton, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.homeButton, 4, 0);
            this.tableLayoutPanel.Controls.Add(this.refreshButton, 6, 0);
            this.tableLayoutPanel.Controls.Add(this.addressTextBox, 5, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 4;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(1008, 537);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // cancelButton
            // 
            this.cancelButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cancelButton.Location = new System.Drawing.Point(108, 42);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(2);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(102, 36);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "取消遍历";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // goThroughButton
            // 
            this.goThroughButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.goThroughButton.Location = new System.Drawing.Point(2, 42);
            this.goThroughButton.Margin = new System.Windows.Forms.Padding(2);
            this.goThroughButton.Name = "goThroughButton";
            this.goThroughButton.Size = new System.Drawing.Size(102, 36);
            this.goThroughButton.TabIndex = 1;
            this.goThroughButton.Text = "遍历页面";
            this.goThroughButton.UseVisualStyleBackColor = true;
            this.goThroughButton.Click += new System.EventHandler(this.GoThroughButton_Click);
            // 
            // webBrowser
            // 
            this.tableLayoutPanel.SetColumnSpan(this.webBrowser, 5);
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.Location = new System.Drawing.Point(214, 42);
            this.webBrowser.Margin = new System.Windows.Forms.Padding(2);
            this.webBrowser.MinimumSize = new System.Drawing.Size(16, 16);
            this.webBrowser.Name = "webBrowser";
            this.tableLayoutPanel.SetRowSpan(this.webBrowser, 3);
            this.webBrowser.ScriptErrorsSuppressed = true;
            this.webBrowser.Size = new System.Drawing.Size(792, 493);
            this.webBrowser.TabIndex = 11;
            this.webBrowser.Url = new System.Uri("https://yande.re", System.UriKind.Absolute);
            this.webBrowser.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.WebBrowser_Navigated);
            // 
            // imageLinkTextBox
            // 
            this.imageLinkTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.tableLayoutPanel.SetColumnSpan(this.imageLinkTextBox, 2);
            this.imageLinkTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageLinkTextBox.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.imageLinkTextBox.Location = new System.Drawing.Point(2, 122);
            this.imageLinkTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.imageLinkTextBox.Multiline = true;
            this.imageLinkTextBox.Name = "imageLinkTextBox";
            this.imageLinkTextBox.ReadOnly = true;
            this.imageLinkTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.imageLinkTextBox.Size = new System.Drawing.Size(208, 413);
            this.imageLinkTextBox.TabIndex = 5;
            // 
            // extractButton
            // 
            this.tableLayoutPanel.SetColumnSpan(this.extractButton, 2);
            this.extractButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extractButton.Location = new System.Drawing.Point(2, 2);
            this.extractButton.Margin = new System.Windows.Forms.Padding(2);
            this.extractButton.Name = "extractButton";
            this.extractButton.Size = new System.Drawing.Size(208, 36);
            this.extractButton.TabIndex = 0;
            this.extractButton.Text = "提取链接";
            this.extractButton.UseVisualStyleBackColor = true;
            this.extractButton.Click += new System.EventHandler(this.ExtractButton_Click);
            // 
            // copyButton
            // 
            this.copyButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.copyButton.Location = new System.Drawing.Point(2, 82);
            this.copyButton.Margin = new System.Windows.Forms.Padding(2);
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new System.Drawing.Size(102, 36);
            this.copyButton.TabIndex = 3;
            this.copyButton.Text = "复制链接";
            this.copyButton.UseVisualStyleBackColor = true;
            this.copyButton.Click += new System.EventHandler(this.CopyButton_Click);
            // 
            // clearButton
            // 
            this.clearButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clearButton.Location = new System.Drawing.Point(108, 82);
            this.clearButton.Margin = new System.Windows.Forms.Padding(2);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(102, 36);
            this.clearButton.TabIndex = 4;
            this.clearButton.Text = "清除列表";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // backButton
            // 
            this.backButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backButton.Location = new System.Drawing.Point(214, 2);
            this.backButton.Margin = new System.Windows.Forms.Padding(2);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(36, 36);
            this.backButton.TabIndex = 6;
            this.backButton.Text = "←";
            this.backButton.UseVisualStyleBackColor = true;
            this.backButton.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // forwardButton
            // 
            this.forwardButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.forwardButton.Location = new System.Drawing.Point(254, 2);
            this.forwardButton.Margin = new System.Windows.Forms.Padding(2);
            this.forwardButton.Name = "forwardButton";
            this.forwardButton.Size = new System.Drawing.Size(36, 36);
            this.forwardButton.TabIndex = 7;
            this.forwardButton.Text = "→";
            this.forwardButton.UseVisualStyleBackColor = true;
            this.forwardButton.Click += new System.EventHandler(this.ForwardButton_Click);
            // 
            // homeButton
            // 
            this.homeButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.homeButton.Location = new System.Drawing.Point(294, 2);
            this.homeButton.Margin = new System.Windows.Forms.Padding(2);
            this.homeButton.Name = "homeButton";
            this.homeButton.Size = new System.Drawing.Size(36, 36);
            this.homeButton.TabIndex = 8;
            this.homeButton.Text = "🏠";
            this.homeButton.UseVisualStyleBackColor = true;
            this.homeButton.Click += new System.EventHandler(this.HomeButton_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.refreshButton.Location = new System.Drawing.Point(970, 2);
            this.refreshButton.Margin = new System.Windows.Forms.Padding(2);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(36, 36);
            this.refreshButton.TabIndex = 10;
            this.refreshButton.Text = "🔄";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // addressTextBox
            // 
            this.addressTextBox.AutoSize = false;
            this.addressTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.addressTextBox.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.addressTextBox.Location = new System.Drawing.Point(334, 2);
            this.addressTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.addressTextBox.Name = "addressTextBox";
            this.addressTextBox.Size = new System.Drawing.Size(632, 34);
            this.addressTextBox.TabIndex = 9;
            this.addressTextBox.Text = "https://yande.re";
            this.addressTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AddressTextBox_KeyDown);
            // 
            // webBrowserDocumentTextCompleteCheckBackgroundWorker
            // 
            this.webBrowserDocumentTextCompleteCheckBackgroundWorker.WorkerReportsProgress = true;
            this.webBrowserDocumentTextCompleteCheckBackgroundWorker.WorkerSupportsCancellation = true;
            this.webBrowserDocumentTextCompleteCheckBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.WebBrowserDocumentTextCompleteCheckBackgroundWorker_DoWork);
            this.webBrowserDocumentTextCompleteCheckBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.WebBrowserDocumentTextCompleteCheckBackgroundWorker_ProgressChanged);
            this.webBrowserDocumentTextCompleteCheckBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.WebBrowserDocumentTextCompleteCheckBackgroundWorker_RunWorkerCompleted);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1008, 537);
            this.Controls.Add(this.tableLayoutPanel);
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "yande.re 图片链接爬虫";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.TextBox imageLinkTextBox;
        private System.Windows.Forms.Button extractButton;
        private System.Windows.Forms.Button copyButton;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Button goThroughButton;
        private System.Windows.Forms.Button cancelButton;
        private System.ComponentModel.BackgroundWorker webBrowserDocumentTextCompleteCheckBackgroundWorker;
        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.Button forwardButton;
        private System.Windows.Forms.Button homeButton;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.TextBox addressTextBox;
    }
}

