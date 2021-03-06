using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using GemBox.Spreadsheet;
using log4net.Config;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using log4net;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.ReportEngine;
using Portal.Modules.OrientalSails.Web.Controls;
using Portal.Modules.OrientalSails.Web.UI;
using Portal.Modules.OrientalSails.Web.Util;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class BookingView : SailsAdminBasePage, ICallbackEventHandler
    {
        #region -- Private Member --

        private readonly ILog _logger = LogManager.GetLogger(typeof(BookingView));
        private string ByCustomerId = string.Empty;
        private Booking _booking;

        private IList _extraServices;

        private int BookingId
        {
            get
            {
                int id;
                if (Request.QueryString["BookingId"] != null && Int32.TryParse(Request.QueryString["BookingId"], out id))
                {
                    return id;
                }
                return -1;
            }
        }

        protected IList ExtraServices
        {
            get
            {
                if (_extraServices == null)
                {
                    _extraServices = Module.ExtraOptionGetAll();
                }
                return _extraServices;
            }
        }

        #endregion

        #region -- Page Event --

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // tạo ra đoạn mã lấy giá trị của đối tượng control txtTaxCode trên JavaScript
                string cbReference = Page.ClientScript.GetCallbackEventReference(this, string.Format("getDropValue('{0}')", cddlBooker.ClientID), "GetBookerPhone",
                                                                                 "null", "GetBookerPhone", true);

                string cbScript = "function UseCallback(arg,context)" + "{" + cbReference + ";" + "}";
                // Đăng ký phương thức vừa tạo
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "UseCallback", cbScript, true);
                cddlBooker.Attributes.Add("onchange", "Validate();");
                cddlTrips.Visible = TripBased;
                plhAccountStatus.Visible = CheckAccountStatus;
                plhCharter.Visible = CheckCharter;
                plhServices.Visible = !DetailService;
                aServices.Visible = !DetailService;
                plhEmo.Visible = !DetailService;
                plhDetailService.Visible = DetailService;


                IList list = Module.ExtraOptionGetCustomer();
                foreach (ExtraOption option in list)
                {
                    ByCustomerId += "#" + option.Id + "#";
                }

                string vids = string.Empty;
                foreach (SailsTrip trip in Module.TripGetAll(false, UserIdentity))
                {
                    if (trip.NumberOfOptions == 2)
                    {
                        vids += "#" + trip.Id + "#";
                    }
                }
                cddlTrips.Attributes.Add("onChange",
                                        string.Format("ddltype_changed('{0}','{1}','{2}')", cddlTrips.ClientID,
                                                      ddlOptions.ClientID, vids));
                ddlAgencies.Attributes.Add("onChange",
                                           string.Format("ddlagency_changed('{0}','{1}')", ddlAgencies.ClientID,
                                                         txtAgencyCode.ClientID));
                if (BookingId <= 0)
                {
                    PageRedirect(string.Format("BookingList.aspx?NodeId={0}&SectionId={1}", Node.Id, Section.Id));
                    return;
                }

                string stayUrl = string.Format("StayDetail.aspx?NodeId={0}&SectionId={1}&bookingid={2}", Node.Id,
                                               Section.Id,
                                               BookingId);
                btnProvisionalDetail.Attributes.Add("onclick", string.Format("window.location='{0}';", stayUrl));
                btnProvisionalDetail.Value = Resources.textProvisionalDetail;
                Title = Resources.titleBookingView;

                #region -- Kiểm tra xem sử dụng custom booking id hay system booking id --

                if (UseCustomBookingId)
                {
                    label_BookingId.Visible = false;
                    txtBookingId.Visible = true;
                }
                else
                {
                    txtBookingId.Visible = false;
                    label_BookingId.Visible = true;
                }

                #endregion

                if (!IsPostBack)
                {
                    ddlStatusType.DataSource = Enum.GetNames(typeof(StatusType));
                    ddlStatusType.DataBind();

                    #region -- Bind agency --

                    ddlAgencies.DataSource = Module.AgencyGetAll();
                    ddlAgencies.DataTextField = "Name";
                    ddlAgencies.DataValueField = "Id";
                    ddlAgencies.DataBind();
                    ddlAgencies.Items.Insert(0, "-- Agency --");

                    cddlBooker.DataSource = Module.ContactGetAllEnabled();
                    cddlBooker.DataTextField = "Name";
                    cddlBooker.DataValueField = "Id";
                    cddlBooker.DataParentField = "AgencyId";
                    cddlBooker.ParentClientID = ddlAgencies.ClientID;
                    cddlBooker.DataBind();
                    cddlBooker.Items.Insert(0, "-- Contact --");

                    #endregion

                    BindTrips();

                    LoadInfo();

                    // Nếu không còn bất kỳ phòng trống nào thì ẩn khung add phòng
                    if (ddlRoomTypes.Items.Count <= 0)
                    {
                        //plhAddRoom.Visible = false;
                    }

                    #region -- Email popup --

                    const string centerPopup =
                        @"function PopupCenter(pageURL, title,h,w) {
var left = (screen.width/2)-(w/2);
var top = (screen.height/2)-(h/2);
var targetWin = window.open (pageURL, title, 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width='+w+', height='+h+', top='+top+', left='+left);
targetWin.focus();
return targetWin;
}";
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(BookingView), "centerPopup", centerPopup, true);

                    string url = string.Format("SendEmail.aspx?NodeId={0}&SectionId={1}&BookId={2}",
                                               Node.Id, Section.Id, BookingId);
                    btnEmail.Attributes.Add("onclick", string.Format("PopupCenter('{0}','Send email',800,600);", url));
                    #endregion
                }

                string servicesUrl = string.Format("BookingServicePrices.aspx?NodeId={0}&SectionId={1}&bookingid={2}",
                                                   Node.Id, Section.Id, BookingId);
                aServices.Attributes.Add("onclick", string.Format("PopupCenter('{0}','',300,400)", servicesUrl));

                string bhurl = string.Format("BookingHistories.aspx?NodeId={0}&SectionId={1}&BookingId={2}",
                                               Node.Id, Section.Id, BookingId);
                btnViewHistory.Attributes.Add("onclick", string.Format("window.location = '{0}'; return false;", bhurl));
            }
            catch (Exception ex)
            {
                _logger.Error("Error when Page_Load in BookingView", ex);
                ShowError(ex.Message);
            }
        }

        #endregion

        #region -- Private Method --

        protected void BindTrips()
        {
            //ddlTrips.DataSource = Module.TripGetAll(false, UserIdentity);
            //ddlTrips.DataTextField = "Name";
            //ddlTrips.DataValueField = "Id";
            //ddlTrips.DataBind();

            ddlOrgs.DataSource = Module.OrganizationGetAllRoot();
            ddlOrgs.DataTextField = "Name";
            ddlOrgs.DataValueField = "Id";
            ddlOrgs.DataBind();

            cddlTrips.DataSource = Module.TripGetAll(false, UserIdentity);
            cddlTrips.DataTextField = "Name";
            cddlTrips.DataValueField = "Id";
            cddlTrips.DataParentField = "OrgId";
            cddlTrips.ParentClientID = ddlOrgs.ClientID;
            cddlTrips.DataBind();
        }

        private void LoadInfo()
        {
            if (BookingId > 0)
            {
                _booking = Module.BookingGetById(BookingId);
                if (_booking.SeeoffTime != null)
                    txtSeeoffTime.Text = _booking.SeeoffTime.Value.ToString("dd/MM/yyyy HH:mm");
                if (_booking.PickupTime != null)
                    txtPickupTime.Text = _booking.PickupTime.Value.ToString("dd/MM/yyyy HH:mm");
                txtPUFlightDetails.Text = _booking.PUFlightDetails;
                txtPUCarRequirements.Text = _booking.PUCarRequirements;

                txtSOFlightDetails.Text = _booking.SOFlightDetails;
                txtSOCarRequirements.Text = _booking.SOCarRequirements;
                txtPUPickupAddress.Text = _booking.PUPickupAddress;
                txtPUDropoffAddress.Text = _booking.PUDropoffAddress;
                txtSOPickupAddress.Text = _booking.SOPickupAddress;
                txtSODropoffAddress.Text = _booking.SODropoffAddress;

                if (_booking.Total == 0)
                {
                    double total = _booking.Calculate(Module, null, ChildPrice,
                                                      Convert.ToDouble(Module.ModuleSettings("AgencySupplement")),
                                                      CustomPriceForRoom, true);

                    _booking.Total = total;
                }

                #region -- Booking total, lock income --
                // Kiểm tra lock income, ưu tiên kiểm tra booking trước vì dữ liệu đã có
                if (_booking.LockIncome)
                {
                    #region -- Lock income --

                    txtTotal.Text = _booking.Total.ToString("#,0.#");
                    txtTotal.CssClass += " locked";
                    if (!Module.PermissionCheck(Permission.ACTION_EDIT_AFTER_LOCK, UserIdentity))
                    {
                        txtTotal.ReadOnly = true;
                    }

                    btnLockIncome.Visible = false;
                    litLockIncome.Visible = true;
                    imgLockIncome.Visible = true;

                    litLockIncome.Text =
                        string.Format(
                            "Locked (individual booking) by {0} at {1:dd/MM/yyyy HH:mm}", _booking.LockBy.UserName,
                            _booking.LockDate.Value);

                    if (Module.PermissionCheck(Permission.ACTION_LOCKINCOME, UserIdentity))
                    {
                        btnUnlockIncome.Visible = true;
                    }

                    #endregion
                }
                else
                {
                    var expense = Module.ExpenseGetByDate(_booking.Trip, _booking.StartDate);
                    if (expense.LockIncome && !_booking.UnlockIncome)
                    {
                        #region -- Lock income --

                        txtTotal.Text = _booking.Total.ToString("#,0.#");
                        txtTotal.CssClass += " locked";
                        if (!Module.PermissionCheck(Permission.ACTION_EDIT_AFTER_LOCK, UserIdentity))
                        {
                            txtTotal.ReadOnly = true;
                        }

                        btnLockIncome.Visible = false;
                        litLockIncome.Visible = true;
                        imgLockIncome.Visible = true;
                        //if (expense.LockDate.HasValue)
                        //    litLockIncome.Text = string.Format("Locked (cruise) by {0} at {1:dd/MM/yyyy HH:mm}", expense.LockBy.FullName,
                        //                                       expense.LockDate.Value);

                        if (Module.PermissionCheck(Permission.ACTION_LOCKINCOME, UserIdentity))
                        {
                            btnUnlockIncome.Visible = true;
                        }
                        #endregion
                    }
                    else
                    {
                        #region -- Load income --
                        if (_booking.Total == 0)
                        {
                            double total = _booking.Calculate(Module, null, ChildPrice,
                                                              Convert.ToDouble(
                                                                  Module.ModuleSettings("AgencySupplement")),
                                                              CustomPriceForRoom, true);

                            _booking.Total = total;
                        }
                        txtTotal.Text = _booking.Total.ToString("#,0.#");

                        btnLockIncome.Visible = Module.PermissionCheck(Permission.ACTION_LOCKINCOME, UserIdentity);

                        if (!Module.PermissionCheck(Permission.ACTION_EDIT_TOTAL, UserIdentity))
                        {
                            txtTotal.ReadOnly = true;
                        }

                        #endregion
                    }
                }
                #endregion
                if (!Module.PermissionCheck(Permission.INPUT_DRIVER_COLLECT, UserIdentity))
                {
                    txtDriverCollect.ReadOnly = true;
                }


                #region -- Booking info --

                label_BookingId.Text = string.Format(BookingFormat, _booking.Id);
                if (_booking.Trip != null)
                {
                    if (_booking.Trip.Organization != null)
                    {
                        ddlOrgs.SelectedValue = _booking.Trip.Organization.Id.ToString();
                    }
                    cddlTrips.SelectedValue = _booking.Trip.Id.ToString();
                    if (_booking.Trip.NumberOfDay <= 2)
                    {
                        ddlOptions.Attributes.CssStyle["display"] = "none";
                    }
                }

                if (_booking.Agency == null)
                {
                    txtAgencyCode.Attributes.CssStyle["display"] = "none";
                }
                else
                {
                    txtAgencyCode.Text = _booking.AgencyCode;
                }

                //if (_booking.Sale != null)
                //{
                //    hyperLink_Sale.Text = _booking.Sale.FullName;
                //}
                hplBookingDate.Text = _booking.CreatedDate.ToString("dd/MM/yyyy");
                //hplBookingDate.NavigateUrl = string.Format("BookingReport.aspx?NodeId={0}&SectionId={1}&Date={2}", Node.Id, Section.Id, hplB)

                if (_booking.CreatedBy != null)
                {
                    litCreated.Text = _booking.CreatedBy.FullName;
                }
                if (_booking.ConfirmedBy != null)
                {
                    litApprovedBy.Text = _booking.ConfirmedBy.FullName;
                }

                txtStartDate.Text = _booking.StartDate.ToString("dd/MM/yyyy");
                ddlStatusType.SelectedValue = Enum.GetName(typeof(StatusType), _booking.Status);

                if (_booking.Booker != null)
                {
                    cddlBooker.SelectedValue = _booking.Booker.Id.ToString();
                    labelTelephone.Text = _booking.Booker.Phone;
                }

                chkIsPaymentNeeded.Checked = _booking.IsPaymentNeeded;
                txtBookingId.Text = _booking.CustomBookingId.ToString();

                #region -- Chuyển qua tàu khác --

                chkIsTransferred.Checked = _booking.IsTransferred;
                txtTransferCost.Text = _booking.TransferCost.ToString();

                chkIsTransferred.Attributes.Add("onclick",
                                                string.Format("toggleVisible('{0}');", trTransferCustomer.ClientID));
                if (!chkIsTransferred.Checked)
                {
                    trTransferCustomer.Style[HtmlTextWriterStyle.Display] = "none";
                }
                if (_booking.TransferAdult > _booking.Adult)
                {
                    txtTransferAdult.Text = _booking.TransferAdult.ToString();
                }
                else
                {
                    txtTransferAdult.Text = _booking.Adult.ToString();
                }

                if (_booking.TransferChildren > _booking.Child)
                {
                    txtTransferChildren.Text = _booking.TransferChildren.ToString();
                }
                else
                {
                    txtTransferChildren.Text = _booking.Child.ToString();
                }

                if (_booking.TransferBaby > _booking.Baby)
                {
                    txtTransferBaby.Text = _booking.TransferBaby.ToString();
                }
                else
                {
                    txtTransferBaby.Text = _booking.Baby.ToString();
                }

                agencyTransferTo.SelectedAgency = _booking.TransferTo;

                #endregion

                txtCustomerInfo.Text = _booking.Note;

                #region -- Khu vực add thêm phòng --

                ddlRoomTypes.Items.Clear();
                // Lấy về các loại phòng có tồn tại (còn trống)
                foreach (RoomClass rclass in AllRoomClasses)
                {
                    foreach (RoomTypex rtype in AllRoomTypes)
                    {
                        int avaiable;
                        if (TripBased)
                        {
                            avaiable = Module.RoomCount(rclass, rtype, _booking.Cruise, _booking.StartDate,
                                                        _booking.Trip.NumberOfDay);
                        }
                        else
                        {
                            avaiable = Module.RoomCount(rclass, rtype, _booking.Cruise, _booking.StartDate,
                                                        (_booking.EndDate - _booking.StartDate).Days);
                        }
                        if (avaiable > 0)
                        {
                            ddlRoomTypes.Items.Add(new ListItem(rtype.Name + " " + rclass.Name,
                                                                rclass.Id + "|" + rtype.Id));
                        }
                    }
                }

                #endregion

                rptCustomers.DataSource = _booking.Customers;
                rptCustomers.DataBind();

                // Số khách - chi tiết số khách
                litPax.Text = _booking.Pax.ToString();
                litPaxDetail.Text = string.Format(Resources.formatPaxDetail, _booking.Adult, _booking.Child,
                                                  _booking.Baby);


                if (_booking.Agency != null)
                {
                    ddlAgencies.SelectedValue = _booking.Agency.Id.ToString();

                    if (string.IsNullOrEmpty(_booking.PickupAddress))
                    {
                        string defAdd = string.Format("Address: {0}, Tel: {1}", _booking.Agency.Address,
                                                      _booking.Agency.Phone);
                        _booking.PickupAddress = defAdd;
                    }
                }

                ddlOptions.SelectedIndex = (int)_booking.TripOption;

                if (_booking.IsVND)
                {
                    txtTotal.Text = _booking.TotalVND.ToString("#,0.#");
                    ddlCurrency.SelectedValue = "true";
                }
                else
                {
                    txtTotal.Text = _booking.Total.ToString("#,0.#");
                    ddlCurrency.SelectedValue = "false";
                }

                if (_booking.IsCommissionVND)
                {
                    txtCommission.Text = _booking.CommissionVND.ToString("#,0.#");
                    ddlCurrencyComission.SelectedValue = "true";
                }
                else
                {
                    txtCommission.Text = _booking.Commission.ToString("#,0.#");
                    ddlCurrencyComission.SelectedValue = "false";
                }

                if (_booking.IsGuideCollectVND)
                {
                    txtGuideCollect.Text = _booking.GuideCollectVND.ToString("#,0.#");
                    ddlCurrencyGuideCollect.SelectedValue = "true";
                }
                else
                {
                    txtGuideCollect.Text = _booking.GuideCollect.ToString("#,0.#");
                    ddlCurrencyGuideCollect.SelectedValue = "false";
                }

                if (_booking.IsDriverCollectVND)
                {
                    txtDriverCollect.Text = _booking.DriverCollectVND.ToString("#,0.#");
                    ddlCurrencyDriverCollect.SelectedValue = "true";
                }
                else
                {
                    txtDriverCollect.Text = _booking.DriverCollect.ToString("#,0.#");
                    ddlCurrencyDriverCollect.SelectedValue = "false";
                }

                if (_booking.IsCancelPayVND)
                {
                    txtPenalty.Text = _booking.CancelPayVND.ToString("#,0.#");
                    ddlCurrencyCPenalty.SelectedValue = "true";
                }
                else
                {
                    txtPenalty.Text = _booking.CancelPay.ToString("#,0.#");
                    ddlCurrencyCPenalty.SelectedValue = "false";
                }

                txtPickup.Text = _booking.PickupAddress;
                txtSpecialRequest.Text = _booking.SpecialRequest;

                //txtGuide.Text = _booking.Guide;
                //txtDriver.Text = _booking.Driver;
                //chkGuideOnboard.Checked = _booking.GuideOnboard;

                #endregion

                if (_booking.AccountingStatus == AccountingStatus.Updated)
                {
                    btnAccountingUpdate.Text = Resources.textSetAsNotUpdated;
                }
                else
                {
                    btnAccountingUpdate.Text = Resources.textSetAsUpdated;
                }

                chkCharter.Checked = _booking.IsCharter;
                chkInvoice.Checked = _booking.HasInvoice;

                rptExtraServices.DataSource = Module.ExtraOptionGetBooking();
                rptExtraServices.DataBind();

                rptServices.DataSource = _booking.Services;
                rptServices.DataBind();

                #region -- Lock data --

                if (_booking.Status == StatusType.Approved && ApprovedLock)
                {
                    foreach (ListItem item in cddlTrips.Items)
                    {
                        if (!item.Selected) item.Enabled = false;
                    }
                    txtStartDate.ReadOnly = true;
                    calendarDate.Enabled = false;
                    foreach (ListItem item in ddlAgencies.Items)
                    {
                        if (!item.Selected) item.Enabled = false;
                    }
                    foreach (ListItem item in cddlBooker.Items)
                    {
                        if (!item.Selected) item.Enabled = false;
                    }
                    txtTotal.ReadOnly = true;
                    lbtCalculate.Enabled = false;
                    chkIsPaymentNeeded.Enabled = false;
                    chkInvoice.Enabled = false;
                    txtPenalty.ReadOnly = true;
                    btnAddService.Visible = false;
                    chkCharter.Enabled = false;
                    chkIsTransferred.Enabled = false;
                    agencyTransferTo.Enabled = false;
                    txtTransferCost.ReadOnly = true;
                    txtTransferAdult.ReadOnly = true;
                    txtTransferChildren.ReadOnly = true;
                    txtTransferBaby.ReadOnly = true;
                    txtTransferCost.ReadOnly = true;
                    txtPickup.ReadOnly = true;
                    txtSpecialRequest.ReadOnly = true;
                    txtCustomerInfo.ReadOnly = true;
                    //plhAddRoom.Visible = false;
                }

                #endregion

                if (_booking.CreatedBy != null)
                {
                    litCreatedTime.Text = string.Format("Created by {0} at {1:dd/MM/yyyy HH:mm}", _booking.CreatedBy.UserName, _booking.CreatedDate);
                }

                if (_booking.ModifiedBy != null)
                {
                    litLastEdited.Text = string.Format("Last edited by {0} at {1:dd/MM/yyyy HH:mm}",
                                                       _booking.ModifiedBy.UserName, _booking.ModifiedDate);
                }
            }

            if (Request.QueryString["Notify"] != null)
            {
                if (Convert.ToInt32(Request.QueryString["Notify"]) == 0)
                {
                    chkSendMail.Checked = false;
                }
            }
        }

        #endregion

        #region -- Control Events --

        protected void rptRoomList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (CustomPriceForRoom)
            {
                IList policies;
                if (_booking.Agency != null && _booking.Agency.Role != null)
                {
                    policies = Module.AgencyPolicyGetByRole(_booking.Agency.Role);
                }
                else
                {
                    policies = Module.AgencyPolicyGetByRole(Module.RoleGetById(4));
                }
                CustomersInfo.rptRoomList_itemDataBound(sender, e, Module, CustomPriceForRoom, policies, this,
                                                        ddlRoomTypes.Items);
            }
            else
            {
                CustomersInfo.rptRoomList_itemDataBound(sender, e, Module, false, null, this, ddlRoomTypes.Items);
            }
        }

        protected void buttonCancel_Click(object sender, EventArgs e)
        {
            PageRedirect(string.Format("BookingList.aspx?NodeId={0}&SectionId={1}", Node.Id, Section.Id));
        }

        protected void buttonSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Module.PermissionCheck(Permission.ACTION_EDITBOOKING, UserIdentity))
                {
                    ShowError(Resources.textDeniedFunction);
                    return;
                }

                //if (string.IsNullOrEmpty(txtPickup.Text) && PuRequired)
                //{
                //    ShowError("Pick up address is required!");
                //    return;
                //}
                _booking = Module.BookingGetById(BookingId);
                try
                {
                    _booking.SeeoffTime = DateTime.ParseExact(Request.Form[txtSeeoffTime.UniqueID], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    _booking.SeeoffTime = null;
                }

                try
                {
                    _booking.PickupTime = DateTime.ParseExact(Request.Form[txtPickupTime.UniqueID], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    _booking.PickupTime = null;
                }

                _booking.PUFlightDetails = Request.Form[txtPUFlightDetails.UniqueID];

                _booking.PUCarRequirements = Request.Form[txtPUCarRequirements.UniqueID];

                _booking.SOFlightDetails = Request.Form[txtSOFlightDetails.UniqueID];

                _booking.SOCarRequirements = Request.Form[txtSOCarRequirements.UniqueID];

                _booking.PUPickupAddress = Request.Form[txtPUPickupAddress.UniqueID];

                _booking.PUDropoffAddress = Request.Form[txtPUDropoffAddress.UniqueID];

                _booking.SOPickupAddress = Request.Form[txtSOPickupAddress.UniqueID];

                _booking.SODropoffAddress = Request.Form[txtSODropoffAddress.UniqueID];

                #region -- Create track --

                // Tạo sẵn track và danh sách trống các thay đổi
                BookingTrack track = new BookingTrack();
                track.ModifiedDate = DateTime.Now;
                track.User = UserIdentity;
                track.Booking = _booking;

                IList changes = new ArrayList();

                #endregion

                #region -- Lưu thông tin book phòng --

                foreach (RepeaterItem item in rptCustomers.Items)
                {
                    #region -- Khách hàng --

                    CustomerInfoRowInput customerInfo1 = item.FindControl("customerData") as CustomerInfoRowInput;
                    if (customerInfo1 != null)
                    {
                        Customer customer1 = customerInfo1.NewCustomer(Module);
                        customer1.Booking = _booking;
                        Module.SaveOrUpdate(customer1);

                        #region -- Dịch vụ khách 1 --

                        Repeater rptService1 = item.FindControl("rptServices1") as Repeater;
                        if (rptService1 != null)
                        {
                            if (DetailService)
                            {
                                CustomerServiceRepeaterHandler.Save(rptService1, Module, customer1);
                            }
                        }

                        #endregion
                    }

                    #endregion
                }

                #endregion

                #region -- Status --

                bool isApproved = false; // Biến cờ: là true nếu booking này trước đó không phải là approved

                StatusType statusType = (StatusType)Enum.Parse(typeof(StatusType), ddlStatusType.SelectedValue);

                // Nếu có sự thay đổi status, lưu lại người thay đổi
                if (_booking.Status != statusType)
                {
                    BookingChanged change = new BookingChanged();
                    change.Parameter = string.Empty;
                    // Nếu chuyển từ trạng thái hủy sang trạng thái chấp nhận hoặc chờ
                    if (_booking.Status == StatusType.Rejected || _booking.Status == StatusType.Cancelled)
                    {
                        if (statusType == StatusType.Approved || statusType == StatusType.Pending)
                        {
                            change.Action = BookingAction.Approved;
                            change.Parameter = _booking.Total.ToString();
                            changes.Add(change);
                        }
                    }

                    // Nếu chuyển từ trạng thái chấp nhận hoặc chờ sang trạng thái hủy
                    if (_booking.Status == StatusType.Approved || _booking.Status == StatusType.Pending)
                    {
                        if (statusType == StatusType.Cancelled || statusType == StatusType.Rejected)
                        {
                            change.Action = BookingAction.Cancelled;
                            change.Parameter = _booking.Total.ToString();
                            changes.Add(change);
                        }
                    }

                    if (_booking.ConfirmedBy == null)
                    {
                        _booking.ConfirmedBy = UserIdentity;
                    }
                    _booking.Status = statusType;
                    // Nếu có sự thay đổi về status, và sự thay đổi đó theo chiều hướng Approved
                    if (_booking.Status == StatusType.Approved)
                    {
                        isApproved = true;
                    }
                }

                #endregion

                #region -- agency --

                if (ddlAgencies.SelectedIndex > 0)
                {
                    if (_booking.Agency == null || _booking.Agency.Id.ToString() != ddlAgencies.SelectedValue)
                    {
                        // Nếu thay đổi Agency
                        _booking.Agency = Module.AgencyGetById(Convert.ToInt32(ddlAgencies.SelectedValue));
                    }
                }
                else
                {
                    _booking.Agency = null;
                }

                #endregion

                #region -- Trip & day & check avaiable --

                DateTime date = DateTime.ParseExact(txtStartDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                // Nếu có sự thay đổi về ngày hoặc thay đổi về trip thì phải check lại số phòng trống
                // Bổ sung: không chỉ có sự thay đổi về ngày, cả thay đổi trạng thái về approved
                // Bổ sung: hoặc là có sự thay đổi về tàu

                SailsTrip trip = null;
                if (_booking.Trip != null && _booking.Trip.Id.ToString() != cddlTrips.SelectedValue ||
                    _booking.StartDate != date || isApproved)
                {
                    trip = Module.TripGetById(Convert.ToInt32(cddlTrips.SelectedValue));
                }

                // Nếu check avaiable không có vấn đề gì thì lưu lại thay đổi về hành trình và ngày
                if (TripBased)
                {
                    if (trip != null)
                    {
                        _booking.Trip = trip;
                    }
                }
                else
                {
                    _booking.Trip = null;
                }
                _booking.StartDate = date;

                #endregion

                #region -- Các thông tin khác về booking --

                _booking.PickupAddress = txtPickup.Text;

                // Kiểm tra nếu booking có address mặc định
                string defAdd = string.Empty;
                if (_booking.Agency != null)
                {
                    defAdd = string.Format("Address: {0}, Tel: {1}", _booking.Agency.Address,
                                           _booking.Agency.Phone);

                    if (string.IsNullOrEmpty(_booking.PickupAddress) || _booking.PickupAddress == defAdd)
                    {
                        if (string.IsNullOrEmpty(_booking.BookerName))
                        {
                            _booking.PickupAddress = string.Format("{0}, Tel: {1}", _booking.Agency.Address,
                                                                   _booking.Agency.Phone);
                        }
                        else
                        {
                            _booking.PickupAddress = string.Format("{0}, Tel: {1}, Booker: {2}",
                                                                   _booking.Agency.Address, _booking.Agency.Phone,
                                                                   _booking.BookerName);
                        }
                    }
                }

                _booking.SpecialRequest = txtSpecialRequest.Text;
                if (cddlBooker.SelectedIndex > 0)
                {
                    _booking.Booker = Module.ContactGetById(Convert.ToInt32(cddlBooker.SelectedValue));
                }

                if (TripBased)
                {
                    if (_booking.Trip.NumberOfDay == 3)
                    {
                        _booking.TripOption = (TripOption)ddlOptions.SelectedIndex;
                    }
                }

                _booking.AgencyCode = txtAgencyCode.Text;
                _booking.IsPaymentNeeded = chkIsPaymentNeeded.Checked;
                _booking.CustomBookingId = Convert.ToInt32(txtBookingId.Text.Replace(" ", ""));
                _booking.Note = txtCustomerInfo.Text;

                //txtGuide.Text = _booking.Guide;
                //txtDriver.Text = _booking.Driver;
                //chkGuideOnboard.Checked = _booking.GuideOnboard;
                #endregion

                #region -- Giá tự nhập, tính tự động và code tự quản lý, chỉ dành cho USD --
                // Lấy giá trị người dùng nhập (hoặc sẵn có)
                double total = !string.IsNullOrEmpty(txtTotal.Text) ? Convert.ToDouble(txtTotal.Text) : 0.0;
                double finalTotal = total;
                //if (ddlCurrency.SelectedValue == "false")
                //{
                //    // Nếu người dùng chưa gõ giá hoặc bằng 0 tính lại giá và lưu
                //    if (total <= 0)
                //    {
                //        try
                //        {
                //            finalTotal = _booking.Calculate(Module, null, ChildPrice,
                //                                            Convert.ToDouble(Module.ModuleSettings("AgencySupplement")),
                //                                            CustomPriceForRoom, true);
                //        }
                //        catch (Exception ex)
                //        {
                //            ShowError(Resources.errorCanNotCalculatePrice);
                //        }
                //    }
                //    else
                //    {
                //        // Nếu không lấy giá người dùng nhập
                //        finalTotal = total;
                //    }
                //}

                if (_booking.CurrencyRate == 0)
                {
                    var rate = Module.ExchangeGetByDate(_booking.CreatedDate);
                    _booking.CurrencyRate = rate.Rate;
                }

                if (ddlCurrency.SelectedValue == "true")
                {
                    _booking.IsVND = true;
                    if (_booking.TotalVND != finalTotal)
                    {
                        _booking.TotalVND = finalTotal;
                        _booking.Total = 0.0;
                        _booking.GuideConfirmed = false;
                        _booking.AgencyConfirmed = false;
                        _booking.IsPaid = false;
                    }
                }
                else
                {
                    _booking.IsVND = false;
                    if (_booking.Total != finalTotal)
                    {
                        _booking.Total = finalTotal;
                        _booking.TotalVND = 0.0;
                        _booking.GuideConfirmed = false;
                        _booking.AgencyConfirmed = false;
                        _booking.IsPaid = false;
                    }
                }

                if (ddlCurrencyComission.SelectedValue == "true")
                {
                    _booking.IsCommissionVND = true;
                    _booking.CommissionVND = !string.IsNullOrEmpty(txtCommission.Text) ? Convert.ToDouble(txtCommission.Text) : 0.0;
                    _booking.Commission = 0.0;
                }
                else
                {
                    _booking.IsCommissionVND = false;
                    _booking.Commission = !string.IsNullOrEmpty(txtCommission.Text) ? Convert.ToDouble(txtCommission.Text) : 0.0;
                    _booking.CommissionVND = 0.0;
                }

                if (ddlCurrencyCPenalty.SelectedValue == "true")
                {
                    _booking.IsCancelPayVND = true;
                    _booking.CancelPayVND = !string.IsNullOrEmpty(txtPenalty.Text) ? Convert.ToDouble(txtPenalty.Text) : 0.0;
                    _booking.CancelPay = 0.0;
                }
                else
                {
                    _booking.IsCancelPayVND = false;
                    _booking.CancelPay = !string.IsNullOrEmpty(txtPenalty.Text) ? Convert.ToDouble(txtPenalty.Text) : 0.0;
                    _booking.CancelPayVND = 0.0;
                }

                var guideCollect = !string.IsNullOrEmpty(txtGuideCollect.Text) ? Convert.ToDouble(txtGuideCollect.Text) : 0.0;
                if (ddlCurrencyGuideCollect.SelectedValue == "true")
                {
                    _booking.IsGuideCollectVND = true;
                    if (_booking.GuideCollectVND != guideCollect)
                    {
                        // Nếu thay đổi guide collect thì tương đương với thay đổi total booking --> not paid
                        _booking.IsPaid = false;
                        _booking.GuideCollectVND = guideCollect;
                        _booking.GuideCollect = 0.0;
                        _booking.GuideConfirmed = false;
                        _booking.AgencyConfirmed = false;
                    }
                }
                else
                {
                    _booking.IsGuideCollectVND = false;
                    if (_booking.GuideCollect != guideCollect)
                    {
                        // Nếu thay đổi guide collect thì tương đương với thay đổi total booking --> not paid
                        _booking.IsPaid = false;
                        _booking.GuideCollect = guideCollect;
                        _booking.GuideCollectVND = 0.0;
                        _booking.GuideConfirmed = false;
                        _booking.AgencyConfirmed = false;
                    }

                }

                var driverCollect = !string.IsNullOrEmpty(txtDriverCollect.Text) ? Convert.ToDouble(txtDriverCollect.Text) : 0.0;
                if (ddlCurrencyDriverCollect.SelectedValue == "true")
                {
                    _booking.IsDriverCollectVND = true;
                    if (_booking.DriverCollectVND != driverCollect)
                    {
                        // Nếu thay đổi guide collect thì tương đương với thay đổi total booking --> not paid
                        _booking.IsPaid = false;
                        _booking.DriverCollectVND = driverCollect;
                        _booking.DriverCollect = 0.0;
                        _booking.GuideConfirmed = false;
                        _booking.AgencyConfirmed = false;
                    }
                }
                else
                {
                    _booking.IsGuideCollectVND = false;
                    if (_booking.DriverCollect != driverCollect)
                    {
                        // Nếu thay đổi guide collect thì tương đương với thay đổi total booking --> not paid
                        _booking.IsPaid = false;
                        _booking.DriverCollect = driverCollect;
                        _booking.DriverCollectVND = 0.0;
                        _booking.GuideConfirmed = false;
                        _booking.AgencyConfirmed = false;
                    }

                }

                // Nếu sử dụng bk code tự quản lý và bk code không hợp lệ (đã trùng)
                if (UseCustomBookingId && !Module.CheckCustomBookingCode(_booking.CustomBookingId, _booking))
                {
                    ShowError(Resources.errorDuplicateBookingCode);
                    return;
                }

                #endregion

                #region -- Booking charter --

                Locked charter = null;

                #region -- Tạo khóa theo charter --

                if (_booking.Charter != null)
                {
                    charter = _booking.Charter;
                    // Nếu từ charter chuyển sang không charter hoặc không approve
                    if (!chkCharter.Checked || _booking.Status != StatusType.Approved)
                    {
                        Module.Delete(charter);
                        charter = null;
                    }
                }
                else
                {
                    // Nếu từ ko phải charter chuyển sang charter
                    if (chkCharter.Checked && _booking.Status == StatusType.Approved)
                    {
                        charter = new Locked();
                        charter.Charter = _booking;
                        charter.Cruise = _booking.Cruise;
                        charter.Description = "Booking charter";
                    }
                }

                if (charter != null)
                {
                    charter.Start = _booking.StartDate;
                    charter.End = _booking.EndDate.AddDays(-1);
                    Module.SaveOrUpdate(charter);
                }

                #endregion

                _booking.Charter = charter;
                _booking.IsCharter = chkCharter.Checked;

                #endregion

                #region -- Chuyển qua tàu khác --

                if (_booking.IsTransferred != chkIsTransferred.Checked)
                {
                    BookingChanged change = new BookingChanged();
                    // Nếu booking này đang chuyển, tức là nhận lại
                    if (_booking.IsTransferred)
                    {
                        // Nhận lại tức là thay đổi = giá trị cũ (không quan tâm giá trị mới vì nó đã thành vô nghĩa)
                        change.Action = BookingAction.Untransfer;
                        change.Parameter = string.Format("{0}", _booking.TransferCost);
                    }
                    else
                    {
                        // Chuyển đi tức là thay đổi = giá trị mới (giá trị chuyển đi) + không quan tâm giá trị cũ
                        change.Action = BookingAction.Transfer;
                        change.Parameter = string.Format("{0}", -Convert.ToDouble(txtTransferCost.Text));
                    }
                    changes.Add(change);
                }
                else if (_booking.IsTransferred)
                {
                    // Nếu không có sự thay đổi, và vẫn là chuyển thì căn cứ vào giá trị thay đổi
                    BookingChanged change = new BookingChanged();
                    change.Action = BookingAction.ChangeTransfer;
                    // Sự thay đổi tính bằng giá trị cũ - giá trị mới (do việc transfer được tính là âm)
                    change.Parameter = string.Format("{0}",
                                                     _booking.TransferCost - Convert.ToDouble(txtTransferCost.Text));
                    changes.Add(change);

                    // Nếu không có sự thay đổi và không phải chuyển qua tàu khác thì không cần tính gì cả
                }

                _booking.IsTransferred = chkIsTransferred.Checked;
                _booking.TransferCost = Convert.ToDouble(txtTransferCost.Text);

                // Số khách chuyển đi
                _booking.TransferAdult = Convert.ToInt32(txtTransferAdult.Text);
                _booking.TransferChildren = Convert.ToInt32(txtTransferChildren.Text);
                _booking.TransferBaby = Convert.ToInt32(txtTransferBaby.Text);

                _booking.TransferTo = agencyTransferTo.SelectedAgency;

                #endregion

                // Nếu có thông tin tracking được lưu, và đã approved trước đó, tăng amend
                // Đã được approved trước đó nghĩa là trạng thái hiện tại đang là approved nhưng không phải
                // mới approve trong lần lưu này
                if (changes.Count > 0 && !isApproved && _booking.Status == StatusType.Approved)
                {
                    _booking.Amended++;
                }

                if (!ApprovedLock || _booking.Status != StatusType.Approved || isApproved)
                {
                    _booking.ModifiedBy = UserIdentity;
                    _booking.ModifiedDate = DateTime.Now;
                    Module.Update(_booking, UserIdentity);
                }

                #region -- Lưu thông tin tracking --

                // Nếu số lượng thay đổi cần lưu lớn hơn 0
                if (changes.Count > 0)
                {
                    // Lưu track trước
                    Module.SaveOrUpdate(track);
                    foreach (BookingChanged change in changes)
                    {
                        change.Track = track;
                        Module.SaveOrUpdate(change);
                    }
                }

                #endregion

                #region -- Thông tin service --

                foreach (RepeaterItem item in rptExtraServices.Items)
                {
                    var hiddenId = (HiddenField)item.FindControl("hiddenId");
                    var chkService = (CheckBox)item.FindControl("chkService");
                    var option = Module.ExtraOptionGetById(Convert.ToInt32(hiddenId.Value));
                    var extra = Module.BookingExtraGet(_booking, option);
                    if (extra.Id <= 0 && chkService.Checked)
                    {
                        Module.SaveOrUpdate(extra);
                    }
                    if (extra.Id > 0 && !chkService.Checked)
                    {
                        Module.Delete(extra);
                    }
                }

                IList bkservice = rptServicesToIList(_booking);
                foreach (BookingService service in bkservice)
                {
                    if (!service.Deleted)
                    {
                        Module.SaveOrUpdate(service);
                    }
                    else
                    {
                        if (service.Id > 0)
                        {
                            Module.Delete(service);
                        }
                    }
                }

                #endregion

                PageRedirect(string.Format("BookingReport.aspx?NodeId={0}&SectionId={1}&Date={2}", Node.Id, Section.Id, _booking.StartDate.ToOADate()));
                //PageRedirect(Request.RawUrl);
            }
            catch (Exception ex)
            {
                _logger.Error("Error when buttonSubmit_Click in BookingView", ex);
                string str = string.Empty;
                Exception temp = ex;
                while (temp != null)
                {
                    str += temp.Message + "<br/>";
                    temp = temp.InnerException;
                }
                ShowError(str);
            }
        }

        protected void rptRoomList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName.ToLower())
            {
                case "delete":
                    BookingRoom booking = Module.BookingRoomGetById(Convert.ToInt32(e.CommandArgument));
                    Module.Delete(booking, UserIdentity);
                    PageRedirect(Request.RawUrl);
                    break;
            }
        }

        protected void btnAddRoom_Click(object sender, EventArgs e)
        {
            #region -- check available --

            //// Căn cứ vào room type và room class check available
            _booking = Module.BookingGetById(BookingId);

            //string[] values = ddlRoomTypes.SelectedValue.Split('|');

            //RoomClass rclass = Module.RoomClassGetById(Convert.ToInt32(values[0]));
            //RoomTypex rtype = Module.RoomTypexGetById(Convert.ToInt32(values[1]));

            //// Nếu đã là chuyển qua tàu khác, vậy thì không cần phải check theo lock
            //int available;
            //if (_booking.IsTransferred)
            //{
            //    if (TripBased)
            //    {
            //        available = Module.RoomCount(rclass, rtype, _booking.Cruise, _booking.StartDate,
            //                                     _booking.Trip.NumberOfDay, true);
            //    }
            //    else
            //    {
            //        available = Module.RoomCount(rclass, rtype, _booking.Cruise, _booking.StartDate,
            //                                     (_booking.EndDate - _booking.StartDate).Days, true);
            //    }
            //}
            //else
            //{
            //    if (TripBased)
            //    {
            //        available = Module.RoomCount(rclass, rtype, _booking.Cruise, _booking.StartDate, _booking.Trip.NumberOfDay);
            //    }
            //    else
            //    {
            //        available = Module.RoomCount(rclass, rtype, _booking.Cruise, _booking.StartDate, (_booking.EndDate - _booking.StartDate).Days);
            //    }
            //}
            //if (available <= 0)
            //{
            //    ShowError(Resources.errorNotEnoughAvailable);
            //    return;
            //}

            #endregion

            #region -- Add rooms to booking --

            //BookingRoom room = new BookingRoom();
            //room.RoomClass = rclass;
            //room.RoomType = rtype;
            //room.Book = _booking;
            //Module.SaveOrUpdate(room);
            Customer customer1 = new Customer();
            customer1.Booking = _booking;
            customer1.Type = CustomerType.Adult;
            Module.SaveOrUpdate(customer1);
            //Customer customer2 = new Customer();
            //customer2.Booking = _booking;
            //customer2.BookingRoom = room;
            //Module.SaveOrUpdate(customer2);

            //BookingTrack track = new BookingTrack();
            //track.Booking = _booking;
            //track.ModifiedDate = DateTime.Now;
            //track.User = UserIdentity;
            //Module.SaveOrUpdate(track);

            //BookingChanged change = new BookingChanged();
            //change.Action = BookingAction.AddRoom;
            //change.Parameter = ddlRoomTypes.SelectedItem.Text;
            //change.Track = track;
            //Module.SaveOrUpdate(change);

            //BookingChanged customerChange = new BookingChanged();
            //customerChange.Action = BookingAction.AddCustomer;
            //customerChange.Parameter = string.Format("{0}|{1}|{2}", 2, 0, 0);
            //customerChange.Track = track;
            //Module.SaveOrUpdate(customerChange);

            PageRedirect(Request.RawUrl);

            #endregion
        }

        protected void lbtCalculate_Click(object sender, EventArgs e)
        {
            _booking = Module.BookingGetById(BookingId);

            // Tính theo agency hiển thị chứ không tính theo agency trong CSDL
            // Phải lưu lại agency cũ
            Agency _oldAgency = null;
            if (ddlAgencies.SelectedIndex > 0)
            {
                _oldAgency = _booking.Agency;
                _booking.Agency = Module.AgencyGetById(Convert.ToInt32(ddlAgencies.SelectedValue));
            }

            double total;
            //if (CustomPriceForRoom)
            //{
            //    total = 0;

            //    // Giá này chưa bao gồm dịch vụ cho từng cá nhân
            //}
            //else
            //{
            //    total = _booking.Calculate(Module, null, ChildPrice,
            //                               Convert.ToDouble(Module.ModuleSettings("AgencySupplement")),
            //                               CustomPriceForRoom, false);
            //    // Giá này chưa bao gồm dịch vụ cho từng cá nhân
            //}

            //// Giá 

            //double extra = 0;
            //IList servicePrices = Module.ServicePriceGetByBooking(_booking);

            #region // Tính giá dịch vụ từng người

            //foreach (RepeaterItem item in rptRoomList.Items)
            //{
            //    BookingRoom bookingRoom = new BookingRoom();
            //    #region -- Lấy danh sách chính sách giá --
            //    Role role;
            //    if (_booking.Agency != null && _booking.Agency.Role != null)
            //    {
            //        role = _booking.Agency.Role;
            //    }
            //    else
            //    {
            //        role = null;
            //    }
            //    IList _policies = Module.AgencyPolicyGetByRole(role);
            //    #endregion

            //    #region -- Lấy thông tin khách hàng --
            //    CheckBox checkBoxAddChild = (CheckBox)item.FindControl("checkBoxAddChild");
            //    CheckBox checkBoxAddBaby = (CheckBox)item.FindControl("checkBoxAddBaby");
            //    CheckBox checkBoxSingle = (CheckBox)item.FindControl("checkBoxSingle");

            //    bookingRoom.HasBaby = checkBoxAddBaby.Checked;
            //    bookingRoom.HasChild = checkBoxAddChild.Checked;
            //    bookingRoom.IsSingle = checkBoxSingle.Checked;
            //    #endregion

            //    #region -- Khách hàng --

            //    Control trCustomer2 = item.FindControl("trCustomer2");

            //    #region -- Dịch vụ khách 1 --
            //    if (DetailService)
            //    {
            //        Repeater rptService1 = item.FindControl("rptServices1") as Repeater;
            //        if (rptService1 != null)
            //        {
            //            IList services = CustomerServiceRepeaterHandler.GetCustomerServiceData(rptService1, Module);
            //            const double rate = 1;
            //            foreach (CustomerService service in services)
            //            {
            //                double unitPrice = -1;
            //                foreach (BookingServicePrice price in servicePrices)
            //                {
            //                    if (price.ExtraOption == service.Service)
            //                    {
            //                        unitPrice = price.UnitPrice;
            //                    }
            //                }
            //                if (unitPrice < 0)
            //                {
            //                    unitPrice = Module.ApplyPriceFor(service.Service.Price, _policies);
            //                }

            //                if (service.Service.IsIncluded && service.IsExcluded)
            //                {
            //                    // Nếu dịch vụ có included mà lại bị excluded thì trừ
            //                    total -= unitPrice * rate;
            //                }

            //                if (!service.Service.IsIncluded && !service.IsExcluded)
            //                {
            //                    // Nếu dịch vụ không included mà lại có thì cộng
            //                    total += unitPrice * rate;
            //                }
            //            }
            //        }
            //    }
            //    #endregion

            //    // Nếu không phải phòng ở ghép, hoặc dòng client 2 được hiển thị
            //    // Tức là nếu ở ghép và dòng client 2 ẩn thì không lưu dòng 2
            //    if (trCustomer2.Visible)
            //    {
            //        if (DetailService)
            //        {
            //            #region -- Dịch vụ khách 2 --

            //            Repeater rptService2 = item.FindControl("rptServices2") as Repeater;
            //            if (rptService2 != null)
            //            {
            //                IList services = CustomerServiceRepeaterHandler.GetCustomerServiceData(rptService2, Module);
            //                foreach (CustomerService service in services)
            //                {
            //                    double rate = 1;
            //                    double unitPrice = -1;
            //                    foreach (BookingServicePrice price in servicePrices)
            //                    {
            //                        if (price.ExtraOption == service.Service)
            //                        {
            //                            unitPrice = price.UnitPrice;
            //                        }
            //                    }
            //                    if (unitPrice < 0)
            //                    {
            //                        unitPrice = Module.ApplyPriceFor(service.Service.Price, _policies);
            //                    }

            //                    if (service.Service.IsIncluded && service.IsExcluded)
            //                    {
            //                        // Nếu dịch vụ có included mà lại bị excluded thì trừ
            //                        total -= unitPrice * rate;
            //                    }

            //                    if (!service.Service.IsIncluded && !service.IsExcluded)
            //                    {
            //                        // Nếu dịch vụ không included mà lại có thì cộng
            //                        total += unitPrice * rate;
            //                    }
            //                }
            //            }

            //            #endregion
            //        }
            //    }

            //    CustomerInfoInput customerChild = (CustomerInfoInput)item.FindControl("customerChild");
            //    Customer child = customerChild.NewCustomer(Module);
            //    if (checkBoxAddChild.Checked)
            //    {
            //        if (DetailService)
            //        {
            //            #region -- Dịch vụ khách trẻ em --

            //            Repeater rptServicesChild = item.FindControl("rptServicesChild") as Repeater;
            //            if (rptServicesChild != null)
            //            {
            //                IList services = CustomerServiceRepeaterHandler.GetCustomerServiceData(rptServicesChild,
            //                                                                                       Module);
            //                foreach (CustomerService service in services)
            //                {
            //                    double rate = 1;
            //                    double unitPrice = -1;
            //                    foreach (BookingServicePrice price in servicePrices)
            //                    {
            //                        if (price.ExtraOption == service.Service)
            //                        {
            //                            unitPrice = price.UnitPrice;
            //                        }
            //                    }
            //                    if (unitPrice < 0)
            //                    {
            //                        unitPrice = Module.ApplyPriceFor(service.Service.Price, _policies);
            //                    }

            //                    if (service.Service.IsIncluded && service.IsExcluded)
            //                    {
            //                        // Nếu dịch vụ có included mà lại bị excluded thì trừ
            //                        total -= unitPrice * rate;
            //                    }

            //                    if (!service.Service.IsIncluded && !service.IsExcluded)
            //                    {
            //                        // Nếu dịch vụ không included mà lại có thì cộng
            //                        total += unitPrice * rate;
            //                    }
            //                }
            //            }

            //            #endregion
            //        }
            //    }
            //    else
            //    {
            //        if (child.Id > 0)
            //        {
            //            Module.Delete(child);
            //        }
            //    }

            //    CustomerInfoInput customerBaby = (CustomerInfoInput)item.FindControl("customerBaby");
            //    Customer baby = customerBaby.NewCustomer(Module);
            //    if (checkBoxAddBaby.Checked)
            //    {
            //        if (DetailService)
            //        {
            //            #region -- Dịch vụ khách trẻ sơ sinh --
            //            Repeater rptServicesBaby = item.FindControl("rptServicesBaby") as Repeater;
            //            if (rptServicesBaby != null)
            //            {
            //                IList services = CustomerServiceRepeaterHandler.GetCustomerServiceData(rptServicesBaby, Module);
            //                foreach (CustomerService service in services)
            //                {
            //                    double rate = 1;
            //                    double unitPrice = -1;
            //                    foreach (BookingServicePrice price in servicePrices)
            //                    {
            //                        if (price.ExtraOption == service.Service)
            //                        {
            //                            unitPrice = price.UnitPrice;
            //                        }
            //                    }
            //                    if (unitPrice < 0)
            //                    {
            //                        unitPrice = Module.ApplyPriceFor(service.Service.Price, _policies);
            //                    }

            //                    if (service.Service.IsIncluded && service.IsExcluded)
            //                    {
            //                        // Nếu dịch vụ có included mà lại bị excluded thì trừ
            //                        total -= unitPrice * rate;
            //                    }

            //                    if (!service.Service.IsIncluded && !service.IsExcluded)
            //                    {
            //                        // Nếu dịch vụ không included mà lại có thì cộng
            //                        total += unitPrice * rate;
            //                    }
            //                }
            //            }
            //            #endregion
            //        }
            //    }
            //    else
            //    {
            //        if (baby.Id > 0)
            //        {
            //            try
            //            {
            //                Module.Delete(baby);
            //            }
            //            catch
            //            {

            //            }
            //        }
            //    }

            //    #endregion
            //}

            #endregion

            //foreach (BookingService service in rptServicesToIList(_booking))
            //{
            //    total += service.UnitPrice*service.Quantity;
            //}


            Domain.SailsTrip sailsTrip = Module.TripGetById(Int32.Parse(cddlTrips.SelectedValue));
            Domain.SailsPriceConfig sailsPriceConfig = Module.SailsPriceConfigGetByNearestValidFrom(sailsTrip, _booking.TripOption, _booking.CreatedDate);

            int adult = 0;
            int child = 0;
            int baby = 0;

            foreach (RepeaterItem item in rptCustomers.Items)
            {
                var customerData = item.FindControl("customerData") as CustomerInfoRowInput;
                var ddlCustomer = customerData.FindControl("ddlCustomerType") as DropDownList;

                switch (ddlCustomer.SelectedItem.Text)
                {
                    case "Adult":
                        adult += 1;
                        break;
                    case "Child":
                        child += 1;
                        break;
                    case "Baby":
                        baby += 1;
                        break;
                    default:
                        break;
                }

            }

            if (ddlCurrency.SelectedValue == "false")
                total = adult * sailsPriceConfig.PriceAdultUSD + child * sailsPriceConfig.PriceChildUSD + baby * sailsPriceConfig.PriceBabyUSD;
            else
                total = adult * sailsPriceConfig.PriceAdultVND + child * sailsPriceConfig.PriceChildVND + baby * sailsPriceConfig.PriceBabyVND;
            txtTotal.Text = total.ToString("#,0.#");

            Agency agency = Module.AgencyGetById(Convert.ToInt32(ddlAgencies.SelectedValue));
            AgencyLevel agencyLevel = agency.AgencyLevel;
            SailsTrip trip = Module.TripGetById(Convert.ToInt32(cddlTrips.SelectedValue));

            double totalCommission = 0.0;
            Domain.AgencyCommission agencyCommission = Module.AgencyCommissionGetByNearestValidFrom(sailsTrip, agencyLevel, _booking.CreatedDate);
            if (ddlCurrency.SelectedValue == "false")
                totalCommission = adult * agencyCommission.CommissionAdultUSD + child * agencyCommission.CommissionChildUSD + baby * agencyCommission.CommissionChildUSD;
            else
                totalCommission = adult * agencyCommission.CommissionAdultVND + child * agencyCommission.CommissionChildVND + baby * agencyCommission.CommissionBabyVND;
            txtCommission.Text = totalCommission.ToString("#,0.#");
            // Sau khi tính xong hoàn trả lại agency như cũ (trong trường hợp người dùng không lưu)
            if (ddlAgencies.SelectedIndex > 0)
            {
                _booking.Agency = _oldAgency;
            }
        }

        protected void rptExtraServices_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            CheckBox chkService = (CheckBox)e.Item.FindControl("chkService");
            ExtraOption option = (ExtraOption)e.Item.DataItem;
            chkService.Checked = _booking.ExtraServices.Contains(option);
            chkService.Text = option.Name;
        }

        protected void btnAccountingUpdate_Click(object sender, EventArgs e)
        {
            _booking = Module.BookingGetById(BookingId);
            if (btnAccountingUpdate.Text == Resources.textSetAsUpdated)
            {
                _booking.AccountingStatus = AccountingStatus.Updated;
                btnAccountingUpdate.Text = Resources.textSetAsNotUpdated;
            }
            else
            {
                btnAccountingUpdate.Text = Resources.textSetAsUpdated;
                _booking.AccountingStatus = AccountingStatus.Modified;
            }
            _booking.ModifiedBy = UserIdentity;
            _booking.ModifiedDate = DateTime.Now;
            Module.SaveOrUpdate(_booking);
        }

        #endregion

        protected void btnConfirmation_Click(object sender, EventArgs e)
        {
            _booking = Module.BookingGetById(Convert.ToInt32(Request.QueryString["bookingid"]));
            BookingConfirmation.Emotion(_booking, this, Response,
                                        Server.MapPath("/Modules/Sails/Admin/ExportTemplates/BkConfirm.xls"));
        }

        protected void rptCustomers_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is Customer)
            {
                Customer customer = (Customer)e.Item.DataItem;
                CustomerInfoRowInput customerData = e.Item.FindControl("customerData") as CustomerInfoRowInput;
                if (customerData != null)
                {
                    customerData.GetCustomer(customer, Module);
                }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Button btnDelete = (Button)sender;
            RepeaterItem item = (RepeaterItem)btnDelete.Parent;
            CustomerInfoRowInput customerData = item.FindControl("customerData") as CustomerInfoRowInput;
            if (customerData != null)
            {
                Customer customer = customerData.NewCustomer(Module);
                Module.Delete(customer);
            }
            PageRedirect(Request.RawUrl);
        }

        #region -- Custom extra services --

        protected void btnAddService_Click(object sender, EventArgs e)
        {
            _booking = Module.BookingGetById(Convert.ToInt32(Request.QueryString["bookingid"]));
            IList list = rptServicesToIList(_booking);
            list.Add(new BookingService());
            rptServices.DataSource = list;
            rptServices.DataBind();
        }

        protected void rptServices_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is BookingService)
            {
                BookingService service = (BookingService)e.Item.DataItem;
                HiddenField hiddenId = (HiddenField)e.Item.FindControl("hiddenId");
                //TextBox txtName = (TextBox)e.Item.FindControl("txtName");
                DropDownList ddlService = (DropDownList)e.Item.FindControl("ddlService");
                TextBox txtUnitPrice = (TextBox)e.Item.FindControl("txtUnitPrice");
                TextBox txtQuantity = (TextBox)e.Item.FindControl("txtQuantity");
                CheckBox chkByCustomer = (CheckBox)e.Item.FindControl("chkByCustomer");

                chkByCustomer.Attributes.Add("onchange",
                                             string.Format("setDefault('{0}','{1}','{2}');", chkByCustomer.ClientID,
                                                           txtQuantity.ClientID, litPax.Text));
                ddlService.Attributes.Add("onchange",
                                          string.Format("serviceChanged('{0}','{1}','{2}','{3}','{4}');",
                                                        ddlService.ClientID, chkByCustomer.ClientID,
                                                        txtQuantity.ClientID, litPax.Text, ByCustomerId));

                ddlService.DataSource = ExtraServices;
                ddlService.DataTextField = "Name";
                ddlService.DataValueField = "Id";
                ddlService.DataBind();

                ListItem item = ddlService.Items.FindByText(service.Name);
                if (item == null && !string.IsNullOrEmpty(service.Name))
                {
                    ddlService.Items.Add(service.Name);
                    ddlService.Items[ddlService.Items.Count - 1].Selected = true;
                }
                else if (item != null)
                {
                    item.Selected = true;
                }

                hiddenId.Value = service.Id.ToString();
                chkByCustomer.Checked = service.IsByCustomer;
                txtUnitPrice.Text = service.UnitPrice.ToString();
                txtQuantity.Text = service.Quantity.ToString();
            }
        }

        protected IList rptServicesToIList(Booking booking)
        {
            IList result = new ArrayList();
            foreach (RepeaterItem item in rptServices.Items)
            {
                HiddenField hiddenId = (HiddenField)item.FindControl("hiddenId");
                //TextBox txtName = (TextBox) item.FindControl("txtName");
                DropDownList ddlService = (DropDownList)item.FindControl("ddlService");
                TextBox txtUnitPrice = (TextBox)item.FindControl("txtUnitPrice");
                TextBox txtQuantity = (TextBox)item.FindControl("txtQuantity");
                CheckBox chkByCustomer = (CheckBox)item.FindControl("chkByCustomer");
                BookingService service = new BookingService();
                service.Id = Convert.ToInt32(hiddenId.Value);
                service.Name = ddlService.SelectedItem.Text;
                service.UnitPrice = Convert.ToDouble(txtUnitPrice.Text);
                service.Quantity = Convert.ToInt32(txtQuantity.Text);
                service.Booking = booking;
                service.IsByCustomer = chkByCustomer.Checked;
                service.Deleted = !item.Visible;
                result.Add(service);
            }
            return result;
        }

        protected void rptServices_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToLower() == "remove")
            {
                e.Item.Visible = false;
            }
        }

        #endregion

        #region ICallbackEventHandler Members

        private string _callbackResult;
        public string GetCallbackResult()
        {
            return _callbackResult;
        }

        public void RaiseCallbackEvent(string eventArgument)
        {
            try
            {
                if (eventArgument.ToLower() == "-- contact --")
                {
                    _callbackResult = string.Empty;
                    return;
                }
                int bookerid = Convert.ToInt32(eventArgument);
                AgencyContact contact = Module.ContactGetById(bookerid);
                _callbackResult = contact.Phone;
                if (string.IsNullOrEmpty(_callbackResult))
                {
                    _callbackResult = "Not available";
                }
            }
            catch (Exception ex)
            {
                _callbackResult = eventArgument;
            }
        }

        #endregion

        protected void btnInvoice_Click(object sender, EventArgs e)
        {
            _booking = Module.BookingGetById(Convert.ToInt32(Request.QueryString["bookingid"]));
            Invoice(_booking, this, Response,
                                        Server.MapPath("/Modules/Sails/Admin/ExportTemplates/invoice.xls"));

        }

        protected void btnLockIncome_Click(object sender, EventArgs e)
        {
            if (!Module.PermissionCheck(Permission.ACTION_LOCKINCOME, UserIdentity))
            {
                ShowError("You do not have permission");
                return;
            }

            _booking = Module.BookingGetById(BookingId);
            _booking.LockDate = DateTime.Now;
            _booking.LockBy = UserIdentity;
            Module.Update(_booking, null);

            PageRedirect(Request.RawUrl);
        }

        protected void btnUnlockIncome_Click(object sender, EventArgs e)
        {
            if (!Module.PermissionCheck(Permission.ACTION_LOCKINCOME, UserIdentity))
            {
                ShowError("You do not have permission");
                return;
            }



            _booking = Module.BookingGetById(BookingId);
            _booking.LockDate = DateTime.Today.AddDays(2);
            _booking.LockBy = UserIdentity;
            //if (_booking.LockIncome)
            //{
            //    _booking.LockDate = null;
            //    _booking.LockBy = null;
            //}
            //else // mở riêng booking này
            //{
            //    _booking.LockDate = DateTime.Today.AddDays(2);
            //    _booking.LockBy = UserIdentity;
            //}
            Module.Update(_booking, null);

            PageRedirect(Request.RawUrl);
        }

        public void Invoice(Booking booking, SailsAdminBasePage Page, HttpResponse Response, string templatePath)
        {
            //IList bkServices = Page.Module.ExtraOptionGetBooking();
            //IList customerServices = Page.Module.ExtraOptionGetCustomer();

            // Bắt đầu thao tác export
            var excelFile = new ExcelFile();
            excelFile.LoadXls(templatePath);
            // Mở sheet 0
            var sheet = excelFile.Worksheets[0];

            #region -- Thông tin booking --

            if (booking.Agency != null)
            {
                sheet.Cells["B6"].Value = booking.Agency.Name;
                sheet.Cells["B7"].Value = booking.Agency.Phone;
                sheet.Cells["B8"].Value = booking.Agency.Fax;
            }
            sheet.Cells["E4"].Value = "CODE: " + string.Format(BookingFormat, booking.Id);
            sheet.Cells["F5"].Value = booking.StartDate;

            if (!string.IsNullOrEmpty(booking.BookerName))
            {
                sheet.Cells["A11"].Value = booking.BookerName;
            }
            sheet.Cells["B11"].Value = booking.Pax;
            sheet.Cells["F11"].Value = booking.CreatedDate;
            sheet.Cells["A15"].Value = booking.StartDate;
            sheet.Cells["B15"].Value = booking.Trip.Name;
            sheet.Cells["D15"].Value = booking.Total / booking.Pax;
            sheet.Cells["E15"].Value = booking.Pax;
            sheet.Cells["F15"].Value = booking.Total;
            if (booking.CreatedBy != null)
            {
                sheet.Cells["A28"].Value = booking.CreatedBy.FullName;
                sheet.Cells["B29"].Value = booking.CreatedBy.Website;
                sheet.Cells["B30"].Value = booking.CreatedBy.Email;
            }

            foreach (Customer c in booking.Customers)
            {
                if (!string.IsNullOrEmpty(c.Fullname))
                {
                    sheet.Cells["B4"].Value = c.Fullname;
                    break;
                }
            }


            #endregion

            #region -- Trả dữ liệu về cho người dùng --

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("content-disposition",
                                  "attachment; filename=" + string.Format("invoice-{0}.xls", booking.Id));

            MemoryStream m = new MemoryStream();

            excelFile.SaveXls(m);

            HSSFWorkbook workbook = new HSSFWorkbook(m);
            Sheet wsheet = workbook.GetSheetAt(0);
            HSSFClientAnchor anchor;
            anchor = new HSSFClientAnchor(0, 0, 0, 255, 0, 0, 4, 7);
            anchor.AnchorType = 2;
            HSSFPatriarch patriarch = (HSSFPatriarch)wsheet.CreateDrawingPatriarch();
            HSSFPicture picture = (HSSFPicture)patriarch.CreatePicture(anchor, LoadImage(templatePath.Replace("invoice.xls", "logo.jpg"), workbook));
            picture.Resize();

            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            Response.OutputStream.Write(output.GetBuffer(), 0, output.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();

            m.Close();
            output.Close();
            Response.End();

            #endregion
        }

        public static int LoadImage(string path, HSSFWorkbook wb)
        {
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[file.Length];
            file.Read(buffer, 0, (int)file.Length);
            return wb.AddPicture(buffer, PictureType.JPEG);

        }
    }
}