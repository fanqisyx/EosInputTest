Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        TextBox2.Text = StringToUnicode(TextBox1.Text)
    End Sub

    '将中文转为unicode编码，如：ZRO 耳麦 12345，转后为：ZRO \u8033\u9EA6 12345
    Function StringToUnicode(strCode As String) As String
        Dim a() As String
        Dim str As String
        Dim i As Integer
        For i = 0 To Len(strCode) - 1
            On Error Resume Next
            str = Mid(strCode, i + 1, 1)
            If isChinese(str) = True Then '//是中文
                'StringToUnicode = StringToUnicode & "&#" & StrDup(4 - Len(Hex(AscW(str))), "0") & Hex(AscW(str))
                StringToUnicode = StringToUnicode & "&#" & AscW(str) & ";"
            Else '//不是中文
                StringToUnicode = StringToUnicode & str
            End If
        Next
    End Function

    '是否为中文
    Public Function isChinese(Text As String) As Boolean

        Dim l As Long
        Dim i As Long
        l = Len(Text)
        isChinese = False

        For i = 1 To l
            If Asc(Mid(Text, i, 1)) < 0 Or Asc(Mid(Text, i, 1)) < 0 Then
                isChinese = True
                Exit Function
            End If
        Next

    End Function

    Private Sub GroupBox2_Enter(sender As Object, e As EventArgs) Handles GroupBox2.Enter

    End Sub

    Private Structure productinfo
        Dim name As String
        Dim code As String
        Dim model As String
        Dim allunit As String
        Dim allnum As Integer
        Dim finishunit As String
        Dim finishnum As Integer
        Dim uid As String
        Dim miaoshu As String
    End Structure
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim str As String = TextBox3.Text
        Dim a As productinfo = getinfoFromString(str)
        TextBox4.Text = Getoutputstring(a)
        My.Computer.Clipboard.SetText(TextBox4.Text)
    End Sub
    Private Function Getoutputstring(a As productinfo) As String
        Dim outputstr As String
        outputstr = String.Format("<products assetTypeEnumId=""AstTpInventory"" 
assetClassEnumId=""AsClsInventory"" 
description=""{2}"" 
productName=""{0}"" 
pseudoId=""{1}"" 
amountUomId=""OTH_ea"" 
amountFixed=""2"" 
lastUpdatedStamp=""1656903551196"" 
productId=""{1}"" 
salesIntroductionDate=""1656903479772"" 
productTypeEnumId=""PtAsset"" 
ownerPartyId=""218921fe-a65a-4af6-bf2a-710f4fc1e49f"" 
statusId=""ProdCreated""/>", StringToUnicode(a.name), a.uid, StringToUnicode(a.miaoshu))
        Return outputstr
    End Function
    Private Function getinfoFromString(str) As productinfo
        Dim arraystr() As String = str.Split(",")
        If arraystr.Length < 7 Then
            MessageBox.Show(str)
        End If
        Dim a As productinfo = New productinfo
        a.name = arraystr(1)
        a.code = arraystr(0)
        a.model = arraystr(2)
        a.allnum = GetStringNum(arraystr(4))
        a.allunit = arraystr(3)
        a.finishunit = arraystr(5)
        a.finishnum = GetStringNum(arraystr(6))

        a.uid = getuid()
        a.miaoshu = "我描述我自己"
        Return a
    End Function
    Private Function GetStringNum(str As String) As Integer
        Dim outputnum As Integer
        If IsNumeric(str) = True Then
            outputnum = Val(str)
        Else
            outputnum = 0
        End If
        Return outputnum
    End Function

    Private Function getuid() As String
        Dim g As Guid = Guid.NewGuid()
        Return g.ToString()
    End Function

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim str() As String = TextBox5.Text.Split(vbNewLine)
        'MessageBox.Show(str(3).ToString())
        Dim outputstring As String = ""
        Try
            For Each mystr In str
                If mystr.Contains(",") = False Then Continue For
                Dim a As productinfo = getinfoFromString(mystr)
                outputstring = outputstring + Getoutputstring(a) + vbNewLine
            Next
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try

        TextBox6.Text = outputstring
        My.Computer.Clipboard.SetText(TextBox6.Text)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If OpenFileDialog1.ShowDialog = DialogResult.OK Then
            TextBox5.Text = IO.File.ReadAllText(OpenFileDialog1.FileName, System.Text.Encoding.Default)
        End If
    End Sub
End Class
