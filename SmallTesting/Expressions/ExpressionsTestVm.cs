using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmallTesting.Expressions
{
    public class ExpressionsTestVm : BaseViewModel
    {
        public ExpressionsTestVm()
        {
            Test2();
        }
        public string Output { get; set; }

        void Test()
        {
            Expression<Func<int, int>> addFive = (num) => num + 5;

            if (addFive.NodeType == ExpressionType.Lambda)
            {
                var lambdaExp = (LambdaExpression)addFive;

                var parameter = lambdaExp.Parameters.First();

                Output += (parameter.Name) + "\n";
                Output += (parameter.Type);


            }
        }
        void Test2()
        {
            int i = 0;
            var e = (ExpressionType)i;
            while (Enum.IsDefined(typeof(ExpressionType), e.ToString()))
            {
                Output += e.ToString() + "\n";
                ++i;
                e = (ExpressionType)i;
            }
        }
    }
}
