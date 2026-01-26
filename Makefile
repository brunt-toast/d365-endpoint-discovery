restore:
	mkdir ./tmp

	curl -sSL https://dot.net/v1/dotnet-install.sh >./tmp/dotnet-install.sh
	curl -sSL https://dot.net/v1/dotnet-install.asc >./tmp/dotnet-install.asc
	curl -sSL https://dot.net/v1/dotnet-install.sig >./tmp/dotnet-install.sig

	gpg --import ./tmp/dotnet-install.asc 
	gpg --verify ./tmp/dotnet-install.sig ./tmp/dotnet-install.sh
	chmod +x ./tmp/dotnet-install.sh || exit 1

	./tmp/dotnet-install.sh --jsonfile ./global.json 

	rm -r ./tmp 

	dotnet tool restore

	find . -type f -name *.*proj | xargs -I{} sh -c 'dotnet workload restore --project "{}"' 

test: restore 
	find ./test/ -type f -name *.Tests.*proj | xargs -I{} sh -c 'dotnet test "{}"'

install: restore
	rm -r ./src/Cli/bin/nupkg/ || true
	dotnet pack ./src/Cli/Cli.csproj -c Release
	dotnet tool install -g --add-source ./src/Cli/bin/nupkg dynsvcdiscovery --allow-downgrade

pack-gui: 
	rm -r ./src/BlazorHybrid/bin
	pwsh.exe ./Build-SignedMsix.ps1

	zip -r ./src/BlazorHybrid/bin/BlazorHybridMsix.zip "./src/BlazorHybrid/bin/Release/net10.0-windows10.0.19041.0/win-x64/AppPackages"

	@echo "Archive file produced at ./src/BlazorHybrid/bin/BlazorHybridMsix.zip"
