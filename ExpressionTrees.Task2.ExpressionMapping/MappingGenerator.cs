using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTrees.Task2.ExpressionMapping
{
    public class MappingGenerator
    {
        public Mapper<TSource, TDestination> Generate<TSource, TDestination>(Expression<Func<TSource, object>> sourceProp, 
            Expression<Func<TDestination, object>> destinationProp)
        {
            if (sourceProp.Body is UnaryExpression sourceUnaryExpression
                && sourceUnaryExpression.Operand is MemberExpression sourceMemberExpression
                && destinationProp.Body is UnaryExpression destUnaryExpression
                && destUnaryExpression.Operand is MemberExpression destMemberExpression)
            {
                var mapFunction = CreateLambdaExpression<TSource, TDestination>(sourceMemberExpression.Member, 
                    destMemberExpression.Member);
                Console.WriteLine($"{mapFunction}");
                return new Mapper<TSource, TDestination>(mapFunction.Compile());
            }

            if (sourceProp.Body is MemberExpression bodySourceMemberExpression
                && destinationProp.Body is MemberExpression bodyDestMemberExpression)
            {
                var mapFunction = CreateLambdaExpression<TSource, TDestination>(bodySourceMemberExpression.Member, 
                    bodyDestMemberExpression.Member);
                Console.WriteLine($"{mapFunction}");
                return new Mapper<TSource, TDestination>(mapFunction.Compile());
            }
            
            throw new ArgumentException("Source's and destination's property types are different.");
        }

        private Expression<Func<TSource, TDestination>> CreateLambdaExpression<TSource, TDestination>(
            MemberInfo sourceMember,
            MemberInfo destinationMember)
        {
            var sourceParam = Expression.Parameter(typeof(TSource));
            var sourceMemberAccess = Expression.MakeMemberAccess(sourceParam, sourceMember);

            var ctor = Expression.New(typeof(TDestination));
            var destinationAssignment = Expression.Bind(destinationMember, sourceMemberAccess);
            var init = Expression.MemberInit(ctor, destinationAssignment);
            
            var mapFunction =
                Expression.Lambda<Func<TSource, TDestination>>(
                    init,
                    sourceParam
                );

            return mapFunction;
        }
    }
}
