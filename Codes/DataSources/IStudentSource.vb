Public Interface IStudentSource


    Function GetAllStudents() As IReadOnlyList(Of Student)
    Function GetAllStudentNumbers() As IReadOnlyList(Of String)
    Function GetStudent(ByVal studentNumber As String) As Student

End Interface
