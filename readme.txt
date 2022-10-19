+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
     _   _ _    _____ ________  ___  ___ _____ _____           
    | | | | |  |_   _|_   _|  \/  | / _ \_   _|  ___|          
    | | | | |    | |   | | | .  . |/ /_\ \| | | |__            
    | | | | |    | |   | | | |\/| ||  _  || | |  __|           
    | |_| | |____| |  _| |_| |  | || | | || | | |___           
     \___/\_____/\_/  \___/\_|  |_/\_| |_/\_/ \____/           
                                                               
                                                               
   ______ _____ _____  _   _ _____ _   _ _____ _   _ _____     
   |  ___|_   _|  __ \| | | |_   _| \ | |_   _| \ | |  __ \    
   | |_    | | | |  \/| |_| | | | |  \| | | | |  \| | |  \/    
   |  _|   | | | | __ |  _  | | | | . ` | | | | . ` | | __     
   | |    _| |_| |_\ \| | | | | | | |\  |_| |_| |\  | |_\ \    
   \_|    \___/ \____/\_| |_/ \_/ \_| \_/\___/\_| \_/\____/    
                                                               
                                                               
     _____ ________  ____   _ _       ___ _____ ___________    
    /  ___|_   _|  \/  | | | | |     / _ \_   _|  _  | ___ \   
    \ `--.  | | | .  . | | | | |    / /_\ \| | | | | | |_/ /   
     `--. \ | | | |\/| | | | | |    |  _  || | | | | |    /    
    /\__/ /_| |_| |  | | |_| | |____| | | || | \ \_/ / |\ \    
    \____/ \___/\_|  |_/\___/\_____/\_| |_/\_/  \___/\_| \_|   
                                                               
+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


+++++++++++++++++++++++++++++++++++++++++++++++
+               CONTRÔLE DU JEU               +
+++++++++++++++++++++++++++++++++++++++++++++++


Rentrer en input des chiffres (pavé numérique puis Entrée) selon la liste des choix disponibles.
Il vous sera demandé de retaper un chiffre si votre input n'est pas conforme à la liste de choix valides.

+++++++++++++++++++++++++++++++++++++++++++++++
+                   MENU                      +
+++++++++++++++++++++++++++++++++++++++++++++++


Joueur VS Ordinateur : Plusieurs niveaux de difficultés de l'IA 
	- Aléatoire : actions choisies aléatoirement par les ordinateurs
	- Attaque uniquement : Les ordinateurs ne font qu'attaquer
	- Normal : les actions choisies par les ordinateurs sont adaptés à la situation (points de vie actuels)
	- Difficile : les actions choisies par les ordinateurs sont adaptés à la situation (rôles des joueurs + points de vie actuels)
	
Simulation : Simulations de 100 parties Ordinateur VS Ordinateur. 
Un tableau récapitulatif des résultats est affiché et actualisé en temps réel.
Les taux de victoires prennent en compte les matchs nuls et les time out. Vous avez comme choix de simulation :
	- Aléatoire : actions choisies aléatoirement par les ordinateurs
	- Attaque uniquement : Les ordinateurs ne font qu'attaquer
	- Difficile : les actions choisies par les ordinateurs sont adaptés à la situation (rôles des joueurs + points de vie actuels)

Quitter le jeu : Message d'aurevoir avec crédits.


+++++++++++++++++++++++++++++++++++++++++++++++
+         FONCTIONNEMENT D'UNE PARTIE         +
+++++++++++++++++++++++++++++++++++++++++++++++

Choix du rôle : 4 rôles disponibles avec des statistiques différentes (Healer, Tank, Damager, Lucker). 
Chaque rôle possède des caractéristiques différentes.

A chaque tour :
	- Choix d'une action parmi les 3 possibles.
	- Les 3 actions possibles : - Attaquer -> inflige des dégâts à hauteur de la force d'attaque
				    - Défendre -> bloque les dégâts en cas d'attaque ennemi
                                    - Spéciale -> effet spécifique selon le rôle choisi
	- Les points de vie du Healer et du Lucker sont limités à 6 (étant les seules classes pouvant s'ajouter des points de vie, nous limitons cette mécanique). 
	- La mise à jour de l'état du jeu se fait une fois les deux actions choisies et se fait en simultanée (possibilité de double KO !).
	- La partie se termine lorsque les points de vie du joueur ou de l'ordinateur tombe à 0.
        - Limite de 20 tours en cas d'anti jeu (exemple d'un joueur prenant uniquement la défense) --> se termine par une égalité.

Resultat : Un message de fin est affiché selon le résultat de la partie, vous êtes ensuite renvoyé automatiquement au menu.



+++++++++++++++++++++++++++++++++++++++++++++++
+                 AFFICHAGE                   +
+++++++++++++++++++++++++++++++++++++++++++++++

Le buffer pour la taille de l'écran a été adapté pour que le jeu Console reste lisible sur n'importe quel écran (sur les petits écrans il suffira de dérouler vers la gauche/droite ou vers le bas/haut).
Il est recommandé de laisser ensuite la console en taille maximale et ne pas modifier sa taille.



QUOC-BAO NGUYEN
SULIVAN MICHALON



                                           
