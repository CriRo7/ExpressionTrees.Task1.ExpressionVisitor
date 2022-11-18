/*
 * Create a class based on ExpressionVisitor, which makes expression tree transformation:
 * 1. converts expressions like <variable> + 1 to increment operations, <variable> - 1 - into decrement operations.
 * 2. changes parameter values in a lambda expression to constants, taking the following as transformation parameters:
 *    - source expression;
 *    - dictionary: <parameter name: value for replacement>
 * The results could be printed in console or checked via Debugger using any Visualizer.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Expression Visitor for increment/decrement.");
            Console.WriteLine();

            // todo: feel free to add your code here
            var model = new Model(10, "model");
            var visitor = new IncDecExpressionVisitor();

            ProcessIncrement(visitor, model);

            ProcessDecrement(visitor, model);

            ProcessReplace(visitor, model);

            Console.ReadLine();
        }

        private static void ProcessIncrement(IncDecExpressionVisitor visitor, Model model)
        {
            Expression<Func<Model, int>> expressionInc = value => value.IntProperty + 1;
            Console.WriteLine($"Input expression: {expressionInc}");

            var resultIncExpression = (Expression<Func<Model, int>>)visitor.Modify(expressionInc);
            Console.WriteLine($"Result expressions: Inc: {resultIncExpression}");
            var funcInc = resultIncExpression.Compile();
            Console.WriteLine($"Result: Inc: {funcInc(model)}");
            Console.WriteLine("=================");
        }

        private static void ProcessDecrement(IncDecExpressionVisitor visitor, Model model)
        {
            Expression<Func<Model, int>> expressionDec = value => value.IntProperty - 1;
            Console.WriteLine($"Input expression: {expressionDec}");

            var resultDecExpression = (Expression<Func<Model, int>>)visitor.Modify(expressionDec);
            Console.WriteLine($"Result expressions: Dec: {resultDecExpression}");
            var funcDec = resultDecExpression.Compile();
            Console.WriteLine($"Result: Dec: {funcDec(model)}");
            Console.WriteLine("=================");
        }

        private static void ProcessReplace(IncDecExpressionVisitor visitor, Model model)
        {
            Expression<Func<Model, int>> expressionInc = value => value.IntProperty + 1;
            Console.WriteLine($"Input expression: {expressionInc}");
            var dictionary = new Dictionary<string, int>() { {"IntProperty", 5}};
            Console.WriteLine($"Dictionary <propName: valueToReplace>: {dictionary.ToArray()[0].Key}, {dictionary.ToArray()[0].Value}");

            var resultSubstituteExpression = (Expression<Func<Model, int>>) visitor.Modify(expressionInc, dictionary);
            Console.WriteLine($"Result expression: Substitute Inc: {resultSubstituteExpression}");
            var funcSub = resultSubstituteExpression.Compile();
            Console.WriteLine($"Result Substitute: {funcSub(model)}");
            Console.WriteLine("=================");
        }
    }

    class Model
    {
        public Model(int intProperty, string stringProperty)
        {
            IntProperty = intProperty;
            StringProperty = stringProperty;
        }
        public int IntProperty { get; set; }
        public string StringProperty { get; set; }
    }
}
