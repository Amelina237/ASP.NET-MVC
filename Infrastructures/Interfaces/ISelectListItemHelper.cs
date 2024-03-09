using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;

namespace ePerhilitanV2.Infrastructures.Interfaces
{
    public interface ISelectListItemHelper
    {
        List<SelectListItem> SelectStatus();
        List<SelectListItem> SelectICType();
        List<SelectListItem> GetDummy();
        List<SelectListItem> GetWorkType();
        List<SelectListItem> GetJenisSyarikat();
        List<SelectListItem> GetJenisPemilikan();
        List<SelectListItem> GetJenispendaftaranSyarikat();
        List<SelectListItem> GetkelasHidupanLiar();
        List<SelectListItem> GetunitKuantiti();
        List<SelectListItem> GetJadual();
        List<SelectListItem> GetUnitSpecies();
        List<SelectListItem> GetJenisLesen();
        List<SelectListItem> GetNegeri();
        List<SelectListItem> GetKategoriPengguna();
        List<SelectListItem> GetJantina();
        List<SelectListItem> GetAcquisition();
        List<SelectListItem> GetSpeciment();
        List<SelectListItem> GetPermitAppType(int type);
        List<SelectListItem> GetAcquisitionPermitType();
        List<SelectListItem> GetThrough();
        List<SelectListItem> GetPostcode();
        List<SelectListItem> GetPenyemak();
        List<SelectListItem> GetBredWild();
        List<SelectListItem> GetSebabBatal();
        List<SelectListItem> GetSebabBatalList();

        #region Pengesahan
        List<SelectListItem> GetImportExportCenter();
        List<SelectListItem> GetCodePurposeTrade();
        List<SelectListItem> SelectJenisPermit();
        List<SelectListItem> SelectStatusPermohonan();
        List<SelectListItem> SelectAppendix();
        List<SelectListItem> SelectSource();
        List<SelectListItem> SelectPengesah(int id);
        List<SelectListItem> StatusLesen();
        List<SelectListItem> StatusPemilik();
        #endregion

        #region Lesen

        List<SelectListItem> GetJenisIdWarganegara();
        List<SelectListItem> GetWarganegara();
        List<SelectListItem> GetIdType();
        List<SelectListItem> GetPekerjaanKerajaan();
        List<SelectListItem> GetJenisLesenMenembak();
        List<SelectListItem> GetStatusAda();
        List<SelectListItem> GetJenisBuruan(string jenisLesen = "");
        List<SelectListItem> GetAktivitiLesen(string jenisLesen = "");
        List<SelectListItem> GetJenisUrusan();
        List<SelectListItem> GetLesenImportExport();
        List<SelectListItem> GetMelalui();
        List<SelectListItem> GetTempoh();
        List<SelectListItem> GetJenisPerolehan();
        List<SelectListItem> GetPegawaiPenyemak(int negeriId = 0);
        List<SelectListItem> GetPegawaiPelulus(int negeriId = 0);
        List<SelectListItem> GetNamaSaintifik();
        List<SelectListItem> GetTindakanSemakan();
        List<SelectListItem> GetPindaanPermohonan(int apptype);
        List<SelectListItem> GetJenisLesen2(int apptype);

        #endregion

    }
}