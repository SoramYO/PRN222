// See https://aka.ms/new-console-template for more information
using SingletonPattern;
using System.Runtime.CompilerServices;

Thread t1 = new Thread(DemoClass.Run);
Thread t2 = new Thread(DemoClass.Run);

t1.Start();
t2.Start();

Console.WriteLine("Finish!");
