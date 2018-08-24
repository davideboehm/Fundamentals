using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fundamentals.Functional.Math.Numerics.TypedNumeric
{
    public class TypedNumeric
    {
        public readonly NumericUnit Units;
        public readonly Numeric Value;

        protected TypedNumeric()
        {
        }

        public TypedNumeric(Numeric value, NumericUnit units)
        {
            this.Value = value;
            this.Units = units;
        }

        public static explicit operator Numeric(TypedNumeric value)
        {
            return value.Value;
        }

        public static TypedNumeric operator /(TypedNumeric first, TypedNumeric second)
        {
            return new TypedNumeric(second.Value / first.Value, first.Units / second.Units);
        }

        public static TypedNumeric operator *(TypedNumeric first, TypedNumeric second)
        {
            return new TypedNumeric(second.Value * first.Value, first.Units * second.Units);
        }

        public static TypedNumeric operator *(TypedNumeric first, Numeric second)
        {
            return new TypedNumeric(first.Value * second, first.Units);
        }
        public static TypedNumeric operator *(Numeric first, TypedNumeric second) => second * first;

        public bool HasCompatibleUnits(TypedNumeric other)
        {
            return (this.Units == other.Units || (this.Units.IsUnit() || other.Units.IsUnit()));
        }

        public static bool operator !=(Numeric first, TypedNumeric second)
        {
            return !(second == first);
        }

        public static bool operator !=(TypedNumeric first, Numeric second)
        {
            return !(first == second);
        }

        public static bool operator ==(Numeric first, TypedNumeric second)
        {
            return second == first;
        }

        public static bool operator ==(TypedNumeric first, Numeric second)
        {
            return second == first.Value;
        }

        public static bool operator !=(TypedNumeric first, TypedNumeric second)
        {
            return !(first == second);
        }

        public static bool operator ==(TypedNumeric first, TypedNumeric second)
        {
            if (first.HasCompatibleUnits(second))
            {
                return second.Value == first.Value;
            }

            return false;
        }

        public static bool operator >(TypedNumeric first, Numeric second)
        {
            return first.Value > second;
        }

        public static bool operator <(TypedNumeric first, Numeric second)
        {
            return first.Value < second;
        }

        public static bool operator >(TypedNumeric first, TypedNumeric second)
        {
            if (first.HasCompatibleUnits(second))
            {
                return first.Value > second.Value;
            }

            return false;
        }

        public static bool operator <(TypedNumeric first, TypedNumeric second)
        {
            if (first.HasCompatibleUnits(second))
            {
                return first.Value < second.Value;
            }

            return false;
        }
    }

    public class TypedNumeric<T> : TypedNumeric where T : NumericUnit
    {
        public new readonly T Units;
        public TypedNumeric(Numeric value, T units) : base(value, units)
        {
            this.Units = units;
        }

        public static TypedNumeric<T> operator *(Numeric first, TypedNumeric<T> second) => second * first;

        public static TypedNumeric<T> operator *(TypedNumeric<T> first, Numeric second)
        {
            return new TypedNumeric<T>(second * first.Value, first.Units);
        }

        public static TypedNumeric<RatioType<T>> operator /(TypedNumeric<T> first, TypedNumeric<T> second)
        {
            return new TypedNumeric<RatioType<T>>(first.Value / second.Value, new RatioType<T>(first.Units, second.Units));
        }

        public static TypedNumeric<T> operator /(TypedNumeric<T> first, Numeric second)
        {
            return new TypedNumeric<T>(first.Value / second, first.Units);
        }

        public static TypedNumeric<T> operator +(TypedNumeric<T> first, TypedNumeric<T> second)
        {
            if (first.HasCompatibleUnits(second))
            {
                return new TypedNumeric<T>(second.Value + first.Value, first.Units);
            }

            throw new ArgumentException("The units of these two values do not match");
        }

        public static TypedNumeric<T> operator -(TypedNumeric<T> first, TypedNumeric<T> second)
        {
            if (first.HasCompatibleUnits(second))
            {
                return new TypedNumeric<T>(second.Value + first.Value, first.Units);
            }

            throw new ArgumentException("The units of these two values do not match");
        }

        public static implicit operator decimal(TypedNumeric<T> value)
        {
            return value.Value;
        }

        public static implicit operator double(TypedNumeric<T> value)
        {
            return value.Value;
        }

        public static implicit operator float(TypedNumeric<T> value)
        {
            return value.Value;
        }

        public static implicit operator int(TypedNumeric<T> value)
        {
            return value.Value;
        }

        public override string ToString()
        {
            return this.Value.ToString() + " " + this.Units.ToString();
        }
    }

    public class TypedNumeric<T, U> : TypedNumeric<T> where T : NumericUnit where U : TypedNumeric<T, U>
    {
        public TypedNumeric(Numeric value, T units) : base(value, units)
        {
        }

        public TypedNumeric(TypedNumeric<T> value) : base(value.Value, value.Units)
        {
        }

        public static U operator /(TypedNumeric<T, U> first, Numeric second)
        {
            return TypedNumeric<T, U>.Generate(first.Value / second, first.Units);
        }

        public static U operator *(Numeric first, TypedNumeric<T, U> second) => second * first;

        public static U operator *(TypedNumeric<T, U> first, Numeric second)
        {
            return TypedNumeric<T, U>.Generate(first.Value * second, first.Units);
        }

        public static U operator +(TypedNumeric<T, U> first, TypedNumeric<T, U> second)
        {
            if (first.HasCompatibleUnits(second))
            {
                return TypedNumeric<T, U>.Generate(first.Value + second.Value, first.Units != NumericUnit.Unit ? first.Units : second.Units);
            }

            throw new ArgumentException("The units of these two values do not match");
        }

        public static U operator -(TypedNumeric<T, U> first, TypedNumeric<T, U> second)
        {
            if (first.HasCompatibleUnits(second))
            {
                return TypedNumeric<T, U>.Generate(first.Value - second.Value, first.Units != NumericUnit.Unit ? first.Units : second.Units);
            }

            throw new ArgumentException("The units of these two values do not match");
        }

        public static U operator -(TypedNumeric<T, U> first, TypedNumeric second)
        {
            if (first.HasCompatibleUnits(second))
            {
                return TypedNumeric<T, U>.Generate(first.Value - second.Value, first.Units);
            }

            throw new ArgumentException("The units of these two values do not match");
        }

        private static U Generate(Numeric value, T units)
        {
            return (U)Activator.CreateInstance(typeof(U),
                                                 BindingFlags.CreateInstance |
                                                 BindingFlags.NonPublic |
                                                 BindingFlags.Instance |
                                                 BindingFlags.OptionalParamBinding,
                                                 null, new Object[] { value, units }, null);
        }
    }
}
