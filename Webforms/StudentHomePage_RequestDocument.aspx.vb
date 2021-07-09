Public Class StudentHomePage_RequestGradeSlip
    Inherits System.Web.UI.Page

    Private studentSource As IStudentSource = New MockStudentSource()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetEmailField()

        If Not IsPostBack Then
            InitializeDocumentsDropDownList()
        End If
    End Sub

    Private Sub SetEmailField()
        Dim student As Student = studentSource.GetStudent(Session.Item(SessionConstants.LOGGED_IN_USER))

        EmailTextbox.Text = student.EmailAddress
    End Sub

    Private Sub InitializeDocumentsDropDownList()
        'todo replace this
        Dim docuList As IReadOnlyList(Of String) = PortalQueriesAndActions.AdminQueriesAndActions.GetAllRequestDocumentOptionsList()
        For Each item As String In docuList
            DocumentDropDownList.Items.Add(New ListItem(item))
        Next
    End Sub


    Protected Sub SendRequestButton_Click(sender As Object, e As EventArgs) Handles SendRequestButton.Click
        DisplaySuccessfulOperations()
    End Sub


    Private Sub DisplaySuccessfulOperations()
        LabelResultStatus.Visible = True
        LabelResultStatus.ForeColor = System.Drawing.Color.Green
        LabelResultStatus.Text = "Request Sent!"
    End Sub

    Private Sub DisplayErrorInOperations(errorMsg As String)
        LabelResultStatus.Visible = True
        LabelResultStatus.ForeColor = System.Drawing.Color.Red
        LabelResultStatus.Text = errorMsg
    End Sub

End Class