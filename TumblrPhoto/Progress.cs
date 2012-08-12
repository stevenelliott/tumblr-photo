using System;
using System.Windows.Forms;

namespace TumblrPhoto
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Progress : Form
    {
        /// <summary>
        /// キャンセル
        /// </summary>
        public bool Cancel { get; set; }

        #region メソッド

        #region コンストラクタ
        /// <summary>
        /// 
        /// </summary>
        public Progress(int maximum)
        {
            InitializeComponent();

            pb.Maximum = maximum;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="message"></param>
        public void CountUp(int value, string message)
        {
            pb.Value = value;

            lblMessage.Text = message;
        }

        public void Message(string message)
        {
            lblMessage.Text = message;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetCancelButton()
        {
            btnAction.Click += new EventHandler(btnCancel_Click);

            btnAction.Text = "キャンセル";
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetOkButton()
        {
            btnAction.Click += new EventHandler(btnOk_Click);

            btnAction.Text = "OK";
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Cancel = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            Close();
        }


    }
}
