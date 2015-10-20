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
                    _locationBll = new LocationBll(DalFactory.Current.LocationDal);
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
    }
}
