using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace webappmvctry.Controllers
{
    [SessionAuthorize]
    public class InstituteProfileController : Controller
    {
        // GET: InstituteProfile
        public ActionResult Index()
        {
            var objUserProfile = DBOperations<InstituteProfile>.GetSpecific(new InstituteProfile() { Opmode = 1, UdiseCode = GlobalSettings.oUserMaster.UserId }, Constant.usp_InstituteProfile);

            var objReqhdr = new Requisition();
            objReqhdr = DBOperations<Requisition>.GetSpecific(new Requisition() { Opmode = 7, UdiseCode = GlobalSettings.oUserMaster.UserId }, Constant.usp_RequisitionView);

            ViewBag.REQCOUNT = objReqhdr.Id;
            return View(objUserProfile);
        }

        [HttpPost]

        public ActionResult UpdateInstituteProfile(InstituteProfile jsonData)
        {
            int result = default(int);
            string msg = string.Empty;
            try
            {
                jsonData.Opmode = 2;
                jsonData.UdiseCode = GlobalSettings.oUserMaster.UserId;
                result = DBOperations<InstituteProfile>.DMLOperation(jsonData, Constant.usp_InstituteProfile);
                GlobalSettings.oUserMaster.IsActive = jsonData.IsActive;
                GlobalSettings.oUserMaster.SchoolMedium = jsonData.SchoolMedium;

                if (result > 0)
                {
                    msg = "Profile updated successfully";
                }
                else
                {
                    msg = "Data saving failed";
                }

                return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Utility.SaveSystemErrorLog(ex);
                return Json(new { Success = 0, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}