using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class Compiler
    {

        private Dictionary<string, int> m_dSymbolTable;
        private int m_cLocals;

        public Compiler()
        {
            m_dSymbolTable = new Dictionary<string, int>();
            m_cLocals = 0;

        }

        public List<string> Compile(string sInputFile)
        {
            List<string> lCodeLines = ReadFile(sInputFile);
            List<Token> lTokens = Tokenize(lCodeLines);
            TokensStack sTokens = new TokensStack();
            for (int i = lTokens.Count - 1; i >= 0; i--)
                sTokens.Push(lTokens[i]);
            JackProgram program = Parse(sTokens);
            return null;
        }

        private JackProgram Parse(TokensStack sTokens)
        {
            JackProgram program = new JackProgram();
            program.Parse(sTokens);
            return program;
        }

        public List<string> Compile(List<string> lLines)
        {

            List<string> lCompiledCode = new List<string>();
            foreach (string sExpression in lLines)
            {
                List<string> lAssembly = Compile(sExpression);
                lCompiledCode.Add("// " + sExpression);
                lCompiledCode.AddRange(lAssembly);
            }
            return lCompiledCode;
        }



        public List<string> ReadFile(string sFileName)
        {
            StreamReader sr = new StreamReader(sFileName);
            List<string> lCodeLines = new List<string>();
            while (!sr.EndOfStream)
            {
                lCodeLines.Add(sr.ReadLine());
            }
            sr.Close();
            return lCodeLines;
        }

        private List<string> Split(string s, char[] aDelimiters)
        {
            List<string> lTokens = new List<string>();
            while (s.Length > 0)
            {
                string sToken = "";
                int i = 0;
                for (i = 0; i < s.Length; i++)
                {
                    if (aDelimiters.Contains(s[i]))
                    {
                        if (sToken.Length > 0)
                            lTokens.Add(sToken);
                        lTokens.Add(s[i] + "");
                        break;
                    }
                    else
                        sToken += s[i];
                }
                if (i == s.Length)
                {
                    lTokens.Add(sToken);
                    s = "";
                }
                else
                    s = s.Substring(i + 1);
            }
            return lTokens;
        }

        public List<Token> Tokenize(List<string> lCodeLines)
        {        
            List<Token> lTokens = new List<Token>();
            int lineNumber = 0;
            int pos = 0;
            int index2 = 0;
            char[] Separators = new char[] { '*', '+', '-', '/', '<', '>', '&', '=', '|', '!', '(', ')', '[', ']', '{', '}', ',', ';', ' ', '\t' };

            for (int i = 0; i < lCodeLines.Count; i++)
            {
                string sline = lCodeLines[i];
                pos = 0;

                if (sline.Length > 1)
                {
                    if (sline.Substring(0, 2) == "//")
                    {
                        lineNumber++;
                        continue;
                    }
                }

                if (sline.Contains("//"))
                {
                    index2 = sline.IndexOf("/");


                }
                else
                {
                    index2 = sline.Length;
                }

                List<string> sp = Split(sline.Substring(0, index2), Separators);

                for (int j = 0; j < sp.Count; j++)
                {
                    if (sp[j] == " ")
                    {
                        pos += 1;
                        continue;
                    }
                    else if (sp[j] == "\t")
                    {
                        pos += 1;
                        continue;
                    }

                    else if (Token.Statements.Contains(sp[j]))
                    {
                        Statement state;

                        if (sp[j] == Token.Statements[0])
                        {
                            state = new Statement("function", lineNumber, pos);
                            lTokens.Add(state);
                            pos += 8;
                        }
                        else if (sp[j] == Token.Statements[1])
                        {
                            state = new Statement("var", lineNumber, pos);
                            lTokens.Add(state);
                            pos += 3;
                        }
                        else if (sp[j] == Token.Statements[2])
                        {
                            state = new Statement("let", lineNumber, pos);
                            lTokens.Add(state);
                            pos += 3;
                        }

                        else if (sp[j] == Token.Statements[3])
                        {
                            state = new Statement("while", lineNumber, pos);
                            lTokens.Add(state);
                            pos += 5;
                        }
                        else if (sp[j] == Token.Statements[4])
                        {
                            state = new Statement("if", lineNumber, pos);
                            lTokens.Add(state);
                            pos += 2;
                        }
                        else if (sp[j] == Token.Statements[5])
                        {
                            state = new Statement("else", lineNumber, pos);
                            lTokens.Add(state);
                            pos += 4;
                        }
                        else if (sp[j] == Token.Statements[6])
                        {
                            state = new Statement("return", lineNumber, pos);
                            lTokens.Add(state);
                            pos += 6;
                        }
                    }

                    else if (Token.VarTypes.Contains(sp[j]))
                    {
                        VarType var;

                        if (sp[j] == Token.VarTypes[0])
                        {
                            var = new VarType("int", lineNumber, pos);
                            lTokens.Add(var);
                            pos += 3;
                        }
                        else if (sp[j] == Token.VarTypes[1])
                        {
                            var = new VarType("char", lineNumber, pos);
                            lTokens.Add(var);
                            pos += 4;
                        }
                        else if (sp[j] == Token.VarTypes[2])
                        {
                            var = new VarType("boolean", lineNumber, pos);
                            lTokens.Add(var);
                            pos += 7;
                        }
                        else if (sp[j] == Token.VarTypes[3])
                        {
                            var = new VarType("array", lineNumber, pos);
                            lTokens.Add(var);
                            pos += 5;
                        }
                    }

                    else if (Token.Constants.Contains(sp[j]))
                    {
                        Constant cons;

                        if (sp[j] == Token.Constants[0])
                        {
                            cons = new Constant("true", lineNumber, pos);
                            lTokens.Add(cons);
                            pos += 4;
                        }
                        else if (sp[j] == Token.Constants[1])
                        {
                            cons = new Constant("false", lineNumber, pos);
                            lTokens.Add(cons);
                            pos += 5;
                        }
                        else if (sp[j] == Token.Constants[2])
                        {
                            cons = new Constant("null", lineNumber, pos);
                            lTokens.Add(cons);
                            pos += 4;
                        }

                    }

                    else if (Token.Operators.Contains(sp[j][0]))
                    {
                        Operator op;

                        if (sp[j][0] == Token.Operators[0])
                        {
                            op = new Operator('*', lineNumber, pos);
                            lTokens.Add(op);
                            pos += 1;
                        }
                        else if (sp[j][0] == Token.Operators[1])
                        {
                            op = new Operator('+', lineNumber, pos);
                            lTokens.Add(op);
                            pos += 1;
                        }
                        else if (sp[j][0] == Token.Operators[2])
                        {
                            op = new Operator('-', lineNumber, pos);
                            lTokens.Add(op);
                            pos += 1;
                        }
                        else if (sp[j][0] == Token.Operators[3])
                        {
                            op = new Operator('/', lineNumber, pos);
                            lTokens.Add(op);
                            pos += 1;
                        }
                        else if (sp[j][0] == Token.Operators[4])
                        {
                            op = new Operator('<', lineNumber, pos);
                            lTokens.Add(op);
                            pos += 1;
                        }
                        else if (sp[j][0] == Token.Operators[5])
                        {
                            op = new Operator('>', lineNumber, pos);
                            lTokens.Add(op);
                            pos += 1;
                        }
                        else if (sp[j][0] == Token.Operators[6])
                        {
                            op = new Operator('&', lineNumber, pos);
                            lTokens.Add(op);
                            pos += 1;
                        }
                        else if (sp[j][0] == Token.Operators[7])
                        {
                            op = new Operator('=', lineNumber, pos);
                            lTokens.Add(op);
                            pos += 1;
                        }
                        else if (sp[j][0] == Token.Operators[8])
                        {
                            op = new Operator('|', lineNumber, pos);
                            lTokens.Add(op);
                            pos += 1;
                        }
                        else if (sp[j][0] == Token.Operators[9])
                        {
                            op = new Operator('!', lineNumber, pos);
                            lTokens.Add(op);
                            pos += 1;
                        }
                    }

                    else if (Token.Parentheses.Contains(sp[j][0]))
                    {
                        Parentheses pare;

                        if (sp[j][0] == Token.Parentheses[0])
                        {
                            pare = new Parentheses('(', lineNumber, pos);
                            lTokens.Add(pare);
                            pos += 1;
                        }
                        else if (sp[j][0] == Token.Parentheses[1])
                        {
                            pare = new Parentheses(')', lineNumber, pos);
                            lTokens.Add(pare);
                            pos += 1;
                        }
                        else if (sp[j][0] == Token.Parentheses[2])
                        {
                            pare = new Parentheses('[', lineNumber, pos);
                            lTokens.Add(pare);
                            pos += 1;
                        }
                        else if (sp[j][0] == Token.Parentheses[3])
                        {
                            pare = new Parentheses(']', lineNumber, pos);
                            lTokens.Add(pare);
                            pos += 1;
                        }
                        else if (sp[j][0] == Token.Parentheses[4])
                        {
                            pare = new Parentheses('{', lineNumber, pos);
                            lTokens.Add(pare);
                            pos += 1;
                        }
                        else if (sp[j][0] == Token.Parentheses[5])
                        {
                            pare = new Parentheses('}', lineNumber, pos);
                            lTokens.Add(pare);
                            pos += 1;
                        }

                    }

                    else if (Token.Separators.Contains(sp[j][0]))
                    {
                        Separator sep;

                        if (sp[j][0] == Token.Separators[0])
                        {
                            sep = new Separator(',', lineNumber, pos);
                            lTokens.Add(sep);
                            pos += 1;
                        }
                        else if (sp[j][0] == Token.Separators[1])
                        {
                            sep = new Separator(';', lineNumber, pos);
                            lTokens.Add(sep);
                            pos += 1;
                        }
                    }

                    else if (Int32.TryParse(sp[j], out int num))
                    {
                        Number number;
                        number = new Number(sp[j], lineNumber, pos);
                        lTokens.Add(number);
                        pos += sp[j].Length;
                    }

                    else
                    {
                        if (!Int32.TryParse(sp[j].Substring(0, 1), out int n) && sp[j] != "#" && sp[j] != "@" && sp[j] != "$" && sp[j] != "%" && sp[j] != "^" && sp[j] != "~")
                        {
                            Identifier id;
                            id = new Identifier(sp[j], lineNumber, pos);
                            lTokens.Add(id);
                            pos += sp[j].Length;
                        }
                        else
                        {
                            throw new SyntaxErrorException("SyntaxErrorException", new Token());
                        }
                    }

                }
                lineNumber++;
            }
                lineNumber = 0;
                return lTokens;
        }
    }
}
