using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace TecnoMundo.CartAPI.Utils
{
    public class RandomIdValueGenerator : ValueGenerator<int>
    {
        private static readonly Random random = new Random();
        public override bool GeneratesTemporaryValues => false;

        public override int Next(EntityEntry entry)
        {
            return random.Next(1, int.MaxValue);
        }
    }
}
