using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BoardgameGroup.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/

        public Models.BoardgameGroupDBContext databaseKontekst = new Models.BoardgameGroupDBContext();
        public Structures.PersistenceMain brukerData;

        // Ja, dette kan skape en krasj om bruker venter lenge med å trykke POST, og sesjonen har gått ut på tid. Dette får jeg se på senere. 
        public void init(string club, int requiredStatus)
        {
            if (club != "")
            {
                Session["clubName"] = club;
            }
            else
            {
                club = Session["clubName"].ToString();
            }
            brukerData = new Structures.PersistenceMain();
            IQueryable<Models.Klubb> klubbIntID = from E in databaseKontekst.klubber where E.klubbURL == club select E;
            brukerData.userClub = klubbIntID.FirstOrDefault();
            string sessionNummer = Session["sid"].ToString();

            var spillerliste = (from m in databaseKontekst.brukersesjoner where m.sessionID == sessionNummer select m).ToList();
            ViewBag.loggedID = 0;
            ViewBag.status = 0;

            if (spillerliste.Count() > 0)
            {
                brukerData.userObject = spillerliste.ElementAt<Models.brukerSession>(0).spillerLogin;
                ViewBag.loggedID = brukerData.userObject.spillerID;
                ViewBag.status = brukerData.userObject.status;
            }

            if (brukerData.userObject == null)
            {
                brukerData.viewStatus = 0;
            }
            else if (brukerData.userObject.status >= requiredStatus)
            {
                brukerData.viewStatus = 2;
            }
            else if (brukerData.userObject.status >= 10)
            {
                brukerData.viewStatus = 1;
            }

            ViewBag.clubURLname = brukerData.userClub.klubbURL;
        }

        public void LogMeOut()
        {
            string sessionID = Session["sid"].ToString();

            Models.brukerSession slettsesjon = (from m in databaseKontekst.brukersesjoner where m.sessionID == sessionID select m).FirstOrDefault();
            if (slettsesjon != null)
            {
                databaseKontekst.brukersesjoner.Remove(slettsesjon);
                databaseKontekst.SaveChanges();
            }

        }


    }
}
