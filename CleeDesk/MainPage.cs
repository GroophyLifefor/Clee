using System;
using System.Drawing;
using System.Windows.Forms;
using Clee;
using ScintillaNET;

namespace CleeDesk
{
    public partial class MainPage : Form
    {
        private int editorMaxLineNumberCharLength;
        private int resultMaxLineNumberCharLength;
        private CodeGeneratorInstance _codeGenerator;

        public MainPage()
        {
            InitializeComponent();
            InitalizeCleeLexer();
            MainPage_SizeChanged(null, null);
            SetLineNumbers(Editor, ref editorMaxLineNumberCharLength);
            SetLineNumbers(Result, ref editorMaxLineNumberCharLength);

            CheckForIllegalCrossThreadCalls = false;
            _codeGenerator = new CodeGeneratorInstance();
            _codeGenerator.OnLog += log => Logs.Text += $"{log}\r\n";
            var debouncedCompiler = new Action(Compile).Debounce();
            Editor.TextChanged += (sender, e) => debouncedCompiler();
            Editor.TextChanged += (sender, e) => SetLineNumbers((Scintilla)sender, ref editorMaxLineNumberCharLength);
            Result.TextChanged += (sender, e) => SetLineNumbers((Scintilla)sender, ref resultMaxLineNumberCharLength);
            

            Editor.Text = """
@echo off
./import(sleep, "system.clee")

fn main()
{-
    echo hi
-}
""".Trim();
        }

        private void InitalizeCleeLexer()
        {
            CleeLexer lexer = new CleeLexer(Editor);

            
            Editor.StyleResetDefault();
            Editor.Styles[Style.Default].Font = "Consolas";
            Editor.Styles[Style.Default].Size = 10;
            Editor.Styles[Style.Default].BackColor = Color.FromArgb(40, 42, 54);
            Editor.CaretForeColor = Color.White;
            Editor.StyleClearAll();
            Editor.Styles[Style.LineNumber].ForeColor = Color.FromArgb(144, 145, 148);
            Editor.Styles[Style.LineNumber].BackColor = Color.FromArgb(40, 42, 54);
            
            Result.StyleResetDefault();
            Result.Styles[Style.Default].Font = "Consolas";
            Result.Styles[Style.Default].Size = 10;
            Result.Styles[Style.Default].BackColor = Color.FromArgb(40, 42, 54);
            Result.Styles[Style.Default].ForeColor = Color.White;
            Result.CaretForeColor = Color.White;
            Result.StyleClearAll();
            Result.Styles[Style.LineNumber].ForeColor = Color.FromArgb(144, 145, 148);
            Result.Styles[Style.LineNumber].BackColor = Color.FromArgb(40, 42, 54);

            
            Editor.Styles[CleeLexer.StyleDefault].ForeColor = Color.White;
            Editor.Styles[CleeLexer.StyleVariable].ForeColor = Color.FromArgb(255, 184, 108);
            Editor.Styles[CleeLexer.StyleInvoking].ForeColor = Color.FromArgb(139, 233, 253);
            Editor.Styles[CleeLexer.StyleFN].ForeColor = Color.FromArgb(242, 109, 157);
            Editor.Styles[CleeLexer.StyleFunctionName].ForeColor = Color.FromArgb(139, 233, 253);
            Editor.Styles[CleeLexer.StyleString].ForeColor = Color.FromArgb(230, 204, 100);
            Editor.Styles[CleeLexer.StyleComment].ForeColor = Color.Gray;

            Result.Lexer = Lexer.Batch;
            Result.Styles[Style.Batch.Default].ForeColor = Color.White;
            Result.Styles[Style.Batch.Identifier].ForeColor = Color.FromArgb(255, 184, 108);
            Result.Styles[Style.Batch.Command].ForeColor = Color.FromArgb(139, 233, 253);
            Result.Styles[Style.Batch.Comment].ForeColor = Color.Gray;

            Editor.Lexer = Lexer.Container;

            Editor.StyleNeeded += (s, se) =>
            {
                var startPos = Editor.GetEndStyled();
                var endPos = se.Position;

                lexer.Style(Editor, startPos, endPos);
            };
        }

        private void MainPage_SizeChanged(object sender, System.EventArgs e)
        {
            Editor.Size = new Size(Width / 2, Height);
            Result.Location = new Point(Editor.Size.Width, 0);
            Result.Size = Editor.Size;
        }
        
        private void SetLineNumbers(Scintilla scintilla, ref int maxLineNumberCharLength)
        {
            // Did the number of characters in the line number display change?
            // i.e. nnn VS nn, or nnnn VS nn, etc...
            var smaxLineNumberCharLength = scintilla.Lines.Count.ToString().Length;
            if (smaxLineNumberCharLength == maxLineNumberCharLength)
                return;

            // Calculate the width required to display the last line number
            // and include some padding for good measure.
            const int padding = 2;
            scintilla.Margins[0].Width = scintilla.TextWidth(Style.LineNumber, new string('9', smaxLineNumberCharLength + 1)) + padding;
            maxLineNumberCharLength = smaxLineNumberCharLength;
        }

        private void Compile()
        {
            Result.Text = _codeGenerator.Transpile(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), Editor.Text);
        }
    }
}