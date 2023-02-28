Imports System.IO

Public Class Form1
    Dim path As String
    Dim both As Boolean = False
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If CheckBox1.Checked = True Then getfile(1) Else getfile(0)
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If CheckBox1.Checked = True Then getdir(1) Else getdir(0)
    End Sub
    Private Function getfile(ctrl As Integer)
        Try
            ListBox1.Items.Clear()

            If ctrl = 1 Then
                Dim txtFilesArray As String() = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                Dim max As Int64 = txtFilesArray.Count
                ProgressBar1.Maximum = max

                For i As Integer = 0 To txtFilesArray.Count - 1
                    ListBox1.Items.Add(txtFilesArray(i))
                    ProgressBar1.Value = i + 1
                Next

            Else
                Dim txtFilesArray As String() = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly)
                Dim max As Int64 = txtFilesArray.Count
                ProgressBar1.Maximum = max

                For i As Integer = 0 To txtFilesArray.Count - 1
                    ListBox1.Items.Add(txtFilesArray(i))
                    ProgressBar1.Value = i + 1
                Next
            End If
        Catch ex As Exception
            MsgBox("Error..")
        End Try
        If Not TextBox1.Text = vbNullString Then
            chfname(1)
        ElseIf TextBox1.Text = vbNullString And Not TextBox3.Text = vbNullString Then
            chfext()
        End If

        If both = True Then
            'both = False
            If CheckBox1.Checked = True Then getdir(1) Else getdir(0)
        End If

        Return True
    End Function

    Private Function getdir(ctrl As Integer)
        Try
            If Not both = True Then
                ListBox1.Items.Clear()
            Else
                both = False
            End If

            If ctrl = 1 Then
                GetSubDirectories()
            Else
                Dim dir As String() = Directory.GetDirectories(path)
                For Each folder As String In dir
                    ListBox1.Items.Add(folder + "\")
                Next
            End If
        Catch ex As Exception
            MsgBox("Error..")
        End Try

        If Not TextBox1.Text = vbNullString Then chfname(2)
        Return True
    End Function

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim fb As New FolderBrowserDialog
        Dim folder As String
        fb.ShowDialog()

        If Not fb.SelectedPath = vbNullString Or vbNull Then
            folder = fb.SelectedPath
            TextBox2.Text = folder
        End If
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        path = TextBox2.Text
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        ListBox1.Items.Clear()
        ProgressBar1.Value = 0
    End Sub
    Public Sub GetSubDirectories()
        Dim subdirectoryEntries As String() = Directory.GetDirectories(path)
        For Each subdirectory As String In subdirectoryEntries
            LoadSubDirs(subdirectory)
        Next
    End Sub
    Private Sub LoadSubDirs(dir As String)
        ListBox1.Items.Add(dir + "\")
        Dim subdirectoryEntries As String() = Directory.GetDirectories(dir)
        For Each subdirectory As String In subdirectoryEntries
            LoadSubDirs(subdirectory)
        Next
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        both = True
        If CheckBox1.Checked = True Then getfile(1) Else getfile(0)

    End Sub

    Private Sub Form1_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
        If Me.Width > 882 Or Me.Width < 882 Then
            Me.Width = 882
        End If
    End Sub

    Private Function chfname(type As Integer)
        Dim count As Integer = ListBox1.Items.Count
        Dim ctrl As Integer = 0
        Dim alt As Integer = 0
        Dim indexs(count) As Integer
        Dim nms(count) As String
        'Dim del() As Integer

        For i As Integer = 1 To ListBox1.Items.Count
            nms(i) = ListBox1.GetItemText(ListBox1.Items(i - 1))
            Dim name As String
            If type = 1 Then
                name = nms(i).Remove(0, nms(i).LastIndexOfAny("\") + 1)
            Else
                Dim nm1 = nms(i).Remove(nms(i).LastIndexOfAny("\"))
                name = nm1.Remove(0, nm1.LastIndexOfAny("\") + 1)
            End If

            If Not name.Contains(TextBox1.Text) Then
                ctrl += 1
                indexs(ctrl) = i
            End If
        Next

        For i As Integer = 1 To ctrl
            alt += 1
            ListBox1.Items.RemoveAt(indexs(i) - alt)
        Next

        If Not TextBox3.Text = vbNullString Then chfext()

        Return True
    End Function

    Private Function chfext()
        Dim count As Integer = ListBox1.Items.Count
        Dim ctrl As Integer = 0
        Dim alt As Integer = 0
        Dim indexs(count) As Integer
        Dim nms(count) As String
        Dim ext As String = TextBox3.Text

        For i As Integer = 1 To ListBox1.Items.Count
            nms(i) = ListBox1.GetItemText(ListBox1.Items(i - 1))
            Dim name = nms(i).Remove(0, nms(i).LastIndexOfAny(".") + 1)

            If Not name = ext Then
                ctrl += 1
                indexs(ctrl) = i
            End If
        Next

        For i As Integer = 1 To ctrl
            alt += 1
            ListBox1.Items.RemoveAt(indexs(i) - alt)
        Next


        Return True
    End Function

    Private Sub TextBox3_GotFocus(sender As Object, e As EventArgs) Handles TextBox3.GotFocus
        Label3.Visible = True
    End Sub

    Private Sub TextBox3_Leave(sender As Object, e As EventArgs) Handles TextBox3.Leave
        Label3.Visible = False
    End Sub
End Class
