using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hospital_Test.DAO;
using Hospital_Test.Models;
using Hospital_Test.Models.Login;

namespace Hospital_Test.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult ChangeToLoginIndex() //Action đệm, tránh HttpPost
        {

            return RedirectToAction("Index", "Login");
        }
        [HttpGet]
        public IActionResult Index()
        {
            Account account = new Account();

            account.Username = TempData["AccName"] == null ? null : TempData["AccName"].ToString(); // Lưu thông tin người dùng nhập
            TempData["AccName"] = null;
            account.Password = TempData["Password"] == null ? null : TempData["Password"].ToString();
            TempData["Password"] = null;
            String loginSubmit = TempData["LoginSubmit"] == null ? "0" : TempData["LoginSubmit"].ToString();


            if (loginSubmit == "1")
            {
                if (account.Username == null) // Chưa nhập tên đăng nhập
                {
                    TempData["msg"] = "Vui lòng nhập tên đăng nhập!";
                    return View("./Views/Shared/Login/Index.cshtml", account);
                }

                Account user = AccountDAO.Instance.GetAccountbyUsername(account.Username);

                if (user == null && loginSubmit == "1")  // Không tìm thấy tên đăng nhập trong database
                {
                    TempData["msg"] = "Tài khoản không tồn tại!";
                    return View("./Views/Shared/Login/Index.cshtml", account);
                }

                // Tìm thấy tên đăng nhập trong database
                if (account.Password == null) // Chưa nhập mật khẩu
                {
                    TempData["msg"] = "Vui lòng nhập mật khẩu";
                    return View("./Views/Shared/Login/Index.cshtml", account);
                }

                else if (account.Password != user.Password) // Nhập sai mật khẩu
                {
                    TempData["msg"] = "Bạn nhập sai mật khẩu!";
                    return View("./Views/Shared/Login/Index.cshtml", account);
                }

                else // Đúng mật khẩu và chuyển hướng loại tài khoản
                {
                    return RedirectToAction("Index", "Management");
                }

            }
            return View("./Views/Shared/Login/Index.cshtml", account);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Account input)
        {
            TempData["AccName"] = input.Username;
            TempData["Password"] = input.Password;
            TempData["LoginSubmit"] = "1";
            return RedirectToAction("Index", "Login");
        }
    }
}
        