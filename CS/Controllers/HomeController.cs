using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;
using DevExpress.Web.Mvc;
using CS.Model;
using DevExpress.Web;
using System.Globalization;

namespace CS.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            return View(new MyViewModel { Products = MyModel.GetProducts() });
        }

        public ActionResult GridViewPartialProducts() {
            return PartialView(MyModel.GetProducts());
        }

        public ActionResult ExportTo() {
            XlsxExportOptionsEx exportOptions = new XlsxExportOptionsEx();
            exportOptions.CustomizeCell += new DevExpress.Export.CustomizeCellEventHandler(exportOptions_CustomizeCell);
            return GridViewExtension.ExportToXlsx(GridViewHelper.ExportGridViewSettings, MyModel.GetProducts(), exportOptions);
        }

        void exportOptions_CustomizeCell(DevExpress.Export.CustomizeCellEventArgs ea) {
            if(ea.AreaType != DevExpress.Export.SheetAreaType.Header && ea.ColumnFieldName == "UnitPrice") {
                if(Convert.ToDecimal(ea.Value) > 15)
                    ea.Formatting.BackColor = System.Drawing.Color.Yellow;
                else
                    ea.Formatting.BackColor = System.Drawing.Color.Green;
                ea.Handled = true;
            }
        }
    }

}
public static class GridViewHelper {
    private static GridViewSettings exportGridViewSettings;

    public static GridViewSettings ExportGridViewSettings {
        get {
            if(exportGridViewSettings == null)
                exportGridViewSettings = CreateExportGridViewSettings();
            return exportGridViewSettings;
        }
    }

    private static GridViewSettings CreateExportGridViewSettings() {
        GridViewSettings settings = new GridViewSettings();

        settings.Name = "gvProducts";
        settings.CallbackRouteValues = new { Controller = "Home", Action = "GridViewPartialProducts" };

        settings.KeyFieldName = "ProductID";
        settings.Settings.ShowFilterRow = true;

        settings.Columns.Add("ProductID");
        settings.Columns.Add("ProductName");
        settings.Columns.Add("UnitPrice");

        return settings;
    }
}
