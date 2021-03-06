using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CMS.Core.Domain;
using CMS.ServerControls;
using GemBox.Spreadsheet;
using log4net;
using NHibernate.Expression;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.ReportEngine;
using Portal.Modules.OrientalSails.Web.UI;
using Portal.Modules.OrientalSails.Web.Util;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class BookingReport : SailsAdminBase
    {
        #region -- PRIVATE MEMBERS --

        private readonly ILog _logger = LogManager.GetLogger(typeof(BookingReport));
        private int _adult;
        private int _baby;
        private int _child;
        private SailsTrip _trip;
        private IList _cruises;
        private double _customerCost;
        private IList _dailyCost;
        protected DateTime _date;
        private int _doubleCabin;
        private IList _guides;
        private double _runningCost;

        private int _numberOfGroups; // số nhóm
        private int _currentGroup;

        /// <summary>
        /// Các dịch vụ/chi phí theo ngày
        /// </summary>
        private IList _services;

        /// <summary>
        /// Tổng số khách
        /// </summary>
        private double _total;

        /// <summary>
        /// Tổng chi phí nhập thủ công
        /// </summary>
        private double _totalCost;

        private int _transferAdult;
        private int _transferChild;
        private int _twin;

        protected IList Suppliers(int costypeid)
        {
            if (Request.QueryString["tripid"] != null)
            {
                SailsTrip trip = Module.TripGetById(Convert.ToInt32(Request.QueryString["tripid"]));
                return Module.SupplierGetAll(costypeid, trip.Organization);
            }

            if (Request.QueryString["orgid"] != null)
            {
                Organization organization =
                    Module.OrganizationGetById(Convert.ToInt32(Request.QueryString["orgid"]));
                return Module.SupplierGetAll(costypeid, organization);
            }
            return Module.SupplierGetAll(costypeid);
        }

        protected IList Guides
        {
            get
            {
                if (_guides == null)
                {
                    if (Request.QueryString["tripid"] != null)
                    {
                        SailsTrip trip = Module.TripGetById(Convert.ToInt32(Request.QueryString["tripoid"]));
                        _guides = Module.GuidesGetAll(trip.Organization);
                    }
                    else if (Request.QueryString["orgid"] != null)
                    {
                        Organization organization =
                            Module.OrganizationGetById(Convert.ToInt32(Request.QueryString["orgid"]));
                        _guides = Module.GuidesGetAll(organization);
                    }
                    else
                    {
                        _guides = Module.GuidesGetAll();
                    }
                }
                return _guides;
            }
        }

        protected IList DailyCost
        {
            get
            {
                if (_dailyCost == null)
                {
                    _dailyCost = Module.CostTypeGetDailyInput();
                }
                return _dailyCost;
            }
        }

        protected SailsTrip ActiveTrip
        {
            get
            {
                if (_trip == null && Request.QueryString["tripid"] != null)
                {
                    _trip = Module.TripGetById(Convert.ToInt32(Request.QueryString["tripid"]));
                }
                return _trip;
            }
        }

        protected IList AllCruises
        {
            get
            {
                if (_cruises == null)
                {
                    _cruises = Module.CruiseGetAll();
                }
                return _cruises;
            }
        }

        /// <summary>
        /// Ghi lại số khách theo hành trình
        /// </summary>
        private readonly Dictionary<int, int> _customers = new Dictionary<int, int>();

        /// <summary>
        /// Ghi lại số khách theo org
        /// </summary>
        private readonly Dictionary<int, int> _customerByOrg = new Dictionary<int, int>();

        private IList _operators;

        /// <summary>
        /// Danh sách operators có cached
        /// </summary>
        protected IList Operators
        {
            get
            {
                if (_operators == null)
                    _operators = Module.OperatorGetAll();
                return _operators;
            }
            set { _operators = value; }
        }
        #endregion

        #region -- PAGE EVENTS --

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Title = Resources.textBookingByDate;
                plhExpenses.Visible = ShowExpenseByDate;
                plhBarRevenue.Visible = ShowBarRevenue;
                if (!IsPostBack)
                {
                    #region -- Chi phí theo ngày --

                    // Lấy danh sách các chi phí nhập thủ công theo ngày để tạo danh sách nút Add
                    _services = new ArrayList();
                    foreach (CostType type in DailyCost)
                    {
                        if (type.IsSupplier)
                        {
                            _services.Add(type);
                        }
                    }

                    if (ActiveTrip == null)
                    {
                        plhOperator.Visible = false;
                    }

                    // Lấy danh sách org, nếu chỉ có 1 org thì redirect
                    var list = Module.OrganizationGetByUser(UserIdentity);
                    if (list.Count == 1)
                    {
                        string date = string.Empty;
                        if (Request.QueryString["date"] == null)
                        {
                            date = DateTime.Today.ToOADate().ToString();
                            date = "&date=" + date;
                            PageRedirect(string.Format("BookingReport.aspx?NodeId={0}&SectionId={1}{3}&orgid={2}",
                                                   Node.Id, Section.Id, ((UserOrganization)list[0]).Organization.Id, date));
                            return;
                        }

                    }


                    #endregion

                    // Lấy ngày cần hiển thị
                    if (Request.QueryString["Date"] != null)
                    {
                        txtDate.Text =
                            DateTime.FromOADate(Convert.ToDouble(Request.QueryString["Date"])).ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        txtDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
                    }

                    if (ActiveTrip != null)
                    {
                        DateTime adate = DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        string script =
                            string.Format(
                                "window.location='CustomerComment.aspx?NodeId={0}&SectionId={1}&cruiseid={2}&date={3}'",
                                Node.Id, Section.Id, ActiveTrip.Id, adate.ToOADate());
                        btnComment.Attributes.Add("onclick", script);

                        var expense = Module.ExpenseGetByDate(ActiveTrip, adate);
                        if (expense.NumberOfGroup > 0)
                        {
                            _numberOfGroups = expense.NumberOfGroup;
                        }
                    }
                    else
                    {
                        btnComment.Visible = false;
                    }

                    GetDataSource();
                    rptBookingList.DataBind();

                    #region -- Danh sách hành trình --

                    rptOrganization.DataSource = Module.OrganizationGetAllRoot();
                    rptOrganization.DataBind();

                    if (Request.QueryString["orgid"] == null)
                    {
                        if (ActiveTrip == null)
                        {
                            rptTrips.DataSource = Module.TripGetAll(false, UserIdentity);
                            rptTrips.DataBind();
                        }
                        else
                        {
                            rptTrips.DataSource = Module.TripGetByOrganization(ActiveTrip.Organization);
                            rptTrips.DataBind();
                        }
                    }
                    else
                    {
                        rptTrips.DataSource =
                            Module.TripGetByOrganization(
                                Module.OrganizationGetById(Convert.ToInt32(Request.QueryString["orgid"])));
                        rptTrips.DataBind();
                    }

                    #endregion

                    DateTime today = Request.QueryString["Date"] != null ? DateTime.FromOADate(Convert.ToDouble(Request.QueryString["Date"])) : DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var shadows = Module.GetBookingShadow(today);
                    var listHistories = new ArrayList();
                    foreach (Booking booking in shadows)
                    {
                        var histories = Module.BookingGetHistory(booking);
                        for (int ii = histories.Count - 1; ii >= 0; ii--)
                        {
                            if (((BookingHistory)(histories[ii])).StartDate == today || ((BookingHistory)(histories[ii])).Status == StatusType.Cancelled)
                            {
                                var bh = (BookingHistory)(histories[ii]);
                                if (bh.Date > today.AddDays(-2) && bh.Date < today.AddDays(2))
                                {
                                    listHistories.Add(booking);
                                }

                                break;
                            }
                        }
                    }

                    rptShadows.DataSource = listHistories;
                    rptShadows.DataBind();

                }

                if (ActiveTrip == null)
                {
                    btnExport.Visible = false;
                }

                foreach (RepeaterItem rptItem in rptBookingList.Items)
                {
                    var ddlGroup = (DropDownList)rptItem.FindControl("ddlGroup");
                    if (ddlGroup.SelectedIndex >= 0)
                    {
                        var hiddenId = (HiddenField)rptItem.FindControl("hiddenId");
                        var bk = Module.BookingGetById(Convert.ToInt32(hiddenId.Value));
                        int group = Convert.ToInt32(ddlGroup.SelectedValue);
                        if (bk.Group != group)
                        {
                            bk.Group = group;
                            Module.SaveOrUpdate(bk);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error when Page_load in BookingList", ex);
                ShowError(ex.Message);
            }
        }

        #endregion

        #region -- CONTROL EVENTS --

        #region -- Report --

        protected void btnSaveExpenses_Click(object sender, EventArgs e)
        {
            IList list;
            int count = GetData(out list, false);
            //if (count == 0)
            //{
            //    ShowError(Resources.errorNoBookingSave);
            //    return;
            //}
            DateTime date = Request.QueryString["Date"] != null ? DateTime.FromOADate(Convert.ToDouble(Request.QueryString["Date"])) : DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            foreach (RepeaterItem rptItem in rptCruiseExpense.Items)
            {
                var hiddenId = (HiddenField)rptItem.FindControl("hiddenId");
                var cruise = Module.TripGetById(Convert.ToInt32(hiddenId.Value));
                var expense = Module.ExpenseGetByDate(cruise, date);

                // chỉ lưu operator khi là một trip cụ thể
                if (ActiveTrip != null)
                {
                    if (ddlOperators.SelectedIndex > 0)
                    {
                        expense.Operator = Module.UserGetById(Convert.ToInt32(ddlOperators.SelectedValue));
                        expense.OperatorName = expense.Operator.FullName;
                        expense.OperatorPhone = expense.Operator.Website;
                    }
                    else
                    {
                        expense.OperatorName = txtOperator.Text;
                        expense.OperatorPhone = txtPhone.Text;
                    }
                    if (ddlSaleInCharge.SelectedIndex > 0)
                    {
                        expense.SaleInCharge = Module.UserGetById(Convert.ToInt32(ddlSaleInCharge.SelectedValue));
                    }
                }

                #region -- Tính toán số group --
                var rptServices = (Repeater)rptItem.FindControl("rptServices");
                IList expenseList = rptServicesToIList(rptServices);
                int numberOfGroup = 1;
                foreach (ExpenseService service in expenseList)
                {
                    numberOfGroup = Math.Max(service.Group, numberOfGroup);
                }
                expense.NumberOfGroup = numberOfGroup;
                expense.IsEvent = true;

                #endregion

                if (expense.Id < 0)
                {
                    Module.SaveOrUpdate(expense);
                }

                foreach (ExpenseService service in expenseList)
                {
                    service.Expense = expense;
                    if (service.IsRemoved)
                    {
                        if (service.Id > 0)
                        {
                            expense.Services.Remove(service);
                        }
                    }
                    else
                    {
                        // Phải có tên hoặc giá thì mới lưu
                        if (!string.IsNullOrEmpty(service.Name) || service.Cost > 0)
                        {
                            Module.SaveOrUpdate(service);
                        }
                    }
                }

                Module.SaveOrUpdate(expense);
            }

            foreach (RepeaterItem rptItem in rptBookingList.Items)
            {
                var ddlGroup = (DropDownList)rptItem.FindControl("ddlGroup");
                if (ddlGroup.SelectedIndex >= 0)
                {
                    var hiddenId = (HiddenField)rptItem.FindControl("hiddenId");
                    var bk = Module.BookingGetById(Convert.ToInt32(hiddenId.Value));
                    int group = Convert.ToInt32(ddlGroup.SelectedValue);
                    if (bk.Group != group)
                    {
                        bk.Group = group;
                        Module.SaveOrUpdate(bk);
                    }
                }
            }

            LoadService(date);

        }

        // Lệnh điều tour
        protected void btnExport_Click(object sender, EventArgs e)
        {
            #region -- Lưu thông tin expense --

            IList list;
            int count = GetData(out list, false);
            if (count == 0)
            {
                ShowError(Resources.errorNoBookingSave);
                return;
            }
            DateTime date = Request.QueryString["Date"] != null ? DateTime.FromOADate(Convert.ToDouble(Request.QueryString["Date"])) : DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            IList expenseList = null;
            foreach (RepeaterItem rptItem in rptCruiseExpense.Items)
            {
                HiddenField hiddenId = (HiddenField)rptItem.FindControl("hiddenId");
                SailsTrip cruise = Module.TripGetById(Convert.ToInt32(hiddenId.Value));
                SailExpense expense = Module.ExpenseGetByDate(cruise, date);

                // chỉ lưu operator khi là một trip cụ thể
                if (ActiveTrip != null)
                {
                    if (ddlOperators.SelectedIndex > 0)
                    {
                        expense.Operator = Module.UserGetById(Convert.ToInt32(ddlOperators.SelectedValue));
                        expense.OperatorName = expense.Operator.FullName;
                        expense.OperatorPhone = expense.Operator.Website;
                    }
                    else
                    {
                        expense.OperatorName = txtOperator.Text;
                        expense.OperatorPhone = txtPhone.Text;
                    }
                    if (ddlSaleInCharge.SelectedIndex > 0)
                    {
                        expense.SaleInCharge = Module.UserGetById(Convert.ToInt32(ddlSaleInCharge.SelectedValue));
                    }
                }

                if (expense.Id < 0)
                {
                    Module.SaveOrUpdate(expense);
                }

                #region -- Tính toán số group --
                Repeater rptServices = (Repeater)rptItem.FindControl("rptServices");
                expenseList = rptServicesToIList(rptServices);
                int numberOfGroup = 1;
                foreach (ExpenseService service in expenseList)
                {
                    numberOfGroup = Math.Max(service.Group, numberOfGroup);
                }
                expense.NumberOfGroup = numberOfGroup;
                expense.IsEvent = true;

                #endregion

                foreach (ExpenseService service in expenseList)
                {
                    service.Expense = expense;
                    if (service.IsRemoved)
                    {
                        if (service.Id > 0)
                        {
                            expense.Services.Remove(service);
                        }
                    }
                    else
                    {
                        // Phải có tên hoặc giá thì mới lưu
                        if (!string.IsNullOrEmpty(service.Name) || service.Cost > 0)
                        {
                            Module.SaveOrUpdate(service);
                        }
                    }
                }

                Module.SaveOrUpdate(expense);
            }

            LoadService(date);

            #endregion

            string tplPath = Server.MapPath("/Modules/Sails/Admin/ExportTemplates/Lenh_dieu_tour.xls");
            TourCommand.Export(list, count, expenseList, _date, BookingFormat, Response, tplPath, ActiveTrip, this);
        }

        // Danh sách phòng
        protected void btnExportRoom_Click(object sender, EventArgs e)
        {
            IList list;
            GetData(out list, false);

            int totalRows = 0;
            foreach (Booking booking in list)
            {
                totalRows += booking.BookingRooms.Count;
            }


            // Bắt đầu thao tác export
            ExcelFile excelFile = new ExcelFile();
            excelFile.LoadXls(Server.MapPath("/Modules/Sails/Admin/ExportTemplates/Rooming_list.xls"));
            // Mở sheet 0
            ExcelWorksheet sheet = excelFile.Worksheets[0];

            #region -- Xuất dữ liệu --

            const int firstrow = 3;
            sheet.Rows[firstrow].InsertCopy(totalRows - 1, sheet.Rows[firstrow]);
            int curr = firstrow;
            foreach (Booking booking in list)
            {
                foreach (BookingRoom room in booking.BookingRooms)
                {
                    sheet.Cells[curr, 0].Value = curr - firstrow + 1;
                    string name = string.Empty;
                    foreach (Customer customer in room.Customers)
                    {
                        if (!string.IsNullOrEmpty(customer.Fullname))
                        {
                            name += customer.Fullname + "\n";
                        }
                    }
                    if (name.Length > 0)
                    {
                        name = name.Remove(name.Length - 1);
                    }
                    sheet.Cells[curr, 1].Value = name;
                    sheet.Cells[curr, 2].Value = room.Adult + room.Child;
                    sheet.Cells[curr, 3].Value = room.RoomType.Name;
                    if (room.Room != null)
                    {
                        sheet.Cells[curr, 4].Value = room.Room.Name;
                    }
                    curr++;
                }
            }

            #endregion

            #region -- Trả dữ liệu về cho người dùng --

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("content-disposition",
                                  "attachment; filename=" + string.Format("Roomlist{0:dd_MMM}", _date));

            MemoryStream m = new MemoryStream();

            excelFile.SaveXls(m);

            Response.OutputStream.Write(m.GetBuffer(), 0, m.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();

            m.Close();
            Response.End();

            #endregion
        }

        //Welcome board
        protected void btnExportWelcome_Click(object sender, EventArgs e)
        {
            IList bklist;
            GetData(out bklist, false);
            IList list = new ArrayList();
            foreach (Booking booking in bklist)
            {
                foreach (Customer customer in booking.Customers)
                {
                    list.Add(customer);
                }
            }
            int count = list.Count;


            // Bắt đầu thao tác export
            ExcelFile excelFile = new ExcelFile();
            excelFile.LoadXls(Server.MapPath("/Modules/Sails/Admin/ExportTemplates/welcome_board.xls"));
            // Mở sheet 0
            ExcelWorksheet sheet = excelFile.Worksheets[0];

            //SailExpense expense = Module.ExpenseGetByDate(ActiveCruise, _date);

            CellRange range = sheet.Cells.GetSubrangeRelative(0, 0, 12, 6);

            #region -- Xuất dữ liệu --

            const int firstrow = 0;
            for (int ii = 1; ii < count; ii++)
            {
                range.CopyTo(ii * 6, 0);
                for (int jj = 0; jj < 6; jj++)
                {
                    sheet.Rows[ii * 6 + jj].Height = sheet.Rows[jj].Height;
                }
            }
            int curr = firstrow;
            foreach (Customer customer in list)
            {
                string name = customer.Fullname;
                sheet.Cells[curr + 2, 0].Value = name;
                sheet.Cells[curr + 4, 4].Value = customer.Booking.PickupAddress;
                sheet.Cells[curr + 4, 9].Value = UserIdentity.FullName;
                curr += 6;
            }

            #endregion

            #region -- Trả dữ liệu về cho người dùng --

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("content-disposition",
                                  "attachment; filename=" + string.Format("WelcomeBoard{0:dd_MMM}", _date));

            MemoryStream m = new MemoryStream();

            excelFile.SaveXls(m);

            Response.OutputStream.Write(m.GetBuffer(), 0, m.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();

            m.Close();
            Response.End();

            #endregion
        }

        //Client detail
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            #region -- excel --

            IList dataSource;
            GetData(out dataSource, false);

            #region -- Xuất dữ liệu ra excel dùng thư viện --

            // Lấy danh sách khách hàng
            IList customers = new ArrayList();
            DateTime startDate = DateTime.MinValue;

            #region -- Danh sách khách hàng --

            foreach (Booking book in dataSource)
            {
                if (startDate < book.StartDate)
                {
                    startDate = book.StartDate;
                }
                foreach (BookingRoom room in book.BookingRooms)
                {
                    foreach (Customer customer in room.RealCustomers)
                    {
                        customers.Add(customer);
                    }
                }
            }

            #endregion

            if (startDate == DateTime.MinValue)
            {
                ShowError("Không có thông tin");
                return;
            }
            string tpl = Server.MapPath("/Modules/Sails/Admin/ExportTemplates/Clients Details.xls");
            ReportEngine.CustomerDetails(customers, startDate, tpl, Response);

            #endregion

            #endregion
        }

        //Xuất form đăng ký tạm trú
        protected void btnProvisional_Click(object sender, EventArgs e)
        {
            IList list;
            GetData(out list, false);
            Purpose defaultPurpose = Module.PurposeGetById(3);
            ReportEngine.Provisional(list, _date, defaultPurpose, this, Response,
                                     Server.MapPath("/Modules/Sails/Admin/ExportTemplates/KhaiBaoTamTru.xls"));
        }

        /// <summary>
        /// Báo cáo doanh thu ngày
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnIncomeDate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDate.Text))
            {
                _date = DateTime.Today;
            }
            else
            {
                _date = Request.QueryString["Date"] != null ? DateTime.FromOADate(Convert.ToDouble(Request.QueryString["Date"])) : DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            IList data = Module.BookingGetByStartDate(_date, null, false);

            ReportEngine.IncomeByDate(data, this, Response,
                                      Server.MapPath("/Modules/Sails/Admin/ExportTemplates/IncomeDate.xls"));
            //IncomeByDate.Emotion(data, this, Response,
            //                     Server.MapPath("/Modules/Sails/Admin/ExportTemplates/IncomeDate.xls"));
        }

        protected void btnFaxHotel_Click(object sender, EventArgs e)
        {
            _date = Request.QueryString["Date"] != null ? DateTime.FromOADate(Convert.ToDouble(Request.QueryString["Date"])) : DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            IList data = Module.ExpenseServiceGet(ActiveTrip, _date, _date, null, null, Request.QueryString["orgid"], UserIdentity, true, null, 0, "", Module.CostTypeGetById(SailsModule.HOTEL));

            if (data.Count == 0)
            {
                ShowError("Chưa lưu thông tin dịch vụ khách sạn");
                return;
            }

            ExpenseService expense = (ExpenseService)data[0];
            string name;
            if (expense.Supplier != null && string.IsNullOrEmpty(expense.Name))
            {
                name = expense.Supplier.Name;
            }
            else
            {
                name = expense.Name;
            }

            IList bklist;
            GetData(out bklist, false);
            IList list = new ArrayList();
            string c_name = string.Empty;
            foreach (Booking booking in bklist)
            {
                foreach (Customer customer in booking.Customers)
                {
                    list.Add(customer);
                    if (!string.IsNullOrEmpty(customer.Fullname))
                    {
                        c_name = customer.Fullname;
                    }
                }
            }
            int count = list.Count;

            #region -- Xuất dữ liệu ra excel dùng thư viện --

            // Bắt đầu thao tác export
            ExcelFile excelFile = new ExcelFile();
            excelFile.LoadXls(Server.MapPath("/Modules/Sails/Admin/ExportTemplates/HotelFax.xls"));

            ExcelWorksheet sheet = excelFile.Worksheets[0];
            sheet.Cells["H4"].Value = UserIdentity.FullName;
            sheet.Cells["C4"].Value = name;
            if (expense.Supplier != null)
            {
                sheet.Cells["C5"].Value = expense.Supplier.Phone;
                sheet.Cells["C6"].Value = expense.Supplier.Fax;
            }
            sheet.Cells["A16"].Value = string.Format("{0}-{1:ddMMyyyy}", ActiveTrip.TripCode, _date);
            sheet.Cells["C16"].Value = count;
            sheet.Cells["D16"].Value = count / 2 + count % 2;
            sheet.Cells["F16"].Value = c_name;


            #endregion

            #region -- Trả dữ liệu về cho người dùng --

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("content-disposition",
                                  "attachment; filename=" + string.Format("FaxHotel{0}.xls", name));

            MemoryStream m = new MemoryStream();

            excelFile.SaveXls(m);

            Response.OutputStream.Write(m.GetBuffer(), 0, m.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();

            m.Close();
            Response.End();

            #endregion
        }

        #endregion

        #region -- Booking --
        protected void rptBookingList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                var thTrip = (HtmlTableCell)e.Item.FindControl("thTrip");
                var thSpecialRequest = (HtmlTableCell)e.Item.FindControl("thSpecialRequest");
                var thTime = (HtmlTableCell)e.Item.FindControl("thTime");
                var thFligthDetails = (HtmlTableCell)e.Item.FindControl("thFlightDetails");
                var thCarRequirements = (HtmlTableCell)e.Item.FindControl("thCarRequirements");
                var thDropoffAddress = (HtmlTableCell)e.Item.FindControl("thDropoffAddress");

                if (ActiveTrip != null)
                {
                    if (ActiveTrip.Name.ToLower() == "airport transfer")
                    {
                        if (thTrip != null)
                            thTrip.Visible = false;
                        //if (thSpecialRequest != null)
                        //    thSpecialRequest.Visible = false;
                        if (thTime != null)
                            thTime.Visible = true;
                        if (thFligthDetails != null)
                            thFligthDetails.Visible = true;
                        if (thCarRequirements != null)
                            thCarRequirements.Visible = true;
                        if (thDropoffAddress != null)
                            thDropoffAddress.Visible = true;
                    }
                    else
                    {
                        if (thTrip != null)
                            thTrip.Visible = true;
                        //if (thSpecialRequest != null)
                        //    thSpecialRequest.Visible = true;
                        if (thTime != null)
                            thTime.Visible = false;
                        if (thFligthDetails != null)
                            thFligthDetails.Visible = false;
                        if (thCarRequirements != null)
                            thCarRequirements.Visible = false;
                        if (thDropoffAddress != null)
                            thDropoffAddress.Visible = false;
                    }
                }
                else
                {
                    if (thTrip != null)
                        thTrip.Visible = true;
                    //if (thSpecialRequest != null)
                    //    thSpecialRequest.Visible = true;
                    if (thTime != null)
                        thTime.Visible = false;
                    if (thFligthDetails != null)
                        thFligthDetails.Visible = false;
                    if (thCarRequirements != null)
                        thCarRequirements.Visible = false;
                    if (thDropoffAddress != null)
                        thDropoffAddress.Visible = false;
                }
            }

            if (e.Item.DataItem is Booking)
            {
                Booking booking = e.Item.DataItem as Booking;
                HtmlTableRow trItem = (HtmlTableRow)e.Item.FindControl("trItem");
                if (booking.StartDate < _date)
                {
                    trItem.Attributes.Add("style", "background-color :" + SailsModule.WARNING);
                }
                Label label_NameOfPax = (Label)e.Item.FindControl("label_NameOfPax");
                Label label_NoOfAdult = (Label)e.Item.FindControl("label_NoOfAdult");
                Label label_NoOfChild = (Label)e.Item.FindControl("label_NoOfChild");
                Label label_NoOfBaby = (Label)e.Item.FindControl("label_NoOfBaby");
                Label label_NoOfDoubleCabin = (Label)e.Item.FindControl("label_NoOfDoubleCabin");
                Label label_NoOfTwinCabin = (Label)e.Item.FindControl("label_NoOfTwinCabin");
                Label label_NoOfTransferAdult = (Label)e.Item.FindControl("label_NoOfTransferAdult");
                Label label_NoOfTransferChild = (Label)e.Item.FindControl("label_NoOfTransferChild");
                Label label_TotalPrice = (Label)e.Item.FindControl("label_TotalPrice");
                HyperLink hyperLink_Partner = (HyperLink)e.Item.FindControl("hyperLink_Partner");
                Literal ltrTime = (Literal)e.Item.FindControl("ltrTime");
                Literal ltrFlightDetails = (Literal)e.Item.FindControl("ltrFlightDetails");
                Literal ltrCarRequirements = (Literal)e.Item.FindControl("ltrCarRequirements");
                var ltrDropoffAddress = (Literal)e.Item.FindControl("ltrDropoffAddress");
                var tdTrip = (HtmlTableCell)e.Item.FindControl("tdTrip");
                var tdSpecialRequest = (HtmlTableCell)e.Item.FindControl("tdSpecialRequest");
                var tdTime = (HtmlTableCell)e.Item.FindControl("tdTime");
                var tdFligthDetails = (HtmlTableCell)e.Item.FindControl("tdFlightDetails");
                var tdCarRequirements = (HtmlTableCell)e.Item.FindControl("tdCarRequirements");
                var tdFeedback = (HtmlTableCell)e.Item.FindControl("tdFeedback");
                var tdDropoffAddress = (HtmlTableCell)e.Item.FindControl("tdDropoffAddress");


                if (booking.SeeoffTime >= _date && booking.SeeoffTime <= _date.Add(new TimeSpan(23, 59, 59)))
                {
                    if (ltrTime != null)
                    {
                        try
                        {

                            ltrTime.Text = booking.SeeoffTime.Value.ToString("HH:mm");

                        }
                        catch (Exception)
                        {
                            ltrTime.Text = "";
                        }
                    }
                }

                if (booking.PickupTime >= _date && booking.PickupTime <= _date.Add(new TimeSpan(23, 59, 59)))
                {
                    if (ltrTime != null)
                    {
                        try
                        {
                            ltrTime.Text = booking.PickupTime.Value.ToString("HH:mm");
                        }
                        catch (Exception)
                        {
                            ltrTime.Text = "";
                        }
                    }
                }



                Literal litIndex = (Literal)e.Item.FindControl("litIndex");
                litIndex.Text =
                    (e.Item.ItemIndex + 1).ToString();

                #region -- NAME --

                label_NameOfPax.Text = booking.CustomerName;

                #endregion

                #region -- Partner --

                if (booking.Agency != null)
                {
                    hyperLink_Partner.Text = booking.Agency.Name;
                }
                else
                {
                    hyperLink_Partner.Text = SailsModule.NOAGENCY;
                }

                #endregion

                #region -- Number of pax --

                label_NoOfAdult.Text = booking.Adult.ToString();
                _adult += booking.Adult;

                label_NoOfChild.Text = booking.Child.ToString();
                label_NoOfBaby.Text = booking.Baby.ToString();
                _child += booking.Child;
                _baby += booking.Baby;

                #endregion

                #region - No of cabins -

                label_NoOfDoubleCabin.Text = booking.DoubleCabin.ToString();
                label_NoOfTwinCabin.Text = string.Format("{1}({0} adults)", booking.TwinCabin,
                                                         booking.TwinCabin / 2 + booking.TwinCabin % 2);

                _doubleCabin += booking.DoubleCabin;
                _twin += booking.TwinCabin;

                #endregion

                #region -- Transfer --

                bool transfer = false;
                foreach (ExtraOption service in booking.ExtraServices)
                {
                    if (service.Id == SailsModule.TRANSFER)
                    {
                        transfer = true;
                        break;
                    }
                }
                if (transfer)
                {
                    label_NoOfTransferAdult.Text = label_NoOfAdult.Text;
                    label_NoOfTransferChild.Text = label_NoOfChild.Text;
                    _transferChild += booking.Child;
                    _transferAdult += booking.Adult;
                }
                else
                {
                    label_NoOfTransferAdult.Text = @"0";
                    label_NoOfTransferChild.Text = @"0";
                }

                #endregion

                #region -- Total --

                label_TotalPrice.Text = booking.TotalVND.ToString("#,0");
                _total += booking.TotalVND;

                #endregion

                #region -- Itinerary --

                Label labelItinerary = e.Item.FindControl("labelItinerary") as Label;
                if (labelItinerary != null)
                {
                    labelItinerary.Text = booking.Trip.TripCode;
                }

                Label labelPuAddress = e.Item.FindControl("labelPuAddress") as Label;
                if (labelPuAddress != null)
                {
                    labelPuAddress.Text = booking.PickupAddress;
                }


                if (booking.Trip.Name.ToLower() == "airport transfer")
                {
                    if (booking.SeeoffTime >= _date && booking.SeeoffTime <= _date.Add(new TimeSpan(23, 59, 59)))
                    {

                        if (labelPuAddress != null)
                        {
                            labelPuAddress.Text = booking.SOPickupAddress;
                        }


                        if (ltrDropoffAddress != null)
                        {
                            ltrDropoffAddress.Text = booking.SODropoffAddress;
                        }

                    }

                    if (booking.PickupTime >= _date && booking.PickupTime <= _date.Add(new TimeSpan(23, 59, 59)))
                    {
                        if (labelPuAddress != null)
                        {
                            labelPuAddress.Text = booking.PUPickupAddress;
                        }

                        if (ltrDropoffAddress != null)
                        {
                            ltrDropoffAddress.Text = booking.PUDropoffAddress;
                        }

                    }
                }

                Label labelSpecialRequest = e.Item.FindControl("labelSpecialRequest") as Label;
                if (labelSpecialRequest != null)
                {
                    labelSpecialRequest.Text = booking.SpecialRequest;
                }

                HyperLink hplCode = e.Item.FindControl("hplCode") as HyperLink;
                if (hplCode != null)
                {
                    if (true)
                    //if (string.IsNullOrEmpty(booking.AgencyCode))
                    {
                        if (booking.CustomBookingId > 0)
                        {
                            hplCode.Text = string.Format(BookingFormat, booking.CustomBookingId);
                        }
                        else
                        {
                            hplCode.Text = string.Format(BookingFormat, booking.Id);
                        }
                    }
                    hplCode.NavigateUrl = string.Format("BookingView.aspx?NodeId={0}&SectionId={1}&BookingId={2}",
                                                        Node.Id, Section.Id, booking.Id);
                }

                #endregion

                HtmlAnchor anchorFeedback = e.Item.FindControl("anchorFeedback") as HtmlAnchor;
                if (anchorFeedback != null)
                {
                    string url = string.Format("SurveyInput.aspx?NodeId={0}&SectionId={1}&BookingId={2}", Node.Id,
                                               Section.Id, booking.Id);
                    anchorFeedback.Attributes.Add("onclick",
                                            CMS.ServerControls.Popup.OpenPopupScript(url, "Survey input", 600, 800));
                }

                // Nếu có số lượng nhóm
                if (_numberOfGroups < int.MaxValue)
                {
                    DropDownList ddlGroup = (DropDownList)e.Item.FindControl("ddlGroup");
                    for (int ii = 1; ii <= _numberOfGroups; ii++)
                    {
                        ddlGroup.Items.Add(ii.ToString());
                    }
                    ddlGroup.SelectedValue = booking.Group.ToString();
                }
                else if (Request.QueryString["tripid"] == null)
                {
                    DropDownList ddlGroup = (DropDownList)e.Item.FindControl("ddlGroup");
                    ddlGroup.Visible = false;

                    Literal litGroup = (Literal)e.Item.FindControl("litGroup");
                    litGroup.Text = booking.Group.ToString();
                }

                if (ActiveTrip != null)
                {
                    if (ActiveTrip.Name.ToLower() == "airport transfer")
                    {
                        if (tdTrip != null)
                            tdTrip.Visible = false;
                        //if (tdSpecialRequest != null)
                        //    tdSpecialRequest.Visible = false;
                        if (tdTime != null)
                            tdTime.Visible = true;
                        if (tdFligthDetails != null)
                            tdFligthDetails.Visible = true;
                        if (tdCarRequirements != null)
                            tdCarRequirements.Visible = true;
                        if (tdFeedback != null)
                            tdFeedback.Visible = false;
                        if (tdDropoffAddress != null)
                            tdDropoffAddress.Visible = true;
                        if (booking.SeeoffTime >= _date && booking.SeeoffTime <= _date.Add(new TimeSpan(23, 59, 59)))
                        {
                            trItem.Attributes.Add("style", "background-color:violet");
                            if (booking.SOCarRequirements != null)
                            {
                                ltrCarRequirements.Text = booking.SOCarRequirements;
                            }

                            if (booking.SOFlightDetails != null)
                            {
                                ltrFlightDetails.Text = booking.SOFlightDetails;
                            }

                        }
                        if (booking.PickupTime >= _date && booking.PickupTime <= _date.Add(new TimeSpan(23, 59, 59)))
                        {
                            trItem.Attributes.Add("style", "background-color:yellowgreen");

                            if (booking.PUCarRequirements != null)
                            {
                                ltrCarRequirements.Text = booking.PUCarRequirements;
                            }

                            if (booking.PUFlightDetails != null)
                            {
                                ltrFlightDetails.Text = booking.PUFlightDetails;
                            }

                        }
                    }
                    else
                    {
                        if (tdTrip != null)
                            tdTrip.Visible = true;
                        //if (tdSpecialRequest != null)
                        //    tdSpecialRequest.Visible = true;
                        if (tdTime != null)
                            tdTime.Visible = false;
                        if (tdFligthDetails != null)
                            tdFligthDetails.Visible = false;
                        if (tdCarRequirements != null)
                            tdCarRequirements.Visible = false;
                        if (tdFeedback != null)
                            tdFeedback.Visible = true;
                        if (tdDropoffAddress != null)
                            tdDropoffAddress.Visible = false;
                    }
                }
                else
                {
                    if (tdTrip != null)
                        tdTrip.Visible = true;
                    //if (tdSpecialRequest != null)
                    //    tdSpecialRequest.Visible = true;
                    if (tdTime != null)
                        tdTime.Visible = false;
                    if (tdFligthDetails != null)
                        tdFligthDetails.Visible = false;
                    if (tdCarRequirements != null)
                        tdCarRequirements.Visible = false;
                    if (tdFeedback != null)
                        tdFeedback.Visible = true;
                    if (tdDropoffAddress != null)
                        tdDropoffAddress.Visible = false;
                }
            }

            if (e.Item.ItemType == ListItemType.Footer)
            {
                Label label_NoOfAdult = (Label)e.Item.FindControl("label_NoOfAdult");
                label_NoOfAdult.Text = _adult.ToString();
                Label label_NoOfChild = (Label)e.Item.FindControl("label_NoOfChild");
                label_NoOfChild.Text = _child.ToString();
                Label label_NoOfBaby = (Label)e.Item.FindControl("label_NoOfBaby");
                label_NoOfBaby.Text = _baby.ToString();
                Label label_NoOfDoubleCabin = (Label)e.Item.FindControl("label_NoOfDoubleCabin");
                label_NoOfDoubleCabin.Text = _doubleCabin.ToString();
                Label label_NoOfTwinCabin = (Label)e.Item.FindControl("label_NoOfTwinCabin");
                label_NoOfTwinCabin.Text = string.Format("{1}({0} adults)", _twin, _twin / 2 + _twin % 2);
                Label label_NoOfTransferAdult = (Label)e.Item.FindControl("label_NoOfTransferAdult");
                label_NoOfTransferAdult.Text = _transferAdult.ToString();
                Label label_NoOfTransferChild = (Label)e.Item.FindControl("label_NoOfTransferChild");
                label_NoOfTransferChild.Text = _transferChild.ToString();
                Label label_TotalPrice = (Label)e.Item.FindControl("label_TotalPrice");
                label_TotalPrice.Text = _total.ToString("#,####");
            }
        }

        protected void rptBookingList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
        }

        #endregion

        #region -- Services --
        protected void rptServices_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is ExpenseService)
            {
                ExpenseService service = (ExpenseService)e.Item.DataItem;

                if (service.Group != _currentGroup && service.Group > 0)
                {
                    _currentGroup = service.Group;
                    HtmlTableRow seperator = (HtmlTableRow)e.Item.FindControl("seperator");
                    seperator.Visible = true;
                }

                var hiddenId = (HiddenField)e.Item.FindControl("hiddenId");
                var hiddenType = (HiddenField)e.Item.FindControl("hiddenType");
                var txtName = (TextBox)e.Item.FindControl("txtName");
                var txtPhone = (TextBox)e.Item.FindControl("txtPhone");
                var ddlSuppliers = (DropDownList)e.Item.FindControl("ddlSuppliers");
                var ddlGuides = (DropDownList)e.Item.FindControl("ddlGuides");
                var ddlGroups = (DropDownList)e.Item.FindControl("ddlGroups");
                var txtCost = (TextBox)e.Item.FindControl("txtCost");
                var litType = (Literal)e.Item.FindControl("litType");

                // Nếu có số lượng nhóm thì thêm vào danh sách chọn nhóm
                if (_numberOfGroups < int.MaxValue)
                {
                    for (int ii = 1; ii <= _numberOfGroups + 1; ii++)
                    {
                        ddlGroups.Items.Add(ii.ToString());
                    }
                }

                hiddenId.Value = service.Id.ToString();
                hiddenType.Value = (service.Type.Id).ToString();
                if (service.Group >= 0)
                {
                    ddlGroups.SelectedValue = service.Group.ToString();
                }
                if (service.Type.IsCustomType)
                {
                    txtName.Text = service.Name;
                    txtPhone.Text = service.Phone;
                }
                else
                {
                    txtName.Enabled = false;
                    txtPhone.Enabled = false;
                    if (service.Supplier != null)
                    {
                        txtName.Text = service.Supplier.Name;
                        txtPhone.Text = service.Supplier.Phone;
                    }
                }
                txtCost.Text = service.Cost.ToString("#,#,0.#");

                txtCost.Attributes.Add("onchange", "this.value = addCommas(this.value);");
                ddlGuides.Visible = false;

                if (service.Type.Id < 0)
                {
                    litType.Text = @"Guide";
                    ddlGuides.DataSource = Guides;
                    ddlGuides.DataTextField = "Name";
                    ddlGuides.DataValueField = "Id";
                    ddlGuides.DataBind();
                    txtName.Visible = false;
                    ddlGuides.Visible = true;
                    ddlSuppliers.Visible = false;
                    if (string.IsNullOrEmpty(txtPhone.Text))
                    {
                        txtPhone.Text = @"AUTO FROM DATABASE";
                    }
                    txtPhone.Enabled = false;
                }
                else
                {
                    litType.Text = service.Type.Name;
                    if (service.Type.IsSupplier)
                    {
                        ddlSuppliers.DataSource = Suppliers(service.Type.Id);
                    }
                    else
                    {
                        ddlSuppliers.Visible = false;
                        e.Item.FindControl("btnRemove").Visible = false;
                        e.Item.FindControl("txtCost").Visible = false;
                    }
                }

                if (ddlSuppliers.Visible)
                {
                    ddlSuppliers.DataTextField = "Name";
                    ddlSuppliers.DataValueField = "Id";
                    ddlSuppliers.DataBind();
                    ddlSuppliers.Items.Insert(0, "-- Select --");
                }

                #region -- Xử lý đặc biệt --
                if (service.Type.IsSupplier)
                {
                    if (service.Supplier != null)
                    {
                        ddlSuppliers.SelectedValue = service.Supplier.Id.ToString();
                    }
                }

                if (Request.QueryString["tripid"] == null)
                {
                    if (service.Cost == 0)
                    {
                        e.Item.Visible = false;
                    }
                }
                #endregion

                if (service.IsRemoved)
                {
                    e.Item.Visible = false;
                }
                else if (service.Type.IsSupplier)
                {
                    _totalCost += service.Cost;
                }
            }
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            Control control = ((Control)sender).Parent;
            control.Visible = false;
        }

        protected void btnAddServiceBlock_Click(object sender, EventArgs e)
        {
            Button btnAddServiceBlock = (Button)sender;
            Repeater rptServices = btnAddServiceBlock.Parent.FindControl("rptServices") as Repeater;
            IList list = rptServicesToIList(rptServices);

            SailsTrip trip = Module.TripGetById(Convert.ToInt32(btnAddServiceBlock.CommandArgument));

            //CostType costType = Module.CostTypeGetById(Convert.ToInt32(btnAddService.CommandArgument));

            int maxGroup = 1;
            foreach (ExpenseService temp in list)
            {
                maxGroup = Math.Max(temp.Group, maxGroup);
            }

            foreach (CostType costType in trip.CostTypes)
            {
                ExpenseService service = new ExpenseService();
                service.Type = costType;
                service.IsRemoved = !costType.IsSupplier;
                service.Group = maxGroup + 1;

                int index = 0;
                foreach (ExpenseService temp in list)
                {
                    index += 1;
                }

                list.Insert(index, service);
            }
            _numberOfGroups = maxGroup + 1; // khi thêm nhóm thì tăng số lượng nhóm lên 1
            rptServices.DataSource = list;
            rptServices.DataBind();
        }

        protected void btnAddService_Click(object sender, EventArgs e)
        {
            var btnAddService = (Button)sender;
            var rptServices = btnAddService.Parent.Parent.Parent.FindControl("rptServices") as Repeater;
            IList list = rptServicesToIList(rptServices);
            var service = new ExpenseService();
            CostType costType = Module.CostTypeGetById(Convert.ToInt32(btnAddService.CommandArgument));
            service.Type = costType;
            service.IsRemoved = !costType.IsSupplier;

            int index = 0;
            int maxGroup = 1;
            foreach (ExpenseService temp in list)
            {
                index += 1;
                maxGroup = Math.Max(temp.Group, maxGroup);
            }

            _numberOfGroups = maxGroup;
            list.Insert(index, service);
            rptServices.DataSource = list;
            rptServices.DataBind();
        }

        protected void rtpAddServices_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is CostType)
            {
                Button btnAddService = (Button)e.Item.FindControl("btnAddService");
                btnAddService.Text = @"Add " + ((CostType)e.Item.DataItem).Name;
            }
        }
        #endregion

        #region -- Hành trình --

        protected void rptTrips_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is SailsTrip)
            {
                var trip = (SailsTrip)e.Item.DataItem;
                var liMenu = e.Item.FindControl("liMenu") as HtmlGenericControl;
                if (liMenu != null)
                {
                    if (trip.Id.ToString() == Request.QueryString["tripid"])
                    {
                        liMenu.Attributes.Add("class", "selected");
                    }
                }

                var hplTrip = e.Item.FindControl("hplTrip") as HyperLink;
                if (hplTrip != null)
                {
                    if (_customers.ContainsKey(trip.Id))
                    {
                        hplTrip.Text = string.Format("{0}({1})", trip.Name, _customers[trip.Id]);
                    }
                    else
                    {
                        hplTrip.Text = trip.Name;
                    }
                    DateTime date = Request.QueryString["Date"] != null ? DateTime.FromOADate(Convert.ToDouble(Request.QueryString["Date"])) : DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    hplTrip.NavigateUrl = string.Format(
                        "BookingReport.aspx?NodeId={0}&SectionId={1}&Date={2}&tripid={3}", Node.Id, Section.Id,
                        date.ToOADate(), trip.Id);
                }
            }
            else
            {
                var liMenu = e.Item.FindControl("liMenu") as HtmlGenericControl;
                if (liMenu != null)
                {
                    if (Request.QueryString["tripid"] == null)
                    {
                        liMenu.Attributes.Add("class", "selected");
                    }
                }

                var hplTrip = e.Item.FindControl("hplTrip") as HyperLink;
                if (hplTrip != null)
                {
                    DateTime date = Request.QueryString["Date"] != null ? DateTime.FromOADate(Convert.ToDouble(Request.QueryString["Date"])) : DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    hplTrip.NavigateUrl = string.Format(
                        "BookingReport.aspx?NodeId={0}&SectionId={1}&Date={2}", Node.Id, Section.Id, date.ToOADate());
                }
            }
        }

        #endregion

        #region -- Others --
        protected void btnDisplay_Click(object sender, EventArgs e)
        {
            DateTime date = DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string url = string.Format("BookingReport.aspx?NodeId={0}&SectionId={1}&Date={2}", Node.Id, Section.Id,
                                       date.ToOADate());
            PageRedirect(url);
        }

        protected void rptCruiseExpense_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is SailsTrip)
            {
                var cruise = (SailsTrip)e.Item.DataItem;
                var rptServices = e.Item.FindControl("rptServices") as Repeater;
                LoadService(rptServices, cruise, _date);

                var rptAddServices = e.Item.FindControl("rptAddServices") as Repeater;
                if (rptAddServices == null) return;
                if (Request.QueryString["tripid"] != null)
                {
                    rptAddServices.DataSource = _services;
                    rptAddServices.DataBind();
                }

                var btnAddServiceBlock = (Button)e.Item.FindControl("btnAddServiceBlock");
                btnAddServiceBlock.CommandArgument = cruise.Id.ToString();
            }
            else
            {
                Literal litSTotal = e.Item.FindControl("litTotal") as Literal;
                if (litSTotal != null)
                {
                    litSTotal.Text = _totalCost.ToString();
                    litCustomerCost.Text = _customerCost.ToString();
                    litRunningCost.Text = _runningCost.ToString();
                    litSubTotal.Text = (_customerCost + _runningCost).ToString();
                    litManual.Text = litSTotal.Text;
                    litTotal.Text = (_customerCost + _runningCost + _totalCost).ToString();
                }
            }
        }
        #endregion

        #region -- Khóa --
        protected void btnLock_Click(object sender, EventArgs e)
        {
            //// Check xem nếu có lock chưa
            //Locked locked = Module.LockedCheckByDate(ActiveCruise, Date);

            //if (locked.Id > 0)
            //{
            //    Module.Delete(locked);
            //}
            //else
            //{
            //    Module.SaveOrUpdate(locked);
            //}
            //GetDataSource();
        }

        protected void btnLockIncome_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #endregion

        #region -- PRIVATE METHODS --

        protected void GetDataSource()
        {
            IList list;
            GetData(out list, true);
            rptBookingList.DataSource = list;
        }

        protected int GetData(out IList list, bool loadService)
        {
            // Điều kiện bắt buộc: chưa xóa và có status là Approved
            ICriterion criterion = Expression.Eq(Booking.DELETED, false);
            criterion = Expression.And(criterion, Expression.Eq(Booking.STATUS, StatusType.Approved));

            // Không bao gồm booking đã transfer
            criterion = Expression.And(criterion, Expression.Not(Expression.Eq("IsTransferred", true)));

            if (string.IsNullOrEmpty(txtDate.Text))
            {
                _date = DateTime.Today;
            }
            else
            {
                _date = Request.QueryString["Date"] != null ? DateTime.FromOADate(Convert.ToDouble(Request.QueryString["Date"])) : DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            criterion = Module.AddDateExpression(criterion, _date);
            ICriterion airportTransferCriterion = null;
            ICriterion pickupTimeCriterion = null;
            pickupTimeCriterion = Expression.And(Expression.Ge("PickupTime", _date), Expression.Le("PickupTime", _date.Add(new TimeSpan(23, 59, 59))));
            ICriterion seeOffTimeCriterion = null;
            seeOffTimeCriterion = Expression.And(Expression.Ge("SeeoffTime", _date), Expression.Le("SeeoffTime", _date.Add(new TimeSpan(23, 59, 59))));
            ICriterion pickUpTimeSeeOffTimeCriterion = null;
            pickUpTimeSeeOffTimeCriterion = Expression.Or(pickupTimeCriterion, seeOffTimeCriterion);
            airportTransferCriterion = pickUpTimeSeeOffTimeCriterion;
            criterion = Expression.Or(criterion, airportTransferCriterion);
            int count;

            // Lấy về danh sách chưa có điều kiện hành trình để tính số khách
            IList bookings = Module.BookingGetByCriterion(criterion, null, out count, 0, 0, false, UserIdentity);
            var filteredBookings1 = new List<Booking>();
            foreach (Booking booking in bookings)
            {
                if (booking.Trip.Name.ToLower() != "airport transfer")
                {
                    if (booking.StartDate <= _date && booking.EndDate > _date)
                    {
                        filteredBookings1.Add(booking);
                        continue;
                    }

                    if (booking.StartDate == _date && booking.StartDate >= booking.EndDate)
                    {
                        filteredBookings1.Add(booking);
                        continue;
                    }
                }

                if (booking.Trip.Name.ToLower() == "airport transfer")
                {
                    if (booking.PickupTime >= _date && booking.PickupTime <= _date.Add(new TimeSpan(23, 59, 59)))
                    {
                        filteredBookings1.Add(booking);
                        continue;
                    }

                    if (booking.SeeoffTime >= _date && booking.SeeoffTime <= _date.Add(new TimeSpan(23, 59, 59)))
                    {
                        filteredBookings1.Add(booking);
                        continue;
                    }

                }
            }
            bookings = filteredBookings1;
            foreach (Booking booking in bookings)
            {
                if (!_customers.ContainsKey(booking.Trip.Id))
                {
                    _customers.Add(booking.Trip.Id, 0);
                }
                _customers[booking.Trip.Id] += booking.Pax;

                if (booking.Trip.Organization != null)
                {
                    if (!_customerByOrg.ContainsKey(booking.Trip.Organization.Id))
                    {
                        _customerByOrg.Add(booking.Trip.Organization.Id, 0);
                    }
                    _customerByOrg[booking.Trip.Organization.Id] += booking.Pax;
                }
            }

            // Điều kiện về hành trình, ưu tiên điều kiện hành trình hơn điều kiện org
            bool tripped = false;
            if (Request.QueryString["tripid"] != null)
            {
                SailsTrip cruise = Module.TripGetById(Convert.ToInt32(Request.QueryString["tripid"]));
                criterion = Expression.And(criterion, Expression.Eq("Trip", cruise));
                tripped = true;
            }

            if (!tripped && Request.QueryString["orgid"] != null)
            {
                Organization org = Module.OrganizationGetById(Convert.ToInt32(Request.QueryString["orgid"]));
                criterion = Expression.And(criterion, Expression.Eq("trip.Organization", org));
                tripped = true;
            }

            list = Module.BookingGetByCriterion(criterion, Order.Asc("Group"), out count, 0, 0, tripped, UserIdentity);
            var filteredBookings = new List<Booking>();
            foreach (Booking booking in list)
            {
                if (booking.Trip.Name.ToLower() != "airport transfer")
                {
                    if (booking.StartDate <= _date && booking.EndDate > _date)
                    {
                        filteredBookings.Add(booking);
                        continue;
                    }

                    if (booking.StartDate == _date && booking.StartDate >= booking.EndDate)
                    {
                        filteredBookings.Add(booking);
                        continue;
                    }
                }

                if (booking.Trip.Name.ToLower() == "airport transfer")
                {
                    if (booking.PickupTime >= _date && booking.PickupTime <= _date.Add(new TimeSpan(23, 59, 59)))
                    {
                        filteredBookings.Add(booking);
                        continue;
                    }

                    if (booking.SeeoffTime >= _date && booking.SeeoffTime <= _date.Add(new TimeSpan(23, 59, 59)))
                    {
                        filteredBookings.Add(booking);
                        continue;
                    }

                }
            }

            list = filteredBookings;

            if (Request.QueryString["tripid"] != null)
            {
                SailsTrip trip = Module.TripGetById(Convert.ToInt32(Request.QueryString["tripid"]));
            }

            // Nếu cần load service và cruise hiện tại khác null
            if (loadService)
            {
                LoadService(_date);
            }

            //plhNote.Visible = ActiveCruise != null;
            return count;
        }

        /// <summary>
        /// Load về dịch vụ phát sinh trong ngày
        /// </summary>
        /// <param name="date"></param>
        private void LoadService(DateTime date)
        {
            #region -- Lấy dữ liệu trip hiện tại, org, ... --
            // Tính số lượng nhóm
            _numberOfGroups = 0;
            IList trips;

            if (Request.QueryString["orgid"] == null)
            {
                if (ActiveTrip == null)
                {
                    trips = Module.TripGetAll(true, UserIdentity);
                    _numberOfGroups = int.MaxValue;
                }
                else
                {
                    trips = new ArrayList();
                    trips.Add(ActiveTrip);
                }
            }
            else
            {
                trips = Module.TripGetByOrganization(
                        Module.OrganizationGetById(Convert.ToInt32(Request.QueryString["orgid"])));
            }

            _date = date;
            #endregion

            // Tạo một expense chung không theo hành trình nào, sau đó tính toán chi phí để phát sinh toàn bộ chi phí cho các tàu
            SailExpense expense = Module.ExpenseGetByDate(null, date);
            if (expense.Id < 0)
            {
                Module.SaveOrUpdate(expense);
            }

            var calculator = new ExpenseCalculator(Module, PartnershipManager);

            _customerCost = 0;
            _runningCost = 0;
            Dictionary<CostType, double> cost = calculator.ExpenseCalculate(null, expense); // Luôn tính toán chi phí với trip = null --> tính mọi chi phí phát sinh
            if (cost != null)
            {
                foreach (KeyValuePair<CostType, double> pair in cost)
                {
                    if (pair.Key.IsSupplier && !pair.Key.IsDailyInput && !pair.Key.IsDaily && !pair.Key.IsMonthly &&
                        !pair.Key.IsYearly)
                    {
                        _customerCost += pair.Value;
                    }
                    else if (pair.Key.IsSupplier && !pair.Key.IsDailyInput && pair.Key.IsDaily)
                    {
                        _runningCost += pair.Value;
                    }
                }
            }

            if (DailyCost.Count > 0)
            {
                rptCruiseExpense.DataSource = trips;
                rptCruiseExpense.DataBind();
            }
            else
            {
                plhDailyExpenses.Visible = false;
                rptCruiseExpense.Visible = false;
            }
        }

        /// <summary>
        /// Load thông tin các chi phí nhập thủ công theo ngày
        /// </summary>
        /// <param name="rptServices"></param>
        /// <param name="cruise"></param>
        /// <param name="date"></param>
        private void LoadService(Repeater rptServices, SailsTrip cruise, DateTime date)
        {
            #region -- Load service --

            SailExpense expense = Module.ExpenseGetByDate(cruise, date);

            ddlOperators.DataSource = Operators;
            ddlOperators.DataTextField = "UserName";
            ddlOperators.DataValueField = "Id";
            ddlOperators.DataBind();
            ddlOperators.Items.Insert(0, "-- Please choose one --");

            if (expense.Operator != null)
            {
                var listitem = ddlOperators.Items.FindByValue(expense.Operator.Id.ToString());
                if (listitem != null)
                {
                    ddlOperators.SelectedValue = expense.Operator.Id.ToString();
                    txtOperator.ReadOnly = true;
                    txtPhone.ReadOnly = true;
                }
            }
            txtOperator.Text = expense.OperatorName;
            txtPhone.Text = expense.OperatorPhone;
            Role role = Module.RoleGetById(Convert.ToInt32(Module.ModuleSettings("Sale")));
            ddlSaleInCharge.DataSource = role.Users;

            ddlSaleInCharge.DataTextField = "UserName";
            ddlSaleInCharge.DataValueField = "Id";
            ddlSaleInCharge.DataBind();
            ddlSaleInCharge.Items.Insert(0, "-- Not selected --");

            if (expense.SaleInCharge != null)
            {
                ddlSaleInCharge.SelectedValue = expense.SaleInCharge.Id.ToString();
                txtSalePhone.Text = expense.SaleInCharge.Website;
            }

            var serviceMap = new Dictionary<CostType, bool>();

            // Lập danh sách toàn bộ các dịch vụ nhập
            foreach (CostType type in DailyCost)
            {
                serviceMap.Add(type, false);
            }

            IList services = new ArrayList();

            // Lấy dịch vụ trong cơ sở dữ liệu vào, tính số nhóm = nhóm của dịch vụ có nhóm lớn nhất
            foreach (ExpenseService service in expense.Services)
            {
                try
                {
                    if (service.Type.IsDailyInput && !service.Type.IsMonthly && !service.Type.IsYearly)
                    {
                        serviceMap[service.Type] = true;
                        services.Add(service);
                        _numberOfGroups = Math.Max(_numberOfGroups, service.Group);
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }

            // Bổ sung dịch vụ còn chưa có trong CSDL
            foreach (CostType type in DailyCost)
            {
                if (!serviceMap[type] && cruise.CostTypes.Contains(type))
                {
                    ExpenseService service = new ExpenseService();
                    service.Type = type;
                    service.IsOwnService = !type.IsSupplier;
                    services.Add(service);
                }
            }

            int count = 0;
            foreach (ExpenseService service in services)
            {
                if (service.Cost > 0 && service.Type.IsDailyInput)
                {
                    count++;
                    break;
                }
            }

            // Nếu không có service thì không hiển thị (chỉ trong trường hợp nhiều trip)
            if (count > 0 || Request.QueryString["tripid"] != null)
            {
                rptServices.DataSource = services;
                rptServices.DataBind();
            }
            else
            {
                rptServices.Parent.Visible = false;
            }

            #endregion

            #region -- Load note --

            //if (ActiveCruise != null)
            //{
            //    DayNote note = Module.DayNoteGetByDay(ActiveCruise, date);
            //    fckNote.Value = note.Note;
            //}
            //else
            //{
            //    plhNote.Visible = false;
            //}

            #endregion
        }

        protected IList rptServicesToIList(Repeater rptServices)
        {
            IList list = new ArrayList();
            foreach (RepeaterItem item in rptServices.Items)
            {
                HiddenField hiddenId = (HiddenField)item.FindControl("hiddenId");
                HiddenField hiddenType = (HiddenField)item.FindControl("hiddenType");
                TextBox txtName = (TextBox)item.FindControl("txtName");
                TextBox txtPhone = (TextBox)item.FindControl("txtPhone");
                DropDownList ddlSuppliers = (DropDownList)item.FindControl("ddlSuppliers");
                DropDownList ddlGuides = (DropDownList)item.FindControl("ddlGuides");
                DropDownList ddlGroups = (DropDownList)item.FindControl("ddlGroups");
                TextBox txtCost = (TextBox)item.FindControl("txtCost");

                int serviceId = Convert.ToInt32(hiddenId.Value);

                ExpenseService service;
                if (serviceId <= 0)
                {
                    service = new ExpenseService();
                }
                else
                {
                    service = Module.ExpenseServiceGetById(serviceId);
                }
                service.Type = Module.CostTypeGetById(Convert.ToInt32(hiddenType.Value));
                service.Name = txtName.Text;
                service.Phone = txtPhone.Text;
                if (ddlSuppliers.SelectedIndex > 0)
                {
                    service.Supplier = Module.AgencyGetById(Convert.ToInt32(ddlSuppliers.SelectedValue));
                }
                else
                {
                    service.Supplier = null;
                }
                service.IsOwnService = !service.Type.IsSupplier;
                service.Cost = Convert.ToDouble(txtCost.Text);
                service.IsRemoved = !item.Visible;
                if (ddlGroups.SelectedIndex >= 0)
                {
                    service.Group = Convert.ToInt32(ddlGroups.SelectedValue);
                }
                list.Add(service);
            }
            return list;
        }

        #endregion

        protected void rptOrganization_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is Organization)
            {
                var organization = (Organization)e.Item.DataItem;
                var liMenu = e.Item.FindControl("liMenu") as HtmlGenericControl;
                if (liMenu != null)
                {
                    if (organization.Id.ToString() == Request.QueryString["orgid"])
                    {
                        liMenu.Attributes.Add("class", "selected");
                    }
                }

                var hplOrganization = e.Item.FindControl("hplOrganization") as HyperLink;
                if (hplOrganization != null)
                {
                    DateTime date = Request.QueryString["Date"] != null ? DateTime.FromOADate(Convert.ToDouble(Request.QueryString["Date"])) : DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    hplOrganization.Text = organization.Name;
                    if (_customerByOrg.ContainsKey(organization.Id))
                    {
                        hplOrganization.Text += string.Format(" ({0})", _customerByOrg[organization.Id]);
                    }
                    hplOrganization.NavigateUrl = string.Format(
                        "BookingReport.aspx?NodeId={0}&SectionId={1}&Date={2}&orgid={3}", Node.Id, Section.Id,
                        date.ToOADate(), organization.Id);
                }

                var rptTrips = e.Item.FindControl("rptTrips") as Repeater;
                if (rptTrips != null)
                {
                    rptTrips.DataSource = Module.TripGetByOrganization(organization);
                    rptTrips.DataBind();
                }
            }
            else
            {
                var liMenu = e.Item.FindControl("liMenu") as HtmlGenericControl;
                if (liMenu != null)
                {
                    if (Request.QueryString["orgid"] == null && Request.QueryString["tripid"] == null)
                    {
                        liMenu.Attributes.Add("class", "selected");
                    }
                }

                var hplOrganization = e.Item.FindControl("hplOrganization") as HyperLink;
                if (hplOrganization != null)
                {
                    DateTime date = Request.QueryString["Date"] != null ? DateTime.FromOADate(Convert.ToDouble(Request.QueryString["Date"])) : DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    hplOrganization.NavigateUrl = string.Format(
                        "BookingReport.aspx?NodeId={0}&SectionId={1}&Date={2}", Node.Id, Section.Id, date.ToOADate());
                }
            }
        }
    }
}