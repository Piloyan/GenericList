using System;

namespace GenericListConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            GenericList<int> GenericList_1 = new();
            GenericList_1.Add(0);
            GenericList_1.Add(6);
            GenericList_1.Add(5);
            GenericList_1.Add(-11);
            GenericList_1.Add(7);
            Console.WriteLine(GenericList_1.Count);
            Console.WriteLine(GenericList_1.Capacity);
            GenericList_1.Remove(3);
            GenericList_1.Insert(4, 22);
            GenericList_1.Reverse();
            GenericList_1.ToArray();
            GenericList_1.Contains(11);

            GenericList<object> GenericList_2 = new() { 1, 2, 0x1, 0b10, 3.5, 10.76, 'z'  };
            GenericList_2.Insert(4, "Hello!");
            foreach (var item in GenericList_2 )
            {
                Console.WriteLine(item);
            }
        }
    }
}