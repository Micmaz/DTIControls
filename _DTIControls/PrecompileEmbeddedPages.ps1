# PrecompileEmbeddedPages.ps1
#
# Precompiles the embedded .aspx/.ascx/.ashx/.Master resources inside DTIControls.dll (and the
# satellite control assemblies such as Reporting.dll / Chart.js.dll) so that sites deployed as
# NON-UPDATABLE precompiled applications (aspnet_compiler without -u, i.e.
# EnableUpdateable=false) can still serve them.
#
# Why this exists:
#   BaseVirtualPathProvider serves embedded pages at ~/res/<Assembly>/<File> by handing markup
#   streams to the BuildManager for runtime compilation. A precompiled site both ignores
#   HostingEnvironment.RegisterVirtualPathProvider and refuses to runtime-compile any virtual
#   path missing from its precompilation manifest, so every /res/... page 404s ("has not been
#   pre-compiled"). The fix: compile those same virtual paths ahead of time into App_Web_*.dll
#   assemblies plus .compiled manifest files. Dropped into a precompiled site's bin, the
#   BuildManager serves them natively - direct requests, LoadControl("~/res/...") and
#   MasterPageFile references all resolve without the VPP.
#
# What it does:
#   1. Extracts every embedded .aspx/.ascx/.ashx/.Master resource from the input assemblies
#      into a stub web site laid out to mirror the VPP url scheme:  res\<RootNamespace>\<File>
#      (VB.NET resource names are RootNamespace.FileName - folders are not part of the name.)
#   2. Adds the "/~/res/BaseClasses/Scripts.as?x" alias paths emitted by Scripts.ScriptsURL()
#      and a root page.aspx copy for the DTIAdminPanel CMS page handler.
#   3. Runs aspnet_compiler -v / -fixednames (non-updatable) over the stub site.
#   4. Harvests App_Web_*.dll + *.compiled manifests into _Output\PrecompiledResources\,
#      partitioned per source assembly so a site only deploys what it ships:
#          PrecompiledResources\               pages embedded in DTIControls.dll
#          PrecompiledResources\Reporting\     pages embedded in Reporting.dll   (etc.)
#          PrecompiledResources\OptionalRootPage\  the root /page.aspx CMS entry
#
# Usage:
#   .\_DTIControls\PrecompileEmbeddedPages.ps1                # after building the solution
#   .\_DTIControls\PrecompileEmbeddedPages.ps1 -KeepTemp      # keep the stub site for debugging
#
# Deployment (consuming site precompiled with EnableUpdateable=false):
#   copy the *.dll and *.compiled files into the precompiled site's bin\ folder.
#   See the generated README.txt for details.

param(
	[string[]]$AssemblyPaths = @(),
	[string]$OutputDir = "",
	[string]$BinSource = "",
	[switch]$KeepTemp
)

$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$primaryName = "DTIControls"
if ($AssemblyPaths.Count -eq 0) {
	# Default: DTIControls.dll first (wins duplicate resource names), then every other
	# resource-bearing assembly sitting next to it in _Output.
	$outputRoot = Join-Path $repoRoot "_Output"
	$primary = Join-Path $outputRoot "DTIControls.dll"
	if (-not (Test-Path $primary)) { throw "Assembly not found: $primary  (build the solution first)" }
	$AssemblyPaths = @($primary) + @(Get-ChildItem (Join-Path $outputRoot "*.dll") |
		Where-Object { $_.Name -ne "DTIControls.dll" } | ForEach-Object { $_.FullName })
} else {
	$primaryName = [System.IO.Path]::GetFileNameWithoutExtension($AssemblyPaths[0])
}
if ($OutputDir -eq "") { $OutputDir = Join-Path $repoRoot "_Output\PrecompiledResources" }
if ($BinSource -eq "") { $BinSource = Split-Path -Parent $AssemblyPaths[0] }

$aspnetCompiler = Join-Path $env:WINDIR "Microsoft.NET\Framework64\v4.0.30319\aspnet_compiler.exe"
if (-not (Test-Path $aspnetCompiler)) {
	$aspnetCompiler = Join-Path $env:WINDIR "Microsoft.NET\Framework\v4.0.30319\aspnet_compiler.exe"
}
if (-not (Test-Path $aspnetCompiler)) { throw "aspnet_compiler.exe (.NET Framework 4.x) not found" }

$workDir   = Join-Path $env:TEMP "DTIResPrecompile"
$siteDir   = Join-Path $workDir "site"
$targetDir = Join-Path $workDir "precompiled"
if (Test-Path $workDir) { Remove-Item -Recurse -Force $workDir }
New-Item -ItemType Directory -Force -Path $siteDir | Out-Null

# vpath (e.g. "/res/BaseClasses/ListResources.aspx") -> source assembly short name
$sourceByVpath = @{}

foreach ($assemblyPath in $AssemblyPaths) {
	if (-not (Test-Path $assemblyPath)) { throw "Assembly not found: $assemblyPath" }
	$shortName = [System.IO.Path]::GetFileNameWithoutExtension($assemblyPath)
	try {
		$asm = [System.Reflection.Assembly]::Load([System.IO.File]::ReadAllBytes($assemblyPath))
		$pageResources = @($asm.GetManifestResourceNames() | Where-Object { $_ -match '\.(aspx|ascx|ashx|master)$' })
	} catch {
		continue  # native / unmanaged dll
	}
	if ($pageResources.Count -eq 0) { continue }
	Write-Host "== Extracting $($pageResources.Count) embedded page resources from $shortName.dll"

	foreach ($resName in $pageResources) {
		$segments = $resName.Split('.')
		if ($segments.Count -lt 3) {
			Write-Warning "Skipping resource with no namespace segment: $resName"
			continue
		}
		# VB embedded resource name = <RootNamespace>.<FileName>.<ext>; the VPP url is /res/<RootNamespace>/<FileName>.<ext>
		$file = $segments[-2..-1] -join '.'
		$ns   = $segments[0..($segments.Count - 3)] -join '.'
		$dir  = Join-Path $siteDir "res\$ns"
		$dest = Join-Path $dir $file
		if (Test-Path $dest) {
			Write-Warning "Duplicate resource path res/$ns/$file (from $shortName!$resName) - keeping first copy"
			continue
		}
		New-Item -ItemType Directory -Force -Path $dir | Out-Null

		$stream = $asm.GetManifestResourceStream($resName)
		try {
			$ms = New-Object System.IO.MemoryStream
			$stream.CopyTo($ms)
			$bytes = $ms.ToArray()
		} finally { $stream.Dispose() }

		# CodeFile= would make aspnet_compiler look for a source file that is not in the stub site.
		# The types are already compiled into the assembly, so rewrite CodeFile= to CodeBehind= (inert).
		$text = [System.Text.Encoding]::UTF8.GetString($bytes)
		if ($text -match '(?i)CodeFile\s*=') {
			$text = $text -replace '(?i)\bCodeFile(\s*=)', 'CodeBehind$1'
			[System.IO.File]::WriteAllText($dest, $text)
		} else {
			[System.IO.File]::WriteAllBytes($dest, $bytes)
		}
		$sourceByVpath["/res/$ns/$file"] = $shortName
	}
}
if ($sourceByVpath.Count -eq 0) { throw "No embedded .aspx/.ascx/.ashx/.Master resources found" }

# Scripts.ScriptsURL() emits root-relative urls with a literal "~" segment ("/~/res/BaseClasses/Scripts.ashx?f=...")
# so client caching works across pages. Mirror those alias paths in the stub site.
$aliasDir = Join-Path $siteDir "~\res\BaseClasses"
foreach ($scriptsFile in @("Scripts.ashx", "Scripts.aspx")) {
	$src = Join-Path $siteDir "res\BaseClasses\$scriptsFile"
	if (Test-Path $src) {
		New-Item -ItemType Directory -Force -Path $aliasDir | Out-Null
		Copy-Item $src (Join-Path $aliasDir $scriptsFile)
		$sourceByVpath["/~/res/BaseClasses/$scriptsFile"] = $sourceByVpath["/res/BaseClasses/$scriptsFile"]
	}
}

# The DTIAdminPanel CMS serves any /page.aspx request through the embedded page. Compile a root
# copy; it is harvested into OptionalRootPage\ since a site may have its own physical page.aspx.
$cmsPage = Join-Path $siteDir "res\DTIAdminPanel\Page.aspx"
if (Test-Path $cmsPage) {
	Copy-Item $cmsPage (Join-Path $siteDir "page.aspx")
	$sourceByVpath["/page.aspx"] = "OptionalRootPage"
}

Write-Host "== Populating stub site bin from $BinSource"
New-Item -ItemType Directory -Force -Path (Join-Path $siteDir "bin") | Out-Null
Copy-Item (Join-Path $BinSource "*.dll") (Join-Path $siteDir "bin")

@"
<?xml version="1.0"?>
<configuration>
  <system.web>
    <compilation targetFramework="4.8" debug="false" />
    <httpRuntime targetFramework="4.8" />
    <pages validateRequest="false" />
  </system.web>
</configuration>
"@ | Set-Content -Path (Join-Path $siteDir "web.config") -Encoding UTF8

Write-Host "== Running aspnet_compiler (non-updatable, fixed names)"
$compilerOutput = & $aspnetCompiler -v / -p $siteDir -f -fixednames $targetDir 2>&1
if ($LASTEXITCODE -ne 0) {
	$compilerOutput | ForEach-Object { Write-Host $_ }
	throw "aspnet_compiler failed with exit code $LASTEXITCODE"
}

Write-Host "== Harvesting precompiled artifacts to $OutputDir"
if (Test-Path $OutputDir) { Remove-Item -Recurse -Force $OutputDir }
New-Item -ItemType Directory -Force -Path $OutputDir | Out-Null

$manifests = @(Get-ChildItem (Join-Path $targetDir "bin") -Filter *.compiled)
$vpaths = New-Object System.Collections.Generic.List[string]
$harvestCount = 0
foreach ($manifest in $manifests) {
	[xml]$xml = Get-Content $manifest.FullName
	$preserve = $xml.preserve
	$vpath = $preserve.virtualPath
	if ($null -eq $vpath) { continue }
	$vpaths.Add($vpath)

	# Partition per source assembly: everything embedded in the primary (merged) assembly goes
	# in the output root; satellite assemblies (Reporting.dll, Chart.js.dll, ...) each get a
	# subfolder so a site only deploys manifests for satellites it actually ships. The root
	# /page.aspx CMS entry is optional: a site may precompile its own physical page.aspx, and
	# two manifests must not claim one path.
	$source = $sourceByVpath[$vpath]
	$dest = $OutputDir
	if ($source -and $source -ne $primaryName) {
		$dest = Join-Path $OutputDir $source
		New-Item -ItemType Directory -Force -Path $dest | Out-Null
	}

	Copy-Item $manifest.FullName $dest
	$harvestCount++
	$asmName = $preserve.assembly
	if ($asmName -and $asmName -like "App_Web_*") {
		$asmFile = Join-Path $targetDir "bin\$asmName.dll"
		if (Test-Path $asmFile) { Copy-Item $asmFile $dest }
	}
}

# Coverage check: every extracted virtual path should have a manifest entry.
$missing = @($sourceByVpath.Keys | Where-Object { -not ($vpaths -contains $_) } | Sort-Object)
Write-Host ""
Write-Host "== Summary"
Write-Host "   Embedded page resources extracted : $($sourceByVpath.Count)"
Write-Host "   Manifest entries harvested        : $harvestCount"
if ($missing.Count -gt 0) {
	Write-Warning "No precompiled manifest was produced for:"
	$missing | ForEach-Object { Write-Warning "   $_" }
} else {
	Write-Host "   Coverage                          : complete"
}

@"
DTIControls precompiled embedded resources
==========================================

These files let a site that is precompiled as NON-updatable (aspnet_compiler without -u /
"EnableUpdateable=false") keep serving the DTIControls embedded pages and user controls
(~/res/<Assembly>/<File>). Precompiled sites ignore the BaseVirtualPathProvider and refuse
runtime compilation, so the embedded markup is compiled ahead of time into these assemblies.

Deployment
----------
1. Precompile your site as usual (aspnet_compiler, no -u), with DTIControls.dll in bin.
2. Copy the *.dll and *.compiled files from THIS folder into the precompiled site's bin\.
3. For each satellite control assembly your site also deploys (Reporting.dll, Chart.js.dll,
   FusionCharts.dll, ...), additionally copy the matching subfolder's files into bin\.
   Skip subfolders for assemblies you do not deploy - their App_Web pages would fail to
   load without the satellite dll.
4. Only if your site uses the DTIAdminPanel CMS /page.aspx handler AND has no physical
   page.aspx of its own: also copy the contents of OptionalRootPage\ into bin\.

Notes
-----
- The .compiled files are manifest entries that map each virtual path (e.g.
  /res/BaseClasses/ListResources.aspx) to its precompiled type; the App_Web_*.dll files hold
  the compiled markup classes. Both are required, and DTIControls.dll must be the same build
  these were generated from (regenerate with _DTIControls\PrecompileEmbeddedPages.ps1 after
  rebuilding).
- Virtual paths were compiled against the site root (-v /). Sites hosted in a sub-application
  keep working for app-relative (~/res/...) references.
- Non-precompiled sites do not need these files; the virtual path provider serves the
  embedded resources directly.

Generated by _DTIControls\PrecompileEmbeddedPages.ps1
"@ | Set-Content -Path (Join-Path $OutputDir "README.txt") -Encoding UTF8

if (-not $KeepTemp) {
	Remove-Item -Recurse -Force $workDir
} else {
	Write-Host "   Stub site kept at $workDir"
}
Write-Host "Done. Artifacts in $OutputDir"
