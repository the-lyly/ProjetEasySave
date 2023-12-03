using System;
using System.Collections.Generic;

namespace LeProjet
{
    class ViewModel
    {
        STATE state = new STATE();
        view VU = new view();
        Main MAIN = new Main();

        public void Setting()
        {

            MAIN.SelectionLangue(VU.ReadInputLangage());

            if (MAIN.Langue == 'F')
            {
                Console.WriteLine("La langue a bien été mise en français \n");
            }
            else if (MAIN.Langue == 'E')
            {
                Console.WriteLine("The language has been set to English \n");
            }

            MAIN.SelectionNbTravaux(VU.ReadInputNbTravaux());
        }

        public void Travaux()
        {
            int a = 0;
            for (int i = 1; i <= MAIN.NbTravaux; i++)
            {
                travail Travail = new travail();

                if (MAIN.Langue == 'F')
                {
                    Console.WriteLine("Paramétrage du travail numéro : {0} \n", i);
                }
                else if (MAIN.Langue == 'E')
                {
                    Console.WriteLine("Setting up job number : {0} \n", i);
                }

                var task = VU.ReadInputTravaux();

                Travail.MiseTravail(task.Name, task.Type, task.Emplacement, task.Deplacement);

               a = state.TFS(a, task.Emplacement);

                Travail.IdentifiantTravail(i);

                MAIN.ListeTravaux.Add(Travail);

                Travail.AfficherDetails();

                if (MAIN.Langue == 'F')
                {
                    Console.WriteLine("\n suivant \n");
                }
                else if (MAIN.Langue == 'E')
                {
                    Console.WriteLine("\n next \n");
                }
            }

            if (MAIN.Langue == 'F')
            {
                Console.WriteLine(" \n Vos {0} ont bien été déclarés avec succès \n", MAIN.NbTravaux);
                Console.WriteLine("\n La taille de vos travaux est {0} :", a);
            }
            else
            {
                Console.WriteLine("\n Your {0} tasks have been successfully declared.\n", MAIN.NbTravaux);
                Console.WriteLine("\n the total size of your tasks is {0} :", a);

            }
            MAIN.TotalSize = a;
            foreach (var travailObj in MAIN.ListeTravaux)
            {
                travailObj.AfficherDetails();
            }

        }
        public void Execution()
        {
            MAIN.Action = VU.ReadInputExecution();

            if (MAIN.Action == 'U')
            {
                int T;

                do
                {
                    T = VU.readinputUNIQUE();
                } while (!IsValidWorkID(T));

                if (MAIN.Langue == 'F')
                {
                    Console.WriteLine("Recherche du travail N° {0}", T);
                }else
                {
                    Console.WriteLine("Searching Backup N° {0}", T);
                }
                travail travailSelectionne = MAIN.GetTravailByID(T);

                if (travailSelectionne != null)
                {
                    if (MAIN.Langue == 'F')
                    {
                        Console.WriteLine("Exécution du travail N° {0}", T);
                        Console.WriteLine("Nom: {0}", travailSelectionne.Nom);
                        Console.WriteLine("Type: {0}", travailSelectionne.Type);
                        Console.WriteLine("Emplacement Source: {0}", travailSelectionne.EmplacementSource);
                        Console.WriteLine("Destination: {0}", travailSelectionne.Destination);
                    }
                    else
                    {
                        Console.WriteLine("Running Backup N° {0}", T);
                        Console.WriteLine("Name: {0}", travailSelectionne.Nom);
                        Console.WriteLine("Type: {0}", travailSelectionne.Type);
                        Console.WriteLine("Source Path: {0}", travailSelectionne.EmplacementSource);
                        Console.WriteLine("Destination: {0}", travailSelectionne.Destination);
                    }
                    if (travailSelectionne.Type == 'D')
                    {
                        MAIN.SaveD(travailSelectionne.EmplacementSource, travailSelectionne.Destination);
                    }
                    else
                    {
                        MAIN.SaveC(travailSelectionne.EmplacementSource, travailSelectionne.Destination);
                    }
                }
                else
                {
                    if (MAIN.Langue == 'F')
                    {
                        Console.WriteLine("Travail avec l'ID {0} N'existe Pas.", T);
                    }
                    else 
                    {
                        Console.WriteLine("Backup with Identifier {0} Not Found.", T);
                    }
                }
            }
            else if (MAIN.Action == 'S')
            {
                int n;

                do
                {
                    if (MAIN.Langue == 'F')
                    {
                        Console.WriteLine("\n Souhaitez-vous entrer une plage de valeurs ou saisir des valeurs manuellement? \n 1. => Saisir une plage \n 2. => Saisir des travaux manuellement \n");
                    }
                    else
                    {
                        Console.WriteLine("\n Would You like to enter a range of values or Enter values Manually? \n 1. => Please Enter value range \n 2. => Please enter backup Number \n");
                    }
                } while (!int.TryParse(Console.ReadLine(), out n) || (n != 1 && n != 2));

                if (n == 1)
                {
                    (int start, int end) plage;

                    do
                    {
                        plage = VU.ReadInputRange();
                    } while (!IsValidWorkID(plage.start) || !IsValidWorkID(plage.end) || plage.end < plage.start);

                    for (int i = plage.start; i <= plage.end; i++)
                    {
                        travail travailSelectionne = MAIN.GetTravailByID(i);

                        if (travailSelectionne != null)
                        {
                            if (MAIN.Langue == 'F')
                            {
                                Console.WriteLine("Exécution du travail N° {0}", i);
                                Console.WriteLine("Nom: {0}", travailSelectionne.Nom);
                                Console.WriteLine("Type: {0}", travailSelectionne.Type);
                                Console.WriteLine("Emplacement Source: {0}", travailSelectionne.EmplacementSource);
                                Console.WriteLine("Destination: {0}", travailSelectionne.Destination);
                            }
                            else
                            {
                                Console.WriteLine("Running Backup N° {0}", i);
                                Console.WriteLine("Name: {0}", travailSelectionne.Nom);
                                Console.WriteLine("Type: {0}", travailSelectionne.Type);
                                Console.WriteLine("Source Path: {0}", travailSelectionne.EmplacementSource);
                                Console.WriteLine("Destination: {0}", travailSelectionne.Destination);
                            }
                            if (travailSelectionne.Type == 'D')
                            {
                                MAIN.SaveD(travailSelectionne.EmplacementSource, travailSelectionne.Destination);
                            }
                            else
                            {
                                MAIN.SaveC(travailSelectionne.EmplacementSource, travailSelectionne.Destination);
                            }
                        }
                        else
                        {
                            if (MAIN.Langue == 'F')
                            {
                                Console.WriteLine("Travail avec l'ID {0} N'existe Pas.", i);
                            }
                            else
                            {
                                Console.WriteLine("Backup with Identifier {0} Not Found.", i);
                            }
                        }
                    }
                }

                else if (n == 2)
                {
                    List<int> lesID = VU.ReadInputWorkSeparer();

                    foreach (var ID in lesID)
                    {
                        travail travailSelectionne = MAIN.GetTravailByID(ID);

                        if (travailSelectionne != null)
                        {
                            if (MAIN.Langue == 'F')
                            {
                                Console.WriteLine("Exécution du travail N° {0}", lesID);
                                Console.WriteLine("Nom: {0}", travailSelectionne.Nom);
                                Console.WriteLine("Type: {0}", travailSelectionne.Type);
                                Console.WriteLine("Emplacement Source: {0}", travailSelectionne.EmplacementSource);
                                Console.WriteLine("Destination: {0}", travailSelectionne.Destination);
                            }
                            else
                            {
                                Console.WriteLine("Running Backup N° {0}", lesID);
                                Console.WriteLine("Name: {0}", travailSelectionne.Nom);
                                Console.WriteLine("Type: {0}", travailSelectionne.Type);
                                Console.WriteLine("Source Path: {0}", travailSelectionne.EmplacementSource);
                                Console.WriteLine("Destination: {0}", travailSelectionne.Destination);
                            }

                            if (travailSelectionne.Type == 'D')
                            {
                                MAIN.SaveD(travailSelectionne.EmplacementSource, travailSelectionne.Destination);
                            }
                            else
                            {
                                MAIN.SaveC(travailSelectionne.EmplacementSource, travailSelectionne.Destination);
                            }
                        }
                        else
                        {
                            if (MAIN.Langue == 'F')
                            {
                                Console.WriteLine("Travail avec l'ID {0} N'existe Pas.", lesID);
                            }
                            else
                            {
                                Console.WriteLine("Backup with Identifier {0} Not Found.", lesID);
                            }
                        }
                    }
                }
            }
        }

        private bool IsValidWorkID(int workID)
        {
            return MAIN.ListeTravaux.Exists(travailObj => travailObj.ID == workID);
        }

    }
}
