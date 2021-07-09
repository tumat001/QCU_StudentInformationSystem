Public Class MockRandomPasswordGenerator
    Implements IRandomPasswordGenerator

    Public Function GenerateRandomPassword() As String Implements IRandomPasswordGenerator.GenerateRandomPassword
        Return "PAD_GeneratedPassword"
    End Function

End Class
