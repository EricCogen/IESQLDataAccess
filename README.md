# IESQLDataAccess
An Easy To Use Lite Weight MSSQL Data Access Layer For .Net


# Examples:
## Get Data Example:
	public List<Detail> Details = null;

	public class Detail
	{
		public string Field1{get;set;}
		public string Field2{get;set;}
		public string Field3{get;set;}
	}

	private void LoadDetails
	{
		Details = new List<Detail>();
		DataTable DT = new DataTable();
		Parameters p = new Parameters();
		p.Add("@Status", SqlDbType.VarChar, "valid", 255);
		Executers Exec = new Executers("SQL-SERVER-001", "Data", true);
		Exec.Execute("dbo.GetData", p, ref DT);
		if (!string.IsNullOrEmpty(Exec.Status))
		{
			//raise exception here;
		}
		else
		{
			if (DT != null && DT.Rows != null)
			{
				foreach (DataRow dr in DT.Rows)
				{
					Detail m = new Detail();
					m.Field1 = dr.Field<string>("Field1");
					m.Field2 = dr.Field<string>("Field2");
					m.Field3 = dr.Field<string>("Field3");
					Details.Add(m);
				}
			}
		}
	}







## Save Data Example:
	private void SaveDetails(Detail d)
	{
		DataTable DT = new DataTable();
		Parameters p = new Parameters();

		p.Add("@Status", SqlDbType.VarChar, "Valid", 255);
		p.Add("@Field1", SqlDbType.VarChar, d.Field1, 50);
		p.Add("@DateUpdated", SqlDbType.DateTime, DateTime.Now);
		Executers Exec = new Executers("SQL-SERVER-001", "Data", true);
		Exec.Execute("dbo.SaveData", p, ref DT);
		if (!string.IsNullOrEmpty(Exec.Status))
		{
			//raise exception here;
		}
	}







## Add Data Example:
	private void SaveDetails(Detail d)
	{
			DataTable DT = new DataTable();
			Parameters p = new Parameters();

			p.Add("@Status", SqlDbType.VarChar, "Valid", 255);
			p.Add("@Field1", SqlDbType.VarChar, d.Field1, 50);
			p.Add("@DateUpdated", SqlDbType.DateTime, DateTime.Now);
			Executers Exec = new Executers("SQL-SERVER-001", "Data", true);
			Exec.Execute("dbo.SaveData", p, ref DT);
			if (!string.IsNullOrEmpty(Exec.Status))
			{
					//raise exception here;
			}
			else
			{
					if (DT != null && DT.Rows != null && DT.Rows.Count > 0)
					{
							//INSERT INTO dbo.Details ... OUTPUT inserted.DetailID ...
							d.DetailID = DT.Rows[0].Field<int>("DetailID");
					}
			}
	}


