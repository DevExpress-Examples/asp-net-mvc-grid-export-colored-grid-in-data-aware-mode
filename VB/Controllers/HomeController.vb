Imports System
Imports System.IO
Imports System.Web
Imports System.Web.Mvc
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraPrintingLinks
Imports DevExpress.Web.Mvc
Imports CS.Model
Imports DevExpress.Web

Namespace CS.Controllers
    Public Class HomeController
        Inherits Controller

        Public Function Index() As ActionResult
            Return View(New MyViewModel With {.Products = MyModel.GetProducts()})
        End Function

        Public Function GridViewPartialProducts() As ActionResult
            Return PartialView(MyModel.GetProducts())
        End Function

        Public Function ExportTo() As ActionResult
            Dim exportOptions As New XlsxExportOptionsEx()
            AddHandler exportOptions.CustomizeCell, AddressOf exportOptions_CustomizeCell
            Return GridViewExtension.ExportToXlsx(GridViewHelper.ExportGridViewSettings, MyModel.GetProducts(), exportOptions)
        End Function

        Private Sub exportOptions_CustomizeCell(ByVal ea As DevExpress.Export.CustomizeCellEventArgs)
            If ea.ColumnFieldName = "UnitPrice" Then
                If Convert.ToInt32(ea.Value) > 15 Then
                    ea.Formatting.BackColor = System.Drawing.Color.Yellow
                Else
                    ea.Formatting.BackColor = System.Drawing.Color.Green
                End If
                ea.Handled = True
            End If

        End Sub

    End Class

End Namespace
Public NotInheritable Class GridViewHelper

    Private Sub New()
    End Sub

    Private Shared exportGridViewSettings_Renamed As GridViewSettings

    Public Shared ReadOnly Property ExportGridViewSettings() As GridViewSettings
        Get
            If exportGridViewSettings_Renamed Is Nothing Then
                exportGridViewSettings_Renamed = CreateExportGridViewSettings()
            End If
            Return exportGridViewSettings_Renamed
        End Get
    End Property

    Private Shared Function CreateExportGridViewSettings() As GridViewSettings
        Dim settings As New GridViewSettings()

        settings.Name = "gvProducts"
        settings.CallbackRouteValues = New With {Key .Controller = "Home", Key .Action = "GridViewPartialProducts"}

        settings.KeyFieldName = "ProductID"
        settings.Settings.ShowFilterRow = True

        settings.Columns.Add("ProductID")
        settings.Columns.Add("ProductName")
        settings.Columns.Add("UnitPrice")

        Return settings
    End Function
End Class
