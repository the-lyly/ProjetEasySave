using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using static System.TimeZoneInfo;
//installer le package newtonsoft.json
namespace LeProjet
{
    public class Main
    {
        public Char Langue;
        public int NbTravaux;
        public List<travail> ListeTravaux;
        public char Action;
        public Log log=new  Log();
        public LogFile logFile;
        public int TotalSize;


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
        { StateController controller = new StateController();


            controller.initState();


                List<STATE> updatedStates = controller.AddState("Type", "Name", "SourceFile", "TargetFile",
                "State", 5, 1024, 3, "InProgress");


                controller.createState();


                List<STATE> openedStates = controller.openState();
         foreach (var travailObj in ListeTravaux) { 
         try
            {
                if (!Directory.Exists(source))
                {
                    if (Langue == 'F')
                    {
                        Console.WriteLine("Le dossier source n'existe pas : {0} \n", source);
                    }
                    else
                    {
                        Console.WriteLine("Source repository does not exist : {0} \n", source);
                    }

                    return;
                }

                if (!Directory.Exists(destination))
                {
                    if (Langue == 'F')
                    {
                        Console.WriteLine("Le dossier destination n'existe pas : {0} \n", destination);
                    }
                    else
                    {
                        Console.WriteLine("Destination repository does not exist : {0} \n", destination);
                    }

                    return;
                }

                CopyDirectory(source, destination);
                if (Langue == 'F')
                {
                    Console.WriteLine("Sauvegarde complète terminée.\n");
                }

                else
                {
                    Console.WriteLine("Complete Backup Completed.\n");
                }

                
                travail travailObj = new travail(); 
                TimeSpan transitionTime = new TimeSpan(); 
                int fileSize = 0; 
                log.Create_Log(travailObj, new FileInfo(destination), transitionTime, fileSize, "Complete");
            }
            catch (Exception ex)
            {
                if (Langue == 'F')
                {
                    Console.WriteLine("Erreur lors de la sauvegarde complète : {0}\n", ex.Message);
                }
                else
                {
                    Console.WriteLine("Error occurred during complete backup : {0}\n", ex.Message);
                }
            }

                    int nbfichier = travailObj.CountFiles();
                    Console.WriteLine($"le nombre de fichiers est : {nbfichier}");

                    long tailleFichiers = travailObj.getSize();
                    Console.WriteLine($"La taille totale des fichiers est : {tailleFichiers} octets");
             
        }



        private void CopyDirectory(string source, string destination)
        {

            string[] files = Directory.GetFiles(source, "*.*", SearchOption.AllDirectories);

            foreach (string file in files)
            {

                string relativePath = Path.GetRelativePath(source, file);
                string destinationFile = Path.Combine(destination, relativePath);


                Directory.CreateDirectory(Path.GetDirectoryName(destinationFile));

                if(Langue=='F')
                {
                    Console.WriteLine("Copie du fichier : {0}\n", file);
                }else 
                {
                    Console.WriteLine("Copying File : {0}\n", file);
                }
                
                File.Copy(file, destinationFile, true);
            }
        }


        //methode pour copie differenciel
        public void SaveD(string source, string destination)
        { StateController controller = new StateController();


            controller.initState();


            List<STATE> updatedStates = controller.AddState("Type", "Name", "SourceFile", "TargetFile",
            "State", 5, 1024, 3, "InProgress");


            controller.createState();


            List<STATE> openedStates = controller.openState();
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
                        if (Langue == 'F')
                        {
                            Console.WriteLine("Copie du fichier : {0} \n", file);
                        }
                        else
                        {
                            Console.WriteLine("Copying file : {0} \n", file);
                        }

                        File.Copy(file, destinationFile, true);
                        
                        travail travailObj = new travail(); 
                        TimeSpan transitionTime = new TimeSpan(); 
                        int fileSize = 0; 
                        log.Create_Log(travailObj, new FileInfo(destination), transitionTime, fileSize, "Differential");
                    }
                    else
                    {
                        if (Langue == 'F')
                        {
                            Console.WriteLine("Le fichier existe déjà et est à jour : {0}\n", file);
                        }
                        else
                        {
                            Console.WriteLine("The file exists and is up to date : {0}\n", file);
                        }
                    }
                }

                if (Langue == 'F')
                {
                    Console.WriteLine("Copie différentielle terminée.\n");
                }
                else
                {
                    Console.WriteLine("Differential Backup Complete.\n");
                }
            }
            catch (Exception ex)
            {
                if (Langue == 'F')
                {
                    Console.WriteLine("Erreur lors de la copie différentielle : {0}\n", ex.Message);
                }
                else
                {
                    Console.WriteLine("Error during Differential Save : {0}\n", ex.Message);
                }
            }

            if (Langue == 'F')
            {
                Console.WriteLine("Sauvegarde différentielle de {0} vers {1} reussi\n", source, destination);
            }
            else
            {
                Console.WriteLine("Differential Backup from {0} to {1} Successful\n", source, destination);
            }
        }

    }
public class STATE
{


    public string type { get; set; }
    public string name { get; set; }
    public string sourceFilePath { get; set; }
    public string targetFilePath { get; set; }
    public string State { get; set; } //booleen d'état
    public int numberOfFile { get; set; }
    public long totalFileSize { get; set; }
    public int numberOfFilesToDo { get; set; }
    public string progressionState { get; set; }


    public STATE()
    {

    }



    public STATE(string type, string name, string sourceFilePath, string targetFilePath, string State, int nbFile, long totalFileSize, int nbFilesToDo, string progressionState)
    {
        this.type = type;
        this.name = name;
        this.sourceFilePath = sourceFilePath;
        this.targetFilePath = targetFilePath;
        this.State = State;
        this.numberOfFile = nbFile;
        this.totalFileSize = totalFileSize;
        this.numberOfFilesToDo = nbFilesToDo;
        this.progressionState = progressionState;
    }

public int TFTC (String File)
        {
            int tftc = File.Length; 

            return (tftc);
        }
        public int TFS (int a, string File) // s'implemente dans la boucle de creation de travaux le a sera a l'exterieur de la boucle et s'incrementera a chaque intenerance
        {
            a = a + File.Length;
            return (a);
        }

}




public class StateController
{
    List<STATE> states;
    public StateController()
    {

        states = new List<STATE>();
    }
    //Return state path
    public string pathFinder()
    {

        string pathApp = Directory.GetCurrentDirectory() + @"C:\Users\Nabilla\Desktop\States";
        return pathApp;

    }
    // Check if state directory exists, if not create it 
    public void initState()
    {
        if (!Directory.Exists(pathFinder()))
        {
            Directory.CreateDirectory(pathFinder());
        }
    }
    public List<STATE> AddState(string type, string name, string sourceFilePath, string targetFilePath,
        string State, int nbFile, long totalFileSize, int nbFilesToDo, string progressionState)
    {
        STATE model = new STATE(type, name, sourceFilePath, targetFilePath, State, nbFile, totalFileSize, nbFilesToDo, progressionState);
        states.Add(model);
        return states;
    }

    public void createState()
    {
        lock (states)
        {
            string pathFileName = pathFinder() + @"\State.json";
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(states, options);
            File.WriteAllText(pathFileName, json);
        }
    }
    // Convert the json state file to a list of objects
    public List<STATE> openState()
    {
        lock (states)
        {
            string pathFileName = pathFinder() + @"\State.json";
            using (StreamReader r = new StreamReader(pathFileName))
            {
                string jsonString = r.ReadToEnd();
                var options = new JsonSerializerOptions { WriteIndented = true };
                states = JsonSerializer.Deserialize<List<STATE>>(jsonString, options);
                return states;
            }
        };

    }


}

    public class travail
    {
        Main main=new Main();
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
            if (main.Langue == 'F') 
            {
                Console.WriteLine($"ID: {ID}, Nom: {Nom}, Type: {Type}, EmplacementSource: {EmplacementSource}, Destination: {Destination}");
            }
            else
            {
                Console.WriteLine($"ID: {ID}, Name: {Nom}, Type: {Type}, Source: {EmplacementSource}, Destination: {Destination}");
            }
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
        

    public class LogFile
    {
        public static string FilePath = "D:\\Local Disk E_6420211819\\Downloads\\LOTR\\EasySaveLogs\\Log.json";

        public string Name { get; set; }
        public string SourceFilePath { get; set; }
        public string TargetFilePath { get; set; }
        public string Size { get; set; }
        public string Time { get; set; }
        public string TransitionTime { get; set; }
        public string LogType { get; set; }  
    }

    public class Log
    {
        public void Create_Log(travail work, FileInfo fileInfo, TimeSpan transitionTime, int size, string logType)
        {
            var time = new JProperty("Timestamp", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
            var jsonDataWork = File.ReadAllText(LogFile.FilePath);
            var logEntry = new LogFile
            {
                Name = work.Nom,
                SourceFilePath = work.EmplacementSource + "\\" + fileInfo.Name,
                TargetFilePath = work.Destination + "\\" + fileInfo.Name,
                TransitionTime = Convert.ToString(transitionTime),
                Size = Convert.ToString(size),
                Time = Convert.ToString(time),
                LogType = logType  
            };
            var logList = JsonConvert.DeserializeObject<List<LogFile>>(jsonDataWork) ?? new List<LogFile>();
            logList.Add(logEntry);

            string jsonString = JsonConvert.SerializeObject(logList, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(LogFile.FilePath, jsonString);
        }

        internal void Create_Log(object travailObject, FileInfo fileInfo, object transitionTime, object fileSize)
        {
            throw new NotImplementedException();
        }
    }
}
