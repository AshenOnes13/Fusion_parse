Public Class popup_form_saving


    Private Sub save_new_Click(sender As Object, e As EventArgs) Handles save_new.Click
        Translate_form.save_statement = 1
        Me.Close()
    End Sub

    Private Sub Rewrite_Click(sender As Object, e As EventArgs) Handles Rewrite.Click
        Translate_form.save_statement = 2
        Me.Close()
    End Sub

    Private Sub cancel_Click(sender As Object, e As EventArgs) Handles cancel.Click
        Translate_form.save_statement = 0
        Me.Close()
    End Sub
End Class