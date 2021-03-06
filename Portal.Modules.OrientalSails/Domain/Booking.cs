using System;
using System.Collections;
using System.Collections.Generic;
using CMS.Core.Domain;
using Portal.Modules.OrientalSails.Web.Util;

namespace Portal.Modules.OrientalSails.Domain
{
    #region Booking

    /// <summary>
    /// Booking object for NHibernate mapped table 'os_Booking'.
    /// </summary>
    public class Booking
    {

        #region Static Columns Name

        public static string CONFIRMEDBY = "ConfirmedBy";
        public static string CREATEDBY = "CreatedBy";
        public static string CREATEDDATE = "CreatedDate";
        public static string DELETED = "Deleted";
        public static string MODIFIEDBY = "ModifiedBy";
        public static string MODIFIEDDATE = "ModifiedDate";
        public static string PARTNERID = "PảPartnerId";
        public static string SALEID = "SaleId";
        public static string STARTDATE = "StartDate";
        public static string STATUS = "Status";
        public static string TRIP = "Trip";
        public static string TRIPOPION = "TripOpion";
        public static string TOTAL = "Total";
        public static string PAID = "Paid";

        #endregion

        #region Member Variables

        protected IList _bookosBookingRooms;
        protected User _confirmedBy;
        protected User _createdBy;
        protected DateTime _createdDate;
        protected bool _deleted;
        protected int _id;
        protected User _modifiedBy;
        protected DateTime _modifiedDate;
        protected User _partnerId;
        protected User _saleId;
        protected DateTime _startDate;
        protected StatusType _status;
        protected SailsTrip _trip;
        protected Cruise _cruise;
        protected TripOption _tripOpion;
        protected IList _customers;
        protected IList _extraServices;
        protected double _total;
        protected double _totalVND;
        protected double _paid;
        protected string _email;
        protected string _name;
        protected string _note;
        protected Agency _agency;
        protected string _pickupAddress;
        protected string _specialRequest;
        protected string _agecyCode;
        protected AgencyContact _booker;
        protected bool _isApproved;
        protected AccountingStatus _accountingStatus;
        protected int _amended;
        protected bool _isCharter;
        protected string _dropoffAddress;
        protected bool _hasInvoice;
        protected double _cancelPay;
        protected double _cancelPayVND;

        protected string _guide;
        protected string _driver;
        protected bool _guideOnboard;

        protected User _locker;

        protected IList _services;

        #region -- transfer --
        protected bool _isTransferred;
        protected double _transferCost;
        protected Agency _transferTo;
        protected int _transferAdult;
        protected int _transferChild;
        protected int _transferBaby;
        #endregion

        protected double _currencyRate;
        protected bool _isPaid;
        protected double _paidBase;
        protected bool _isPaymentNeeded;


        protected string _bookingId;
        protected string _customBookingCode;
        protected int _customBookingId;

        protected DateTime _endDate;

        protected Locked _lock;

        protected double _commission;
        protected double _commissionVND;

        protected double _comUSDpayed;
        protected double _comVNDpayed;
        protected double _comRate;
        protected bool _comPaid;

        #region -- Countable --
        protected int _adult;
        protected int _child;
        protected int _baby;
        protected bool _counted;

        protected int _double;
        protected int _twin;
        protected bool _roomCounted;
        #endregion

        #endregion

        #region Constructors

        public Booking()
        {
            _id = -1;
        }

        public Booking(bool deleted, User createdBy, DateTime createdDate, User modifiedBy, DateTime modifiedDate,
                       User partnerId, User saleId, DateTime startDate, StatusType status, TripOption tripOpion,
                       SailsTrip trip)
        {
            _deleted = deleted;
            _createdBy = createdBy;
            _createdDate = createdDate;
            _modifiedBy = modifiedBy;
            _modifiedDate = modifiedDate;
            _partnerId = partnerId;
            _saleId = saleId;
            _startDate = startDate;
            _status = status;
            _tripOpion = tripOpion;
            _trip = trip;
        }

        #endregion

        #region Public Properties

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual string BookingIdOS
        {
            get { return string.Format("ATM{0:00000} - {1}", _id, _trip.TripCode); }
        }

        public virtual bool Deleted
        {
            get { return _deleted; }
            set { _deleted = value; }
        }

        public virtual User CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

        public virtual DateTime CreatedDate
        {
            get { return _createdDate; }
            set { _createdDate = value; }
        }

        public virtual User ModifiedBy
        {
            get { return _modifiedBy; }
            set { _modifiedBy = value; }
        }

        public virtual User ConfirmedBy
        {
            get { return _confirmedBy; }
            set { _confirmedBy = value; }
        }

        public virtual DateTime ModifiedDate
        {
            get { return _modifiedDate; }
            set { _modifiedDate = value; }
        }

        public virtual User Partner
        {
            get { return _partnerId; }
            set { _partnerId = value; }
        }

        public virtual User Sale
        {
            get { return _saleId; }
            set { _saleId = value; }
        }

        public virtual DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        public virtual StatusType Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public virtual TripOption TripOption
        {
            get { return _tripOpion; }
            set { _tripOpion = value; }
        }

        public virtual SailsTrip Trip
        {
            get { return _trip; }
            set { _trip = value; }
        }

        /// <summary>
        /// Tổng số tiền (tính theo tiền tệ USD, tự động convert theo tỉ giá nếu là VND)
        /// </summary>
        public virtual double Total { set; get; }

        public virtual double TotalVND { set; get; }

        public virtual bool IsVND { get; set; }
        public virtual bool IsGuideCollectVND { get; set; }
        public virtual bool IsDriverCollectVND { get; set; }
        public virtual bool IsCommissionVND { get; set; }
        public virtual bool IsCancelPayVND { get; set; }

        public virtual double Paid
        {
            get { return _paid; }
            set { _paid = value; }
        }

        /// <summary>
        /// Email liên lạc, dùng cho anonymous book
        /// </summary>
        public virtual string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        /// <summary>
        /// Tên liên lạc, dùng cho anonymous book
        /// </summary>
        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public virtual Agency Agency
        {
            get { return _agency; }
            set { _agency = value; }
        }

        public virtual string PickupAddress
        {
            get { return _pickupAddress; }
            set { _pickupAddress = value; }
        }

        public virtual string SpecialRequest
        {
            get { return _specialRequest; }
            set { _specialRequest = value; }
        }

        public virtual string AgencyCode
        {
            get { return _agecyCode; }
            set { _agecyCode = value; }
        }

        /// <summary>
        /// Tỉ giá hối đoái áp dụng cho thanh toán
        /// </summary>
        public virtual double CurrencyRate
        {
            get { return _currencyRate; }
            set { _currencyRate = value; }
        }

        /// <summary>
        /// Tiền thanh toán bằng đồng tiền gốc
        /// </summary>
        public virtual double PaidBase
        {
            get { return _paidBase; }
            set { _paidBase = value; }
        }

        /// <summary>
        /// Đánh dấu thanh toán hết cho book (kể cả có chênh lệnh giữa tổng paid và tổng giá)
        /// </summary>
        public virtual bool IsPaid
        {
            get
            {
                // Lưu ý: trạng thái IsPaid không phụ thuộc vào guide collect
                // Chỉ có số tiền còn lại sẽ = (Tổng giá trị - guide collect)
                return _isPaid;
            }
            set { _isPaid = value; }
        }

        public virtual AccountingStatus AccountingStatus
        {
            get { return _accountingStatus; }
            set { _accountingStatus = value; }
        }

        public virtual IList Customers
        {
            get
            {
                if (_customers == null)
                {
                    _customers = new ArrayList();
                }
                return _customers;
            }
            set { _customers = value; }
        }
        public virtual IList BookingRooms
        {
            get
            {
                if (_bookosBookingRooms == null)
                {
                    _bookosBookingRooms = new ArrayList();
                }
                return _bookosBookingRooms;
            }
            set { _bookosBookingRooms = value; }
        }
        public virtual IList ExtraServices
        {
            get
            {
                if (_extraServices == null)
                {
                    _extraServices = new ArrayList();
                }
                return _extraServices;
            }
            set { _extraServices = value; }
        }

        /// <summary>
        /// Các dịch vụ nhập custom
        /// </summary>
        public virtual IList Services
        {
            get
            {
                if (_services == null)
                {
                    _services = new ArrayList();
                }
                return _services;
            }
            set { _services = value; }
        }

        public virtual string BookingId
        {
            get { return _bookingId; }
            set { _bookingId = value; }
        }

        public virtual int CustomBookingId
        {
            get { return _customBookingId; }
            set { _customBookingId = value; }
        }

        public virtual string CustomBookingCode
        {
            get { return _customBookingCode; }
            set { _customBookingCode = value; }
        }

        public virtual Locked Charter
        {
            get { return _lock; }
            set { _lock = value; }
        }

        #region -- Chuyển sang tàu khác --
        /// <summary>
        /// Phí khi chuyển sang cho tàu khác
        /// </summary>
        public virtual double TransferCost
        {
            get { return _transferCost; }
            set { _transferCost = value; }
        }

        /// <summary>
        /// Có đánh dấu chuyển sang tàu khác không
        /// </summary>
        public virtual bool IsTransferred
        {
            get { return _isTransferred; }
            set { _isTransferred = value; }
        }

        public virtual Agency TransferTo
        {
            get { return _transferTo; }
            set { _transferTo = value; }
        }

        public virtual int TransferAdult
        {
            get { return _transferAdult; }
            set { _transferAdult = value; }
        }

        public virtual int TransferChildren
        {
            get { return _transferChild; }
            set { _transferChild = value; }
        }

        public virtual int TransferBaby
        {
            get { return _transferBaby; }
            set { _transferBaby = value; }
        }
        #endregion

        public virtual int Amended
        {
            get { return _amended; }
            set { _amended = value; }
        }

        public virtual Cruise Cruise
        {
            get { return _cruise; }
            set { _cruise = value; }
        }

        public virtual bool IsCharter
        {
            get
            {
                if (_lock != null)
                {
                    _isCharter = true;
                }
                return _isCharter;
            }
            set { _isCharter = value; }
        }

        public virtual bool HasInvoice
        {
            get { return _hasInvoice; }
            set { _hasInvoice = value; }
        }

        public virtual double CancelPay
        {
            get { return _cancelPay; }
            set { _cancelPay = value; }
        }

        public virtual double CancelPayVND
        {
            get { return _cancelPayVND; }
            set { _cancelPayVND = value; }
        }

        public virtual string Guide
        {
            get { return _guide; }
            set { _guide = value; }
        }

        public virtual string Driver
        {
            get { return _driver; }
            set { _driver = value; }
        }

        public virtual bool GuideOnboard
        {
            get { return _guideOnboard; }
            set { _guideOnboard = value; }
        }

        public virtual User Locker
        {
            get { return _locker; }
            set { _locker = value; }
        }

        #region -- Commission --
        public virtual double Commission
        {
            get { return _commission; }
            set { _commission = value; }
        }

        public virtual double CommissionVND
        {
            get { return _commissionVND; }
            set { _commissionVND = value; }
        }

        public virtual double ComVNDpayed
        {
            get { return _comVNDpayed; }
            set { _comVNDpayed = value; }
        }

        public virtual double ComUSDpayed
        {
            get { return _comUSDpayed; }
            set { _comUSDpayed = value; }
        }

        public virtual double ComRate
        {
            get { return _comRate; }
            set { _comRate = value; }
        }

        public virtual bool ComPaid
        {
            get { return _comPaid; }
            set { _comPaid = value; }
        }
        #endregion

        private int _group;
        public virtual int Group
        {
            get
            {
                if (_group == 0)
                {
                    _group = 1;
                }
                return _group;
            }
            set { _group = value; }
        }

        #region -- Guide collect --
        /// <summary>
        /// Số tiền guide thu hộ
        /// </summary>
        public virtual double GuideCollect { get; set; }

        public virtual double DriverCollect { get; set; }

        public virtual double GuideCollectVND { get; set; }

        public virtual double DriverCollectVND { get; set; }
        /// <summary>
        /// Đánh dấu đã thu
        /// </summary>
        public virtual bool GuideCollected { get; set; }

        public virtual bool DriverCollected { get; set; }

        /// <summary>
        /// Số tiền đã thu từ guide theo USD
        /// </summary>
        public virtual double GuideCollectedUSD { get; set; }

        public virtual double DriverCollectedUSD { get; set; }

        /// <summary>
        /// Số tiền đã thu từ guide bằng VND
        /// </summary>
        public virtual double GuideCollectedVND { get; set; }

        public virtual double DriverCollectedVND { get; set; }
        /// <summary>
        /// Số tiền phải thu theo guide collect
        /// </summary>
        public virtual double GuideCollectReceivable
        {
            get
            {
                // Nếu đã trả rồi thì là 0
                if (GuideCollected)
                {
                    return 0;
                }
                // Nếu không tính bằng (tổng giá in USD - đã trả in USD)* tỉ giá - đã trã in VNĐ == còn lại theo VNĐ
                if (IsGuideCollectVND)
                    return GuideCollectVND - GuideCollectedUSD * _currencyRate - GuideCollectedVND;
                else
                    return (GuideCollect - GuideCollectedUSD) * _currencyRate - GuideCollectedVND;
            }
        }

        public virtual double DriverCollectReceivable
        {
            get
            {
                // Nếu đã trả rồi thì là 0
                if (DriverCollected)
                {
                    return 0;
                }
                // Nếu không tính bằng (tổng giá in USD - đã trả in USD)* tỉ giá - đã trã in VNĐ == còn lại theo VNĐ
                if (IsDriverCollectVND)
                    return DriverCollectVND - DriverCollectedUSD * _currencyRate - DriverCollectedVND;
                else
                    return (DriverCollect - DriverCollectedUSD) * _currencyRate - DriverCollectedVND;
            }
        }
        #endregion

        /// <summary>
        /// Ngày thanh toán
        /// </summary>
        public virtual DateTime? PaidDate { get; set; }

        /// <summary>
        /// Ngày thanh toán commission
        /// </summary>
        public virtual DateTime? ComPaidDate { get; set; }

        /// <summary>
        /// Xác định là đã trả dù có sai lệch về giá trị thanh toán
        /// </summary>
        public virtual bool GuideConfirmed { get; set; }

        /// <summary>
        /// Xác định là đối tác đã trả dù có sai lệch về giá trị thanh toán
        /// </summary>
        public virtual bool AgencyConfirmed { get; set; }

        #region -- Partner receivable --

        private IList<Transaction> _transactions;
        public virtual IList<Transaction> Transactions
        {
            get
            {
                if (_transactions == null)
                {
                    _transactions = new List<Transaction>();
                }
                return _transactions;
            }
            set { _transactions = value; }
        }

        #endregion

        #endregion

        #region -- Formula properties --
        /// <summary>
        /// 
        /// </summary>
        public virtual double GuideCollectedRemain { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual double AgencyRemain { get; set; }
        #endregion

        #region -- Calculated properties --

        #region -- Pax --
        /// <summary>
        /// Số lượng khách người lớn, tính bằng tổng khách người lớn trong từng room
        /// </summary>
        public virtual int Adult
        {
            get
            {
                if (!_counted)
                {
                    CountCustomers();
                }
                return _adult;
            }
        }

        /// <summary>
        /// Số lượng khách (bao gồm cả người lớn và trẻ em)
        /// </summary>
        public virtual int Pax
        {
            get { return _customers.Count; }
        }

        /// <summary>
        /// Số lượng khách trẻ em
        /// </summary>
        public virtual int Child
        {
            get
            {
                if (!_counted)
                {
                    CountCustomers();
                }
                return _child;
            }
        }

        /// <summary>
        /// Số lượng trẻ sơ sinh
        /// </summary>
        public virtual int Baby
        {
            get
            {
                if (!_counted)
                {
                    CountCustomers();
                }
                return _baby;
            }
        }

        /// <summary>
        /// Hàm đếm số khách từng loại
        /// </summary>
        protected virtual void CountCustomers()
        {
            _child = 0;
            _baby = 0;
            _adult = 0;
            foreach (Customer customer in _customers)
            {
                switch (customer.Type)
                {
                    case CustomerType.Adult:
                        _adult++;
                        break;
                    case CustomerType.Children:
                        _child++;
                        break;
                    case CustomerType.Baby:
                        _baby++;
                        break;
                }
            }
            _counted = true;
        }
        #endregion

        #region -- Cabin --
        public virtual int DoubleCabin
        {
            get
            {
                if (!_roomCounted)
                {
                    CountCabin();
                }
                return _double;
            }
        }

        public virtual int TwinCabin
        {
            get
            {
                if (!_roomCounted)
                {
                    CountCabin();
                }
                return _twin;
            }
        }

        protected virtual void CountCabin()
        {
            _double = 0;
            _twin = 0;
            foreach (BookingRoom bookingRoom in BookingRooms)
            {
                if (bookingRoom.RoomType != null && bookingRoom.RoomType.Id == SailsModule.DOUBLE)
                {
                    _double++;
                }
                if (bookingRoom.RoomType != null && bookingRoom.RoomType.IsShared)
                {
                    _twin += bookingRoom.Adult;
                }
            }
            _roomCounted = true;
        }
        #endregion

        /// <summary>
        /// Tổng Số tiền cần thu (bao gồm cả guide collect chưa thu)
        /// </summary>
        public virtual double TotalReceivable
        {
            get { return GuideCollectReceivable + AgencyReceivable; }
        }

        /// <summary>
        /// Tổng số tiền phải thu từ đối tác ( đã trừ guide collect)
        /// </summary>
        public virtual double AgencyReceivable
        {
            get
            {
                // Nếu đã trả rồi thì là 0
                if (_isPaid)
                {
                    return 0;
                }
                if (IsGuideCollectVND)
                {
                    if (IsVND)
                        return ValueVND - _paid + GuideCollectedUSD * _currencyRate + GuideCollectedVND - GuideCollectVND - _paidBase;
                    else
                    {
                        return Value * _currencyRate - _paid * _currencyRate + GuideCollectedUSD * _currencyRate + GuideCollectedVND - GuideCollectVND - _paidBase;
                    }
                }
                else
                {
                    // Nếu không tính bằng Tổng giá trị VND (đã trả in USD + guide collect)* tỉ giá - đã trã in VNĐ == còn lại theo VNĐ
                    if (IsVND)
                        return ValueVND - (_paid + GuideCollect) * _currencyRate - _paidBase + GuideCollectedUSD * _currencyRate + GuideCollectedVND;
                    else
                        return Value * _currencyRate - (_paid + GuideCollect) * _currencyRate - _paidBase + GuideCollectedUSD * _currencyRate + GuideCollectedVND;

                }
            }
        }

        /// <summary>
        /// Còn lại theo VND
        /// </summary>
        public virtual double CommissionLeft
        {
            get
            {
                // Nếu đã trả rồi thì là 0
                if (_comPaid)
                {
                    return 0;
                }
                // Nếu không tính bằng (tổng giá in USD - đã trả in USD)* tỉ giá - đã trã in VNĐ == còn lại theo VNĐ
                if (IsCommissionVND)
                    return CommissionVND - _comUSDpayed * _comRate - _comVNDpayed;
                else
                    return (Commission - _comUSDpayed) * _comRate - _comVNDpayed;
            }
        }

        public virtual string ContactEmail
        {
            get
            {
                if (string.IsNullOrEmpty(_email))
                {
                    if (_createdBy != null)
                    {
                        return _createdBy.Email;
                    }
                    return string.Empty;
                }
                return _email;
            }
        }

        public virtual string ContactName
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                {
                    if (_createdBy != null)
                    {
                        return _createdBy.FullName;
                    }
                    return string.Empty;
                }
                return _name;
            }
        }

        public virtual string CustomerName
        {
            get
            {
                string name = string.Empty;
                //foreach (BookingRoom room in _bookosBookingRooms)
                //{
                foreach (Customer customer in Customers)
                {
                    if (!string.IsNullOrEmpty(customer.Fullname))
                    {
                        name = name + customer.Fullname + "<br/>";
                    }
                }
                //}
                return name;
            }
        }

        public virtual string RoomName
        {
            get
            {
                string name = string.Empty;
                try
                {
                    Dictionary<string, int> rooms = new Dictionary<string, int>();
                    foreach (BookingRoom room in _bookosBookingRooms)
                    {
                        string key = room.RoomClass.Name + " " + room.RoomType.Name;
                        if (rooms.ContainsKey(key))
                        {
                            rooms[key] += 1;
                        }
                        else
                        {
                            rooms.Add(key, 1);
                        }
                    }

                    foreach (KeyValuePair<string, int> entry in rooms)
                    {
                        name += entry.Value + " " + entry.Key + "<br/>";
                    }
                }
                catch
                {
                }
                return name;
            }
        }

        public virtual string Confirmer
        {
            get
            {
                if (_confirmedBy != null)
                {
                    return _confirmedBy.FullName;
                }
                return "System";
            }
        }

        public virtual string Note
        {
            get { return _note; }
            set { _note = value; }
        }

        public virtual AgencyContact Booker
        {
            get { return _booker; }
            set { _booker = value; }
        }

        public virtual string BookerName
        {
            get
            {
                if (_booker != null)
                    return _booker.Name;
                return string.Empty;
            }
        }

        /// <summary>
        /// Trường đặc biệt, dùng để phân biệt booking tự add và booking do người khác book
        /// </summary>
        public virtual bool IsApproved
        {
            get { return _isApproved; }
            set { _isApproved = value; }
        }

        /// <summary>
        /// Xác định xem có phải booking cần payment ngay không
        /// </summary>
        public virtual bool IsPaymentNeeded
        {
            get { return _isPaymentNeeded; }
            set { _isPaymentNeeded = value; }
        }

        /// <summary>
        /// Giá trị doanh thu của booking, là total nếu approved, là penalty nếu cancelled, là 0 nếu khác
        /// Đơn vị tính: VND
        /// </summary>
        public virtual double Value
        {
            get
            {
                switch (_status)
                {
                    case StatusType.Approved:
                        return Total;
                    case StatusType.Cancelled:
                        return CancelPay;
                    default:
                        return 0;
                }
            }
        }

        public virtual double ValueVND
        {
            get
            {
                switch (_status)
                {
                    case StatusType.Approved:
                        return TotalVND;
                    case StatusType.Cancelled:
                        return CancelPayVND;
                    default:
                        return 0;
                }
            }
        }

        public virtual DateTime? LockDate { get; set; }

        public virtual User LockBy { get; set; }

        public virtual bool LockIncome
        {
            get
            {
                if (LockDate.HasValue && LockDate.Value < DateTime.Now)
                {
                    return true;
                }
                return false;
            }
        }

        public virtual bool UnlockIncome
        {
            get
            {
                if (LockDate.HasValue && LockDate.Value > DateTime.Now)
                {
                    return true;
                }
                return false;
            }
        }

        public virtual DateTime? PickupTime { get; set; }
        public virtual DateTime? SeeoffTime { get; set; }
        public virtual String PUFlightDetails { get; set; }
        public virtual String PUCarRequirements { get; set; }
        public virtual String SOFlightDetails { get; set; }
        public virtual String SOCarRequirements { get; set; }
        public virtual String PUPickupAddress { get; set; }
        public virtual String PUDropoffAddress { get; set; }
        public virtual String SOPickupAddress { get; set; }
        public virtual String SODropoffAddress { get; set; } 
        #endregion

        #region -- Function properties --
        public virtual DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }
        #endregion

        #region -- Methods --
        /// <summary>
        /// Tính giá booking
        /// </summary>
        /// <param name="Module"></param>
        /// <param name="agency"></param>
        /// <param name="childPrice"></param>
        /// <param name="agencySup"></param>
        /// <param name="customPrice"></param>
        /// <param name="singleService">Có bao gồm dịch vụ cá nhân không</param>
        /// <returns></returns>
        public virtual double Calculate(SailsModule Module, Agency agency, double childPrice, double agencySup, bool customPrice, bool singleService)
        {
            Role applyRole;
            if (agency != null && agency.Role != null)
            {
                applyRole = agency.Role;
            }
            else
            {
                applyRole = Module.RoleGetById(4);
            }
            double total = 0;
            #region -- Lấy danh sách chính sách giá --
            Role role;
            if (_agency != null && _agency.Role != null)
            {
                role = _agency.Role;
            }
            else
            {
                role = applyRole;
            }
            IList _policies = Module.AgencyPolicyGetByRole(role);
            #endregion

            #region -- Giá dịch vụ --

            IList services = Module.ExtraOptionGetBooking();
            IList servicePrices = Module.ServicePriceGetByBooking(this);

            foreach (ExtraOption extra in services)
            {
                double child = Child;
                double unitPrice = -1;
                foreach (BookingServicePrice price in servicePrices)
                {
                    if (price.ExtraOption == extra)
                    {
                        unitPrice = price.UnitPrice;
                    }
                }
                if (unitPrice < 0)
                {
                    unitPrice = Module.ApplyPriceFor(extra.Price, _policies);
                }

                if (extra.IsIncluded)
                {
                    // Nếu dịch vụ đã include thì xem xem có không check không để trừ
                    if (!_extraServices.Contains(extra))
                    {
                        total -= unitPrice * (Adult + child * childPrice / 100);
                    }
                }
                else
                {
                    // Nếu là dịch vụ chưa include thì xem có có không để cộng
                    if (_extraServices.Contains(extra))
                    {
                        total += unitPrice * (Adult + child * childPrice / 100);
                    }
                }
            }

            //TODO: Cần phải check cả dịch vụ dành cho từng khách nữa

            ////foreach (ExtraOption extra in _extraServices)
            ////{
            ////    if (services.Contains(extra))
            ////    {
            ////        total += Module.ApplyPriceFor(extra.Price, _policies)*Adult;
            ////    }
            ////}
            #endregion

            #region -- giá phòng --
            // Tính giá theo từng phòng
            IList cServices = Module.ExtraOptionGetCustomer();
            foreach (BookingRoom broom in _bookosBookingRooms)
            {
                if (customPrice)
                {
                    double tempTotal = 0;
                    foreach (Customer customer in broom.RealCustomers)
                    {
                        tempTotal += customer.Total;
                    }
                    if (tempTotal > 0)
                    {
                        total += tempTotal;
                    }
                    else
                    {
                        total += broom.Total;
                    }
                }
                else
                {
                    total += broom.Calculate(Module, _policies, childPrice, agencySup, _agency);
                }

                if (singleService)
                {
                    // Ngoài giá phòng còn có thể có dịch vụ cá nhân
                    foreach (Customer customer in broom.RealCustomers)
                    {
                        // Baby không tính dịch vụ
                        if (customer.Type == CustomerType.Baby)
                        {
                            continue;
                        }

                        foreach (ExtraOption service in cServices)
                        {
                            CustomerService customerService = Module.CustomerServiceGetByCustomerAndService(customer,
                                                                                                            service);
                            if (customerService != null)
                            {
                                double rate = 1;
                                //if (customer.IsChild)
                                //{
                                //    rate = childPrice / 100;
                                //}

                                double unitPrice = -1;
                                foreach (BookingServicePrice price in servicePrices)
                                {
                                    if (price.ExtraOption == service)
                                    {
                                        unitPrice = price.UnitPrice;
                                    }
                                }
                                if (unitPrice < 0)
                                {
                                    unitPrice = Module.ApplyPriceFor(service.Price, _policies);
                                }

                                if (service.IsIncluded && customerService.IsExcluded)
                                {
                                    // Nếu dịch vụ có included mà lại bị excluded thì trừ
                                    total -= unitPrice * rate;
                                }

                                if (!service.IsIncluded && !customerService.IsExcluded)
                                {
                                    // Nếu dịch vụ không included mà lại có thì cộng
                                    total += unitPrice * rate;
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            return total;
        }

        /// <summary>
        /// Tính giá của booking, không bao gồm dịch vụ cho từng cá nhân và theo tổng giá phòng cho trước
        /// </summary>
        /// <param name="Module"></param>
        /// <param name="agency"></param>
        /// <param name="childPrice"></param>
        /// <param name="agencySup"></param>
        /// <param name="customPrice"></param>
        /// <param name="roomTotal"></param>
        /// <returns></returns>
        public virtual double Calculate(SailsModule Module, Agency agency, double childPrice, double agencySup, bool customPrice, double roomTotal)
        {
            Role applyRole;
            if (agency != null && agency.Role != null)
            {
                applyRole = agency.Role;
            }
            else
            {
                applyRole = Module.RoleGetById(4);
            }
            double total = roomTotal;
            #region -- Lấy danh sách chính sách giá --
            Role role;
            if (_agency != null && _agency.Role != null)
            {
                role = _agency.Role;
            }
            else
            {
                role = applyRole;
            }
            IList _policies = Module.AgencyPolicyGetByRole(role);
            #endregion

            #region -- Giá dịch vụ --

            IList services = Module.ExtraOptionGetBooking();
            IList servicePrices = Module.ServicePriceGetByBooking(this);

            foreach (ExtraOption extra in services)
            {
                double child = Child;

                double unitPrice = -1;
                // Với mỗi dịch vụ ưu tiên lấy giá nhập trước
                foreach (BookingServicePrice price in servicePrices)
                {
                    if (price.ExtraOption == extra)
                    {
                        unitPrice = price.UnitPrice;
                    }
                }

                if (unitPrice < 0)
                {
                    unitPrice = Module.ApplyPriceFor(extra.Price, _policies);
                }

                if (extra.IsIncluded)
                {
                    // Nếu dịch vụ đã include thì xem xem có không check không để trừ
                    if (!_extraServices.Contains(extra))
                    {
                        total -= unitPrice * (Adult + child * childPrice / 100);
                    }
                }
                else
                {
                    // Nếu là dịch vụ chưa include thì xem có có không để cộng
                    if (_extraServices.Contains(extra))
                    {
                        total += unitPrice * (Adult + child * childPrice / 100);
                    }
                }
            }

            //TODO: Cần phải check cả dịch vụ dành cho từng khách nữa

            #endregion

            return total;
        }

        /// <summary>
        /// Tính giá từng dịch vụ sử dụng cho Booking hiện tại
        /// </summary>
        /// <param name="table">Bảng giá áp dụng</param>
        /// <param name="costTypes">Danh sách các dịch vụ cần tính</param>        
        /// <returns></returns>
        public virtual Dictionary<CostType, double> Cost(CostingTable table, IList costTypes)
        {
            Dictionary<CostType, double> serviceTotal = new Dictionary<CostType, double>();
            foreach (CostType type in costTypes)
            {
                serviceTotal.Add(type, 0);
            }

            // Lấy bảng giá theo dịch vụ
            Dictionary<CostType, Costing> map = table.GetCostMap(costTypes);
            foreach (CostType type in costTypes)
            {
                // Tính lại giá với các dịch vụ cố định, bắt buộc tính (không phải dịch vụ do khách lựa chọn)
                if (!type.IsDailyInput && type.Service == null)
                {
                    if (map.ContainsKey(type))
                    {
                        // Giá dịch vụ = tổng số child, tổng số baby, tổng số adult
                        serviceTotal[type] = map[type].Adult * Adult + map[type].Child * Child + map[type].Baby * Baby;
                    }
                }

                if (!type.IsDailyInput && type.Service != null) // Nếu không nhập thủ công, và có dịch vụ đi kèm
                {
                    if (!map.ContainsKey(type))
                    {
                        throw new Exception("Price setting is not completed:" + type.Name);
                    }
                    // Nếu là giá tương ứng với dịch vụ thì phải xem lại
                    // Nếu là dịch vụ cho từng khách
                    if (type.Service.Target == ServiceTarget.Customer)
                    {
                        foreach (BookingRoom bkroom in BookingRooms)
                        {
                            // Workaround: nếu là khách thứ hai và là single thì bỏ qua
                            int _index = 0;
                            foreach (Customer customer in bkroom.Customers)
                            {
                                _index++;
                                if (bkroom.IsSingle && _index == 2)
                                {
                                    continue;
                                }
                                if (customer.CustomerExtraOptions.Contains(type.Service))
                                {
                                    if (customer.IsChild)
                                    {
                                        serviceTotal[type] += map[type].Child;
                                        continue;
                                    }
                                    switch (customer.Type)
                                    {
                                        case CustomerType.Adult:
                                            serviceTotal[type] += map[type].Adult;
                                            break;
                                        case CustomerType.Children:
                                            serviceTotal[type] += map[type].Child;
                                            break;
                                        case CustomerType.Baby:
                                            serviceTotal[type] += map[type].Baby;
                                            break;
                                    }
                                }
                            }
                        }
                    }

                    // Nếu là dịch vụ cho cả book
                    if (type.Service.Target == ServiceTarget.Booking)
                    {
                        serviceTotal[type] = map[type].Adult * Adult + map[type].Child * Child + map[type].Baby * Baby;
                    }
                }
            }

            return serviceTotal;
        }

        #endregion
    }

    public enum AccountingStatus
    {
        New,
        Modified,
        Updated
    }
    #endregion
}