Public MustInherit Class BasePortalQuery

    Protected Shared ReadOnly PORTAL_DATABASE_CONNECTION_STRING As String = PortalQueriesAndActions.PORTAL_DATABASE_CONNECTION_STRING

    Protected Shared ReadOnly STUDENT_TABLE_NAME As String = PortalQueriesAndActions.STUDENT_TABLE_NAME
    Protected Shared ReadOnly STUDENT_TABLE_USERNAME_COLUMN_NAME As String = PortalQueriesAndActions.STUDENT_TABLE_USERNAME_COLUMN_NAME
    Protected Shared ReadOnly STUDENT_TABLE_PASSWORD_COLUMN_NAME As String = PortalQueriesAndActions.STUDENT_TABLE_PASSWORD_COLUMN_NAME
    Protected Shared ReadOnly STUDENT_TABLE_EMAIL_ADDRESS_COLUMN_NAME As String = PortalQueriesAndActions.STUDENT_TABLE_EMAIL_ADDRESS_COLUMN_NAME
    Protected Shared ReadOnly STUDENT_TABLE_DISABLED_COLUMN_NAME As String = PortalQueriesAndActions.STUDENT_TABLE_DISABLED_COLUMN_NAME

    Protected Shared ReadOnly ADMIN_TABLE_NAME As String = PortalQueriesAndActions.ADMIN_TABLE_NAME
    Protected Shared ReadOnly ADMIN_TABLE_USERNAME_COLUMN_NAME As String = PortalQueriesAndActions.ADMIN_TABLE_USERNAME_COLUMN_NAME
    Protected Shared ReadOnly ADMIN_TABLE_PASSWORD_COLUMN_NAME As String = PortalQueriesAndActions.ADMIN_TABLE_PASSWORD_COLUMN_NAME
    Protected Shared ReadOnly ADMIN_TABLE_PRIVILAGE_COLUMN_NAME As String = PortalQueriesAndActions.ADMIN_TABLE_PRIVILAGE_COLUMN_NAME

    Protected Shared ReadOnly DOCUMENT_TABLE_NAME As String = PortalQueriesAndActions.DOCUMENT_TABLE_NAME
    Protected Shared ReadOnly DOCUMENT_TABLE_ID_COLUMN_NAME As String = PortalQueriesAndActions.DOCUMENT_TABLE_ID_COLUMN_NAME
    Protected Shared ReadOnly DOCUMENT_TABLE_NAME_COLUMN_NAME As String = PortalQueriesAndActions.DOCUMENT_TABLE_NAME_COLUMN_NAME

    Protected Sub New()

    End Sub

End Class
