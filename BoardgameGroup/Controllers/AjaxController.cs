using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook;

namespace BoardgameGroup.Controllers
{
    public class AjaxController : BaseController
    {
        //
        // GET: /Ajax/



        public String Index()
        {
            return "ajax test";
        }


        public ActionResult FacebookLogin() {



            string accessToken = Request.Form["accessToken"];
            FacebookClient loggeren = new FacebookClient(accessToken);
            dynamic result = loggeren.Get("me", new { fields = "name,id" });

            string faceNavn = result.name.ToString();
            string faceID = result.id.ToString();

            faceNavn = faceNavn.Trim();
            faceID = faceID.Trim();

            IEnumerable<Models.Spiller> relspillere = (from m in databaseKontekst.spillere where m.facebookID == faceID select m).ToList();
            if (relspillere.Count() > 0)
            {
                LoggSpiller(relspillere, faceID);
                return RedirectToAction("Frontpage", "Main");
            }
            else {
                relspillere = (from m in databaseKontekst.spillere where (m.fornavn.Trim() + " " + m.etternavn.Trim()) == faceNavn select m).ToList();
                if (relspillere.Count() > 0)
                {
                    LoggSpiller(relspillere, faceID);
                    return RedirectToAction("Frontpage","Main");
                }
                else {
                    Models.LoginFail fail = new Models.LoginFail();
                    fail.faceID = faceID;
                    fail.faceNavn = faceNavn;
                    databaseKontekst.loginfail.Add(fail);
                    databaseKontekst.SaveChanges();
                    return RedirectToAction("Login", "Main");
                }
            }

            
        }

        public Boolean LoggSpiller(IEnumerable<Models.Spiller> relspillere,string faceID) {

            Models.Spiller spilleren = relspillere.FirstOrDefault();
            spilleren.facebookID = faceID;
            databaseKontekst.SaveChanges();
            Models.SpillerLogin loggit = (from m in databaseKontekst.spillereLogin where m.spillerID == spilleren.spillerID select m).FirstOrDefault();
            if (loggit != null)
            {
                LogMeOut();
                Structures.BoardgamesHelper.LogMeIn(loggit, Session["sid"].ToString(), this);
                return true;
            }  
            else{
                return false;
            }
        
        }


        public String AddUser(string club)
        {
            String returstreng = "feil";

            init(club, 20);
            if (brukerData.viewStatus == 2) { 
                string firstname = Request.QueryString["first"];
                string secondname = Request.QueryString["second"];
                if (firstname.Length > 0 && secondname.Length > 0) { 
                    Models.Klubb nyklubb = (from m in databaseKontekst.klubber select m).FirstOrDefault();
                    Models.Spiller nyspiller = new Models.Spiller();
                    Models.KlubbMedlemskap nymed = new Models.KlubbMedlemskap();
                    nymed.klubbID = nyklubb.klubbID;
                    nyspiller.fornavn = firstname;
                    nyspiller.etternavn = secondname;
                    databaseKontekst.spillere.Add(nyspiller);
                    databaseKontekst.SaveChanges();

                    nymed.spillerID = nyspiller.spillerID;
                    databaseKontekst.klubbmedlemskap.Add(nymed);
                    databaseKontekst.SaveChanges();

                    returstreng = "" + nyspiller.spillerID + ";" + nyspiller.fornavn + " " + nyspiller.etternavn;
                }
            }
            return returstreng;
        }

        public String AddSelskap(string club)
        {
            String returstreng = "feil";

            init(club, 20);
            if (brukerData.viewStatus == 2)
            {
                string name = Request.QueryString["first"];
                if (name.Length > 0)
                {
                    Models.Selskap nyselskap = new Models.Selskap();
                    nyselskap.navn = name;
                    databaseKontekst.selskaper.Add(nyselskap);
                    databaseKontekst.SaveChanges();

                    databaseKontekst.SaveChanges();

                    returstreng = "" + nyselskap.selskapID + ";" + nyselskap.navn;
                }
            }
            return returstreng;
        }

        public String RateGame(string club) {
            string returtekst = "";
            init(club, 20);
            if (brukerData.userObject != null) {
                returtekst = returtekst + "inne";
                Models.SpillRangering rangering = new Models.SpillRangering();
                string spillIDst = Request.QueryString["second"];
                string spillRangeringst = Request.QueryString["first"];
                int spillID, spillRangeringNum;

                if (Int32.TryParse(spillIDst, out spillID) && Int32.TryParse(spillRangeringst, out spillRangeringNum)) { 
                    IEnumerable<Models.SpillRangering> rangerte = (from m in databaseKontekst.rangeringer where m.spillID == spillID && m.spillerID == brukerData.userObject.spillerID select m).ToList();
                    if (rangerte.Count() > 0)
                    {
                        rangering = rangerte.FirstOrDefault();
                    }
                    else {
                        rangering.spillerID = brukerData.userObject.spillerID;
                        rangering.spillID = spillID;
                        databaseKontekst.rangeringer.Add(rangering);
                    }
                    rangering.rangering = spillRangeringNum;
                    databaseKontekst.SaveChanges();

                }
            }
            return returtekst;
        }
        
    }
}
