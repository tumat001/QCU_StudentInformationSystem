Public Class AdminHomePage_EditAdminProfile
    Inherits System.Web.UI.Page

    Private selectedAdminUsernames As HashSet(Of String)
    Private selectionOfPrivilageMode As IDictionary(Of String, PrivilageMode)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        OnNonPostBack()
        OnPostBack()

        InitializeSelectPrivilageModeListBox()
        UpdateDisplayOfSelectedAdmins()
    End Sub

#Region "OnNonPostBack"
    Private Sub OnNonPostBack()
        If Not IsPostBack Then
            NoChangeRadioButton.Checked = True
        End If
    End Sub
#End Region

#Region "OnPostBack"
    Private Sub OnPostBack()
        If IsPostBack Then

        End If
    End Sub
#End Region

#Region "UpdateControls"

    Private Sub InitializeSelectPrivilageModeListBox()
        InitializeStringToPrivilageModeDictionary()

        For Each pm As String In selectionOfPrivilageMode.Keys
            PrivilageModeDropDownList.Items.Add(pm)
        Next
    End Sub

    Private Sub InitializeStringToPrivilageModeDictionary()
        selectionOfPrivilageMode = New Dictionary(Of String, PrivilageMode)
        selectionOfPrivilageMode.Add("(No Change)", Nothing)
        selectionOfPrivilageMode.Add("Normal Admin", PrivilageMode.NORMAL_ADMIN)
        selectionOfPrivilageMode.Add("Super Admin", PrivilageMode.SUPER_ADMIN)
    End Sub

    Private Sub UpdateDisplayOfSelectedAdmins()
        selectedAdminUsernames = Session.Item(AdminHomePage_ViewAdminTableContent.SELECTED_ADMINS_USERNAME_SESSION_CONSTANT)

        SelectedAccountsListBox.Items.Clear()
        For Each username As String In selectedAdminUsernames
            SelectedAccountsListBox.Items.Add(username)
        Next
    End Sub

#End Region

#Region "Confirm"
    Protected Sub ConfirmChangesButton_Click(sender As Object, e As EventArgs) Handles ConfirmChangesButton.Click
        If Not AreInputsValid() Then
            Return
        End If

        For Each adminUsername As String In selectedAdminUsernames
            EditAdminAccount(adminUsername)
        Next

        Response.Redirect(PageUrlConstants.ADMIN_VIEW_ADMIN_TABLE_PAGE_URL)
    End Sub

    Private Function AreInputsValid() As Boolean
        If SetManuallyRadioButton.Checked Then
            Dim inputPassword As String = ManualSetPasswordField.Text
            Dim inputConfirmPassword As String = ConfirmPasswordField.Text

            If Not inputPassword.Equals(inputConfirmPassword) Then
                ShowErrorMessage("Passwords do not match")
                Return False
            End If

            If inputPassword.Length = 0 Then
                ShowErrorMessage("Password field is required")
                Return False
            End If

            If Not PortalQueriesAndActions.NewPasswordConstraint.TryEvaluate(inputPassword) Then
                ShowErrorMessage("New password is not valid")
                Return False
            End If

            Return True
        Else

            Return True
        End If
    End Function

    Private Sub ShowErrorMessage(msg As String)
        ErrorLabel.Visible = True
        ErrorLabel.Text = msg
    End Sub

    Private Sub EditAdminAccount(username As String)
        Try
            Dim actionAsSelf As PortalQueriesAndActions = New PortalQueriesAndActions(Session.Item(SessionConstants.LOGGED_IN_USER))

            EditPrivilageMode(actionAsSelf, username)
            EditPassword(actionAsSelf, username)
        Catch ex As AccountDoesNotExistException

        End Try
    End Sub

    Private Sub EditPrivilageMode(actionAsSelf As PortalQueriesAndActions, targetUsername As String)
        Dim newPrivilageMode As PrivilageMode = selectionOfPrivilageMode.Item(PrivilageModeDropDownList.SelectedValue)

        If newPrivilageMode IsNot Nothing Then
            actionAsSelf.AdminAccountRelated.ChangePrivilageModeOfAdminAccount(targetUsername, newPrivilageMode)
        End If
    End Sub

    Private Sub EditPassword(actionAsSelf As PortalQueriesAndActions, targetUsername As String)
        Dim newPassword As String = Nothing

        If SetManuallyRadioButton.Checked Then
            newPassword = ManualSetPasswordField.Text
        ElseIf AutoGenerateRadioButton.Checked Then
            newPassword = PortalQueriesAndActions.RandomPasswordGenerator.GenerateRandomPassword()
        End If

        If newPassword IsNot Nothing Then
            actionAsSelf.AdminAccountRelated.ChangePasswordOfAdminAccount(targetUsername, newPassword)
        End If
    End Sub

#End Region

#Region "Cancel"
    Protected Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        Response.Redirect(PageUrlConstants.ADMIN_VIEW_ADMIN_TABLE_PAGE_URL)
    End Sub
#End Region
End Class