# PRD - OrgChartBuilder Blazor WebAssembly

## Objectif
OrgChartBuilder est une application Blazor WebAssembly permettant de créer, visualiser et éditer dynamiquement un organigramme hiérarchique des employés, directement dans le navigateur.

## Public cible
- Managers et RH
- Développeurs d’outils internes
- Formateurs et consultants

## Fonctionnalités principales

### 1. Création et édition des employés
- Ajouter/supprimer des employés (nom, poste, département, email)
- Modifier informations via panneau latéral ou double-clic sur le noeud

### 2. Visualisation interactive
- Rendu hiérarchique interactif avec D3.js
- Drag & drop pour réorganiser la hiérarchie
- Zoom, pan et repli/expansion des branches

### 3. Relations hiérarchiques
- Création et modification de liens manager → employé
- Vérification automatique pour éviter cycles hiérarchiques

### 4. Filtres et recherche
- Recherche par nom, poste ou département
- Filtrage par équipe ou rôle

### 5. Templates et export
- Export SVG/PNG de l’organigramme
- Génération de rapports texte via Liquid templates (Fluid.Core)
- Exemple template:
```liquid
Employee: {{ name }} - {{ position }} ({{ department }})
Manager: {{ manager.name }}
```

### 6. Stockage local
- Persistance via IndexedDB / localStorage
- Gestion de plusieurs organigrammes

## Architecture technique
- Blazor WebAssembly – frontend 100% client
- D3.js – rendu graphique et interactions
- Fluid.Core – génération de rapports via templates Liquid
- StorageService – persistance locale
- OrgChartService – gestion CRUD des employés et relations

## UI/UX
- Panneau gauche : liste des employés et filtres
- Panneau central : organigramme interactif D3.js
- Panneau droit : édition des propriétés d’un employé et template Liquid
- Boutons principaux : Ajouter employé, Supprimer, Exporter, Générer rapport

## Extensions possibles
- Collaboration multi-utilisateur via API
- Import/export CSV ou JSON d’employés
- Indicateurs visuels pour performance, ancienneté ou KPI par employé
- Intégration avec LDAP / Active Directory
