using System;

namespace Portal.Modules.OrientalSails.Domain
{
    #region Customer

    /// <summary>
    /// Customer object for NHibernate mapped table 'os_Customer'.
    /// </summary>
    public class AgencyContract
    {
        #region Static Columns Name

        public static string BIRTHDAY = "Birthday";
        public static string BOOKING = "Booking";
        public static string COUNTRY = "Country";
        public static string FULLNAME = "Fullname";
        public static string PASSPORT = "Passport";
        public static string BOOKINGROOM = "BookingRoom";

        #endregion

        #region Member Variables

        protected int _id;
        protected string _contractName;
        protected byte[] _contractFile;
        protected DateTime _expiredDate;
        protected Agency _agency;
        protected string _fileName;

        #endregion

        #region Constructors

        public AgencyContract()
        {
            _id = -1;
        }

        #endregion

        #region Public Properties

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual string ContractName
        {
            get { return _contractName; }
            set { _contractName = value; }
        }

        public virtual byte[] ContractFile
        {
            get { return _contractFile; }
            set { _contractFile = value; }
        }

        public virtual DateTime ExpiredDate
        {
            get { return _expiredDate; }
            set { _expiredDate = value; }
        }

        public virtual Agency Agency
        {
            get { return _agency; }
            set { _agency = value; }
        }

        public virtual string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public virtual string FilePath { get; set; }

        public virtual DateTime? CreateDate { set; get; }

        public virtual Boolean Received { set; get; }
        #endregion
    }

    #endregion
}
