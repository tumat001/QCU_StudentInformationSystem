Public Class AdminHomePage_ViewStudentTableContent
    Inherits System.Web.UI.Page

    Private allStudentsList As List(Of StudentAccount)
    Private filteredStudentsList As List(Of StudentAccount)
    Private selectedStudentUsernameList As HashSet(Of String)
    Private filtersApplied As Dictionary(Of String, Predicate(Of StudentAccount))

    Friend Shared ReadOnly SELECTED_STUDENT_USERNAME_SESSION_CONSTANT As String = "OVoG_SelectedStudentSessionConstant"
    Private Shared ReadOnly ALL_STUDENTS_LIST_SESSION_CONSTANT As String = "OVoG_AllStudentsSessionConstant"
    Private Shared ReadOnly FILTERED_STUDENTS_LIST_SESSION_CONSTANT As String = "OVoG_FilteredStudentsSessionConstant"
    Private Shared ReadOnly FILTERS_APPLIED_LIST_SESSION_CONSTANT As String = "OVoG_StudentFiltersAppliedSessionConstant"

    Private Shared ReadOnly FILTER_NAME As String = "FilterType_Name"

    Private ReadOnly studentSource As IStudentSource = New MockStudentSource()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        OnPageLoad_NonPostback()
        OnPageLoad_PostBack()

        SetControlVisibilityBasedOnAdmin()
    End Sub

#Region "RefeshStudentTable"

    Private Sub DisplayStudentsToGridView()
        RecalculateFilteredStudents()
        UpdateSelectedStudentsBasedOnCurrentPageOfGridView()

        Dim studentTable As DataTable = New DataTable()
        studentTable.Columns.Add("Username", "".GetType)
        studentTable.Columns.Add("EmailAddress", "".GetType)
        studentTable.Columns.Add("Selected", True.GetType)

        For Each account As StudentAccount In filteredStudentsList
            Dim studentRow As DataRow = studentTable.NewRow()

            studentRow.Item("Username") = account.Username
            studentRow.Item("EmailAddress") = account.EmailAddress

            Dim isSelected = selectedStudentUsernameList.Contains(account.Username)
            studentRow.Item("Selected") = isSelected

            studentTable.Rows.Add(studentRow)
        Next

        StudentGridView.DataSource = studentTable
        StudentGridView.DataBind()
    End Sub

    Protected Sub StudentGridView_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles StudentGridView.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim checkBox As CheckBox = TryCast(e.Row.Cells(2).Controls(0), CheckBox)
            checkBox.Enabled = True
        End If
    End Sub

#End Region

#Region "PageLoad_NonPostBack"

    Private Sub OnPageLoad_NonPostback()
        If Not IsPostBack Then
            FirstTimeDisplay()
        End If
    End Sub

    Private Sub FirstTimeDisplay()
        RegisterAllStudentsList()
        RegisterSelectedStudentsInSession()
        RegisterFilteredStudentsInSession()
        RegisterFiltersAppliedList()

        GetSelectedStudentUsernamesFromSession()
        GetAllStudentFromSession()
        GetFilteredStudentsFromSession()
        GetFiltersAppliedFromSession()

        DisplayStudentsToGridView()
    End Sub

    Private Sub RegisterAllStudentsList()
        If Session.Item(ALL_STUDENTS_LIST_SESSION_CONSTANT) Is Nothing Then
            Session.Add(ALL_STUDENTS_LIST_SESSION_CONSTANT, PortalQueriesAndActions.StudentQueriesAndActions.GetAllStudentAccounts())
        Else
            Session.Item(ALL_STUDENTS_LIST_SESSION_CONSTANT) = PortalQueriesAndActions.StudentQueriesAndActions.GetAllStudentAccounts()
        End If
    End Sub

    Private Sub RegisterSelectedStudentsInSession()
        If Session.Item(SELECTED_STUDENT_USERNAME_SESSION_CONSTANT) Is Nothing Then
            Session.Add(SELECTED_STUDENT_USERNAME_SESSION_CONSTANT, New HashSet(Of String))
        Else
            Session.Item(SELECTED_STUDENT_USERNAME_SESSION_CONSTANT) = New HashSet(Of String)
        End If
    End Sub

    Private Sub RegisterFilteredStudentsInSession()
        If Session.Item(FILTERED_STUDENTS_LIST_SESSION_CONSTANT) Is Nothing Then
            Session.Add(FILTERED_STUDENTS_LIST_SESSION_CONSTANT, PortalQueriesAndActions.StudentQueriesAndActions.GetAllStudentAccounts())
        Else
            Session.Item(FILTERED_STUDENTS_LIST_SESSION_CONSTANT) = PortalQueriesAndActions.StudentQueriesAndActions.GetAllStudentAccounts()
        End If
    End Sub

    Private Sub RegisterFiltersAppliedList()
        If Session.Item(FILTERS_APPLIED_LIST_SESSION_CONSTANT) Is Nothing Then
            Session.Add(FILTERS_APPLIED_LIST_SESSION_CONSTANT, New Dictionary(Of String, Predicate(Of StudentAccount)))
        Else
            Session.Item(FILTERS_APPLIED_LIST_SESSION_CONSTANT) = New Dictionary(Of String, Predicate(Of StudentAccount))
        End If
    End Sub

    '
    Private Sub GetSelectedStudentUsernamesFromSession()
        selectedStudentUsernameList = Session.Item(SELECTED_STUDENT_USERNAME_SESSION_CONSTANT)
    End Sub

    Private Sub GetAllStudentFromSession()
        allStudentsList = Session.Item(ALL_STUDENTS_LIST_SESSION_CONSTANT)
    End Sub

    Private Sub GetFilteredStudentsFromSession()
        filteredStudentsList = Session.Item(FILTERED_STUDENTS_LIST_SESSION_CONSTANT)
    End Sub

    Private Sub GetFiltersAppliedFromSession()
        filtersApplied = Session.Item(FILTERS_APPLIED_LIST_SESSION_CONSTANT)
    End Sub

#End Region

#Region "PageLoad_PostBack"

    Private Sub OnPageLoad_PostBack()
        If IsPostBack Then
            GetSelectedStudentUsernamesFromSession()
            GetAllStudentFromSession()
            GetFilteredStudentsFromSession()
            GetFiltersAppliedFromSession()
        End If
    End Sub

#End Region

#Region "Control Visibility (Based on privilage mode)"

    Private Sub SetControlVisibilityBasedOnAdmin()

        Dim privMode As PrivilageMode = PortalQueriesAndActions.AdminQueriesAndActions.GetPrivilageModeOfAdminAccount(Session.Item(SessionConstants.LOGGED_IN_USER))
        If privMode Is PrivilageMode.DEFAULT_ADMIN Or privMode Is PrivilageMode.SUPER_ADMIN Or privMode Is PrivilageMode.NORMAL_ADMIN Then
            CreateStudentButton.Visible = True
            SyncAccountsButton.Visible = True
            EditPropertiesButton.Visible = True
            LabelIns02.Visible = True
        Else
            CreateStudentButton.Visible = False
            SyncAccountsButton.Visible = False
            EditPropertiesButton.Visible = False
            LabelIns02.Visible = False
        End If

    End Sub

#End Region

#Region "UpdateSelectedStudents"

    Private Sub UpdateSelectedStudentsBasedOnCurrentPageOfGridView()

        For Each row As GridViewRow In StudentGridView.Rows
            Dim isSelected As Boolean = TryCast(row.Cells(2).Controls(0), CheckBox).Checked
            Dim username As String = row.Cells(0).Text

            If isSelected Then
                selectedStudentUsernameList.Add(username)
            Else
                selectedStudentUsernameList.Remove(username)
            End If
        Next

    End Sub

#End Region

#Region "ApplyFilters_SearchForName"

    'Postback opening
    Protected Sub SearchNameButton_Click(sender As Object, e As EventArgs) Handles SearchNameButton.Click
        Dim searchInput As String = SearchField.Text
        Dim predicate As Predicate(Of StudentAccount) = Function(toTest As StudentAccount) As Boolean
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
        DisplayStudentsToGridView()
    End Sub

    Private Sub RecalculateFilteredStudents()
        Dim tempBucket_filteredStudents As IList(Of StudentAccount) = New List(Of StudentAccount)

        For Each studentAccount As StudentAccount In allStudentsList
            If IfStudentPassesAllPredicates(studentAccount) Then
                tempBucket_filteredStudents.Add(studentAccount)
            End If
        Next

        filteredStudentsList = tempBucket_filteredStudents
        Session.Item(FILTERED_STUDENTS_LIST_SESSION_CONSTANT) = tempBucket_filteredStudents
    End Sub

    Private Function IfStudentPassesAllPredicates(account As StudentAccount) As Boolean
        For Each filter As Predicate(Of StudentAccount) In filtersApplied.Values
            If Not filter.Invoke(account) Then
                Return False
            End If
        Next

        Return True
    End Function

#End Region

#Region "StudentTablePageIndexChanged"

    'Postback opening
    Protected Sub StudentGridView_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles StudentGridView.PageIndexChanging
        StudentGridView.PageIndex = e.NewPageIndex

        DisplayStudentsToGridView()
    End Sub

    Protected Sub StudentGridView_PageIndexChanged(sender As Object, e As EventArgs) Handles StudentGridView.PageIndexChanged

    End Sub

#End Region

#Region "SelectionButtons"

    Protected Sub UnselectAllButton_Click(sender As Object, e As EventArgs) Handles UnselectAllButton.Click
        For Each row As GridViewRow In StudentGridView.Rows
            Dim checkbox As CheckBox = TryCast(row.Cells(2).Controls(0), CheckBox)
            checkbox.Checked = False
        Next

        For Each account As StudentAccount In filteredStudentsList
            selectedStudentUsernameList.Remove(account.Username)
        Next
        Session.Item(SELECTED_STUDENT_USERNAME_SESSION_CONSTANT) = selectedStudentUsernameList

        DisplayStudentsToGridView()
    End Sub

    Protected Sub SelectAllButton_Click(sender As Object, e As EventArgs) Handles SelectAllButton.Click
        For Each row As GridViewRow In StudentGridView.Rows
            Dim checkbox As CheckBox = TryCast(row.Cells(2).Controls(0), CheckBox)
            checkbox.Checked = True
        Next

        For Each account As StudentAccount In filteredStudentsList
            selectedStudentUsernameList.Add(account.Username)
        Next

        Session.Item(SELECTED_STUDENT_USERNAME_SESSION_CONSTANT) = selectedStudentUsernameList

        DisplayStudentsToGridView()
    End Sub

#End Region

#Region "RefreshData"

    Private Sub RefreshAllStudentLists()

        allStudentsList = PortalQueriesAndActions.StudentQueriesAndActions.GetAllStudentAccounts
        Session.Item(ALL_STUDENTS_LIST_SESSION_CONSTANT) = allStudentsList

    End Sub

#End Region

    '#Region "DeleteSelected"

    '    Protected Sub DeleteSelectedButton_Click(sender As Object, e As EventArgs) Handles DeleteSelectedButton.Click

    '        UpdateSelectedStudentsBasedOnCurrentPageOfGridView()

    '        Dim selfUsername As String = Session.Item(SessionConstants.LOGGED_IN_USER)
    '        Dim actionsAsSelf As PortalQueriesAndActions = New PortalQueriesAndActions(selfUsername)

    '        For Each username As String In selectedStudentUsernameList
    '            Try
    '                actionsAsSelf.StudentAccountRelated.DeleteStudentAccount(username)
    '            Catch ex As AccountDoesNotExistException

    '            End Try
    '        Next

    '        selectedStudentUsernameList.Clear()
    '        Session.Item(SELECTED_STUDENT_USERNAME_SESSION_CONSTANT) = selectedStudentUsernameList

    '        RefreshAllStudentLists()
    '        DisplayStudentsToGridView()
    '    End Sub

    '#End Region

#Region "EditProperties"

    Protected Sub EditPropertiesButton_Click(sender As Object, e As EventArgs) Handles EditPropertiesButton.Click
        DisplayStudentsToGridView()
        Response.Redirect(PageUrlConstants.ADMIN_EDIT_STUDENT_PROFILE_PAGE_URL)
    End Sub

#End Region

#Region "CreateStudentRedirect"

    Protected Sub CreateStudentButton_Click(sender As Object, e As EventArgs) Handles CreateStudentButton.Click
        Response.RedirectPermanent(PageUrlConstants.ADMIN_ADD_STUDENT_ACCOUNT_PAGE_URL)
    End Sub

#End Region


#Region "SyncRelated"

    Protected Sub SyncAccountsButton_Click(sender As Object, e As EventArgs) Handles SyncAccountsButton.Click
        Dim actionAsAdmin As PortalQueriesAndActions = New PortalQueriesAndActions(Session.Item(LOGGED_IN_USER))

        Try
            actionAsAdmin.StudentAccountRelated.SyncStudentAccountRelated.CreateAccountsForAccountlessStudents(studentSource)
            FirstTimeDisplay()
        Catch ex As Exception

        End Try
    End Sub

#End Region

End Class