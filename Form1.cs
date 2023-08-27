using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Office.Interop.Word;
using Font = System.Drawing.Font;
using Word = Microsoft.Office.Interop.Word;

namespace PageComparison
{
    public partial class Form1 : Form
    {
        int style = -1;     // 比对显示模式，0表示展示公共部分，1表示展示不同部分
        int[,] op = new int[0, 0];
        int[] most_sim = new int[0];
        double[,] similarity = new double[0, 0];
        List<Item> files = new List<Item>();
        string rootPath = Path.GetFullPath(
            Path.Combine(System.Windows.Forms.Application.StartupPath, "..\\..\\..\\"));

        public class Item
        {
            public string filepath { get; set; }
            public string filename { get; set; }

            public Item(string path, string name)
            {
                this.filename = name;
                this.filepath = path;
            }
        }

        public Form1()
        {
            InitializeComponent();
            setMenu();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void setMenu()
        {
            string[] bn = { "双文档比对", "多文档比对", "退出" };
            EventHandler[] bh = new EventHandler[]{
                twoComparisonButton_Click, multiComparisonButton_Click, quitButton_Click
            };
            setMainPanel(5, 1, bn, bh);
        }

        private void setTwoComparisonMode()
        {
            string[] bn = { "打开第一个文档", "打开第二个文档", "开始比对", "切换样式", "返回" };
            EventHandler[] bh = new EventHandler[]{
                openFirstButton_Click, openSecondButton_Click,
                compareTwoButton_Click, switchButton_Click, backButton_Click
            };
            setMainPanel(7, 1, bn, bh);
        }

        private void setMultiComparisonMode()
        {
            string[] bn = { "添加文档", "移除文档", "清空文档", "开始比对", "输出报表", "返回" };
            EventHandler[] bh = new EventHandler[]{
                addButton_Click, removeButton_Click, clearButton_Click,
                compareButton_Click, exportButton_Click, backButton_Click
            };
            setMainPanel(8, 1, bn, bh);
        }

        private void twoComparisonButton_Click(object sender, EventArgs e)
        {
            setTwoComparisonMode();
        }

        private void multiComparisonButton_Click(object sender, EventArgs e)
        {
            setMultiComparisonMode();
        }
        private void quitButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
        private void backButton_Click(object sender, EventArgs e)
        {
            setMenu();
            fileTextBox.Text = firstRichTextBox.Text = secondRichTextBox.Text = string.Empty;
        }

        private void openFirstButton_Click(object sender, EventArgs e)
        {
            showContentTo(firstRichTextBox);
        }

        private void openSecondButton_Click(object sender, EventArgs e)
        {
            showContentTo(secondRichTextBox);
        }

        private void compareTwoButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(firstRichTextBox.Text) ||
                string.IsNullOrEmpty(secondRichTextBox.Text))
            {
                MessageBox.Show("请先导入文档或手动输入文本");
                return;
            }

            double similarRate = 0.0;
            compare(firstRichTextBox.Text, secondRichTextBox.Text, ref similarRate);

            style = 0;
            showStyle();

            //Debug.WriteLine("**********");
        }

        // 切换比对显示模式
        private void switchButton_Click(object sender, EventArgs e)
        {
            if (style < 0) { MessageBox.Show("请先进行比对"); return; } // 并未设置格式           
            style ^= 1;
            showStyle();
        }
        private void addButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Word文档|*.doc;*.docx";
            //openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.Title = "选择要加载的 Word 文档文件";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //创建 Word 应用程序对象
                var wordApp = new Word.Application();

                //打开指定的 Word 文档文件
                string filePath = openFileDialog.FileName;
                string fileName = Path.GetFileName(filePath);

                Item newItem = new Item(filePath, fileName);
                files.Add(newItem);

                showFilenameAt(fileTextBox);
            }
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if (files.Count == 0) return;

            files.RemoveAt(files.Count - 1);
            showFilenameAt(fileTextBox);
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            files.Clear();
            fileTextBox.Text = "";
        }

        private void compareButton_Click(object sender, EventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerAsync();
        }

        private void exportButton_Click(object sender, EventArgs e)
        {
            string Msg = "";
            for (int i = 0; i < most_sim.Length; ++i)
            {
                int j = most_sim[i];
                string name1 = files[i].filename, name2 = files[j].filename;

                Msg += "与 " + name1 + " 相似度最高的文章是 " + name2 + "\r\n";
                Msg += "相似度为 " + similarity[i, j] * 100 + "%" + "\r\n\r\n";
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Word Documents|*.docx";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = saveFileDialog.FileName;

                // 创建 Word 文档并写入文本
                Word.Application wordApp = new Word.Application();
                Document doc = wordApp.Documents.Add();
                doc.Content.Text = Msg;
                doc.SaveAs(selectedFilePath);
                doc.Close();
                wordApp.Quit();

                MessageBox.Show("文档已导出到：" + Path.GetFileName(selectedFilePath));
            }
        }

        private void showCommonStyle()
        {
            int n = firstRichTextBox.Text.Length, m = secondRichTextBox.Text.Length;
            int p = n, q = m;
            while (p >= 0 && q >= 0)
            {
                int t = op[p, q];
                if (t == 0)
                {
                    if (p - 1 >= 0) highlightTextInTextBox(firstRichTextBox, p - 1, 1);
                    if (q - 1 >= 0) highlightTextInTextBox(secondRichTextBox, q - 1, 1);
                    p--; q--;
                }
                else if (t == 1) { p--; }
                else if (t == 2) { q--; }
                else { p--; q--; }
            }
        }

        private void showContrastStyle()
        {
            int n = firstRichTextBox.Text.Length, m = secondRichTextBox.Text.Length;
            int p = n, q = m;
            while (p >= 0 && q >= 0)
            {
                int t = op[p, q];
                if (t == 0) { p--; q--; }
                else if (t == 1)
                {
                    StrickoutTextInTextBox(firstRichTextBox, p - 1, 1);
                    p--;
                }
                else if (t == 2)
                {
                    highlightTextInTextBox(secondRichTextBox, q - 1, 1, Color.SkyBlue);
                    q--;
                }
                else
                {
                    StrickoutTextInTextBox(firstRichTextBox, p - 1, 1);
                    highlightTextInTextBox(secondRichTextBox, q - 1, 1, Color.SkyBlue);
                    p--; q--;
                }
            }
        }

        private void richBoxClearStyle()
        {
            RichTextBox fst = firstRichTextBox, scd = secondRichTextBox;
            fst.SelectAll(); scd.SelectAll();
            scd.SelectionBackColor = fst.SelectionBackColor = fst.BackColor; // 恢复默认背景色
            fst.SelectionColor = scd.ForeColor = fst.ForeColor; // 恢复默认字体颜色
            fst.SelectionFont = new Font("楷体", 10, FontStyle.Regular); // 去掉删除线
            scd.SelectionFont = new Font("楷体", 10, FontStyle.Regular);
            fst.DeselectAll(); scd.DeselectAll();
        }


        private void showContentTo(RichTextBox textBox)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Word文档|*.doc;*.docx";
            //openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.Title = "选择要加载的 Word 文档文件";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // 清空原文本框
                textBox.Text = string.Empty;

                //创建 Word 应用程序对象
                var wordApp = new Word.Application();

                //打开指定的 Word 文档文件
                string filePath = openFileDialog.FileName;
                Document wordDoc = wordApp.Documents.Open(filePath);

                //将文档内容复制到剪贴板
                //并使用 RichText Box中的 Paste 方法将剪贴板中的文本粘贴到RichTextBox控件中
                wordDoc.Content.Copy();
                textBox.Paste();

                textBox.Text = textBox.Text.Remove(textBox.Text.Length - 1);

                wordDoc.Close();
                wordApp.Quit();

                //var (name, content) = ReadWordDocument(new Word.Application(), openFileDialog.FileName);
                //textBox.Text = content;
            }
        }

        //后台异步执行耗时的计算操作
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            int count = files.Count;
            similarity = new double[count, count];

            var wordApp = new Word.Application();
            wordApp.Visible = false; // 设置 Word 应用程序为不可见

            for (int i = 0; i < count; i++)
                for (int j = i + 1; j < count; j++)
                {
                    if (i == j) continue;
                    var (_, text1) = ReadWordDocument(wordApp, files[i].filepath);
                    var (_, text2) = ReadWordDocument(wordApp, files[j].filepath);

                    compare(text1, text2, ref similarity[i, j]);
                }

            for (int i = 0; i < count; ++i)
                for (int j = i - 1; j >= 0; --j)
                    similarity[i, j] = similarity[j, i];

            DrawHeatmap2(similarity);

            most_sim = new int[count];
            for (int i = 0; i < count; ++i)
            {
                int k = 0;
                for (int j = 0; j < count; ++j)
                    if (similarity[i, j] > similarity[i, k])
                        k = j;
                most_sim[i] = k;
            }
        }

        private void showFilenameAt(TextBox textBox)
        {
            string text = "";
            foreach (var item in files)
            {
                text += item.filename;
                text += "\r\n";
            }
            textBox.Text = text;
        }

        private void highlightTextInTextBox(RichTextBox textBox, int index, int legth, Color c = default)
        {
            if (c == default) c = Color.Yellow;  //默认为黄色高亮
            textBox.SelectionStart = index;
            textBox.SelectionLength = legth;
            textBox.SelectionBackColor = c;
        }

        private void StrickoutTextInTextBox(RichTextBox textBox, int index, int legth)
        {
            textBox.SelectionStart = index;
            textBox.SelectionLength = legth;
            Font originalFont = textBox.SelectionFont;
            Font newFont = new Font(originalFont.FontFamily, originalFont.Size, originalFont.Style | FontStyle.Strikeout);
            textBox.SelectionFont = newFont;
            textBox.SelectionColor = Color.Red;
        }

        private void InsertTextBoxAtRow(TableLayoutPanel panel, int row, string text)
        {
            TextBox textBox = new TextBox();
            textBox.Text = text;
            textBox.BackColor = SystemColors.ButtonHighlight;
            textBox.Font = new Font("楷体", 13, FontStyle.Bold);
            textBox.ForeColor = SystemColors.MenuHighlight;
            textBox.Dock = DockStyle.Bottom;
            textBox.Anchor = AnchorStyles.Bottom;
            textBox.Height = panel.Height / 2;
            textBox.Width = panel.Width / 2;
            textBox.TextAlign = HorizontalAlignment.Center;
            textBox.ReadOnly = true;
            textBox.SelectionStart = textBox.SelectionLength = 0; // 确保没有选中任何内容

            panel.Controls.Add(textBox, 0, row);
        }

        private void InsertImageAtRow(int row, Image image)
        {
            PictureBox pictureBox = new PictureBox();
            pictureBox.Image = image;
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.Dock = DockStyle.Fill;

            mainTableLayoutPanel.Controls.Add(pictureBox, 0, row);
        }

        // 创建按钮并设置其响应事件
        private Button CreateButton(string text, EventHandler clickHandler)
        {
            Button button = new Button();
            button.Text = text;
            button.BackColor = Color.LightBlue;
            //button.Font = new Font("Microsoft YaHei", 15);
            button.Font = new Font("楷体", 12, FontStyle.Regular); 
            button.UseVisualStyleBackColor = false;
            button.Dock = DockStyle.Fill;
            button.Click += clickHandler;
            return button;
        }

        //设置MainPanel的风格及功能
        private void setMainPanel(int row, int col, string[] bn, EventHandler[] bh)
        {
            TableLayoutPanel tlp = mainTableLayoutPanel;
            tlp.Controls.Clear();
            tlp.RowStyles.Clear();
            tlp.RowCount = row;
            tlp.ColumnCount = col;

            int width = tlp.Width, height = tlp.Height;
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, height / 10));
            InsertTextBoxAtRow(mainTableLayoutPanel, 0, "应用列表");

            for (int i = 0; i < tlp.RowCount - 2; ++i)
            {
                tlp.RowStyles.Add(new RowStyle(SizeType.Percent, height * 3 / 5 / (tlp.RowCount - 2)));
                tlp.Controls.Add(CreateButton(bn[i], bh[i]), 0, i + 1);
            }

            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, height * 3 / 10));
            string imagePath = Path.Combine(rootPath, "Resources", "logo.png");
            InsertImageAtRow(tlp.RowCount - 1, Image.FromFile(imagePath));
        }

        static (string, string) ReadWordDocument(Word.Application wordApp, string filePath)
        {
            Document doc = wordApp.Documents.Open(filePath);
            string text = doc.Content.Text;
            doc.Close(false);
            return (Path.GetFileName(filePath), text);
        }

        void compare(string text1, string text2, ref double similarity)
        {
            string a = " " + text1, b = " " + text2;
            int n = a.Length - 1, m = b.Length - 1;

            //f[i, j]表示 a[l~i]要变成 b[1~j]至少需要的编辑步数
            //op[i, j]表示 a[l~i]要变成 b[1~j] 的最后一步操作是什么
            //0表示无需做变换，1表示要把a[i]删除，2表示要在a[i]后增添b[j], 3表示要将a[i]替换成b[j]
            int[,] f = new int[n + 5, m + 5];
            op = new int[n + 5, m + 5];
            for (int i = 1; i <= m; ++i) { f[0, i] = i; op[0, i] = 2; }
            for (int i = 1; i <= n; ++i) { f[i, 0] = i; op[i, 0] = 1; }

            //Debug.WriteLine(f[0, 0]);

            for (int i = 1; i <= n; ++i)
                for (int j = 1; j <= m; ++j)
                    if (a[i] == b[j]) f[i, j] = f[i - 1, j - 1];
                    else
                    {
                        int t = 0;     //哪种操作步数最少
                        int[] alter = {
                            (int)1e9, f[i - 1, j] + 1, f[i, j - 1] + 1, f[i - 1, j - 1] + 1
                        };
                        for (int k = 0; k < 4; ++k)
                            if (alter[k] < alter[t])
                                t = k;
                        f[i, j] = alter[t];
                        op[i, j] = t;
                    }

            int distance = f[n, m];
            similarity = 1 - (double)distance / Math.Max(n, m);
        }

        private void showStyle()
        {
            richBoxClearStyle();
            if (style == 0) showCommonStyle();
            else showContrastStyle();
        }

        private void DrawHeatmap2(double[,] correlationData)
        {
            // 创建绘图区域
            Bitmap heatmap = new Bitmap(400, 400);
            Graphics graphics = Graphics.FromImage(heatmap);

            // 相关性数据
            /*  double[,] correlationData = {
          { 1.0, 0.8, 0.2 },
          { 0.8, 1.0, 0.6 },
          { 0.2, 0.6, 1.0 }
      };
  */
            // 定义颜色映射范围
            Color startColor = Color.White; // 起始颜色
            Color endColor = Color.Blue; // 结束颜色

            int width = correlationData.GetLength(0);
            int height = correlationData.GetLength(1);
            int xx = outputpictureBox.Width / (width + 1);
            int yy = outputpictureBox.Height / (height + 1);
            // 计算每个数据点的颜色并绘制矩形
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    double value = correlationData[i, j];
                    Color color = GetHeatmapColor(value, startColor, endColor);

                    using (Brush brush = new SolidBrush(color))
                    {
                        int rectWidth = xx;
                        int rectHeight = yy;
                        int x = i * rectWidth + xx;
                        int y = j * rectHeight;

                        graphics.FillRectangle(brush, x, y, rectWidth, rectHeight);
                    }
                }
            }
            Font labelFontx = new Font(Font.FontFamily, Math.Min(xx / 2, yy / 2));
            //Font labelFonty = new Font(Font.FontFamily, yy / 2);
            // 绘制横坐标刻度标签
            for (int i = 0; i < width; i++)
            {
                int labelX = i * xx + xx;
                int labelY = height * yy;

                graphics.DrawString(i.ToString(), labelFontx, Brushes.Black, labelX, labelY);
            }

            // 绘制纵坐标刻度标签
            for (int j = 0; j < height; j++)
            {
                int labelX = 0;
                int labelY = j * yy;

                graphics.DrawString(j.ToString(), labelFontx, Brushes.Red, labelX, labelY);
            }
            outputpictureBox.Image = heatmap;
        }


        private Color GetHeatmapColor(double value, Color startColor, Color endColor)
        {
            // 根据相关性值获取颜色
            double min = 0.0;
            double max = 1.0;

            // 计算相关性值所在范围的百分比
            double percentage = (value - min) / (max - min);

            // 根据百分比在颜色映射范围内进行插值
            int red = (int)(startColor.R + (endColor.R - startColor.R) * percentage);
            int green = (int)(startColor.G + (endColor.G - startColor.G) * percentage);
            int blue = (int)(startColor.B + (endColor.B - startColor.B) * percentage);

            return Color.FromArgb(red, green, blue);
        }

        private void firstRichTextBox_TextChanged(object sender, EventArgs e)
        {
        }

        private void secondRichTextBox_TextChanged(object sender, EventArgs e)
        {
        }
    }
}