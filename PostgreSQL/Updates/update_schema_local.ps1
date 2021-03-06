function ConvertFrom-SecureToPlain {
    
    param( [Parameter(Mandatory=$true)][System.Security.SecureString] $SecurePassword)
    
    # Create a "password pointer".
    $PasswordPointer = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($SecurePassword)
    
    # Get the plain text version of the password.
    $PlainTextPassword = [Runtime.InteropServices.Marshal]::PtrToStringAuto($PasswordPointer)
    
    # Free the pointer.
    [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($PasswordPointer)
    
    # Return the plain text password.
    $PlainTextPassword
}


Write-Host "";
Write-Host "";
Write-Host "****";
Write-Host "This is Clients SCHEMA update for LOCAL Sana Live server";
Write-Host "****";

$DbUpdatesPath = "..\..\DbUpdates_Build\Windows\netcoreapp2.0\DbUpdates.dll";
$DbNameTemplate = "Client";
$VersionFieldName = "SchemaVersion";
$UpdatesFolderPath = ".\Schema"

$ServerAddress = Read-Host "Enter your local server address";
$UserId = Read-Host "Enter your local postgres server user id";

$SecurePassword = Read-Host "Enter LOCAL PostgreSQL Server password" -AsSecureString;
$PlainTextPassword = ConvertFrom-SecureToPlain $SecurePassword;

$ConnectionString = "Server=$ServerAddress;Port=5432;Database=postgres;User Id=$UserId;Password=$PlainTextPassword;"

dotnet $DbUpdatesPath -cs $ConnectionString -vfn $VersionFieldName -db $DbNameTemplate -uf $UpdatesFolderPath;