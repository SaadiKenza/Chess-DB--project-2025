## Etudiantes:

- Saadi Kenza :23268
- Umme Kulsum :22156


## Projet_2025_Chess_DB

Le but du projet est de réaliser une application desktop de gestion de matchs d'une fédération d'échecs.<br>
Dans cette application, on retrouve trois fenêtres : <br>
1. Le *Dashboard* : la page d'accueil du gestionnaire<br>
2. *Player* : pour la gestion des joueurs (inscription, modification des informations ainsi que la suppression d'un joueur). Il est a noté que la colonne *Elo* du tableau fait apparaitre une flèche, au survol permettant de classer les joueurs dans l'odre croissant ou décroissant, de faire un classement Elo. <br>
3. *Tournament* : contient une partie de gestion de tournois (création, modification et suppression des tournois) et une partie d'ajout de compétitions.<br>
Pour pouvoir ajouter un joueur dans un tournoi, on clique sur le bouton jaune et une petite fenêtre apparait. Sur celle-ci, on insére le matricule du joueur qui participe au tournoi et on voit la liste des matricules des joueurs déjà inscrit. On dispose aussi d'une barre de recherche pour faciliter la gestion des joueurs inscrits.<br>
Pour pouvoir ajouter une compétition, il faut d'abord sélectionner le tournois où a été joué la compétition. En sélectionnant un tournoi, la liste des compétitions apparait.<br>

Le projet a été réalisé en ¨*C#* avec une base de données en *SQLite* ainsi qu'une interface graphique *Avalonia UI*.


## Architecture et fonctionnement général

On retrouve dans le projet un modèle *MVVM* :
- *Models* : fichiers qui servent d'empreinte, de structure des différents entités du projet.
- *Views* : fichiers qui représentent les différentes fenêtres de l'application. Elles gérent l'affichage (".axaml") et l'interaction avec l'utilisateur (".axaml.cs").
- *ViewsModels* : fichiers qui font le lien entre les interactions depuis l'UI et les données (SQLite).

On y retrouve aussi un dossier *Services* qui contient les différents fichiers qui servent à la base de données. Chaque fichier a une responsabilité, ne gère qu'une tache. Par exemple, le fichier *DatabaseService.cs* gère la création des différentes tables de la base de données.

Le fonctionnement est le suivant: 
L'utilisateur insére quelque chose depuis l'UI. Le fichier *View* conserné transmet cette information au *ViewModel* approprié et celui-ci va à son tour trasmettre cette information au fichier *Service* correspondant pour effectuer les changements dans la base de données. <br>
Une fois le changement dans la base de données effectué, on fait le chemin inverse pour afficher les modifications apportés.

## Description de la fonctionnalité supplémentaire:

La fonctionnalité supplémentaire que l'on va décrire est la barre de recherche. Il y en a une dans *Player*, une dans *Tournament* et une dans la petite fenêtre secondaire d'ajout d'un joueur à un tournoi. <br>
Cette barre sert à filtrer les tableaux de données pour faciliter la gestion. Il est à noter que la recherche ne se fait que par la clé primaire du tableau : le matricule pour les joueurs et le nom du tournoi pour les tournois. <br>
Par exemple, si l'on veut modifier une informations personnelles d'un joueur ou le supprimer, on n'a qu'à le rechercher par son matricule dans la barre de recherche. Cela rend l'utilisation de l'application plus dynamique et la gestion plus facile et rapide.


## Principes SOLID utilisés:

Le premier principe utilisé est le *Single Responsibility Principle*. <br>
Ce principe stipule que chaque classe ne doit avoir qu'une seule responsabilité. Notre projet respecte bien ce principe puisque chaque rôle est attribué à un seul fichier.
Ce principe a brièvement été justifié dans la section *Architecture et fonctionnement général*. <br>
Prenons l'exemple de la fenêtre *Player* et gestion des joueurs. Chaque fichier, de même dossier ou différent, joue un rôle propre :
- Dans *Models*, *Player.Cs" donne la structure de création d'un joueur. C'est la façon dont il est représenté dans le projet.
- Dans *Views*, on a deux fichiers : *PlayerPageView.axaml* qui gère l'UI de la page *Player* et *PlayerPageView.axaml.cs* qui gère ses interactions avec l'utilisateur (quoi faire lorsqu'on apporte une modification à un tableau ou un clic par exemple).
- Dans *ViewModels*, le fichier *PlayerPageViewModel.cs" a le rôle d'intermédiaire entre l'UI de la page *Player* et sa base de données.
- Dans *Services*, le fichier *PlayerRepository.cs* contient tous les actions a réaliser dans la table *Players* de la base des données (AddPlayer, DeletePlayer, UpdatePlayerElo, etc.).<br>

Chaque fichier a une responsabilité définie. On ne mélange pas les différentes actions si elles n'ont pas de même rôle. <br>

Le deuxième principe est le *Open/Closed Principle*. <br>
Avec ce principe, on doit pouvoir ajouter un nouveau bout de code sans que l'ancien ne soit modifié. 
Grâce au principe *Single Responsibility Principle* et à l'architecture MVVM, chaque fichier ne s'occupe que d'un seul rôle et permet de facilement ajouter du code sans casser l'existant.
Par exemple, après la création de la table *Players* dans la base de données, la fonctionnalité supprimer a pu être ajoutée. Celle-ci a pu être implémentée dans le fichier *PlayerRepository.cs* sans apporter de modifications du code déjà existant. <br>
De plus, si l'on souhaite ajouter une nouvelle fenêtre, créer les fichiers MVVM ainsi que la base de données ne modifie pas les pages existantes.

## Qualité d'adaptabilité à une autre fédération:

Le projet a été réalisé de manière à pouvoir être adapté à d'autres fédérations sportives.
L'UI repose sur des concepts communs à nombreuses fédérations : inscription de joueurs, création de tournois, les compétitions. La structure générale de l'UI peut donc être conservée, avec quelques adaptations. 
Par exemple, remplacer le mode de calculs des scores (propres à chaque sport) est réalisable facilement grâce à l'indépendance des fichiers. En effet, comme chaque fichier a une responsabilité propre, il suffit d'aller changer le mode de calculs dans le fichier correspondant sans que ca n'impacte toute l'architecture. 

## Diagramme de classes:
<img width="1240" height="610" alt="image" src="https://github.com/user-attachments/assets/ba5c02f1-2eaa-4bbb-bf47-7a50da259f2b" />


## Diagramme de séquences:
<img width="1329" height="805" alt="image" src="https://github.com/user-attachments/assets/dc6587fc-b890-41a9-8df9-684b8afba366" />


## Diagramme d'activité:


## Conclusion:








