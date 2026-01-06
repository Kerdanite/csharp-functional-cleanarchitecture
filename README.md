# csharp-functional-cleanarchitecture

Clean Architecture example in C# with a functional approach.

## 🎯 Objectif du projet

Ce projet est un laboratoire personnel visant à explorer :

- une modélisation métier riche
- une approche fonctionnelle en C#
- une implémentation Clean Architecture + DDD
- un CQRS sans MediatR
- une gestion d’infrastructure simplifiée avec .NET Aspire

L’objectif n’est pas de fournir un framework générique, mais un exemple cohérent et assumé.

---

## 🧠 Énoncé fonctionnel

### Domaine métier : calendrier vétérinaire

Vous devez réaliser un service web de gestion de calendrier pour un vétérinaire.

Le système doit permettre de :

- Créer un client
- Créer un patient et le rattacher à un client
- Booker un rendez-vous
- Annuler un rendez-vous
- Reprogrammer un rendez-vous
- Consulter la liste des créneaux de rendez-vous disponibles

L’accent est mis sur la cohérence métier et les règles du domaine, pas sur l’UI.

---

## 🧱 Architecture

Le projet suit une Clean Architecture classique :

- **Domain**
  - Entités
  - Value Objects
  - Règles métier
- **Application**
  - Cas d’usage
  - CQRS
  - Orchestration métier (handler)
- **Infrastructure**
  - Base de données
  - Persistence
  - Intégrations techniques
- **API**
  - Exposition HTT

Les dépendances vont toujours vers le cœur métier.

---

## 🧩 Domain-Driven Design (DDD)

Le projet adopte une approche DDD pragmatique :

- Modèle métier explicite
- Invariants protégés dans le domaine
- Pas de logique dans l’infrastructure
- Le domaine ne dépend d’aucun framework

Le modèle est pensé pour exprimer le métier avant la technique.

---

## 🔁 CQRS sans MediatR

Au lieu d’utiliser MediatR, une couche CQRS maison a été mise en place :

- Séparation claire entre Commands et Queries
- Dépendances explicites
- Se passer de la version payante de MediatR

Ce choix est volontaire et pédagogique.

---

## 🧮 Approche fonctionnelle & Result Pattern

L’ensemble du projet repose sur le Result Pattern :

- Pas d’exceptions pour le flux nominal
- Les erreurs sont des valeurs
- Chaînage fonctionnel (`Map`, `Bind`, etc.)
- Flux métier explicites et prédictibles

Bénéfices :

- Code plus lisible
- Moins de `try/catch`
- Tests plus simples
- Meilleure compréhension des cas d’erreur

---

## 🐳 Infrastructure & .NET Aspire

Le projet utilise .NET Aspire pour gérer l’infrastructure locale :

- Provision automatique de la base de données
- Gestion de l’image de base de données
- Lancement simplifié de l’environnement
- Expérience développeur fluide

L’objectif est de réduire la friction technique pour se concentrer sur le métier.

---

## ⚠️ Ce que ce projet n’est pas

- ❌ Un template de production
- ❌ Un framework générique
- ❌ Une démonstration de performance

C’est un projet d’exploration et de réflexion technique.

---

## ✅ Pourquoi ce repo existe

- Tester des idées d’architecture
- Explorer le functional en C#
- Challenger certains standards (ex. MediatR)
