using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Office.Interop.Word;
using Font = System.Drawing.Font;
using Word = Microsoft.Office.Interop.Word;

namespace PageComparison
{
    public partial class Form1 : Form
    {
        int style = -1;     // �ȶ���ʾģʽ��0��ʾչʾ�������֣�1��ʾչʾ��ͬ����
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
            string[] bn = { "˫�ĵ��ȶ�", "���ĵ��ȶ�", "�˳�" };
            EventHandler[] bh = new EventHandler[]{
                twoComparisonButton_Click, multiComparisonButton_Click, quitButton_Click
            };
            setMainPanel(5, 1, bn, bh);
        }

        private void setTwoComparisonMode()
        {
            string[] bn = { "�򿪵�һ���ĵ�", "�򿪵ڶ����ĵ�", "��ʼ�ȶ�", "�л���ʽ", "����" };
            EventHandler[] bh = new EventHandler[]{
                openFirstButton_Click, openSecondButton_Click,
                compareTwoButton_Click, switchButton_Click, backButton_Click
            };
            setMainPanel(7, 1, bn, bh);
        }

        private void setMultiComparisonMode()
        {
            string[] bn = { "����ĵ�", "�Ƴ��ĵ�", "����ĵ�", "��ʼ�ȶ�", "�������", "����" };
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
                MessageBox.Show("���ȵ����ĵ����ֶ������ı�");
                return;
            }

            double similarRate = 0.0;
            compare(firstRichTextBox.Text, secondRichTextBox.Text, ref similarRate);

            style = 0;
            showStyle();

            //Debug.WriteLine("**********");
        }

        // �л��ȶ���ʾģʽ
        private void switchButton_Click(object sender, EventArgs e)
        {
            if (style < 0) { MessageBox.Show("���Ƚ��бȶ�"); return; } // ��δ���ø�ʽ           
            style ^= 1;
            showStyle();
        }
        private void addButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Word�ĵ�|*.doc;*.docx";
            //openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.Title = "ѡ��Ҫ���ص� Word �ĵ��ļ�";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //���� Word Ӧ�ó������
                var wordApp = new Word.Application();

                //��ָ���� Word �ĵ��ļ�
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

                Msg += "�� " + name1 + " ���ƶ���ߵ������� " + name2 + "\r\n";
                Msg += "���ƶ�Ϊ " + similarity[i, j] * 100 + "%" + "\r\n\r\n";
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Word Documents|*.docx";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = saveFileDialog.FileName;

                // ���� Word �ĵ���д���ı�
                Word.Application wordApp = new Word.Application();
                Document doc = wordApp.Documents.Add();
                doc.Content.Text = Msg;
                doc.SaveAs(selectedFilePath);
                doc.Close();
                wordApp.Quit();

                MessageBox.Show("�ĵ��ѵ�������" + Path.GetFileName(selectedFilePath));
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
            scd.SelectionBackColor = fst.SelectionBackColor = fst.BackColor; // �ָ�Ĭ�ϱ���ɫ
            fst.SelectionColor = scd.ForeColor = fst.ForeColor; // �ָ�Ĭ��������ɫ
            fst.SelectionFont = new Font("����", 10, FontStyle.Regular); // ȥ��ɾ����
            scd.SelectionFont = new Font("����", 10, FontStyle.Regular);
            fst.DeselectAll(); scd.DeselectAll();
        }


        private void showContentTo(RichTextBox textBox)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Word�ĵ�|*.doc;*.docx";
            //openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.Title = "ѡ��Ҫ���ص� Word �ĵ��ļ�";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // ���ԭ�ı���
                textBox.Text = string.Empty;

                //���� Word Ӧ�ó������
                var wordApp = new Word.Application();

                //��ָ���� Word �ĵ��ļ�
                string filePath = openFileDialog.FileName;
                Document wordDoc = wordApp.Documents.Open(filePath);

                //���ĵ����ݸ��Ƶ�������
                //��ʹ�� RichText Box�е� Paste �������������е��ı�ճ����RichTextBox�ؼ���
                wordDoc.Content.Copy();
                textBox.Paste();

                textBox.Text = textBox.Text.Remove(textBox.Text.Length - 1);

                wordDoc.Close();
                wordApp.Quit();

                //var (name, content) = ReadWordDocument(new Word.Application(), openFileDialog.FileName);
                //textBox.Text = content;
            }
        }

        //��̨�첽ִ�к�ʱ�ļ������
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            int count = files.Count;
            similarity = new double[count, count];

            var wordApp = new Word.Application();
            wordApp.Visible = false; // ���� Word Ӧ�ó���Ϊ���ɼ�

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
            if (c == default) c = Color.Yellow;  //Ĭ��Ϊ��ɫ����
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
            textBox.Font = new Font("����", 13, FontStyle.Bold);
            textBox.ForeColor = SystemColors.MenuHighlight;
            textBox.Dock = DockStyle.Bottom;
            textBox.Anchor = AnchorStyles.Bottom;
            textBox.Height = panel.Height / 2;
            textBox.Width = panel.Width / 2;
            textBox.TextAlign = HorizontalAlignment.Center;
            textBox.ReadOnly = true;
            textBox.SelectionStart = textBox.SelectionLength = 0; // ȷ��û��ѡ���κ�����

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

        // ������ť����������Ӧ�¼�
        private Button CreateButton(string text, EventHandler clickHandler)
        {
            Button button = new Button();
            button.Text = text;
            button.BackColor = Color.LightBlue;
            //button.Font = new Font("Microsoft YaHei", 15);
            button.Font = new Font("����", 12, FontStyle.Regular); 
            button.UseVisualStyleBackColor = false;
            button.Dock = DockStyle.Fill;
            button.Click += clickHandler;
            return button;
        }

        //����MainPanel�ķ�񼰹���
        private void setMainPanel(int row, int col, string[] bn, EventHandler[] bh)
        {
            TableLayoutPanel tlp = mainTableLayoutPanel;
            tlp.Controls.Clear();
            tlp.RowStyles.Clear();
            tlp.RowCount = row;
            tlp.ColumnCount = col;

            int width = tlp.Width, height = tlp.Height;
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, height / 10));
            InsertTextBoxAtRow(mainTableLayoutPanel, 0, "Ӧ���б�");

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

            //f[i, j]��ʾ a[l~i]Ҫ��� b[1~j]������Ҫ�ı༭����
            //op[i, j]��ʾ a[l~i]Ҫ��� b[1~j] �����һ��������ʲô
            //0��ʾ�������任��1��ʾҪ��a[i]ɾ����2��ʾҪ��a[i]������b[j], 3��ʾҪ��a[i]�滻��b[j]
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
                        int t = 0;     //���ֲ�����������
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
            // ������ͼ����
            Bitmap heatmap = new Bitmap(400, 400);
            Graphics graphics = Graphics.FromImage(heatmap);

            // ���������
            /*  double[,] correlationData = {
          { 1.0, 0.8, 0.2 },
          { 0.8, 1.0, 0.6 },
          { 0.2, 0.6, 1.0 }
      };
  */
            // ������ɫӳ�䷶Χ
            Color startColor = Color.White; // ��ʼ��ɫ
            Color endColor = Color.Blue; // ������ɫ

            int width = correlationData.GetLength(0);
            int height = correlationData.GetLength(1);
            int xx = outputpictureBox.Width / (width + 1);
            int yy = outputpictureBox.Height / (height + 1);
            // ����ÿ�����ݵ����ɫ�����ƾ���
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
            // ���ƺ�����̶ȱ�ǩ
            for (int i = 0; i < width; i++)
            {
                int labelX = i * xx + xx;
                int labelY = height * yy;

                graphics.DrawString(i.ToString(), labelFontx, Brushes.Black, labelX, labelY);
            }

            // ����������̶ȱ�ǩ
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
            // ���������ֵ��ȡ��ɫ
            double min = 0.0;
            double max = 1.0;

            // ���������ֵ���ڷ�Χ�İٷֱ�
            double percentage = (value - min) / (max - min);

            // ���ݰٷֱ�����ɫӳ�䷶Χ�ڽ��в�ֵ
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