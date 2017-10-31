using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _ATestApplication
{
	public partial class Default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
		}

		protected void btnTurnEditOn_Click(object sender, EventArgs e)
		{
			DTIControls.Share.EditModeOn = !DTIControls.Share.EditModeOn;
		}

	}
}