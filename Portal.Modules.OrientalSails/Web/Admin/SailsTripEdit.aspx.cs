using System;
using System.Web.UI.WebControls;
using CMS.ServerControls.FileUpload;
using CMS.Web.Util;
using log4net;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class SailsTripEdit : SailsAdminBase
    {
        #region -- Private Member --

        private readonly ILog _logger = LogManager.GetLogger(typeof (SailsTripEdit));
        private SailsTrip _trip;

        private int TripId
        {
            get
            {
                int id;
                if (Request.QueryString["TripId"] != null && Int32.TryParse(Request.QueryString["TripId"], out id))
                {
                    return id;
                }
                return -1;
            }
        }

        #endregion

        #region -- Page Event --

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Title = Resources.titleSailsTripEdit;

                #region -- Ajax Image --

                fileUploaderMap.addCustomJS(FileUploaderAJAX.customJSevent.postUpload,
                                            FileHelper.InsertImagePostUploadJS("divMap", textBoxHiddenMap));
                fileUploaderMap.addCustomJS(FileUploaderAJAX.customJSevent.postDelete,
                                            FileHelper.ClearData("divMap", textBoxHiddenMap));
                fileUploaderMap.addCustomJS(FileUploaderAJAX.customJSevent.postHide,
                                            FileHelper.ClearData("divMap", textBoxHiddenMap));
                if (fileUploaderMap.IsPosting)
                {
                    FileHelper.ManageAjaxPost(fileUploaderMap, 0, "Image\\Sails\\",
                                              HttpPostedFileAJAX.fileType.image);
                    return;
                }

                #endregion

                if (!IsPostBack)
                {
                    ddlOrganizations.DataSource = Module.OrganizationGetAllRoot();
                    ddlOrganizations.DataValueField = "Id";
                    ddlOrganizations.DataTextField = "Name";
                    ddlOrganizations.DataBind();

                    LoadInfo();
                    rptCostTypes.DataSource = Module.CostTypeGetByTrips();
                    rptCostTypes.DataBind();                    
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error when Page_Load in SailsTripEdit", ex);
                ShowError(ex.Message);
            }
        }

        #endregion

        #region -- Private Method --

        private void LoadInfo()
        {
            if (TripId > 0)
            {
                _trip = Module.TripGetById(TripId);
                textBoxName.Text = _trip.Name;
                textBoxNumberOfDay.Text = _trip.NumberOfDay.ToString();
                fckItinerary.Value = _trip.Itinerary;
                fckDescription.Value = _trip.Description;
                fckExclusions.Value = _trip.Exclusions;
                fckWhatToTake.Value = _trip.WhatToTake;
                fckInclusions.Value = _trip.Inclusions;
                textBoxHiddenMap.Text = _trip.Image;
                txtTripCode.Text = _trip.TripCode;
                ddlNumberOfOptions.SelectedValue = _trip.NumberOfOptions.ToString();
                fileUploaderMap.addCustomJS(FileUploaderAJAX.customJSevent.preLoad,
                                            FileHelper.InsertImagePostloadJS("divMap", textBoxHiddenMap,
                                                                             _trip.Image));
                if (_trip.Organization !=null)
                {
                    ddlOrganizations.SelectedValue = _trip.Organization.Id.ToString();
                }
            }
        }

        #endregion

        #region -- Control Event --

        protected void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid)
                {
                    if (TripId > 0)
                    {
                        _trip = Module.TripGetById(TripId);
                    }
                    else
                    {
                        _trip = new SailsTrip();
                        _trip.CreatedBy = UserIdentity;
                    }
                    _trip.ModifiedBy = UserIdentity;
                    _trip.Name = textBoxName.Text;
                    int numOdays;
                    if (!string.IsNullOrEmpty(textBoxNumberOfDay.Text) &&
                        Int32.TryParse(textBoxNumberOfDay.Text, out numOdays))
                    {
                        _trip.NumberOfDay = numOdays;
                    }
                    else
                    {
                        _trip.NumberOfDay = 0;
                    }
                    _trip.NumberOfOptions = Convert.ToInt32(ddlNumberOfOptions.SelectedValue);
                    _trip.Image = textBoxHiddenMap.Text;
                    _trip.Itinerary = fckItinerary.Value;
                    _trip.Description = fckDescription.Value;
                    _trip.Exclusions = fckExclusions.Value;
                    _trip.Inclusions = fckInclusions.Value;
                    _trip.WhatToTake = fckWhatToTake.Value;
                    _trip.TripCode = txtTripCode.Text;

                    _trip.Organization = Module.OrganizationGetById(Convert.ToInt32(ddlOrganizations.SelectedValue));
                    if (TripId > 0)
                    {
                        Module.Update(_trip);
                    }
                    else
                    {
                        Module.Save(_trip);
                    }
                    PageRedirect(string.Format("SailsTripList.aspx?NodeId={0}&SectionId={1}", Node.Id, Section.Id));
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error when buttonSave_Click in SailsTripEdit", ex);
                ShowError(ex.Message);
            }
        }

        protected void buttonCancel_Clicl(object sender, EventArgs e)
        {
            PageRedirect(string.Format("SailsTripList.aspx?NodeId={0}&SectionId={1}", Node.Id, Section.Id));
        }

        protected void rptCostTypes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is CostType)
            {
                var type = (CostType)e.Item.DataItem;

                var chkCostType = e.Item.FindControl("chkCostType") as CheckBox;
                if (chkCostType != null)
                {
                    chkCostType.Text = type.Name;
                    if (_trip!=null && _trip.CostTypes.Contains(type))
                    {
                        chkCostType.Checked = true;
                    }
                    else
                    {
                        chkCostType.Checked = false;
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            if (TripId > 0)
            {
                _trip = Module.TripGetById(TripId);
            }
            else
            {
                _trip = new SailsTrip();
                _trip.CreatedBy = UserIdentity;
            }
            _trip.ModifiedBy = UserIdentity;
            _trip.Name = textBoxName.Text;
            int numOdays;
            if (!string.IsNullOrEmpty(textBoxNumberOfDay.Text) &&
                Int32.TryParse(textBoxNumberOfDay.Text, out numOdays))
            {
                _trip.NumberOfDay = numOdays;
            }
            else
            {
                _trip.NumberOfDay = 0;
            }
            _trip.NumberOfOptions = Convert.ToInt32(ddlNumberOfOptions.SelectedValue);
            _trip.Image = textBoxHiddenMap.Text;
            _trip.Itinerary = fckItinerary.Value;
            _trip.Description = fckDescription.Value;
            _trip.Exclusions = fckExclusions.Value;
            _trip.Inclusions = fckInclusions.Value;
            _trip.WhatToTake = fckWhatToTake.Value;
            _trip.TripCode = txtTripCode.Text;
            _trip.Organization = Module.OrganizationGetById(Convert.ToInt32(ddlOrganizations.SelectedValue));
            //_trip.HalfDay = Convert.ToInt32(ddlHalfDay.SelectedValue);
            if (TripId > 0)
            {
                Module.Update(_trip);
            }
            else
            {
                Module.Save(_trip);
            }

            // Đối với mỗi đối tượng kiểm tra danh sách bến và thứ tự của chúng rồi lưu lại
            foreach (RepeaterItem item in rptCostTypes.Items)
            {
                HiddenField hiddenId = (HiddenField)item.FindControl("hiddenId");
                CheckBox chkCostType = (CheckBox)item.FindControl("chkCostType");
                CostType type = Module.CostTypeGetById(Convert.ToInt32(hiddenId.Value));
                if (!chkCostType.Checked && _trip.CostTypes.Contains(type))
                {
                    // Xóa bỏ
                    TripCostType temp = Module.TripCostTypeGet(_trip, type);
                    if (temp != null)
                    {
                        Module.Delete(temp);
                    }
                }
                if (chkCostType.Checked && !_trip.CostTypes.Contains(type))
                {
                    // Thêm vào
                    var temp = new TripCostType();
                    temp.Trip = _trip;
                    temp.CostType = type;

                    Module.SaveOrUpdate(temp);
                }
            }

            PageRedirect(string.Format("SailsTripList.aspx?NodeId={0}&SectionId={1}", Node.Id, Section.Id));
        }

        #endregion
    }
}