//
// BusinessTier:  business logic, acting as interface between UI and data store.
//

using System;
using System.Collections.Generic;
using System.Data;


namespace BusinessTier
{
  ///
  /// <summary>
  /// Ways to sort the Areas in Chicago.
  /// </summary>
  /// 
  public enum OrderAreas
  {
    ByNumber,
    ByName
  };


  //
  // Business:
  //
  public class Business
  {
    //
    // Fields:
    //
    private string _DBFile;
    private DataAccessTier.Data dataTier;


    ///
    /// <summary>
    /// Constructs a new instance of the business tier.  The format
    /// of the filename should be either |DataDirectory|\filename.mdf,
    /// or a complete Windows pathname.
    /// </summary>
    /// <param name="DatabaseFilename">Name of database file</param>
    /// 
    public Business(string DatabaseFilename)
    {
      _DBFile = DatabaseFilename;

      dataTier = new DataAccessTier.Data(DatabaseFilename);
    }


    ///
    /// <summary>
    ///  Opens and closes a connection to the database, e.g. to
    ///  startup the server and make sure all is well.
    /// </summary>
    /// <returns>true if successful, false if not</returns>
    /// 
    public bool OpenCloseConnection()
    {
      return dataTier.OpenCloseConnection();
    }

            
    ///
    /// <summary>
    /// Returns overall stats about crimes in Chicago.
    /// </summary>
    /// <returns>CrimeStats object</returns>
    ///
    public CrimeStats GetOverallCrimeStats()
    {
      CrimeStats cs;
      Business bus = new Business("C:\\Users\\Jacky\\Downloads\\CrimeDB.mdf");
      string sql = @"
Select Min(Year) As MinYear, Max(Year) As MaxYear, Count(*) As Total
From Crimes;
";
            DataSet result = dataTier.ExecuteNonScalarQuery(sql);
            DataRow row = result.Tables["TABLE"].Rows[0];

            int minYear = System.Convert.ToInt32(row["MinYear"]);
            int maxYear = System.Convert.ToInt32(row["MaxYear"]);
            long total = System.Convert.ToInt64(row["Total"]);
            //
            // TODO!
            //
            cs = new CrimeStats(total, minYear, maxYear);

      return cs;
    }


    ///
    /// <summary>
    /// Returns all the areas in Chicago, ordered by area # or name.
    /// </summary>
    /// <param name="ordering"></param>
    /// <returns>List of Area objects</returns>
    /// 
    public List<Area> GetChicagoAreas(OrderAreas ordering)
    {
      List<Area> areas = new List<Area>();
            Business bus = new Business("C:\\Users\\Jacky\\Downloads\\CrimeDB.mdf");
            string sql = @"
SELECT * FROM Areas
WHERE Area > 0
ORDER BY AreaName ASC;
";
            //
            // TODO!
            //
            //int counter = 1;
            DataSet result = dataTier.ExecuteNonScalarQuery(sql);
            
                foreach (DataRow row in result.Tables["TABLE"].Rows)
                {
                    Area a;
                    a = new Area((Convert.ToInt32(row["Area"])), Convert.ToString(row["AreaName"]));
                    areas.Add(a);
                    //counter++;
                }
            


      return areas;
    }


    ///
    /// <summary>
    /// Returns all the crime codes and their descriptions.
    /// </summary>
    /// <returns>List of CrimeCode objects</returns>
    ///
    public List<CrimeCode> GetCrimeCodes()
    {
      List<CrimeCode> codes = new List<CrimeCode>();
            Business bus = new Business("C:\\Users\\Jacky\\Downloads\\CrimeDB.mdf");

            //
            // TODO!
            //
            string sql = @"
SELECT * FROM Codes 
ORDER BY IUCR;
";
            DataSet result = dataTier.ExecuteNonScalarQuery(sql);

            foreach (DataRow row in result.Tables["TABLE"].Rows)
            {
                CrimeCode c;
                c = new CrimeCode(Convert.ToString(row["IUCR"]), Convert.ToString(row["PrimaryDesc"]), Convert.ToString(row["SecondaryDesc"]));
                codes.Add(c);
            }
               

      return codes;
    }


    ///
    /// <summary>
    /// Returns a hash table of years, and total crimes each year.
    /// </summary>
    /// <returns>Dictionary where year is the key, and total crimes is the value</returns>
    ///
    public Dictionary<int, long> GetTotalsByYear()
    {
      Dictionary<int, long> totalsByYear = new Dictionary<int, long>();
            Business bus = new Business("C:\\Users\\Jacky\\Downloads\\CrimeDB.mdf");
            string sql = @"
Select Year, Count(*) As Total
From Crimes
Group By Year
Order By Year ASC;
";
            DataSet result = dataTier.ExecuteNonScalarQuery(sql);
            foreach (DataRow row in result.Tables["TABLE"].Rows)
            {
                totalsByYear.Add(Convert.ToInt32(row["Year"]), Convert.ToInt64(row["Total"]));
            }
            //
            // TODO!
            //

            return totalsByYear;
    }


    ///
    /// <summary>
    /// Returns a hash table of months, and total crimes each month.
    /// </summary>
    /// <returns>Dictionary where month is the key, and total crimes is the value</returns>
    /// 
    public Dictionary<int, long> GetTotalsByMonth()
    {
      Dictionary<int, long> totalsByMonth = new Dictionary<int, long>();
            Business bus = new Business("C:\\Users\\Jacky\\Downloads\\CrimeDB.mdf");

            //
            // TODO!
            //
            string sql = @"
SELECT DatePart(month, CrimeDate) As Month, COUNT(*) As Total
FROM Crimes
GROUP BY DatePart(month, CrimeDate)
ORDER BY DatePart(month, CrimeDate);
";
            DataSet result = dataTier.ExecuteNonScalarQuery(sql);
            foreach (DataRow row in result.Tables["TABLE"].Rows)
            {
                totalsByMonth.Add(Convert.ToInt32(row["Month"]), Convert.ToInt64(row["Total"]));
            }
            return totalsByMonth;
    }


    ///
    /// <summary>
    /// Returns a hash table of areas, and total crimes each area.
    /// </summary>
    /// <returns>Dictionary where area # is the key, and total crimes is the value</returns>
    ///
    public Dictionary<int, long> GetTotalsByArea()
    {
      Dictionary<int, long> totalsByArea = new Dictionary<int, long>();
            Business bus = new Business("C:\\Users\\Jacky\\Downloads\\CrimeDB.mdf");
            //
            // TODO!
            //
            string sql = @"
Select Area, Count(*) As Total
From Crimes
Where Area > 0
Group By Area
Order By Area ASC;
";


            DataSet result = dataTier.ExecuteNonScalarQuery(sql);
            foreach (DataRow row in result.Tables["TABLE"].Rows)
            {
                totalsByArea.Add(Convert.ToInt32(row["Area"]), Convert.ToInt64(row["Total"]));
            }
            return totalsByArea;
    }


    ///
    /// <summary>
    /// Returns a hash table of years, and arrest percentages each year.
    /// </summary>
    /// <returns>Dictionary where the year is the key, and the arrest percentage is the value</returns>
    /// 
    public Dictionary<int, double> GetArrestPercentagesByYear()
    {
      Dictionary<int, double> percentagesByYear = new Dictionary<int, double>();
            Business bus = new Business("C:\\Users\\Jacky\\Downloads\\CrimeDB.mdf");

            string sql = @"
Select Year, Count(*) As Total, Avg(Convert(float,Arrested))*100.0 As ArrestPercentage
From Crimes
Group By Year
Order By Year;
";
            //
            // TODO!
            //
            DataSet result = dataTier.ExecuteNonScalarQuery(sql);
            foreach (DataRow row in result.Tables["TABLE"].Rows)
            {
                percentagesByYear.Add(Convert.ToInt32(row["Year"]), Convert.ToInt64(row["ArrestPercentage"]));
            }
            return percentagesByYear;
    }
    

    public long crimesClicked(int p1, string p2, int p3)
    {
            Business bus = new Business("C:\\Users\\Jacky\\Downloads\\CrimeDB.mdf");
            string where = "";
            if(p1 != -1 )
            {
                where = string.Format("Year = {0}", p1);
            }
            if(!(p2.Equals("")))
            {
                if (where.Length > 0)
                    where += " AND ";
                where += string.Format(" IUCR = '{0}'", p2);
            }
            if(p3!=-1)
            {
                if (where.Length > 0)
                    where += " AND ";
                where += string.Format(" Area = '{0}'", p3);
            }
            

            string sql = string.Format(@"
SELECT Count(*) As Total 
FROM Crimes 
WHERE {0};
",
where);
            object result = dataTier.ExecuteScalarQuery(sql);
            long total = System.Convert.ToInt64(result);
            return total;
        }

        public Dictionary<string, int> topArea(int num)
        {
            Dictionary<string, int> topAreaNum = new Dictionary<string, int>();
            Business bus = new Business("C:\\Users\\Jacky\\Downloads\\CrimeDB.mdf");
            string sql = string.Format(@"
SELECT TOP {0} AreaName, Count(*) AS Total
FROM Crimes
INNER JOIN
  (SELECT * FROM AREAS WHERE Area > 0) AS T
ON T.Area = Crimes.Area
GROUP BY T.AreaName
ORDER BY Total DESC;
",
num);
            DataSet result = dataTier.ExecuteNonScalarQuery(sql);
            foreach (DataRow row in result.Tables["TABLE"].Rows)
            {
                topAreaNum.Add(Convert.ToString(row["AreaName"]), Convert.ToInt32(row["Total"]));
            }
            
            return topAreaNum;
        }

        public Dictionary<SmallerCrimeCode, int> topTypes(int num)
        {
            Dictionary<SmallerCrimeCode, int> topTypeNum = new Dictionary<SmallerCrimeCode, int>();
            Business bus = new Business("C:\\Users\\Jacky\\Downloads\\CrimeDB.mdf");
            string sql = string.Format(@"
SELECT TOP {0} T.PrimaryDesc, T.SecondaryDesc, Count(*) AS Total
FROM Crimes
INNER JOIN
  (SELECT * FROM Codes) AS T
ON T.IUCR = Crimes.IUCR
GROUP BY T.PrimaryDesc, T.SecondaryDesc
ORDER BY Total DESC;
",
num);
            DataSet result = dataTier.ExecuteNonScalarQuery(sql);
            foreach (DataRow row in result.Tables["TABLE"].Rows)
            {
                // topTypeNum.Add(Convert.ToString(row["T.PrimaryDesc"]), Convert.ToString(row["T.SecondaryDesc"]), Convert.ToInt32(row["Total"]));
                SmallerCrimeCode c;
                c = new SmallerCrimeCode(Convert.ToString(row["PrimaryDesc"]), Convert.ToString(row["SecondaryDesc"]));
                topTypeNum.Add(c, Convert.ToInt32(row["Total"]));
            }

            return topTypeNum;
        }
        public Dictionary<string, int> topAreaType(int num, string code)
        {
            Dictionary<string, int> top = new Dictionary<string, int>();
            Business bus = new Business("C:\\Users\\Jacky\\Downloads\\CrimeDB.mdf");
            string sql = string.Format(@"
SELECT TOP {0} AreaName, Count(*) AS Total
FROM Crimes
INNER JOIN
  (SELECT * FROM AREAS WHERE Area > 0) AS T
ON T.Area = Crimes.Area
WHERE Crimes.IUCR = '{1}'
GROUP BY T.AreaName
ORDER BY Total DESC;
",
num,
code);

            DataSet result = dataTier.ExecuteNonScalarQuery(sql);
            foreach (DataRow row in result.Tables["TABLE"].Rows)
            {
                top.Add(Convert.ToString(row["AreaName"]), Convert.ToInt32(row["Total"]));
            }
            return top;
        }
        public Dictionary<SmallerCrimeCode, int> topAreaYear(int top,int min, int max,string name)
        {

            Dictionary<SmallerCrimeCode, int> topTypeNum = new Dictionary<SmallerCrimeCode, int>();
            Business bus = new Business("C:\\Users\\Jacky\\Downloads\\CrimeDB.mdf");
            string sql = string.Format(@"
SELECT TOP {0} T.PrimaryDesc, T.SecondaryDesc, Count(*) AS Total
FROM Crimes
INNER JOIN
  (SELECT * FROM Codes) AS T
ON T.IUCR = Crimes.IUCR
WHERE Crimes.Year >= {1} AND 
      Crimes.Year <= {2} AND
      Crimes.Area = (SELECT Area FROM AREAS WHERE AreaName = '{3}')
GROUP BY T.PrimaryDesc, T.SecondaryDesc
ORDER BY Total DESC;
",
top,
min,
max,
name);
            DataSet result = dataTier.ExecuteNonScalarQuery(sql);
            foreach (DataRow row in result.Tables["TABLE"].Rows)
            {
                // topTypeNum.Add(Convert.ToString(row["T.PrimaryDesc"]), Convert.ToString(row["T.SecondaryDesc"]), Convert.ToInt32(row["Total"]));
                SmallerCrimeCode c;
                c = new SmallerCrimeCode(Convert.ToString(row["PrimaryDesc"]), Convert.ToString(row["SecondaryDesc"]));
                topTypeNum.Add(c, Convert.ToInt32(row["Total"]));
            }

            return topTypeNum;
        }
    }//class
}//namespace
