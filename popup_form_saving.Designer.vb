<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class popup_form_saving
    Inherits System.Windows.Forms.Form

    'Форма переопределяет dispose для очистки списка компонентов.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Является обязательной для конструктора форм Windows Forms
    Private components As System.ComponentModel.IContainer

    'Примечание: следующая процедура является обязательной для конструктора форм Windows Forms
    'Для ее изменения используйте конструктор форм Windows Form.  
    'Не изменяйте ее в редакторе исходного кода.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.save_new = New System.Windows.Forms.Button()
        Me.Rewrite = New System.Windows.Forms.Button()
        Me.cancel = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'save_new
        '
        Me.save_new.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.save_new.Location = New System.Drawing.Point(164, 96)
        Me.save_new.Name = "save_new"
        Me.save_new.Size = New System.Drawing.Size(101, 31)
        Me.save_new.TabIndex = 0
        Me.save_new.Text = "Save as"
        Me.save_new.UseVisualStyleBackColor = True
        '
        'Rewrite
        '
        Me.Rewrite.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Rewrite.Enabled = False
        Me.Rewrite.Location = New System.Drawing.Point(57, 96)
        Me.Rewrite.Name = "Rewrite"
        Me.Rewrite.Size = New System.Drawing.Size(101, 31)
        Me.Rewrite.TabIndex = 1
        Me.Rewrite.Text = "Save"
        Me.Rewrite.UseVisualStyleBackColor = True
        '
        'cancel
        '
        Me.cancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cancel.Location = New System.Drawing.Point(271, 96)
        Me.cancel.Name = "cancel"
        Me.cancel.Size = New System.Drawing.Size(101, 31)
        Me.cancel.TabIndex = 2
        Me.cancel.Text = "Cancel"
        Me.cancel.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(299, 25)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Як ви бажаєте зберігти файл?"
        '
        'popup_form_saving
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(384, 141)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cancel)
        Me.Controls.Add(Me.Rewrite)
        Me.Controls.Add(Me.save_new)
        Me.MaximumSize = New System.Drawing.Size(400, 180)
        Me.MinimumSize = New System.Drawing.Size(400, 180)
        Me.Name = "popup_form_saving"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "popup_form_saving"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents save_new As Button
    Friend WithEvents Rewrite As Button
    Friend WithEvents cancel As Button
    Friend WithEvents Label1 As Label
End Class
