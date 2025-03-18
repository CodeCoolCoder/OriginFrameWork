namespace OriginFrameWork.API
{
    public static class useoption
    {
        public static void useoption1(this IServiceCollection services, Action<testoption> action)
        {
            var ss = new testoption();
            action.Invoke(ss);
        }
        public static void tconfigure<T>(this IServiceCollection services, Action<T> action)
        {
            action.Invoke((T)Activator.CreateInstance(typeof(T)));
        }
    }
}
