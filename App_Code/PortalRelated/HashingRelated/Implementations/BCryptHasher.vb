Imports BCryptDep = BCrypt.Net.BCrypt

Public Class BCryptHasher
    Implements IHasher

    'Right now, 8 is the right balance of speed and randomness costs
    Private ReadOnly workFactor As Integer = 8

    Public Function HashWithoutSalt(password As String) As String Implements IHasher.HashWithoutSalt
        Throw New NotImplementedException()
    End Function

    Public Function HashWithSalt(password As String, salt As String) As String Implements IHasher.HashWithSalt
        Throw New NotImplementedException()
    End Function

    Public Function HashWithGeneratedSalt(password As String) As String Implements IHasher.HashWithGeneratedSalt
        Return BCryptDep.EnhancedHashPassword(password, workFactor)
    End Function
End Class
