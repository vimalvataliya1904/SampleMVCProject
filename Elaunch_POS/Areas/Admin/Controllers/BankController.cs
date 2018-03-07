using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Elaunch_POS_Repository.Data;
using Elaunch_POS_Repository.Service;
using Elaunch_POS_Repository.ServiceContract;
using Elaunch_POS.Models;
using Elaunch_POS_Repository.DataServices;

namespace Elaunch_POS.Areas.Admin.Controllers
{
    public class BankController : Controller
    {
        public IBanks_Repository _IBanks_Repository;

        public BankController()
        {
            this._IBanks_Repository = new Banks_Repository();
        }

        // GET: Admin/Bank
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ManageBank(int id = 0)
        {
            try
            {
                Bank objBank = new Bank();
                if (id > 0)
                {
                    objBank = _IBanks_Repository.GetById(id);
                    ViewBag.Check =  "Edit";
                }
                else
                {
                    ViewBag.Check =  "Add";
                }
                return View(objBank);
            }
            catch (Exception ex)
            {
                ex.SetLog(ex.Message.ToString());
                TempData["errormsg"] = ex.GetErrorMsg();
                return Json(new { success = true, url = Url.Action("Index", "Bank") });
            }
        }


        /// <summary>
        /// INSERT AND UPDATE BANK 
        /// </summary>
        /// <param name="objBank"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ManageBank(Bank objBank)
        {
            try
            {
                if (objBank.Bank_ID == 0)
                {
                    ViewBag.Check =  "Add";
                }
                else
                {
                    ViewBag.Check = "Edit";
                }

                if (ModelState.IsValid)
                {

                    if (!_IBanks_Repository.IsExist(objBank.Bank_Name, objBank.Bank_ID))
                    {
                        if (objBank.Bank_ID == 0)
                        {
                            _IBanks_Repository.Insert(objBank);
                            TempData["successmsg"] ="Successfully Inserted.";
                            return Json(new { success = true, url = Url.Action("Index", "Bank") });
                        }
                        else
                        {
                            _IBanks_Repository.Update(objBank);
                            TempData["successmsg"] = "Successfully Updated.";
                            return Json(new { success = true, url = Url.Action("Index", "Bank") });
                        }
                    }
                    else
                    {
                        TempData["successmsg"] = null;
                        ModelState.AddModelError("Bank_Name", "This bankname is already exist");
                        return PartialView(objBank);
                    }

                }
                else
                {
                    TempData["errormsg"] = null;
                    return PartialView(objBank);
                }
            }
            catch (Exception ex)
            {
                ex.SetLog(ex.Message.ToString());
                TempData["errormsg"] = ex.GetErrorMsg();
                return PartialView(objBank);
            }
        }


        /// <summary>
        /// delete bank
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _IBanks_Repository.Delete(id);
                TempData["successmsg"] = "Bank Deleted";
                return Json(new { success = true, url = Url.Action("Index", "Bank") });
            }
            catch (Exception ex)
            {
                //ex.SetLog(ex.Message.ToString());
                TempData["errormsg"] = "This bank is already in use.";
                return Json(new { success = true, url = Url.Action("Index", "Bank") });
            }
        }

        protected override void Dispose(bool disposing)
        {
            _IBanks_Repository.Dispose();
            base.Dispose(disposing);
        }
    }
}