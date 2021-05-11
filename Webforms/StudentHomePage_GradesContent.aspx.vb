Imports StringBuilder = System.Text.StringBuilder

Public Class StudentHomePage_GradesContent
    Inherits System.Web.UI.Page

    Private gradeSource As IGradeSource
    Private studentSource As IStudentSource
    Private gradesOfStudent As SemesterGrades

    Private schoolYear As String
    Private semesterTerm As Integer

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        OnPostBack()
        OnNonPostBack()

        InitializeSchoolYearAndSemesterTerm()
        InitializeGradeSource()
        GetGradesFromStudent()
        DisplayGrades()

        InitializeStudentSource()
        DisplayStudentInformation()
    End Sub

#Region "OnPostBack"

    Private Sub OnPostBack()
        If IsPostBack() Then

        End If
    End Sub

#End Region

#Region "OnNonPostBack"

    Private Sub OnNonPostBack()
        If Not IsPostBack Then

        End If
    End Sub

#End Region

#Region "DisplayGradesRelated"

    Private Sub InitializeSchoolYearAndSemesterTerm()
        schoolYear = "2020-2021"
        semesterTerm = 1
    End Sub

    Private Sub InitializeGradeSource()
        'TODO Make this non-mock once readyy uwu ~~
        gradeSource = New MockGradeSource()
    End Sub

    Private Sub GetGradesFromStudent()
        Dim studentNumber As String = Session.Item(SessionConstants.LOGGED_IN_USER)

        gradesOfStudent = gradeSource.GetLatestSchoolYearGradesOfStudent(studentNumber, semesterTerm)
    End Sub

    Private Sub DisplayGrades()
        Dim gradesTable As DataTable = New DataTable()
        gradesTable.Columns.Add("SubCode", "".GetType)
        gradesTable.Columns.Add("Description", "".GetType)
        gradesTable.Columns.Add("Units", "".GetType)
        gradesTable.Columns.Add("Grade", "".GetType)
        gradesTable.Columns.Add("Remarks", "".GetType)
        gradesTable.Columns.Add("Professor", "".GetType)
        gradesTable.Columns.Add("Class", "".GetType)

        For Each subjectGrade As SubjectGrade In gradesOfStudent.Grades
            Dim subjectGradeRow As DataRow = gradesTable.NewRow()

            subjectGradeRow.Item("SubCode") = subjectGrade.Code
            subjectGradeRow.Item("Description") = subjectGrade.Description
            subjectGradeRow.Item("Units") = subjectGrade.Units.ToString
            subjectGradeRow.Item("Grade") = subjectGrade.Grade
            subjectGradeRow.Item("Remarks") = subjectGrade.Remarks
            subjectGradeRow.Item("Professor") = subjectGrade.Professor
            subjectGradeRow.Item("Class") = subjectGrade.Class

            gradesTable.Rows.Add(subjectGradeRow)
        Next

        StudentGradeInfoGridView.DataSource = gradesTable
        StudentGradeInfoGridView.DataBind()
    End Sub

#End Region

#Region "DisplayStudentInformation"

    Private Sub InitializeStudentSource()
        'Replace this with non-mock once ready uwu ~~
        studentSource = New MockStudentSource()
    End Sub

    Private Sub DisplayStudentInformation()

        Dim studentNumber As String = Session.Item(SessionConstants.LOGGED_IN_USER)
        Dim student As Student = studentSource.GetStudent(studentNumber)

        StudentNameLabel.Text = GetNameOfStudent(student)
        CampusLabel.Text = student.Campus
        SchoolYearLabel.Text = schoolYear
        TermLabel.Text = semesterTerm.ToString()
    End Sub

    Private Function GetNameOfStudent(student As Student) As String
        Dim builder As StringBuilder = New StringBuilder()

        If student.LastName IsNot Nothing AndAlso Not student.LastName.Equals("Null") Then
            builder.Append(student.LastName).Append(", ")
        End If
        If student.FirstName IsNot Nothing AndAlso Not student.FirstName.Equals("Null") Then
            builder.Append(student.FirstName).Append(" ")
        End If
        If student.AuxName IsNot Nothing AndAlso Not student.AuxName.Equals("Null") Then
            builder.Append(student.AuxName).Append(" ")
        End If
        If student.MiddleName IsNot Nothing AndAlso Not student.MiddleName.Equals("Null") Then
            builder.Append(student.MiddleName).Append(" ")
        End If

        Return builder.ToString()
    End Function

#End Region

End Class