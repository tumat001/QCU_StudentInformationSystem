Public Class PeekAccountlessStudentsQuery
    Inherits BasePortalQuery

    Public Sub New()

    End Sub

    ''' <summary>
    ''' Collects a list of students in provided student source that do not have an account based on their username.<br></br>
    ''' A student's student number is used (as the username) to determine if they possess an account.
    ''' </summary>
    ''' <param name="studentSource"></param>
    ''' <returns>A list of students who do not have an account</returns>
    Public Function PeekAccountlessStudents(studentSource As IStudentSource) As IReadOnlyList(Of Student)
        Dim accountlessStudents As IList(Of Student) = New List(Of Student)
        Dim accountExistsQuery As IfAccountExistsQuery = New IfAccountExistsQuery()

        For Each candidateStudent As Student In studentSource.GetAllStudents()
            If Not accountExistsQuery.IfStudentAccountExists(candidateStudent.StudentNumber) Then
                accountlessStudents.Add(candidateStudent)
            End If
        Next

        Return accountlessStudents
    End Function

End Class
