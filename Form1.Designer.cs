namespace PageComparison
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            tableLayoutPanel1 = new TableLayoutPanel();
            mainTableLayoutPanel = new TableLayoutPanel();
            button1 = new Button();
            button2 = new Button();
            pictureBox1 = new PictureBox();
            applicationTitleTextBox = new TextBox();
            addButton = new Button();
            tableLayoutPanel3 = new TableLayoutPanel();
            tableLayoutPanel4 = new TableLayoutPanel();
            firstRichTextBox = new RichTextBox();
            secondRichTextBox = new RichTextBox();
            outputpictureBox = new PictureBox();
            fileTitleTextBox = new TextBox();
            fileTextBox = new TextBox();
            tableLayoutPanel1.SuspendLayout();
            mainTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)outputpictureBox).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Outset;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25.3333321F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 74.66667F));
            tableLayoutPanel1.Controls.Add(mainTableLayoutPanel, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 1, 0);
            tableLayoutPanel1.Location = new Point(36, 13);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 21F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 21F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 21F));
            tableLayoutPanel1.Size = new Size(1203, 626);
            tableLayoutPanel1.TabIndex = 4;
            // 
            // mainTableLayoutPanel
            // 
            mainTableLayoutPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            mainTableLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;
            mainTableLayoutPanel.ColumnCount = 1;
            mainTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainTableLayoutPanel.Controls.Add(button1, 0, 2);
            mainTableLayoutPanel.Controls.Add(button2, 0, 3);
            mainTableLayoutPanel.Controls.Add(pictureBox1, 0, 4);
            mainTableLayoutPanel.Controls.Add(applicationTitleTextBox, 0, 0);
            mainTableLayoutPanel.Controls.Add(addButton, 0, 1);
            mainTableLayoutPanel.Location = new Point(2, 2);
            mainTableLayoutPanel.Margin = new Padding(0);
            mainTableLayoutPanel.Name = "mainTableLayoutPanel";
            mainTableLayoutPanel.RowCount = 5;
            mainTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            mainTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));
            mainTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 15.967742F));
            mainTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 14.83871F));
            mainTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 44.0322571F));
            mainTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            mainTableLayoutPanel.Size = new Size(303, 622);
            mainTableLayoutPanel.TabIndex = 0;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            button1.BackColor = Color.LightSkyBlue;
            button1.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            button1.Location = new Point(2, 158);
            button1.Margin = new Padding(0);
            button1.Name = "button1";
            button1.Size = new Size(299, 97);
            button1.TabIndex = 9;
            button1.Text = "多文档比对";
            button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            button2.BackColor = Color.LightSkyBlue;
            button2.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            button2.Location = new Point(2, 257);
            button2.Margin = new Padding(0);
            button2.Name = "button2";
            button2.Size = new Size(299, 90);
            button2.TabIndex = 8;
            button2.Text = "退出";
            button2.UseVisualStyleBackColor = false;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.BackgroundImage = (Image)resources.GetObject("pictureBox1.BackgroundImage");
            pictureBox1.BackgroundImageLayout = ImageLayout.Center;
            pictureBox1.Location = new Point(5, 352);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(293, 265);
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            // 
            // applicationTitleTextBox
            // 
            applicationTitleTextBox.Anchor = AnchorStyles.Bottom;
            applicationTitleTextBox.BackColor = SystemColors.ButtonHighlight;
            applicationTitleTextBox.BorderStyle = BorderStyle.None;
            applicationTitleTextBox.Font = new Font("楷体", 12.7384615F, FontStyle.Bold, GraphicsUnit.Point);
            applicationTitleTextBox.ForeColor = SystemColors.MenuHighlight;
            applicationTitleTextBox.Location = new Point(66, 32);
            applicationTitleTextBox.Multiline = true;
            applicationTitleTextBox.Name = "applicationTitleTextBox";
            applicationTitleTextBox.ReadOnly = true;
            applicationTitleTextBox.RightToLeft = RightToLeft.No;
            applicationTitleTextBox.Size = new Size(170, 28);
            applicationTitleTextBox.TabIndex = 3;
            applicationTitleTextBox.Text = "应用列表";
            applicationTitleTextBox.TextAlign = HorizontalAlignment.Center;
            // 
            // addButton
            // 
            addButton.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            addButton.BackColor = Color.LightSkyBlue;
            addButton.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            addButton.Location = new Point(2, 65);
            addButton.Margin = new Padding(0);
            addButton.Name = "addButton";
            addButton.Size = new Size(299, 91);
            addButton.TabIndex = 7;
            addButton.Text = "双文档比对";
            addButton.UseVisualStyleBackColor = false;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel3.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Controls.Add(tableLayoutPanel4, 0, 2);
            tableLayoutPanel3.Controls.Add(fileTitleTextBox, 0, 0);
            tableLayoutPanel3.Controls.Add(fileTextBox, 0, 1);
            tableLayoutPanel3.Location = new Point(307, 2);
            tableLayoutPanel3.Margin = new Padding(0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 3;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 10.16129F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 33.064518F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 56.935482F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.Size = new Size(894, 622);
            tableLayoutPanel3.TabIndex = 1;
            tableLayoutPanel3.TabStop = true;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.CellBorderStyle = TableLayoutPanelCellBorderStyle.OutsetPartial;
            tableLayoutPanel4.ColumnCount = 3;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.7292175F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36.34204F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 29.8099766F));
            tableLayoutPanel4.Controls.Add(firstRichTextBox, 0, 0);
            tableLayoutPanel4.Controls.Add(secondRichTextBox, 1, 0);
            tableLayoutPanel4.Controls.Add(outputpictureBox, 2, 0);
            tableLayoutPanel4.Location = new Point(5, 273);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Size = new Size(884, 344);
            tableLayoutPanel4.TabIndex = 5;
            // 
            // firstRichTextBox
            // 
            firstRichTextBox.Dock = DockStyle.Fill;
            firstRichTextBox.Font = new Font("楷体", 9.969231F, FontStyle.Regular, GraphicsUnit.Point);
            firstRichTextBox.Location = new Point(6, 6);
            firstRichTextBox.Name = "firstRichTextBox";
            firstRichTextBox.Size = new Size(288, 332);
            firstRichTextBox.TabIndex = 0;
            firstRichTextBox.Text = "";
            firstRichTextBox.ModifiedChanged += firstRichTextBox_TextChanged;
            // 
            // secondRichTextBox
            // 
            secondRichTextBox.Dock = DockStyle.Fill;
            secondRichTextBox.Font = new Font("楷体", 9.969231F, FontStyle.Regular, GraphicsUnit.Point);
            secondRichTextBox.Location = new Point(303, 6);
            secondRichTextBox.Name = "secondRichTextBox";
            secondRichTextBox.Size = new Size(311, 332);
            secondRichTextBox.TabIndex = 1;
            secondRichTextBox.Text = "";
            secondRichTextBox.TextChanged += secondRichTextBox_TextChanged;
            // 
            // outputpictureBox
            // 
            outputpictureBox.Dock = DockStyle.Fill;
            outputpictureBox.Location = new Point(623, 6);
            outputpictureBox.Name = "outputpictureBox";
            outputpictureBox.Size = new Size(255, 332);
            outputpictureBox.TabIndex = 2;
            outputpictureBox.TabStop = false;
            // 
            // fileTitleTextBox
            // 
            fileTitleTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            fileTitleTextBox.BackColor = SystemColors.ButtonHighlight;
            fileTitleTextBox.BorderStyle = BorderStyle.None;
            fileTitleTextBox.Font = new Font("楷体", 11.0769234F, FontStyle.Bold, GraphicsUnit.Point);
            fileTitleTextBox.ForeColor = SystemColors.MenuHighlight;
            fileTitleTextBox.Location = new Point(8, 38);
            fileTitleTextBox.Margin = new Padding(6, 0, 0, 3);
            fileTitleTextBox.Name = "fileTitleTextBox";
            fileTitleTextBox.Size = new Size(139, 23);
            fileTitleTextBox.TabIndex = 4;
            fileTitleTextBox.Text = "文档列表";
            // 
            // fileTextBox
            // 
            fileTextBox.AcceptsReturn = true;
            fileTextBox.AcceptsTab = true;
            fileTextBox.Location = new Point(5, 69);
            fileTextBox.Multiline = true;
            fileTextBox.Name = "fileTextBox";
            fileTextBox.ReadOnly = true;
            fileTextBox.ScrollBars = ScrollBars.Both;
            fileTextBox.Size = new Size(884, 195);
            fileTextBox.TabIndex = 6;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonHighlight;
            ClientSize = new Size(1274, 651);
            Controls.Add(tableLayoutPanel1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "Form1";
            Text = "Word对比处理器";
            Load += Form1_Load;
            tableLayoutPanel1.ResumeLayout(false);
            mainTableLayoutPanel.ResumeLayout(false);
            mainTableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)outputpictureBox).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel mainTableLayoutPanel;
        private TableLayoutPanel tableLayoutPanel3;
        private TextBox applicationTitleTextBox;
        private TextBox fileTitleTextBox;
        private PictureBox pictureBox1;
        private TableLayoutPanel tableLayoutPanel4;
        private TextBox fileTextBox;
        private RichTextBox firstRichTextBox;
        private RichTextBox secondRichTextBox;
        private PictureBox outputpictureBox;
        private Button button1;
        private Button button2;
        private Button addButton;
    }
}