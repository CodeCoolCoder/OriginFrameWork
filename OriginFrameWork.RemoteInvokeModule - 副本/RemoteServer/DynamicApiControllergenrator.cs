using Castle.Core.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OriginFrameWork.RemoteInvokeModule.RemoteAttributes;
using System.Reflection;
using System.Reflection.Emit;

namespace OriginFrameWork.RemoteInvokeModule.RemoteServer
{
    public class DynamicApiControllergenrator
    {
        private readonly IServiceCollection _services;
        /// <summary>
        /// 控制器默认前缀
        /// </summary>
        private readonly string Prefix = "api";
        public DynamicApiControllergenrator(IServiceCollection services, string prefix)
        {
            services.BuildServiceProvider();
            _services = services;
            Prefix = prefix;
        }
        //where TService : class
        //   where TImplementation : class, TService <TService, TImplementation>
        // , Type TImplementation
        /// <summary>
        /// 控制器注册
        /// </summary>
        /// <param name="TService"></param>
        public void RegisterService(Type TService)
        {
            var IserviceType = TService;
            // 创建动态控制器
            var controllerType = CreateControllerType(IserviceType);
            // 注册控制器
            _services.AddMvc()
                .AddApplicationPart(controllerType.Assembly)
                .AddControllersAsServices();
        }
        /// <summary>
        /// 创建动态控制器
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        private Type CreateControllerType(Type serviceType)
        {
            //前面的类名称
            var serviceName = serviceType.GetAttribute<RemoteServiceAttribute>().RemoteServiceGroup;

            var assemblyName = new AssemblyName($"DynamicApi_{serviceType.Name}");
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);

            var controllerTypeName = $"{serviceName}Controller";
            var typeBuilder = moduleBuilder.DefineType(controllerTypeName,
                TypeAttributes.Public | TypeAttributes.Class,
                typeof(ControllerBase));

            // 添加 API Controller 特性
            var apiControllerAttr = typeof(ApiControllerAttribute);
            var apiControllerCtor = apiControllerAttr.GetConstructor(Type.EmptyTypes);
            typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(apiControllerCtor, Array.Empty<object>()));

            // 添加 Route 特性
            var routeAttr = typeof(RouteAttribute);
            var routeCtor = routeAttr.GetConstructor(new[] { typeof(string) });
            typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(
                routeCtor,
                new object[] { $"{Prefix}/{serviceName}" }));

            // 添加服务字段
            var serviceField = typeBuilder.DefineField("_service", serviceType,
                FieldAttributes.Private | FieldAttributes.InitOnly);

            // 添加构造函数
            AddConstructor(typeBuilder, serviceField, serviceType);

            // 为每个远程服务方法创建 Action
            foreach (var method in serviceType.GetMethods())
            {
                var remoteClassAttr = method.DeclaringType.GetCustomAttribute<RemoteServiceAttribute>();
                var remoteMethodAttr = method.GetCustomAttribute<RemoteServiceIndividualAttribute>();
                if (remoteClassAttr != null)
                {
                    if (remoteMethodAttr != null)
                    {
                        CreateAction(typeBuilder, method, serviceField, null, remoteMethodAttr, method.Name);
                    }
                    else
                    {
                        CreateAction(typeBuilder, method, serviceField, remoteClassAttr, null, method.Name);
                    }

                }
            }

            return typeBuilder.CreateType();
        }
        /// <summary>
        /// 添加构造函数
        /// </summary>
        /// <param name="typeBuilder"></param>
        /// <param name="serviceField"></param>
        /// <param name="serviceType"></param>
        /// <exception cref="InvalidOperationException"></exception>
        private void AddConstructor(TypeBuilder typeBuilder, FieldBuilder serviceField, Type serviceType)
        {
            var ctor = typeBuilder.DefineConstructor(
                MethodAttributes.Public,
                CallingConventions.Standard,
                new[] { serviceType });

            var il = ctor.GetILGenerator();
            var baseCtor = typeof(ControllerBase).GetConstructor(
             BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                null,
            Type.EmptyTypes,
            null);

            if (baseCtor == null)
            {
                throw new InvalidOperationException("Cannot find ControllerBase constructor");
            }

            // 调用基类构造函数
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, baseCtor);

            // 设置服务字段
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, serviceField);
            il.Emit(OpCodes.Ret);
        }
        /// <summary>
        /// 添加action
        /// </summary>
        /// <param name="typeBuilder"></param>
        /// <param name="method"></param>
        /// <param name="serviceField"></param>
        /// <param name="attrclass"></param>
        /// <param name="attrmethod"></param>
        /// <param name="routeName"></param>
        private void CreateAction(TypeBuilder typeBuilder, MethodInfo method, FieldBuilder serviceField,
            RemoteServiceAttribute? attrclass, RemoteServiceIndividualAttribute? attrmethod, string routeName)
        {

            string httpMethod = "";
            if (attrclass == null)
            {
                httpMethod = attrmethod.HttpMethodByMethod;

            }
            else
            {
                httpMethod = attrclass.remoteHttpMethod;
            }
            var parameters = method.GetParameters();

            // 定义方法
            var methodBuilder = typeBuilder.DefineMethod(
                method.Name,
                MethodAttributes.Public | MethodAttributes.Virtual,
                method.ReturnType,
                parameters.Select(p => p.ParameterType).ToArray());

            // 添加 HTTP 方法特性
            var httpMethodAttr = GetHttpMethodAttribute(httpMethod);
            var httpMethodCtor = httpMethodAttr.GetConstructor(Type.EmptyTypes);
            methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(
                httpMethodCtor,
                Array.Empty<object>()));

            // 为每个参数添加绑定特性
            for (int i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                var paramBuilder = methodBuilder.DefineParameter(i + 1, parameter.Attributes, parameter.Name);

                // 根据 HTTP 方法和参数类型决定使用哪种绑定特性
                if (httpMethod?.ToUpper() == "GET")
                {
                    // GET 请求所有参数都使用 FromQuery
                    AddFromQueryAttribute(paramBuilder);
                }
                else
                {
                    // POST/PUT 请求，根据参数类型决定
                    if (IsSimpleType(parameter.ParameterType))
                    {
                        // 简单类型使用 FromQuery
                        AddFromQueryAttribute(paramBuilder);
                    }
                    else
                    {
                        // 复杂类型使用 FromBody
                        AddFromBodyAttribute(paramBuilder);
                    }
                }

                // 添加参数名称绑定
                if (!string.IsNullOrEmpty(parameter.Name))
                {
                    var fromRouteAttr = typeof(FromRouteAttribute);
                    var fromRouteCtor = fromRouteAttr.GetConstructor(Type.EmptyTypes);
                    var fromRouteBuilder = new CustomAttributeBuilder(fromRouteCtor, Array.Empty<object>());
                    paramBuilder.SetCustomAttribute(fromRouteBuilder);
                }
            }

            // 添加路由特性
            if (!string.IsNullOrEmpty(routeName))
            {
                var routeAttr = typeof(RouteAttribute);
                var routeCtor = routeAttr.GetConstructor(new[] { typeof(string) });
                methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(
                    routeCtor,
                    new object[] { routeName }));
            }

            // 添加 ProducesResponseType 特性
            AddProducesResponseTypeAttribute(methodBuilder, method.ReturnType);

            // 生成方法体
            var il = methodBuilder.GetILGenerator();

            // 加载服务实例
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, serviceField);

            // 加载参数
            for (int i = 0; i < parameters.Length; i++)
            {
                il.Emit(OpCodes.Ldarg, i + 1);
            }

            // 调用服务方法
            il.Emit(OpCodes.Callvirt, method);
            il.Emit(OpCodes.Ret);
        }
        private bool IsSimpleType(Type type)
        {
            return type.IsPrimitive
                || type == typeof(string)
                || type == typeof(decimal)
                || type == typeof(DateTime)
                || type == typeof(DateTimeOffset)
                || type == typeof(TimeSpan)
                || type == typeof(Guid)
                || type.IsEnum;
        }
        private void AddFromQueryAttribute(ParameterBuilder paramBuilder)
        {
            var fromQueryAttr = typeof(FromQueryAttribute);
            var fromQueryCtor = fromQueryAttr.GetConstructor(Type.EmptyTypes);
            var fromQueryBuilder = new CustomAttributeBuilder(fromQueryCtor, Array.Empty<object>());
            paramBuilder.SetCustomAttribute(fromQueryBuilder);
        }
        private void AddFromBodyAttribute(ParameterBuilder paramBuilder)
        {
            var fromBodyAttr = typeof(FromBodyAttribute);
            var fromBodyCtor = fromBodyAttr.GetConstructor(Type.EmptyTypes);
            var fromBodyBuilder = new CustomAttributeBuilder(fromBodyCtor, Array.Empty<object>());
            paramBuilder.SetCustomAttribute(fromBodyBuilder);
        }
        private void AddProducesResponseTypeAttribute(MethodBuilder methodBuilder, Type returnType)
        {
            // 处理 Task<T>
            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                returnType = returnType.GetGenericArguments()[0];
            }

            var producesAttr = typeof(ProducesResponseTypeAttribute);
            var producesCtor = producesAttr.GetConstructor(new[] { typeof(Type), typeof(int) });
            var producesBuilder = new CustomAttributeBuilder(
                producesCtor,
                new object[] { returnType, 200 });
            methodBuilder.SetCustomAttribute(producesBuilder);
        }
        private Type GetHttpMethodAttribute(string httpMethod)
        {
            return (httpMethod?.ToUpper()) switch
            {
                "GET" => typeof(HttpGetAttribute),
                "POST" => typeof(HttpPostAttribute),
                "PUT" => typeof(HttpPutAttribute),
                "DELETE" => typeof(HttpDeleteAttribute),
                "PATCH" => typeof(HttpPatchAttribute),
                _ => typeof(HttpPostAttribute) // 默认使用 POST
            };
        }
    }
}
