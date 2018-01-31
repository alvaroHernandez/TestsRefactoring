using System;

namespace TestsRefactoring.Tests.Extensions
{
    public static class ArrayExtension
    {
        public static T[] Shuffle<T>(this T[] list)
        {
            var r = new Random((int)DateTime.Now.Ticks);
            for (int i = list.Length - 1; i > 0; i--)
            {
                int j = r.Next(0, i - 1);
                var e = list[i];
                list[i] = list[j];
                list[j] = e;
            }
            return list;
        }
    }
}