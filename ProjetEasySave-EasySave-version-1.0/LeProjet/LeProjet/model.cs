using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;

namespace LeProjet
{
    public class Main
    {
        public Char Langue;
        public int NbTravaux;
        public List<travail> ListeTravaux;
        public char Action;

        public void SelectionLangue(char L)
        {
            Langue = L;
        }
        public void SelectionNbTravaux(int Nb)
        {
            NbTravaux = Nb;
        }
        public Main()
        {
            ListeTravaux = new List<travail>();
        }
        public travail GetTravailByID(int id)
        {
            return ListeTravaux.Find(travailObj => travailObj.ID == id);
        }
        public void SaveC(string source, string destination)
        {
            foreach (var travailObj in ListeTravaux)
            {
                try
                {
                    if (!Directory.Exists(source))
                    {
                        Console.WriteLine("Le dossier source n'existe pas : {0} \n", source);
                        return;
                    }

                    if (!Directory.Exists(destination))
                    {
                        Console.WriteLine("Le dossier destination n'existe pas : {0} \n", destination);
                        return;
                    }

                    CopyDirectory(source, destination);

                    Console.WriteLine("Sauvegarde complète terminée.\n");

                    int nbfichier = travailObj.CountFiles();
                    Console.WriteLine($"le nombre de fichiers est : {nbfichier}");

                    long tailleFichiers = travailObj.getSize();
                    Console.WriteLine($"La taille totale des fichiers est : {tailleFichiers} octets");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur lors de la sauvegarde complète : {0}\n", ex.Message);
                }


            }
         }



        private void CopyDirectory(string source, string destination)
        {
            
            
                string[] files = Directory.GetFiles(source, "*.*", SearchOption.AllDirectories);

                foreach (string file in files)
                {

                    string relativePath = Path.GetRelativePath(source, file);
                    string destinationFile = Path.Combine(destination, relativePath);


                    Directory.CreateDirectory(Path.GetDirectoryName(destinationFile));


                    Console.WriteLine("Copie du fichier : {0}", file);
                    File.Copy(file, destinationFile, true);


                }

                
            

        }


        //methode pour copie differentielle
        public void SaveD(string source, string destination)
        {
            try
            {
                if (!Directory.Exists(source))
                {
                    Console.WriteLine("Le dossier source n'existe pas : {0} \n", source);
                    return;
                }

                if (!Directory.Exists(destination))
                {
                    Console.WriteLine("Le dossier destination n'existe pas : {0} \n", destination);
                    return;
                }

                string[] files = Directory.GetFiles(source, "*.*", SearchOption.AllDirectories);

                foreach (string file in files)
                {

                    string relativePath = Path.GetRelativePath(source, file);
                    string destinationFile = Path.Combine(destination, relativePath);

                    if (!File.Exists(destinationFile) || File.GetLastWriteTimeUtc(file) > File.GetLastWriteTimeUtc(destinationFile))
                    {
                        Console.WriteLine("Copie du fichier : {0} \n", file);
                        File.Copy(file, destinationFile, true);
                    }

                    else
                    {
                        Console.WriteLine("Le fichier existe déjà et est à jour : {0}\n", file);
                    }
                }

                Console.WriteLine("Copie différentielle terminée.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la copie différentielle : {0}\n", ex.Message);
            }
            Console.WriteLine("Sauvegarde différentielle de {0} vers {1} reussi\n", source, destination);
        }


    }

    public class travail
    {
        public string Nom { get; set; }
        public char Type { get; set; }
        public string EmplacementSource { get; set; }
        public string Destination { get; set; }
        public int ID { get; set; }
        public void MiseTravail(string N, char T, string E, string D)
        {
            Nom = N;
            Type = T;
            EmplacementSource = E;
            Destination = D;

        }

        public void IdentifiantTravail(int a)
        {
            ID = a;
        }

        public void AfficherDetails()
        {
            Console.WriteLine($"ID: {ID}, Nom: {Nom}, Type: {Type}, EmplacementSource: {EmplacementSource}, Destination: {Destination}");
        }

         public int CountFiles ()
        {
                    string[] fichiers = Directory.GetFiles(EmplacementSource);
                    Console.WriteLine($"le nombre de fichiers dans le dossier {EmplacementSource}: est {fichiers.Length}");
                    return fichiers.Length;


        }
        public long getSize()
        {
            long totalSize = 0;

            try
            {
                string[] files = Directory.GetFiles(EmplacementSource, "*.*", SearchOption.AllDirectories);

                foreach (string file in files)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    totalSize += fileInfo.Length;
                }

                Console.WriteLine($"La taille totale des fichiers dans le dossier {EmplacementSource}: est {totalSize} octets");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du calcul de la taille des fichiers : {ex.Message}");
            }

            return totalSize;

        }

    }


    // modification lynda

    public class State
    {
        public DateTime datetime { get; set; }
        public string pathdufichierstate { get; set; }
        public travail state { get; set; }

        public int filesLeft()
       {
            // Implémentez la logique pour retourner le nombre de fichiers restants
            // en fonction de l'état du travail de sauvegarde.
            // Vous pouvez utiliser les propriétés de la classe travail pour obtenir les informations nécessaires.
        }

        public long SizeLeft()
       {
            // Implémentez la logique pour retourner la taille des fichiers restants
            // en fonction de l'état du travail de sauvegarde.
        }



        public void CreateStateFile()
        {
            string stateFilePath = "chemin_vers_le_fichier_etat"; // Remplacez par le chemin réel de votre fichier d'état

            try
            {
                // Créer un objet pour contenir les informations d'état
                var stateObject = new
                {
                    DateHeure = datetime.ToString(),
                    NomTravail = state.Nom,
                    EtatTravail = state.Type,
                 //   FichiersRestants = filesLeft(),
                  //  TailleRestante = SizeLeft()
                };

                // Convertir l'objet en une chaîne JSON
                string jsonState = JsonSerializer.Serialize(stateObject);

                // Vérifier si le fichier d'état existe
                if (File.Exists(stateFilePath))
                {
                    // Écraser le contenu existant avec les nouvelles informations JSON
                    File.WriteAllText(stateFilePath, jsonState);

                    Console.WriteLine("Fichier d'état mis à jour avec succès.");
                }
                else
                {
                    // Si le fichier n'existe pas, créer un nouveau fichier d'état
                    using (StreamWriter sw = File.CreateText(stateFilePath))
                    {
                        // Écrire les informations JSON dans le fichier
                        sw.Write(jsonState);
                    }

                    Console.WriteLine("Fichier d'état créé avec succès.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la création/mise à jour du fichier d'état : {ex.Message}");
            }
        }  
    }
}



