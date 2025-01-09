using AutoMapper;

namespace OriginFrameWork.Ability;

public class OriginSystemProfile : Profile
{
    public OriginSystemProfile()
    {
        //开始映射，因为productdto是四个类的集合，所以从四个类本身向dto中映射
        //ReverseMap();表示相互映射是双向的，去除的话就是单向的，只能从product到productdto,写了就是从productdto到product

        //为何要加ForMember，因为在你分别获取到四个类的实体的值的时候，开始通过automapper向dto中转换的时候，
        //id不同，dto只有一个id，但是四个类可能有四个不同的id，所以在转换的时候忽略掉id,只保留一个即可
        // CreateMap<ProductSale, ProductDto>().ForMember(m => m.Id, opt => opt.Ignore()).ReverseMap();
        // CreateMap<ProductPhoto, ProductDto>().ForMember(m => m.Id, opt => opt.Ignore()).ReverseMap();
        // CreateMap<ProductSaleAreaDiff, ProductDto>().ForMember(m => m.Id, opt => opt.Ignore()).ReverseMap();
        //CreateMap<MainDevice, InStockDeviceDto>().ReverseMap();
        // CreateMap<MoveStockRecord, MoveStockRecordDto>().ReverseMap();

    }
}
