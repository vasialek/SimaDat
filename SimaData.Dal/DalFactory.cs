namespace SimaData.Dal
{
    public class DalFactory
    {
        private static DalFactory _dalFactory;

        private ILocationDal _locationDal;

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
