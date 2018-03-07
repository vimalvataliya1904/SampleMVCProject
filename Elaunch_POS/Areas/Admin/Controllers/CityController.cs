using Elaunch_POS.Models;
using Elaunch_POS_Repository.Data;
using Elaunch_POS_Repository.DataServices;
using Elaunch_POS_Repository.Service;
using Elaunch_POS_Repository.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Elaunch_POS.Areas.Admin.Controllers
{
    public class CityController : Controller
    {
        private ICities_Repository _ICities_Repository;

        public CityController()
        {
            this._ICities_Repository = new Cities_Repository();
        }

        // GET: Admin/City
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ManageCity(int id = 0)
        {
            try
            {
                City objCity = new City();
                if (id > 0)
                {
                    objCity = _ICities_Repository.GetById(id);
                    ViewBag.Check =  "Edit";
                }
                else
                {
                    ViewBag.Check =  "Add";
                }
                return View(objCity);
            }
            catch (Exception ex)
            {
                ex.SetLog(ex.Message.ToString());
                TempData["errormsg"] = ex.GetErrorMsg();
                return Json(new { success = true, url = Url.Action("Index", "City") });
            }
        }

        /// <summary>
        /// INSERT AND UPDATE CITY
        /// </summary>
        /// <param name="objCity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ManageCity(City objCity)
        {
            try
            {
                if (objCity.City_ID == 0)
                {
                    ViewBag.Check ="Add";
                }
                else
                {
                    ViewBag.Check ="Edit";
                }

                if (ModelState.IsValid)
                {

                    if (!_ICities_Repository.IsExist(objCity.City_Name, objCity.City_ID))
                    {
                        if (objCity.City_ID == 0)
                        {
                            _ICities_Repository.Insert(objCity);
                            TempData["successmsg"] =  "Successfully Inserted.";
                        }
                        else
                        {
                            _ICities_Repository.Update(objCity);
                            TempData["successmsg"] = "Successfully Updated.";
                        }
                        return Json(new { success = true, url = Url.Action("Index", "City") });
                    }
                    else
                    {
                        TempData["successmsg"] = "This city is already exist";
                        ModelState.AddModelError("City_Name", "This city is already exist");
                        return PartialView(objCity);
                    }
                }
                else
                {
                    TempData["errormsg"] = null;
                    return PartialView(objCity);
                }
            }
            catch (Exception ex)
            {
                ex.SetLog(ex.Message.ToString());
                TempData["errormsg"] = ex.GetErrorMsg();
                return PartialView(objCity);
            }
        }

        /// <summary>
        /// DELETE CITY
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _ICities_Repository.Delete(id);
                TempData["successmsg"] = "City Deleted";
                return Json(new { success = true, url = Url.Action("Index", "City") });
            }
            catch (Exception ex)
            {
                //ex.SetLog(ex.Message.ToString());
                TempData["errormsg"] = "This city is in use.";
                return Json(new { success = true, url = Url.Action("Index", "City") });
            }
        }

        protected override void Dispose(bool disposing)
        {
            _ICities_Repository.Dispose();
            base.Dispose(disposing);
        }
    }
}