using System.Text.RegularExpressions;

namespace TextEditor_1._0
{
    public partial class Form1 : Form
    {
        /*const */
        string filePath; //= "text.txt";
        //string newFilePath;
        public Form1()
        {
            InitializeComponent();
        }
        // // // OPEN: open file // // //
        private string charToString(char[] myText)
        {
            string myString = string.Empty;
            foreach(char literal in myText)
            {
                myString += literal;
            }

            return myString+'\n'+filePath;
        }
        private void OpenFile()
        {
            StreamReader swr = new StreamReader(filePath);
            char[] _text = new char[500];
            swr.ReadBlock(_text);
            richTextBox1.Text = charToString(_text);
            swr.Close();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            if (Program.args != null && Program.args.Length > 0)
            {
                filePath = Program.args[0];
                string[] splitedFilePath = filePath.Split('\\');
                this.Text = splitedFilePath[splitedFilePath.Length-1];
                if (File.Exists(filePath))
                {
                    OpenFile(); 
                }
            }
        }
        // // // SAVE: save file // // //
        private void SaveFile()
        {
            if (File.Exists(filePath))
            {
                string _richBoxText = richTextBox1.Text;
                StreamWriter swr = new StreamWriter(filePath);
                swr.WriteLine(_richBoxText);
                swr.Close();
                return;
            }
            MessageBox.Show("ERROR1:filePath is wrong");

        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Хочете зберегти зміни?", "Зберегти файл", MessageBoxButtons.YesNo);
            if (dialogResult ==  DialogResult.Yes)
            {
                SaveFile();
            }
        }

        // // // SEARCH: search some text // // //
        //string textBoxText = ;
        private void textBoxInit()
        {
            richTextBox1.SelectAll();
            richTextBox1.SelectionColor = Color.Black;
        }
        private void searchFunc(string mySearchedText, bool mode)
        {
            try
            {
                if (mySearchedText != string.Empty)
                {
                    Regex regex = new Regex(mySearchedText);
                    var matches = regex.Matches(richTextBox1.Text);

                    foreach (Match match in matches)
                    {
                        richTextBox1.Select(match.Index, match.Length);
                        richTextBox1.SelectionColor = Color.Red;
                        richTextBox1.SelectionFont = Font;
                    }
                    if (mode)
                        MessageBox.Show("Таке послідовність букв " + matches.Count + " разів.");
                }
                else
                    MessageBox.Show("Fill in the text field");
            }
            catch(Exception ex)
            {
                //MessageBox.Show("searchFunc():: " + ex.Message);
                return;
            }
        }
        private int MaxLenght(List<string> wordList)
        {
            int maxLen = -1;
            foreach (var word in wordList)
            {
                if (maxLen < word.Length)
                {
                    maxLen = word.Length;
                }
            }
            return maxLen;
        }
        private List<string> findTheLongRecFunc(int lenght, int numOf, List<string> wordList)
        {
            List<string> maxList = new List<string>();
            List<string> newWordList = new List<string>();// = wordList;
            newWordList.AddRange(wordList);
            foreach (var word in wordList)
            {
                if (lenght == word.Length)
                {
                    maxList.Add(word);
                    newWordList.Remove(word);
                }
                if (maxList.Count == numOf)
                {
                    return maxList;
                }
            }
            if (maxList.Count < numOf)
            {
                maxList.AddRange( findTheLongRecFunc(MaxLenght(newWordList), numOf, newWordList));
            }
            return maxList;
        }
        private void findTheLongest()
        {
            try
            {
                repNuberToolStripMenuItem_Click(new object(), new EventArgs());
                int maxLen = MaxLenght(richTextBox1.Text.Split(' ').ToList());
                List<string> maxLenghtlist = findTheLongRecFunc(maxLen, richTextBox1.Text.Split(" ").Length > 20 ? 10 : richTextBox1.Text.Split(" ").Length / 2, richTextBox1.Text.Split(' ').ToList());
                foreach (var item in maxLenghtlist)
                {
                    searchFunc(item,false);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("findTheLongest():: " + ex.Message);
                return;
            }
        }
        private void searchTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == ((char)Keys.Enter))
            {
                textBoxInit();
                searchFunc(searchTextBox.Text, false);
            }
        }

        private void findTheLongestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxInit();
            findTheLongest();
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxInit();
            searchFunc(searchTextBox.Text,false);

        }

        private void repNuberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string newText = string.Empty;
            foreach (var _string in richTextBox1.Text.Split(" "))
            {
                foreach (var word in _string.Split('\n'))
                {
                    if (word != "")
                        newText += word.TrimStart('\n') + " ";
                }
            }
            richTextBox1.Text = newText;
        }
        // // //
    }
}