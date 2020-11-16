using System;
using System.Collections.Generic;
using System.Text;

namespace Zyzzyva.src.Main.Option.Core
{
    public interface IOption<T>
    {
        bool IsEmpty<A>(IOption<A> opt);
        B getOrElse<A, B>(IOption<A> opt, B orElse) where B : A;
        IOption<B> flatMap<A, B>(Func<A,IOption<B>> f);
    }
    public class Some<T> : Impl<T>
    { 
        public T _a { get; }
        public Some(T a) => _a = a;
    }
    public class None<T> : Impl<T>
    { }
    public abstract class Impl<T> : IOption<T>
    {
        public IOption<B> flatMap<A, B>(Func<A, IOption<B>> f) => this switch
        {
            new Some(a)=>f(a),
            _ => new None();
        }; 

        public B getOrElse<A, B>(IOption<A> opt, B orElse) where B : A
        {
            throw new NotImplementedException();
        }

        public bool IsEmpty<A>(IOption<A> opt)=> opt switch
        {
            new None() => true,
            _ => false
        };
    }
}
     
