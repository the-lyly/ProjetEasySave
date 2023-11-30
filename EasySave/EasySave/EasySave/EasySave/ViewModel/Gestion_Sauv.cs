using EasySave.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave.ViewModel
{
    internal class Gestion_Sauv
    {
        Travail work = new Travail();
        public void Method_sauv()
        {
            if (work.TypeTravail == "Complet")
            {
                work.Travail_complet();
            }
            else
            {
                work.Travail_differentiel();
            }
        }
    }
}
