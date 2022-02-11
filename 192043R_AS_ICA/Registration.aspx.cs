using System;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace _192043R_AS_ICA
{
    public partial class Registration : System.Web.UI.Page
    {
        // Define database paras
        string AccountDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AccountDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;
        bool allOk = true;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected bool Validation()
        {
            // init a bool value
            bool validate = true;

            // check if firstName is empty
            if (string.IsNullOrEmpty(tb_fname.Text.ToString()))
            {
                lbl_fnStat.Text = "Field required.";
                validate = false;

            }

            // check if firstName has only alphabets
            if (!Regex.IsMatch(tb_fname.Text.Trim(), "^[a-zA-Z]{3,20}$"))
            {
                lbl_fnStat.Text = "Field can only contain alphabets.";
                validate = false;
            }

            // check if lastName is empty
            if (string.IsNullOrEmpty(tb_lname.Text.ToString()))
            {
                lbl_lnStat.Text = "Field required.";
                validate = false;

            }

            // check if lastName has only alphabets
            if (!Regex.IsMatch(tb_fname.Text.Trim(), "^[a-zA-Z]{3,20}$"))
            {
                lbl_lnStat.Text = "Field can only contain alphabets.";
                validate = false;
            }

            // check if email is empty
            if (string.IsNullOrEmpty(tb_email.Text.ToString()))
            {
                lbl_emailStat.Text = "Field required.";
                validate = false;

            }

            // check if Email is valid
            if (!Regex.IsMatch(tb_email.Text.Trim(), @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{1,})$"))
            {
                lbl_emailStat.Text = "Email invalid.";
                validate = false;
            }
            
            // check if dob is valid
            if (!DateTime.TryParse(tb_dob.Text.ToString().Trim(), out DateTime dob))
            {
                lbl_dobStat.Text = "Invalid date of birth";
                validate = false;
            }

            // check for password strength
            if (!PassStrength())
            {
                validate = false;
            }

            // check if both password & confirmpassword are the same
            if (tb_password.Text.ToString() != tb_passwordCfm.Text.ToString())
            {
                lbl_pwdConfirm.Text = "Password and Confirm Password is not the same";
            }

            // validate credit card number
            if (!Regex.IsMatch(tb_creditNum.Text.ToString().Trim(), "^[0-9]{16,16}$"))
            {
                lbl_creditNumStat.Text = "Invalid credit card number";
            }

            // Making Credit card expiry date normal date time
            string expirydate = "28/" + tb_creditExpiry.Text.ToString().Trim();

            // validate expiry date
            if (!DateTime.TryParse(expirydate, out DateTime expiredate))
            {
                lbl_creditExpiryStat.Text = "Invalid expiry date";
                validate = false;
            }

            // check if expiry date is over
            if (DateTime.Compare(Convert.ToDateTime(expiredate), DateTime.Now) < 0)
            {
                lbl_creditExpiryStat.Text = "Card expired";
                validate = false;
            }

            // validate cvv
            if (!Regex.IsMatch(tb_creditCVV.Text.ToString().Trim(), "^[0-9]{3,4}$"))
            {
                lbl_creditCVVStat.Text = "Invalid CVV";
                validate = false;
            }

            return validate;

        }

        // check password strength
        protected bool PassStrength()
        {
            bool strongpass = true;
            string weakness = "";

            if (tb_password.Text.ToString().Trim().Length < 8)
            {
                weakness += "Password cannot be less than 8 character long.\n";
                strongpass = false;
            }
            if (!Regex.IsMatch(tb_password.Text.Trim(), "[a-z]"))
            {
                weakness += "Password requires at least one lower case character.\n";
                strongpass = false;
            }
            if (!Regex.IsMatch(tb_password.Text.Trim(), "[A-Z]"))
            {
                weakness += "Password requires at least one upper case character.\n";
                strongpass = false;
            }
            if (!Regex.IsMatch(tb_password.Text.Trim(), "[0-9]"))
            {
                weakness += "Password requires at least one numeric character.\n";
                strongpass = false;
            }
            if (!Regex.IsMatch(tb_password.Text.Trim(), "[^A-Za-z0-9]"))
            {
                weakness += "Password requires at least one special character.";
                strongpass = false;
            }
            lbl_pwdStat.Text = weakness;
            return strongpass;
        }

        public void createAccount()
        {
           /* try
            {*/
                using (SqlConnection con = new SqlConnection(AccountDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO [Account] VALUES(@fname, @lname, @email, @dob, @password, @passwordHash, @passwordSalt, @creditNum, @creditExpiry, @creditCVV, @IV, @Key, @failedLogin)"))
                    {
                    Random r = new Random();
                    int rng = r.Next();

                        Guid id = Guid.NewGuid();
                        cmd.CommandType = CommandType.Text;
                    //cmd.Parameters.AddWithValue("@id", id.ToString());
                        //cmd.Parameters.AddWithValue("@rng", rng);
                        cmd.Parameters.AddWithValue("@fname", tb_fname.Text.ToString().Trim());
                        cmd.Parameters.AddWithValue("@lname", tb_lname.Text.ToString().Trim());
                        cmd.Parameters.AddWithValue("@email", tb_email.Text.ToString().Trim());
                        DateTime dob = Convert.ToDateTime(tb_dob.Text.ToString().Trim());
                        cmd.Parameters.AddWithValue("@dob", dob);
                        cmd.Parameters.AddWithValue("@password", finalHash);
                        cmd.Parameters.AddWithValue("@passwordHash", finalHash);
                        cmd.Parameters.AddWithValue("@passwordSalt", salt);
                        cmd.Parameters.AddWithValue("@creditNum", Convert.ToBase64String(encryptData(tb_creditNum.Text.ToString().Trim())));
                        cmd.Parameters.AddWithValue("@creditExpiry", Convert.ToBase64String(encryptData(tb_creditExpiry.Text.ToString().Trim())));
                        cmd.Parameters.AddWithValue("@creditCVV", Convert.ToBase64String(encryptData(tb_creditCVV.Text.ToString().Trim())));
                        cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                        cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
                        cmd.Parameters.AddWithValue("@failedLogin", 0);
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            /*}
            catch (Exception ex)
            {
                //throw new Exception(ex.ToString());
            }*/
        }

        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged Cipher = new RijndaelManaged();
                Cipher.Key = Key;
                Cipher.IV = IV;

                ICryptoTransform encryptTransformNumber = Cipher.CreateEncryptor();
                byte[] plainTextNumber = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransformNumber.TransformFinalBlock(plainTextNumber, 0, plainTextNumber.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return cipherText;
        }

        //private bool DoesAccountExist()
        //{
        //    bool exists = false;
        //    SqlConnection connection = new SqlConnection(AccountDBConnectionString);
        //    string sql = "SELECT * FROM Account WHERE email=@INPUTEMAIL";
        //    SqlCommand command = new SqlCommand(sql, connection);
        //    command.Parameters.AddWithValue("@INPUTEMAIL", tb_email.Text);
        //    try
        //    {
        //        connection.Open();
        //        using (SqlDataReader reader = command.ExecuteReader())
        //        {
        //            if (reader.Read()) { if (reader["email"] != DBNull.Value) exists = true; }
        //        }
        //    }
        //    catch { }
        //    finally
        //    {
        //        connection.Close();
        //    }
        //    return exists;
        //}

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            bool validated = Validation();
            // check if everything is valid
            if (validated)
            {
                string password = tb_password.Text.ToString().Trim();

                // generate salt (random)
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] saltByte = new byte[8];

                // fills byte array with strong sequence of random values
                rng.GetBytes(saltByte);
                salt = Convert.ToBase64String(saltByte);

                // generate hash
                SHA512Managed hashing = new SHA512Managed();

                // and salt to the password
                string passwsalt = password + salt;

                // hash password with salt
                byte[] hashwSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(passwsalt));

                finalHash = Convert.ToBase64String(hashwSalt);

                // generate cypher for credit card information
                RijndaelManaged cccipher = new RijndaelManaged();
                cccipher.GenerateKey();
                Key = cccipher.Key;
                IV = cccipher.IV;


                createAccount();

                Session["registered"] = "Registered";

                Response.Redirect("login.aspx");

            }
        }


    }
}