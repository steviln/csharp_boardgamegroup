using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;


namespace BoardgameGroup.Structures
{
    static class BoardgamesHelper
    {
        public static decimal[] calcRating(int spillerID, List<Models.Spillsesjon> sesjoner, int playermin, int playermax, Boolean restriction) {
            decimal rating = 0,temprating = 0, tempnumber, tempavg, average = 0 , winrate = 0, winnumber = 0, lossrate = 0;
            int number = 0, totrating = 0, avgnumber = 0, tnumber, totwon = 0, losses = 0;
           
            foreach (Models.Spillsesjon sesjon in sesjoner) {
                tempnumber = (decimal) sesjon.Spillere.Count();
                if (tempnumber >= playermin && tempnumber <= playermax)
                {
                    foreach (Models.Spilldeltakelse deltak in sesjon.Spillere)
                    {
                        if (deltak.deltakerID == spillerID && Int32.TryParse(deltak.posisjon.ToString(), out tnumber))
                        {

                            temprating = (decimal)((tempnumber - deltak.posisjon) / (tempnumber - 1));
                            rating += temprating;
                            number++;
                            winnumber++;
                            if (decimal.TryParse(deltak.poengsum.ToString(), out tempavg)) {
                                avgnumber++;
                                average += tempavg;
                            }
                            if (deltak.posisjon == 1) {
                                totwon++;
                            }
                            else if (deltak.posisjon == sesjon.Spillere.Count) {
                                losses++;
                            }
                        }
                    }
                }
            }

            if (avgnumber > 0) {
                average = (decimal)(average / avgnumber);
            }
            if (number > 0) {
                winrate = (decimal) ((totwon / winnumber) * 100);
                lossrate = (decimal)((losses / winnumber) * 100);
                totrating = (int)Math.Round((rating / number) * 100);
            }
      

            
            if (number == 1 && restriction)
            {
                totrating = (int) (totrating / 2);
            }
            else if (number == 2 && restriction)
            {
                totrating = (int) (totrating / 1.5);
            }
            if (totrating < 0) {
                totrating = 0;
            }

            average = Decimal.Round(average, 0);
            winrate = Decimal.Round(winrate, 0);
            lossrate = Decimal.Round(lossrate, 0);

            return new decimal[] {totrating, average, winrate, lossrate};
        }

        public static float[] calcFactionRating(int fraksjonID, List<Models.Spillsesjon> sesjoner, int playermin, int playermax)
        {
            float rating = 0, temprating = 0, tempnumber, tempavg, average = 0;
            int number = 0, totrating = 0, avgnumber = 0;

            foreach (Models.Spillsesjon sesjon in sesjoner)
            {
                tempnumber = (float)sesjon.Spillere.Count();
                if (tempnumber >= playermin && tempnumber <= playermax)
                {
                    foreach (Models.Spilldeltakelse deltak in sesjon.Spillere)
                    {
                        if (deltak.fraksjonID == fraksjonID)
                        {

                            temprating = (float)((tempnumber - deltak.posisjon) / (tempnumber - 1));
                            rating += temprating;
                            number++;
                            if (float.TryParse(deltak.poengsum.ToString(), out tempavg))
                            {
                                avgnumber++;
                                average += tempavg;
                            }
                        }
                    }
                }
            }

            if (avgnumber > 0)
            {
                average = (float)(average / avgnumber);
            }

            totrating = (int)Math.Round((rating / number) * 100);

            if (totrating < 0)
            {
                totrating = 0;
            }
            return new float[] { totrating, average };
        }
        public static string TryRegistrer(Models.SpillerLoginSamler brukedata, Models.Klubb nyklubb, string testpassord, Models.BoardgameGroupDBContext minkontroll, Boolean modelstatus, string mysession, Controllers.MainController hovedcontro) {
            string suksess = "";

            if (testpassord != brukedata.spillereLogin.passord) {
                suksess += "De to passordene du har fylt ut er ikke like ";
            }

            if (suksess == "" && modelstatus)
            {
                minkontroll.spillere.Add(brukedata.spillere);
                minkontroll.SaveChanges();

                brukedata.spillereLogin.status = 1;
                brukedata.spillereLogin.Spiller = brukedata.spillere;
                minkontroll.spillereLogin.Add(brukedata.spillereLogin);
                minkontroll.SaveChanges();

                if (nyklubb != null)
                {
                    //brukedata.spillere.Klubber.Add(nyklubb);
                    Models.KlubbMedlemskap medlemskap = new Models.KlubbMedlemskap();
                    medlemskap.spiller = brukedata.spillere;
                    medlemskap.klubb = nyklubb;
                    minkontroll.klubbmedlemskap.Add(medlemskap);
                    minkontroll.SaveChanges();
                }

                Structures.BoardgamesHelper.LogMeIn(brukedata.spillereLogin, mysession, hovedcontro);
                
            }
            else {
                suksess += "Du har ikke fylt ut alle de obligatoriske feltene ";
            }

            return suksess;
        }

        public static Models.SpillerLogin genererSpillerLogin(Models.SpillerLogin nylogin, Models.BoardgameGroupDBContext minkontroll) {
            Models.Spiller relevantspiller = (from m in minkontroll.spillere where m.spillerID == nylogin.spillerID select m).FirstOrDefault();
            nylogin.brukernavn = relevantspiller.fornavn.Substring(0, Math.Min(3, relevantspiller.fornavn.Length)) + relevantspiller.etternavn.Substring(0, Math.Min(3, relevantspiller.fornavn.Length));
            nylogin.passord = BoardgamesHelper.genererPassordRandom();
            nylogin.status = 1;
            return nylogin;
        }

        public static string genererPassordRandom() {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            string finalString = new String(stringChars);
            return finalString;
        }

        public static string TryAddPlayer(Models.SpillerLoginSamler brukedata, Models.Klubb nyklubb, Models.BoardgameGroupDBContext minkontroll, Boolean modelstatus, string mysession, Controllers.MainController hovedcontro)
        {
            string suksess = "";



            if (suksess == "" && modelstatus)
            {
                minkontroll.spillere.Add(brukedata.spillere);
                minkontroll.SaveChanges();
                if (nyklubb != null)
                {
                    //brukedata.spillere.Klubber.Add(nyklubb);
                    Models.KlubbMedlemskap medlemskap = new Models.KlubbMedlemskap();
                    medlemskap.spiller = brukedata.spillere;
                    medlemskap.klubb = nyklubb;
                    minkontroll.klubbmedlemskap.Add(medlemskap);
                    minkontroll.SaveChanges();
                }
            }
            else
            {
                suksess += "Du har ikke fylt ut alle de obligatoriske feltene ";
            }

            return suksess;
        }

        public static void LogMeIn(Models.SpillerLogin spillere, string sessionID, Controllers.BaseController contro)
        {

            Models.brukerSession nysesjon = new Models.brukerSession();
            nysesjon.sessionID = sessionID;
            nysesjon.spillerLogin = spillere;
            contro.databaseKontekst.brukersesjoner.Add(nysesjon);
            contro.databaseKontekst.SaveChanges();

        }

        public static void ConstructFormData(Models.SpillRegData spillreg, Controllers.MainController contro, int sesjonID, int spillID) {

            int i = 0;

            if (sesjonID == 0)
            {
                spillreg.sesjonen = new Models.Spillsesjon();
                spillreg.sesjonen.dato = DateTime.Today.Date;
                spillreg.sesjonen.Spill = (from m in contro.databaseKontekst.spillene where m.spillID == spillID select m).FirstOrDefault();
                spillreg.sesjonen.spillID = spillreg.sesjonen.Spill.spillID;
                spillreg.sesjonen.klubbID = contro.brukerData.userClub.klubbID;
                spillreg.sesjonen.Spillere = new List<Models.Spilldeltakelse>();

                for (i = 0; i < spillreg.sesjonen.Spill.spillerantall; i++)
                {
                    Models.Spilldeltakelse nydel = new Models.Spilldeltakelse();
                    nydel.sesjon = spillreg.sesjonen;
                    nydel.deltakerID = -1;
                    nydel.fraksjonID = 0;
                    spillreg.sesjonen.Spillere.Add(nydel);
                }


            }
            else { 
                spillreg.sesjonen = (from m in contro.databaseKontekst.spillsesjoner where m.spillsesjonID == sesjonID select m).FirstOrDefault();
                foreach (Models.Spilldeltakelse nyvar in spillreg.sesjonen.Spillere){
                    if (nyvar.fraksjonID == null) {
                        nyvar.fraksjonID = 0;
                    }
                }
            }

            if (sesjonID == 0 && spillreg.sesjonen.spillrapport == null) {
                Models.SpillRapport rapporten = new Models.SpillRapport();
                rapporten.spillerID = contro.brukerData.userObject.spillerID;
                spillreg.sesjonen.spillrapport = rapporten;
            }

            var getQuery = (from m in contro.databaseKontekst.spillere select new { name = m.fornavn + " " + m.etternavn, ID = m.spillerID }).ToList();
            getQuery.Add(new { name = "Ukjent", ID = 0 });
            getQuery.Add(new { name = "Ikke i bruk", ID = -1 });
            spillreg.spillerne = new List<Models.SelectListCustomItem>();
            foreach(var getSingle in getQuery){
                spillreg.spillerne.Add(new Models.SelectListCustomItem(getSingle.ID, getSingle.name));
            }

            
            var fraksjoner = (from m in contro.databaseKontekst.spillfraksjoner where m.spillID == spillreg.sesjonen.Spill.spillID select new { name = m.navn, ID = m.fraksjonID }).ToList();
            if (fraksjoner.Count > 0)
            {
                fraksjoner.Add(new { name = "Ikke spesifisert", ID = 0 });
                spillreg.fraksjonene = new List<Models.SelectListCustomItem>();
                foreach (var fraksje in fraksjoner) {
                    spillreg.fraksjonene.Add(new Models.SelectListCustomItem(fraksje.ID, fraksje.name));
                }
            }
            else {
                spillreg.fraksjonene = null;
            }

            var scenarioer = (from m in contro.databaseKontekst.spillscenarier where m.spillID == spillreg.sesjonen.Spill.spillID select new { name = m.navn, ID = m.scenarioID }).ToList();
            if (scenarioer.Count > 0)
            {
                scenarioer.Add(new { name = "Ikke spesifisert", ID = 0 });
                spillreg.scenariene = new List<Models.SelectListCustomItem>();
                foreach (var fraksje in scenarioer)
                {
                    spillreg.scenariene.Add(new Models.SelectListCustomItem(fraksje.ID, fraksje.name));
                }
            }
            else
            {
                spillreg.scenariene = null;
            }
        
        }
        
    }


}