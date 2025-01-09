namespace OriginFrameWork.EntityFrameWorkCoreModule;
public class PageWithSortDto
{
    public bool IsSort { get; set; } = false;
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 30;
    public string Sort { get; set; } = "Id";
    public OrderType OrderType { get; set; } = OrderType.Asc;

}
public enum OrderType
{
    Asc,
    Desc
}
