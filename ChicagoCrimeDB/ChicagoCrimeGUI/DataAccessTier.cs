﻿//
// Data Access Tier:  interface between business tier and data store.
//

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace DataAccessTier
{

  public class Data
  {
    //
    // Fields:
    //
    private string _DBFile;
    private string _DBConnectionInfo;


    ///
    /// <summary>
    /// Constructs a new instance of the data access tier.  The format
    /// of the filename should be either |DataDirectory|\filename.mdf,
    /// or a complete Windows pathname.
    /// </summary>
    /// <param name="DatabaseFilename">Name of database file</param>
    /// 
    public Data(string DatabaseFilename)
    {
      string version;

      version = "MSSQLLocalDB";  // for VS 2015:

      _DBFile = DatabaseFilename;
      _DBConnectionInfo = String.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename={1};Integrated Security=True;",
        version,
        DatabaseFilename);
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
      SqlConnection db = new SqlConnection(_DBConnectionInfo);

      bool  state = false;

      try
      {
        db.Open();

        state = (db.State == ConnectionState.Open);
      }
      catch
      {
        // nothing, just discard:
      }
      finally
      {
        db.Close();
      }

      return state;
    }


    ///
    /// <summary>
    /// Executes an sql SELECT query that returns a single value.
    /// </summary>
    /// <param name="sql">query to execute</param>
    /// <returns>an object containing the single, scalar result</returns>
    ///
    public object ExecuteScalarQuery(string sql)
    {
            //
            // TODO!
            //
            string version = "MSSQLLocalDB";
            string filename = "|DataDirectory|\\CrimeDB.mdf";
            Data data = new Data(filename);
            string connectionInfo = String.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename=|DataDirectory|\{1};Integrated Security=True;", version, filename);
            SqlConnection db = new SqlConnection(data._DBConnectionInfo);
            //MessageBox.Show(connectionInfo);
            db.Open();
            //string msg = db.State.ToString();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db;
            cmd.CommandText = sql;
            object result = cmd.ExecuteScalar();
            db.Close();
            return result;
    }


    ///
    /// <summary>
    /// Executes an sql SELECT query that generates a temporary 
    /// table containing 0 or more rows.
    /// </summary>
    /// <param name="sql">query to execute</param>
    /// <returns>a table in the form of a DataSet</returns>
    /// 
    public DataSet ExecuteNonScalarQuery(string sql)
    {
            //
            // TODO!
            //
            //string version = "MSSQLLocalDB";
            string filename = "|DataDirectory|\\CrimeDB.mdf";
            Data data = new Data(filename);
            //string connectionInfo = String.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename=|DataDirectory|\{1};Integrated Security=True;", version, filename);
            SqlConnection db = new SqlConnection(data._DBConnectionInfo);
            //MessageBox.Show(connectionInfo);
            db.Open();
            //string msg = db.State.ToString();


            //string sql = string.Format("Select IUCR,PrimaryDesc,SecondaryDesc from Codes ORDER BY IUCR ASC");

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db;
            cmd.CommandText = sql;


            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            cmd.CommandText = sql;
            adapter.Fill(ds);

            db.Close();
            return ds;
        }

  }//class

}//namespace
