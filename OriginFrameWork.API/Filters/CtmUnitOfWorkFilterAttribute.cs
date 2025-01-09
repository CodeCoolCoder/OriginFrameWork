//using Microsoft.AspNetCore.Mvc.Filters;


//namespace OriginFrameWork.API.Filters;

//public class CtmUnitOfWorkFilterAttribute : Attribute, IAsyncActionFilter
//{
//    public CtmUnitOfWorkFilterAttribute(bool IsTurnOn)
//    {
//        this.IsTurnOn = IsTurnOn;
//    }

//    public bool IsTurnOn { get; }

//    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
//    {
//        //行为过滤器中添加是否开启事务
//        if (IsTurnOn)
//        {
//            var workUnit = context.HttpContext.RequestServices.GetRequiredService<IOriginUnitOfWork<OriginFrameWorkDbContext>>();
//            workUnit.BeginTransaction();
//            var res = await next();
//            //执行过程中没有异常，提交事务
//            if (res.Exception == null || res.ExceptionHandled)
//            {
//                workUnit.Commit();
//            }
//        }
//        else
//        {
//            await next();
//        }     
//    }

//}
