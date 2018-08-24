using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundamentals.Functional.Math.Numerics.TypedNumeric
{
    public class NumericUnit
    {
        public static readonly NumericUnit Unit = new NumericUnit();

        protected struct TypeStruct
        {
            public readonly String Type;
            public readonly int Power;

            public TypeStruct(string type, int power)
            {
                this.Type = type;
                this.Power = power;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is TypeStruct))
                {
                    return false;
                }

                var @struct = (TypeStruct)obj;
                return Type == @struct.Type &&
                       Power == @struct.Power;
            }

            public override int GetHashCode()
            {
                var hashCode = 1097676891;
                hashCode = hashCode * -1521134295 + base.GetHashCode();
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Type);
                hashCode = hashCode * -1521134295 + Power.GetHashCode();
                return hashCode;
            }

            public override string ToString()
            {
                return this.Power != 0 ? (this.Type + (this.Power != 1 ? $"^{this.Power}" : "")) : "Unit";
            }
        }

        protected readonly SortedList<string, TypeStruct> types;

        private NumericUnit()
        {
            this.types = new SortedList<string, TypeStruct>
            {
                { "Unit", new TypeStruct("Unit", 0) }
            };
        }

        protected NumericUnit(TypeStruct type)
        {
            this.types = new SortedList<string, TypeStruct>
            {
                { type.Type, type }
            };
        }

        protected NumericUnit(params TypeStruct[] types)
        {
            this.types = new SortedList<string, TypeStruct>();
            foreach (var type in types)
            {
                if (!this.types.ContainsKey(type.Type))
                {
                    this.types.Add(type.Type, type);
                }
                else
                {
                    var newPower = this.types[type.Type].Power + type.Power;
                    if (newPower != 0)
                    {
                        this.types[type.Type] = new TypeStruct(type.Type, newPower);
                    }
                    else
                    {
                        this.types.Remove(type.Type);
                    }
                }
            }
        }

        protected NumericUnit(IEnumerable<TypeStruct> types)
        {
            this.types = new SortedList<string, TypeStruct>();
            foreach (var type in types)
            {
                this.types.Add(type.Type, type);
            }
        }

        public static NumericUnit CreateNumericUnit(params NumericUnit[] others)
        {
            var result = new NumericUnit();
            foreach (var other in others)
            {
                result = result.Multiply(other);
            }

            return result;
        }

        public override string ToString()
        {
            var result = "";
            if (this.types.Values.Count == 1)
            {
                result = this.types.Values[0].ToString();
            }
            else
            {
                result = "(";
                foreach (var type in this.types.Values)
                {
                    result += $"({type.ToString()})";
                }
                result += ")";
            }
            return result;
        }

        public string ToString(string formatting, params object[] args)
        {
            return string.Format(formatting, args) + this.ToString();
        }

        public static NumericUnit operator *(NumericUnit first, NumericUnit second) => first.Multiply(second);

        public virtual NumericUnit Multiply(NumericUnit second)
        {
            if (this == Unit || (this.types.Count == 1 && this.types.ContainsKey("Unit")))
            {
                return new NumericUnit(second.types.Values);
            }

            var resultTypes = new List<TypeStruct>();

            foreach (var key in this.types.Keys)
            {
                if (second.types.TryGetValue(key, out var otherType))
                {
                    var newPower = this.types[key].Power + otherType.Power;
                    if (newPower != 0)
                    {
                        resultTypes.Add(new TypeStruct(key, newPower));
                    }
                }
                else
                {
                    resultTypes.Add(new TypeStruct(key, this.types[key].Power));
                }
            }

            return new NumericUnit(resultTypes);
        }

        public static NumericUnit operator /(NumericUnit first, NumericUnit second) => first.Divide(second);

        public virtual NumericUnit Divide(NumericUnit second)
        {
            return this.Multiply(new NumericUnit(second.types.Values.Select((value) => new TypeStruct(value.Type, -1 * value.Power))));
        }

        public static bool operator ==(NumericUnit first, NumericUnit second)
        {
            return !first.Equals(null) && !second.Equals(null) && first.Equals(second);
        }

        public static bool operator !=(NumericUnit first, NumericUnit second)
        {
            return (first is null && !(second is null)) ||
                   (!(first is null) && (second is null || !(first.Equals(second))));
        }

        public bool Equals(NumericUnit other)
        {
            if (!(other is null))
            {
                var allTypesMatch = this.types.Count == other.types.Count &&
                    this.types.All((pair) => other.types.TryGetValue(pair.Key, out var otherValue) && otherValue.Power == pair.Value.Power);

                return allTypesMatch;
            }
            return false;
        }

        public bool IsUnit()
        {
            return this.types.Count == 0 || (this.types.Count == 1 && this.types.ContainsKey("Unit"));
        }

        public override bool Equals(object obj)
        {
            return !(obj is null) && this.Equals(obj as NumericUnit);
        }

        public override int GetHashCode()
        {
            return 627613354 + EqualityComparer<SortedList<string, TypeStruct>>.Default.GetHashCode(types);
        }
    }

    public class NumericUnit<T> : NumericUnit
    {
        public new static readonly NumericUnit<T> Unit = new NumericUnit<T>();

        public NumericUnit(T value, int power = 1) : base(new TypeStruct(value.ToString(), power))
        {
        }

        protected NumericUnit() : base(new TypeStruct("Unit", 0))
        {
        }
    }
}
