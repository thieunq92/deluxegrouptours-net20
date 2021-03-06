using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Web.Util;
using log4net;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.ReportEngine;
using Portal.Modules.OrientalSails.Web.Controls;
using Portal.Modules.OrientalSails.Web.UI;
using Portal.Modules.OrientalSails.Web.Util;
using CMS.Core.Domain;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class BookingHistories : SailsAdminBasePage
    {
        #region -- PRIVATE MEMBERS --

        private readonly ILog _logger = LogManager.GetLogger(typeof(BookingHistories));

        private BookingHistory _prev;
        #endregion

        #region -- Page Event --

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var booking = Module.BookingGetById(Convert.ToInt32(Request.QueryString["bookingid"]));
                var histories = Module.BookingGetHistory(booking);

                _prev = null;
                rptAgencies.DataSource = histories;
                rptAgencies.DataBind();

                _prev = null;
                rptDates.DataSource = histories;
                rptDates.DataBind();

                _prev = null;
                rptStatus.DataSource = histories;
                rptStatus.DataBind();

                _prev = null;
                rptTrips.DataSource = histories;
                rptTrips.DataBind();

                _prev = null;
                rptSpecialRequest.DataSource = histories;
                rptSpecialRequest.DataBind();
            }
            catch (Exception ex)
            {
                _logger.Error("Error when Page_Load in BookingView", ex);
                ShowError(ex.Message);
            }
        }

        #endregion

        protected void rptHistory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected void rptDates_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is BookingHistory)
            {
                var history = (BookingHistory)e.Item.DataItem;

                if (_prev != null)
                {
                    if (_prev.StartDate == history.StartDate)
                    {
                        e.Item.Visible = false;
                        return;
                    }
                }

                try
                {
                    ValueBinder.BindLiteral(e.Item, "litTime", history.Date.ToString("dd-MMM-yyyy HH:mm"));
                    ValueBinder.BindLiteral(e.Item, "litUser", history.User.FullName);
                    ValueBinder.BindLiteral(e.Item, "litTo", history.StartDate.ToString("dd/MM/yyyy"));
                }
                catch (Exception) { }

                if (_prev != null)
                {
                    try
                    {
                        ValueBinder.BindLiteral(e.Item, "litFrom", _prev.StartDate.ToString("dd/MM/yyyy"));
                    }
                    catch (Exception) { }
                }
                _prev = history;
            }
        }

        protected void rptStatus_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is BookingHistory)
            {
                var history = (BookingHistory)e.Item.DataItem;

                if (_prev != null)
                {
                    if (_prev.Status == history.Status)
                    {
                        e.Item.Visible = false;
                        return;
                    }
                }
                try
                {
                    ValueBinder.BindLiteral(e.Item, "litTime", history.Date.ToString("dd-MMM-yyyy HH:mm"));
                    ValueBinder.BindLiteral(e.Item, "litUser", history.User.FullName);
                    ValueBinder.BindLiteral(e.Item, "litTo", history.Status.ToString());
                }
                catch (Exception) { }

                if (_prev != null)
                {
                    try
                    {
                        ValueBinder.BindLiteral(e.Item, "litFrom", _prev.Status.ToString());
                    }
                    catch (Exception) { }
                }
                _prev = history;
            }
        }

        protected void rptTrips_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is BookingHistory)
            {
                var history = (BookingHistory)e.Item.DataItem;

                if (_prev != null)
                {
                    if (_prev.Trip.Id == history.Trip.Id)
                    {
                        e.Item.Visible = false;
                        return;
                    }
                }

                try
                {
                    ValueBinder.BindLiteral(e.Item, "litTime", history.Date.ToString("dd-MMM-yyyy HH:mm"));
                    ValueBinder.BindLiteral(e.Item, "litUser", history.User.FullName);
                    ValueBinder.BindLiteral(e.Item, "litTo", history.Trip.Name);
                }
                catch (Exception) { }

                if (_prev != null)
                {
                    try
                    {
                        ValueBinder.BindLiteral(e.Item, "litFrom", _prev.Trip.Name);
                    }
                    catch (Exception) { }
                }
                _prev = history;
            }
        }

        protected void rptAgencies_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is BookingHistory)
            {
                var history = (BookingHistory)e.Item.DataItem;

                if (_prev != null)
                {
                    try
                    {
                        if (_prev.Agency.Id == history.Agency.Id)
                        {
                            e.Item.Visible = false;
                            return;
                        }
                    }
                    catch (Exception) { }
                }

                try
                {
                    ValueBinder.BindLiteral(e.Item, "litTime", history.Date.ToString("dd-MMM-yyyy HH:mm"));
                    ValueBinder.BindLiteral(e.Item, "litUser", history.User.FullName);
                    ValueBinder.BindLiteral(e.Item, "litTo", history.Agency.Name);
                }
                catch (Exception) { }

                if (_prev != null)
                {
                    try
                    {
                        ValueBinder.BindLiteral(e.Item, "litFrom", _prev.Agency.Name);
                    }
                    catch (Exception) { }
                }
                _prev = history;
            }
        }

        protected void rptTotals_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is BookingHistory)
            {
                var history = (BookingHistory)e.Item.DataItem;

                if (_prev != null)
                {
                    if (_prev.Total == history.Total && _prev.TotalCurrency == history.TotalCurrency)
                    {
                        e.Item.Visible = false;
                        return;
                    }
                }

                try
                {
                    ValueBinder.BindLiteral(e.Item, "litTime", history.Date.ToString("dd-MMM-yyyy HH:mm"));
                    ValueBinder.BindLiteral(e.Item, "litUser", history.User.FullName);
                }
                catch (Exception) { }

                ValueBinder.BindLiteral(e.Item, "litTo", history.Total + history.TotalCurrency);

                if (_prev != null)
                {
                    ValueBinder.BindLiteral(e.Item, "litFrom", _prev.Total + _prev.TotalCurrency);
                }
                _prev = history;
            }
        }

        protected void rptSpecialRequest_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is BookingHistory)
            {
                var history = (BookingHistory)e.Item.DataItem;

                if (_prev != null)
                {
                    if (_prev.SpecialRequest == history.SpecialRequest)
                    {
                        e.Item.Visible = false;
                        return;
                    }
                }

                try
                {
                    ValueBinder.BindLiteral(e.Item, "litTime", history.Date.ToString("dd-MMM-yyyy HH:mm"));
                    ValueBinder.BindLiteral(e.Item, "litUser", history.User.FullName);
                }
                catch (Exception) { }
                ValueBinder.BindLiteral(e.Item, "litTo", history.SpecialRequest);

                if (_prev != null)
                {
                    ValueBinder.BindLiteral(e.Item, "litFrom", _prev.SpecialRequest);
                }
                _prev = history;
            }
        }
    }
}