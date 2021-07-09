Public Class AdminHomePage_EditStudentProfile
    Inherits System.Web.UI.Page

    Private selectedStudentUsernames As HashSet(Of String)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        OnNonPostBack()
        OnPostBack()

        UpdateDisplayOfSelectedAdmins()
    End Sub

#Region "OnNonPostBack"
    Private Sub OnNonPostBack()
        If Not IsPostBack Then
            NoChangePasswordRadioButton.Checked = True
            NoChangeEmailRadioButton.Checked = True
        End If
    End Sub
#End Region

#Region "OnPostBack"
    Private Sub OnPostBack()
        If IsPostBack Then

        End If
    End Sub
#End Region

#Region "DisplayUsernames"

    Private Sub UpdateDisplayOfSelectedAdmins()
        selectedStudentUsernames = Session.Item(AdminHomePage_ViewStudentTableContent.SELECTED_STUDENT_USERNAME_SESSION_CONSTANT)

        SelectedAccountsListBox.Items.Clear()
        For Each username As String In selectedStudentUsernames
            SelectedAccountsListBox.Items.Add(username)
        Next
    End Sub

#End Region

#Region "Confirm"
    Protected Sub ConfirmChangesButton_Click(sender As Object, e As EventArgs) Handles ConfirmChangesButton.Click
        EditSelectedStudents()

        Response.Redirect(PageUrlConstants.ADMIN_VIEW_STUDENT_TABLE_PAGE_URL)
    End Sub

    Private Sub EditSelectedStudents()
        If Not AreInputsValid() Then
            Return
        End If

        For Each usernameItem As ListItem In SelectedAccountsListBox.Items
            Dim username As String = usernameItem.ToString()

            Try
                Dim actionsAsSelf As PortalQueriesAndActions = New PortalQueriesAndActions(Session.Item(SessionConstants.LOGGED_IN_USER))

                EditStudentEmail(actionsAsSelf, username)
                EditStudentPassword(actionsAsSelf, username)
            Catch ex As AccountDoesNotExistException

            End Try
        Next

    End Sub

    Private Sub EditStudentEmail(actionsAsSelf As PortalQueriesAndActions, targetUsername As String)
        If SetManuallyEmailRadioButton.Checked Then
            Dim newEmail As String = ManualSetEmailField.Text
            actionsAsSelf.StudentAccountRelated.ChangeEmailAddressOfStudentAccount(targetUsername, newEmail)
        End If
    End Sub

    Private Sub EditStudentPassword(actionsAsSelf As PortalQueriesAndActions, targetUsername As String)
        Dim newPassword As String = Nothing

        If SetManuallyPasswordRadioButton.Checked Then
            newPassword = ManualSetPasswordField.Text
        ElseIf AutoGeneratePasswordRadioButton.Checked Then
            newPassword = PortalQueriesAndActions.RandomPasswordGenerator.GenerateRandomPassword()
        End If

        If newPassword IsNot Nothing Then
            actionsAsSelf.StudentAccountRelated.ChangePasswordOfStudentAccount(targetUsername, newPassword)
        End If
    End Sub

#End Region

#Region "InputChecking"

    Private Function AreInputsValid() As Boolean
        If Not IsEmailValid() Then
            Return False
        End If

        If Not IsPasswordValid() Then
            Return False
        End If

        Return True
    End Function

    Private Function IsEmailValid() As Boolean
        If SetManuallyEmailRadioButton.Checked Then
            Dim emailToCheck As String = ManualSetEmailField.Text

            If emailToCheck.Length = 0 Then
                ShowEmailErrorMsg("Email field is required")
                Return False
            End If

            If Not PortalQueriesAndActions.NewEmailAddressConstraint.TryEvaluate(emailToCheck) Then
                ShowEmailErrorMsg("Email is not valid")
                Return False
            End If

            HideEmailErrorMsg()
            Return True
        Else
            HideEmailErrorMsg()
            Return True
        End If
    End Function

    Private Sub ShowEmailErrorMsg(msg As String)
        ErrorInEmailLabel.Text = msg
        ErrorInEmailLabel.Visible = True
    End Sub

    Private Sub HideEmailErrorMsg()
        ErrorInEmailLabel.Visible = False
    End Sub

    Private Function IsPasswordValid() As Boolean
        If SetManuallyPasswordRadioButton.Checked Then
            Dim inputPassword As String = ManualSetPasswordField.Text
            Dim inputConfirmPassword As String = ConfirmPasswordField.Text

            If Not inputPassword.Equals(inputConfirmPassword) Then
                ShowPasswordErrorMsg("Passwords do not match")
                Return False
            End If

            If inputPassword.Length = 0 Then
                ShowPasswordErrorMsg("Password field is required")
                Return False
            End If

            If Not PortalQueriesAndActions.NewPasswordConstraint.TryEvaluate(inputPassword) Then
                ShowPasswordErrorMsg("Passowrd field is not valid")
                Return False
            End If

            HidePasswordErrorMsg()
            Return True
        Else
            HidePasswordErrorMsg()
            Return True
        End If
    End Function

    Private Sub ShowPasswordErrorMsg(msg As String)
        ErrorInPasswordLabel.Text = msg
        ErrorInPasswordLabel.Visible = True
    End Sub

    Private Sub HidePasswordErrorMsg()
        ErrorInPasswordLabel.Visible = False
    End Sub

#End Region

#Region "Cancel"
    Protected Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        Response.Redirect(PageUrlConstants.ADMIN_VIEW_STUDENT_TABLE_PAGE_URL)
    End Sub
#End Region

End Class