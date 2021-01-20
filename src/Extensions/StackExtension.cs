using System.Collections.Generic;

namespace CeloIsYou.Extensions
{
    public static class StackExtension
    {
        public static void PushAll<T>(this Stack<T> stack, IEnumerable<T> objects)
        {
            foreach (var @object in objects)
                stack.Push(@object);
        }
    }
}
