using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaseClasses;
using System.Reflection;
using System.Collections;
using System.Text.RegularExpressions;

namespace _ATestApplication
{
	public partial class Default : BaseSecurityPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			var i = new List<int>();
			i.Add(1);
            i.Add(3);
            i.Add(4);
            var dt= sqlHelper.FillDataTable("select * from DTIContentManagerhistory where id in (@ids) and areaName = @area and id not in @notInid", new int[] { 2,3,4,5,6 }, "default_EditPanel1", i);

            DTIControls.Share.AdminPanelOn = true;
            Hashtable f = new Hashtable();
			Regex c;
			//for(var f in System.IO.DirectoryInfo("C:"))
		}

		protected void btnTurnEditOn_Click(object sender, EventArgs e)
		{
			Assembly a;

			DTIControls.Share.EditModeOn = !DTIControls.Share.EditModeOn;
		}
		protected void btnTurnAdminOn_Click(object sender, EventArgs e)
		{
			DTIControls.Share.AdminPanelOn = !DTIControls.Share.AdminPanelOn;
		}

	}
}