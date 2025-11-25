using System;
using Autofac;

namespace AutofacDemo;

class Program
{
    static void Main(string[] args)
    {
        var builder = new ContainerBuilder();

        builder.RegisterType<ConsoleLogger>().As<ILogger>();

        builder.RegisterType<UserService>();

        var container = builder.Build();

        using (var scope = container.BeginLifetimeScope())
        {
            var userService = scope.Resolve<UserService>();

            userService.CreateUser("JohnDoe");
        }

        Console.ReadKey();
    }
}