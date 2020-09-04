using AtWork_API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AtWork_API.ViewModels
{
    public class ComamiesModel
    {
        public tbl_Companies Companies { get; set; }
        public tbl_Volunteers Volunteers { get; set; }
    }
    public class NewsCommets
    {
        public tbl_News News { get; set; }
        public int Comments_Likes { get; set; }
        public string Day { get; set; }
        public tbl_Volunteers Volunteers { get; set; }
        public int Id { get; set; }
        public int LikeCount { get; set; }
        public string coUniqueID { get; set; }
        public string newsUniqueID { get; set; }
        public string comByID { get; set; }
        public DateTime? comDate { get; set; }
        public string comContent { get; set; }
        public bool LikeByLoginUser { get; set; }
        //public int LikeId { get; set; }

    }
    public class NewsList
    {
        //public List<string> Image { get; set; }
        public int CommentsCount { get; set; }
        public int LikeCount { get; set; }
        public tbl_News news { get; set; }
        public tbl_Volunteers Volunteers { get; set; }
        public string Day { get; set; }

    }
    public class CommentsList
    {
        public int CommentsCount { get; set; }
        public int LikeCount { get; set; }
        public string coUniqueID { get; set; }
        public string newsUniqueID { get; set; }
        public string comByID { get; set; }
        public DateTime? comDate { get; set; }
        public string comContent { get; set; }
        public tbl_Volunteers Volunteers { get; set; }
    }

}