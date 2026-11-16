param(
    [Parameter(Mandatory = $true)][string]$Server,
    [Parameter(Mandatory = $true)][string]$Username,
    [Parameter(Mandatory = $true)][ValidateSet('Admin', 'Editor', 'Accountant')][string]$Role,
    [string]$Database = 'PressDMS'
)
$ErrorActionPreference = 'Stop'
if ($Username.Length -gt 50 -or $Username.Trim().Length -eq 0) { throw 'Username must be 1-50 characters.' }
$securePassword = Read-Host 'Set initial account password' -AsSecureString
$bstr = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($securePassword)
$password = $null
try {
    $password = [Runtime.InteropServices.Marshal]::PtrToStringBSTR($bstr)
    if ($password.Length -lt 12) { throw 'Use an initial password of at least 12 characters.' }
    $salt = New-Object byte[] 32
    $rng = [Security.Cryptography.RandomNumberGenerator]::Create()
    try { $rng.GetBytes($salt) } finally { $rng.Dispose() }
    $derive = New-Object Security.Cryptography.Rfc2898DeriveBytes($password, $salt, 310000, [Security.Cryptography.HashAlgorithmName]::SHA256)
    try { $hash = $derive.GetBytes(32) } finally { $derive.Dispose() }

    $connectionString = 'Data Source=' + $Server + ';Initial Catalog=' + $Database + ';Integrated Security=SSPI;Encrypt=True;TrustServerCertificate=False;Application Name=ManipalDMS-Provisioning'
    $connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
    try {
        $connection.Open()
        $command = $connection.CreateCommand()
        $command.CommandText = 'INSERT INTO dbo.Users (Username, PasswordHash, PasswordSalt, PasswordAlgorithm, Role) VALUES (@u, @h, @s, @a, @r)'
        [void]$command.Parameters.Add('@u', [System.Data.SqlDbType]::NVarChar, 50)
        [void]$command.Parameters.Add('@h', [System.Data.SqlDbType]::VarBinary, 32)
        [void]$command.Parameters.Add('@s', [System.Data.SqlDbType]::VarBinary, 32)
        [void]$command.Parameters.Add('@a', [System.Data.SqlDbType]::NVarChar, 30)
        [void]$command.Parameters.Add('@r', [System.Data.SqlDbType]::NVarChar, 30)
        $command.Parameters['@u'].Value = $Username.Trim()
        $command.Parameters['@h'].Value = $hash
        $command.Parameters['@s'].Value = $salt
        $command.Parameters['@a'].Value = 'PBKDF2-SHA256-310000'
        $command.Parameters['@r'].Value = $Role
        [void]$command.ExecuteNonQuery()
        Write-Host ('Provisioned account ' + $Username.Trim() + ' with role ' + $Role + '.')
    } finally { $connection.Dispose() }
} finally {
    if ($null -ne $password) { $password = $null }
    [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($bstr)
    $securePassword.Dispose()
}
