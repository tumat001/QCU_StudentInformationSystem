Imports BCryptDep = BCrypt.Net.BCrypt

Public Class BCryptHashValueComparator
    Implements IHashValueComparator

    Public Function ValueEquals(input As String, hash As String) As Boolean Implements IHashValueComparator.ValueEquals
        Return BCryptDep.EnhancedVerify(input, hash)
    End Function

End Class
