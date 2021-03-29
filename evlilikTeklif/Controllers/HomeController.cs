using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using evlilikTeklif.Models;
using Microsoft.AspNetCore.Identity;

namespace evlilikTeklif.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;


        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {

            using (var context = new evlilikTeklifContext())
            {
                QuestionModel questionsListView = new QuestionModel()
                {
                    questions = context.Questions.ToList()
                };
                return View(questionsListView);
            }

        }


        [HttpPost]
        public string Answerquestion(AnswerModel model)
        {
            if (model.Answer != null)
            {

                using (var context = new evlilikTeklifContext())
                {

                    var x = context.Questions.Where(i => i.number == model.QuestionNumber && i.answer == model.Answer.ToUpper()).FirstOrDefault();
                    if (x != null)
                    {
                        x.isAnswered = true;
                        context.Entry(x).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        context.SaveChanges();
                        return "success";
                    }
                    else
                        throw new Exception("Yanlış cevap");
                }
            }
            throw new Exception("Yanlış cevap");
        }

        [HttpGet]
        public IActionResult Login()
        {

            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı adı Bulunamadı");
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Kullanıcı adı veya parola yanlış");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {

            await _signInManager.SignOutAsync();
            ModelState.AddModelError("", "Çıkış Yapıldı");
            return Redirect("~/");

        }
        [HttpGet]
        public IActionResult list()
        {
            using (var context = new evlilikTeklifContext())
            {

                var x = context.Questions.ToList();
                return View(x);
            }


        }
        [HttpPost]
        public string refreshValue()
        {
            using (var context = new evlilikTeklifContext())
            {

                var x = context.Questions.Where(i => i.isAnswered == true).ToList();
                if (x.Count() > 0)
                {
                    foreach (var item in x)
                    {
                        item.isAnswered = false;
                        context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    }
                  
                    context.SaveChanges();
                }
                return "success";
            }


        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
