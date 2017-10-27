using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

public partial class ExportToPDFMultiple : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable ds = new DataTable();
        ds = (DataTable)Session["reportMultiple"];
        string fileName = (string)Session["ExportFileNameMultiple"];

        if (ds.Columns.IndexOf("Get start Location") != -1)
        {
            ds.Columns.Remove("Get start Location");
        }
        if (ds.Columns.IndexOf("Get end Location") != -1)
        {
            ds.Columns.Remove("Get end Location");
        }
        if (ds.Columns.IndexOf("Get Location") != -1)
        {
            ds.Columns.Remove("Get Location");
        }
        HttpResponse Response = HttpContext.Current.Response;
        Response.ContentType = "application/pdf";
        Response.AddHeader("content-disposition", "attachment;filename=\"" + fileName + "\"");

        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        // instantiate a datagrid
        DataGrid dg = new DataGrid();
        Label l = new Label();
        l.Text = (string)Session["headerStringMultiple"];
        l.RenderControl(hw);
        hw.WriteBreak();
        l.Text = "";
        hw.Write("<br/><br/><div id=\"report\" style=\"width:200px;\" >");
        l.RenderControl(hw);
        dg.DataSource = ds;
        dg.DataBind();
        dg.RenderControl(hw);
        dg.Style.Add("text-decoration", "none");
        dg.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
        dg.Style.Add("font-size", "5px");
        hw.Write("</div>");
        StringReader sr = new StringReader(sw.ToString());
        Document pdfDoc = new Document(PageSize.A4, 50, 40, 40, 40);
        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        pdfDoc.Open();
        htmlparser.Parse(sr);
        pdfDoc.Close();
        sr.Close();
        sw.Close();
        hw.Close();
        Response.Write(pdfDoc);
        Response.End();

    }
}