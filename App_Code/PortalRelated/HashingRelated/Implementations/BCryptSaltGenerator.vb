Imports BCryptDep = BCrypt.Net.BCrypt

Public Class BCryptSaltGenerator
    Implements ISaltGenerator

    Public Function GenerateSalt() As String Implements ISaltGenerator.GenerateSalt
        Return BCryptDep.GenerateSalt()
    End Function

End Class
