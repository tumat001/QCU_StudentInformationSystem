Public MustInherit Class BasePortalAction

    Protected Shared ReadOnly PORTAL_DATABASE_CONNECTION_STRING As String = PortalQueriesAndActions.PORTAL_DATABASE_CONNECTION_STRING

    Protected Shared ReadOnly STUDENT_TABLE_NAME As String = PortalQueriesAndActions.STUDENT_TABLE_NAME
    Protected Shared ReadOnly STUDENT_TABLE_USERNAME_COLUMN_NAME As String = PortalQueriesAndActions.STUDENT_TABLE_USERNAME_COLUMN_NAME
    Protected Shared ReadOnly STUDENT_TABLE_PASSWORD_COLUMN_NAME As String = PortalQueriesAndActions.STUDENT_TABLE_PASSWORD_COLUMN_NAME
    Protected Shared ReadOnly STUDENT_TABLE_EMAIL_ADDRESS_COLUMN_NAME As String = PortalQueriesAndActions.STUDENT_TABLE_EMAIL_ADDRESS_COLUMN_NAME

    Protected Shared ReadOnly ADMIN_TABLE_NAME As String = PortalQueriesAndActions.ADMIN_TABLE_NAME
    Protected Shared ReadOnly ADMIN_TABLE_USERNAME_COLUMN_NAME As String = PortalQueriesAndActions.ADMIN_TABLE_USERNAME_COLUMN_NAME
    Protected Shared ReadOnly ADMIN_TABLE_PASSWORD_COLUMN_NAME As String = PortalQueriesAndActions.ADMIN_TABLE_PASSWORD_COLUMN_NAME
    Protected Shared ReadOnly ADMIN_TABLE_PRIVILAGE_COLUMN_NAME As String = PortalQueriesAndActions.ADMIN_TABLE_PRIVILAGE_COLUMN_NAME

    Protected Shared ReadOnly DOCUMENT_TABLE_NAME As String = PortalQueriesAndActions.DOCUMENT_TABLE_NAME
    Protected Shared ReadOnly DOCUMENT_TABLE_ID_COLUMN_NAME As String = PortalQueriesAndActions.DOCUMENT_TABLE_ID_COLUMN_NAME
    Protected Shared ReadOnly DOCUMENT_TABLE_NAME_COLUMN_NAME As String = PortalQueriesAndActions.DOCUMENT_TABLE_NAME_COLUMN_NAME

    Protected Shared ReadOnly Property NewUsernameConstraint As IStringConstraints = PortalQueriesAndActions.NewUsernameConstraint
    Protected Shared ReadOnly Property NewPasswordConstraint As IStringConstraints = PortalQueriesAndActions.NewPasswordConstraint
    Protected Shared ReadOnly Property NewEmailAddressConstraint As IStringConstraints = PortalQueriesAndActions.NewEmailAddressConstraint

    Public ReadOnly Property AllowedPrivilageModes As IReadOnlyList(Of PrivilageMode)

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="allowedPrivilageModes"></param>
    ''' <param name="executorUser"></param>
    ''' <exception cref="AccountDoesNotExistException"></exception>
    ''' <exception cref="PrivilageLevelNotMetException"></exception>
    Protected Sub New(allowedPrivilageModes As IReadOnlyList(Of PrivilageMode), executorUser As String)
        Me.AllowedPrivilageModes = allowedPrivilageModes

        CheckIfUserHasAllowedPrivilageMode(executorUser)
    End Sub

    Private Function CheckIfUserHasAllowedPrivilageMode(executorUser As String) As Boolean
        Dim privilageLevelOfUser As PrivilageMode = PortalQueriesAndActions.GetPrivilageModeOfAccount(executorUser)

        If AllowedPrivilageModes.Contains(privilageLevelOfUser) Then
            Return True
        Else
            Throw New PrivilageLevelNotMetException(privilageLevelOfUser)
        End If
    End Function

End Class
