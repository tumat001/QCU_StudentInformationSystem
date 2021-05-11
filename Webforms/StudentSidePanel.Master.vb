Public Class StudentSidePanel
    Inherits StudentMustBeLoggedIn_MasterPage

    Protected Overrides Sub OnPageLoad()
        Dim username As String = Session.Item(LOGGED_IN_USER)
        WelcomeLabel.Text = String.Format("Welcome, {0}", username)
    End Sub

#Region "Logout"

    Protected Sub LogOutButton_Click(sender As Object, e As EventArgs) Handles LogOutButton.Click
        LogOut()
    End Sub

#End Region

#Region "EditProfile"

    Protected Sub EditProfileButton_Click(sender As Object, e As EventArgs) Handles EditProfileButton.Click
        Response.RedirectPermanent(PageUrlConstants.STUDENT_EDIT_SELF_PROFILE_PAGE_URL)
    End Sub

#End Region

End Class