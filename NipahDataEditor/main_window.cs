using NipahData;
using NipahData_Tokenizer;
using NipahData_Tokenizer.TokenizerTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsSyntaxHighlighter;

namespace NipahDataEditor
{
    public partial class main_window : Form
    {
        readonly SyntaxHighlighter syntaxHighlighter;
        public main_window()
        {
            InitializeComponent();

            textbox.ZoomFactor = 2;
            textbox.KeyDown += Textbox_KeyDown;
            textbox.BackColor = Color.FromArgb(255, 31, 9, 54);
            textbox.ForeColor = Color.White;

            syntaxHighlighter = new SyntaxHighlighter(textbox);

            // That's it. Now tell me how you'd like to see what...

            // multi-line comments; I'd like to see them in dark-sea-green and italic
            syntaxHighlighter.AddPattern(
                new PatternDefinition(new Regex(@"/\*(.|[\r\n])*?\*/",
                    RegexOptions.Multiline | RegexOptions.Compiled)),
                new SyntaxStyle(Color.DarkSeaGreen, bold: false, italic: true));

            // singlie-line comments; I'd like to see them in Green and italic
            syntaxHighlighter.AddPattern(
                new PatternDefinition(new Regex(@"//.*?$",
                    RegexOptions.Multiline | RegexOptions.Compiled)),
                new SyntaxStyle(Color.Green, bold: false, italic: true));

            // numbers; I'd like to see them in purple
            syntaxHighlighter.AddPattern(
                new PatternDefinition(@"\d+\.\d+|\d+"),
                new SyntaxStyle(Color.FromArgb(255, 25, 255, 167)));

            // double quote strings; I'd like to see them in Red
            syntaxHighlighter.AddPattern(
                new PatternDefinition(@"\""([^""]|\""\"")+\"""),
                new SyntaxStyle(Color.FromArgb(255, 214, 135, 85)));

            // single quote strings; I'd like to see them in Salmon 
            syntaxHighlighter.AddPattern(
                new PatternDefinition(@"\'([^']|\'\')+\'"),
                new SyntaxStyle(Color.Salmon));

            /*// 1st set of keywords; I'd like to see them in Blue
            syntaxHighlighter.AddPattern(
                new PatternDefinition("for", "foreach", "int", "var"),
                new SyntaxStyle(Color.Blue));

            // 2nd set of keywords; I'd like to see them in bold Navy, and they must be case insensitive
            syntaxHighlighter.AddPattern(
                new CaseInsensitivePatternDefinition("public", "partial", "class", "void"),
                new SyntaxStyle(Color.Navy, true, false));*/

            // operators; I'd like to see them in Brown
            syntaxHighlighter.AddPattern(
                new PatternDefinition("+", ">", "<", "&", "|", "{", "}", "[", "]", ","),
                new SyntaxStyle(Color.FromArgb(255, 255, 221, 25)));

            //errorForm = new Form();
            //errorBox = new RichTextBox();
            //this.AddOwnedForm(errorForm);

            //errorForm.Controls.Add(errorBox);
            textbox.AcceptsTab = true;
            showError("");

            Text = $"Nipah Data Editor";

            if (File.Exists("Last.txt"))
            {
                current = File.ReadAllText("Last.txt");
                loadData();
            }
            Application.ApplicationExit += Application_ApplicationExit;
        }

        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            if (current != null)
                File.WriteAllText("Last.txt", current);
        }

        void showError(string error)
        {
            errorBox.Font = textbox.Font;
            errorBox.ZoomFactor = textbox.ZoomFactor;
            errorBox.BackColor = Color.Black;

            errorBox.ForeColor = textbox.ForeColor;

            errorBox.Text = error;

            //errorForm.Show();
            textbox.Focus();

            //errorForm.Location = SumPoint(textbox.GetPositionFromCharIndex(textbox.SelectionStart), new Point(15, 15));
        }
        void hideError()
        {
            errorBox.Clear();
        }
        //Point SumPoint(Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y);
        //Form errorForm;
        //RichTextBox errorBox;

        private void Textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.Control)
                return;
            switch(e.KeyCode)
            {
                case Keys.Add:
                    textbox.ZoomFactor += 0.5f;
                    break;
                case Keys.Subtract:
                    textbox.ZoomFactor -= 0.5f;
                    break;
                case Keys.S:
                    if (e.Shift)
                    {
                        setNewPath();
                        saveFile();
                    }
                    else
                        saveFile();
                    break;
                case Keys.L:
                    loadFile();
                    break;
                case Keys.N:
                    newData();
                    break;
            }
        }
        OpenFileDialog openFileDialog = new OpenFileDialog()
        {
            Filter = "Nipah Data File (*.miidat)|*.miidat",
            Multiselect = false
        };
        SaveFileDialog saveFileDialog = new SaveFileDialog()
        {
            Filter = "Nipah Data File (*.miidat)|*.miidat"
        };

        string current;
        void loadFile()
        {
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                current = openFileDialog.FileName;
                loadData();
            }
        }
        void saveFile()
        {
            if (string.IsNullOrEmpty(current))
                setNewPath();
            File.WriteAllText(current, textbox.Text);
        }
        void setNewPath()
        {
            if(saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                current = saveFileDialog.FileName;
                Text = $"Nipah Data Editor — ${Path.GetFileName(current)}";
            }
        }
        void newData()
        {
            current = null;
            textbox.Clear();
            errorBox.Clear();

            Text = $"Nipah Data Editor";
        }

        void loadData()
        {
            if (string.IsNullOrEmpty(current) || !File.Exists(current))
                return;
            textbox.Text = File.ReadAllText(current);
            Text = $"Nipah Data Editor — ${Path.GetFileName(current)}";

            syntaxHighlighter.ReHighlight();
        }


        private void new_menu_Click(object sender, EventArgs e)
        {
            newData();
        }

        private void load_menu_Click(object sender, EventArgs e)
        {
            loadFile();
        }

        private void save_menu_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        private void saveAs_menu_Click(object sender, EventArgs e)
        {
            setNewPath();
            saveFile();
        }

        private void exit_menu_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        Tokenizer tokenizer = new Tokenizer();
        private void textbox_TextChanged(object sender, EventArgs e)
        {
            Tokenizer.AcceptSeparatedID = true;
            try
            {
                processText();
            }
            catch(CompileError cer)
            {
                showError(cer.Message);
            }
        }
        Token def = new Token("NULL", TokenType.None, 0, 0);
        void processText()
        {
            string text = textbox.Text;
            var tokens = new ProgressiveList<Token>(tokenizer.Tokenize(text));
            Token token;
            List<string> track = new List<string>();
            Token old = null;
            bool openned = false;
            while (token = tokens.Next(t => t == TokenType.EOF))
            {
                if (old == null)
                    old = token;
                if (token == null)
                    throw (old ?? token ?? def).IError("Null token exception, at");
                if(token == TokenType.CloseBrackets)
                {
                    openned = false;
                    track.Clear();
                    continue;
                }
                if(token == TokenType.OpenBrackets)
                {
                    openned = true;
                    old = token ?? old;
                    token = tokens.Next(t => t == TokenType.EOF);
                    if (token == null)
                        throw (old ?? token ?? def).IError("Null token exception, at");
                    if (token == TokenType.CloseBrackets)
                        continue;
                    string key = (string)token.value;
                    old = token ?? old;
                    token = tokens.Next();
                    if (token == null)
                        throw (old ?? token ?? def).IError("Null token exception, at");
                    token.Assert(TokenType.Descript);
                    for(int i = 0; i < track.Count; i++)
                    {
                        old = token ?? old;
                        token = tokens.Next();
                        if (token == TokenType.EOF)
                            throw token.IError($"Expecting {track.Count - i} values of {track.Count}");
                        if(i > 0)
                        {
                            token.Assert(TokenType.Comma);
                            token = tokens.Next();
                        }
                        if (token == null)
                            throw (old ?? token ?? def).IError("Null token exception, at");
                        token.AssertValue();
                    }
                    if (token == null)
                        throw (old ?? token ?? def).IError("Null token exception, at");
                    token = tokens.Next();
                    token.Assert(TokenType.EOF);
                    continue;
                }
                if (track.Count > 0)
                {
                    if (token == null)
                        throw (old ?? token ?? def).IError("Null token exception, at");
                    token.Assert(TokenType.Comma);
                    old = token ?? old;
                    token = tokens.Next();
                }
                if (token == null)
                    throw (old ?? token ?? def).IError("Null token exception, at");
                track.Add((string)token.value);
            }
            if (openned)
                throw old.IError("Expecting '}', at ");
            if (track.Count > 0)
                throw old.IError("Expecting '{', at ");
            hideError();
        }
    }
}
