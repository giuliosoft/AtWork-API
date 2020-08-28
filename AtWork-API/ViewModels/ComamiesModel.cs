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

    }
    public class NewsList
    {
        //public string[] Image { get; set; }
        public tbl_News_Comments CommentsCount { get; set; }
        public tbl_News_Comments_Likes LikeCount { get; set; }
        public tbl_News news { get; set; }
        public tbl_Volunteers Volunteers { get; set; }

    }
    
}