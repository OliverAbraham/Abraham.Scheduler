# Abraham.Scheduler

![](https://img.shields.io/github/downloads/oliverabraham/Abraham.Scheduler/total) ![](https://img.shields.io/github/license/oliverabraham/Abraham.Scheduler) ![](https://img.shields.io/github/languages/count/oliverabraham/Abraham.Scheduler) ![GitHub Repo stars](https://img.shields.io/github/stars/oliverabraham/Abraham.Scheduler?label=repo%20stars) ![GitHub Repo stars](https://img.shields.io/github/stars/oliverabraham?label=user%20stars)


## OVERVIEW

Execute periodic actions in your app very easily.
Possible with only one line of code.
You can easily set up a scheduler that calls your method every seconds, minute, hour, day or special fractions.
You can also schedule a call at the beginning of every minute, hour or day.
For examples, please refer to the demo project on github.



## LICENSE

Licensed under Apache licence.
https://www.apache.org/licenses/LICENSE-2.0


## Compatibility

The nuget package was build with DotNET 6.



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


## AUTHOR

Oliver Abraham, mail@oliver-abraham.de, https://www.oliver-abraham.de

Please feel free to comment and suggest improvements!


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



## SOURCE CODE

The source code is hosted at:

https://github.com/OliverAbraham/Abraham.Scheduler

The nuget package is hostet at:

https://www.nuget.org/packages/Abraham.Scheduler



## SCREENSHOTS

![](Screenshots/Screenshot1.jpg)



# MAKE A DONATION !

If you find this application useful, buy me a coffee!
I would appreciate a small donation on https://www.buymeacoffee.com/oliverabraham

<a href="https://www.buymeacoffee.com/app/oliverabraham" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/v2/default-yellow.png" alt="Buy Me A Coffee" style="height: 60px !important;width: 217px !important;" ></a>
