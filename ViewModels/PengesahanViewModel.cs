using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Drawing;

namespace ePerhilitanV2.Models.ViewModels.PengesahanViewModel
{
    #region Semak Maklumat
    public class UserDetail
    {
        public int Id { get; set; }
        public int Owner_Id { get; set; }
        public string Name { get; set; }
        public string IC_No { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Fax_No { get; set; }
        public string Company_Name { get; set; }
        public string Roc_No { get; set; }
        public string Address { get; set; }
        public int Postcode { get; set; }
        public string Company_Type { get; set; }
        public string Company_Type2 { get; set; }
        public string Occupation { get; set; }
        public string Nationality { get; set; }
        public UserDetail GetUserDetails { get; set; }
    }

    #endregion

    #region Import Export

    public class ImportExport       //inherit from applicationviewmodel
    {
        public int License_Id { get; set; }
        public int Civil_Id { get; set; }
        public int Type {  get; set; }
        public string Page { get; set; }
        public string License_Type { get; set; }
        public string Type_Matter { get; set; }
        public string Through {  get; set; }
        public string Import_Export_Center { get; set; }
        public string Cites_No { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Verification), ErrorMessageResourceName = "E_CitesNo")]
        public string Jenis_Permit { get; set; }
        public List<SelectListItem> SelectJenisPermit { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Verification), ErrorMessageResourceName = "E_StatusPenyemak")]
        public string Status_By_Penyemak {  get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Verification), ErrorMessageResourceName = "E_StatusPengesah")]
        public string Status_By_Pengesah { get; set; }
        public List<SelectListItem> SelectStatus { get; set; }

        public List<SelectListItem> SelectAppendix { get; set; }

        public List<SelectListItem> SelectSource { get; set; }
        public string MainAppendix { get; set; }
        public string MainSource { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Verification), ErrorMessageResourceName = "E_SahSehingga")]
        public string Sah_Sehingga { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Verification), ErrorMessageResourceName = "E_NamaImport")]
        public string Nama_Import { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Verification), ErrorMessageResourceName = "E_NegaraImport")]
        public string Negara_Import { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Verification), ErrorMessageResourceName = "E_AlamatImport")]
        public string Alamat_Pengimport { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Verification), ErrorMessageResourceName = "E_NamaEksport")]
        public string Nama_Eksport { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Verification), ErrorMessageResourceName = "E_NegaraEksport")]
        public string Negara_Eksport { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Verification), ErrorMessageResourceName = "E_AlamatEksport")]
        public string Alamat_Pengeksport { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Verification), ErrorMessageResourceName = "E_NoStem")]
        public string No_Stem { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Verification), ErrorMessageResourceName = "E_NamaPemeriksa")]
        public string Nama_Pemeriksa { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Verification), ErrorMessageResourceName = "E_LokasiPemeriksaan")]
        public string Lokasi_Pemeriksaan { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Verification), ErrorMessageResourceName = "E_TarikhMasaSemakan")]
        public string Tarikh_MasaSemakan { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Verification), ErrorMessageResourceName = "E_Ulasan")]
        public string Catatan_Penyemak { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Verification), ErrorMessageResourceName = "E_NamaPengesah")]
        public string Nama_Pengesah { get; set; }
        public List<SelectListItem> SelectPengesah { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Verification), ErrorMessageResourceName = "E_Ulasan")]
        public string Ulasan_Pengesah { get; set; }
        public string submitType { get; set; }

        public string FileType { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Verification), ErrorMessageResourceName = "E_Tujuan")]
        public string Tujuan { get; set; }
        public int User_Id { get; set; }
        public string Pegawai_Penyemak { get; set; }
        public string Pegawai_Pengesah { get; set; }
        public HttpPostedFileBase salinanCitesOriginal { get; set; }
        public HttpPostedFileBase salinanCitesChanges { get; set; }
        public List<SelectListItem> getListImportExportCenter { get; set; }
        public List<SelectListItem> getListCodePurposeTrade { get; set; }
        public OwnerDetails GetOwnerDetails { get; set; }
        public AddressDetails GetAddressDetails { get; set; }
        public CompanyDetails GetCompanyDetails { get; set; }
        public Species getSpeciesDetails { get; set; }
        public ReviewVerify getReviewVerify {  get; set; }
        public List<Species> getSpeciesList { get; set; }
        public List<Species> getTotalSpeciesList { get; set; }
        public List<Species> getTotalQuantityHidup { get; set; }
        public List<Document> getListDocument {  get; set; }
        public Document getSpecificDoc { get; set; }
        public ReviewVerify getNamaPengesahSahaja { get; set; }
        public List<StatusLesen> getStatusBatalLesen { get; set; }
        public List<StatusLesen> getTamatTempohLesen { get; set; }
        public List<StatusLesen> getKuatkuasaLesen { get; set; }

    }

    public class Species   //inherit property from ListPermohonan
    {
        public int Main_Spesis_Id { get; set; }
        public int Spesis_Id { get; set; }
        public string Spesis_Name_Scientific { get; set; }
        public string Spesis_Name_Tempatan { get; set; }
        public string Quantity { get; set; }
        public string NoLesenMengambil { get; set; }
        public string Gender { get; set; }
        public string NamaJantina { get; set; }
        public string Specimen { get; set; }
        public string No_CR { get; set; }
        public string Original_CitesNo { get; set; }
        public string Source { get; set; }
        public string Appendix { get; set; }
        public string Catatan { get; set; }
        public string No_Penandaan { get; set; }
        public string No_Penandaan_Disahkan { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Verification), ErrorMessageResourceName = "E_QtySah")]
        public float? Spesis_Quantity_Disahkan { get; set; }
        public bool IsSelected { get; set; }

        public float? Spesis_Quantity_DisahkanHidup { get; set; }

    }

    #endregion

    #region Review and Verify
    public class ReviewVerify
    {
        public int User_Id { get; set; }
        public string Pegawai_Penyemak { get; set; }
        public string Pegawai_Pengesah { get; set; }
       
    }
    #endregion

    #region Document Attachment

    public class Document
    {
        public string File_Location { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources.Verification), ErrorMessageResourceName = "E_CopyCites")]
        public string File_Name { get; set; }
    }
    #endregion

    #region Semak Status Lesen
    public class StatusLesen
    {
        public int ActiveTabIndex { get; set; }
        public int User_Id { get; set; }
        public int JenisLesen { get; set; }
        public int LesenID { get; set; }
        public string NamaLesen { get; set; }
        public string NoLesen { get; set; }
        public string PejabatPengeluar { get; set; }
        public string TarikhMula { get; set; }
        public string TarikhTamat { get; set; }
        public string Status { get; set; }
        //public string SubmitType { get; set; }
        public string Catatan { get; set; }
        public int? StatusPemilik { get; set; }
        public string StatusLesenSemakan { get; set; }
        public List<SelectListItem> ListStatusLesen { get; set; }
        public List<SelectListItem> ListStatusPemilik { get; set; }
        public List<Document> ListDocument { get; set; }

        public HttpPostedFileBase LampiranDibatal { get; set; }
        public HttpPostedFileBase LampiranDigantung { get; set; }
        public HttpPostedFileBase Lampiran { get; set; }

        public string TarikhTindakan { get; set; }
        public string CatatanStatus { get; set; }
        public ViewSemakanLesenMemburuMengambil ViewSemakanLesenMemburuMengambil {  get; set; }
        public ViewSemakanLesenMenyimpanMenggunaKajian ViewSemakanLesenMenyimpanMenggunaKajian {  get; set; }
        public ViewSemakanLesenNiagaTaksidermi ViewSemakanLesenNiagaTaksidermi {  get; set; }
        public ViewSemakanImportEksportReEksport ViewSemakanImportEksportReEksport {  get; set; }
        public ViewSemakanPermit ViewSemakanPermit {  get; set; }
        public OwnerDetails GetOwnerDetails { get; set; }
        public AddressDetails GetAddressDetails { get; set; }
        public CompanyDetails GetCompanyDetails { get; set; }
        public List<Species> getTotalSpeciesList { get; set; }
        public Species getSpeciesDetails { get; set; }
    }
    public class ViewSemakanLesenMemburuMengambil
    {
        public bool SyaratMemburu { get; set; }
        public string TarikhBukuSenjataApi { get; set; }
        public string NoBuku { get; set; }
        public string NoSiriBuKuSenjataApi { get; set; }
        public string SuratKebenaran { get; set; }
        public string JenisSenjataApi { get; set; }
        public string NamaPembuatdanNombor { get; set; }
        public string NamaSaintifik { get; set; }
        public string Kuantiti {  get; set; }
        public int Tempoh { get;set; }
        public string Daerah { get; set; }

    }
    public class ViewSemakanLesenMenyimpanMenggunaKajian
    {
        public string NoResit { get; set; }
        public string NoLesenPerniagaan { get; set; }
        public int BakiDalamSimpanan { get; set; }
        public string SalinanResitBelian { get; set; }
        public string NamaSyarikat { get; set; }
        public string CatatanPerolehan { get; set; }
        public string Spesis { get; set; }
        public string NoLesenMengambil { get; set; }
        public string NoPenandaan { get; set; }
        public string Jantina { get; set; }
        public string NamaJantina { get; set; }

    }
    public class ViewSemakanLesenNiagaTaksidermi
    {
        public string KategoriPerniagaan { get; set; }
        public int AktivitiLesen { get; set; }
        public string AktivitiLesenDescr { get; set; }
        public string SalinanDaftarSyarikat { get; set; }
        public string SalinankelulusanPBT { get; set; }

        

    }
    public class ViewSemakanPermit
    {
        public string JenisPermit { get; set; }
        public string JenisPermitKhas { get; set; }
        public string AktivitiLesenDescr { get; set; }
        public string SalinanKertasKerja { get; set; }
        public string SalinanHidupanLiar { get; set; }
        public string SalinanSuratKelulusan { get; set; }
        public string TarikhKelulusan { get; set; }
        public string TarikhKelulusanPK { get; set; }
        public string NoSuratKelulusan { get; set; }
        public string NoSuratKelulusanPK { get; set; }
        public string Spesis { get; set; }
        public string NoPenandaan { get; set; }
        public string Jantina { get; set; }
        public string Specimen { get; set; }
        public string NamaJantina { get; set; }
        public int Melalui { get; set; }
        public string NamaMelalui { get; set; }
        public string NoCitesNegaraAsal { get; set; }
        public string NoCR { get; set; }



    }
    public class ViewSemakanImportEksportReEksport
    {
        public string Specimen { get; set; }

    }

    #endregion
}
