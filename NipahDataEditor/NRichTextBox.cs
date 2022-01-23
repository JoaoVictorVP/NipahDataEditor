/*
 * Criado por SharpDevelop.
 * Usuário: Computador Pessoal
 * Data: 02/02/2020
 * Hora: 00:08
 * 
 * Para alterar este modelo use Ferramentas | Opções | Codificação | Editar Cabeçalhos Padrão.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace NipahScriptEditor
{
	public class NRichTextBox : RichTextBox
	{
		public const int TAB_SIZE = 3;
		public static bool UseNTab = true;

		public static void InsertTab(NRichTextBox textbox)
		{
			int at = textbox.SelectionStart;
			textbox.Text = textbox.Text.Insert(at, NTab);
			textbox.SelectionStart = at + TAB_SIZE;
		}
		public static void RemoveTab(NRichTextBox textbox)
		{
			int at = textbox.SelectionStart;
			string text = textbox.Text;
			textbox.Text = text.Remove(at - TAB_SIZE, TAB_SIZE);
			textbox.SelectionStart = at - TAB_SIZE;
		}
		public static bool AreTab(NRichTextBox textbox)
		{
			int at = textbox.SelectionStart;
			string text = textbox.Text;
			//int size = text.Length;

			int satisfied = 0;
			for(int i = at - 1; i >= 0; i--) {
				var c = text[i];
				if(c == ' ')
					satisfied++;
				else
					return false;

				if(satisfied == TAB_SIZE)
					return true;
			}
			return false;
		}
		public static void AddNewLine(NRichTextBox textbox)
		{
			var loc = textbox.SelectionStart;
			string text = textbox.Text;
			int spaces = countSpacesBefore(text, loc - 1);
			string newLine = buildNewLine(spaces);
			text = text.Insert(loc, newLine);
			textbox.Text = text;
			textbox.SelectionStart = loc + newLine.Length;
		}
		static string buildNewLine(int spaces)
		{
			string str = "\n";
			for(int i = 0; i < spaces; i++)
				str += ' ';
			return str;
		}
		static int countSpacesBefore(string text, int pos)
		{
			int count = 0;
			for(int i = pos; i >= 0; i--) {
				var c = text[i];
				if(c == ' ')
					count++;
				else if(c == '\n')
					break;
				else
					count = 0;
			}
			return count;
		}
		static string ntab;
		public static string NTab
		{
			get
			{
				if(ntab == null)
				{
					for(int i = 0; i < TAB_SIZE; i++)
						ntab += ' ';
				}
				return ntab;
			}
		}

		public event KeyHandle handle;

		struct Event
		{
			public string text;
			public int loc;

			public void Load(NRichTextBox textbox)
			{
				textbox.Text = text;
				textbox.SelectionStart = loc;
			}

			public Event(NRichTextBox textbox)
			{
				this.text = textbox.Text;
				this.loc = textbox.SelectionStart;
			}
		}
		protected override void OnTextChanged(EventArgs e)
		{
			if(this.Text.Contains("#TAB#"))
				this.Text = this.Text.Replace("#TAB#", "   ");
			base.OnTextChanged(e);
		}
		Stack<Event> events = new Stack<Event>(32);
		Stack<Event> undoed = new Stack<Event>(32);
		public void DoUndo()
		{
			var cur = new Event(this);
			if(!undoed.Contains(cur))
				undoed.Push(cur);
			if(events.Count > 0)
			{
				var e = events.Pop();
				e.Load(this);
				if(!undoed.Contains(e))
				undoed.Push(e);
			}
		}
		public void DoRedo()
		{
			if(undoed.Count > 0)
			{
				var e = undoed.Pop();
				e.Load(this);
				if(!events.Contains(e))
					events.Push(e);
			}
		}
		public void ClearEvents()
		{
			events.Clear();
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			events.Push(new Event(this));
		}
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Space 
			    || e.KeyCode == Keys.Tab
			    || e.KeyCode == Keys.Enter)
				events.Push(new Event(this));

			if(handle != null)
			{
				bool handled = e.Handled;
				handle(e, ref handled);
				e.Handled = handled;
				if(handled)
					return;
			}
			base.OnKeyDown(e);
			if(e.SuppressKeyPress)
				return;
			
			if(UseNTab)
			{
				if(e.KeyCode == Keys.Tab)
				{
					InsertTab(this);
					e.Handled = true;
					e.SuppressKeyPress = true;
					return;
				}
				else if(e.KeyCode == Keys.Back && AreTab(this))
				{
					RemoveTab(this);
					e.Handled = true;
					e.SuppressKeyPress = true;
					return;
				}
				if(e.KeyCode == Keys.Enter)
				{
					AddNewLine(this);
					e.Handled = true;
					e.SuppressKeyPress = true;
				}
			}

			if(e.Control) {
				if(e.KeyCode == Keys.Z) {
					if(e.Shift)
						DoRedo();
					else
						DoUndo();
					e.SuppressKeyPress = true;
				}
				if(e.KeyCode == Keys.Add) {
					ZoomFactor += 0.25F;
					e.SuppressKeyPress = true;
				}
				else if(e.KeyCode == Keys.Subtract) {
					ZoomFactor -= 0.25F;
					e.SuppressKeyPress = true;
				}
			}
		}
		public delegate void KeyHandle(KeyEventArgs e, ref bool isHandled);
	}
}
