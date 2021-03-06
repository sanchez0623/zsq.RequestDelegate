﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace zsq.RequestDelegate
{
    //参考链接
    //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-2.1
    //源码地址
    //https://github.com/aspnet/HttpAbstractions
    //https://github.com/aspnet/HttpAbstractions/blob/master/src/Microsoft.AspNetCore.Http/Internal/ApplicationBuilder.cs
    //https://github.com/aspnet/HttpAbstractions/blob/master/src/Microsoft.AspNetCore.Http.Abstractions/Extensions/UseExtensions.cs
    //参考dotnet core RequestDelegate源码 实现的RequestDelegate demo
    class Program
    {
        public static List<Func<RequestDelegate, RequestDelegate>> list = new List<Func<RequestDelegate, RequestDelegate>>();

        static void Main(string[] args)
        {
            Use(next =>
            {
                return context =>
                {
                    Console.WriteLine("1");
                    return next.Invoke(context);
                    //如果这里不调用next.Invoke()，则整个请求管道停止
                };
            });

            Use(next =>
            {
                return context =>
                {
                    Console.WriteLine("2");
                    return next.Invoke(context);
                };
            });

            RequestDelegate end = (context) =>
            {
                Console.WriteLine("end");
                return Task.CompletedTask;
            };

            list.Reverse();
            foreach (var middleware in list)
            {
                end = middleware.Invoke(end);
            }

            end.Invoke(new Context());

            Console.ReadLine();
        }

        public static void Use(Func<RequestDelegate, RequestDelegate> middleware)
        {
            list.Add(middleware);
        }
    }
}
