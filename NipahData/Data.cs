using NipahData_Tokenizer;
using NipahData_Tokenizer.TokenizerTools;
using System;
using System.Collections.Generic;
using System.Text;

namespace NipahData
{
    public class Data
    {
        Dictionary<string, DataMember> members = new Dictionary<string, DataMember>(16);

        public string this[string member, string key]
        {
            get => members[member][key];
            set => members[member][key] = value;
        }
        public DataMember this[string member]
        {
            get
            {
                members.TryGetValue(member, out DataMember result);
                return result;
            }
            set => members[member] = value;
        }
        /// <summary>
        /// Get's all the members with the specified prefix + '_'
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public List<DataMember> GetMembersWithPrefix(string prefix)
        {
            List<DataMember> result = new List<DataMember>(8);
            foreach(var member in members)
            {
                if (checkPrefix(prefix, member.Key))
                    result.Add(member.Value);
            }
            bool checkPrefix(string prf, string id)
            {
                int crp = 0;
                int prfCount = prf.Length;
                int count = id.Length;
                if (count < prfCount)
                    return false;
                for(int i = 0; i < count; i++)
                {
                    var c = id[i];
                    if(i < prfCount)
                    {
                        if (c == prf[i])
                            crp++;
                    }else
                    {
                        if (c == '_')
                            return crp == prfCount;
                        return false;
                    }
                }
                return false;
            }
            return result;
        }

        public Dictionary<string, DataMember>.Enumerator GetEnumerator() => members.GetEnumerator();

        static Tokenizer tokenizer = new Tokenizer();
        public static Data ReadDataFrom(string dataString)
        {
            Tokenizer.AcceptSeparatedID = true;
            var data = new Data();
            var currentCharging = Lists<string>.Get();
            var tokens = new ProgressiveList<Token>(tokenizer.Tokenize(dataString));
            Token token;
            while(token = tokens.Next())
            {
                if(token == TokenType.OpenBrackets)
                {
                    readMember(token, tokens, data, currentCharging);
                    currentCharging.Clear();
                    continue;
                }
                if (token == TokenType.Comma)
                {
                    if (currentCharging.Count == 0)
                        throw token.IError("Expecting 'ID', instead, found ',', at ");
                }
                if (currentCharging.Count > 0)
                {
                    if(!token.Assert(TokenType.Comma))
                        return null;
                    token = tokens.Next();
                }
                currentCharging.Add((string)token.value);
            }
            currentCharging.Return();
            return data;
        }
        static void readMember(Token token, ProgressiveList<Token> tokens, Data data, List<string> keepTracking)
        {
            List<DataMember> members = Lists<DataMember>.Get();
            foreach (var keep in keepTracking)
                members.Add(new DataMember(keep));
            keepTracking.Clear();
            int count = members.Count;
            while(token = tokens.Next(t => t == TokenType.EOF))
            {
                if (token == TokenType.CloseBrackets)
                    break;
                if(token == TokenType.Dot)
                {
                    token = tokens.Next();
                    members.ForEach(m => m.AddClass((string)token.value));
                    continue;
                }
                if(token == TokenType.Hashtag)
                {
                    token = tokens.Next();
                    members.ForEach(m => m.Predicate = (string)token.value);
                    continue;
                }
                string key = (string)token.value;
                token = tokens.Next();
                token.Assert(TokenType.Descript);
                while(token = tokens.Next())
                {
                    if(token == TokenType.EOF)
                    {
                        if(keepTracking.Count < count)
                            throw token.IError($"Expecting {count} values, instead, received {keepTracking.Count}. At ");
                        for (int i = 0; i < count; i++)
                            members[i][key] = keepTracking[i];
                        keepTracking.Clear();
                        break;
                    }
                    if(keepTracking.Count > 0)
                    {
                        token.Assert(TokenType.Comma);
                        token = tokens.Next();
                    }
                    token.AssertValue();
                    string value = token.value.ToString();
                    keepTracking.Add(value);
                    if(tokens.Look_Next() == TokenType.Dot)
                    {
                        token = tokens.Next();
                        token = tokens.Next();
                        token.Assert(TokenType.Dot);
                        token = tokens.Next();
                        token.Assert(TokenType.Dot);
                        token = tokens.Next();
                        if(token == TokenType.Multiply)
                        {
                            int final = count - keepTracking.Count;
                            for (int i = 0; i < final; i++)
                                keepTracking.Add(value);
                        }else if(token == TokenType.IntegerLiteral)
                        {
                            int final = (int)token.value;
                            for (int i = 0; i < final; i++)
                                keepTracking.Add(value);
                        }
                    }
                    if (keepTracking.Count > count)
                        throw token.IError($"Expecting {count} values, instead, received {keepTracking.Count}. At ");
                }
            }
            DataMember old = null;
            foreach (var member in members)
            {
                data[member.id] = member;
                if (old != null)
                    member.SetLink(old);
                old = member;
            }
            members.Return();
        }
        static StringBuilder builder = new StringBuilder(32);
        public override string ToString()
        {
            builder.Clear();
            List<DataMember> passed = Lists<DataMember>.Get();
            List<DataMember> linkList = Lists<DataMember>.Get();
            foreach(var memberEntry in members)
            {
                string id = memberEntry.Key;
                DataMember member = memberEntry.Value;
                if (passed.Contains(member))
                    continue;
                if (member.Linked != null)
                {
                    DataMember temp = member;
                detect:
                    linkList.Add(temp);
                    passed.Add(temp);
                    if (temp.Linked != null)
                    {
                        temp = member.Linked;
                        goto detect;
                    }
                }
                else
                {
                    linkList.Add(member);
                    passed.Add(member);
                }
                for (int i = 0; i < linkList.Count; i++)
                {
                    if (i > 0)
                        builder.Append(", ");
                    DataMember link = linkList[i];
                    builder.Append(link.id);
                }
                builder.AppendLine(" {");
                foreach(var link in linkList)
                {
                    bool insertKey = false;
                    foreach(var value in link)
                    {
                        if (!insertKey)
                        {
                            builder.Append($"{value.Key}: ");
                            insertKey = true;
                        }
                        else
                            builder.Append(", ");
                        builder.Append($"\"{value.Value}\"");
                    }
                    builder.AppendLine();
                }
                builder.AppendLine("}");
            }
            linkList.Return();
            return builder.ToString();
        }
    }
    public class DataMember
    {
        public readonly string id;
        Dictionary<string, string> values = new Dictionary<string, string>(32);
        List<string> classes = new List<string>();
        string predicate;
        public string Predicate
        {
            get => predicate;
            set => predicate = value;
        }
        public DataMember Linked => linked;

        public void AddClass(string cls) => classes.Add(cls);
        public bool HasClass(string cls) => classes.Contains(cls);
        public bool RemoveClass(string cls) => classes.Remove(cls);

        public bool Compatible(string pre, List<string> cls)
        {
            if (!string.IsNullOrEmpty(predicate) && pre != predicate)
                return false;
            return classes.Exists(f => cls.Contains(f));
        }

        DataMember linked;
        public void SetLink(DataMember member) => linked = member;

        public string this[string key]
        {
            get
            {
                values.TryGetValue(key, out string value);
                return value;
            }
            set => values[key] = value;
        }
        public Dictionary<string, string> GetValues() => values;

        public Dictionary<string, string>.Enumerator GetEnumerator()
        {
            return values.GetEnumerator();
        }

        public DataMember(string id)
        {
            this.id = id;
        }
    }
}
