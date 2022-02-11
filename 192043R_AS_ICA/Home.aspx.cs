using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _192043R_AS_ICA
{
    public partial class Home : System.Web.UI.Page
    {
        string AccountDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AccountDBConnection"].ConnectionString;
        string email;
        string fname;
        string lname;
        string dob;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["loggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"].Value != null)
            {
                if (Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    lbl_email.Visible = true;
                    lbl_dob.Visible = true;
                    lbl_name.Visible = true;
                    btn_logout.Visible = true;

                    SqlConnection con = new SqlConnection(AccountDBConnectionString);
                    string sqlstr = "select * from [Account] where email = @email";
                    SqlCommand cmd = new SqlCommand(sqlstr, con);
                    cmd.Parameters.AddWithValue("@email", Session["loggedIn"].ToString());

                    try
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader["email"] != DBNull.Value)
                                {
                                    email = reader["email"].ToString();
                                }

                                if (reader["dob"] != DBNull.Value)
                                {
                                    dob = reader["dob"].ToString();
                                }

                                if (reader["fname"] != DBNull.Value)
                                {
                                    fname = reader["fname"].ToString();
                                }

                                if (reader["lname"] != DBNull.Value)
                                {
                                    fname = reader["lname"].ToString();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.ToString());
                    }
                    finally
                    {
                        con.Close();
                    }
                    lbl_showDOB.Text = dob;
                    lbl_showEmail.Text = email;
                    lbl_showName.Text = fname + lname;


                }
                else
                {
                    btn_logout.Visible = true;
                }
            }
            else
            {
                Response.Redirect("/login.aspx");
            }
        }

        protected void btn_logout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            Response.Redirect("Login.aspx", false);

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }

            if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }
        }
    }
}