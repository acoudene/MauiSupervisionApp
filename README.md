# MauiSupervisionApp

_in french_

# Objectif et principe de base

L'objectif de ce document est d'expliquer les principes à suivre pour gérer les erreurs ou les exceptions sous MAUI Blazor.
Les erreurs doivent être gérées par des exceptions.
Les exceptions doivent être catégorisées et typées pour permettre une aide plus ciblée par le support client.

# Modèle

Le modèle utilisé pour cette gestion des erreurs est fourni ici : https://github.com/acoudene/MauiSupervisionApp

# Diagramme de fonctionnement

En quelques mots, le principe est simple : 
- Tout prérequis de programmation (précondition) ou tout cas d'erreur doivent générer une exception catégorisée (typée). 
- Toutes les exceptions mêmes celles non maîtrisées sont interceptées par le composant **CustomErrorBoundary** que j'ai introduit.
- Les logs peuvent être remontés jusqu'au serveur si on le souhaite (support) mais mécanisme non activé par défaut.
- Un **système de protection** est mis en place au cas où une exception serait générée lors du mécanisme du chargement de la page afin d'éviter une réentrance infinie. En effet, sans cela, ce mécanisme bouclerait car on souhaite afficher la page avec une boite de notification (note : implémentation découplée et donc interchangeable)

![Error Management in MAUI - Client Exception Mechanism.png](/.attachments/Error%20Management%20in%20MAUI%20-%20Client%20Exception%20Mechanism-3ffd41f7-5b69-46b6-96d2-513b111a4c72.png)

# Organisation du code du modèle

## Instanciation par IoC

Il suffit d'ajouter cette ligne au démarrage de l'application :
`builder.Services.AddExceptionManagement();`

Par défaut, sans repoting de log vers le serveur, seuls ces 2 entrées sont nécessaires :
```
services.TryAdd(services.AddLocalization());
services.AddScoped<ILogService, AlertService>();
```

_Optionnel : une version avec la boite SnackBar de MudBlazor est également fournie mais ne se comporte pas très bien sous MAUI Blazor, si besoin, il faut ajouter et transformer les lignes par :_
```
services.TryAdd(services.AddLocalization());
services.TryAdd(services.AddMudServices());

services.AddScoped<ILogService, SnackbarService>();
```

## Intégration du CustomErrorBoundary

Le CustomErrorBoundary s'intègre directement dans **Main.razor** sous forme de "tag englobant".

![image.png](/.attachments/image-4ad0f00d-8c61-4a9a-a474-2ba3d4f7c86b.png)

## Organisation des classes

Toutes les classes sont découpées, classées et réparties à l'intérieur du **crosscutting** (=élément transverse) de gestion des Exceptions.

![image.png](/.attachments/image-60626585-aa17-4ee0-bc32-1b57a031ae2a.png)

## Création de catégories d'erreur/d'exception

L'idée principal est de déclarer autant de classe que notre applicatif a besoin de découper en catégorie d'erreur.
Chaque classe d'exception devra dériver de **LoggedExceptionBase** en respectant le pattern de Microsoft :  https://learn.microsoft.com/fr-fr/dotnet/standard/exceptions/how-to-create-localized-exception-messages

Exemple

![image.png](/.attachments/image-965a2b4c-f40f-4c37-9da9-e167dcbb53b2.png)

### Globalization

Chaque catégorie d'exception est "traduisible" et possède des ressources dédiées : **LogResource.resx**, **LogResource.fr-FR.resx**, ...
Le principe est simple, le nom de la classe d'exception doit être reporté dans ces fichiers de ressource afin de donner une traduction correspondant à sa catégorie.

Exemple en français

![image.png](/.attachments/image-367cfd21-71c8-47d4-93dd-7f76bff8d1ec.png)

### Contenu du Log

Le contenu du Log côté client est géré par un objet de type **LogVO** (stocké dans le répertoire Models de Exceptions pour respecter la nomenclature MVVM)

![image.png](/.attachments/image-cb2c9765-0d0f-47ae-a5dd-5adab71fb91f.png)

Ces champs peuvent être enrichis ou modifiés en lien avec les classes d'implémentation de ILogService.

Exemple avec **AlertService** 

![image.png](/.attachments/image-a5864543-844c-4a8f-b82b-ae709ddf8255.png)

Avec l'exploitation du contenu directement dans un DisplayAlert MAUI 

![image.png](/.attachments/image-0fb80d99-21fd-423b-8255-beb335413e28.png)

# Annexe

## Diagramme textuel 

```
title Error Management in MAUI - Client Exception Mechanism

User -> Page : Display
Page -> Page : Trigger(exception)
note over Page, CustomErrorBoundary 
Exception must be typed by category.
Exception could be unhandled. 
Developer could choose to throw exceptions directly
or wrap an existing one.
end note
Page ->> CustomErrorBoundary : Intercept(exception)
CustomErrorBoundary -> CustomErrorBoundary : CheckReentrancy(exception)
CustomErrorBoundary -> ILogService: Notify(exception)
ILogService -> ILogService : log=CreateLog
note over ILogService 
At this level, LogDTO sets
error context: Guid, creation date,
Category from exception type,
Exception information (Stacktrace, 
inner Exception messages, ...)
end note
alt if option for log remoting is activated
ILogService  ->> ILogClient : Report(log)
ILogClient ->> Server: Report(log)
note over Server
At this level, by IoC, the log could be written and enriched 
to dedicated server log sinks like: console, file or Promotheus
end note
end alt
ILogService --> User: Displayed(log)

alt Reentrancy not detected
CustomErrorBoundary -> Page
CustomErrorBoundary -> CustomErrorBoundary : AddReentrantException(exception)
CustomErrorBoundary -> Page : PassThrough
Page --> User : Displayed
else Reentrancy detected
CustomErrorBoundary -> CustomErrorBoundary : StopPageDisplay
CustomErrorBoundary -> CustomErrorBoundary : Display(fatalException)
CustomErrorBoundary --> User : Displayed(fatalException)
end alt
note over User, Page 
No error after that, 
everything remains ok
end note
User -> Page : Display
Page -> CustomErrorBoundary : Recover
CustomErrorBoundary -> CustomErrorBoundary : ResetReentrancy

```
