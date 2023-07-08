using Deblog.Areas.Identity.Data;
using Deblog.Data;
using Deblog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Deblog.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> usermanager;

        public UserController(ApplicationDbContext db, UserManager<ApplicationUser> usermanager)
        {
            _db = db;
            this.usermanager = usermanager;
        }

        public IActionResult Index()
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var username = User.FindFirst(ClaimTypes.Name).Value;
            Userdata dataobj = _db.Userdata.FirstOrDefault(x => x.Id == userid);

            if (dataobj == null)
            {
                Userdata newUser = new Userdata();
                newUser.Id = userid;
                newUser.Username = username;
                newUser.Fullname = username;
                newUser.UserDesc = "Hello World";
                newUser.ImageURL = "/images/userimg.png";

                _db.Userdata.Add(newUser);
                _db.SaveChanges();
            }

            TempData["userimage"] = dataobj.ImageURL;
            TempData["fullname"] = dataobj.Fullname;
            TempData["username"] = dataobj.Username;
            TempData["userdesc"] = dataobj.UserDesc;

            List<Blog> blogList = _db.Blogs.Where(p => p.BlogAuthor == userid).ToList();
            return View(blogList);
        }

        //GET
        public IActionResult Settings()
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Userdata dataobj = _db.Userdata.FirstOrDefault(x => x.Id == userid);

            Userform formobj = new Userform();
            formobj.Id = userid;
            formobj.Fullname = dataobj.Fullname;
            formobj.UserDesc = dataobj.UserDesc;
            TempData["userimage"] = dataobj.ImageURL;
            return View(formobj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Settings(Userform formobj)
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Userdata dataobj = _db.Userdata.FirstOrDefault(x => x.Id == userid);
            if(dataobj == null)
            {
                return RedirectToAction("Index");
            }
            if(ModelState.IsValid)
            {
                dataobj.Id = formobj.Id;
                dataobj.Fullname = formobj.Fullname;
                dataobj.UserDesc = formobj.UserDesc;
                if (formobj.Image != null)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Projectdata/");

                    //create folder if not exist
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);


                    var newfilename = $"UserImage-{formobj.Id}.png";

                    var filePath = Path.Combine(path, newfilename);

                    filePath = filePath.Replace("\\", "/");

                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        formobj.Image.CopyTo(fs);
                    }
                    dataobj.ImageURL = Path.Combine("/images/Projectdata/", newfilename);
                }

                _db.Userdata.Update(dataobj);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(formobj);
        }

		[Authorize]
		public IActionResult YourBlogs()
		{
			var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
			List<Blog> blogList = _db.Blogs.Where(p => p.BlogAuthor == userid).ToList();
			return View(blogList);
		}
	}
}
