using System;

namespace Fundamentals.Functional.Math.Numerics
{
    public class Numeric
    {
        public static Numeric Zero = new Numeric();

        protected Numeric()
        {
            type = ValueType.zero;
        }

        protected Numeric(Numeric other)
        {
            this.type = other.type;
            this.intValue = other.intValue;
            this.doubleValue = other.doubleValue;
            this.decimalValue = other.decimalValue;
            this.floatValue = other.floatValue;
        }

        protected enum ValueType
        {
            zero,
            intType,
            doubleType,
            decimalType,
            floatType
        }
        private ValueType type;
        private int intValue;
        private double doubleValue;
        private decimal decimalValue;
        private float floatValue;

        public override int GetHashCode()
        {
            switch (this.type)
            {
                case ValueType.decimalType:
                    {
                        return this.decimalValue.GetHashCode();
                    }
                case ValueType.floatType:
                    {
                        return this.floatValue.GetHashCode();
                    }
                case ValueType.intType:
                    {
                        return this.intValue.GetHashCode();
                    }
                case ValueType.doubleType:
                    {
                        return this.doubleValue.GetHashCode();
                    }
                default:
                    {
                        return 0;
                    }
            }
        }

        public override bool Equals(object obj)
        {
            var other = obj as Numeric;
            if(other !=null)
            {
                return this == other;
            }

            return false;
        }

        public static bool operator >(Numeric first, Numeric second)
        {
            switch (first.type)
            {
                case ValueType.intType:
                    {
                        switch (second.type)
                        {
                            case ValueType.intType:
                                {
                                    return first.intValue > second.intValue;
                                }
                            case ValueType.doubleType:
                                {
                                    return first.intValue > second.doubleValue;
                                }
                            case ValueType.decimalType:
                                {
                                    return (decimal)first.intValue > second.decimalValue;
                                }
                            case ValueType.floatType:
                                {
                                    return (float)first.intValue > second.floatValue;
                                }
                            case ValueType.zero:
                                {
                                    return first.intValue > 0;
                                }
                            default:
                                {
                                    throw new NotImplementedException();
                                }
                        }
                    }
                case ValueType.doubleType:
                    {
                        switch (second.type)
                        {
                            case ValueType.intType:
                                {
                                    return first.doubleValue > second.intValue;
                                }
                            case ValueType.doubleType:
                                {
                                    return first.doubleValue > second.doubleValue;
                                }
                            case ValueType.decimalType:
                                {
                                    return (decimal)first.doubleValue > second.decimalValue;
                                }
                            case ValueType.floatType:
                                {
                                    return first.doubleValue > second.floatValue;
                                }
                            case ValueType.zero:
                                {
                                    return first.doubleValue > 0;
                                }
                            default:
                                {
                                    throw new NotImplementedException();
                                }
                        }
                    }
                case ValueType.decimalType:
                    {
                        switch (second.type)
                        {
                            case ValueType.intType:
                                {
                                    return first.decimalValue > second.intValue;
                                }
                            case ValueType.doubleType:
                                {
                                    return first.decimalValue > (decimal)second.doubleValue;
                                }
                            case ValueType.decimalType:
                                {
                                    return first.decimalValue > second.decimalValue;
                                }
                            case ValueType.floatType:
                                {
                                    return first.decimalValue > (decimal)second.floatValue;
                                }
                            case ValueType.zero:
                                {
                                    return first.decimalValue > 0;
                                }
                            default:
                                {
                                    throw new NotImplementedException();
                                }
                        }
                    }
                case ValueType.floatType:
                    {
                        switch (second.type)
                        {
                            case ValueType.intType:
                                {
                                    return first.floatValue > second.intValue;
                                }
                            case ValueType.doubleType:
                                {
                                    return first.floatValue > second.doubleValue;
                                }
                            case ValueType.decimalType:
                                {
                                    return (decimal)first.floatValue > second.decimalValue;
                                }
                            case ValueType.floatType:
                                {
                                    return first.floatValue > second.floatValue;
                                }
                            case ValueType.zero:
                                {
                                    return first.floatValue > 0;
                                }
                            default:
                                {
                                    throw new NotImplementedException();
                                }
                        }
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }

        public static bool operator <(Numeric first, Numeric second)
        {
            switch (first.type)
            {
                case ValueType.intType:
                    {
                        switch (second.type)
                        {
                            case ValueType.intType:
                                {
                                    return first.intValue < second.intValue;
                                }
                            case ValueType.doubleType:
                                {
                                    return first.intValue < second.doubleValue;
                                }
                            case ValueType.decimalType:
                                {
                                    return (decimal)first.intValue < second.decimalValue;
                                }
                            case ValueType.floatType:
                                {
                                    return (float)first.intValue < second.floatValue;
                                }
                            case ValueType.zero:
                                {
                                    return first.intValue < 0;
                                }
                            default:
                                {
                                    throw new NotImplementedException();
                                }
                        }
                    }
                case ValueType.doubleType:
                    {
                        switch (second.type)
                        {
                            case ValueType.intType:
                                {
                                    return first.doubleValue < second.intValue;
                                }
                            case ValueType.doubleType:
                                {
                                    return first.doubleValue < second.doubleValue;
                                }
                            case ValueType.decimalType:
                                {
                                    return (decimal)first.doubleValue < second.decimalValue;
                                }
                            case ValueType.floatType:
                                {
                                    return first.doubleValue < second.floatValue;
                                }
                            case ValueType.zero:
                                {
                                    return first.doubleValue < 0;
                                }
                            default:
                                {
                                    throw new NotImplementedException();
                                }
                        }
                    }
                case ValueType.decimalType:
                    {
                        switch (second.type)
                        {
                            case ValueType.intType:
                                {
                                    return first.decimalValue < second.intValue;
                                }
                            case ValueType.doubleType:
                                {
                                    return first.decimalValue < (decimal)second.doubleValue;
                                }
                            case ValueType.decimalType:
                                {
                                    return first.decimalValue < second.decimalValue;
                                }
                            case ValueType.floatType:
                                {
                                    return first.decimalValue < (decimal)second.floatValue;
                                }
                            case ValueType.zero:
                                {
                                    return first.decimalValue < 0;
                                }
                            default:
                                {
                                    throw new NotImplementedException();
                                }
                        }
                    }
                case ValueType.floatType:
                    {
                        switch (second.type)
                        {
                            case ValueType.intType:
                                {
                                    return first.floatValue < second.intValue;
                                }
                            case ValueType.doubleType:
                                {
                                    return first.floatValue < second.doubleValue;
                                }
                            case ValueType.decimalType:
                                {
                                    return (decimal)first.floatValue < second.decimalValue;
                                }
                            case ValueType.floatType:
                                {
                                    return first.floatValue < second.floatValue;
                                }
                            case ValueType.zero:
                                {
                                    return first.floatValue < 0;
                                }
                            default:
                                {
                                    throw new NotImplementedException();
                                }
                        }
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }

        public static bool operator !=(Numeric first, Numeric second)
        {
            return !(first == second);
        }

        public static bool operator ==(Numeric first, Numeric second)
        {
            switch (first.type)
            {
                case ValueType.intType:
                    {
                        switch (second.type)
                        {
                            case ValueType.intType:
                                {
                                    return first.intValue == second.intValue;
                                }
                            case ValueType.doubleType:
                                {
                                    return first.intValue == second.doubleValue;
                                }
                            case ValueType.decimalType:
                                {
                                    return (decimal)first.intValue == second.decimalValue;
                                }
                            case ValueType.floatType:
                                {
                                    return (float)first.intValue == second.floatValue;
                                }
                            case ValueType.zero:
                                {
                                    return first.intValue == 0;
                                }
                            default:
                                {
                                    throw new NotImplementedException();
                                }
                        }
                    }
                case ValueType.doubleType:
                    {
                        switch (second.type)
                        {
                            case ValueType.intType:
                                {
                                    return first.doubleValue == second.intValue;
                                }
                            case ValueType.doubleType:
                                {
                                    return first.doubleValue == second.doubleValue;
                                }
                            case ValueType.decimalType:
                                {
                                    return (decimal)first.doubleValue == second.decimalValue;
                                }
                            case ValueType.floatType:
                                {
                                    return first.doubleValue == second.floatValue;
                                }
                            case ValueType.zero:
                                {
                                    return first.doubleValue == 0;
                                }
                            default:
                                {
                                    throw new NotImplementedException();
                                }
                        }
                    }
                case ValueType.decimalType:
                    {
                        switch (second.type)
                        {
                            case ValueType.intType:
                                {
                                    return first.decimalValue == second.intValue;
                                }
                            case ValueType.doubleType:
                                {
                                    return first.decimalValue == (decimal)second.doubleValue;
                                }
                            case ValueType.decimalType:
                                {
                                    return first.decimalValue == second.decimalValue;
                                }
                            case ValueType.floatType:
                                {
                                    return first.decimalValue == (decimal)second.floatValue;
                                }
                            case ValueType.zero:
                                {
                                    return first.decimalValue == 0;
                                }
                            default:
                                {
                                    throw new NotImplementedException();
                                }
                        }
                    }
                case ValueType.floatType:
                    {
                        switch (second.type)
                        {
                            case ValueType.intType:
                                {
                                    return first.floatValue == second.intValue;
                                }
                            case ValueType.doubleType:
                                {
                                    return first.floatValue == second.doubleValue;
                                }
                            case ValueType.decimalType:
                                {
                                    return (decimal)first.floatValue == second.decimalValue;
                                }
                            case ValueType.floatType:
                                {
                                    return first.floatValue == second.floatValue;
                                }
                            case ValueType.zero:
                                {
                                    return first.floatValue == 0;
                                }
                            default:
                                {
                                    throw new NotImplementedException();
                                }
                        }
                    }
                    case ValueType.zero:
                    {
                        switch (second.type)
                        {
                            case ValueType.intType:
                                {
                                    return 0 == second.intValue;
                                }
                            case ValueType.doubleType:
                                {
                                    return 0 == second.doubleValue;
                                }
                            case ValueType.decimalType:
                                {
                                    return 0 == second.decimalValue;
                                }
                            case ValueType.floatType:
                                {
                                    return 0 == second.floatValue;
                                }
                            case ValueType.zero:
                                {
                                    return true;
                                }
                            default:
                                {
                                    throw new NotImplementedException();
                                }
                        }
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }

        public static Numeric operator /(Numeric first, Numeric second)
        {
            if (second == Numeric.Zero)
            {
                throw new DivideByZeroException();
            }

            if (first == Numeric.Zero)
            {
                return Numeric.Zero;
            }

            switch (first.type)
            {
                case ValueType.intType:
                    {
                        switch (second.type)
                        {
                            case ValueType.intType:
                                {
                                    return first.intValue / second.intValue;
                                }
                            case ValueType.doubleType:
                                {
                                    return first.intValue / second.doubleValue;
                                }
                            case ValueType.decimalType:
                                {
                                    return (decimal)first.intValue / second.decimalValue;
                                }
                            case ValueType.floatType:
                                {
                                    return (float)first.intValue / second.floatValue;
                                }
                            default:
                                {
                                    throw new NotImplementedException();
                                }
                        }
                    }
                case ValueType.doubleType:
                    {
                        switch (second.type)
                        {
                            case ValueType.intType:
                                {
                                    return first.doubleValue / second.intValue;
                                }
                            case ValueType.doubleType:
                                {
                                    return first.doubleValue / second.doubleValue;
                                }
                            case ValueType.decimalType:
                                {
                                    return (decimal)first.doubleValue / second.decimalValue;
                                }
                            case ValueType.floatType:
                                {
                                    return first.doubleValue / second.floatValue;
                                }
                            default:
                                {
                                    throw new NotImplementedException();
                                }
                        }
                    }
                case ValueType.decimalType:
                    {
                        switch (second.type)
                        {
                            case ValueType.intType:
                                {
                                    return first.decimalValue / second.intValue;
                                }
                            case ValueType.doubleType:
                                {
                                    return first.decimalValue / (decimal)second.doubleValue;
                                }
                            case ValueType.decimalType:
                                {
                                    return first.decimalValue / second.decimalValue;
                                }
                            case ValueType.floatType:
                                {
                                    return first.decimalValue / (decimal)second.floatValue;
                                }
                            default:
                                {
                                    throw new NotImplementedException();
                                }
                        }
                    }
                case ValueType.floatType:
                    {
                        switch (second.type)
                        {
                            case ValueType.intType:
                                {
                                    return first.floatValue / second.intValue;
                                }
                            case ValueType.doubleType:
                                {
                                    return first.floatValue / second.doubleValue;
                                }
                            case ValueType.decimalType:
                                {
                                    return (decimal)first.floatValue / second.decimalValue;
                                }
                            case ValueType.floatType:
                                {
                                    return first.floatValue / second.floatValue;
                                }
                            default:
                                {
                                    throw new NotImplementedException();
                                }
                        }
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }

        public static Numeric operator -(Numeric first, Numeric second)
        {
            if (second == Numeric.Zero)
            {
                return first;
            }

            if (first == Numeric.Zero)
            {
                return second * (-1);
            }

            switch (first.type)
            {
                case ValueType.intType:
                    {
                        switch (second.type)
                        {
                            case ValueType.intType:
                                {
                                    return first.intValue - second.intValue;
                                }
                            case ValueType.doubleType:
                                {
                                    return first.intValue - second.doubleValue;
                                }
                            case ValueType.decimalType:
                                {
                                    return (decimal)first.intValue - second.decimalValue;
                                }
                            case ValueType.floatType:
                                {
                                    return (float)first.intValue - second.floatValue;
                                }
                            default:
                                {
                                    throw new NotImplementedException();
                                }
                        }
                    }
                case ValueType.doubleType:
                    {
                        switch (second.type)
                        {
                            case ValueType.intType:
                                {
                                    return first.doubleValue - second.intValue;
                                }
                            case ValueType.doubleType:
                                {
                                    return first.doubleValue - second.doubleValue;
                                }
                            case ValueType.decimalType:
                                {
                                    return (decimal)first.doubleValue - second.decimalValue;
                                }
                            case ValueType.floatType:
                                {
                                    return first.doubleValue - second.floatValue;
                                }
                            default:
                                {
                                    throw new NotImplementedException();
                                }
                        }
                    }
                case ValueType.decimalType:
                    {
                        switch (second.type)
                        {
                            case ValueType.intType:
                                {
                                    return first.decimalValue - second.intValue;
                                }
                            case ValueType.doubleType:
                                {
                                    return first.decimalValue - (decimal)second.doubleValue;
                                }
                            case ValueType.decimalType:
                                {
                                    return first.decimalValue - second.decimalValue;
                                }
                            case ValueType.floatType:
                                {
                                    return first.decimalValue - (decimal)second.floatValue;
                                }
                            default:
                                {
                                    throw new NotImplementedException();
                                }
                        }
                    }
                case ValueType.floatType:
                    {
                        switch (second.type)
                        {
                            case ValueType.intType:
                                {
                                    return first.floatValue - second.intValue;
                                }
                            case ValueType.doubleType:
                                {
                                    return first.floatValue - second.doubleValue;
                                }
                            case ValueType.decimalType:
                                {
                                    return (decimal)first.floatValue - second.decimalValue;
                                }
                            case ValueType.floatType:
                                {
                                    return first.floatValue - second.floatValue;
                                }
                            default:
                                {
                                    throw new NotImplementedException();
                                }
                        }
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }

        public static Numeric operator +(Numeric first, Numeric second)
        {
            if (second == Numeric.Zero)
            {
                return first;
            }

            if (first == Numeric.Zero)
            {
                return second;
            }

            switch (first.type)
            {
                case ValueType.intType:
                    {
                        switch (second.type)
                        {
                            case ValueType.intType:
                                {
                                    return first.intValue + second.intValue;
                                }
                            case ValueType.doubleType:
                                {
                                    return first.intValue + second.doubleValue;
                                }
                            case ValueType.decimalType:
                                {
                                    return (decimal)first.intValue + second.decimalValue;
                                }
                            case ValueType.floatType:
                                {
                                    return (float)first.intValue + second.floatValue;
                                }
                            default:
                                {
                                    throw new NotImplementedException();
                                }
                        }
                    }
                case ValueType.doubleType:
                    {
                        switch (second.type)
                        {
                            case ValueType.intType:
                                {
                                    return first.doubleValue + second.intValue;
                                }
                            case ValueType.doubleType:
                                {
                                    return first.doubleValue + second.doubleValue;
                                }
                            case ValueType.decimalType:
                                {
                                    return (decimal)first.doubleValue + second.decimalValue;
                                }
                            case ValueType.floatType:
                                {
                                    return first.doubleValue + second.floatValue;
                                }
                            default:
                                {
                                    throw new NotImplementedException();
                                }
                        }
                    }
                case ValueType.decimalType:
                    {
                        switch (second.type)
                        {
                            case ValueType.intType:
                                {
                                    return first.decimalValue + second.intValue;
                                }
                            case ValueType.doubleType:
                                {
                                    return first.decimalValue + (decimal)second.doubleValue;
                                }
                            case ValueType.decimalType:
                                {
                                    return first.decimalValue + second.decimalValue;
                                }
                            case ValueType.floatType:
                                {
                                    return first.decimalValue + (decimal)second.floatValue;
                                }
                            default:
                                {
                                    throw new NotImplementedException();
                                }
                        }
                    }
                case ValueType.floatType:
                    {
                        switch (second.type)
                        {
                            case ValueType.intType:
                                {
                                    return first.floatValue + second.intValue;
                                }
                            case ValueType.doubleType:
                                {
                                    return first.floatValue + second.doubleValue;
                                }
                            case ValueType.decimalType:
                                {
                                    return (decimal)first.floatValue + second.decimalValue;
                                }
                            case ValueType.floatType:
                                {
                                    return first.floatValue + second.floatValue;
                                }
                            default:
                                {
                                    throw new NotImplementedException();
                                }
                        }
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }

        public static Numeric operator *(Numeric first, Numeric second)
        {
            if (second == Numeric.Zero || first == Numeric.Zero)
            {
                return Numeric.Zero;
            }

            switch (first.type)
            {
                case ValueType.intType:
                    {
                        switch (second.type)
                        {
                            case ValueType.intType:
                                {
                                    return first.intValue * second.intValue;
                                }
                            case ValueType.doubleType:
                                {
                                    return first.intValue * second.doubleValue;
                                }
                            case ValueType.decimalType:
                                {
                                    return (decimal)first.intValue * second.decimalValue;
                                }
                            case ValueType.floatType:
                                {
                                    return (float)first.intValue * second.floatValue;
                                }
                            default:
                                {
                                    throw new NotImplementedException();
                                }
                        }
                    }
                case ValueType.doubleType:
                    {
                        switch (second.type)
                        {
                            case ValueType.intType:
                                {
                                    return first.doubleValue * second.intValue;
                                }
                            case ValueType.doubleType:
                                {
                                    return first.doubleValue * second.doubleValue;
                                }
                            case ValueType.decimalType:
                                {
                                    return (decimal)first.doubleValue * second.decimalValue;
                                }
                            case ValueType.floatType:
                                {
                                    return first.doubleValue * second.floatValue;
                                }
                            default:
                                {
                                    throw new NotImplementedException();
                                }
                        }
                    }
                case ValueType.decimalType:
                    {
                        switch (second.type)
                        {
                            case ValueType.intType:
                                {
                                    return first.decimalValue * second.intValue;
                                }
                            case ValueType.doubleType:
                                {
                                    return first.decimalValue * (decimal)second.doubleValue;
                                }
                            case ValueType.decimalType:
                                {
                                    return first.decimalValue * second.decimalValue;
                                }
                            case ValueType.floatType:
                                {
                                    return first.decimalValue * (decimal)second.floatValue;
                                }
                            default:
                                {
                                    throw new NotImplementedException();
                                }
                        }
                    }
                case ValueType.floatType:
                    {
                        switch (second.type)
                        {
                            case ValueType.intType:
                                {
                                    return first.floatValue * second.intValue;
                                }
                            case ValueType.doubleType:
                                {
                                    return first.floatValue * second.doubleValue;
                                }
                            case ValueType.decimalType:
                                {
                                    return (decimal)first.floatValue * second.decimalValue;
                                }
                            case ValueType.floatType:
                                {
                                    return first.floatValue * second.floatValue;
                                }
                            default:
                                {
                                    throw new NotImplementedException();
                                }
                        }
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }

        public static implicit operator decimal(Numeric value)
        {
            switch (value.type)
            {
                case ValueType.intType:
                    {
                        return value.intValue;
                    }
                case ValueType.doubleType:
                    {
                        return (decimal)value.doubleValue;
                    }
                case ValueType.decimalType:
                    {
                        return value.decimalValue;
                    }
                case ValueType.floatType:
                    {
                        return (decimal)value.floatValue;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }

        public static implicit operator double(Numeric value)
        {
            switch (value.type)
            {
                case ValueType.intType:
                    {
                        return value.intValue;
                    }
                case ValueType.doubleType:
                    {
                        return value.doubleValue;
                    }
                case ValueType.decimalType:
                    {
                        return (double)value.decimalValue;
                    }
                case ValueType.floatType:
                    {
                        return value.floatValue;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }

        public static implicit operator int(Numeric value)
        {
            switch (value.type)
            {
                case ValueType.intType:
                    {
                        return value.intValue;
                    }
                case ValueType.doubleType:
                    {
                        return (int)value.doubleValue;
                    }
                case ValueType.decimalType:
                    {
                        return (int)value.decimalValue;
                    }
                case ValueType.floatType:
                    {
                        return (int)value.floatValue;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }

        public static implicit operator float(Numeric value)
        {
            switch (value.type)
            {
                case ValueType.intType:
                    {
                        return value.intValue;
                    }
                case ValueType.doubleType:
                    {
                        return (float)value.doubleValue;
                    }
                case ValueType.decimalType:
                    {
                        return (float)value.decimalValue;
                    }
                case ValueType.floatType:
                    {
                        return value.floatValue;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }

        public static implicit operator Numeric(decimal value)
        {
            return new Numeric
            {
                type = ValueType.decimalType,
                decimalValue = value
            };
        }

        public static implicit operator Numeric(int value)
        {
            return new Numeric
            {
                type = ValueType.intType,
                intValue = value
            };
        }

        public static implicit operator Numeric(double value)
        {
            return new Numeric
            {
                type = ValueType.doubleType,
                doubleValue = value
            };
        }

        public static implicit operator Numeric(float value)
        {
            return new Numeric
            {
                type = ValueType.floatType,
                floatValue = value
            };
        }

        public override string ToString()
        {
            switch (this.type)
            {
                case ValueType.intType:
                    {
                        return this.intValue.ToString();
                    }
                case ValueType.doubleType:
                    {
                        return this.doubleValue.ToString();
                    }
                case ValueType.decimalType:
                    {
                        return this.decimalValue.ToString();
                    }
                case ValueType.floatType:
                    {
                        return this.floatValue.ToString();
                    }
                default:
                    {
                        return "0";
                    }
            }
        }
    }
}
