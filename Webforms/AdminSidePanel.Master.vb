Public Class AdminSidePanel
    Inherits AdminMustBeLoggedIn_MasterPage

    Protected Overrides Sub OnPageLoad()
        Dim username As String = Session.Item(LOGGED_IN_USER)
        WelcomeUserLabel.Text = String.Format("Welcome, {0}", username)

        Dim privMode As PrivilageMode = PortalQueriesAndActions.AdminQueriesAndActions.GetPrivilageModeOfAdminAccount(Session.Item(SessionConstants.LOGGED_IN_USER))
        If privMode Is PrivilageMode.DEFAULT_ADMIN Or privMode Is PrivilageMode.SUPER_ADMIN Then
            ViewBackupPageButton.Visible = True
        Else
            ViewBackupPageButton.Visible = False
        End If

    End Sub

#Region "LogOutButton"

    Protected Sub LogOutButton_Click(sender As Object, e As EventArgs) Handles LogOutButton.Click
        LogOut()
    End Sub

#End Region

#Region "EditProfileButton"

    Protected Sub EditProfileButton_Click(sender As Object, e As EventArgs) Handles EditProfileButton.Click
        Response.RedirectPermanent(PageUrlConstants.ADMIN_EDIT_SELF_PROFILE_PAGE_URL)
    End Sub

    Protected Sub ViewAdminTableButton_Click(sender As Object, e As EventArgs) Handles ViewAdminTableButton.Click
        Response.RedirectPermanent(PageUrlConstants.ADMIN_VIEW_ADMIN_TABLE_PAGE_URL)
    End Sub

    Protected Sub ViewStudentTableButton_Click(sender As Object, e As EventArgs) Handles ViewStudentTableButton.Click
        Response.RedirectPermanent(PageUrlConstants.ADMIN_VIEW_STUDENT_TABLE_PAGE_URL)
    End Sub

    Protected Sub ViewBackupPageButton_Click(sender As Object, e As EventArgs) Handles ViewBackupPageButton.Click
        Response.RedirectPermanent(PageUrlConstants.ADMIN_VIEW_BACKUP_PAGE_URL)
    End Sub

    Protected Sub ChangeRequestableDocumentsButton_Click(sender As Object, e As EventArgs) Handles ChangeRequestableDocumentsButton.Click
        Response.RedirectPermanent(PageUrlConstants.ADMIN_EDIT_REQUESTABLE_DOCUMENT_PAGE_URL)
    End Sub

#End Region
End Class