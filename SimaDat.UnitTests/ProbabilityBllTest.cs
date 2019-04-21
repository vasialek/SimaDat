using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimaDat.Bll;
using SimaDat.Models;
using SimaDat.Models.Characters;
using SimaDat.Models.Datings;
using SimaDat.Models.Enums;
using SimaDat.Models.Exceptions;
using SimaDat.Models.Interfaces;
using System;

namespace SimaDat.UnitTests
{
	[TestClass]
    public class ProbabilityBllTest
	{
        private IProbabilityBll _bll = null;
        private Hero _me = null;
        private Girl _girl = null;

        [TestInitialize]
        public void TestInit()
        {
            _bll = new ProbabilityBll();

            _me = new Hero();
            _me.CurrentLocationId = 100;
            _me.ModifySkill(Models.Enums.HeroSkills.Charm, MySettings.MaxCharmForHero);

            _girl = new Girl("Friend girl", Models.Enums.FriendshipLevels.Friend);
            _girl.CurrentLocationId = _me.CurrentLocationId;
        }

        #region Request dating

        [TestMethod]
        public void RequestDating_True()
        {
            bool isDating = _bll.RequestDating(_me, _girl);

            isDating.Should().BeTrue();
        }

        [TestMethod]
        public void RequestDating_False()
        {
            _me.ModifySkill(Models.Enums.HeroSkills.Charm, -MySettings.MaxCharmForHero);

            bool isDating = _bll.RequestDating(_me, _girl);

            isDating.Should().BeFalse();
        }

        [TestMethod]
        public void RequestDating_50pct()
        {
            _me.ModifySkill(Models.Enums.HeroSkills.Charm, MySettings.MaxCharmForHero / -2);

			int positive = CountPositiveProbability(() => { return _bll.RequestDating(_me, _girl); });

			// Expected more less 50% of positive
			positive.Should().BeInRange(40, 70);
        }

		#endregion

		#region Kiss

		[TestMethod]
		public void Kiss_Exception_WhenDatingIsOver()
		{
			bool isOk = false;
			var dl = SetupForKiss(_girl, 10);
			dl.IsOver = true;

			try
			{
				_bll.Kiss(dl);
			}
			catch (ObjectDoesNotExistException odneex) when (odneex.ObjectId == dl.DatingLocationId)
			{
				isOk = true;
			}

			// Expecting exception when dating is over
			isOk.Should().BeTrue();
		}

		[TestMethod]
		public void Kiss_HighProbability_WhenCharmAndLover()
		{
			var lover = new Girl("Lover girl", FriendshipLevels.Lover);
			var dl = SetupForKiss(lover, MySettings.MaxCharmForHero);

			int positive = CountPositiveProbability(() => { return _bll.Kiss(dl); });

			// Expecting high probability when charm & lover
			positive.Should().BeInRange(80, 99);
		}

		[TestMethod]
		public void Kiss_LowProbabilityWithoutCharm()
		{
			var datingLocation = SetupForKiss(_girl, 0);

			int positive = CountPositiveProbability(() => { return _bll.Kiss(datingLocation); });
			
			// Expecting low probability, but possible
			positive.Should().BeGreaterOrEqualTo(1);
		}

		#endregion

		#region Setup

		private DatingLocation SetupForKiss(Girl g, int heroCharm)
		{
			_me.ModifySkill(HeroSkills.Charm, heroCharm);
			_me.ResetTtl();

			var datingLocation = new DatingLocation("Test", 0)
			{
				DatingLocationId = 100,
				Girl = g,
				Hero = _me,
			};

			BllFactory.Current.DatingBll.JoinDating(_me, g, datingLocation);

			return datingLocation;
		}

		#endregion

		#region Helpers

		private int CountPositiveProbability(Func<bool> act)
		{
			int positive = 0;
			for (int i = 0; i < 100; i++)
			{
				if (act.Invoke())
				{
					positive++;
				}
			}
			return positive;
		}

		#endregion

	}
}
