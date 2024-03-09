using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ePerhilitanV2.Models;
using ePerhilitanV2.Infrastructures;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using System.Collections;
using ePerhilitanV2.Models.ViewModels;
using ePerhilitanV2.Infrastructures.Interfaces;
using Antlr.Runtime.Misc;
using Newtonsoft.Json.Linq;
using System.Drawing;
using Resources;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Security.Policy;
using System.Resources;

namespace ePerhilitanV2.Infrastructures.Helpers
{
    public class SelectListItemHelper : ISelectListItemHelper
    {
        private readonly eLesen_SPJPEntities _context;
        public SelectListItemHelper()
        {
            _context = new eLesen_SPJPEntities();
        }



        #region Pengesahan

        public List<SelectListItem> GetImportExportCenter()
        {
            return _context.dImportEksportCenters.Select(x => new SelectListItem()
            {
                Value = x.id.ToString(),
                Text = x.center_name
            }).ToList();

        }

        public List<SelectListItem> GetCodePurposeTrade()
        {
            return _context.dCodePurposeTrades.Select(x => new SelectListItem()
            {
                Value = x.Code,
                Text = x.Code + " - " + x.Description,
            }).ToList();

        }

        public List<SelectListItem> SelectJenisPermit()
        {
            List<SelectListItem> jenisPermit = new List<SelectListItem>
            {
                new SelectListItem {Text = Verification.VV_Cites_Info, Value = "1"},
                new SelectListItem {Text = Verification.VV_NonCites, Value = "2"},
            };

            return jenisPermit.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value.ToString()
            }).ToList();
        }

        public List<SelectListItem> SelectStatusPermohonan()
        {
            List<SelectListItem> statusPermohonan = new List<SelectListItem>
            {
                new SelectListItem {Text = Verification.Valid, Value = "1"},
                new SelectListItem {Text = Verification.NonValid, Value = "2"},
            };

            return statusPermohonan.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value.ToString()
            }).ToList();
        }

        public List<SelectListItem> SelectAppendix()
        {
            List<SelectListItem> appendix = new List<SelectListItem>
            {
                new SelectListItem {Text = "I", Value = "I"},
                new SelectListItem {Text = "II", Value = "II"},
                new SelectListItem {Text = "III", Value = "III"},
            };

            return appendix.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value.ToString(),
            }).ToList();
        }

        public List<SelectListItem> SelectSource()
        {
            List<SelectListItem> source = new List<SelectListItem>
            {
                new SelectListItem {Text = "W", Value = "W"},
                new SelectListItem {Text = "X", Value = "X"},
                new SelectListItem {Text = "R", Value = "R"},
                new SelectListItem {Text = "D", Value = "D"},
                new SelectListItem {Text = "A", Value = "A"},
                new SelectListItem {Text = "C", Value = "C"},
                new SelectListItem {Text = "F", Value = "F"},
                new SelectListItem {Text = "U", Value = "U"},
                new SelectListItem {Text = "I", Value = "I"},
                new SelectListItem {Text = "O", Value = "O"},
            };

            return source.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value.ToString(),
            }).ToList();
        }

        public List<SelectListItem> SelectPengesah(int id)
        {
           

            var namaPengesah = (from a in _context.GBL_USERS_MST
                                join b in _context.GBL_USER_POSITION_MST on a.id equals b.user_id
                                join c in _context.dPositions on b.user_position_id equals c.posId
                                where b.user_position_id == 12 && a.user_status == 1 && id != a.id
                                select new SelectListItem
                                {
                                    Value = a.id.ToString(),
                                    Text = a.user_fullname
                                }).ToList();
            return namaPengesah;

        }

        public List<SelectListItem> StatusLesen()
        {
            
            return _context.dStatus.Where(a => a.id == 13 || a.id == 14 || a.id == 17 || a.id == 18).Select(x => new SelectListItem()
            {
                Value = x.id.ToString(),
                Text = x.desc_status.ToUpper()
            }).ToList();

        }

        public List<SelectListItem> StatusPemilik()
        {

            List<SelectListItem> statusPemilik = new List<SelectListItem>
            {
                new SelectListItem {Text = Verification._Active, Value = "1"},
                new SelectListItem {Text = Verification._NotActive, Value = "2"},
            };
            return statusPemilik.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value.ToString()
            }).ToList();
        }

        #endregion

        