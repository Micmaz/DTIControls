using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaseClasses;

namespace _ATestApplication
{
	public partial class Default : BaseSecurityPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			
		}

		protected void btnTurnEditOn_Click(object sender, EventArgs e)
		{
			DTIControls.Share.EditModeOn = !DTIControls.Share.EditModeOn;
		}
		protected void btnTurnAdminOn_Click(object sender, EventArgs e)
		{
			DTIControls.Share.AdminPanelOn = !DTIControls.Share.AdminPanelOn;
		}

	}
}