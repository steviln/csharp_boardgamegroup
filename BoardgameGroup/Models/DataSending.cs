using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Web.Mvc;

namespace BoardgameGroup.Models
{
    public class DataSending
    {
    }

 

    public class SpillerLoginSamler
    {
        public Spiller spillere { get; set; }
        public SpillerLogin spillereLogin { get; set; }
        public List<SelectListCustomItem> statuser { get; set; }
    }
    public class SpillSeData
    {
        public Spill spillet { get; set; }
        public List<Models.Spillsesjon> sesjoner { get; set; }
        public List<Models.DisplayPlayerFront> rangertespillere { get; set; }
        public List<Models.DisplayFactionFront> rangertefraksjoner { get; set; }
        public List<Models.DisplaySelskapSpill> urangerteselskap { get; set; }
        public List<Models.spillerRangeringList> spilleronsker { get; set; }
        public SpillRangering rangeringen { get; set; }
        public string[] rangeringsverdier { get; set; }
        public int spillerID { get; set; }
    }
    public class spillerRangeringList
    {
        public string spillernavn { get; set; }
        public int rangering { get; set; }
        public int spillerID { get; set; }
    }
    public class SpillRegData
    {       
        public Spillsesjon sesjonen { get; set; }
        public List<SelectListCustomItem> spillerne { get; set; }
        public List<SelectListCustomItem> fraksjonene { get; set; }
        public List<SelectListCustomItem> scenariene { get; set; }
    }
    public class BoardgameReg
    {
        public Spill spillet { get; set; }
        public List<SelectListCustomItem> selskaper { get; set; }
        public SpillRangering rangeringen { get; set; }
        public int spillerID { get; set; }
        public string[] rangeringsverdier { get; set; }
    }
    public class SelectListCustomItem
    {
        public int ID { get; set; }
        public string name { get; set; }

        public SelectListCustomItem(int newID, string newName)
        {
            ID = newID;
            name = newName;
        }
    }
    public class HtmlListDisplay
    {
        public int primaryID { get; set; }
        public int secondaryID { get; set; }
        public string name { get; set; }
    }
    public class UserDisplayData
    {
        public Spiller spilleren { get; set; }
        public List<Models.Spillsesjon> sesjoner { get; set; }
        public int rating { get; set; }
        public int toplayerrating { get; set; }
    }
    public class DisplayPlayerFront
    {
        public string spillernavn { get; set; }
        public int rating { get; set; }
        public int spillerID { get; set; }
        public float average { get; set; }
        public float winrate { get; set; }
        public float lossrate { get; set; }
    }
    public class DisplayFactionFront 
    {
        public string fraksjonsavn { get; set; }
        public int rating { get; set; }
        public int fraksjonID { get; set; }
        public float average { get; set; }
    }
    public class DisplaySelskapSpill
    {
        public string selskapsnavn { get; set; }
        public int selskapID { get; set; }
    }
    public class FrontpageData
    {
        public List<Models.DisplayPlayerFront> rangertespillere { get; set; }
        public List<Models.Spillsesjon> sesjoner { get; set; }
    }
}