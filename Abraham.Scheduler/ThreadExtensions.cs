namespace Abraham.Scheduler;

/// <summary>
/// Extension for the standard thread class.
/// Enable me to shutdown the thread gracefully.
/// This is a supplement for my Scheduler Nuget package.
/// 
/// Author:
/// Oliver Abraham, mail@oliver-abraham.de, https://www.oliver-abraham.de
/// 
/// Source code hosted at: 
/// https://github.com/OliverAbraham/Abraham.Scheduler
/// 
/// Nuget Package hosted at: 
/// https://www.nuget.org/packages/Abraham.Scheduler/
/// 
/// </summary>
/// 
/// <typeparam name="T">your class containing your data (typically named Configuration)</typeparam>
/// 
/// <example>
/// ------------- Implementation example -------------------------------
///
///        using Abraham.Scheduler;
///
///        private ThreadExtensions _myThread;
///
///        private void StartSupervisorThread()
///        {
///            _myThread = new ThreadExtensions(MyThreadProc);
///            _myThread.thread.Start();
///        }
///
///        private void StopSupervisorThread()
///        {
///            _myThread.SendStopSignalAndWait();
///        }
///
///        private void MyThreadProc()
///        {
///            do
///            {
///            	   try
///            	   {
///            	   }
///            	   catch (Exception ex)
///            	   {
///            	   	   System.Diagnostics.Debug.WriteLine($"Error: {ex.ToString()}");
///            	   }
///            }
///            while (_myThread.Run);
///        }
///
/// </example>
public class ThreadExtensions
{
    #region ------------- Properties ----------------------------------------------------------

    /// <summary>
    /// Access the internal thread
    /// </summary>
    public Thread Thread { get; set; }

    /// <summary>
    /// The Thread Procedure should check this flag often and stop working if false
    /// </summary>
    public bool Run => !CancellationTokenSource.IsCancellationRequested;

    public CancellationTokenSource CancellationTokenSource { get; private set; }
    #endregion



    #region ------------- Init ----------------------------------------------------------------
    public ThreadExtensions(ThreadStart threadProc, string name = "MyThread")
    {
        CancellationTokenSource = new CancellationTokenSource();
        Thread = new Thread(threadProc);
        Thread.Name = name;
        System.Diagnostics.Debug.WriteLine($"Started new Thread '{name} with ManagedThreadId={Thread.ManagedThreadId}");
    }

    public ThreadExtensions(ParameterizedThreadStart threadProc, string name = "MyThread")
    {
        CancellationTokenSource = new CancellationTokenSource();
        Thread = new Thread(threadProc);
        Thread.Name = name;
        System.Diagnostics.Debug.WriteLine($"Started new Thread '{name} with ManagedThreadId={Thread.ManagedThreadId}");
    }
    #endregion



    #region ------------- Methods -------------------------------------------------------------
    public void SendStopSignal()
    {
        System.Diagnostics.Debug.WriteLine($"SendStopSignal and don't wait");
        CancellationTokenSource.Cancel();
    }

    public void SendStopSignalAndWait(int timeoutInSeconds = 10)
    {
        System.Diagnostics.Debug.WriteLine($"SendStopSignalAndWait");
        CancellationTokenSource.Cancel();

        for (int i = 0; Thread.IsAlive && i < (10 * timeoutInSeconds); i++)
            Thread.Sleep(100);

        if (Thread.IsAlive)
        {
            System.Diagnostics.Debug.WriteLine($"SendStopSignalAndWait: The thread didn't respond, aborting now");
            try
            {
                Thread.Abort();
            }
            catch (Exception)
            { 
            }
            System.Diagnostics.Debug.WriteLine($"SendStopSignalAndWait: abort finished");
        }
        else
        {
            System.Diagnostics.Debug.WriteLine($"SendStopSignalAndWait: The thread has ended normally.");
        }
    }

    /// <summary>
    /// Safe method to wait a certain time, but stopping immediately when the thread stop is requested
    /// </summary>
    public void Sleep(int milliseconds)
    {
        CancellationTokenSource.Token.WaitHandle.WaitOne(milliseconds);
    }
    #endregion
}
