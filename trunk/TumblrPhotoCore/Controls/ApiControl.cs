using System;
using System.Collections.Generic;
using System.Linq;
using TumblrPhotoCore.Infos;
using System.ComponentModel;

namespace TumblrPhotoCore.Controls
{
    public class ApiControl
    {
        #region 定数

        /// <summary>
        /// URI
        /// </summary>
        private const string API_URI = "http://api.tumblr.com/v2/blog/{0}/posts/photo?api_key={1}&offset={2}";

        #endregion

        #region 変数


        /// <summary>
        /// 
        /// </summary>
        private int offset = 0;

        /// <summary>
        /// 
        /// </summary>
        public string BaseHostname { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalPosts { get; set; }

        /// <summary>
        /// APIキー
        /// </summary>
        public string  ApiKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private BackgroundWorker pvBackgroundWorker = new BackgroundWorker();

        /// <summary>
        /// JSONパーサ
        /// </summary>
        private JsonProcessor pvJsonProcessor;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public List<PostInfo> PostInfoList { get; set; }

        #region イベント
        /// <summary>
        /// 
        public event EventHandler ReadEnd;

        public event EventHandler ProgressChanged;
        
        /// </summary>
        #endregion

        #region メッソド

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        public ApiControl(string agBaseHostname, string agApiKey)
        {
            BaseHostname = agBaseHostname ;

            ApiKey = agApiKey;

            pvJsonProcessor = new JsonProcessor();

            PostInfoList = new List<PostInfo>();

            TotalPosts = pvJsonProcessor.GetTotalPosts(string.Format(API_URI, BaseHostname, ApiKey, 1));
        }

        /// <summary>
        /// メソッド
        /// </summary>
        /// <returns></returns>
        public bool CreatePostInfoList()
        {
            pvBackgroundWorker.WorkerReportsProgress = true;

            pvBackgroundWorker.WorkerSupportsCancellation = true;

            pvBackgroundWorker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);

            pvBackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(BackgroundWorker_ProgressChanged);

            pvBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker_RunWorkerCompleted);

            pvBackgroundWorker.RunWorkerAsync();

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void CreatePostInfoListCancel()
        {
            pvBackgroundWorker.CancelAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (offset = 0; offset < TotalPosts; offset += 20)
            {
                if (pvBackgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

                PostInfoList.AddRange(pvJsonProcessor.GetPostInfoList(string.Format(API_URI, BaseHostname, ApiKey, offset)));

                pvBackgroundWorker.ReportProgress(PostInfoList.Count());

                System.Threading.Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="senderm"></param>
        /// <param name="e"></param>
        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressChanged(this, EventArgs.Empty);
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ReadEnd(this, EventArgs.Empty);
        }

        #endregion

    }
}
