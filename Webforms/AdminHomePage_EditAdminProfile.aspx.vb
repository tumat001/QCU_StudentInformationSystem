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

        Dim errorForAdmins As IList(Of String) = New List(Of String)

        For Each adminUsername As String In selectedAdminUsernames
            Dim success As Boolean = EditAdminAccount(adminUsername)
            If success = False Then
                errorForAdmins.Add(adminUsername)
            End If
        Next

        If errorForAdmins.Count = 0 Then
            SuccessLabel.Visible = True
            ErrorLabel.Visible = False

        Else
            SuccessLabel.Visible = False

            Dim builder As StringBuilder = New StringBuilder()
            builder.Append("Error in editing users: ")
            For Each username As String In errorForAdmins
                builder.Append(username & ", ")
            Next

            ShowErrorMessage(builder.ToString())
        End If
        'Response.Redirect(PageUrlConstants.ADMIN_VIEW_ADMIN_TABLE_PAGE_URL)
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

        SuccessLabel.Visible = False
    End Sub

    Private Function EditAdminAccount(username As String) As Boolean
        Dim success As Boolean
        Try
            Dim actionAsSelf As PortalQueriesAndActions = New PortalQueriesAndActions(Session.Item(SessionConstants.LOGGED_IN_USER))

            success = EditPrivilageMode(actionAsSelf, username) And EditPassword(actionAsSelf, username)

        Catch ex As Exception 'AccountDoesNotExistException
            success = False
        End Try

        Return success
    End Function

    Private Function EditPrivilageMode(actionAsSelf As PortalQueriesAndActions, targetUsername As String) As Boolean
        Dim newPrivilageMode As PrivilageMode = selectionOfPrivilageMode.Item(PrivilageModeDropDownList.SelectedValue)

        Dim success As Boolean
        Try
            If newPrivilageMode IsNot Nothing Then
                success = actionAsSelf.AdminAccountRelated.ChangePrivilageModeOfAdminAccount(targetUsername, newPrivilageMode)
            Else
                success = True 'Do not edit selected
            End If
        Catch ex As Exception
            success = False
        End Try

        Return success
    End Function

    Private Function EditPassword(actionAsSelf As PortalQueriesAndActions, targetUsername As String) As Boolean
        Dim newPassword As String = Nothing

        If SetManuallyRadioButton.Checked Then
            newPassword = ManualSetPasswordField.Text
        ElseIf AutoGenerateRadioButton.Checked Then
            newPassword = PortalQueriesAndActions.RandomPasswordGenerator.GenerateRandomPassword()
        End If

        Dim success As Boolean
        Try
            If newPassword IsNot Nothing Then
                success = actionAsSelf.AdminAccountRelated.ChangePasswordOfAdminAccount(targetUsername, newPassword)
            Else
                success = True 'Do not edit selected
            End If
        Catch ex As Exception
            success = False
        End Try

        Return success
    End Function

#End Region

#Region "Cancel"
    Protected Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        Response.Redirect(PageUrlConstants.ADMIN_VIEW_ADMIN_TABLE_PAGE_URL)
    End Sub
#End Region
End Class