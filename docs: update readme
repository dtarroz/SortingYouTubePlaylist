# Sorting YouTube Playlist

Tri des listes de lecture YouTube (playlists) par leur durée et leur date d'ajout selon un algorithme particulier.

## Sommaire
- [Pourquoi faire ?](#pourquoi-faire-)
- [Mon algorithme de trie](#mon-algorithme-de-trie)
- [Comment récupérer ses informations Google Client ID et Client Secret ?](#comment-récupérer-ses-informations-google-client-id-et-client-secret-)
- [Connexion à son compte Youtube](#connexion-à-son-compte-youtube)
- [Tri de votre liste de lecture (playlist)](#tri-de-votre-liste-de-lecture-playlist)
- [Lister les vidéos de la liste de lecture (playlist)](#lister-les-vidéos-de-la-liste-de-lecture-playlist)
- [Comment je m’en sers ?](#comment-je-m-en-sers-)
- [FAQ](#faq)

## Pourquoi faire ?
Pendant longtemps, je me servais de YouTube seulement pour regarder les vidéos sur un sujet particulier et sur l’instant. Puis j’ai commencé à suivre des chaines intéressantes et j'arrivais à regarder leurs dernières vidéos facilement. Quand mon nombre de chaines à suivre est devenu plus importante, là je me suis fait submerger par les vidéos et j’ai débuté mon utilisation du bouton “A regarder plus tard”.

L’avantage que j'ai trouvé à ces playlists, c’est la continuité de l’enchaînement des vidéos que j’ai choisi préalablement.

Mon utilisation primaire était de lire la première vidéo de la liste puis enchaîner, mais des inconvénients sont très vite arrivés. Si j’avais ajouté beaucoup de vidéos d’une nouvelle chaîne ou bien plusieurs conférences, car le « week-end » de évènement venait de passer, j’avais à la suite des vidéos de la même chaîne ou des vidéos qui durent plus d’une heure. Un autre inconvénient était que je voyais le nombre de vidéos dans la playlist augmenter. 

Alors je suis parti sur un nettoyage des vidéos par leur durée, j’ai regardé toutes les vidéos les plus courtes et pour éviter de parcourir toute la liste pour chercher la plus courte, j’ai fait par tranche de 5 min. Je parcours la liste et je regarde toutes les vidéos de moins de 5 min, arrivé à la fin, je regarde toutes les vidéos de moins de 10 min et ensuite de suite. Ok je diminue le nombre de vidéos mais celles qui ont une durée parmi les plus élevées ne sont jamais lues. Solution, dès que je reprend depuis le début je lis la première de la playlist. Mais si on ajoute régulièrement des vidéos assez courtes, on n'avance pas dans la lecture des vidéos dont la durée seraient plus longues. 

Après plusieurs mois, ça devenait lassant de toujours s’arrêter après une vidéo pour choisir manuellement la suivante, c’est pour cette raison que j’ai étudié la possibilité d’automatiser tout ça.

J’ai codé le procédé précédent pour test et je suis tombé sur d’autres points à améliorer.

## Mon algorithme de trie
Après plusieurs expérimentations, voici mon algorithme de trie actuel.

Le principe de base repose sur la succession de plusieurs groupes de 6 vidéos (```ABCDE```). Chacune de ces vidéos est déterminée par un choix algorithmique selon leur ordre parmi ces 5. 

- Vidéo ```A```
    - Prendre la vidéo la plus courte ajoutée il y a plus d’un mois dans la liste de lecture
    - Sinon aucune vidéo
- Vidéo ```B```
    - Prendre la vidéo la plus courte
- Vidéo ```C```
    - Prendre la vidéo la plus courte
- Vidéo ```D```
    - Prendre la vidéo la plus courte
- Vidéo ```E```
    - Prendre la vidéo la plus ancienne ajoutée dans la liste de lecture

Cela permet de regarder les plus courtes pour diminuer rapidement le nombre de vidéos dans la playlist (```B```, ```C``` et ```D```), de diminuer les vidéos qui sont présentes de plus d’un mois (```A```) et les plus anciennes qui peuvent avoir des durées plus longues (```E```).

**Des contraintes supplémentaires ont été ajoutées :**

- Les vidéos d’une même chaîne ne se suivent pas dans un groupe mais aussi sur la succession des groupes.
- S’il ne reste que des vidéos d’une même chaîne à trier, on prend la plus courte pour les vidéos ```B```, ```C``` et ```D```, et la plus ancienne ajoutée pour la vidéo ```E```. La vidéo ```A``` n'est pas utilisé pour ce cas.
- Les 3 premières vidéos de la playlist ne sont pas triées. C’est pour éviter de déplacer la vidéo en cours de lecture et des vidéos qui nécessitent une lecture plus confortable _(visionnage sur un autre écran, ne pas être interrompu pendant la lecture, vidéo dans une langue étrangère (sous titre), …)_
- Si les vidéos qui suivent la zone qui ne bouge pas _(les 3 premiéres vidéos)_ sont du même groupe que la troisiéme vidéo, alors on ne touche pas à leur ordre. C’est pour éviter d’avoir toujours une vidéo ```A``` qui entre dans la zone si on ne regarde qu’une vidéo entre chaque trie.
- Comme il faut au minimum 2 vidéos pour trier et que les 3 premières ne bougent pas, le trie ne fonctionne que si la playlist contient plus de 5 vidéos.

Pour savoir si une vidéo est dans le même groupe qu’une autre, j’utilise une fonctionnalité disparue de l’interface YouTube qui permettait de saisir une « note » sur une vidéo d’une playlist. Cette note est toujours accessible depuis l’API donc je m’en sers pour stocker l’identifiant unique du groupe à laquelle elle appartient.

## Comment récupérer ses informations Google Client ID et Client Secret ?
Vous avez besoin des informations Google Client ID et Client Secret pour utiliser l’API de Youtube. Vous devez vous créer un compte ([Google Developers Console](https://console.developers.google.com/apis)) et le configurer pour pouvoir télécharger le fichier « client_secret.json »

## Connexion à son compte Youtube
Pour se connecter à son compte YouTube. Lancer la commande suivante :

```cmd
SortingYoutubePlaylist authorize
```

Il va ouvrir une page dans le navigateur pour vous demandez de vous connecter à votre compte YouTube. 
Après la connexion, vous pouvez fermer la page du navigateur. Vous aurez vos informations (tokens) de connexion qui serviront ensuite pour trier vos listes de lecture. Par défaut, il sera contenu dans le répertoire ```youtube-data-store```.

**Paramètres complémentaires :**
 - -c : le nom du fichier contenant votre Google Client ID et Client Secret. Par défaut la valeur est ```client_secret.json```.
 - -d : le nom du répertoire qui va contenir vos informations de compte YouTube après la connexion. Par défaut la valeur est ```youtube-data-store```.

## Tri de votre liste de lecture (playlist)
Premièrement, vous avez besoin de l’identifiant de votre liste de lecture.
Pour faire simple, aller depuis un navigateur sur votre liste de lecture et récupérer la valeur dans l’URL qui correspond au paramètre ```list```. 

Vous devez préalablement vous connecter à votre compte YouTube avant de procéder au trie ([Connexion à son compte Youtube](#connexion-à-son-compte-youtube)).

Lancer la commande suivante :

```cmd
SortingYoutubePlaylist sort -p <MON_ID_PLAYLIST>
```

Il va récupérer les informations de toutes les vidéos de la liste de lecture puis il va trier ses vidéos selon l’algorithme expliqué précédemment.  
Aucune vidéo ne sera supprimé pendant le processus, seulement l’ordre d’affichage « manuel ». 

**Paramètres complémentaires :**
 - -c : le nom du fichier contenant votre Google Client ID et Client Secret. Par défaut la valeur est ```client_secret.json```.
 - -d : le nom du répertoire contenant vos informations de compte YouTube. Par défaut la valeur est ```youtube-data-store```.

## Lister les vidéos de la liste de lecture (playlist)
Premièrement, vous avez besoin de l’identifiant de votre liste de lecture.
Pour faire simple, aller depuis un navigateur sur votre liste de lecture et récupérer la valeur dans l’URL qui correspond au paramètre ```list```. 

Vous devez préalablement vous connecter à votre compte YouTube avant de procéder au trie ([Connexion à son compte Youtube](#connexion-à-son-compte-youtube)).

Lancer la commande suivante :

```cmd
SortingYoutubePlaylist list -p <MON_ID_PLAYLIST>
```

Il va récupérer les informations de toutes les vidéos de la liste de lecture puis il va afficher des informations simplifiées de celles ci. 

**Paramètres complémentaires :**
 - -c : le nom du fichier contenant votre Google Client ID et Client Secret. Par défaut la valeur est ```client_secret.json```.
 - -d : le nom du répertoire contenant vos informations de compte YouTube. Par défaut la valeur est ```youtube-data-store```.

## Comment je m’en sers ?
- je me suis crée un compte Google Developper avec une application de test, juste pour avoir le fichier ```client_secret.json```. C’est juste pour un usage personnel.
- J’ai lancé la connexion à mon compte YouTube depuis mon poste de travail pour récupérer les tokens de connexion (```youtube-data-store```).
- J’ai configuré Github Actions pour qu’il compile et exécute l’application pour lancer automatiquement le trie de mes listes de lecture durant chaque nuit.
- J’utilise les Github Secrets pour contenir le fichier ```client_secret.json```, les tokens de connexions (```youtube-data-store```) et les identifiants des listes de lectures.
- A ce stade, il me trie mes listes de lecture chaque nuit automatiquement mais que pendant 7 jours car j’ai une configuration de « Test » pour l’API YouTube. Solution pour mettre à jour les tokens de connexion chaque semaine : Utiliser Raccourcie d'iOS pour me connecter de nouveau à mon compte YouTube et mettre à jour le Github Secret associé.

## FAQ
**- On peut trier la liste de lecture “A regarder plus tard” ?**
 
> Non, l’API Youtube ne permet pas de récupérer et de modifier la liste de lecture “A regarder plus tard”.

**- Pour quel type de chaîne cet algorithme de trie n’est pas adapté ?**

> Cet algorithme de trie n’est pas adapté pour les types de chaînes suivantes :
> 
> - Celles qui font du contenu qui sont à suivre dans un ordre précis
> - Celles dont on regarde « rapidement » les contenu postés (_seulement si la playlist à trier possède énormément de vidéos_)

**- Pourquoi j’ai l’erreur `Error:"invalid_grant", Description:"Token has been expired or revoked.", Uri:""` au bout de 7 jours ?**

> Un projet Google Cloud Platform avec un écran de consentement OAuth configuré pour un type d'utilisateur externe et dont l'état de publication est "Test" se voit émettre un jeton d'actualisation qui expire dans sept jours.
