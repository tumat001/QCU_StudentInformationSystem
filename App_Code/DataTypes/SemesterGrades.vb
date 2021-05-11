Public Class SemesterGrades

    Public ReadOnly Property StudentNumber As String
    Public ReadOnly Property Grades As IReadOnlyList(Of SubjectGrade)
    Public ReadOnly Property SchoolYear As String
    Public ReadOnly Property Semester As String

    Sub New(studentNumber As String, grades As IList(Of SubjectGrade), schoolYear As String, semester As String)
        Me.StudentNumber = studentNumber
        Me.Grades = grades
        Me.SchoolYear = schoolYear
        Me.Semester = semester
    End Sub

End Class
