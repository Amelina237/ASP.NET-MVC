using System;
using System.Collections.Generic;

namespace ePerhilitanV2.Models.ViewModels.JqueryDatatableParamVM
{
    public class JqueryDatatableParamVM
    {
        public string draw { get; set; }
        public string search { get; set; }
        public int Length { get; set; }
        public int Start { get; set; }
        public int iColumns { get; set; }
        public int iSortCol_0 { get; set; }
        public string sSortDir_0 { get; set; }
        public int iSortingCols { get; set; }
        public string sColumns { get; set; }
        public string FileName { get; set; }
        public string RoleCode { get; set; }
    }

    public class JqueryDatatableServiceParamVM
    {
        public string draw { get; set; }
        public int iTotalRecords { get; set; }
        public int iTotalDisplayRecords { get; set; }
        public List<JqueryDatatableServiceParamListDataVM> listData { get; set; }

    }

    public class JqueryDatatableServiceParamListDataVM
    {
        public string dataString1 { get; set; }
        public string dataString2 { get; set; }
        public string dataString3 { get; set; }
        public string dataString4 { get; set; }
        public string dataString5 { get; set; }
        public string dataString6 { get; set; }
        public string dataString7 { get; set; }
        public string dataString8 { get; set; }
        public string dataString9 { get; set; }
        public string dataString10 { get; set; }
        public int dataInt1 { get; set; }
        public int dataInt2 { get; set; }
        public int dataInt3 { get; set; }
        public int dataInt4 { get; set; }
        public int dataInt5 { get; set; }
        public DateTime dataDate1 { get; set; }
        public DateTime dataDate2 { get; set; }
        public DateTime dataDate3 { get; set; }
        public DateTime dataDate4 { get; set; }
        public DateTime dataDate5 { get; set; }
        public DateTime? dataDate6 { get; set; }
        public DateTime? dataDate7 { get; set; }
        public int? dataInt6 { get; set; }
        public int? dataInt7 { get; set; }
        public int? dataInt8 { get; set; }
        public int? dataInt9 { get; set; }
        public bool? dataBool1 { get; set; }
        public int? dataStatus { get; set; }
        public string dataStatusStr { get; set; }
        public double? dataDouble1 { get; set; }
        public string dataImage { get; set; }
        public List<int> UserPositions { get; set; }

        #region Pengesahan

        public string Id_Lesen { get; set; }
        public string No_Lesen { get; set;}
        public string NamaSyarikat { get; set;}
        public string NamaSaintifik { get; set;}
        public string NamaBiasa { get; set;}
        public string Emel { get; set;}
        public string Alamat { get; set;}
        public int JenisAlamat { get; set;}
        public string JenisNamaAlamat { get; set;}
        public int OwnerTable { get; set;}
        public string NoPenandaan { get; set;}
        public string RocNo { get; set;}
        public string Cites_No { get; set; }
        public string Kategori_Lesen { get; set;}
        public string Nama_Pemegang { get; set;}
        public string Id_Pemegang { get; set;}
        public string NoTel { get; set;}
        public float Kuantiti_Disahkan { get; set; }
        public string Penyemak_Pintu_Masuk {  get; set;}
        public string Pengesah_Pintu_Masuk {  get; set;}
        public DateTime? Tarikh_Htr_Borang { get; set; }
        public DateTime? TarikhMula { get; set; }
        public DateTime? TarikhTamat { get; set; }
        public string Status_Penyemak { get; set; }
        public string Status_Pengesah { get; set; }
        public string PejabatPengeluar { get; set; }
        #endregion
    }
}
