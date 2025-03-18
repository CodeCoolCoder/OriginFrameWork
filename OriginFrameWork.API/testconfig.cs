namespace OriginFrameWork.API
{
    public static class testconfig
    {
        public static void config<T>(this IServiceCollection services, Action<T> action)
        {
            var instance = (T)Activator.CreateInstance(typeof(T));
            action.Invoke(instance);
            var a = instance;
        }
    }
}
