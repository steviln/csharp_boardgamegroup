using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace BoardgameGroup.Models
{
    class DataInit : DropCreateDatabaseAlways<BoardgameGroupDBContext>
    {

        protected override void Seed(BoardgameGroupDBContext context)
        {
            context.klubber.Add(new Klubb() { klubbID = 1, klubbNavn = "Drammen spillklubb", klubbURL = "DSF" });
            context.spillere.Add(new Spiller() { spillerID = 1, fornavn = "App", etternavn = "Eier" });
            context.klubbmedlemskap.Add(new KlubbMedlemskap() { medlemskapID = 1, klubbID = 1, spillerID = 1 });
            context.spillereLogin.Add(new SpillerLogin() { spillerloginID = 1, spillerID = 1, status = 200, brukernavn = "test", passord = "test" });
            context.spillere.Add(new Spiller() { spillerID = 2, fornavn = "Tester 1", etternavn = "Testesen" });
            context.spillere.Add(new Spiller() { spillerID = 3, fornavn = "Tester 2", etternavn = "Testesen" });
            context.spillere.Add(new Spiller() { spillerID = 4, fornavn = "Tester 3", etternavn = "Testesen" });
            context.spillere.Add(new Spiller() { spillerID = 5, fornavn = "Tester 4", etternavn = "Testesen" });
            context.spillere.Add(new Spiller() { spillerID = 6, fornavn = "Tester 5", etternavn = "Testesen" });
            context.klubbmedlemskap.Add(new KlubbMedlemskap() { medlemskapID = 2, klubbID = 1, spillerID = 2 });
            context.klubbmedlemskap.Add(new KlubbMedlemskap() { medlemskapID = 3, klubbID = 1, spillerID = 3 });
            context.klubbmedlemskap.Add(new KlubbMedlemskap() { medlemskapID = 4, klubbID = 1, spillerID = 4 });
            context.klubbmedlemskap.Add(new KlubbMedlemskap() { medlemskapID = 5, klubbID = 1, spillerID = 5 });
            context.klubbmedlemskap.Add(new KlubbMedlemskap() { medlemskapID = 6, klubbID = 1, spillerID = 6 });
            context.spillene.Add(new Spill() { spillID = 1, navn = "Testspill", spillerantall = 6, selskapID = 1 });
            context.selskaper.Add(new Selskap() { selskapID = 1, navn = "Testselskap" });
            context.spillfraksjoner.Add(new SpillFraksjon() { fraksjonID = 1, navn = "Rød", spillID = 1 });
            context.spillfraksjoner.Add(new SpillFraksjon() { fraksjonID = 2, navn = "Blå", spillID = 1 });
            context.spillfraksjoner.Add(new SpillFraksjon() { fraksjonID = 3, navn = "Grønn", spillID = 1 });
            context.spillfraksjoner.Add(new SpillFraksjon() { fraksjonID = 4, navn = "Gul", spillID = 1 });
            context.spillfraksjoner.Add(new SpillFraksjon() { fraksjonID = 5, navn = "Oransje", spillID = 1 });
            context.spillfraksjoner.Add(new SpillFraksjon() { fraksjonID = 6, navn = "Brun", spillID = 1 });
            context.spillsesjoner.Add(new Spillsesjon() { spillsesjonID = 1, klubbID = 1, spillerID = 1, spillID = 1, dato = DateTime.Today  });
            context.deltakelser.Add(new Spilldeltakelse() { spilldeltakelseID = 1, deltakerID = 2, fraksjonID = 1, sesjonID = 1, poengsum = 34, posisjon = 3 });
            context.deltakelser.Add(new Spilldeltakelse() { spilldeltakelseID = 2, deltakerID = 3, fraksjonID = 2, sesjonID = 1, poengsum = 39, posisjon = 1 });
            context.deltakelser.Add(new Spilldeltakelse() { spilldeltakelseID = 3, deltakerID = 4, fraksjonID = 3, sesjonID = 1, poengsum = 12, posisjon = 5 });
            context.deltakelser.Add(new Spilldeltakelse() { spilldeltakelseID = 4, deltakerID = 5, fraksjonID = 4, sesjonID = 1, poengsum = 35, posisjon = 2 });
            context.deltakelser.Add(new Spilldeltakelse() { spilldeltakelseID = 5, deltakerID = 6, fraksjonID = 5, sesjonID = 1, poengsum = 19, posisjon = 4 });
            base.Seed(context);
        }
    }
}