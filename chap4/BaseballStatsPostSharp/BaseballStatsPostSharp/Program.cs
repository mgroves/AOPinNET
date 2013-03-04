using System;
using PostSharp.Aspects;

namespace BaseballStatsPostSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var stats = new BaseballStats();
            var player = "Joey Votto";
            var battingAvg = stats.GetBattingAverage(player);
            Console.WriteLine("{0}'s batting average: {1}", player, battingAvg);
        }
    }

    public class BaseballStats
    {
        [MyBoundaryAspect]
        public decimal GetBattingAverage(string playerName)
        {
            if (playerName == "Joey Votto")
                return 0.309M;
            if (playerName == "Brandon Phillips")
                return 0.300M;
            return 0.000M;
        }
    }

    [Serializable]
    public class MyBoundaryAspect : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            Console.WriteLine("Before the method");
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            Console.WriteLine("After the method");
        }
    }
}
