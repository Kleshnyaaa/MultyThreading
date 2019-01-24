using System;

namespace FindMinimal
{
    public static class ArrayGenerator
    {
        public static int[] GetArrayOfIntNumbers(int size)
        {
            var random = new Random(DateTime.Now.Millisecond);

            var resultArray = new int[size];

            for (int i = 0; i < size; i++)
            {
                resultArray[i] = random.Next(-10000, 10000);
            }

            return resultArray;
        }
    }
}
