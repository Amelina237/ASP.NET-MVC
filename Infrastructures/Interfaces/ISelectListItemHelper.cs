using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;

namespace ePerhilitanV2.Infrastructures.Interfaces
{
    public interface ISelectListItemHelper
    {
       
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


    }
}