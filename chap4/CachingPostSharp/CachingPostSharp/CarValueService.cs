using System;
using System.Threading;

namespace CachingPostSharp
{
    public class CarValueArgs
    {
        public int MakeId { get; set; }
        public int Year { get; set; }
        public int ConditionId { get; set; }
    }

    public class CarValueService
    {
        readonly Random _rand;

        public CarValueService()
        {
            _rand = new Random();
        }

        [CacheAspect]
        public decimal GetValue(int makeId, int conditionId, int year)
        {
            //Thread.Sleep(5000);
            return _rand.Next(10000, 20000);
        }

        [CacheAspect]
        public decimal GetValue(CarValueArgs args)
        {
            Thread.Sleep(5000);
            return _rand.Next(10000, 20000);
        }
    }
}