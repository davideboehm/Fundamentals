using System;
using System.Collections.Generic;
using System.Linq;

namespace Fundamentals.Functional
{
    public static class MaybeExtensions
    {
        public static Maybe<T> ToMaybe<T>(this T value) where T : class
        {
            return value != null
                ? Maybe.Some(value)
                : Maybe<T>.None;
        }

        public static Maybe<T> ToMaybe<T>(this T? nullable) where T : struct
        {
            return nullable.HasValue
                ? Maybe.Some(nullable.Value)
                : Maybe<T>.None;
        }

        public static Maybe<string> NoneIfEmpty(this string s)
        {
            return string.IsNullOrEmpty(s)
                ? Maybe<string>.None
                : Maybe.Some(s);
        }

        public static Maybe<T> FirstOrNone<T>(this IEnumerable<T> self) where T : class
        {
            return self.FirstOrDefault().ToMaybe();
        }

        public static Maybe<T> FirstOrNone<T>(this IEnumerable<T?> self) where T : struct
        {
            return self.FirstOrDefault().ToMaybe();
        }
    }

    public interface IMaybe<T>
    {
        bool HasValue();
        T Value();
        U Case<U>(Func<T, U> some, Func<U> none);
    }

    public static class Maybe
    {
        public static Maybe<T> Some<T>(T value)
        {
            return Maybe<T>.Some(value);
        }
    }

    public struct Maybe<T> : IMaybe<T>
    {
        readonly T value;

        public static Maybe<T> None => new Maybe<T>();

        public static Maybe<T> Some(T value)
        {
            if (value == null)
            {
                throw new InvalidOperationException();
            }

            return new Maybe<T>(value);
        }

        private Maybe(T value)
        {
            this.value = value;
        }

        public bool HasValue() => value != null;

        public T Value()
        {
            if (!HasValue())
            {
                throw new InvalidOperationException("Maybe does not have a value");
            }

            return value;
        }

        public void Case(Action<T> some, Action none)
        {
            if (this.HasValue())
            {
                some(this.Value());
            }
            else
            {
                none();
            }
        }
        public void Case(Action<T> some)
        {
            if (this.HasValue())
            {
                some(this.Value());
            }
        }

        public U Case<U>(Func<T, U> some, Func<U> none)
        {
            return this.HasValue()
              ? some(this.Value())
              : none();
        }
        public Maybe<U> Case<U>(Func<T, Maybe<U>> some)
        {
            return this.HasValue()
              ? some(this.Value())
              : Maybe<U>.None;
        }

        public Maybe<U> Apply<U>(Func<T, U> function)
        {
            return this.HasValue()
              ? Maybe<U>.Some(function(this.Value()))
              : Maybe<U>.None;
        }

        public override string ToString()
        {
            return this.Case(
                some: (value) => $"Some:({value.ToString()})",
                none: () => "None");
        }
    }   
}
