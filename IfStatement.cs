using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class IfStatement : StatetmentBase
    {
        public Expression Term { get; private set; }
        public List<StatetmentBase> DoIfTrue { get; private set; }
        public List<StatetmentBase> DoIfFalse { get; private set; }

        public IfStatement()
        {
            DoIfTrue = new List<StatetmentBase>();
            DoIfFalse = new List<StatetmentBase>();
        }

        public override void Parse(TokensStack sTokens)
        {
         
            Token tif = sTokens.Pop();
            if (!(tif is Statement) || ((Statement)tif).Name != "if")
                throw new SyntaxErrorException("Expected if received: " + tif, tif);
            
            Token open = sTokens.Pop(); //(
            if (!(open is Parentheses) || ((Parentheses)open).Name != '(')
                throw new SyntaxErrorException("Expected ( received: " + open, open);
            Expression e = Expression.Create(sTokens);
            if (e != null)
            {
                e.Parse(sTokens);
                Term = e;
            }
            else
                throw new SyntaxErrorException("Expected term received: " , null);
        
            Token close = sTokens.Pop();//)
            if (!(close is Parentheses) || ((Parentheses)close).Name != ')')
                throw new SyntaxErrorException("Expected ) received: " + close, close);
            Token startbody = sTokens.Pop();//{
            if (!(startbody is Parentheses) || ((Parentheses)startbody).Name != '{')
                throw new SyntaxErrorException("Expected { received: " + startbody, startbody);

            while (sTokens.Count > 0 && sTokens.Peek() is Statement)
            {
                StatetmentBase st = Create(sTokens.Peek());
                if (st != null)
                {
                    st.Parse(sTokens);
                    DoIfTrue.Add(st);
                }
                else
                {                 
                    throw new SyntaxErrorException("Expected statment received: " , null);
                }
            }
            Token tEnd = sTokens.Pop();//}
            if (!(tEnd is Parentheses) || ((Parentheses)tEnd).Name != '}')
                throw new SyntaxErrorException("Expected } received: " + tEnd, tEnd);



            if (sTokens.Count > 0 &&  ((Statement)sTokens.Peek()).Name == "else" ) {

                Token elseif = sTokens.Pop();

                Token start = sTokens.Pop();//{
                if (!(start is Parentheses) || ((Parentheses)start).Name != '{')
                    throw new SyntaxErrorException("Expected { received: " + start, start);

                while (sTokens.Count > 0 && sTokens.Peek() is Statement)
                {
                    StatetmentBase sta = Create(sTokens.Peek());
                    if (sta != null)
                    {
                        sta.Parse(sTokens);
                        DoIfFalse.Add(sta);
                    }
                    else
                    {
                        throw new SyntaxErrorException("Expected statment received: ", null);
                    }
                }
                Token End = sTokens.Pop();//}
                if (!(End is Parentheses) || ((Parentheses)End).Name != '}')
                    throw new SyntaxErrorException("Expected '}', received " + End, End);

            }
        }
    
    

        public override string ToString()
        {
            string sIf = "if(" + Term + "){\n";
            foreach (StatetmentBase s in DoIfTrue)
                sIf += "\t\t\t" + s + "\n";
            sIf += "\t\t}";
            if (DoIfFalse.Count > 0)
            {
                sIf += "else{";
                foreach (StatetmentBase s in DoIfFalse)
                    sIf += "\t\t\t" + s + "\n";
                sIf += "\t\t}";
            }
            return sIf;
        }

    }
}
