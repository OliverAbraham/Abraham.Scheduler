namespace Abraham.Scheduler;

/// <summary>
/// Execute periodic actions in your app very easily.
/// Possible with only one line of code.
/// You can easily set up a scheduler that calls your method every seconds, minute, hour, day or special fractions.
/// You can also schedule a call at the beginning of every minute, hour or day.
/// For examples, please refer to the demo project on github.
/// 
/// Author:
/// Oliver Abraham, mail@oliver-abraham.de, https://www.oliver-abraham.de
/// 
/// Source code hosted at: (with demo project)
/// https://github.com/OliverAbraham/Abraham.Scheduler
/// 
/// Nuget Package hosted at: 
/// https://www.nuget.org/packages/Abraham.Scheduler/
/// 
/// </summary>
/// 
/// <typeparam name="T">your class containing your data (typically named Configuration)</typeparam>
/// 
public class Scheduler
{
    #region ------------- Types and constants -------------------------------------------------
    public delegate Task AsyncTaskActionHandler();
    public delegate void ExceptionHandler(Exception ex);
    public delegate void SchedulerEndedHandler();
    #endregion



    #region ------------- Properties ----------------------------------------------------------
    public Action OnAction 
    {
        get { return _syncTaskActionHandler; }
        set { if (value != null) _syncTaskActionHandler = value; else _syncTaskActionHandler = SyncNullObject; }
    }

    public AsyncTaskActionHandler OnAsyncAction 
    {
        get { return _asyncTaskActionHandler; }
        set { if (value != null) _asyncTaskActionHandler = value; else _asyncTaskActionHandler = AsyncNullObject; }
    }

    public ExceptionHandler OnScheduleException
    {
        get { return _onScheduleException; }
        set { if (value != null) _onScheduleException = value; else _onScheduleException = ExceptionNullObject; }
    }

    public SchedulerEndedHandler OnSchedulerEnded
    {
        get { return _onSchedulerEnded; }
        set { if (value != null) _onSchedulerEnded = value; else _onSchedulerEnded = SchedulerEndedNullObject; }
    }

    public CancellationTokenSource CancellationTokenSource { get; private set; }

    public bool IsRunning { get; private set; }
    #endregion



    #region ------------- Fields --------------------------------------------------------------
    private ThreadExtensions         _thread;
    private TimeSpan                 _timeSpan;
    private TimeSpan                 _firstIntervalTimeSpan;
    private Action                   _syncTaskActionHandler;
    private AsyncTaskActionHandler   _asyncTaskActionHandler;
    private ExceptionHandler         _onScheduleException;
    private SchedulerEndedHandler    _onSchedulerEnded;
    private bool                     _timeSpanNextMinute;
    private bool                     _timeSpanNextHour;
    private bool                     _timeSpanNextDay;
    #endregion



    #region ------------- Init ----------------------------------------------------------------
    public Scheduler()
    {
        _timeSpan = new TimeSpan(0, 0, 1);
        _firstIntervalTimeSpan = default(TimeSpan);
        OnAction = null;
        OnAsyncAction = null;
        OnScheduleException = null;
        OnSchedulerEnded = null;
    }
    #endregion



    #region ------------- Methods -------------------------------------------------------------
    /// <summary>
    /// Set a function that will be called by the scheduler periodically
    /// </summary>
    public Scheduler UseAction(Action actionHandler)
    {
        _syncTaskActionHandler = actionHandler;
        return this;
    }

    /// <summary>
    /// Set a function that will be called by the scheduler periodically
    /// </summary>
    public Scheduler UseAsyncAction(AsyncTaskActionHandler actionHandler)
    {
        _asyncTaskActionHandler = actionHandler;
        return this;
    }

    /// <summary>
    /// Set the scheduled interval (the period between the calls)
    /// </summary>
    public Scheduler UseInterval(TimeSpan timeSpan)
    {
        _timeSpan = timeSpan;
        return this;
    }

    /// <summary>
    /// Set the time until the first call is made (for example if you want to start at the beginning of the program right now, but then every 10 minutes)
    /// </summary>
    public Scheduler UseFirstInterval(TimeSpan timeSpan)
    {
        _firstIntervalTimeSpan = timeSpan;
        return this;
    }

    /// <summary>
    /// Set the first call to be made right now
    /// </summary>
    public Scheduler UseFirstStartRightNow()
    {
        _firstIntervalTimeSpan = new TimeSpan(0);
        return this;
    }

    /// <summary>
    /// Set the scheduled interval (the period between the calls)
    /// </summary>
    public Scheduler UseIntervalSeconds(int seconds)
    {
        _timeSpan = new TimeSpan(0, 0, seconds);
        return this;
    }

    /// <summary>
    /// Set the scheduled interval (the period between the calls)
    /// </summary>
    public Scheduler UseIntervalMinutes(int minutes)
    {
        _timeSpan = new TimeSpan(0, minutes, 0);
        return this;
    }

    /// <summary>
    /// Set the scheduled interval (the period between the calls)
    /// </summary>
    public Scheduler UseIntervalHours(int hours)
    {
        _timeSpan = new TimeSpan(hours, 0, 0);
        return this;
    }

    /// <summary>
    /// Set the scheduled interval to the beginning of the next minute
    /// </summary>
    public Scheduler UseIntervalNextStartingMinute()
    {
        _timeSpanNextMinute = true;
        return this;
    }

    /// <summary>
    /// Set the scheduled interval to the beginning of the next hour
    /// </summary>
    public Scheduler UseIntervalNextStartingHour()
    {
        _timeSpanNextHour = true;
        return this;
    }

    /// <summary>
    /// Set the scheduled interval to the beginning of the next day
    /// </summary>
    public Scheduler UseIntervalNextStartingDay()
    {
        _timeSpanNextDay = true;
        return this;
    }

    /// <summary>
    /// Call this method to start the scheduler
    /// </summary>
    public Scheduler Start()
    {
        CancellationTokenSource = new CancellationTokenSource();
        _thread = new ThreadExtensions(new ParameterizedThreadStart(
            delegate (object data)
            {
                SchedulerProc();
            }));

        _thread.Thread.Start();
        return this;
    }

    /// <summary>
    /// Call this method to do the next call right now
    /// </summary>
    public Scheduler Restart()
    {
        CancellationTokenSource.Cancel();
        return this;
    }

    /// <summary>
    /// Call this method to stop the scheduler
    /// </summary>
    public Scheduler Stop()
    {
        System.Diagnostics.Debug.WriteLine($"Scheduler: stop begin");
        _thread.SendStopSignal();
        CancellationTokenSource.Cancel();
        System.Diagnostics.Debug.WriteLine($"Scheduler: stop end");
        return this;
    }

    /// <summary>
    /// Call this method to stop the scheduler and wait until it finishes (for example at the end of your program)
    /// </summary>
    public Scheduler StopAndWait(int timeoutInSeconds = 10)
    {
        CancellationTokenSource.Cancel();
        _thread.SendStopSignalAndWait(timeoutInSeconds);
        return this;
    }
    #endregion



    #region ------------- Implementation ------------------------------------------------------
    private void SchedulerProc()
    {
        System.Diagnostics.Debug.WriteLine("SchedulerProc: SchedulerProc entered");
        try
        {
            IsRunning = true;
            while (_thread.Run && !CancellationTokenSource.IsCancellationRequested)
            {
                _syncTaskActionHandler();
                //_asyncTaskActionHandler().GetAwaiter().GetResult();
                if (CancellationTokenSource.IsCancellationRequested)
                {
                    System.Diagnostics.Debug.WriteLine($"SchedulerProc: Cancellation requested run={_thread.Run}");
                    break;
                }
                Wait();
                System.Diagnostics.Debug.WriteLine($"SchedulerProc: wait ended run={_thread.Run}");
            }
            System.Diagnostics.Debug.WriteLine("SchedulerProc: scheduler loop exited");
        }
        catch (Exception ex)
        {
            IsRunning = false;
            _onScheduleException(ex);
        }
        finally
        {
            IsRunning = false;
            _onSchedulerEnded();
        }
        System.Diagnostics.Debug.WriteLine("SchedulerProc: SchedulerProc exited");
    }

    private void Wait()
    {
        TimeSpan span;
        if (_firstIntervalTimeSpan != default(TimeSpan))
        {
            span = _firstIntervalTimeSpan;
            _firstIntervalTimeSpan = default(TimeSpan);
        }
        else if (_timeSpanNextMinute)
        {
            var seconds = 60 - DateTime.Now.Second;
            span = new TimeSpan(0, 0, seconds);
        }
        else if (_timeSpanNextHour)
        {
            var now = DateTime.Now;
            var seconds = 60 - now.Second;
            var minutes = 60 - now.Minute;
            span = new TimeSpan(0, minutes, seconds);
        }
        else if (_timeSpanNextDay)
        {
            var now = DateTime.Now;
            var nextMidnight = now.AddDays(1);
            var nextDayDate = new DateTime(nextMidnight.Year, nextMidnight.Month, nextMidnight.Day, 0, 0, 0);
            span = nextDayDate - now;
        }
        else if (_timeSpan != default(TimeSpan))
        {
            span = _timeSpan;
        }
        else
        {
            throw new Exception("unknown wait time!");
        }
            
        CancellationTokenSource.Token.WaitHandle.WaitOne(span);
    }

    #pragma warning disable 1998
    private async Task AsyncNullObject()
    {
    }

    private void SyncNullObject()
    {
    }

    private void ExceptionNullObject(Exception ex)
    {
    }

    private void SchedulerEndedNullObject()
    {
    }
    #endregion
}