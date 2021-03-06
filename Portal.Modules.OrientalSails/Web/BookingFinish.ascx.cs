using System;
using System.Collections;
using System.Web.UI.WebControls;
using CMS.Core.Domain;
using CMS.Web.UI;
using CMS.Web.Util;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.Util;

namespace Portal.Modules.OrientalSails.Web
{
    public partial class BookingFinish : BaseModuleControl
    {
        #region -- Private Member --

        private double _total;
        private new SailsModule Module
        {
            get
            {
                return base.Module as SailsModule;
            }
        }

        private Booking _booking;

        #endregion

        #region -- Control events --
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Finish"]==null)
                {
                    PageEngine.PageRedirect(UrlHelper.GetUrlFromSection(Module.Section));
                    return;
                }
                Role role;

                if (PageEngine.CuyahogaUser!=null)
                {
                    role = PageEngine.CuyahogaUser.Roles[0] as Role;
                }
                else
                {
                    role = Module.RoleGetById(4);
                }

                _booking = Module.BookingGetById(Convert.ToInt32(Session["Finish"]));
                rptRooms.DataSource = Module.BookingRoomGetByBooking(_booking);
                _total = _booking.Calculate(Module, _booking.Agency, Convert.ToDouble(Module.ModuleSettings("CHILD_PRICE")), Convert.ToDouble(Module.ModuleSettings("AgencySupplement")), false, false);
                rptRooms.DataBind();
                labelTotal.Text = string.Format("{0:#,0.#}", _total);
            }
        }

        protected void rptRooms_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is BookingRoom)
            {
                BookingRoom book = (BookingRoom) e.Item.DataItem;

                #region -- Tên phòng --
                Literal litRoom = e.Item.FindControl("litRoom") as Literal;
                if (litRoom != null)
                {
                    if (book.Room != null)
                    {
                        litRoom.Text = book.Room.Name + "(" + book.RoomClass.Name + " - " + book.RoomType.Name + ")";
                    }
                    else
                    {
                        litRoom.Text = book.RoomClass.Name + " - " + book.RoomType.Name;
                    }
                }
                #endregion

                #region -- Số người --
                Literal litPeople = e.Item.FindControl("litPeople") as Literal;
                if (litPeople != null)
                {
                    litPeople.Text = string.Format("{0} adult, {1} child, {2} baby",book.Adult, book.Child, book.Baby);
                }
                #endregion

                #region -- old --
                //#region -- Tính giá --
                //SailsPriceConfig priceConfig = Module.SailsPriceConfigGet(book.RoomClass, book.RoomType, _booking.Trip,
                //                                                          _booking.TripOption, _booking.StartDate,
                //                                                          book.BookingType);
                //double price;
                //if (book.RoomType.Id == SailsModule.TWIN)
                //{
                //    price = Module.ApplyPriceFor(priceConfig.NetPrice*book.Adult,_policies);
                //}
                //else
                //{
                //    price = Module.ApplyPriceFor(priceConfig.NetPrice,_policies);
                //}

                //Literal litPrice = e.Item.FindControl("litPrice") as Literal;
                //if (litPrice != null)
                //{
                //    litPrice.Text = price.ToString("#,0");
                //}
                //#endregion

                //#region -- Giá extra service --
                //double extraPrice = 0;
                //Literal litService = e.Item.FindControl("litService") as Literal;
                //if (litService!=null)
                //{                    
                //    foreach (ExtraOption service in book.Book.ExtraServices)
                //    {
                //        extraPrice += Module.ApplyPriceFor(service.Price, _policies) * book.Adult;
                //    }
                //    litService.Text = extraPrice.ToString("#,0");
                //}
                //#endregion

                //#region -- Tổng --
                //Literal litTotal = e.Item.FindControl("litTotal") as Literal;
                //if (litTotal!=null)
                //{
                //    _total += price + extraPrice;
                //    litTotal.Text = (price + extraPrice).ToString("#,0");
                //}
                //#endregion
                #endregion
            }
        }
        #endregion

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            _booking = Module.BookingGetById(Convert.ToInt32(Session["Finish"]));
            _booking.Status = StatusType.Pending;
            Role role;

            if (PageEngine.CuyahogaUser != null)
            {
                _booking.IsApproved = true;                
                role = PageEngine.CuyahogaUser.Roles[0] as Role;
            }
            else
            {
                _booking.IsApproved = false;
                role = Module.RoleGetById(4);
            }
            _booking.Total = _booking.Calculate(Module, _booking.Agency, Convert.ToDouble(Module.ModuleSettings("CHILD_PRICE")), Convert.ToDouble(Module.ModuleSettings("AgencySupplement")), false, false);
            Module.Update(_booking,null);
            PageEngine.PageRedirect(UrlHelper.GetUrlFromSection(Module.Section));
        }
    }
}