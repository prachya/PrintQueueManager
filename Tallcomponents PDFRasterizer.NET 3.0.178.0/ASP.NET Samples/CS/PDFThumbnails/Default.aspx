<%@ Page language="c#" Inherits="PdfThumbnails._Default" %>

<HTML>
    <body>
        <form method="post" runat="server" enctype="multipart/form-data">
            <input width="100%" type="file" size="50" name="pdf" /><br/>
            <asp:button Runat="server" Text="Display thumbs" ID="DisplayThumbs" OnClick="DisplayThumbs_Click" /><br/>
            <asp:Panel ID="thumbnails" runat="server" />
        </form>
    </body>
</HTML>
