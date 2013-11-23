using System;
using System.Diagnostics;
namespace UserApp.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            dynamic api = new ClientProxy("51ded0be98035");

            var result = api.user.login(
                login: "test",
                password: "test123"
            );

            var userResult = api.User.Get();

            api.User.Logout();
        }
    }
}
