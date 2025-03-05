using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingletonPattern
{
    internal class SuperHeavyClass
    {
        private static SuperHeavyClass _singleton;
        private static object _locker = new object();
        private SuperHeavyClass()
        {
            Thread.Sleep(500);//Tốn 0.5s
            Console.WriteLine("Object created with hashcode {0}",this.GetHashCode());
        }

        public static SuperHeavyClass GetInstance()
        {
            //Thread safe by locker
            lock (_locker)
            {
                if (_singleton == null)
                {
                    Thread.Sleep(100);
                    _singleton = new SuperHeavyClass();
                }
            }
            return _singleton;
        }

        public void ToString()
        {
            Console.WriteLine("I'm an instance of {0} with hashcode {1}",
                this.GetType().ToString(), this.GetHashCode());
        }

    }
}
