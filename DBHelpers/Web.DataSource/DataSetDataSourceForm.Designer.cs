namespace Web.DataSource
{
    partial class DataSetDataSourceForm
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
            System.Windows.Forms.Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataSetDataSourceForm));
            System.Windows.Forms.Button button1;
            System.Windows.Forms.Button button2;
            this.comboBoxComponent = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            button1 = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // button1
            // 
            resources.ApplyResources(button1, "button1");
            button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            button1.Name = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            resources.ApplyResources(button2, "button2");
            button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            button2.Name = "button2";
            button2.UseVisualStyleBackColor = true;
            // 
            // comboBoxComponent
            // 
            resources.ApplyResources(this.comboBoxComponent, "comboBoxComponent");
            this.comboBoxComponent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxComponent.FormattingEnabled = true;
            this.comboBoxComponent.Name = "comboBoxComponent";
            // 
            // DataSetDataSourceForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(button2);
            this.Controls.Add(button1);
            this.Controls.Add(label1);
            this.Controls.Add(this.comboBoxComponent);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DataSetDataSourceForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxComponent;
    }
}