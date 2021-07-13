Public Class LogInPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CheckIfAlreadyLoggedIn()
    End Sub

#Region "CheckIfAlreadyLoggedIn"

    Private Sub CheckIfAlreadyLoggedIn()
        Dim username As String = Session.Item(SessionConstants.LOGGED_IN_USER)

        If username IsNot Nothing Then
            If PortalQueriesAndActions.StudentQueriesAndActions.IfStudentAccountExists(username) Then
                LogInAsStudent(username)
            ElseIf PortalQueriesAndActions.AdminQueriesAndActions.IfAdminAccountExists(username) Then
                LogInAsAdmin(username)
            End If
        End If
    End Sub

#End Region

    Protected Sub LogInButton_Click(sender As Object, e As EventArgs) Handles LogInButton.Click
        AttemptLogIn()
    End Sub

#Region "LogInAttempt"

    Private Sub AttemptLogIn()
        Dim inputUsername As String = UsernameField.Text
        Dim inputPassword As String = PasswordField.Text

        Dim safe As Boolean = InspectInput(inputUsername, inputPassword)
        If Not safe Then
            ShowIncorrectMessage()
            Return
        End If

        If Not IfAccountExists(inputUsername) Then
            ShowIncorrectMessage()
            Return
        End If

        If IfStudentCredentialsCheckOut(inputUsername, inputPassword) Then
            LogInAsStudent(inputUsername)
        ElseIf IfAdminCredentialsCheckOut(inputUsername, inputPassword) Then
            LogInAsAdmin(inputUsername)
        Else
            ShowIncorrectMessage()
        End If
    End Sub

    'Mainly as protection from injection
    Private Function InspectInput(username As String, password As String) As Boolean
        If Not PortalQueriesAndActions.NewUsernameConstraint.TryEvaluate(username) Then
            Return False
        End If
        If Not PortalQueriesAndActions.NewPasswordConstraint.TryEvaluate(password) Then
            Return False
        End If
        Return True
    End Function

    Private Sub ShowIncorrectMessage()
        IncorrectInputLabel.Visible = True
    End Sub

    Private Function IfAccountExists(username As String) As Boolean
        Return PortalQueriesAndActions.IfAccountExists(username)
    End Function

    Private Function IfStudentCredentialsCheckOut(username As String, password As String) As Boolean
        Try
            Return PortalQueriesAndActions.StudentQueriesAndActions.AttemptLogInAsStudent(username, password)
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function IfAdminCredentialsCheckOut(username As String, password As String) As Boolean
        Try
            Return PortalQueriesAndActions.AdminQueriesAndActions.IsPasswordOfAdminAccountEqualTo(username, password)
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub LogInAsStudent(username As String)
        SetUsernameSession(username)
        SetHomePageSession(PageUrlConstants.STUDENT_HOME_PAGE_URL)
        Response.RedirectPermanent(PageUrlConstants.STUDENT_HOME_PAGE_URL)
    End Sub

    Private Sub LogInAsAdmin(username As String)
        SetUsernameSession(username)
        SetHomePageSession(PageUrlConstants.ADMIN_HOME_PAGE_URL)
        Response.RedirectPermanent(PageUrlConstants.ADMIN_HOME_PAGE_URL)
    End Sub

    Private Sub SetUsernameSession(username As String)
        If Session.Item(SessionConstants.LOGGED_IN_USER) Is Nothing Then
            Session.Add(SessionConstants.LOGGED_IN_USER, username)
        Else
            Session.Item(SessionConstants.LOGGED_IN_USER) = username
        End If
    End Sub

    Private Sub SetHomePageSession(homepage As String)
        If Session.Item(SessionConstants.REDIRECT_HOME_ADDRESS_OF_GENERIC_PAGE) Is Nothing Then
            Session.Add(SessionConstants.REDIRECT_HOME_ADDRESS_OF_GENERIC_PAGE, homepage)
        Else
            Session.Item(SessionConstants.REDIRECT_HOME_ADDRESS_OF_GENERIC_PAGE) = homepage
        End If
    End Sub

#End Region

    'Protected Sub ShowPasswordCheckbox_CheckedChanged(sender As Object, e As EventArgs) Handles ShowPasswordCheckbox.CheckedChanged
    '    Dim fieldMode As TextBoxMode = PasswordField.TextMode

    '    If fieldMode = TextBoxMode.Password Then
    '        PasswordField.TextMode = TextBoxMode.SingleLine

    '    ElseIf fieldMode = TextBoxMode.SingleLine Then
    '        PasswordField.TextMode = TextBoxMode.Password

    '    End If
    'End Sub

End Class