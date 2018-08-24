using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundamentals.Functional.Math.Numerics.TypedNumeric
{
    public class RatioUnit : NumericUnit
    {
        public new static readonly RatioUnit Unit = new RatioUnit();

        protected RatioUnit() : base()
        {
        }

        private string numerator;
        private string denominator;
        public RatioUnit(string numerator, string denominator) : base(new TypeStruct(numerator, 1), new TypeStruct(denominator, -1))
        {
            this.numerator = numerator;
            this.denominator = denominator;
        }

        public override string ToString()
        {
            return $"{numerator} per {denominator}";
        }
    }

    public class RatioType<T, U> : RatioUnit
    {
        public readonly T Numerator;
        public readonly U Denominator;
        public new static readonly RatioType<T, U> Unit = new RatioType<T, U>();

        protected RatioType() : base()
        {
        }

        public RatioType(T numerator, U denominator) : base(numerator.ToString(), denominator.ToString())
        {
            this.Numerator = numerator;
            this.Denominator = denominator;
        }
    }

    public class RatioType<T> : RatioType<T, T>
    {
        public new static readonly RatioType<T> Unit = new RatioType<T>();

        protected RatioType() : base()
        {
        }
        public RatioType(T numerator, T denominator) : base(numerator, denominator)
        {
        }
    }
}
