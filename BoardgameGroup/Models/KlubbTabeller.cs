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
    



    public class BoardgameGroupDBContext : DbContext
    {
        public DbSet<Klubb> klubber { get; set; }
        public DbSet<Spiller> spillere { get; set; }
        public DbSet<Spillsesjon> spillsesjoner { get; set; }
        public DbSet<Spill> spillene { get; set; }
        public DbSet<Spilldeltakelse> deltakelser { get; set; }
        public DbSet<SpillerLogin> spillereLogin { get; set; }
        public DbSet<brukerSession> brukersesjoner { get; set; }
        public DbSet<KlubbMedlemskap> klubbmedlemskap { get; set; }
        public DbSet<SpillFraksjon> spillfraksjoner { get; set; }
        public DbSet<SpillScenario> spillscenarier { get; set; }
        public DbSet<SpillRapport> spillrapporter { get; set; }
        public DbSet<Selskap> selskaper { get; set; }
        public DbSet<SpillRangering> rangeringer { get; set; }
        public DbSet<LoginFail> loginfail { get; set; }

        public BoardgameGroupDBContext() {
            Database.SetInitializer(new DataInit());
        }

        
    }

    // Her kommer de databaserelevante tabellene

    public class Klubb 
    {
        [Key]
        public int klubbID { get; set; }
        public string klubbNavn { get; set; }
        public string klubbURL { get; set; }
        public virtual ICollection<KlubbMedlemskap> spillere { get; set; } 

        public Klubb(){
            spillere = new HashSet<KlubbMedlemskap>();
        }
    }

    public class KlubbMedlemskap 
    {
        [Key]
        public int medlemskapID { get; set; }
        [ForeignKey("spillerID")]
        public virtual Spiller spiller { get; set; }
        [ForeignKey("klubbID")]
        public virtual Klubb klubb { get; set; }
        public int spillerID { get; set; }
        public int klubbID { get; set; }
    }

    public class Spiller {
        [Key]
        public int spillerID { get; set; }
        [Required]
        [StringLength(160, MinimumLength=1)]
        public string fornavn { get; set; }
        [Required]
        [StringLength(160, MinimumLength = 1)]
        public string etternavn { get; set; }
        public string epost { get; set; }
        public string phone { get; set; }
        public string mobile { get; set; }
        public string facebookID { get; set; }
        public virtual ICollection<KlubbMedlemskap> Klubber { get; set; }  
 
       public Spiller(){
            Klubber = new HashSet<KlubbMedlemskap>();
        }
    }

    public class SpillerLogin {
        [Key]
        public int spillerloginID { get; set; }
        [ForeignKey("spillerID")]
        public virtual Spiller Spiller { get; set; }
        [Required]
        [StringLength(160, MinimumLength = 1)]
        public string brukernavn { get; set; }
        [Required]
        [StringLength(160, MinimumLength = 1)]
        public string passord { get; set; }
        public int status { get; set; }
        public int spillerID { get; set; }
    }

    public class brukerSession {
        [Key]
        public int brukerSessionID { get; set; }
        public string sessionID { get; set; }
        public int spillerloginID { get; set; }
        [ForeignKey("spillerloginID")]
        public virtual SpillerLogin spillerLogin { get; set; }
    }

    public class Spillsesjon {
        [Key]
        public int spillsesjonID { get; set; }
        [ForeignKey("spillID")]
        public virtual Spill Spill { get; set; }
        [ForeignKey("klubbID")]
        public virtual Klubb Klubb { get; set; }
        public int spillID { get; set; }
        public int klubbID { get; set; }
        public virtual List<Spilldeltakelse> Spillere { get; set; }
        public DateTime dato { get; set; }
        [ForeignKey("scenarioID")]
        public virtual SpillScenario scenarion { get; set; }
        public int? scenarioID { get; set; }
        [ForeignKey("spillerID")]
        public virtual Spiller registrar { get; set; }
        public int? spillerID { get; set; }
        [ForeignKey("rapportID")]
        public virtual SpillRapport spillrapport { get; set; }
        public int? rapportID { get; set; }
    }

    public class SpillRapport {
        [Key]
        public int? spillrapportID { get; set; }
        [ForeignKey("spillerID")]
        public virtual Spiller Spiller { get; set; }
        public int? spillerID { get; set; }
        [StringLength(255)]
        public string overskrift { get; set; }
        [StringLength(40000)]
        public string rapport { get; set; }
    }

    public class Spilldeltakelse {
        [Key]
        public int spilldeltakelseID { get; set; }
        public int posisjon { get; set; }
        [ForeignKey("deltakerID")]
        public virtual Spiller Spiller { get; set; }
        public int deltakerID { get; set; }
        public int poengsum { get; set; }
        [ForeignKey("fraksjonID")]
        public virtual SpillFraksjon fraksjonen { get; set; }
        public int? fraksjonID { get; set; }
        [ForeignKey("sesjonID")]
        public virtual Spillsesjon sesjon { get; set; }
        public int sesjonID { get; set; }

    }

    public class SpillScenario {
        [Key]
        public int scenarioID { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string navn { get; set; }
        public string kode { get; set; }
        [ForeignKey("spillID")]
        public virtual Spill spill { get; set; }
        public int spillID { get; set; }
    }

    public class Spill {
        [Key]
        public int spillID { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string navn { get; set; }
        public string boardgamegeekID { get; set; }
        public int spillerantall { get; set; }
        public virtual ICollection<SpillFraksjon> fraksjoner { get; set; }
        public virtual ICollection<SpillScenario> scenarier { get; set; }
        [ForeignKey("selskapID")]
        public virtual Selskap selskap { get; set; }
        public int? selskapID { get; set; }
    }

    public class Selskap {
        [Key]
        public int selskapID { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string navn { get; set; }
        public virtual ICollection<Spill> spillene { get; set; }
    }

    public class SpillFraksjon {
        [Key]
        public int fraksjonID { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string navn { get; set; }
        public string farge { get; set; }
        [ForeignKey("spillID")]
        public virtual Spill spill { get; set; }
        public int spillID { get; set; }
    }

    public class SpillRangering
    {
        [Key]
        public int spillrangeringID { get; set; }
        [ForeignKey("spillerID")]
        public virtual Spiller spiller { get; set; }
        public int spillerID { get; set; }
        [ForeignKey("spillID")]
        public virtual Spill spill { get; set; }
        public int spillID { get; set; }
        public int rangering { get; set; }
    }

    public class LoginFail
    {
        [Key]
        public int loginfailID { get; set; }
        public string faceNavn { get; set; }
        public string faceID { get; set; }
    
    
    }

}