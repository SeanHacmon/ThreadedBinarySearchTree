using System;
using System.Threading;

namespace ThreadedBinarySearchTree
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadedBinarySearchTree t = new ThreadedBinarySearchTree();
            for (int i = 0; i <= 10; i++)
            {
                t.add(i);
            }
            Thread t1 = new Thread(() => t.remove(6));
            Thread t2 = new Thread( t.print);
            Thread t3 = new Thread(() => t.search(14));
            Thread t4 = new Thread(() => t.search(5));
            Thread t5 = new Thread(() => t.clear());
            Thread t6 = new Thread(() => t.print());
            t1.Start();
            t1.Join();
            t2.Start();
            t2.Join();
            t3.Start();
            t3.Join();
            t4.Start();
            t4.Join();
            t6.Start();
            t6.Join();
            t5.Start();
            t5.Join();
            Console.WriteLine();
        }
    }
}
