Public Class SemesterGrades
    Implements IComparable(Of SemesterGrades)

    Public ReadOnly Property StudentNumber As String
    Public ReadOnly Property Grades As IReadOnlyList(Of SubjectGrade)
    Public ReadOnly Property SchoolYear As String

    Public ReadOnly Property SchoolYearStart As Integer
    Public ReadOnly Property SchoolYearEnd As Integer

    Public ReadOnly Property Semester As String

    Public ReadOnly Property SemesterAsNumber As Byte

    Public ReadOnly Property TimeSignature As String


    Sub New(studentNumber As String, grades As IList(Of SubjectGrade), schoolYearStart As Integer, schoolYearEnd As Integer, semester As Integer)
        Me.StudentNumber = studentNumber
        Me.Grades = grades
        Me.SchoolYearStart = schoolYearStart
        Me.SchoolYearEnd = schoolYearEnd
        Me.SchoolYear = Str(schoolYearStart) + "-" + Str(schoolYearEnd)
        Me.SemesterAsNumber = semester

        If semester = 1 Then
            Me.Semester = "1st"
        ElseIf semester = 2 Then
            Me.Semester = "2nd"
        End If

        TimeSignature = SchoolYear + ", " + Me.Semester + " Sem"
    End Sub


    Public Function CompareTo(other As SemesterGrades) As Integer Implements IComparable(Of SemesterGrades).CompareTo
        If other.SchoolYearStart = Me.SchoolYearStart Then
            Return other.SemesterAsNumber.CompareTo(Me.SemesterAsNumber)
        Else
            Return other.SemesterAsNumber.CompareTo(Me.SemesterAsNumber)
        End If
    End Function

End Class
