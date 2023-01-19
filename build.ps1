$tag = git describe --tags --abbrev=0
$image = "ghcr.io/katasec/arkserver:$tag"
docker build . -t  $image


#dotnet publish --os linux --arch x64 /t:PublishContainer -c Release
