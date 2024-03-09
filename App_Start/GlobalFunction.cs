using Antlr.Runtime;
using Antlr.Runtime.Misc;
using ePerhilitanV2.Infrastructures.Helpers;
using ePerhilitanV2.Infrastructures.Services;
using ePerhilitanV2.Models;
using ePerhilitanV2.Models.ViewModels;
using ePerhilitanV2.Models.ViewModels.JqueryDatatableParamVM;
using ePerhilitanV2.Models.ViewModels.PengesahanViewModel;
using Microsoft.Owin.BuilderProperties;
using Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace ePerhilitanV2
{
    public class GlobalFunction
    {
        private static CultureInfo ci = new CultureInfo("en-US");
        private static AccountService _accountService = new AccountService();

        #region Encode Decode
        public static string FromBase64String(string s)
        {
            string base64Encoded = s;
            byte[] data = System.Convert.FromBase64String(base64Encoded);
            string base64Decoded = System.Text.ASCIIEncoding.ASCII.GetString(data);
            return base64Decoded;
        }

        public static string ToBase64String(string s)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(s);
            string data = System.Convert.ToBase64String(bytes);

            return data;
        }

        //------------------End Base64------------------------------

        //----------------Encode Base64 + Salt-------------------------

        public static string encodeThis(string str)
        {
            var salt = "sz"; // try to keep it in 2 character
            var salted = ToBase64String(str + salt);

            var reversed = string.Join("", salted.Reverse());
            //var encoded = ToBase64String(reversed.ToString()).Replace("+/", "-_").TrimEnd('=');//OLD

            //new
            var encoded = ToBase64String(reversed.ToString());//old

            string converted = encoded.Replace('-', '+');
            encoded = converted.Replace('_', '/');

            return encoded;
        }

        public static string decodeThis(string str)
        {
            var input = str.Replace("-_", "+/").PadRight(str.Length % 4, '=');
            var decoded = "";

            try
            {
                decoded = FromBase64String(input);
            }
            catch (Exception)
            {
                try
                {
                    decoded = FromBase64String(input.Replace('-', '+').Replace('_', '/'));
                }
                catch (Exception) { }
                try
                {
                    decoded = FromBase64String(input.Replace('-', '+').Replace('_', '/') + "=");
                }
                catch (Exception) { }
                try
                {
                    decoded = FromBase64String(input.Replace('-', '+').Replace('_', '/') + "==");
                }
                catch (Exception) { }
            }

            var unreversed = string.Join("", decoded.Reverse());
            var decrypted_id_raw = FromBase64String(unreversed);
            var decrypted_id = decrypted_id_raw.Remove(decrypted_id_raw.Length - 2);

            return decrypted_id;
        }

        internal static string decodeThis(int faq_id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Get Data Table 
        public static string getData(string column, string table, string pk, string pk_id)
        {

            var _ctx = new eLesen_SPJPEntities();

            var sp_status = _ctx.sp_get_data(column, table, pk, pk_id);
            var status = sp_status.FirstOrDefault();
            if (status != null)
            {
                return status.ToString();
            }
            return null;
        }
        #endregion

        #region Format
        public static string ToTitleCase(string str)
        {
            if (str != null)
            {
                if (str.Length > 0)
                {
                    str = str.ToLower();
                    return ci.TextInfo.ToTitleCase(str);
                }
            }

            return str;

        }

        public static string ToUpperCase(string str)
        {
            if (str != null)
            {
                if (str.Length > 0)
                {
                    str = str.ToLower();
                    return ci.TextInfo.ToUpper(str);
                }
            }

            return str;

        }

        public static string DoFormat(double myNumber)
        {
            var s = string.Format("{0:0.00}", myNumber);

            if (s.EndsWith("00"))
            {
                return ((int)myNumber).ToString();
            }
            else
            {
                return s;
            }
        }
        #endregion

        #region Alamat Lesen (Syarikat/Premis/Surat Menyurat/Semasa/Lesen Menyimpan/Penyelidikan)
        //    <-- Type -->
        //    1- Alamat Syarikat/Premis - Table GBL_LICENSE_MST.lic_org_id == GBL_ORGANIZATION_MST.id (7)
        //    2- Alamat Surat Menyurat (Pemegang Lesen) - Table GBL_LICENSE_MST.lic_civil_id == GBL_CIVILANT_MST.id (4) - AddressType 1
        //    3- Alamat Semasa(Pemegang Lesen) - Table GBL_LICENSE_MST.lic_civil_id == GBL_CIVILANT_MST.id (4)  - AddressType 2
        //    4- Alamat Lesen Menyimpan - Table GBL_LICENSE_MST.id == LicenseId (5)
        //    5- Alamat Penyelidikan - Table GBL_SPESIS_MST.license_id == GBL_LICENSE_MST. id (11)

        public static AddressDetails getAddressLicense(int LicenseId, int type)
        {
            var _ctx = new eLesen_SPJPEntities();

            var OwnerId = 0;
            var OwnerTable = 0;

            if (type == 1)
            {
                OwnerTable = 7;
                var company = (from a in _ctx.GBL_LICENSE_MST
                               join b in _ctx.GBL_ORGANIZATION_MST on a.lic_org_id equals b.id into table1
                               from b in table1.DefaultIfEmpty()
                               where a.id == LicenseId
                               select new
                               {
                                   LicenseId = a.id,
                                   Ownerid = b != null ? b.id : 0,
                               }).FirstOrDefault();

                if (company != null)
                    OwnerId = company.Ownerid;

            }
            else if (type == 2 || type == 3)
            {
                OwnerTable = 4;

                var owner_surat = (from a in _ctx.GBL_LICENSE_MST
                                   join b in _ctx.GBL_CIVILANT_MST on a.lic_civil_id equals b.id into table1
                                   from b in table1.DefaultIfEmpty()
                                   where a.id == LicenseId
                                   select new
                                   {
                                       LicenseId = a.id,
                                       Ownerid = b.id
                                   }).FirstOrDefault();

                if (owner_surat != null)
                    OwnerId = owner_surat.Ownerid;
            }
            else if (type == 4)
            {
                OwnerTable = 5;
                OwnerId = LicenseId;

            }
            else if (type == 5)
            {
                OwnerTable = 11;

                var penyelidikan = (from a in _ctx.GBL_LICENSE_MST
                                    join b in _ctx.GBL_SPESIS_MST on a.id equals b.license_id into table1
                                    from b in table1.DefaultIfEmpty()
                                    where a.id == LicenseId
                                    select new
                                    {
                                        LicenseId = a.id,
                                        Ownerid = b != null ? b.id : 0
                                    }).FirstOrDefault();

                if (penyelidikan != null)
                    OwnerId = penyelidikan.Ownerid;
            }


            var address_1 = _getAddressDetails(type, OwnerId, OwnerTable); //Get Address
            var address = _DisplayAddressDetails(address_1); //Display Address

            return address;
        }

        public static GBL_ADDRESS_MST _getAddressDetails(int type, int OwnerId, int OwnerTable) //sub function
        {
            var _ctx = new eLesen_SPJPEntities();
            var address_1 = new GBL_ADDRESS_MST();

            if (type == 2) //surat menyurat
            {
                address_1 = _ctx.GBL_ADDRESS_MST.Where(x => x.ownerid == OwnerId && x.ownertable == OwnerTable && x.address_type == 1).FirstOrDefault();
            }
            else if (type == 3) //Semasa
            {
                address_1 = _ctx.GBL_ADDRESS_MST.Where(x => x.ownerid == OwnerId && x.ownertable == OwnerTable && x.address_type == 2).FirstOrDefault();
            }
            else
            {
                address_1 = _ctx.GBL_ADDRESS_MST.Where(x => x.ownerid == OwnerId && x.ownertable == OwnerTable).FirstOrDefault();
            }
            return address_1;

        }

        public static AddressDetails _DisplayAddressDetails(GBL_ADDRESS_MST address_1) //sub function
        {
            var _ctx = new eLesen_SPJPEntities();
            var address = new AddressDetails();

            if (address_1 != null)
            {
                var stateName = "";
                var Postcode_No = "";
                var CityName = "";

                var postcode = _ctx.dPostcodes.Where(x => x.id == address_1.postcode).FirstOrDefault();

                if (postcode != null)
                {
                    Postcode_No = postcode.post_no;
                    CityName = postcode.city;

                    var state = _ctx.dStates.Where(x => x.id == postcode.parent_state).FirstOrDefault();

                    stateName = state != null ? state.state_desc : stateName;
                }

                address.Address = ToUpperCase(address_1.address);
                address.PostcodeID = address_1.postcode != null ? (int)address_1.postcode : 0;
                address.Postcode = Postcode_No;
                address.State = ToUpperCase(stateName);
                address.City = ToUpperCase(CityName);

            }

            return address;

        }
        #endregion

        #region Maklumat Pemegang Lesen (Pemilik) / Maklumat Syarikat
        public static OwnerDetails getOwnerLicense(int LicenseId)
        {
            var _ctx = new eLesen_SPJPEntities();
            var details = (from a in _ctx.GBL_LICENSE_MST
                           join b in _ctx.GBL_CIVILANT_MST on a.lic_civil_id equals b.id into table1
                           from b in table1.DefaultIfEmpty()
                           join c in _ctx.GBL_CIVDOC_MST on b.id equals c.civil_id into table2
                           from c in table2.DefaultIfEmpty()
                           join d in _ctx.dNationalities on c.civil_type equals d.id into table3
                           from d in table3.DefaultIfEmpty()
                           where a.id == LicenseId
                           select new OwnerDetails
                           {
                               Warganegara = d != null ? d.national_desc : "",
                               IdNo = c != null ? c.civil_doc : " ",
                               NamaPemegang = b != null ? b.civil_name : " ",
                               Pekerjaan = b != null ? b.civil_pekerjaan1.ToString() : " ",
                               Pekerjaan_Sub = b != null ? b.civil_pekerjaan2.ToString() : " ",
                               NoTel = b != null ? b.civil_tel : "",
                               NoFax = b != null ? b.civil_fax : "",
                               Emel = b != null ? b.civil_email : " ",
                               Alamat_Semasa = new AddressDetails(),
                               Alamat_Surat_Menyurat = new AddressDetails(),
                           }).FirstOrDefault();

            //get details Pekerja
            if (details != null)
            {
                if (details.Pekerjaan != " ")
                {
                    var w_1 = Convert.ToInt32(details.Pekerjaan);

                    var work = (from a in _ctx.dOccupation_1
                                join b in _ctx.dOccupation_2 on a.id equals b.parent_occu_id into table1
                                from b in table1.DefaultIfEmpty()
                                where a.id == w_1
                                select new
                                {
                                    Pekerjaan_1 = a.occu_desc,
                                    Pekerjaan_2 = b != null ? b.sub_occu_desc : ""
                                }).FirstOrDefault();

                    if (work != null)
                    {
                        details.Pekerjaan = ToUpperCase(work.Pekerjaan_1);
                        details.Pekerjaan_Sub = ToUpperCase(work.Pekerjaan_2);
                    }
                }

            }

            details.Alamat_Semasa = getAddressLicense(LicenseId, 3);
            details.Alamat_Surat_Menyurat = getAddressLicense(LicenseId, 2);


            return details;
        }

        public static CompanyDetails getOrganisationLicense(int LicenseId)
        {
            var _ctx = new eLesen_SPJPEntities();
            var details = (from a in _ctx.GBL_LICENSE_MST
                           join b in _ctx.GBL_ORGANIZATION_MST on a.lic_org_id equals b.id into table1
                           from b in table1.DefaultIfEmpty()
                           join c in _ctx.dOrganizationType1 on b.org_type1 equals c.id into table3
                           from c in table3.DefaultIfEmpty()
                           join d in _ctx.dOrganizationDocumentTypes on b.org_roc_type equals d.id into table4
                           from d in table4.DefaultIfEmpty()
                           where a.id == LicenseId
                           select new CompanyDetails
                           {
                               JenisSyarikat = c != null ? c.doc_desc : "",
                               NoSyarikat = b != null ? b.org_roc : " ",
                               TypeSyarikat = d != null ? d.doc_desc : " ",
                               NamaSyarikat = b != null ? b.org_name : " ",
                               NoTel = b != null ? b.org_tel : "",
                               NoFax = b != null ? b.org_fax : "",
                               Alamat_Syarikat = new AddressDetails(),
                           }).FirstOrDefault();

            details.Alamat_Syarikat = getAddressLicense(LicenseId, 1);

            return details;
        }


        #endregion

        #region UserLogin Details

        public static UserDetailsViewModel GetUserLoginDetails(int UserId)
        {
            var _ctx = new eLesen_SPJPEntities();
            var checkData = new UserDetailsViewModel();

            checkData = (from a in _ctx.GBL_USERS_MST
                         join b in _ctx.dNationalities on a.user_nationality equals b.id into table1
                         from b in table1.DefaultIfEmpty()
                         join c in _ctx.GBL_CIVDOC_MST on a.user_name equals c.civil_doc into table2
                         from c in table2.DefaultIfEmpty()
                         join d in _ctx.GBL_CIVILANT_MST on c.civil_id equals d.id into table3
                         from d in table3.DefaultIfEmpty()
                         where a.id == UserId
                         select new UserDetailsViewModel
                         {
                             UserId = UserId,
                             IdNo = a.user_name,
                             NamaPengguna = a.user_fullname,
                             NoTel = a.user_mphone,
                             NoFax = a.user_phone,
                             Emel = a.user_email,
                             Warganegara = b != null ? b.national_desc : "",
                             SatateId = a.user_negeri.HasValue ? (int)a.user_negeri : 0,
                             user_nationality = a.user_nationality != null ? (int)a.user_nationality : 0,
                             BirthDate = a.user_birth_date != null ? a.user_birth_date.ToString() : null,
                             ExpiryDate_Passport = a.user_passport_expired != null ? a.user_passport_expired.ToString() : null,

                         }).FirstOrDefault();

            if (checkData != null)
            {

                //---convert date---
                if (checkData.BirthDate != null)
                {
                    checkData.BirthDate = Convert.ToDateTime(checkData.BirthDate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                if (checkData.ExpiryDate_Passport != null)
                {
                    checkData.ExpiryDate_Passport = Convert.ToDateTime(checkData.ExpiryDate_Passport).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }


                //----check data civilant-----
                var civilant = (from k in _ctx.GBL_CIVDOC_MST
                                join m in _ctx.GBL_CIVILANT_MST on k.civil_id equals m.id
                                where k.civil_doc == checkData.IdNo && k.civil_status == 1
                                select new
                                {
                                    CheckType = 1,  //1 - Dah ada data civilant , 0- Tiada dalam civilant
                                    CivilId = k.civil_id,
                                    Pekerjaan = m.civil_pekerjaan1 != null ? m.civil_pekerjaan1.ToString() : "",
                                    Pekerjaan_Sub = m.civil_pekerjaan2 != null ? m.civil_pekerjaan2.ToString() : "",
                                    Pekerjaan_Id = m.civil_pekerjaan1 != null ? (int)m.civil_pekerjaan1 : 0,
                                    CivilTypeId = k.civil_type != null ? k.civil_type : 0,
                                    PassportChecked = k.passport_checked,
                                }).FirstOrDefault();

                if (civilant != null)
                {
                    var typee = _ctx.dDocumentTypes.Find(civilant.CivilTypeId);

                    //--- re-assign---
                    checkData.CheckType = civilant.CheckType;
                    checkData.CivilId = (int)civilant.CivilId;
                    checkData.Pekerjaan = civilant.Pekerjaan;
                    checkData.Pekerjaan_Sub = civilant.Pekerjaan_Sub;
                    checkData.Pekerjaan_Id = civilant.Pekerjaan_Id;
                    checkData.Civil_Type = typee != null ? typee.doc_desc : "";
                    checkData.Passport_Verified = civilant.PassportChecked != null ? (bool)civilant.PassportChecked : false;

                    //------ get data address --------
                    if (checkData.CheckType == 1) //dah pernah login dan dah update profile
                    {
                        var address_1 = _getAddressDetails(3, checkData.CivilId, 4); //Get Address
                        checkData.Alamat_Semasa = _DisplayAddressDetails(address_1); //Display Address

                        var address_2 = _getAddressDetails(2, checkData.CivilId, 4); //Get Address
                        checkData.Alamat_Surat_Menyurat = _DisplayAddressDetails(address_2); //Display Address
                    }

                    //----- get pekerjaan ------
                    if (checkData.Pekerjaan != "")
                    {
                        var w_1 = Convert.ToInt32(checkData.Pekerjaan);

                        var work = (from a in _ctx.dOccupation_1
                                    join b in _ctx.dOccupation_2 on a.id equals b.parent_occu_id into table1
                                    from b in table1.DefaultIfEmpty()
                                    where a.id == w_1
                                    select new
                                    {
                                        Pekerjaan_1 = a.occu_desc,
                                        Pekerjaan_2 = b != null ? b.sub_occu_desc : ""
                                    }).FirstOrDefault();

                        if (work != null)
                        {
                            checkData.Pekerjaan = ToUpperCase(work.Pekerjaan_1);
                            checkData.Pekerjaan_Sub = ToUpperCase(work.Pekerjaan_2);
                        }
                    }
                }



                //----- get userRoles------
                if (_accountService != null)
                {
                    checkData.UserRoles = _accountService.getRole(UserId);
                    checkData.isStaff = _accountService.isStaff(UserId);
                }


            }


            return checkData;
        }


        #endregion

        #region Insert Log
        public static string Add_log(AuditLogViewModel model)
        {
            var _ctx = new eLesen_SPJPEntities();
            audit_log obj_sam = new audit_log();
            obj_sam.tahap_id = model.Tahap; //dtahap permohonan
            obj_sam.status_id = model.Status; //dstatus permohonan
            obj_sam.license_id = model.LicenseId; //id GBL_LICENSE_MST lesen,permit, permit khas
            obj_sam.rec_created_by = model.ActionBy;
            obj_sam.rec_created_by_roleId = model.ActionBy_RoleId; // dPosition
            obj_sam.remarks = model.Remarks; //add on
            obj_sam.rec_created_date = Convert.ToDateTime(DateTime.Now);

            _ctx.audit_log.Add(obj_sam);
            _ctx.SaveChangesAsync();

            return "ok";
        }
        #endregion

        #region constant
        public static class Constants
        {
            public static string modulnameLocal = "";
            public static string modulnameServer = "/eLesenPerhilitan";
            public static string baseurlServer = "https://simpleadvantage.dyndns.biz" + modulnameServer;//server
            public static string baseurlLocal = "https://localhost:44325/";//opis
        }
        #endregion

        #region Details State based on Postcode

        public static PostcodeViewModel getPostcodeDetails(int postcodeId)
        {
            var _ctx = new eLesen_SPJPEntities();
            var postcode = (from a in _ctx.dPostcodes
                            join b in _ctx.dStates on a.parent_state equals b.id into table1
                            from b in table1.DefaultIfEmpty()
                            where a.id == postcodeId && a.publish == true
                            select new PostcodeViewModel
                            {
                                PostcodeId = a.id,
                                PostcodeName = a.post_no,
                                CityName = a.city,
                                StateName = b != null ? b.state_desc : "",
                            }).FirstOrDefault();

            return postcode;
        }

        public static List<SelectListItem> getPostcodeSelect(string postcodeN = "")
        {
            var _ctx = new eLesen_SPJPEntities();

            if (postcodeN == "")
            {
                return (from a in _ctx.dPostcodes
                        join b in _ctx.dStates on a.parent_state equals b.id into table1
                        from b in table1.DefaultIfEmpty()
                        where a.publish == true
                        select new SelectListItem
                        {
                            Text = a.post_no + " - " + a.city + " - " + b.state_desc,
                            Value = a.id.ToString()
                        }).ToList();
            }
            else
            {
                return (from a in _ctx.dPostcodes
                        join b in _ctx.dStates on a.parent_state equals b.id into table1
                        from b in table1.DefaultIfEmpty()
                        where a.post_no.Equals(postcodeN) && a.publish == true
                        select new SelectListItem
                        {
                            Text = a.post_no + " - " + a.city + " - " + b.state_desc,
                            Value = a.id.ToString()
                        }).ToList();
            }



        }
        #endregion

        #region Check User/Civilant based on Id No 

        public static UserDetailsViewModel GetCivilantDetailsbyCivilId(int civilId)
        {
            var _ctx = new eLesen_SPJPEntities();
            var checkData = new UserDetailsViewModel();

            checkData = (from a in _ctx.GBL_CIVDOC_MST
                         join b in _ctx.GBL_CIVILANT_MST on a.civil_id equals b.id
                         join c in _ctx.dDocumentTypes on a.civil_type equals c.id into table2
                         from c in table2.DefaultIfEmpty()
                         join d in _ctx.GBL_USERS_MST on a.civil_doc equals d.user_name into table3
                         from d in table3.DefaultIfEmpty()
                         where a.civil_id == civilId && a.civil_status == 1
                         select new UserDetailsViewModel
                         {
                             UserId = d != null ? d.id : 0,
                             IdNo = a.civil_doc,
                             NamaPengguna = b.civil_name,
                             NoTel = b.civil_tel,
                             NoFax = b.civil_fax,
                             Emel = b.civil_email,
                             Warganegara = a.civil_type == 2 ? Resources.Resource.NonMalaysian : Resources.Resource.Malaysian,
                             CivilId = b.id,
                             Pekerjaan = b.civil_pekerjaan1 != null ? b.civil_pekerjaan1.ToString() : "",
                             Pekerjaan_Sub = b.civil_pekerjaan2 != null ? b.civil_pekerjaan2.ToString() : "",
                             Pekerjaan_Id = b.civil_pekerjaan1 != null ? (int)b.civil_pekerjaan1 : 0,
                             BirthDate = a.civil_birth_date != null ? a.civil_birth_date.ToString() : null,
                             ExpiryDate_Passport = a.civil_passport_expired != null ? a.civil_passport_expired.ToString() : null,
                             CheckType = 1,
                             user_nationality = c != null ? (int)c.type_status : 1,
                             Civil_Type = c != null ? c.doc_desc : "",
                             Passport_Verified = a.passport_checked != null ? (bool)a.passport_checked : false,

                         }).FirstOrDefault();

            if (checkData != null)
            {
                //---convert date---
                if (checkData.BirthDate != null)
                {
                    checkData.BirthDate = Convert.ToDateTime(checkData.BirthDate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (checkData.ExpiryDate_Passport != null)
                {
                    checkData.ExpiryDate_Passport = Convert.ToDateTime(checkData.ExpiryDate_Passport).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                //------ get data address --------
                var address_1 = _getAddressDetails(3, checkData.CivilId, 4); //Get Address
                checkData.Alamat_Semasa = _DisplayAddressDetails(address_1); //Display Address

                var address_2 = _getAddressDetails(2, checkData.CivilId, 4); //Get Address
                checkData.Alamat_Surat_Menyurat = _DisplayAddressDetails(address_2); //Display Address

                //----- get pekerjaan ------
                if (checkData.Pekerjaan != "")
                {
                    var w_1 = Convert.ToInt32(checkData.Pekerjaan);

                    var work = (from a in _ctx.dOccupation_1
                                join b in _ctx.dOccupation_2 on a.id equals b.parent_occu_id into table1
                                from b in table1.DefaultIfEmpty()
                                where a.id == w_1
                                select new
                                {
                                    Pekerjaan_1 = a.occu_desc,
                                    Pekerjaan_2 = b != null ? b.sub_occu_desc : ""
                                }).FirstOrDefault();

                    if (work != null)
                    {
                        checkData.Pekerjaan = ToUpperCase(work.Pekerjaan_1);
                        checkData.Pekerjaan_Sub = ToUpperCase(work.Pekerjaan_2);
                    }
                }

            }


            return checkData;
        }

        public static UserDetailsViewModel GetCivilantDetailsbyIdno(string IdNo)
        {
            var _ctx = new eLesen_SPJPEntities();
            var checkData = new UserDetailsViewModel();

            checkData = (from a in _ctx.GBL_CIVDOC_MST
                         join b in _ctx.GBL_CIVILANT_MST on a.civil_id equals b.id into table1
                         from b in table1.DefaultIfEmpty()
                         join c in _ctx.dDocumentTypes on a.civil_type equals c.id into table2
                         from c in table2.DefaultIfEmpty()
                         join d in _ctx.GBL_USERS_MST on a.civil_doc equals d.user_name into table3
                         from d in table3.DefaultIfEmpty()
                         where a.civil_doc == IdNo && a.civil_status == 1
                         select new UserDetailsViewModel
                         {
                             UserId = d != null ? d.id : 0,
                             IdNo = a.civil_doc,
                             NamaPengguna = b.civil_name,
                             NoTel = b.civil_tel,
                             NoFax = b.civil_fax,
                             Emel = b.civil_email,
                             Warganegara = a.civil_type == 2 ? Resources.Resource.NonMalaysian : Resources.Resource.Malaysian,
                             CivilId = b.id,
                             Pekerjaan = b != null ? b.civil_pekerjaan1.ToString() : "",
                             Pekerjaan_Sub = b != null ? b.civil_pekerjaan2.ToString() : "",
                             Pekerjaan_Id = b != null ? (int)b.civil_pekerjaan1 : 0,
                             BirthDate = a.civil_birth_date != null ? a.civil_birth_date.ToString() : null,
                             ExpiryDate_Passport = a.civil_passport_expired != null ? a.civil_passport_expired.ToString() : null,
                             CheckType = 1,
                             user_nationality = c != null ? (int)c.type_status : 1,
                             Civil_Type = c != null ? c.doc_desc : "",
                             Passport_Verified = a.passport_checked != null ? (bool)a.passport_checked : false,

                         }).FirstOrDefault();

            if (checkData != null)
            {
                //---convert date---
                if (checkData.BirthDate != null)
                {
                    checkData.BirthDate = Convert.ToDateTime(checkData.BirthDate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (checkData.ExpiryDate_Passport != null)
                {
                    checkData.ExpiryDate_Passport = Convert.ToDateTime(checkData.ExpiryDate_Passport).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                //------ get data address --------
                var address_1 = _getAddressDetails(3, checkData.CivilId, 4); //Get Address
                checkData.Alamat_Semasa = _DisplayAddressDetails(address_1); //Display Address

                var address_2 = _getAddressDetails(2, checkData.CivilId, 4); //Get Address
                checkData.Alamat_Surat_Menyurat = _DisplayAddressDetails(address_2); //Display Address

                //----- get pekerjaan ------
                if (checkData.Pekerjaan != "")
                {
                    var w_1 = Convert.ToInt32(checkData.Pekerjaan);

                    var work = (from a in _ctx.dOccupation_1
                                join b in _ctx.dOccupation_2 on a.id equals b.parent_occu_id into table1
                                from b in table1.DefaultIfEmpty()
                                where a.id == w_1
                                select new
                                {
                                    Pekerjaan_1 = a.occu_desc,
                                    Pekerjaan_2 = b != null ? b.sub_occu_desc : ""
                                }).FirstOrDefault();

                    if (work != null)
                    {
                        checkData.Pekerjaan = ToUpperCase(work.Pekerjaan_1);
                        checkData.Pekerjaan_Sub = ToUpperCase(work.Pekerjaan_2);
                    }
                }

            }


            return checkData;
        }
        #endregion

        #region Semak Status Lesen

        public static List<StatusLesen> getStatusBatalLesen(int civilId)
        {
            var _ctx = new eLesen_SPJPEntities();
            //var checkStatusLesen = new StatusLesen();

            var checkStatusLesen = (from a in _ctx.GBL_LICENSE_MST
                                    join b in _ctx.GBL_LICENSE_SLV on a.id equals b.parent_id
                                    join c in _ctx.dStates on a.negeri_id equals c.id
                                    join d in _ctx.dStatus on a.status equals d.id.ToString()
                                    join e in _ctx.dLicenseDetails on a.lic_type equals e.id
                                    where a.lic_civil_id == civilId && (a.status == "13" || a.status == "14" || a.status == "16" || a.status == "18" || a.status == "10" || a.status == "17")
                                    orderby e.desc_LicenseDetail
                                    select new StatusLesen
                                    {
                                        LesenID = a.id,
                                        JenisLesen = (int)a.lic_type,
                                        NoLesen = a != null ? a.lic_no : "",
                                        NamaLesen = e.desc_LicenseDetail,
                                        PejabatPengeluar = c.state_desc,
                                        TarikhMula = b.new_start_date.ToString(),
                                        TarikhTamat = b.new_last_date.ToString(),
                                        Status = d.desc_status,
                                        Catatan = a.catat_periksa

                                    }).ToList();

            return checkStatusLesen;
        }
        public static List<StatusLesen> getKuatkuasaLesen(int civilId)
        {
            var _ctx = new eLesen_SPJPEntities();
            //var checkStatusLesen = new StatusLesen();
            DateTime dateNow = DateTime.Now;

            var checkStatusLesen = (from a in _ctx.GBL_LICENSE_MST
                                    join b in _ctx.GBL_LICENSE_SLV on a.id equals b.parent_id
                                    join c in _ctx.dStates on a.negeri_id equals c.id
                                    join d in _ctx.dStatus on a.status equals d.id.ToString()
                                    join e in _ctx.dLicenseDetails on a.lic_type equals e.id
                                    where a.lic_civil_id == civilId && b.new_last_date > dateNow //&& a.status == "9"
                                    orderby e.desc_LicenseDetail
                                    select new StatusLesen
                                    {
                                        LesenID = a.id,
                                        JenisLesen = (int)a.lic_type,
                                        NoLesen = a != null ? a.lic_no : "",
                                        NamaLesen = e.desc_LicenseDetail,
                                        PejabatPengeluar = c.state_desc,
                                        TarikhMula = b.new_start_date.ToString(),
                                        TarikhTamat = b.new_last_date.ToString(),
                                        Status = d.desc_status,
                                        Catatan = a.catat_periksa

                                    }).ToList();

            return checkStatusLesen;
        }

        public static List<StatusLesen> getPermohonanLesen(int civilId)
        {
            var _ctx = new eLesen_SPJPEntities();
            //var checkStatusLesen = new StatusLesen();
            DateTime dateNow = DateTime.Now;

            var checkStatusLesen = (from a in _ctx.GBL_LICENSE_MST
                                    join b in _ctx.GBL_LICENSE_SLV on a.id equals b.parent_id
                                    join c in _ctx.dStates on a.negeri_id equals c.id
                                    join d in _ctx.dStatus on a.status equals d.id.ToString()
                                    join e in _ctx.dLicenseDetails on a.lic_type equals e.id
                                    where a.lic_civil_id == civilId && b.new_last_date > dateNow //&& a.status == "9"
                                    orderby e.desc_LicenseDetail
                                    select new StatusLesen
                                    {
                                        LesenID = a.id,
                                        JenisLesen = (int)a.lic_type,
                                        NoLesen = a != null ? a.lic_no : "",
                                        NamaLesen = e.desc_LicenseDetail,
                                        PejabatPengeluar = c.state_desc,
                                        TarikhMula = b.new_start_date.ToString(),
                                        TarikhTamat = b.new_last_date.ToString(),
                                        Status = d.desc_status,
                                        Catatan = a.catat_periksa

                                    }).ToList();

            return checkStatusLesen;
        }
        public static List<StatusLesen> getTamatTempohLesen(int civilId)
        {
            var _ctx = new eLesen_SPJPEntities();
            //var checkStatusLesen = new StatusLesen();
            DateTime dateNow = DateTime.Now;

            var checkStatusLesen = (from a in _ctx.GBL_LICENSE_MST
                                    join b in _ctx.GBL_LICENSE_SLV on a.id equals b.parent_id
                                    join c in _ctx.dStates on a.negeri_id equals c.id
                                    join d in _ctx.dStatus on a.status equals d.id.ToString()
                                    join e in _ctx.dLicenseDetails on a.lic_type equals e.id
                                    where a.lic_civil_id == civilId && b.new_last_date < dateNow && a.status == "9"    //9 - aktif
                                    orderby e.desc_LicenseDetail
                                    select new StatusLesen
                                    {
                                        LesenID = a.id,
                                        JenisLesen = (int)a.lic_type,
                                        NoLesen = a != null ? a.lic_no : "",
                                        NamaLesen = e.desc_LicenseDetail,
                                        PejabatPengeluar = c.state_desc,
                                        TarikhMula = b.new_start_date.ToString(),
                                        TarikhTamat = b.new_last_date.ToString(),
                                        Status = d.desc_status,
                                        Catatan = a.catat_periksa

                                    }).ToList();

            return checkStatusLesen;
        }
        #endregion

        #region ViewSemakan Lesen
        public static StatusLesen getAllViewLesen(int LesenID)
        {
            var _ctx = new eLesen_SPJPEntities();
            var allLesen = (from a in _ctx.GBL_LICENSE_MST
                            join c in _ctx.dLicenseDetails on a.lic_type equals c.id
                            where a.id == LesenID
                            select new StatusLesen
                            {
                                NamaLesen = c.desc_LicenseDetail,
                                JenisLesen = (int)a.lic_type,

                            }).FirstOrDefault();

            if (allLesen != null)
            {
                if (allLesen.JenisLesen == 10 || allLesen.JenisLesen == 11)  //PERMIT//PERMITKHAS
                {
                    var PermitPermitKhas = (from a in _ctx.GBL_LICENSE_MST
                                            join f in _ctx.GBL_ATTACHMENT_MST on a.id equals f.license_id into table1
                                            from f in table1.DefaultIfEmpty()
                                            join g in _ctx.GBL_PERMIT_MST on a.id equals g.license_id into table2
                                            from g in table2.DefaultIfEmpty()
                                            join h in _ctx.dLicenseType_Permit on g.permit_type equals h.id.ToString() into table3
                                            from h in table3.DefaultIfEmpty()
                                            join k in _ctx.GBL_SPESIS_MST on a.id equals k.license_id into table4
                                            from k in table4.DefaultIfEmpty()
                                            join i in _ctx.dSpesisNames on k.spesis_id equals i.id
                                            join m in _ctx.GBL_PERMITKHAS_MST on a.id equals m.license_id into table5
                                            from m in table5.DefaultIfEmpty()
                                            join n in _ctx.GBL_IMPORT_MST on a.id equals n.license_id into table6
                                            from n in table6.DefaultIfEmpty()
                                            join p in _ctx.dLicenseType_PermitKhas on a.lic_type_2 equals p.id.ToString() into table7
                                            from p in table7.DefaultIfEmpty()
                                            where a.id == LesenID
                                            select new ViewSemakanPermit
                                            {
                                                JenisPermit = h != null ? h.lic_type_desc : "",
                                                TarikhKelulusan = a != null ? a.date_kelulusan.ToString() : "",
                                                NoSuratKelulusan = a != null ? a.no_kelulusan : "",
                                                Spesis = i.spp_saintifik,
                                                NoPenandaan = k != null ? k.penandaan : "",
                                                NoCitesNegaraAsal = k != null ? n.cites_asal : "",
                                                NoCR = k != null ? k.cr : "",
                                                Jantina = k != null ? k.jantina : "",
                                                Melalui = n != null ? n.melalui ?? 0 : 0,
                                                JenisPermitKhas = p != null ? p.lic_name : "",
                                                TarikhKelulusanPK = m != null ? m.date_surat.ToString() : "",
                                                NoSuratKelulusanPK = m != null ? m.surat : "",
                                                Specimen = k != null ? k.spesimen : "",

                                            }).FirstOrDefault();

                    var jantina = PermitPermitKhas.Jantina;
                    var melalui = PermitPermitKhas.Melalui;

                    if (PermitPermitKhas != null)
                    {
                        if (jantina == "1")
                        {
                            PermitPermitKhas.NamaJantina = Resources.Verification._Jantan;
                        }
                        else if (jantina == "2")
                        {
                            PermitPermitKhas.NamaJantina = Resources.Verification._Betina;
                        }
                        else if (jantina == "3")
                        {
                            PermitPermitKhas.NamaJantina = Resources.Verification._TidakPasti;
                        }

                        var namaMelalui = _ctx.dImportEksportCenters.Where(a => a.id == melalui).Select(a => a.center_name).FirstOrDefault();
                        PermitPermitKhas.NamaMelalui = ToUpperCase(namaMelalui);

                        allLesen.ViewSemakanPermit = PermitPermitKhas;
                    }
                }

                else if (allLesen.JenisLesen == 1 || allLesen.JenisLesen == 2)  // Memburu/Mengambil
                {
                    var buruAmbilMemungut = (from a in _ctx.GBL_LICENSE_MST
                                             join b in _ctx.GBL_ARM_MST on a.id equals b.license_id into table1
                                             from b in table1.DefaultIfEmpty()
                                             join c in _ctx.dLicenseDetails on a.lic_type equals c.id
                                             join d in _ctx.GBL_SPESIS_MST on a.id equals d.license_id
                                             join e in _ctx.dSpesisNames on d.spesis_id equals e.id
                                             join f in _ctx.GBL_ATTACHMENT_MST on a.id equals f.license_id into table2
                                             from f in table2.DefaultIfEmpty()
                                             where a.id == LesenID
                                             select new ViewSemakanLesenMemburuMengambil
                                             {
                                                 NoBuku = b != null ? b.arm_book_no : "",
                                                 NoSiriBuKuSenjataApi = b != null ? b.arm_serial_no : "",
                                                 Daerah = b.daerah,
                                                 TarikhBukuSenjataApi = b != null ? b.arm_date.ToString() : "",
                                                 JenisSenjataApi = b.jenis,
                                                 NamaPembuatdanNombor = b.nama_pembuat,
                                                 SyaratMemburu = b != null ? b.arm_condition ?? false : false,   //SyaratMemburu will be assigned b.arm_condition if b is not null and b.arm_condition is not null.
                                                 NamaSaintifik = e != null ? e.spp_saintifik : "",
                                                 Kuantiti = d != null ? d.quantity : "",
                                                 Tempoh = d != null ? d.tempoh ?? 0 : 0,

                                             }).FirstOrDefault();

                    if (buruAmbilMemungut != null)
                    {
                        allLesen.ViewSemakanLesenMemburuMengambil = buruAmbilMemungut;
                    }
                }

                else if (allLesen.JenisLesen == 3 || allLesen.JenisLesen == 4 || allLesen.JenisLesen == 13)  //simpanGunaKajian
                {
                    var menyimpanMenggunaKajian = (from a in _ctx.GBL_LICENSE_MST
                                                   join c in _ctx.dLicenseDetails on a.lic_type equals c.id
                                                   join d in _ctx.GBL_SPESIS_MST on a.id equals d.license_id
                                                   join e in _ctx.dSpesisNames on d.spesis_id equals e.id
                                                   join f in _ctx.GBL_PEROLEHAN_MST on a.id equals f.parent_license_id into table1
                                                   from f in table1.DefaultIfEmpty()
                                                   where a.id == LesenID
                                                   select new ViewSemakanLesenMenyimpanMenggunaKajian
                                                   {
                                                       Spesis = e != null ? e.spp_saintifik : "",
                                                       NoPenandaan = d != null ? d.penandaan : "",
                                                       Jantina = d != null ? d.jantina : "",
                                                       CatatanPerolehan = a != null ? a.lic_no : "",
                                                       NamaSyarikat = f != null ? f.nama_syarikat : "",
                                                       BakiDalamSimpanan = f != null ? f.baki ?? 0 : 0,
                                                       NoResit = f != null ? f.resit : "",
                                                       NoLesenPerniagaan = f != null ? f.lesen_niaga : "",

                                                   }).FirstOrDefault();

                    var jantina = menyimpanMenggunaKajian.Jantina;
                    if (menyimpanMenggunaKajian != null)
                    {
                        if (jantina == "1")
                        {
                            menyimpanMenggunaKajian.NamaJantina = Resources.Verification._Jantan;
                        }
                        else if (jantina == "2")
                        {
                            menyimpanMenggunaKajian.NamaJantina = Resources.Verification._Betina;
                        }
                        else if (jantina == "3")
                        {
                            menyimpanMenggunaKajian.NamaJantina = Resources.Verification._TidakPasti;
                        }

                        allLesen.ViewSemakanLesenMenyimpanMenggunaKajian = menyimpanMenggunaKajian;
                    }


                }

                else if (allLesen.JenisLesen == 5 || allLesen.JenisLesen == 6)  //niagaTaksidermi
                {
                    var niagaTaksidermi = (from a in _ctx.GBL_LICENSE_MST
                                           join c in _ctx.dLicenseDetails on a.lic_type equals c.id
                                           join f in _ctx.GBL_ATTACHMENT_MST on a.id equals f.license_id into table1
                                           from f in table1.DefaultIfEmpty()
                                           join g in _ctx.GBL_PREMIS_MST on a.id equals g.license_id into table2
                                           from g in table2.DefaultIfEmpty()
                                           join h in _ctx.dLicenseType_License on g.premis_business equals h.id into table3
                                           from h in table3.DefaultIfEmpty()
                                           where a.id == LesenID
                                           select new ViewSemakanLesenNiagaTaksidermi
                                           {
                                               KategoriPerniagaan = h != null ? h.lic_type_desc : "",
                                               SalinanDaftarSyarikat = f != null ? f.attach_type : "Tiada Lampiran",
                                               SalinankelulusanPBT = f != null ? f.attach_type : "Tiada Lampiran",
                                               AktivitiLesen = g != null ? g.aktiviti ?? 0 : 0, //jika null,assign kan value 0 pada aktiviti

                                           }).FirstOrDefault();

                    int AktivitiLesen = niagaTaksidermi.AktivitiLesen;

                    if (niagaTaksidermi != null)
                    {
                        if (AktivitiLesen != null)
                        {
                            var aktiviLesenDescr = _ctx.dLicenseType_License.Where(x => x.id == AktivitiLesen).Select(x => x.lic_type_desc).FirstOrDefault();
                            niagaTaksidermi.AktivitiLesenDescr = aktiviLesenDescr;
                        }

                        allLesen.ViewSemakanLesenNiagaTaksidermi = niagaTaksidermi;
                    }

                }

                else if (allLesen.JenisLesen == 7 || allLesen.JenisLesen == 8 || allLesen.JenisLesen == 9)
                {
                    var importEskport = (from a in _ctx.GBL_LICENSE_MST
                                         join i in _ctx.GBL_SPESIS_MST on a.id equals i.license_id
                                         join j in _ctx.dSpesisNames on i.spesis_id equals j.id
                                         join m in _ctx.GBL_IMPORT_MST on a.id equals m.license_id
                                         where a.id == LesenID
                                         select new ViewSemakanImportEksportReEksport
                                         {
                                             Specimen = i != null ? i.spesimen : "",
                                         }).FirstOrDefault();

                    //var specimen = importEskport.Specimen;

                    if (importEskport != null)
                    {
                        var specimen = importEskport.Specimen;
                        if (specimen != null)
                        {
                            allLesen.ViewSemakanImportEksportReEksport = importEskport;
                        }

                    }

                }
            }

            return allLesen;
        }
        #endregion

        #region convert jadual

        static string[] ordinalWordsBM = { "", "Pertama", "Kedua", "Ketiga", "Keempat", "Kelima", /* Add more as needed */ };
        static string[] ordinalWordsBI = { "", "Pertama", "Kedua", "Ketiga", "Keempat", "Kelima", /* Add more as needed */ };

        public static string NumToWordBM(int number)
        {
            if (number < 1 || number > ordinalWordsBM.Length - 1)
            {
                return "Luar jangkauan";
            }

            return ordinalWordsBM[number];
        }

        public static string NumToWordBI(int number)
        {
            if (number < 1 || number > ordinalWordsBI.Length - 1)
            {
                return "Luar jangkauan";
            }

            return ordinalWordsBI[number];
        }
        #endregion
    }




    public enum TitleCase
    {
        First,
        All
    }
}