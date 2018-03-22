using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CMS.Core.Domain;
using CMS.ServerControls.FileUpload;
using CMS.Web.Admin.Controls;
using CMS.Web.Util;
using log4net;
using Portal.Modules.OrientalSails.Domain;
using Portal.Modules.OrientalSails.Web.UI;

namespace Portal.Modules.OrientalSails.Web.Admin
{
    public partial class AgencyEdit : SailsAdminBase
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(AgencyEdit));
        private Agency _agency;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    uploadContract.addCustomJS(FileUploaderAJAX.customJSevent.postUpload, FileHelper.InsertFileNameUploadJS(txtPath));                    
                }

                if (uploadContract.IsPosting)
                {
                    FileHelper.ManageAjaxPost(uploadContract, 1000, "Contract\\");
                    return;
                }

                Title = Resources.textAgencyEdit;

                if (!IsPostBack)
                {
                    if (!Module.PermissionCheck(Permission.ACTION_EDITAGENCY, UserIdentity))
                    {
                        btnSave.Visible = false;
                        buttonSave.Visible = false;
                        btnAdd.Visible = false;
                        btnAddContact.Visible = false;
                    }

                    if (Request.QueryString["AgencyId"] == null)
                    {
                        buttonSave.Visible = true;
                    }

                    BindRoles();
                    BindSales();
                    BindLevel();
                    BindPaymentPeriod();

                    ddlOrganizations.DataSource = Module.OrganizationGetAllRoot();
                    ddlOrganizations.DataTextField = "Name";
                    ddlOrganizations.DataValueField = "Id";
                    ddlOrganizations.DataBind();

                    LoadInfo();

                    rptContracts.DataSource = Module.ContractGetByAgency(_agency);
                    rptContracts.DataBind();

                    if (Request.QueryString["agencyid"] != null)
                    {
                        string url = string.Format("AgencyContactEdit.aspx?Nodeid={0}&SectionId={1}&agencyid={2}", Node.Id,
                                                   Section.Id, _agency.Id);
                        btnAddContact.Attributes.Add("onclick",
                                                     CMS.ServerControls.Popup.OpenPopupScript(url, "Contact", 300, 400));
                        plhContracts.Visible = true;
                        rptContracts.DataSource = Module.ContractGetByAgency(_agency);
                        rptContracts.DataBind();                        
                    }
                    else
                    {
                        plhContracts.Visible = false;
                        btnAddContact.Visible = false;
                    }
                }                
            }
            catch (Exception ex)
            {
                _logger.Error("Error when Page_Load in RoomEdit", ex);
                ShowError(ex.Message);
            }
        }

        protected void BindPaymentPeriod()
        {
            ddlPaymentPeriod.DataSource = Enum.GetNames(typeof (PaymentPeriod));
            ddlPaymentPeriod.DataBind();
        }

        private void BindSales()
        {
            if (Module.ModuleSettings("Sale")!=null)
            {
                Role role = Module.RoleGetById(Convert.ToInt32(Module.ModuleSettings("Sale")));
                ddlSales.DataSource = role.Users;
                ddlSales.DataTextField = "Username";
                ddlSales.DataValueField = "Id";
                ddlSales.DataBind();
                ddlSales.Items.Insert(0, "-- Unbound sales --");
                // role.Users
            }
        }

        private void BindLevel()
        {
                IList agencyLevelList = Module.AgencyLevelGetAll();
                ddlLevel.DataSource = agencyLevelList;
                ddlLevel.DataTextField = "Name";
                ddlLevel.DataValueField = "Id";
                ddlLevel.DataBind();            
        }

        protected void buttonSave_Click(object sender, EventArgs e)
        {
            bool isAddnew = false;
            if (Request.QueryString["AgencyId"] != null)
            {
                _agency = Module.AgencyGetById(Convert.ToInt32(Request.QueryString["AgencyId"]));                
            }
            else
            {
                _agency = new Agency();
                isAddnew = true;
            }
            _agency.Name = textBoxName.Text;
            _agency.Phone = txtPhone.Text;
            _agency.Address = txtAddress.Text;
            _agency.Role = Module.RoleGetById(Convert.ToInt32(ddlRoles.SelectedValue));
            _agency.Fax = txtFax.Text;
            _agency.Email = txtEmail.Text;
            _agency.TaxCode = txtTaxCode.Text;
            _agency.Description = txtDescription.Text;
            _agency.ContractStatus = Convert.ToInt32(ddlContractStatus.SelectedValue);
            _agency.Accountant = txtAccountant.Text;
            _agency.PaymentPeriod = (PaymentPeriod) Enum.Parse(typeof(PaymentPeriod),ddlPaymentPeriod.SelectedValue);
            _agency.Organization = Module.OrganizationGetById(Convert.ToInt32(ddlOrganizations.SelectedValue));
            _agency.AgencyLevel = Module.AgencyLevelGetById(Convert.ToInt32(ddlLevel.SelectedValue));

            if (!string.IsNullOrEmpty(txtPath.Text))
            {
                _agency.Contract = txtPath.Text;
            }

            if (ddlSales.Items.Count > 0 && ddlSales.SelectedIndex > 0)
            {
                _agency.Sale = Module.UserGetById(Convert.ToInt32(ddlSales.SelectedValue));
            }
            else
            {
                _agency.Sale = null;
            }
            if (_agency.Id <= 0)
            {
                _agency.CreatedBy = UserIdentity;
                _agency.CreatedDate = DateTime.Now;
            }
            else
            {
                _agency.ModifiedBy = UserIdentity;
                _agency.ModifiedDate = DateTime.Now;
            }

            Module.SaveOrUpdate(_agency);

            if (isAddnew)
            {
                PageRedirect(string.Format("AgencyEdit.aspx?NodeId={0}&SectionId={1}&AgencyId={2}", Node.Id, Section.Id, _agency.Id));
            }
            else
            {
                PageRedirect(string.Format("AgencyList.aspx?NodeId={0}&SectionId={1}", Node.Id, Section.Id));
            }
        }

        protected void BindRoles()
        {
            ddlRoles.DataSource = Module.RoleGetAll();
            ddlRoles.DataTextField = "Name";
            ddlRoles.DataValueField = "Id";
            ddlRoles.DataBind();
        }


        public void LoadInfo()
        {
            if (Request.QueryString["AgencyId"]!=null)
            {
                _agency = Module.AgencyGetById(Convert.ToInt32(Request.QueryString["AgencyId"]));

                if (_agency.Sale!=null && _agency.Sale.Id == UserIdentity.Id)
                {
                    btnSave.Visible = true;
                    buttonSave.Visible = true;
                    btnAdd.Visible = true;
                    btnAddContact.Visible = true;
                }

                if (_agency.Organization != null)
                {
                    ddlOrganizations.SelectedValue = _agency.Organization.Id.ToString();
                }

                textBoxName.Text = _agency.Name;
                txtPhone.Text = _agency.Phone;
                txtAddress.Text = _agency.Address;
                txtFax.Text = _agency.Fax;
                txtEmail.Text = _agency.Email;
                txtTaxCode.Text = _agency.TaxCode;
                txtDescription.Text = _agency.Description;
                ddlContractStatus.SelectedValue = _agency.ContractStatus.ToString();
                txtAccountant.Text = _agency.Accountant;
                ddlLevel.SelectedValue = _agency.AgencyLevel.Id.ToString();

                if (_agency.CreatedBy != null && _agency.CreatedDate.HasValue)
                {
                    litCreated.Text = string.Format("Created by {0} at {1:dd/MM/yyyy HH:MM}", _agency.CreatedBy.FullName, _agency.CreatedDate.Value);
                }
                if (_agency.ModifiedBy != null && _agency.ModifiedDate.HasValue)
                {
                    litModified.Text = string.Format("Last edited by {0} at {1:dd/MM/yyyy HH:MM}", _agency.ModifiedBy.FullName, _agency.ModifiedDate.Value);
                }
                ddlPaymentPeriod.SelectedValue = _agency.PaymentPeriod.ToString();

                if (ddlSales.Items.Count > 0)
                {
                    if (_agency.Sale != null)
                    {
                        ddlSales.SelectedValue = _agency.Sale.Id.ToString();
                    }
                    else
                    {
                        ddlSales.SelectedIndex = 0;
                    }
                }

                if (!string.IsNullOrEmpty(_agency.Contract))
                {
                    hplOldContract.NavigateUrl = _agency.Contract;
                }
                else
                {
                    hplOldContract.Visible = false;
                }
                if (_agency.Role != null)
                {
                    ddlRoles.SelectedValue = _agency.Role.Id.ToString();
                }
                rptUsers.DataSource = _agency.Users;
                rptUsers.DataBind();
                plhAssignedUser.Visible = true;

                rptContacts.DataSource = Module.ContactGetByAgency(_agency);
                rptContacts.DataBind();
            }
            else
            {
                plhAssignedUser.Visible = false;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["AgencyId"] != null)
            {
                _agency = Module.AgencyGetById(Convert.ToInt32(Request.QueryString["AgencyId"]));
            }
            UserSelector selector = userSelector as UserSelector;
            if (selector == null)
            {
                throw new Exception("Selector error");
            }
            if (selector.SelectedUserId > 0)
            {
                AgencyUser data = Module.AgencyUserGet(_agency, Module.UserGetById(selector.SelectedUserId));
                rptUsers.DataSource = _agency.Users;
                rptUsers.DataBind();
            }
        }

        protected void rptUsers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName.ToLower())
            {
                case "delete":
                    _agency = Module.AgencyGetById(Convert.ToInt32(Request.QueryString["AgencyId"]));
                    AgencyUser data = Module.AgencyUserGet(_agency, Module.UserGetById(Convert.ToInt32(e.CommandArgument)));
                    Module.Delete(data);
                    rptUsers.DataSource = _agency.Users;
                    rptUsers.DataBind();
                    break;
            }
        }

        protected void rptContacts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is AgencyContact)
            {
                AgencyContact contact = (AgencyContact) e.Item.DataItem;
                Literal litEnabled = e.Item.FindControl("litEnabled") as Literal;
                if (litEnabled != null)
                {
                    if (contact.Enabled)
                    {
                        litEnabled.Text = "Enabled";
                    }
                    else
                    {
                        litEnabled.Text = "Disabled";
                    }
                }

                HtmlInputButton btnEdit = e.Item.FindControl("btnEdit") as HtmlInputButton;
                if (btnEdit!=null)
                {
                    string url = string.Format("AgencyContactEdit.aspx?Nodeid={0}&SectionId={1}&contactid={2}", Node.Id,
                                               Section.Id, contact.Id);
                    btnEdit.Attributes.Add("onclick",
                                                 CMS.ServerControls.Popup.OpenPopupScript(url, "Contact", 300, 400));
                }

                Button btnEnabled = e.Item.FindControl("btnEnabled") as Button;
                if (btnEnabled!=null)
                {
                    if (contact.Enabled)
                    {
                        btnEnabled.Text = "Disable";
                        btnEnabled.CommandName = "disable";
                    }
                    else
                    {
                        btnEnabled.Text = "Enable";
                        btnEnabled.CommandName = "enable";
                    }
                }
            }
        }

        protected void rptContacts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            AgencyContact contact = Module.ContactGetById(Convert.ToInt32(e.CommandArgument));
            Literal litEnabled = (Literal) e.Item.FindControl("litEnabled");
            switch (e.CommandName.ToLower())
            {
                case "disable":
                    contact.Enabled = false;
                    Module.SaveOrUpdate(contact);
                    ((Button) e.CommandSource).Text = "Enable";
                    litEnabled.Text = "Disabled";
                    break;
                case "enable":
                    contact.Enabled = true;
                    Module.SaveOrUpdate(contact);
                    ((Button)e.CommandSource).Text = "Disable";
                    litEnabled.Text = "Enabled";
                    break;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            AgencyContract contract;
            if (string.IsNullOrEmpty(hiddenContractId.Value))
            {
                contract = new AgencyContract();
            }
            else
            {
                contract = Module.ContractGetById(Convert.ToInt32(hiddenContractId.Value));
            }
            contract.ContractName = txtContractName.Text;
            contract.ExpiredDate = DateTime.ParseExact(txtExpiredDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (fileUploadContract.HasFile)
            {
                contract.FileName = fileUploadContract.FileName;
                contract.ContractFile = fileUploadContract.FileBytes;
            }

            if (Request.QueryString["agencyid"]!=null)
            {
                _agency = Module.AgencyGetById(Convert.ToInt32(Request.QueryString["agencyid"]));
            }
            contract.Agency = _agency;
        }

        protected void rptContracts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            AgencyContract contract = Module.ContractGetById(Convert.ToInt32(e.CommandArgument));
            switch (e.CommandName.ToLower())
            {
                case "download":
                    Response.Clear();
                    Response.ContentType = "application/octet-stream";
                    Response.AppendHeader("content-disposition",
                                  "attachment; filename=" + contract.FileName);

                    Stream outputStream = Response.OutputStream;
                    outputStream.Write(contract.ContractFile,0, contract.ContractFile.Length);
                    outputStream.Flush();
                    outputStream.Close();

                    Response.End();

                    break;
                case "edit":
                    hiddenContractId.Value = e.CommandArgument.ToString();
                    txtContractName.Text = contract.ContractName;
                    txtExpiredDate.Text = contract.ExpiredDate.ToString("dd/MM/yyyy");
                    break;
            }
        }

        protected void rptContracts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem is AgencyContract)
            {
                AgencyContract contract = (AgencyContract) e.Item.DataItem;
                Literal litExpiredDate = e.Item.FindControl("litExpiredDate") as Literal;
                if (litExpiredDate!=null)
                {
                    litExpiredDate.Text = contract.ExpiredDate.ToString("dd/MM/yyyy");
                }
            }
        }
    }
}
