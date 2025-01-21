# Az204Labs

## Lab 1 Usage

### Create Azure Env
*Using the 2 PS scripts and arm template*

- Create a storage account
	- Create a file share 
	- Save the arm template json file in the file share

- Create a Automation Account 
	- Create 2 Runbooks
		- Create Az Env
		- Delete Az Env

- Create A MI with RBAC - "Contributor"
	- Create resource group
	- Deploy Arm template 
	- Read the Arm template from azure storage account file share


## Deploy Web Apps to Newly Created Azure Environment
*Using the 2 Web app zip files*

- Zip the azure web app folders 
- Deploy using: 
 - `az webapp deploy --resource-group <group-name> --name <app-name> --src-path <zip-package-path>`
- Once deployed the App will be available in kudu
	-this means when you upload a static file, the index file is browseable using dir path such as -> `https://<AppName>.azurewebsites.net/<folderName>/index.html`
