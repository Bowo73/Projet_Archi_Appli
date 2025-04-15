# Projet_Archi_Appli

Ce projet est une application Angular + .NET qui permet à un utilisateur d'interagir avec un assistant IA via l'API Azure OpenAI.

ajouter un .env à la racine du dossier chatbot sous la forme suivante:

AZURE_OPENAI_ENDPOINT=
AZURE_OPENAI_API_KEY=
AZURE_OPENAI_DEPLOYMENT_ID=
AZURE_OPENAI_API_VERSION=

Installe angular:

npm install -g @angular/cli

Si déjà fait, 
1. Va dans le dossier `chatbot-front/`
2. Installe les dépendances :

npm install
npm i @angular/material 

3. lance le projet :

ng s

## 🛠 Installation des dépendances (développeur)

### Sous Windows
```bash
powershell -ExecutionPolicy Bypass -File ./scripts/install.ps1
