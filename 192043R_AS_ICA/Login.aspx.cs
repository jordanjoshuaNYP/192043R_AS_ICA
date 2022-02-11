using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _192043R_AS_ICA
{
    public partial class Login : System.Web.UI.Page
    {
        string AccountDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AccountDBConnection"].ConnectionString;
        public class MyObject
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["registered"] != null)
            {
                lbl_msg.Text = "Account registered successfully.";
                Session.Remove("registered");
            }
        }

        protected void btn_login_Click(object sender, EventArgs e)
        {
            string email = tb_email.Text.ToString().Trim();
            string pass = tb_password.Text.ToString().Trim();

            SHA512Managed hashing = new SHA512Managed();
            string dbHash = getDBHash(email);
            string dbSalt = getDBSalt(email);
            int dbLoginFail = getDBLoginFail(email);

            try
            {
                if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                {
                    if (!(Convert.ToInt32(dbLoginFail) < 3))
                    {
                        lbl_msg.Text = "Account locked.";
                    }
                    else
                    {
                        string passWSalt = pass + dbSalt;
                        byte[] hashWSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(passWSalt));
                        string userhash = Convert.ToBase64String(hashWSalt);

                        if (userhash == dbHash)
                        {
                            Session["loggedIn"] = tb_email.Text.ToString().Trim();

                            // reset failed attempts after logged in successfully
                            SqlConnection con = new SqlConnection(AccountDBConnectionString);
                            string sqlstr = "UPDATE [Account] SET failedLogin = 0 WHERE email=@email";
                            SqlCommand cmd = new SqlCommand(sqlstr, con);
                            cmd.Parameters.AddWithValue("@email", email);
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();

                            // create a GUID
                            string guid = Guid.NewGuid().ToString();
                            // save new Guid into a session
                            Session["AuthToken"] = guid;

                            // create cookie with save vaule as session "AuthToken"
                            Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                            Response.Redirect("Home.aspx", false);
                        }
                        else
                        {
                            lbl_msg.Text = "Email or password incorrect. Please try again";
                            addFail(email);
                        }
                    }
                }
                else
                {
                    lbl_msg.Text = "Email or password incorrect. Please try again";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        protected string getDBHash(string email)
        {
            string h = null;
            SqlConnection con = new SqlConnection(AccountDBConnectionString);
            string sqlstr = "select passwordHash FROM [Account] WHERE email=@email";
            SqlCommand cmd = new SqlCommand(sqlstr, con);
            cmd.Parameters.AddWithValue("@email", email);
            try
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["passwordHash"] != null)
                        {
                            if (reader["passwordHash"] != DBNull.Value)
                            {
                                h = reader["passwordHash"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { con.Close(); }
            return h;
        }

        protected string getDBSalt(string email)
        {
            string s = null;
            SqlConnection con = new SqlConnection(AccountDBConnectionString);
            string sqlstr = "select passwordSalt FROM [Account] WHERE email=@email";
            SqlCommand cmd = new SqlCommand(sqlstr, con);
            cmd.Parameters.AddWithValue("@email", email);
            try
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["passwordSalt"] != null)
                        {
                            if (reader["passwordSalt"] != DBNull.Value)
                            {
                                s = reader["passwordSalt"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { con.Close(); }
            return s;
        }

        protected int getDBLoginFail(string email)
        {
            int count = 0;
            SqlConnection con = new SqlConnection(AccountDBConnectionString);
            string sqlstr = "select failedLogin FROM [Account] WHERE email=@email";
            SqlCommand cmd = new SqlCommand(sqlstr, con);
            cmd.Parameters.AddWithValue("@email", email);
            try
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["failedLogin"] != null)
                        {
                            if (reader["failedLogin"] != DBNull.Value)
                            {
                                string strcount = reader["failedLogin"].ToString();
                                count = int.Parse(strcount);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { con.Close(); }
            return count;
        }

        protected void addFail(string email)
        {
            int Fails = getDBLoginFail(email);

            SqlConnection con = new SqlConnection(AccountDBConnectionString);
            string sqlstr = "UPDATE [Account] SET failedLogin =@failedLogin WHERE email =@email";
            SqlCommand cmd = new SqlCommand(sqlstr, con);
            int addfail = Fails + 1;
            cmd.Parameters.AddWithValue("@failedLogin", addfail);
            cmd.Parameters.AddWithValue("@email", email);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public bool ValidateCaptcha()
        {
            bool result = true;


            string captchaResponse = Request.Form["g-recaptcha-response"];


            HttpWebRequest req = (HttpWebRequest)WebRequest.Create
                ("https://www.google.com/recaptcha/api/siteverify?secret= &response=" + captchaResponse);

            try
            {

                using (WebResponse webRes = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(webRes.GetResponseStream()))
                    {
                        // The response in JSON format
                        string jsonResponse = readStream.ReadToEnd();

                        JavaScriptSerializer js = new JavaScriptSerializer();

                        // Create jsonObject to handle the response i.e. success or error
                        // Deserialize Json
                        MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);

                        // Convert the string
                        result = Convert.ToBoolean(jsonObject.success);
                    }
                }

                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

       
    }
}