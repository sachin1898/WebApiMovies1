using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApiMovieEx1.Models
{
    [Table("Movies")]
    public class Movies
    {
        [Key]
        public int MId { get; set; }
        public string MName { get; set; }
        public string Starcast { get; set; }
        public string Producer { get; set; }
        public string Director { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}