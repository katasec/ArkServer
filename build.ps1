#!/usr/bin/env pwsh

param (
    [Parameter(Mandatory)]
    [string]$target 
)

# ------------------------------------------------------------------------------------------------------------
# Functions
# ------------------------------------------------------------------------------------------------------------

function Test-Usage {
    if ($target -ne "worker" -and $target -ne "server") {
        $message = "-target should be 'worker' or 'server', got $($target)"
        throw $message
    }
}

function Get-DockerFileName() {
    if ($target -eq "worker") {
        return "Arkworker.Dockerfile"
    }
    else {
        return "ArkServer.Dockerfile"
    }
}

function Get-DockerImageName() {
    $tag = git describe --tags --abbrev=0
    if ($target -eq "worker") {
        return "ghcr.io/katasec/arkworker:$tag"
    }
    else {
        return "ghcr.io/katasec/arkserver:$tag"
    }
}

function Build-Image() {
    $dockerFileName = Get-DockerFileName
    $dockerImageName = Get-DockerImageName
    Write-Output "Building $dockerImageName"
    Write-Output "Using $dockerFileName"
    docker build . -t $dockerImageName -f $dockerFileName
}

# ------------------------------------------------------------------------------------------------------------
# Main
# ------------------------------------------------------------------------------------------------------------

Test-Usage
Build-Image
