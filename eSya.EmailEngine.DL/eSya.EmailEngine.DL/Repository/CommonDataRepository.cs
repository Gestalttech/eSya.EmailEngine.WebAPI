using eSya.EmailEngine.DL.Entities;
using eSya.EmailEngine.DO;
using eSya.EmailEngine.DO.StaticVariables;
using eSya.EmailEngine.IF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.EmailEngine.DL.Repository
{
    public class CommonDataRepository: ICommonDataRepository
    {
        public async Task<List<DO_BusinessLocation>> GetBusinessKey()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var bk = db.GtEcbslns
                        .Where(w => w.ActiveStatus)
                        .Select(r => new DO_BusinessLocation
                        {
                            BusinessKey = r.BusinessKey,
                            LocationDescription = r.BusinessName + "-" + r.LocationDescription
                        }).ToListAsync();

                    return await bk;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_Forms>> GetFormDetails()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {

                    var result = db.GtEcfmfds
                        .Join(db.GtEcfmpas,
                            f => f.FormId,
                            p => p.FormId,
                            (f, p) => new { f, p })
                       .Where(w => w.f.ActiveStatus
                                  && w.p.ParameterId == ParameterIdValues.Form_isEmailIntegration
                                  && w.p.ActiveStatus)
                                  .Select(r => new DO_Forms
                                  {
                                      FormID = r.f.FormId,
                                      FormName = r.f.FormName,
                                      FormCode = r.f.FormCode,
                                  }).OrderBy(o => o.FormName).ToListAsync();

                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_Forms>> GetFormDetailsbyBusinessKey(int businesskey)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEcbsmns.Join(db.GtEcmnfls,
                        b => new { b.MenuKey },
                        l => new { l.MenuKey },
                        (b, l) => new { b, l }).Join(db.GtEcfmfds,
                        bl => new { bl.l.FormId },
                        f => new { f.FormId },
                        (bl, f) => new { bl, f }).Join(db.GtEcfmpas,
                        fa => new { fa.f.FormId },
                        p => new { p.FormId },
                        (fa, p) => new { fa, p })
                        .Where(w => w.fa.bl.b.BusinessKey == businesskey && w.fa.bl.b.ActiveStatus
                        && w.fa.bl.l.ActiveStatus && w.fa.f.ActiveStatus && w.p.ParameterId == ParameterIdValues.Form_isEmailIntegration && w.p.ActiveStatus)
                                  .Select(r => new DO_Forms
                                  {
                                      FormID = r.fa.f.FormId,
                                      FormName = r.fa.f.FormName,
                                      FormCode=r.fa.f.FormCode
                                  }).OrderBy(o => o.FormName).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_ApplicationCodes>> GetApplicationCodesByCodeType(int codeType)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcapcds
                        .Where(w => w.CodeType == codeType && w.ActiveStatus)
                        .Select(r => new DO_ApplicationCodes
                        {
                            ApplicationCode = r.ApplicationCode,
                            CodeDesc = r.CodeDesc
                        }).OrderBy(o => o.CodeDesc).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_BusinessLocation>> GetBusinessKeyByEmailIntegration()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var bk = db.GtEcbslns.Join(db.GtEcpabls,
                        f => f.BusinessKey,
                        p => p.BusinessKey,
                        (f, p) => new { f, p })
                        .Where(w => w.f.ActiveStatus && w.p.ParameterId == ParameterIdValues.Location_isEmailIntegration
                                  && w.p.ParmAction && w.p.ActiveStatus)
                        .Select(r => new DO_BusinessLocation
                        {
                            BusinessKey = r.f.BusinessKey,
                            LocationDescription = r.f.BusinessName + "-" + r.f.LocationDescription
                        }).ToListAsync();

                    return await bk;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> GetLocationEmailApplicable(int BusinessKey)
        {
            using (var db = new eSyaEnterprise())
            {
                var lg = await db.GtEcpabls
                    .Where(w => w.BusinessKey == BusinessKey && w.ParameterId == ParameterIdValues.Location_isEmailIntegration && w.ActiveStatus)
                    .Select(s => s.ParmAction).FirstOrDefaultAsync();
                return lg;
            }
        }
    }
}
