using SimaDat.Models.Interfaces;
using SimaData.Dal;

namespace SimaDat.Core
{
    public class BllFactory
    {
        private static BllFactory _bllFactory;
        private ILocationBll _locationBll;
        private IHeroBll _heroBll;
        private ICharactersBll _charactersBll;
        private IShopBll _shopBll;
        private IProbabilityBll _possibilityBll;
        private IDatingBll _datingBll;
        private IRandomProvider _randomProvider;

        public static BllFactory Current
        {
            get
            {
                if (_bllFactory == null)
                {
                    _bllFactory = new BllFactory();
                }
                return _bllFactory;
            }
        }

        public ILocationBll LocationBll
        {
            get
            {
                if (_locationBll == null)
                {
                    _locationBll = new LocationBll(CharactersBll, DalFactory.Current.LocationDal);
                }
                return _locationBll;
            }
        }

        public IHeroBll HeroBll
        {
            get
            {
                if (_heroBll == null)
                {
                    _heroBll = new HeroBll(LocationBll);
                }
                return _heroBll;
            }
        }

        public ICharactersBll CharactersBll
        {
            get
            {
                if (_charactersBll == null)
                {
                    _charactersBll = new CharactersBll();
                }
                return _charactersBll;
            }
        }

        public IShopBll ShopBll
        {
            get
            {
                if (_shopBll == null)
                {
                    _shopBll = new ShopBll();
                }
                return _shopBll;
            }
        }

        public IProbabilityBll ProbabilityBll
        {
            get
            {
                if (_possibilityBll == null)
                {
                    _possibilityBll = new ProbabilityBll(Current.RandomProvider);
                }
                return _possibilityBll;
            }
        }

        public IDatingBll DatingBll
        {
            get
            {
                return _datingBll ?? (_datingBll = new DatingBll());
            }
        }

        public IRandomProvider RandomProvider
        {
            get
            {
                return _randomProvider ?? (_randomProvider = new RandomProvider());
            }
        }
    }
}
