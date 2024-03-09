using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ePerhilitanV2.Infrastructures.Interfaces;
using ePerhilitanV2.Models;
using ePerhilitanV2.Models.ViewModels.PengesahanViewModel;
using ePerhilitanV2.Infrastructures.Helpers;
using ePerhilitanV2.Models.ViewModels.JqueryDatatableParamVM;
using System.Web.Mvc;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Ajax.Utilities;
using System.Data.Entity.Validation;
using System.Runtime.Remoting.Contexts;
using System.Diagnostics;

namespace ePerhilitanV2.Infrastructures.Services
{
    public class PengesahanService
    {
        private ISelectListItemHelper _helper;
        private eLesen_SPJPEntities _context;
        public PengesahanService()
        {
            _context = new eLesen_SPJPEntities();
            _helper = new SelectListItemHelper();

        }

        public ImportExport GetIEDetails(int id)
        {
            var importExport = (from a in _context.GBL_LICENSE_MST
                                join d in _context.GBL_IMPORT_MST on a.id equals d.license_id
                                join j in _context.dLicenseDetails on a.lic_type equals j.id
                                join k in _context.dImportEksportCenters on d.melalui equals k.id
                                join l in _context.dCodePurposeTrades on d.tujuan_import equals l.Code into table1
                                from l in table1.DefaultIfEmpty()
                                where id == a.id

                                select new ImportExport
                                {
                                    Through = k.center_name,
                                    Type_Matter = d.urusan,
                                    License_Id = a.id,
                                    //Civil_Id = (int)a.lic_civil_id,
                                    License_Type = j.desc_LicenseDetail,
                                    Cites_No = d != null ? d.cites_no : "",
                                    Sah_Sehingga = d != null ? d.sah_sehingga.ToString() : "", //check if null data
                                    Jenis_Permit = d.jenis_permit,
                                    Nama_Eksport = d.nama_export,
                                    Negara_Eksport = d.negara_eksport,
                                    Alamat_Pengeksport = d.alamat_export,
                                    Nama_Import = d.nama_import,
                                    Negara_Import = d.negara_import,
                                    Alamat_Pengimport = d.alamat_import,
                                    Tujuan = l.Code,
                                    No_Stem = d.no_setem,
                                    Nama_Pemeriksa = d.nama_pemeriksa,
                                    Lokasi_Pemeriksaan = d.lokasi_pemeriksaan,
                                    //Lokasi_Pemeriksaan = (int)d.lokasi_pemeriksaan,
                                    Tarikh_MasaSemakan = d != null ? d.tarikh_masa_semakan.ToString() : "",
                                    Catatan_Penyemak = d.catatan_semakan,
                                    Ulasan_Pengesah = d.ulasan_pengesah,
                                    Status_By_Penyemak = d.status_pengesahan,
                                    Status_By_Pengesah = d.status_pegawai_pengesah,
                                    Nama_Pengesah = d.id_pelulus,
                                    Pegawai_Penyemak = d.id_penyemak,
                                }).FirstOrDefault();

            return importExport;
        }

        #region Species

        public Species getSpeciesDetails(int id)
        {
            var speciesDetail = (from e in _context.GBL_LICENSE_MST
                                 join i in _context.GBL_SPESIS_MST on e.id equals i.license_id
                                 join j in _context.dSpesisNames on i.spesis_id equals j.id
                                 join m in _context.GBL_IMPORT_MST on e.id equals m.license_id
                                 where id == e.id && i.quantity != ""
                                 select new Species
                                 {
                                     Spesis_Name_Scientific = j.spp_saintifik,
                                     Spesis_Name_Tempatan = j.spp_tempatan,
                                     Quantity = i.quantity,
                                     Specimen = i.spesimen,
                                     Gender = i.jantina,
                                     Source = i.sumber,
                                     Appendix = i.appendiks,
                                     Spesis_Quantity_Disahkan = (float?)i.qty_endorsed,
                                     Spesis_Id = (int)i.spesis_id,
                                     Main_Spesis_Id = (int)i.spesis_id,
                                     Catatan = i.remarks,   //kena check condition jgk tkut null
                                     Original_CitesNo = m.cites_asal,
                                     No_Penandaan = i.penandaan,
                                     No_CR = i.cr,

                                 }).FirstOrDefault();

            if (speciesDetail != null)
            {

                if (speciesDetail.Gender == "1")
                {
                    speciesDetail.NamaJantina = "JANTAN";
                }
                else if (speciesDetail.Gender == "2")
                {
                    speciesDetail.NamaJantina = "BETINA";
                }
                else if (speciesDetail.Gender == "3")
                {
                    speciesDetail.NamaJantina = "TIDAK PASTI";
                }

            }

            return speciesDetail;
        }
        public List<Species> GetSpeciesList(int id)
        {
            float? c = 0;
            var SpeciesList = (from e in _context.GBL_LICENSE_MST
                               join i in _context.GBL_SPESIS_MST on e.id equals i.license_id
                               join j in _context.dSpesisNames on i.spesis_id equals j.id
                               join m in _context.GBL_IMPORT_MST on e.id equals m.license_id
                               where id == e.id && i.quantity != ""
                               select new Species
                               {
                                   Spesis_Name_Scientific = j.spp_saintifik,
                                   Spesis_Name_Tempatan = j.spp_tempatan,
                                   Quantity = i.quantity,
                                   Specimen = i.spesimen,
                                   Gender = i.jantina,
                                   Source = i.sumber,
                                   Appendix = i.appendiks,
                                   Spesis_Quantity_Disahkan = (float)_context.GBL_SPESIS_MST.Where(a => a.license_id == e.id && a.spesis_id == i.spesis_id).Sum(i => i.qty_endorsed),//(float?)i.qty_endorsed,
                                   Spesis_Id = (int)i.spesis_id,
                                   No_CR = i.cr,
                                   Original_CitesNo = i.cites_asal,
                                   Catatan = i.remarks,
                                   Main_Spesis_Id = i.id    //utk smentara--nti ubah balik


                               })
                              .GroupBy(x => x.Spesis_Name_Scientific)
                              .Select(g => g.FirstOrDefault())  //select first item group
                              .ToList();    //convert result into list



            return SpeciesList;
        }
        public List<Species> getTotalQuantityHidup(int id) //to get total species list that belong to license and spesis id
        {
            var totQtyHidup = (from e in _context.GBL_LICENSE_MST
                               join i in _context.GBL_SPESIS_MST on e.id equals i.license_id
                               join j in _context.dSpesisNames on i.spesis_id equals j.id
                               where id == e.id
                               group i by i.spesis_id into speciesGroup
                               select new Species
                               {
                                   Spesis_Id = (int)speciesGroup.Key,
                                   Quantity = speciesGroup.Count().ToString(),  //to get total count hy each species in 'Hidup' category
                                   Spesis_Quantity_Disahkan = speciesGroup.Count(),
                                   //Specimen = i.spesimen,
                                   //Spesis_Quantity_Disahkan = (float?)i.qty_endorsed ?? 0,
                                   //Catatan = i.remarks,

                               }).ToList();

            return totQtyHidup;

        }
        public List<Species> getTotalQuantitySpecies(int id)
        {
            var totalQty = (from e in _context.GBL_LICENSE_MST
                            join i in _context.GBL_SPESIS_MST on e.id equals i.license_id
                            join j in _context.dSpesisNames on i.spesis_id equals j.id
                            join m in _context.GBL_IMPORT_MST on e.id equals m.license_id into table1
                            from m in table1.DefaultIfEmpty()
                            where id == e.id && i.quantity != ""
                            select new Species
                            {
                                Spesis_Name_Scientific = j.spp_saintifik,
                                Spesis_Name_Tempatan = j.spp_tempatan,
                                Quantity = i.quantity,
                                Specimen = i.spesimen,
                                Gender = i.jantina,
                                Source = i.sumber,
                                Appendix = i.appendiks,
                                Spesis_Quantity_Disahkan = (float)_context.GBL_SPESIS_MST.Where(a => a.license_id == e.id && a.spesis_id == i.spesis_id).Sum(i => i.qty_endorsed),//(float?)i.qty_endorsed,
                                Main_Spesis_Id = i.id,
                                Spesis_Id = (int)i.spesis_id,    //id of each species
                                No_Penandaan = i.penandaan,
                                No_Penandaan_Disahkan = i.penandaan_disahkan,
                                Catatan = i.remarks,
                                No_CR = i.cr,
                                Original_CitesNo = m.cites_asal,
                                NoLesenMengambil = i!=null ? i.no_license:"",

                            }).ToList();

            if (totalQty != null)
            {
                foreach (var s in totalQty)
                {
                    if (s.Gender == "1")
                    {
                        s.NamaJantina = "JANTAN";
                    }
                    else if (s.Gender == "2")
                    {
                        s.NamaJantina = "BETINA";
                    }
                    else if (s.Gender == "3")
                    {
                        s.NamaJantina = "TIDAK PASTI";
                    }
                }
            }


            return totalQty;
        }

        #endregion Species

        #region List Import Export Status

        public Object getStatusSemakSemula(JqueryDatatableParamVM param, FormCollection frm, int userId)
        {

            var c = (from a in _context.GBL_LICENSE_MST
                     join b in _context.GBL_IMPORT_MST on a.id equals b.license_id
                     join k in _context.GBL_SPESIS_MST on a.id equals k.license_id
                     join d in _context.GBL_USERS_MST on b.id_penyemak equals d.id.ToString()
                     join e in _context.dLicenseDetails on a.lic_type equals e.id
                     join f in _context.GBL_CIVILANT_MST on a.lic_civil_id equals f.id
                     join g in _context.GBL_CIVDOC_MST on f.id equals g.civil_id
                     join h in _context.GBL_USERS_MST on b.id_pelulus equals h.id.ToString()
                     where b.status_pegawai_pengesah == "2" && userId == d.id && b.semak_semula == null //penyemak tk hantar semak semula lg
                     orderby a.id
                     select new JqueryDatatableServiceParamListDataVM
                     {
                         Id_Lesen = a.id.ToString(),   //license_id
                         No_Lesen = a.lic_no,
                         Cites_No = b.cites_no,
                         Kategori_Lesen = e.desc_LicenseDetail,
                         Id_Pemegang = g.civil_doc,
                         Nama_Pemegang = f.civil_name,
                         Penyemak_Pintu_Masuk = d.user_fullname,
                         Pengesah_Pintu_Masuk = h.user_fullname,
                         Kuantiti_Disahkan = (float?)k.qty_endorsed ?? 0,
                     });

            var searchByCites = frm["searchByCites"].ToString();

            if (searchByCites != "")
                c = c.Where(a => a.Cites_No.ToString().StartsWith(searchByCites));

            //var data = c.Skip(param.Start).Take(param.Length).ToList();
            var data = c.DistinctBy(x => x.Id_Lesen).Skip(param.Start).Take(param.Length).ToList();

            //after retrieved data, data is in form of memory(dalam bentuk objek) - so bleh apply any c# methods - tkde limitation dri LINQ provider
            var encodedData = data.Select(x => new JqueryDatatableServiceParamListDataVM
            {
                Id_Lesen = GlobalFunction.encodeThis(x.Id_Lesen.ToString()),   //license_id
                No_Lesen = x.No_Lesen,
                Cites_No = x.Cites_No,
                Kategori_Lesen = x.Kategori_Lesen,
                Id_Pemegang = x.Id_Pemegang,
                Nama_Pemegang = x.Nama_Pemegang,
                Penyemak_Pintu_Masuk = x.Pengesah_Pintu_Masuk,
                Pengesah_Pintu_Masuk = x.Pengesah_Pintu_Masuk,
                Kuantiti_Disahkan = x.Kuantiti_Disahkan,

            }).ToList();

            JqueryDatatableServiceParamVM p = new JqueryDatatableServiceParamVM();
            p.draw = param.draw;
            p.iTotalRecords = c.Count();
            p.iTotalDisplayRecords = c.Count();
            p.listData = encodedData;

            return p;

        }

        public Object getDisemak(JqueryDatatableParamVM param, FormCollection frm, int userId)
        {

            var c = (from a in _context.GBL_LICENSE_MST
                     join b in _context.GBL_IMPORT_MST on a.id equals b.license_id
                     join k in _context.GBL_SPESIS_MST on a.id equals k.license_id
                     join d in _context.GBL_USERS_MST on b.id_penyemak equals d.id.ToString()
                     join e in _context.dLicenseDetails on a.lic_type equals e.id
                     join f in _context.GBL_CIVILANT_MST on a.lic_civil_id equals f.id
                     join g in _context.GBL_CIVDOC_MST on f.id equals g.civil_id
                     join h in _context.GBL_USERS_MST on b.id_pelulus equals h.id.ToString()
                     where b.status_pengesahan != null && userId == d.id
                     //orderby a.id
                     select new JqueryDatatableServiceParamListDataVM
                     {
                         Id_Lesen = a.id.ToString(),  //license_id
                         No_Lesen = a.lic_no,
                         Cites_No = b.cites_no,
                         Kategori_Lesen = e.desc_LicenseDetail,
                         Id_Pemegang = g.civil_doc,
                         Nama_Pemegang = f.civil_name,
                         Penyemak_Pintu_Masuk = d.user_fullname,
                         Pengesah_Pintu_Masuk = h.user_fullname,
                         Kuantiti_Disahkan = (float?)k.qty_endorsed ?? 0,
                         Tarikh_Htr_Borang = (DateTime)b.created_at
                     })
                      .GroupBy(x => x.Id_Lesen)
                      .Select(g => g.FirstOrDefault()); //select first item group;

            c = c.OrderBy(x => x.Id_Lesen);

            var searchByCites = frm["searchByCites"].ToString();

            if (searchByCites != "")
                c = c.Where(a => a.Cites_No.ToString().StartsWith(searchByCites));

            var data = c.Skip(param.Start).Take(param.Length).ToList();

            var encodedData = data.Select(x => new JqueryDatatableServiceParamListDataVM
            {
                Id_Lesen = GlobalFunction.encodeThis(x.Id_Lesen.ToString()),   //license_id
                No_Lesen = x.No_Lesen,
                Cites_No = x.Cites_No,
                Kategori_Lesen = x.Kategori_Lesen,
                Id_Pemegang = x.Id_Pemegang,
                Nama_Pemegang = x.Nama_Pemegang,
                Penyemak_Pintu_Masuk = x.Penyemak_Pintu_Masuk,
                Pengesah_Pintu_Masuk = x.Pengesah_Pintu_Masuk,
                Kuantiti_Disahkan = x.Kuantiti_Disahkan,
                Tarikh_Htr_Borang = x.Tarikh_Htr_Borang,

            }).ToList();

            JqueryDatatableServiceParamVM p = new JqueryDatatableServiceParamVM();
            p.draw = param.draw;
            p.iTotalRecords = c.Count();
            p.iTotalDisplayRecords = c.Count();
            p.listData = encodedData;

            return p;

        }

        public Object getSahStatus(JqueryDatatableParamVM param, FormCollection frm, int userId)
        {

            var c = (from a in _context.GBL_LICENSE_MST
                     join b in _context.GBL_IMPORT_MST on a.id equals b.license_id
                     join k in _context.GBL_SPESIS_MST on a.id equals k.license_id
                     join d in _context.GBL_USERS_MST on b.id_penyemak equals d.id.ToString()
                     join e in _context.dLicenseDetails on a.lic_type equals e.id
                     join f in _context.GBL_CIVILANT_MST on a.lic_civil_id equals f.id
                     join g in _context.GBL_CIVDOC_MST on f.id equals g.civil_id
                     join h in _context.GBL_USERS_MST on b.id_pelulus equals h.id.ToString() into table1
                     from h in table1.DefaultIfEmpty()
                     where b.status_pengesahan == "1" && userId == h.id && b.status_pegawai_pengesah == null //1 = SAH
                     orderby a.id
                     select new JqueryDatatableServiceParamListDataVM
                     {
                         Id_Lesen = a.id.ToString(),   //license_id
                         No_Lesen = a.lic_no,
                         Cites_No = b.cites_no,
                         Kategori_Lesen = e.desc_LicenseDetail,
                         Id_Pemegang = g.civil_doc,
                         Nama_Pemegang = f.civil_name,
                         Penyemak_Pintu_Masuk = d.user_fullname,
                         Pengesah_Pintu_Masuk = h.user_fullname,
                         Kuantiti_Disahkan = (float?)k.qty_endorsed ?? 0,
                         Status_Penyemak = b.status_pengesahan,
                         //dataDate1 = (DateTime)b.created_at,
                     });


            var searchByCites = frm["searchByCites"].ToString();

            if (searchByCites != "")
                c = c.Where(a => a.Cites_No.ToString().StartsWith(searchByCites));

            var data = c.DistinctBy(x => x.Id_Lesen).Skip(param.Start).Take(param.Length).ToList();

            var encodedData = data.Select(x => new JqueryDatatableServiceParamListDataVM
            {
                Id_Lesen = GlobalFunction.encodeThis(x.Id_Lesen.ToString()),   //license_id
                No_Lesen = x.No_Lesen,
                Cites_No = x.Cites_No,
                Kategori_Lesen = x.Kategori_Lesen,
                Id_Pemegang = x.Id_Pemegang,
                Nama_Pemegang = x.Nama_Pemegang,
                Penyemak_Pintu_Masuk = x.Penyemak_Pintu_Masuk,
                Pengesah_Pintu_Masuk = x.Pengesah_Pintu_Masuk,
                Kuantiti_Disahkan = x.Kuantiti_Disahkan,
                Status_Penyemak = x.Status_Penyemak

            }).ToList();

            JqueryDatatableServiceParamVM p = new JqueryDatatableServiceParamVM();
            p.draw = param.draw;
            p.iTotalRecords = encodedData.Count();
            p.iTotalDisplayRecords = encodedData.Count();
            p.listData = encodedData;

            return p;

        }

        public Object getStatusSelesai(JqueryDatatableParamVM param, FormCollection frm, int userId)
        {

            var c = (from a in _context.GBL_LICENSE_MST
                     join b in _context.GBL_IMPORT_MST on a.id equals b.license_id
                     join k in _context.GBL_SPESIS_MST on a.id equals k.license_id
                     join d in _context.GBL_USERS_MST on b.id_penyemak equals d.id.ToString()
                     join e in _context.dLicenseDetails on a.lic_type equals e.id
                     join f in _context.GBL_CIVILANT_MST on a.lic_civil_id equals f.id
                     join g in _context.GBL_CIVDOC_MST on f.id equals g.civil_id
                     join h in _context.GBL_USERS_MST on b.id_pelulus equals h.id.ToString()
                     where b.status_pegawai_pengesah == "1" && userId == h.id && b.status_pengesahan == "1"  //selesai
                     orderby a.id
                     select new JqueryDatatableServiceParamListDataVM
                     {
                         Id_Lesen = a.id.ToString(),   //license_id
                         No_Lesen = a.lic_no,
                         Cites_No = b.cites_no,
                         Kategori_Lesen = e.desc_LicenseDetail,
                         Id_Pemegang = g.civil_doc,
                         Nama_Pemegang = f.civil_name,
                         Penyemak_Pintu_Masuk = d.user_fullname,
                         Pengesah_Pintu_Masuk = h.user_fullname,
                         Kuantiti_Disahkan = (float?)k.qty_endorsed ?? 0,
                         //dataDate1 = (DateTime)b.created_at,
                     });

            var searchByCites = frm["searchByCites"].ToString();

            if (searchByCites != "")
                c = c.Where(a => a.Cites_No.ToString().StartsWith(searchByCites));

            var data = c.DistinctBy(x => x.Id_Lesen).Skip(param.Start).Take(param.Length).ToList(); //psatikan jumlah sama

            var encodedData = data.Select(x => new JqueryDatatableServiceParamListDataVM
            {
                Id_Lesen = GlobalFunction.encodeThis(x.Id_Lesen.ToString()),   //license_id
                No_Lesen = x.No_Lesen,
                Cites_No = x.Cites_No,
                Kategori_Lesen = x.Kategori_Lesen,
                Id_Pemegang = x.Id_Pemegang,
                Nama_Pemegang = x.Nama_Pemegang,
                Penyemak_Pintu_Masuk = x.Penyemak_Pintu_Masuk,
                Pengesah_Pintu_Masuk = x.Pengesah_Pintu_Masuk,
                Kuantiti_Disahkan = x.Kuantiti_Disahkan,

            }).ToList();

            JqueryDatatableServiceParamVM p = new JqueryDatatableServiceParamVM();
            p.draw = param.draw;
            p.iTotalRecords = c.DistinctBy(x => x.Id_Lesen).Count(); //pastikan total sama dgn jumlah data yg dipaparkan utk elak error length pada datatable
            p.iTotalDisplayRecords = c.DistinctBy(x => x.Id_Lesen).Count();
            p.listData = encodedData;

            return p;

        }

        public Object getTakSahStatus(JqueryDatatableParamVM param, FormCollection frm)
        {

            var c = (from a in _context.GBL_LICENSE_MST
                     join b in _context.GBL_IMPORT_MST on a.id equals b.license_id
                     join k in _context.GBL_SPESIS_MST on a.id equals k.license_id
                     join d in _context.GBL_USERS_MST on b.id_pelulus equals d.id.ToString() into table1
                     from d in table1.DefaultIfEmpty()
                     join e in _context.dLicenseDetails on a.lic_type equals e.id
                     join f in _context.GBL_CIVILANT_MST on a.lic_civil_id equals f.id
                     join g in _context.GBL_CIVDOC_MST on f.id equals g.civil_id
                     join h in _context.GBL_USERS_MST on b.id_penyemak equals h.id.ToString()
                     where b.status_pengesahan == "2"
                     orderby a.id
                     select new JqueryDatatableServiceParamListDataVM
                     {
                         Id_Lesen = a.id.ToString(),   //license_id
                         No_Lesen = a.lic_no,
                         Cites_No = b.cites_no,
                         Kategori_Lesen = e.desc_LicenseDetail,
                         Id_Pemegang = g.civil_doc,
                         Nama_Pemegang = f.civil_name,
                         Penyemak_Pintu_Masuk = h.user_fullname,
                         Pengesah_Pintu_Masuk = d != null ? d.user_fullname : "",
                         Kuantiti_Disahkan = (float?)k.qty_endorsed ?? 0,
                         //dataDate1 = (DateTime)b.created_at,
                     });

            var searchByCites = frm["searchByCites"].ToString();

            if (searchByCites != "")
                c = c.Where(a => a.Cites_No.ToString().StartsWith(searchByCites));

            var data = c.DistinctBy(x => x.Id_Lesen).Skip(param.Start).Take(param.Length).ToList();
            var encodedData = data.Select(x => new JqueryDatatableServiceParamListDataVM
            {
                Id_Lesen = GlobalFunction.encodeThis(x.Id_Lesen.ToString()),   //license_id
                No_Lesen = x.No_Lesen,
                Cites_No = x.Cites_No,
                Kategori_Lesen = x.Kategori_Lesen,
                Id_Pemegang = x.Id_Pemegang,
                Nama_Pemegang = x.Nama_Pemegang,
                Penyemak_Pintu_Masuk = x.Penyemak_Pintu_Masuk,
                Pengesah_Pintu_Masuk = x.Pengesah_Pintu_Masuk,
                Kuantiti_Disahkan = x.Kuantiti_Disahkan,

            }).ToList();

            JqueryDatatableServiceParamVM p = new JqueryDatatableServiceParamVM();
            p.draw = param.draw;
            p.iTotalRecords = c.Count();
            p.iTotalDisplayRecords = c.Count();
            p.listData = encodedData;

            return p;

        }

        public Object getDlmSemakanSemula(JqueryDatatableParamVM param, FormCollection frm, int userId)
        {

            var c = (from a in _context.GBL_LICENSE_MST
                     join b in _context.GBL_IMPORT_MST on a.id equals b.license_id
                     join k in _context.GBL_SPESIS_MST on a.id equals k.license_id
                     join d in _context.GBL_USERS_MST on b.id_penyemak equals d.id.ToString()
                     join e in _context.dLicenseDetails on a.lic_type equals e.id
                     join f in _context.GBL_CIVILANT_MST on a.lic_civil_id equals f.id
                     join g in _context.GBL_CIVDOC_MST on f.id equals g.civil_id
                     join h in _context.GBL_USERS_MST on b.id_pelulus equals h.id.ToString() into table1
                     from h in table1.DefaultIfEmpty()
                     where b.status_pengesahan == "1" && userId == h.id && b.status_pegawai_pengesah == "2" && b.semak_semula == "1" //1 = Pengesah - Dalam Semakan Semula
                     orderby a.id
                     select new JqueryDatatableServiceParamListDataVM
                     {
                         Id_Lesen = a.id.ToString(),   //license_id
                         No_Lesen = a.lic_no,
                         Cites_No = b.cites_no,
                         Kategori_Lesen = e.desc_LicenseDetail,
                         Id_Pemegang = g.civil_doc,
                         Nama_Pemegang = f.civil_name,
                         Penyemak_Pintu_Masuk = d.user_fullname,
                         Pengesah_Pintu_Masuk = h.user_fullname,
                         Kuantiti_Disahkan = (float?)k.qty_endorsed ?? 0,
                         Status_Penyemak = b.status_pengesahan,
                         //dataDate1 = (DateTime)b.created_at,
                     });

            var searchByCites = frm["searchByCites"].ToString();

            if (searchByCites != "")
                c = c.Where(a => a.Cites_No.ToString().StartsWith(searchByCites));

            var data = c.DistinctBy(x => x.Id_Lesen).Skip(param.Start).Take(param.Length).ToList();

            var encodedData = data.Select(x => new JqueryDatatableServiceParamListDataVM
            {
                Id_Lesen = GlobalFunction.encodeThis(x.Id_Lesen.ToString()),   //license_id
                No_Lesen = x.No_Lesen,
                Cites_No = x.Cites_No,
                Kategori_Lesen = x.Kategori_Lesen,
                Id_Pemegang = x.Id_Pemegang,
                Nama_Pemegang = x.Nama_Pemegang,
                Penyemak_Pintu_Masuk = x.Penyemak_Pintu_Masuk,
                Pengesah_Pintu_Masuk = x.Pengesah_Pintu_Masuk,
                Kuantiti_Disahkan = x.Kuantiti_Disahkan,
                Status_Penyemak = x.Status_Penyemak

            }).ToList();

            JqueryDatatableServiceParamVM p = new JqueryDatatableServiceParamVM();
            p.draw = param.draw;
            p.iTotalRecords = c.Count();
            p.iTotalDisplayRecords = c.Count();
            p.listData = encodedData;

            return p;

        }

        #endregion

        #region Document
        public List<Document> getListDocument(int id)
        {
            var doc = (from a in _context.GBL_LICENSE_MST
                       join b in _context.GBL_ATTACHMENT_MST on a.id equals b.license_id
                       where id == a.id
                       select new Document
                       {
                           File_Name = b.attach_type,
                           File_Location = b.attach_url,
                       }).ToList();

            //if (doc != null)
            //{
            //    foreach (var d in doc)
            //    {
            //        if (d.File_Name == "roc")
            //        {
            //            d.File_Location = d.File_Location;
            //        }

            //    }
            //}


            return doc;
        }

        public Document getSpecificDoc(int id)
        {
            var doc = (from a in _context.GBL_LICENSE_MST
                       join b in _context.GBL_ATTACHMENT_MST on a.id equals b.license_id
                       where id == a.id
                       select new Document
                       {
                           File_Name = b.attach_type,
                           File_Location = b.attach_url,
                       }).FirstOrDefault();

            return doc;
        }

        #endregion

        public ReviewVerify getNamaPengesah()
        {
            var n = (from a in _context.GBL_USERS_MST
                     join b in _context.GBL_USER_POSITION_MST on a.id equals b.user_id
                     join c in _context.dPositions on b.user_position_id equals c.posId
                     where b.user_position_id == 12 && a.user_status == 1
                     select new ReviewVerify
                     {
                         Pegawai_Pengesah = a != null ? a.user_fullname : "",
                     }).FirstOrDefault();
            return n;
        }

        public ReviewVerify getReviewVerify(int id)
        {
            var rnv = (from a in _context.GBL_LICENSE_MST
                       join b in _context.GBL_IMPORT_MST on a.id equals b.license_id
                       join c in _context.GBL_USERS_MST on b.id_pelulus equals c.id.ToString() into table1
                       from c in table1.DefaultIfEmpty()
                       join d in _context.GBL_USERS_MST on b.id_penyemak equals d.id.ToString()
                       where id == a.id
                       select new ReviewVerify
                       {
                           User_Id = d.id,
                           Pegawai_Penyemak = d.user_fullname,
                           Pegawai_Pengesah = c != null ? c.user_fullname : "",
                       }).FirstOrDefault();

            return rnv;
        }

        public async Task<int> SaveImportExport(ImportExport model, string page, string FileLocation)
        {
            var _accountService = new AccountService();
            var Role = _accountService.getRole(model.User_Id);
            //int decodeLesenID = Convert.ToInt32(GlobalFunction.decodeThis(model.License_Id));

            if (model.License_Id > 0)
            {
                var ImportMST = await _context.GBL_IMPORT_MST.FirstOrDefaultAsync(x => x.license_id == model.License_Id);

                //if (Role.Contains(10))
                //{
                if (page == "1")
                {
                    ImportMST.jenis_permit = model.Jenis_Permit;
                    ImportMST.sah_sehingga = DateTime.ParseExact(model.Sah_Sehingga, "dd/MM/yyyy", CultureInfo.InvariantCulture); //kena convert kepada datatime balik
                }
                if (page == "2")
                {
                    var speciesList = model.getSpeciesList;
                    var species = model.getTotalSpeciesList;


                    if (speciesList != null) //parent table 
                    {
                        foreach (var m in speciesList)
                        {
                            var SpesisMST = await _context.GBL_SPESIS_MST.FirstOrDefaultAsync(x => x.id == m.Main_Spesis_Id && x.license_id == model.License_Id && x.spesis_id == m.Spesis_Id);

                            if (SpesisMST != null)
                            {
                                //Update existing SpesisMST entity

                                SpesisMST.qty_endorsed = m.Spesis_Quantity_Disahkan;
                                SpesisMST.id = m.Main_Spesis_Id;
                                SpesisMST.spesis_id = m.Spesis_Id;
                            }

                            else
                            {
                                // Create new SpesisMST entity if it doesn't exist
                                var newSpesisMST = new GBL_SPESIS_MST();
                                newSpesisMST.license_id = model.License_Id;
                                newSpesisMST.spesis_id = m.Spesis_Id;
                                //newSpesisMST.appendiks = m.Appendix;
                                //newSpesisMST.sumber = m.Source;
                                newSpesisMST.qty_endorsed = m.Spesis_Quantity_Disahkan;

                                _context.GBL_SPESIS_MST.Add(newSpesisMST);
                            }
                        }
                    }

                    if (species != null)     //table child
                    {
                        foreach (var m in species)
                        {
                            var SpesisMST = await _context.GBL_SPESIS_MST.FirstOrDefaultAsync(x => x.id == m.Main_Spesis_Id && x.license_id == model.License_Id && x.spesis_id == m.Spesis_Id);

                            if (SpesisMST != null)
                            {
                                //Update existing SpesisMST entity
                                if (m.Specimen == "Hidup")
                                {
                                    if (m.IsSelected == true) // true = checkbox is ticked
                                    {
                                        SpesisMST.qty_endorsed = 1;
                                    }
                                    else
                                    {
                                        SpesisMST.qty_endorsed = null;
                                    }

                                }
                                SpesisMST.appendiks = m.Appendix;
                                SpesisMST.sumber = m.Source;
                                SpesisMST.remarks = m.Catatan;
                                SpesisMST.penandaan_disahkan = m.No_Penandaan_Disahkan;
                                SpesisMST.id = m.Main_Spesis_Id;
                                SpesisMST.spesis_id = m.Spesis_Id;
                            }

                            else
                            {
                                // Create new SpesisMST entity if it doesn't exist
                                var newSpesisMST = new GBL_SPESIS_MST();
                                newSpesisMST.license_id = model.License_Id;
                                newSpesisMST.spesis_id = m.Spesis_Id;
                                newSpesisMST.appendiks = m.Appendix;
                                newSpesisMST.sumber = m.Source;
                                _context.GBL_SPESIS_MST.Add(newSpesisMST);
                            }
                        }
                    }


                    await _context.SaveChangesAsync();
                    return model.License_Id;
                }
                if (page == "3")
                {
                    ImportMST.alamat_import = model.Alamat_Pengimport;
                    ImportMST.negara_import = model.Negara_Import;
                    ImportMST.nama_import = model.Nama_Import;
                    ImportMST.negara_eksport = model.Negara_Eksport;
                    ImportMST.tujuan_import = model.Tujuan;
                    ImportMST.alamat_export = model.Alamat_Pengeksport;
                    ImportMST.nama_export = model.Nama_Eksport;
                    ImportMST.no_setem = model.No_Stem;
                    ImportMST.license_id = model.License_Id;

                    var AttachMST = await _context.GBL_ATTACHMENT_MST.FirstOrDefaultAsync(x => x.license_id == model.License_Id);

                    if (AttachMST != null && model.FileType == "CitesOri")
                    {
                        //AttachMST.attach_type = FileName?.Substring(0, Math.Min(50, FileName?.Length ?? 0));
                        AttachMST.attach_type = "maklumat_cites";
                        AttachMST.attach_url = FileLocation?.Substring(0, Math.Min(200, FileLocation?.Length ?? 0));
                    }
                    else if (AttachMST != null && model.FileType == "Cites2")
                    {
                        //AttachMST.attach_type = FileName?.Substring(0, Math.Min(50, FileName?.Length ?? 0));
                        AttachMST.attach_type = "maklumat_cites_ubah";
                        AttachMST.attach_url = FileLocation?.Substring(0, Math.Min(200, FileLocation?.Length ?? 0));
                        AttachMST.license_id = model.License_Id;
                        _context.GBL_ATTACHMENT_MST.Add(AttachMST);
                    }

                    else
                    {

                        var documentMST = new GBL_ATTACHMENT_MST();
                        //documentMST.attach_type = FileName?.Substring(0, Math.Min(50, FileName?.Length ?? 0));  //klau exceed,ia akan ambil sampai limit tu shja
                        if (model.FileType == "CitesOri")
                        {
                            documentMST.attach_type = "maklumat_cites";
                        }

                        if (model.FileType == "Cites2")
                        {
                            documentMST.attach_type = "maklumat_cites_berubah";
                        }

                        documentMST.attach_url = FileLocation?.Substring(0, Math.Min(200, FileLocation?.Length ?? 0)); ;
                        documentMST.license_id = model.License_Id;
                        _context.GBL_ATTACHMENT_MST.Add(documentMST);
                    }

                    _context.SaveChanges();

                    return model.License_Id;

                }
                if (page == "4")
                {
                    ImportMST.nama_pemeriksa = model.Nama_Pemeriksa;
                    ImportMST.tarikh_masa_semakan = DateTime.ParseExact(model.Tarikh_MasaSemakan, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    ImportMST.lokasi_pemeriksaan = model.Lokasi_Pemeriksaan;

                }
                if (page == "5")
                {
                    //nanti kena differentiate by role

                    ImportMST.status_pengesahan = model.Status_By_Penyemak;
                    if (model.Status_By_Penyemak == "1")    //sah
                    {
                        ImportMST.id_pelulus = model.Nama_Pengesah;
                    }
                    else
                    {
                        ImportMST.id_pelulus = null;
                    }

                    if (model.Status_By_Penyemak == "1" && model.Status_By_Pengesah == "2")
                    {
                        ImportMST.semak_semula = "1";
                    }
                    ImportMST.id_penyemak = model.User_Id.ToString();
                    ImportMST.catatan_semakan = model.Catatan_Penyemak;
                }
                //}

                //if (Role.Contains(12))
                //{
                if (page == "6")
                {
                    //nanti differentiate by role
                    ImportMST.status_pegawai_pengesah = model.Status_By_Pengesah;
                    if (model.Status_By_Pengesah == "2")    //pengesah tk sahkan, penyemak kena semak semula
                    {
                        ImportMST.semak_semula = null;
                    }
                    ImportMST.ulasan_pengesah = model.Ulasan_Pengesah;
                }
                //}


                ImportMST.tarikh_kemaskini = DateTime.Now;
                ImportMST.created_at = DateTime.Now;

            }
            _context.SaveChanges();
            return model.License_Id;

        }

        #region Count List

        public int GetDisemakCount(int userId)
        {
            var count = (from a in _context.GBL_LICENSE_MST
                         join b in _context.GBL_IMPORT_MST on a.id equals b.license_id
                         join k in _context.GBL_SPESIS_MST on a.id equals k.license_id
                         join d in _context.GBL_USERS_MST on b.id_penyemak equals d.id.ToString()
                         join e in _context.dLicenseDetails on a.lic_type equals e.id
                         join f in _context.GBL_CIVILANT_MST on a.lic_civil_id equals f.id
                         join g in _context.GBL_CIVDOC_MST on f.id equals g.civil_id
                         join h in _context.GBL_USERS_MST on b.id_pelulus equals h.id.ToString()
                         where b.status_pengesahan != null && userId == d.id
                         select a).Distinct().Count();

            return count;
        }

        public int GetSemakSemulaCount(int userId)
        {
            var count = (from a in _context.GBL_LICENSE_MST
                         join b in _context.GBL_IMPORT_MST on a.id equals b.license_id
                         join k in _context.GBL_SPESIS_MST on a.id equals k.license_id
                         join d in _context.GBL_USERS_MST on b.id_penyemak equals d.id.ToString()
                         join e in _context.dLicenseDetails on a.lic_type equals e.id
                         join f in _context.GBL_CIVILANT_MST on a.lic_civil_id equals f.id
                         join g in _context.GBL_CIVDOC_MST on f.id equals g.civil_id
                         join h in _context.GBL_USERS_MST on b.id_pelulus equals h.id.ToString()
                         where b.status_pegawai_pengesah == "2" && userId == d.id && b.semak_semula == null
                         select a).Distinct().Count();

            return count;
        }

        public int GetSelesaiCount(int userId)
        {
            var count = (from a in _context.GBL_LICENSE_MST
                         join b in _context.GBL_IMPORT_MST on a.id equals b.license_id
                         join k in _context.GBL_SPESIS_MST on a.id equals k.license_id
                         join d in _context.GBL_USERS_MST on b.id_penyemak equals d.id.ToString()
                         join e in _context.dLicenseDetails on a.lic_type equals e.id
                         join f in _context.GBL_CIVILANT_MST on a.lic_civil_id equals f.id
                         join g in _context.GBL_CIVDOC_MST on f.id equals g.civil_id
                         join h in _context.GBL_USERS_MST on b.id_pelulus equals h.id.ToString()
                         where b.status_pegawai_pengesah == "1" && userId == h.id && b.status_pengesahan == "1"  //selesai
                         select a).Distinct().Count();

            return count;
        }

        public int GetSahCount(int userId)
        {
            var count = (from a in _context.GBL_LICENSE_MST
                         join b in _context.GBL_IMPORT_MST on a.id equals b.license_id
                         join k in _context.GBL_SPESIS_MST on a.id equals k.license_id
                         join d in _context.GBL_USERS_MST on b.id_penyemak equals d.id.ToString()
                         join e in _context.dLicenseDetails on a.lic_type equals e.id
                         join f in _context.GBL_CIVILANT_MST on a.lic_civil_id equals f.id
                         join g in _context.GBL_CIVDOC_MST on f.id equals g.civil_id
                         join h in _context.GBL_USERS_MST on b.id_pelulus equals h.id.ToString() into table1
                         from h in table1.DefaultIfEmpty()
                         where b.status_pengesahan == "1" && userId == h.id && b.status_pegawai_pengesah == null //1 = SAH
                         select a).Distinct().Count();

            return count;
        }

        public int GetPengesahSemakSemula(int userId)
        {
            var count = (from a in _context.GBL_LICENSE_MST
                         join b in _context.GBL_IMPORT_MST on a.id equals b.license_id
                         join k in _context.GBL_SPESIS_MST on a.id equals k.license_id
                         join d in _context.GBL_USERS_MST on b.id_penyemak equals d.id.ToString()
                         join e in _context.dLicenseDetails on a.lic_type equals e.id
                         join f in _context.GBL_CIVILANT_MST on a.lic_civil_id equals f.id
                         join g in _context.GBL_CIVDOC_MST on f.id equals g.civil_id
                         join h in _context.GBL_USERS_MST on b.id_pelulus equals h.id.ToString() into table1
                         from h in table1.DefaultIfEmpty()
                         where b.status_pengesahan == "1" && userId == h.id && b.status_pegawai_pengesah == "2" && b.semak_semula == "1" //1 = Pengesah - Semakan Semula
                         select a).Distinct().Count();

            return count;
        }

        public int GetTakSahCount(int userId)
        {
            var count = (from a in _context.GBL_LICENSE_MST
                         join b in _context.GBL_IMPORT_MST on a.id equals b.license_id
                         join k in _context.GBL_SPESIS_MST on a.id equals k.license_id
                         join d in _context.GBL_USERS_MST on b.id_pelulus equals d.id.ToString() into table1
                         from d in table1.DefaultIfEmpty()
                         join e in _context.dLicenseDetails on a.lic_type equals e.id
                         join f in _context.GBL_CIVILANT_MST on a.lic_civil_id equals f.id
                         join g in _context.GBL_CIVDOC_MST on f.id equals g.civil_id
                         join h in _context.GBL_USERS_MST on b.id_penyemak equals h.id.ToString()
                         where b.status_pengesahan == "2"
                         select a).Count();

            return count;
        }
        #endregion

        #region Semakan Datatable

        public Object getNamaPemilik(JqueryDatatableParamVM param, FormCollection frm)
        {

            var c = (from a in _context.GBL_LICENSE_MST
                     join b in _context.GBL_CIVILANT_MST on a.lic_civil_id equals b.id
                     join k in _context.GBL_CIVDOC_MST on b.id equals k.civil_id
                     join d in _context.dNationalities on k.civil_type equals d.id
                     join e in _context.GBL_ORGANIZATION_MST on a.lic_org_id equals e.id into table1
                     from e in table1.DefaultIfEmpty()
                     orderby a.id
                     select new JqueryDatatableServiceParamListDataVM
                     {
                         Id_Lesen = a.id.ToString(),
                         Nama_Pemegang = b.civil_name,
                         Id_Pemegang = k.civil_doc,
                         NoTel = b.civil_tel,
                         NamaSyarikat = e.org_name,
                         RocNo = e.org_roc,
                         Emel = b.civil_email,

                     });
            var search = frm["search"].ToString(); //value dapat daripada input

            var jenisCarianSelect = frm["jenisCarianSelect"];

            if (search != "" && jenisCarianSelect == "1") //NAMA PEMILIK
            {
                c = c.Where(a => a.Nama_Pemegang.ToString().Contains(search));
            }
            else if (search != "" && jenisCarianSelect == "2") //NAMA SYARIKAT
            {
                c = c.Where(a => a.NamaSyarikat.ToString().Contains(search));
            }
            else if (search != "" && jenisCarianSelect == "3") //NO IC/PASSPORT
            {
                c = c.Where(a => a.Id_Pemegang.ToString() == search);
            }
            else if (search != "" && jenisCarianSelect == "4") //NO PENDAFTARAN SYARIKAT
            {
                c = c.Where(a => a.RocNo.ToString() == search);
            }
            else if (search != "" && jenisCarianSelect == "8")   //NO TEL
            {
                c = c.Where(a => a.NoTel.ToString() == search);
            }

            //var data = c.Skip(param.Start).Take(param.Length).ToList();
            var data = c.DistinctBy(x => x.Nama_Pemegang).Skip(param.Start).Take(param.Length).ToList();

            ////after retrieved data, data is in form of memory(dalam bentuk objek) - so bleh apply any c# methods - tkde limitation dri LINQ provider
            var encodedData = data.Select(x => new JqueryDatatableServiceParamListDataVM
            {
                Id_Lesen = GlobalFunction.encodeThis(x.Id_Lesen.ToString()),   //license_id
                Nama_Pemegang = x.Nama_Pemegang,
                Id_Pemegang = x.Id_Pemegang,
                NoTel = x.NoTel,
                NamaSyarikat = x.NamaSyarikat,
                RocNo = x.RocNo,
                Emel = x.Emel


            }).ToList();

            JqueryDatatableServiceParamVM p = new JqueryDatatableServiceParamVM();
            p.draw = param.draw;
            p.iTotalRecords = c.DistinctBy(x => x.Nama_Pemegang).Count();
            //p.iTotalRecords = c.Count();
            p.iTotalDisplayRecords = c.DistinctBy(x => x.Nama_Pemegang).Count();
            //p.iTotalDisplayRecords = c.Count();
            p.listData = encodedData;

            return p;

        }

        public Object getNoPenandaan(JqueryDatatableParamVM param, FormCollection frm)
        {

            var c = (from a in _context.GBL_LICENSE_MST
                     join b in _context.GBL_CIVILANT_MST on a.lic_civil_id equals b.id
                     join k in _context.GBL_SPESIS_MST on a.id equals k.license_id
                     join d in _context.dSpesisNames on k.spesis_id equals d.id
                     orderby a.id
                     select new JqueryDatatableServiceParamListDataVM
                     {
                         Id_Lesen = a.id.ToString(),
                         Nama_Pemegang = b.civil_name,
                         No_Lesen = a != null ? a.lic_no : "",
                         NamaSaintifik = d != null ? d.spp_saintifik : "",
                         NamaBiasa = d != null ? d.spp_common : "",
                         NoPenandaan = k != null ? k.penandaan : ""
                     });

            var search = frm["search"].ToString(); //value dapat daripada input

            var jenisCarianSelect = frm["jenisCarianSelect"];

            if (search != "" && jenisCarianSelect == "7")
            {
                c = c.Where(a => a.NoPenandaan.ToString() == search);
            }

            var data = c.Skip(param.Start).Take(param.Length).ToList();
            //var data = c.DistinctBy(x => x.Nama_Pemegang).Skip(param.Start).Take(param.Length).ToList();

            ////after retrieved data, data is in form of memory(dalam bentuk objek) - so bleh apply any c# methods - tkde limitation dri LINQ provider
            var encodedData = data.Select(x => new JqueryDatatableServiceParamListDataVM
            {
                Id_Lesen = GlobalFunction.encodeThis(x.Id_Lesen.ToString()),   //license_id
                Nama_Pemegang = x.Nama_Pemegang,
                No_Lesen = x.No_Lesen,
                NamaSaintifik = x.NamaSaintifik,
                NamaBiasa = x.NamaBiasa,
                NoPenandaan = x.NoPenandaan

            }).ToList();

            JqueryDatatableServiceParamVM p = new JqueryDatatableServiceParamVM();
            p.draw = param.draw;
            p.iTotalRecords = c.Count();
            p.iTotalDisplayRecords = c.Count();
            p.listData = encodedData;

            return p;

        }

        public Object getNoLesen(JqueryDatatableParamVM param, FormCollection frm)
        {

            var c = (from a in _context.GBL_LICENSE_MST
                     join b in _context.GBL_CIVILANT_MST on a.lic_civil_id equals b.id
                     join k in _context.GBL_CIVDOC_MST on b.id equals k.civil_id
                     join e in _context.dLicenseDetails on a.lic_type equals e.id
                     join f in _context.dStates on a.negeri_id equals f.id
                     join i in _context.GBL_LICENSE_SLV on a.id equals i.parent_id
                     orderby a.id
                     select new JqueryDatatableServiceParamListDataVM
                     {
                         Id_Lesen = a.id.ToString(),
                         Nama_Pemegang = b.civil_name,
                         Id_Pemegang = k.civil_doc,
                         NoTel = b.civil_tel,
                         No_Lesen = a != null ? a.lic_no : "",
                         Kategori_Lesen = e.desc_LicenseDetail,
                         TarikhMula = i.new_start_date,
                         TarikhTamat = i.new_last_date,
                         PejabatPengeluar = f.state_desc

                     });

            var search = frm["search"].ToString(); //value dapat daripada input

            var jenisCarianSelect = frm["jenisCarianSelect"];

            if (search != "" && jenisCarianSelect == "5")
            {
                c = c.Where(a => a.No_Lesen.ToString() == search);
            }

            var data = c.Skip(param.Start).Take(param.Length).ToList();
            //var data = c.DistinctBy(x => x.Nama_Pemegang).Skip(param.Start).Take(param.Length).ToList();

            ////after retrieved data, data is in form of memory(dalam bentuk objek) - so bleh apply any c# methods - tkde limitation dri LINQ provider
            var encodedData = data.Select(x => new JqueryDatatableServiceParamListDataVM
            {
                Id_Lesen = GlobalFunction.encodeThis(x.Id_Lesen.ToString()),   //license_id
                Nama_Pemegang = x.Nama_Pemegang,
                Id_Pemegang = x.Id_Pemegang,
                NoTel = x.NoTel,
                No_Lesen = x.No_Lesen,
                Kategori_Lesen = x.Kategori_Lesen,
                TarikhMula = x.TarikhMula,
                TarikhTamat = x.TarikhTamat,
                PejabatPengeluar = x.PejabatPengeluar

            }).ToList();

            JqueryDatatableServiceParamVM p = new JqueryDatatableServiceParamVM();
            p.draw = param.draw;
            p.iTotalRecords = c.Count();
            p.iTotalDisplayRecords = c.Count();
            p.listData = encodedData;

            return p;

        }

        public Object getAlamat(JqueryDatatableParamVM param, FormCollection frm)
        {

            var c = (from a in _context.GBL_LICENSE_MST
                     join b in _context.GBL_CIVILANT_MST on a.lic_civil_id equals b.id
                     join k in _context.GBL_CIVDOC_MST on b.id equals k.civil_id
                     join e in _context.GBL_ADDRESS_MST on k.id equals e.ownerid
                     join f in _context.dOwnerTables on e.ownertable equals f.id
                     join i in _context.dAddressTypes on e.address_type equals i.id

                     orderby a.id
                     select new JqueryDatatableServiceParamListDataVM
                     {
                         Id_Lesen = a.id.ToString(),
                         Nama_Pemegang = b.civil_name,
                         Id_Pemegang = k.civil_doc,
                         NoTel = b.civil_tel,
                         No_Lesen = a != null ? a.lic_no : "",
                         Alamat = e != null ? e.address : "",
                         JenisNamaAlamat = i != null ? i.type_desc : "",
                         OwnerTable = (int)e.ownertable,
                         JenisAlamat = (int)e.address_type,
                     });

            var search = frm["search"].ToString(); //value dapat daripada input

            var jenisCarianSelect = frm["jenisCarianSelect"];

            var kategoriAlamatSelect = frm["kategoriAlamatSelect"].ToString(); //value dapat daripada input

            if (search != "" && jenisCarianSelect == "9")
            {
                if (kategoriAlamatSelect == "1") //alamat surat menyurat
                {
                    c = c.Where(a => a.Alamat.ToString().Contains(search) && a.OwnerTable == 4 && a.JenisAlamat == 1);
                }
                else if (kategoriAlamatSelect == "9") // alamat menyimpan or // Hidupan Liar Ditempatkan
                {
                    c = c.Where(a => a.Alamat.ToString().Contains(search) && a.OwnerTable == 5 && a.JenisAlamat == 9);
                }
                else if (kategoriAlamatSelect == "5") // alamat menyimpan or // Hidupan Liar Ditempatkan
                {
                    c = c.Where(a => a.Alamat.ToString().Contains(search) && a.OwnerTable == 5 && a.JenisAlamat == 5);
                }
                else if (kategoriAlamatSelect == "7") // alamat premis
                {
                    c = c.Where(a => a.Alamat.ToString().Contains(search) && a.OwnerTable == 7 && a.JenisAlamat == 7);
                }

            }
            //c = c.Where( x=>x.JenisAlamat == 4|| x.JenisAlamat == 5 || x.JenisAlamat == 7);

            var data = c.Skip(param.Start).Take(param.Length).ToList();
            //var data = c.DistinctBy(x => x.Nama_Pemegang).Skip(param.Start).Take(param.Length).ToList();

            ////after retrieved data, data is in form of memory(dalam bentuk objek) - so bleh apply any c# methods - tkde limitation dri LINQ provider
            var encodedData = data.Select(x => new JqueryDatatableServiceParamListDataVM
            {
                Id_Lesen = GlobalFunction.encodeThis(x.Id_Lesen.ToString()),   //license_id
                Nama_Pemegang = x.Nama_Pemegang,
                Id_Pemegang = x.Id_Pemegang,
                NoTel = x.NoTel,
                No_Lesen = x.No_Lesen,
                Alamat = x.Alamat,
                JenisNamaAlamat = x.JenisNamaAlamat,
                OwnerTable = x.OwnerTable,
                JenisAlamat = x.JenisAlamat

            }).ToList();

            JqueryDatatableServiceParamVM p = new JqueryDatatableServiceParamVM();
            p.draw = param.draw;
            p.iTotalRecords = c.Count();
            p.iTotalDisplayRecords = c.Count();
            p.listData = encodedData;

            return p;

        }

        public Object getByNoCites(JqueryDatatableParamVM param, FormCollection frm)
        {

            var c = (from a in _context.GBL_LICENSE_MST
                     join b in _context.GBL_CIVILANT_MST on a.lic_civil_id equals b.id
                     join k in _context.GBL_CIVDOC_MST on b.id equals k.civil_id
                     join d in _context.GBL_IMPORT_MST on a.id equals d.license_id
                     join e in _context.dLicenseDetails on a.lic_type equals e.id
                     orderby a.id
                     select new JqueryDatatableServiceParamListDataVM
                     {
                         Id_Lesen = a.id.ToString(),
                         Nama_Pemegang = b.civil_name,
                         Id_Pemegang = k.civil_doc,
                         No_Lesen = a != null ? a.lic_no : "",
                         Cites_No = d != null ? d.cites_no : ""

                     }); ;

            var search = frm["search"].ToString(); //value dapat daripada input

            var jenisCarianSelect = frm["jenisCarianSelect"];

            if (search != "" && jenisCarianSelect == "6")
                c = c.Where(a => a.Cites_No.ToString() == search);


            var data = c.Skip(param.Start).Take(param.Length).ToList();
            //var data = c.DistinctBy(x => x.Id_Lesen).Skip(param.Start).Take(param.Length).ToList();

            ////after retrieved data, data is in form of memory(dalam bentuk objek) - so bleh apply any c# methods - tkde limitation dri LINQ provider
            var encodedData = data.Select(x => new JqueryDatatableServiceParamListDataVM
            {
                Id_Lesen = GlobalFunction.encodeThis(x.Id_Lesen.ToString()),   //license_id
                Nama_Pemegang = x.Nama_Pemegang,
                Id_Pemegang = x.Id_Pemegang,
                No_Lesen = x.No_Lesen,
                Cites_No = x.Cites_No

            }).ToList();

            JqueryDatatableServiceParamVM p = new JqueryDatatableServiceParamVM();
            p.draw = param.draw;
            p.iTotalRecords = c.Count();
            p.iTotalDisplayRecords = c.Count();
            p.listData = encodedData;

            return p;

        }

        #endregion

        #region SaveStatusLesen
        public async Task<int> SaveStatusLesen(StatusLesen model)
        {
            var _accountService = new AccountService();
            var Role = _accountService.getRole(model.User_Id);
            //int decodeLesenID = Convert.ToInt32(GlobalFunction.decodeThis(model.License_Id));

            if (model.LesenID > 0)
            {
                var LicenseMST = await _context.GBL_LICENSE_MST.FirstOrDefaultAsync(x => x.id == model.LesenID);

                if (LicenseMST != null)
                {
                    LicenseMST.status = model.StatusLesenSemakan;
                    LicenseMST.date_periksa = DateTime.ParseExact(model.TarikhTindakan, "dd/MM/yyyy", CultureInfo.InvariantCulture); //kena convert kepada datatime balik
                    LicenseMST.catat_periksa = model.Catatan;

                    var CIV_MST = await _context.GBL_CIVILANT_MST.FirstOrDefaultAsync(a => a.id == LicenseMST.lic_civil_id);
                    if (CIV_MST != null)
                    {
                        var CivDoc_MST = await _context.GBL_CIVDOC_MST.FirstOrDefaultAsync(b => b.civil_id == CIV_MST.id);

                        if (CivDoc_MST != null)
                        {
                            CivDoc_MST.civil_status = model.StatusPemilik;
                            CivDoc_MST.updated_date = DateTime.Now;
                        }
                    }
                }

            }
            _context.SaveChanges();
            return model.LesenID;

        }
        #endregion
    }
}