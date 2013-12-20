# .NET library for UserApp

## Getting started

### Finding your App Id and Token

If you don't have a UserApp account, you need to [create one](https://app.userapp.io/#/sign-up/).

* **App Id**: The App Id identifies your app. After you have logged in, you should see your `App Id` instantly. If you're having trouble finding it, [follow this guide](https://help.userapp.io/customer/portal/articles/1322336-how-do-i-find-my-app-id-).

*  **Token**: A token authenticates a user on your app. If you want to create a token for your logged in user, [follow this guide](https://help.userapp.io/customer/portal/articles/1364103-how-do-i-create-an-api-token-). If you want to authenticate using a username/password, you can acquire your token by calling `api.User.Login(...);`

### Referencing the library

To reference the library you need to add [UserApp](https://www.nuget.org/packages/UserApp/) using NuGet. If you're unfamiliar with adding NuGet references, read more about it here: [http://docs.nuget.org/docs/start-here/Managing-NuGet-Packages-Using-The-Dialog](http://docs.nuget.org/docs/start-here/Managing-NuGet-Packages-Using-The-Dialog)

### Creating your first client

	dynamic api = new UserApp.API("YOUR APP ID");

#### Additional ways of creating a client

If you want to create a client with additional options  the easiest way is to pass an *anonymous object* with the options as shown below.

    dynamic api = new UserApp.API(new {
        Debug = true,
        ThrowErrors = false
    });

If you pass a string value into the constructor the first argument will be treated as the `App Id`, the second as the `Token`. If you pass an *anonymous object* then it will always be treated as additional options. I.e. some valid constructs are:

	dynamic api = new UserApp.API("MY APP ID");
#
	dynamic api = new UserApp.API("MY APP ID", new {
        Option = "some value"
    });
#
	dynamic api = new UserApp.API("MY APP ID", "MY TOKEN", new {
        Option = "some value"
    });

## Calling services and methods

This client has no hard-coded API definitions built into it. It merly acts as a proxy which means that you'll never have to update the client once new API methods are released. If you want to call a service/method all you have to do is look at the [API documentation](https://app.userapp.io/#/docs/) and follow the convention below:

    dynamic result = api.[Service].[Method](argument: value, otherArgument: "otherValue");

#### Some examples

The API [`user.login`](https://app.userapp.io/#/docs/user/#login) and it's arguments `login` and `password` translates to:

    var loginResult = api.User.Login(
        login: "test",
        password: test"
    );

The API [`user.invoice.search`](https://app.userapp.io/#/docs/invoice/#search) and it's argument `user_id` translates to:

    var invoices = api.User.Invoice.Search(
        userId: "test123"
    );

The API [`property.save`](https://app.userapp.io/#/docs/property/#save) and it's arguments `name`, `type` and `default_value` translates to:

    var property = api.Property.Save(
        name: "my new property",
        type: "boolean",
        defaultValue: true
    );

The API [`user.logout`](https://app.userapp.io/#/docs/user/#logout) without any arguments translates to:

    api.User.Logout();

## Configuration

Options determine the configuration of a client. If no options are passed to a client, the options will automatically be inherited from the client global options (`UserApp.ClientOptions.GetGlobal()`).

### Available options

* **Version** (`Version`): Version of the API to call against. Default `1`.
* **App Id** (`AppId`): App to authenticate against. Default `null`.
* **Token** (`Token`): Token to authenticate with. Default `null`.
* **Debug mode** (`Debug`): Log steps performed when sending/recieving data from UserApp. Default: `false`.
* **Secure mode** (`Secure`): Call the API using HTTPS. Default: `true`.
* **Base address** (`BaseAddress`): The address to call against. Default: `api.userapp.io`.
* **Throw errors** (`ThrowErrors`): Whether or not to throw an exception when response is an error. I.e. result `{"error_code":"SOME_ERROR","message":"Some message"}` results in an exception of type `UserApp.Exceptions.ServiceException`.

### Setting options

Options can either be set in global or local scope. Global is the scope in which all clients inherit their default options from.

#### Global scope

Global options can be set using 

    UserApp.API.SetGlobalOptions(new {
        AppId = "MY APP ID",
        Token = "MY TOKEN"
    });

### Local scope

The easiest way to set a local scoped option is to do it in the constructor when creating a new client.

    dynamic api = new UserApp.API(new {
        Debug = true,
        ThrowErrors = false
    });

If you want to set an option after the client has been created you can do it as shown below.

    api.GetOptions().Debug = true;

Setting multiple options is done almost the same way.

    api.SetOptions(new {
        Debug = true,
        ThrowErrors = false
    });

## Example code

A more detailed set of examples can be found in /examples.

### Example code (sign up a new user)

    dynamic api = new UserApp.API("YOUR APP-ID");

    var newUser = api.User.Save(
       login: "johndoe81",
       password: "iwasfirst!111"
    );

### Example code (logging in and updating a user)

    dynamic api = new UserApp.API("YOUR APP-ID");

    api.User.Login(
       login: "johndoe81",
       password: "iwasfirst!111"
    );

    api.User.Save(
       userId: "self",
       firstName: "John",
       lastName: "Doe"
    );

	api.User.Logout();

### Example code (finding a specific user)

    dynamic api = new UserApp.API("YOUR APP-ID", "YOUR TOKEN");

    var searchResult = api.User.Search(
       filters: new {
           query: "*bob*"
       },
       sort: new {
           createdAt: "desc"
       }
    );

    var items = searchResult.items;

## Versioning

If you want to configure the client to call a specific API version you can do it by either setting the `version` option, or by calling the client using the convention `api.V[version number]`. If no version is set it will automatically default to `1`.

### Examples

Since no version has been specified, this call will be made against version `1` (default).

    api.User.Login(login: "test", password: "test");

Since the version has been explicitly specified using options, the call will be made against version `2`.

	dynamic api = new UserApp.API(new { Version = 2 });
    api.User.Login(login: "test", password: "test");

Since the version has been explicitly specified, the call will be made against version `3`.

    api.V3.User.Login(login: "test", password: "test");

## Error handling

### Debugging

Sometimes to debug an API error it's important to see what is being sent/recieved from the calls that one make to understand the underlying reason. If you're interested in seeing these logs, you can set the client option `Debug` to `true`, then hook into the log event stream and output it to the console as shown below.

	var options = api.GetOptions() as ClientOptions;
	options.Logger.OnLogAdded += (log) => {
	    Console.WriteLine(log);
	};

### Catching errors

When the option `throw_errors` is set to `true` (default) the client will automatically throw a `\UserApp\Exceptions\ServiceException` exception when a call results in an error. I.e.

	try
    {
		api.User.Save(userId: "invalid user id");
	}
    catch(UserApp.Exceptions.ServiceException exception)
    {
		switch(exception.GetErrorCode()){
            // Handle specific error
            case "INVALID_ARGUMENT_USER_ID":
				throw new Exception("User does not exist");
			default:
				throw;
        }
	}

Setting `ThrowErrors` to `false` is more of a way to tell the client to be silent. This will not throw any service specific exceptions. Though, it might throw a `UserApp.Exceptions.UserAppException`.

	var result = api.User.Save(userId: "invalid user id");

	if((result as IDictionary<string, Object>) != null && result.ContainsKey("error_code")){
		if(result.error_code == "INVALID_ARGUMENT_USER_ID"){
            // Handle sepcific error
        }
    }

## Solving issues

### See what is being sent to/from UserApp

1. Set the client option `Debug` to `true` (see section *options* for more details on setting client options). If no logger is set, this automatically adds a MemoryLogger to your API client. The logger is retrievable using `api.GetOptions().Logger`.
2. Like above, set the option `ThrowErrors` to `false`. This disables any error exceptions (`UserApp.Exceptions.ServiceException`) being thrown.
3. Make the API calls that you want to debug. E.g. `api.User.Login(login: "test");`
4. Print the logs! See the section `Debugging`.
5. Stuck? Send the output to [support@userapp.io](mailto:support@userapp.io) and we'll help you out.

## Special cases

Even though this client works as a proxy and there are no hard-coded API definitions built into it. There are still a few tweaks that are API specific.

#### Calling API `user.login` will automatically set the client token

In other words:

	var loginResult = api.User.Login(login: "test", password: "test");

Is exactly the same as:
	
	var loginResult = api.User.Login(login: "test", password: "test");
	api.GetOptions().Token = loginResult.token;

#### Calling API `user.logout` will automatically remove the client token

In other words:

	api.User.Logout();

Is exactly the same as:
	
	api.User.Logout();
	api.GetOptions().Token = null;
	
## Code Convention Magic

To improve language integration, this library automatically translates naming conventions between the C# and UserApp  domain. I.e. even though the UserApp docs state `user.paymentMethod.get` argument `user_id`, this library is able to accept it as `User.PaymentMethod.Get` argument `UserId`. This also applies to objects. So if the UserApp docs state that a object has the property `first_name` it can be accessed as `FirstName` (i.e. user.FirstName).

## Dependencies

* JsonFx
* EasyHttp

## License

MIT - For more details, see LICENSE.
