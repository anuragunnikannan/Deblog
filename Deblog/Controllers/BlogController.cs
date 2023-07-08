using Deblog.Areas.Identity.Data;
using Deblog.Data;
using Deblog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Security.Claims;

namespace Deblog.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> usermanager;

        public BlogController(ApplicationDbContext db, UserManager<ApplicationUser> usermanager)
        {
            _db = db;
            this.usermanager = usermanager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult AddBlog()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult AddBlog(Blog obj)
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            obj.BlogAuthor = userid;
            obj.BlogBody = obj.BlogTitle;
            obj.BlogDateTime = DateTime.Now;
            obj.BlogStatus = "draft";

            if(ModelState.IsValid)
            {
                _db.Blogs.Add(obj);
                _db.SaveChanges();

                return RedirectToAction("WriteBlog", "Blog", new {id = obj.BlogId});
            }
            return View();
        }

        [Authorize]
        public IActionResult EditBlog(int id)
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Blog obj = _db.Blogs.FirstOrDefault(p => p.BlogId == id);
            if(obj == null)
            {
                return NotFound("This id does not exist.");
            }
            if(obj.BlogAuthor != userid)
            {
                return NotFound("You are not authorized.");
            }
            return View(obj);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditBlog(Blog obj)
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (ModelState.IsValid && obj.BlogAuthor == userid)
            {
                obj.BlogDateTime = DateTime.Now;
                _db.Blogs.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index", "User");
            }
            return View(obj);
        }

        [Authorize]
        public IActionResult DeleteBlog(int id)
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Blog obj = _db.Blogs.FirstOrDefault(p => p.BlogId == id);
            if (obj == null)
            {
                return NotFound("This id does not exist.");
            }
            if (obj.BlogAuthor != userid)
            {
                return NotFound("You are not authorized.");
            }
            _db.Blogs.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index", "User");
        }

        public IActionResult ViewBlog(int id)
        {
            Blog obj = _db.Blogs.FirstOrDefault(p => p.BlogId == id);
            Userdata userobj = _db.Userdata.FirstOrDefault(x => x.Id == obj.BlogAuthor);
            Tuple<Blog, string> data = new Tuple<Blog, string>(obj, userobj.Fullname);

            if (User.Identity.IsAuthenticated)
            {
                var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Bookmark bookmarkobj = _db.Bookmarks.FirstOrDefault(p => p.Id == userid && p.BlogId == id);
                if (bookmarkobj == null)
                {
                    TempData["bookmarkstatus"] = "false";
                }
                else
                {
                    TempData["bookmarkstatus"] = "true";
                }
            }
			return View(data);
        }

        [Authorize]
        public IActionResult WriteBlog(int id)
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            Blog obj = _db.Blogs.FirstOrDefault(p => p.BlogId == id);

            if(obj == null)
            {
				return NotFound("This id does not exist.");
			}

			if (obj.BlogAuthor != userid)
			{
				return NotFound("You are not authorized.");
			}

            BlogContent content = new BlogContent();
            content.BlogId = obj.BlogId;
            content.BlogAuthor = obj.BlogAuthor;
            content.BlogBody = obj.BlogBody;
            content.BlogStatus = obj.BlogStatus;

            return View(content);
		}

        [Authorize]
        [HttpPost]
        
        public IActionResult WriteBlog(BlogContent content)
        {
			var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Blog obj = _db.Blogs.FirstOrDefault(p => p.BlogId == content.BlogId);
            Hashtable response;
            if(ModelState.IsValid && obj.BlogAuthor == userid && content.BlogAuthor == userid)
            {
                obj.BlogDateTime = DateTime.Now;
                obj.BlogBody = content.BlogBody;
                obj.BlogStatus = content.BlogStatus;

                _db.Blogs.Update(obj);
                _db.SaveChanges();

                response = new Hashtable()
                {
                    {"Status", 200}
                };
                return Json(response);
            }
            else
            {
                response = new Hashtable()
                {
                    {"Status", 404 }
                };
                return Json(response);
            }
		}

        [Authorize]
        public IActionResult BookmarkBlog(int BlogId)
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
			Hashtable response;
			Blog blogobj = _db.Blogs.FirstOrDefault(p => p.BlogId == BlogId);
            if(blogobj == null)
            {
				response = new Hashtable()
				{
					{"Status", 404 }
				};
				return Json(response);
			}
            Bookmark obj = _db.Bookmarks.FirstOrDefault(p => p.Id == userid && p.BlogId == BlogId);
			if (obj == null)
            {
                Bookmark obj1 = new Bookmark();
                obj1.Id = userid;
                obj1.BlogId = BlogId;
                _db.Bookmarks.Add(obj1);
                _db.SaveChanges();
                
				response = new Hashtable()
				{
					{"BookmarkState", true},
                    {"Status", 200 }
				};
				return Json(response);

			}
            else
            {
                _db.Bookmarks.Remove(obj);
                _db.SaveChanges();
                TempData["bookmarkstatus"] = "false";
				response = new Hashtable()
				{
					{"BookmarkState", false},
					{"Status", 200 }
				};
				return Json(response);
			}
		}

        [Authorize]
        public IActionResult Bookmarked()
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<int> BookmarkList = _db.Bookmarks.Where(p => p.Id == userid).Select(p => p.BlogId).ToList();
            List<Blog> BlogList = _db.Blogs.ToList();
            List<Blog> bookmarked = new List<Blog>();
            foreach(var obj in BlogList)
            {
                if(BookmarkList.Contains(obj.BlogId))
                {
                    bookmarked.Add(obj);
                }
            }
            return View(bookmarked);
        }
    }
}
