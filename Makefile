config ?= Debug

build:
	dotnet build -c $(config) monomerge
	dotnet test -c $(config) monomerge

publish: build
	rm -rf out
	mkdir out
	dotnet publish -c $(config) -r win10-x64 -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained -o $(PWD)/out/win10 monomerge
	cd out/win10; zip -r ../win10.zip ./*

	dotnet publish -c $(config) -r osx-x64 -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained -o $(PWD)/out/osx monomerge
	cd out/osx; zip -r ../osx.zip ./*

	dotnet publish -c $(config) -r linux-x64 -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained -o $(PWD)/out/linux monomerge
	cd out/linux; zip -r ../linux.zip ./*

init:
	rm -rf /Users/pierre/src/tessan/teleconsultation
	cd monomerge; dotnet run -- init /Users/pierre/src/tessan/teleconsultation git@github.com:tessan-tech/teleconsultation.git

convert:
	cd monomerge; dotnet run -- convert /Users/pierre/src/tessan/teleconsultation
