using SimaDat.Models.Interfaces;

namespace SimaDat.Core
{
	public class RandomProvider : IRandomProvider
	{
		public double NextDouble()
		{
			return new Random().NextDouble();
		}
	}
}
