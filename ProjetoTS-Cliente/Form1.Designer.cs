namespace ProjetoTS_Cliente
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.buttonGenerateKeys = new System.Windows.Forms.Button();
            this.buttonSavePublicKey = new System.Windows.Forms.Button();
            this.buttonSavePrivateKey = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(433, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Mensagem";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonGenerateKeys
            // 
            this.buttonGenerateKeys.Location = new System.Drawing.Point(12, 45);
            this.buttonGenerateKeys.Name = "buttonGenerateKeys";
            this.buttonGenerateKeys.Size = new System.Drawing.Size(169, 23);
            this.buttonGenerateKeys.TabIndex = 15;
            this.buttonGenerateKeys.Text = "Generate Keys (Private / Public)";
            this.buttonGenerateKeys.UseVisualStyleBackColor = true;
            this.buttonGenerateKeys.Click += new System.EventHandler(this.buttonGenerateKeys_Click);
            // 
            // buttonSavePublicKey
            // 
            this.buttonSavePublicKey.Location = new System.Drawing.Point(198, 45);
            this.buttonSavePublicKey.Name = "buttonSavePublicKey";
            this.buttonSavePublicKey.Size = new System.Drawing.Size(139, 23);
            this.buttonSavePublicKey.TabIndex = 20;
            this.buttonSavePublicKey.Text = "Save PublicKey.txt";
            this.buttonSavePublicKey.UseVisualStyleBackColor = true;
            this.buttonSavePublicKey.Click += new System.EventHandler(this.buttonSavePublicKey_Click);
            // 
            // buttonSavePrivateKey
            // 
            this.buttonSavePrivateKey.Location = new System.Drawing.Point(356, 45);
            this.buttonSavePrivateKey.Name = "buttonSavePrivateKey";
            this.buttonSavePrivateKey.Size = new System.Drawing.Size(138, 23);
            this.buttonSavePrivateKey.TabIndex = 21;
            this.buttonSavePrivateKey.Text = "Save PrivateKey.txt";
            this.buttonSavePrivateKey.UseVisualStyleBackColor = true;
            this.buttonSavePrivateKey.Click += new System.EventHandler(this.buttonSavePrivateKey_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 136);
            this.Controls.Add(this.buttonSavePrivateKey);
            this.Controls.Add(this.buttonSavePublicKey);
            this.Controls.Add(this.buttonGenerateKeys);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonGenerateKeys;
        private System.Windows.Forms.Button buttonSavePublicKey;
        private System.Windows.Forms.Button buttonSavePrivateKey;
    }
}

