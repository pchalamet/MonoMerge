# Monomerge
Monomerge is a tool to merge several GIT repositories into a single mono-repository.
All History is preserved on main branch.

# How to build
make

# Usage
monomerge usage
````
Usage:
  usage : display help on command or area
  monomerge <folder> <main repository>: initialize workspace
````

# Configuration
Create a master repository, it's required to initialize a new workspace.

The repository must contains the `sbs.yaml` (in root) file describing all available repositories:
````
repositories:
    - name: sbs                                                                                  
      uri: git@git@github.com:pchalamet/SmartBuildSystem.git                
    - name: npolybool                                                                                              
      uri: git@github.com:pchalamet/NPolyBool.git                                  
````
