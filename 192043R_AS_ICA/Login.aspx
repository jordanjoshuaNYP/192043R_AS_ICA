<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="_192043R_AS_ICA.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
            height: 26px;
        }
        .auto-style2 {
            width: 47%;
        }
        .auto-style3 {
            width: 134px;
        }
        .auto-style4 {
            height: 26px;
            width: 134px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="https://www.google.com/recaptcha/api.js?render=6LfWjnAeAAAAAOv7ch_uQuJbslxOxKTLGsZdc2j3"></script>    <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
    <script>
         grecaptcha.ready(function () {
             grecaptcha.execute('6LfWjnAeAAAAAOv7ch_uQuJbslxOxKTLGsZdc2j3', { action: 'Login' }).then(function (token) {
            document.getElementById("g-recaptcha-response").value = token;
         });
         });
    </script>
    <h1>LOGIN</h1>
    <p/>
        <table class="auto-style2">
            <tr>
                <td class="auto-style3">
                    <asp:Label ID="Label1" runat="server" Text="Email:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_email" runat="server" Width="250px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="auto-style4">
                    <asp:Label ID="Label2" runat="server" Text="Password:"></asp:Label>
                </td>
                <td class="auto-style1">
                    <asp:TextBox ID="tb_password" runat="server" Width="250px" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="auto-style4"></td>
                <td class="auto-style1">
                    <asp:Label ID="lbl_msg" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style4"></td>
                <td class="auto-style1">
                    <asp:Button ID="btn_login" runat="server" OnClick="btn_login_Click" Text="Login" Width="258px" />
                </td>
            </tr>
            <tr>
                <td class="auto-style4">&nbsp;</td>
                <td class="auto-style1">
                    <asp:HyperLink ID="hl_register" runat="server" NavigateUrl="~/Registration.aspx">Don&#39;t have an account? Register!</asp:HyperLink>
                </td>
            </tr>
        </table>
    </p/>
</asp:Content>
