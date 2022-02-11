<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="_192043R_AS_ICA.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style2 {
            width: 168px;
        }
        .auto-style3 {
            width: 110px;
        }
        .auto-style4 {
            width: 50%;
        }
        .auto-style5 {
            width: 110px;
            height: 26px;
        }
        .auto-style6 {
            width: 168px;
            height: 26px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 id="welcome_text">WELCOME BACK</h1>
    <p>
        <table class="auto-style4">
            <tr>
                <td class="auto-style5">
                    <asp:Label ID="lbl_email" runat="server" Text="Email:"></asp:Label>
                </td>
                <td class="auto-style6">
                    <asp:Label ID="lbl_showEmail" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style5">
                    <asp:Label ID="lbl_name" runat="server" Text="Name:"></asp:Label>
                </td>
                <td class="auto-style6">
                    <asp:Label ID="lbl_showName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style3">
                    <asp:Label ID="lbl_dob" runat="server" Text="Date of Birth:"></asp:Label>
                </td>
                <td class="auto-style2">
                    <asp:Label ID="lbl_showDOB" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style3">&nbsp;</td>
                <td class="auto-style2">
                    <asp:Button ID="btn_logout" runat="server" Text="Logout" Width="83px" OnClick="btn_logout_Click" />
                </td>
            </tr>
        </table>
    </p>
</asp:Content>
