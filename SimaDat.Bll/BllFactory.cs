using SimaDat.Models.Interfaces;
using SimaData.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimaDat.Bll
{

    public class BllFactory
    {
        private static BllFactory _bllFactory = null;

        private ILocationBll _locationBll = null;

        private IHeroBll _heroBll = null;

        private ICharactersBll _charactersBll = null;

        private IShopBll _shopBll = null;

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
    }
}
