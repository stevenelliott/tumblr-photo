using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Diagnostics;
using TumblrPhotoCore.Infos;
using System.Collections.Generic;
using System.ComponentModel;

namespace TumblrPhotoCore.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class DownloadControl
    {

        /// <summary>
        /// 
        /// </summary>
        private List<PostInfo> pvPostInfoList;

        /// <summary>
        /// 
        /// </summary>
        private string pvPath;

        /// <summary>
        /// 
        /// </summary>
        public int TotalPosts { get; set; }

        public int Offset { get; set; }

        /// 
        /// </summary>
        private BackgroundWorker pvBw = new BackgroundWorker();

        #region イベント定義

        /// <summary>
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler DownloadEnd;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler ProgressChanged;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agPostInfoList"></param>
        /// <param name="agPath"></param>
        public DownloadControl(List<PostInfo> agPostInfoList, string agPath)
        {
            pvPostInfoList = agPostInfoList;

            pvPath = agPath;

            TotalPosts = agPostInfoList.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="agPath"></param>
        public void ImageDonload()
        {
            pvBw.WorkerReportsProgress = true;

            pvBw.WorkerSupportsCancellation = true;

            pvBw.DoWork += new DoWorkEventHandler(bw_DoWork);

            pvBw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);

            pvBw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

            pvBw.RunWorkerAsync();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DownloadEnd(this, EventArgs.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {

            string[] filePaths = Directory.GetFiles(pvPath);

            foreach (PostInfo postInfo in pvPostInfoList)
            {

                if (pvBw.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

                Offset += 1;

                for (int i = 0; i < postInfo.UriList.Count; i++)
                {

                    try
                    {
                        if (Array.Exists(filePaths, delegate(string s) { return s.IndexOf(string.Format("{0}_{1}", postInfo.Id, i)) > -1; })) continue;

                        WebClient pvImageWebClient = new WebClient();

                        byte[] result = pvImageWebClient.DownloadData(new Uri(postInfo.UriList[i]));

                        Debug.WriteLine(createSha1(result));

                        File.WriteAllBytes(string.Format("{0}\\{1}_{2}.{3}", pvPath, postInfo.Id, i, getExtension(result)), result);

                    }
                    catch (Exception)
                    {
                    }
                    
                    System.Threading.Thread.Sleep(2000);
                }

                pvBw.ReportProgress(Offset);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private string createSha1(byte[] buffer)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

            byte[] hashs = sha1.ComputeHash(buffer);

            StringBuilder result = new StringBuilder();

            foreach (byte hash in hashs)
            {
                result.Append(hash.ToString("x2"));
            }

            return result.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public void DownloadCacnel()
        {
            pvBw.CancelAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private string getExtension(byte[] bytes)
        {

            if (bytes[0] == 0x42 && bytes[1] == 0x4D)
            {
                return "bmp";
            }
            else if(bytes[0] == 0xFF && bytes[1] == 0xD8 && bytes[2] == 0xFF) {
                return "jpg";
            }
            else if (bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47 && bytes[4] == 0x0D && bytes[5] == 0x0A && bytes[6] == 0x1A && bytes[7] == 0x0A)
            {
                return "png";
            }
            else if (bytes[0] == 0x47 && bytes[1] == 0x49 && bytes[2] == 0x46 && bytes[3] == 0x38 && bytes[4] == 0x39 && bytes[5] == 0x61)
            {
                return "gif";
            }
            else if (bytes[0] == 0x47 && bytes[1] == 0x49 && bytes[2] == 0x46 && bytes[3] == 0x38 && bytes[4] == 0x37 && bytes[5] == 0x61)
            {
                return "gif";
            }

            return null;
        }
    }
}
