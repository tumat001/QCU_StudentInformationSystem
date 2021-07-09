Public Class AdminHomePage_BackupPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "CreateBackup"
    Protected Sub CreateBackupButton_Click(sender As Object, e As EventArgs) Handles CreateBackupButton.Click
        Try
            Dim actionsAs As PortalQueriesAndActions = New PortalQueriesAndActions(Session.Item(LOGGED_IN_USER))
            actionsAs.PromptCreateDatabaseBackup()
        Catch ex As AccountDoesNotExistException
            DirectCast(Master, AdminMustBeLoggedIn_MasterPage).LogOut()
        End Try
    End Sub
#End Region

#Region "LoadBackup"
    Protected Sub LoadBackupButton_Click(sender As Object, e As EventArgs) Handles LoadBackupButton.Click
        Try
            Dim actionsAs As PortalQueriesAndActions = New PortalQueriesAndActions(Session.Item(LOGGED_IN_USER))
            actionsAs.PromptLoadDatabaseBackup()
        Catch ex As AccountDoesNotExistException
            DirectCast(Master, AdminMustBeLoggedIn_MasterPage).LogOut()
        End Try
    End Sub
#End Region

End Class