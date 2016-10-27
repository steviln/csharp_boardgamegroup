using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BoardgameGroup.Controllers
{
    public class MainController : BaseController
    {
        // Vet dette er "fyfy" men trengte en rask løsning

        //
        // GET: /Main/

        public string[] rangeringer = new string[] { "Ikke rangert", "Ønsker absolutt ikke å spille dette spillet", "Vil aller helst ikke spille dette spillet", "Kan til nød spille dette spillet", "Helt greit spill å spille", "Kan gjerne spille dette spillet", "Har veldig lyst til å spille dette spillet", "Favorittspill" };


        public ActionResult Index(string club)
        {
            init(club,0);
            return RedirectToAction("Frontpage");
        }

        public ActionResult Login(string club)
        {
            init(club,0);
            return View(brukerData);
        }

        public ActionResult Logout(string club) {
            LogMeOut();
            return RedirectToAction("Login");
        }

        [HttpPost]
        public ActionResult Login(string brukernavn, string passord)
        {
            init("", 0);
            List<Models.SpillerLogin> spillerne = (from m in databaseKontekst.spillereLogin where m.brukernavn == brukernavn && m.passord == passord select m).ToList();
            if (spillerne.Count() == 0)
            {
                ViewBag.errorMessage = "Feil brukernavn og/eller passord";
                return View();
            }
            else
            {
                LogMeOut();
                Structures.BoardgamesHelper.LogMeIn(spillerne.ElementAt<Models.SpillerLogin>(0), Session["sid"].ToString(), this);
             
                return RedirectToAction("Frontpage");
            }

        }

        public ActionResult redigerSelskap(string club)
        {
            int selskapID;
            Models.Selskap fraksel;
            init(club, 20);

            if (brukerData.viewStatus == 2)
            {
                selskapID = 0;

                if (Int32.TryParse(Request["selskapID"], out selskapID))
                {
                    fraksel = (from m in databaseKontekst.selskaper where m.selskapID == selskapID select m).FirstOrDefault();
                }
                else
                {
                    fraksel = new Models.Selskap();
                    fraksel.selskapID = selskapID;
                }
                return View(fraksel);
            }
            else {
                return RedirectToAction("Login");
            }
        
        }

        [HttpPost]
        public ActionResult redigerSelskap(Models.Selskap fraksel) 
        {
            init("", 20);
            if (brukerData.viewStatus == 2)
            {
                if (ModelState.IsValid)
                {
                    if (fraksel.selskapID == 0)
                    {
                        databaseKontekst.selskaper.Add(fraksel);
                        databaseKontekst.SaveChanges();
                    }
                    else
                    {
                        Models.Selskap editsek = (from m in databaseKontekst.selskaper where m.selskapID == fraksel.selskapID select m).FirstOrDefault();
                        editsek.navn = fraksel.navn;
                        databaseKontekst.SaveChanges();
                    }
                }
            }
            return RedirectToAction("/Selskap");
        }

        public ActionResult Selskap(string club)
        {
            init(club, 0);

            IEnumerable<Models.Selskap> selskapsliste = (from m in databaseKontekst.selskaper select m).ToList();

            return View(selskapsliste);
        }

        public ActionResult redigerScenario(string club)
        {
            int scenID, spillID;
            Models.SpillScenario frakred;
            init(club, 20);

            if (brukerData.viewStatus == 2)
            {
                scenID = 0;
                spillID = 0;

                if (Int32.TryParse(Request["spillID"], out spillID))
                {

                    if (Int32.TryParse(Request["itemID"], out scenID))
                    {
                        frakred = (from m in databaseKontekst.spillscenarier where m.scenarioID == scenID select m).FirstOrDefault();
                    }
                    else
                    {
                        frakred = new Models.SpillScenario();
                        frakred.spillID = spillID;
                    }
                    return View(frakred);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Login");
            }

        }

        [HttpPost]
        public ActionResult redigerScenario(Models.SpillScenario redfraksjon)
        {
            init("", 20);
            if (brukerData.viewStatus == 2)
            {
                if (ModelState.IsValid)
                {
                    if (redfraksjon.scenarioID == 0)
                    {
                        databaseKontekst.spillscenarier.Add(redfraksjon);
                        databaseKontekst.SaveChanges();
                    }
                    else
                    {
                        Models.SpillScenario editspill = (from m in databaseKontekst.spillscenarier where m.scenarioID == redfraksjon.scenarioID select m).FirstOrDefault();
                        editspill.navn = redfraksjon.navn;
                        editspill.kode = redfraksjon.kode;
                        databaseKontekst.SaveChanges();
                    }
                }

                return RedirectToAction("/Boardgames/editgame", new { itemID = redfraksjon.spillID });
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult redigerFraksjon(string club) {
            int frakID, spillID;
            Models.SpillFraksjon frakred;
            init(club,20);

            if (brukerData.viewStatus == 2)
            {
                frakID = 0;
                spillID = 0;

                if (Int32.TryParse(Request["spillID"], out spillID))
                {

                    if (Int32.TryParse(Request["itemID"], out frakID))
                    {
                        frakred = (from m in databaseKontekst.spillfraksjoner where m.fraksjonID == frakID select m).FirstOrDefault();
                    }
                    else
                    {
                        frakred = new Models.SpillFraksjon();
                        frakred.spillID = spillID;
                    }
                    return View(frakred);
                }
                else
                {
                    return View();
                }
            }
            else {
                return RedirectToAction("Login");
            }
            
        }

        [HttpPost]
        public ActionResult redigerFraksjon(Models.SpillFraksjon redfraksjon) {
            init("", 20);
            if (brukerData.viewStatus == 2)
            {
                if (ModelState.IsValid)
                {
                    if (redfraksjon.fraksjonID == 0)
                    {
                        databaseKontekst.spillfraksjoner.Add(redfraksjon);
                        databaseKontekst.SaveChanges();
                    }
                    else
                    {
                        Models.SpillFraksjon editspill = (from m in databaseKontekst.spillfraksjoner where m.fraksjonID == redfraksjon.fraksjonID select m).FirstOrDefault();
                        editspill.navn = redfraksjon.navn;
                        editspill.farge = redfraksjon.farge;
                        databaseKontekst.SaveChanges();
                    }
                }

                return RedirectToAction("/Boardgames/editgame", new { itemID = redfraksjon.spillID });
            }
            else {
                return RedirectToAction("Login");
            }
        }

        public ActionResult NyBruker(string club) {
            init(club,20);
            Models.SpillerLoginSamler datasender = new Models.SpillerLoginSamler();
            datasender.spillere = new Models.Spiller();
            datasender.spillereLogin = new Models.SpillerLogin();
            ViewBag.klubbID = brukerData.userClub.klubbID;
            return View(datasender);
        }

        // Denne metoden inneholder potensielt sikkerhetshull, må sjekkes skikkelig. 
        [HttpPost]
        public ActionResult NyBruker(Models.SpillerLoginSamler nyinfo){
            init("",20);
            string resultat;
            Models.Klubb nyklubb = null;
            string clubIDtemp = Request["clubID"].ToString();
            int newClubID;
            if (Int32.TryParse(clubIDtemp,out newClubID)) {
                nyklubb = (from m in databaseKontekst.klubber where m.klubbID == newClubID select m).FirstOrDefault();
                ViewBag.klubbID = newClubID;
            }
           
            // Her vil det krasje om bruker først later inn register skjema, logger inn i en annen fane og så trykker "submit", men det får jeg fikse senere. 
            if (brukerData.userObject == null)
            {
                resultat = Structures.BoardgamesHelper.TryRegistrer(nyinfo, nyklubb, Request["passordkopi"].ToString(), databaseKontekst, ModelState.IsValid, Session["sid"].ToString(), this);
            }
            else {
                resultat = Structures.BoardgamesHelper.TryAddPlayer(nyinfo, nyklubb, databaseKontekst, ModelState.IsValid, Session["sid"].ToString(), this);
            }
            if (resultat == "")
            {
                return RedirectToAction("Frontpage");
            }
            else
            {
                ViewBag.feilstreng = resultat;
                return View(nyinfo);
            }
        }

        public ActionResult registrerSpilldeltakelse(string club) {
            init(club,10);
     
            int spillID = 0;
            int sesjonID = 0;
            Int32.TryParse(Request["sesjonID"], out sesjonID);
            if (Int32.TryParse(Request["spillID"], out spillID))
            {
                    Models.SpillRegData spilldata = new Models.SpillRegData();
                    Structures.BoardgamesHelper.ConstructFormData(spilldata, this, sesjonID, spillID);
                    spilldata.sesjonen.Spillere.Sort(new Structures.Poengsorter());
                    //spilldata.sesjonen.Spillere.OrderBy(x => x.posisjon);
                    if (brukerData.viewStatus == 2 && (brukerData.userObject.status >= 25 || brukerData.userObject.spillerID == spilldata.sesjonen.spillerID || spilldata.sesjonen.spillsesjonID == 0))
                    {
                        return View(spilldata);
                    }
                    else {
                        return View("Resviews/Spilling", spilldata.sesjonen);
                    }
             }
             else
             {
                    return View();
             }
   

        }

        [HttpPost]
        public ActionResult registrerSpilldeltakelse(Models.SpillRegData spillreg) {
            int i = 0;

            init("",10);
            if (brukerData.viewStatus == 2)
            {
                if (ModelState.IsValid)
                //if(1 == 1)
                {
                    Models.SpillRapport rapporten = null;
                    if (spillreg.sesjonen.spillrapport.rapport != null && spillreg.sesjonen.spillrapport.rapport.Length > 0) {
                        if (spillreg.sesjonen.spillrapport.spillrapportID == 0)
                        {
                            rapporten = spillreg.sesjonen.spillrapport;
                            databaseKontekst.spillrapporter.Add(rapporten);
                            databaseKontekst.SaveChanges();
                        }
                        else {
                            rapporten = (from m in databaseKontekst.spillrapporter where m.spillrapportID == spillreg.sesjonen.spillrapport.spillrapportID select m).FirstOrDefault();
                            rapporten.overskrift = spillreg.sesjonen.spillrapport.overskrift;
                            rapporten.rapport = spillreg.sesjonen.spillrapport.rapport;
                        }
                    }

                    if (spillreg.sesjonen.spillsesjonID == 0)
                    {
                        List<Models.Spilldeltakelse> fjernmeg = new List<Models.Spilldeltakelse>();

                        if (spillreg.sesjonen.scenarioID <= 0)
                        {
                            spillreg.sesjonen.scenarioID = null;
                        }

                        for (i = 0; i < spillreg.sesjonen.Spillere.Count(); i++)
                        {
                            if (spillreg.sesjonen.Spillere[i].deltakerID <= 0)
                            {
                                fjernmeg.Add(spillreg.sesjonen.Spillere[i]);
                            }
                            else if (spillreg.sesjonen.Spillere[i].fraksjonID <= 0)
                            {
                                spillreg.sesjonen.Spillere[i].fraksjonID = null;
                            }
                          
                        }

                        foreach (Models.Spilldeltakelse tabort in fjernmeg)
                        {
                            spillreg.sesjonen.Spillere.Remove(tabort);
                        }

                        spillreg.sesjonen.spillerID = brukerData.userObject.spillerID;
                        if (rapporten != null) {
                            spillreg.sesjonen.spillrapport = rapporten;
                        }

                        databaseKontekst.spillsesjoner.Add(spillreg.sesjonen);
                        databaseKontekst.SaveChanges();

                    }
                    else
                    {
                        Models.Spillsesjon endresesjon = (from m in databaseKontekst.spillsesjoner where m.spillsesjonID == spillreg.sesjonen.spillsesjonID select m).FirstOrDefault();
                        endresesjon.dato = spillreg.sesjonen.dato;
                        endresesjon.scenarioID = spillreg.sesjonen.scenarioID;

                        endresesjon.spillrapport = rapporten;
                        if (rapporten == null) {
                            if (endresesjon.spillrapport != null) {
                                databaseKontekst.spillrapporter.Remove(endresesjon.spillrapport);
                            }
                            endresesjon.rapportID = null;
                        }

                        if (endresesjon.scenarioID <= 0)
                        {
                            endresesjon.scenarioID = null;
                        }
                        // Ja, dette kan gjøre smidigiere uten en ekstra spørring for hver spilldeltakelse, men tenker sikkerhet og enkelhet først her. 
                        for (i = 0; i < spillreg.sesjonen.Spillere.Count(); i++)
                        {
                            Models.Spilldeltakelse firstuse = spillreg.sesjonen.Spillere[i];
                            Models.Spilldeltakelse orgdeltak = (from m in endresesjon.Spillere where m.spilldeltakelseID == firstuse.spilldeltakelseID select m).FirstOrDefault();
                            if (firstuse.deltakerID <= 0)
                            {
                                databaseKontekst.deltakelser.Remove(orgdeltak);
                            }
                            else
                            {
                                if (firstuse.fraksjonID <= 0)
                                {
                                    firstuse.fraksjonID = null;
                                }

                                orgdeltak.fraksjonID = firstuse.fraksjonID;
                                orgdeltak.deltakerID = firstuse.deltakerID;
                                orgdeltak.poengsum = firstuse.poengsum;
                                orgdeltak.posisjon = firstuse.posisjon;
                            }
                        }
                        databaseKontekst.SaveChanges();
                    }
                    return RedirectToAction("SpillSesjoner/");
                }
                else { 
                    return View("Login");
                }

              
            }
            else {
                return RedirectToAction("Login");
            }
        }

        public ActionResult SpillSesjoner(string club) {
            init(club,0);
            if (brukerData.viewStatus >= 0)
            {
                int sesjonID = 0;

                Int32.TryParse(Request["sesjonID"], out sesjonID);
                if (sesjonID == 0)
                {
                    List<Models.HtmlListDisplay> listedata = (from sesjon in databaseKontekst.spillsesjoner join spillet in databaseKontekst.spillene on sesjon.spillID equals spillet.spillID select new Models.HtmlListDisplay { primaryID = sesjon.spillsesjonID, secondaryID = spillet.spillID, name = spillet.navn }).ToList<Models.HtmlListDisplay>();
                    return View("Sesjonsliste", listedata);
                }
                else
                {
                    return View();
                }
            }
            else {
                return RedirectToAction("Login");
            }

        }
        // må sjekkes senere for effekter av login
        public ActionResult Boardgames(string club, string subaction) {
            init(club,20);
            Models.Spill editspill = null;
            decimal[] backvalues;
            float[] backval;

            Models.SpillRangering rangert = null;


            if (subaction == "addgame") {
                editspill = new Models.Spill();
            }
            else if (subaction == "editgame" || subaction == "viewgame") {
                int rediger;
                if (Int32.TryParse(Request["itemID"],out rediger))
                {
                    editspill = (from m in databaseKontekst.spillene where m.spillID == rediger select m).FirstOrDefault(); 
                }
            }
            ViewBag.subaction = subaction;

            if (brukerData.userObject != null && editspill != null) { 
                IEnumerable<Models.SpillRangering> rangerte = (from m in databaseKontekst.rangeringer where m.spillID == editspill.spillID && m.spillerID == brukerData.userObject.spillerID select m).ToList();
                if (rangerte.Any())
                {
                    rangert = rangerte.FirstOrDefault();
                }
                else {
                    rangert = null;
                }
            }


            if ((brukerData.viewStatus < 2 || subaction == "viewgame") && editspill != null)
            {
                Models.SpillSeData nyeditspill = new Models.SpillSeData();
                nyeditspill.spillet = editspill;
                nyeditspill.rangeringen = rangert;
                nyeditspill.rangeringsverdier = rangeringer;
                nyeditspill.sesjoner = (from m in databaseKontekst.spillsesjoner where m.spillID == editspill.spillID select m).ToList<Models.Spillsesjon>();
                nyeditspill.rangertespillere = (from m in databaseKontekst.spillere select new Models.DisplayPlayerFront { spillerID = m.spillerID, spillernavn = m.fornavn, rating = 0 }).ToList<Models.DisplayPlayerFront>();
                nyeditspill.urangerteselskap = (from m in databaseKontekst.selskaper select new Models.DisplaySelskapSpill { selskapID = m.selskapID, selskapsnavn = m.navn }).ToList<Models.DisplaySelskapSpill>();
                nyeditspill.spilleronsker = (from m in databaseKontekst.rangeringer join k in databaseKontekst.spillere on m.spillerID equals k.spillerID where m.spillID == editspill.spillID select new Models.spillerRangeringList { rangering = m.rangering, spillerID = k.spillerID, spillernavn = k.fornavn + " " + k.etternavn }).OrderBy( x => x.rangering ).ToList<Models.spillerRangeringList>();

                nyeditspill.spillerID = 0;
                if (brukerData.userObject != null)
                {
                    nyeditspill.spillerID = brukerData.userObject.spillerID;
                }
                foreach (Models.DisplayPlayerFront spillpunkt in nyeditspill.rangertespillere)
                {
                    backvalues = Structures.BoardgamesHelper.calcRating(spillpunkt.spillerID, nyeditspill.sesjoner, 2, 100, false);
                    spillpunkt.rating = (int) backvalues[0];
                    spillpunkt.average = (float) backvalues[1];
                    spillpunkt.winrate = (float) backvalues[2];
                    spillpunkt.lossrate = (float)backvalues[3];
                }
                nyeditspill.rangertespillere.Sort(new Structures.SpillerFrontSorter());

                nyeditspill.rangertefraksjoner = (from m in databaseKontekst.spillfraksjoner where m.spillID == editspill.spillID select new Models.DisplayFactionFront { fraksjonID = m.fraksjonID, fraksjonsavn = m.navn, rating = 0 }).ToList<Models.DisplayFactionFront>();
                foreach (Models.DisplayFactionFront spillpunkt in nyeditspill.rangertefraksjoner)
                {
                    backval = Structures.BoardgamesHelper.calcFactionRating(spillpunkt.fraksjonID, nyeditspill.sesjoner, 2, 100);
                    spillpunkt.rating = (int) backval[0];
                    spillpunkt.average = backval[1];
                }
                nyeditspill.rangertefraksjoner.Sort(new Structures.FactionFrontSorter());
                return View("Resviews/Boardgame",nyeditspill);
            }
            else if (editspill == null)
            {
                return View("SpillListe", databaseKontekst.spillene.ToList());
            }
            else {
                Models.BoardgameReg sendedata = new Models.BoardgameReg();
                sendedata.spillet = editspill;
                sendedata.rangeringen = rangert;
                sendedata.rangeringsverdier = rangeringer;
                sendedata.spillerID = 0;
                if (brukerData.userObject != null) {
                    sendedata.spillerID = brukerData.userObject.spillerID;
                }
                List<Models.SelectListCustomItem> selskapsliste = new List<Models.SelectListCustomItem>();
                IEnumerable<Models.Selskap> selskapene = databaseKontekst.selskaper.ToList();
                selskapsliste.Add(new Models.SelectListCustomItem(0, "Ikke valgt"));
                foreach (Models.Selskap selskapet in selskapene) {
                    selskapsliste.Add(new Models.SelectListCustomItem(selskapet.selskapID, selskapet.navn));
                }
                sendedata.selskaper = selskapsliste;
            
                return View(sendedata);
            }
        }

        [HttpPost]
        public ActionResult Boardgames(Models.BoardgameReg sendedata) {
            init("",20);
            if (brukerData.viewStatus == 2)
            {
                Models.Spill nyspill = sendedata.spillet;
                if (ModelState.IsValid)
                {
                    if (nyspill.spillID == 0)
                    {
                        databaseKontekst.spillene.Add(nyspill);
                        databaseKontekst.SaveChanges();
                    }
                    else
                    {
                        Models.Spill editspill = (from m in databaseKontekst.spillene where m.spillID == nyspill.spillID select m).FirstOrDefault();
                        editspill.navn = nyspill.navn;
                        editspill.boardgamegeekID = nyspill.boardgamegeekID;
                        editspill.spillerantall = nyspill.spillerantall;
                        editspill.selskapID = nyspill.selskapID;
                        databaseKontekst.SaveChanges();
                    }
                }
                return RedirectToAction("Boardgames/");
            }
            else {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Users(string club) {
            init(club, 0);

            int spillerenID = 0; 
            Int32.TryParse(Request["itemID"], out spillerenID);
            decimal[] backvalues;

            if (spillerenID == 0)
            {
                IEnumerable<Models.Spiller> spillerliste = (from m in databaseKontekst.spillere select m).ToList<Models.Spiller>();
                return View(spillerliste);
            }
            else {
                Models.UserDisplayData senddata = new Models.UserDisplayData();
                senddata.spilleren = (from m in databaseKontekst.spillere where m.spillerID == spillerenID select m).FirstOrDefault();
                //senddata.sesjoner = (from m in databaseKontekst.spillsesjoner join n in databaseKontekst.deltakelser on m.spillID equals n.sesjonID where n.deltakerID == spillerenID select m).ToList<Models.Spillsesjon>();
                senddata.sesjoner = (from m in databaseKontekst.deltakelser join n in databaseKontekst.spillsesjoner on m.sesjonID equals n.spillsesjonID where m.Spiller.spillerID == spillerenID join a in databaseKontekst.spillene on n.spillID equals a.spillID select n).OrderByDescending(x => x.dato).ToList<Models.Spillsesjon>();
                backvalues = Structures.BoardgamesHelper.calcRating(spillerenID, senddata.sesjoner, 3, 100, true);
                senddata.rating = (int) backvalues[0];
                backvalues = Structures.BoardgamesHelper.calcRating(spillerenID, senddata.sesjoner, 2, 2, true);
                senddata.toplayerrating = (int)backvalues[0];

                return View("DisplayUser", senddata);
            }


        }

        public ActionResult Profilen(string club) {
            init(club,1);

            Models.SpillerLoginSamler datasend = new Models.SpillerLoginSamler();
            if (brukerData != null && brukerData.userObject != null)
            {
                int spillerID = brukerData.userObject.spillerID;
                Structures.DropdownVerdier[] statuser = { new Structures.DropdownVerdier { navn = "utestengt", val = 0 }, new Structures.DropdownVerdier { navn = "vanlig bruker", val = 1 }, new Structures.DropdownVerdier { navn = "aktiv bruker", val = 10 }, new Structures.DropdownVerdier { navn = "administrator", val = 20 }, new Structures.DropdownVerdier { navn = "eier", val = 25 } };
                datasend.statuser = new List<Models.SelectListCustomItem>();
                foreach (Structures.DropdownVerdier getSingle in statuser)
                {
                    if (getSingle.val <= brukerData.userObject.status)
                    {
                        datasend.statuser.Add(new Models.SelectListCustomItem(getSingle.val, getSingle.navn));
                    }
                }
                Int32.TryParse(Request["spillerID"], out spillerID);
                if (spillerID > 0)
                {
                    datasend.spillere = (from m in databaseKontekst.spillere where m.spillerID == spillerID select m).FirstOrDefault();
                    datasend.spillereLogin = (from m in databaseKontekst.spillereLogin where m.spillerID == spillerID select m).FirstOrDefault();

                }
                else
                {
                    datasend.spillere = new Models.Spiller();
                    datasend.spillereLogin = new Models.SpillerLogin();
                }
            }
            return View(datasend);  
        
        }

        [HttpPost]
        public ActionResult Profilen(Models.SpillerLoginSamler samleren){
            init("", 20);

            if (samleren != null && samleren.spillere != null && brukerData != null && brukerData.userObject != null && (brukerData.userObject.status > 20 || brukerData.userObject.spillerID == samleren.spillere.spillerID))
            {

                string passorden, passordto;

                Models.Spiller spilleren = (from m in databaseKontekst.spillere where m.spillerID == samleren.spillere.spillerID select m).FirstOrDefault();
                spilleren.epost = samleren.spillere.epost;
                spilleren.etternavn = samleren.spillere.etternavn;
                spilleren.fornavn = samleren.spillere.fornavn;
                spilleren.facebookID = samleren.spillere.facebookID;
                databaseKontekst.SaveChanges();

                Models.SpillerLogin spilllogin = (from m in databaseKontekst.spillereLogin where m.spillerID == spilleren.spillerID select m).FirstOrDefault();

                if (samleren.spillereLogin == null && spilllogin == null)
                {
                    if (Request["paalogginfo"] == "true,false")
                    {
                        spilllogin = new Models.SpillerLogin();
                        spilllogin.spillerID = spilleren.spillerID;
                        spilllogin = Structures.BoardgamesHelper.genererSpillerLogin(spilllogin, this.databaseKontekst);
                        databaseKontekst.spillereLogin.Add(spilllogin);
                        databaseKontekst.SaveChanges();
                    }
                }
                else if (samleren.spillereLogin != null && spilllogin != null)
                {
                    spilllogin.brukernavn = samleren.spillereLogin.brukernavn;
                    int brukstatus = spilllogin.status;
                    if (samleren.spillereLogin.status <= brukerData.userObject.status) {
                        brukstatus = samleren.spillereLogin.status;
                    }
                    spilllogin.status = brukstatus;
                    passorden = Request["passord"]; 
                    passordto = Request["passordkopi"];
                    if (passorden == passordto && passorden.Length > 0)
                    {
                        spilllogin.passord = passorden;
                    }
                    databaseKontekst.SaveChanges();
                }
            }
       
            return RedirectToAction("/Profilen", new { spillerID = samleren.spillere.spillerID });
        }

        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Frontpage(string club)
        {
            decimal[] backverdier;
            init(club,0);
            List<Models.DisplayPlayerFront> spillerliste = (from m in databaseKontekst.spillere select new Models.DisplayPlayerFront { spillerID = m.spillerID, spillernavn = m.fornavn, rating = 0 }).ToList<Models.DisplayPlayerFront>();
            List<Models.Spillsesjon> sesjonene = (from m in databaseKontekst.spillsesjoner select m).OrderByDescending( x => x.dato ).ToList<Models.Spillsesjon>();
            foreach (Models.DisplayPlayerFront spillpunkt in spillerliste) {
                backverdier = Structures.BoardgamesHelper.calcRating(spillpunkt.spillerID, sesjonene, 3, 100, true);
                spillpunkt.rating = (int) backverdier[0];
                spillpunkt.winrate = (float) backverdier[2];
                spillpunkt.lossrate = (float)backverdier[3];
            }
            spillerliste.Sort(new Structures.SpillerFrontSorter());

            Models.FrontpageData sendedata = new Models.FrontpageData();
            sendedata.rangertespillere = spillerliste;
            sendedata.sesjoner = sesjonene;

            return View(sendedata);

        }


    }
}
