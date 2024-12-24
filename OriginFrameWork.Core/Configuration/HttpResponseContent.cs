namespace OriginFrameWork.Core.Configuration;
public class HttpResponseContent
{
    public bool Status { get; set; }
    public string Code { get; set; }
    public string? Message { get; set; }
    public dynamic? Data { get; set; }

    // public HttpResponseContent Set(bool status, string Code, Object? Data = null, string? msg = null)
    // {
    //     this.Status = status;
    //     this.Code = Code;

    //     if (msg != null)
    //     {
    //         this.Message = msg;
    //     }
    //     if (Data != null)
    //     {
    //         this.Data = Data;
    //     }
    //     return this;
    // }

    public HttpResponseContent IsOk(dynamic? Data = null, string? msg = null)
    {
        this.Status = true;
        this.Code = "200";
        if (msg != null)
        {
            this.Message = msg;
        }
        if (Data != null)
        {
            this.Data = Data;
        }
        return this;
    }

    public HttpResponseContent IsError(dynamic? Data = null, string? msg = null)
    {
        this.Status = false;
        this.Code = "400";
        if (msg != null)
        {
            this.Message = msg;
        }
        if (Data != null)
        {
            this.Data = Data;
        }
        return this;
    }
}
