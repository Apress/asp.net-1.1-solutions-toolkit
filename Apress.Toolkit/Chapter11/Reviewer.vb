Imports System
Imports System.ComponentModel
Imports System.Web.UI
Imports System.Reflection
Imports System.Data
Imports System.Data.SqlClient

Namespace Apress.Toolkit

    ' SQL Server version of ReviewerDB Class - OleDb version is commented out below

    Public Class ReviewerDB
        Dim _connection As String

        Public Property ConnectionString() As String
            Get
                Return _connection
            End Get

            Set(ByVal Value As String)
                _connection = Value
            End Set
        End Property

        Dim _productid As Integer

        Public Property ProductID() As Integer
            Get
                Return _productid
            End Get

            Set(ByVal Value As Integer)
                _productid = Value
            End Set
        End Property

        Public Sub InsertReview(ByVal strName As String, ByVal strReview As String, ByVal iRating As Integer)
            Dim dbConn As New SqlConnection(ConnectionString)
            Dim dbComm As New SqlCommand
            Dim dbTransaction As SqlTransaction

            dbComm.Connection = dbConn
            dbComm.CommandText = "review_InsertReview"
            dbComm.CommandType = CommandType.StoredProcedure

            dbComm.Parameters.Add("@productid", SqlDbType.Int)
            dbComm.Parameters.Add("@author", SqlDbType.VarChar, 50)
            dbComm.Parameters.Add("@review", SqlDbType.VarChar, 8000)
            dbComm.Parameters.Add("@rating", SqlDbType.Char, 1)

            dbComm.Parameters(0).Value = ProductID
            dbComm.Parameters(1).Value = strName
            dbComm.Parameters(2).Value = strReview
            dbComm.Parameters(3).Value = iRating

            Try
                dbConn.Open()
                dbComm.ExecuteNonQuery()
            Finally
                If dbConn.State = ConnectionState.Open Then
                    dbConn.Close()
                End If
            End Try

        End Sub

        Public ReadOnly Property GetAllReviews() As DataSet
            Get
                Dim dbConn As New SqlConnection(ConnectionString)
                Dim dbComm As New SqlCommand

                dbComm.Connection = dbConn
                dbComm.CommandText = "review_GetReviews"
                dbComm.CommandType = CommandType.StoredProcedure

                dbComm.Parameters.Add("@productid", SqlDbType.Int)
                dbComm.Parameters(0).Value = ProductID

                Dim da As New SqlDataAdapter(dbComm)
                Dim ds As New DataSet("REVIEWS")
                Try
                    da.Fill(ds)
                    Return ds
                Catch
                    Return Nothing
                End Try

            End Get
        End Property

        Public ReadOnly Property GetLastReview() As DataSet
            Get
                Dim dbConn As New SqlConnection(ConnectionString)
                Dim dbComm As New SqlCommand

                dbComm.Connection = dbConn
                dbComm.CommandText = "review_GetLastReview"
                dbComm.CommandType = CommandType.StoredProcedure

                dbComm.Parameters.Add("@productid", SqlDbType.Int)
                dbComm.Parameters(0).Value = ProductID

                Dim da As New SqlDataAdapter(dbComm)
                Dim ds As New DataSet("REVIEWS")

                Try
                    da.Fill(ds)
                    Return ds
                Catch
                    Return Nothing
                End Try
            End Get
        End Property

        Public ReadOnly Property GetAverage() As Single
            Get
                Dim dbConn As New SqlConnection(ConnectionString)
                Dim dbCmd As New SqlCommand
                Dim dbReader As SqlDataReader

                dbCmd.Connection = dbConn
                dbCmd.CommandText = "review_GetAverage"
                dbCmd.CommandType = CommandType.StoredProcedure

                dbCmd.Parameters.Add("@productid", SqlDbType.Int)
                dbCmd.Parameters(0).Value = ProductID

                dbConn.Open()
                dbReader = dbCmd.ExecuteReader(CommandBehavior.CloseConnection)
                If (dbReader.HasRows) Then
                    dbReader.Read()
                    Dim avg As Object = dbReader.GetValue(0)
                    If (TypeOf avg Is DBNull) Then
                        Return 0
                    Else
                        Return (CType(dbReader.GetValue(0), Single))
                    End If
                Else
                    Return 0
                End If
            End Get
        End Property


    End Class



End Namespace
