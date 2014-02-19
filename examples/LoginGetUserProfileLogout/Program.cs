using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginGetUserProfileLogout
{
    class Program
    {
        static void Main(string[] args)
        {
            dynamic api = new UserApp.ClientProxy("YOUR APP ID");

            var result = api.User.Login(
                login: "LOGIN OF USER",
                password: "PASSWORD OF USER"
            );

            var user = api.User.Get()[0];

            Console.WriteLine("FirstName={0}, LastName={1}", user.FirstName, user.LastName);
            Console.WriteLine("Login={0}", user.Login);
            Console.WriteLine("Email={0}", user.Email);

            api.User.Logout();

            Console.ReadLine();
        }
    }
}
