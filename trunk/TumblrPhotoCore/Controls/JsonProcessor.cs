using System;
using System.Collections.Generic;
using System.Net;
using Codeplex.Data;
using System.Diagnostics;
using TumblrPhotoCore.Infos;

namespace TumblrPhotoCore.Controls
{
    /// <summary>
    /// JSONパーサクラス
    /// </summary>
    public class JsonProcessor
    {
        /// <summary>
        /// ポスト情報リスト取得
        /// </summary>
        /// <returns></returns>
        public List<PostInfo> GetPostInfoList(string uri)
        {
            List<PostInfo> wkPostInfoList = new List<PostInfo>();
            
            var wkJson = new WebClient().DownloadString(@uri);

            var wkTumblr = DynamicJson.Parse(wkJson);

            foreach (var wkPost in (dynamic[])wkTumblr.response.posts)
            {
                PostInfo wkPostInfo = find(wkPostInfoList, wkPost.id);

                wkPostInfo.Id = wkPost.id;

                foreach (var photo in wkPost.photos)
                {
                    if (wkPostInfo.UriList == null) wkPostInfo.UriList = new List<string>();

                    wkPostInfo.UriList.Add(photo.original_size.url);

                    Debug.WriteLine((string)photo.original_size.url);
                }
                
                wkPostInfoList.Add(wkPostInfo);
            }
                
            return wkPostInfoList;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agPostInfoList"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private PostInfo find(List<PostInfo> agPostInfoList, double id)
        {
            foreach (PostInfo wkPostInfo in agPostInfoList)
            {
                if (wkPostInfo.Id == id) return wkPostInfo;
            }

            return new PostInfo();

        }

        /// <summary>
        /// POST数取得
        /// </summary>
        /// <param name="uri">URI</param>
        /// <returns></returns>
        public int GetTotalPosts(string uri)
        {
                var json = new WebClient().DownloadString(@uri);

                var tumblr = DynamicJson.Parse(json);

                Debug.WriteLine((decimal)tumblr.response.total_posts);

                return (int)tumblr.response.total_posts;
        }

    }
}
