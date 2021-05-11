'TODO AddAdminPage not tested yet
Public Class AdminHomePage_AddAdminAccount
    Inherits System.Web.UI.Page

    Private Shared ReadOnly PRIVILAGE_MODE_NORMAL As String = "Normal Admin"
    Private Shared ReadOnly PRIVILAGE_MODE_SUPER As String = "Super Admin"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        OnPostBack()
        OnNonPostBack()

        InitializePrivilageModeList()
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

#Region "Initialization"

    Private Sub InitializePrivilageModeList()
        PrivilageModeDropDownList.Items.Clear()
        PrivilageModeDropDownList.Items.Add(PRIVILAGE_MODE_NORMAL)
        PrivilageModeDropDownList.Items.Add(PRIVILAGE_MODE_SUPER)
    End Sub

#End Region

#Region "AddStudent"

    Protected Sub AddAdminButton_Click(sender As Object, e As EventArgs) Handles AddAdminButton.Click
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

        If Not IsPrivilageModeValid() Then
            'This error message should not happen (normally)
            ShowErrorInErrorMessage("Priviage Mode is not valid")
            Return
        End If

        HideErrorMessage()
        AddAdmin()
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

    Private Function IsPrivilageModeValid() As Boolean
        Return PrivilageModeDropDownList.SelectedValue.Equals(PRIVILAGE_MODE_NORMAL) OrElse
               PrivilageModeDropDownList.SelectedValue.Equals(PRIVILAGE_MODE_SUPER)
    End Function

    Private Sub ShowErrorInErrorMessage(errorMsg As String)
        ErrorLabel.Visible = True
        ErrorLabel.Text = errorMsg
    End Sub

    Private Sub HideErrorMessage()
        ErrorLabel.Visible = False
    End Sub

    '
    Private Sub AddAdmin()
        Dim username As String = UsernameField.Text
        Dim password As String = GetPasswordToUse()
        Dim privilageMode As PrivilageMode = GetPrivilageModeToUse()

        Dim builder As AdminAccount.Builder = New AdminAccount.Builder(username, privilageMode)

        Dim adminExecuterUsername As String = Session.Item(SessionConstants.LOGGED_IN_USER)
        Dim action As PortalQueriesAndActions = New PortalQueriesAndActions(adminExecuterUsername)
        action.AdminAccountRelated.AttemptCreateAdminAccount(builder, password)
    End Sub

    Private Function GetPasswordToUse() As String
        If GeneratePasswordCheckbox.Checked Then
            Return PortalQueriesAndActions.RandomPasswordGenerator.GenerateRandomPassword()
        Else
            Return PasswordField.Text
        End If
    End Function

    Private Function GetPrivilageModeToUse() As PrivilageMode
        Dim selectedPrivilageMode As String = PrivilageModeDropDownList.SelectedValue

        If selectedPrivilageMode.Equals(PRIVILAGE_MODE_NORMAL) Then
            Return PrivilageMode.NORMAL_ADMIN
        Else
            Return PrivilageMode.SUPER_ADMIN
        End If
    End Function

#End Region

End Class