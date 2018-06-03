using System;

namespace Serializer
{
    class Program
    {
        static void Main(string[] args)
        {
            BinarySerializer serializer = new BinarySerializer();

            DisplayInfo(serializer, 0);
            DisplayInfo(serializer, true);
            DisplayInfo(serializer, 0F);
            DisplayInfo(serializer, 0D);
            DisplayInfo(serializer, 0M);
            DisplayInfo(serializer, "string");

            Console.ReadKey();
        }

        private static void DisplayInfo<T>(BinarySerializer p_serializer, T p_object)
        {
            byte[] raw = p_serializer.Serialize(p_object);

            Console.Write("Bytes:");
            Console.WriteLine(raw.Length);

            T result = p_serializer.Deserialize<T>(raw);
            Console.Write("Value:");
            Console.WriteLine(result);

            Console.Write("Type:");
            Console.WriteLine(typeof(T).FullName);
            Console.WriteLine();
        }
    }
}