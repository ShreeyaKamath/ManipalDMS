param(
    [Parameter(Mandatory = $true)][string]$ServiceIdentity,
    [string]$Path = "$env:ProgramData\ManipalDMS\Documents"
)
$ErrorActionPreference = 'Stop'
if (-not (Test-Path -LiteralPath $Path)) { New-Item -ItemType Directory -Path $Path -Force | Out-Null }
$acl = New-Object System.Security.AccessControl.DirectorySecurity
$acl.SetAccessRuleProtection($true, $false)
$inherit = [System.Security.AccessControl.InheritanceFlags]'ContainerInherit, ObjectInherit'
$none = [System.Security.AccessControl.PropagationFlags]::None
$allow = [System.Security.AccessControl.AccessControlType]::Allow
$acl.AddAccessRule((New-Object System.Security.AccessControl.FileSystemAccessRule($ServiceIdentity, 'Modify', $inherit, $none, $allow)))
$acl.AddAccessRule((New-Object System.Security.AccessControl.FileSystemAccessRule('BUILTIN\Administrators', 'FullControl', $inherit, $none, $allow)))
$acl.AddAccessRule((New-Object System.Security.AccessControl.FileSystemAccessRule('NT AUTHORITY\SYSTEM', 'FullControl', $inherit, $none, $allow)))
Set-Acl -LiteralPath $Path -AclObject $acl
Write-Host "Applied protected ACL to $Path for $ServiceIdentity, Administrators, and SYSTEM."
