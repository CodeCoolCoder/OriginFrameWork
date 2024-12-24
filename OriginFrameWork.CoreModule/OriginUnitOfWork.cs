namespace OriginFrameWork.CoreModule;

public class OriginUnitOfWorkAttribute : Attribute
{
    public OriginUnitOfWorkAttribute(bool IsTurnOn)
    {
        this.IsTurnOn = IsTurnOn;
    }

    public bool IsTurnOn { set; get; }

}
