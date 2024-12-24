namespace OriginFrameWork.Core.Configuration;

public class JWTTokenModel
{
    /// <summary>
    /// 发行人
    /// </summary>
    /// <value></value>
    public string Issuer { get; set; }
    /// <summary>
    /// 用户
    /// </summary>
    /// <value></value>
    public string Audience { get; set; }
    /// <summary>
    /// 过期时间
    /// </summary>
    /// <value></value>
    public int Expires { get; set; }

    public string Security { get; set; }

    public int Id { get; set; }
    public string UserNo { get; set; }
    public string UserName { get; set; }
}
