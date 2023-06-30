Imports System.IO.Compression
Imports System.IO
Imports System.Text
Imports System.Xml
Imports Google.Cloud.Translation.V2


Public Class Translate_form

    Const GoogleCloudApiKey = "AIzaSyA_lbC-g1PfLkg6nskzmd0tJ0NagGhQ-D0"

    Dim str_arr(,) As String            'масив значень початкового документу
    Dim translated() As String          'масив перекладених значень
    Dim xml_path As String              'шлях до документу

    Public save_statement As Integer = 0        'опції для зберігання окремого файлу 0 - close, 1 - save new, 2 - rewrite

    'відкриття та обробка документу
    Private Sub Read_xml(ByVal XMLFilepath As String)

        Dim XMLDoc As New Xml.XmlDocument
        Dim XMLNode As Xml.XmlNode
        Dim count_rows As Integer
        Dim i As Integer = 0
        Dim elemList As XmlNodeList = XMLDoc.GetElementsByTagName("label")  'пошук вузлів по заголовку

        Try
            XMLDoc.Load(XMLFilepath)
        Catch e As Exception
            Throw New Exception(e.Message)
        End Try

        'підрахунок рядків
        count_rows = elemList.Count
        If count_rows = 0 Then
            count_rows += 1
        End If

        ReDim str_arr(2, count_rows - 1)

        'заповнення масиву значення з xml файлу
        For Each XMLNode In XMLDoc.DocumentElement.ChildNodes

            str_arr(0, i) = XMLNode.Attributes(0).Value
            str_arr(1, i) = XMLNode.Attributes(1).Value
            str_arr(2, i) = XMLNode.Attributes(2).Value

            i += 1
        Next

    End Sub


    'відображення таблиці поточних значень файлу
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

        'заповнення таблиці значеннями з масиву
        For n = 0 To row - 1
            For k = 0 To col - 1
                DataGridView1.Rows(n).Cells(k + 1).Value = str_arr(k, n)

            Next k
        Next n

    End Sub


    'переклад з використанням google
    Private Sub trans_table()


        'ініціалізація перекладача
        Dim translateClient As TranslationClient        'перекладач
        Dim response As TranslationResult               'місце для зберігання результату перекладу
        Dim temp_row As String
        translateClient = TranslationClient.CreateFromApiKey(GoogleCloudApiKey)


        ReDim translated(str_arr.GetLength(1) - 1)


        For i = 0 To str_arr.GetLength(1) - 1

            Label2.Text = CStr((i + 1) & " / " & str_arr.GetLength(1))      'виведення обсягу рядків поточного документу
            ProgressBar1.Value = Math.Round(100 * (i + 1) / str_arr.GetLength(1))


            'розбиття рядка по символах переносу рядка
            Dim splitedrow() As String

            'для англійської мови str_arr(1, i)

            splitedrow = str_arr(2, i).Split(vbLf)
            ' splitedrow = str_arr(1, i).Split(vbLf)

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

                Application.DoEvents()      'підтримування активності головного вікна

            Next

            'запис у масив з перекладом та вивід у таблицю
            translated(i) = temp_row
            DataGridView1.Rows(i).Cells(4).Value = temp_row


        Next

        ' MessageBox.Show("Done")

    End Sub


    'виклик вікна збереження файлу 
    Sub write_xml_window()

        popup_form_saving.ShowDialog()      'вікно діалогу збереження

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


    'формування перекладеного xml файлу та його збереження
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
            'створення нового вузла
            NewXMLNode = XMLDoc.CreateNode(Xml.XmlNodeType.Element, "label", "label", "")
            'додавання атрибутів вузла
            XMLAttribute1 = XMLDoc.CreateAttribute("commandName")
            XMLAttribute1.Value = str_arr(0, i)
            XMLAttribute2 = XMLDoc.CreateAttribute("devLabel")
            XMLAttribute2.Value = str_arr(1, i)
            XMLAttribute3 = XMLDoc.CreateAttribute("translation")
            XMLAttribute3.Value = translated(i)
            'запис атрибутів у файл
            NewXMLNode.Attributes.Append(XMLAttribute1)
            NewXMLNode.Attributes.Append(XMLAttribute2)
            NewXMLNode.Attributes.Append(XMLAttribute3)

            XMLDoc.DocumentElement.AppendChild(NewXMLNode)

        Next
        'зберігання
        XMLDoc.Save(XMLFilePath)

    End Sub

    'виконання функції збереження для відкритого файлу
    Sub rewrite_file()

        save_new(xml_path)

    End Sub

    'запуск на виконня парсингу xml файлу
    Private Sub Start_Click(sender As Object, e As EventArgs) Handles Start.Click

        DataGridView1.Rows.Clear()

        OpenFileDialog1.InitialDirectory = Directory.GetCurrentDirectory()

        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then

            xml_path = OpenFileDialog1.FileName
            Read_xml(xml_path)

        End If

        generate_table()

    End Sub

    'виклик функції перекладу
    Private Sub Translate_start_Click(sender As Object, e As EventArgs) Handles Translate_start.Click

        trans_table()

    End Sub

    'відкриття вікна збереження фійлу
    Private Sub save_file_Click(sender As Object, e As EventArgs) Handles save_file.Click

        write_xml_window()

    End Sub

    'автоматичне опрацювання обраної папки файлів
    Private Sub do_all_Click(sender As Object, e As EventArgs) Handles do_all.Click

        Dim files_count As Integer
        Dim cur_file As Integer = 0


        If FolderBrowserDialog1.ShowDialog() = DialogResult.OK Then


            Dim allFiles As IEnumerable(Of String) = Directory.EnumerateFiles(FolderBrowserDialog1.SelectedPath, "*.xml", SearchOption.AllDirectories)
            Dim filePaths As IList(Of String) = allFiles.ToList()


            files_count = filePaths.Count

            If files_count = 0 Then
                MessageBox.Show("File not found")
            Else
                For Each filePath As String In filePaths

                    Label1.Text = CStr((cur_file + 1) & " / " & files_count)

                    DataGridView1.Rows.Clear()

                    xml_path = filePath
                    Read_xml(xml_path)
                    generate_table()
                    trans_table()
                    save_new(xml_path)

                    cur_file += 1

                    Application.DoEvents()

                Next
            End If

            'files_count = Directory.GetFiles(FolderBrowserDialog1.SelectedPath).Count() 'підрахунок кількості файлів

            ''цикл відкриття та обробки кожного файлу
            'For Each LogFile In Directory.GetFiles(FolderBrowserDialog1.SelectedPath)

            '    Label1.Text = CStr((cur_file + 1) & " / " & files_count)

            '    DataGridView1.Rows.Clear()

            '    xml_path = LogFile
            '    Read_xml(xml_path)
            '    generate_table()
            '    trans_table()
            '    save_new(xml_path)

            '    cur_file += 1

            '    Application.DoEvents()

            'Next

        End If

    End Sub
End Class
