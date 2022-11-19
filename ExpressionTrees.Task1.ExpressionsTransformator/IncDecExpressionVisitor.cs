using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    public class IncDecExpressionVisitor : ExpressionVisitor
    {
        private Dictionary<string, int> _dictioanry;

        public IncDecExpressionVisitor()
        {
            _dictioanry = new Dictionary<string, int>();
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (_dictioanry.TryGetValue(node.Member.Name, out var value))
            {
                return Expression.Constant(value);
            }

            return base.VisitMember(node);
        }

        public Expression Modify(Expression node, Dictionary<string, int> dictionary = null)
        {
            if (dictionary != null)
                _dictioanry = dictionary;
            
            return Visit(node);
        }
        
        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Add:
                    var expressionInc = Visit(node.Left);
                    return Expression.Increment(expressionInc);
                case ExpressionType.Subtract:
                    var expressionDec = Visit(node.Left);
                    return Expression.Decrement(expressionDec);
            }
            return base.VisitBinary(node);
        }
    }
}
