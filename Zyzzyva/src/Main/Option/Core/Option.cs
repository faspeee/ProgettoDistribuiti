using System;
using System.Collections.Generic;
using System.Text;

namespace Zyzzyva.src.Main.Option.Core
{
    public interface IOption<T>
    {
        bool IsEmpty<A>(IOption<A> opt);
        B GetOrElse<A, B>(IOption<A> opt, B orElse) where B : A;
        IOption<B> flatMap<A, B>(Func<A,IOption<B>> f);
    }
    public class Some<T> where T: notnull
    { 
        public readonly T value; 
        public Some(T value) => this.value = value;
         
    }
    public sealed class None<T> : Option<T>
    {
    } 
    public sealed class None
    {
        public static None Value { get; } = new None();

        private None() { }
    }

    public class Option<T> : IOption<T>
    {
        public IOption<B> flatMap<A, B>(Func<A, IOption<B>> f)
        {
            throw new NotImplementedException();
        }

        public B GetOrElse<A, B>(IOption<A> opt, B orElse) where B : A
        {
            throw new NotImplementedException();
        }

        public bool IsEmpty<A>(IOption<A> opt)
        {
            throw new NotImplementedException();
        }
    }
}
     
