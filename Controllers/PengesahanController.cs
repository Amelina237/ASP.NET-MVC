using ePerhilitanV2.Infrastructures.Helpers;
using ePerhilitanV2.Infrastructures.Interfaces;
using ePerhilitanV2.Infrastructures.Services;
using ePerhilitanV2.Models;
using ePerhilitanV2.Models.ViewModels.JqueryDatatableParamVM;
using ePerhilitanV2.Models.ViewModels.PengesahanViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace ePerhilitanV2.Controllers
{
    //inherit table MyController--utk guna file Resources(dwibahasa)

    [Authorize]
    public class PengesahanController : MyController
    {
        private PengesahanService _PengesahanService;
        private AccountService _accountService;
        private eLesen_SPJPEntities _context;
        private ISelectListItemHelper _helper;

        public PengesahanController()
        {
            _PengesahanService = new PengesahanService();
            _accountService = new AccountService();
            _context = new eLesen_SPJPEntities();
            _helper = new SelectListItemHelper();
        }

        // GET: Pengesahan
        public ActionResult HalamanUtama()
        {
            return View();
        }
        public ActionResult MaklumatCarian(string id)
        {
            int LesenID = Convert.ToInt32(GlobalFunction.decodeThis(id));
            int CivilID = _context.GBL_LICENSE_MST.Where(a => a.id == LesenID).Select(a => (int)a.lic_civil_id).FirstOrDefault();


            ImportExport model = new ImportExport();
            //model = _PengesahanService.GetIEDetails(LesenID);
            model.License_Id = LesenID;
            model.Civil_Id = CivilID;

            model.GetOwnerDetails = GlobalFunction.getOwnerLicense(LesenID);
            model.getStatusBatalLesen = GlobalFunction.getStatusBatalLesen(CivilID);
            model.getTamatTempohLesen = GlobalFunction.getTamatTempohLesen(CivilID);
            model.getKuatkuasaLesen = GlobalFunction.getKuatkuasaLesen(CivilID);
            return View(model);
        }

        public ActionResult CariSemakanSpesis()
        {
            return View();
        }

        public ActionResult SemakMaklumatPemohon()
        {
            return View();
        }

        public ActionResult SemakMaklumatPemohon2()
        {
            return View();
        }

        //Action to return PartialView with user detail data -- Nanti replace with actual data from DB
        public PartialViewResult GetOwnerDetail(string id)
        {
            int LesenID = Convert.ToInt32(GlobalFunction.decodeThis(id));
            int CivilID = _context.GBL_LICENSE_MST.Where(a => a.id == LesenID).Select(a => (int)a.lic_civil_id).FirstOrDefault();

            ImportExport model = new ImportExport();
            model.License_Id = LesenID;
            model.Civil_Id = CivilID;
            model.GetOwnerDetails = GlobalFunction.getOwnerLicense(LesenID);
            model.GetCompanyDetails = GlobalFunction.getOrganisationLicense(LesenID);

            return PartialView("_InfoPemohon", model);

        }
        public ActionResult ViewPaparanPenyemak()
        {
            return View();
        }

        public ActionResult EditMaklumat(string page, string id)
        {
            var Page = GlobalFunction.decodeThis(page);
            ImportExport model = new ImportExport();
            //var userId = Convert.ToInt32(User.Identity.Name);
            var userId = Convert.ToInt32(User.Identity.Name);
            var Role = _accountService.getRole(userId);

            int decodePage = Convert.ToInt32(Page);
            int nextPage = decodePage + 1;

            //assign nextPage to viewbag
            ViewBag.NextPage = nextPage;

            if (Role.Contains(7) || Role.Contains(10) || Role.Contains(12)) // 7 - superadmin, 10 - penyemak p.msk, 12 - penyemak/pengesah p.msk
            {
                int LesenID = Convert.ToInt32(GlobalFunction.decodeThis(id));

                model = _PengesahanService.GetIEDetails(LesenID);
                model.User_Id = userId;
                model.Page = Page;
                //model.License_Id = LesenID;
                model.GetOwnerDetails = GlobalFunction.getOwnerLicense(LesenID);
                model.GetCompanyDetails = GlobalFunction.getOrganisationLicense(LesenID);
                model.getSpeciesList = _PengesahanService.GetSpeciesList(LesenID);
                model.getSpeciesDetails = _PengesahanService.getSpeciesDetails(LesenID);
                model.getTotalSpeciesList = _PengesahanService.getTotalQuantitySpecies(LesenID);
                model.SelectJenisPermit = _helper.SelectJenisPermit();
                model.SelectStatus = _helper.SelectStatusPermohonan();
                model.SelectAppendix = _helper.SelectAppendix();
                model.SelectSource = _helper.SelectSource();
                model.getListCodePurposeTrade = _helper.GetCodePurposeTrade();
                model.getListImportExportCenter = _helper.GetImportExportCenter();
                model.getListDocument = _PengesahanService.getListDocument(LesenID);
                model.getSpecificDoc = _PengesahanService.getSpecificDoc(LesenID);
                model.getTotalQuantityHidup = _PengesahanService.getTotalQuantityHidup(LesenID);
                model.SelectPengesah = _helper.SelectPengesah(userId);
                model.getNamaPengesahSahaja = _PengesahanService.getNamaPengesah();
            }

            return View(model);
        }

        public ActionResult ViewMaklumat(string id)
        {
            int LesenID = Convert.ToInt32(GlobalFunction.decodeThis(id));
            var LicenseType = _context.GBL_LICENSE_MST.Where(a => a.id == LesenID).Select(a => a.lic_type).FirstOrDefault();

            ViewBag.LicenseType = LicenseType; //assign value LicenseType to viewbag

            //7 - import, 8 - export, 9 - re-export
            if (LicenseType == 7 || LicenseType == 8 || LicenseType == 9)
            {
                ImportExport model = new ImportExport();
                model = _PengesahanService.GetIEDetails(LesenID);
                model.GetOwnerDetails = GlobalFunction.getOwnerLicense(LesenID);
                model.GetCompanyDetails = GlobalFunction.getOrganisationLicense(LesenID);
                model.getSpeciesList = _PengesahanService.GetSpeciesList(LesenID);
                model.getSpeciesDetails = _PengesahanService.getSpeciesDetails(LesenID);
                model.SelectJenisPermit = _helper.SelectJenisPermit();
                model.getReviewVerify = _PengesahanService.getReviewVerify(LesenID);
                model.getTotalSpeciesList = _PengesahanService.getTotalQuantitySpecies(LesenID);
                model.getTotalQuantityHidup = _PengesahanService.getTotalQuantityHidup(LesenID);
                model.getListDocument = _PengesahanService.getListDocument(LesenID);
                return View(model);
            }

            TempData["Message"] = "No Record Found!";
            return View();
        }

        public ActionResult ViewSemakanLesen(string id)
        {
            int LesenID = Convert.ToInt32(GlobalFunction.decodeThis(id));
            var LicenseType = _context.GBL_LICENSE_MST.Where(a => a.id == LesenID).Select(a => a.lic_type).FirstOrDefault();

            var userId = Convert.ToInt32(User.Identity.Name);
            var Role = _accountService.getRole(userId);

            ViewBag.LicenseType = LicenseType; //assign value LicenseType to viewbag

            var NoLesen = _context.GBL_LICENSE_MST.Where(a => a.id == LesenID).Select(a => a.lic_no).FirstOrDefault();

            StatusLesen model = new StatusLesen();
            model.NoLesen = NoLesen;
            model.User_Id = userId;
            model = GlobalFunction.getAllViewLesen(LesenID);
            model.LesenID = LesenID;
            model.ListStatusLesen = _helper.StatusLesen();
            model.ListStatusPemilik = _helper.StatusPemilik();
            model.ListDocument = _PengesahanService.getListDocument(LesenID);
            model.GetOwnerDetails = GlobalFunction.getOwnerLicense(LesenID);
            model.GetCompanyDetails = GlobalFunction.getOrganisationLicense(LesenID);
            model.getSpeciesDetails = _PengesahanService.getSpeciesDetails(LesenID);
            model.getTotalSpeciesList = _PengesahanService.getTotalQuantitySpecies(LesenID);

            if (LicenseType == 3 || LicenseType == 4 || LicenseType == 13) //Mengguna/Menyimpan/Kajian
            {
                model.GetAddressDetails = GlobalFunction.getAddressLicense(LesenID, 4); //alamat menyimpan
                return View(model);

            }
            return View(model);
        }

        //public ActionResult ViewMaklumat(int id)
        //{
        //        ImportExport model = new ImportExport();
        //        model = _PengesahanService.GetIEDetails(id);
        //        model.GetOwnerDetails = GlobalFunction.getOwnerLicense(id);
        //        model.GetCompanyDetails = GlobalFunction.getOrganisationLicense(id);
        //        model.getSpeciesList = _PengesahanService.GetSpeciesList(id);
        //        model.getSpeciesDetails = _PengesahanService.getSpeciesDetails(id);
        //        model.SelectJenisPermit = _helper.SelectJenisPermit();
        //        model.getReviewVerify = _PengesahanService.getReviewVerify(id);
        //        model.getTotalSpeciesList = _PengesahanService.getTotalQuantitySpecies(id);
        //        model.getTotalQuantityHidup = _PengesahanService.getTotalQuantityHidup(id);
        //        model.getListDocument = _PengesahanService.getListDocument(id);
        //        return View(model);
        //}

        #region  Semak Maklumat Pemohon - JSON

        public JsonResult GetDataByNamaPemilik(string keyword)
        {
            var dataSearch = (from a in _context.GBL_LICENSE_MST
                              join b in _context.GBL_CIVILANT_MST on a.lic_civil_id equals b.id
                              join c in _context.GBL_CIVDOC_MST on b.id equals c.civil_id
                              join d in _context.dNationalities on c.civil_type equals d.id
                              where b.civil_name.Contains(keyword)
                              select new
                              {
                                  id_user = a.id,
                                  ownerName = b.civil_name,
                                  ic = c.civil_doc,
                                  phone = b.civil_tel,
                              })
                              .GroupBy(x => x.ownerName)
                              .Select(g => g.FirstOrDefault())  //select first item group
                              .ToList();    //convert result into list

            var encodeData = dataSearch.Select(x => new
            {
                id_user = GlobalFunction.encodeThis(x.id_user.ToString()),
                ownerName = x.ownerName,
                ic = x.ic,
                phone = x.phone

            }).ToList();

            return Json(encodeData);
        }

        public JsonResult GetDataByPhone(string keyword)
        {
            var dataSearch = (from a in _context.GBL_LICENSE_MST
                              join b in _context.GBL_CIVILANT_MST on a.lic_civil_id equals b.id
                              join c in _context.GBL_CIVDOC_MST on b.id equals c.civil_id
                              join d in _context.dNationalities on c.civil_type equals d.id
                              where b.civil_tel == keyword
                              select new
                              {
                                  id_user = a.id,
                                  ownerName = b.civil_name,
                                  phone = b.civil_tel,
                                  ic = c.civil_doc,
                              })
                              .GroupBy(x => x.ownerName)
                              .Select(g => g.FirstOrDefault())  //select first item group
                              .ToList();    //convert result into list

            var encodeData = dataSearch.Select(x => new
            {
                id_user = GlobalFunction.encodeThis(x.id_user.ToString()),
                ownerName = x.ownerName,
                ic = x.ic,
                phone = x.phone

            }).ToList();

            return Json(encodeData);
        }

        public JsonResult GetDataByRoc(string keyword)
        {
            var dataSearch = (from a in _context.GBL_LICENSE_MST
                              join b in _context.GBL_CIVILANT_MST on a.lic_civil_id equals b.id
                              join c in _context.GBL_ORGANIZATION_MST on a.lic_org_id equals c.id
                              where c.org_roc.Contains(keyword)
                              select new
                              {
                                  id_user = a.id,
                                  ownerName = b.civil_name,
                                  companyName = c.org_name,
                                  rocNo = c.org_roc,
                              })
                              .GroupBy(x => x.ownerName)
                              .Select(g => g.FirstOrDefault())  //select first item group
                              .ToList();    //convert result into list

            var encodeData = dataSearch.Select(x => new
            {
                id_user = GlobalFunction.encodeThis(x.id_user.ToString()),
                ownerName = x.ownerName,
                companyName = x.companyName,
                rocNo = x.rocNo,

            }).ToList();

            return Json(encodeData);
        }

        public JsonResult GetDataByCompanyName(string keyword)
        {
            var dataSearch = (from a in _context.GBL_LICENSE_MST
                              join b in _context.GBL_CIVILANT_MST on a.lic_civil_id equals b.id
                              join c in _context.GBL_ORGANIZATION_MST on a.lic_org_id equals c.id
                              where c.org_name.Contains(keyword)
                              select new
                              {
                                  id_user = a.id,
                                  ownerName = b.civil_name,
                                  companyName = c.org_name,
                                  rocNo = c.org_roc,
                              })
                              .GroupBy(x => x.companyName)
                              .Select(g => g.FirstOrDefault())  //select first item group
                              .ToList();    //convert result into list

            var encodeData = dataSearch.Select(x => new
            {
                id_user = GlobalFunction.encodeThis(x.id_user.ToString()),
                ownerName = x.ownerName,
                companyName = x.companyName,
                rocNo = x.rocNo,

            }).ToList();

            return Json(encodeData);
        }

        public JsonResult GetDataByNoPenandaan(string keyword)
        {
            var dataSearch = (from a in _context.GBL_LICENSE_MST
                              join b in _context.GBL_CIVILANT_MST on a.lic_civil_id equals b.id
                              join c in _context.GBL_SPESIS_MST on a.id equals c.license_id
                              join d in _context.dSpesisNames on c.spesis_id equals d.id
                              where c.penandaan.Contains(keyword)
                              select new
                              {
                                  id_user = a.id,
                                  ownerName = b.civil_name,
                                  spp_saintifik = d.spp_saintifik,
                                  spp_common = d.spp_common,
                                  noPenandaan = c != null ? c.penandaan : "",
                                  noLesen = a != null ? a.lic_no : "",

                              })
                              .GroupBy(x => x.noLesen)
                              .Select(g => g.FirstOrDefault())  //select first item group
                              .ToList();    //convert result into list

            var encodeData = dataSearch.Select(x => new
            {
                id_user = GlobalFunction.encodeThis(x.id_user.ToString()),
                ownerName = x.ownerName,
                spp_saintifik = x.spp_saintifik,
                spp_common = x.spp_common,
                noPenandaan = x.noPenandaan,
                noLesen = x.noLesen,

            }).ToList();

            return Json(encodeData);
        }

        public JsonResult GetDataByICUser(string keyword)
        {
            var details = (from a in _context.GBL_LICENSE_MST
                           join b in _context.GBL_CIVILANT_MST on a.lic_civil_id equals b.id
                           join c in _context.GBL_CIVDOC_MST on b.id equals c.civil_id
                           join d in _context.GBL_IMPORT_MST on a.id equals d.license_id
                           join e in _context.dLicenseDetails on a.lic_type equals e.id
                           where c.civil_doc.Contains(keyword)
                           select new
                           {
                               id_user = a.id,
                               ic = c.civil_doc,
                               ownerName = b.civil_name,
                               phone = b.civil_tel,
                               noLesen = a != null ? a.lic_no : "",
                               noCites = d != null ? d.cites_no : "",
                               category = e.desc_LicenseDetail,
                               status_penyemak = d.status_pengesahan,
                               status_pengesah = d.status_pegawai_pengesah

                           }).ToList();    //convert result into list

            var encodeData = details.Select(x => new
            {
                id_user = GlobalFunction.encodeThis(x.id_user.ToString()),
                x.ic,
                x.ownerName,
                x.phone,
                x.noLesen,
                x.noCites,
                x.category,
                x.status_penyemak,
                x.status_pengesah,

            }).ToList();

            return Json(encodeData);
        }

        #endregion

        #region CariSemakanSpesis - JSON

        public string EncodeThis(string str)
        {
            return GlobalFunction.encodeThis(str);
        }

        public JsonResult GetDataByIC(string keyword)
        {
            var details = (from a in _context.GBL_LICENSE_MST
                           join b in _context.GBL_CIVILANT_MST on a.lic_civil_id equals b.id
                           join c in _context.GBL_CIVDOC_MST on b.id equals c.civil_id
                           join d in _context.GBL_IMPORT_MST on a.id equals d.license_id
                           join e in _context.dLicenseDetails on a.lic_type equals e.id
                           where c.civil_doc.Contains(keyword)
                           select new
                           {
                               id_user = a.id,
                               ic = c.civil_doc,
                               ownerName = b.civil_name,
                               phone = b.civil_tel,
                               noLesen = a != null ? a.lic_no : "",
                               noCites = d != null ? d.cites_no : "",
                               category = e.desc_LicenseDetail,
                               status_penyemak = d.status_pengesahan,
                               status_pengesah = d.status_pegawai_pengesah

                           }).ToList();

            var encode = details.Select(x => new
            {
                id_user = GlobalFunction.encodeThis(x.id_user.ToString()),
                x.ic,
                x.ownerName,
                x.phone,
                x.noLesen,
                x.noCites,
                x.category,
                x.status_penyemak,
                x.status_pengesah,

            }).ToList();

            return Json(encode);
        }

        public JsonResult GetDataByNoLesen(string keyword)
        {
            var details = (from a in _context.GBL_LICENSE_MST
                           join b in _context.GBL_CIVILANT_MST on a.lic_civil_id equals b.id
                           join c in _context.GBL_CIVDOC_MST on b.id equals c.civil_id
                           join d in _context.GBL_IMPORT_MST on a.id equals d.license_id
                           join e in _context.dLicenseDetails on a.lic_type equals e.id
                           join f in _context.dStates on a.negeri_id equals f.id
                           where a.lic_no.Contains(keyword)
                           select new
                           {
                               id_user = a.id,
                               ic = c.civil_doc,
                               ownerName = b.civil_name,
                               phone = b.civil_tel,
                               noLesen = a != null ? a.lic_no : "",
                               noCites = d != null ? d.cites_no : "",
                               category = e.desc_LicenseDetail,
                               status_penyemak = d.status_pengesahan,
                               status_pengesah = d.status_pegawai_pengesah,
                               pejabatPengeluar = f.state_desc,
                               tarikhMula = a.date_create,

                           }).ToList();

            var encode = details.Select(x => new
            {
                id_user = GlobalFunction.encodeThis(x.id_user.ToString()),
                x.ic,
                x.ownerName,
                x.phone,
                x.noLesen,
                x.noCites,
                x.category,
                x.status_penyemak,
                x.status_pengesah,
                x.pejabatPengeluar,
                x.tarikhMula,

            }).ToList();

            return Json(encode);
        }

        public JsonResult GetDataByNoCites(string keyword)
        {
            //LINQ does not allow to directly encode function
            var details = (from a in _context.GBL_LICENSE_MST
                           join b in _context.GBL_CIVILANT_MST on a.lic_civil_id equals b.id
                           join c in _context.GBL_CIVDOC_MST on b.id equals c.civil_id
                           join d in _context.GBL_IMPORT_MST on a.id equals d.license_id
                           join e in _context.dLicenseDetails on a.lic_type equals e.id
                           where d.cites_no.Contains(keyword)
                           select new
                           {
                               id_user = a.id,
                               ic = c.civil_doc,
                               ownerName = b.civil_name,
                               phone = b.civil_tel,
                               noLesen = a != null ? a.lic_no : "",
                               noCites = d != null ? d.cites_no : "",
                               category = e.desc_LicenseDetail,
                               status_penyemak = d.status_pengesahan,
                               status_pengesah = d.status_pegawai_pengesah

                           }).ToList();

            var encode = details.Select(x => new
            {
                id_user = GlobalFunction.encodeThis(x.id_user.ToString()),
                x.ic,
                x.ownerName,
                x.phone,
                x.noLesen,
                x.noCites,
                x.category,
                x.status_penyemak,
                x.status_pengesah,

            }).ToList();


            return Json(encode);
        }
        #endregion

        #region List of Table
        public ActionResult SenaraiPermohonanTelahDisemak()
        {
            return View();
        }

        public ActionResult SenaraiPermohonanDisemakSemula()
        {

            return View();
        }

        public ActionResult SenaraiPermohonanDisahkan()
        {
            return View();
        }

        public ActionResult SenaraiPermohonanTidakSah()
        {
            return View();
        }

        public ActionResult SenaraiPermohonanSelesai()
        {

            return View();
        }

        public ActionResult SenaraiPermohonanDalamProsesSemakSemula()
        {
            return View();
        }

        public JsonResult queryDataTable(JqueryDatatableParamVM param, FormCollection frm)
        {
            try
            {
                var sortColumn = frm["columns[1][data]"];
                var sortColumnDirection = frm["order[0][dir]"];

                var userId = Convert.ToInt32(User.Identity.Name);
                var type = frm["type"].ToString();

                if (type == "1")    //disemak semula
                {
                    var a = _PengesahanService.getStatusSemakSemula(param, frm, userId);

                    var iTotalRecords = a.GetType().GetProperty("iTotalRecords").GetValue(a, null);
                    var iTotalDisplayRecords = a.GetType().GetProperty("iTotalDisplayRecords").GetValue(a, null);
                    var aaData = a.GetType().GetProperty("listData").GetValue(a, null);
                    return Json(new
                    {
                        param.draw,
                        iTotalRecords = iTotalRecords,
                        iTotalDisplayRecords = iTotalDisplayRecords,
                        aaData = aaData
                    }); ;
                }
                else if (type == "2") //disemak
                {
                    var a = _PengesahanService.getDisemak(param, frm, userId);

                    var iTotalRecords = a.GetType().GetProperty("iTotalRecords").GetValue(a, null);
                    var iTotalDisplayRecords = a.GetType().GetProperty("iTotalDisplayRecords").GetValue(a, null);
                    var aaData = a.GetType().GetProperty("listData").GetValue(a, null);
                    return Json(new
                    {
                        param.draw,
                        iTotalRecords = iTotalRecords,
                        iTotalDisplayRecords = iTotalDisplayRecords,
                        aaData = aaData
                    }); ;
                }
                else if (type == "3") // sah
                {
                    var a = _PengesahanService.getSahStatus(param, frm, userId);

                    var iTotalRecords = a.GetType().GetProperty("iTotalRecords").GetValue(a, null);
                    var iTotalDisplayRecords = a.GetType().GetProperty("iTotalDisplayRecords").GetValue(a, null);
                    var aaData = a.GetType().GetProperty("listData").GetValue(a, null);
                    return Json(new
                    {
                        param.draw,
                        iTotalRecords = iTotalRecords,
                        iTotalDisplayRecords = iTotalDisplayRecords,
                        aaData = aaData
                    }); ;
                }

                else if (type == "4") // selesai
                {
                    var a = _PengesahanService.getStatusSelesai(param, frm, userId);

                    var iTotalRecords = a.GetType().GetProperty("iTotalRecords").GetValue(a, null);
                    var iTotalDisplayRecords = a.GetType().GetProperty("iTotalDisplayRecords").GetValue(a, null);
                    var aaData = a.GetType().GetProperty("listData").GetValue(a, null);
                    return Json(new
                    {
                        param.draw,
                        iTotalRecords = iTotalRecords,
                        iTotalDisplayRecords = iTotalDisplayRecords,
                        aaData = aaData
                    }); ;
                }

                else if (type == "5") // tk sah
                {
                    var a = _PengesahanService.getTakSahStatus(param, frm);

                    var iTotalRecords = a.GetType().GetProperty("iTotalRecords").GetValue(a, null);
                    var iTotalDisplayRecords = a.GetType().GetProperty("iTotalDisplayRecords").GetValue(a, null);
                    var aaData = a.GetType().GetProperty("listData").GetValue(a, null);
                    return Json(new
                    {
                        param.draw,
                        iTotalRecords = iTotalRecords,
                        iTotalDisplayRecords = iTotalDisplayRecords,
                        aaData = aaData
                    }); ;
                }

                else if (type == "6") // sah
                {
                    var a = _PengesahanService.getDlmSemakanSemula(param, frm, userId);

                    var iTotalRecords = a.GetType().GetProperty("iTotalRecords").GetValue(a, null);
                    var iTotalDisplayRecords = a.GetType().GetProperty("iTotalDisplayRecords").GetValue(a, null);
                    var aaData = a.GetType().GetProperty("listData").GetValue(a, null);
                    return Json(new
                    {
                        param.draw,
                        iTotalRecords = iTotalRecords,
                        iTotalDisplayRecords = iTotalDisplayRecords,
                        aaData = aaData
                    }); ;
                }

                return Json(new
                {
                    param.draw,
                    iTotalRecords = 0,
                    iTotalDisplayRecords = 0,
                    aaData = ""
                });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                return Json(new
                {
                    param.draw,
                    iTotalRecords = 0,
                    iTotalDisplayRecords = 0,
                    aaData = ""
                });
            }

        }

        #endregion

        #region Simpan Maklumat

        [HttpPost, ValidateInput(false)]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> EditMaklumat(ImportExport model)
        {
            var page = GlobalFunction.decodeThis(model.Page);
            int decodePage = Convert.ToInt32(page);

            int nextPage = decodePage + 1;

            var FileLocation = "";
            var fileName = "";

            var userId = Convert.ToInt32(User.Identity.Name);
            var Role = _accountService.getRole(userId);

            //File Upload
            string folderPath = Server.MapPath("~/Content/uploads/maklumatcites/");

            if (model.FileType == "CitesOri" && model.FileType != null)
            {
                //Generate unique file name to prevent overwriting exsiting file
                fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.salinanCitesOriginal.FileName);
                //save file to directory
                string fileUrl = Path.Combine(folderPath, fileName);
                model.salinanCitesOriginal.SaveAs(fileUrl);

                // assign filename of SalinanCitesOriginal into model.getSpecificDoc.File_Name
                model.getSpecificDoc.File_Name = model.salinanCitesOriginal.FileName;

                //set the path url on db
                FileLocation = "uploads/maklumatcites/" + fileName;

            }

            else if (model.FileType == "Cites2")
            {
                //Generate unique file name to prevent overwriting exsiting file
                fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.salinanCitesChanges.FileName);
                //save file to directory
                string fileUrl = Path.Combine(folderPath, fileName);
                model.salinanCitesChanges.SaveAs(fileUrl);

                //set the path url on db
                FileLocation = "uploads/maklumat_cites_ubah/" + fileName;
            }

            //Save Import Export
            if (model.submitType == "Draft")
            {

                RemoveModelState(model);



                if (ModelState.IsValid)
                {
                    await _PengesahanService.SaveImportExport(model, page, FileLocation);
                    TempData["Message"] = Resources.Resource.SuccessSave;
                    return RedirectToAction("EditMaklumat", new { Page = GlobalFunction.encodeThis(page), id = GlobalFunction.encodeThis(model.License_Id.ToString()) });
                }

            }

            if (model.submitType == "SaveNext")
            {
                RemoveModelState(model);

                if (ModelState.IsValid)
                {
                    await _PengesahanService.SaveImportExport(model, page, FileLocation);
                    TempData["Message"] = Resources.Resource.SuccessSave;
                    return RedirectToAction("EditMaklumat", new { Page = GlobalFunction.encodeThis(nextPage.ToString()), id = GlobalFunction.encodeThis(model.License_Id.ToString()) });
                }
            }

            //redirect to Senarai
            if (model.submitType == "Submit")
            {
                RemoveModelState(model);
                if (ModelState.IsValid)
                {
                    await _PengesahanService.SaveImportExport(model, page, FileLocation);

                    if (Role.Contains(10))
                    {
                        if (model.Status_By_Penyemak == "1" && model.Status_By_Pengesah == null) //penyemak sahkan
                        {
                            TempData["Message"] = "Permohonan Perlu Disahkan";
                            return RedirectToAction("Index", "Dashboard");
                        }

                        if (model.Status_By_Penyemak == "1" && model.Status_By_Pengesah == "2") //g ke dashboard dalam proses semakan semula 
                        {
                            TempData["Message"] = "Permohonan Dalam Proses Semakan Semula";
                            return RedirectToAction("Index", "Dashboard");
                        }
                    }

                    if (Role.Contains(12))
                    {
                        if (model.Status_By_Pengesah == "2") //untuk disemak semula
                        {
                            TempData["Message"] = "Permohonan Perlu Disemak Semula";
                            return RedirectToAction("Index", "Dashboard");
                        }
                    }

                    if (model.Status_By_Penyemak == "2") //penyemak tk sahkan
                    {
                        TempData["Message"] = "Permohonan Tidak Sah";
                        return RedirectToAction("SenaraiPermohonanTidakSah");
                    }

                    else if (model.Status_By_Penyemak == "1" && model.Status_By_Pengesah == "1") //penyemak sahkan
                    {
                        TempData["Message"] = "Permohonan Telah Disahkan dan Selesai";
                        return RedirectToAction("SenaraiPermohonanSelesai");
                    }
                }
            }

            model = _PengesahanService.GetIEDetails(model.License_Id);
            model.Page = page;
            model.User_Id = userId;
            model.License_Id = model.License_Id;
            model.GetOwnerDetails = GlobalFunction.getOwnerLicense(model.License_Id);
            model.GetCompanyDetails = GlobalFunction.getOrganisationLicense(model.License_Id);
            model.getSpeciesList = _PengesahanService.GetSpeciesList(model.License_Id);
            model.getSpeciesDetails = _PengesahanService.getSpeciesDetails(model.License_Id);
            model.getTotalSpeciesList = _PengesahanService.getTotalQuantitySpecies(model.License_Id);
            model.SelectJenisPermit = _helper.SelectJenisPermit();
            model.SelectStatus = _helper.SelectStatusPermohonan();
            model.SelectAppendix = _helper.SelectAppendix();
            model.SelectSource = _helper.SelectSource();
            model.getListCodePurposeTrade = _helper.GetCodePurposeTrade();
            model.getListImportExportCenter = _helper.GetImportExportCenter();
            model.getListDocument = _PengesahanService.getListDocument(model.License_Id);
            model.getTotalQuantityHidup = _PengesahanService.getTotalQuantityHidup(model.License_Id);
            model.SelectPengesah = _helper.SelectPengesah(userId);

            return View(model);
        }

        private void RemoveModelState(ImportExport model)
        {
            int page = Convert.ToInt32(GlobalFunction.decodeThis(model.Page));
            var userId = Convert.ToInt32(User.Identity.Name);
            var Role = _accountService.getRole(userId);

            if (page == 1)
            {
                //remove error utk specific field
                ModelState.Remove("Nama_Pemeriksa");
                ModelState.Remove("Lokasi_Pemeriksaan");
                ModelState.Remove("Tarikh_MasaSemakan");
                ModelState.Remove("Status_By_Penyemak");
                ModelState.Remove("Status_By_Pengesah");
                ModelState.Remove("Nama_Pengesah");
                ModelState.Remove("Catatan_Penyemak");
                ModelState.Remove("Ulasan_Pengesah");
                ModelState.Remove("Nama_Import");
                ModelState.Remove("Negara_Import");
                ModelState.Remove("Alamat_Pengimport");
                ModelState.Remove("No_Stem");
                ModelState.Remove("Tujuan");
                ModelState.Remove("Nama_Eksport");
                ModelState.Remove("Negara_Eksport");
                ModelState.Remove("Alamat_Pengeksport");
                ModelState.Remove("salinanCitesOriginal");
                ModelState.Remove("Spesis_Quantity_Disahkan");
            }
            else if (page == 2)
            {
                ModelState.Remove("Jenis_Permit");
                ModelState.Remove("Sah_Sehingga");
                ModelState.Remove("Nama_Pemeriksa");
                ModelState.Remove("Lokasi_Pemeriksaan");
                ModelState.Remove("Tarikh_MasaSemakan");
                ModelState.Remove("Status_By_Penyemak");
                ModelState.Remove("Status_By_Pengesah");
                ModelState.Remove("Nama_Pengesah");
                ModelState.Remove("Catatan_Penyemak");
                ModelState.Remove("Ulasan_Pengesah");
                ModelState.Remove("Nama_Import");
                ModelState.Remove("Negara_Import");
                ModelState.Remove("Alamat_Pengimport");
                ModelState.Remove("No_Stem");
                ModelState.Remove("Tujuan");
                ModelState.Remove("Nama_Eksport");
                ModelState.Remove("Negara_Eksport");
                ModelState.Remove("Alamat_Pengeksport");
                ModelState.Remove("salinanCitesOriginal");

                for (int i = 0; i < model.getTotalSpeciesList.Count; i++)
                {
                    var qtySah = "getTotalSpeciesList[" + i + "].Spesis_Quantity_Disahkan";
                    //guna ModelState.ContainsKey - bila ada collection/lists of data - to remove ModelState error dynamically based pada key - good practise utk elak exception!
                    if (ModelState.ContainsKey(qtySah)) //check whether there is key or not
                    {
                        //klau ada key qtySah dri model getTotalSpeciesList - dia akan remove error tu
                        ModelState.Remove(qtySah);

                    }
                }



            }
            else if (page == 3)
            {
                if (model.License_Type == "Eksport" || model.License_Type == "Eksport Semula")
                {
                    ModelState.Remove("Nama_Eksport");
                    ModelState.Remove("Negara_Eksport");
                    ModelState.Remove("Alamat_Pengeksport");
                }
                else if (model.License_Type == "Import")
                {
                    ModelState.Remove("Nama_Import");
                    ModelState.Remove("Negara_Import");
                    ModelState.Remove("Alamat_Pengimport");
                }
                ModelState.Remove("Nama_Pemeriksa");
                ModelState.Remove("Lokasi_Pemeriksaan");
                ModelState.Remove("Tarikh_MasaSemakan");
                ModelState.Remove("Status_By_Penyemak");
                ModelState.Remove("Status_By_Pengesah");
                ModelState.Remove("Nama_Pengesah");
                ModelState.Remove("Catatan_Penyemak");
                ModelState.Remove("Ulasan_Pengesah");
                ModelState.Remove("Jenis_Permit");
                ModelState.Remove("Sah_Sehingga");
                ModelState.Remove("Spesis_Quantity_Disahkan");

                if (model.getSpecificDoc.File_Name == null)
                {
                    ModelState.AddModelError("getSpecificDoc.File_Name", string.Format(Resources.Verification.E_CopyCites));
                }

            }
            else if (page == 4)
            {
                ModelState.Remove("Jenis_Permit");
                ModelState.Remove("Sah_Sehingga");
                ModelState.Remove("Status_By_Penyemak");
                ModelState.Remove("Status_By_Pengesah");
                ModelState.Remove("Nama_Pengesah");
                ModelState.Remove("Catatan_Penyemak");
                ModelState.Remove("Ulasan_Pengesah");
                ModelState.Remove("Nama_Import");
                ModelState.Remove("Negara_Import");
                ModelState.Remove("Alamat_Pengimport");
                ModelState.Remove("No_Stem");
                ModelState.Remove("Tujuan");
                ModelState.Remove("Nama_Eksport");
                ModelState.Remove("Negara_Eksport");
                ModelState.Remove("Alamat_Pengeksport");
                ModelState.Remove("salinanCitesOriginal");
                ModelState.Remove("Spesis_Quantity_Disahkan");
            }
            else if (page == 5)
            {

                ModelState.Remove("Jenis_Permit");
                ModelState.Remove("Sah_Sehingga");
                ModelState.Remove("Nama_Pemeriksa");
                ModelState.Remove("Lokasi_Pemeriksaan");
                ModelState.Remove("Tarikh_MasaSemakan");
                ModelState.Remove("Status_By_Pengesah");
                ModelState.Remove("Ulasan_Pengesah");
                ModelState.Remove("Nama_Import");
                ModelState.Remove("Negara_Import");
                ModelState.Remove("Alamat_Pengimport");
                ModelState.Remove("No_Stem");
                ModelState.Remove("Tujuan");
                ModelState.Remove("Nama_Eksport");
                ModelState.Remove("Negara_Eksport");
                ModelState.Remove("Alamat_Pengeksport");
                ModelState.Remove("salinanCitesOriginal");
                ModelState.Remove("Spesis_Quantity_Disahkan");

                if (model.Status_By_Penyemak == "2")
                {
                    ModelState.Remove("Nama_Pengesah");
                }
            }
            else if (page == 6)
            {
                ModelState.Remove("Jenis_Permit");
                ModelState.Remove("Sah_Sehingga");
                ModelState.Remove("Nama_Pemeriksa");
                ModelState.Remove("Lokasi_Pemeriksaan");
                ModelState.Remove("Tarikh_MasaSemakan");
                ModelState.Remove("Status_By_Penyemak");
                ModelState.Remove("Nama_Pengesah");
                ModelState.Remove("Catatan_Penyemak");
                ModelState.Remove("Nama_Import");
                ModelState.Remove("Negara_Import");
                ModelState.Remove("Alamat_Pengimport");
                ModelState.Remove("No_Stem");
                ModelState.Remove("Tujuan");
                ModelState.Remove("Nama_Eksport");
                ModelState.Remove("Negara_Eksport");
                ModelState.Remove("Alamat_Pengeksport");
                ModelState.Remove("salinanCitesOriginal");
                ModelState.Remove("Spesis_Quantity_Disahkan");
            }
        }

        public ActionResult DownloadFile(string fileName)
        {
            // FileLocation is the relative path stored in the database
            string relativePath = "uploads/maklumatcites/" + fileName;

            // Map the relative path to the physical path
            string filePath = Server.MapPath("~/Content/" + relativePath);

            // Check if the file exists
            if (System.IO.File.Exists(filePath))
            {
                // Return the file for download
                return File(filePath, "application/octet-stream", fileName);
            }
            else
            {
                // If the file does not exist, return a 404 Not Found
                return HttpNotFound();
            }
        }
        #endregion

        #region Datatable Semakan
        public JsonResult queryTableSemakan(JqueryDatatableParamVM param, FormCollection frm)
        {
            try
            {
                var sortColumn = frm["columns[1][data]"];
                var sortColumnDirection = frm["order[0][dir]"];

                var userId = Convert.ToInt32(User.Identity.Name);
                var type = frm["type"].ToString();


                if (type == "1" || type == "2" || type == "7" || type == "8")    //NamaPemilik/NamaSyarikat
                {
                    var a = _PengesahanService.getNamaPemilik(param, frm);

                    var iTotalRecords = a.GetType().GetProperty("iTotalRecords").GetValue(a, null);
                    var iTotalDisplayRecords = a.GetType().GetProperty("iTotalDisplayRecords").GetValue(a, null);
                    var aaData = a.GetType().GetProperty("listData").GetValue(a, null);
                    return Json(new
                    {
                        param.draw,
                        iTotalRecords = iTotalRecords,
                        iTotalDisplayRecords = iTotalDisplayRecords,
                        aaData = aaData
                    }); ;

                }

                else if (type == "3")    //No Penandaan
                {
                    var a = _PengesahanService.getNoPenandaan(param, frm);

                    var iTotalRecords = a.GetType().GetProperty("iTotalRecords").GetValue(a, null);
                    var iTotalDisplayRecords = a.GetType().GetProperty("iTotalDisplayRecords").GetValue(a, null);
                    var aaData = a.GetType().GetProperty("listData").GetValue(a, null);
                    return Json(new
                    {
                        param.draw,
                        iTotalRecords = iTotalRecords,
                        iTotalDisplayRecords = iTotalDisplayRecords,
                        aaData = aaData
                    }); ;

                }

                else if (type == "4")    //No Lesen
                {
                    var a = _PengesahanService.getNoLesen(param, frm);

                    var iTotalRecords = a.GetType().GetProperty("iTotalRecords").GetValue(a, null);
                    var iTotalDisplayRecords = a.GetType().GetProperty("iTotalDisplayRecords").GetValue(a, null);
                    var aaData = a.GetType().GetProperty("listData").GetValue(a, null);
                    return Json(new
                    {
                        param.draw,
                        iTotalRecords = iTotalRecords,
                        iTotalDisplayRecords = iTotalDisplayRecords,
                        aaData = aaData
                    }); ;

                }

                else if (type == "5")    //Alamat
                {
                    var a = _PengesahanService.getAlamat(param, frm);

                    var iTotalRecords = a.GetType().GetProperty("iTotalRecords").GetValue(a, null);
                    var iTotalDisplayRecords = a.GetType().GetProperty("iTotalDisplayRecords").GetValue(a, null);
                    var aaData = a.GetType().GetProperty("listData").GetValue(a, null);
                    return Json(new
                    {
                        param.draw,
                        iTotalRecords = iTotalRecords,
                        iTotalDisplayRecords = iTotalDisplayRecords,
                        aaData = aaData
                    }); ;

                }

                else if (type == "6")
                {
                    var a = _PengesahanService.getByNoCites(param, frm);

                    var iTotalRecords = a.GetType().GetProperty("iTotalRecords").GetValue(a, null);
                    var iTotalDisplayRecords = a.GetType().GetProperty("iTotalDisplayRecords").GetValue(a, null);
                    var aaData = a.GetType().GetProperty("listData").GetValue(a, null);
                    return Json(new
                    {
                        param.draw,
                        iTotalRecords = iTotalRecords,
                        iTotalDisplayRecords = iTotalDisplayRecords,
                        aaData = aaData
                    }); ;

                }

                //else if (type == "8")    
                //{
                //    var a = _PengesahanService.getByNoTel(param, frm);

                //    var iTotalRecords = a.GetType().GetProperty("iTotalRecords").GetValue(a, null);
                //    var iTotalDisplayRecords = a.GetType().GetProperty("iTotalDisplayRecords").GetValue(a, null);
                //    var aaData = a.GetType().GetProperty("listData").GetValue(a, null);
                //    return Json(new
                //    {
                //        param.draw,
                //        iTotalRecords = iTotalRecords,
                //        iTotalDisplayRecords = iTotalDisplayRecords,
                //        aaData = aaData
                //    }); ;

                //}

                return Json(new
                {
                    param.draw,
                    iTotalRecords = 0,
                    iTotalDisplayRecords = 0,
                    aaData = ""
                });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                return Json(new
                {
                    param.draw,
                    iTotalRecords = 0,
                    iTotalDisplayRecords = 0,
                    aaData = ""
                });
            }

        }
        #endregion

        [HttpPost, ValidateInput(false)]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> ViewSemakanLesen(StatusLesen model)
        {
            
            if (model.StatusPemilik == null)
            {
                ModelState.AddModelError("StatusPemilik", string.Format(Resources.Verification.E_StatusPemilik));
            }
            if (model.StatusLesenSemakan == null)
            {
                ModelState.AddModelError("StatusLesenSemakan", string.Format(Resources.Verification.E_StatusLesen));
            }

            //add untuk lampiran

            //if (model.StatusLesenSemakan == "13") //batal
            //{
            //    if (model.LampiranDibatal == null)
            //    {
            //        ModelState.AddModelError("LampiranDibatal", string.Format(Resources.Verification.E_StatusLesen)); //add lampiran dibatal
            //    }
            //}

            //if (model.StatusLesenSemakan == "14")   //gantung
            //{
            //    if (model.LampiranDibatal == null)
            //    {
            //        ModelState.AddModelError("LampiranDibatal", string.Format(Resources.Verification.E_StatusLesen)); //add lampiran dibatal
            //    }
            //}

            if (model.TarikhTindakan == null)
            {
                ModelState.AddModelError("TarikhTindakan", string.Format(Resources.Verification.E_TarikhTindakan));
            }

            if (ModelState.IsValid)
            {
                await _PengesahanService.SaveStatusLesen(model);
                TempData["Message"] = Resources.Resource.SuccessSave;
                return RedirectToAction("MaklumatCarian", new { id = GlobalFunction.encodeThis(model.LesenID.ToString()) });
            }
            
            int LesenID = model.LesenID;
            var LicenseType = _context.GBL_LICENSE_MST.Where(a => a.id == LesenID).Select(a => a.lic_type).FirstOrDefault();
            ViewBag.LicenseType = LicenseType;

            model = GlobalFunction.getAllViewLesen(LesenID);
            model.ListStatusLesen = _helper.StatusLesen();
            model.ListStatusPemilik = _helper.StatusPemilik();
            model.ListDocument = _PengesahanService.getListDocument(LesenID);
            model.GetOwnerDetails = GlobalFunction.getOwnerLicense(LesenID);
            model.GetCompanyDetails = GlobalFunction.getOrganisationLicense(LesenID);
            model.getSpeciesDetails = _PengesahanService.getSpeciesDetails(LesenID);
            model.getTotalSpeciesList = _PengesahanService.getTotalQuantitySpecies(LesenID);
            model.ListStatusLesen = _helper.StatusLesen();
            model.ListStatusPemilik = _helper.StatusPemilik();
            model.ListDocument = _PengesahanService.getListDocument(LesenID);
            model.GetAddressDetails = GlobalFunction.getAddressLicense(LesenID, 4);

            model.ActiveTabIndex = 2; // SET STATUS = 2



            return View(model);
        }

    }
}