# Monomerge
Monomerge consolidates all specified GIT repositories into a single one, while preserving all history on specified branch.

# How to build
`make` is required to build and publish.

* `make [build]`: build application (default action)
* `make publish`: build & publish application in `out` folder

# Configuration
Create a new repository, it's required to initialize a new workspace. This repository will be used to discover configuration. All repositories will be added to this one.

The repository must contains the `sbs.yaml` (in root) file describing all available repositories:
```
repositories:
    - name: sbs                                                                                  
      uri: git@git@github.com:pchalamet/SmartBuildSystem.git
      branch: main
    - name: npolybool                                                                                              
      uri: git@github.com:pchalamet/NPolyBool.git
      branch: master
```

# Usage
* `usage` : display help on command or area
* `monomerge init <folder> <mono repository>`: initialize workspace starting from scratch
* `monomerge init --continue <folder> <mono repository>`: initialize workspace continuing where it has previous stopped
