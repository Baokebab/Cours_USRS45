using System;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.InteropServices;

namespace FightingGame
{


    class Program
    {
        #region Config Windows Max Size
        //Pris d'internet, permet d'ouvrir la console directement maximisé using System.RunTime.InteropServices
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        private static IntPtr ThisConsole = GetConsoleWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int MAXIMIZE = 3;
        #endregion
        static void Main(string[] args)
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight); //Configure la console pour être à la taille maximale de votre écran
            ShowWindow(ThisConsole, MAXIMIZE); //Ouvre la console maximisé (la fenêtre peut être de taille maximale sans pour autant fit l'écran. C'est comme appuyé sur "Agrandir"
            Console.SetBufferSize(Math.Max(Console.LargestWindowWidth, 237), 9000); //Il faut au minimum une largeur de 230 et quelques pour voir les caractères de notre jeu d'où le buffer à 237 minimum

            while (Console.KeyAvailable) //Enlève la mémoire tampon des inputs dans le cas ou le joueur spam (cela évite de repartir trop vite par erreur dans une sélection)
                Console.ReadKey(true);
            #region Titre et choix du mode
            DisplayTitle(); //Affichage du titre avec les couleurs
            Console.WriteLine("Bienvenue dans l'arène!                                                                                                     \n");
            Console.WriteLine("Menu  : ");
            Console.WriteLine("1 - Joueur VS Ordinateur");
            Console.WriteLine("2 - Ordinateur VS Ordinateur (Test d'équilibrage)");
            Console.WriteLine("3 - Quitter le jeu");
            string choice = Console.ReadLine();
            int numberChoice, numberOfTries = 0;
            //Tant que l'input n'est pas un chiffre égal à 1, 2 ou 3 on redemande à l'utilisateur de taper.
            while (!int.TryParse(choice, out numberChoice) || !(numberChoice == 1 || numberChoice == 2 || numberChoice == 3))
            {
                Console.WriteLine();
                Console.WriteLine("'{0}' n'est pas un choix valide. Merci de taper 1, 2 ou 3", choice);
                choice = Console.ReadLine();
                Console.WriteLine();
                numberOfTries++; //On va garder ici le nombre de fois ou l'utilisateur se trompe (pour compter le nombre de ligne dans la Console)
            }
            #endregion

            //Cas du JOUEUR VS IA
            if (numberChoice == 1)
            {
                ClearLastLines(7 + numberOfTries * 4 + 1); //On clean uniquement les lignes qui nous intéressent avec notre fonction ClearLastLines (plus beau que de faire du Console.Clear)
                //Demande le niveau de l'ordinateur pour la partie
                Console.WriteLine("Difficulté de l'ordinateur :");
                Console.WriteLine("1 - Random Mode : Les actions de l'ordinateur sont choisies aléatoirement.");
                Console.WriteLine("2 - Dumb Mode : L'ordinateur ne fait que vous attaquer.");
                Console.WriteLine("3 - Normal Mode : Les actions sont choisies en fonction des points de vie.");
                Console.WriteLine("4 - Smart Mode : L'ordinateur fait ses actions en fonction de vos points de vie et de votre rôle.");
                string IAmode = Console.ReadLine();
                int IAnumberChoice;
                //Tant que l'input n'est pas un chiffre valide on redemande à l'utilisateur de taper.
                while (!int.TryParse(IAmode, out IAnumberChoice) || !(IAnumberChoice == 1 || IAnumberChoice == 2 || IAnumberChoice == 3 || IAnumberChoice == 4))
                {
                    Console.WriteLine();
                    Console.WriteLine("'{0}' n'est pas un choix valide. Merci de taper 1, 2, 3 ou 4", IAmode);
                    IAmode = Console.ReadLine();
                    Console.WriteLine();
                }
                Console.SetCursorPosition(0, 0);
                PlayGame(IAnumberChoice);  //Fonction d'une partie avec en argument le niveau de difficulté
                Main(args); //A la fin de la partie on revient au menu
            }

            //Cas IA VS IA (SIMULATION)
            else if (numberChoice == 2)
            {
                Dictionary<int, string> SimuMode = new Dictionary<int, string>()  //Dictionnaire uniquement pour les commentaires de fin de tableau
                {
                    {1,"RANDOM MODE" },
                    {2,"DUMB MODE" },
                    {3,"NORMAL MODE" },
                    {4,"SMART MODE" }
                };
                ClearLastLines(7 + numberOfTries * 4 + 1);
                Console.WriteLine("Veuillez choisir le type de simulation  : ");
                Console.WriteLine("1 - RANDOM MODE :  Actions choisies aléatoirement");
                Console.WriteLine("2 - DUMB MODE : Les ordinateurs attaquent à tous les tours");
                Console.WriteLine("3 - NORMAL MODE : L'ordinateur s'adapte en fonction de la situation");
                Console.WriteLine("4 - Revenir au menu");
                string choiceSimu = Console.ReadLine();
                int numberChoiceSimu;
                //Tant que l'input n'est pas un chiffre valide on redemande à l'utilisateur de taper.
                while (!int.TryParse(choiceSimu, out numberChoiceSimu) || !(numberChoiceSimu == 1 || numberChoiceSimu == 2 || numberChoiceSimu == 3 || numberChoiceSimu == 4))
                {
                    Console.WriteLine();
                    Console.WriteLine("'{0}' n'est pas un choix valide. Merci de taper 1, 2, 3 ou 4", choiceSimu);
                    choiceSimu = Console.ReadLine();
                    Console.WriteLine();
                }

                if (numberChoiceSimu == 4)
                {
                    Console.Clear();
                    Main(args);
                }
                Console.Clear();
                int[][] ResultatSimu = new int[4][]; //4 car c'est le nombre de rôle que nous avons (pourrait être mis dynamiquement avec un role.Count
                for (int i = 1; i < 5; i++)
                {
                    int[] temp = new int[4];
                    ResultatSimu[i - 1] = temp;
                } //initialise notre array int[4][4]

                #region fixing pour les très petits écrans... (on écrit les 1ère lignes du tableau de simulation et on actualise qu'a partir de la 12e)
                //Code repris de notre Display Simu car l'affichage fait un autoscroll un peu épileptique sur les tout petits écrans
                Console.SetCursorPosition(0, 0);
                Console.CursorVisible = false;
                for (int i = 3; i < 12; i++)
                {
                    string temp = "                                  |" + HealerASCII[i] + "    |        " + TankASCII[i] + "       |       " + DamagerASCII[i] + "       |      " + LuckerASCII[i];
                    Console.WriteLine(temp);

                }
                Console.WriteLine("      ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                for (int i = 3; i < 5; i++)
                {
                    string temp = HealerASCII[i] + "|";
                    temp += "                                      |                                                 |"
                  + "                                                |";
                    Console.WriteLine(temp);
                }
                #endregion
                #region Actualisation des données pour chaque combat
                for (int p = 0; p < 100; p++) //Action faites 100 fois
                {
                    for (int i = 1; i < 5; i++) //Pour chaque rôle en ligne i
                    {
                        for (int j = 1; j < 5; j++) // Le rôle en ligne i va affronter chaque rôle en colonne j
                        {
                            if (i != j)  //On ne prend pas les match up miroir
                            {
                                ResultatSimu[i - 1][j - 1] += SimulationIA(i, j, numberChoiceSimu); //La fonction SimulationIA retourne 1 en cas de victoire/match nul et 0 sinon
                            }
                        }
                    }
                    DisplaySimu(ResultatSimu); //Affichage avec les ASCII sous forme de tableau
                }
                #endregion


                //Commentaires pour la compéhension du joueur
                Console.WriteLine();
                Console.Write("Simulation terminée pour le {0} ! ", SimuMode[numberChoiceSimu]);
                Console.WriteLine("Sens de lecture : Le rôle en ligne i a un taux de victoire* de x% sur le rôle de colonne j.\n");
                Console.WriteLine("* les pourcentages incluent les matchs nuls (double KO) et les time out (20 tours passés sans vainqueur).\n");
                Console.WriteLine("Appuyez sur une touche pour revenir au menu.");
                Console.ReadKey();
                Console.Clear();
                Main(args); //Retour au menu
            }

            //Quitter le jeu
            else
            {
                Console.Clear();
                Random random = new Random();
                Console.CursorVisible = false;
                MoveCredit(random.Next(3, Console.WindowWidth - Prenoms[0].Length - 5), random.Next(1, 5), 20, 80000); //En argument : Colonne de démarrage, 1ère direction, Ralentissement, Nombre de rebonds avant fin
                Console.CursorVisible = true;
            }
        }
        #region ASCII ZONE
        static List<string> HealerASCII = new List<string>()
        {"                                  ",
         "                HEALER            ",
         "            _   vvvvvvvvv   _     ",
         "           ( `-._\\...../_.-' )    ",
         "            \\   ((('_')))   /     ",
         "             )   ))) (((   (      ",
         "            (   ((( v )))   )     ",
         "             )`--' )X( `-._(      ",
         "            /   _./   \\._   \\     ",
         "           /  .' /     \\ `.  \\    ",
         "          (__/  /       \\  \\__)   ",
         "               /         \\        ",
         "              /           \\       ",
         "              -------------       "
        };
        static List<string> TankASCII = new List<string>()
        {"                                  ",
         "            TANK                  ",
         "                                  ",
         "              !                   ",
         "             .-.                  ",
         "           __|=|__                ",
         "          (_/`-`\\_)               ",
         "          //\\___/\\                ",
         "          <>/   \\<>               ",
         "           \\|_._|/                ",
         "            <_I_>                 ",
         "             |||                  ",
         "            /_|_\\                 ",
         "                                  ",
        };
        static List<string> DamagerASCII = new List<string>()
        {"                                  ",
         "            DAMAGER               ",
         "                                  ",
         "                                  ",
         "          _  ____________.-       ",
         "            \\`'  __________|      ",
         "            /   (_)__]            ",
         "           |    |                 ",
         "          .'   .'                 ",
         "          |____]                  ",
         "                                  ",
         "                                  ",
         "                                  ",
         "                                  ",
         "                                  "
        };
        static List<string> LuckerASCII = new List<string>()
        {"                                  ",
         "            LUCKER                ",
         "                                  ",
         "              .-. .-.             ",
         "             (   |   )            ",
         "           .-.:  |  ;,-.          ",
         "          (_ __`.|.'__ _)         ",
         "          (    .'|`.    )         ",
         "           `-'/  |  \\`-'          ",
         "             (   !   )            ",
         "              `-' `-'\\            ",
         "                      \\           ",
         "                       )          ",
         "                                  ",
         "                                  "
        };
        static List<string> zeroASCII = new List<string>()
        {
            "000000",
            "00  00",
            "00  00",
            "00  00",
            "000000"
        };
        static List<string> oneASCII = new List<string>()
        {
            " 1111 ",
            "   11 ",
            "   11 ",
            "   11 ",
            "111111"
        };
        static List<string> twoASCII = new List<string>()
        {
            "222222",
            "     2",
            "222222",
            "2     ",
            "222222"
        };
        static List<string> threeASCII = new List<string>()
        {
            "333333",
            "    33",
            "333333",
            "    33",
            "333333"
        };
        static List<string> fourASCII = new List<string>()
        {
            "44  44",
            "44  44",
            "444444",
            "    44",
            "    44"
        };
        static List<string> fiveASCII = new List<string>()
        {
            "555555",
            "55    ",
            "555555",
            "    55",
            "555555"
        };
        static List<string> sixASCII = new List<string>()
        {
            "666666",
            "66    ",
            "666666",
            "66  66",
            "666666"
        };
        static List<string> sevenASCII = new List<string>()
        {
            "777777",
            "    77",
            "    77",
            "    77",
            "    77"
        };
        static List<string> eightASCII = new List<string>()
        {
            "888888",
            "88  88",
            "888888",
            "88  88",
            "888888"
        };
        static List<string> nineASCII = new List<string>()
        {
            "999999",
            "99  99",
            "999999",
            "    99",
            "999999",
        };
        static List<List<string>> numberASCII = new List<List<string>>() { zeroASCII, oneASCII, twoASCII, threeASCII, fourASCII, fiveASCII, sixASCII, sevenASCII, eightASCII, nineASCII };
        static List<string> percentASCII = new List<string>()
        {   " _    __ ",
            "(_)  / / ",
            "    / /  ",
            "   / /   ",
            "  / /  _ ",
            " /_/  (_)"
        };
        static List<string> Prenoms = new List<string>()
        {
                "______ __   __ _____     ______ __   __ _____   _ ",
                "| ___ \\\\ \\ / /|  ___|    | ___ \\\\ \\ / /|  ___| | |",
                "| |_/ / \\ V / | |__      | |_/ / \\ V / | |__   | |",
                "| ___ \\  \\ /  |  __|     | ___ \\  \\ /  |  __|  | |",
                "| |_/ /  | |  | |___     | |_/ /  | |  | |___  |_|",
                "\\____/   \\_/  \\____/     \\____/   \\_/  \\____/  (_)",
                "                                                  ",
                "              MADE BY BAOBAB AND SULIVAN          "
        };
        static List<string> NumberToASCII(int num, int percent = 0) //Fonction pour convertir un integer en ASCII
        {
            List<string> result = new List<string>();
            if (percent == 0) //Si on ne met pas le 2e argument, ajoute l'ASCII du % (pour les simulations)
            {
                result.Add("                " + percentASCII[0]);
                if (num >= 10)  //On sait qu'on ne prendra pas de chiffres > 100 , on divise par les cas à 2 chiffres et à 1 (>= 10 et < 10 pas de 100%, rien n'est jamais sûr dans ce monde ce sont des stats...)
                {
                    int num1, num2;
                    if (num != 100)
                    {
                        num1 = num / 10;
                        num2 = num % 10;
                    }
                    else
                    {
                        num1 = 9;
                        num2 = 9;
                    }
                    for (int i = 0; i < 5; i++) //5 = la hauteur de chaque ASCII donc le nombre de ligne, mais on pourrait mettre numberASCII[0].Length
                    {
                        string temp = numberASCII[num1][i] + " " + numberASCII[num2][i] + "   " + percentASCII[i + 1];  //On ajoute les caratères des dizaines, le caractères des unités et le signe %
                        result.Add(temp);
                    }
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        string temp = "       " + numberASCII[num][i] + "   " + percentASCII[i + 1]; //On ajoute uniquement les unités avec des espaces avant (longueur de 6)
                        result.Add(temp);
                    }
                }
                return result;
            }
            else
            {
                if (num >= 10)
                {
                    int num1, num2;
                    if (num != 100)
                    {
                        num1 = num / 10;
                        num2 = num % 10;
                    }
                    else
                    {
                        num1 = 9;
                        num2 = 9;
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        string temp = numberASCII[num1][i] + " " + numberASCII[num2][i];
                        result.Add(temp);
                    }
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        string temp = "       " + numberASCII[num][i];
                        result.Add(temp);
                    }
                }
                return result;
            }

        }
        #endregion
        #region Fonctions pour les simulations  

        //Similaire à PlayGame mais chaque action est choisie aléatoirement pour les 2 rôles et il n'y a pas d'output/Console.Write en console.  Elle retourne 1 si le role1 a gagné/match nul et 0 sinon
        static int SimulationIA(int role1, int role2, int choiceSimuMode) //Les rôles ont étés choisis au préalable aléatoirement, on les récupère en argument
        {
            int nombredeTour = 0;
            Random random = new Random(); //Pour les statistiques du Lucker
            List<string> roles = new List<string>(){
                "1 - Healer",
                "2 - Tank",
                "3 - Damager",
                "4 - Lucker"};
            //Point de vie
            List<int> VieBase = new List<int>(){
                4, //Healer
                5, //Tank
                3, //Damager
                random.Next(1,6) // Lucker
                };
            int vie1 = VieBase[role1 - 1];
            int vie2 = VieBase[role2 - 1];

            bool FinDePartie = false;
            while (!FinDePartie)
            {
                nombredeTour++;
                List<string> actions = new List<string>(){
                    "1 - Attaquer",
                    "2 - Défendre",
                    "3 - Spécial"};
                int action1 = 0;
                int action2 = 0;

                if (choiceSimuMode == 1)
                {
                    action1 = random.Next(1, actions.Count + 1);
                    action2 = random.Next(1, actions.Count + 1);
                }
                else if (choiceSimuMode == 2)
                {
                    action1 = 1;
                    action2 = 1;
                }
                else
                {
                    action1 = IaDarkChoice(actions, role1, role2, vie1, vie2, 1);
                    action2 = IaDarkChoice(actions, role2, role1, vie2, vie1, 1);
                }

                Tuple<int, int> TourUpdate = ResolutionAction_SansConsole(action1, role1, action2, role2);

                vie1 = Math.Min(TourUpdate.Item1 + vie1, 6);
                vie2 = Math.Min(TourUpdate.Item2 + vie2, 6);
                if (vie1 < 1 || vie2 < 1) FinDePartie = true;
                if (nombredeTour > 20) return 1;

            }
            if ((vie1 < 1 && vie2 < 1) || vie1 > 0) return 1;
            else return 0;
        }
        //Affichage du tableau des taux de victoires en ASCII en reprenant les valeurs de l'array simulations
        static void DisplaySimu(int[][] simulations)
        {
            #region     fixing pour les très petits écrans. Portions de Code remonté dans la fonction main pour ne pas etre rappelé à chaque simulation et auto-scroll
            /*
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;
            for (int i = 3; i < 12; i++)
            {
                string temp = "                                  |" + HealerASCII[i] + "    |        " + TankASCII[i] + "       |       " + DamagerASCII[i] + "       |      " + LuckerASCII[i];
                Console.WriteLine(temp);

            }
            Console.WriteLine("      ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            
            
            for (int i = 3; i < 5; i++)
            {
                string temp = HealerASCII[i] + "|";
                temp += "                                      |                                                 |"
              + "                                                |";
                Console.WriteLine(temp);
            }
            */
            #endregion
            Console.CursorVisible = false; //On cache le curseur pour les joueurs épileptiques
            Console.SetCursorPosition(0, 12);
            for (int i = 5; i < HealerASCII.Count - 1; i++)  //Pourcentages du Healer (les autres classes sont similaires)
            {
                string temp = HealerASCII[i] + "|";  //Dessin de la du 1er rôle = Healer. Temp sera la chaine de caractère d'une ligne entière avec les chiffres en ASCII
                if (i > 4 && i < 11) temp += "                                      |         "   //Healer VS Healer on ne met rien 
                 + NumberToASCII(simulations[0][1])[i - 5] + "               |            "   //Stats du Rôle 1 (Healer) VS Rôle 2(Tank)  l'array int[][] simulations est 0-indexed
                 + NumberToASCII(simulations[0][2])[i - 5] + "           |            "   //Stats du Rôle 1 (Healer) VS Rôle 3(Damager) 
                 + NumberToASCII(simulations[0][3])[i - 5];                             //Stats du Rôle 1 (Healer) VS Rôle 3(Lucker)
                else
                {
                    temp += "                                      |                                                 |"
              + "                                                |";
                }

                //Colorisation des caractères en fonction du pourcentage affiché. < 40 en rouge, [40,60] en jaune , et > 60 en vert
                WriteWithColor_SIMU(-1, temp, 0, 73); Console.Write("|");  // Les colonnes sont divisés comme tels : 0 - 35 ||  35 - 74 || 74 - 124 || 124 - 173 || 173 - reste
                WriteWithColor_SIMU(simulations[0][1], temp, 74, 123); Console.Write("|");
                WriteWithColor_SIMU(simulations[0][2], temp, 124, 172); Console.Write("|");
                WriteWithColor_SIMU(simulations[0][3], temp, 173, temp.Length);
                Console.WriteLine();
                //Console.WriteLine(temp);

            }
            Console.WriteLine("      ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            for (int i = 4; i < TankASCII.Count; i++) //idem pour le tank
            {
                string temp = TankASCII[i] + "|";
                if (i > 5 && i < 12) temp += "         " + NumberToASCII(simulations[1][0])[i - 6] + "    |                                                 |            "
                + NumberToASCII(simulations[1][2])[i - 6] + "           |            "
                + NumberToASCII(simulations[1][3])[i - 6];
                else
                {
                    temp += "                                      |                                                 |"
              + "                                                |";
                }



                Console.Write(temp.Remove(35));
                WriteWithColor_SIMU(simulations[1][0], temp, 35, 73); Console.Write("|");
                WriteWithColor_SIMU(-1, temp, 74, 123); Console.Write("|");
                WriteWithColor_SIMU(simulations[1][2], temp, 124, 172); Console.Write("|");
                WriteWithColor_SIMU(simulations[1][3], temp, 173, temp.Length);
                Console.WriteLine();
                //Console.WriteLine(temp);
            }
            Console.WriteLine("      ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            for (int i = 2; i < DamagerASCII.Count - 3; i++) //idem le damager
            {
                string temp = DamagerASCII[i] + "|";
                if (i > 3 && i < 10) temp += "         " + NumberToASCII(simulations[2][0])[i - 4] + "    |         "
                 + NumberToASCII(simulations[2][1])[i - 4] + "               |                                " + "                |            "
                 + NumberToASCII(simulations[2][3])[i - 4];
                else
                {
                    temp += "                                      |                                                 |"
             + "                                                |";
                }

                Console.Write(temp.Remove(35));
                WriteWithColor_SIMU(simulations[2][0], temp, 35, 73); Console.Write("|");
                WriteWithColor_SIMU(simulations[2][1], temp, 74, 123); Console.Write("|");
                WriteWithColor_SIMU(-1, temp, 124, 172); Console.Write("|");
                WriteWithColor_SIMU(simulations[2][3], temp, 173, temp.Length);
                Console.WriteLine();
                //Console.WriteLine(temp);
            }
            Console.WriteLine("      ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            for (int i = 2; i < LuckerASCII.Count - 2; i++) //idem le lucker
            {
                string temp = LuckerASCII[i] + "|";
                if (i > 3 && i < 10) temp += "         " + NumberToASCII(simulations[3][0])[i - 4] + "    |         "
                + NumberToASCII(simulations[3][1])[i - 4] + "               |            "
                + NumberToASCII(simulations[3][2])[i - 4] + "           |";
                else
                {
                    temp += "                                      |                                                 |"
            + "                                                |";
                }

                Console.Write(temp.Remove(35));
                WriteWithColor_SIMU(simulations[3][0], temp, 35, 73); Console.Write("|");
                WriteWithColor_SIMU(simulations[3][1], temp, 74, 123); Console.Write("|");
                WriteWithColor_SIMU(simulations[3][2], temp, 124, 172); Console.Write("|");
                WriteWithColor_SIMU(-1, temp, 173, temp.Length);
                Console.WriteLine();
                //Console.WriteLine(temp);
            }

            Console.CursorVisible = true;
        }

        //Similaire à ResoluationAction mais sans d'output en console.
        static Tuple<int, int> ResolutionAction_SansConsole(int actionJoueur, int roleJoueur, int actionIA, int roleIA) //Exactement pareil que ResoluationAction mais sans ouput de console. Voir commentaire sur ResolutionAction
        {
            Random random = new Random();
            int Lucker = random.Next(0, 2);
            int Lucker2 = random.Next(0, 2);
            if (actionJoueur == 1)
            {
                if (actionIA == 1)
                {
                    int att1 = DommageParRole(roleIA), att2 = DommageParRole(roleJoueur);
                    return new Tuple<int, int>(-att1, -att2);
                }
                else if (actionIA == 2) return new Tuple<int, int>(0, 0);
                else
                {
                    if (roleIA == 1) return new Tuple<int, int>(0, -DommageParRole(roleJoueur) + 2);
                    else if (roleIA == 2) return new Tuple<int, int>(-2, -DommageParRole(roleJoueur) - 1);
                    else if (roleIA == 3) return new Tuple<int, int>(-DommageParRole(roleJoueur), -DommageParRole(roleJoueur));
                    else
                    {
                        if (Lucker == 0) return new Tuple<int, int>(-DommageParRole(roleIA), -DommageParRole(roleJoueur) + 1);
                        else return new Tuple<int, int>(0, -DommageParRole(roleJoueur));
                    }
                }
            }
            else if (actionJoueur == 2)
            {
                if (actionIA == 1 || actionIA == 2) return new Tuple<int, int>(0, 0);
                else
                {
                    if (roleIA == 1) return new Tuple<int, int>(0, 2);
                    else if (roleIA == 2) return new Tuple<int, int>(-1, -1);
                    else if (roleIA == 3) return new Tuple<int, int>(0, 0);
                    else
                    {
                        if (Lucker == 0) return new Tuple<int, int>(0, 1);
                        else return new Tuple<int, int>(0, 0);
                    }
                }
            }
            else
            {
                if (roleJoueur == 1)
                {
                    if (actionIA == 1) return new Tuple<int, int>(-DommageParRole(roleIA) + 2, 0);
                    else if (actionIA == 2) return new Tuple<int, int>(2, 0);
                    else
                    {
                        if (roleIA == 1) return new Tuple<int, int>(2, 2);
                        else if (roleIA == 2) return new Tuple<int, int>(0, -1);
                        else if (roleIA == 3) return new Tuple<int, int>(2, 0);
                        else
                        {
                            if (Lucker == 0) return new Tuple<int, int>(-DommageParRole(roleIA) + 2, 1);
                            else return new Tuple<int, int>(2, 0);
                        }
                    }

                }
                else if (roleJoueur == 2)
                {
                    if (actionIA == 1) return new Tuple<int, int>(-DommageParRole(roleIA) - 1, -2);
                    else if (actionIA == 2) return new Tuple<int, int>(-1, -1);
                    else
                    {
                        if (roleIA == 1) return new Tuple<int, int>(-1, 0);
                        else if (roleIA == 2) return new Tuple<int, int>(-3, -3);
                        else if (roleIA == 3) return new Tuple<int, int>(-3, -2);
                        else
                        {
                            if (Lucker == 0) return new Tuple<int, int>(-DommageParRole(roleIA) - 1, -1);
                            else return new Tuple<int, int>(-1, -2);
                        }
                    }
                }
                else if (roleJoueur == 3)
                {
                    if (actionIA == 1) return new Tuple<int, int>(-DommageParRole(roleIA), -DommageParRole(roleIA));
                    else if (actionIA == 2) return new Tuple<int, int>(0, 0);
                    else
                    {
                        if (roleIA == 1) return new Tuple<int, int>(0, 2);
                        else if (roleIA == 2) return new Tuple<int, int>(-2, -3);
                        else if (roleIA == 3) return new Tuple<int, int>(0, 0);
                        else
                        {
                            if (Lucker == 0)
                            {
                                int temp = DommageParRole(roleIA);
                                return new Tuple<int, int>(-temp, 1 - temp);
                            }
                            else return new Tuple<int, int>(0, 0);
                        }
                    }
                }
                else
                {
                    if (actionIA == 1) return new Tuple<int, int>(-DommageParRole(roleIA), -DommageParRole(roleJoueur));
                    else if (actionIA == 2) return new Tuple<int, int>(0, 0);
                    else
                    {
                        if (Lucker == 0)
                        {
                            if (roleIA == 1) return new Tuple<int, int>(1, 2 - DommageParRole(roleJoueur));
                            else if (roleIA == 2) return new Tuple<int, int>(-1, -DommageParRole(roleJoueur - 1));
                            else if (roleIA == 3) return new Tuple<int, int>(-DommageParRole(roleJoueur) + 1, -DommageParRole(roleJoueur));
                            else
                            {
                                if (Lucker2 == 0)
                                {
                                    int temp = DommageParRole(roleJoueur);
                                    int temp2 = DommageParRole(roleIA);
                                    return new Tuple<int, int>(1 - temp2, 1 - temp);
                                }
                                else return new Tuple<int, int>(1, -DommageParRole(roleJoueur));
                            }
                        }
                        else
                        {
                            if (roleIA == 1) return new Tuple<int, int>(0, 2);
                            else if (roleIA == 2) return new Tuple<int, int>(-2, -1);
                            else if (roleIA == 3) return new Tuple<int, int>(0, 0);
                            else
                            {
                                if (Lucker2 == 0) return new Tuple<int, int>(-DommageParRole(roleIA), 1);
                                else return new Tuple<int, int>(0, 0);
                            }
                        }
                    }
                }
            }
        }
        #endregion
        #region Fonctions Affichage (titres, rôles, combat..)
        static void DisplayTitle() //Affiche le titre en ASCII
        {
            List<string> Title = new List<string>()
            {
            "+--------------------------------------------------------------------------------------------------------------------------------------+",
            "|                                                                                                                                      |",
            "|    _   _ _ _   _                 _        ______ _       _     _   _                   _                 _       _                   |",
            "|   | | | | | | (_)               | |       |  ___(_)     | |   | | (_)                 (_)               | |     | |                  |",
            "|   | | | | | |_ _ _ __ ___   __ _| |_ ___  | |_   _  __ _| |__ | |_ _ _ __   __ _   ___ _ _ __ ___  _   _| | __ _| |_ ___  _ __       |",
            "|   | | | | | __| | '_ ` _ \\ / _` | __/ _ \\ |  _| | |/ _` | '_ \\| __| | '_ \\ / _` | / __| | '_ ` _ \\| | | | |/ _` | __/ _ \\| '__|      |",
            "|   | |_| | | |_| | | | | | | (_| | ||  __/ | |   | | (_| | | | | |_| | | | | (_| | \\__ \\ | | | | | | |_| | | (_| | || (_) | |         |",
            "|    \\___/|_|\\__|_|_| |_| |_|\\__,_|\\__\\___| \\_|   |_|\\__, |_| |_|\\__|_|_| |_|\\__, | |___/_|_| |_| |_|\\__,_|_|\\__,_|\\__\\___/|_|         |",
            "|                                                    __/ |                   __/ |                                                     |",
            "|                                                    |___/                   |___/                                                     |",
            "|                                                                                                                                      |",
            "+--------------------------------------------------------------------------------------------------------------------------------------+"
        };
            Console.WriteLine(Title[0]);
            for (int i = 1; i < Title.Count - 1; i++)
            {
                Console.Write(Title[i][0]); //120-ish caractères pour notre titres et 10 couleurs, on fait des couleurs tous les 12 caractères
                WriteWithColor_Cut("Magenta", Title[i], 1, 12);
                WriteWithColor_Cut("DarkMagenta", Title[i], 12, 24);
                WriteWithColor_Cut("DarkBlue", Title[i], 24, 36);
                WriteWithColor_Cut("Blue", Title[i], 36, 48);
                WriteWithColor_Cut("DarkCyan", Title[i], 48, 60);
                WriteWithColor_Cut("DarkGreen", Title[i], 60, 72);
                WriteWithColor_Cut("Green", Title[i], 72, 84);
                WriteWithColor_Cut("DarkYellow", Title[i], 84, 96);
                WriteWithColor_Cut("Yellow", Title[i], 96, 108);
                WriteWithColor_Cut("Red", Title[i], 108, Title[i].Length - 1);
                Console.Write(Title[i][Title[i].Length - 1]);
                Console.WriteLine();
            }
            Console.WriteLine(Title[Title.Count - 1]);
            Console.WriteLine("                                                                                                              ");
            Console.WriteLine("                                                                                                              ");
        }
        static void DisplayRoles() //Affiche les rôles pour la sélection en début de partie
        {
            Console.WriteLine("_________________________________________________________________________________________________________________________________________________________________________________________________________________________________________");
            Console.WriteLine("                                                                                     \n                                                                                     ");
            Console.WriteLine("            _   vvvvvvvvv   _         HEALER                                                                 |                !            TANK ");
            Console.WriteLine("           ( `-._\\...../_.-' )                                                                               |               .-.");
            //Console.WriteLine("            \\   ((('_')))   /         Point de vie : ♥ ♥ ♥ ♥                                                 |             __|=|__         Point de vie : ♥ ♥ ♥ ♥ ♥");
            string FirstLineFirstPart = "            \\   ((('_')))   /         Point de vie : ♥ ♥ ♥ ♥",
            FirstLineSecondPart = "                                                 |             __|=|__         Point de vie : ♥ ♥ ♥ ♥ ♥";
            WriteWithColor("Green", FirstLineFirstPart, FirstLineFirstPart.IndexOf("♥")); //On écrit en vert à partir du premier coeur trouvé
            WriteWithColor("Green", FirstLineSecondPart, FirstLineSecondPart.IndexOf("♥")); //idem
            Console.WriteLine();
            Console.WriteLine("1111         )   ))) (((   (                                                                                 |   2222     (_/`-`\\_)");
            //Console.WriteLine("  11        (   ((( v )))   )         Force d'attaque : o                                                    |  22  22    //\\___/\\         Force d'attaque : o");
            string FirstAttFirstPart = "  11        (   ((( v )))   )         Force d'attaque : o",
            FirstAttSecPart = "                                                    |  22  22    //\\___/\\         Force d'attaque : o";
            WriteWithColor("Red", FirstAttFirstPart, FirstAttFirstPart.IndexOf('o', FirstAttFirstPart.IndexOf('o') + 1)); //on écrit en rouge à partir du 2e o trouvé (car il y a un o dans Force..)
            WriteWithColor("Red", FirstAttSecPart, FirstAttSecPart.IndexOf('o', FirstAttSecPart.IndexOf('o') + 1));
            Console.WriteLine();
            Console.WriteLine("  11         )`--' )X( `-._(                                                                                 |     22     <>/   \\<>");
            //Console.WriteLine("  11        /   _./   \\._   \\         Capacité spéciale - Soin : Gagne  deux points de vie (+ ♥ ♥)           |    22       \\|_._|/         Capacité spéciale - Attaque puissante : Correspond à une attaque durant laquelle le");
            //Console.WriteLine("111111     /  .' /     \\ `.  \\                                                                               |  222222      <_I_>          Tank sacrifie un de ses points de vie pour augmenter sa force d'attaque de 1 et ce");
            //Console.WriteLine("          (__/  /       \\  \\__)                                                                              |               |||           uniquement durant le tours en cours. (- ♥ => + o). La force d'attaque bonus traverse les");
            //Console.WriteLine("               /         \\                                                                                   |              /_|_\\          défenses.");

            string FirstSpeFirst = "  11        /   _./   \\._   \\         Capacité spéciale - Soin : Gagne  deux points de vie",
            FirstSpeParenth = " (+ ",
            FirstSpeSec = "♥ ♥",
            FirstSpeThird = ")           |    22       \\|_._|/         Capacité spéciale - Attaque puissante : Correspond à une attaque durant laquelle le",
            FirstSpeFour = "111111     /  .' /     \\ `.  \\                                                                               |  222222      <_I_>          Tank sacrifie un de ses points de vie pour augmenter sa force d'attaque de 1 et ce",
            FirstSpeFive = "          (__/  /       \\  \\__)                                                                              |               |||           uniquement durant le tours en cours. (",
            FirstSpeInter5 = "- ♥",
            FirstSpeInter51 = " => + o",
            FirstSpeInter6 = "). La force d'attaque bonus traverse les",
            FirstSpe6 = "               /         \\                                                                                   |              /_|_\\          défenses.";
            WriteWithColor("Yellow", FirstSpeFirst, FirstSpeFirst.IndexOf("Gagne"));
            Console.Write(FirstSpeParenth);
            WriteWithColor("Green", FirstSpeSec, 0);
            WriteWithColor("Yellow", FirstSpeThird, FirstSpeThird.IndexOf("Correspond")); Console.WriteLine();
            WriteWithColor("Yellow", FirstSpeFour, FirstSpeFour.IndexOf("Tank")); Console.WriteLine();
            WriteWithColor("Yellow", FirstSpeFive, FirstSpeFive.IndexOf("uniquement"));
            WriteWithColor("Green", FirstSpeInter5, 2); WriteWithColor("Red", FirstSpeInter51, 6); WriteWithColor("Yellow", FirstSpeInter6, 0); Console.WriteLine();
            WriteWithColor("Yellow", FirstSpe6, FirstSpe6.IndexOf("défenses")); Console.WriteLine();
            Console.WriteLine("              /           \\                                                                                  |");
            Console.WriteLine("              -------------                                                                                  |");
            Console.Write("\n");
            Console.WriteLine("_________________________________________________________________________________________________________________________________________________________________________________________________________________________________________");
            Console.Write("\n\n");
            Console.WriteLine("                                      DAMAGER                                                                |               .-. .-.       LUCKER");
            Console.WriteLine("                                                                                                             |              (   |   )      ");
            //Console.WriteLine("           _  ____________            Point de vie : ♥ ♥ ♥                                                   |            .-.:  |  ;,-.    Point de vie : ♥ ? ? ? ? (aléatoire entre 1 et 5)");
            string SecondLineFirstPart = "           _  ____________            Point de vie : ♥ ♥ ♥",
            SecondLineSecondPart = "                                                   |            .-.:  |  ;,-.    Point de vie : ♥",
            SecondLineThirdPart = " ? ? ? ?",
            SecondLineLastPart = " (aléatoire entre 1 et 5)";
            WriteWithColor("Green", SecondLineFirstPart, SecondLineFirstPart.IndexOf("♥"));
            WriteWithColor("Green", SecondLineSecondPart, SecondLineSecondPart.IndexOf("♥"));
            WriteWithColor("Yellow", SecondLineThirdPart, 0);
            Console.Write(SecondLineLastPart);
            Console.WriteLine();
            Console.WriteLine(" 3333       \\`'  __________|                                                                                 |  44  44   (_ __`.|.'__ _)  ");
            //Console.WriteLine("33  33      /   (_)__]                Force d'attaque : o o                                                  |  44  44   (    .'|`.    )   Force d'attaque : o ? (aléatoire à chaque attaque entre 1 et 2)");
            string SecAttFirstPart = "33  33      /   (_)__]                Force d'attaque : o o",
            SecAttSecPart = "                                                  |  44  44   (    .'|`.    )   Force d'attaque : o ",
            SecAttThirPart = "? ",
            SecAttLastPart = "(aléatoire à chaque attaque entre 1 et 2)";
            WriteWithColor("Red", SecAttFirstPart, SecAttFirstPart.IndexOf('o', SecAttFirstPart.IndexOf('o') + 1));
            WriteWithColor("Red", SecAttSecPart, SecAttSecPart.IndexOf('o', SecAttSecPart.IndexOf('o') + 1));
            WriteWithColor("Yellow", SecAttThirPart, 0);
            Console.Write(SecAttLastPart);
            Console.WriteLine();
            Console.WriteLine("   333     |    |                                                                                            |  444444    `-'/  |  \\`-'    ");
            //Console.WriteLine("33  33    .'   .'                     Capacité spéciale - Rage : Inflige en retour les dégâts qui            |      44      (   !   )      Capacité spéciale - Lancé de pièce :  le Lucker décide de lancer une pièce, s’il ");
            //Console.WriteLine(" 3333     |____]                      lui sont infligés durant ce tour. Les dégâts sont quand même subis     |      44       `-' `-'\\      tombe sur pile il ne fait rien. S'il tombe sur face il lance une attaque et gagne");
            //Console.WriteLine("                                      par le Damager.                                                        |                       \\     un point de vie (+ ♥) ");
            string SecSpe1 = "33  33    .'   .'                     Capacité spéciale - Rage : Inflige en retour les dégâts qui",
            SecSpe12 = "            |      44      (   !   )      Capacité spéciale - Lancé de pièce :  le Lucker décide de lancer une pièce, s’il ",
            SecSpe2 = " 3333     |____]                      lui sont infligés durant ce tour. Les dégâts sont quand même subis",
            SecSpe21 = "     |      44       `-' `-'\\      tombe sur pile il ne fait rien. S'il tombe sur face il lance une attaque et gagne",
            SecSpe3 = "                                      par le Damager.",
            SecSpe31 = "                                                        |                       \\     un point de vie",
            SecSpe32 = " (+ ♥",
            SpecSpe3last = ") ";
            WriteWithColor("Yellow", SecSpe1, SecSpe1.IndexOf("Inflige"));
            WriteWithColor("Yellow", SecSpe12, SecSpe12.IndexOf("le Lucker")); Console.WriteLine();
            WriteWithColor("Yellow", SecSpe2, SecSpe2.IndexOf("lui"));
            WriteWithColor("Yellow", SecSpe21, SecSpe21.IndexOf("tombe")); Console.WriteLine();
            WriteWithColor("Yellow", SecSpe3, SecSpe3.IndexOf("par"));
            WriteWithColor("Yellow", SecSpe31, SecSpe31.IndexOf("un"));
            WriteWithColor("Green", SecSpe32, SecSpe32.IndexOf("♥")); Console.Write(SpecSpe3last); Console.WriteLine();
            Console.WriteLine("                                                                                                             |                        )    ");
            Console.WriteLine("                                                                                                             |                             ");
            Console.WriteLine("                                                                                                             |                             ");
            Console.Write("\n");
            Console.WriteLine("_________________________________________________________________________________________________________________________________________________________________________________________________________________________________________");

        }
        static void DisplayRoleVersus(int role1, int role2, int numberTour) //Affiche les rôles du match actuel + le nombre de tour restants
        {
            List<string> asci1 = new List<string>(), asci2 = new List<string>();
            //Récupérations des rôles passés en argument
            if (role1 == 1) asci1 = HealerASCII;
            else if (role1 == 2) asci1 = TankASCII;
            else if (role1 == 3) asci1 = DamagerASCII;
            else if (role1 == 4) asci1 = LuckerASCII;

            if (role2 == 1) asci2 = HealerASCII;
            else if (role2 == 2) asci2 = TankASCII;
            else if (role2 == 3) asci2 = DamagerASCII;
            else if (role2 == 4) asci2 = LuckerASCII;
            List<string> AsciConvert = NumberToASCII(numberTour, 1); //Affichage du nombre de tours restants

            for (int i = 0; i < Math.Min(asci1.Count, asci2.Count); i++)
            {
                string temp = asci1[i] + "                                " + asci2[i] + "                                "; //Temp correspond à la chaine de caractère d'une ligne
                if (i > 3 && i < 9) temp += AsciConvert[i - 4];
                Console.WriteLine(temp);
            }
        }
        static void DisplayStats(int vie1, int role1, int vie2, int role2) // Affichage des stats points de vie, force d'attaque en continu selon le déroulé de la partie
        {
            Console.WriteLine();
            string hp1 = "Vie : ", hp2 = "Vie : ", att1 = "", att2 = "";
            string hpResult = "", AttResult = "";

            Console.SetCursorPosition(0, 27);
            Console.WriteLine("                                                                                                                                                                 ");
            Console.SetCursorPosition(0, 27);
            #region Point de vie
            for (int i = 0; i < Math.Max(vie1, 0); i++)
            {
                hp1 += " ♥ ";
            }
            for (int i = 0; i < Math.Max(vie2, 0); i++)
            {
                hp2 += " ♥ ";
            }
            hpResult = "           " + hp1;
            for (int i = hpResult.Length - 15; i < 60; i++)
            {
                hpResult += " ";
            }
            WriteWithColor("Green", hpResult, 18);
            WriteWithColor("Green", hp2, 6);
            Console.WriteLine();
            //hpResult += hp2;
            //Console.WriteLine(hpResult);
            #endregion
            #region Force d'attaque
            if (role1 == 1 || role1 == 2)
            {
                att1 = "Attaque : o ";
            }
            else if (role1 == 3)
            {
                att1 = "Attaque :  o  o ";
            }
            else if (role1 == 4)
            {
                att1 = "Attaque :  o  ? ";
            }
            if (role2 == 1 || role2 == 2)
            {
                att2 = "Attaque :  o ";
            }
            else if (role2 == 3)
            {
                att2 = "Attaque :  o  o ";
            }
            else if (role2 == 4)
            {
                att2 = "Attaque :  o  ? ";
            }
            AttResult = "           " + att1;
            for (int i = AttResult.Length - 15; i < 60; i++)
            {
                AttResult += " ";
            }
            WriteWithColor("Red", AttResult, 20);
            WriteWithColor("Red", att2, 10);
            Console.WriteLine();
            //AttResult += att2;
            //Console.WriteLine(AttResult);
            #endregion


            //Remettre le curseur à la bonne position

            Console.SetCursorPosition(0, 32);
        }
        static void WriteWithColor(string color, string targetLine, int start) //Ecrit en console la chaine de caractère en paramètre dans la couleur rentrée en paramètres à partir de l'index start de la chaine
        {
            string firstPart = "", coloredPart = "";
            #region Splitting du string pour prise mettre les couleurs
            for (int i = 0; i < start; i++) { firstPart += targetLine[i]; }
            for (int i = start; i < targetLine.Length; i++) { coloredPart += targetLine[i]; }
            #endregion
            Console.Write(firstPart);
            #region Couleur en fonction du string color
            if (color == "Red") Console.ForegroundColor = ConsoleColor.Red;
            else if (color == "Green") Console.ForegroundColor = ConsoleColor.Green;
            else if (color == "Yellow") Console.ForegroundColor = ConsoleColor.Yellow;
            else if (color == "DarkGreen") Console.ForegroundColor = ConsoleColor.DarkGreen;
            else if (color == "Magenta") Console.ForegroundColor = ConsoleColor.Magenta;
            else if (color == "DarkMagenta") Console.ForegroundColor = ConsoleColor.DarkMagenta;
            else if (color == "DarkBlue") Console.ForegroundColor = ConsoleColor.DarkBlue;
            else if (color == "Blue") Console.ForegroundColor = ConsoleColor.Blue;
            else if (color == "DarkCyan") Console.ForegroundColor = ConsoleColor.DarkCyan;
            else if (color == "DarkYellow") Console.ForegroundColor = ConsoleColor.DarkYellow;
            else if (color == "DarkRed") Console.ForegroundColor = ConsoleColor.DarkRed;
            #endregion
            Console.Write(coloredPart);
            Console.ForegroundColor = ConsoleColor.White;
        }
        static void WriteWithColor_Cut(string color, string targetLine, int start, int end) //Similaire à WriteWithColor mais avec un index de fin de couleur
        {
            string coloredPart = "";
            #region Splitting du string pour prise mettre les couleurs
            for (int i = start; i < end; i++) { coloredPart += targetLine[i]; }
            #endregion
            #region Couleur en fonction du string color
            if (color == "Red") Console.ForegroundColor = ConsoleColor.Red;
            else if (color == "Green") Console.ForegroundColor = ConsoleColor.Green;
            else if (color == "Yellow") Console.ForegroundColor = ConsoleColor.Yellow;
            else if (color == "DarkGreen") Console.ForegroundColor = ConsoleColor.DarkGreen;
            else if (color == "Magenta") Console.ForegroundColor = ConsoleColor.Magenta;
            else if (color == "DarkMagenta") Console.ForegroundColor = ConsoleColor.DarkMagenta;
            else if (color == "DarkBlue") Console.ForegroundColor = ConsoleColor.DarkBlue;
            else if (color == "Blue") Console.ForegroundColor = ConsoleColor.Blue;
            else if (color == "DarkCyan") Console.ForegroundColor = ConsoleColor.DarkCyan;
            else if (color == "DarkYellow") Console.ForegroundColor = ConsoleColor.DarkYellow;
            else if (color == "DarkRed") Console.ForegroundColor = ConsoleColor.DarkRed;
            #endregion
            Console.Write(coloredPart);
            Console.ForegroundColor = ConsoleColor.White;
        }
        static void WriteWithColor_SIMU(int percentage1, string targetLine, int start, int end) //Similaire à Color_Cut mais avec un integer au lieu d'une couleur afin de mettre une couleur en fonction du pourcentage
        {
            string coloredPart = "";
            #region Splitting du string pour prise mettre les couleurs
            for (int i = start; i < end; i++) { coloredPart += targetLine[i]; }
            #endregion
            #region Couleur en fonction du string color
            if (percentage1 == -1) Console.ForegroundColor = ConsoleColor.White;
            else if (percentage1 < 40) Console.ForegroundColor = ConsoleColor.Red;
            else if (percentage1 > 60) Console.ForegroundColor = ConsoleColor.Green;
            else Console.ForegroundColor = ConsoleColor.Yellow;
            #endregion
            Console.Write(coloredPart);
            Console.ForegroundColor = ConsoleColor.White;
        }
        static void ClearLastLines(int number) //Efface les number dernières lignes de la console
        {
            for (int i = 0; i < number; i++)
            {
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, Console.CursorTop - 1);
            }
        }
        static void DisplayCredit(int j, int i) //Display le BYE BYE de crédit à la ligne i et colonne j (prend directement la liste Prenoms des crédits/byebye sans les mettre en argument
        {
            for (int k = 0; k < Prenoms.Count; k++)
            {
                Console.SetCursorPosition(i, j + k);
                Console.WriteLine(Prenoms[k]);
            }
        }

        static void ChangeColorCredit(int i)
        {
            if(i==1) Console.ForegroundColor = ConsoleColor.White;
            if (i == 2) Console.ForegroundColor = ConsoleColor.Green;
            if (i == 3) Console.ForegroundColor = ConsoleColor.Cyan;
            if (i == 4) Console.ForegroundColor = ConsoleColor.Magenta;
            if (i == 5) Console.ForegroundColor = ConsoleColor.Yellow;
            if (i == 6) Console.ForegroundColor = ConsoleColor.DarkMagenta;
            if (i == 7) Console.ForegroundColor = ConsoleColor.DarkBlue;
            if (i == 8) Console.ForegroundColor = ConsoleColor.Red;
        }
        static void MoveCredit(int col, int direction, int slow, int nbRebonds)
        {
            Random random = new Random();
            int j = col + 1;
            while (nbRebonds > 0) //Conditions pour sortir une fois le nombre de rebonds atteint 
            {
                int i = Console.CursorTop + 1;
                if (direction == 1) // Direction en bas à droite
                {
                    while (i < Console.WindowHeight - Prenoms.Count && i > 0 && j > 0 && j < Console.WindowWidth - Prenoms[0].Length - 4) //Ne pas dépasser l'écran
                    {
                        DisplayCredit(i++, j++); //on incrémente directement ligne++ -> en bas, colone++ -> à droite
                        Thread.Sleep(slow); //histoire que ça ne soit pas trop rapide
                        ClearLastLines(Prenoms.Count + 1); //On clear les lignes à chaque fois              
                    }
                    if (i >= Console.WindowHeight - Prenoms.Count) direction = 4; // Si les crédits touchent le sol alors on va en haut à droite \ /  (logique)
                    else direction = 2; // Sinon on va en bas à gauche car il aurait touché à droite de l'écran > (logique)
                    j = Math.Min(j, Console.WindowWidth - Prenoms[0].Length - 5);
                    ChangeColorCredit(random.Next(1, 9));
                }
                else if (direction == 2) //Direction en bas à gauche
                {
                    while (i < Console.WindowHeight - Prenoms.Count && i > 0 && j > 0 && j < Console.WindowWidth - Prenoms[0].Length - 4)
                    {
                        DisplayCredit(i++, j--);
                        Thread.Sleep(slow);
                        ClearLastLines(Prenoms.Count + 1);
                    }
                    if (j <= 0) direction = 1; //Si on touche à gauche on va en bas à droite
                    else direction = 3; //Sinon on va en haut à gauche
                    j = Math.Max(j, 1);
                    ChangeColorCredit(random.Next(1, 9));

                }
                else if (direction == 3) //En haut à gauche
                {
                    while (i < Console.WindowHeight - Prenoms.Count && i > 0 && j > 0 && j < Console.WindowWidth - Prenoms[0].Length - 4)
                    {
                        DisplayCredit(i--, j--);
                        Thread.Sleep(slow);
                        ClearLastLines(Prenoms.Count + 1);
                    }
                    if (i <= 0) direction = 2; //Si on touche en haut on va en bas à gauche
                    else direction = 4; //Sinon on va en haut à droite
                    j = Math.Max(j, 1);
                    ChangeColorCredit(random.Next(1, 9));
                }
                else if (direction == 4) // En haut à droite
                {
                    while (i < Console.WindowHeight - Prenoms.Count && i > 0 && j > 0 && j < Console.WindowWidth - Prenoms[0].Length - 4)
                    {
                        DisplayCredit(i--, j++);
                        Thread.Sleep(slow);
                        ClearLastLines(Prenoms.Count + 1);
                    }
                    if (i <= 0) direction = 1; //Si on touche en haut on va en bas à droite
                    else direction = 3; // Sinon on va en haut à gauche
                    j = Math.Min(j, Console.WindowWidth - Prenoms[0].Length - 5);
                    ChangeColorCredit(random.Next(1, 9));
                }
                nbRebonds--;
            }
            Console.Clear();
            Thread.Sleep(1000);
            int colMiddle = (Console.WindowWidth - Prenoms[0].Length) / 2, rowMiddle = (Console.WindowHeight - Prenoms.Count) / 2;
            for (int k = 0; k < Prenoms.Count; k++)
            {
                Console.SetCursorPosition(colMiddle, rowMiddle + k);
                Console.WriteLine(Prenoms[k]);
            }
            for (int k = 0; k < (Console.WindowHeight - Prenoms.Count) / 2; k++)
            {
                Console.WriteLine();
            }
        } //Fonction pour le mouvement des crédits à la DVD 
        #endregion
        #region Principales Fonctions GamePlay
        static void PlayGame(int IAnumberChoice)
        {
            Random random = new Random(); //Pour les statistiques du Lucker
            List<string> roles = new List<string>(){
                "1 - Healer",
                "2 - Tank",
                "3 - Damager",
                "4 - Lucker"};

            DisplayTitle();   //Affichage du Titre dans la console
            Console.WriteLine("Bienvenue dans l'arène!                                                                ");
            Console.WriteLine("                                                       ");
            #region Choix des rôles
            string ChoixRole = "Veuillez choisir un personnage parmis ceux disponibles (tapez le chiffre correspondant) : ";
            Console.WriteLine(ChoixRole);
            DisplayRoles(); //affichage de rôles
            int roleJoueur = RolesChoice(roles); //Choix des rôles pour le joueur
            int roleIA = IaRandomChoice(roles); //Aléatoire pour l'IA
            Console.WriteLine("\nQue le meilleur gagne !");
            Thread.Sleep(1600); //petit temps d'attente pour simuler un chargement
            ClearLastLines(44); //On clear tout sauf le titre
            #endregion
            //Point de vie
            List<int> VieBase = new List<int>(){
                4, //Healer
                5, //Tank
                3, //Damager
                random.Next(1,6) // Lucker
                };

            int vieJoueur = VieBase[roleJoueur - 1];
            int vieIA = VieBase[roleIA - 1];

            bool FinDePartie = false;
            int nombreTour = 20;
            while (!FinDePartie)
            {
                Console.SetCursorPosition(0, 14);
                DisplayRoleVersus(roleJoueur, roleIA, nombreTour); //Affichage des ASCII des rôles + nombres de tour
                DisplayStats(vieJoueur, roleJoueur, vieIA, roleIA); //Affichage des coeurs et force d'attaque
                //Choix Action
                Console.WriteLine();
                string ChoixAction = "Actions possibles : ";
                List<string> actions = new List<string>(){
                    "1 - Attaquer",
                    "2 - Défendre",
                    "3 - Spécial"};
                int actionJoueur = PlayerChoice(ChoixAction, actions); //Choix du joueur pour l'action
                int actionIA;

                //Choix de l'IA en fonction de la difficulté choisie
                if (IAnumberChoice == 1)//mode aléatoire
                {
                    actionIA = IaRandomChoice(actions);
                }
                else if (IAnumberChoice == 2)//mode attaque only
                {
                    actionIA = 1;
                    Console.WriteLine("L'ordinateur a choisi {0}.", actions[0]);
                }
                else if (IAnumberChoice == 3)//mode normal
                {
                    actionIA = IaNormalChoice(actions, roleIA, roleJoueur, vieIA, vieJoueur);
                }
                else //mode difficile
                {
                    actionIA = IaDarkChoice(actions, roleIA, roleJoueur, vieIA, vieJoueur);
                }
                Tuple<int, int> TourUpdate = ResolutionAction(actionJoueur, roleJoueur, actionIA, roleIA); //Calcul de l'update à partir de Résolution action (textbook du PDF)
                vieJoueur = Math.Min(TourUpdate.Item1 + vieJoueur, 6); //On limite à 6 HP MAX pour le Healer/Lucker qui peuvent s'ajouter des pdv (on aurait pu rajouter une variable hpMax)
                vieIA = Math.Min(TourUpdate.Item2 + vieIA, 6);
                Console.WriteLine();
                Console.WriteLine("- Bilan du tour {0} -", 21 - nombreTour);
                //Affichage du bilan du tour
                if (roleJoueur == 1 && vieJoueur == 6 && actionJoueur == 3) Console.WriteLine("Joueur : {0}{1} ♥ (dans la limite de 6 points de vie)", TourUpdate.Item1 > 0 ? '+' : ' ', TourUpdate.Item1);
                else { Console.WriteLine("Joueur : {0}{1} ♥", TourUpdate.Item1 > 0 ? '+' : ' ', TourUpdate.Item1); }
                if (roleIA == 1 && vieIA == 6 && actionIA == 3) Console.WriteLine("Ordinateur : {0}{1} ♥ (dans la limite de 6 points de vie)", TourUpdate.Item2 > 0 ? '+' : ' ', TourUpdate.Item2);
                else { Console.WriteLine("Joueur : {0}{1} ♥", TourUpdate.Item2 > 0 ? '+' : ' ', TourUpdate.Item2); }
                nombreTour--; //nombre de tour qui s'actualise
                if (vieJoueur < 1 || vieIA < 1 || nombreTour < 1) FinDePartie = true; //sortie de boucle
            }
            Console.SetCursorPosition(0, 14);
            DisplayRoleVersus(roleJoueur, roleIA, nombreTour);
            DisplayStats(vieJoueur, roleJoueur, vieIA, roleIA);
            Console.WriteLine();
            Thread.Sleep(1500); //Simulation d'attente
            Endgame(vieJoueur, vieIA, nombreTour); //Affichage du résultat de fin de partir en fonction des PV des joueurs
        }
        static int RolesChoice(List<string> options) //Similaire à PlayerChoice mais adapté avec du nettoyage de Console pour la visibilité
        {
            string choice = Console.ReadLine();
            int numberChoice, numberOfTry = 0;
            while (!int.TryParse(choice, out numberChoice) || numberChoice > options.Count || numberChoice < 1)
            {
                if (numberOfTry > 2) //Lorsque le nombre d'essai est égal à 3 on nettoie la console (il faut arrêter de se tromper...)
                {
                    numberOfTry = 0;
                    ClearLastLines(11); //Enleve les 11 dernières lignes de la console
                    Console.Write("\n\n");
                }
                Console.WriteLine("'{1}' n'est pas valide. Veuillez choisir un choix possible (taper un chiffre de 1 à {0}) ", options.Count, choice);
                Console.WriteLine();
                choice = Console.ReadLine();
                numberOfTry++;
            }
            ClearLastLines(numberOfTry * 3 + 1); //On enleve les lignes des essais avec erreur
            Console.WriteLine(choice + "                       ");
            Console.WriteLine("\nVous avez choisi {0}.", options[numberChoice - 1]);
            return numberChoice;
        }
        static int PlayerChoice(string message, List<string> options)
        {

            #region Nettoyage de la console
            Console.SetCursorPosition(0, 33);
            Console.WriteLine("                                                                                                                                       ");
            Console.SetCursorPosition(0, 37);
            Console.WriteLine("                                                                                                                                       ");
            Console.SetCursorPosition(0, 33);
            Console.WriteLine(message);
            options.ForEach(x => Console.WriteLine(x));
            string choice = Console.ReadLine();
            int numberChoice;
            while (!int.TryParse(choice, out numberChoice) || numberChoice > options.Count || numberChoice < 1) //Tant que le choix n'est pas bon
            {
                Console.SetCursorPosition(0, 33);
                Console.WriteLine("Veuillez choisir un choix possible (taper un chiffre de 1 à {0}) ", options.Count);
                Console.SetCursorPosition(0, 37);
                Console.WriteLine("                                                                                                                                       ");
                Console.SetCursorPosition(0, 37);
                choice = Console.ReadLine();
                Console.WriteLine();
            }
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("                                                                                                                                       ");
            }
            Console.SetCursorPosition(0, 39);
            #endregion
            Console.WriteLine("Vous avez choisi {0}.", options[numberChoice - 1]);
            return numberChoice;
        } //Fonction pour le choix d'action
        static void Endgame(int vieJoueur, int vieIA, int tour) //Affichage du résultat de fin de jeu et retour au menu
        {
            Console.Clear();
            DisplayTitle();
            if (tour < 1)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(" _____ ________  ___ _____        _____ _   _ _____ ");
                Console.WriteLine("|_   _|_   _|  \\/  ||  ___|      |  _  | | | |_   _|");
                Console.WriteLine("  | |   | | | .  . || |__        | | | | | | | | |  ");
                Console.WriteLine("  | |   | | | |\\/| ||  __|       | | | | | | | | |  ");
                Console.WriteLine("  | |  _| |_| |  | || |___       \\ \\_/ / |_| | | |  ");
                Console.WriteLine("  \\_/  \\___/\\_|  |_/\\____/        \\___/ \\___/  \\_/  ");
            }
            else if (vieJoueur < 1 && vieIA < 1) //Match nul si entre-tués
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("__   __                                      _             _    _      ______  _____   ___  ______   _");
                Console.WriteLine("\\ \\ / /                                     | |           | |  | |     |  _  \\|  ___| / _ \\ |  _  \\ | |");
                Console.WriteLine(" \\ V /   ___   _   _     __ _  _ __   ___   | |__    ___  | |_ | |__   | | | || |__  / /_\\ \\| | | | | |");
                Console.WriteLine("  \\ /   / _ \\ | | | |   / _` || '__| / _ \\  | '_ \\  / _ \\ | __|| '_ \\  | | | ||  __| |  _  || | | | | |");
                Console.WriteLine("  | |  | (_) || |_| |  | (_| || |   |  __/  | |_) || (_) || |_ | | | | | |/ / | |___ | | | || |/ /  |_|");
                Console.WriteLine("  \\_/   \\___/  \\__,_|   \\__,_||_|    \\___|  |_.__/  \\___/  \\__||_| |_| |___/  \\____/ \\_| |_/|___/   (_)");
            }
            else if (vieJoueur < 1) //Défaite si notre vie est inférieure ou égale à 0
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("__   _______ _   _      _     _____ _____ _____ ");
                Console.WriteLine("\\ \\ / /  _  | | | |    | |   |  _  /  ___|  ___|");
                Console.WriteLine(" \\ V /| | | | | | |    | |   | | | \\ `--.| |__  ");
                Console.WriteLine("  \\ / | | | | | | |    | |   | | | |`--. \\  __| ");
                Console.WriteLine("  | | \\ \\_/ / |_| |    | |___\\ \\_/ /\\__/ / |___ ");
                Console.WriteLine("  \\_/  \\___/ \\___/     \\_____/\\___/\\____/\\____/ ");
            }
            else //Victoire
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("__   __                 _    _  _____  _   _   _");
                Console.WriteLine("\\ \\ / /                | |  | ||_   _|| \\ | | | |");
                Console.WriteLine(" \\ V /   ___   _   _   | |  | |  | |  |  \\| | | |");
                Console.WriteLine("  \\ /   / _ \\ | | | |  | |/\\| |  | |  | . ` | | |");
                Console.WriteLine("  | |  | (_) || |_| |  \\  /\\  / _| |_ | |\\  | |_|");
                Console.WriteLine("  \\_/   \\___/  \\__,_|   \\/  \\/  \\___/ \\_| \\_/ (_)");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Thread.Sleep(4000);
            Console.Clear();
        }
        static int IaRandomChoice(List<string> options)  //Faire le choix aléatoire du role par l'ordinateur
        {
            Random random = new Random();
            int rnd = random.Next(1, options.Count + 1);
            Console.WriteLine("L'ordinateur a choisi {0}.", options[rnd - 1]); //Chaque choix se fait à partir de 1 mais les listes sont initialisés à partir de 0
            return rnd;
        }
        static int IaNormalChoice(List<string> options, int roleIA, int roleJoueur, int vieIA, int vieJoueur)  //Choix de l'ordinater en mode normal
        {
            if (roleIA == 1) //Si le bot est healer
            {
                if (vieJoueur == 1) { Console.WriteLine("L'ordinateur a choisi {0}.", options[0]); return 1; } //Achever l'adversaire en priorité 
                else if (vieIA <= 2) { Console.WriteLine("L'ordinateur a choisi {0}.", options[2]); return 3; } //n'a que 2 pv ou moins se soigner
                else { Console.WriteLine("L'ordinateur a choisi {0}.", options[0]); return 1; } //sinon attaquer à chaque fois
            }
            else if (roleIA == 2) //Si le bot est tank 
            {
                if (vieJoueur == 1) { Console.WriteLine("L'ordinateur a choisi {0}.", options[0]); return 1; } //Achever l'adversaire en priorité 
                else if (vieIA >= 4) { Console.WriteLine("L'ordinateur a choisi {0}.", options[2]); return 3; } //Se donne des dégats s'il a suffisament de PV
                else { Console.WriteLine("L'ordinateur a choisi {0}.", options[0]); return 1; } //Sinon attaque à chaque fois
            }
            else if (roleIA == 3) //Si le bot est damager
            {
                if (vieJoueur == 2) { Console.WriteLine("L'ordinateur a choisi {0}.", options[0]); return 1; } //Achever l'adversaire en priorité 
                else if (vieIA == 3) { Console.WriteLine("L'ordinateur a choisi {0}.", options[2]); return 3; }//Utilise son spécial car peut se permettre de perdre de la vie
                else { Console.WriteLine("L'ordinateur a choisi {0}.", options[0]); return 1; } //Sinon attaque à chaque fois 
            }
            else //Si le bot est lucker
            {
                if (vieJoueur == 1) { Console.WriteLine("L'ordinateur a choisi {0}.", options[0]); return 1; } //Achever l'adversaire en priorité 
                else if (vieIA == 1) { Console.WriteLine("L'ordinateur a choisi {0}.", options[2]); return 3; } //Fait son spécial quand 1PV histoire de pouvoir régénérer
                else { Console.WriteLine("L'ordinateur a choisi {0}.", options[0]); return 1; } //Sinon attaque à chaque fois
            }
        }
        static int IaDarkChoice(List<string> options, int roleIA, int roleJoueur, int vieIA, int vieJoueur, int simu = -1) //Choix d l'ordinateur en difficile
        {
            if (simu == -1)
            {
                if (roleIA == 1) //Si le bot est healer
                {
                    if (vieJoueur == 1) { Console.WriteLine("L'ordinateur a choisi {0}.", options[0]); return 1; } //Achever l'adversaire en priorité 
                    else if (vieIA <= 2) { Console.WriteLine("L'ordinateur a choisi {0}.", options[2]); return 3; } //n'a que 2 pv ou moins se soigner
                    else { Console.WriteLine("L'ordinateur a choisi {0}.", options[0]); return 1; } //sinon attaquer à chaque fois
                }
                else if (roleIA == 2) //Si le bot est tank 
                {
                    if (vieJoueur == 1) { Console.WriteLine("L'ordinateur a choisi {0}.", options[0]); return 1; } //Achever l'adversaire en priorité 
                    else if ((roleJoueur == 1 || roleJoueur == 4) && vieJoueur == 2 && vieIA == 2) { Console.WriteLine("L'ordinateur a choisi {0}.", options[2]); return 3; }
                    else if (vieIA >= 4) { Console.WriteLine("L'ordinateur a choisi {0}.", options[2]); return 3; } //Se donne des dégats s'il a suffisament de PV
                    else { Console.WriteLine("L'ordinateur a choisi {0}.", options[0]); return 1; } //Sinon attaque à chaque fois
                }
                else if (roleIA == 3) { Console.WriteLine("L'ordinateur a choisi {0}.", options[0]); return 1; }//Si le bot est damager
                else //Si le bot est lucker
                {
                    if (vieJoueur == 1) { Console.WriteLine("L'ordinateur a choisi {0}.", options[0]); return 1; } //Achever l'adversaire en priorité 
                    else { Console.WriteLine("L'ordinateur a choisi {0}.", options[2]); return 3; } //Sinon attaque à chaque fois
                }
            }
            else
            {
                if (roleIA == 1) //Si le bot est healer
                {
                    if (vieJoueur == 1) { return 1; } //Achever l'adversaire en priorité 
                    else if (vieIA <= 2) { return 3; } //n'a que 2 pv ou moins se soigner
                    else { return 1; } //sinon attaquer a chaque fois
                }
                else if (roleIA == 2) //Si le bot est tank 
                {
                    if (vieJoueur == 1) { return 1; } //Achever l'adversaire en priorité 
                    else if ((roleJoueur == 1 || roleJoueur == 4) && vieJoueur == 2 && vieIA == 2) return 3;
                    else if (vieIA >= 4) { return 3; } //Se donne des dégats si il a suffisament de PV
                    else { return 1; } //Sinon attaque a chaque fois
                }
                else if (roleIA == 3) { return 1; }//Si le bot est damager
                else //Si le bot est lucker
                {
                    if (vieJoueur == 1) { return 1; } //Achever l'adversaire en priorité 
                    else { return 3; } //Sinon attaque a chaque fois
                }
            }

        }
        static int DommageParRole(int role)  //Trouver les dégats en fonction du role
        {
            Random random = new Random();
            var charactersDM = new Dictionary<int, int>() { { 1, 1 }, { 2, 1 }, { 3, 2 }, { 4, random.Next(1, 3) } }; //Dommage aléatoire pour le Lucker de 1 à 2 recalculé à chaque appel
            return charactersDM[role];
        }
        static Tuple<int, int> ResolutionAction(int actionJoueur, int roleJoueur, int actionIA, int roleIA) //Réalisation/bilan des actions 
        {
            Random random = new Random();
            int Lucker = random.Next(0, 2), Lucker2 = random.Next(0, 2);
            if (actionJoueur == 1) //Le joueur attaque
            {
                if (actionIA == 1) //Si l'IA attaque, les 2 joueurs vont perdres des pdv égaux aux forces d'attaques respectives
                {
                    int att1 = DommageParRole(roleIA), att2 = DommageParRole(roleJoueur);
                    Console.WriteLine("BAGARRE !! Vous perdez {0} de vie et l'ordinateur en perd {1}. \n", att1, att2);
                    return new Tuple<int, int>(-att1, -att2);
                }
                else if (actionIA == 2) // L'IA défend il ne se passe rien
                {
                    Console.WriteLine("L'ordinateur choisit lâchement la défense et contre vos attaques.");
                    return new Tuple<int, int>(0, 0);
                }
                else //Action spéciale IA
                {
                    if (roleIA == 1) return new Tuple<int, int>(0, -DommageParRole(roleJoueur) + 2); //Le healer se heal de 2 et se prends nos dégats
                    else if (roleIA == 2) return new Tuple<int, int>(-2, -DommageParRole(roleJoueur) - 1); //Le tank prend nos dégats + les 1 de son attaque spéciale. On subit 2
                    else if (roleIA == 3) return new Tuple<int, int>(-DommageParRole(roleJoueur), -DommageParRole(roleJoueur)); //On inflige des dégats et on se prend les dégats de la Rage
                    else
                    {
                        if (Lucker2 == 0) //Si l'IA a de la chance
                        {
                            Console.WriteLine("L'ordinateur a de la chance! Il vous attaque et se soigne.");
                            return new Tuple<int, int>(-DommageParRole(roleIA), -DommageParRole(roleJoueur) + 1);
                        }
                        else //L'IA a pas de chance il ne fait rien 
                        {
                            Console.WriteLine("L'ordinateur n'a pas de chance.");
                            return new Tuple<int, int>(0, -DommageParRole(roleJoueur));
                        }
                    }
                }
            }
            else if (actionJoueur == 2) //Si on se défend
            {
                if (actionIA == 1 || actionIA == 2)
                {
                    Console.WriteLine("Arrête de te défendre et bats toi... Aucune goutte de sang n'a été versée.");
                    return new Tuple<int, int>(0, 0); //il ne se passe rien si l'IA attaque ou se défend
                }
                else
                {
                    if (roleIA == 1) return new Tuple<int, int>(0, 2); //L'IA se heal de 2
                    else if (roleIA == 2) return new Tuple<int, int>(-1, -1); //L'IA fait son attaque spéciale qui traverse notre défense pour 1. Il recoit 1 de son attaque spéciale
                    else if (roleIA == 3) return new Tuple<int, int>(0, 0); //il ne se passe rien car on n'attaque pas
                    else
                    {
                        if (Lucker2 == 0) //Si l'IA a de la chance
                        {
                            Console.WriteLine("L'ordinateur a de la chance! Il se soigne.");
                            return new Tuple<int, int>(0, 1);
                        }
                        else
                        {
                            Console.WriteLine("L'ordinateur n'a pas de chance.");
                            return new Tuple<int, int>(0, 0);
                        }
                    }
                }
            }
            else
            {
                if (roleJoueur == 1) //Le joueur joue le healer
                {
                    if (actionIA == 1) return new Tuple<int, int>(-DommageParRole(roleIA) + 2, 0);
                    else if (actionIA == 2) return new Tuple<int, int>(2, 0);
                    else
                    {
                        if (roleIA == 1) return new Tuple<int, int>(2, 2);
                        else if (roleIA == 2) return new Tuple<int, int>(0, -1);
                        else if (roleIA == 3) return new Tuple<int, int>(2, 0);
                        else
                        {
                            if (Lucker2 == 0)
                            {
                                Console.WriteLine("L'ordinateur a de la chance! Il vous attaque et se soigne.");
                                return new Tuple<int, int>(-DommageParRole(roleIA) + 2, 1);
                            }
                            else
                            {
                                Console.WriteLine("L'ordinateur n'a pas de chance.");
                                return new Tuple<int, int>(2, 0);
                            }
                        }
                    }

                }
                else if (roleJoueur == 2) //Le joueur joue le tank
                {
                    if (actionIA == 1) return new Tuple<int, int>(-DommageParRole(roleIA) - 1, -2);
                    else if (actionIA == 2) return new Tuple<int, int>(-1, -1);
                    else
                    {
                        if (roleIA == 1) return new Tuple<int, int>(-1, 0);
                        else if (roleIA == 2) return new Tuple<int, int>(-3, -3);
                        else if (roleIA == 3) return new Tuple<int, int>(-3, -2);
                        else
                        {
                            if (Lucker2 == 0)
                            {
                                Console.WriteLine("L'ordinateur a de la chance! Il vous attaque et se soigne.");
                                return new Tuple<int, int>(-DommageParRole(roleIA) - 1, -1);
                            }
                            else
                            {
                                Console.WriteLine("L'ordinateur n'a pas de chance.");
                                return new Tuple<int, int>(-1, -2);
                            }
                        }
                    }
                }
                else if (roleJoueur == 3) //Le joueur joue le damager
                {
                    if (actionIA == 1) return new Tuple<int, int>(-DommageParRole(roleIA), -DommageParRole(roleIA));
                    else if (actionIA == 2) return new Tuple<int, int>(0, 0);
                    else
                    {
                        if (roleIA == 1) return new Tuple<int, int>(0, 2);
                        else if (roleIA == 2) return new Tuple<int, int>(-2, -3);
                        else if (roleIA == 3) return new Tuple<int, int>(0, 0);
                        else
                        {
                            if (Lucker2 == 0)
                            {

                                Console.WriteLine("L'ordinateur a de la chance! Il vous attaque et se soigne.");
                                int temp = DommageParRole(roleIA);
                                return new Tuple<int, int>(-temp, 1 - temp);
                            }
                            else
                            {
                                Console.WriteLine("L'ordinateur n'a pas de chance.");
                                return new Tuple<int, int>(0, 0);
                            }
                        }
                    }
                }
                else //Le joueur joue le Lucker
                {
                    if (actionIA == 1) return new Tuple<int, int>(-DommageParRole(roleIA), -DommageParRole(roleJoueur));
                    else if (actionIA == 2) return new Tuple<int, int>(0, 0);
                    else
                    {
                        if (Lucker == 0)
                        {
                            if (roleIA == 1) return new Tuple<int, int>(1, 2 - DommageParRole(roleJoueur));
                            else if (roleIA == 2) return new Tuple<int, int>(-1, -DommageParRole(roleJoueur - 1));
                            else if (roleIA == 3) return new Tuple<int, int>(-DommageParRole(roleJoueur) + 1, -DommageParRole(roleJoueur));
                            else
                            {
                                if (Lucker2 == 0)
                                {
                                    Console.WriteLine("L'ordinateur a de la chance mais vous aussi! Vous effectuez chacun une attaque double ! ");
                                    int temp = DommageParRole(roleJoueur);
                                    int temp2 = DommageParRole(roleIA);
                                    return new Tuple<int, int>(1 - temp2, 1 - temp);
                                }
                                else
                                {
                                    Console.WriteLine("Vous avez de la chance et l'ordinateur n'en a pas ! Vous l'attaquez et vous vous soignez.");
                                    return new Tuple<int, int>(1, -DommageParRole(roleJoueur));
                                }
                            }
                        }
                        else
                        {
                            if (roleIA == 1) return new Tuple<int, int>(0, 2);
                            else if (roleIA == 2) return new Tuple<int, int>(-2, -1);
                            else if (roleIA == 3) return new Tuple<int, int>(0, 0);
                            else
                            {
                                if (Lucker2 == 0)
                                {
                                    Console.WriteLine("L'ordinateur a de la chance mais pas vous! Il vous attaque et se soigne.");
                                    return new Tuple<int, int>(-DommageParRole(roleIA), 1);
                                }
                                else
                                {
                                    Console.WriteLine("L'ordinateur tout comme vous n'avez pas de chance.");
                                    return new Tuple<int, int>(0, 0);
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
