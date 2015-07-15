﻿using System;
using System.Linq;
using System.Web.Mvc;
using BL.Discussions;
using Bootstrapper;
using UI.WebApp.Helpers;

namespace UI.WebApp.Controllers
{
    [Authorize(Roles = UserRole.ReadForum)]
    public class DiscussionsController : BaseController
    {
        public ActionResult ShowAll()
        {
            ViewBag.MenuItem = MenuItem.Discussions;
            var dm = Loader.GetDiscussionManager();
            return View(dm.Discussions);
        }

        public ActionResult Show(int? id)
        {
            ViewBag.MenuItem = MenuItem.Discussions;
            if (!id.HasValue)
            {
                return RedirectToAction("ShowAll");
            }
            var dm = Loader.GetDiscussionManager();
            var discussion = dm.Discussions.SingleOrDefault(d => d.Id == id);
            return View(discussion);
        }

        public ActionResult AddDiscussion(string name)
        {
            if (name != string.Empty)
            {
                Loader.GetDiscussionManager().CreateDiscussion(name);
            }
            return RedirectToAction("ShowAll");
        }

        public JsonResult GetAllMessages(int discussionId)
        {
            var dm = Loader.GetDiscussionManager();
            var discussion = dm.Discussions.Single(d => d.Id == discussionId);
            var converter = new Converter();
            var data = discussion.Messages.Select(msg => converter.GetDataObject(msg));
            return new JsonResult {Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet};
        }

        [Authorize(Roles = UserRole.WriteForum)]
        public JsonResult RemoveMessage(int discussionId, int messageId)
        {
            var dm = Loader.GetDiscussionManager();
            var discussion = dm.Discussions.Single(d => d.Id == discussionId);
            var msg = discussion.Messages.Single(m => m.Id == messageId);
            if (msg.Author.Id != User.Id)
            {
                return new JsonResult {Data = new {result = "Access denied!"}};
            }
            discussion.Messages.Remove(msg);
            dm.Update();

            return new JsonResult {Data = new {result = "success"}};
        }

        [Authorize(Roles = UserRole.WriteForum)]
        public JsonResult SendMessage(int discussionId, string message)
        {
            var dm = Loader.GetDiscussionManager();
            var um = Loader.GetUserManager();
            var user = um.Users.Single(u => u.Id == User.Id);
            var discussion = dm.Discussions.Single(d => d.Id == discussionId);

            var msg = new Message {Author = user, Body = message, Date = DateTime.Now};
            discussion.Messages.Add(msg);
            dm.Update();

            var msgData = new Converter().GetDataObject(msg);
            return new JsonResult
            {
                Data = new {result = "success", message = msgData}
            };
        }
    }
}