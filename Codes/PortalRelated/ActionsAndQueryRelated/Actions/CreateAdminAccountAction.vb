Imports System.Data.SqlClient

Class CreateAdminAccountAction
    Inherits BasePortalAction

    Private Shared ReadOnly ALLOWED_PRIVILAGE_MODES As IReadOnlyList(Of PrivilageMode) = New List(Of PrivilageMode) From {
        PrivilageMode.SUPER_ADMIN, PrivilageMode.DEFAULT_ADMIN
        }

    ReadOnly Property ExecutorUsername As String

    ''' <summary>
    ''' Instantiates a <see cref="CreateAdminAccountAction"/> that allows the provided executor to create accounts.
    ''' </summary>
    ''' <param name="executorUsername"></param>
    ''' <exception cref="AccountDoesNotExistException"></exception>
    ''' <exception cref="PrivilageLevelNotMetException"></exception>
    Sub New(executorUsername As String)
        MyBase.New(ALLOWED_PRIVILAGE_MODES, executorUsername)
        Me.ExecutorUsername = executorUsername
    End Sub

    ''' <summary>
    ''' Attemps to create an admin account with the provided attributes in the provided <see cref="AdminAccount.Builder"/>.<br /><br />
    ''' Passing <see cref="PrivilageMode.DEFAULT_ADMIN"/> as the admin privilage mode will cause this method to return false, 
    ''' since a new default admin cannot be created.
    ''' </summary>
    ''' <param name="buildInfo"></param>
    ''' <param name="password"></param>
    ''' <returns>True, if the admin account was created. False otherwise.</returns>
    ''' <exception cref="AccountAlreadyExistsException"></exception>
    ''' <exception cref="StringConstraintsViolatedException"></exception>
    Public Function CreateAdminAccount(buildInfo As AdminAccount.Builder, password As String) As Boolean
        'Exception Check
        If PortalQueriesAndActions.IfAccountExists(buildInfo.Username) Then
            Throw New AccountAlreadyExistsException(buildInfo.Username)
        End If

        'Do not allow default admins to be created
        If buildInfo.PrivilageMode.Equals(PrivilageMode.DEFAULT_ADMIN) Then
            Return False
        End If


        Dim username As String = buildInfo.Username
        Dim adminPrivilage As PrivilageMode = buildInfo.PrivilageMode

        'Input contraints Check
        NewUsernameConstraint.Evaluate(username)
        NewPasswordConstraint.Evaluate(password)

        'Hash the password
        password = PortalQueriesAndActions.Hasher.HashWithGeneratedSalt(password)

        'Do sql stuffs
        Using connection As SqlConnection = New SqlConnection(PORTAL_DATABASE_CONNECTION_STRING)
            connection.Open()
            Dim command As SqlCommand = connection.CreateCommand()
            Dim sqlTrans As SqlTransaction = connection.BeginTransaction()

            command.Connection = connection
            command.Transaction = sqlTrans

            Try
                Dim sqlCom As String = String.Format("INSERT INTO [AdminAccountTable] ([{0}], [{1}], [{2}]) VALUES (@UsernameValue, @PasswordValue, @PrivilageValue)",
                                    ADMIN_TABLE_USERNAME_COLUMN_NAME, ADMIN_TABLE_PASSWORD_COLUMN_NAME, ADMIN_TABLE_PRIVILAGE_COLUMN_NAME)
                command.CommandText = sqlCom
                command.Parameters.Add(New SqlParameter("AdminAccountTable", ADMIN_TABLE_NAME))
                command.Parameters.Add(New SqlParameter("UsernameValue", username))
                command.Parameters.Add(New SqlParameter("PasswordValue", password))
                command.Parameters.Add(New SqlParameter("PrivilageValue", adminPrivilage.ToString))
                Dim affectedRows = command.ExecuteNonQuery()
                sqlTrans.Commit()
                Return affectedRows = 1

            Catch e As Exception
                sqlTrans.Rollback()
                Return False
            End Try

        End Using
    End Function

    ''' <summary>
    ''' For use of <see cref="PortalNormalizer"/> only!
    ''' </summary>
    ''' <param name="buildInfo"></param>
    ''' <param name="password"></param>
    ''' <returns></returns>
    Friend Shared Function CreateDefaultAdminAccount(buildInfo As AdminAccount.Builder, password As String) As Boolean
        'Checks
        Dim getter As GetDefaultAdminUsernameQuery = New GetDefaultAdminUsernameQuery()
        If getter.GetDefaultAdminUsername IsNot Nothing Then
            Return False
        End If
        If PortalQueriesAndActions.IfAccountExists(buildInfo.Username) Then
            Throw New AccountAlreadyExistsException(buildInfo.Username)
        End If
        If Not buildInfo.PrivilageMode.Equals(PrivilageMode.DEFAULT_ADMIN) Then
            Return False
        End If

        Dim username As String = buildInfo.Username
        Dim adminPrivilage As PrivilageMode = buildInfo.PrivilageMode

        'Input Constrinat Checker
        NewUsernameConstraint.Evaluate(username)
        NewPasswordConstraint.Evaluate(password)

        'Hash the password
        password = PortalQueriesAndActions.Hasher.HashWithGeneratedSalt(password)

        Using connection As SqlConnection = New SqlConnection(PORTAL_DATABASE_CONNECTION_STRING)
            connection.Open()
            Dim command As SqlCommand = connection.CreateCommand()
            Dim sqlTrans As SqlTransaction = connection.BeginTransaction()

            command.Connection = connection
            command.Transaction = sqlTrans

            Try
                Dim sqlCom As String = String.Format("INSERT INTO [AdminAccountTable] ([{0}], [{1}], [{2}]) VALUES (@UsernameValue, @PasswordValue, @PrivilageValue)",
                                    ADMIN_TABLE_USERNAME_COLUMN_NAME, ADMIN_TABLE_PASSWORD_COLUMN_NAME, ADMIN_TABLE_PRIVILAGE_COLUMN_NAME)
                command.CommandText = sqlCom
                command.Parameters.Add(New SqlParameter("AdminAccountTable", ADMIN_TABLE_NAME))
                command.Parameters.Add(New SqlParameter("UsernameValue", username))
                command.Parameters.Add(New SqlParameter("PasswordValue", password))
                command.Parameters.Add(New SqlParameter("PrivilageValue", adminPrivilage.ToString))
                Dim affectedRows = command.ExecuteNonQuery()
                sqlTrans.Commit()
                Return affectedRows = 1

            Catch e As Exception
                sqlTrans.Rollback()
                Return False
            End Try

        End Using
    End Function

End Class
