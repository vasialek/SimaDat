using SimaDat.Models.Interfaces;
using System;

namespace SimaDat.Bll
{
	public class RandomProvider : IRandomProvider
	{
		public double NextDouble()
		{
			return new Random().NextDouble();
		}
	}
}
