Imports StringBuilder = System.Text.StringBuilder

Public Class StudentHomePage_CoursesEnrolled
    Inherits System.Web.UI.Page

    Private gradeSource As IGradeSource
    Private studentSource As IStudentSource

    Private schoolYear As String
    Private semesterTerm As Integer
    Private allSemYearCourseList As IReadOnlyList(Of String)

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        OnPostBack()
        OnNonPostBack()

        InitializeGradeSource()
        InitializeSchoolYearCourseAndSemesterTerm()
        DisplayCourses()

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

#Region "DisplayCoursesSelection Related"

    Private Sub InitializeGradeSource()
        'TODO Make this non-mock once readyy uwu ~~
        gradeSource = New MockGradeSource()
    End Sub

    Private Sub InitializeSchoolYearCourseAndSemesterTerm()
        allSemYearCourseList = gradeSource.GetAllSemYearCourseWithStudent(Session.Item(SessionConstants.LOGGED_IN_USER))

        If Session.Item(SessionConstants.SEM_OF_GRADE_DISPLAY) = Nothing Then
            If Not allSemYearCourseList.Count = 0 Then
                Dim semYear As String() = gradeSource.ConvertSemYearCourseStringToSemAndYearArray(allSemYearCourseList.Item(0))

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

    Private Sub DisplayCourses()
        If Not allSemYearCourseList.Count = 0 Then
            Dim semYearCourseTable As DataTable = New DataTable()
            semYearCourseTable.Columns.Add("Course", "".GetType)
            semYearCourseTable.Columns.Add("School Year", "".GetType)

            For Each semYearCourseString As String In allSemYearCourseList
                Dim stringArrSemYearCourse As String() = gradeSource.ConvertSemYearCourseStringToSemAndYearArray(semYearCourseString)

                Dim subjectGradeRow As DataRow = semYearCourseTable.NewRow()

                subjectGradeRow.Item("Course") = stringArrSemYearCourse(2)
                subjectGradeRow.Item("School Year") = stringArrSemYearCourse(0) + "-" + stringArrSemYearCourse(1)


                semYearCourseTable.Rows.Add(subjectGradeRow)
            Next

            StudentCoursesGridView.DataSource = semYearCourseTable
            StudentCoursesGridView.DataBind()
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


End Class