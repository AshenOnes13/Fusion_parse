Imports System.IO.Compression
Imports System.IO
Imports System.Text
Imports System.Xml
Imports Google.Cloud.Translation.V2



Public Class Translate_form


    Const GoogleCloudApiKey = "AIzaSyA_lbC-g1PfLkg6nskzmd0tJ0NagGhQ-D0"

    Dim str_arr(,) As String
    Dim translated() As String

    Public save_statement As Integer = 0 '0 - close, 1 - save new, 2 - rewrite


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


    'створення 
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
        response = translateClient.TranslateText(TextBox1.Text, LanguageCodes.Ukrainian)
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

                response = translateClient.TranslateHtml(splitedrow(s), LanguageCodes.Ukrainian)

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

    Sub write_xml()

        popup_form_saving.ShowDialog()

        Dim myStream As String

        'зберігання нового файлу
        If save_statement = 1 Then

            If SaveFileDialog1.ShowDialog() = DialogResult.OK Then
                myStream = SaveFileDialog1.FileName()
                If (myStream IsNot Nothing) Then
                    save_new(myStream)
                End If
            End If

            'перезапис існуючого
        ElseIf save_statement = 2 Then
            rewrite_file()
        End If
    End Sub

    Sub save_new(ByVal XMLFilePath As String)

        Dim XMLDoc As New Xml.XmlDocument
        Dim RootNode() As String
        Dim NewXMLNode As Xml.XmlNode

        Dim XMLAttribute1 As Xml.XmlAttribute
        Dim XMLAttribute2 As Xml.XmlAttribute
        Dim XMLAttribute3 As Xml.XmlAttribute

        RootNode = {"label"}

        XMLDoc.LoadXml(("<?xml version='1.0'  encoding='utf-8'?>" & "<Resource>" & "</Resource>"))

        For i = 0 To str_arr.GetLength(1) - 1
            NewXMLNode = XMLDoc.CreateNode(Xml.XmlNodeType.Element, "label", "label", "")

            XMLAttribute1 = XMLDoc.CreateAttribute("commandName")
            XMLAttribute1.Value = str_arr(0, i)
            XMLAttribute2 = XMLDoc.CreateAttribute("devLabel")
            XMLAttribute2.Value = str_arr(1, i)
            XMLAttribute3 = XMLDoc.CreateAttribute("translation")
            XMLAttribute3.Value = translated(i)

            NewXMLNode.Attributes.Append(XMLAttribute1)
            NewXMLNode.Attributes.Append(XMLAttribute2)
            NewXMLNode.Attributes.Append(XMLAttribute3)

            XMLDoc.DocumentElement.AppendChild(NewXMLNode)

        Next

        XMLDoc.Save(XMLFilePath)

    End Sub

    Sub rewrite_file()

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

    Private Sub save_file_Click(sender As Object, e As EventArgs) Handles save_file.Click

        write_xml()

    End Sub


End Class
