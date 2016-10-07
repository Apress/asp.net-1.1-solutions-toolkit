Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

Namespace Apress.Toolkit.SearchEngine

    Public Class IndexFileUtility
        Inherits System.ComponentModel.Component

        Public Sub Save(ByVal index As IndexEntryCollection, _
          ByVal filePath As String, ByVal overwrite As Boolean)

            Dim Mode As FileMode
            If overwrite Then
                Mode = FileMode.Create
            Else
                Mode = FileMode.CreateNew
            End If
            Dim fs As New FileStream(filePath, Mode)
            Dim bf As New BinaryFormatter

            bf.Serialize(fs, index)
            fs.Close()

        End Sub

        Public Function Load(ByVal filePath As String) _
          As IndexEntryCollection

            Dim fs As New FileStream(filePath, FileMode.Open)
            Dim bf As New BinaryFormatter

            Dim Index As IndexEntryCollection
            Index = CType(bf.Deserialize(fs), IndexEntryCollection)
            fs.Close()
            Return Index

        End Function

    End Class

End Namespace