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

        public List<SelectListItem> SelectStatus()
        {
            List<SelectListItem> statuses = new List<SelectListItem>
            {
                new SelectListItem {Text = "Ya", Value="1"},
                new SelectListItem {Text = "Tidak", Value = "0"},
            };

            return statuses.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value.ToString()
            }).ToList();
        }

        public List<SelectListItem> SelectICType()
        {
            return _context.dDocumentTypes.Select(x => new SelectListItem()
            {
                Text = x.doc_desc,
                Value = x.id.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetDummy()
        {
            List<SelectListItem> item = new List<SelectListItem>();

            return item;
        }

        public List<SelectListItem> GetWorkType()
        {
            return _context.dOccupation_1.Select(x => new SelectListItem()
            {
                Text = x.occu_desc,
                Value = x.id.ToString()
            }).ToList();

        }

        public List<SelectListItem> GetJenisSyarikat()
        {
            return _context.dOrganizationType1.Select(x => new SelectListItem()
            {
                Text = x.doc_desc,
                Value = x.id.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetJenisPemilikan()
        {
            return _context.dOrganizationType2.Select(x => new SelectListItem()
            {
                Text = x.sub_doc_desc,
                Value = x.id.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetJenispendaftaranSyarikat()
        {
            return _context.dOrganizationDocumentTypes.Select(x => new SelectListItem()
            {
                Text = x.doc_desc,
                Value = x.id.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetkelasHidupanLiar()
        {

            List<SelectListItem> statuses = new List<SelectListItem>
            {
                new SelectListItem {Text = "Mamalia", Value = "1"},
                new SelectListItem {Text = "Burung", Value = "2"},
                new SelectListItem {Text = "Reptilia", Value = "3"},
                new SelectListItem {Text = "Serangga", Value = "4"},
                new SelectListItem {Text = "Hirudinoidea", Value = "5"},
                new SelectListItem {Text = "Gastropoda", Value = "6"},
                new SelectListItem {Text = "Amfibia", Value = "7"},
                new SelectListItem {Text = "Araknida", Value = "8"},
                new SelectListItem {Text = "Insekta", Value = "9"},
            };

            return statuses.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetunitKuantiti()
        {
            List<SelectListItem> statuses = new List<SelectListItem>
            {
                new SelectListItem {Text = "Ekor", Value = "ekor"},
                new SelectListItem {Text = "Unit", Value = "unit"},
                new SelectListItem {Text = "Keping", Value = "keping"},
                new SelectListItem {Text = "Pasang", Value = "pasang"},
                new SelectListItem {Text = "Kilo", Value = "kilo"},
            };

            return statuses.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetJadual()
        {

            List<SelectListItem> statuses = new List<SelectListItem>
            {
                new SelectListItem {Text = "Tidak Terancam", Value = "1"},
                new SelectListItem {Text = "Terancam", Value = "2"},
            };

            return statuses.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetUnitSpecies()
        {

            List<SelectListItem> statuses = new List<SelectListItem>
            {
                new SelectListItem {Text = "Ekor", Value = "ekor"},
                new SelectListItem {Text = "Unit", Value = "unit"},
                new SelectListItem {Text = "Keping", Value = "keping"},
                new SelectListItem {Text = "Pasang", Value = "pasang"},
                new SelectListItem {Text = "Kilo", Value = "kilo"},
                new SelectListItem {Text = "Lesen", Value = "lesen"},
                new SelectListItem {Text = "1", Value = "1"},
                new SelectListItem {Text = "Kumpul_10", Value = "kumpul_10"},
                new SelectListItem {Text = "Kumpul_100", Value = "kumpul_100"},
            };

            return statuses.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetJenisLesen()
        {
            return _context.dLicenseDetails.Select(x => new SelectListItem()
            {
                Text = x.desc_LicenseDetail,
                Value = x.id.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetNegeri()
        {

            return _context.dStates.Select(x => new SelectListItem()
            {
                Text = x.state_desc,
                Value = x.id.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetKategoriPengguna()
        {
            return _context.dPositions.Select(x => new SelectListItem()
            {
                Text = x.posName,
                Value = x.posId.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetJantina()
        {
            List<SelectListItem> statuses = new List<SelectListItem>
            {
                new SelectListItem {Text = "Jantan", Value="1"},
                new SelectListItem {Text = "Betina", Value = "2"},
                new SelectListItem {Text = "Tidak Pasti", Value = "3"},
            };

            return statuses.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetAcquisition()
        {
            List<SelectListItem> statuses = new List<SelectListItem>
            {
                new SelectListItem {Text = "Pertukaran", Value="tukar"},
                new SelectListItem {Text = "Pinjaman Pembiakan", Value = "biak"},
                new SelectListItem {Text = "Permit Khas Mengambil", Value = "tangkap"},
                new SelectListItem {Text = "Permit Khas Import", Value = "import"},
                new SelectListItem {Text = "Lain - Lain", Value = "lain"},
            };

            return statuses.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetSpeciment()
        {
            List<SelectListItem> statuses = new List<SelectListItem>
            {
                new SelectListItem {Text = "Hidup", Value="1"},
                new SelectListItem {Text = "Bahagian / Terbitan", Value = "2"},
                new SelectListItem {Text = "Trofi", Value = "3"},
            };

            return statuses.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetPermitAppType(int type = 0)
        {
            List<SelectListItem> statuses = new List<SelectListItem> { };

            if (type == 3)
            {
                statuses = new List<SelectListItem>
                {
                    new SelectListItem {Text = "menggunakan mana - mana hidupan liar yang dilindungi sepenuhnya bagi pengendalian zoo", Value="1"},
                    new SelectListItem {Text = "menggunakan mana - mana hidupan liar yang dilindungi sepenuhnya bagi sarkas atau pameran hidupan liarnya", Value = "2"},
                    new SelectListItem {Text = "menggunakan mana - mana hidupan liar yang dilindungi sepenuhnya bagi pembiakbakaan dalam kurungan komersial", Value = "3"},
                };
            }
            else if (type == 4)
            {
                statuses = new List<SelectListItem>
                {
                    new SelectListItem {Text = "Penyelidikan Atau Kajian Ke Atas Hidupan Liar-Komersial(Dilindungi Sepenuhnya)", Value="1"},
                    new SelectListItem {Text = "Penyelidikan  Atau Kajian Ke Atas Hidupan Liar-Akademik(Dilindungi Sepenuhnya)", Value = "3"},
                };
            }


            return statuses.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetAcquisitionPermitType()
        {
            List<SelectListItem> statuses = new List<SelectListItem>
            {
                new SelectListItem {Text = "Permit Khas Memburu/ Mengambil", Value="tangkap"},
                new SelectListItem {Text = "Permit Khas Import", Value = "import"},
                new SelectListItem {Text = "Permit Khas Menyimpan", Value = "simpan"},
                new SelectListItem {Text = "Lain - lain", Value = "lain"},
            };

            return statuses.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetThrough()
        {
            return _context.dImportEksportCenters.Select(x => new SelectListItem()
            {
                Text = x.center_name,
                Value = x.id.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetPostcode()
        {
            return _context.dPostcodes.Select(x => new SelectListItem()
            {
                Text = x.post_no + " - " + x.city,
                Value = x.id.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetPenyemak()
        {

            List<SelectListItem> statuses = new List<SelectListItem>
            {
                new SelectListItem {Value = "712", Text="AIMI AMALINA BINTI ABDULLAH"},
                new SelectListItem {Value = "883", Text = "AINI HAYATI BINTI ALIAS"},
                new SelectListItem {Value = "896", Text = "AMAL GHAZALI BIN NASRON"},
                new SelectListItem {Value = "598", Text = "ARHAM SYAZAILI YAHYA ARIFF"},
                new SelectListItem {Value = "633", Text = "EZRUL HAZIQ BIN JAMALUDIN"},
                new SelectListItem {Value = "746", Text = "HANIS IRYANI BINTI ISMAIL"},
                new SelectListItem {Value = "644", Text = "MAHATHIR BIN MOHAMAD"},
            };

            return statuses.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetBredWild()
        {
            List<SelectListItem> statuses = new List<SelectListItem>
            {
                new SelectListItem {Text = @Resources.Management.MGM_Bred, Value="1"},
                new SelectListItem {Text = @Resources.Management.MGM_Wild, Value = "2"},
            };

            return statuses.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value.ToString()
            }).ToList();
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
            ////return all list of user_position = 12 (penyemak/pengesah pintu masuk)
            //return _context.GBL_USERS_MST.Where(a => a.user_position == 12 && a.user_status == 1).Select(x => new SelectListItem()
            //{
            //    Value = x.id.ToString(),
            //    Text = x.user_fullname
            //}).ToList();

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

        #region Lesen
        public List<SelectListItem> GetJenisIdWarganegara()
        {
            return _context.dDocumentTypes.Where(x => x.type_status == 1).Select(x => new SelectListItem()
            {
                Text = x.doc_desc,
                Value = x.id.ToString()
            }).ToList();
        }
        public List<SelectListItem> GetWarganegara()
        {
            return _context.dNationalities.Select(x => new SelectListItem()
            {
                Text = x.national_desc,
                Value = x.id.ToString()
            }).ToList();
        }
        public List<SelectListItem> GetIdType()
        {
            List<SelectListItem> statuses = new List<SelectListItem>
            {
                new SelectListItem { Text = Resources.Resource._Ic_No, Value = "1" },
                new SelectListItem { Text = Resources.Resource._Passport_No, Value = "2" }
            };

            return statuses.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetPekerjaanKerajaan()
        {
            return _context.dOccupation_2.Select(x => new SelectListItem()
            {
                Text = x.sub_occu_desc,
                Value = x.id.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetJenisLesenMenembak()
        {
            List<SelectListItem> statuses = new List<SelectListItem>
            {
                new SelectListItem { Text = "Dengan Menembak", Value = "1" },
                new SelectListItem { Text = "Selain Menembak", Value = "2" }
            };

            return statuses.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetStatusAda()
        {
            List<SelectListItem> statuses = new List<SelectListItem>
            {
                new SelectListItem { Text = "Tiada", Value = "0" },
                new SelectListItem { Text = "Ada", Value = "1" }
            };

            return statuses.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetJenisBuruan(string jenisLesen = "")
        {
            List<SelectListItem> statuses = new List<SelectListItem>();

            if (jenisLesen == "2")
            {
                statuses = new List<SelectListItem>
                {
                    new SelectListItem { Text = Resources.License.LES_HidupanLiar, Value = "1" }
                };
            }
            else if (jenisLesen == "14")
            {
                statuses = new List<SelectListItem>
                {
                    new SelectListItem { Text = Resources.License.LES_MemungutSarangBurung, Value = "2" }
                };
            }
            else
            {
                statuses = new List<SelectListItem>
                {
                    new SelectListItem { Text = Resources.License.LES_HidupanLiar, Value = "1" },
                    new SelectListItem { Text = Resources.License.LES_MemungutSarangBurung, Value = "2" }
                };
            }

            return statuses.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetAktivitiLesen(string jenisLesen = "")
        {
            List<SelectListItem> statuses = new List<SelectListItem>();

            if (jenisLesen == "5")
            {
                statuses = new List<SelectListItem>
                {
                    new SelectListItem { Text = "membuat atau mencipta suatu barang atau benda daripada mana-mana hidupan liar atau mana-mana bahagian atau terbitan mana-mana hidupan liar", Value = "2" },
                    new SelectListItem { Text = "menjual mana-mana hidupan liar yang dilindungi atau mana-mana bahagian atau terbitan mana-mana hidupan liar yang dilindungi sebagai makanan atau bagi maksud perubatan", Value = "3" },
                    new SelectListItem { Text = "menjual atau membeli untuk dijual semula mana-mana bahagian atau terbitan mana-mana hidupan liar yang dilindungi", Value = "4" },
                    new SelectListItem { Text = "menempatkan, mengurung atau membiakkan mana-mana hidupan liar untuk dijual", Value = "5" }
                };
            }
            else if (jenisLesen == "6")
            {
                statuses = new List<SelectListItem>
                {
                    new SelectListItem { Text = "menyediakan, mengawet, mengisi atau melekapkan mana-mana hidupan liar atau mana-mana bahagian atau terbitan mana-mana hidupan liar", Value = "1" },
                };
            }
            else
            {
                statuses = new List<SelectListItem>
                {
                    new SelectListItem { Text = "menyediakan, mengawet, mengisi atau melekapkan mana-mana hidupan liar atau mana-mana bahagian atau terbitan mana-mana hidupan liar", Value = "1" },
                    new SelectListItem { Text = "membuat atau mencipta suatu barang atau benda daripada mana-mana hidupan liar atau mana-mana bahagian atau terbitan mana-mana hidupan liar", Value = "2" },
                    new SelectListItem { Text = "menjual mana-mana hidupan liar yang dilindungi atau mana-mana bahagian atau terbitan mana-mana hidupan liar yang dilindungi sebagai makanan atau bagi maksud perubatan", Value = "3" },
                    new SelectListItem { Text = "menjual atau membeli untuk dijual semula mana-mana bahagian atau terbitan mana-mana hidupan liar yang dilindungi", Value = "4" },
                    new SelectListItem { Text = "menempatkan, mengurung atau membiakkan mana-mana hidupan liar untuk dijual", Value = "5" }
                };
            }
            
            

            return statuses.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetJenisUrusan()
        {
            List<SelectListItem> statuses = new List<SelectListItem>
            {
                new SelectListItem { Text = "Luar Negara", Value = "1" },
                new SelectListItem { Text = "Sabah / Sarawak", Value = "2" }
            };

            return statuses.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetLesenImportExport()
        {
            List<SelectListItem> statuses = new List<SelectListItem>
            {
                new SelectListItem { Text = "Import", Value = "1" },
                new SelectListItem { Text = "Eksport", Value = "2" },
                new SelectListItem { Text = "Eksport Semula", Value = "3" }
            };

            return statuses.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetMelalui()
        {
            return _context.dImportEksportCenters.Select(x => new SelectListItem()
            {
                Text = x.center_name,
                Value = x.id.ToString()
            }).ToList();
        }

        
        public List<SelectListItem> GetSebabBatal()
        {
            List<SelectListItem> statuses = new List<SelectListItem>
            {
                new SelectListItem { Text = @Resources.Management.MGM_LesenCancel, Value = "1" },
                new SelectListItem { Text = @Resources.Management.MGM_LesenSuspend, Value = "2" },
            };

            return statuses.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetSebabBatalList()
        {
            List<SelectListItem> statuses = new List<SelectListItem>
            {
                new SelectListItem { Text = "Pemilik meninggal dunia", Value = "1" },
                new SelectListItem { Text = "Pemilik serah hidupan liar kepada jabatan", Value = "2" },
                new SelectListItem { Text = "Pemilik langgar syarat lesen / permit / permit khas", Value = "3" },
                new SelectListItem { Text = "Pemilik tidak patuhi Akta", Value = "4" },
                new SelectListItem { Text = "Pemilik disabitkan di mahkamah", Value = "5" },
                new SelectListItem { Text = "Pemilik mendapatkan lesen melalui representasi palsu", Value = "6" },
            };

            return statuses.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetTempoh()
        {
            List<SelectListItem> statuses = new List<SelectListItem>
            {
                new SelectListItem { Text = "1", Value = "1" },
                new SelectListItem { Text = "3", Value = "3" },
                new SelectListItem { Text = "4", Value = "4" },
                new SelectListItem { Text = "6", Value = "6" },
                new SelectListItem { Text = "12", Value = "12" }
            };

            return statuses.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetJenisPerolehan()
        {
            List<SelectListItem> statuses = new List<SelectListItem>
            {
                new SelectListItem { Text = "Belian", Value = "1" },
                new SelectListItem { Text = "Jualan Jabatan", Value = "2" },
                new SelectListItem { Text = "Lesen Memburu / Mengambil", Value = "3" },
                new SelectListItem { Text = "Lesen Import (CITES)", Value = "4" },
                new SelectListItem { Text = "Permit Zoo", Value = "5" },
                new SelectListItem { Text = "Permit Pembiakbakaan", Value = "6" },
                new SelectListItem { Text = "Permit Pameran Tetap", Value = "7" },
                new SelectListItem { Text = "Lain-lain", Value = "8" }
            };

            return statuses.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetPegawaiPenyemak(int negeriId = 0)
        {
            if (negeriId != 0)
            {
                return _context.GBL_USERS_MST.Where(x => (x.user_position == 2 || x.user_position == 9) && x.user_negeri == negeriId).Select(x =>
                    new SelectListItem()
                    {
                        Text = x.user_fullname,
                        Value = x.id.ToString()
                    }).ToList();
            }

            return _context.GBL_USERS_MST.Where(x => x.user_position == 2 || x.user_position == 9).Select(x =>
                new SelectListItem()
                {
                    Text = x.user_fullname,
                    Value = x.id.ToString()
                }).ToList();

        }

        public List<SelectListItem> GetPegawaiPelulus(int negeriId = 0)
        {
            if (negeriId != 0)
            {
                return _context.GBL_USERS_MST.Where(x => (x.user_position == 3 || x.user_position == 9) && x.user_negeri == negeriId).Select(x =>
                    new SelectListItem()
                    {
                        Text = x.user_fullname,
                        Value = x.id.ToString()
                    }).ToList();
            }

            return _context.GBL_USERS_MST.Where(x => x.user_position == 3 || x.user_position == 9).Select(x =>
                new SelectListItem()
                {
                    Text = x.user_fullname,
                    Value = x.id.ToString()
                }).ToList();
        }

        public List<SelectListItem> GetNamaSaintifik()
        {
            return _context.dSpesisNames.Select(x => new SelectListItem()
            {
                Text = x.spp_saintifik,
                Value = x.id.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetTindakanSemakan()
        {
            List<SelectListItem> statuses = new List<SelectListItem>
            {
                new SelectListItem { Text = "Lulus", Value = "1" },
                new SelectListItem { Text = "Pinda", Value = "2" },
            };

            return statuses.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetPindaanPermohonan(int apptype)
        {
            List<SelectListItem> statuses = new List<SelectListItem>();

            if (apptype == 1)
            {
                statuses = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Lesen", Value = "1" },
                    new SelectListItem { Text = "Hidupan Liar", Value = "2" }
                };
            }
            else if (apptype == 2)
            {
                statuses = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Alamat Menyimpan", Value = "3" },
                    new SelectListItem { Text = "Perolehan", Value = "4" },
                    new SelectListItem { Text = "Senarai Hidupan Liar", Value = "5" }
                };
            }
            else if (apptype == 3)
            {
                statuses = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Syarikat", Value = "6" },
                    new SelectListItem { Text = "Lesen", Value = "1" },
                    new SelectListItem { Text = "Lampiran", Value = "7" }
                };
            }
            else if (apptype == 4)
            {
                statuses = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Syarikat", Value = "6" },
                    new SelectListItem { Text = "Lesen", Value = "1" },
                    new SelectListItem { Text = "Senarai Hidupan Liar", Value = "5" },
                    new SelectListItem { Text = "Lampiran", Value = "7"}
                };
            }

            return statuses.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetJenisLesen2(int apptype)
        {
            List<SelectListItem> statuses = new List<SelectListItem>();
            if (apptype == 1)
            {
                statuses = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Memburu", Value = "1" },
                    new SelectListItem { Text = "Mengambil", Value = "2" },
                    new SelectListItem { Text = "Memungut", Value = "14" },
                };
            }
            else if (apptype == 2)
            {
                statuses = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Menyimpan", Value = "4" },
                };
            }
            else if (apptype == 3)
            {
                statuses = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Perniagaan", Value = "5" },
                    new SelectListItem { Text = "Taksidermi", Value = "6" },
                };
            }
            else if (apptype == 4)
            {
                statuses = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Import", Value = "7" },
                    new SelectListItem { Text = "Eksport", Value = "8" },
                    new SelectListItem { Text = "Eksport Semula", Value = "9" },
                };
            };

            return statuses.Select(x => new SelectListItem()
            {
                Text = x.Text,
                Value = x.Value.ToString()
            }).ToList();
        }
        #endregion
    }

}