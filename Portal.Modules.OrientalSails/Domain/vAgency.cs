using System;
using System.Collections;
using CMS.Core.Domain;

namespace Portal.Modules.OrientalSails.Domain
{
    #region Customer

    /// <summary>
    /// Customer object for NHibernate mapped table 'os_Customer'.
    /// </summary>
    public class vAgency
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
        protected string _name;
        protected string _phone;
        protected string _address;
        protected Role _role;
        protected IList _users;
        protected string _fax;
        protected string _email;
        protected string _taxCode;
        protected int _contractStatus;
        protected string _description;
        protected string _contract;
        protected User _sale;
        protected string _accountant;
        protected PaymentPeriod _paymentMethod;
        protected DateTime? _lastBooking;
        protected IList _bookings;

        protected DateTime? _createdDate;
        protected User _createdBy;
        protected DateTime? _modifiedDate;
        protected User _modifiedBy;

        protected double _total;
        protected double _paid;
        protected double _paidBase;

        protected double _payable;

        #endregion

        #region Constructors

        public vAgency()
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

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public virtual string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        public virtual string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        public virtual string Contract
        {
            get { return _contract; }
            set { _contract = value; }
        }

        public virtual string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public virtual int ContractStatus
        {
            get { return _contractStatus; }
            set { _contractStatus = value; }
        }

        public virtual string TaxCode
        {
            get { return _taxCode; }
            set { _taxCode = value; }
        }

        public virtual string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public virtual string Fax
        {
            get { return _fax; }
            set { _fax = value; }
        }

        public virtual Role Role
        {
            get { return _role; }
            set { _role = value; }
        }

        public virtual IList Users
        {
            get
            {
                if (_users == null)
                {
                    _users = new ArrayList();
                }
                return _users;
            }
            set { _users = value; }
        }

        public virtual IList Bookings
        {
            get
            {
                if (_bookings == null)
                {
                    _bookings = new ArrayList();
                }
                return _bookings;
            }
            set { _bookings = value; }
        }

        public virtual User Sale
        {
            get { return _sale; }
            set { _sale = value; }
        }

        public virtual string Accountant
        {
            get { return _accountant; }
            set { _accountant = value; }
        }

        public virtual PaymentPeriod PaymentPeriod
        {
            get { return _paymentMethod; }
            set { _paymentMethod = value; }
        }

        public virtual DateTime? LastBooking
        {
            get { return _lastBooking; }
            set { _lastBooking = value; }
        }

        /// <summary>
        /// Tổng giá trị các booking chưa được đánh dấu đã thanh toán
        /// </summary>
        public virtual double Total
        {
            get { return _total; }
            set { _total = value; }
        }

        /// <summary>
        /// Tổng tiền thanh toán bằng USD các booking chưa được đánh dấu đã thanh toán
        /// </summary>
        public virtual double Paid
        {
            get { return _paid; }
            set { _paid = value; }
        }

        /// <summary>
        /// Tổng tiền thanh toán bằng VND các booking chưa được đánh dấu đã thanh toán
        /// </summary>
        public virtual double PaidBase
        {
            get { return _paidBase; }
            set { _paidBase = value; }
        }

        /// <summary>
        /// Tổng lượng tiền cần thanh toán cho đối tác
        /// </summary>
        public virtual double Payable
        {
            get { return _payable; }
            set { _payable = value; }
        }

        public virtual DateTime? CreatedDate
        {
            get { return _createdDate; }
            set { _createdDate = value; }
        }

        public virtual DateTime? ModifiedDate
        {
            get { return _modifiedDate; }
            set { _modifiedDate = value; }
        }

        public virtual User CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

        public virtual User ModifiedBy
        {
            get { return _modifiedBy; }
            set { _modifiedBy = value; }
        }

        public virtual Organization Organization { get; set; }

        #endregion
    }
    #endregion
}
