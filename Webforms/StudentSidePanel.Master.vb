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

#Region "Other links"

    Protected Sub EditProfileButton_Click(sender As Object, e As EventArgs) Handles EditProfileButton.Click
        Response.RedirectPermanent(PageUrlConstants.STUDENT_EDIT_SELF_PROFILE_PAGE_URL)
    End Sub

    Protected Sub GradesButton_Click(sender As Object, e As EventArgs) Handles GradesButton.Click
        Response.RedirectPermanent(PageUrlConstants.STUDENT_HOME_PAGE_URL)
    End Sub

    Protected Sub EnrolledCourseButton_Click(sender As Object, e As EventArgs) Handles EnrolledCourseButton.Click
        Response.RedirectPermanent(PageUrlConstants.STUDENT_ENROLLED_COURSES_PAGE_URL)
    End Sub

    Protected Sub RequestDocuLink_Click(sender As Object, e As EventArgs) Handles RequestDocuLink.Click
        Response.RedirectPermanent(PageUrlConstants.STUDENT_REQUEST_DOCUMENT_PAGE_URL)
    End Sub

#End Region

End Class