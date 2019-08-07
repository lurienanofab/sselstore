Imports System.IO
Imports sselStore.AppCode
Imports BLL = sselStore.AppCode.BLL

Namespace Admin
    Public Class CategoryManager
        Inherits StorePage

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not String.IsNullOrEmpty(hidOpenImage.Value) Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "toggle-image-script", "$(document).ready(function(){ toggleImage(" + hidOpenImage.Value + "); });", True)
            End If

            If Not Page.IsPostBack Then
                ddlParent.DataSource = BLL.CategoryManager.SelectParents()
                ddlParent.DataBind()

                rptCategories.DataSource = BLL.CategoryManager.SelectParents()
                rptCategories.DataBind()
            End If
        End Sub

        Protected Sub CategoryItem_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            litErrorMessageTop.Text = String.Empty
            litErrorMessageBottom.Text = String.Empty

            Dim ritem As RepeaterItem = rptCategories.Items(Convert.ToInt32(e.CommandArgument))
            Dim catID As Integer = Convert.ToInt32(CType(ritem.FindControl("lblCatID"), Label).Text)

            Select Case e.CommandName
                Case "edit"
                    ritem.FindControl("divEditButton").Visible = False
                    ritem.FindControl("divSaveCancelButton").Visible = True

                    ritem.FindControl("trUploadImage").Visible = True

                    ritem.FindControl("lblParentID").Visible = False
                    ritem.FindControl("txtEditParentID").Visible = True

                    ritem.FindControl("lblHierarchy").Visible = False
                    ritem.FindControl("txtEditHierarchy").Visible = True

                    ritem.FindControl("lblName").Visible = False
                    ritem.FindControl("txtEditName").Visible = True

                    ritem.FindControl("lblDescription").Visible = False
                    ritem.FindControl("txtEditDescription").Visible = True

                    CType(ritem.FindControl("chkActive"), CheckBox).Enabled = True

                    CType(ritem.FindControl("chkDisplay"), CheckBox).Enabled = True
                Case Else
                    If e.CommandName = "save" Then
                        Dim parentID As Integer
                        If Not Integer.TryParse(CType(ritem.FindControl("txtEditParentID"), TextBox).Text, parentID) Then
                            litErrorMessageTop.Text = "ParentID must be an integer."
                            litErrorMessageBottom.Text = "ParentID must be an integer."
                            Return
                        End If

                        Dim hierarchy As Integer
                        If Not Integer.TryParse(CType(ritem.FindControl("txtEditHierarchy"), TextBox).Text, hierarchy) Then
                            litErrorMessageTop.Text = "Hierarchy must be an integer."
                            litErrorMessageBottom.Text = "Hierarchy must be an integer."
                            Return
                        End If

                        Dim name As String = CType(ritem.FindControl("txtEditName"), TextBox).Text
                        If name = String.Empty Then
                            litErrorMessageTop.Text = "Please enter a category name."
                            litErrorMessageBottom.Text = "Please enter a category name."
                            Return
                        End If

                        Dim desc As String = CType(ritem.FindControl("txtEditDescription"), TextBox).Text
                        Dim active As Boolean = CType(ritem.FindControl("chkActive"), CheckBox).Checked
                        Dim display As Boolean = CType(ritem.FindControl("chkDisplay"), CheckBox).Checked

                        BLL.CategoryManager.Update(catID, parentID, hierarchy, name, desc, active, display)

                        Dim fupCatImage As FileUpload = CType(ritem.FindControl("fupCatImage"), FileUpload)
                        If fupCatImage.HasFile Then
                            Dim saveas_path As String = Server.MapPath("~/images/items/") + "cat" + catID.ToString() + ".jpg"
                            If File.Exists(saveas_path) Then File.Delete(saveas_path)
                            fupCatImage.SaveAs(saveas_path)
                        End If

                        Dim p As String = ddlParent.SelectedValue
                        ddlParent.DataSource = BLL.CategoryManager.SelectParents()
                        ddlParent.DataBind()
                        ddlParent.SelectedValue = p

                        rptCategories.DataSource = BLL.CategoryManager.SelectByParentID(Convert.ToInt32(p))
                        rptCategories.DataBind()
                    End If
                    ritem.FindControl("divEditButton").Visible = True
                    ritem.FindControl("divSaveCancelButton").Visible = False

                    ritem.FindControl("trUploadImage").Visible = False

                    ritem.FindControl("lblParentID").Visible = True
                    ritem.FindControl("txtEditParentID").Visible = False

                    ritem.FindControl("lblHierarchy").Visible = True
                    ritem.FindControl("txtEditHierarchy").Visible = False

                    ritem.FindControl("lblName").Visible = True
                    ritem.FindControl("txtEditName").Visible = False

                    ritem.FindControl("lblDescription").Visible = True
                    ritem.FindControl("txtEditDescription").Visible = False

                    CType(ritem.FindControl("chkActive"), CheckBox).Enabled = False

                    CType(ritem.FindControl("chkDisplay"), CheckBox).Enabled = False
            End Select
        End Sub

        Protected Sub btnAddCategory_Click(ByVal sender As Object, ByVal e As EventArgs)
            litErrorMessageTop.Text = String.Empty
            litErrorMessageBottom.Text = String.Empty

            Dim footer As Control = rptCategories.Controls(rptCategories.Controls.Count - 1)
            Dim isParent As Boolean = (ddlParent.SelectedValue = "-1")

            Dim parentID As Integer
            If isParent Then
                parentID = 0
            Else
                If Not Integer.TryParse(CType(footer.FindControl("txtAddParentID"), TextBox).Text, parentID) Then
                    litErrorMessageTop.Text = "ParentID must be an integer."
                    litErrorMessageBottom.Text = "ParentID must be an integer."
                    Return
                End If
            End If

            Dim hierarchy As Integer
            If Not Integer.TryParse(CType(footer.FindControl("txtAddHierarchy"), TextBox).Text, hierarchy) Then
                litErrorMessageTop.Text = "Hierarchy must be an integer."
                litErrorMessageBottom.Text = "Hierarchy must be an integer."
                Return
            End If

            Dim name As String = CType(footer.FindControl("txtAddName"), TextBox).Text
            If name = String.Empty Then
                litErrorMessageTop.Text = "Please enter a category name."
                litErrorMessageBottom.Text = "Please enter a category name."
                Return
            End If

            Dim desc As String = CType(footer.FindControl("txtAddDescription"), TextBox).Text

            If isParent Then
                BLL.CategoryManager.AddParent(hierarchy, name, desc)
            Else
                BLL.CategoryManager.AddChild(parentID, hierarchy, name, desc)
            End If

            Dim p As String = ddlParent.SelectedValue
            ddlParent.DataSource = BLL.CategoryManager.SelectParents()
            ddlParent.DataBind()
            ddlParent.SelectedValue = p

            rptCategories.DataSource = BLL.CategoryManager.SelectByParentID(Convert.ToInt32(p))
            rptCategories.DataBind()

            If p <> "-1" Then
                footer = rptCategories.Controls(rptCategories.Controls.Count - 1)
                CType(footer.FindControl("txtAddParentID"), TextBox).Visible = True
                CType(footer.FindControl("txtAddParentID"), TextBox).Enabled = False
                CType(footer.FindControl("txtAddParentID"), TextBox).Text = p
                CType(footer.FindControl("txtAddHierarchy"), TextBox).Text = "1"
            End If
        End Sub

        Protected Sub ddlParent_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)

            If ddlParent.SelectedValue = "-1" Then
                rptCategories.DataSource = BLL.CategoryManager.SelectParents()
                rptCategories.DataBind()

                Dim footer As Control = rptCategories.Controls(rptCategories.Controls.Count - 1)
                Dim txtAddParentID As TextBox = CType(footer.FindControl("txtAddParentID"), TextBox)
                Dim txtAddHierarchy As TextBox = CType(footer.FindControl("txtAddHierarchy"), TextBox)

                txtAddParentID.Visible = False
                txtAddHierarchy.Enabled = False
                txtAddHierarchy.Text = "0"
            Else
                rptCategories.DataSource = BLL.CategoryManager.SelectByParentID(Convert.ToInt32(ddlParent.SelectedValue))
                rptCategories.DataBind()

                Dim footer As Control = rptCategories.Controls(rptCategories.Controls.Count - 1)
                Dim txtAddParentID As TextBox = CType(footer.FindControl("txtAddParentID"), TextBox)
                Dim txtAddHierarchy As TextBox = CType(footer.FindControl("txtAddHierarchy"), TextBox)

                txtAddParentID.Visible = True
                txtAddParentID.Enabled = False
                txtAddParentID.Text = ddlParent.SelectedValue
                txtAddHierarchy.Enabled = False
                txtAddHierarchy.Text = "1"
            End If
        End Sub

        Protected Sub ddlParent_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles ddlParent.DataBound
            ddlParent.Items.Insert(0, New ListItem("[All]", "-1"))
        End Sub

        Protected Function CatImgURL(ByVal catID As String) As String
            Return "~/images/items/" + BLL.CategoryManager.CategoryImageFileName(Convert.ToInt32(catID))
        End Function
    End Class
End Namespace