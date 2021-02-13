namespace SomerenUI
{
    partial class AddActivityForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbActivityTypes = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.calendarActivity = new System.Windows.Forms.MonthCalendar();
            this.cmbHour = new System.Windows.Forms.ComboBox();
            this.cmbMins = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCreateActivity = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cmbActivityTypes
            // 
            this.cmbActivityTypes.FormattingEnabled = true;
            this.cmbActivityTypes.Location = new System.Drawing.Point(171, 47);
            this.cmbActivityTypes.Name = "cmbActivityTypes";
            this.cmbActivityTypes.Size = new System.Drawing.Size(174, 24);
            this.cmbActivityTypes.TabIndex = 0;
            this.cmbActivityTypes.SelectedIndexChanged += new System.EventHandler(this.cmbActivityTypes_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Activity Type:";
            // 
            // calendarActivity
            // 
            this.calendarActivity.Cursor = System.Windows.Forms.Cursors.Default;
            this.calendarActivity.Location = new System.Drawing.Point(137, 134);
            this.calendarActivity.MinDate = new System.DateTime(1990, 2, 6, 0, 0, 0, 0);
            this.calendarActivity.Name = "calendarActivity";
            this.calendarActivity.ShowTodayCircle = false;
            this.calendarActivity.TabIndex = 2;
            // 
            // cmbHour
            // 
            this.cmbHour.FormattingEnabled = true;
            this.cmbHour.Location = new System.Drawing.Point(137, 347);
            this.cmbHour.Name = "cmbHour";
            this.cmbHour.Size = new System.Drawing.Size(45, 24);
            this.cmbHour.TabIndex = 3;
            // 
            // cmbMins
            // 
            this.cmbMins.FormattingEnabled = true;
            this.cmbMins.Location = new System.Drawing.Point(207, 347);
            this.cmbMins.Name = "cmbMins";
            this.cmbMins.Size = new System.Drawing.Size(45, 24);
            this.cmbMins.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(186, 346);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 25);
            this.label2.TabIndex = 5;
            this.label2.Text = ":";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 173);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Select day:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 350);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "Select time:";
            // 
            // btnApply
            // 
            this.btnApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApply.Location = new System.Drawing.Point(191, 409);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(105, 32);
            this.btnApply.TabIndex = 8;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(12, 99);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(153, 17);
            this.label5.TabIndex = 9;
            this.label5.Text = "Create activity type:";
            // 
            // txtCreateActivity
            // 
            this.txtCreateActivity.Location = new System.Drawing.Point(171, 99);
            this.txtCreateActivity.MaxLength = 20;
            this.txtCreateActivity.Name = "txtCreateActivity";
            this.txtCreateActivity.Size = new System.Drawing.Size(174, 22);
            this.txtCreateActivity.TabIndex = 10;
            // 
            // AddActivityForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 447);
            this.Controls.Add(this.txtCreateActivity);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbMins);
            this.Controls.Add(this.cmbHour);
            this.Controls.Add(this.calendarActivity);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbActivityTypes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "AddActivityForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AddActivityForm";
            this.Load += new System.EventHandler(this.AddActivityForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbActivityTypes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MonthCalendar calendarActivity;
        private System.Windows.Forms.ComboBox cmbHour;
        private System.Windows.Forms.ComboBox cmbMins;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCreateActivity;
    }
}