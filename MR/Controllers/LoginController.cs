using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using OtherClasses.Models;
using System.Data.SqlClient;
using msfunc;
using GLS;
using SCS.DataAccess;
using HPL.BissClass;
using OtherClasses;

namespace MR.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index(MR_DATA.MRSTLEV loginData)
        {
            //if cookie exists-delete it
            HttpCookie myCookie = new HttpCookie("mrCooks");
            myCookie = Request.Cookies["mrCooks"];
            if (myCookie != null)
            {
                Response.Cookies["mrCooks"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["mrName"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["mrTime"].Expires = DateTime.Now.AddDays(-1);
            }

            return View();
            //  else -check if new user
            //       -compare with db and allow access
            //       -refuse access
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IndexLogin(MR_DATA.MRSTLEV loginData)
        {
            MR_DATA.MRSTLEV rowRet = ErpFunc.RGet<MR_DATA.MRSTLEV>("MR_DATA", "SELECT * FROM MRSTLEV WHERE OPERATOR = @p1 ",
                loginData.OPERATOR.ToString());

            if (rowRet != null)
            {
                if (string.IsNullOrWhiteSpace(rowRet.PASSWORD))
                {
                    string newUserDataPswrd = Dataaccess.EncryptString(loginData.PASSWORD.ToString());
                    ErpFunc.RwAlter("MR_DATA", "Update MRSTLEV SET PASSWORD = @p1" +
                        " where OPERATOR = @p2", newUserDataPswrd, loginData.OPERATOR.ToString());

                    Response.Cookies["mrCooks"].Value = newUserDataPswrd;
                    Response.Cookies["mrName"].Value = rowRet.OPERATOR;
                    Response.Cookies["mrTime"].Value = DateTime.Now.ToString();

                    if (Request.Cookies["mrRefrURL"] != null)
                    {
                        string[] url = Request.Cookies["mrRefrURL"].Value.ToString().Split('/');
                        Response.Cookies["mrRefrURL"].Expires = DateTime.Now.AddDays(-1);
                        return RedirectToAction(url[1], url[0], new { id = url[2].Trim() });
                    }
                    else return RedirectToAction("Index", "Home");
                }
                else
                {
                    if (loginData.PASSWORD == Dataaccess.DecryptString(rowRet.PASSWORD.ToString()))
                    {
                        Response.Cookies["mrCooks"].Value = rowRet.PASSWORD;
                        Response.Cookies["mrName"].Value = rowRet.OPERATOR;
                        Response.Cookies["mrTime"].Value = DateTime.Now.ToString();

                        if (Request.Cookies["mrRefrURL"] != null)
                        {
                            string[] url = Request.Cookies["mrRefrURL"].Value.ToString().Split('/');
                            Response.Cookies["mrRefrURL"].Expires = DateTime.Now.AddDays(-1);
                            return RedirectToAction(url[1], url[0], new { id = url[2].Trim() });
                        }
                        else return RedirectToAction("Index", "Home");
                    }
                    else {
                        ViewBag.Wrong = "Wrong Username or Password entered.";
                        return View("Index");
                    }
                }
            }
            else {
                ViewBag.Wrong = "Wrong Username or Password entered.";
                return View("Index");
            }
        }

    }
}