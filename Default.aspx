<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MP3Thumbnails._Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MP3 Stream</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server" />
    <div>
        Upload mp3:
            <asp:FileUpload ID="upload" runat="server" />
            <asp:Button ID="submitButton" runat="server" Text="Submit" OnClick="submitButton_Click" />
    </div>
    <div>
            <asp:UpdatePanel ID="up1" runat="server">
                <ContentTemplate>
                    <asp:ListView ID="MP3SamplesDisplayControl" runat="server">
                        <LayoutTemplate>
                            <div id="itemPlaceholder" runat="server" />
                        </LayoutTemplate>
                        <ItemTemplate>
                            <div>
                                <asp:Literal ID="label" Text='<%# Eval("Title") %>' runat="server"/>
                                <audio src='<%# Eval("Url") %>' controls="" preload="none"></audio>
                            </div>
                        </ItemTemplate>
                    </asp:ListView>
                    <asp:Button ID="refreshButton" runat="server" Text="Refresh" OnClick="Page_PreRender"/>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
