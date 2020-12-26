using System.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ReflectionDelegateDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var homeController = new HomeController();
            var homeControllerType = homeController.GetType();

            var property = homeControllerType.GetProperties().FirstOrDefault(pr => pr.IsDefined(typeof(DataAttribute), true));

            var getMethod = property.GetMethod;
            var stopWatch = Stopwatch.StartNew();       

            for (int i = 0; i < 100000000; i++)
            {
                var dict = (IDictionary<string, object>)getMethod.Invoke(homeController, Array.Empty<object>());
            }
            Console.WriteLine(stopWatch.Elapsed);
            //   Console.WriteLine(dict["Name"]);

            // Func<HomeController,IDictionary<string, object>> func = (controller) => controller.Data;

            var deleg = (Func<HomeController, IDictionary<string, object>>)getMethod.CreateDelegate(typeof(Func<HomeController, IDictionary<string, object>>));    
            stopWatch = Stopwatch.StartNew();
            for (int i = 0; i < 100000000; i++)
            {
                var dict = deleg(homeController);
            }

             Console.WriteLine(stopWatch.Elapsed);
        }
    }
}
