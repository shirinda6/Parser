using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class LetStatement : StatetmentBase
    {
        public string Variable { get; set; }
        public Expression Value { get; set; }

      
        public override string ToString()
        {
            return "let " + Variable + " = " + Value + ";";
        }

        public override void Parse(TokensStack sTokens)
        {
            Token let = sTokens.Pop();
            if (!(let is Statement) || (((Statement)let).Name != "let"))
                throw new SyntaxErrorException("Expected let, received " + let, let);        
            Token id = sTokens.Pop();
            if (!(id is Identifier))
                throw new SyntaxErrorException("Expected id received " + id, id);       
            Token eq = sTokens.Pop();
            if (!(eq is Operator) || ((Operator)eq).Name != '=')
                throw new SyntaxErrorException("Expected operator = received " + eq, eq);
                                   
            Variable = ((Identifier)id).Name;              
            Expression e = Expression.Create(sTokens);                    
            if (e != null)
            {
                e.Parse(sTokens);
                Value = e;
            }
            else
            {
               
                throw new SyntaxErrorException("Expected operator = received " , null);
            }
            Token tEnd = sTokens.Pop();
            if (!(tEnd is Separator) || ((Separator)tEnd).Name != ';')
                throw new SyntaxErrorException("Expected ';' , received " + tEnd, tEnd);
           

            //And call the Parse method of the statement to parse the different parts of the statement    
        }

    }
}
