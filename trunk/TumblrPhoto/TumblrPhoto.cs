using System;
using System.Linq;
using System.Windows.Forms;
using TumblrPhotoCore.Controls;
using System.Configuration;
using System.IO;

namespace TumblrPhoto
{
    /// <summary>
    /// 
    /// </summary>
    public partial class TumblrPhoto : Form
    {
        /// <summary>
        /// 
        /// </summary>
        private Progress downloadProgress;

        #region イベント

        #region ボタン

        /// <summary>
        /// ダウンロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDownload_Click(object sender, EventArgs e)
        {

            if (txtApiKey.Text == string.Empty)
            {
                MessageBox.Show(this, "APIキーが設定されていません。", Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                return;
            }

            if (string.IsNullOrEmpty(txtFolder.Text) || !Directory.Exists(txtFolder.Text))
            {
                MessageBox.Show(this, "保存先が無効です。", Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                return;
            }

            if (txtUrl.Text == string.Empty)
            {
                MessageBox.Show(this, "URLが設定されていません。", Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                return;
            }



            ApiControl wkApiControl = new ApiControl(new Uri(txtUrl.Text).Host, txtApiKey.Text);

            downloadProgress = new Progress(wkApiControl.TotalPosts);

            wkApiControl.ProgressChanged += new EventHandler(wkApiControl_ProgressChanged);

            wkApiControl.ReadEnd += new EventHandler(wkApiControl_ReadEnd);

            wkApiControl.CreatePostInfoList();

            downloadProgress.SetCancelButton();

            downloadProgress.ShowDialog(this);
        }

        #endregion
        
        #region ダウンロードリスト作成

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wkApiControl_ProgressChanged(object sender, EventArgs e)
        {
            ApiControl wkApiControl = (ApiControl)sender;

            downloadProgress.CountUp(wkApiControl.PostInfoList.Count(), string.Format("ダウンロードリストを作成中です...{0}/{1}", wkApiControl.PostInfoList.Count(), wkApiControl.TotalPosts ));

            if (downloadProgress.Cancel)
            {
                wkApiControl.CreatePostInfoListCancel();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void wkApiControl_ReadEnd(object sender, EventArgs e)
        {
            if (downloadProgress.Cancel)
            {
                downloadProgress.Message("ダウンロードリスト作成を中断しました...");

                downloadProgress.SetOkButton();
            }
            else
            {
                downloadProgress.Message("ダウンロードリスト作成が終了しました...");

                download((ApiControl)sender);
            }
        }
        #endregion

        #endregion

        #region メソッド

        #region コンストラクタ

        /// <summary>
        /// 
        /// </summary>
        public TumblrPhoto()
        {
            InitializeComponent();

            Text = string.Format("{0} {1}"
                , Text
                , System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);

            if (string.IsNullOrEmpty(Properties.Settings.Default.apikey)
                && string.IsNullOrEmpty(Properties.Settings.Default.folder)
                && string.IsNullOrEmpty(Properties.Settings.Default.url))
            {
                Properties.Settings.Default.Upgrade();
            }
            
            txtApiKey.Text = Properties.Settings.Default.apikey;

            txtFolder.Text = Properties.Settings.Default.folder;

            txtUrl.Text = Properties.Settings.Default.url;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agApiControl"></param>
        private void download(ApiControl agApiControl)
        {
            DownloadControl wkDownloadControl = new DownloadControl(agApiControl.PostInfoList, txtFolder.Text);

            wkDownloadControl.ProgressChanged += new EventHandler(DownloadControl_ProgressChanged);

            wkDownloadControl.DownloadEnd += new EventHandler(DownloadControl_DownloadEnd);

            wkDownloadControl.ImageDonload();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DownloadControl_DownloadEnd(object sender, EventArgs e)
        {
            if (downloadProgress.Cancel)
            {
                downloadProgress.Message("画像のダウンロードを中断しました...");

                downloadProgress.SetOkButton();
            }
            else
            {
                downloadProgress.Message("画像のダウンロードが終了しました...");

               downloadProgress.SetOkButton();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DownloadControl_ProgressChanged(object sender, EventArgs e)
        {
            DownloadControl wkDownloadControl = (DownloadControl)sender;

            downloadProgress.CountUp(wkDownloadControl.Offset, string.Format("画像をダウンロード中です...{0}/{1}", wkDownloadControl.Offset, wkDownloadControl.TotalPosts));

            if (downloadProgress.Cancel)
            {
                wkDownloadControl.DownloadCacnel();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog wkFbd = new FolderBrowserDialog();

            wkFbd.Description = "ダウンロードファイルの保存先を選択してください。";

            wkFbd.RootFolder = Environment.SpecialFolder.Desktop;

            if (Directory.Exists(txtFolder.Text))
            {
                wkFbd.SelectedPath = txtFolder.Text;                
            }

            if (wkFbd.ShowDialog(this) == DialogResult.OK) txtFolder.Text = wkFbd.SelectedPath;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TumblrPhoto_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (MessageBox.Show(this
                , "TumblrPhotoの設定を保存しますか？"
                , Text
                , MessageBoxButtons.YesNo
                , MessageBoxIcon.Question
                , MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No) return;

            Properties.Settings.Default.url = txtUrl.Text;

            Properties.Settings.Default.folder = txtFolder.Text; 

            Properties.Settings.Default.apikey = txtApiKey.Text;

            Properties.Settings.Default.Save();

        }

    }
}
