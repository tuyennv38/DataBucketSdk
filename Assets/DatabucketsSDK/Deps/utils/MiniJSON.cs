// MiniJSON.cs
// Source: https://gist.github.com/darktable/1411710
// Slightly formatted for clarity and compatibility with Unity

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DatabucketsSDK.Utils
{
    public static class Json
    {
        public static object Deserialize(string json)
        {
            if (json == null) return null;
            return Parser.Parse(json);
        }

        public static string Serialize(object obj)
        {
            try {
                return Serializer.Serialize(obj);
            } catch (Exception) {
                return "{}";
            }
        }

        private sealed class Parser : IDisposable
        {
            const string WORD_BREAK = "{}[],:\"";

            public static bool IsWordBreak(char c)
            {
                return Char.IsWhiteSpace(c) || WORD_BREAK.IndexOf(c) != -1;
            }

            enum TOKEN
            {
                NONE, CURLY_OPEN, CURLY_CLOSE, SQUARE_OPEN, SQUARE_CLOSE,
                COLON, COMMA, STRING, NUMBER, TRUE, FALSE, NULL
            }

            StringReader json;

            Parser(string jsonString)
            {
                json = new StringReader(jsonString);
            }

            public static object Parse(string jsonString)
            {
                try
                {
                    using (var instance = new Parser(jsonString))
                    {
                        return instance.ParseValue();
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            public void Dispose()
            {
                json.Dispose();
                json = null;
            }

            Dictionary<string, object> ParseObject()
            {
                Dictionary<string, object> table = new Dictionary<string, object>();

                json.Read(); // skip '{'

                while (true)
                {
                    switch (NextToken)
                    {
                        case TOKEN.NONE:
                            return null;
                        case TOKEN.COMMA:
                            continue;
                        case TOKEN.CURLY_CLOSE:
                            return table;
                        default:
                            string name = ParseString();
                            if (name == null) return null;

                            if (NextToken != TOKEN.COLON) return null;
                            json.Read(); // skip ':'

                            table[name] = ParseValue();
                            break;
                    }
                }
            }

            List<object> ParseArray()
            {
                List<object> array = new List<object>();
                json.Read(); // skip '['

                var parsing = true;
                while (parsing)
                {
                    TOKEN nextToken = NextToken;

                    switch (nextToken)
                    {
                        case TOKEN.NONE:
                            return null;
                        case TOKEN.COMMA:
                            continue;
                        case TOKEN.SQUARE_CLOSE:
                            parsing = false;
                            break;
                        default:
                            object value = ParseValue();
                            array.Add(value);
                            break;
                    }
                }

                return array;
            }

            object ParseValue()
            {
                switch (NextToken)
                {
                    case TOKEN.STRING: return ParseString();
                    case TOKEN.NUMBER: return ParseNumber();
                    case TOKEN.CURLY_OPEN: return ParseObject();
                    case TOKEN.SQUARE_OPEN: return ParseArray();
                    case TOKEN.TRUE: return true;
                    case TOKEN.FALSE: return false;
                    case TOKEN.NULL: return null;
                    default: return null;
                }
            }

            string ParseString()
            {
                try
                {
                    StringBuilder s = new StringBuilder();
                    char c;

                    json.Read(); // skip "

                    bool parsing = true;
                    while (parsing)
                    {
                        if (json.Peek() == -1) break;
                        c = NextChar;

                        switch (c)
                        {
                            case '"':
                                parsing = false;
                                break;
                            case '\\':
                                if (json.Peek() == -1) parsing = false;
                                c = NextChar;
                                switch (c)
                                {
                                    case '"': s.Append('"'); break;
                                    case '\\': s.Append('\\'); break;
                                    case '/': s.Append('/'); break;
                                    case 'b': s.Append('\b'); break;
                                    case 'f': s.Append('\f'); break;
                                    case 'n': s.Append('\n'); break;
                                    case 'r': s.Append('\r'); break;
                                    case 't': s.Append('\t'); break;
                                    case 'u':
                                        try
                                        {
                                            var hex = new char[4];
                                            for (int i = 0; i < 4; i++) hex[i] = NextChar;
                                            s.Append((char)Convert.ToInt32(new string(hex), 16));
                                        }
                                        catch
                                        {
                                            // Invalid unicode escape, skip it
                                        }
                                        break;
                                }
                                break;
                            default:
                                s.Append(c);
                                break;
                        }
                    }

                    return s.ToString();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            object ParseNumber()
            {
                string number = NextWord;

                if (number.IndexOf('.') == -1)
                {
                    if (long.TryParse(number, out long parsedInt)) return parsedInt;
                }

                if (double.TryParse(number, out double parsedDouble)) return parsedDouble;

                return null;
            }

            void EatWhitespace()
            {
                while (Char.IsWhiteSpace(PeekChar)) json.Read();
            }

            char PeekChar => Convert.ToChar(json.Peek());
            char NextChar => Convert.ToChar(json.Read());

            string NextWord
            {
                get
                {
                    StringBuilder word = new StringBuilder();
                    while (!IsWordBreak(PeekChar))
                    {
                        word.Append(NextChar);
                        if (json.Peek() == -1) break;
                    }
                    return word.ToString();
                }
            }

            TOKEN NextToken
            {
                get
                {
                    EatWhitespace();
                    if (json.Peek() == -1) return TOKEN.NONE;

                    char c = PeekChar;
                    switch (c)
                    {
                        case '{': return TOKEN.CURLY_OPEN;
                        case '}': json.Read(); return TOKEN.CURLY_CLOSE;
                        case '[': return TOKEN.SQUARE_OPEN;
                        case ']': json.Read(); return TOKEN.SQUARE_CLOSE;
                        case ',': json.Read(); return TOKEN.COMMA;
                        case ':': return TOKEN.COLON;
                        case '"': return TOKEN.STRING;
                    }

                    string word = NextWord;
                    switch (word)
                    {
                        case "true": return TOKEN.TRUE;
                        case "false": return TOKEN.FALSE;
                        case "null": return TOKEN.NULL;
                    }

                    return double.TryParse(word, out _) ? TOKEN.NUMBER : TOKEN.NONE;
                }
            }
        }

        private sealed class Serializer
        {
            StringBuilder builder;

            Serializer()
            {
                builder = new StringBuilder();
            }

            public static string Serialize(object obj)
            {
                try {
                    var instance = new Serializer();
                    instance.SerializeValue(obj);
                    return instance.builder.ToString();
                } catch (Exception) {
                    return "{}";
                }
            }

            void SerializeValue(object value)
            {
                try {
                    if (value == null)
                    {
                        builder.Append("null");
                    }
                    else if (value is string)
                    {
                        SerializeString((string)value);
                    }
                    else if (value is bool)
                    {
                        builder.Append((bool)value ? "true" : "false");
                    }
                    else if (value is IList)
                    {
                        SerializeArray((IList)value);
                    }
                    else if (value is IDictionary)
                    {
                        SerializeObject((IDictionary)value);
                    }
                    else if (value is char)
                    {
                        SerializeString(new string((char)value, 1));
                    }
                    else
                    {
                        SerializeOther(value);
                    }
                } catch (Exception) {
                    builder.Append("null");
                }
            }

            void SerializeObject(IDictionary obj)
            {
                try {
                    bool first = true;

                    builder.Append('{');
                    foreach (object e in obj.Keys)
                    {
                        if (!first) builder.Append(',');
                        SerializeString(e.ToString());
                        builder.Append(':');
                        SerializeValue(obj[e]);
                        first = false;
                    }
                    builder.Append('}');
                } catch (Exception) {
                    builder.Append("{}");
                }
            }

            void SerializeArray(IList array)
            {
                try {
                    builder.Append('[');
                    bool first = true;
                    foreach (object obj in array)
                    {
                        if (!first) builder.Append(',');
                        SerializeValue(obj);
                        first = false;
                    }
                    builder.Append(']');
                } catch (Exception) {
                    builder.Append("[]");
                }
            }

            void SerializeString(string str)
            {
                try {
                    builder.Append('\"');

                    foreach (char c in str)
                    {
                        switch (c)
                        {
                            case '"': builder.Append("\\\""); break;
                            case '\\': builder.Append("\\\\"); break;
                            case '\b': builder.Append("\\b"); break;
                            case '\f': builder.Append("\\f"); break;
                            case '\n': builder.Append("\\n"); break;
                            case '\r': builder.Append("\\r"); break;
                            case '\t': builder.Append("\\t"); break;
                            default:
                                if (c < 32 || c > 126)
                                {
                                    builder.Append("\\u");
                                    builder.Append(((int)c).ToString("x4"));
                                }
                                else
                                {
                                    builder.Append(c);
                                }
                                break;
                        }
                    }

                    builder.Append('\"');
                } catch (Exception) {
                    builder.Append("\"\"");
                }
            }

            void SerializeOther(object value)
            {
                try {
                    if (value is float || value is double || value is decimal)
                    {
                        builder.Append(Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        builder.Append(value.ToString());
                    }
                } catch (Exception) {
                    builder.Append("null");
                }
            }
        }
    }
}
