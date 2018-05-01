using CIF.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace CIF.Statics
{
    public partial class SQL : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                string sql = Request.Form["TextArea1"];
                command.CommandText = sql;
                try
                {
                    if (!sql.StartsWith("SELECT"))
                    {
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        DataTable table = new DataTable();
                        table.Load(reader);

                        //Building an HTML string.
                        StringBuilder html = new StringBuilder();

                        //Table start.
                        html.Append("<table border = '1'>");

                        //Building the Header row.
                        html.Append("<tr>");
                        foreach (DataColumn column in table.Columns)
                        {
                            html.Append("<th>");
                            html.Append(column.ColumnName);
                            html.Append("</th>");
                        }
                        html.Append("</tr>");

                        //Building the Data rows.
                        foreach (DataRow row in table.Rows)
                        {
                            html.Append("<tr>");
                            foreach (DataColumn column in table.Columns)
                            {
                                html.Append("<td>");
                                html.Append(row[column.ColumnName]);
                                html.Append("</td>");
                            }
                            html.Append("</tr>");
                        }

                        //Table end.
                        html.Append("</table>");

                        //Append the HTML string to Placeholder.
                        PlaceHolder1.Controls.Add(new Literal { Text = html.ToString() });
                    }
                }
                catch (Exception ex)
                {
                    lbeError.Text = ex.Message;
                }
            }
        }
    }
}