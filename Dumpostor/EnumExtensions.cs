using System;

namespace Dumpostor
{
    public static class EnumExtensions
    {
        public static T[] GetValues<T>() where T : Enum
        {
            return (T[]) Enum.GetValues(typeof(T));
        }
    }
}
