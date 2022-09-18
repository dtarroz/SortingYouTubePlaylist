# Sorting YouTube Playlist

Tri des listes de lecture YouTube (playlists) par leur durée et leur date d'ajout selon un algorthime particulier.

## Sommaire
- [Pourquoi faire ?](#pourquoi-faire-)
- [Mon algorithme de trie](#mon-algorithme-de-trie)
- [Getting Started](#getting-started)
- [Générer le fichier de connexion à votre compte YouTube](#générer-le-fichier-de-connexion-à-votre-compte-youtube)
- [Tri de votre liste de lecture (playlist)](#tri-de-votre-liste-de-lecture-playlist)
- [FAQ](#faq)

## Pourquoi faire ?
Pendant longtemps, je me servais de YouTube seulement pour regarder les vidéos sur un sujet particulier et sur l’instant. Puis j’ai commencé à suivre des chaines intéressantes et j'arrivais à regarder leurs dernières vidéos facilement. Quand mon nombre de chaines à suivre est devenu plus importante, là je me suis fait submerger par les vidéos et j’ai débuté mon utilisation du bouton “A regarder plus tard”.

L’avantage que j'ai trouvé à ces playlists, c’est la continuité de l’enchaînement des vidéos que j’ai choisi préalablement.

Mon utilisation primaire était de lire la première vidéo de la liste puis enchaîner, mais des inconvénients sont très vite arrivés. Si j’avais ajouté beaucoup de vidéos d’une nouvelle chaîne ou bien plusieurs conférences, car le « week-end » de évènement venait de passer, j’avais à la suite des vidéos de la même chaîne ou des vidéos qui durent plus d’une heure. Un autre inconvénient était que je voyais le nombre de vidéos dans la playlist augmenter. 

Alors je suis parti sur un nettoyage des vidéos par leur durée, j’ai regardé toutes les vidéos les plus courtes et pour éviter de parcourir toute la liste pour chercher la plus courte, j’ai fait par tranche de 5 min. Je parcours la liste et je regarde toutes les vidéos de moins de 5 min, arrivé à la fin, je regarde toutes les vidéos de moins de 10 min et ensuite de suite. Ok je diminue le nombre de vidéos mais celles qui ont une durée parmi les plus élevées ne sont jamais lues. Solution, dès que je reprend depuis le début je lis la première de la playlist. Mais si on ajoute régulièrement des vidéos assez courtes, on n'avance pas dans la lecture des vidéos dont la durée seraient plus longues. 

Après plusieurs mois, ça devenait lassant de toujours s’arrêter après une vidéo pour choisir manuellement la suivante, c’est pour cette raison que j’ai étudié la possibilité d’automatiser tout ça.

J’ai codé le procédé précédent pour test et je suis tombé sur d’autres points à améliorer.

## Mon algorithme de trie
Le principe de base repose sur la succession de plusieurs groupes de 4 vidéos (```ABCD```). Chacune de ces vidéos est déterminée par un choix algorithmique selon leur ordre parmi ces 4. 

- Vidéo ```A```
    - Prendre la vidéo la plus courte ajoutée il y a plus d’un mois dans la liste de lecture
    - Sinon prendre la vidéo la plus courte
- Vidéo ```B```
    - Prendre la vidéo la plus courte
- Vidéo ```C```
    - Prendre la vidéo la plus courte
- Vidéo ```D```
    - Prendre la vidéo la plus ancienne ajoutée dans la liste de lecture

Cela permet de regarder les plus courtes pour diminuer rapidement le nombre de vidéos dans la playlist (```B``` et ```C```), de diminuer les vidéos qui sont présentes de plus d’un mois (```A```) et les plus anciennes qui peuvent avoir des durées plus longues (```D```).

**Des contraintes supplémentaires ont été ajoutées :**

- Les vidéos d’une même chaîne ne se suivent pas dans un groupe mais aussi sur la succession des groupes.
- S’il ne reste que des vidéos d’une même chaîne à trier, on prend la plus courte pour les vidéos ```A```, ```B``` et ```C```, et la plus ancienne ajoutée pour la vidéo ```D```.
- Les 4 premières vidéos de la playlist ne sont pas triées. C’est pour éviter de déplacer la vidéo en cours de lecture et des vidéos qui nécessitent une lecture plus confortable _(visionnage sur un autre écran, ne pas être interrompu pendant la lecture, vidéo dans une langue étrangère (sous titre), …)_
- Si les vidéos qui suivent la zone qui ne bouge pas _(les 4 premiéres vidéos)_ sont du même groupe que la quatrième vidéo, alors on ne touche pas à leur ordre. C’est pour éviter d’avoir toujours une vidéo ```A``` qui entre dans la zone si on ne regarde qu’une vidéo entre chaque trie.
- Comme il faut au minimum 2 vidéos pour trier et que les 4 premières ne bougent pas, le trie ne fonctionne que si la playlist contient plus de 6 vidéos.

Pour savoir si une vidéo est dans le même groupe qu’une autre, j’utilise une fonctionnalité disparue de l’interface YouTube qui permettait de saisir une « note » sur une vidéo d’une playlist. Cette note est toujours accessible depuis l’API donc je m’en sers pour stocker l’identifiant unique du groupe à laquelle elle appartient.

## Getting Started
*coming soon*

## Générer le fichier de connexion à votre compte YouTube
*coming soon*

## Tri de votre liste de lecture (playlist)
*coming soon*

## FAQ
*coming soon*
