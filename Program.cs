using System.Linq;
using System;
using System.Collections.Generic;

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

            var dict = (IDictionary<string, object>)getMethod.Invoke(homeController, Array.Empty<object>());
            Console.WriteLine(dict["Name"]);



        }
    }
}
