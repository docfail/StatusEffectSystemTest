using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModularExpressionTest
{
    class ModularExpression<T>
    {
        // Internal Components and Methods

        private SortedSet<Module<T>> modules = new SortedSet<Module<T>>(new ByPriority());
        public T Base { get; set; }
        private T lastCalculatedValue;
        private bool cacheValue;
        //private uint idCounter = 0;
        private T Compile()
        {
            T value = Base;
            foreach (Module<T> module in modules)
            {
                value = module.func(value);
            }
            lastCalculatedValue = value;
            return lastCalculatedValue;
        }
        
        // Basic Usage
        
        public T Value
        {
            get
            {
                if (cacheValue)
                {
                    return lastCalculatedValue;
                }
                else
                {
                    return Compile();
                }
            }
        }

        public ModularExpression(T baseValue, bool cacheValue = true)
        {
            this.Base = baseValue;
            this.cacheValue = cacheValue;
            lastCalculatedValue = this.Base;
        }

        // Public Mutator Methods

        public Guid AddModule(int priority, Func<T,T> expression)
        {
            Module<T> module = new Module<T>(priority,Guid.NewGuid(),expression);
            //Module<T> module = new Module<T>(priority,idCounter++,expression);
            modules.Add(module);
            Compile();
            return module.uid;
        }

        public void RemoveModule(Guid uid)
        {
            modules.RemoveWhere((mod)=>(uid==mod.uid));
            Compile();
        }

        // DEBUG/TESTING

        public void PrintCollection()
        {
            Console.WriteLine("State of Internal Collection:");
            if (modules.Count == 0) Console.WriteLine("<EMPTY>");
            foreach (var item in modules)
            {
                Console.WriteLine(item);
            }
        }

        // Conversion Methods

        public override string ToString()
        {
            return Value.ToString();
        }
        public static implicit operator string(ModularExpression<T> me) => me.ToString(); // Not sure if its a good idea to allow implicit conversion to string, might want to change this back to explicit...
        public static explicit operator int(ModularExpression<T> me) => Convert.ToInt32(me.Value);
        public static explicit operator float(ModularExpression<T> me) => (float)Convert.ToDouble(me.Value);
        public static explicit operator double(ModularExpression<T> me) => Convert.ToDouble(me.Value);
        public static explicit operator bool(ModularExpression<T> me) => Convert.ToBoolean(me.Value);

        // Ancillary DataTypes

        private class ByPriority : IComparer<Module<T>>
        {
            public int Compare(Module<T> x, Module<T> y)
            {
                int o = x.priority.CompareTo(y.priority);
                if (o == 0)
                    return x.uid.CompareTo(y.uid);
                else
                    return o;
            }
        }

        private readonly struct Module<X>
        {
            public readonly int priority;
            public readonly Guid uid;
            //public readonly uint uid;
            public readonly Func<X, X> func;

            public Module(int priority, Guid uid, Func<X, X> func)
            {
                this.priority = priority;
                this.uid = uid;
                this.func = func;
            }
            override public string ToString()
            {
                return $"<PRIORITY:{priority}|ID:{uid}|ExpressionHandle:{func}>";
            }
        }
    }
}
