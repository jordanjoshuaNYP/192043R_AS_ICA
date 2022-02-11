<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="_192043R_AS_ICA.Registration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 67%;
            height: 77px;
        }
        .auto-style2 {
            height: 26px;
        }
        .auto-style3 {
            width: 184px;
        }
        .auto-style4 {
            height: 26px;
            width: 184px;
        }
        .auto-style5 {
            width: 254px;
        }
        .auto-style6 {
            height: 26px;
            width: 254px;
        }
        .auto-style7 {
            width: 184px;
            height: 29px;
        }
        .auto-style8 {
            width: 254px;
            height: 29px;
        }
        .auto-style9 {
            height: 29px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="https://www.google.com/recaptcha/api.js?render=6LfWjnAeAAAAAOv7ch_uQuJbslxOxKTLGsZdc2j3"></script>    <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
    <script>
        function validate() {
            var str = document.getElementById('<%=tb_password.ClientID%>');
            str = str.value
            console.log(str)
            if (str.length < 8) {
                document.getElementById("<%=lbl_pwdStat.ClientID%>").innerHTML = "Password length must be at least 8 characters.";
                document.getElementById("<%=lbl_pwdStat.ClientID%>").style.color = "Red";
                document.getElementById("<%=lbl_pwdStat.ClientID%>").style.display = "show";
                return ("too_short");
            }
            else if (str.search(/[0-9]/) == -1) {
                document.getElementById("<%=lbl_pwdStat.ClientID%>").innerHTML = "Password require at least 1 number.";
                document.getElementById("<%=lbl_pwdStat.ClientID%>").style.color = "Red";
                return ("no_number");
            }
            else if (str.search(/[A-Z]/) == -1) {
                document.getElementById("<%=lbl_pwdStat.ClientID%>").innerHTML = "Password require at least 1 uppercase.";
                document.getElementById("<%=lbl_pwdStat.ClientID%>").style.color = "Red";
                return ("no_upper");
            }
            else if (str.search(/[a-z]/) == -1) {
                document.getElementById("<%=lbl_pwdStat.ClientID%>").innerHTML = "Password require at least 1 lowercase.";
                document.getElementById("<%=lbl_pwdStat.ClientID%>").style.color = "Red";
                return ("no_lower");
            }
            else if (str.search(/[ `!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?~]/) == -1) {
                document.getElementById("<%=lbl_pwdStat.ClientID%>").innerHTML = "Password require at least 1 special character.";
                document.getElementById("<%=lbl_pwdStat.ClientID%>").style.color = "Red";
                return ("no_special");
            }
            else {
                document.getElementById("<%=lbl_pwdStat.ClientID%>").innerHTML = " ";
            }
        }

        function cdnumval() {
            var str = document.getElementById('<%=tb_creditNum.ClientID%>').value;
            if (str.length != 16) {

                document.getElementById("<%=lbl_creditNumStat.ClientID%>").innerHTML = "Credit card number must be 16 characters";
                document.getElementById("<%=lbl_creditNumStat.ClientID%>").style.color = "Red";
                return ("too_short");
            }
            else {
                var retn = true;
                for (let i = 0; i < str.length; i++) {
                    if (isNaN(str[i])) retn = false; break;
                }



                //is all numbers
                if (retn) {
                    document.getElementById("<%=lbl_creditNumStat.ClientID%>").innerHTML = " ";
                }
                else {
                    document.getElementById("<%=lbl_creditNumStat.ClientID%>").innerHTML = "Must be numbers";
                    document.getElementById("<%=lbl_creditNumStat.ClientID%>").style.color = "Red";
                }
            }
        }



         grecaptcha.ready(function () {
             grecaptcha.execute('6LfWjnAeAAAAAOv7ch_uQuJbslxOxKTLGsZdc2j3', { action: 'Login' }).then(function (token) {
            document.getElementById("g-recaptcha-response").value = token;
         });
         });
    </script>
    <h1>REGISTRATION</h1>
    <p/>
        <table class="auto-style1">
            <tr>
                <td class="auto-style3">
                    <asp:Label ID="Label1" runat="server" Text="First Name:"></asp:Label>
                </td>
                <td class="auto-style5">
                    <asp:TextBox ID="tb_fname" runat="server" Width="250px"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lbl_fnStat" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style7">
                    <asp:Label ID="Label2" runat="server" Text="Last Name:"></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:TextBox ID="tb_lname" runat="server" Width="250px"></asp:TextBox>
                </td>
                <td class="auto-style9">
                    <asp:Label ID="lbl_lnStat" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style7">
                    <asp:Label ID="Label3" runat="server" Text="Email:"></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:TextBox ID="tb_email" runat="server" Width="250px"></asp:TextBox>
                </td>
                <td class="auto-style9">
                    <asp:Label ID="lbl_emailStat" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style7">
                    <asp:Label ID="Label4" runat="server" Text="Date of Birth:"></asp:Label>
                </td>
                <td class="auto-style8">
                    <asp:TextBox ID="tb_dob" runat="server" TextMode="Date" Width="250px"></asp:TextBox>
                </td>
                <td class="auto-style9">
                    <asp:Label ID="lbl_dobStat" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style4">
                    <asp:Label ID="Label5" runat="server" Text="Password:"></asp:Label>
                </td>
                <td class="auto-style6">
                    <asp:TextBox ID="tb_password" runat="server" TextMode="Password" Width="250px" onkeyup="javascript:validate()"></asp:TextBox>
                </td>
                <td class="auto-style2">
                    <asp:Label ID="lbl_pwdStat" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style3">
                    <asp:Label ID="Label6" runat="server" Text="Confirm Password:"></asp:Label>
                </td>
                <td class="auto-style5">
                    <asp:TextBox ID="tb_passwordCfm" runat="server" TextMode="Password" Width="250px"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lbl_pwdConfirm" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style3">&nbsp;</td>
                <td class="auto-style5">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style3">
                    <asp:Label ID="Label7" runat="server" Text="Credit Card Number:"></asp:Label>
                </td>
                <td class="auto-style5">
                    <asp:TextBox ID="tb_creditNum" runat="server" Width="250px" onkeyup="javascript:cdnumval()"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lbl_creditNumStat" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style3">
                    <asp:Label ID="Label8" runat="server" Text="Credit Card Expiry: (MM/YYYY)"></asp:Label>
                </td>
                <td class="auto-style5">
                    <asp:TextBox ID="tb_creditExpiry" runat="server" Width="250px"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lbl_creditExpiryStat" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style3">
                    <asp:Label ID="Label9" runat="server" Text="Credit Card CVV:"></asp:Label>
                </td>
                <td class="auto-style5">
                    <asp:TextBox ID="tb_creditCVV" runat="server" Width="250px"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lbl_creditCVVStat" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style3">&nbsp;</td>
                <td class="auto-style5">
                    <asp:Button ID="btn_submit" runat="server" OnClick="btn_submit_Click" Text="Submit" Width="258px" />
                </td>
                <td>&nbsp;</td>
            </tr>
        </table>
    </p/>

</asp:Content>
