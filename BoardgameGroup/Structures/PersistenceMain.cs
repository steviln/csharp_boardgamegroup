using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace BoardgameGroup.Structures
{
    public class PersistenceMain
    {

        public Models.Klubb userClub;
        public Models.SpillerLogin userObject;
        public int userID;
        public int viewStatus;

        public PersistenceMain() {
            userID = 0;
            userClub = null;
            userObject = null;
            viewStatus = 0;
        }


    }

    public class DropdownVerdier 
    {
        public string navn { get; set; }
        public int val { get; set; }
    }

    public class Poengsorter : IComparer<Models.Spilldeltakelse> {
        public int Compare(Models.Spilldeltakelse en, Models.Spilldeltakelse to) {
            return en.posisjon.CompareTo(to.posisjon);
        }  
    }

    public class SpillerFrontSorter : IComparer<Models.DisplayPlayerFront> {
        public int Compare(Models.DisplayPlayerFront en, Models.DisplayPlayerFront to) {
            return to.rating.CompareTo(en.rating);
        }
    }

    public class FactionFrontSorter : IComparer<Models.DisplayFactionFront>
    {
        public int Compare(Models.DisplayFactionFront en, Models.DisplayFactionFront to)
        {
            return to.rating.CompareTo(en.rating);
        }
    }
}