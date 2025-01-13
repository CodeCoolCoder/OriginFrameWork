using OriginFrameWork.CoreModule.OriginInterface;

namespace OriginFrameWork.EntityFrameWork;

public partial class MainDevice : BaseModel
{
    public int Id { get; set; }

    public string? MaterialNo { get; set; }

    public string? MaterialName { get; set; }

    public string? Brand { get; set; }

    public string? SpecsType { get; set; }

    public int? StockNumber { get; set; }

    public string? Unit { get; set; }

    public string? StockNo { get; set; }

    public string? StockName { get; set; }

    public string? SlotNo { get; set; }

    public int? DeviceStatus { get; set; }

    public string? InStockTime { get; set; }

    public string? InStockUser { get; set; }

    public string? DeviceType { get; set; }

    public string? Memo { get; set; }

    public double? DevicePrice { get; set; }

    public string? DeviceSource { get; set; }

    public string? DeviceStatusName { get; set; }

    public string? SourceId { get; set; }

    public int? DeptrootId { get; set; }

    public int? TeamId { get; set; }

    public string? CanItBeTransferred { get; set; }

    public string? InventoryClassification { get; set; }

    public string? IsItIdleAndBacklogged { get; set; }

    public string? MaterialCategory { get; set; }

    public string? MaterialParameters { get; set; }

    public double? TotalAmount { get; set; }

    public string? IsAccidentParts { get; set; }

    public int? OldOrNew { get; set; }

    public string? TechnicalParameters { get; set; }
}
