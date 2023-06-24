Imports System.IO.Compression
Imports System.IO
Imports System.Text
Imports System.Xml
Imports Google.Cloud.Translation.V2



Public Class Form1


    Const GoogleCloudApiKey = "AIzaSyA_lbC-g1PfLkg6nskzmd0tJ0NagGhQ-D0"

    Dim str_arr(,) As String
    Dim translated() As String

    Private Sub Read_xml(ByVal XMLFilepath As String)

        Dim XMLDoc As New Xml.XmlDocument
        Dim XMLNode As Xml.XmlNode
        Dim count_rows As Integer
        Dim i As Integer = 0
        Dim elemList As XmlNodeList = XMLDoc.GetElementsByTagName("label")

        Try
            XMLDoc.Load(XMLFilepath)
        Catch e As Exception
            Throw New Exception(e.Message)
        End Try


        count_rows = elemList.Count
        If count_rows = 0 Then
            count_rows += 1
        End If

        ReDim str_arr(2, count_rows - 1)

        For Each XMLNode In XMLDoc.DocumentElement.ChildNodes

            str_arr(0, i) = XMLNode.Attributes(0).Value
            str_arr(1, i) = XMLNode.Attributes(1).Value
            str_arr(2, i) = XMLNode.Attributes(2).Value

            i += 1
        Next

    End Sub


    Sub generate_table()
        Dim col As Integer
        Dim row As Integer

        col = str_arr.GetLength(0)    'количество столбцов
        row = str_arr.GetLength(1)      'количество строк

        DataGridView1.ColumnCount = col + 2
        DataGridView1.RowCount = row


        'нумерація рядків
        For i = 0 To row - 1
            DataGridView1.Rows(i).Cells(0).Value = i + 1
        Next

        For n = 0 To row - 1
            For k = 0 To col - 1
                DataGridView1.Rows(n).Cells(k + 1).Value = str_arr(k, n)

            Next k
        Next n

    End Sub

    Private Sub Translate_testing()

        Dim translateClient As TranslationClient
        Dim response As TranslationResult


        translateClient = TranslationClient.CreateFromApiKey(GoogleCloudApiKey)
        response = translateClient.TranslateText(TextBox1.Text, LanguageCodes.Ukrainian, LanguageCodes.English)
        Label1.Text = response.TranslatedText

    End Sub


    'переклад з використанням google
    Private Sub trans_table()


        'ініціалізація перекладача
        Dim translateClient As TranslationClient
        Dim response As TranslationResult
        Dim temp_row As String
        translateClient = TranslationClient.CreateFromApiKey(GoogleCloudApiKey)

        ReDim translated(str_arr.GetLength(1) - 1)


        For i = 0 To str_arr.GetLength(1) - 1


            'розбивання строки по символай переносу рядка
            Dim splitedrow() As String

            splitedrow = str_arr(2, i).Split(vbLf)
            temp_row = ""

            'переклад окремих частин, розбитого рядка
            For s = 0 To splitedrow.Length - 1

                response = translateClient.TranslateHtml(splitedrow(s), LanguageCodes.Ukrainian, LanguageCodes.Polish)

                'збирання перекладеного рядка та додавання переносів
                If s < splitedrow.Length - 1 Then
                    temp_row = temp_row + response.TranslatedText + vbLf
                Else
                    temp_row = temp_row + response.TranslatedText
                End If

            Next

            'запис у масив з перекладом та вивід у таблицю
            translated(i) = temp_row
            DataGridView1.Rows(i).Cells(4).Value = temp_row


            'response = translateClient.TranslateHtml(str_arr(2, i), LanguageCodes.Ukrainian, LanguageCodes.Polish)
            'translated(i) = response.TranslatedText
            'DataGridView1.Rows(i).Cells(4).Value = response.TranslatedText


        Next

            MessageBox.Show("Done")


    End Sub



    'запуск на виконня парсингу xml файлу
    Private Sub Start_Click(sender As Object, e As EventArgs) Handles Start.Click
        Dim xml_path As String

        OpenFileDialog1.InitialDirectory = Directory.GetCurrentDirectory()

        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then

            xml_path = OpenFileDialog1.FileName
            Read_xml(xml_path)

        End If

        generate_table()

    End Sub

    'тестове бознащо 
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Translate_testing()

    End Sub


    'виклик функції перекладу
    Private Sub Translate_start_Click(sender As Object, e As EventArgs) Handles Translate_start.Click

        trans_table()


    End Sub
End Class
