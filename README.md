# Abraham.Scheduler

## OVERVIEW

Execute periodic actions in your app very easily.
Possible with only one line of code.
You can easily set up a scheduler that calls your method every seconds, minute, hour, day or special fractions.
You can also schedule a call at the beginning of every minute, hour or day.
For examples, please refer to the demo project on github.




## INSTALLATION

Install the Nuget package "Abraham.Scheduler" into your application (from https://www.nuget.org).

Add the following code:
```C#
    private static Scheduler _myScheduler;

    static void Main(string[] args)
    {
        _myScheduler = new Scheduler()
            .UseAction( () => Console.WriteLine($"Action!") )
            .Start();
    }
```


That's it!
This one-liner will call your action every second.

For more options, please refer to my Demo application in the github repository (see below).
The Demo and the nuget source code is well documented.



## HOW TO INSTALL A NUGET PACKAGE
This is very simple:
- Start Visual Studio (with NuGet installed) 
- Right-click on your project's References and choose "Manage NuGet Packages..."
- Choose Online category from the left
- Enter the name of the nuget package to the top right search and hit enter
- Choose your package from search results and hit install
- Done!


or from NuGet Command-Line:

    Install-Package Abraham.Scheduler





## AUTHOR

Oliver Abraham, mail@oliver-abraham.de, https://www.oliver-abraham.de

Please feel free to comment and suggest improvements!



## SOURCE CODE

The source code is hosted at:

https://github.com/OliverAbraham/Abraham.Scheduler

The Nuget Package is hosted at: 

https://www.nuget.org/packages/Abraham.Scheduler
