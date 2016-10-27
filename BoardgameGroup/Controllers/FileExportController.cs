using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using Newtonsoft.Json;

namespace BoardgameGroup.Controllers
{
    public class FileExportController : BaseController
    {
        //
        // GET: /FileExport/

        public ActionResult Index()
        {
            return View();
        }

        public string GetGames(string subaction)
        {

            init("DSF", 20);
            string returer;

            List<Models.Spill> spillene = (from s in databaseKontekst.spillene select s).ToList<Models.Spill>();
            try
            {
                returer = JsonConvert.SerializeObject(spillene, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore });
            }
            catch (Exception e) {
                returer = e.ToString();
            }
          
            return returer;
        }

        public string GetCompanies(string subaction)
        {

            init("DSF", 20);
            string returer;

            List<Models.Selskap> spillene = (from s in databaseKontekst.selskaper select s).ToList<Models.Selskap>();
            try
            {
                returer = JsonConvert.SerializeObject(spillene, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore });
            }
            catch (Exception e)
            {
                returer = e.ToString();
            }

            return returer;
        }

        public string GetSpiller(string subaction)
        {

            init("DSF", 20);

            string returer;

            List<Object> spillene = (from s in databaseKontekst.spillere join l in databaseKontekst.spillereLogin on s.spillerID equals l.spillerID into glist from sl in glist.DefaultIfEmpty() select new { id = s.spillerID, fornavn = s.fornavn, etternavn = s.etternavn, facebook = s.facebookID, epost = s.epost, brukernavn = (sl == null ? String.Empty : sl.brukernavn), passord = (sl == null ? String.Empty : sl.passord) }).ToList<Object>();
            try
            {
                returer = JsonConvert.SerializeObject(spillene, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore });
            }
            catch (Exception e)
            {
                returer = e.ToString();
            }


            return returer;
        }

        public string GetSpillsesjon(string subaction)
        {

            init("DSF", 20);

            string returer;

            List<Object> spillene = (from s in databaseKontekst.spillsesjoner select new { id = s.spillsesjonID, spill = s.spillID, dato = s.dato, scenarioID = s.scenarioID, registrar = s.registrar, deltakelser = (from d in databaseKontekst.deltakelser where d.sesjonID == s.spillsesjonID select new { spillerID = d.deltakerID, fraksjonID = d.fraksjonID, poengsum = d.poengsum, posisjon = d.posisjon }).ToList<Object>() }).ToList<Object>();
            try
            {
                returer = JsonConvert.SerializeObject(spillene, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore });
            }
            catch (Exception e)
            {
                returer = e.ToString();
            }


            return returer;
        }
    }
}
