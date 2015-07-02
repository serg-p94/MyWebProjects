using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using MvcCalculator.Helper;

namespace MvcCalculator.Models
{
    public class Movie
    {
        public const string CoversPathUrl = "~/Covers/";

        public int MovieId { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public MovieGenre Genre { get; set; }
        public float Rating { get; set; }
        public int MarksNumber { get; set; }
        public string Description { get; set; }
        public string Cover { get; set; }
        public Actor[] Actors { get; set; }

        [Authorize]
        public int GetMark()
        {
            return new RatingManager().GetMark(HttpContext.Current.User.Identity.Name, MovieId);
        }
    }

    public class Actor
    {
        public int ActorId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public static int CompareAsc(Actor a1, Actor a2)
        {
            return (a1.Surname + a1.Name).CompareTo(a2.Surname + a2.Name);
        }

        public static int CompareDesc(Actor a2, Actor a1)
        {
            return (a1.Surname + a1.Name).CompareTo(a2.Surname + a2.Name);
        }
    }
    public enum MovieGenre
    {
        Action,
        Adventure,
        Animation,
        Biography,
        Comedy,
        Crime,
        Documentary,
        Drama,
        Family,
        Fantasy,
        History,
        Horror,
        Music,
        Musical,
        Mystery,
        News,
        Romance,
        SciFi,
        Sport,
        Thriller,
        War,
        Western
    }
}