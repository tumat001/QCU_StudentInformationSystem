Public Class AdminHomePage_RequestableDocumentEditPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            RefreshListBoxData()
        End If
    End Sub

    Private Sub RefreshListBoxData()
        Dim allDocu As IReadOnlyList(Of String) = PortalQueriesAndActions.AdminQueriesAndActions.GetAllRequestDocumentOptionsList()
        ReqDocumentListBox.Items.Clear()
        For Each docu As String In allDocu
            ReqDocumentListBox.Items.Add(New ListItem(docu))
        Next
    End Sub

    Protected Sub AddDocumentButton_Click(sender As Object, e As EventArgs) Handles AddDocumentButton.Click
        If Not AddDocumentLabel.Text.Length = 0 Then
            ReqDocumentListBox.Items.Add(AddDocumentField.Text)
        End If
    End Sub

    Protected Sub UpdateSelectedDocumentButton_Click(sender As Object, e As EventArgs) Handles UpdateSelectedDocumentButton.Click
        If Not ReqDocumentListBox.SelectedIndex = -1 Then
            If Not UpdateSelectedDocumentField.Text.Length = 0 Then
                ReqDocumentListBox.Items(ReqDocumentListBox.SelectedIndex).Text = UpdateSelectedDocumentField.Text
                ReqDocumentListBox.Items(ReqDocumentListBox.SelectedIndex).Value = UpdateSelectedDocumentField.Text
            End If
        End If
    End Sub

    Protected Sub DeleteSelectedDocument_Click(sender As Object, e As EventArgs) Handles DeleteSelectedDocument.Click
        If Not ReqDocumentListBox.SelectedIndex = -1 Then
            ReqDocumentListBox.Items.RemoveAt(ReqDocumentListBox.SelectedIndex)
        End If
    End Sub


    Protected Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        RefreshListBoxData()
    End Sub

    Protected Sub FinalizeChangesButton_Click(sender As Object, e As EventArgs) Handles FinalizeChangesButton.Click
        Dim newList As IList(Of String) = New List(Of String)

        For Each docuLItem As ListItem In ReqDocumentListBox.Items
            newList.Add(docuLItem.Value)
        Next

        Dim actions As PortalQueriesAndActions = New PortalQueriesAndActions(Session.Item(SessionConstants.LOGGED_IN_USER))
        actions.AdminAccountRelated.UpdateRequestDocumentsList(newList)
    End Sub

End Class