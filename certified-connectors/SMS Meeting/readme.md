# SMS Meeting - Power Automate Connector

**SMS Meeting** vous permet d'automatiser l'envoi de SMS, notifications, rappels, et la gestion de vos contacts directement depuis Microsoft Power Automate, Power Apps et Logic Apps.

## Publisher: Ventus

## Prérequis

- Un compte SMS Meeting actif.
- Un abonnement API SMS Meeting (offre payante) souscrit depuis l’espace client : [my.sms-meeting.com](https://my.sms-meeting.com).
- Une clé API (Secret Key) générée dans votre espace client.

## Opérations supportées

### Envoyer un SMS (`CreateSms`)
Envoie un SMS à un ou plusieurs destinataires. Permet de spécifier le contenu, l’expéditeur, la date d’envoi différé, etc.

### Lister tous les SMS (`GetAllSms`)
Récupère la liste de tous les SMS envoyés ou programmés.

### Lister tous les contacts (`GetContacts`)
Affiche les contacts disponibles dans votre compte SMS Meeting.

### Créer un contact (`CreateContact`)
Ajoute un nouveau contact dans votre carnet d’adresses SMS Meeting.

### Lister les templates (`GetTemplates`)
Affiche la liste des modèles de messages disponibles.

### Utiliser les listes de contacts
Créer, modifier, supprimer et gérer vos listes de contacts pour des envois groupés.

## Obtention des identifiants (Clé API)

1. Connectez-vous à votre espace client [my.sms-meeting.com](https://my.sms-meeting.com).
2. Souscrivez à l’offre “API SMS Meeting” si ce n’est pas déjà fait.
3. Accédez à la rubrique **Clés API / Secret Key**.
4. Générez une clé secrète pour votre licence.
5. Saisissez cette clé lors de la création de la connexion dans Power Automate.

## Mise en route

- Ajoutez le connecteur personnalisé à votre environnement Power Platform.
- Lors de la première utilisation, renseignez votre clé API (“Secret Key”).
- Utilisez les actions fournies dans vos flux pour automatiser l’envoi et la gestion de vos SMS.

## Problèmes connus et limitations

- Ce connecteur requiert un abonnement API actif.
- Le quota d’envoi de SMS dépend de votre offre.
- Les champs obligatoires doivent être respectés (notamment le type de SMS : `confirmation` ou `reminder`).
- La gestion avancée (templates, contacts) nécessite des droits associés à votre licence.

## Questions fréquentes

### Puis-je envoyer un SMS à plusieurs numéros en une seule action ?
Oui, il suffit d’utiliser la fonctionnalité boucle de Power Automate et d’appeler l’action “Envoyer un SMS” pour chaque numéro.

### Que faire si ma clé API ne fonctionne pas ?
Vérifiez que votre offre API est active et que la clé n’a pas été révoquée. Générez une nouvelle clé depuis votre espace client si besoin.

## Instructions de déploiement

1. Ajoutez les fichiers `apiDefinition.swagger.json` et `apiProperties.json` dans le dossier requis.
2. Suivez la procédure Microsoft pour [soumettre un connecteur certifié ou personnalisé](https://learn.microsoft.com/en-us/connectors/custom-connectors/submit-certification).
3. Une fois approuvé, le connecteur sera disponible sur Power Automate pour tous vos utilisateurs.

---

Support & contact : [hello@sms-meeting.com](mailto:hello@sms-meeting.com)  
Site web : [www.sms-meeting.com](https://www.sms-meeting.com)

