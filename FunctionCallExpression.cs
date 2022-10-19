using System;
using System.Collections.Generic;

namespace SimpleCompiler
{
    public class FunctionCallExpression : Expression
    {
        public string FunctionName { get; private set; }
        public List<Expression> Args { get; private set; }

        public FunctionCallExpression()
        {
            Args = new List<Expression>();
        }

        public override void Parse(TokensStack sTokens)
        {
            Token tFunc = sTokens.Pop();
            if (!(tFunc is Identifier)) 
                throw new SyntaxErrorException("Expected function received: " + tFunc, tFunc);
            else
            {
                FunctionName = ((Identifier)tFunc).Name;
            }
            Token open = sTokens.Pop();//(
            if (!(open is Parentheses))
            {
               throw new SyntaxErrorException("Expected '(' received: " + open, open);
            }
            while(sTokens.Count > 0) 
            {          
                Expression exe = Create(sTokens);
                exe.Parse(sTokens);
                Args.Add(exe);

               if (sTokens.Count > 0 && sTokens.Peek() is Separator)//,
                  sTokens.Pop();
                if ((sTokens.Peek() is Parentheses) && (((Parentheses)sTokens.Peek()).Name == ')'))
                    break;

            }

            Token close = sTokens.Pop();//)
            if (!(close is Parentheses))
            {
                throw new SyntaxErrorException("Expected ')' received: " + close, close);
            }
        }

        public override string ToString()
        {
            string sFunction = FunctionName + "(";
            for (int i = 0; i < Args.Count - 1; i++)
                sFunction += Args[i] + ",";
            if (Args.Count > 0)
                sFunction += Args[Args.Count - 1];
            sFunction += ")";
            return sFunction;
        }
    }
}