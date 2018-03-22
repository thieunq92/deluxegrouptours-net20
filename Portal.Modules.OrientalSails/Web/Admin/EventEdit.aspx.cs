using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CMS.Web.Util;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;
using System.Collections;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class EventEdit : SailsAdminBasePage
    {
        #region -- PRIVATE MEMBERS --

        private double _expenseTotal;
        private double _expensePaid;
        private double _payable;

        private double _total;
        private double _paid;
        private double _paidBase;
        private double _receivable;
        private double _child;
        private double _baby;
        private double _adult;

        #endregion

        #region -- PAGE EVENTS --
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["expenseid"] != null && Request.QueryString["group"] != null)
                {
                    var expense = Module.ExpenseGetById(Convert.ToInt32(Request.QueryString["expenseid"]));
                    int group = Convert.ToInt32(Request.QueryString["group"]);

                    var code = new EventCode(Module) { SailExpense = expense, Group = group };

                    litDate.Text = expense.Date.ToString("dd/MM/yyyy");
                    titleBookingView.Text = string.Format("{0}{1:ddMMyy}-{2:00}", expense.Trip.TripCode, expense.Date, group);
                    litTrip.Text = expense.Trip.Name;

                    int pax = 0;
                    double revenuePaid = 0;
                    double revenueReceivable = 0;
                    double revenueTotal = 0;

                    double expensePaid = 0;
                    double expensePayable = 0;
                    double expenseTotal = 0;
                    foreach (Booking booking in code.Bookings)
                    {
                        pax += booking.Pax;

                        double paid = booking.Total * booking.CurrencyRate - booking.TotalReceivable;
                        revenuePaid += paid;
                        revenueReceivable += booking.TotalReceivable;
                        revenueTotal += booking.Total * booking.CurrencyRate;
                    }

                    foreach (ExpenseService service in code.Services)
                    {
                        expensePaid += service.Paid;
                        expensePayable += service.Cost - service.Paid;
                        expenseTotal += service.Cost;
                    }

                    litPax.Text = pax.ToString();
                    litRevenueTotal.Text = revenueTotal.ToString("#,0.#");
                    litRevenuePaid.Text = revenuePaid.ToString("#,0.#");
                    litReceivable.Text = revenueReceivable.ToString("#,0.#");
                    litPayableTotal.Text = expenseTotal.ToString("#,0.#");
                    litPayablePaid.Text = expensePaid.ToString("#,0.#");
                    litPayable.Text = expensePayable.ToString("#,0.#");
                    litExpected.Text = (revenueTotal - expenseTotal).ToString("#,0.#");
                    litRealCash.Text = (revenuePaid - expensePaid).ToString("#,0.#");

                    rptExpenseServices.DataSource = code.Services;
                    rptExpenseServices.DataBind();

                    rptBookingList.DataSource = code.Bookings;
                    rptBookingList.DataBind();
                }
            }
        }

        #endregion

        #region -- CONTROL EVENTS --
        protected void rptExpenseServices_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is ExpenseService)
            {
                var service = (ExpenseService)e.Item.DataItem;

                //if (service.Cost == 0 || service.Type.IsMonthly || service.Type.IsYearly)
                if (service.Type.IsMonthly || service.Type.IsYearly)
                {
                    e.Item.Visible = false;
                    return;
                }
                var hplDate = e.Item.FindControl("hplDate") as HyperLink;
                if (hplDate != null)
                {
                    hplDate.Text = service.Expense.Date.ToString("dd/MM/yyyy");
                }

                var hplTripCode = e.Item.FindControl("hplTripCode") as HyperLink;
                if (hplTripCode != null)
                {
                    hplTripCode.Text = string.Format("{0}{1}-{2:00}", service.Expense.Trip.TripCode, service.Expense.Date.ToString("ddMMyy"), service.Group);
                    hplTripCode.NavigateUrl =
                        string.Format("EventEdit.aspx?NodeId={0}&SectionId={1}&expenseid={2}&group={3}", Node.Id,
                                      Section.Id, service.Expense.Id, service.Group);
                }

                var hplPartner = e.Item.FindControl("hplPartner") as HyperLink;
                if (hplPartner != null)
                {
                    if (service.Supplier != null)
                    {
                        hplPartner.Text = service.Supplier.Name;
                        hplPartner.NavigateUrl =
                            string.Format("PayableList.aspx?NodeId={0}&SectionId={1}&from={3}&to={4}&supplierid={2}", Node.Id, Section.Id,
                                          service.Supplier.Id, Request.QueryString["from"], Request.QueryString["to"]);
                    }
                }

                var hplService = e.Item.FindControl("hplService") as HyperLink;
                if (hplService != null)
                {
                    hplService.Text = service.Type.Name;
                    hplService.NavigateUrl = string.Format("PayableList.aspx?NodeId={0}&SectionId={1}", Node.Id, Section.Id);
                }

                var litTotal = e.Item.FindControl("litTotal") as Literal;
                if (litTotal != null)
                {
                    litTotal.Text = service.Cost.ToString("#,0.#");
                    _expenseTotal += service.Cost;
                }

                Literal litPaid = e.Item.FindControl("litPaid") as Literal;
                if (litPaid != null)
                {
                    litPaid.Text = service.Paid.ToString("#,0.#");
                    _expensePaid += service.Paid;
                }

                Literal litPayable = e.Item.FindControl("litPayable") as Literal;
                if (litPayable != null)
                {
                    litPayable.Text = (service.Cost - service.Paid).ToString("#,0.#");
                    _payable += service.Cost - service.Paid;
                }

                if (service.PaidDate.HasValue)
                {
                    ValueBinder.BindLiteral(e.Item, "litPaidOn", service.PaidDate.Value);
                }
            }

            if (e.Item.ItemType == ListItemType.Footer)
            {
                Literal litTotal = e.Item.FindControl("litTotal") as Literal;
                if (litTotal != null)
                {
                    litTotal.Text = _expenseTotal.ToString("#,0.#");
                }

                Literal litPaid = e.Item.FindControl("litPaid") as Literal;
                if (litPaid != null)
                {
                    litPaid.Text = _expensePaid.ToString("#,0.#");
                }

                Literal litPayable = e.Item.FindControl("litPayable") as Literal;
                if (litPayable != null)
                {
                    litPayable.Text = _payable.ToString("#,0.#");
                }
            }
        }

        protected void rptExpenseServices_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Footer)
            {
                ExpenseService service = Module.ExpenseServiceGetById(Convert.ToInt32(e.CommandArgument));
                TextBox txtPay = (TextBox)e.Item.FindControl("txtPay");
                double paid;

                if (!string.IsNullOrEmpty(txtPay.Text))
                {
                    paid = Convert.ToDouble(txtPay.Text);
                }
                else
                {
                    paid = service.Cost - service.Paid;
                }

                service.Paid += paid;

                if (service.Paid == service.Cost && !service.PaidDate.HasValue)
                {
                    service.PaidDate = DateTime.Now;
                }
                var transaction = new Transaction();
                transaction.CreatedBy = UserIdentity;
                transaction.CreatedDate = DateTime.Now;
                transaction.Agency = service.Supplier;
                transaction.IsExpense = true;
                transaction.VNDAmount = paid;

                if (service.Type.IsDailyInput)
                {
                    transaction.TransactionType = Transaction.MANUALDAILY_EXPENSE;
                }
                else
                {
                    transaction.TransactionType = Transaction.CALCULATED_EXPENSE;
                }

                Module.SaveOrUpdate(transaction);
                Module.SaveOrUpdate(service);
                PageRedirect(Request.RawUrl);
            }
            else
            {
                IList list = new ArrayList();//Module.ExpenseServiceGet(null, from, to, agency, null, Request.QueryString["orgid"], UserIdentity, true, type);
                foreach (ExpenseService service in list)
                {
                    service.Paid = service.Cost;
                    Module.SaveOrUpdate(service);
                }
                rptExpenseServices.DataSource = list;
                rptExpenseServices.DataBind();
            }
        }
        #endregion

        protected void rptBookingList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is Booking)
            {
                var booking = (Booking)e.Item.DataItem;

                var hplCode = e.Item.FindControl("hplCode") as HyperLink;
                if (hplCode != null)
                {
                    hplCode.Text = string.Format(BookingFormat, booking.Id);
                    hplCode.NavigateUrl = string.Format("BookingView.aspx?NodeId={0}&SectionId={1}&BookingId={2}",
                                                        Node.Id, Section.Id, booking.Id);
                }

                var hplCruise = e.Item.FindControl("hplCruise") as HyperLink;
                if (hplCruise != null)
                {
                    if (booking.Cruise != null)
                    {
                        hplCruise.Text = booking.Cruise.Name;
                        hplCruise.NavigateUrl = string.Format("PaymentReport.aspx?NodeId={0}&SectionId={1}&cruiseid={2}", Node.Id,
                                              Section.Id,
                                              booking.Cruise.Id);
                    }
                }

                #region -- Others --

                Literal litDate = e.Item.FindControl("litDate") as Literal;
                if (litDate != null)
                {
                    litDate.Text = booking.StartDate.ToString("dd/MM/yyyy");
                }

                Literal litService = e.Item.FindControl("litService") as Literal;
                if (litService != null)
                {
                    litService.Text = booking.Trip.TripCode;
                }

                #endregion

                #region -- Number of pax --

                Label label_NoOfAdult = e.Item.FindControl("label_NoOfAdult") as Label;
                if (label_NoOfAdult != null)
                {
                    label_NoOfAdult.Text = booking.Adult.ToString();
                }

                Label label_NoOfChild = e.Item.FindControl("label_NoOfChild") as Label;
                if (label_NoOfChild != null)
                {
                    label_NoOfChild.Text = booking.Child.ToString();
                }

                Label label_NoOfBaby = e.Item.FindControl("label_NoOfBaby") as Label;
                if (label_NoOfBaby != null)
                {
                    label_NoOfBaby.Text = booking.Baby.ToString();
                }

                _adult += booking.Adult;
                _child += booking.Child;
                _baby += booking.Baby;

                #endregion

                ValueBinder.BindLiteral(e.Item, "litGuideCollect", booking.GuideCollect);

                #region -- Paid/Total --

                Label label_TotalPrice = e.Item.FindControl("label_TotalPrice") as Label;
                if (label_TotalPrice != null)
                {
                    if (booking.Value > 0)
                    {
                        label_TotalPrice.Text = booking.Value.ToString("#,###");
                    }
                    else
                    {
                        label_TotalPrice.Text = "0";
                    }
                }

                #endregion

                #region -- Editable --

                var aPayment = e.Item.FindControl("aPayment") as HtmlAnchor;
                if (aPayment != null)
                {
                    string url = string.Format("BookingPayment.aspx?NodeId={0}&SectionId={1}&BookingId={2}", Node.Id,
                                               Section.Id, booking.Id);
                    aPayment.Attributes.Add("onclick",
                                            CMS.ServerControls.Popup.OpenPopupScript(url, "Payment", 600, 500));
                }

                var litPaid = e.Item.FindControl("litPaid") as Literal;
                if (litPaid != null)
                {
                    litPaid.Text = booking.Paid.ToString("#,0.#");
                }

                Literal litPaidBase = e.Item.FindControl("litPaidBase") as Literal;
                if (litPaidBase != null)
                {
                    litPaidBase.Text = booking.PaidBase.ToString("#,0.#");
                }

                Literal litCurrentRate = e.Item.FindControl("litCurrentRate") as Literal;
                if (litCurrentRate != null)
                {
                    if (booking.CurrencyRate > 0)
                    {
                        litCurrentRate.Text = booking.CurrencyRate.ToString("#,0.#");
                    }
                    else
                    {
                        booking.CurrencyRate = GetCurrentRate();
                        litCurrentRate.Text = booking.CurrencyRate.ToString("#,0.#");
                    }
                }

                Literal litReceivable = e.Item.FindControl("litReceivable") as Literal;
                if (litReceivable != null)
                {
                    litReceivable.Text = booking.TotalReceivable.ToString("#,0.#");
                }
                _total += booking.Value;
                _paid += booking.Paid;
                _paidBase += booking.PaidBase;
                _receivable += booking.TotalReceivable;

                #endregion

                #region -- Tô màu --

                HtmlTableRow trItem = e.Item.FindControl("trItem") as HtmlTableRow;
                if (trItem != null)
                {
                    string color = string.Empty;
                    if (booking.Agency != null && booking.Agency.PaymentPeriod != PaymentPeriod.Monthly)
                    {
                        color = SailsModule.WARNING;
                    }
                    if (booking.IsPaymentNeeded)
                    {
                        color = SailsModule.IMPORTANT;
                    }
                    if (booking.IsPaid)
                    {
                        color = SailsModule.GOOD;
                    }
                    if (!string.IsNullOrEmpty(color))
                    {
                        trItem.Attributes.Add("style", "background-color:" + color);
                    }
                }

                #endregion

                Literal litSaleInCharge = e.Item.FindControl("litSaleInCharge") as Literal;
                if (litSaleInCharge != null)
                {
                    if (booking.Agency != null && booking.Agency.Sale != null)
                    {
                        litSaleInCharge.Text = booking.Agency.Sale.UserName;
                    }
                }

                Literal litTACode = e.Item.FindControl("litTACode") as Literal;
                if (litTACode != null)
                {
                    if (!string.IsNullOrEmpty(booking.AgencyCode))
                    {
                        litTACode.Text = booking.AgencyCode;
                    }
                }

                if (booking.PaidDate.HasValue)
                {
                    ValueBinder.BindLiteral(e.Item, "litPaidOn", booking.PaidDate.Value);
                }
            }
            else
            {
                if (e.Item.ItemType == ListItemType.Footer)
                {
                    #region -- get control --

                    Label label_NoOfAdult = (Label)e.Item.FindControl("label_NoOfAdult");
                    Label label_NoOfChild = (Label)e.Item.FindControl("label_NoOfChild");
                    Label label_NoOfBaby = (Label)e.Item.FindControl("label_NoOfBaby");
                    Label label_TotalPrice = (Label)e.Item.FindControl("label_TotalPrice");
                    Literal litPaid = (Literal)e.Item.FindControl("litPaid");
                    Literal litPaidBase = (Literal)e.Item.FindControl("litPaidBase");
                    Literal litReceivable = (Literal)e.Item.FindControl("litReceivable");

                    #endregion

                    #region -- set value --

                    label_NoOfAdult.Text = _adult.ToString();
                    label_NoOfChild.Text = _child.ToString();
                    label_NoOfBaby.Text = _baby.ToString();
                    label_TotalPrice.Text = _total.ToString("#,###");
                    if (_paid > 0)
                    {
                        litPaid.Text = _paid.ToString("#,###");
                    }
                    else
                    {
                        litPaid.Text = "0";
                    }

                    litPaidBase.Text = _paidBase.ToString("#,0.#");

                    if (_receivable > 0)
                    {
                        litReceivable.Text = _receivable.ToString("#,###");
                    }
                    else
                    {
                        litReceivable.Text = "0";
                    }

                    #endregion

                    HtmlAnchor aPayment = e.Item.FindControl("aPayment") as HtmlAnchor;
                    if (aPayment != null)
                    {
                        string url;
                        if (Request.QueryString["agencyid"] != null)
                        {
                            url =
                                string.Format(
                                    "BookingPayment.aspx?NodeId={0}&SectionId={1}&agencyid={4}&from={2}&to={3}",
                                    Node.Id, Section.Id, Request.QueryString["from"],
                                    Request.QueryString["to"], Request.QueryString["agencyid"]);
                        }
                        else if (Request.QueryString["agencyname"] != null)
                        {
                            url =
                                string.Format(
                                    "BookingPayment.aspx?NodeId={0}&SectionId={1}&agencyname={4}&from={2}&to={3}",
                                    Node.Id, Section.Id, Request.QueryString["from"],
                                    Request.QueryString["to"], Request.QueryString["agencyname"]);
                        }
                        else
                        {
                            url = string.Format("BookingPayment.aspx?NodeId={0}&SectionId={1}&from={2}&to={3}", Node.Id,
                                                Section.Id, Request.QueryString["from"],
                                                Request.QueryString["to"]);
                            aPayment.Visible = false;
                        }
                        aPayment.Attributes.Add("onclick",
                                                CMS.ServerControls.Popup.OpenPopupScript(url, "Payment", 300, 400));
                    }
                }
            }
        }

        private double _currentRate;
        protected double GetCurrentRate()
        {
            if (_currentRate <= 0)
            {
                _currentRate = Module.ExchangeGetByDate(DateTime.Now).Rate;
            }
            return _currentRate;
        }
    }
}