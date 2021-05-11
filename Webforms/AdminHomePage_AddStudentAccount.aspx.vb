'TODO AddStudentPage not tested yet
Public Class AdminHomePage_AddStudentAccount
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        OnPostBack()
        OnNonPostBack()
    End Sub

#Region "OnPostBack"

    Private Sub OnPostBack()
        If IsPostBack Then

        End If
    End Sub

#End Region

#Region "OnNonPostBack"

    Private Sub OnNonPostBack()
        If Not IsPostBack Then

        End If
    End Sub

#End Region

#Region "AddStudent"

    Protected Sub AddStudentButton_Click(sender As Object, e As EventArgs) Handles AddStudentButton.Click
        If Not IsUsernameValid() Then
            ShowErrorInErrorMessage("Username is not valid")
            Return
        End If

        If Not IsPasswordValid() Then
            ShowErrorInErrorMessage("Password is not valid")
            Return
        End If

        If Not IsConfirmPasswordValid() Then
            ShowErrorInErrorMessage("Password does not match confirm password")
            Return
        End If

        If Not IsEmailValid() Then
            ShowErrorInErrorMessage("Email is not valid")
            Return
        End If

        HideErrorMessage()
        AddStudent()
    End Sub

    Private Function IsUsernameValid() As Boolean
        Dim username As String = UsernameField.Text
        Return PortalQueriesAndActions.NewUsernameConstraint.TryEvaluate(username)
    End Function

    Private Function IsPasswordValid() As Boolean
        If GeneratePasswordCheckbox.Checked Then
            Return True
        End If

        Dim password As String = PasswordField.Text
        Return PortalQueriesAndActions.NewPasswordConstraint.TryEvaluate(password)
    End Function

    Private Function IsConfirmPasswordValid() As Boolean
        If GeneratePasswordCheckbox.Checked Then
            Return True
        End If

        Dim confirmPassword As String = ConfirmPasswordField.Text
        Dim password As String = PasswordField.Text

        Return confirmPassword.Equals(password)
    End Function

    Private Function IsEmailValid() As Boolean
        Dim email As String = EmailField.Text
        Return PortalQueriesAndActions.NewEmailAddressConstraint.TryEvaluate(email)
    End Function

    Private Sub ShowErrorInErrorMessage(errorMsg As String)
        ErrorLabel.Visible = True
        ErrorLabel.Text = errorMsg
    End Sub

    Private Sub HideErrorMessage()
        ErrorLabel.Visible = False
    End Sub

    '
    Private Sub AddStudent()
        Dim username As String = UsernameField.Text
        Dim password As String = GetPasswordToUse()
        Dim email As String = EmailField.Text

        Dim builder As StudentAccount.Builder = New StudentAccount.Builder(username)
        builder.EmailAddress = email

        Dim adminUsername As String = Session.Item(SessionConstants.LOGGED_IN_USER)
        Dim action As PortalQueriesAndActions = New PortalQueriesAndActions(adminUsername)
        action.StudentAccountRelated.AttemptCreateStudentAccount(builder, password)
    End Sub

    Private Function GetPasswordToUse() As String
        If GeneratePasswordCheckbox.Checked Then
            Return PortalQueriesAndActions.RandomPasswordGenerator.GenerateRandomPassword()
        Else
            Return PasswordField.Text
        End If
    End Function

#End Region

End Class