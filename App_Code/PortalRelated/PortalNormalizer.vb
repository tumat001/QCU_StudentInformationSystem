''' <summary>
''' A class responsible in checking to see if data in the portal database is normal.
''' <br/><br/>
''' Right now, its only function is to see if the database has a default admin
''' </summary>
Friend Class PortalNormalizer

    Friend Shared ReadOnly Property DEFAULT_ADMIN_USERNAME As String = "DefaultAdmin"
    Friend Shared ReadOnly Property DEFAULT_ADMIN_PASSWORD As String = "DefaultPassword000"

    Public Sub New()

    End Sub

    ''' <summary>
    ''' Checks if portal is in normal conditions.
    ''' <br/><br/>
    ''' As of now, this only checks for the prescence of a default admin.
    ''' </summary>
    ''' <returns>True if portal is in normal condition. False otherwise</returns>
    Public Function IsPortalNormalized() As Boolean
        Dim getter As GetDefaultAdminUsernameQuery = New GetDefaultAdminUsernameQuery()
        Return getter.GetDefaultAdminUsername IsNot Nothing
    End Function

    ''' <summary>
    ''' Normalizes the portal data.
    ''' <br/><br/>
    ''' Right now this only creates a default admin if one does not exist
    ''' </summary>
    Public Sub NormalizePortal()
        CreateDefaultAdmin()
    End Sub

    Private Sub CreateDefaultAdmin()
        Dim defaultAdminBuild As AdminAccount.Builder = New AdminAccount.Builder(DEFAULT_ADMIN_USERNAME, PrivilageMode.DEFAULT_ADMIN)
        CreateAdminAccountAction.CreateDefaultAdminAccount(defaultAdminBuild, DEFAULT_ADMIN_PASSWORD)
    End Sub

End Class
