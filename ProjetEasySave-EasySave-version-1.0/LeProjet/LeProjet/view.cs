using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LeProjet
{
    public class view
    {
        private char Langue;

        public char ReadInputLangage()
        {
            char langage;

            do
            {
                Console.WriteLine("Bienvenue sur EasySave => Choisissez la langue.\n Welcome to EasySave, Select your language." +
                    "\n Tapez: \n F -> pour Français. \n E -> for English.");

                langage = char.ToUpper(Console.ReadLine()[0]);
            } while (langage != 'F' && langage != 'E');

            if (langage == 'F')
            {
                Console.WriteLine("\n la langue par defaut est en français");
            }
            else
            {
                Console.WriteLine("\n The default language is English.");
            }
            Langue = langage;
            return langage;
        }

        public int ReadInputNbTravaux()
        {
            int Nb;

            do
            {
                if (Langue == 'F')
                {
                    Console.WriteLine("\n Veuillez sélectionner le nombre de travaux que vous souhaitez creer. \n REMARQUE: \n " +
                        "Le nombre maximum de travaux que vous pouvez creer a la fois ne peut dépasser 5 travaux!");
                }
                else
                {
                    Console.WriteLine("\n Please select the number of tasks you want to Create. \n NOTE: \n " +
                        "The maximum number of tasks you can create at a time cannot exceed 5 tasks!");
                }

            } while (!int.TryParse(Console.ReadLine(), out Nb) || Nb <= 0 || Nb > 5);

            return Nb;
        }

        //methode pour remplir les valeurs des attribus des travaux une fois que monsieu saisie le nombre de taff qu'il veux créer

        public (string Name, char Type, string Emplacement, string Deplacement) ReadInputTravaux()
        {
            string name;
            char type;
            string emplacement;
            string deplacement;

            if (Langue == 'F')
            {
                Console.WriteLine("Veuillez saisir le nom du travail : \n ");
            }
            else if (Langue == 'E')
            {
                Console.WriteLine("Please enter the task name: \n ");
            }

            name = Console.ReadLine();

            do
            {
                if (Langue == 'F')
                {
                    Console.WriteLine("\n Veuillez saisir le type du copie tapez C pour complète et D pour différentielle : \n ");
                }
                else if (Langue == 'E')

                {
                    Console.WriteLine("\n Please enter the backup type, C for complete and D for differential: \n ");
                }

                type = char.ToUpper(Console.ReadKey().KeyChar);
            } while (type != 'D' && type != 'C');

            do
            {
                if (Langue == 'F')
                {
                    Console.WriteLine(" \n Veuillez saisir l'emplacement source du travail. \n REMARQUE : cette commande se réexécutera" +
                        " tant que vous n'aurez pas saisi un emplacement déjà existant : \n ");
                }
                else if (Langue == 'E')
                {
                    Console.WriteLine(" \n Please enter the task source path. \n NOTE: This command will run again" +
                        " until you enter an existing location: \n ");
                }

                emplacement = Console.ReadLine();
            } while (!Directory.Exists(emplacement));

            do
            {
                if (Langue == 'F')
                {
                    Console.WriteLine("\n Veuillez saisir la destination ou vous souhetez copier votre travail : \n");
                }
                else if (Langue == 'E')
                {
                    Console.WriteLine("\n Please enter the task's destination :\n ");
                }

                deplacement = Console.ReadLine();
            } while (!IsValidPath(deplacement));

            return (name, type, emplacement, deplacement);
        }
        
        // la methode qui vient aprés que l'utilisateur ai rempli tout les travaux 
        public char ReadInputExecution()
        {
            char execution;
            if (Langue == 'F') 
            { 
                Console.WriteLine("Choisissez les travaux que vous souhaitez executer \n ");
            }
            else 
            {
                Console.WriteLine("Choose the tasks you would like to run \n ");
            }
            
            do
            {
                if (Langue == 'F')
                {
                    Console.WriteLine("\n Voulez-vous exécuter une copie séquentielle ou une copie entière ? Vous pouvez également choisir d'exécuter qu'un seul travail." +
                        " Tapez S pour séquentiel ou U pour travail unique \n");
                }
                else if (Langue == 'E')
                {
                    Console.WriteLine("\n Do you want to run a sequential copy or run only one job." +
                        " Type S for sequential or U for single job \n");
                }

                execution = char.ToUpper(Console.ReadKey().KeyChar);
                Console.WriteLine();
            } while (execution != 'S'  && execution != 'U');

            if (execution == 'U')
            {
                if (Langue == 'F')
                {
                    Console.WriteLine("\n Vous avez choisi de n'exécuter qu'un seul des travaux... \n");
                }
                else if (Langue == 'E')
                {
                    Console.WriteLine(" \n You have chosen to run only one of the tasks... \n");
                }
            }

            else
            {
                if (Langue == 'F')
                {
                    Console.WriteLine(" \n Copie séquentielle sélectionnée, analyse de vos besoins... \n ");
                }
                else if (Langue == 'E')
                {
                    Console.WriteLine(" \n Sequential copy selected, analyzing your needs... \n ");
                }
            }
            return execution;
        }
        // fin de la methode execution ou l'utilisateur saisi quesqu'il veux effectuer comme travaux. PS: fallais mettre ça je me rettrouvais plus dans le code UWU

        //methode pour le saisie de plage x-y pour executer les travaux allant de x a y uwu
        public (int start, int end) ReadInputRange()
        {
            int start;
            int end;

            if (Langue == 'E')
            {
                Console.WriteLine("Enter a range of numbers in this form 'x-y':");
            }
            else if (Langue == 'F')
            {
                Console.WriteLine("Veuillez entrer une plage de chiffres sous la forme 'x-y':");
            }

            string input = Console.ReadLine();

            if (TryParseRange(input, out start, out end) && start > 0 && start < 6 && end > 0 && end < 6 && start < end)
            {
                if (Langue == 'E')
                {
                    Console.WriteLine($"You entered the range: {start} - {end}");
                }
                else if (Langue == 'F')
                {
                    Console.WriteLine($"Vous avez saisi la plage : {start} - {end}");
                }
            }
            else
            {
                if (Langue == 'E')
                {
                    Console.WriteLine("Invalid Input.");
                }
                else if (Langue == 'F')
                {
                    Console.WriteLine("La saisie n'est pas valide.");
                }

                start = end = 0;
            }

            return (start, end);
        }

        //methode bch l'utilisateur saisie sous forme x,y,z les travaux li hab ydirhom 
        public List<int> ReadInputWorkSeparer()
        {
            List<int> workIDs = new List<int>();
            if (Langue == 'F')
            {
                Console.WriteLine("Entrez les ID des travaux que vous souhaitez exécuter sous la forme:'x,y,z':");
            }else
            {
                Console.WriteLine("Enter the ID for the tasks you would like to run in this form : 'x,y,z':");
            }
               
            string input = Console.ReadLine();

            string[] idStrings = input.Split(',');

            foreach (string idString in idStrings)
            {
                if (int.TryParse(idString, out int workID))
                {
                    if (workID > 0 && workID < 6 && !workIDs.Contains(workID))
                    {
                        workIDs.Add(workID);
                    }
                    else
                    {
                        if (Langue == 'F')
                        {
                             Console.WriteLine($"L'ID '{workID}' n'est pas valide et sera ignoré.");
                        }
                        else
                        {
                            Console.WriteLine($"The ID '{workID}' is invalid and will be ignored.");
                        }

                    }
                }
                else
                {
                    if (Langue == 'F')
                    {
                        Console.WriteLine($"'{idString}' n'est pas un nombre valide et sera ignoré.");
                    }
                    else
                    {
                        Console.WriteLine($"'{idString}' is not a valid number, will be ignored.");
                    }
                }
            }

            return workIDs;
        }

        //pour retourner l'ID du SEUL travail que l'utilisateur veux faire
        public int readinputUNIQUE()
        {
            int taff;
            do {
                if(Langue=='F')
                {
                    Console.WriteLine("veuillez saisir le travail que vous souhaitez executer. REMARQUE: cette action se repetera tant que vous n'aurez pas saisie un travail existant");

                }
                else 
                {  
                    Console.WriteLine("please enter the job you wish to execute. NOTE: this action will be repeated until you enter an existing job.");
                }
            } while (!int.TryParse(Console.ReadLine(), out taff) || taff <= 0 );
            return (taff);
        }


        static bool TryParseRange(string input, out int start, out int end)
        {
            start = 0;
            end = 0;

            // Diviser la saisie en parties séparées par le caractère '-'
            string[] parts = input.Split('-');

            if (parts.Length == 2 && int.TryParse(parts[0], out start) && int.TryParse(parts[1], out end))
            {
                return true;
            }
            return false;
        }

        //verifie que le path cohere 
        private bool IsValidPath(string path)
        {
            try
            {
                var fullPath = Path.GetFullPath(path);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

