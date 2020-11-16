using System;
using System.Collections.Generic;
using System.Text; 
using Akka.Actor; 
namespace Zyzzyva.src.Main
{
    class Actor : ReceiveActor
    { 
        private int fibonacci(int x){
            static int fibHelper(int xx, int prev = 0, int next = 1) => xx switch
            {
                0 => prev,
                1 => next,
                _ => fibHelper(xx - 1, next, next + prev)
            };

            return fibHelper(x);
        }
        Func<int, int> multiplyByFive = x =>
        {
            static int fibHelper(int xx, int prev = 0, int next = 1) => xx switch
            {
                0 => prev,
                1 => next,
                _ => fibHelper(xx - 1, next, next + prev)
            };

            return fibHelper(x);
        };
    }
}