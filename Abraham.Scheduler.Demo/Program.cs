using Abraham.Scheduler;

namespace Abraham.Scheduler.Demo;

/// <summary>
/// Demo of the Scheduler Nuget package.
/// Demonstrates how to schedule different intervals.
/// 
/// Author:
/// Oliver Abraham, mail@oliver-abraham.de, https://www.oliver-abraham.de
/// 
/// Source code hosted at: 
/// https://github.com/OliverAbraham/Abraham.ProgramSettingsManager
/// 
/// Nuget Package hosted at: 
/// https://www.nuget.org/packages/Abraham.ProgramSettingsManager/
/// 
/// </summary>
/// 
internal class Program
{
    private static Scheduler _myScheduler;

    static void Main(string[] args)
    {
        Console.WriteLine("Demo for the Nuget package 'Abraham.Scheduler'");
        Console.WriteLine("Press any key to end the demo.");



        // easy version:
        _myScheduler = new Scheduler()
            .UseAction( () => Console.WriteLine($"Action!") )
            .Start();






        //       // set the interval to 10 seconds
        //       _myScheduler = new Scheduler()
        //           .UseAction( () => Console.WriteLine($"Action!") )
        //           .UseIntervalSeconds(10)
        //           .Start();
        //
        //
        //       // set the interval to 10 minutes
        //       _myScheduler = new Scheduler()
        //           .UseAction( () => Console.WriteLine($"Action!") )
        //           .UseIntervalMinutes(10)
        //           .Start();
        //
        //
        //       // set the interval to 1 hour
        //       _myScheduler = new Scheduler()
        //           .UseAction( () => Console.WriteLine($"Action!") )
        //           .UseIntervalHours(1)
        //           .Start();
        //
        //
        //       // set the interval to a fraction: every 1 day, 2 hours, 3 minutes (i know, just for demostration)
        //       _myScheduler = new Scheduler()
        //           .UseAction( () => Console.WriteLine($"Action!") )
        //           .UseInterval(new TimeSpan(1, 2, 3, 0))
        //           .Start();
        //
        //
        //       // set the interval to every beginning minute
        //       _myScheduler = new Scheduler()
        //           .UseAction(() => Console.WriteLine($"Action! time is now {DateTime.Now}"))
        //           .UseIntervalNextStartingMinute()
        //           .Start();
        //
        //
        //       // set the interval to every beginning hour (will schedule to 1am, 2am, 3am etc)
        //       _myScheduler = new Scheduler()
        //           .UseAction(() => Console.WriteLine($"Action! time is now {DateTime.Now}"))
        //           .UseIntervalNextStartingHour()
        //           .Start();
        //
        //
        //       // set the interval to every beginning day (will schedule every mignight)
        //       _myScheduler = new Scheduler()
        //           .UseAction(() => Console.WriteLine($"Action! time is now {DateTime.Now}"))
        //           .UseIntervalNextStartingDay()
        //           .Start();
        //       
        //
        //       // version using a async handler:
        //       _myScheduler = new Scheduler()
        //           .UseAsyncAction(MyAsyncActionHandler)
        //           .Start();

        Console.ReadKey();
        _myScheduler.StopAndWait();
    }

//    private static async Task MyAsyncActionHandler()
//    {
//        Console.WriteLine($"Action!");
//    }
}