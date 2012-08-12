using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TumblrPhotoCore.Infos
{
    /// <summary>
    /// ポスト情報クラス
    /// </summary>
    public class PostInfo
    {
        /// <summary>
        /// ポストID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 画像のURI
        /// </summary>
        public List<string> UriList { get; set; }
    }
}
