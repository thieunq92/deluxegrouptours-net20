using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CMS.Web.UI;
using CMS.Web.Util;
using log4net;
using Portal.Modules.OrientalSails.Domain;

namespace Portal.Modules.OrientalSails.Web
{
    public partial class PreferedRooms : BaseModuleControl
    {
        #region -- Private Member --

        private SailsTrip _trip;
        private readonly ILog _logger = LogManager.GetLogger(typeof(PreferedRooms));
        private Dictionary<string, int> currentMap;

        /// <summary>
        /// Danh sách phòng được chọn
        /// </summary>
        private Dictionary<string, int> roomMap;
        private new SailsModule Module
        {
            get { return base.Module as SailsModule; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                roomMap = new Dictionary<string, int>();
                #region -- Get room data --

                #region -- Lấy dữ liệu từ session --
                int configCount = Convert.ToInt32(Session["ConfigCount"]);

                string[] roomList = new string[configCount];
                // MAP các room theo format <"ClassId#TypeID", số lượng>
                for (int i = 1; i <= configCount; i++)
                {
                    //Lấy từng dữ liệu cấu hình có trong session ra
                    //Lấy room class, room type theo số lượng yêu cầu
                    string sessionStr = Session["Config" + i].ToString();
                    string[] data = sessionStr.Split(',');

                    // Lấy về ba dữ liệu trong mỗi config là class, type và số lượng
                    int classId = Convert.ToInt32(data[0]);
                    int typeId = Convert.ToInt32(data[1]);
                    int number = Convert.ToInt32(data[2]);

                    // Tạo thành từ điển phòng loại nào có bao nhiêu phòng
                    if (typeId == SailsModule.TWIN)
                    {
                        if (number % 2 == 1)
                        {
                            panelWarning.Visible = true;
                        }
                        number = number/2;                        
                    }
                    roomMap.Add(string.Format("{0}#{1}", classId, typeId), number);

                    // Nếu là lần đầu load thì cần hiển thị danh sách phòng cần chọn cho người dùng xem
                    if (!IsPostBack)
                    {
                        RoomTypex rtype = Module.RoomTypexGetById(typeId);
                        RoomClass rclass = Module.RoomClassGetById(classId);
                        roomList[i - 1] = string.Format("{0} {1} {2} room(s)", number, rclass.Name, rtype.Name);
                    }
                }

                if (!IsPostBack)
                {
                    rptRooms.DataSource = roomList;
                    rptRooms.DataBind();
                }

                #endregion

                #region build current room data from listbox
                currentMap = new Dictionary<string, int>();
                foreach (ListItem item in listSelectedRooms.Items)
                {
                    string[] values = item.Value.Split(',');
                    string key = values[0] + "#" + values[1];
                    if (currentMap.ContainsKey(key))
                    {
                        currentMap[key] += 1;
                    }
                    else
                    {
                        currentMap.Add(key, 1);
                    }
                }
                #endregion
                #endregion

                if (!IsPostBack)
                {
                    LocalizeControls();
                    if (Session["StartDate"] != null)
                    {
                        _trip = Module.TripGetById(Convert.ToInt32(Session["TripId"]));
                        CultureInfo cultureInfo = new CultureInfo("vi-VN");
                        DateTime startDate = DateTime.ParseExact(Session["StartDate"].ToString(), "dd/MM/yyyy",
                                                                 cultureInfo.DateTimeFormat);
                        rptRoomList.DataSource = Module.RoomGetAllWithAvaiableStatus(null,startDate, _trip.NumberOfDay);
                    }
                    else
                    {
                        rptRoomList.DataSource = Module.RoomGetAll(true);
                    }

                    rptRoomList.DataBind();
                }
                ScriptManager.RegisterClientScriptInclude(Page, GetType(), "dropdown", "/js/dropdown.js");
            }
            catch (Exception ex)
            {
                _logger.Error("Error when Page_Load in PreferedRooms contrrol", ex);
                throw;
            }
        }

        protected void rptRoomList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is Room)
            {
                Room item = e.Item.DataItem as Room;
                if (item != null)
                {
                    ImageButton imageRoom = (ImageButton)e.Item.FindControl("imageRoom");

                    if (item.IsAvailable)
                    {
                        imageRoom.ImageUrl = "/Modules/Sails/Images/available.gif";
                    }
                    else
                    {
                        imageRoom.ImageUrl = "/Modules/Sails/Images/locked.gif";
                        imageRoom.Enabled = false;
                    }

                    HtmlGenericControl roomcontent = (HtmlGenericControl)e.Item.FindControl("roomcontent");
                    roomcontent.Attributes.Add("class", "hover_content");
                    roomcontent.InnerHtml = string.Format("Name: {0}<br />Class: {1}<br />Type: {2}<br />", item.Name, item.RoomClass.Name, item.RoomType.Name);

                    imageRoom.Attributes.Add("onmouseover",
                                                "at_show_if('" + imageRoom.ClientID + "','" + roomcontent.ClientID +
                                                "');");

                    Literal litRoom = (Literal)e.Item.FindControl("litRoom");
                    litRoom.Text = string.Format("{0} - {1}", item.RoomClass.Name, item.RoomType.Name);
                }
            }
        }

        protected void rptRoomList_itemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                // Lấy thông tin phòng từ id
                Room item = Module.RoomGetById(Convert.ToInt32(e.CommandArgument));

                // Mặc định là có đánh dấu cờ hợp lệ
                bool flag = true;

                // Lấy key của phòng
                string key = item.RoomClass.Id + "#" + item.RoomType.Id;

                // Nếu trong danh sách không có key tương ứng tức là không có phòng này để chọn
                // Hoặc trong danh sách đã chọn đã có key này và số lượng đã chọn lớn hơn được chọn (đã chọn hết số phòng của key đó)
                // Đánh dấu cờ là false
                if (!roomMap.ContainsKey(key) || (currentMap.ContainsKey(key) && currentMap[key] >= roomMap[key]))
                {
                    flag = false;
                }

                // Kiểm tra thêm lần nữa, nếu phòng này đồng thời đã có trong danh sách phòng đã chọn
                foreach (ListItem listItem in listSelectedRooms.Items)
                {
                    string[] values = listItem.Value.Split(',');
                    if (values[2] == item.Id.ToString())
                    {
                        flag = false;
                        listSelectedRooms.Items.Remove(listItem);
                        ImageButton imageRoom = (ImageButton)e.Item.FindControl("imageRoom");
                        imageRoom.ImageUrl = "/Modules/Sails/Images/available.gif";
                    }
                }

                // Cuối cùng nếu hợp lệ
                if (flag)
                {
                    listSelectedRooms.Items.Add(new ListItem(item.Name, string.Format("{0},{1},{2}", item.RoomClass.Id, item.RoomType.Id, item.Id)));
                    ImageButton imageRoom = (ImageButton)e.Item.FindControl("imageRoom");
                    imageRoom.ImageUrl = "/Modules/Sails/Images/prefered.gif";                    
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error when rptRoomList_itemCommand in PreferedRooms control", ex);
                throw;
            }
        }

        protected void buttonRemove_Click(object sender, EventArgs e)
        {
            listSelectedRooms.Items.RemoveAt(listSelectedRooms.SelectedIndex);
        }

        protected void buttonSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (listSelectedRooms.Items.Count > 0)
                {
                    int i = 1;
                    // Save thông tin các phòng đã chọn vào trong session
                    foreach (ListItem item in listSelectedRooms.Items)
                    {
                        Session.Add("Room" + i, item.Value.Split(',')[2]);
                        i++;
                    }

                    Session.Add("SelectedRoom", i - 1);
                    // Đối với từng phòng đã chọn tìm match tương ứng trong Booking


                    //Session.Add("Finish",true);
                    PageEngine.PageRedirect(string.Format("{0}/{1}{2}", UrlHelper.GetUrlFromSection(Module.Section),
                                                                  SailsModule.ACTION_CUSTOMER_INFO_PARAM,
                                                                  UrlHelper.EXTENSION));
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error when buttonSubmit_Click in PreferedRooms", ex);
                throw;
            }
        }
    }
}