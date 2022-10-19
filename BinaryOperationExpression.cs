using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class BinaryOperationExpression : Expression
    {
        public string Operator { get;  set; }
        public Expression Operand1 { get;  set; }
        public Expression Operand2 { get;  set; }

        public override string ToString()
        {
            return "(" + Operand1 + " " + Operator + " " + Operand2 + ")";
        }

        public override void Parse(TokensStack sTokens)
        {
            Token pare1 = sTokens.Pop();
            if (!(pare1 is Parentheses) || ((Parentheses)pare1).Name != '(')
                throw new SyntaxErrorException("Expected ( received: " + pare1, pare1);
         
            Expression ex1 = Create(sTokens);
            if (!(ex1 == null))
            {
                ex1.Parse(sTokens);
                Operand1 = ex1;

            }
            else
            {
               
                throw new SyntaxErrorException("Expected operand received: " , null);
            }

            Token oper = sTokens.Pop();
            if (!(oper is Operator))
            {
                throw new SyntaxErrorException("Expected operator received: " + oper, oper);          
            }
            else
            {
                char c = ((Operator)oper).Name;
                Operator = c.ToString();
            }

            Expression ex2 = Create(sTokens);
            if (!(ex2 == null))
            {
                ex2.Parse(sTokens);
                Operand2 = ex2;

            }
            else
            {
                
                throw new SyntaxErrorException("Expected operand received: " ,null);
            }

            Token pare2 = sTokens.Pop();
            if (!(pare2 is Parentheses) || ((Parentheses)pare2).Name != ')')
                throw new SyntaxErrorException("Expected ) received: " + pare2, pare2);


        }
    }
}
