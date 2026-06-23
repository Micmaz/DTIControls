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
			var s = BaseClasses.DataBase.getHelper();
            /*
                        var cmd1= sqlHelper.ShowCommandSQL(@"
            SELECT *
                  FROM StorageLocationPvt slp
                  WHERE slp.foreignTable = 'inventory'
                    AND ISNULL(slp.archive, 0) = 0
                    --AND DATE(slp.insertDate) <= DATE('now', printf('-%d days', 30))
                    AND DATEADD('day', 30, insertDate) > getdate()
                    AND Convert(DATE,slp.insertDate )  > getdate()
                    AND CAST(slp.insertDate AS DATE) <= getdate() - 30
            ");

                        var dta = s.FillDataTable(@"
            SELECT *
                  FROM StorageLocationPvt slp
                  WHERE slp.foreignTable = 'inventory'
                    AND ISNULL(slp.archive, 0) = 0
                    --AND DATE(slp.insertDate) <= DATE('now', printf('-%d days', 30))
                    AND DATEADD('day', 30, insertDate) > getdate()
                    AND Convert(DATE,slp.insertDate )  > getdate()
                    AND CAST(slp.insertDate AS DATE) <= getdate() - 30
            ");

                        var i = new List<int>();
                        i.Add(1);
                        i.Add(3);
                        i.Add(4);
                        var idlist = new List<int>();
                        idlist.Add(1);
                        idlist.Add(2);
                        idlist.Add(3);
                        idlist.ToArray();

                        var sql1 = sqlHelper.ShowCommandSQL(@"
            select * from DTIContentManagerhistory 
            where id <> @NotinID and id+1 <> @NotinID
            and id in @inIDList
            and id-1 <> @NotinID
            and id+2 in @inIDList
            ", 999, i);


                        var dt = sqlHelper.FillDataTable(@"
            select * from DTIContentManagerhistory 
            where id <> @NotinID and id+1 <> @NotinID
            and id in @inIDList
            and id-1 <> @NotinID
            and id+2 in @inIDList
            ", 999, i);




                        //, new int[] { 2, 3, 4, 5, 6 }, "default_EditPanel1", i);

                        var dt2a = sqlHelper.FillDataTable(@"
            WITH Settings AS (
                SELECT 
                    ISNULL((SELECT value FROM SiteSettings WHERE name = 'auditElection'), 'false') AS auditElection,
                    cast((SELECT isnull(value,0) FROM SiteSettings WHERE name = 'auditFrequency') AS INT) AS auditFrequency
            ),
            AuditCutoff AS (
                SELECT 
                    CASE 
                        WHEN s.auditElection = 'true' THEN (
                            SELECT MAX(election_dt) AS lastElectionDate FROM Election where CAST(election_dt as date) < 
                            (SELECT election_dt AS lastElectionDate FROM Election where id = (select top 1 id from CurrentElection order by id desc))
                        )
                        ELSE (GETDATE() - s.auditFrequency * 30)
                    END AS cutoffDate
                FROM Settings s
            )
            SELECT * 
            FROM Inventory i
            LEFT OUTER JOIN (
                SELECT 
                    slp.quantity + ISNULL(sll.totalAmount, 0) AS totalCount,
                    (sl.area + ' ' + sl.bin) AS location,
                    slp.ForeignID
                FROM StorageLocationPvt slp
                LEFT OUTER JOIN StorageLocation sl ON sl.ID = slp.LocationID
                OUTER APPLY (
                    SELECT SUM(amount) AS totalAmount 
                    FROM StorageLocation_Log 
                    WHERE StorageLocationPvtID = slp.ID
                ) sll
                WHERE ISNULL(slp.archive, 0) = 0
            ) tbl1 ON tbl1.ForeignID = i.ID
            WHERE i.ID IN (
                SELECT ForeignID 
                FROM StorageLocationPvt slp
                CROSS JOIN AuditCutoff ac
                WHERE slp.ID NOT IN (
                    SELECT StorageLocationPvtID 
                    FROM StorageLocation_Log 
                    WHERE ISNULL(audit, 0) = 1
                    AND CAST(insertDate AS DATE) > ac.cutoffDate
                )
                AND foreignTable = 'inventory'
                AND ISNULL(archive, 0) = 0
            )
            AND ISNULL(i.archive, 0) = 0
            AND ISNULL(assetItem, 0) = 0
            ORDER BY name;
                ");


                        var dt2 = sqlHelper.FillDataTable(@"
             SELECT top 5 notes, amount, iif(sl.area like 'coc%','', sl.area) + ' ' + sl.bin location, 
            iif(sl1.area like 'coc%','', sl1.area + ' ') + '' + sl1.bin affectedLocation, sll.insertDate, sll.id id
                            FROM StorageLocation_log sll 
                            LEFT OUTER JOIN StorageLocationPvt slp ON slp.ID = sll.StorageLocationPvtID
                            LEFT OUTER JOIN StorageLocation sl ON sl.ID = slp.LocationID
                            LEFT OUTER JOIN StorageLocation sl1 ON sl1.ID = sll.LocationAffectedID
                            WHERE --ISNULL(slp.archive, 0) = 0 and 
                            slp.foreignTable = 'inventory' 
                            order by sll.insertDate desc
                        ");

                        var dt3a1 = sqlHelper.FillDataTable(@"
            SELECT
                                 convert(varchar(25), cast(sr.insertDate as date) , 101) reqdate,
                                 (select (first_name + ' ' + last_name) from ExtUsers where id = sr.insertUser) requestor,
                                 sr.* , inv.name, inv.MinOnhandQty, inv.UnitType, inv.Vendor, inv.url, inv.id invid
                                    from Supply_Request sr
                                    left outer join Inventory inv on inv.ID = sr.inventoryID
                                    where sr.reqstatus <> 'deleted' and IsNULL(name,'') <> ''
            ");

                        var dt3a = sqlHelper.FillDataTable(@"
            select STUFF((
                                    SELECT distinct(cast(area + ' ' + bin as varchar))+', ' 
                                    FROM StorageLocation rpt
                                    WHERE rpt.ID in (select LocationID from StorageLocationPvt where ForeignID = i.ID)
                                    FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 0, '') AS roles
                                , i.* from Inventory i
                                where ISNULL(archive,0)=0 and ISNULL(assetItem,0)=0
                                order by name
            ");


                        var dt3 = sqlHelper.FillDataTable(@"
            select * from (
                            select tbl.*, (tbl.quantity + tbl.totAmount) totalCount
                            ,STUFF((
                                    SELECT distinct(cast(area + ' ' + bin as varchar))+', ' 
                                    FROM StorageLocation rpt
                                    WHERE rpt.ID in (select LocationID from StorageLocationPvt where ForeignID = i.ID)
                                    FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 0, '') AS roles
                                , i.* from Inventory i
                                left outer join (
                                    select distinct(ForeignID) ForeignID, sum(quantity) quantity 
                                    ,(
                                        select isnull(SUM(amount),0) amount 
                                        from StorageLocation_Log sll
                                        left outer join StorageLocationPvt slp on slp.ID = sll.StorageLocationPvtID
                                        where ForeignTable = 'inventory' and slp.ForeignID = slpp.ForeignID
                                    ) totAmount

                                    from StorageLocationPvt slpp where ForeignTable = 'inventory' and ISNULL(archive,0)=0 group by ForeignID) tbl
                                on tbl.ForeignID = i.ID
                                ) tbl1
                                where ISNULL(archive,0)=0 and ISNULL(assetItem,0)=0
                                order by name
                        ");

                        var dt1 = sqlHelper.FillDataTable(@"
                                        select cast(id as varchar) id, Name, isnull(Priority,999) priority, roleType
                                        from roles r where roleType not in ('other','staff') 							
                                        union
                                            (select '-5' id, 'Include Staff',-5, 'Staff'
                                            from (                        
                                                select STUFF((
                                                    SELECT distinct(cast(id as varchar))+', ' 
                                                    FROM Roles 
                                                    WHERE roleType = 'staff'
                                                    FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 0, '') AS roles						
                                            ) tbl)
                                        union
                                            (select '-4' id, 'Include Board Members',-4, roleType from Roles where name = 'Board Member')
                                        union
                                            (select '-3' id, 'Include State Board',-3, roleType from Roles where name = 'SBE Employee')
                                        order by Priority,Name
                        ");
            */

            sqlHelper.checkAndCreateTable(new RowSet<UglyTable>(), addColumnsIfMissing: true);
            var dt = sqlHelper.FillDataTable("select * from UglyTable");
            dt.Rows.Add("test", 1, 1.2);
            sqlHelper.Update(dt);

            DTIControls.Share.AdminPanelOn = true;
            Hashtable f = new Hashtable();
			Regex c;
			//for(var f in System.IO.DirectoryInfo("C:"))
		}

        public class UglyTable : PocoBase<UglyTable>
        {

            public string Display = string.Empty;
            public int? value = 0;
            public double? value2 = 0;
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