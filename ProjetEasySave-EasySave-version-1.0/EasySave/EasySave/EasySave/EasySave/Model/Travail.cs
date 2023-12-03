using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave.Model
{
    internal class Travail
    {
        public string Name { get; set; }
        public string SourcePath { get; set; }
        public string DestinationPath{ get; set; }
        public enum TypeTravail { Complet, Differentiel } //jsp si ca c juste ou pas comment on fait sinon ?
        public enum StateTravail { Actif, Inactif } //ca aussi pour le coup

        public void CreateTravail(string nom,string Src,string Dest, TypeTravail type, StateTravail state)
        {
            Name = nom;
            SourcePath = Src;
            DestinationPath = Dest;
            TypeTravail = type;
            StateTravail= state;
        }
        public void Travail_complet()
        {

        }
        public void Travail_differentiel()
        {

        }
        public void getSize()
        {

        }
        public void Count_files()
        {

        }
        public void Count_time()
        {

        }
        public void Travail()
        {

        }


    }
}
