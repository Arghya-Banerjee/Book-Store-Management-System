using Core;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace webappmvctry.Controllers
{
    [SessionAuthorize]
    public class RequisitionController : Controller
    {
        // GET: Requisition
        public ActionResult GetTotalRecordCount(int firstload = 1, int pageNo = 1)
        {

            List<Requisition> lst = new List<Requisition>();
            string sortExpression = string.Empty;
            string conditionExpression = string.Empty;
            long TotalRecords = default(long);
            sortExpression = " R.Id DESC ";
            conditionExpression = " R.UdiseCode = " + GlobalSettings.oUserMaster.UserId;


            try
            {
                var tlst = DBOperations<Requisition>.GetAllOrByRange(new Requisition()
                {
                    strSortFields = sortExpression,
                    strCondition = conditionExpression,
                    Opmode = 1
                }, Constant.usp_RequisitionView);
                if (tlst != null && tlst.Count() > default(int) && tlst[0].TotalRecords > default(long))
                {
                    TotalRecords = tlst[0].TotalRecords;
                }

            }
            catch (Exception ex)
            {
                var ERRUSERID = "USER ID - " + GlobalSettings.oUserMaster.UserId;
                Utility.SaveSystemErrorLog(ex, ERRUSERID);
            }
            return Json(new { Success = 1, Message = TotalRecords }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Index(int firstload = 1, int pageNo = 1)
        {

            //if (GlobalSettings.oUserMaster.IsActive != 1)
            //{
            //    return RedirectToAction("Index", "InstituteProfile", new { area = "" });
            //}

            if (GlobalSettings.oUserMaster.ReqActive <= 0)
            {
                return RedirectToAction("Index", "InstituteProfile", new { area = "" }); // to off requisition
            }


            List<Requisition> lst = new List<Requisition>();
            string sortExpression = string.Empty;
            string conditionExpression = string.Empty;
            long TotalRecords = default(long);
            sortExpression = " R.Id DESC ";
            conditionExpression = " R.UdiseCode = " + GlobalSettings.oUserMaster.UserId;

            try
            {

                var tlst = DBOperations<Requisition>.GetAllOrByRange(new Requisition()
                {
                    strSortFields = sortExpression,
                    strCondition = conditionExpression,
                    Opmode = 1
                }, Constant.usp_RequisitionView);
                if (tlst != null && tlst.Count() > default(int) && tlst[0].TotalRecords > default(long))
                {
                    TotalRecords = tlst[0].TotalRecords;
                }
                ViewBag.TotalRecords = TotalRecords;
                ViewBag.pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["RecordsPerPage"]);
                var page = Utility.PageResults(Convert.ToInt64(TotalRecords), -1, Convert.ToInt32(ConfigurationManager.AppSettings["RecordsPerPage"]), pageNo);
                if (page.Start > 0 && page.End > 0)
                {
                    lst = DBOperations<Requisition>.GetAllOrByRange(new Requisition()
                    {
                        strSortFields = sortExpression,
                        strCondition = conditionExpression,
                        Opmode = 1,
                        RowStartIndex = page.Start,
                        RowEndIndex = page.End
                    }, Constant.usp_RequisitionView);
                }
            }
            catch (Exception ex)
            {
                var ERRUSERID = "USER ID - " + GlobalSettings.oUserMaster.UserId;
                Utility.SaveSystemErrorLog(ex, ERRUSERID);
            }

            var objReqhdr = new Requisition();
            objReqhdr = DBOperations<Requisition>.GetSpecific(new Requisition() { Opmode = 7, UdiseCode = GlobalSettings.oUserMaster.UserId }, Constant.usp_RequisitionView);

            ViewBag.REQCOUNT = objReqhdr.Id;
            if (firstload > default(int))
                return View(lst);
            else
                return PartialView("_RequisitionList", lst);
        }
        [HttpGet]
        public ActionResult AddEdit(string id)
        {
            if (GlobalSettings.oUserMaster.ReqActive <= 0)
            {
                return RedirectToAction("Index", "InstituteProfile", new { area = "" }); // to off requisition
            }

            int reqId = default(int);
            var objReqhdr = new Requisition();
            try
            {
                int.TryParse(Encryption64.DecryptQueryString(id), out reqId);
                if (reqId > default(int))
                {
                    objReqhdr = DBOperations<Requisition>.GetSpecific(new Requisition() { Opmode = 4, Id = reqId }, Constant.usp_RequisitionView);
                }

            }
            catch (Exception ex)
            {
                var ERRUSERID = "USER ID - " + GlobalSettings.oUserMaster.UserId;
                Utility.SaveSystemErrorLog(ex, ERRUSERID);
            }

            var objInsProfile = DBOperations<InstituteProfile>.GetSpecific(new InstituteProfile() { Opmode = 1, UdiseCode = GlobalSettings.oUserMaster.UserId }, Constant.usp_InstituteProfile);
            var objMedium = DBOperations<MediumMaster>.GetSpecific(new MediumMaster() { Opmode = 2, Id = objInsProfile.SchoolMedium }, Constant.usp_Medium);

            ViewBag.REQID = reqId;
            ViewBag.InstituName = objInsProfile.InstituName;
            ViewBag.UdiseCode = objInsProfile.UdiseCode;
            ViewBag.Medium = objMedium.Medium;

            return View(objReqhdr);
        }

        [HttpGet]
        public ActionResult GetCategory()
        {

            List<CategoryMaster> lst = new List<CategoryMaster>();
            try
            {

                lst = DBOperations<CategoryMaster>.GetAllOrByRange(new CategoryMaster() { Opmode = 0 }, Constant.usp_MadrasahCategory);


                if (lst != null && lst.Count() > default(int))
                {

                    return Json(new { Success = 1, Message = lst }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    throw new Exception("No category found");
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
        public ActionResult GetSubCategory(int category)
        {

            List<SubCategoryMaster> lst = new List<SubCategoryMaster>();
            try
            {

                lst = DBOperations<SubCategoryMaster>.GetAllOrByRange(new SubCategoryMaster() { Opmode = 0, CategoryId = category }, Constant.usp_MadrasahSubCategory);


                if (lst != null && lst.Count() > default(int))
                {

                    return Json(new { Success = 1, Message = lst }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    throw new Exception("No sub category found");
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
        public ActionResult GetMedium()
        {

            List<MediumMaster> lst = new List<MediumMaster>();
            try
            {

                lst = DBOperations<MediumMaster>.GetAllOrByRange(new MediumMaster() { Opmode = 0 }, Constant.usp_Medium);


                if (lst != null && lst.Count() > default(int))
                {

                    return Json(new { Success = 1, Message = lst }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    throw new Exception("No medium found");
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
        public ActionResult GetRequisitionMedium(int cat_id = 0) // shoe medium w.r.t scholl profile medium
        {

            List<MediumMaster> lst = new List<MediumMaster>();
            try
            {

                if (cat_id > 0 && (cat_id == 1 || cat_id == 2))
                {
                    lst = DBOperations<MediumMaster>.GetAllOrByRange(new MediumMaster() { Opmode = 1, Id = GlobalSettings.oUserMaster.SchoolMedium }, Constant.usp_Medium);
                }

                else // for XI_XII
                {
                    lst = DBOperations<MediumMaster>.GetAllOrByRange(new MediumMaster() { Opmode = 3 }, Constant.usp_Medium);
                }

                if (lst != null && lst.Count() > default(int))
                {

                    return Json(new { Success = 1, Message = lst }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    throw new Exception("No medium found");
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
        public ActionResult generateItemsummary(int category, int subcategory, int mediumid)
        {

            List<ItemSummary> lst = new List<ItemSummary>();
            try
            {

                lst = DBOperations<ItemSummary>.GetAllOrByRange(new ItemSummary() { Opmode = 0, CategoryId = category, SubcategoryId = subcategory, MediumId = mediumid }, Constant.usp_ItemSummary);


                if (lst != null && lst.Count() > default(int))
                {

                    return Json(new { Success = 1, Message = lst }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    throw new Exception("No item found");
                }
            }
            catch (Exception ex)
            {
                var ERRUSERID = "USER ID - " + GlobalSettings.oUserMaster.UserId;
                Utility.SaveSystemErrorLog(ex, ERRUSERID);
                return Json(new { Success = default(int), Message = ex.Message }, JsonRequestBehavior.AllowGet);

            }

        }

        [HttpPost]

        public ActionResult SaveRequisition(ReqRequest jsonData)
        {
            int result = default(int);
            string msg = string.Empty;
            string strResult = string.Empty;
            Requisition obj = new Requisition();
            obj.UdiseCode = GlobalSettings.oUserMaster.UserId;
            obj.CategoryId = jsonData.CategoryId;
            obj.SubCategoryId = jsonData.SubCategoryId;
            obj.MediumId = jsonData.MediumId;


            obj.CreatedBy = GlobalSettings.oUserMaster.UserId;
            obj.Status = jsonData.Status;
            obj.Opmode = 5;
            obj.xData = Utility.CreateXml(Utility.ToDataTable<RequisitionDetail>(jsonData.lstDtl.ToList())).InnerXml;


            try
            {
                if (obj.MediumId != 4) // not XI-XX
                {
                    if (GlobalSettings.oUserMaster.SchoolMedium != 3)
                    {
                        var chkreqexist = DBOperations<Requisition>.GetSpecific(new Requisition() { Opmode = 9, UdiseCode = GlobalSettings.oUserMaster.UserId, FYId = GlobalSettings.oUserMaster.ACAD_ID }, Constant.usp_RequisitionView);
                        if (chkreqexist.Id > 0)
                        {
                            msg = "Requisition already exist for your Institute. Please check Requisition List";
                            return Json(new { Success = 0, Exist = 1, Message = msg }, JsonRequestBehavior.AllowGet);
                        }

                    }
                    else // for bilingual
                    {
                        //for bi lingual, user cannot submit req of different category and subcategory from previous requisition
                        var chkreqexist = DBOperations<Requisition>.GetSpecific(new Requisition() { Opmode = 7, UdiseCode = GlobalSettings.oUserMaster.UserId, FYId = GlobalSettings.oUserMaster.ACAD_ID }, Constant.usp_RequisitionView);
                        if (chkreqexist.Id > 0)
                        {
                            if (chkreqexist.CategoryId != jsonData.CategoryId)
                            {
                                return Json(new { Success = 0, Message = "You can't give requisition for different category!" }, JsonRequestBehavior.AllowGet);
                            }
                            if (chkreqexist.SubCategoryId != jsonData.SubCategoryId)
                            {
                                return Json(new { Success = 0, Message = "You can't give requisition for different sub category!" }, JsonRequestBehavior.AllowGet);
                            }

                        }


                        var chklangexist = DBOperations<Requisition>.GetSpecific(new Requisition() { Opmode = 8, UdiseCode = GlobalSettings.oUserMaster.UserId, MediumId = jsonData.MediumId, FYId = GlobalSettings.oUserMaster.ACAD_ID }, Constant.usp_RequisitionView);

                        if (chklangexist.Id > default(int))
                        {
                            msg = "Requisition already exist for this Medium. Please check Requisition List ";

                            return Json(new { Success = 0, Exist = 1, Message = msg }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            var chkreqexist1 = DBOperations<Requisition>.GetSpecific(new Requisition() { Opmode = 9, UdiseCode = GlobalSettings.oUserMaster.UserId, FYId = GlobalSettings.oUserMaster.ACAD_ID }, Constant.usp_RequisitionView);
                            if (chkreqexist1.Id == 2)
                            {
                                msg = "2 Requisitions already exist for your institute";
                                return Json(new { Success = 0, Exist = 1, Message = msg }, JsonRequestBehavior.AllowGet);
                            }

                        }

                    }
                }

                else
                {
                    var chklangexist = DBOperations<Requisition>.GetSpecific(new Requisition() { Opmode = 8, UdiseCode = GlobalSettings.oUserMaster.UserId, MediumId = jsonData.MediumId, FYId = GlobalSettings.oUserMaster.ACAD_ID }, Constant.usp_RequisitionView);

                    if (chklangexist.Id > default(int))
                    {
                        msg = "Requisition already exist for this Medium. Please check Requisition List ";

                        return Json(new { Success = 0, Exist = 1, Message = msg }, JsonRequestBehavior.AllowGet);
                    }
                }

                result = DBOperations<Requisition>.DMLOperation(obj, Constant.usp_RequisitionView);

                return Json(new { Success = result, Message = "Requisition saved successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var ERRUSERID = "USER ID - " + GlobalSettings.oUserMaster.UserId;
                Utility.SaveSystemErrorLog(ex, ERRUSERID);
                return Json(new { Success = 0, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public ActionResult getReqDtl(int reqId)
        {

            List<RequisitionDetail> lst = new List<RequisitionDetail>();
            try
            {

                lst = DBOperations<RequisitionDetail>.GetAllOrByRange(new RequisitionDetail() { Opmode = 0, ReqId = reqId }, Constant.usp_TrxRequisitionDtl);


                if (lst != null && lst.Count() > default(int))
                {

                    return Json(new { Success = 1, Message = lst }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    throw new Exception("No detail found");
                }
            }
            catch (Exception ex)
            {
                var ERRUSERID = "USER ID - " + GlobalSettings.oUserMaster.UserId;
                Utility.SaveSystemErrorLog(ex, ERRUSERID);
                return Json(new { Success = default(int), Message = ex.Message }, JsonRequestBehavior.AllowGet);

            }

        }

        [HttpPost]

        public ActionResult UpdateRequisition(ReqRequest jsonData)
        {
            int result = default(int);
            string msg = string.Empty;
            string strResult = string.Empty;
            Requisition obj = new Requisition();
            obj.UdiseCode = GlobalSettings.oUserMaster.UserId;
            obj.CategoryId = jsonData.CategoryId;
            obj.SubCategoryId = jsonData.SubCategoryId;
            obj.MediumId = jsonData.MediumId;

            obj.CreatedBy = GlobalSettings.oUserMaster.UserId;
            // obj.SAVE_STATUS = jsonData.SAVE_STATUS;

            obj.Opmode = 6;
            obj.Id = jsonData.Id;
            obj.xData = Utility.CreateXml(Utility.ToDataTable<RequisitionDetail>(jsonData.lstDtl.ToList())).InnerXml;


            try
            {
                result = DBOperations<Requisition>.DMLOperation(obj, Constant.usp_RequisitionView);

                return Json(new { Success = result, Message = "Requisition updated successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var ERRUSERID = "USER ID - " + GlobalSettings.oUserMaster.UserId;
                Utility.SaveSystemErrorLog(ex, ERRUSERID);
                return Json(new { Success = 0, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            int result = default(int);
            string msg = string.Empty;
            try
            {
                Requisition obj = new Requisition();
                obj.Id = id;
                obj.Opmode = 2;
                result = DBOperations<Requisition>.DMLOperation(obj, Constant.usp_RequisitionView);

                if (result > default(int))
                {
                    msg = "Requisition deleted successfully";
                }
                else
                {
                    msg = "Unable to delete";
                }
                return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var ERRUSERID = "USER ID - " + GlobalSettings.oUserMaster.UserId;
                Utility.SaveSystemErrorLog(ex, ERRUSERID);
                return Json(new { Success = 0, Message = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }
        [HttpPost]
        public ActionResult ConfirmSelected(List<int> postdata)
        {
            try
            {
                if (postdata != null && postdata.Count() > default(int))
                {
                    System.Threading.ThreadPool.QueueUserWorkItem(s =>
                    {
                        SaveConfirmed(postdata);
                    });

                    return Json(new { Success = 1, Message = "Selected requisition(s) have been confirmed" }, JsonRequestBehavior.AllowGet);
                }
                else
                    throw new Exception("No requisition has been selected for confirmation");
            }
            catch (Exception ex)
            {
                Utility.SaveSystemErrorLog(ex);
                return Json(new { Success = default(int), Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private void SaveConfirmed(List<int> ids)
        {
            var req = new Requisition();
            int result = default(int);
            // var objSend = new CircleProfile();
            //  objSend.Opmode = 0;


            foreach (var i in ids)
            {
                try
                {
                    result = DBOperations<Requisition>.DMLOperation(new Requisition() { Opmode = 3, Id = i }, Constant.usp_RequisitionView);

                }
                catch (Exception ex)
                {
                    Utility.SaveSystemErrorLog(ex);
                    continue;
                }
            }
        }

        [HttpGet]
        public ActionResult CheckReqExist(int MediumId)
        {
            int result = default(int);
            string msg = string.Empty;
            try
            {

                if (MediumId != 4)
                {
                    if (GlobalSettings.oUserMaster.SchoolMedium != 3)
                    {
                        var chkreqexist = DBOperations<Requisition>.GetSpecific(new Requisition() { Opmode = 9, UdiseCode = GlobalSettings.oUserMaster.UserId, FYId = GlobalSettings.oUserMaster.ACAD_ID }, Constant.usp_RequisitionView);
                        if (chkreqexist.Id > 0)
                        {
                            msg = "Requisition already exist for your Institute. Please check Requisition List";
                            return Json(new { Success = 1, Exist = 1, Message = msg }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            msg = "proceed";
                            return Json(new { Success = 1, Exist = 0, Message = msg }, JsonRequestBehavior.AllowGet);
                        }


                    }
                    else // for bilingual
                    {
                        Requisition obj = new Requisition();
                        var chklangexist = DBOperations<Requisition>.GetSpecific(new Requisition() { Opmode = 8, UdiseCode = GlobalSettings.oUserMaster.UserId, MediumId = MediumId, FYId = GlobalSettings.oUserMaster.ACAD_ID }, Constant.usp_RequisitionView);

                        if (chklangexist.Id > default(int))
                        {
                            msg = "Requisition already exist for this Medium. Please check Requisition List";
                            return Json(new { Success = 1, Exist = 1, Message = msg }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            var chkreqexist = DBOperations<Requisition>.GetSpecific(new Requisition() { Opmode = 9, UdiseCode = GlobalSettings.oUserMaster.UserId, FYId = GlobalSettings.oUserMaster.ACAD_ID }, Constant.usp_RequisitionView);
                            if (chkreqexist.Id == 2)
                            {
                                msg = "2 Requisitions already exist for your institute";
                                return Json(new { Success = 1, Exist = 1, Message = msg }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                msg = "proceed";
                                return Json(new { Success = 1, Exist = 0, Message = msg }, JsonRequestBehavior.AllowGet);
                            }


                        }
                    }
                }

                else //for XI_XII
                {
                    Requisition obj = new Requisition();
                    var chklangexist = DBOperations<Requisition>.GetSpecific(new Requisition() { Opmode = 8, UdiseCode = GlobalSettings.oUserMaster.UserId, MediumId = MediumId, FYId = GlobalSettings.oUserMaster.ACAD_ID }, Constant.usp_RequisitionView);

                    if (chklangexist.Id > default(int))
                    {
                        msg = "Requisition already exist for this Medium. Please check Requisition List";
                        return Json(new { Success = 1, Exist = 1, Message = msg }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        msg = "proceed";
                        return Json(new { Success = 1, Exist = 0, Message = msg }, JsonRequestBehavior.AllowGet);
                    }
                }



            }
            catch (Exception ex)
            {
                var ERRUSERID = "USER ID - " + GlobalSettings.oUserMaster.UserId;
                Utility.SaveSystemErrorLog(ex, ERRUSERID);
                return Json(new { Success = 0, Message = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpGet]
        public ActionResult RequisitionReport(long reqId = 0)
        {
            string filepath = string.Empty;
            string reporttype = string.Empty;
            string filename = string.Empty;
            string rptname = string.Empty;

            var challandtl = DBOperations<Requisition>.GetSpecific(new Requisition() { Opmode = 4, Id = reqId }, Constant.usp_RequisitionView);
            filename = challandtl.ReqCode + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf";
            rptname = "CrystalReport1.rpt";




            List<RequisitionDetail1> lst = new List<RequisitionDetail1>();
            try
            {

                 lst = DBOperations<RequisitionDetail1>.GetAllOrByRange(new RequisitionDetail1() { ReqId = reqId  }, Constant.usp_TrxRequisitionDtl1);




                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Reports"), rptname));

                rd.SetDataSource(lst);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();]

                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                rd.Close();
                return File(stream, "application/pdf", filename);

            } 
            catch (Exception ex)
            {
                var ERRUSERID = "USER ID - " + GlobalSettings.oUserMaster.UserId;
                Utility.SaveSystemErrorLog(ex, ERRUSERID);
                return Json(new { Success = 0, Message = ex.Message + " Report Server Not Avaiable" }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}