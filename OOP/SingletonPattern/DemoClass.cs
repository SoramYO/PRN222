using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingletonPattern
{
    public class DemoClass
    {
        public static void Run()
        {
            List<SuperHeavyClass> list = new List<SuperHeavyClass>();
            for (int i = 0; i < 10; i++)
            {
                SuperHeavyClass superHeavy = SuperHeavyClass.GetInstance();
                list.Add(superHeavy);

            }

            foreach (SuperHeavyClass heavyClass in list)
            {
                heavyClass.ToString();
            }

        }
    }
}
