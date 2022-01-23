using System;
using System.Collections;
using System.Collections.Generic;

namespace NipahData_Tokenizer.TokenizerTools
{
	public class Token
	{
		public static bool LineBreakCountAsEOF = true;
		//public static string functionText = "func";
		//public static string trueText = "true";
		//public static string falseText = "false";
		//public static string ifText = "if";
		//public static string elseText = "else";
		//public static string thenText = "then";
		//public static string endText = "end";
		public string text;
		public TokenType type;
		public int position;
		public int line;
		public object value;
		public bool consumed;

		public bool isValue => Tokenizer.isValue(type);
		public bool isOperator => Tokenizer.isOperator(type);
		public bool isMathOperator => Tokenizer.isMathOperator(type);
		public bool isComparer => Tokenizer.isComparisson(type);
		public bool isConditional => Tokenizer.isConditional(type) || text.ToLower() == "xor";
		public bool isId => type == TokenType.ID;
		public bool anyClosure => type == TokenType.EOF 
		                                           || type == TokenType.LineBreak
		                                           || type == TokenType.Comma
		                                           || isConditional
		                                           || isComparer
		                                           || isOperator
		                                           || isMathOperator
		                                           || type == TokenType.OpenBrackets
		                                           || type == TokenType.CloseBrackets
		                                           || type == TokenType.OpenSquares
		                                           || type == TokenType.CloseSquares
		                                           || type == TokenType.End;
		public bool anyClosure_Open => type == TokenType.OpenSquares
		                                                || type == TokenType.OpenBrackets
		                                                || type == TokenType.OpenParenthesis;
		public bool anyClosure_Close => type == TokenType.CloseSquares
		                                                 || type == TokenType.CloseBrackets
		                                                 || type == TokenType.CloseParenthesis;

		public Token Modify(string text, TokenType type, object value)
		{
			this.text = text;
			this.type = type;
			this.value = value;

			return this;
		}

		public void Error()
		{
			throw new CompileError($"Can't compile token [{text}], at", this);
		}
		public void SError(string source)
		{
			throw new CompileError($"Can't compile token [{text}], at", this, source);
		}
		public CompileError ISError(string source)
		{
			return new CompileError($"Can't compile token [{text}], at", this, source);
		}
		public CompileError IError()
		{
			return new CompileError($"Can't compile token [{text}], at", this);
		}
		public void Error(string message)
		{
			throw new CompileError(message, this);
		}
		public void Error(string message, string source)
		{
			throw new CompileError(message, this, source);
		}
		public CompileError IError(string message)
		{
			return new CompileError(message, this);
		}
		public CompileError IError(string message, string source)
		{
			return new CompileError(message, this, source);
		}

		public static implicit operator string (Token token) => token?.text;
		public static implicit operator TokenType (Token token) => token?.type ?? 
		                                                                 TokenType.None;
		public static implicit operator bool (Token token) => token != null;

		public static Token build(SplitItem item)
		{
			string text = item.text;
			object value = null;
			var type = TokenType.None;
			//text = text.ToLower();
			switch(text)
			{
				//case "say":type = TokenType.Say;break;
				//case "echo":type = TokenType.Echo;break;
				//case "choice":type = TokenType.Choice;break;
				//case "chose":type = TokenType.Chose;break;
				//case "define":type = TokenType.Define;break;
				//case "const":type = TokenType.Constant;break;
				//case "set":type = TokenType.Set;break;
				//case "invoke":type = TokenType.Invoke;break;
				//case "input":type = TokenType.Input;break;
				//case "label":type = TokenType.Label;break;
				//case "goto":type = TokenType.Goto;break;
				//case "to":type = TokenType.To;break;

				case "@": type = TokenType.Email;break;

				case "//":type = TokenType.Comment;break;
				case "/*": type = TokenType.CommentBegin;break;
				case "*/": type = TokenType.CommentEnd;break;

				case "(":type = TokenType.OpenParenthesis;break;
				case ")":type = TokenType.CloseParenthesis;break;
				case "[":type = TokenType.OpenSquares;break;
				case "]":type = TokenType.CloseSquares;break;
				case "{":type = TokenType.OpenBrackets;break;
				case "}":type = TokenType.CloseBrackets;break;
				case "true":
				 type = TokenType.TrueLiteral;
				 value = true;
				 break;
				case "false":
				 type = TokenType.FalseLiteral;
				 value = false;
				 break;
				case "null":type = TokenType.NullLiteral;break;
				//case "null": type = TokenType.ID;break;
				case "+":type = TokenType.Plus;break;
				case "-":type = TokenType.Minus;break;
				case "/":type = TokenType.Divide;break;
				case "*":type = TokenType.Multiply;break;
				//case "if":type = TokenType.If;break;
				//case "else":type = TokenType.Else;break;
				//case "function":type = TokenType.Function;break;
				//case "global":type = TokenType.Field;break;
				//case "public":type = TokenType.Public;break;
				//case "private":type = TokenType.Private;break;
				//case "static":type = TokenType.Static;break;
				//case "internal":type = TokenType.Internal;break;
				//case "export":type = TokenType.Export;break;
				//case "type":type = TokenType.Type;break;
				//case "for":type = TokenType.For;break;
				//case "while":type = TokenType.While;break;
				//case "foreach":type = TokenType.ForEach;break;
				case "void":type = TokenType.VoidLiteral;break;
				//case "var":type = TokenType.Variable;break;
				case "=":type = TokenType.Bind;break;
				case "==":type = TokenType.Equal;break;
				case "!=":type = TokenType.Different;break;
				case ">":type = TokenType.Larger;break;
				case "<":type = TokenType.Lower;break;
				case ">=":type = TokenType.LargerOrEqual;break;
				case "<=":type = TokenType.LowerOrEqual;break;
				case "\n":type = LineBreakCountAsEOF? TokenType.EOF : TokenType.LineBreak;break;
				case ";":type = TokenType.EOF;break;
				case ",":type = TokenType.Comma;break;
				case ":":type = TokenType.Descript;break;
				case "$":type = TokenType.Rich;break;
				case "&":type = TokenType.Reference;break;

				case "->": type = TokenType.Access;break;

				case "=>": type = TokenType.To; break;

				case "AND":type = TokenType.And;break;
				case "&&":type = TokenType.And;break;
				case "OR":type = TokenType.Or;break;
				case "||":type = TokenType.Or;break;

				case ".": type = TokenType.Dot;break;

				case "#": type = TokenType.Hashtag;break;

				case "!": type = TokenType.Exclamation;break;

				//case "return":type = TokenType.Return;break;
				//case "then":type = TokenType.Then;break;
				//case "end":type = TokenType.End;break;

				//case "loop":type = TokenType.Loop;break;
			}
			//switch(text.ToLower())
			//{
			//	case "scope":type = TokenType.Scope;break;
			//}
			if(type == TokenType.None)
			{
				if(text[0] == '"' && text[text.Length-1] == '"' && text.Length > 1)
				{
					type = TokenType.StringLiteral;
					value = text.Remove(0, 1).Remove(text.Length-2, 1);
				}else
				{
					if(tryID(text))
					{
						type = TokenType.ID;
						value = text;
						goto end;
					}
					int integer;
					if(int.TryParse(text, out integer))
					{
						type = TokenType.IntegerLiteral;
						value = integer;
						goto end;
					}
					float single;
					if(text[text.Length - 1] == 'f') {
						if(float.TryParse(text.Replace('.', ',').Replace("f", ""), out single))
						{
							type = TokenType.FloatLiteral;
							value = single;
							goto end;
						}
					}
					if(float.TryParse(text.Replace('.', ','), out single))
					{
						type = TokenType.FloatLiteral;
						value = single;
						goto end;
					}
					char c;
					if(char.TryParse(text, out c))
					{
						type = TokenType.CharLiteral;
						value = c;
					}
				}
			}
			end:
			return new Token(text, type, item.position, item.line, value);
		}
		public static bool IsValidIdentifier(string text) => tryID(text);
		static bool tryID(string text)
		{
			if(text == "" || text == null)
				return false;
			if(char.IsDigit(text[0]))
				return false;
			if(text[0] == '.')
				return false;
			foreach(var c in text)
			{
				if(char.IsDigit(c) || char.IsLetter(c) || c == '_' || c == '.')
					continue;
				if(Tokenizer.AcceptSeparatedID && c == '-')
					continue;
				return false;
			}
			return true;
		}

		public Token(string text, TokenType type, int position, int line, object value = null)
		{
			this.text = text;
			this.type = type;
			this.position = position;
			this.line = line;
			this.value = value;
		}
		public override string ToString ()
		{
			if(value != null)
				return $"Token: {text} : {type} = {value} [line: {line}]";
			return $"Token: {text} : {type} [line: {line}]";
		}
	}
	public static class TokenHelper
	{
		public static bool Assert(this Token token, TokenType type, string error = "AUTO", 
		                         string source = null)
		{
			bool result = token?.type == type;
			if(error == "AUTO")
				error = $"Expecting '{type.GetString()}', found '{token?.text ?? "NULL"}' at";
			if (token == null)
				throw new CompileError("NULL REFERENCE EXCEPTION at (0, 0)");
			if(!result && error != null)
				token.Error(error, source);
			return result;
		}
		public static bool Assert(this Token token, string text, string error = "AUTO", 
		                          string source = null)
		{
			bool result = token?.text == text;
			if(error == "AUTO")
				error = $"Expecting '[{text}]', found '[{token?.text ?? "NULL"}]' at";
			if(!result && error != null)
				token.Error(error, source);
			return result;
		}
		public static bool AssertValue(this Token token, string error = "AUTO", string source = null)
		{
			bool result = token?.isValue ?? false;
			if(error == "AUTO")
				error = $"Token '[{token.text}]' is not value, at";
			if(!result && error != null)
				token.Error(error, source);
			return result;
		}
		public static TokenType Type(this Token token) => token?.type ?? TokenType.None;
		public static object Value(this Token token) => token?.value;
		public static T Value<T>(this Token token) => (T)(token?.value ?? null);

		public static bool IsValue(this Token token) => token != null && token.isValue;
		public static bool IsOperator(this Token token) => token != null && token.isOperator;
		public static bool IsComparer(this Token token) => token != null && token.isComparer;

		public static string GetString(this TokenType type)
		{
			switch(type)
			{
				case TokenType.Access:
					return "->";
				case TokenType.To:
					return "=>";
				case TokenType.And:
					return "&&";
				case TokenType.Bind:
					return "=";
				case TokenType.CloseBrackets:
					return "}";
				case TokenType.CloseParenthesis:
					return ")";
				case TokenType.CloseSquares:
					return "]";
				case TokenType.Comma:
					return ",";
				case TokenType.Comment:
					return "//";
				case TokenType.CommentBegin:
					return "/*";
				case TokenType.CommentEnd:
					return "*/";
				case TokenType.Descript:
					return ":";
				case TokenType.Different:
					return "!=";
				case TokenType.Divide:
					return "/";
				case TokenType.Dot:
					return ".";
				case TokenType.Email:
					return "@";
				case TokenType.EOF:
					return ";";
				case TokenType.Equal:
					return "==";
				case TokenType.Exclamation:
					return "!";
				case TokenType.FalseLiteral:
					return "false";
				case TokenType.Hashtag:
					return "#";
				case TokenType.Larger:
					return ">";
				case TokenType.LargerOrEqual:
					return ">=";
				case TokenType.LineBreak:
					return "\n";
				case TokenType.Lower:
					return "<";
				case TokenType.LowerOrEqual:
					return "<=";
				case TokenType.Minus:
					return "-";
				case TokenType.Multiply:
					return "*";
				case TokenType.NullLiteral:
					return "null";
				case TokenType.OpenBrackets:
					return "{";
				case TokenType.OpenParenthesis:
					return "(";
				case TokenType.OpenSquares:
					return "[";
				case TokenType.Or:
					return "||";
				case TokenType.Plus:
					return "+";
				case TokenType.Reference:
					return "&";
				case TokenType.Rich:
					return "$";
				case TokenType.TrueLiteral:
					return "true";
				case TokenType.VoidLiteral:
					return "void";
			}
			return type.ToString();
		}

		public static Token TryPeek(this Queue<Token> tokens)
		{
			if(tokens.Count > 0)
				return tokens.Peek();
			return null;
		}
		public static Token TryDequeue(this Queue<Token> tokens)
		{
			if(tokens.Count > 0)
				return tokens.Dequeue();
			return null;
		}
		public static Queue<Token> Add(this Queue<Token> tokens, Token token)
		{
			List<Token> toks = new List<Token>(tokens);
			toks.Add(token);
			return new Queue<Token>(toks);
		}
		public static Queue<Token> Insert(this Queue<Token> tokens, int index, Token token)
		{
			List<Token> toks = new List<Token>(tokens);
			toks.Insert(index, token);
			return new Queue<Token>(toks);
		}
		public static Queue<Token> Remove(this Queue<Token> tokens, Token token)
		{
			List<Token> toks = new List<Token>(tokens);
			toks.Remove(token);
			return new Queue<Token>(toks);
		}
		public static Queue<Token> RemoveAt(this Queue<Token> tokens, int index)
		{
			List<Token> toks = new List<Token>(tokens);
			toks.RemoveAt(index);
			return new Queue<Token>(toks);
		}
	}
	public class RuntimeError : Exception
	{
		public RuntimeError(string message) : base(message)
		{
		}
	}
	public class CompileError : Exception
	{
		public override string Message
		{
			get
			{
				if(token != null && source == null)
					return base.Message + $" (Line {token.line}, Position {token.position})";
				else if(token == null && source != null)
					return base.Message + $" (File {source})";
				else if(token != null && source != null)
					return base.Message + $" (Line {token.line}, Position {token.position}," +
						       $" File {source})";
				else
					return base.Message;
			}
		}
		public Token Token => token;
		public string FileSource => source;
		Token token;
		string source;
		public CompileError(string message) : base(message)
		{
		}
		public CompileError(string message, Token token, string source = null) : base(message)
		{
			this.token = token;
			this.source = source;
		}
	}
}
