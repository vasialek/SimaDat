using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimaData.Dal
{
    public class DalFactory
    {
        private static DalFactory _dalFactory = null;

        private ILocationDal _locationDal = null;

        public static DalFactory Current
        {
            get
            {
                if (_dalFactory == null)
                {
                    _dalFactory = new DalFactory();
                }
                return _dalFactory;
            }
        }

        public ILocationDal LocationDal
        {
            get
            {
                if (_locationDal == null)
                {
                    _locationDal = new LocationDal();
                }
                return _locationDal;
            }
        }
    }
}
