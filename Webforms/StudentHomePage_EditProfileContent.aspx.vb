Public Class StudentHomePage_EditProfileContent
    Inherits System.Web.UI.Page

    Private Shared ReadOnly WRONG_CURRENT_PASSWORD_MESSAGE As String = "Incorrect current password!"
    Private Shared ReadOnly NEW_PASSWORDS_DO_NOT_MATCH_MESSAGE As String = "New passwords do not match!"
    Private Shared ReadOnly INVALID_NEW_PASSWORD_MESSAGE As String = "Invalid new password used!"
    Private Shared ReadOnly EXCEPTIONAL_ERROR_MESSAGE As String = "Fatal error occurred!"

#Region "ShowPassword"

    Protected Sub ShowNewPasswordCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles ShowNewPasswordCheckBox.CheckedChanged
        If ShowNewPasswordCheckBox.Checked Then

            NewPasswordField.TextMode = TextBoxMode.SingleLine
            ConfirmPasswordField.TextMode = TextBoxMode.SingleLine
        Else

            NewPasswordField.TextMode = TextBoxMode.Password
            ConfirmPasswordField.TextMode = TextBoxMode.Password
        End If
    End Sub

#End Region

#Region "ChangePassword"

    Protected Sub ChangePasswordButton_Click(sender As Object, e As EventArgs) Handles ChangePasswordButton.Click
        If Not IsCurrentPasswordCorrect(CurrentPasswordField.Text) Then
            CustomErrorLabel.Text = WRONG_CURRENT_PASSWORD_MESSAGE
            Return
        End If

        If Not IsNewPasswordValid(NewPasswordField.Text) Then
            CustomErrorLabel.Text = INVALID_NEW_PASSWORD_MESSAGE
            Return
        End If

        If Not IfNewAndConfirmPasswordsMatch(NewPasswordField.Text, ConfirmPasswordField.Text) Then
            CustomErrorLabel.Text = NEW_PASSWORDS_DO_NOT_MATCH_MESSAGE
            Return
        End If

        'At this point, all are satisfied
        ChangePasswordOfSelf()
    End Sub

    Private Function IsCurrentPasswordCorrect(oldPassword As String) As Boolean
        Try
            Dim user As String = Session.Item(SessionConstants.LOGGED_IN_USER)
            Return PortalQueriesAndActions.StudentQueriesAndActions.IsPasswordOfStudentAccountEqualTo(user, oldPassword)
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function IsNewPasswordValid(newPassword As String) As Boolean
        Return PortalQueriesAndActions.NewPasswordConstraint.TryEvaluate(newPassword)
    End Function

    Private Function IfNewAndConfirmPasswordsMatch(newPassword As String, confirmPassword As String) As Boolean
        Return newPassword.Equals(confirmPassword)
    End Function

    Private Sub ChangePasswordOfSelf()
        Dim user As String = Session.Item(SessionConstants.LOGGED_IN_USER)
        Try
            Dim actionAs As PortalQueriesAndActions = New PortalQueriesAndActions(user)
            actionAs.StudentAccountRelated.ChangePasswordOfStudentAccount(user, NewPasswordField.Text)
        Catch ex As Exception
            CustomErrorLabel.Text = EXCEPTIONAL_ERROR_MESSAGE
        End Try
    End Sub

    Protected Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        Response.RedirectPermanent(PageUrlConstants.STUDENT_HOME_PAGE_URL)
    End Sub

#End Region

End Class