Public Class AdminHomePage_ViewAdminTableContent
    Inherits System.Web.UI.Page

    Private allAdminsList As List(Of AdminAccount)
    Private filteredAdminsList As List(Of AdminAccount)
    Private selectedAdminUsernameList As HashSet(Of String)
    Private filtersApplied As Dictionary(Of String, Predicate(Of AdminAccount))

    Friend Shared ReadOnly SELECTED_ADMINS_USERNAME_SESSION_CONSTANT As String = "OVoG_SelectedAdminsSessionConstant"
    Private Shared ReadOnly ALL_ADMINS_LIST_SESSION_CONSTANT As String = "OVoG_AllAdminsSessionConstant"
    Private Shared ReadOnly FILTERED_ADMINS_LIST_SESSION_CONSTANT As String = "OVoG_FilteredAdminsSessionConstant"
    Private Shared ReadOnly FILTERS_APPLIED_LIST_SESSION_CONSTANT As String = "OVoG_AdminFiltersAppliedSessionConstant"

    Private Shared ReadOnly FILTER_NAME As String = "FilterType_Name"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        OnPageLoad_NonPostback()
        OnPageLoad_PostBack()

        SetControlVisibilityBasedOnAdmin()
    End Sub

#Region "RefeshAdminTable"

    Private Sub DisplayAdminsToGridView()
        RecalculateFilteredAdmins()
        UpdateSelectedAdminsBasedOnCurrentPageOfGridView()

        Dim adminTable As DataTable = New DataTable()
        adminTable.Columns.Add("Username", "".GetType)
        adminTable.Columns.Add("PrivilageMode", "".GetType)
        adminTable.Columns.Add("Selected", True.GetType)

        For Each account As AdminAccount In filteredAdminsList
            Dim adminRow As DataRow = adminTable.NewRow()

            adminRow.Item("Username") = account.Username
            adminRow.Item("PrivilageMode") = account.PrivilageMode.ToString()

            Dim isSelected = selectedAdminUsernameList.Contains(account.Username)
            adminRow.Item("Selected") = isSelected

            adminTable.Rows.Add(adminRow)
        Next

        AdminGridView.DataSource = adminTable
        AdminGridView.DataBind()
    End Sub

    Protected Sub AdminGridView_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles AdminGridView.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim checkBox As CheckBox = TryCast(e.Row.Cells(2).Controls(0), CheckBox)
            checkBox.Enabled = True
        End If
    End Sub

#End Region

#Region "PageLoad_NonPostBack"

    Private Sub OnPageLoad_NonPostback()
        If Not IsPostBack Then
            RegisterAllAdminsList()
            RegisterSelectedAdminsInSession()
            RegisterFilteredAdminsInSession()
            RegisterFiltersAppliedList()

            GetSelectedAdminUsernamesFromSession()
            GetAllAdminsFromSession()
            GetFilteredAdminsFromSession()
            GetFiltersAppliedFromSession()

            DisplayAdminsToGridView()
        End If
    End Sub

    Private Sub RegisterAllAdminsList()
        If Session.Item(ALL_ADMINS_LIST_SESSION_CONSTANT) Is Nothing Then
            Session.Add(ALL_ADMINS_LIST_SESSION_CONSTANT, PortalQueriesAndActions.AdminQueriesAndActions.GetAllAdminAccounts())
        Else
            Session.Item(ALL_ADMINS_LIST_SESSION_CONSTANT) = PortalQueriesAndActions.AdminQueriesAndActions.GetAllAdminAccounts()
        End If
    End Sub

    Private Sub RegisterSelectedAdminsInSession()
        If Session.Item(SELECTED_ADMINS_USERNAME_SESSION_CONSTANT) Is Nothing Then
            Session.Add(SELECTED_ADMINS_USERNAME_SESSION_CONSTANT, New HashSet(Of String))
        Else
            Session.Item(SELECTED_ADMINS_USERNAME_SESSION_CONSTANT) = New HashSet(Of String)
        End If
    End Sub

    Private Sub RegisterFilteredAdminsInSession()
        If Session.Item(FILTERED_ADMINS_LIST_SESSION_CONSTANT) Is Nothing Then
            Session.Add(FILTERED_ADMINS_LIST_SESSION_CONSTANT, PortalQueriesAndActions.AdminQueriesAndActions.GetAllAdminAccounts())
        Else
            Session.Item(FILTERED_ADMINS_LIST_SESSION_CONSTANT) = PortalQueriesAndActions.AdminQueriesAndActions.GetAllAdminAccounts()
        End If
    End Sub

    Private Sub RegisterFiltersAppliedList()
        If Session.Item(FILTERS_APPLIED_LIST_SESSION_CONSTANT) Is Nothing Then
            Session.Add(FILTERS_APPLIED_LIST_SESSION_CONSTANT, New Dictionary(Of String, Predicate(Of AdminAccount)))
        Else
            Session.Item(FILTERS_APPLIED_LIST_SESSION_CONSTANT) = New Dictionary(Of String, Predicate(Of AdminAccount))
        End If
    End Sub

    '
    Private Sub GetSelectedAdminUsernamesFromSession()
        selectedAdminUsernameList = Session.Item(SELECTED_ADMINS_USERNAME_SESSION_CONSTANT)
    End Sub

    Private Sub GetAllAdminsFromSession()
        allAdminsList = Session.Item(ALL_ADMINS_LIST_SESSION_CONSTANT)
    End Sub

    Private Sub GetFilteredAdminsFromSession()
        filteredAdminsList = Session.Item(FILTERED_ADMINS_LIST_SESSION_CONSTANT)
    End Sub

    Private Sub GetFiltersAppliedFromSession()
        filtersApplied = Session.Item(FILTERS_APPLIED_LIST_SESSION_CONSTANT)
    End Sub

#End Region

#Region "PageLoad_PostBack"

    Private Sub OnPageLoad_PostBack()
        If IsPostBack Then
            GetSelectedAdminUsernamesFromSession()
            GetAllAdminsFromSession()
            GetFilteredAdminsFromSession()
            GetFiltersAppliedFromSession()
        End If
    End Sub

#End Region

#Region "Control Visibility (Based on privilage mode)"

    Private Sub SetControlVisibilityBasedOnAdmin()

        Dim privMode As PrivilageMode = PortalQueriesAndActions.AdminQueriesAndActions.GetPrivilageModeOfAdminAccount(Session.Item(SessionConstants.LOGGED_IN_USER))
        If privMode Is PrivilageMode.DEFAULT_ADMIN Or privMode Is PrivilageMode.SUPER_ADMIN Then
            CreateAdminButton.Visible = True
            EditPropertiesButton.Visible = True
            Label2.Visible = True
        Else
            CreateAdminButton.Visible = False
            EditPropertiesButton.Visible = False
            Label2.Visible = False
        End If

    End Sub

#End Region

#Region "UpdateSelectedAdmins"

    Private Sub UpdateSelectedAdminsBasedOnCurrentPageOfGridView()

        For Each row As GridViewRow In AdminGridView.Rows
            Dim isSelected As Boolean = TryCast(row.Cells(2).Controls(0), CheckBox).Checked
            Dim username As String = row.Cells(0).Text

            If isSelected Then
                selectedAdminUsernameList.Add(username)
            Else
                selectedAdminUsernameList.Remove(username)
            End If
        Next

    End Sub

#End Region

#Region "ApplyFilters_SearchForName"

    'Postback opening
    Protected Sub SearchNameButton_Click(sender As Object, e As EventArgs) Handles SearchNameButton.Click
        Dim searchInput As String = SearchField.Text
        Dim predicate As Predicate(Of AdminAccount) = Function(toTest As AdminAccount) As Boolean
                                                          Return toTest.Username.Contains(searchInput)
                                                      End Function

        If Not searchInput.Length = 0 AndAlso filtersApplied.ContainsKey(FILTER_NAME) Then
            filtersApplied.Item(FILTER_NAME) = predicate

        ElseIf Not searchInput.Length = 0 AndAlso Not filtersApplied.ContainsKey(FILTER_NAME) Then
            filtersApplied.Add(FILTER_NAME, predicate)

        Else
            filtersApplied.Remove(FILTER_NAME)

        End If

        Session.Item(FILTERS_APPLIED_LIST_SESSION_CONSTANT) = filtersApplied
        DisplayAdminsToGridView()
    End Sub

    Private Sub RecalculateFilteredAdmins()
        Dim tempBucket_filteredAdmins As IList(Of AdminAccount) = New List(Of AdminAccount)

        For Each adminAccount As AdminAccount In allAdminsList
            If IfAdminPassesAllPredicates(adminAccount) Then
                tempBucket_filteredAdmins.Add(adminAccount)
            End If
        Next

        filteredAdminsList = tempBucket_filteredAdmins
        Session.Item(FILTERED_ADMINS_LIST_SESSION_CONSTANT) = tempBucket_filteredAdmins
    End Sub

    Private Function IfAdminPassesAllPredicates(account As AdminAccount) As Boolean
        For Each filter As Predicate(Of AdminAccount) In filtersApplied.Values
            If Not filter.Invoke(account) Then
                Return False
            End If
        Next

        Return True
    End Function

#End Region

#Region "AdminTablePageIndexChanged"

    'Postback opening
    Protected Sub AdminGridView_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles AdminGridView.PageIndexChanging
        AdminGridView.PageIndex = e.NewPageIndex

        DisplayAdminsToGridView()
    End Sub

    Protected Sub AdminGridView_PageIndexChanged(sender As Object, e As EventArgs) Handles AdminGridView.PageIndexChanged

    End Sub

#End Region

#Region "SelectionButtons"

    Protected Sub UnselectAllButton_Click(sender As Object, e As EventArgs) Handles UnselectAllButton.Click
        For Each row As GridViewRow In AdminGridView.Rows
            Dim checkbox As CheckBox = TryCast(row.Cells(2).Controls(0), CheckBox)
            checkbox.Checked = False
        Next

        For Each account As AdminAccount In filteredAdminsList
            selectedAdminUsernameList.Remove(account.Username)
        Next
        Session.Item(SELECTED_ADMINS_USERNAME_SESSION_CONSTANT) = selectedAdminUsernameList

        DisplayAdminsToGridView()
    End Sub

    Protected Sub SelectAllButton_Click(sender As Object, e As EventArgs) Handles SelectAllButton.Click
        For Each row As GridViewRow In AdminGridView.Rows
            Dim checkbox As CheckBox = TryCast(row.Cells(2).Controls(0), CheckBox)
            checkbox.Checked = True
        Next

        For Each account As AdminAccount In filteredAdminsList
            selectedAdminUsernameList.Add(account.Username)
        Next

        Session.Item(SELECTED_ADMINS_USERNAME_SESSION_CONSTANT) = selectedAdminUsernameList

        DisplayAdminsToGridView()
    End Sub

#End Region

#Region "RefreshData"

    Private Sub RefreshAllAdminsList()

        allAdminsList = PortalQueriesAndActions.AdminQueriesAndActions.GetAllAdminAccounts()
        Session.Item(ALL_ADMINS_LIST_SESSION_CONSTANT) = allAdminsList

    End Sub

#End Region

    '#Region "DeleteSelected"

    'Protected Sub DeleteSelectedButton_Click(sender As Object, e As EventArgs) Handles DeleteSelectedButton.Click

    '    UpdateSelectedAdminsBasedOnCurrentPageOfGridView()

    '    Dim selfUsername As String = Session.Item(SessionConstants.LOGGED_IN_USER)
    '    Dim actionsAsSelf As PortalQueriesAndActions = New PortalQueriesAndActions(selfUsername)

    '    For Each username As String In selectedAdminUsernameList
    '        Try
    '            actionsAsSelf.AdminAccountRelated.DeleteAdminAccount(username)
    '        Catch ex As AccountDoesNotExistException

    '        End Try
    '    Next

    '    selectedAdminUsernameList.Clear()
    '    Session.Item(SELECTED_ADMINS_USERNAME_SESSION_CONSTANT) = selectedAdminUsernameList

    '    RefreshAllAdminsList()
    '    DisplayAdminsToGridView()
    'End Sub

    '#End Region

#Region "EditSelected"

    Protected Sub EditPropertiesButton_Click(sender As Object, e As EventArgs) Handles EditPropertiesButton.Click
        'For update purposes
        DisplayAdminsToGridView()
        Response.Redirect(PageUrlConstants.ADMIN_EDIT_ADMIN_PROFILE_PAGE_URL)
    End Sub

#End Region

#Region "CreateAdmin"

    Protected Sub CreateAdminButton_Click(sender As Object, e As EventArgs) Handles CreateAdminButton.Click
        Response.RedirectPermanent(PageUrlConstants.ADMIN_ADD_ADMIN_ACCOUNT_PAGE_URL)
    End Sub

#End Region

End Class