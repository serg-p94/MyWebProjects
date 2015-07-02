using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using MvcCalculator.Helper;
using MvcCalculator.Models;
using System;
using System.Web;
using System.Web.Mvc;

namespace MvcCalculator.Controllers
{
    public class MovieController : Controller
    {
        [Authorize]
        public ActionResult Add(string title, string releaseDate, string genre, string description, string actors)
        {
            ViewBag.MainMenu = MainMenuItems.Movies;

            DateTime dtReleaseDate;
            bool readyToAdd = true;
            var mov = new Movie();
            mov.Title = title;
            mov.Description = description;
            if (releaseDate != null &&
                DateTime.TryParse(releaseDate, new CultureInfo("ru-RU"), DateTimeStyles.None, out dtReleaseDate))
            {
                mov.ReleaseDate = dtReleaseDate;
            }
            else
            {
                readyToAdd = false;
            }
            MovieGenre movGenre;
            if (genre != null && Enum.TryParse(genre, true, out movGenre))
            {
                mov.Genre = movGenre;
            }
            else
            {
                readyToAdd = false;
            }
            var mm = new MovieManager();
            if (actors != null)
            {
                mov.Actors = mm.ExtractActors(actors);
            }
            else
            {
                mov.Actors = mm.GetActors();
            }
            if (readyToAdd && mov.Title != null && mov.Description != null)
            {
                int movieId = mm.Add(mov);
                mm.AddCover(movieId, Request.Files["cover"]);
                return RedirectToAction("Details", new {movieId = movieId});
            }
            return View(model: mov);
        }

        [Authorize]
        public ActionResult Show(string sort, bool? ascending)
        {
            ViewBag.MainMenu = MainMenuItems.Movies;
            SortingOptions sortOps;
            if (sort != string.Empty && Enum.TryParse(sort, true, out sortOps))
            {
                if (ascending.HasValue && !ascending.Value)
                {
                    return View(model: new MovieManager().GetAll(sortOps, false));
                }
                return View(model: new MovieManager().GetAll(sortOps, true));
            }
            return View(model: new MovieManager().GetAll(SortingOptions.No, true));
        }

        [Authorize]
        public ActionResult Details(int movieId)
        {
            ViewBag.MainMenu = MainMenuItems.Movies;
            return View(model: new MovieManager().GetMovie(movieId));
        }

        [Authorize]
        public ActionResult SetMark(int movieId, int mark)
        {
            new RatingManager().SetMark(HttpContext.User.Identity.Name, movieId, mark);
            return RedirectToAction("Details", new {movieId = movieId});
        }

        [Authorize]
        public ActionResult SetMarkAjax(int movieId, int mark)
        {
            new RatingManager().SetMark(HttpContext.User.Identity.Name, movieId, mark);
            return Json(new MovieManager().GetMovie(movieId), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult AddActorAjax(string name, string surname)
        {
            var result = new ActorAdding();
            if (name != null && surname != null)
            {
                result.Actor.Name = name;
                result.Actor.Surname = surname;
                var mm = new MovieManager();
                if (mm.ActorExists(name, surname))
                {
                    result.Result = ActorAddingResult.AlreadyExists;
                }
                else
                {
                    mm.AddActor(result.Actor);
                    result.Result = ActorAddingResult.Success;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult AddActorsToMovie(int movieId, string actors)
        {
            ViewBag.MainMenu = MainMenuItems.Movies;
            if (actors != null)
            {
                var mm = new MovieManager();
                mm.AddMovieActors(movieId, mm.ExtractActors(actors));
                return RedirectToAction("Details", new {movieId = movieId});
            }
            var model = new Movie();
            model.MovieId = movieId;
            model.Actors = new MovieManager().GetNotAddedActors(movieId);
            return View(model);
        }

        [Authorize]
        public ActionResult GetAllActors()
        {
            return Json(new MovieManager().GetActors(), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetActorsExcluding(int movieId)
        {
            return Json(new MovieManager().GetActorsExcluding(movieId), JsonRequestBehavior.AllowGet);
        }
    }
}
