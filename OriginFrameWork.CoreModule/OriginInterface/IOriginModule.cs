namespace OriginFrameWork.CoreModule.OriginInterface
{
    public interface IOriginModule
    {
        void ConfigureServices(OriginServiceConfigurationContext context);
        void ApplicationInitialization(OriginApplicationInitializationContext context);
    }
}
