Imports StringBuilder = System.Text.StringBuilder

Public Class StudentHomePage_GradesContent
    Inherits System.Web.UI.Page

    Private gradeSource As IGradeSource
    Private studentSource As IStudentSource
    Private gradesOfStudent As SemesterGrades

    Private schoolYear As String
    Private semesterTerm As Integer
    Private allSemYearList As IReadOnlyList(Of String)

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        InitializeGradeSource()
        InitializeStudentSource()

        InitializeSchoolYearAndSemesterTerm()
        GetGradesFromStudent()

        OnPostBack()
        OnNonPostBack()

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
            DisplaySemYearSelection()
            DisplayGrades()

            DisplayStudentInformation()
        End If
    End Sub

#End Region

#Region "DisplayGradesAndSemYearSelection Related"

    Private Sub InitializeGradeSource()
        'TODO Make this non-mock once readyy uwu ~~
        gradeSource = New MockGradeSource()
    End Sub

    Private Sub InitializeSchoolYearAndSemesterTerm()
        allSemYearList = gradeSource.GetAllSemYearCourseWithStudent(Session.Item(SessionConstants.LOGGED_IN_USER))

        If Session.Item(SessionConstants.SEM_OF_GRADE_DISPLAY) = Nothing Then
            If Not allSemYearList.Count = 0 Then
                Dim semYear As String() = gradeSource.ConvertSemYearCourseStringToSemAndYearArray(allSemYearList.Item(0))

                Session.Item(SessionConstants.SEM_OF_GRADE_DISPLAY) = semYear(0)
                Session.Item(SessionConstants.YEAR_OF_GRADE_DISPLAY) = semYear(1)

                semesterTerm = semYear(0)
                schoolYear = semYear(1)

            End If
        Else

            semesterTerm = Session.Item(SessionConstants.SEM_OF_GRADE_DISPLAY)
            schoolYear = Session.Item(SessionConstants.YEAR_OF_GRADE_DISPLAY)

        End If
    End Sub

    Private Sub GetGradesFromStudent()
        Dim studentNumber As String = Session.Item(SessionConstants.LOGGED_IN_USER)

        gradesOfStudent = gradeSource.GetGradesOfStudentInSemYear(studentNumber, semesterTerm, schoolYear)
    End Sub

    Private Sub DisplayGrades()
        If Not gradesOfStudent Is Nothing Then
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
        End If
    End Sub

    Private Sub DisplaySemYearSelection()
        SemYearDropDownList.Items.Clear()

        For Each semYear As String In allSemYearList
            SemYearDropDownList.Items.Add(New ListItem(semYear, semYear))
        Next

        If Not Session.Item(SessionConstants.SEM_YEAR_DROPDOWN_SELECTED_INDEX) = Nothing Then
            SemYearDropDownList.SelectedIndex = Session.Item(SessionConstants.SEM_YEAR_DROPDOWN_SELECTED_INDEX)
        End If
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

        If Not student Is Nothing Then
            StudentNameLabel.Text = GetNameOfStudent(student)
            CampusLabel.Text = student.Campus
            SchoolYearLabel.Text = schoolYear.ToString()
            TermLabel.Text = semesterTerm.ToString()
        End If
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


#Region "SemYear Related"

    Protected Sub YearDropDownList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles SemYearDropDownList.SelectedIndexChanged
        Dim selectedSemYear As String() = gradeSource.ConvertSemYearCourseStringToSemAndYearArray(SemYearDropDownList.SelectedValue)

        Session.Item(SessionConstants.SEM_YEAR_DROPDOWN_SELECTED_INDEX) = SemYearDropDownList.SelectedIndex
        Session.Item(SessionConstants.SEM_OF_GRADE_DISPLAY) = selectedSemYear(0)
        Session.Item(SessionConstants.YEAR_OF_GRADE_DISPLAY) = selectedSemYear(1)

        semesterTerm = Session.Item(SessionConstants.SEM_OF_GRADE_DISPLAY)
        schoolYear = Session.Item(SessionConstants.YEAR_OF_GRADE_DISPLAY)

        GetGradesFromStudent()

        DisplayGrades()
        DisplayStudentInformation()
        'DisplaySemYearSelection()


    End Sub

#End Region

    Protected Sub RequestGradeSlipButton_Click(sender As Object, e As EventArgs) Handles RequestGradeSlipButton.Click
        Response.RedirectPermanent(PageUrlConstants.STUDENT_REQUEST_DOCUMENT_PAGE_URL)
    End Sub

End Class