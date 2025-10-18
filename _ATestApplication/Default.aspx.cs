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
			var idlist = new List<int>();
			idlist.Add(1);
			idlist.Add(2);
			idlist.Add(3);
			idlist.ToArray();
			var dt = sqlHelper.FillDataTable(@"
select * from DTIContentManagerhistory 
where id in (@ids) 
and areaName = @area and id not in @notInid"
, new int[] { 2, 3, 4, 5, 6 }, "default_EditPanel1", i);





			//			var dt2 = sqlHelper.FillDataTable(@"
			//SELECT
			//      case
			//      when reqstatus = 'Referred' and reqUrgency = 'High' then 1
			//      when reqstatus = 'Referred' and reqUrgency = 'Medium' then 2
			//      when reqstatus = 'Referred' and reqUrgency = 'Low' then 3
			//      when reqstatus = 'Active' and reqUrgency = 'High' then 4
			//      when reqstatus = 'Active' and reqUrgency = 'Medium' then 5
			//      when reqstatus = 'Active' and reqUrgency = 'Low' then 6
			//      when reqstatus = 'Approved' and reqUrgency = 'High' then 7
			//      when reqstatus = 'Approved' and reqUrgency = 'Medium' then 8
			//      when reqstatus = 'Approved' and reqUrgency = 'Low' then 9
			//      when reqstatus = 'Ordered' and reqUrgency = 'High' then 10
			//      when reqstatus = 'Ordered' and reqUrgency = 'Medium' then 11
			//      when reqstatus = 'Ordered' and reqUrgency = 'Low' then 12
			//      when reqstatus = 'Received' then 13
			//      else 100
			//      end as priority
			//      --, convert(varchar(25), cast(sr.insertDate as date) , 101) reqdate
			//          , (select (first_name + ' ' + last_name) from ExtUsers
			//              where id = sr.insertUser) requestor
			//      , sr.* , inv.name, inv.MinOnhandQty, inv.UnitType, inv.Vendor, inv.url, inv.id invid
			//      from Supply_Request sr
			//      left outer join Inventory inv on inv.ID = sr.inventoryID
			//      where sr.reqstatus <> 'deleted'
			//      and ifnull(name,'') <> '' order by priority, name ;
			//");

			//			var dt3 = sqlHelper.FillDataTable(@"
			//select * from Inventory
			//                     where id in (
			//                      select ForeignID from StorageLocationPvt where id not in (
			//						select StorageLocationPvtID from StorageLocation_Log where IfNULL(audit,0)=1 and cast(insertDate as date) > '2025-09-01' and cast(insertDate as date) < '2025-11-30'
			//						) and foreignTable = 'inventory' and IfNULL(archive,0)=0 --and ISNULL(assetItem,0)=0
			//                     ) and IfNULL(archive,0)=0 and IfNULL(assetItem,0)=0
			//                    order by name
			//");

			//            var dt1 = sqlHelper.FillDataTable(@"
			//                select cast(id as varchar) id, Name, isnull(Priority,999) priority, roleType
			//				from roles r where roleType not in ('other','staff') 							
			//				union
			//					(select '-5' id, 'Include Staff',-5, 'Staff'
			//					from (                        
			//						select STUFF((
			//							SELECT distinct(cast(id as varchar))+', ' 
			//							FROM Roles 
			//							WHERE roleType = 'staff'
			//							FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 0, '') AS roles						
			//					) tbl)
			//				union
			//					(select '-4' id, 'Include Board Members',-4, roleType from Roles where name = 'Board Member')
			//				union
			//					(select '-3' id, 'Include State Board',-3, roleType from Roles where name = 'SBE Employee')
			//				order by Priority,Name
			//");
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