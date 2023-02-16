internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var punchClock = new RoutinR.Services.PunchClockService();

        Console.WriteLine($"PunchClock running before starting: {punchClock.IsRunning}");

        punchClock.Start();

        Console.WriteLine($"PunchClock running after starting: {punchClock.IsRunning}");



        var tick = Tick();

        Console.WriteLine($"PunchClock outputting on main thread running after starting");
        await Task.Delay(1);

        Console.WriteLine($"PunchClock just funnily outputting from main thread again");
        await Task.Delay(1);

        Console.WriteLine($"PunchClock main thread waiting for tick to finish");
        await Task.Delay(1);

        var finalValue = await tick;

        punchClock.Stop();

        Console.WriteLine($"PunchClock running after stopping: {punchClock.IsRunning}");
    }

    private static async Task<bool> Tick()
    {
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine("ticking");
            await Task.Delay(1);
        }

        return true;
    }
}