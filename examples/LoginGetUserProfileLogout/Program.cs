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
            dynamic api = new UserApp.API("51ded0be98035", new {BaseAddress="dev-api.userapp.io", ThrowErrors = true});

            var result = api.User.Login(
                login: "root",
                password: "test"
            );

            var user = api.User.Get();

            /*Console.WriteLine("FirstName={0}, LastName={1}", user.FirstName, user.LastName);
            Console.WriteLine("Login={0}", user.Login);
            Console.WriteLine("Email={0}", user.Email);

            api.User.Logout();*/

            var dss = user;

            Console.ReadLine();
        }
    }
}
