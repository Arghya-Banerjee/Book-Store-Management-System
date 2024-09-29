using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tutorial1.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Validate(string username, string password)
        {
            try
            {

                Login obj = new Login();
                obj.Opmode = 0;
                obj.UserId = username;
                obj.UserPassword = password;
                var objLogin = DBOperations<Login>.GetSpecific(obj, Constant.usp_Login);

                UserSec objsession = new UserSec();

                if (objLogin != null && objLogin.UserPassword == obj.UserPassword)
                {
                    var objAccYear = DBOperations<AcademicYear>.GetSpecific(new AcademicYear() { Opmode = 0 }, Constant.usp_AcademicYear);

                    objsession.Id = objLogin.Id;
                    objsession.UserId = objLogin.UserId;
                    objsession.RefId = objLogin.RefId;
                    objsession.IsActive = objLogin.IsActive;
                    objsession.UserType = objLogin.UserType;
                    objsession.ReqActive = objLogin.ReqActive;
                    objsession.ACAD_ID = objAccYear.ACAD_ID;
                    // added 13.9.24
                    objsession.ACAD_YEAR = objAccYear.ACAD_YEAR;

                    GlobalSettings.oUserMaster = objsession;
                    if (objLogin.UserType == 1) // school user
                    {
                        GlobalSettings.oUserMaster.vUserRole = UserRole.SCHOOL;
                        var objUserProfile = DBOperations<InstituteProfile>.GetSpecific(new InstituteProfile() { Opmode = 1, UdiseCode = GlobalSettings.oUserMaster.UserId }, Constant.usp_InstituteProfile);
                        GlobalSettings.oUserMaster.SchoolMedium = objUserProfile.SchoolMedium;
                        //var chkreqexist = DBOperations<Requisition>.GetSpecific(new Requisition() { Opmode = 7, UdiseCode = GlobalSettings.oUserMaster.UserId }, Constant.usp_RequisitionView);
                        //if(chkreqexist.Id>0)
                        //{
                        //    GlobalSettings.oUserMaster.CategoryId = chkreqexist.CategoryId;
                        //    GlobalSettings.oUserMaster.SubCategoryId = chkreqexist.SubCategoryId;
                        //}
                    }
                    if (objLogin.UserType == 2) // directorate user
                    {
                        GlobalSettings.oUserMaster.vUserRole = UserRole.DIRECTORATE;
                    }
                    if (objLogin.UserType == 3) // TB user
                    {
                        GlobalSettings.oUserMaster.vUserRole = UserRole.TB;
                    }
                    if (objLogin.UserType == 4) // ADMIN user
                    {
                        GlobalSettings.oUserMaster.vUserRole = UserRole.ADMIN;
                    }
                }
                else
                {
                    throw new Exception("Invalid login credential!");
                }

                return Json(new { Success = 1, UserType = objLogin.UserType, Message = "Login Successfull" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var ERRUSERID = "TRY LOGIN FROM - " + username;
                Utility.SaveSystemErrorLog(ex, ERRUSERID);
                return Json(new { Success = 0, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AllowAnonymous]

        public ActionResult Logout()
        {
            GlobalSettings.oUserMaster = (UserSec)null;
            this.Session.Clear();
            this.Session.Abandon();
            this.Session.RemoveAll();
            return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public ActionResult GetAllDistrict()
        {

            List<DistrictMaster> lst = new List<DistrictMaster>();
            try
            {

                lst = DBOperations<DistrictMaster>.GetAllOrByRange(new DistrictMaster() { Opmode = 0 }, Constant.usp_District);


                if (lst != null && lst.Count() > default(int))
                {

                    return Json(new { Success = 1, Message = lst }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    throw new Exception("No district found");
                }
            }
            catch (Exception ex)
            {
                var ERRUSERID = "USER ID - " + GlobalSettings.oUserMaster.UserId;
                Utility.SaveSystemErrorLog(ex, ERRUSERID);
                return Json(new { Success = default(int), Message = ex.Message }, JsonRequestBehavior.AllowGet);

            }

        }

        [HttpGet]
        public ActionResult GetAllBlock(int districtid)
        {

            List<BlockMaster> lst = new List<BlockMaster>();
            try
            {

                lst = DBOperations<BlockMaster>.GetAllOrByRange(new BlockMaster() { Opmode = 0, DistrictId = districtid }, Constant.usp_Block);


                if (lst != null && lst.Count() > default(int))
                {

                    return Json(new { Success = 1, Message = lst }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    throw new Exception("No block found");
                }
            }
            catch (Exception ex)
            {
                var ERRUSERID = "USER ID - " + GlobalSettings.oUserMaster.UserId;
                Utility.SaveSystemErrorLog(ex, ERRUSERID);
                return Json(new { Success = default(int), Message = ex.Message }, JsonRequestBehavior.AllowGet);

            }

        }

        [HttpGet]
        public ActionResult GetLoginOTP(string username)
        {
            try
            {
                var objuserDtl = DBOperations<Login>.GetSpecific(new Login() { Opmode = 1, UserId = username }, Constant.usp_Login);
                var Mobile = string.Empty;
                var Email = string.Empty;
                var OTP = string.Empty;
                var SendMsg = string.Empty;
                int result = default(int);

                if (objuserDtl != null && objuserDtl.Id > default(long))
                {
                    if (objuserDtl.MobileNo != null) // CIRCLE 
                    {
                        Mobile = objuserDtl.MobileNo.ToString();
                    }

                    else
                    {
                        throw new Exception("Mobile number not found");
                    }

                    if (!string.IsNullOrWhiteSpace(objuserDtl.Email))
                    {
                        Email = objuserDtl.Email.Trim();
                    }

                    else
                    {
                        Email = "";
                    }

                    OTP = new Random().Next(10000, 99999).ToString();
                    result = DBOperations<Login>.DMLOperation(new Login() { Opmode = 3, UserId = username, UserPassword = OTP }, Constant.usp_Login);
                    if (result > default(int))
                    {
                        var MailSubject = "Login OTP for MADRASAH Web Portal";
                        var MailContent = "Your one time password is " + OTP + ". Please use the OTP to login in MADRASAH Web Portal. WB Textbook Corporation";
                        if (Email.Trim() != "")
                        {
                            //Utility.SendHtmlFormattedEmail(Email.Trim(), MailSubject, MailContent, false, "");
                        }
                        Utility.SendSMS(Mobile.Trim(), MailContent, "1107161019014449062");

                        SendMsg = string.Concat("".PadLeft(6, '*'), Mobile.Substring(Mobile.Length - 4));


                        return Json(new { Success = 1, Message = "OTP Sent to " + SendMsg, Username = username }, JsonRequestBehavior.AllowGet);
                    }

                    else
                    {
                        throw new Exception("OTP Failed");
                    }
                }
                else
                {
                    throw new Exception("User doesn't exist!");
                }


            }
            catch (Exception ex)
            {
                var ERRUSERID = "USER ID - " + GlobalSettings.oUserMaster.UserId;
                Utility.SaveSystemErrorLog(ex, ERRUSERID);
                return Json(new { Success = 0, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        }
    }