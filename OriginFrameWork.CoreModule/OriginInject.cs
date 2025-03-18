namespace OriginFrameWork.CoreModule
{
    /// <summary>
    /// 额外模块需加上此特性
    /// </summary>
    public class OriginInject : Attribute
    {
        public OriginInject(params Type[]? ModuleType)
        {
            this.ModuleType = ModuleType;
        }
        public Type[] ModuleType { get; }
    }
}
