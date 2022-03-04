using FGPortal.Models;
using FGPortal.Services;
using Microsoft.AspNetCore.Mvc;

namespace FGPortal.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInternetUserRepository _userRepo;
        public UserController(IUnitOfWork unitOfWork, IInternetUserRepository userRepo) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userRepo = userRepo;
        }

        public ActionResult Index()
        {
            //if (base.IsLoggedIn)
            //{
            //    return RedirectToAction("Index", "App");
            //}
            return RedirectToAction("Signin");
        }



        //public ActionResult Reset()
        //{
        //    if (base.IsLoggedIn)
        //    {
        //        return RedirectToAction("Index", "App");
        //    }
        //    return View(new PasswordResetModel());
        //}

        //public ActionResult ResetPassword(string vToken)
        //{
        //    if (!string.IsNullOrWhiteSpace(vToken))
        //    {
        //        string email = vToken.Decrypt().Split('_')[0];
        //        string token = vToken.Decrypt().Split('_')[1];
        //        DateTime value = DateTime.Parse(vToken.Decrypt().Split('_')[2]);
        //        if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(token) && DateTime.UtcNow.Subtract(value).TotalMinutes <= 60.0)
        //        {
        //            using AppDbContext courierConnectEntities = new AppDbContext();
        //            InternetUser internetUser = courierConnectEntities.InternetUser.SingleOrDefault((InternetUser x) => x.Email == email && x.Token == token);
        //            if (internetUser != null)
        //            {
        //                return View(new PasswordResetModel
        //                {
        //                    PasswordResetToken = vToken
        //                });
        //            }
        //        }
        //    }
        //    return RedirectToAction("Index");
        //}

        //public ActionResult Signout(string RedirectTo)
        //{
        //    base.Session.Clear();
        //    base.Session.Abandon();
        //    string[] allKeys = base.Request.Cookies.AllKeys;
        //    foreach (string name in allKeys)
        //    {
        //        HttpCookie httpCookie = base.Response.Cookies[name];
        //        if (httpCookie != null)
        //        {
        //            httpCookie.Expires = DateTime.Now.AddDays(-1.0);
        //        }
        //    }
        //    if (!string.IsNullOrWhiteSpace(RedirectTo))
        //    {
        //        return RedirectToAction("Signin", new { RedirectTo });
        //    }
        //    return RedirectToAction("Index", "App");
        //}

        //[MemberCheck]
        public ActionResult PostLogin()
        {
            return View();
        }

        public ActionResult Signin(string RedirectTo)
        {
            if (base.IsLoggedIn)
            {
                return RedirectToAction("Index", "App");
            }
            AuthenticationViewModel model = new AuthenticationViewModel
            {
                RedirectTo = RedirectTo
            };
            base.ViewBag.PortalAnnouncements = new List<PortalAnnouncement>(); //PortalAnnouncements();
            return View(model);
        }

        [HttpPost]
        public ActionResult Signin(AuthenticationViewModel model)
        {
            if (base.IsLoggedIn)
            {
                return RedirectToAction("Index", "App");
            }

            string text = "";
            string text2 = "warning";
            if (base.ModelState.IsValid || true)
            {
                string password = model.Password.ToMD5();
                var user = _unitOfWork.InternetUsers.Query().FirstOrDefault(x => x.Username.ToLower() == model.Username.ToLower() && x.Password.ToLower() == password);
                if (user != null)
                {
                    if (user.Active)
                    {
                        base.Response.SetCookie("token", user.Token);
                        HttpContext.Items["token"] = user.Token;
                        //base.Response.SetCookie(cookie);
                        if (!string.IsNullOrEmpty(model.RedirectTo) && model.RedirectTo != "/")
                        {
                            base.Response.Redirect(model.RedirectTo);
                            return null;
                        }
                        return RedirectToAction("PostLogin", "User");
                    }
                    text2 = "danger";
                    text = "Signin-Disabled";
                }
                else
                {
                    text2 = "danger";
                    text = "Signin-InvalidCred";
                }
                return RedirectToAction("Signin", new
                {
                    responseCode = text,
                    responseType = text2
                });
            }
            return View("Signin", model);
        }

        //[HttpPost]
        //public ActionResult SendResetEmail(PasswordResetModel model)
        //{
        //    if (base.ModelState.IsValid)
        //    {
        //        using AppDbContext courierConnectEntities = new AppDbContext();
        //        InternetUser internetUser = courierConnectEntities.InternetUser.SingleOrDefault((InternetUser x) => x.Username == model.Username);
        //        if (internetUser != null && !string.IsNullOrWhiteSpace(internetUser.Email))
        //        {
        //            string vToken = $"{internetUser.Email}_{internetUser.Token}_{DateTime.UtcNow}".Encrypt();
        //            string subject = "CourierConnect - Reset your password";
        //            string text = "Dear customer,";
        //            text += "<br /><br />";
        //            text += "We are sending you this email because our records shows that you've asked us to help reset your password.";
        //            text += "<br /><br />";
        //            text += string.Format("Please use this <a href='{0}{1}'>LINK</a> to reset your password. (Link expires in 1 hour)", base.BaseUrl, base.Url.Action("ResetPassword", "User", new { vToken }));
        //            text += "<br /><br />";
        //            text += "<strong>*** Please ignore this email if you haven't requested to have your password reset. ***</strong>";
        //            text += "<br /><br />";
        //            text += "Best Regards,";
        //            text += "<br />";
        //            text += "CourierConnect Team";
        //            EmailHelper.SendEmail(internetUser.Email, subject, text);
        //            model = new PasswordResetModel
        //            {
        //                EmailSent = true
        //            };
        //        }
        //    }
        //    return View("Reset", model);
        //}

        //[HttpPost]
        //public ActionResult ResetPassword(PasswordResetModel model)
        //{
        //    if (base.ModelState.IsValid)
        //    {
        //        string email = model.PasswordResetToken.Decrypt().Split('_')[0];
        //        string token = model.PasswordResetToken.Decrypt().Split('_')[1];
        //        DateTime value = DateTime.Parse(model.PasswordResetToken.Decrypt().Split('_')[2]);
        //        if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(token) && DateTime.UtcNow.Subtract(value).TotalMinutes <= 60.0)
        //        {
        //            using AppDbContext courierConnectEntities = new AppDbContext();
        //            InternetUser internetUser = courierConnectEntities.InternetUser.SingleOrDefault((InternetUser x) => x.Email == email && x.Token == token);
        //            if (internetUser != null)
        //            {
        //                internetUser.Password = model.Password.ToMD5();
        //                internetUser.Token = TextHelper.newGUID();
        //                courierConnectEntities.SaveChanges();
        //                model = new PasswordResetModel
        //                {
        //                    PasswordChanged = true
        //                };
        //            }
        //        }
        //    }
        //    return View("ResetPassword", model);
        //}

        //[MemberCheck]
        //public ActionResult ChangePassword()
        //{
        //    return View();
        //}

        //[MemberCheck]
        //[HttpPost]
        //public ActionResult ChangePassword(AuthenticationViewModel model)
        //{
        //    if (base.ModelState.IsValid)
        //    {
        //        string responseCode = "ok";
        //        string responseType = "success";
        //        using AppDbContext courierConnectEntities = new AppDbContext();
        //        InternetUser internetUser = courierConnectEntities.InternetUser.SingleOrDefault((InternetUser x) => x.ID == UserId);
        //        if (internetUser != null)
        //        {
        //            internetUser.Password = model.Password.ToMD5();
        //            courierConnectEntities.SaveChanges();
        //            return RedirectToAction("ChangePassword", new { responseCode, responseType });
        //        }
        //    }
        //    return View("ChangePassword", model);
        //}

        private List<string> PortalAnnouncements()
        {
            using AppDbContext courierConnectEntities = new AppDbContext();
            return (from x in courierConnectEntities.PortalAnnouncement
                    where x.StartTime <= DateTime.Now && x.EndTime >= DateTime.Now
                    select x.Message).ToList();
        }
    }
}
