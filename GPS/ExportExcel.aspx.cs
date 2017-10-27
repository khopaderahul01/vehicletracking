using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;


public partial class ExportExcel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
      
        DataTable ds=new DataTable();
        ds = (DataTable)Session["report"] ;
        string fileName = (string)Session["ExportFileName"];
        HttpResponse response = HttpContext.Current.Response;
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
        // first let's clean up the response.object
        //response.Clear();
        response.Charset = "";
        response.Buffer = true;
        
        // set the response mime type for excel
        response.ContentType = "application/vnd.ms-excel";
        response.AddHeader("Content-Disposition", "attachment;filename=\"" + fileName+".xls" + "\"");
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);        
        // create a string writer
        // instantiate a datagrid
        Label l = new Label();
        DataGrid dg = new DataGrid();
        dg.DataSource = ds;
        dg.DataBind();
        l.Text = (string)Session["headerString"];
        l.RenderControl(htw);
        htw.WriteBreak();
        l.Text = "";
        htw.Write("<br/><br/><div id=\"report\" style=\"width:200px;\" >");
        l.RenderControl(htw);
        dg.DataSource = ds;
        dg.DataBind();
        dg.RenderControl(htw);
        dg.Style.Add("text-decoration", "none");
        dg.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
        dg.Style.Add("font-size", "5px");
        htw.Write("</div>");
        response.Write(sw.ToString());
        response.End();

   

    }
}